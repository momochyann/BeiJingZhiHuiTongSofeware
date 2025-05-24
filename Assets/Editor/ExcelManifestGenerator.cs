using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Excel文件清单生成工具
/// </summary>
public class ExcelManifestGenerator : EditorWindow
{
    private string manifestFileName = "ExcelManifest.txt";
    private string excelFolderPath = "Assets/StreamingAssets";
    private bool includeSubfolders = true;
    private Vector2 scrollPosition;
    private List<string> excelFiles = new List<string>();
    private bool showPreview = false;

    [MenuItem("MomoTools/Excel文件清单生成器")]
    public static void ShowWindow()
    {
        GetWindow<ExcelManifestGenerator>("Excel文件清单生成器");
    }

    private void OnGUI()
    {
        GUILayout.Label("Excel文件清单生成器", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 设置区域
        GUILayout.Label("设置", EditorStyles.boldLabel);
        
        manifestFileName = EditorGUILayout.TextField("清单文件名:", manifestFileName);
        
        EditorGUILayout.BeginHorizontal();
        excelFolderPath = EditorGUILayout.TextField("Excel文件夹路径:", excelFolderPath);
        if (GUILayout.Button("浏览", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("选择Excel文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                // 尝试将路径转换为相对于项目的路径
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                excelFolderPath = path;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        includeSubfolders = EditorGUILayout.Toggle("包含子文件夹:", includeSubfolders);
        
        EditorGUILayout.Space();

        // 操作按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("扫描Excel文件"))
        {
            ScanExcelFiles();
        }
        
        if (GUILayout.Button("生成清单文件"))
        {
            GenerateManifestFile();
        }
        EditorGUILayout.EndHorizontal();

        // 预览切换
        showPreview = EditorGUILayout.Foldout(showPreview, "预览扫描结果");
        
        if (showPreview && excelFiles.Count > 0)
        {
            EditorGUILayout.Space();
            GUILayout.Label($"找到 {excelFiles.Count} 个Excel文件:", EditorStyles.boldLabel);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (string file in excelFiles)
            {
                EditorGUILayout.LabelField(file);
            }
            EditorGUILayout.EndScrollView();
        }
    }

    /// <summary>
    /// 扫描Excel文件
    /// </summary>
    private void ScanExcelFiles()
    {
        excelFiles.Clear();
        
        string fullPath = excelFolderPath;
        if (!Path.IsPathRooted(fullPath))
        {
            // 如果是相对路径，则相对于项目根目录
            fullPath = Path.Combine(Application.dataPath, "..", fullPath);
            fullPath = Path.GetFullPath(fullPath);
        }
        
        if (!Directory.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("错误", $"文件夹不存在: {fullPath}", "确定");
            return;
        }
        
        SearchOption searchOption = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] files = Directory.GetFiles(fullPath, "*.xlsx", searchOption);
        
        string streamingAssetsFullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "StreamingAssets"));
        
        foreach (string file in files)
        {
            // 获取相对于StreamingAssets的路径
            string relativePath;
            
            if (file.StartsWith(streamingAssetsFullPath))
            {
                // 如果文件在StreamingAssets目录下，获取相对路径
                relativePath = file.Substring(streamingAssetsFullPath.Length).TrimStart('\\', '/');
                // 移除.xlsx扩展名
                if (relativePath.EndsWith(".xlsx", System.StringComparison.OrdinalIgnoreCase))
                {
                    relativePath = relativePath.Substring(0, relativePath.Length - 5);
                }
            }
            else
            {
                // 如果文件不在StreamingAssets目录下，只获取文件名（不含扩展名）
                relativePath = Path.GetFileNameWithoutExtension(file);
            }
            
            // 确保路径分隔符统一为正斜杠（/）
            relativePath = relativePath.Replace('\\', '/');
            
            excelFiles.Add(relativePath);
        }
        
        Debug.Log($"找到 {excelFiles.Count} 个Excel文件");
    }

    /// <summary>
    /// 生成清单文件
    /// </summary>
    private void GenerateManifestFile()
    {
        if (excelFiles.Count == 0)
        {
            EditorUtility.DisplayDialog("警告", "没有找到Excel文件，请先扫描", "确定");
            return;
        }
        
        // 确保StreamingAssets目录存在
        string streamingAssetsPath = Path.Combine(Application.dataPath, "StreamingAssets");
        if (!Directory.Exists(streamingAssetsPath))
        {
            Directory.CreateDirectory(streamingAssetsPath);
        }
        
        string manifestPath = Path.Combine(streamingAssetsPath, manifestFileName);
        
        try
        {
            File.WriteAllLines(manifestPath, excelFiles.ToArray());
            Debug.Log($"清单文件已生成: {manifestPath}");
            EditorUtility.DisplayDialog("成功", $"清单文件已生成: {manifestPath}", "确定");
            
            // 刷新资源数据库以显示新文件
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"生成清单文件时出错: {e.Message}");
            EditorUtility.DisplayDialog("错误", $"生成清单文件时出错: {e.Message}", "确定");
        }
    }
} 