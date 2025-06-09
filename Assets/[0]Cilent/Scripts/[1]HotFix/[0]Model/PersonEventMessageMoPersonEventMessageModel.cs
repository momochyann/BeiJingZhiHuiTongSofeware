using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;

public interface IPersonnelCrisisEventMessageBase : ICan2List
{
    int ID { get; set; }
    string name { get; set; }
    string gender { get; set; }
    string category { get; set; }
    string dateOfBirth { get; set; }
    string Description { get; set; }
}
public enum PersonalCrisisEventMessageFlag
{
    Red,
    Blue,
    Green
}
public interface IPersonalPersonnelCrisisEventMessage : IPersonnelCrisisEventMessageBase
{
    PersonalCrisisEventProperty personalCrisisEventProperty { get; set; }
    PersonalCrisisEventMessageFlag personalCrisisEventMessageFlag { get; set; }
}

public interface IGroupPersonnelCrisisEventMessage : IPersonnelCrisisEventMessageBase
{
    GroupCrisisIncident groupCrisisIncident { get; set; }
    string EventContactTime { get; set; }
    string EventContactProcess { get; set; }
    int affectedLevelIndex { get; set; }
    string focusOfTheWork { get; set; }
    string notes { get; set; }
}
/// <summary>
/// 群体危机人员类
/// </summary>
public class GroupPersonnelCrisisEventMessage : IGroupPersonnelCrisisEventMessage
{
    public int ID { get; set; }
    public string name { get; set; }
    public string gender { get; set; }
    public string category { get; set; }
    public string dateOfBirth { get; set; }
    public string Description { get; set; }
    public GroupCrisisIncident groupCrisisIncident { get; set; }
    public string EventContactTime { get; set; }
    public string EventContactProcess { get; set; }
    public int affectedLevelIndex { get; set; }
    public string focusOfTheWork { get; set; }
    public string notes { get; set; }
}
/// <summary>
/// 个人危机人员类
/// </summary>

public class PersonalPersonnelCrisisEventMessage : IPersonalPersonnelCrisisEventMessage
{
    public int ID { get; set; }
    public string name { get; set; }
    public string gender { get; set; }
    public string category { get; set; }
    public string dateOfBirth { get; set; }
    public string Description { get; set; }
    public PersonalCrisisEventProperty personalCrisisEventProperty { get; set; }
    public PersonalCrisisEventMessageFlag personalCrisisEventMessageFlag { get; set; }
}
/// <summary>
/// 个人危机人员数据库
/// </summary>
public class PersonalPersonnelCrisisEventMessageModel : CrisisIncidentBaseModel<PersonalPersonnelCrisisEventMessage>
{
    public List<PersonalPersonnelCrisisEventMessage> personalPersonnelCrisisEventMessages => dataList;
    protected override string GetStorageKey()
    {
        return "personalPersonnelCrisisEventMessages";
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
            if (dataList[i].name.Contains(keyword))
            {
                indexList.Add(i);
            }
        }
        return indexList;
    }
}
/// <summary>
/// 群体危机人员数据库
/// </summary>
public class GroupPersonnelCrisisEventMessageModel : CrisisIncidentBaseModel<GroupPersonnelCrisisEventMessage>
{
    public List<GroupPersonnelCrisisEventMessage> groupPersonnelCrisisEventMessages => dataList;
    protected override string GetStorageKey()
    {
        return "groupPersonnelCrisisEventMessage";
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
            if (dataList[i].name.Contains(keyword))
            {
                indexList.Add(i);
            }
        }
        return indexList;
    }
}


