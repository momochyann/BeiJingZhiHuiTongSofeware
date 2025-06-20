using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class H_显示备注 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Text 备注文本;
    [SerializeField] Button 关闭按钮;
    void Start()
    {
        if (备注文本 == null)
        {
            Debug.LogError("备注文本组件未找到");
        }
        if (关闭按钮 == null)
        {
            Debug.LogError("关闭按钮组件未找到");
        }
        关闭按钮.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void 显示备注(string 备注内容)
    {
        if (备注文本 == null)
        {
            Debug.LogError("备注文本组件未找到");
            return;
        }
        备注文本.text = 备注内容;
    }
    public void 显示人员(List<GroupPersonnelCrisisEventMessage> 人员列表)
    {
        if (备注文本 == null)
        {
            Debug.LogError("备注文本组件未找到");
            return;
        }
        if (人员列表 == null)
        {
            Debug.LogError("人员列表为null");
            return;
        }
        if (人员列表.Count == 0)
        {
            Debug.LogWarning("人员列表为空");
            备注文本.text = "暂无人员信息";
            return;
        }

        备注文本.text = ""; // 清空之前的内容
        foreach (var item in 人员列表)
        {
            if (item == null)
            {
                Debug.LogWarning("人员列表中存在null项");
                continue;
            }
            备注文本.text += $"姓名：{item.name}\n";
        }
    }
}
