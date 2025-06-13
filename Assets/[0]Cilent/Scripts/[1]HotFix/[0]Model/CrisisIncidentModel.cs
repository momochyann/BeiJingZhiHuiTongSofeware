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

/// <summary>
/// 群体危机事件类
/// </summary>
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
/// <summary>
/// 群体危机事件数据库
/// </summary>
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

public class 干预实施团队 : ICan2List
{
    public string 组别名称 { get; set; }
    public string 主干预人员 { get; set; }
    public string 助理人员 { get; set; }
    public List<GroupPersonnelCrisisEventMessage> 人员列表 { get; set; }
    public string 备注 { get; set; }
}

public class 干预实施Model : CrisisIncidentBaseModel<干预实施团队>
{
    public List<干预实施团队> 干预实施列表 => dataList;
    protected override string GetStorageKey()
    {
        return "干预实施";
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
            if (dataList[i].组别名称.Contains(keyword))
            {
                indexList.Add(i);
            }
        }
        return indexList;
    }
    
}
