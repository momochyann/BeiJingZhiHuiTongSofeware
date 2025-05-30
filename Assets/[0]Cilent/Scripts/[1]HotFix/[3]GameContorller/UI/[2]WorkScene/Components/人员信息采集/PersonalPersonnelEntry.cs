using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
public interface IEntry
{
    public void DisEntry(int index);
    public bool IsChoose { get; set; }
    public ICan2List can2ListValue { get; set; }
}
public class PersonalPersonnelEntry : MonoBehaviour, IController, IEntry
{
    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    TMP_Text genderText;
    [SerializeField]
    TMP_Text categoryText;
    [SerializeField]
    TMP_Text dateOfBirthText;
    [SerializeField]
    ExpandableText crisisEventPropertyExpandableText;
    [SerializeField]
    Toggle chooseToggle;
    [SerializeField]
    Image flag;
    public bool IsChoose { get => chooseToggle.isOn; set => chooseToggle.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    PersonalPersonnelCrisisEventMessage EntryRawValue;
    public void DisEntry(int index)
    {
        var model = this.GetModel<PersonalPersonnelCrisisEventMessageModel>();
        var message = model.personalPersonnelCrisisEventMessages[index];
        EntryRawValue = message;
        _can2List = message;
        nameText.text = message.name;
        genderText.text = message.gender;
        categoryText.text = message.category;
        dateOfBirthText.text = message.dateOfBirth;
        
        if (message.personalCrisisEventProperty != null)
        {
            crisisEventPropertyExpandableText.SetText(message.personalCrisisEventProperty.eventDescription);
        }
        else
        {
            crisisEventPropertyExpandableText.SetText("");
        }
        
        flag.color = message.personalCrisisEventMessageFlag == PersonalCrisisEventMessageFlag.Green ? Color.green : message.personalCrisisEventMessageFlag == PersonalCrisisEventMessageFlag.Blue ? Color.blue : Color.red;
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }

}
