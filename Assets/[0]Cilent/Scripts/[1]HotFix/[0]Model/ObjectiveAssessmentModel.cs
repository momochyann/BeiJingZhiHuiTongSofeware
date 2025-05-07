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


public class ObjectiveAssessment : ICan2List
{
    public string 量表简介 { get; set; }
    public string 量表名称 { get; set; }
    public List<题目> 题目列表 { get; set; }
    public 记分规则 记分规则 { get; set; }
}

public class ObjectiveAssessmentModel : CrisisIncidentBaseModel<ObjectiveAssessment>
{
    public List<ObjectiveAssessment> objectiveAssessments => dataList;
    protected override void OnInit()
    {
        base.OnInit();
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

        var objectiveAssessment = await excelReader.ReadObjectiveAssessmentDataAsync("（EQ)情商测试量表+答题卡");
        if (objectiveAssessment != null)
        {
            objectiveAssessments.Add(objectiveAssessment);
            Debug.Log("objectiveAssessment.量表名称: " + objectiveAssessment.量表简介);
        }
        else
        {
            Debug.Log("objectiveAssessment.量表名称为空");
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