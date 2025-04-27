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
    string stressSceneDescription { get; set; }
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


