using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

/// <summary>
/// 精简的JSON文件选择器
/// </summary>
public class JsonFileSelector : MonoBehaviour,IController
{
    [Header("UI组件")]
    public Dropdown fileDropdown;
    public Button importButton;
    public Button refreshButton;
    public Button openDirectoryButton;
    public Toggle clearExistingToggle;
    public Text statusText;
    
    private List<string> availableFiles = new List<string>();
    private string selectedFileName = "";
    
    public event Action<string, bool> OnImportRequested;
    
    void Start()
    {
        InitializeUI();
        RefreshFileList();
    }
    
    void InitializeUI()
    {
        if (refreshButton != null)
            refreshButton.onClick.AddListener(RefreshFileList);
            
        if (importButton != null)
        {
            importButton.onClick.AddListener(ImportSelectedFile);
            importButton.interactable = false;
        }
        
        if (openDirectoryButton != null)
            openDirectoryButton.onClick.AddListener(OpenDirectory);
            
        if (fileDropdown != null)
            fileDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
            
        if (clearExistingToggle != null)
            clearExistingToggle.isOn = true;
    }
    
    /// <summary>
    /// 刷新文件列表
    /// </summary>
    public void RefreshFileList()
    {
        availableFiles.Clear();
        
        var jsonUtility = this.GetUtility<JsonDataUtility>();
        availableFiles = jsonUtility.GetAvailableJsonFileNames();
        
        UpdateDropdown();
        UpdateStatus($"找到 {availableFiles.Count} 个JSON文件");
    }
    
    /// <summary>
    /// 更新下拉菜单
    /// </summary>
    void UpdateDropdown()
    {
        if (fileDropdown == null) return;
        
        fileDropdown.ClearOptions();
        
        if (availableFiles.Count == 0)
        {
            fileDropdown.options.Add(new Dropdown.OptionData("未找到JSON文件"));
            fileDropdown.interactable = false;
            if (importButton != null)
                importButton.interactable = false;
        }
        else
        {
            fileDropdown.AddOptions(availableFiles);
            fileDropdown.interactable = true;
            
            // 默认选择第一个文件
            if (availableFiles.Count > 0)
            {
                selectedFileName = availableFiles[0];
                if (importButton != null)
                    importButton.interactable = true;
            }
        }
    }
    
    /// <summary>
    /// 下拉菜单值改变事件
    /// </summary>
    /// <param name="index">选中的索引</param>
    void OnDropdownValueChanged(int index)
    {
        if (index >= 0 && index < availableFiles.Count)
        {
            selectedFileName = availableFiles[index];
            UpdateStatus($"选中文件: {selectedFileName}");
            
            if (importButton != null)
                importButton.interactable = true;
        }
    }
    
    /// <summary>
    /// 导入选中的文件
    /// </summary>
    void ImportSelectedFile()
    {
        if (string.IsNullOrEmpty(selectedFileName))
        {
            UpdateStatus("未选择文件");
            return;
        }
        
        bool clearExisting = clearExistingToggle != null ? clearExistingToggle.isOn : true;
        
        OnImportRequested?.Invoke(selectedFileName, clearExisting);
        UpdateStatus($"请求导入: {selectedFileName}");
    }
    
    /// <summary>
    /// 打开导出目录
    /// </summary>
    void OpenDirectory()
    {
        var jsonUtility = this.GetUtility<JsonDataUtility>();
        jsonUtility.OpenExportDirectory();
        UpdateStatus("已尝试打开导出目录");
    }
    
    /// <summary>
    /// 更新状态显示
    /// </summary>
    /// <param name="message">状态消息</param>
    void UpdateStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
            
        Debug.Log($"[JsonFileSelector] {message}");
    }
    
    /// <summary>
    /// 获取当前选中的文件名
    /// </summary>
    /// <returns>文件名</returns>
    public string GetSelectedFileName()
    {
        return selectedFileName;
    }
    
    /// <summary>
    /// 获取所有可用的文件名
    /// </summary>
    /// <returns>文件名列表</returns>
    public List<string> GetAvailableFileNames()
    {
        return new List<string>(availableFiles);
    }

    public IArchitecture GetArchitecture()
    {
       return HotFixTemplateArchitecture.Interface;
    }
} 