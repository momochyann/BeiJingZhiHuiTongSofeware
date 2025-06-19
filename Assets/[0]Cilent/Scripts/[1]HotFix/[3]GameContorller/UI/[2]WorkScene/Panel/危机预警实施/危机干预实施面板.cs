using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using System;
using TMPro;
using Michsky.MUIP;
public interface I干预实施咨询接口
{
    public List<干预实施咨询组件> 干预实施咨询组件列表 { get; set; }
    public MonoBehaviour 干预实施实例 { get; }
}

/// <summary>
/// 密码箱干预第一步面板类
/// </summary>
public class 危机干预实施面板 : PanelBase, I干预实施咨询接口
{
    [SerializeField] string 干预名称;
    [SerializeField] ButtonManager 取消实施返回按钮;
    // Start is called before the first frame update
    /// <summary>
    /// 密码箱描述问题
    /// </summary>
    public List<TMP_Text> 密码箱描述问题 = new List<TMP_Text>();

    /// <summary>
    /// 当前步骤索引
    /// </summary>
    [SerializeField] int index;


    public List<干预实施咨询组件> 干预实施咨询组件列表 { get; set; }
    public MonoBehaviour 干预实施实例
    {
        get => this;
    }

    /// <summary>
    /// 面板启动时调用
    /// </summary>
    protected override void Start()
    {
        // 调用父类的Start方法
        base.Start();
        初始化问答列表组件();
        if(取消实施返回按钮 != null)
        {
            取消实施返回按钮.onClick.AddListener(取消按钮监听Virtual);
        }
        WorkSceneManager.Instance.干预实施咨询面板队列.Push(this);
        foreach (var item in WorkSceneManager.Instance.干预实施咨询面板队列)
        {
            Debug.Log(item.干预实施实例.gameObject.name);
            foreach (var item1 in item.干预实施咨询组件列表)
            {
                Debug.Log(item1.问题文本框.text);
                Debug.Log(item1.录音按钮.录音地址);
            }
        }
       
    }
    void 初始化问答列表组件()
    {
        干预实施咨询组件列表 = new List<干预实施咨询组件>();
        foreach (var 问题 in 密码箱描述问题)
        {
            干预实施咨询组件 问答 = new 干预实施咨询组件();
            问答.问题文本框 = 问题;
            问答.回答输入框 = 问题.GetComponentInChildren<TMP_InputField>();
            问答.录音按钮 = 问题.GetComponentInChildren<干预实施录制按钮>();
            干预实施咨询组件列表.Add(问答);
        }
    }
    /// <summary>
    /// 下一步按钮点击事件的虚拟方法重写
    /// </summary>
    protected override void 下一步按钮监听Virtual()
    {
        // 遍历所有密码箱描述输入框，检查是否有空的输入框
        foreach (var 描述 in 干预实施咨询组件列表)
        {
            // 如果发现有空的输入框，直接返回，不执行后续操作
            if (string.IsNullOrEmpty(描述.回答输入框.text) && string.IsNullOrEmpty(描述.录音按钮.录音地址))
            {
                WorkSceneManager.Instance.加载提示($"问题{干预实施咨询组件列表.IndexOf(描述)+1}还没有描述").Forget();
                return;
            }
        }

        // 将所有输入框的内容添加到干预系统的干预描述中

        // 调用父类的下一步按钮监听方法
        Instantiate(下个面板pfb, transform.parent);
    }

    /// <summary>
    /// 保存按钮点击事件的虚拟方法重写
    /// </summary>
    protected override void 保存按钮监听Virtual()
    {
        显示提示面板().Forget();
    }

    /// <summary>
    /// 异步显示确认提示面板
    /// </summary>
    async UniTaskVoid 显示提示面板()
    {
        // 异步加载确认提示弹窗预制体
        var 提示面板pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("确认提示弹窗");

        // 在Canvas下实例化提示面板
        var P_TipPanel = Instantiate(提示面板pfb, FindObjectOfType<Canvas>().transform);

        // 设置提示面板显示的文本内容
        P_TipPanel.GetComponent<P_TipPanel>().显示面板("是否保存干预数据");

        // 为提示面板的确认按钮添加回调方法
        P_TipPanel.GetComponent<P_TipPanel>().确认回调 += 保存干预数据;
    }
    protected override void 返回按钮监听Virtual()
    {
        WorkSceneManager.Instance.干预实施咨询面板队列.Pop();
        Destroy(gameObject);
    }
    protected override void 取消按钮监听Virtual()
    {
        WorkSceneManager.Instance.清除干预面板();
    }
    /// <summary>
    /// 保存干预数据的异步方法
    /// </summary>
    void 保存干预数据()
    {
        WorkSceneManager.Instance.添加干预是实施数据(干预名称).Forget();
    }
}
