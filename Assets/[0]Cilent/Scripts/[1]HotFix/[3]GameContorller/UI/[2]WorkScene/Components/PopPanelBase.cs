using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Reflection;
using System;
using TMPro;
/// <summary>
/// 用于标记必填的TMP_InputField
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class 必填InputFieldAttribute : Attribute
{
    public string 错误提示 { get; private set; }

    public 必填InputFieldAttribute(string 错误提示 = "输入框不能为空")
    {
        this.错误提示 = 错误提示;
    }
}


public class PopPanelBase : MonoBehaviour, IController
{
    // Start is called before the first frame update
    protected GameObject 弹出页面;
    protected Button 关闭按钮;
    bool isClose = false;
    protected virtual void Awake()
    {
        弹出页面 = transform.Find("弹出页面").gameObject;
        关闭按钮 = 弹出页面.transform.Find("关闭按钮").GetComponent<Button>();
    }
     void Start()
    {
        OpenPanel();
    }
    public virtual void 编辑条目(ICan2List ICan2List)
    {

    }
    protected virtual bool 验证输入情况()
    {
        Type 类型 = this.GetType();
        FieldInfo[] 字段列表 = 类型.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var 字段 in 字段列表)
        {
            var 特性列表 = 字段.GetCustomAttributes(typeof(必填InputFieldAttribute), true);
            if (特性列表.Length > 0)
            {
                var 特性 = (必填InputFieldAttribute)特性列表[0];

                // 获取字段值
                var 字段值 = 字段.GetValue(this);
                if (字段值 is TMP_InputField inputField)
                {
                    if (string.IsNullOrEmpty(inputField.text))
                    {
                        验证失败处理(特性.错误提示);
                        return false;
                    }
                }
            }
        }

        return true;
    }
    protected virtual void 验证失败处理(string 错误提示)
    {
        WorkSceneManager.Instance.加载提示(错误提示).Forget();
    }
    protected virtual void OpenPanel()
    {
        弹出页面.transform.DOScale(1, 0.3f).From(0);
        关闭按钮.onClick.AddListener(ClosePanel);
    }
    // Update is called once per frame
    public void ClosePanel()
    {
        Debug.Log("ClosePanel");
        if (isClose) return;
        isClose = true;
        弹出页面.transform.DOScale(0, 0.1f).From(1);
        Destroy(gameObject, 0.5f);
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
