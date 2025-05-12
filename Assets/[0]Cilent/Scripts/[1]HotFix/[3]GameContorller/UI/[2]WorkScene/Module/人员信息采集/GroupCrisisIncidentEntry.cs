using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class GroupCrisisIncidentEntry : MonoBehaviour, IController, IEntry
{
    // Start is called before the first frame update
    [SerializeField]
    Toggle chooseToggle;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text dateText;
    [SerializeField]
    Text placeText;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    Image image;
    [SerializeField]
    Text typeText;
    [SerializeField]
    Text affectedLevelText;
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
        nameText.text = EntryRawValue.incidentName;
        dateText.text = EntryRawValue.occurrenceTime;
        placeText.text = EntryRawValue.occurrencePlace;
        descriptionText.text = EntryRawValue.incidentDescription;
        LoadImage(EntryRawValue.incidentImageURL).Forget();
        if (EntryRawValue.groupCrisisIncidentType != null)
            typeText.text = EntryRawValue.groupCrisisIncidentType.CrisisIncidentTypeDescription;
        //  affectedLevelText.text = EntryRawValue.affectedLevel.affectedLevel.TryGetValue("一级受害者:", out var value) ? value : "";
        affectedLevelText.text = "级别";
    }
    async UniTaskVoid LoadImage(string imageURL)
    {
        if (string.IsNullOrEmpty(imageURL)) return;
        var sprite = await this.GetModel<YooAssetPfbModel>().LoadConfig<Sprite>(imageURL);
        image.sprite = sprite;
    }
    public bool IsChoose { get => chooseToggle.isOn; set => chooseToggle.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    GroupCrisisIncident EntryRawValue;
    void EntryInit(int index)
    {
        var model = this.GetModel<GroupCrisisIncidentModel>();
        var message = model.groupCrisisIncidents[index];
        EntryRawValue = message;
        _can2List = message;
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
