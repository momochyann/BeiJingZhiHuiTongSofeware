using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEditor;
using Cysharp.Threading.Tasks;
[System.Serializable]
public class 题目
{
    public string 题目名称;
    public List<string> 选项 ;
    public List<string> 答题号 ;//A,B,C
    public List<string> 分值 ;
}

/// <summary>
/// 客观评估类
/// </summary>
[System.Serializable]
public class ObjectiveAssessment : ICan2List
{
    public string 量表简介 ;
    public string 量表名称 ;
    public List<题目> 题目列表 ;
    public 记分规则 记分规则 ;
}
/// <summary>
/// 客观评估数据库
/// </summary>
public class ObjectiveAssessmentModel : CrisisIncidentBaseModel<ObjectiveAssessment>
{
    public List<ObjectiveAssessment> objectiveAssessments => dataList;

    protected override void OnInit()
    {
        // base.OnInit();
        dataList = new List<ObjectiveAssessment>();
        this.GetSystem<GetCan2ListModelByStringSystem>().Register2System(this);
        //    this.ClearItems();
        从序列化文件中读取量表数据().Forget();
    }
    public List<ObjectiveAssessment> GetObjectiveAssessments()
    {
        return objectiveAssessments;
    }
    protected override string GetStorageKey()
    {
        return "groupCrisisIncidents";
    }
    async UniTaskVoid 从序列化文件中读取量表数据()
    {
        Debug.Log("从序列化文件中读取量表数据");
        ScaleScoringConfig scaleScoringConfig = await LoadYooAssetsTool.LoadAsset<ScaleScoringConfig>("ScaleScoringConfig");
        ExcelDataContainer excelDataContainer = await LoadYooAssetsTool.LoadAsset<ExcelDataContainer>("ExcelDataContainer");
        Debug.Log("excelDataContainer.assessmentList.Count: " + excelDataContainer.assessmentList.Count);
        Debug.Log("scaleScoringConfig.记分规则列表.Count: " + scaleScoringConfig.记分规则列表.Count);
        foreach (var item in scaleScoringConfig.记分规则列表)
        {
            Debug.Log("item.量表名称: " + item.量表名称);
        }
        foreach (var assessment in excelDataContainer.assessmentList)
        {
            Debug.Log("assessment.量表名称: " + assessment.量表名称);

            var 记分规则 = scaleScoringConfig.记分规则列表.Find(x => x.量表名称 == assessment.量表名称);
            if (记分规则 != null)
            {
                Debug.Log("assessment.量表名称: " + assessment.量表名称);
                assessment.记分规则 = 记分规则;
                objectiveAssessments.Add(assessment);
            }
        }
    }
    //     async UniTaskVoid LoadObjectiveAssessmentData()
    //     {
    //         var excelReader = this.GetUtility<ExcelReader>();

    //         // 确保Excel目录存在
    //         excelReader.EnsureExcelDirectoryExists();

    //         // 尝试生成清单文件
    //         excelReader.GenerateExcelManifest();

    //         // 优先使用GetAllExcelFilesAsync而不是ReadFileManifestAsync
    //         var excelFiles = await excelReader.GetAllExcelFilesAsync();

    //         // 如果没有找到文件，再尝试读取清单
    //         if (excelFiles.Count == 0)
    //         {
    //             excelFiles = await excelReader.ReadFileManifestAsync();
    //         }

    //         ScaleScoringConfig scaleScoringConfig = await LoadYooAssetsTool.LoadAsset<ScaleScoringConfig>("ScaleScoringConfig");
    //         Debug.Log("excelFiles.Count: " + excelFiles.Count);
    //         foreach (var fileName in excelFiles)
    //         {
    //             if (objectiveAssessments.Find(x => x.量表名称 == fileName) != null)
    //             {
    //                 continue;
    //             }
    //             var assessment = await excelReader.ReadObjectiveAssessmentDataAsync(fileName);
    //             if (assessment != null)
    //             {

    //                 var 记分规则 = scaleScoringConfig.记分规则列表.Find(x => x.量表名称 == assessment.量表名称);
    //                 if (记分规则 != null)
    //                 {
    //                     assessment.记分规则 = 记分规则;
    //                     objectiveAssessments.Add(assessment);
    //                     Debug.Log("objectiveAssessment.量表名称: " + assessment.量表名称 + " " + assessment.量表简介);
    //                     Debug.Log("assessment.记分规则: " + assessment.记分规则.量表名称);
    //                 }
    //                 else
    //                 {
    //                     Debug.Log("objectiveAssessment.量表名称" + assessment.量表名称 + "记分规则为空");
    //                 }
    //             }
    //             else
    //             {
    //                 Debug.Log("objectiveAssessment.量表名称为空");
    //             }
    //         }
    //         foreach (var assessment in objectiveAssessments)
    //         {
    //             Debug.Log("objectiveAssessment.量表名称: " + assessment.量表名称 + " " + assessment.量表简介);
    //         }
    //     }
    // }
    // //     }
    // //     public void SetObjectiveAssessments(List<ObjectiveAssessment> assessments)
    // //     {
    // //     // Start is called before the first frame update
    // //     void Start()
    // //     {

    // //     }

    // //     // Update is called once per frame
    // //     void Update()
    // //     {

    // //     }
    // // }

    // // public class GroupCrisisIncidentModel : CrisisIncidentBaseModel<GroupCrisisIncident>
    // // {
    // //     public List<GroupCrisisIncident> groupCrisisIncidents => dataList;
    // //     protected override string GetStorageKey()
    // //     {
    // //         return "groupCrisisIncidents";
    // //     }
    // //     protected override void OnInit()
    // //     {
    // //          base.OnInit();
    // //     }
    // //         protected override List<int> OnSearchByName(string keyword)
    // //     {
    // //         List<int> indexList = new List<int>();
    // //         for (int i = 0; i < dataList.Count; i++)
    // //         {
    // //             if (dataList[i].incidentName.Contains(keyword))
    // //             {
    // //                 indexList.Add(i);
    // //             }
    // //         }
    // //         return indexList;
    // //     }
}