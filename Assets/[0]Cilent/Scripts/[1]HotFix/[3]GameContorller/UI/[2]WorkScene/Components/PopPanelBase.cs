using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Reflection;
using System;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using System.Linq;
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
    protected async UniTaskVoid 加载预制体(string 提示文本内容)
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("弹出界面提示");
        Transform canvas = FindObjectsOfType<Canvas>().Where(c => c.gameObject.name == "Canvas").FirstOrDefault().transform;
        var Instance = Instantiate(pfb,canvas.transform);
        Debug.Log(Instance.name+提示文本内容);
        Instance.GetComponent<H_弹出界面提示>().显示提示(提示文本内容);
        Debug.Log("加载预制体"+提示文本内容);
        // 淡出效果会自动处理销毁，无需手动延迟销毁
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
        显示提示并自动消失(错误提示);
        Debug.Log(错误提示);
    }
    
    protected virtual void OpenPanel()
    {
        弹出页面.SetActive(true);
        关闭按钮.onClick.AddListener(ClosePanel);
    }
    public void ClosePanel()
    {
        Debug.Log("ClosePanel");
        if (isClose) return;
        isClose = true;
        弹出页面.SetActive(false);
        Destroy(gameObject);
    }
    /// <summary>
    /// 显示提示并在3秒后自动消失
    /// </summary>
    /// <param name="提示文本内容">要显示的提示内容</param>
    public void 显示提示并自动消失(string 提示文本内容)
    {
        加载预制体(提示文本内容).Forget();
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
