using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

namespace CustomKeyboard
{
    /// <summary>
    /// TMP_InputField扩展组件
    /// 自动集成自定义键盘功能，适用于Android 13/14设备
    /// </summary>
    [RequireComponent(typeof(TMP_InputField))]
    public class CustomInputFieldExtension : MonoBehaviour, IPointerClickHandler, ISelectHandler
    {
        [Header("键盘设置")]
        [SerializeField] private bool useCustomKeyboard = true;          // 是否使用自定义键盘
        [SerializeField] private bool hideSystemKeyboard = true;        // 是否隐藏系统键盘
        [SerializeField] private bool autoShowOnFocus = true;           // 获得焦点时自动显示键盘
        [SerializeField] private bool autoHideOnSubmit = true;          // 提交时自动隐藏键盘
        
        [Header("动画设置")]
        [SerializeField] private float keyboardShowDelay = 0.1f;        // 键盘显示延迟
        
        private TMP_InputField inputField;
        private bool isInitialized = false;
        
        private void Awake()
        {
            InitializeInputField();
        }
        
        private void Start()
        {
            if (!isInitialized)
            {
                InitializeInputField();
            }
        }
        
        private void InitializeInputField()
        {
            inputField = GetComponent<TMP_InputField>();
            if (inputField == null)
            {
                Debug.LogError("CustomInputFieldExtension: 找不到TMP_InputField组件！");
                return;
            }
            
            // 设置输入框属性
            if (hideSystemKeyboard)
            {
                inputField.shouldHideMobileInput = true;
            }
            
            // 添加事件监听
            SetupEventListeners();
            
            isInitialized = true;
        }
        
        private void SetupEventListeners()
        {
            if (inputField == null) return;
            
            // 监听选择事件
            inputField.onSelect.AddListener(OnInputFieldSelected);
            
            // 监听取消选择事件
            inputField.onDeselect.AddListener(OnInputFieldDeselected);
            
            // 监听提交事件
            if (autoHideOnSubmit)
            {
                inputField.onSubmit.AddListener(OnInputFieldSubmitted);
            }
            
            // 监听值改变事件（可选）
            inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (useCustomKeyboard && autoShowOnFocus)
            {
                ShowCustomKeyboard();
            }
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            if (useCustomKeyboard && autoShowOnFocus)
            {
                ShowCustomKeyboard();
            }
        }
        
        private void OnInputFieldSelected(string value)
        {
            if (useCustomKeyboard && autoShowOnFocus)
            {
                Invoke(nameof(ShowCustomKeyboardDelayed), keyboardShowDelay);
            }
        }
        
        private void OnInputFieldDeselected(string value)
        {
            // 输入框失去焦点时的处理
            // 注意：这里不直接隐藏键盘，因为用户可能是点击了键盘按钮
        }
        
        private void OnInputFieldSubmitted(string value)
        {
            if (useCustomKeyboard)
            {
                HideCustomKeyboard();
            }
        }
        
        private void OnInputFieldValueChanged(string value)
        {
            // 可以在这里添加值改变时的处理逻辑
            // 例如：验证输入、格式化文本等
        }
        
        private void ShowCustomKeyboardDelayed()
        {
            ShowCustomKeyboard();
        }
        
        /// <summary>
        /// 显示自定义键盘
        /// </summary>
        public void ShowCustomKeyboard()
        {
            if (!useCustomKeyboard || inputField == null) return;
            
            // 确保CustomKeyboardManager存在
            if (CustomKeyboardManager.Instance != null)
            {
                CustomKeyboardManager.Instance.ShowKeyboard(inputField);
            }
            else
            {
                Debug.LogWarning("CustomInputFieldExtension: CustomKeyboardManager未找到！");
            }
        }
        
        /// <summary>
        /// 隐藏自定义键盘
        /// </summary>
        public void HideCustomKeyboard()
        {
            if (CustomKeyboardManager.Instance != null)
            {
                CustomKeyboardManager.Instance.HideKeyboard();
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
                    HideCustomKeyboard();
                }
                else
                {
                    ShowCustomKeyboard();
                }
            }
        }
        
        /// <summary>
        /// 设置是否使用自定义键盘
        /// </summary>
        /// <param name="useCustom">是否使用自定义键盘</param>
        public void SetUseCustomKeyboard(bool useCustom)
        {
            useCustomKeyboard = useCustom;
            
            if (inputField != null)
            {
                inputField.shouldHideMobileInput = useCustom && hideSystemKeyboard;
            }
            
            if (!useCustom)
            {
                HideCustomKeyboard();
            }
        }
        
        /// <summary>
        /// 获取输入框组件
        /// </summary>
        /// <returns>TMP_InputField组件</returns>
        public TMP_InputField GetInputField()
        {
            return inputField;
        }
        
        private void OnDestroy()
        {
            // 清理事件监听
            if (inputField != null)
            {
                inputField.onSelect.RemoveListener(OnInputFieldSelected);
                inputField.onDeselect.RemoveListener(OnInputFieldDeselected);
                inputField.onSubmit.RemoveListener(OnInputFieldSubmitted);
                inputField.onValueChanged.RemoveListener(OnInputFieldValueChanged);
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            // 应用获得/失去焦点时的处理
            if (!hasFocus && useCustomKeyboard)
            {
                HideCustomKeyboard();
            }
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            // 应用暂停/恢复时的处理
            if (pauseStatus && useCustomKeyboard)
            {
                HideCustomKeyboard();
            }
        }
    }
} 