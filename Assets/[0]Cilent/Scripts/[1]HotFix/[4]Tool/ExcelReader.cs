using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.IO;
using ExcelDataReader;
using Cysharp.Threading.Tasks;
using System.Text;
using System.Data;
using System;
using UnityEngine.Networking;

public class ExcelReader : IUtility
{
    public void Init()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// 读取量表数据并返回单个ObjectiveAssessment对象
    /// </summary>
    async public UniTask<ObjectiveAssessment> ReadObjectiveAssessmentDataAsync(string excelFileName)
    {
        // 创建量表对象
        ObjectiveAssessment assessment = new ObjectiveAssessment
        {
            题目列表 = new List<题目>(),
            记分规则 = new 记分规则()
        };

        try
        {
            // 修改这里：使用persistentDataPath而不是streamingAssetsPath
            string filePath = Path.Combine(Application.persistentDataPath, "ExcelData", excelFileName + ".xlsx");

            // 读取文件（不再需要UnityWebRequest，因为persistentDataPath是可直接访问的）
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Excel文件不存在: {filePath}");
                return assessment;
            }

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
                            UseHeaderRow = false // 不使用第一行作为列名
                        }
                    });

                    // 读取第一个表格（量表信息）
                    if (result.Tables.Count > 0)
                    {
                        DataTable 量表信息表 = result.Tables[0];

                        // 遍历量表信息表
                        foreach (DataRow row in 量表信息表.Rows)
                        {
                            // 检查A列是否包含"量表名称"
                            if (row[0].ToString() == "量表名称")
                            {
                                assessment.量表名称 = row[1].ToString();
                            }
                            // 检查A列是否包含"量表简介"
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

                        // 查找标题行（包含"题号"、"题目名称"等）
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
                            // 找到了标题行
                            DataRow 标题行 = 量表答题卡表.Rows[标题行索引];

                            // 找到答题号列的范围（从C列开始）
                            int 答题号开始列 = 2; // C列
                            int 答题号结束列 = -1;

                            // 找到答题选项列的范围
                            int 答题选项开始列 = -1;
                            for (int i = 0; i < 标题行.ItemArray.Length; i++)
                            {
                                if (标题行[i].ToString() == "答题选项")
                                {
                                    答题选项开始列 = i;
                                    答题号结束列 = i - 1; // 答题号结束列是答题选项开始列的前一列
                                    break;
                                }
                            }

                            if (答题选项开始列 == -1)
                            {
                                // 如果没有找到"答题选项"列，尝试查找其他可能的列名
                                for (int i = 0; i < 标题行.ItemArray.Length; i++)
                                {
                                    string columnName = 标题行[i].ToString();
                                    if (columnName.Contains("选项") || columnName.Contains("答案"))
                                    {
                                        答题选项开始列 = i;
                                        答题号结束列 = i - 1;
                                        break;
                                    }
                                }
                            }

                            // 如果仍然没有找到，使用默认值
                            if (答题选项开始列 == -1)
                            {
                                答题选项开始列 = 5; // 默认F列
                                答题号结束列 = 4;   // 默认E列
                            }

                            // 找到计分分值列的范围
                            int 计分分值开始列 = -1;
                            int 计分分值结束列 = -1;
                            for (int i = 0; i < 标题行.ItemArray.Length; i++)
                            {
                                if (标题行[i].ToString() == "计分分值")
                                {
                                    计分分值开始列 = i;
                                    计分分值结束列 = 标题行.ItemArray.Length - 1; // 到最后一列
                                    break;
                                }
                            }

                            // 如果没有找到计分分值列，尝试查找其他可能的列名
                            if (计分分值开始列 == -1)
                            {
                                for (int i = 0; i < 标题行.ItemArray.Length; i++)
                                {
                                    string columnName = 标题行[i].ToString();
                                    if (columnName.Contains("分值") || columnName.Contains("分数"))
                                    {
                                        计分分值开始列 = i;
                                        计分分值结束列 = 标题行.ItemArray.Length - 1;
                                        break;
                                    }
                                }
                            }

                            // 直接创建答题号列表，按字母顺序填入
                            List<string> 答题号列表 = new List<string>();
                            int 答题号数量 = 答题号结束列 - 答题号开始列 + 1;

                            // 确保答题号数量合理
                            答题号数量 = Math.Max(1, Math.Min(答题号数量, 26)); // 最少1个，最多26个（A-Z）

                            // 按字母顺序创建答题号
                            for (int i = 0; i < 答题号数量; i++)
                            {
                                char 答题号字母 = (char)('A' + i);
                                答题号列表.Add(答题号字母.ToString());
                            }

                       

                            // 从标题行之后开始读取题目数据
                            for (int rowIndex = 标题行索引 + 1; rowIndex < 量表答题卡表.Rows.Count; rowIndex++)
                            {
                                DataRow row = 量表答题卡表.Rows[rowIndex];

                                // 确保至少有题号和题目名称
                                if (row.ItemArray.Length >= 2 && !string.IsNullOrEmpty(row[1].ToString()))
                                {
                                    题目 新题目 = new 题目
                                    {
                                        题目名称 = row[1].ToString(), // 第二列是题目名称
                                        选项 = new List<string>(),
                                        答题号 = new List<string>(),
                                        分值 = new List<string>()
                                    };

                                    // 添加答题号、选项和分值
                                    for (int i = 0; i < 答题号列表.Count; i++)
                                    {
                                        // 检查选项是否有实际数据
                                        int 选项列索引 = 答题选项开始列 + i;
                                        bool 有选项数据 = false;
                                        
                                        if (选项列索引 < row.ItemArray.Length)
                                        {
                                            string 选项内容 = row[选项列索引].ToString().Trim(); // 去除前后空格
                                            有选项数据 = !string.IsNullOrEmpty(选项内容);
                                        }
                                        
                                        // 只有当选项有实际数据时，才添加答题号、选项和分值
                                        if (有选项数据)
                                        {
                                            
                                            // 添加答题号
                                            新题目.答题号.Add(答题号列表[i]);
                                            
                                            // 添加选项
                                            新题目.选项.Add(row[选项列索引].ToString().Trim());
                                            
                                            // 添加分值
                                            int 分值列索引 = 计分分值开始列 + i;
                                            if (分值列索引 < row.ItemArray.Length && !string.IsNullOrEmpty(row[分值列索引].ToString().Trim()))
                                            {
                                                新题目.分值.Add(row[分值列索引].ToString().Trim());
                                            }
                                            else
                                            {
                                                新题目.分值.Add("0"); // 默认分值
                                            }
                                        }
                                    }

                                    assessment.题目列表.Add(新题目);
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("未找到标题行（包含'题号'）");
                        }
                    }
                }
            }

            // 打印读取结果，便于调试
            // Debug.Log($"成功读取量表: {assessment.量表名称}");
            // Debug.Log($"题目数量: {assessment.题目列表.Count}");
            // // if (assessment.题目列表.Count > 0)
            // // {
            // //     Debug.Log($"第一题: {assessment.题目列表[0].题目名称}");
            // //     Debug.Log($"答题号: {string.Join(", ", assessment.题目列表[0].答题号)}");
            // //     Debug.Log($"选项: {string.Join(", ", assessment.题目列表[0].选项)}");
            // //     Debug.Log($"分值: {string.Join(", ", assessment.题目列表[0].分值)}");
            // // }
            // foreach (var item in assessment.题目列表)
            // {
            //     Debug.Log($"题目名称: {item.题目名称}");
            //     Debug.Log($"答题号: {string.Join(", ", item.答题号)} {item.答题号.Count}");
            //     Debug.Log($"选项: {string.Join(", ", item.选项)} {item.选项.Count}");
            //     Debug.Log($"分值: {string.Join(", ", item.分值)} {item.分值.Count}");
            // }
        }
        catch (Exception e)
        {
            Debug.LogError($"读取Excel文件时发生错误: {e.Message}\n{e.StackTrace}");
        }

        return assessment;
    }

    /// <summary>
    /// 获取持久化数据目录下所有的xlsx文件
    /// </summary>
    /// <returns>xlsx文件名列表（不含扩展名）</returns>
    async public UniTask<List<string>> GetAllExcelFilesAsync()
    {
        List<string> excelFiles = new List<string>();
        
        try
        {
            // 修改这里：使用persistentDataPath
            string excelDir = Path.Combine(Application.persistentDataPath, "ExcelData");
            
            // 确保目录存在
            if (!Directory.Exists(excelDir))
            {
                Directory.CreateDirectory(excelDir);
                return excelFiles; // 如果目录不存在，返回空列表
            }
            
            // 直接读取目录（在Android上也可以直接访问persistentDataPath）
            string[] files = Directory.GetFiles(excelDir, "*.xlsx");
            foreach (string file in files)
            {
                // 提取文件名（不含路径和扩展名）
                string fileName = Path.GetFileNameWithoutExtension(file);
                excelFiles.Add(fileName);
            }
            
            // 如果没有找到文件，尝试读取清单
            if (excelFiles.Count == 0)
            {
                excelFiles = await ReadFileManifestAsync();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"获取Excel文件列表时发生错误: {e.Message}\n{e.StackTrace}");
        }
        
        return excelFiles;
    }
    
    /// <summary>
    /// 读取文件清单
    /// </summary>
    /// <param name="manifestFileName">清单文件名，默认为ExcelManifest.txt</param>
    /// <returns>xlsx文件名列表（不含扩展名）</returns>
    async public UniTask<List<string>> ReadFileManifestAsync(string manifestFileName = "ExcelManifest.txt")
    {
        List<string> fileList = new List<string>();
        
        try
        {
            // 修改这里：使用persistentDataPath
            string manifestPath = Path.Combine(Application.persistentDataPath, "ExcelData", manifestFileName);
            
            if (File.Exists(manifestPath))
            {
                string[] lines = await File.ReadAllLinesAsync(manifestPath);
                
                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();
                    if (!string.IsNullOrEmpty(trimmedLine))
                    {
                        // 如果文件名包含扩展名，则移除
                        string fileName = trimmedLine;
                        if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName = fileName.Substring(0, fileName.Length - 5);
                        }
                        fileList.Add(fileName);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"清单文件不存在: {manifestPath}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"读取文件清单时发生错误: {e.Message}\n{e.StackTrace}");
        }
        
        return fileList;
    }

    /// <summary>
    /// 生成Excel文件清单
    /// </summary>
    public void GenerateExcelManifest()
    {
        try
        {
            string excelDir = Path.Combine(Application.persistentDataPath, "ExcelData");
            
            // 确保目录存在
            if (!Directory.Exists(excelDir))
            {
                Debug.LogWarning($"Excel目录不存在: {excelDir}");
                return;
            }
            
            // 获取所有Excel文件
            string[] files = Directory.GetFiles(excelDir, "*.xlsx");
            
            // 创建清单文件
            string manifestPath = Path.Combine(excelDir, "ExcelManifest.txt");
            using (StreamWriter writer = new StreamWriter(manifestPath))
            {
                foreach (string file in files)
                {
                    // 只写入文件名（包含扩展名）
                    writer.WriteLine(Path.GetFileName(file));
                }
            }
            
            Debug.Log($"Excel清单文件已生成: {manifestPath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"生成Excel清单文件时发生错误: {e.Message}\n{e.StackTrace}");
        }
    }

    /// <summary>
    /// 确保Excel目录存在并初始化
    /// </summary>
    public void EnsureExcelDirectoryExists()
    {
        string excelDir = Path.Combine(Application.persistentDataPath, "ExcelData");
        
        // 确保目录存在
        if (!Directory.Exists(excelDir))
        {
            Directory.CreateDirectory(excelDir);
            Debug.Log($"已创建Excel目录: {excelDir}");
        }
        
        // 输出目录路径，方便在设备上查找
        Debug.Log($"Excel文件应放置在: {excelDir}");
    }
}
