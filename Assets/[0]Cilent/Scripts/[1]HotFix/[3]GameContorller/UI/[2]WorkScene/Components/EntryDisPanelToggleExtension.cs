using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Linq;
using UnityEngine.Events;

/// <summary>
/// EntryDisPanelNew的Toggle监听扩展组件
/// </summary>
public class EntryDisPanelToggleExtension : MonoBehaviour, IController
{
    [Header("监听设置")]
    [SerializeField] private bool 监听Entry中的Toggle = true;
    [SerializeField] private bool 监听面板中的Toggle = true;
    
    [Header("事件")]
    public UnityEvent<Toggle, bool> OnEntryToggleChanged;
    public UnityEvent<Toggle, bool> OnPanelToggleChanged;
    
    private EntryDisPanelNew entryDisPanel;
    private List<Toggle> entryToggles = new List<Toggle>();
    private List<Toggle> panelToggles = new List<Toggle>();
    
    void Start()
    {
        entryDisPanel = GetComponent<EntryDisPanelNew>();
        if (entryDisPanel == null)
        {
            Debug.LogError("EntryDisPanelToggleExtension需要附加到有EntryDisPanelNew组件的GameObject上");
            return;
        }
        
        // 延迟初始化，等待EntryDisPanel完成初始化
        StartCoroutine(DelayedInitialize());
    }
    
    IEnumerator DelayedInitialize()
    {
        // 等待EntryDisPanel初始化完成
        yield return new WaitForSeconds(0.5f);
        
        InitializeToggleListeners();
        
        // 注册EntryDisPanel的数据变化事件，当数据变化时重新监听
        this.RegisterEvent<Can2ListModelChangeEvent>(OnModelChange).UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    
    /// <summary>
    /// 初始化Toggle监听器
    /// </summary>
    public void InitializeToggleListeners()
    {
        ClearAllListeners();
        
        if (监听Entry中的Toggle)
        {
            ListenToEntryToggles();
        }
        
        if (监听面板中的Toggle)
        {
            ListenToPanelToggles();
        }
    }
    
    /// <summary>
    /// 监听Entry中的Toggle（如选择框等）
    /// </summary>
    private void ListenToEntryToggles()
    {
        // 获取entryBox中所有Entry的Toggle
        Transform entryBox = entryDisPanel.transform.Find("entryBox");
        if (entryBox != null)
        {
            IEntry[] entries = entryBox.GetComponentsInChildren<IEntry>();
            
            foreach (IEntry entry in entries)
            {
                if (entry is MonoBehaviour entryMono)
                {
                    Toggle[] toggles = entryMono.GetComponentsInChildren<Toggle>();
                    
                    foreach (Toggle toggle in toggles)
                    {
                        if (toggle != null && !entryToggles.Contains(toggle))
                        {
                            entryToggles.Add(toggle);
                            toggle.onValueChanged.AddListener((bool isOn) => OnEntryToggleValueChanged(toggle, isOn));
                            
                            Debug.Log($"为Entry中的Toggle '{toggle.name}' 添加了监听器");
                        }
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 监听面板中的其他Toggle
    /// </summary>
    private void ListenToPanelToggles()
    {
        // 获取面板中除了Entry以外的Toggle
        Toggle[] allToggles = GetComponentsInChildren<Toggle>();
        
        foreach (Toggle toggle in allToggles)
        {
            // 排除已经在entryToggles中的Toggle
            if (!entryToggles.Contains(toggle) && !panelToggles.Contains(toggle))
            {
                panelToggles.Add(toggle);
                toggle.onValueChanged.AddListener((bool isOn) => OnPanelToggleValueChanged(toggle, isOn));
                
                Debug.Log($"为面板中的Toggle '{toggle.name}' 添加了监听器");
            }
        }
    }
    
    /// <summary>
    /// Entry中Toggle状态改变的回调
    /// </summary>
    private void OnEntryToggleValueChanged(Toggle toggle, bool isOn)
    {
        Debug.Log($"Entry中的Toggle '{toggle.name}' 状态改变: {isOn}");
        
        // 触发事件
        OnEntryToggleChanged?.Invoke(toggle, isOn);
        
        // 处理Entry选择逻辑
        HandleEntryToggleChange(toggle, isOn);
    }
    
    /// <summary>
    /// 面板中Toggle状态改变的回调
    /// </summary>
    private void OnPanelToggleValueChanged(Toggle toggle, bool isOn)
    {
        Debug.Log($"面板中的Toggle '{toggle.name}' 状态改变: {isOn}");
        
        // 触发事件
        OnPanelToggleChanged?.Invoke(toggle, isOn);
        
        // 处理面板Toggle逻辑
        HandlePanelToggleChange(toggle, isOn);
    }
    
    /// <summary>
    /// 处理Entry中Toggle的变化
    /// </summary>
    private void HandleEntryToggleChange(Toggle toggle, bool isOn)
    {
        // 获取Toggle所属的Entry
        IEntry entry = toggle.GetComponentInParent<IEntry>();
        if (entry != null)
        {
            // 更新Entry的选择状态
            entry.IsChoose = isOn;
            
            // 获取Entry的显示信息
            string entryInfo = GetEntryInfo(entry);
            
            if (isOn)
            {
                Debug.Log($"选中了Entry: {entryInfo}");
            }
            else
            {
                Debug.Log($"取消选中Entry: {entryInfo}");
            }
        }
    }
    
    /// <summary>
    /// 处理面板中Toggle的变化
    /// </summary>
    private void HandlePanelToggleChange(Toggle toggle, bool isOn)
    {
        // 获取Toggle的文本信息
        Text toggleText = toggle.GetComponentInChildren<Text>();
        string toggleName = toggleText != null ? toggleText.text : toggle.name;
        
        if (isOn)
        {
            Debug.Log($"面板选项被选中: {toggleName}");
        }
        else
        {
            Debug.Log($"面板选项被取消: {toggleName}");
        }
    }
    
    /// <summary>
    /// 获取Entry的信息
    /// </summary>
    private string GetEntryInfo(IEntry entry)
    {
        if (entry?.can2ListValue != null)
        {
            // 根据不同类型的Entry获取不同的信息
            if (entry.can2ListValue is PersonalPersonnelCrisisEventMessage personalMsg)
            {
                return $"{personalMsg.name} - {personalMsg.category}";
            }
            else if (entry.can2ListValue is GroupPersonnelCrisisEventMessage groupMsg)
            {
                return $"{groupMsg.name} - {groupMsg.category}";
            }
        }
        
        return entry?.GetType().Name ?? "未知Entry";
    }
    
    /// <summary>
    /// 获取所有选中的Entry
    /// </summary>
    public List<IEntry> GetSelectedEntries()
    {
        List<IEntry> selectedEntries = new List<IEntry>();
        
        foreach (Toggle toggle in entryToggles)
        {
            if (toggle != null && toggle.isOn)
            {
                IEntry entry = toggle.GetComponentInParent<IEntry>();
                if (entry != null && !selectedEntries.Contains(entry))
                {
                    selectedEntries.Add(entry);
                }
            }
        }
        
        return selectedEntries;
    }
    
    /// <summary>
    /// 获取所有选中的面板Toggle
    /// </summary>
    public List<Toggle> GetSelectedPanelToggles()
    {
        return panelToggles.Where(t => t != null && t.isOn).ToList();
    }
    
    /// <summary>
    /// 数据模型变化时的回调
    /// </summary>
    private void OnModelChange(Can2ListModelChangeEvent _event)
    {
        // 数据变化时重新初始化Toggle监听
        StartCoroutine(DelayedReinitialize());
    }
    
    IEnumerator DelayedReinitialize()
    {
        yield return new WaitForSeconds(0.1f);
        InitializeToggleListeners();
    }
    
    /// <summary>
    /// 清除所有监听器
    /// </summary>
    public void ClearAllListeners()
    {
        foreach (Toggle toggle in entryToggles)
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveAllListeners();
            }
        }
        
        foreach (Toggle toggle in panelToggles)
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveAllListeners();
            }
        }
        
        entryToggles.Clear();
        panelToggles.Clear();
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