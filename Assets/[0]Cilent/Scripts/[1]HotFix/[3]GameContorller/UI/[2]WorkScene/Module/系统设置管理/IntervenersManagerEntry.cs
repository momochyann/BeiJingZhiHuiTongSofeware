using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;


public class IntervenersManagerEntry : MonoBehaviour, IController, IEntry
{
    [SerializeField]
    Text 姓名;
    [SerializeField]
    Text 性别;
    [SerializeField]
    Text 电话;
    [SerializeField]
    Text 邮箱;
    [SerializeField]
    Text 简介;
    [SerializeField]
    Text 用户名;
    [SerializeField]

    Toggle chooseToggle;

    public bool IsChoose { get => chooseToggle.isOn; set => chooseToggle.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    Intervener EntryRawValue;
    public void DisEntry(int index)
    {
        var model = this.GetModel<IntervenersModel>();
        var message = model.intervenerList[index];
        EntryRawValue = message;
        _can2List = message;
        姓名.text = message.name;
        性别.text = message.性别;
        电话.text = message.电话;
        邮箱.text = message.邮箱;
        简介.text = message.简介;
        用户名.text = message.用户名;
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }

}
