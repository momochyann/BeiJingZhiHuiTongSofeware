using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "ExcelDataContainer", menuName = "IncidentConfig/Excel Data Container")]
public class ExcelDataContainer : ScriptableObject
{
    [Header("Excel文件数据")]
    public List<ObjectiveAssessment> assessmentList = new List<ObjectiveAssessment>();
    

    [Header("文件信息")]
    public string sourceFolder = "";
    public string lastUpdateTime = "";
    public int totalFiles = 0;

    /// <summary>
    /// 根据量表名称查找量表
    /// </summary>
    public ObjectiveAssessment FindAssessmentByName(string name)
    {
        return assessmentList.Find(a => a.量表名称 == name);
    }

    /// <summary>
    /// 获取所有量表名称
    /// </summary>
    public List<string> GetAllAssessmentNames()
    {
        List<string> names = new List<string>();
        foreach (var assessment in assessmentList)
        {
            if (!string.IsNullOrEmpty(assessment.量表名称))
            {
                names.Add(assessment.量表名称);
            }
        }
        return names;
    }

    /// <summary>
    /// 清空数据
    /// </summary>
    public void ClearData()
    {
        assessmentList.Clear();
        totalFiles = 0;
        lastUpdateTime = "";
    }

    /// <summary>
    /// 添加量表数据
    /// </summary>
    public void AddAssessment(ObjectiveAssessment assessment)
    {
        if (assessment != null)
        {
            assessmentList.Add(assessment);
        }
    }
}