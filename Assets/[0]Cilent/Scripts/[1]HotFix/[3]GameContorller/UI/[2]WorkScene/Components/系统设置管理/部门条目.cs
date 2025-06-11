using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;

public class 部门条目 : MonoBehaviour, IController, IEntry
{
    [SerializeField]
    TMP_Text 部门名称Text;
    [SerializeField]
    TMP_Text 部门编号Text;
    [SerializeField]
    TMP_Text 部门级别Text;
    [SerializeField]
    ExpandableText 部门详情ExpandableText;
    [SerializeField]
    Toggle chooseToggle;
  
    
    public bool IsChoose { get => chooseToggle.isOn; set => chooseToggle.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    部门 EntryRawValue;
    
    public void DisEntry(int index)
    {
        var model = this.GetModel<部门数据Model>();
        var 部门信息 = model.部门列表[index];
        EntryRawValue = 部门信息;
        _can2List = 部门信息;
        
        if (部门名称Text != null)
            部门名称Text.text = 部门信息.部门名称 ?? "";
            
        if (部门级别Text != null)
            部门级别Text.text = 部门信息.部门级别 ?? "";
            
        if (部门编号Text != null)
            部门编号Text.text = 部门信息.部门编号 ?? "";
    
        if (部门详情ExpandableText != null)
        {
            if (!string.IsNullOrEmpty(部门信息.部门描述))
            {
                部门详情ExpandableText.SetText(部门信息.部门描述);
            }
            else
            {
                部门详情ExpandableText.SetText("暂无部门描述");
            }
        }
        else
        {
            Debug.LogWarning("部门详情ExpandableText 组件未赋值");
        }
    }
    
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
