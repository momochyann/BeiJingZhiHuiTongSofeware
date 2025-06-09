using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEditor;
using Cysharp.Threading.Tasks;

public class 题目
{
    public string 题目名称 { get; set; }
    public List<string> 选项 { get; set; }
    public List<string> 答题号 { get; set; }//A,B,C
    public List<string> 分值 { get; set; } 
}

/// <summary>
/// 客观评估类
/// </summary>
public class ObjectiveAssessment : ICan2List
{
    public string 量表简介 { get; set; }
    public string 量表名称 { get; set; }
    public List<题目> 题目列表 { get; set; }
    public 记分规则 记分规则 { get; set; }
}
/// <summary>
/// 客观评估数据库
/// </summary>
public class ObjectiveAssessmentModel : CrisisIncidentBaseModel<ObjectiveAssessment>
{
    public List<ObjectiveAssessment> objectiveAssessments => dataList;
    
    protected override void OnInit()
    {
        base.OnInit();
        //    this.ClearItems();
        LoadObjectiveAssessmentData().Forget();
    }
    public List<ObjectiveAssessment> GetObjectiveAssessments()
    {
        return objectiveAssessments;
    }
    protected override string GetStorageKey()
    {
        return "groupCrisisIncidents";
    }
    async UniTaskVoid LoadObjectiveAssessmentData()
    {
        var excelReader = this.GetUtility<ExcelReader>();
        
        // 确保Excel目录存在
        excelReader.EnsureExcelDirectoryExists();
        
        // 尝试生成清单文件
        excelReader.GenerateExcelManifest();
        
        // 优先使用GetAllExcelFilesAsync而不是ReadFileManifestAsync
        var excelFiles = await excelReader.GetAllExcelFilesAsync();
        
        // 如果没有找到文件，再尝试读取清单
        if (excelFiles.Count == 0)
        {
            excelFiles = await excelReader.ReadFileManifestAsync();
        }
        
        ScaleScoringConfig scaleScoringConfig = await LoadYooAssetsTool.LoadAsset<ScaleScoringConfig>("ScaleScoringConfig");
        Debug.Log("excelFiles.Count: " + excelFiles.Count);
        foreach (var fileName in excelFiles)
        {
            if (objectiveAssessments.Find(x => x.量表名称 == fileName) != null)
            {
                continue;
            }
            var assessment = await excelReader.ReadObjectiveAssessmentDataAsync(fileName);
            if (assessment != null)
            {

                var 记分规则 = scaleScoringConfig.记分规则列表.Find(x => x.量表名称 == assessment.量表名称);
                if (记分规则 != null)
                {
                    assessment.记分规则 = 记分规则;
                    objectiveAssessments.Add(assessment);
                    Debug.Log("objectiveAssessment.量表名称: " + assessment.量表名称 + " " + assessment.量表简介);
                    Debug.Log("assessment.记分规则: " + assessment.记分规则.量表名称);
                }
                else
                {
                    Debug.Log("objectiveAssessment.量表名称" + assessment.量表名称 + "记分规则为空");
                }
            }
            else
            {
                Debug.Log("objectiveAssessment.量表名称为空");
            }
        }
        foreach (var assessment in objectiveAssessments)
        {
            Debug.Log("objectiveAssessment.量表名称: " + assessment.量表名称 + " " + assessment.量表简介);
        }
    }
}
//     }
//     public void SetObjectiveAssessments(List<ObjectiveAssessment> assessments)
//     {
//     // Start is called before the first frame update
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }
// }

// public class GroupCrisisIncidentModel : CrisisIncidentBaseModel<GroupCrisisIncident>
// {
//     public List<GroupCrisisIncident> groupCrisisIncidents => dataList;
//     protected override string GetStorageKey()
//     {
//         return "groupCrisisIncidents";
//     }
//     protected override void OnInit()
//     {
//          base.OnInit();
//     }
//         protected override List<int> OnSearchByName(string keyword)
//     {
//         List<int> indexList = new List<int>();
//         for (int i = 0; i < dataList.Count; i++)
//         {
//             if (dataList[i].incidentName.Contains(keyword))
//             {
//                 indexList.Add(i);
//             }
//         }
//         return indexList;
//     }
// }