using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class NeptuneApp : MonoBehaviour
{
    [System.Serializable]
    public class PasswordTogglePair
    {
        public TMP_InputField inputField;
        public Button toggleButton;

        [HideInInspector] public bool isVisible = false;
    }

    public List<PasswordTogglePair> togglePairs;

    [Header("UI Panels")]
    public GameObject SelectPanel;
    public GameObject TeacherPanel;
    public GameObject StudentPanel;
    public GameObject TeacherLobbyPanel;

    [Header("Teacher Panel UI")]
    public TMP_InputField TeacherMailInput;
    public TMP_InputField TeacherUsernameInput;
    public TMP_InputField TeacherPassInput;
    public Button TeacherSignUpButton;

    [Header("Teacher Login UI")]
    public TMP_InputField TeacherLoginEmailInput;
    public TMP_InputField TeacherLoginPasswordInput;
    public Button TeacherLoginButton;

    [Header("Teacher code text in Teacher_Popup_User_Info panel")]
    public TMP_Text TeacherIDText;

    [Header("Student adding UI elements in Teacher_Popup_User_Info panel")]
    public TMP_InputField StudentIDAddInput;
    public Button AddStudentButton;

    [Header("Student removing UI elements in Teacher_Popup_User_Info panel")]
    public TMP_InputField StudentIDRemoveInput;
    public Button RemoveStudentButton;

    [Header("Student Panel UI")]
    public TMP_InputField StudentMailInput;
    public TMP_InputField StudentUsernameInput;
    public TMP_InputField StudentPassInput;
    public Button StudentSignUpButton;

    [Header("Student Login UI")]
    public TMP_InputField StudentSigninUsername;
    public TMP_InputField StudentSigninPassword;
    public Button StudentSigninButton;

    [Header("Teacher Lobby - Student List UI")]
    public Transform StudentListContent; 
    public GameObject StudentListItemPrefab; 
    public GameObject DetailedStudentInfoPrefab; 
    public GameObject DetailedScrollRectPrefab;
    public Button UpdateStudentInfoButton;

    public GameObject PopUpPanel;
    public GameObject PopUpMessageItems;
    
    // Student and other objects to be dragged from here for the SignUp Success panel.
    public GameObject StudentIDObject;
    public GameObject TeacherIDObject;
    public GameObject LoginSuccessObject;

    public TMP_Text StudentIDPrivateCode;
    public TMP_Text TeacherIDPrivateCode;

    [Header("Teacher Lobby - Teacher Username Display")]
    public TMP_Text TeacherUserNameText;

    [Header("Logout Buttons")]
    public Button TeacherLogoutButton;  // Logout button in Teacher panel
    public Button StudentLogoutButton;  // Logout button in Student panel

    private FirebaseProxyService firebaseProxyService;

    private bool _isRegistering = false; 
    private bool _isLoggingIn = false; 
    private bool _isAddingStudent = false; 
    private bool _isRemovingStudent = false; 
    private bool _isLoadingStudents = false; 

    // List of added students
    private List<StudentInfo> studentList = new List<StudentInfo>();
    
    // Need to add this to the button's onClick and mark ShowTeacherUserInfo.
    public void ShowTeacherUserInfo()
    {
        // Show teacher code
        if (TeacherIDText != null && firebaseProxyService != null)
        {
            TeacherIDText.text = firebaseProxyService.teacherPrivateCode;
            Debug.Log("Teacher code being displayed: " + firebaseProxyService.teacherPrivateCode);
        }
        else
        {
            Debug.LogWarning("TeacherIDText component not assigned or firebaseProxyService not found!");
        }
    }
    
    public void PopUpError(int x)
    {
        Debug.Log($"PopUpError called with index: {x}");
        Debug.Log($"PopUpMessageItems child count: {PopUpMessageItems.transform.childCount}");
        
        PopUpPanel.SetActive(true);
        
        // Check if the requested index exists
        if (x >= PopUpMessageItems.transform.childCount)
        {
            Debug.LogError($"Popup index {x} is out of range! Child count: {PopUpMessageItems.transform.childCount}");
            return;
        }
        
        for (int i = 0; i < PopUpMessageItems.transform.childCount; i++)
        {
            GameObject childObj = PopUpMessageItems.transform.GetChild(i).gameObject;
            bool shouldBeActive = (i == x);
            childObj.SetActive(shouldBeActive);
            
            if (shouldBeActive)
            {
                Debug.Log($"Activated popup at index {i}: {childObj.name}");
            }
        }
        
        // Login Success was appearing repeatedly, I'll open this when I add login but need to close others - DON'T FORGET.
        if (LoginSuccessObject != null)
        {
            LoginSuccessObject.SetActive(false);
        }
    }
    
    // New method to show Login Success popup
    public void ShowLoginSuccess()
    {
        PopUpPanel.SetActive(true);
        
        // Close all panels
        for (int i = 0; i < PopUpMessageItems.transform.childCount; i++)
        {
            PopUpMessageItems.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        // Only open LoginSuccess panel
        if (LoginSuccessObject != null)
        {
            LoginSuccessObject.SetActive(true);
        }
    }

    private void Start()
    {
        foreach (var pair in togglePairs)
        {
            if (pair.toggleButton != null)
            {
                pair.toggleButton.onClick.AddListener(() => ToggleVisibility(pair));
            }
        }

        firebaseProxyService = FirebaseProxyService.Instance;
        if (firebaseProxyService == null)
        {
            Debug.LogError("FirebaseProxyService instance not found! Make sure you've added it to the scene.");
        }

        SelectPanel.SetActive(true);
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        
        if (TeacherLobbyPanel != null)
        {
            TeacherLobbyPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("TeacherLobbyPanel not assigned! The screen to be shown when teacher logs in is missing.");
        }

        // Clear button listeners and then add them again to prevent duplication.
        if (TeacherSignUpButton != null)
        {
            TeacherSignUpButton.onClick.RemoveAllListeners(); 
            TeacherSignUpButton.onClick.AddListener(HandleTeacherSignUp);
            Debug.Log("TeacherSignUpButton listener added.");
        }
        else
        {
            Debug.LogError("TeacherSignUpButton reference not assigned!");
        }
            
        if (TeacherLoginButton != null)
        {
            TeacherLoginButton.onClick.RemoveAllListeners(); 
            TeacherLoginButton.onClick.AddListener(HandleTeacherLogin);
            Debug.Log("TeacherLoginButton listener added.");
        }
        else
        {
            Debug.LogWarning("TeacherLoginButton reference not assigned! Teacher login button will not work.");
        }

        if (StudentSignUpButton != null)
        {
            StudentSignUpButton.onClick.RemoveAllListeners(); 
            StudentSignUpButton.onClick.AddListener(HandleStudentSignUp);
            Debug.Log("StudentSignUpButton listener added.");
        }
        else
        {
            Debug.LogError("StudentSignUpButton reference not assigned!");
        }
        
        if (StudentSigninButton != null)
        {
            StudentSigninButton.onClick.RemoveAllListeners();
            StudentSigninButton.onClick.AddListener(HandleStudentLogin);
            Debug.Log("StudentSigninButton listener added.");
        }
        else
        {
            Debug.LogWarning("StudentSigninButton reference not assigned! Student login button will not work.");
        }
        
        
        if (AddStudentButton != null)
        {
            AddStudentButton.onClick.RemoveAllListeners();
            AddStudentButton.onClick.AddListener(HandleAddStudent);
            Debug.Log("AddStudentButton listener added.");
        }
        else
        {
            Debug.LogWarning("AddStudentButton reference not assigned! Student addition button will not work.");
        }
        
        if (RemoveStudentButton != null)
        {
            RemoveStudentButton.onClick.RemoveAllListeners();
            RemoveStudentButton.onClick.AddListener(HandleRemoveStudent);
            Debug.Log("RemoveStudentButton listener added.");
        }
        else
        {
            Debug.LogWarning("RemoveStudentButton reference not assigned! Student removal button will not work.");
        }
        
        
        if (UpdateStudentInfoButton != null)
        {
            UpdateStudentInfoButton.onClick.RemoveAllListeners();
            UpdateStudentInfoButton.onClick.AddListener(HandleUpdateAllStudentInfo);
            Debug.Log("UpdateStudentInfoButton listener added.");
        }
        
        // Logout Button Listeners
        if (TeacherLogoutButton != null)
        {
            TeacherLogoutButton.onClick.RemoveAllListeners();
            TeacherLogoutButton.onClick.AddListener(HandleLogout);
            Debug.Log("TeacherLogoutButton listener added.");
        }
        else
        {
            Debug.LogWarning("TeacherLogoutButton reference not assigned!");
        }
        
        if (StudentLogoutButton != null)
        {
            StudentLogoutButton.onClick.RemoveAllListeners();
            StudentLogoutButton.onClick.AddListener(HandleLogout);
            Debug.Log("StudentLogoutButton listener added.");
        }
        else
        {
            Debug.LogWarning("StudentLogoutButton reference not assigned!");
        }
    }

    // Student addition part - the section that enables teachers to add students.
    public void HandleAddStudent()
    {
        if (_isAddingStudent)
        {
            Debug.LogWarning("A student addition process is already in progress, please wait.");
            return;
        }
        string studentIdentifier = StudentIDAddInput.text.Trim();
        Debug.Log($"Student identifier input: '{studentIdentifier}' (Length: {studentIdentifier.Length})");
        if (string.IsNullOrEmpty(studentIdentifier) || string.IsNullOrWhiteSpace(studentIdentifier) || 
            studentIdentifier == "Student-ID" || studentIdentifier == "Student ID" || studentIdentifier == "Enter Student ID")
        {
            Debug.LogError("Student identifier field cannot be left empty.");
            PopUpError(8); 
            return;
        }
        if (!studentIdentifier.StartsWith("STU-"))
        {
            Debug.LogError("Student code must start with STU-.");
            PopUpError(8); // popup error
            return;
        }
        Debug.Log($"Student addition process starting: Identifier: {studentIdentifier}");
        _isAddingStudent = true;
        if (firebaseProxyService != null)
        {
            firebaseProxyService.AddStudentToTeacher(studentIdentifier, (success, message) => {
                _isAddingStudent = false;
                OnAddStudentCompleted(success, message);
            });
        }
        else
        {
            _isAddingStudent = false; 
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }
    
    
    private void OnAddStudentCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Student successfully added! Message: {message}");
            
            // Show success message
            PopUpError(4); // Blue popup panel (for success message) - index corrected
            
            
            if (StudentIDAddInput != null)
            {
                StudentIDAddInput.text = "";
            }
            
            
            LoadStudentList();
        }
        else
        {
            Debug.LogError($"Student addition failed! Error: {message}");
            
            // Check error message to show appropriate popup
            if (message.Contains("No student found") || message.Contains("No valid student found"))
            {
                PopUpError(12); // Show popup for student not found
            }
            else
            {
                PopUpError(9); // Show error popup for other errors (like duplicate student)
            }
        }
    }

    // Student removal part - the section that enables teachers to remove students.
    public void HandleRemoveStudent()
    {
        if (_isRemovingStudent)
        {
            Debug.LogWarning("A student removal process is already in progress, please wait.");
            return;
        }
        string studentIdentifier = StudentIDRemoveInput.text.Trim();
        Debug.Log($"Student identifier input for removal: '{studentIdentifier}' (Length: {studentIdentifier.Length})");
        if (string.IsNullOrEmpty(studentIdentifier) || string.IsNullOrWhiteSpace(studentIdentifier) || 
            studentIdentifier == "Student-ID" || studentIdentifier == "Student ID" || studentIdentifier == "Enter Student ID")
        {
            Debug.LogError("Student identifier field cannot be left empty.");
            PopUpError(8); 
            return;
        }
        if (!studentIdentifier.StartsWith("STU-"))
        {
            Debug.LogError("Student code must start with STU-.");
            PopUpError(8); // popup error
            return;
        }
        Debug.Log($"Student removal process starting: Identifier: {studentIdentifier}");
        _isRemovingStudent = true;
        if (firebaseProxyService != null)
        {
            firebaseProxyService.RemoveStudentFromTeacher(studentIdentifier, (success, message) => {
                _isRemovingStudent = false;
                OnRemoveStudentCompleted(success, message);
            });
        }
        else
        {
            _isRemovingStudent = false; 
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }
    
    private void OnRemoveStudentCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Student successfully removed! Message: {message}");
            
            // Show success message
            PopUpError(11); // Show student removal success popup
            
            
            if (StudentIDRemoveInput != null)
            {
                StudentIDRemoveInput.text = "";
            }
            
            
            LoadStudentList();
        }
        else
        {
            Debug.LogError($"Student removal failed! Error: {message}");
            
            // Check error message to show appropriate popup
            if (message.Contains("not in the teacher's list"))
            {
                PopUpError(13); // Show popup for student is not in the teacher's list
            }
            else if (message.Contains("No student found") || message.Contains("No valid student found"))
            {
                PopUpError(12); // Show popup for student not found in backend
            }
            else
            {
                PopUpError(9); // Show error popup for other errors
            }
        }
    }

    public void OnSelectTeacher()
    {
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(true);
        StudentPanel.SetActive(false);
        Debug.Log("Teacher panel selected.");
    }

    public void OnSelectStudent()
    {
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(true);
        Debug.Log("Student panel selected.");
    }

    public void HandleTeacherSignUp()
    {
        // If a registration process is already in progress, don't start a new one
        if (_isRegistering)
        {
            Debug.LogWarning("A registration process is already in progress, please wait.");
            return;
        }
        
        string email = TeacherMailInput.text;
        string username = TeacherUsernameInput.text;
        string password = TeacherPassInput.text;

        // Show popup 14 if any field is empty
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email, username and password fields cannot be left empty.");
            PopUpError(14); // Show new popup for empty fields
            return;
        }

        var userProfile = new Dictionary<string, object>
        {
            { "userType", "teacher" },
            { "username", username }
        };

        Debug.Log($"Teacher registration starting: Email: {email}");
        
        // Start the registration process
        _isRegistering = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.RegisterUserWithProfile(email, password, userProfile, (success, message) => {
                
                _isRegistering = false;
                OnSignUpCompleted(success, message);
            });
        }
        else
        {
            _isRegistering = false; // Reset flag if FirebaseProxyService doesn't exist
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }
    
    // New method for teacher login
    public void HandleTeacherLogin()
    {
        // If a login process is already in progress, don't start a new one
        if (_isLoggingIn)
        {
            Debug.LogWarning("A login process is already in progress, please wait.");
            return;
        }
        
        string email = TeacherLoginEmailInput.text;
        string password = TeacherLoginPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email and password fields cannot be left empty.");
            PopUpError(3); // Show EmptyError popup (for consistency)
            return;
        }

        Debug.Log($"Teacher login starting: Email: {email}");
        
        
        _isLoggingIn = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.LoginUser(email, password, (success, message) => {
                
                _isLoggingIn = false;
                OnLoginCompleted(success, message);
            });
        }
        else
        {
            _isLoggingIn = false; 
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }
    
    // Login callback method
    private void OnLoginCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Login successful! UserID: {message}");
            
            // Check if user type is teacher (since this is teacher login)
            if (firebaseProxyService.UserType != "teacher")
            {
                Debug.LogError("Student account cannot login from teacher panel!");
                PopUpError(15); // Show sign in error popup
                return; // Early return to prevent further processing
            }
            
            // Show Login Success popup
            ShowLoginSuccess();
            
            // Perform action based on user type
            if (firebaseProxyService.UserType == "teacher")
            {
                            // Show teacher username in Teacher Lobby
            if (TeacherUserNameText != null && firebaseProxyService != null)
            {
                // Show username if available, otherwise fallback to teacher code
                string displayName = !string.IsNullOrEmpty(firebaseProxyService.Username) ? 
                                   firebaseProxyService.Username : 
                                   firebaseProxyService.teacherPrivateCode;
                TeacherUserNameText.text = displayName;
                Debug.Log("Teacher username assigned to TeacherUserNameText: " + displayName);
            }
            else
            {
                Debug.LogWarning("TeacherUserNameText reference not assigned or firebaseProxyService not found!");
            }
                
                // Redirect to teacher panel after a short delay
                StartCoroutine(ShowTeacherLobbyAfterDelay(2.0f));
            }
        }
        else
        {
            Debug.LogError($"Login failed! Error: {message}");
            PopUpError(15); // Show sign in error popup
        }
    }
    
    // Student login callback method
    private void OnStudentLoginCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Student login successful! UserID: {message}");
            
            // Check if user type is student (since this is student login)
            if (firebaseProxyService.UserType != "student")
            {
                Debug.LogError("Teacher account cannot login from student panel!");
                PopUpError(15); // Show sign in error popup
                return; // Early return to prevent further processing
            }
            
            // Show Login Success popup
            ShowLoginSuccess();
            StartCoroutine(ShowStudentLobbyAfterDelay(2.0f));

            // Check user type
            if (firebaseProxyService.UserType == "student")
            {
                Debug.Log("Student successfully logged in!");
                Debug.Log("Student code: " + firebaseProxyService.PrivateCode);
                Debug.Log("Current Level: " + firebaseProxyService.CurrentLevel);
                Debug.Log("Current World: " + firebaseProxyService.CurrentWorld);
                
                // Student dashboard/panel redirection can be added in the future
                // StartCoroutine(ShowStudentDashboardAfterDelay(2.0f));
            }
        }
        else
        {
            Debug.LogError($"Student login failed! Error: {message}");
            PopUpError(15); // Show sign in error popup
        }
    }
    
    // Coroutine for showing teacher panel with delay
    private IEnumerator ShowTeacherLobbyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Close all panels
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        PopUpPanel.SetActive(false);
        
        // Show teacher lobby
        if (TeacherLobbyPanel != null)
        {
            TeacherLobbyPanel.SetActive(true);
            Debug.Log("Teacher lobby is being shown.");
            
            // Check teacher username one more time (backup)
            if (TeacherUserNameText != null && firebaseProxyService != null)
            {
                // Show username if available, otherwise fallback to teacher code
                string displayName = !string.IsNullOrEmpty(firebaseProxyService.Username) ? 
                                   firebaseProxyService.Username : 
                                   firebaseProxyService.teacherPrivateCode;
                if (!string.IsNullOrEmpty(displayName))
                {
                    TeacherUserNameText.text = displayName;
                    Debug.Log("Teacher username backup check: " + displayName);
                }
            }
            
            // Load student list
            LoadStudentList();
        }
        else
        {
            Debug.LogError("TeacherLobbyPanel not assigned! Teacher lobby cannot be shown.");
        }
    }

    private IEnumerator ShowStudentLobbyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Close all panels
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        PopUpPanel.SetActive(false);

        SceneManager.LoadScene("v2_MainMenu");
    }

    public void HandleStudentSignUp()
    {
        // If a registration process is already in progress, don't start a new one
        if (_isRegistering)
        {
            Debug.LogWarning("A registration process is already in progress, please wait.");
            return;
        }
        
        string email = StudentMailInput.text;
        string username = StudentUsernameInput.text;
        string password = StudentPassInput.text;

        // Show popup 14 if any field is empty
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email, username and password fields cannot be left empty.");
            PopUpError(14); // Show new popup for empty fields
            return;
        }

        var userProfile = new Dictionary<string, object>
        {
            { "userType", "student" },
            { "username", username }
        };

        Debug.Log($"Student registration starting: Email: {email}");
        
        // Start the registration process
        _isRegistering = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.RegisterUserWithProfile(email, password, userProfile, (success, message) => {
                // Registration process completed, reset flag
                _isRegistering = false;
                OnSignUpCompleted(success, message);
            });
        }
        else
        {
            _isRegistering = false; // Reset flag if FirebaseProxyService doesn't exist
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }
    
    // New method for student login
    public void HandleStudentLogin()
    {
        // If a login process is already in progress, don't start a new one
        if (_isLoggingIn)
        {
            Debug.LogWarning("A login process is already in progress, please wait.");
            return;
        }
        
        string email = StudentSigninUsername.text;
        string password = StudentSigninPassword.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email and password fields cannot be left empty.");
            PopUpError(3); // Show EmptyError popup (for consistency)
            return;
        }

        Debug.Log($"Student login starting: Email: {email}");
        
        _isLoggingIn = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.LoginUser(email, password, (success, message) => {
                
                _isLoggingIn = false;
                OnStudentLoginCompleted(success, message);
            });
        }
        else
        {
            _isLoggingIn = false; 
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }

    public void PostSignUpStudentLogin()
    {
        PopUpPanel.GetComponent<ActiveStateWatcher>().OnBecameInactive.RemoveListener(PostSignUpStudentLogin);
        
        PopUpPanel.SetActive(false);
        Debug.Log($"Student login starting: Email: {StudentMailInput.text}");
        _isLoggingIn = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.LoginUser(StudentMailInput.text, StudentPassInput.text, (success, message) => {
                
                _isLoggingIn = false;
                OnStudentLoginCompleted(success, message);
            });
        }
        else
        {
            _isLoggingIn = false; 
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }

    private void PostSignUpTeacherLogin()
    {
        PopUpPanel.GetComponent<ActiveStateWatcher>().OnBecameInactive.RemoveListener(PostSignUpTeacherLogin);
        
        _isLoggingIn = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.LoginUser(TeacherMailInput.text, TeacherPassInput.text, (success, message) => {
                
                _isLoggingIn = false;
                OnLoginCompleted(success, message);
            });
        }
        else
        {
            _isLoggingIn = false; 
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }

    private void OnSignUpCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Registration successful! Message/UserID: {message}");
            
            
            PopUpError(1); // Show SignUp Success popup notification from here (the one in 1st position)
            
            // Show based on whether they are student or teacher - I'm showing based on UserType here. 
            if (firebaseProxyService.UserType == "teacher")
            {
                PopUpPanel.GetComponent<ActiveStateWatcher>().OnBecameInactive.AddListener(PostSignUpTeacherLogin);

                string teacherCode = firebaseProxyService.teacherPrivateCode;
                
                // TeacherID display section, but if it's a teacher, close student and open teacher here.
                if (TeacherIDObject != null) TeacherIDObject.SetActive(true);
                if (StudentIDObject != null) StudentIDObject.SetActive(false);
                
                if (TeacherIDPrivateCode != null)
                {
                    TeacherIDPrivateCode.text = "Teacher ID: " + teacherCode;
                }
                else
                {
                    Debug.LogWarning("TeacherIDPrivateCode component not assigned! Teacher code: " + teacherCode);
                }
            }
            else if (firebaseProxyService.UserType == "student")
            {
                PopUpPanel.GetComponent<ActiveStateWatcher>().OnBecameInactive.AddListener(PostSignUpStudentLogin);
                
                string studentCode = firebaseProxyService.PrivateCode;
                
                // Here I'm doing the opposite, opening the student.
                if (StudentIDObject != null) StudentIDObject.SetActive(true);
                if (TeacherIDObject != null) TeacherIDObject.SetActive(false);
                
                if (StudentIDPrivateCode != null)
                {
                    StudentIDPrivateCode.text = "Student ID: " + studentCode;
                }
                else
                {
                    Debug.LogWarning("StudentIDPrivateCode component not assigned! Student code: " + studentCode);
                }
            }
        }
        else
        {
            Debug.LogError($"Registration failed! Error: {message}");
            if (message.Contains("email address is already in use"))
            {
                PopUpError(16); // Show popup for student is not in the teacher's list
            }
            else
                PopUpError(0); // Show error here too.
        }
    }

    public void GoToSelectPanel()
    {
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        SelectPanel.SetActive(true);
    }

    // Method to load student list
    private void LoadStudentList()
    {
        if (_isLoadingStudents)
        {
            Debug.LogWarning("A student list loading process is already in progress, please wait.");
            return;
        }
        
        _isLoadingStudents = true;
        
        if (firebaseProxyService != null)
        {
            // First clear existing list elements
            ClearStudentList();
            
            firebaseProxyService.GetTeacherStudents((success, students) => {
                _isLoadingStudents = false;
                
                if (success && students != null)
                {
                    Debug.Log($"Student list loaded successfully! Student count: {students.Count}");
                    DisplayStudentList(students);
                }
                else
                {
                    Debug.LogError("Student list could not be loaded!");
                    // Show a demo student by default
                    var demoStudents = new List<StudentInfo>
                    {
                        new StudentInfo { 
                            studentId = "STU-DEMO", 
                            username = "DemoStudent",
                            currentLevel = 1, 
                            currentWorld = 1, 
                            lastTimePlayed = "Not played yet", 
                            totalTime = 0,
                            worldsData = new WorldsData { worlds = new List<WorldInfo>() }
                        }
                    };
                    DisplayStudentList(demoStudents);
                }
            });
        }
        else
        {
            _isLoadingStudents = false;
            Debug.LogError("FirebaseProxyService cannot be used.");
        }
    }
    
    // Clear existing student list
    private void ClearStudentList()
    {
        if (StudentListContent == null)
        {
            Debug.LogError("StudentListContent reference not assigned!");
            return;
        }
        
        // Clear student list, detail areas and old ScrollRect
        for (int i = StudentListContent.childCount - 1; i >= 0; i--)
        {
            Transform child = StudentListContent.GetChild(i);
            // Delete student main info, DetailedScrollRects and old ScrollRect
            if (child.name.Contains("student-main-info") || 
                child.name.Contains("DetailedScrollRect-") || 
                child.name == "ScrollRect")
            {
                Destroy(child.gameObject);
                Debug.Log($"Deleted: {child.name}");
            }
            else
            {
                Debug.Log($"Preserved: {child.name}");
            }
        }
        
        studentList.Clear();
    }
    
    
    private void DisplayStudentList(List<StudentInfo> students)
    {
        if (StudentListContent == null)
        {
            Debug.LogError("StudentListContent reference not assigned!");
            return;
        }
        
        if (StudentListItemPrefab == null)
        {
            Debug.LogError("StudentListItemPrefab reference not assigned!");
            return;
        }
        
        studentList = students;
        
        // Create a row for each student
        foreach (var student in students)
        {
            // Instantiate the prefab
            GameObject listItem = Instantiate(StudentListItemPrefab, StudentListContent);
            
            // Set student information
            var studentId = listItem.transform.Find("Added-Student-ID")?.GetComponent<TMP_Text>();
            var playerName = listItem.transform.Find("PlayerName")?.GetComponent<TMP_Text>();
            var currentLevel = listItem.transform.Find("Current-level")?.GetComponent<TMP_Text>();
            var currentWorld = listItem.transform.Find("Current-world")?.GetComponent<TMP_Text>();
            var lastTimePlayed = listItem.transform.Find("Last-time-played")?.GetComponent<TMP_Text>();
            var totalTime = listItem.transform.Find("total-time")?.GetComponent<TMP_Text>();
            
            if (studentId != null)
            {
                studentId.text = student.studentId; // Show private code (STU-1234)
            }
            
            if (playerName != null)
            {
                // Show username if available, otherwise fallback to "No Username"
                string displayUsername = !string.IsNullOrEmpty(student.username) ? 
                                       student.username : 
                                       "No Username";
                playerName.text = displayUsername;
                Debug.Log($"Player name: {displayUsername} (Username: {student.username}, ID: {student.studentId})");
            }
            
            if (currentLevel != null)
            {
                currentLevel.text = student.currentLevel.ToString();
            }
            
            if (currentWorld != null)
            {
                currentWorld.text = student.currentWorld.ToString();
            }
            
            if (lastTimePlayed != null)
            {
                lastTimePlayed.text = DateUtils.GetDaysAgoStringFromString(student.lastTimePlayed);
            }
            
            if (totalTime != null)
            {
                totalTime.text = SessionTimeTracker.GetFormattedTimePlayed(student.totalTime);
            }
            
            // Button functionality - create/show detail area for each student
            Button studentButton = listItem.GetComponent<Button>();
            if (studentButton == null)
            {
                studentButton = listItem.AddComponent<Button>();
                Debug.Log($"Button component added: {student.studentId}");
            }
            
            // Add click event to button - new dynamic ScrollRect logic
            studentButton.onClick.RemoveAllListeners();
            studentButton.onClick.AddListener(() => {
                Debug.Log($"Student button clicked: {student.studentId}");
                ToggleStudentDetailedInfo(student, listItem.transform);
            });
        }
    }
    
    // Method to update all student information
    public void HandleUpdateAllStudentInfo()
    {
        Debug.Log("Student list refreshing...");
        LoadStudentList(); // Only refresh data, don't write to Firebase
    }

    // Open/close student detailed information
    private void ToggleStudentDetailedInfo(StudentInfo student, Transform studentItemTransform)
    {
        // Check if that student's DetailedScrollRectPrefab exists
        string detailedScrollRectName = $"DetailedScrollRect-{student.studentId}";
        Transform existingDetailedScrollRect = StudentListContent.Find(detailedScrollRectName);
        
        if (existingDetailedScrollRect != null)
        {
            // If it exists, open/close it
            bool isActive = existingDetailedScrollRect.gameObject.activeSelf;
            existingDetailedScrollRect.gameObject.SetActive(!isActive);
            Debug.Log($"DetailedScrollRect {(isActive ? "closed" : "opened")}: {student.studentId}");
        }
        else
        {
            // If it doesn't exist, create it and place it right under the student
            CreateDetailedScrollRectForStudent(student, studentItemTransform);
        }
    }
    
    // Create dynamic DetailedScrollRect for student
    private void CreateDetailedScrollRectForStudent(StudentInfo student, Transform studentItemTransform)
    {
        if (DetailedScrollRectPrefab == null)
        {
            Debug.LogError("DetailedScrollRectPrefab not assigned! Assign it in the Inspector.");
            return;
        }
        
        // Create DetailedScrollRectPrefab
        GameObject detailedScrollRect = Instantiate(DetailedScrollRectPrefab, StudentListContent);
        detailedScrollRect.name = $"DetailedScrollRect-{student.studentId}";
        
        // Place it right under the student
        int studentIndex = studentItemTransform.GetSiblingIndex();
        detailedScrollRect.transform.SetSiblingIndex(studentIndex + 1);
        
        Debug.Log($"DetailedScrollRect created and placed: {student.studentId}, Index: {studentIndex + 1}");
        
        // Add detailed-info inside - find Content according to prefab structure
        Transform content = null;
        
        // First check if prefab itself has ScrollRect component
        ScrollRect scrollRectComponent = detailedScrollRect.GetComponent<ScrollRect>();
        if (scrollRectComponent != null)
        {
            // If prefab itself is ScrollRect, get Content directly
            content = detailedScrollRect.transform.Find("Content");
            Debug.Log("ScrollRect component found at prefab root.");
        }
        else
        {
            // Check if there's a child named "ScrollRect" inside the prefab
            Transform scrollRectTransform = detailedScrollRect.transform.Find("ScrollRect");
            if (scrollRectTransform != null)
            {
                content = scrollRectTransform.Find("Content");
                Debug.Log("ScrollRect found as child.");
            }
        }
        
        if (content == null)
        {
            Debug.LogError("'Content' not found inside DetailedScrollRectPrefab! Check prefab structure.");
            Debug.Log($"Prefab structure: {detailedScrollRect.name}");
            for (int i = 0; i < detailedScrollRect.transform.childCount; i++)
            {
                Debug.Log($"Child {i}: {detailedScrollRect.transform.GetChild(i).name}");
            }
            return;
        }
        
        // Load detailed information
        DisplayDetailedStudentInfoInContent(student, content);
    }

    private void DisplayDetailedStudentInfoInContent(StudentInfo student, Transform content)
    {
        if (DetailedStudentInfoPrefab == null)
        {
            Debug.LogError("DetailedStudentInfoPrefab not assigned! Assign it in the Inspector.");
            return;
        }
        
        // Clear existing detailed information
        foreach (Transform child in content)
        {
            if (child.name.Contains("studen-detailed-info"))
            {
                Destroy(child.gameObject);
            }
        }
        
        // Check new hierarchical data structure
        if (student.worldsData == null || student.worldsData.worlds == null || student.worldsData.worlds.Count == 0)
        {
            Debug.Log($"No detailed information for student {student.studentId}, creating default data.");

            // Create default data (this is about what and how we return, I'll make firebase accordingly with mapping etc.)
            student.worldsData = new WorldsData
            {
                worlds = new List<WorldInfo>
                {
                    new WorldInfo
                    {
                        worldNumber = 1,
                        levels = new List<LevelInfo>
                        {
                            new LevelInfo
                            {
                                levelNumber = 1,
                                attempts = 0,
                                successes = 0,
                                //fails = 0,
                                maxScore = 0,
                                //isCorrect = false,
                                successRate = 0.0f,
                                starRating = 0,
                                accuracyBreakdown = new List<AccuracyBreakdownItem>()
                                // Other fields (created with default values)
                            }
                        }
                    }
                }
            };
        }
        
        
        foreach (var world in student.worldsData.worlds)
        {
            foreach (var level in world.levels)
            {
                
                GameObject detailItem = Instantiate(DetailedStudentInfoPrefab, content);
                detailItem.SetActive(true);
                
                // Information assignment place - assigning detailed inner locations here.
                var worldText = detailItem.transform.Find("World")?.GetComponent<TMP_Text>();
                var levelText = detailItem.transform.Find("Level")?.GetComponent<TMP_Text>();
                var attemptsText = detailItem.transform.Find("Attempts")?.GetComponent<TMP_Text>();
                var successesText = detailItem.transform.Find("Successes")?.GetComponent<TMP_Text>();
                var failsText = detailItem.transform.Find("Fails")?.GetComponent<TMP_Text>();
                var maxScoreText = detailItem.transform.Find("MaxScore")?.GetComponent<TMP_Text>();
                var averageAccuracyText = detailItem.transform.Find("Average-accuracy")?.GetComponent<TMP_Text>();
                //var accuracyBreakdownText = detailItem.transform.Find("AccuracyBreakdown")?.GetComponent<TMP_Text>();
                var goodText = detailItem.transform.Find("Good")?.GetComponent<TMP_Text>();
                var greatText = detailItem.transform.Find("Great")?.GetComponent<TMP_Text>();
                var missText = detailItem.transform.Find("Miss")?.GetComponent<TMP_Text>();
                var okText = detailItem.transform.Find("OK")?.GetComponent<TMP_Text>();
                var perfectText = detailItem.transform.Find("Perfect")?.GetComponent<TMP_Text>();

                var checkObj = detailItem.transform.Find("Check")?.gameObject;
                var falseObj = detailItem.transform.Find("False")?.gameObject;

                //var starPrefab = detailItem.transform.Find("Star")?.gameObject;
                //var starParent = starPrefab?.transform.parent;

                var starsContainer = detailItem.transform.Find("Stars");
                var starTemplate = starsContainer?.Find("Star");

                // Required fields (what we show in any case)
                if (worldText != null) worldText.text = "W: " + world.worldNumber;
                if (levelText != null) levelText.text = "LV: " + level.levelNumber;
                
                // Dynamic fields (only show existing ones)
                if (attemptsText != null) {
                    if (level.attempts > 0) {
                        attemptsText.text = "" + level.attempts.ToString();
                        attemptsText.gameObject.SetActive(true);
                    } else {
                        attemptsText.gameObject.SetActive(false);
                    }
                }
                
                if (successesText != null) {
                    if (level.successes > 0) {
                        successesText.text = "" + level.successes.ToString();
                        successesText.gameObject.SetActive(true);
                    } else {
                        successesText.gameObject.SetActive(false);
                    }
                }
                
                if (failsText != null) {
                    // if (level.fails > 0) {
                    //     failsText.text = "" + level.fails.ToString();
                    //     failsText.gameObject.SetActive(true);
                    // } else {
                        failsText.gameObject.SetActive(false);
                    //}
                }
                
                if (maxScoreText != null) {
                    if (level.maxScore > 0) {
                        maxScoreText.text = "" + level.maxScore.ToString();
                        maxScoreText.gameObject.SetActive(true);
                    } else {
                        maxScoreText.gameObject.SetActive(false);
                    }
                }
                
                if (averageAccuracyText != null) {
                    if (level.successRate > 0.0f) {
                        averageAccuracyText.text = "%" + level.successRate.ToString("F1");
                        averageAccuracyText.gameObject.SetActive(true);
                    } else {
                        averageAccuracyText.gameObject.SetActive(false);
                    }
                }

                if (goodText != null) goodText.gameObject.SetActive(false);
                if (greatText != null) greatText.gameObject.SetActive(false);
                if (missText != null) missText.gameObject.SetActive(false);
                if (okText != null) okText.gameObject.SetActive(false);
                if (perfectText != null) perfectText.gameObject.SetActive(false);

                if (level.accuracyBreakdown != null && level.accuracyBreakdown.Count > 0)
                {

                    foreach (var breakdown in level.accuracyBreakdown)
                    {
                        switch (breakdown.noteName)
                        {
                            case "Good":
                                if (goodText != null)
                                {
                                    goodText.text = "%" + breakdown.percentage.ToString("F1");
                                    goodText.gameObject.SetActive(true);
                                }
                                break;
                            case "Great":
                                if (greatText != null)
                                {
                                    greatText.text = "%" + breakdown.percentage.ToString("F1");
                                    greatText.gameObject.SetActive(true);
                                }
                                break;
                            case "Miss":
                                if (missText != null)
                                {
                                    missText.text = "%" + breakdown.percentage.ToString("F1");
                                    missText.gameObject.SetActive(true);
                                }
                                break;
                            case "OK":
                                if (okText != null)
                                {
                                    okText.text = "%" + breakdown.percentage.ToString("F1");
                                    okText.gameObject.SetActive(true);
                                }
                                break;
                            case "Perfect":
                                if (perfectText != null)
                                {
                                    perfectText.text = "%" + breakdown.percentage.ToString("F1");
                                    perfectText.gameObject.SetActive(true);
                                }
                                break;
                        }
                    }
                    Debug.Log($"Wrote AccuracyBreakdown to separate TextMeshPro objects - Count: {level.accuracyBreakdown.Count}");
                }
                else
                {
                    Debug.Log($"No AccuracyBreakdown data - W:{world.worldNumber} LV:{level.levelNumber}");
                }

                //if (accuracyBreakdownText != null)
                //{
                //    if (level.accuracyBreakdown != null && level.accuracyBreakdown.Count > 0)
                //    {
                //        string breakdownText = "Piano: ";
                //        for (int i = 0; i < level.accuracyBreakdown.Count; i++)
                //        {
                //            var breakdown = level.accuracyBreakdown[i];
                //            breakdownText += breakdown.noteName + " %" + breakdown.percentage.ToString("F1");
                //            if (i < level.accuracyBreakdown.Count - 1)
                //            {
                //                breakdownText += ", ";
                //            }
                //        }
                //        accuracyBreakdownText.text = breakdownText;
                //        accuracyBreakdownText.gameObject.SetActive(true);
                //        Debug.Log($"AccuracyBreakdown displayed: {breakdownText}");
                //    }
                //    else
                //    {
                //        accuracyBreakdownText.gameObject.SetActive(false);
                //        Debug.Log($"No AccuracyBreakdown data - W:{world.worldNumber} LV:{level.levelNumber}");
                //    }
                //}

                // Special logic for Check/False (check if isCorrect field exists)
                // Note: It's difficult to check default values for bool fields in Unity, so we hide both if necessary
                if (checkObj != null) checkObj.SetActive(/*level.isCorrect*/ level.successes > 0);
                if (falseObj != null) falseObj.SetActive(/*!level.isCorrect*/ level.successes < 1);


                // Dynamic star creation
                if (starsContainer != null && starTemplate != null)
                {

                    int starCount = level.starRating;


                    for (int i = starsContainer.childCount - 1; i >= 0; i--)
                    {
                        var child = starsContainer.GetChild(i);
                        if (child.name.Contains("DynamicStar"))
                        {
                            DestroyImmediate(child.gameObject);
                        }
                    }


                    starTemplate.gameObject.SetActive(false);


                    for (int i = 0; i < starCount; i++)
                    {
                        GameObject newStar = Instantiate(starTemplate.gameObject, starsContainer);
                        newStar.name = $"DynamicStar_{i}";
                        newStar.SetActive(true);

                        Debug.Log($"Star {i + 1}/{starCount} created (in Stars container) - Level: W{level.levelNumber}");
                    }

                    Debug.Log($"Total {starCount} stars created (in Stars container) - Level: W{level.levelNumber}");
                }
                else
                {
                    Debug.LogWarning($"Stars container or Star template not found - Level: W{level.levelNumber}");
                }

                Debug.Log($"Level detail added: W:{world.worldNumber} LV:{level.levelNumber} Attempts:{level.attempts}");
            }
        }
    }

    // Logout process - for both teacher and student
    public void HandleLogout()
    {
        Debug.Log("Starting logout process...");
        
        if (firebaseProxyService == null)
        {
            Debug.LogError("FirebaseProxyService unavailable!");
            // Fallback: just switch UI to login screen
            GoToSelectPanel();
            return;
        }

        // Send logout request to backend
        firebaseProxyService.LogoutWithBackend((success, message) => {
            if (success)
            {
                Debug.Log("Logout successful: " + message);
                ShowLogoutSuccess();
            }
            else
            {
                Debug.LogWarning("Logout partially failed: " + message);
                // Even if partially failed, switch UI to login screen
                ShowLogoutSuccess();
            }
        });
    }

    // Show login screen after logout
    private void ShowLogoutSuccess()
    {
        Debug.Log("Logout completed, returning to login screen...");
        
        // Close all panels
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        if (TeacherLobbyPanel != null)
        {
            TeacherLobbyPanel.SetActive(false);
        }
        PopUpPanel.SetActive(false);
        
        // Clear form fields (for security)
        ClearAllLoginFields();
        
        // Open login screen
        SelectPanel.SetActive(true);
        
        Debug.Log("Login screen displayed.");
    }

    // Clear all login form fields
    private void ClearAllLoginFields()
    {
        // Clear Teacher Login fields
        if (TeacherLoginEmailInput != null)
        {
            TeacherLoginEmailInput.text = "";
            Debug.Log("Teacher login email field cleared.");
        }
        
        if (TeacherLoginPasswordInput != null)
        {
            TeacherLoginPasswordInput.text = "";
            Debug.Log("Teacher login password field cleared.");
        }
        
        // Clear Student Login fields
        if (StudentSigninUsername != null)
        {
            StudentSigninUsername.text = "";
            Debug.Log("Student login username field cleared.");
        }
        
        if (StudentSigninPassword != null)
        {
            StudentSigninPassword.text = "";
            Debug.Log("Student login password field cleared.");
        }
        
        // Clear SignUp fields too (bonus security)
        if (TeacherMailInput != null)
        {
            TeacherMailInput.text = "";
        }
        
        if (TeacherUsernameInput != null)
        {
            TeacherUsernameInput.text = "";
        }
        
        if (TeacherPassInput != null)
        {
            TeacherPassInput.text = "";
        }
        
        if (StudentMailInput != null)
        {
            StudentMailInput.text = "";
        }
        
        if (StudentUsernameInput != null)
        {
            StudentUsernameInput.text = "";
        }
        
        if (StudentPassInput != null)
        {
            StudentPassInput.text = "";
        }
        
        Debug.Log("All form fields cleared for security.");
    }

    void ToggleVisibility(PasswordTogglePair pair)
    {
        pair.isVisible = !pair.isVisible;
        pair.inputField.contentType = pair.isVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        pair.inputField.ForceLabelUpdate();
    }
}