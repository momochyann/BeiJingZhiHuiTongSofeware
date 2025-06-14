using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
public class GroupCrisisIncidentEntry : MonoBehaviour, IController, IEntry
{
    // Start is called before the first frame update
    [SerializeField]
    Toggle chooseToggle;
    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    TMP_Text dateText;
    [SerializeField]
    TMP_Text placeText;
    [SerializeField]
    TMP_Text descriptionText;
    [SerializeField]
    Image image;
    [SerializeField]
    TMP_Text typeText;
    [SerializeField]
    TMP_Text affectedLevelText;
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
        var texture = await this.GetUtility<ImagePickerUtility>().LoadImageFromCacheAsync(imageURL);
        // 创建Sprite并显示
        if (texture == null) return;
        Sprite sprite = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
        image.sprite = sprite;
        image.preserveAspect = true;
        image.color = Color.white; // 确保显示正常颜色
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
