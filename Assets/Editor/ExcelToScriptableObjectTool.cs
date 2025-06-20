using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;
using ExcelDataReader;
using System.Data;
using System.Text;
using Cysharp.Threading.Tasks;

/// <summary>
/// Excel文件转ScriptableObject工具
/// </summary>
public class ExcelToScriptableObjectTool : EditorWindow
{
    // EditorPrefs键名常量
    private const string PREF_SOURCE_FOLDER = "ExcelTool_SourceFolder";
    private const string PREF_OUTPUT_FOLDER = "ExcelTool_OutputFolder";
    private const string PREF_CONTAINER_NAME = "ExcelTool_ContainerName";
    private const string PREF_INCLUDE_SUBFOLDERS = "ExcelTool_IncludeSubfolders";
    private const string PREF_AUTO_CREATE_FOLDERS = "ExcelTool_AutoCreateFolders";
    private const string PREF_SHOW_LOG = "ExcelTool_ShowLog";
    
    private string sourceFolderPath = "Assets/ExcelData";
    private string outputFolderPath = "Assets/Data/ScriptableObjects";
    private string containerName = "ExcelDataContainer";
    private bool includeSubfolders = true;
    private bool autoCreateFolders = true;
    private Vector2 scrollPosition;
    private List<string> excelFiles = new List<string>();
    private List<string> processLog = new List<string>();
    private bool showLog = false;
    private ExcelDataContainer targetContainer;

    [MenuItem("MomoTools/Excel转ScriptableObject工具")]
    public static void ShowWindow()
    {
        GetWindow<ExcelToScriptableObjectTool>("Excel转ScriptableObject工具");
    }

    private void OnEnable()
    {
        // 初始化编码提供程序
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        // 加载保存的设置
        LoadSettings();
    }
    
    private void OnDisable()
    {
        // 保存设置
        SaveSettings();
    }

    /// <summary>
    /// 加载保存的设置
    /// </summary>
    private void LoadSettings()
    {
        sourceFolderPath = EditorPrefs.GetString(PREF_SOURCE_FOLDER, "Assets/ExcelData");
        outputFolderPath = EditorPrefs.GetString(PREF_OUTPUT_FOLDER, "Assets/Data/ScriptableObjects");
        containerName = EditorPrefs.GetString(PREF_CONTAINER_NAME, "ExcelDataContainer");
        includeSubfolders = EditorPrefs.GetBool(PREF_INCLUDE_SUBFOLDERS, true);
        autoCreateFolders = EditorPrefs.GetBool(PREF_AUTO_CREATE_FOLDERS, true);
        showLog = EditorPrefs.GetBool(PREF_SHOW_LOG, false);
    }
    
    /// <summary>
    /// 保存设置
    /// </summary>
    private void SaveSettings()
    {
        EditorPrefs.SetString(PREF_SOURCE_FOLDER, sourceFolderPath);
        EditorPrefs.SetString(PREF_OUTPUT_FOLDER, outputFolderPath);
        EditorPrefs.SetString(PREF_CONTAINER_NAME, containerName);
        EditorPrefs.SetBool(PREF_INCLUDE_SUBFOLDERS, includeSubfolders);
        EditorPrefs.SetBool(PREF_AUTO_CREATE_FOLDERS, autoCreateFolders);
        EditorPrefs.SetBool(PREF_SHOW_LOG, showLog);
    }

    private void OnGUI()
    {
        GUILayout.Label("Excel转ScriptableObject工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 设置区域
        GUILayout.Label("基本设置", EditorStyles.boldLabel);
        
        // 源文件夹路径
        EditorGUILayout.BeginHorizontal();
        string newSourcePath = EditorGUILayout.TextField("Excel文件夹路径:", sourceFolderPath);
        if (newSourcePath != sourceFolderPath)
        {
            sourceFolderPath = newSourcePath;
            SaveSettings(); // 实时保存
        }
        
        if (GUILayout.Button("浏览", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("选择Excel文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                sourceFolderPath = path;
                SaveSettings(); // 保存新路径
            }
        }
        EditorGUILayout.EndHorizontal();
        
        // 输出文件夹路径
        EditorGUILayout.BeginHorizontal();
        string newOutputPath = EditorGUILayout.TextField("输出文件夹路径:", outputFolderPath);
        if (newOutputPath != outputFolderPath)
        {
            outputFolderPath = newOutputPath;
            SaveSettings(); // 实时保存
        }
        
        if (GUILayout.Button("浏览", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("选择输出文件夹", Application.dataPath, "");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                outputFolderPath = path;
                SaveSettings(); // 保存新路径
            }
        }
        EditorGUILayout.EndHorizontal();
        
        string newContainerName = EditorGUILayout.TextField("容器文件名:", containerName);
        if (newContainerName != containerName)
        {
            containerName = newContainerName;
            SaveSettings(); // 实时保存
        }
        
        bool newIncludeSubfolders = EditorGUILayout.Toggle("包含子文件夹:", includeSubfolders);
        if (newIncludeSubfolders != includeSubfolders)
        {
            includeSubfolders = newIncludeSubfolders;
            SaveSettings(); // 实时保存
        }
        
        bool newAutoCreateFolders = EditorGUILayout.Toggle("自动创建文件夹:", autoCreateFolders);
        if (newAutoCreateFolders != autoCreateFolders)
        {
            autoCreateFolders = newAutoCreateFolders;
            SaveSettings(); // 实时保存
        }
        
        EditorGUILayout.Space();
        
        // 目标容器选择
        GUILayout.Label("目标容器", EditorStyles.boldLabel);
        targetContainer = (ExcelDataContainer)EditorGUILayout.ObjectField("现有容器 (可选):", targetContainer, typeof(ExcelDataContainer), false);
        
        EditorGUILayout.Space();

        // 操作按钮
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("扫描Excel文件"))
        {
            ScanExcelFiles();
        }
        
        if (GUILayout.Button("处理Excel文件"))
        {
            ProcessExcelFiles();
        }
        
        if (GUILayout.Button("清空日志"))
        {
            processLog.Clear();
        }
        
        if (GUILayout.Button("重置设置"))
        {
            ResetSettings();
        }
        EditorGUILayout.EndHorizontal();

        // 文件列表预览
        if (excelFiles.Count > 0)
        {
            EditorGUILayout.Space();
            GUILayout.Label($"找到 {excelFiles.Count} 个Excel文件:", EditorStyles.boldLabel);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(150));
            foreach (string file in excelFiles)
            {
                EditorGUILayout.LabelField(file);
            }
            EditorGUILayout.EndScrollView();
        }
        
        // 处理日志
        bool newShowLog = EditorGUILayout.Foldout(showLog, "处理日志");
        if (newShowLog != showLog)
        {
            showLog = newShowLog;
            SaveSettings(); // 保存日志显示状态
        }
        
        if (showLog && processLog.Count > 0)
        {
            EditorGUILayout.Space();
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.MaxHeight(200));
            foreach (string log in processLog)
            {
                EditorGUILayout.LabelField(log);
            }
            EditorGUILayout.EndScrollView();
        }
    }
    
    /// <summary>
    /// 重置所有设置到默认值
    /// </summary>
    private void ResetSettings()
    {
        if (EditorUtility.DisplayDialog("重置设置", "确定要重置所有设置到默认值吗？", "确定", "取消"))
        {
            // 删除所有保存的EditorPrefs
            EditorPrefs.DeleteKey(PREF_SOURCE_FOLDER);
            EditorPrefs.DeleteKey(PREF_OUTPUT_FOLDER);
            EditorPrefs.DeleteKey(PREF_CONTAINER_NAME);
            EditorPrefs.DeleteKey(PREF_INCLUDE_SUBFOLDERS);
            EditorPrefs.DeleteKey(PREF_AUTO_CREATE_FOLDERS);
            EditorPrefs.DeleteKey(PREF_SHOW_LOG);
            
            // 重新加载默认设置
            LoadSettings();
            
            AddLog("设置已重置到默认值");
        }
    }

    /// <summary>
    /// 扫描Excel文件
    /// </summary>
    private void ScanExcelFiles()
    {
        excelFiles.Clear();
        processLog.Clear();
        
        if (!Directory.Exists(sourceFolderPath))
        {
            AddLog($"错误: 源文件夹不存在 - {sourceFolderPath}");
            EditorUtility.DisplayDialog("错误", $"源文件夹不存在: {sourceFolderPath}", "确定");
            return;
        }
        
        SearchOption searchOption = includeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        string[] files = Directory.GetFiles(sourceFolderPath, "*.xlsx", searchOption);
        
        foreach (string file in files)
        {
            // 排除临时文件（以~$开头的文件）
            string fileName = Path.GetFileName(file);
            if (!fileName.StartsWith("~$"))
            {
                excelFiles.Add(file);
            }
        }
        
        AddLog($"扫描完成，找到 {excelFiles.Count} 个Excel文件");
    }

    /// <summary>
    /// 处理Excel文件
    /// </summary>
    private async void ProcessExcelFiles()
    {
        if (excelFiles.Count == 0)
        {
            EditorUtility.DisplayDialog("警告", "没有找到Excel文件，请先扫描", "确定");
            return;
        }
        
        try
        {
            // 确保输出文件夹存在
            if (autoCreateFolders && !Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
                AssetDatabase.Refresh();
                AddLog($"已创建输出文件夹: {outputFolderPath}");
            }
            
            // 创建或获取容器
            ExcelDataContainer container = GetOrCreateContainer();
            if (container == null)
            {
                AddLog("错误: 无法创建或获取容器");
                return;
            }
            
            // 清空现有数据
            container.ClearData();
            container.sourceFolder = sourceFolderPath;
            container.lastUpdateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            int successCount = 0;
            int failCount = 0;
            
            // 处理每个Excel文件
            foreach (string filePath in excelFiles)
            {
                try
                {
                    AddLog($"正在处理: {Path.GetFileName(filePath)}");
                    
                    ObjectiveAssessment assessment = await ReadExcelFile(filePath);
                    if (assessment != null && !string.IsNullOrEmpty(assessment.量表名称))
                    {
                        container.AddAssessment(assessment);
                        successCount++;
                        AddLog($"成功处理: {assessment.量表名称}");
                    }
                    else
                    {
                        failCount++;
                        AddLog($"处理失败: {Path.GetFileName(filePath)} - 数据无效");
                    }
                }
                catch (Exception e)
                {
                    failCount++;
                    AddLog($"处理失败: {Path.GetFileName(filePath)} - {e.Message}");
                }
            }
            
            // 更新容器信息
            container.totalFiles = successCount;
            
            // 保存资源
            EditorUtility.SetDirty(container);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            AddLog($"处理完成! 成功: {successCount}, 失败: {failCount}");
            EditorUtility.DisplayDialog("完成", $"处理完成!\n成功: {successCount}\n失败: {failCount}", "确定");
        }
        catch (Exception e)
        {
            AddLog($"处理过程中发生错误: {e.Message}");
            EditorUtility.DisplayDialog("错误", $"处理过程中发生错误: {e.Message}", "确定");
        }
    }

    /// <summary>
    /// 获取或创建容器
    /// </summary>
    private ExcelDataContainer GetOrCreateContainer()
    {
        if (targetContainer != null)
        {
            return targetContainer;
        }
        
        string containerPath = Path.Combine(outputFolderPath, containerName + ".asset");
        
        // 尝试加载现有容器
        ExcelDataContainer container = AssetDatabase.LoadAssetAtPath<ExcelDataContainer>(containerPath);
        
        if (container == null)
        {
            // 创建新容器
            container = CreateInstance<ExcelDataContainer>();
            AssetDatabase.CreateAsset(container, containerPath);
            AddLog($"已创建新容器: {containerPath}");
        }
        else
        {
            AddLog($"使用现有容器: {containerPath}");
        }
        
        return container;
    }

    /// <summary>
    /// 读取Excel文件 (基于ExcelReader的实现)
    /// </summary>
    private async UniTask<ObjectiveAssessment> ReadExcelFile(string filePath)
    {
        ObjectiveAssessment assessment = new ObjectiveAssessment
        {
            题目列表 = new List<题目>(),
            记分规则 = new 记分规则()
        };

        try
        {
            byte[] fileData = await File.ReadAllBytesAsync(filePath);
            var stream = new MemoryStream(fileData);

            using (stream)
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = false
                        }
                    });

                    // 读取第一个表格（量表信息）
                    if (result.Tables.Count > 0)
                    {
                        DataTable 量表信息表 = result.Tables[0];

                        foreach (DataRow row in 量表信息表.Rows)
                        {
                            if (row[0].ToString() == "量表名称")
                            {
                                assessment.量表名称 = row[1].ToString();
                            }
                            else if (row[0].ToString() == "量表简介")
                            {
                                assessment.量表简介 = row[1].ToString();
                            }
                        }
                    }

                    // 读取第二个表格（量表答题卡）
                    if (result.Tables.Count > 1)
                    {
                        DataTable 量表答题卡表 = result.Tables[1];

                        // 查找标题行
                        int 标题行索引 = -1;
                        for (int i = 0; i < 量表答题卡表.Rows.Count; i++)
                        {
                            if (量表答题卡表.Rows[i][0].ToString() == "题号")
                            {
                                标题行索引 = i;
                                break;
                            }
                        }

                        if (标题行索引 >= 0)
                        {
                            DataRow 标题行 = 量表答题卡表.Rows[标题行索引];

                            // 找到各列的范围
                            int 答题号开始列 = 2;
                            int 答题号结束列 = -1;
                            int 答题选项开始列 = -1;
                            int 计分分值开始列 = -1;

                            for (int i = 0; i < 标题行.ItemArray.Length; i++)
                            {
                                string columnName = 标题行[i].ToString();
                                if (columnName == "答题选项" || columnName.Contains("选项"))
                                {
                                    答题选项开始列 = i;
                                    答题号结束列 = i - 1;
                                }
                                else if (columnName == "计分分值" || columnName.Contains("分值"))
                                {
                                    计分分值开始列 = i;
                                }
                            }

                            // 设置默认值
                            if (答题选项开始列 == -1)
                            {
                                答题选项开始列 = 5;
                                答题号结束列 = 4;
                            }

                            // 创建答题号列表
                            List<string> 答题号列表 = new List<string>();
                            int 答题号数量 = Math.Max(1, Math.Min(答题号结束列 - 答题号开始列 + 1, 26));

                            for (int i = 0; i < 答题号数量; i++)
                            {
                                char 答题号字母 = (char)('A' + i);
                                答题号列表.Add(答题号字母.ToString());
                            }

                            // 读取题目数据
                            for (int rowIndex = 标题行索引 + 1; rowIndex < 量表答题卡表.Rows.Count; rowIndex++)
                            {
                                DataRow row = 量表答题卡表.Rows[rowIndex];

                                if (row.ItemArray.Length >= 2 && !string.IsNullOrEmpty(row[1].ToString()))
                                {
                                    题目 新题目 = new 题目
                                    {
                                        题目名称 = row[1].ToString(),
                                        选项 = new List<string>(),
                                        答题号 = new List<string>(),
                                        分值 = new List<string>()
                                    };

                                    // 添加选项和分值
                                    for (int i = 0; i < 答题号列表.Count; i++)
                                    {
                                        int 选项列索引 = 答题选项开始列 + i;
                                        if (选项列索引 < row.ItemArray.Length)
                                        {
                                            string 选项内容 = row[选项列索引].ToString().Trim();
                                            if (!string.IsNullOrEmpty(选项内容))
                                            {
                                                新题目.答题号.Add(答题号列表[i]);
                                                新题目.选项.Add(选项内容);

                                                int 分值列索引 = 计分分值开始列 + i;
                                                if (分值列索引 < row.ItemArray.Length && !string.IsNullOrEmpty(row[分值列索引].ToString().Trim()))
                                                {
                                                    新题目.分值.Add(row[分值列索引].ToString().Trim());
                                                }
                                                else
                                                {
                                                    新题目.分值.Add("0");
                                                }
                                            }
                                        }
                                    }

                                    assessment.题目列表.Add(新题目);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"读取Excel文件时发生错误: {e.Message}");
            throw;
        }

        return assessment;
    }

    /// <summary>
    /// 添加日志
    /// </summary>
    private void AddLog(string message)
    {
        processLog.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
        Debug.Log(message);
        Repaint();
    }
} 