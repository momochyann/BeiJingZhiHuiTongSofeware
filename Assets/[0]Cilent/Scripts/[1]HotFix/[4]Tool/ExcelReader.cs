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
            // 构建完整的文件路径
            string filePath = Path.Combine(Application.streamingAssetsPath, excelFileName + ".xlsx");

            // 读取文件（使用UnityWebRequest适用于安卓平台）
            byte[] fileData;

            // 在安卓平台上使用UnityWebRequest
            if (Application.platform == RuntimePlatform.Android)
            {
                string uri = "file://" + filePath;
                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
                {
                    await webRequest.SendWebRequest();

                    if (webRequest.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"无法加载Excel文件: {webRequest.error}");
                        return assessment;
                    }

                    fileData = webRequest.downloadHandler.data;
                }
            }
            else // 在其他平台上直接读取文件
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogError($"Excel文件不存在: {filePath}");
                    return assessment;
                }

                fileData = await File.ReadAllBytesAsync(filePath);
            }

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

                            // 读取答题号
                            List<string> 答题号列表 = new List<string>();
                            if (标题行索引 + 1 < 量表答题卡表.Rows.Count)
                            {
                                DataRow 答题号行 = 量表答题卡表.Rows[标题行索引 + 1];
                                for (int i = 答题号开始列; i <= 答题号结束列; i++)
                                {
                                    if (i < 答题号行.ItemArray.Length && !string.IsNullOrEmpty(答题号行[i].ToString()))
                                    {
                                        答题号列表.Add(答题号行[i].ToString());
                                    }
                                }
                            }

                            Debug.Log($"找到答题号: {string.Join(", ", 答题号列表)}");
                            Debug.Log($"答题选项开始列: {答题选项开始列}, 计分分值开始列: {计分分值开始列}");

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

                                    // 添加答题号
                                    foreach (string 答题号 in 答题号列表)
                                    {
                                        新题目.答题号.Add(答题号);
                                    }

                                    // 添加选项
                                    if (答题选项开始列 >= 0)
                                    {
                                        int 选项数量 = 答题号列表.Count;
                                        for (int i = 0; i < 选项数量; i++)
                                        {
                                            int 列索引 = 答题选项开始列 + i;
                                            if (列索引 < row.ItemArray.Length && !string.IsNullOrEmpty(row[列索引].ToString()))
                                            {
                                                新题目.选项.Add(row[列索引].ToString());
                                            }
                                            else
                                            {
                                                新题目.选项.Add("");
                                            }
                                        }
                                    }

                                    // 添加分值
                                    if (计分分值开始列 >= 0)
                                    {
                                        int 分值数量 = 答题号列表.Count;
                                        for (int i = 0; i < 分值数量; i++)
                                        {
                                            int 列索引 = 计分分值开始列 + i;
                                            if (列索引 < row.ItemArray.Length && !string.IsNullOrEmpty(row[列索引].ToString()))
                                            {
                                                新题目.分值.Add(row[列索引].ToString());
                                            }
                                            else
                                            {
                                                新题目.分值.Add("0");
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
            Debug.Log($"成功读取量表: {assessment.量表名称}");
            Debug.Log($"题目数量: {assessment.题目列表.Count}");
            // if (assessment.题目列表.Count > 0)
            // {
            //     Debug.Log($"第一题: {assessment.题目列表[0].题目名称}");
            //     Debug.Log($"答题号: {string.Join(", ", assessment.题目列表[0].答题号)}");
            //     Debug.Log($"选项: {string.Join(", ", assessment.题目列表[0].选项)}");
            //     Debug.Log($"分值: {string.Join(", ", assessment.题目列表[0].分值)}");
            // }
            foreach (var item in assessment.题目列表)
            {
                Debug.Log($"题目名称: {item.题目名称}");
                Debug.Log($"答题号: {string.Join(", ", item.答题号)}");
                Debug.Log($"选项: {string.Join(", ", item.选项)}");
                Debug.Log($"分值: {string.Join(", ", item.分值)}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"读取Excel文件时发生错误: {e.Message}\n{e.StackTrace}");
        }

        return assessment;
    }
}
