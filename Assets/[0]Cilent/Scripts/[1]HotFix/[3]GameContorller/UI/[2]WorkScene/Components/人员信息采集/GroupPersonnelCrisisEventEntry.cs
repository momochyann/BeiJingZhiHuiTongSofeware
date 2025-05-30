using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
// public interface IEntry
// {
//     public void DisEntry(int index);
//     public bool IsChoose { get; set; }
//     public ICan2List can2ListValue { get; set; }
// }
public class GroupPersonnelCrisisEventEntry : MonoBehaviour, IController, IEntry
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text genderText;
    [SerializeField]
    Text categoryText;
    [SerializeField]
    Text dateOfBirthText;
    [SerializeField]
    Text crisisEventNameText;
    [SerializeField]
    Text crisisEventDescriptionText;
    [SerializeField]
    Text crisisEventTimeText;
    [SerializeField]
    Text affectedLevelText;
    [SerializeField]
    Text focusOfTheWorkText;

    [SerializeField]
    Toggle chooseToggle;
    string[] 级别名称 = new string[] { "一级受害者", "二级受害者", "三级受害者", "四级受害者", "五级受害者" };
    public bool IsChoose { get => chooseToggle != null ? chooseToggle.isOn : false; set => chooseToggle.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    GroupPersonnelCrisisEventMessage EntryRawValue;
    public void DisEntry(int index)
    {
        var model = this.GetModel<GroupPersonnelCrisisEventMessageModel>();
        var message = model.groupPersonnelCrisisEventMessages[index];
        EntryRawValue = message;
        _can2List = message;
        nameText.text = message.name;
        genderText.text = message.gender;
        categoryText.text = message.category;
        dateOfBirthText.text = message.dateOfBirth;
        crisisEventDescriptionText.text = message.Description;
        crisisEventTimeText.text = message.EventContactTime;
        affectedLevelText.text = 级别名称[message.affectedLevelIndex];
        focusOfTheWorkText.text = message.focusOfTheWork;
        crisisEventNameText.text = message.groupCrisisIncident.incidentName;
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }

}
