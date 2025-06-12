using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

/// <summary>
/// EntryDisPanelToggleExtension使用示例
/// </summary>
public class 使用示例_EntryDisPanelToggleExtension : MonoBehaviour, IController
{
    private EntryDisPanelToggleExtension toggleExtension;
    
    void Start()
    {
        // 方法1: 自动添加组件
        添加Toggle监听组件();
        
        // 方法2: 如果已经有组件，直接获取
        // toggleExtension = GetComponent<EntryDisPanelToggleExtension>();
        
        // 设置事件监听
        设置事件监听();
    }
    
    /// <summary>
    /// 自动添加Toggle监听组件
    /// </summary>
    void 添加Toggle监听组件()
    {
        // 检查是否已经有EntryDisPanelNew组件
        EntryDisPanelNew entryDisPanel = GetComponent<EntryDisPanelNew>();
        if (entryDisPanel == null)
        {
            Debug.LogError("当前GameObject没有EntryDisPanelNew组件！");
            return;
        }
        
        // 检查是否已经有扩展组件
        toggleExtension = GetComponent<EntryDisPanelToggleExtension>();
        if (toggleExtension == null)
        {
            // 添加扩展组件
            toggleExtension = gameObject.AddComponent<EntryDisPanelToggleExtension>();
            Debug.Log("已添加EntryDisPanelToggleExtension组件");
        }
    }
    
    /// <summary>
    /// 设置事件监听
    /// </summary>
    void 设置事件监听()
    {
        if (toggleExtension == null) return;
        
        // 监听Entry中Toggle的变化（如人员选择框）
        toggleExtension.OnEntryToggleChanged.AddListener(OnEntryToggleChanged);
        
        // 监听面板中Toggle的变化（如筛选选项）
        toggleExtension.OnPanelToggleChanged.AddListener(OnPanelToggleChanged);
    }
    
    /// <summary>
    /// Entry中Toggle状态改变时的处理
    /// </summary>
    void OnEntryToggleChanged(Toggle toggle, bool isOn)
    {
        Debug.Log($"Entry Toggle变化: {toggle.name} = {isOn}");
        
        // 获取选中的所有Entry
        List<IEntry> selectedEntries = toggleExtension.GetSelectedEntries();
        Debug.Log($"当前选中的Entry数量: {selectedEntries.Count}");
        
        // 处理具体的业务逻辑
        if (isOn)
        {
            处理Entry被选中(toggle);
        }
        else
        {
            处理Entry被取消选中(toggle);
        }
        
        // 更新UI显示
        更新选中状态显示();
    }
    
    /// <summary>
    /// 面板中Toggle状态改变时的处理
    /// </summary>
    void OnPanelToggleChanged(Toggle toggle, bool isOn)
    {
        Debug.Log($"面板 Toggle变化: {toggle.name} = {isOn}");
        
        // 获取选中的面板Toggle
        List<Toggle> selectedPanelToggles = toggleExtension.GetSelectedPanelToggles();
        Debug.Log($"当前选中的面板Toggle数量: {selectedPanelToggles.Count}");
        
        // 处理面板选项变化
        处理面板选项变化(toggle, isOn);
    }
    
    /// <summary>
    /// 处理Entry被选中
    /// </summary>
    void 处理Entry被选中(Toggle toggle)
    {
        // 获取Entry信息
        IEntry entry = toggle.GetComponentInParent<IEntry>();
        if (entry?.can2ListValue is PersonalPersonnelCrisisEventMessage personalMsg)
        {
            Debug.Log($"选中人员: {personalMsg.name} - {personalMsg.category}");
            
            // 这里可以添加你的业务逻辑
            // 例如：更新选中人员列表、显示详细信息等
        }
    }
    
    /// <summary>
    /// 处理Entry被取消选中
    /// </summary>
    void 处理Entry被取消选中(Toggle toggle)
    {
        IEntry entry = toggle.GetComponentInParent<IEntry>();
        if (entry?.can2ListValue is PersonalPersonnelCrisisEventMessage personalMsg)
        {
            Debug.Log($"取消选中人员: {personalMsg.name}");
            
            // 这里可以添加你的业务逻辑
        }
    }
    
    /// <summary>
    /// 处理面板选项变化
    /// </summary>
    void 处理面板选项变化(Toggle toggle, bool isOn)
    {
        Text toggleText = toggle.GetComponentInChildren<Text>();
        string optionName = toggleText != null ? toggleText.text : toggle.name;
        
        if (isOn)
        {
            Debug.Log($"启用面板选项: {optionName}");
            // 处理选项启用逻辑
        }
        else
        {
            Debug.Log($"禁用面板选项: {optionName}");
            // 处理选项禁用逻辑
        }
    }
    
    /// <summary>
    /// 更新选中状态显示
    /// </summary>
    void 更新选中状态显示()
    {
        if (toggleExtension == null) return;
        
        List<IEntry> selectedEntries = toggleExtension.GetSelectedEntries();
        List<Toggle> selectedPanelToggles = toggleExtension.GetSelectedPanelToggles();
        
        // 更新UI显示选中数量
        Debug.Log($"总共选中: {selectedEntries.Count} 个Entry, {selectedPanelToggles.Count} 个面板选项");
        
        // 这里可以更新UI文本显示
        // 例如：已选择人员数量、已启用的筛选条件等
    }
    
    /// <summary>
    /// 获取所有选中人员的信息
    /// </summary>
    public List<PersonalPersonnelCrisisEventMessage> 获取选中人员列表()
    {
        List<PersonalPersonnelCrisisEventMessage> selectedPersons = new List<PersonalPersonnelCrisisEventMessage>();
        
        if (toggleExtension != null)
        {
            List<IEntry> selectedEntries = toggleExtension.GetSelectedEntries();
            
            foreach (IEntry entry in selectedEntries)
            {
                if (entry.can2ListValue is PersonalPersonnelCrisisEventMessage personalMsg)
                {
                    selectedPersons.Add(personalMsg);
                }
            }
        }
        
        return selectedPersons;
    }
    
    /// <summary>
    /// 清空所有选择
    /// </summary>
    public void 清空所有选择()
    {
        if (toggleExtension != null)
        {
            // 获取所有Entry的Toggle并设置为false
            List<IEntry> allEntries = new List<IEntry>();
            Transform entryBox = transform.Find("entryBox");
            if (entryBox != null)
            {
                IEntry[] entries = entryBox.GetComponentsInChildren<IEntry>();
                foreach (IEntry entry in entries)
                {
                    entry.IsChoose = false;
                }
            }
            
            Debug.Log("已清空所有选择");
        }
    }
    
    /// <summary>
    /// 选择所有Entry
    /// </summary>
    public void 选择所有Entry()
    {
        if (toggleExtension != null)
        {
            Transform entryBox = transform.Find("entryBox");
            if (entryBox != null)
            {
                IEntry[] entries = entryBox.GetComponentsInChildren<IEntry>();
                foreach (IEntry entry in entries)
                {
                    entry.IsChoose = true;
                }
            }
            
            Debug.Log("已选择所有Entry");
        }
    }
    
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
} 