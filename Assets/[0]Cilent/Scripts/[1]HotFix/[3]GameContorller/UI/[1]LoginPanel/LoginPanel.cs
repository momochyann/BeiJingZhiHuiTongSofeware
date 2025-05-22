using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using QFramework;
using Ricimi;
public class LoginPanel : MonoBehaviour, IController
{
    [Header("UI引用")]
    [SerializeField] private Button loginButton;
    [SerializeField] private Button forgetPasswordButton;
    [SerializeField] private InputField accountInputField;
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private Toggle rememberPasswordToggle;
    [SerializeField] private Text errorMessageText;

    private bool isLoggingIn = false;
    private const string ACCOUNT_PREFS_KEY = "SavedAccountName";
    private const string PASSWORD_PREFS_KEY = "SavedPassword";
    private const string REMEMBER_PREFS_KEY = "RememberPassword";

    void Start()
    {
        InitializeUI();
        LoadSavedCredentials();

        // 初始化时隐藏所有Gradient
        ToggleAllGradients(accountInputField.gameObject, false);
        ToggleAllGradients(passwordInputField.gameObject, false);
        passwordInputField.contentType = InputField.ContentType.Password;
        // 添加输入框焦点事件监听
        SetupInputFieldFocusEvents();

    }

    private void InitializeUI()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
        forgetPasswordButton.onClick.AddListener(OnForgetPasswordButtonClick);

        // 添加输入验证
        accountInputField.onValueChanged.AddListener(_ => ValidateInput());
        passwordInputField.onValueChanged.AddListener(_ => ValidateInput());

        // 初始状态
        if (errorMessageText != null)
            errorMessageText.gameObject.SetActive(false);
    }

    // 设置输入框焦点事件
    private void SetupInputFieldFocusEvents()
    {
        // 使用Update方法检查焦点状态

        StartCoroutine(CheckInputFieldFocus());
    }

    // 检查输入框焦点状态的协程
    private IEnumerator CheckInputFieldFocus()
    {
        while (true)
        {
            // Debug.Log(accountInputField.isFocused);
            // 检查账号输入框是否有焦点
            if (accountInputField.isFocused)
            {
                ToggleAllGradients(accountInputField.gameObject, true);
                ToggleAllGradients(passwordInputField.gameObject, false);
            }
            // 检查密码输入框是否有焦点
            else if (passwordInputField.isFocused)
            {
                ToggleAllGradients(passwordInputField.gameObject, true);
                ToggleAllGradients(accountInputField.gameObject, false);
            }
            // 如果两个输入框都没有焦点
            else
            {
                ToggleAllGradients(accountInputField.gameObject, false);
                ToggleAllGradients(passwordInputField.gameObject, false);
            }

            // 等待一小段时间再检查
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 切换物体及其子物体上的所有Gradient组件的显示状态
    private void ToggleAllGradients(GameObject targetObject, bool isEnabled)
    {
        // Debug.Log("ToggleAllGradients: " + targetObject.name + " " + isEnabled);
        // 获取目标物体上的所有Gradient组件
        Ricimi.Gradient[] gradients = targetObject.GetComponents<Ricimi.Gradient>();
        foreach (var gradient in gradients)
        {
            gradient.enabled = isEnabled;
        }

        // 获取子物体上的所有Gradient组件
        Ricimi.Gradient[] childGradients = targetObject.GetComponentsInChildren<Ricimi.Gradient>(true);
        foreach (var gradient in childGradients)
        {
            gradient.enabled = isEnabled;
        }
    }

    private void LoadSavedCredentials()
    {
        if (PlayerPrefs.HasKey(REMEMBER_PREFS_KEY) && PlayerPrefs.GetInt(REMEMBER_PREFS_KEY) == 1)
        {
            rememberPasswordToggle.isOn = true;
            accountInputField.text = PlayerPrefs.GetString(ACCOUNT_PREFS_KEY, "");
            passwordInputField.text = PlayerPrefs.GetString(PASSWORD_PREFS_KEY, "");
            passwordInputField.ActivateInputField();
              passwordInputField.ForceLabelUpdate();
        }
    }

    private void SaveCredentials()
    {
        if (rememberPasswordToggle.isOn)
        {
            PlayerPrefs.SetString(ACCOUNT_PREFS_KEY, accountInputField.text);
            PlayerPrefs.SetString(PASSWORD_PREFS_KEY, passwordInputField.text);
            PlayerPrefs.SetInt(REMEMBER_PREFS_KEY, 1);
        }
        else
        {
            PlayerPrefs.DeleteKey(PASSWORD_PREFS_KEY);
            PlayerPrefs.SetInt(REMEMBER_PREFS_KEY, 0);
        }
        PlayerPrefs.Save();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(accountInputField.text) &&
                      !string.IsNullOrEmpty(passwordInputField.text);

        loginButton.interactable = isValid;

        if (errorMessageText != null)
            errorMessageText.gameObject.SetActive(false);
    }

    private void OnLoginButtonClick()
    {
        if (isLoggingIn)
            return;

        isLoggingIn = true;
        loginButton.interactable = false;

        try
        {
            string account = accountInputField.text;
            string password = passwordInputField.text;

            // 验证账号密码
            bool loginSuccess = ValidateLogin(account, password);

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
            loginButton.interactable = true;
            StartCoroutine(LoginCooldown());
        }
    }

    private IEnumerator LoginCooldown()
    {
        yield return new WaitForSeconds(1f);
        isLoggingIn = false;
    }

    private bool ValidateLogin(string account, string password)
    {
        // 从IntervenersModel获取干预人员列表
        var model = this.GetModel<IntervenersModel>();

        // 如果模型中有数据，则遍历验证
        if (model != null && model.intervenerList != null && model.intervenerList.Count > 0)
        {
            foreach (var intervener in model.intervenerList)
            {
                if (intervener.用户名 == account && intervener.密码 == password)
                {
                    this.GetSystem<WorkSceneSystem>().干预者 = intervener.name;
                    return true;
                }
            }
        }
        else
        {
            // 如果没有数据，使用默认账号密码
            if (account == "admin" && password == "123456")
                return true;
        }

        return false;
    }

    private void ShowErrorMessage(string message)
    {
        if (errorMessageText != null)
        {
            errorMessageText.text = message;
            errorMessageText.gameObject.SetActive(true);
        }
    }

    private void OnLoginSuccess()
    {
        Debug.Log($"登录成功: {accountInputField.text}");

        // 保存当前登录用户信息
        PlayerPrefs.SetString("CurrentUser", accountInputField.text);
        PlayerPrefs.Save();
        LoadYooAssetsTool.LoadSceneAsync("MainChooseScene");
        // 这里添加登录成功后的逻辑，例如加载主场景
        // SceneManager.LoadScene("MainScene");
    }

    private void OnForgetPasswordButtonClick()
    {
        Debug.Log("打开忘记密码界面");
        // 可以在这里添加忘记密码的逻辑
    }

    private void OnDestroy()
    {
        // 清理事件监听
        if (loginButton != null)
            loginButton.onClick.RemoveAllListeners();

        if (forgetPasswordButton != null)
            forgetPasswordButton.onClick.RemoveAllListeners();

        if (accountInputField != null)
        {
            accountInputField.onValueChanged.RemoveAllListeners();
        }

        if (passwordInputField != null)
        {
            passwordInputField.onValueChanged.RemoveAllListeners();
        }
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
