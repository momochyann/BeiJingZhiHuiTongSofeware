using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;

public class StorageTest : MonoBehaviour, IController
{
    // Start is called before the first frame update
    void Start()
    {
        var model = this.GetModel<PersonalPersonnelCrisisEventMessageModel>();
        Debug.Log(model.personalPersonnelCrisisEventMessages.Count);
        var button = this.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            model.AddItem(NewPersonalPersonnelCrisisEventMessage(model.personalPersonnelCrisisEventMessages.Count));
            var storage = this.GetUtility<Storage>();
            List<PersonalPersonnelCrisisEventMessage> _personalPersonnelCrisisEventMessages = new List<PersonalPersonnelCrisisEventMessage>();
            if (storage.GetValue("personalPersonnelCrisisEventMessages", ref _personalPersonnelCrisisEventMessages))
            {
                Debug.Log(_personalPersonnelCrisisEventMessages[1].name);
            }
        });
    }
    PersonalPersonnelCrisisEventMessage NewPersonalPersonnelCrisisEventMessage(int _ID)
    {
        var personalPersonnelCrisisEventMessage = new PersonalPersonnelCrisisEventMessage();
        personalPersonnelCrisisEventMessage.name = "张三" + _ID;
        personalPersonnelCrisisEventMessage.gender = "男";
        personalPersonnelCrisisEventMessage.category = "职工组";
        personalPersonnelCrisisEventMessage.personalCrisisEventMessageFlag = (PersonalCrisisEventMessageFlag)Random.Range(0, 3);
        personalPersonnelCrisisEventMessage.personalCrisisEventProperty = new PersonalCrisisEventProperty() { eventDescription = "抑郁" };
        personalPersonnelCrisisEventMessage.dateOfBirth = "2000-01-01";
        personalPersonnelCrisisEventMessage.Description = "Test";
        personalPersonnelCrisisEventMessage.ID = _ID;
        return personalPersonnelCrisisEventMessage;
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}