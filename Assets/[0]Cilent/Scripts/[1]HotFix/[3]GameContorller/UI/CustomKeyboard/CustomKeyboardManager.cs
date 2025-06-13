using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CustomKeyboard
{
    /// <summary>
    /// 自定义软键盘管理器 - 用于Android 13/14 设备
    /// 替代系统键盘，提供自定义键盘输入功能
    /// </summary>
    public class CustomKeyboardManager : MonoBehaviour
    {
        [Header("键盘设置")]
        public GameObject keyboardPrefab; // 键盘预制体
        public Canvas keyboardCanvas;     // 键盘画布
        public int sortingOrder = 1000;   // 渲染层级
        
        [Header("输入设置")]
        public float keyPressAnimationTime = 0.1f; // 按键动画时间
        public AudioClip keyPressSound;             // 按键音效
        
        private static CustomKeyboardManager _instance;
        public static CustomKeyboardManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CustomKeyboardManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CustomKeyboardManager");
                        _instance = go.AddComponent<CustomKeyboardManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }
        
        private GameObject currentKeyboard;
        private TMP_InputField currentInputField;
        private AudioSource audioSource;
        private Dictionary<string, Action> specialKeys;
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeKeyboard();
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        private void InitializeKeyboard()
        {
            // 创建音频源
            audioSource = gameObject.AddComponent<AudioSource>();
            
            // 初始化特殊按键
            InitializeSpecialKeys();
            
            // Android平台特殊初始化
            InitializeAndroidSupport();
            
            // 如果没有设置画布，创建一个
            if (keyboardCanvas == null)
            {
                CreateKeyboardCanvas();
            }
        }
        
        /// <summary>
        /// 初始化Android平台支持
        /// </summary>
        private void InitializeAndroidSupport()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidKeyboardHelper.InitializeForCustomKeyboard();
#endif
        }
        
        private void CreateKeyboardCanvas()
        {
            GameObject canvasObject = new GameObject("KeyboardCanvas");
            keyboardCanvas = canvasObject.AddComponent<Canvas>();
            keyboardCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            keyboardCanvas.sortingOrder = sortingOrder;
            
            // 添加GraphicRaycaster用于UI交互
            canvasObject.AddComponent<GraphicRaycaster>();
            
            // 添加CanvasScaler用于自适应
            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            
            DontDestroyOnLoad(canvasObject);
        }
        
        private void InitializeSpecialKeys()
        {
            specialKeys = new Dictionary<string, Action>
            {
                { "Backspace", HandleBackspace },
                { "Space", HandleSpace },
                { "Enter", HandleEnter },
                { "Shift", HandleShift },
                { "Clear", HandleClear },
                { "Hide", HideKeyboard }
            };
        }
        
        /// <summary>
        /// 显示键盘
        /// </summary>
        /// <param name="inputField">目标输入框</param>
        public void ShowKeyboard(TMP_InputField inputField)
        {
            if (inputField == null) return;
            
            currentInputField = inputField;
            
            // 禁用输入框的移动端输入
            inputField.shouldHideMobileInput = true;
            
            // 如果键盘已经存在，直接显示
            if (currentKeyboard != null)
            {
                currentKeyboard.SetActive(true);
                return;
            }
            
            // 创建键盘实例
            CreateKeyboardInstance();
        }
        
        /// <summary>
        /// 隐藏键盘
        /// </summary>
        public void HideKeyboard()
        {
            if (currentKeyboard != null)
            {
                currentKeyboard.SetActive(false);
            }
            currentInputField = null;
        }
        
        /// <summary>
        /// 检查键盘是否可见
        /// </summary>
        /// <returns>键盘是否可见</returns>
        public bool IsKeyboardVisible()
        {
            return currentKeyboard != null && currentKeyboard.activeInHierarchy;
        }
        
        private void CreateKeyboardInstance()
        {
            if (keyboardPrefab == null)
            {
                CreateDefaultKeyboard();
            }
            else
            {
                currentKeyboard = Instantiate(keyboardPrefab, keyboardCanvas.transform);
                SetupKeyboardButtons();
            }
        }
        
        private void CreateDefaultKeyboard()
        {
            // 创建默认键盘UI
            GameObject keyboard = new GameObject("DefaultKeyboard");
            keyboard.transform.SetParent(keyboardCanvas.transform, false);
            
            RectTransform keyboardRect = keyboard.AddComponent<RectTransform>();
            keyboardRect.anchorMin = new Vector2(0, 0);
            keyboardRect.anchorMax = new Vector2(1, 0.4f);
            keyboardRect.offsetMin = Vector2.zero;
            keyboardRect.offsetMax = Vector2.zero;
            
            // 添加背景
            Image background = keyboard.AddComponent<Image>();
            background.color = new Color(0.2f, 0.2f, 0.2f, 0.9f);
            
            // 创建键盘布局
            CreateKeyboardLayout(keyboard);
            
            currentKeyboard = keyboard;
        }
        
        private void CreateKeyboardLayout(GameObject parent)
        {
            // 第一行：数字键
            string[] numberRow = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            CreateKeyRow(parent, numberRow, 0);
            
            // 第二行：字母键 (QWERTY)
            string[] topRow = { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
            CreateKeyRow(parent, topRow, 1);
            
            // 第三行：字母键
            string[] middleRow = { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
            CreateKeyRow(parent, middleRow, 2);
            
            // 第四行：字母键和特殊键
            string[] bottomRow = { "Shift", "Z", "X", "C", "V", "B", "N", "M", "Backspace" };
            CreateKeyRow(parent, bottomRow, 3);
            
            // 第五行：功能键
            string[] functionRow = { "Hide", "Space", "Clear", "Enter" };
            CreateKeyRow(parent, functionRow, 4);
        }
        
        private void CreateKeyRow(GameObject parent, string[] keys, int rowIndex)
        {
            GameObject row = new GameObject($"Row{rowIndex}");
            row.transform.SetParent(parent.transform, false);
            
            RectTransform rowRect = row.AddComponent<RectTransform>();
            rowRect.anchorMin = new Vector2(0, 1 - (rowIndex + 1) * 0.2f);
            rowRect.anchorMax = new Vector2(1, 1 - rowIndex * 0.2f);
            rowRect.offsetMin = Vector2.zero;
            rowRect.offsetMax = Vector2.zero;
            
            HorizontalLayoutGroup layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = 5;
            layout.padding = new RectOffset(10, 10, 5, 5);
            
            foreach (string keyText in keys)
            {
                CreateKey(row, keyText);
            }
        }
        
        private void CreateKey(GameObject parent, string keyText)
        {
            GameObject keyObject = new GameObject($"Key_{keyText}");
            keyObject.transform.SetParent(parent.transform, false);
            
            // 设置按钮组件
            Button button = keyObject.AddComponent<Button>();
            Image buttonImage = keyObject.AddComponent<Image>();
            buttonImage.color = Color.white;
            
            // 设置按钮大小
            LayoutElement layoutElement = keyObject.AddComponent<LayoutElement>();
            if (keyText == "Space")
            {
                layoutElement.flexibleWidth = 3f;
            }
            else if (keyText == "Shift" || keyText == "Backspace")
            {
                layoutElement.flexibleWidth = 1.5f;
            }
            else
            {
                layoutElement.flexibleWidth = 1f;
            }
            layoutElement.preferredHeight = 80f;
            
            // 创建文本
            GameObject textObject = new GameObject("Text");
            textObject.transform.SetParent(keyObject.transform, false);
            
            TextMeshProUGUI keyLabel = textObject.AddComponent<TextMeshProUGUI>();
            keyLabel.text = GetKeyDisplayText(keyText);
            keyLabel.fontSize = 24;
            keyLabel.color = Color.black;
            keyLabel.alignment = TextAlignmentOptions.Center;
            
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            
            // 设置按钮事件
            button.onClick.AddListener(() => OnKeyPressed(keyText));
        }
        
        private string GetKeyDisplayText(string keyText)
        {
            switch (keyText)
            {
                case "Space": return "空格";
                case "Backspace": return "←";
                case "Enter": return "回车";
                case "Shift": return "⇧";
                case "Clear": return "清空";
                case "Hide": return "隐藏";
                default: return keyText;
            }
        }
        
        private void SetupKeyboardButtons()
        {
            // 查找所有按钮并设置事件
            Button[] buttons = currentKeyboard.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                string keyText = button.name.Replace("Key_", "");
                button.onClick.AddListener(() => OnKeyPressed(keyText));
            }
        }
        
        /// <summary>
        /// 处理按键点击
        /// </summary>
        /// <param name="key">按键文本</param>
        public void OnKeyPressed(string key)
        {
            if (currentInputField == null) return;
            
            // 播放按键音效
            PlayKeySound();
            
            // 处理特殊按键
            if (specialKeys.ContainsKey(key))
            {
                specialKeys[key].Invoke();
                return;
            }
            
            // 处理普通字符输入
            InsertCharacter(key);
        }
        
        private void InsertCharacter(string character)
        {
            if (currentInputField == null) return;
            
            int caretPosition = currentInputField.caretPosition;
            string currentText = currentInputField.text;
            
            // 在光标位置插入字符
            string newText = currentText.Insert(caretPosition, character);
            currentInputField.text = newText;
            
            // 更新光标位置
            currentInputField.caretPosition = caretPosition + character.Length;
        }
        
        private void HandleBackspace()
        {
            if (currentInputField == null || string.IsNullOrEmpty(currentInputField.text)) return;
            
            int caretPosition = currentInputField.caretPosition;
            if (caretPosition > 0)
            {
                string currentText = currentInputField.text;
                currentInputField.text = currentText.Remove(caretPosition - 1, 1);
                currentInputField.caretPosition = caretPosition - 1;
            }
        }
        
        private void HandleSpace()
        {
            InsertCharacter(" ");
        }
        
        private void HandleEnter()
        {
            if (currentInputField != null)
            {
                // 触发输入框的提交事件
                currentInputField.onSubmit.Invoke(currentInputField.text);
                HideKeyboard();
            }
        }
        
        private void HandleShift()
        {
            // TODO: 实现大小写切换逻辑
            Debug.Log("Shift key pressed - 大小写切换");
        }
        
        private void HandleClear()
        {
            if (currentInputField != null)
            {
                currentInputField.text = "";
                currentInputField.caretPosition = 0;
            }
        }
        
        private void PlayKeySound()
        {
            if (keyPressSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(keyPressSound);
            }
        }
        
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
} 