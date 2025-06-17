using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class H_显示名称 : MonoBehaviour
{
    

    public TMP_Text 总称文本框;

    void Start()
    {
        // 获取PlayerPrefs中的值
        setname();
    }
    
    public void setname()
    {
        string 单位名称值 = PlayerPrefs.GetString("单位名称", "");
        string 软件名称值 = PlayerPrefs.GetString("软件名称", "");
        
        // 如果两个值都为空，显示默认文本
        if (string.IsNullOrEmpty(单位名称值) && string.IsNullOrEmpty(软件名称值))
        {
            总称文本框.text = "未设置名称";
            return;
        }
        
        // 如果其中一个为空，只显示另一个
        if (string.IsNullOrEmpty(单位名称值))
        {
            总称文本框.text = 软件名称值;
            return;
        }
        
        if (string.IsNullOrEmpty(软件名称值))
        {
            总称文本框.text = 单位名称值;
            return;
        }
        
        // 两个值都不为空，显示组合名称
        总称文本框.text = $"{单位名称值}-{软件名称值}";
    }

}
