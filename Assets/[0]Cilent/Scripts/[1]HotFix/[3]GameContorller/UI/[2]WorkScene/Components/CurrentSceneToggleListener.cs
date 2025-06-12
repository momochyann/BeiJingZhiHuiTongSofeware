using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Linq;
using UnityEngine.Events;

[System.Serializable]
public class ToggleChangeEvent : UnityEvent<Toggle, bool> { }

public class CurrentSceneToggleListener : MonoBehaviour, IController
{
    [Header("监听设置")]
    [SerializeField] private bool 包含子物体 = true;
    [SerializeField] private bool 包含非激活对象 = true;
    [SerializeField] private bool 自动初始化 = true;
    
    [Header("事件")]
    public ToggleChangeEvent OnToggleChanged;
    public UnityEvent<List<Toggle>> OnAllTogglesFound;
    
    private List<Toggle> currentToggles = new List<Toggle>();
    private Dictionary<Toggle, bool> toggleStates = new Dictionary<Toggle, bool>();
    
    void Start()
    {
        if (自动初始化)
        {
            InitializeToggleListeners();
        }
    }
    
    /// <summary>
    /// 初始化Toggle监听器
    /// </summary>
    public void InitializeToggleListeners()
    {
        FindAndListenToToggles();
    }
    
    /// <summary>
    /// 查找并监听当前界面的所有Toggle
    /// </summary>
    public void FindAndListenToToggles()
    {
        // 清除之前的监听器
        ClearAllListeners();
        
        // 查找当前界面的所有Toggle
        if (包含子物体)
        {
            currentToggles = GetComponentsInChildren<Toggle>(包含非激活对象).ToList();
        }
        else
        {
            currentToggles = GetComponents<Toggle>().ToList();
        }
        
        Debug.Log($"找到 {currentToggles.Count} 个Toggle组件");
        
        // 为每个Toggle添加监听器
        foreach (Toggle toggle in currentToggles)
        {
            if (toggle != null)
            {
                // 记录初始状态
                toggleStates[toggle] = toggle.isOn;
                
                // 添加监听器
                toggle.onValueChanged.AddListener((bool isOn) => OnToggleValueChanged(toggle, isOn));
                
                Debug.Log($"为Toggle '{toggle.name}' 添加了监听器，初始状态: {toggle.isOn}");
            }
        }
        
        // 触发找到所有Toggle的事件
        OnAllTogglesFound?.Invoke(currentToggles);
    }
    
    /// <summary>
    /// Toggle状态改变时的回调
    /// </summary>
    private void OnToggleValueChanged(Toggle toggle, bool isOn)
    {
        Debug.Log($"Toggle '{toggle.name}' 状态改变: {isOn}");
        
        // 更新状态记录
        toggleStates[toggle] = isOn;
        
        // 触发事件
        OnToggleChanged?.Invoke(toggle, isOn);
        
        // 你可以在这里添加具体的业务逻辑
        HandleToggleChange(toggle, isOn);
    }
    
    /// <summary>
    /// 处理Toggle状态改变的具体逻辑
    /// </summary>
    protected virtual void HandleToggleChange(Toggle toggle, bool isOn)
    {
        // 可以在子类中重写这个方法来实现具体的业务逻辑
        
        // 示例：获取Toggle的文本内容
        Text toggleText = toggle.GetComponentInChildren<Text>();
        string toggleName = toggleText != null ? toggleText.text : toggle.name;
        
        if (isOn)
        {
            Debug.Log($"选中了: {toggleName}");
        }
        else
        {
            Debug.Log($"取消选中: {toggleName}");
        }
    }
    
    /// <summary>
    /// 获取所有选中的Toggle
    /// </summary>
    public List<Toggle> GetSelectedToggles()
    {
        return currentToggles.Where(t => t != null && t.isOn).ToList();
    }
    
    /// <summary>
    /// 获取所有未选中的Toggle
    /// </summary>
    public List<Toggle> GetUnselectedToggles()
    {
        return currentToggles.Where(t => t != null && !t.isOn).ToList();
    }
    
    /// <summary>
    /// 获取Toggle的显示文本
    /// </summary>
    public string GetToggleText(Toggle toggle)
    {
        if (toggle == null) return "";
        
        Text text = toggle.GetComponentInChildren<Text>();
        return text != null ? text.text : toggle.name;
    }
    
    /// <summary>
    /// 设置所有Toggle的状态
    /// </summary>
    public void SetAllToggles(bool isOn)
    {
        foreach (Toggle toggle in currentToggles)
        {
            if (toggle != null)
            {
                toggle.isOn = isOn;
            }
        }
    }
    
    /// <summary>
    /// 清除所有监听器
    /// </summary>
    public void ClearAllListeners()
    {
        foreach (Toggle toggle in currentToggles)
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveAllListeners();
            }
        }
        
        currentToggles.Clear();
        toggleStates.Clear();
    }
    
    /// <summary>
    /// 刷新Toggle列表（当界面内容动态变化时使用）
    /// </summary>
    public void RefreshToggleList()
    {
        FindAndListenToToggles();
    }
    
    /// <summary>
    /// 获取当前所有Toggle的状态信息
    /// </summary>
    public Dictionary<string, bool> GetAllToggleStates()
    {
        Dictionary<string, bool> states = new Dictionary<string, bool>();
        
        foreach (Toggle toggle in currentToggles)
        {
            if (toggle != null)
            {
                string key = GetToggleText(toggle);
                states[key] = toggle.isOn;
            }
        }
        
        return states;
    }
    
    void OnDestroy()
    {
        ClearAllListeners();
    }
    
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
} 