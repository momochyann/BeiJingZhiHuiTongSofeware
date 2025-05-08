using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;
public class ObjectiveAssessmentEntry : MonoBehaviour, IController, IEntry
{
    // Start is called before the first frame update
    [SerializeField]
    Toggle chooseToggle;
    [SerializeField]
    Text 量表名称;
    [SerializeField]
    Text 问题数量;
    [SerializeField]
    Text 建议时间;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisEntry(int index)
    {
        EntryInit(index);
        量表名称.text = EntryRawValue.量表名称;
        问题数量.text = EntryRawValue.题目列表.Count.ToString();
        建议时间.text = "10";
        // nameText.text = EntryRawValue.incidentName;
        // dateText.text = EntryRawValue.occurrenceTime;
        // placeText.text = EntryRawValue.occurrencePlace;
        // descriptionText.text = EntryRawValue.incidentDescription;
        // LoadImage(EntryRawValue.incidentImageURL).Forget();
        // typeText.text = EntryRawValue.groupCrisisIncidentType.CrisisIncidentTypeDescription;
        // //  affectedLevelText.text = EntryRawValue.affectedLevel.affectedLevel.TryGetValue("一级受害者:", out var value) ? value : "";
        // affectedLevelText.text = "级别";
    }
    
    public bool IsChoose { get => chooseToggle.isOn; set => chooseToggle.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    ObjectiveAssessment EntryRawValue;
    void EntryInit(int index)
    {
        Debug.Log("EntryInit"+index);
        var model = this.GetModel<ObjectiveAssessmentModel>();
        var message = model.objectiveAssessments[index];
        Debug.Log("message.量表名称: "+message.量表名称);
        EntryRawValue = message;
        _can2List = message;
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
