using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;

public class 团体干预实施信息条目 : MonoBehaviour, IController, IEntry
{
    // Start is called before the first frame update

    [SerializeField] 
    TMP_Text 组别名称;
    [SerializeField] TMP_Text 主干预人员;
    [SerializeField] TMP_Text 助理人员;
    [SerializeField] Button 详情按钮;
    [SerializeField] TMP_Text 人员数量;
    [SerializeField] Button 备注按钮;
    [SerializeField] Toggle 选择;
    public bool IsChoose { get => 选择.isOn; set => 选择.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    干预实施团队 EntryRawValue;
    
    
    public void DisEntry(int index)
    {
        // 实现接口方法
        var model = this.GetModel<干预实施Model>();
        var message = model.干预实施列表[index];
        EntryRawValue = message;
        _can2List = message;
        组别名称.text = message.组别名称;
        主干预人员.text = message.主干预人员;
        助理人员.text = message.助理人员;
        人员数量.text = message.人员列表.Count.ToString();
        备注按钮.onClick.AddListener(() =>
        {
            WorkSceneManager.Instance.加载提示("备注").Forget();
        });
        详情按钮.onClick.AddListener(() =>
        {
            WorkSceneManager.Instance.加载提示("详情").Forget();
        });
        选择.onValueChanged.AddListener((bool isOn) =>
        {
            if (isOn)
            {
                WorkSceneManager.Instance.加载提示("选择").Forget();
            }
        });

    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }

    // Update is called once per frame
}
