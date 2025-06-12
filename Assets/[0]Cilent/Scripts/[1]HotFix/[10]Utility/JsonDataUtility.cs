using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using LitJson;
using System.IO;
using System;

/// <summary>
/// JSON导入导出工具
/// </summary>
public class JsonDataUtility : IUtility
{
    /// <summary>
    /// 获取跨平台的数据目录路径
    /// </summary>
    /// <returns>数据目录路径</returns>
    public string GetDataDirectoryPath()
    {
        string dataPath;
        
#if UNITY_ANDROID && !UNITY_EDITOR
        dataPath = Path.Combine(Application.persistentDataPath, "Exports");
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        dataPath = Path.Combine(documentsPath, Application.productName, "Exports");
#else
        dataPath = Path.Combine(Application.persistentDataPath, "Exports");
#endif
        
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        
        return dataPath;
    }
    
    /// <summary>
    /// 导出列表数据为JSON文件
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="dataList">数据列表</param>
    /// <param name="fileName">文件名（不含扩展名，可选）</param>
    /// <returns>是否导出成功</returns>
    public bool ExportToJson<T>(List<T> dataList, string fileName = null)
    {
        try
        {
            // 如果没有指定文件名，使用默认名称
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{typeof(T).Name}_{DateTime.Now:yyyyMMdd_HHmmss}";
            }
            
            // 清理文件名中的非法字符
            fileName = SanitizeFileName(fileName);
            
            string filePath = Path.Combine(GetDataDirectoryPath(), fileName + ".json");
            
            // 序列化为JSON
            string jsonData = JsonMapper.ToJson(dataList);
            
            // 写入文件
            File.WriteAllText(filePath, jsonData, System.Text.Encoding.UTF8);
            
            Debug.Log($"数据已成功导出到: {filePath}");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"导出JSON文件失败: {ex.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// 从JSON文件导入数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="fileName">文件名（含或不含扩展名）</param>
    /// <returns>导入的数据列表，失败时返回null</returns>
    public List<T> ImportFromJson<T>(string fileName)
    {
        try
        {
            // 确保文件名有扩展名
            if (!fileName.EndsWith(".json"))
            {
                fileName += ".json";
            }
            
            string filePath = Path.Combine(GetDataDirectoryPath(), fileName);
            
            // 检查文件是否存在
            if (!File.Exists(filePath))
            {
                Debug.LogError($"文件不存在: {filePath}");
                return null;
            }
            
            // 读取文件内容
            string jsonData = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            
            // 反序列化JSON
            List<T> importedData = JsonMapper.ToObject<List<T>>(jsonData);
            
            if (importedData == null)
            {
                Debug.LogError("JSON数据解析失败");
                return null;
            }
            
            Debug.Log($"从 {fileName} 成功导入 {importedData.Count} 条数据");
            return importedData;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"导入JSON文件失败: {ex.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 获取所有可用的JSON文件名
    /// </summary>
    /// <returns>文件名列表</returns>
    public List<string> GetAvailableJsonFileNames()
    {
        List<string> fileNames = new List<string>();
        
        try
        {
            string dataPath = GetDataDirectoryPath();
            
            if (Directory.Exists(dataPath))
            {
                string[] files = Directory.GetFiles(dataPath, "*.json");
                
                foreach (string file in files)
                {
                    fileNames.Add(Path.GetFileName(file));
                }
                
                // 按文件名排序
                fileNames.Sort();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"获取JSON文件列表失败: {ex.Message}");
        }
        
        return fileNames;
    }
    
    /// <summary>
    /// 打开导出目录（仅在PC端有效）
    /// </summary>
    public void OpenExportDirectory()
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        try
        {
            string path = GetDataDirectoryPath();
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"打开目录失败: {ex.Message}");
        }
#elif UNITY_EDITOR_WIN
        try
        {
            string path = GetDataDirectoryPath();
            System.Diagnostics.Process.Start("explorer.exe", path);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"打开目录失败: {ex.Message}");
        }
#else
        Debug.Log($"导出目录: {GetDataDirectoryPath()}");
#endif
    }
    
    /// <summary>
    /// 清理文件名中的非法字符
    /// </summary>
    /// <param name="fileName">原文件名</param>
    /// <returns>清理后的文件名</returns>
    private string SanitizeFileName(string fileName)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            fileName = fileName.Replace(c, '_');
        }
        return fileName;
    }
} 