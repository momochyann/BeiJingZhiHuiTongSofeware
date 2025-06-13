using UnityEngine;
using TMPro;

namespace CustomKeyboard
{
    /// <summary>
    /// 自定义键盘使用示例
    /// 展示如何在现有的UI中集成自定义键盘功能
    /// </summary>
    public class CustomKeyboardExample : MonoBehaviour
    {
        [Header("示例输入框")]
        public TMP_InputField[] inputFields;  // 需要使用自定义键盘的输入框数组
        
        [Header("控制按钮")]
        public UnityEngine.UI.Button toggleKeyboardButton;  // 切换键盘按钮
        public UnityEngine.UI.Button hideKeyboardButton;    // 隐藏键盘按钮
        
        private void Start()
        {
            SetupInputFields();
            SetupControlButtons();
            
            // 显示当前平台信息
            DisplayPlatformInfo();
        }
        
        /// <summary>
        /// 设置输入框
        /// </summary>
        private void SetupInputFields()
        {
            if (inputFields == null || inputFields.Length == 0)
            {
                // 如果没有手动指定，自动查找所有TMP_InputField
                inputFields = FindObjectsOfType<TMP_InputField>();
            }
            
            foreach (var inputField in inputFields)
            {
                if (inputField != null)
                {
                    // 为每个输入框添加CustomInputFieldExtension组件（如果没有的话）
                    if (inputField.GetComponent<CustomInputFieldExtension>() == null)
                    {
                        inputField.gameObject.AddComponent<CustomInputFieldExtension>();
                    }
                }
            }
            
            Debug.Log($"CustomKeyboardExample: 已为 {inputFields.Length} 个输入框启用自定义键盘");
        }
        
        /// <summary>
        /// 设置控制按钮
        /// </summary>
        private void SetupControlButtons()
        {
            if (toggleKeyboardButton != null)
            {
                toggleKeyboardButton.onClick.AddListener(ToggleKeyboard);
            }
            
            if (hideKeyboardButton != null)
            {
                hideKeyboardButton.onClick.AddListener(HideKeyboard);
            }
        }
        
        /// <summary>
        /// 切换键盘显示状态
        /// </summary>
        public void ToggleKeyboard()
        {
            if (CustomKeyboardManager.Instance != null)
            {
                if (CustomKeyboardManager.Instance.IsKeyboardVisible())
                {
                    CustomKeyboardManager.Instance.HideKeyboard();
                    Debug.Log("CustomKeyboardExample: 键盘已隐藏");
                }
                else
                {
                    // 显示键盘需要一个输入框作为目标
                    TMP_InputField firstInputField = GetFirstActiveInputField();
                    if (firstInputField != null)
                    {
                        CustomKeyboardManager.Instance.ShowKeyboard(firstInputField);
                        Debug.Log("CustomKeyboardExample: 键盘已显示");
                    }
                    else
                    {
                        Debug.LogWarning("CustomKeyboardExample: 没有找到可用的输入框");
                    }
                }
            }
        }
        
        /// <summary>
        /// 隐藏键盘
        /// </summary>
        public void HideKeyboard()
        {
            if (CustomKeyboardManager.Instance != null)
            {
                CustomKeyboardManager.Instance.HideKeyboard();
                Debug.Log("CustomKeyboardExample: 键盘已隐藏");
            }
        }
        
        /// <summary>
        /// 获取第一个激活的输入框
        /// </summary>
        /// <returns>第一个激活的输入框</returns>
        private TMP_InputField GetFirstActiveInputField()
        {
            foreach (var inputField in inputFields)
            {
                if (inputField != null && inputField.gameObject.activeInHierarchy)
                {
                    return inputField;
                }
            }
            return null;
        }
        
        /// <summary>
        /// 为指定的输入框启用自定义键盘
        /// </summary>
        /// <param name="inputField">目标输入框</param>
        public void EnableCustomKeyboardForInputField(TMP_InputField inputField)
        {
            if (inputField == null) return;
            
            CustomInputFieldExtension extension = inputField.GetComponent<CustomInputFieldExtension>();
            if (extension == null)
            {
                extension = inputField.gameObject.AddComponent<CustomInputFieldExtension>();
            }
            
            extension.SetUseCustomKeyboard(true);
            Debug.Log($"CustomKeyboardExample: 已为输入框 '{inputField.name}' 启用自定义键盘");
        }
        
        /// <summary>
        /// 为指定的输入框禁用自定义键盘
        /// </summary>
        /// <param name="inputField">目标输入框</param>
        public void DisableCustomKeyboardForInputField(TMP_InputField inputField)
        {
            if (inputField == null) return;
            
            CustomInputFieldExtension extension = inputField.GetComponent<CustomInputFieldExtension>();
            if (extension != null)
            {
                extension.SetUseCustomKeyboard(false);
                Debug.Log($"CustomKeyboardExample: 已为输入框 '{inputField.name}' 禁用自定义键盘");
            }
        }
        
        /// <summary>
        /// 批量设置所有输入框的自定义键盘状态
        /// </summary>
        /// <param name="useCustomKeyboard">是否使用自定义键盘</param>
        public void SetAllInputFieldsCustomKeyboard(bool useCustomKeyboard)
        {
            int count = 0;
            foreach (var inputField in inputFields)
            {
                if (inputField != null)
                {
                    if (useCustomKeyboard)
                    {
                        EnableCustomKeyboardForInputField(inputField);
                    }
                    else
                    {
                        DisableCustomKeyboardForInputField(inputField);
                    }
                    count++;
                }
            }
            
            Debug.Log($"CustomKeyboardExample: 已为 {count} 个输入框设置自定义键盘状态: {useCustomKeyboard}");
        }
        
        /// <summary>
        /// 显示平台信息
        /// </summary>
        private void DisplayPlatformInfo()
        {
            Debug.Log("=== 自定义键盘平台信息 ===");
            Debug.Log($"Unity平台: {Application.platform}");
            Debug.Log($"Android版本信息: {AndroidKeyboardHelper.GetAndroidVersionInfo()}");
            Debug.Log($"是否为Android 13+: {AndroidKeyboardHelper.IsAndroid13Plus()}");
            Debug.Log("========================");
        }
        
        /// <summary>
        /// 在Inspector中调用的测试方法
        /// </summary>
        [ContextMenu("测试显示键盘")]
        public void TestShowKeyboard()
        {
            TMP_InputField firstInputField = GetFirstActiveInputField();
            if (firstInputField != null && CustomKeyboardManager.Instance != null)
            {
                CustomKeyboardManager.Instance.ShowKeyboard(firstInputField);
            }
        }
        
        [ContextMenu("测试隐藏键盘")]
        public void TestHideKeyboard()
        {
            if (CustomKeyboardManager.Instance != null)
            {
                CustomKeyboardManager.Instance.HideKeyboard();
            }
        }
        
        [ContextMenu("启用所有输入框自定义键盘")]
        public void TestEnableAllCustomKeyboards()
        {
            SetAllInputFieldsCustomKeyboard(true);
        }
        
        [ContextMenu("禁用所有输入框自定义键盘")]
        public void TestDisableAllCustomKeyboards()
        {
            SetAllInputFieldsCustomKeyboard(false);
        }
    }
} 