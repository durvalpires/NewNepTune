using System.Text.RegularExpressions;
using _App_v2.Scripts._Core.Firebase.Analytics.Data.Events;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
using _App_v2.Scripts._Core.Firebase.Auth;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using EasyUI.Popup;
#if !UNITY_WEBGL
using Firebase.Auth;
#else
using MarksAssets.FirebaseWebGL.Auth;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace _App_v2.Scripts._Core.Firebase.UI
{
    public class FirebaseAuthScreen : MonoBehaviour
    {
        private const string NEXT_SCENE = "v2_MainMenu";
        //private const string NEXT_SCENE = "v2_ProfileScreen";

        [Header("Login Inputs")]
        [SerializeField] private TMP_InputField loginEmailField;
        [SerializeField] private TMP_InputField loginPassField;
        [SerializeField] private Button loginButton;
        
        public Button toggleButton; 
        public Sprite showPasswordIcon;
        public Sprite hidePasswordIcon;

        [Header("Register Inputs")]
        [SerializeField] private TMP_InputField registerUsernameField;
        [SerializeField] private TMP_InputField registerEmailField;
        [SerializeField] private TMP_InputField registerPassField;
        [SerializeField] private Button registerButton;
        
        [Header("Apple")]
        [SerializeField] private Button appleButton;
        
        [Header("Other")]
        [SerializeField] private Button logoutButton;
        [SerializeField] private Button playButton;
        [SerializeField] private Button mainLoginButton;
        [SerializeField] private Button mainRegisterButton;
        [SerializeField] private Button exceptionButton;
        
        private bool _isLocked;
        private IAuthService _authService;
        private IAnalyticsService _analytics;
        #if !UNITY_WEBGL
        private FirebaseAuth firebaseAuth;
        #endif

         private bool isPasswordVisible = false;
        // public UnityEvent<string> OnAuthFailedEvent;
        // public UnityEvent OnPasswordNotValid;
        // public UnityEvent OnEmailNotValid;

        #if !UNITY_WEBGL
        [SerializeField] private FBAuthErrorMessagesSO FBErrorsSO;
        #endif
        

        private void Awake()
        {
            _authService = FireService.Instance.Auth;
            _analytics = FireService.Instance.AnalyticsService;
            
            _authService.OnAuthSuccess += OnAuthSuccess;
            _authService.OnAuthFailed += OnAuthFailed;


            toggleButton.onClick.AddListener(TogglePasswordVisibility);
            loginButton.onClick.AddListener(Login);
            registerButton.onClick.AddListener(Register);
            //appleButton.onClick.AddListener(AppleLogin);
            logoutButton.onClick.AddListener(Logout);
            playButton.onClick.AddListener(Play);
            exceptionButton.onClick.AddListener(ThrowException);
            
            UpdateButtons();
        }
        
        private void OnDestroy()
        {
            _authService.OnAuthSuccess -= OnAuthSuccess;
            #if !UNITY_WEBGL
            _authService.OnAuthFailed -= OnAuthFailed;
            #endif

            
            loginButton.onClick.RemoveListener(Login);
            registerButton.onClick.RemoveListener(Register);
            //appleButton.onClick.RemoveListener(AppleLogin);
            logoutButton.onClick.RemoveListener(Logout);
            playButton.onClick.RemoveListener(Play);
            exceptionButton.onClick.RemoveListener(ThrowException);
        }

        private void AppleLogin()
        {
            if (!CheckIfCanProceed())
                return;
            
            _isLocked = true;
            _authService.LogIn(AuthMethod.Apple);
        }

        private void Register()
        {
            if (!CheckIfCanProceed())
                return;
            
            var email = registerEmailField.text;
            var password = registerPassField.text;
            var username = registerUsernameField.text;
            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
            {
                Popup.Show("Error,", "Email, password or username is empty", "OK", PopupColor.Red);
                Debug.LogError("Email, password or username is empty");
                return;
            }

            if (!IsValidPassword(password))
            {
                Popup.Show("Error", "Invalid password", "OK", PopupColor.Red);
                Debug.LogError("Invalid password");
                registerEmailField.gameObject.GetComponent<Image>().DOColor(new Color(255, 170, 170),
                    0.5f);                
                registerEmailField.gameObject.GetComponent<Image>().color = Color.black;
                return;
            }
            
            if (!IsValidEmail(email))
            {
                Popup.Show("Error", "Invalid email format", "OK", PopupColor.Red);
                Debug.LogError("Invalid email format");
                registerEmailField.gameObject.GetComponent<Image>().DOColor(new Color(255, 170, 170),
                    0.5f);
                return;
            }
            
            _isLocked = true;
            _authService.SignIn(AuthMethod.EmailAndPassword, username, email, password);
        }
        void TogglePasswordVisibility()
        {
            isPasswordVisible = !isPasswordVisible;

            if (isPasswordVisible)
            {
                loginPassField.contentType = TMP_InputField.ContentType.Standard; 
                    toggleButton.image.sprite = hidePasswordIcon; 
            }
            else
            {
                loginPassField.contentType = TMP_InputField.ContentType.Password;
                    toggleButton.image.sprite = showPasswordIcon;
            }

            loginPassField.ForceLabelUpdate(); 
        }
        private void Login()
        {
            if (!CheckIfCanProceed())
                return;
                
            var email = loginEmailField.text;
            var password = loginPassField.text;
            
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Popup.Show("Error,", "Email or password is empty", "OK", PopupColor.Red);
                Debug.LogError("Email or password is empty");
                return;
            }
            
            _isLocked = true;
            _authService.LogIn(AuthMethod.EmailAndPassword, email, password);
        }
        
        private bool CheckIfCanProceed()
        {
            if (_isLocked)
            {
                Popup.Show("Error", "Another operation is in progress", "OK", PopupColor.Red);
                Debug.LogWarning("Another operation is in progress");
                return false;
            }

            Debug.LogWarning("Before Check if can proceed signin state: " + _authService == null);
            Debug.LogWarning("Before Check if can proceed signin state: " + _authService.SignedIn == null);
            if (_authService.SignedIn)
            {
                Popup.Show("Error", "User is already signed in", "OK", PopupColor.Red);
                Debug.Log("Already signed in");
                return false;
            }

            return true;
        }
        
        #if !UNITY_WEBGL
        private void OnAuthFailed(AuthError error)
        {
            _isLocked = false;
            #if !UNITY_WEBGL
            Debug.LogError($"Auth failed. Error: {FBErrorsSO.GetDescription(error)}");
            Popup.Show("Error", FBErrorsSO.GetUserMessage(error), "OK", PopupColor.Red);
            #endif

            //OnAuthFailedEvent?.Invoke(FBErrorsSO.GetUserMessage(error));
        }
        #else
        private void OnAuthFailed(AuthError error){
            _isLocked = false;
            Debug.LogError($"Auth failed. Error: {error.code}");
            Popup.Show("Error", error.code.ToString(), "OK", PopupColor.Red);
        }
        #endif


        public void OnAuthSuccess()
        {
            _isLocked = false;
            Debug.Log("Auth success");
            
            Play();
            if (_analytics != null)
                _analytics.SendEvent(new LogInEvent(_authService.ProviderId));
        }

        private void UpdateButtons()
        {
            var isUserSignedIn = _authService.SignedIn;
            mainLoginButton.gameObject.SetActive(!isUserSignedIn);
            mainRegisterButton.gameObject.SetActive(!isUserSignedIn);
            appleButton.gameObject.SetActive(!isUserSignedIn);
            logoutButton.gameObject.SetActive(isUserSignedIn);
        }
        
        private async void Play()
        {
            await LoadSaves();
            await UniTask.SwitchToMainThread();
            
            SceneManager.LoadScene(NEXT_SCENE);
        }

        private void Logout()
        {
            if (!_authService.SignedIn)
            {
                Debug.LogWarning("User is not signed in");
                Popup.Show("Error", "User is not signed in", "OK", PopupColor.Red);
                return;
            }
            
            if (_isLocked)
            {
                Debug.LogWarning("Another operation is in progress");
                Popup.Show("Error", "Another operation is in progress", "OK", PopupColor.Red);
                return;
            }
            
            _isLocked = true;

            _authService.OnAuthStateChanged += UpdateUserState;
            _authService.LogOut();
        }

        private void UpdateUserState()
        {
            Debug.Log($"User state updated. Is signed in: {_authService.SignedIn}");
            _authService.OnAuthStateChanged -= UpdateUserState;
            _isLocked = false;
            UpdateButtons();

            if (!_authService.SignedIn && _analytics != null)
                _analytics.SendEvent(new LogOutEvent());
        }

        private UniTask LoadSaves() => PlayerModelBase.LoadData();

        private void ThrowException()
        {
            throw new System.Exception("Test exception");
        }
        
        /// <summary>
        /// Validates password strength.
        /// </summary>
        private bool IsValidPassword(string password)
        {
            return password.Length >= 6;
        }
        
        /// <summary>
        /// Checks if an email is formatted correctly.
        /// </summary>
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
  
    }
}

