# Unity 自定义软键盘系统

## 概述

这是一个为Unity开发的自定义软键盘系统，专门针对Android 13/14设备进行优化。该系统允许你替换系统键盘，使用自定义的UI键盘来处理用户输入。

## 功能特性

- ✅ 支持Android 13/14设备
- ✅ 完全自定义的键盘UI
- ✅ 自动禁用系统键盘
- ✅ 简单易用的集成方式
- ✅ 支持TextMeshPro InputField
- ✅ 可自定义键盘布局和样式
- ✅ 内置按键音效支持
- ✅ 响应式设计，适配不同屏幕尺寸

## 文件结构

```
CustomKeyboard/
├── CustomKeyboardManager.cs      # 键盘管理器（核心组件）
├── CustomInputFieldExtension.cs  # 输入框扩展组件
├── AndroidKeyboardHelper.cs      # Android平台辅助类
├── CustomKeyboardExample.cs      # 使用示例
└── README.md                     # 文档说明
```

## 快速开始

### 1. 基础设置

#### 方法一：自动设置（推荐）
1. 在场景中创建一个空物体，命名为"CustomKeyboardManager"
2. 添加`CustomKeyboardManager`组件
3. 运行游戏，系统会自动创建键盘

#### 方法二：手动设置
1. 在场景中添加`CustomKeyboardManager`组件
2. 创建键盘预制体（可选）
3. 设置相关参数

### 2. 为输入框启用自定义键盘

#### 方法一：使用扩展组件
```csharp
// 为任何TMP_InputField添加CustomInputFieldExtension组件
TMP_InputField inputField = GetComponent<TMP_InputField>();
inputField.gameObject.AddComponent<CustomInputFieldExtension>();
```

#### 方法二：代码控制
```csharp
// 直接使用键盘管理器
TMP_InputField inputField = GetComponent<TMP_InputField>();
CustomKeyboardManager.Instance.ShowKeyboard(inputField);
```

### 3. 使用示例组件

添加`CustomKeyboardExample`组件到场景中的任何物体上，它会：
- 自动为所有TMP_InputField启用自定义键盘
- 提供测试按钮和方法
- 显示平台信息

## 详细使用说明

### CustomKeyboardManager 参数说明

```csharp
[Header("键盘设置")]
public GameObject keyboardPrefab;     // 自定义键盘预制体（可选）
public Canvas keyboardCanvas;         // 键盘画布（自动创建）
public int sortingOrder = 1000;       // 渲染层级

[Header("输入设置")]
public float keyPressAnimationTime = 0.1f;  // 按键动画时间
public AudioClip keyPressSound;             // 按键音效
```

### CustomInputFieldExtension 参数说明

```csharp
[Header("键盘设置")]
public bool useCustomKeyboard = true;       // 是否使用自定义键盘
public bool hideSystemKeyboard = true;      // 是否隐藏系统键盘
public bool autoShowOnFocus = true;         // 获得焦点时自动显示键盘
public bool autoHideOnSubmit = true;        // 提交时自动隐藏键盘

[Header("动画设置")]
public float keyboardShowDelay = 0.1f;      // 键盘显示延迟
```

## 高级用法

### 1. 创建自定义键盘预制体

如果你想要完全自定义键盘外观：

1. 创建一个新的GameObject作为键盘根节点
2. 设计你的键盘布局
3. 为每个按键添加Button组件
4. 按键命名格式：`Key_字符`（例如：`Key_A`, `Key_Space`）
5. 将这个GameObject制作成预制体
6. 在CustomKeyboardManager中设置keyboardPrefab

### 2. 特殊按键处理

系统支持以下特殊按键：
- `Backspace` - 退格
- `Space` - 空格
- `Enter` - 回车
- `Shift` - 切换大小写
- `Clear` - 清空
- `Hide` - 隐藏键盘

### 3. 代码API

```csharp
// 显示键盘
CustomKeyboardManager.Instance.ShowKeyboard(inputField);

// 隐藏键盘
CustomKeyboardManager.Instance.HideKeyboard();

// 检查键盘是否可见
bool isVisible = CustomKeyboardManager.Instance.IsKeyboardVisible();

// 处理按键输入
CustomKeyboardManager.Instance.OnKeyPressed("A");
```

### 4. Android平台特殊处理

```csharp
// 检查Android版本
bool isAndroid13Plus = AndroidKeyboardHelper.IsAndroid13Plus();

// 强制隐藏系统键盘
AndroidKeyboardHelper.ForceHideSystemKeyboard();

// 恢复系统键盘
AndroidKeyboardHelper.RestoreSystemKeyboard();
```

## 与现有代码集成

### 更新现有的InputField代码

如果你已经有使用TMP_InputField的代码，可以这样集成：

```csharp
// 原有代码
public class LoginPanel : MonoBehaviour 
{
    [SerializeField] private TMP_InputField accountInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    
    private void Start()
    {
        // 添加自定义键盘支持
        accountInputField.gameObject.AddComponent<CustomInputFieldExtension>();
        passwordInputField.gameObject.AddComponent<CustomInputFieldExtension>();
        
        // 原有的其他初始化代码...
    }
}
```

### 批量处理多个InputField

```csharp
// 为场景中所有TMP_InputField启用自定义键盘
TMP_InputField[] allInputFields = FindObjectsOfType<TMP_InputField>();
foreach (var inputField in allInputFields)
{
    if (inputField.GetComponent<CustomInputFieldExtension>() == null)
    {
        inputField.gameObject.AddComponent<CustomInputFieldExtension>();
    }
}
```

## 构建设置

### Android设置

1. **Player Settings** → **Other Settings**：
   - **Target API Level**: 33 (Android 13) 或更高
   - **Minimum API Level**: 21 或更高

2. **XR Settings**（如果使用）：
   - 确保没有冲突的XR设置

3. **输入系统**：
   - 使用New Input System或Legacy Input System都可以

## 故障排除

### 常见问题

1. **键盘不显示**
   - 检查CustomKeyboardManager是否存在
   - 确保输入框有CustomInputFieldExtension组件
   - 查看Console是否有错误信息

2. **系统键盘仍然显示**
   - 确保shouldHideMobileInput设置为true
   - 在Android设备上测试
   - 检查Android版本是否为13+

3. **按键无响应**
   - 检查EventSystem是否存在
   - 确保键盘Canvas的GraphicRaycaster组件正常
   - 检查按键Button组件的事件设置

4. **UI层级问题**
   - 调整keyboardCanvas的sortingOrder
   - 确保没有其他UI挡住键盘

### 调试技巧

1. 启用详细日志：
```csharp
// 在CustomKeyboardManager.Start()中添加
Debug.Log("键盘管理器初始化完成");
```

2. 使用示例组件的测试方法：
   - 右键点击CustomKeyboardExample组件
   - 选择相应的测试方法

3. 检查平台信息：
```csharp
Debug.Log($"当前平台: {Application.platform}");
Debug.Log($"Android版本: {AndroidKeyboardHelper.GetAndroidVersionInfo()}");
```

## 性能优化

1. **对象池**：对于频繁创建销毁的键盘，考虑使用对象池
2. **事件优化**：及时移除不需要的事件监听
3. **内存管理**：合理控制键盘的生命周期

## 更新日志

### v1.0.0
- 初始版本发布
- 支持基础的自定义键盘功能
- Android 13/14兼容性

## 支持与反馈

如果您在使用过程中遇到问题或有改进建议，请：
1. 检查本文档的故障排除部分
2. 查看Console中的错误日志
3. 尝试使用示例组件进行测试

## 许可证

本项目遵循项目整体的许可证协议。 