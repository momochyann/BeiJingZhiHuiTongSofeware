using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;




[Serializable]
public class PersonalCrisisEventProperty
{
    public string eventDescription;
}


[Serializable]
public class GroupCrisisIncidentType
{
    public string CrisisIncidentTypeName;
    public string CrisisIncidentTypeDescription;
}
public interface IAffectedLevel
{
    Dictionary<string, string> affectedLevel { get; set; }
}

public class AffectedLevel : IAffectedLevel
{
    public Dictionary<string, string> affectedLevel { get; set; }

}

public interface IGroupCrisisIncident : ICan2List
{
    int incidentId { get; set; }
    string incidentName { get; set; }
    string occurrenceTime { get; set; }
    string occurrencePlace { get; set; }
    string incidentDescription { get; set; }
    string incidentImageURL { get; set; }
    GroupCrisisIncidentType groupCrisisIncidentType { get; set; }
    AffectedLevel affectedLevel { get; set; }
}
public class GroupCrisisIncident : IGroupCrisisIncident
{
    public int incidentId { get; set; }
    public string incidentName { get; set; }
    public string occurrenceTime { get; set; }
    public string occurrencePlace { get; set; }
    public string incidentDescription { get; set; }
    public string incidentImageURL { get; set; }
    public GroupCrisisIncidentType groupCrisisIncidentType { get; set; }
    public AffectedLevel affectedLevel { get; set; }

}

public class GroupCrisisIncidentModel : CrisisIncidentBaseModel<GroupCrisisIncident>
{
    public List<GroupCrisisIncident> groupCrisisIncidents => dataList;
    protected override string GetStorageKey()
    {
        return "groupCrisisIncidents";
    }
    protected override void OnInit()
    {
         base.OnInit();
    }
        protected override List<int> OnSearchByName(string keyword)
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].incidentName.Contains(keyword))
            {
                indexList.Add(i);
            }
        }
        return indexList;
    }
}


