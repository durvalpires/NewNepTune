using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    public TMP_InputField TeacherPassInput;
    public Button TeacherSignUpButton;

    [Header("Teacher Login UI")]
    public TMP_InputField TeacherLoginEmailInput;
    public TMP_InputField TeacherLoginPasswordInput;
    public Button TeacherLoginButton;

    [Header("Teacher_Popup_User_Info panelindeki öğretmen kodu texti")]
    public TMP_Text TeacherIDText;

    [Header("Teacher_Popup_User_Info panelindeki öğrenci ekleme UI'ları")]
    public TMP_InputField StudentIDAddInput;
    public Button AddStudentButton;

    [Header("Student Panel UI")]
    public TMP_InputField StudentMailInput;
    public TMP_InputField StudentPassInput;
    public Button StudentSignUpButton;

    [Header("Student Login UI")]
    public TMP_InputField StudentSigninUsername;
    public TMP_InputField StudentSigninPassword;
    public Button StudentSigninButton;

    [Header("Teacher Lobby - Öğrenci Listesi UI")]
    public Transform StudentListContent; 
    public GameObject StudentListItemPrefab; 
    public GameObject DetailedStudentInfoPrefab; 
    public GameObject DetailedScrollRectPrefab;
    public Button UpdateStudentInfoButton;

    public GameObject PopUpPanel;
    public GameObject PopUpMessageItems;
    
    // SignUp Success panel için öğrenci falan objeleri sürükleyecem buradan.
    public GameObject StudentIDObject;
    public GameObject TeacherIDObject;
    public GameObject LoginSuccessObject;

    public TMP_Text StudentIDPrivateCode;
    public TMP_Text TeacherIDPrivateCode;

    [Header("Teacher Lobby - Öğretmen Kullanıcı Adı Gösterimi")]
    public TMP_Text TeacherUserNameText;

    [Header("Logout Buttons")]
    public Button TeacherLogoutButton;  // Teacher panel'deki logout butonu
    public Button StudentLogoutButton;  // Student panel'deki logout butonu

    private FirebaseProxyService firebaseProxyService;

    private bool _isRegistering = false; 
    private bool _isLoggingIn = false; 
    private bool _isAddingStudent = false; 
    private bool _isLoadingStudents = false; 

    // Eklenen öğrencilerin listesi
    private List<StudentInfo> studentList = new List<StudentInfo>();
    
    // Burayı o butondaki on clikce ekleyip ShowTeacherUserInfo 'yu işaretlememe lazım.
    public void ShowTeacherUserInfo()
    {
        // Öğretmen kodunu göster
        if (TeacherIDText != null && firebaseProxyService != null)
        {
            TeacherIDText.text = firebaseProxyService.teacherPrivateCode;
            Debug.Log("Öğretmen kodu gösteriliyor: " + firebaseProxyService.teacherPrivateCode);
        }
        else
        {
            Debug.LogWarning("TeacherIDText bileşeni atanmamış veya firebaseProxyService bulunamadı!");
        }
    }
    
    public void PopUpError(int x)
    {
        PopUpPanel.SetActive(true);
        for (int i = 0; i < PopUpMessageItems.transform.childCount; i++)
        {
            PopUpMessageItems.transform.GetChild(i).gameObject.SetActive(i == x);
        }
        
        // Login Success ikide bir çıkıyordu , login eklediğimde burayı açıcam ama diğerlerini kapatmam lazım o UNUTMA.
        if (LoginSuccessObject != null)
        {
            LoginSuccessObject.SetActive(false);
        }
    }
    
    // Login Success popup'ını göstermek için yeni metot
    public void ShowLoginSuccess()
    {
        PopUpPanel.SetActive(true);
        
        // Tüm panelleri kapat
        for (int i = 0; i < PopUpMessageItems.transform.childCount; i++)
        {
            PopUpMessageItems.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        // Sadece LoginSuccess panelini aç
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
            Debug.LogError("FirebaseProxyService instance bulunamadı! Sahneye eklediğinizden emin olun.");
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
            Debug.LogWarning("TeacherLobbyPanel atanmamış! Öğretmen giriş yaptığında gösterilecek ekran eksik.");
        }

        // Button listener'ları temizle ve sonrasında yendiden ekliyorum burda ki çiftlemenin önüne geçebileyim .
        if (TeacherSignUpButton != null)
        {
            TeacherSignUpButton.onClick.RemoveAllListeners(); 
            TeacherSignUpButton.onClick.AddListener(HandleTeacherSignUp);
            Debug.Log("TeacherSignUpButton listener eklendi.");
        }
        else
        {
            Debug.LogError("TeacherSignUpButton referansı atanmamış!");
        }
            
        if (TeacherLoginButton != null)
        {
            TeacherLoginButton.onClick.RemoveAllListeners(); 
            TeacherLoginButton.onClick.AddListener(HandleTeacherLogin);
            Debug.Log("TeacherLoginButton listener eklendi.");
        }
        else
        {
            Debug.LogWarning("TeacherLoginButton referansı atanmamış! Öğretmen giriş butonu çalışmayacak.");
        }

        if (StudentSignUpButton != null)
        {
            StudentSignUpButton.onClick.RemoveAllListeners(); 
            StudentSignUpButton.onClick.AddListener(HandleStudentSignUp);
            Debug.Log("StudentSignUpButton listener eklendi.");
        }
        else
        {
            Debug.LogError("StudentSignUpButton referansı atanmamış!");
        }
        
        if (StudentSigninButton != null)
        {
            StudentSigninButton.onClick.RemoveAllListeners();
            StudentSigninButton.onClick.AddListener(HandleStudentLogin);
            Debug.Log("StudentSigninButton listener eklendi.");
        }
        else
        {
            Debug.LogWarning("StudentSigninButton referansı atanmamış! Öğrenci giriş butonu çalışmayacak.");
        }
        
        
        if (AddStudentButton != null)
        {
            AddStudentButton.onClick.RemoveAllListeners();
            AddStudentButton.onClick.AddListener(HandleAddStudent);
            Debug.Log("AddStudentButton listener eklendi.");
        }
        else
        {
            Debug.LogWarning("AddStudentButton referansı atanmamış! Öğrenci ekleme butonu çalışmayacak.");
        }
        
        
        if (UpdateStudentInfoButton != null)
        {
            UpdateStudentInfoButton.onClick.RemoveAllListeners();
            UpdateStudentInfoButton.onClick.AddListener(HandleUpdateAllStudentInfo);
            Debug.Log("UpdateStudentInfoButton listener eklendi.");
        }
        
        // Logout Button Listeners
        if (TeacherLogoutButton != null)
        {
            TeacherLogoutButton.onClick.RemoveAllListeners();
            TeacherLogoutButton.onClick.AddListener(HandleLogout);
            Debug.Log("TeacherLogoutButton listener eklendi.");
        }
        else
        {
            Debug.LogWarning("TeacherLogoutButton referansı atanmamış!");
        }
        
        if (StudentLogoutButton != null)
        {
            StudentLogoutButton.onClick.RemoveAllListeners();
            StudentLogoutButton.onClick.AddListener(HandleLogout);
            Debug.Log("StudentLogoutButton listener eklendi.");
        }
        else
        {
            Debug.LogWarning("StudentLogoutButton referansı atanmamış!");
        }
    }

    //öğreni ekleme yerini yani öğretmenin öğrenci ekleme yapmasını sağlayan kısım.
    public void HandleAddStudent()
    {
        // Zaten öğrenci ekleme işlemi devam ediyorsa, yeni bir işlem başlatma
        if (_isAddingStudent)
        {
            Debug.LogWarning("Zaten bir öğrenci ekleme işlemi devam ediyor, lütfen bekleyin.");
            return;
        }
        
        // Öğrenci ID'sini al
        string studentID = StudentIDAddInput.text.Trim();
        
        if (string.IsNullOrEmpty(studentID))
        {
            Debug.LogError("Öğrenci ID alanı boş bırakılamaz.");
            PopUpError(3); 
            return;
        }
        
        // ID formatını kontrol et (STU- ile başlamalı) - STU ile başlamayan öğrenci kodunda hata almamız lazımdı.
        if (!studentID.StartsWith("STU-"))
        {
            Debug.LogError("Geçersiz öğrenci ID formatı. ID 'STU-' ile başlamalıdır.");
            PopUpError(2); 
            return;
        }
        
        Debug.Log($"Öğrenci ekleme işlemi başlatılıyor: StudentID: {studentID}");
        
        // Öğrenci ekleme işlemini başlat
        _isAddingStudent = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.AddStudentToTeacher(studentID, (success, message) => {
                
                _isAddingStudent = false;
                OnAddStudentCompleted(success, message);
            });
        }
        else
        {
            _isAddingStudent = false; 
            Debug.LogError("FirebaseProxyService kullanılamıyor.");
        }
    }
    
    
    private void OnAddStudentCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Öğrenci başarıyla eklendi! Mesaj: {message}");
            
            // Başarı mesajı göster
            PopUpError(4); // Blue popup paneli (başarı mesajı için) - indeksi düzeltildi
            
            
            if (StudentIDAddInput != null)
            {
                StudentIDAddInput.text = "";
            }
            
            
            LoadStudentList();
        }
        else
        {
            Debug.LogError($"Öğrenci ekleme başarısız! Hata: {message}");
            PopUpError(0); // Hata popup'ını göster
        }
    }

    public void OnSelectTeacher()
    {
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(true);
        StudentPanel.SetActive(false);
        Debug.Log("Öğretmen paneli seçildi.");
    }

    public void OnSelectStudent()
    {
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(true);
        Debug.Log("Öğrenci paneli seçildi.");
    }

    public void HandleTeacherSignUp()
    {
        // Zaten kayıt işlemi devam ediyorsa, yeni bir kayıt başlatma
        if (_isRegistering)
        {
            Debug.LogWarning("Zaten bir kayıt işlemi devam ediyor, lütfen bekleyin.");
            return;
        }
        
        string email = TeacherMailInput.text;
        string password = TeacherPassInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email ve şifre alanları boş bırakılamaz.");
            PopUpError(2);
            return;
        }

        var userProfile = new Dictionary<string, object>
        {
            { "userType", "teacher" }
        };

        Debug.Log($"Öğretmen kaydı başlatılıyor: Email: {email}");
        
        // Kayıt işlemini başlat
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
            _isRegistering = false; // FirebaseProxyService yoksa flag'i reset et
            Debug.LogError("FirebaseProxyService kullanılamıyor.");
        }
    }
    
    // Öğretmen girişi için yeni metot
    public void HandleTeacherLogin()
    {
        // Zaten giriş işlemi devam ediyorsa, yeni bir giriş başlatma
        if (_isLoggingIn)
        {
            Debug.LogWarning("Zaten bir giriş işlemi devam ediyor, lütfen bekleyin.");
            return;
        }
        
        string email = TeacherLoginEmailInput.text;
        string password = TeacherLoginPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email ve şifre alanları boş bırakılamaz.");
            PopUpError(3); // EmptyError popup'ını göster (tutarlılık için)
            return;
        }

        Debug.Log($"Öğretmen girişi başlatılıyor: Email: {email}");
        
        
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
            Debug.LogError("FirebaseProxyService kullanılamıyor.");
        }
    }
    
    // Login callback metodu
    private void OnLoginCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Giriş başarılı! UserID: {message}");
            
            // Login Success popup'ını göster
            ShowLoginSuccess();
            
            // Kullanıcı tipine göre işlem yap
            if (firebaseProxyService.UserType == "teacher")
            {
                // Öğretmen kullanıcı adını Teacher Lobby'de göster
                if (TeacherUserNameText != null && firebaseProxyService != null)
                {
                    TeacherUserNameText.text = firebaseProxyService.teacherPrivateCode;
                    Debug.Log("Öğretmen kullanıcı adı TeacherUserNameText'e atandı: " + firebaseProxyService.teacherPrivateCode);
                }
                else
                {
                    Debug.LogWarning("TeacherUserNameText referansı atanmamış veya firebaseProxyService bulunamadı!");
                }
                
                // Kısa bir süre sonra öğretmen paneline yönlendir
                StartCoroutine(ShowTeacherLobbyAfterDelay(2.0f));
            }
            else if (firebaseProxyService.UserType == "student")
            {
                // İleride öğrenci paneline yönlendirme yapılabilir
                Debug.Log("Öğrenci girişi yapıldı, öğrenci paneline yönlendirme eklenecek.");
            }
        }
        else
        {
            Debug.LogError($"Giriş başarısız! Hata: {message}");
            PopUpError(0); // Hata popup'ını göster
        }
    }
    
    // Öğrenci login callback metodu
    private void OnStudentLoginCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Öğrenci girişi başarılı! UserID: {message}");
            
            // Login Success popup'ını göster
            ShowLoginSuccess();
            StartCoroutine(ShowStudentLobbyAfterDelay(2.0f));

            // Kullanıcı tipini kontrol et
            if (firebaseProxyService.UserType == "student")
            {
                Debug.Log("Öğrenci başarıyla giriş yaptı!");
                Debug.Log("Öğrenci kodu: " + firebaseProxyService.privateCode);
                Debug.Log("Current Level: " + firebaseProxyService.CurrentLevel);
                Debug.Log("Current World: " + firebaseProxyService.CurrentWorld);
                
                // İleride öğrenci dashboard/panel'ine yönlendirme yapılabilir
                // StartCoroutine(ShowStudentDashboardAfterDelay(2.0f));
            }
            else
            {
                Debug.LogWarning("Kullanıcı tipi öğrenci değil, beklenmeyen durum!");
                PopUpError(0); // Hata popup'ını göster
            }
        }
        else
        {
            Debug.LogError($"Öğrenci girişi başarısız! Hata: {message}");
            PopUpError(0); // Hata popup'ını göster
        }
    }
    
    // Öğretmen panelini gecikmeli gösterme coroutine'i
    private IEnumerator ShowTeacherLobbyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Tüm panelleri kapat
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        PopUpPanel.SetActive(false);
        
        // Öğretmen lobisini göster
        if (TeacherLobbyPanel != null)
        {
            TeacherLobbyPanel.SetActive(true);
            Debug.Log("Öğretmen lobisi gösteriliyor.");
            
            // Öğretmen kullanıcı adını bir kez daha kontrol et (backup)
            if (TeacherUserNameText != null && firebaseProxyService != null && !string.IsNullOrEmpty(firebaseProxyService.teacherPrivateCode))
            {
                TeacherUserNameText.text = firebaseProxyService.teacherPrivateCode;
                Debug.Log("Öğretmen kullanıcı adı backup kontrolü: " + firebaseProxyService.teacherPrivateCode);
            }
            
            // Öğrenci listesini yükle
            LoadStudentList();
        }
        else
        {
            Debug.LogError("TeacherLobbyPanel atanmamış! Öğretmen lobisi gösterilemiyor.");
        }
    }

    private IEnumerator ShowStudentLobbyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Tüm panelleri kapat
        //SelectPanel.SetActive(false);
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        PopUpPanel.SetActive(false);

        SceneManager.LoadScene("v2_MainMenu");
    }

    public void HandleStudentSignUp()
    {
        // Zaten kayıt işlemi devam ediyorsa, yeni bir kayıt başlatma
        if (_isRegistering)
        {
            Debug.LogWarning("Zaten bir kayıt işlemi devam ediyor, lütfen bekleyin.");
            return;
        }
        
        string email = StudentMailInput.text;
        string password = StudentPassInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email ve şifre alanları boş bırakılamaz.");
            PopUpError(3); // EmptyError popup'ını göster (tutarlılık için)
            return;
        }

        var userProfile = new Dictionary<string, object>
        {
            { "userType", "student" }
        };

        Debug.Log($"Öğrenci kaydı başlatılıyor: Email: {email}");
        
        // Kayıt işlemini başlat
        _isRegistering = true;
        
        if (firebaseProxyService != null)
        {
            firebaseProxyService.RegisterUserWithProfile(email, password, userProfile, (success, message) => {
                // Kayıt işlemi tamamlandı, flag'i reset et
                _isRegistering = false;
                OnSignUpCompleted(success, message);
            });
        }
        else
        {
            _isRegistering = false; // FirebaseProxyService yoksa flag'i reset et
            Debug.LogError("FirebaseProxyService kullanılamıyor.");
        }
    }
    
    // Öğrenci girişi için yeni metot
    public void HandleStudentLogin()
    {
        // Zaten giriş işlemi devam ediyorsa, yeni bir giriş başlatma
        if (_isLoggingIn)
        {
            Debug.LogWarning("Zaten bir giriş işlemi devam ediyor, lütfen bekleyin.");
            return;
        }
        
        string email = StudentSigninUsername.text;
        string password = StudentSigninPassword.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email ve şifre alanları boş bırakılamaz.");
            PopUpError(3); // EmptyError popup'ını göster (tutarlılık için)
            return;
        }

        Debug.Log($"Öğrenci girişi başlatılıyor: Email: {email}");
        
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
            Debug.LogError("FirebaseProxyService kullanılamıyor.");
        }
    }

    private void OnSignUpCompleted(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Kayıt başarılı! Mesaj/UserID: {message}");
            
            
            PopUpError(1); // SignUp Success Yazan popup bildirimi buradan gösteriyoruz ( 1. sırada olan o )
            
            // Öğrenci veya öğretmen oluşuna göre göster eğer Usertype a göre gösteriyorum burada. 
            if (firebaseProxyService.UserType == "teacher")
            {
                string teacherCode = firebaseProxyService.teacherPrivateCode;
                
                // TeacherID yi gösterme bölümü ancak öğretmesek eğer , öğrenciyi kapatıyoruz öğretmeni açıyorum burada.
                if (TeacherIDObject != null) TeacherIDObject.SetActive(true);
                if (StudentIDObject != null) StudentIDObject.SetActive(false);
                
                if (TeacherIDPrivateCode != null)
                {
                    TeacherIDPrivateCode.text = "Teacher ID: " + teacherCode;
                }
                else
                {
                    Debug.LogWarning("TeacherIDPrivateCode bileşeni atanmamış! Öğretmen kodu: " + teacherCode);
                }
            }
            else if (firebaseProxyService.UserType == "student")
            {
                string studentCode = firebaseProxyService.privateCode;
                
                // Burada da tam tersi öğrenciyi açıyorum.
                if (StudentIDObject != null) StudentIDObject.SetActive(true);
                if (TeacherIDObject != null) TeacherIDObject.SetActive(false);
                
                if (StudentIDPrivateCode != null)
                {
                    StudentIDPrivateCode.text = "Student ID: " + studentCode;
                }
                else
                {
                    Debug.LogWarning("StudentIDPrivateCode bileşeni atanmamış! Öğrenci kodu: " + studentCode);
                }
            }
        }
        else
        {
            Debug.LogError($"Kayıt başarısız! Hata: {message}");
            PopUpError(2); // Burada da Hatayı gösteriyorum.
        }
    }

    public void GoToSelectPanel()
    {
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        SelectPanel.SetActive(true);
    }

    // Öğrenci listesini yükleme metodu
    private void LoadStudentList()
    {
        if (_isLoadingStudents)
        {
            Debug.LogWarning("Zaten bir öğrenci listesi yükleme işlemi devam ediyor, lütfen bekleyin.");
            return;
        }
        
        _isLoadingStudents = true;
        
        if (firebaseProxyService != null)
        {
            // Önce mevcut liste elemanlarını temizle
            ClearStudentList();
            
            firebaseProxyService.GetTeacherStudents((success, students) => {
                _isLoadingStudents = false;
                
                if (success && students != null)
                {
                    Debug.Log($"Öğrenci listesi başarıyla yüklendi! Öğrenci sayısı: {students.Count}");
                    DisplayStudentList(students);
                }
                else
                {
                    Debug.LogError("Öğrenci listesi yüklenemedi!");
                    // Varsayılan olarak bir demo öğrenci göster
                    var demoStudents = new List<StudentInfo>
                    {
                        new StudentInfo { 
                            studentId = "STU-DEMO", 
                            currentLevel = 1, 
                            currentWorld = 1, 
                            lastTimePlayed = "Henüz oynamadı", 
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
            Debug.LogError("FirebaseProxyService kullanılamıyor.");
        }
    }
    
    // Mevcut öğrenci listesini temizle
    private void ClearStudentList()
    {
        if (StudentListContent == null)
        {
            Debug.LogError("StudentListContent referansı atanmamış!");
            return;
        }
        
        // Öğrenci listesi, detay alanları ve eski ScrollRect'i temizle
        for (int i = StudentListContent.childCount - 1; i >= 0; i--)
        {
            Transform child = StudentListContent.GetChild(i);
            // Öğrenci ana bilgilerini, DetailedScrollRect'leri ve eski ScrollRect'i sil
            if (child.name.Contains("student-main-info") || 
                child.name.Contains("DetailedScrollRect-") || 
                child.name == "ScrollRect")
            {
                Destroy(child.gameObject);
                Debug.Log($"Silindi: {child.name}");
            }
            else
            {
                Debug.Log($"Korundu: {child.name}");
            }
        }
        
        studentList.Clear();
    }
    
    
    private void DisplayStudentList(List<StudentInfo> students)
    {
        if (StudentListContent == null)
        {
            Debug.LogError("StudentListContent referansı atanmamış!");
            return;
        }
        
        if (StudentListItemPrefab == null)
        {
            Debug.LogError("StudentListItemPrefab referansı atanmamış!");
            return;
        }
        
        studentList = students;
        
        // Her öğrenci için bir satır oluştur
        foreach (var student in students)
        {
            // Prefabı instantiate et
            GameObject listItem = Instantiate(StudentListItemPrefab, StudentListContent);
            
            // Öğrenci bilgilerini ayarla
            var studentId = listItem.transform.Find("Added-Student-ID")?.GetComponent<TMP_Text>();
            var currentLevel = listItem.transform.Find("Current-level")?.GetComponent<TMP_Text>();
            var currentWorld = listItem.transform.Find("Current-world")?.GetComponent<TMP_Text>();
            var lastTimePlayed = listItem.transform.Find("Last-time-played")?.GetComponent<TMP_Text>();
            var totalTime = listItem.transform.Find("total-time")?.GetComponent<TMP_Text>();
            
            if (studentId != null)
            {
                studentId.text = student.studentId;
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
                lastTimePlayed.text = student.lastTimePlayed;
            }
            
            if (totalTime != null)
            {
                totalTime.text = student.totalTime.ToString() + " dk";
            }
            
            // Buton işlevselliği - her öğrenci için kendi detay alanını oluştur/göster
            Button studentButton = listItem.GetComponent<Button>();
            if (studentButton == null)
            {
                studentButton = listItem.AddComponent<Button>();
                Debug.Log($"Button component eklendi: {student.studentId}");
            }
            
            // Butona tıklama eventi ekle - yeni dinamik ScrollRect mantığı
            studentButton.onClick.RemoveAllListeners();
            studentButton.onClick.AddListener(() => {
                Debug.Log($"Öğrenci butonuna tıklandı: {student.studentId}");
                ToggleStudentDetailedInfo(student, listItem.transform);
            });
        }
    }
    
    // Tüm öğrencilerin bilgilerini güncelleme metodu
    public void HandleUpdateAllStudentInfo()
    {
        Debug.Log("Öğrenci listesi yenileniyor...");
        LoadStudentList(); // Sadece veri yenile, Firebase'e yazma
    }

    // Öğrenci detaylı bilgilerini aç/kapat
    private void ToggleStudentDetailedInfo(StudentInfo student, Transform studentItemTransform)
    {
        // O öğrencinin DetailedScrollRectPrefab'ı var mı kontrol et
        string detailedScrollRectName = $"DetailedScrollRect-{student.studentId}";
        Transform existingDetailedScrollRect = StudentListContent.Find(detailedScrollRectName);
        
        if (existingDetailedScrollRect != null)
        {
            // Varsa aç/kapat
            bool isActive = existingDetailedScrollRect.gameObject.activeSelf;
            existingDetailedScrollRect.gameObject.SetActive(!isActive);
            Debug.Log($"DetailedScrollRect {(isActive ? "kapatıldı" : "açıldı")}: {student.studentId}");
        }
        else
        {
            // Yoksa oluştur ve öğrencinin hemen altına yerleştir
            CreateDetailedScrollRectForStudent(student, studentItemTransform);
        }
    }
    
    // Öğrenci için dinamik DetailedScrollRect oluştur
    private void CreateDetailedScrollRectForStudent(StudentInfo student, Transform studentItemTransform)
    {
        if (DetailedScrollRectPrefab == null)
        {
            Debug.LogError("DetailedScrollRectPrefab atanmamış! Inspector'da atayın.");
            return;
        }
        
        // DetailedScrollRectPrefab'ı oluştur
        GameObject detailedScrollRect = Instantiate(DetailedScrollRectPrefab, StudentListContent);
        detailedScrollRect.name = $"DetailedScrollRect-{student.studentId}";
        
        // Öğrencinin hemen altına yerleştir
        int studentIndex = studentItemTransform.GetSiblingIndex();
        detailedScrollRect.transform.SetSiblingIndex(studentIndex + 1);
        
        Debug.Log($"DetailedScrollRect oluşturuldu ve yerleştirildi: {student.studentId}, Index: {studentIndex + 1}");
        
        // İçine detailed-info'ları ekle - prefab yapısına göre Content'i bul
        Transform content = null;
        
        // Önce prefab'ın kendisinde ScrollRect component'i var mı kontrol et
        ScrollRect scrollRectComponent = detailedScrollRect.GetComponent<ScrollRect>();
        if (scrollRectComponent != null)
        {
            // Prefab'ın kendisi ScrollRect ise, direkt Content'i al
            content = detailedScrollRect.transform.Find("Content");
            Debug.Log("ScrollRect component prefab'ın root'unda bulundu.");
        }
        else
        {
            // Prefab'ın içinde "ScrollRect" isimli child var mı kontrol et
            Transform scrollRectTransform = detailedScrollRect.transform.Find("ScrollRect");
            if (scrollRectTransform != null)
            {
                content = scrollRectTransform.Find("Content");
                Debug.Log("ScrollRect child olarak bulundu.");
            }
        }
        
        if (content == null)
        {
            Debug.LogError("DetailedScrollRectPrefab içinde 'Content' bulunamadı! Prefab yapısını kontrol edin.");
            Debug.Log($"Prefab yapısı: {detailedScrollRect.name}");
            for (int i = 0; i < detailedScrollRect.transform.childCount; i++)
            {
                Debug.Log($"Child {i}: {detailedScrollRect.transform.GetChild(i).name}");
            }
            return;
        }
        
        // Detaylı bilgileri yükle
        DisplayDetailedStudentInfoInContent(student, content);
    }

    private void DisplayDetailedStudentInfoInContent(StudentInfo student, Transform content)
    {
        if (DetailedStudentInfoPrefab == null)
        {
            Debug.LogError("DetailedStudentInfoPrefab atanmamış! Inspector'da atayın.");
            return;
        }
        
        // Mevcut detaylı bilgileri temizle
        foreach (Transform child in content)
        {
            if (child.name.Contains("studen-detailed-info"))
            {
                Destroy(child.gameObject);
            }
        }
        
        // Yeni hiyerarşik veri yapısını kontrol et
        if (student.worldsData == null || student.worldsData.worlds == null || student.worldsData.worlds.Count == 0)
        {
            Debug.Log($"Öğrenci {student.studentId} için detaylı bilgi yok, varsayılan veri oluşturuluyor.");

            // Varsayılan veri oluştur ( burası işte neyi nasıl döndürdüğümüzle alakalı firebase'i de buna göre yapcam mapi falan )
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
                                fails = 0,
                                maxScore = 0,
                                isCorrect = false,
                                averageAccuracy = 0.0f,
                                starRating = 0,
                                accuracyBreakdown = new List<AccuracyBreakdownItem>()
                                // Diğer alanlar (varsayılan değerlerle oluşturuluyor)
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
                
                // Bilgileri atama yerimiz detailed içindeki yerleri atıyorum burada.
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

                // Zorunlu alanlar (her halükarda gösterdiğimiz yer burası)
                if (worldText != null) worldText.text = "W: " + world.worldNumber;
                if (levelText != null) levelText.text = "LV: " + level.levelNumber;
                
                // Dinamik alanlar (sadece mevcut olanları göster)
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
                    if (level.fails > 0) {
                        failsText.text = "" + level.fails.ToString();
                        failsText.gameObject.SetActive(true);
                    } else {
                        failsText.gameObject.SetActive(false);
                    }
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
                    if (level.averageAccuracy > 0.0f) {
                        averageAccuracyText.text = "%" + level.averageAccuracy.ToString("F1");
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
                    Debug.Log($"AccuracyBreakdown ayrı TextMeshPro'lara yazdırdık - Count: {level.accuracyBreakdown.Count}");
                }
                else
                {
                    Debug.Log($"AccuracyBreakdown verisi yok - W:{world.worldNumber} LV:{level.levelNumber}");
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
                //        Debug.Log($"AccuracyBreakdown gösterildi: {breakdownText}");
                //    }
                //    else
                //    {
                //        accuracyBreakdownText.gameObject.SetActive(false);
                //        Debug.Log($"AccuracyBreakdown verisi yok - W:{world.worldNumber} LV:{level.levelNumber}");
                //    }
                //}

                // Check/False için özel mantık (isCorrect alanı mevcut mu kontrol et)
                // Not: Unity'de bool alanları varsayılan değer kontrol etmek zor, bu yüzden her ikisini de gizleriz eğer gerekirse
                if (checkObj != null) checkObj.SetActive(level.isCorrect);
                if (falseObj != null) falseObj.SetActive(!level.isCorrect);


                // Dinamik star oluşturma
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

                        Debug.Log($"Star {i + 1}/{starCount} oluşturuldu (Stars container içinde) - Level: W{level.levelNumber}");
                    }

                    Debug.Log($"Toplam {starCount} star oluşturuldu (Stars container içinde) - Level: W{level.levelNumber}");
                }
                else
                {
                    Debug.LogWarning($"Stars container veya Star template bulunamadı - Level: W{level.levelNumber}");
                }

                Debug.Log($"Level detayı eklendi: W:{world.worldNumber} LV:{level.levelNumber} Attempts:{level.attempts}");
            }
        }
    }

    // Logout işlemi - hem teacher hem student için
    public void HandleLogout()
    {
        Debug.Log("Logout işlemi başlatılıyor...");
        
        if (firebaseProxyService == null)
        {
            Debug.LogError("FirebaseProxyService kullanılamıyor!");
            // Fallback: sadece UI'ı giriş ekranına çevir
            GoToSelectPanel();
            return;
        }

        // Backend'e logout isteği gönder
        firebaseProxyService.LogoutWithBackend((success, message) => {
            if (success)
            {
                Debug.Log("Logout başarılı: " + message);
                ShowLogoutSuccess();
            }
            else
            {
                Debug.LogWarning("Logout kısmen başarısız: " + message);
                // Kısmen başarısız olsa bile UI'ı giriş ekranına çevir
                ShowLogoutSuccess();
            }
        });
    }

    // Logout sonrası giriş ekranını göster
    private void ShowLogoutSuccess()
    {
        Debug.Log("Logout tamamlandı, giriş ekranına dönülüyor...");
        
        // Tüm panelleri kapat
        TeacherPanel.SetActive(false);
        StudentPanel.SetActive(false);
        if (TeacherLobbyPanel != null)
        {
            TeacherLobbyPanel.SetActive(false);
        }
        PopUpPanel.SetActive(false);
        
        // Form alanlarını temizle (güvenlik için)
        ClearAllLoginFields();
        
        // Giriş ekranını aç
        SelectPanel.SetActive(true);
        
        Debug.Log("Giriş ekranı gösteriliyor.");
    }

    // Tüm login form alanlarını temizle
    private void ClearAllLoginFields()
    {
        // Teacher Login alanlarını temizle
        if (TeacherLoginEmailInput != null)
        {
            TeacherLoginEmailInput.text = "";
            Debug.Log("Teacher login email alanı temizlendi.");
        }
        
        if (TeacherLoginPasswordInput != null)
        {
            TeacherLoginPasswordInput.text = "";
            Debug.Log("Teacher login password alanı temizlendi.");
        }
        
        // Student Login alanlarını temizle
        if (StudentSigninUsername != null)
        {
            StudentSigninUsername.text = "";
            Debug.Log("Student login username alanı temizlendi.");
        }
        
        if (StudentSigninPassword != null)
        {
            StudentSigninPassword.text = "";
            Debug.Log("Student login password alanı temizlendi.");
        }
        
        // SignUp alanlarını da temizle (bonus güvenlik)
        if (TeacherMailInput != null)
        {
            TeacherMailInput.text = "";
        }
        
        if (TeacherPassInput != null)
        {
            TeacherPassInput.text = "";
        }
        
        if (StudentMailInput != null)
        {
            StudentMailInput.text = "";
        }
        
        if (StudentPassInput != null)
        {
            StudentPassInput.text = "";
        }
        
        Debug.Log("Tüm form alanları güvenlik için temizlendi.");
    }

    void ToggleVisibility(PasswordTogglePair pair)
    {
        pair.isVisible = !pair.isVisible;
        pair.inputField.contentType = pair.isVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        pair.inputField.ForceLabelUpdate();
    }
}