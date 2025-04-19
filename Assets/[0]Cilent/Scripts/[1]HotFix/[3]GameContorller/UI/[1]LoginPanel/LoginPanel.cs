using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class LoginPanel : MonoBehaviour
{
    [Header("UI引用")]
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _forgetPasswordButton;
    [SerializeField] private InputField _accountInputField;
    [SerializeField] private InputField _passwordInputField;
    [SerializeField] private Toggle _rememberPasswordToggle;
    [SerializeField] private Text _errorMessageText;
    
    [Header("配置")]
    [SerializeField] private float _loginCooldownTime = 1f;
    
    private bool _isLoggingIn = false;
    private const string ACCOUNT_PREFS_KEY = "SavedAccountName";
    private const string PASSWORD_PREFS_KEY = "SavedPassword";
    private const string REMEMBER_PREFS_KEY = "RememberPassword";
    
    // 本地账户数据 - 实际项目中可能会使用加密存储或其他安全方式
    [System.Serializable]
    private class UserAccount
    {
        public string username;
        public string password;
    }
    
    // 示例用户数据 - 实际项目中应该从配置文件或数据库加载
    private UserAccount[] _localAccounts = new UserAccount[]
    {
        new UserAccount { username = "admin", password = "admin123" },
        new UserAccount { username = "user", password = "user123" }
    };
    
    void Start()
    {
        InitializeUI();
        LoadSavedCredentials();
    }
    
    private void InitializeUI()
    {
        _loginButton.onClick.AddListener(OnLoginButtonClick);
        _forgetPasswordButton.onClick.AddListener(OnForgetPasswordButtonClick);
        
        // 添加输入验证
        _accountInputField.onValueChanged.AddListener(OnAccountInputChanged);
        _passwordInputField.onValueChanged.AddListener(OnPasswordInputChanged);
        
        // 初始状态
        if (_errorMessageText != null)
            _errorMessageText.gameObject.SetActive(false);
    }
    
    private void LoadSavedCredentials()
    {
        if (PlayerPrefs.HasKey(REMEMBER_PREFS_KEY) && PlayerPrefs.GetInt(REMEMBER_PREFS_KEY) == 1)
        {
            _rememberPasswordToggle.isOn = true;
            _accountInputField.text = PlayerPrefs.GetString(ACCOUNT_PREFS_KEY, "");
            _passwordInputField.text = PlayerPrefs.GetString(PASSWORD_PREFS_KEY, "");
        }
        else
        {
            _rememberPasswordToggle.isOn = false;
            _accountInputField.text = "";
            _passwordInputField.text = "";
        }
    }
    
    private void SaveCredentials()
    {
        if (_rememberPasswordToggle.isOn)
        {
            PlayerPrefs.SetString(ACCOUNT_PREFS_KEY, _accountInputField.text);
            PlayerPrefs.SetString(PASSWORD_PREFS_KEY, _passwordInputField.text);
            PlayerPrefs.SetInt(REMEMBER_PREFS_KEY, 1);
        }
        else
        {
            PlayerPrefs.DeleteKey(PASSWORD_PREFS_KEY);
            PlayerPrefs.SetInt(REMEMBER_PREFS_KEY, 0);
        }
        PlayerPrefs.Save();
    }
    
    private void OnAccountInputChanged(string value)
    {
        ValidateInput();
    }
    
    private void OnPasswordInputChanged(string value)
    {
        ValidateInput();
    }
    
    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_accountInputField.text) && 
                      !string.IsNullOrEmpty(_passwordInputField.text);
        
        _loginButton.interactable = isValid;
        
        if (_errorMessageText != null)
            _errorMessageText.gameObject.SetActive(false);
    }
    
    private void OnLoginButtonClick()
    {
        if (_isLoggingIn)
            return;
        
        _isLoggingIn = true;
        _loginButton.interactable = false;
        
        try
        {
            string account = _accountInputField.text;
            string password = _passwordInputField.text;
            
            // 本地验证账号密码
            bool loginSuccess = ValidateLocalLogin(account, password);
            
            if (loginSuccess)
            {
                SaveCredentials();
                OnLoginSuccess();
            }
            else
            {
                ShowErrorMessage("登录失败，账号或密码错误");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"登录过程中发生错误: {ex.Message}");
            ShowErrorMessage("登录过程中发生错误");
        }
        finally
        {
            _loginButton.interactable = true;
            StartCoroutine(LoginCooldown());
        }
    }
    
    private IEnumerator LoginCooldown()
    {
        yield return new WaitForSeconds(_loginCooldownTime);
        _isLoggingIn = false;
    }
    
    private bool ValidateLocalLogin(string account, string password)
    {
        // 方法1: 使用预设的账号列表验证
        foreach (var user in _localAccounts)
        {
            if (user.username == account && user.password == password)
                return true;
        }
        
        // 方法2: 使用PlayerPrefs存储的注册账号验证
        // 如果你的应用允许用户注册，可以使用这种方式
        string savedPassword = PlayerPrefs.GetString($"User_{account}_Password", null);
        if (!string.IsNullOrEmpty(savedPassword) && savedPassword == password)
            return true;
            
        // 方法3: 开发阶段可以添加万能密码
        #if UNITY_EDITOR
        if (password == "dev123456")
            return true;
        #endif
        
        return false;
    }
    
    private void ShowErrorMessage(string message)
    {
        if (_errorMessageText != null)
        {
            _errorMessageText.text = message;
            _errorMessageText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning(message);
        }
    }
    
    private void OnLoginSuccess()
    {
        Debug.Log($"登录成功: {_accountInputField.text}");
        
        // 保存当前登录用户信息
        PlayerPrefs.SetString("CurrentUser", _accountInputField.text);
        PlayerPrefs.Save();
        
        // 这里应该跳转到游戏主界面或下一个场景
        // SceneManager.LoadScene("MainMenu");
        // 或者
        // UIManager.Instance.ShowPanel<MainMenuPanel>();
        // GameManager.Instance.OnUserLoggedIn(_accountInputField.text);
    }
    
    private void OnForgetPasswordButtonClick()
    {
        Debug.Log("打开忘记密码界面");
        // 本地登录的忘记密码可以实现为重置密码功能
        // UIManager.Instance.ShowPanel<ResetPasswordPanel>();
    }
    
    // 可选：添加注册新用户功能
    public void OnRegisterButtonClick()
    {
        string newUsername = _accountInputField.text;
        string newPassword = _passwordInputField.text;
        
        // 检查用户名是否已存在
        if (PlayerPrefs.HasKey($"User_{newUsername}_Password"))
        {
            ShowErrorMessage("该用户名已被注册");
            return;
        }
        
        // 保存新用户
        PlayerPrefs.SetString($"User_{newUsername}_Password", newPassword);
        PlayerPrefs.Save();
        
        ShowErrorMessage("注册成功，请登录");
    }
    
    private void OnDestroy()
    {
        // 清理事件监听，防止内存泄漏
        if (_loginButton != null)
            _loginButton.onClick.RemoveAllListeners();
            
        if (_forgetPasswordButton != null)
            _forgetPasswordButton.onClick.RemoveAllListeners();
            
        if (_accountInputField != null)
            _accountInputField.onValueChanged.RemoveAllListeners();
            
        if (_passwordInputField != null)
            _passwordInputField.onValueChanged.RemoveAllListeners();
    }
}
