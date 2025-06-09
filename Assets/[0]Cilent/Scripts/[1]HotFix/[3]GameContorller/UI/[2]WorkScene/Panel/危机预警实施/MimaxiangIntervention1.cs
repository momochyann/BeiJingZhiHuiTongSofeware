using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using System;
using TMPro;

/// <summary>
/// 密码箱干预第一步面板类
/// </summary>
public class MimaxiangIntervention1 : PanelBase
{
    // Start is called before the first frame update
    /// <summary>
    /// 密码箱描述输入框列表
    /// </summary>
    public List<TMP_InputField> 密码箱描述 = new List<TMP_InputField>();
    
    /// <summary>
    /// 当前步骤索引
    /// </summary>
    [SerializeField] int index;
    
    /// <summary>
    /// 面板启动时调用
    /// </summary>
    protected override void Start()
    {
        // 调用父类的Start方法
        base.Start();
    }

    /// <summary>
    /// 下一步按钮点击事件的虚拟方法重写
    /// </summary>
    protected override void 下一步按钮监听Virtual()
    {
        // 遍历所有密码箱描述输入框，检查是否有空的输入框
        foreach (var 描述 in 密码箱描述)
        {
            // 如果发现有空的输入框，直接返回，不执行后续操作
            if (描述.text == "")
            {
                return;
            }
        }
        
        // 将所有输入框的内容添加到干预系统的干预描述中
        foreach (var 描述 in 密码箱描述)
        {
            // 将当前输入框的文本内容添加到指定索引的干预描述中
            this.GetSystem<InterventionSystem>().interventionDescription[index] += 描述.text;
        }
        
        // 调用父类的下一步按钮监听方法
        base.下一步按钮监听Virtual();
    }
    
    /// <summary>
    /// 保存按钮点击事件的虚拟方法重写
    /// </summary>
    protected override void 保存按钮监听Virtual()
    {
        // 获取当前正在干预的人员信息
        var 当前人员 = this.GetSystem<InterventionSystem>().当前人员;
        
        // 设置当前干预档案的基本信息
        this.GetSystem<InterventionSystem>().当前干预档案.name = 当前人员.name; // 设置姓名
        this.GetSystem<InterventionSystem>().当前干预档案.gender = 当前人员.gender; // 设置性别
        this.GetSystem<InterventionSystem>().当前干预档案.category = 当前人员.category; // 设置类别
        this.GetSystem<InterventionSystem>().当前干预档案.isCreateReport = false; // 设置是否创建报告为false
        this.GetSystem<InterventionSystem>().当前干预档案.FangAnName = "密码箱"; // 设置方案名称为"密码箱"
        this.GetSystem<InterventionSystem>().当前干预档案.interventionDescription = ""; // 初始化干预描述为空字符串
        
        // 创建一个临时字符串来存储所有干预描述
        string interventionDescription = "";
        
        // 遍历干预系统中的所有干预描述，将它们连接成一个完整的描述
        foreach (var 描述 in this.GetSystem<InterventionSystem>().interventionDescription)
        {
            interventionDescription += 描述; // 将每个描述添加到总描述中
        }

        // 将完整的干预描述设置到当前干预档案中
        this.GetSystem<InterventionSystem>().当前干预档案.interventionDescription = interventionDescription;
        
        // 异步显示提示面板
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
    
    /// <summary>
    /// 保存干预数据的异步方法
    /// </summary>
    async void 保存干预数据()
    {
        // 设置干预结束时间为当前时间
        this.GetSystem<InterventionSystem>().当前干预档案.endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        // 设置档案创建时间为当前时间
        this.GetSystem<InterventionSystem>().当前干预档案.createDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        // 发送添加条目命令，将当前干预档案保存到个人干预档案模型中
        this.SendCommand(new AddEntryCommand(this.GetSystem<InterventionSystem>().当前干预档案, "IndividualInterventionArchiveModel"));
        
        // 异步加载危机干预实施选择界面预制体
        var 危机干预实施选择界面pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("危机干预实施选择界面");
        
        // 在父级变换下实例化危机干预实施选择界面
        var 危机干预实施选择界面 = Instantiate(危机干预实施选择界面pfb, transform.parent);
        
        // 销毁当前游戏对象
        Destroy(gameObject);
    }
}
