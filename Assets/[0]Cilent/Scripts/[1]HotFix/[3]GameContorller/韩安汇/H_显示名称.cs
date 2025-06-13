using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class H_显示名称 : MonoBehaviour
{
    
    void Awake()
    {
       
    }

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
        // 设置标题
        总称文本框.text = $"{单位名称值}-{软件名称值}";
    }

}
