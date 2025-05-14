using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

[CreateAssetMenu(fileName = "PersonnelStateColumnConfig", menuName = "IncidentConfig/PersonnelStateColumnConfig")]
public class PersonnelStateConfig : ScriptableObject
{
    public List<PersonnelStateColumn> PersonnelStateColumns;
}

public class PersonnelState
{
    public Dictionary<string, string> personnelState { get; set; }
}

[System.Serializable]
public class PersonnelStateColumn
{
    public string ColumnName { get; set; }
    public string[] ColumnValueOptions { get; set; }
}

public interface IArchivesBase : ICan2List
{
    string name { get; set; }
    string gender { get; set; }
    string category { get; set; }
    string Interveners { get; set; }
    string description { get; set; }
    bool isCreateReport { get; set; }
}

public interface ISubjectiveAssessmentArchive : IArchivesBase
{
    string stressEventDescription { get; set; }
    string influenceEvaluation { get; set; }
    PersonnelState personnelStateEvaluation { get; set; }

}

public interface IObjectiveAssessmentArchive : IArchivesBase
{
    string FormIntroduction { get; set; }
    string ScoreSituation { get; set; }
}

public interface IIndividualInterventionArchive : IArchivesBase
{
    string startDate { get; set; }
    string endDate { get; set; }
    string interventionDescription { get; set; }
    string ScoreSituation { get; set; }
}

public class SubjectiveAssessmentArchive : ISubjectiveAssessmentArchive
{
    public string name { get; set; }
    public string gender { get; set; }
    public string category { get; set; }
    public string Interveners { get; set; }
    public string description { get; set; }
    public bool isCreateReport { get; set; }
    public string createDate { get; set; }
    /// <summary>
    ///应激事件描述
    /// </summary>
    public string stressEventDescription { get; set; }
    /// <summary>
    /// 影响评估
    /// </summary>
    public string influenceEvaluation { get; set; }
    /// <summary>
    /// 人员状态评估
    /// </summary>
    public PersonnelState personnelStateEvaluation { get; set; }

}


public class IndividualInterventionArchive : IIndividualInterventionArchive
{
    public string name { get; set; }
    public string gender { get; set; }
    public string category { get; set; }
    public string Interveners { get; set; }
    public string description { get; set; }
    public bool isCreateReport { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public string interventionDescription { get; set; }
    public string ScoreSituation { get; set; }
}

public interface IGroupInterventionArchive : IArchivesBase
{
    string assistants { get; set; }
    string peopleDetails { get; set; }
    string NumberOfPeople { get; set; }
    string groupName { get; set; }
    string startDate { get; set; }
    string endDate { get; set; }
    string interventionDescription { get; set; }
}

public interface IGroupInterventionMessage
{
    string groupIndex { get; set; }
    string Interveners { get; set; }
    string assistants { get; set; }
    List<PersonalPersonnelCrisisEventMessage> personnelDetails { get; set; }
    string numberOfPeople { get; set; }
    string note { get; set; }
}
public class ObjectiveAssessmentArchive : IObjectiveAssessmentArchive
{
    public string name { get; set; }
    public string gender { get; set; }
    public string category { get; set; }
    public string scaleName { get; set; }
    public string Interveners { get; set; }
    public string description { get; set; }
    public bool isCreateReport { get; set; }
    public string FormIntroduction { get; set; }
    public string ScoreSituation { get; set; }
    public string createDate { get; set; }
}



public class ObjectiveAssessmentArchiveModel : CrisisIncidentBaseModel<ObjectiveAssessmentArchive>
{
    public List<ObjectiveAssessmentArchive> objectiveAssessmentArchives => dataList;
    protected override string GetStorageKey()
    {
        return "ObjectiveAssessmentArchive";
    }
    protected override void OnInit()
    {
        base.OnInit();
        foreach (var item in objectiveAssessmentArchives)
        {
            Debug.Log("item.name: " + item.ScoreSituation);
        }
    }
    // protected override List<int> OnSearchByName(string keyword)
    // {
    //     List<int> indexList = new List<int>();
    //     for (int i = 0; i < dataList.Count; i++)
    //     {
    //         if (dataList[i].incidentName.Contains(keyword))
    //         {
    //             indexList.Add(i);
    //         }
    //     }
    //     return indexList;
    // }
}

public class SubjectiveAssessmentArchiveModel : CrisisIncidentBaseModel<SubjectiveAssessmentArchive>
{
    public List<SubjectiveAssessmentArchive> subjectiveAssessmentArchives => dataList;
    protected override string GetStorageKey()
    {
        return "ObjectiveAssessmentArchive";
    }
    protected override void OnInit()
    {
        base.OnInit();
        foreach (var item in subjectiveAssessmentArchives)
        {
            // Debug.Log("item.name: " + item.ScoreSituation);
        }
    }
    // protected override List<int> OnSearchByName(string keyword)
    // {
    //     List<int> indexList = new List<int>();
    //     for (int i = 0; i < dataList.Count; i++)
    //     {
    //         if (dataList[i].incidentName.Contains(keyword))
    //         {
    //             indexList.Add(i);
    //         }
    //     }
    //     return indexList;
    // }
}