using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;
using Michsky.MUIP;

public class P添加部门界面2 : PopPanelBase
{
    [必填InputField("部门名称不能为空")]
    TMP_InputField 部门名称输入框;
    TMP_InputField 部门描述输入框;
    
    ICan2List 旧数据;
    private string 单位名称;

    public void 设置单位名称(string 名称)
    {
        单位名称 = 名称;
    }

    protected override void Awake()
    {
        base.Awake();
        
        // 确保弹出页面存在
        if (弹出页面 == null)
        {
            Debug.LogError("弹出页面未找到");
            return;
        }

        try
        {
            // 获取并验证所有组件
            var 部门名称栏 = 弹出页面.transform.Find("部门名称栏");
            if (部门名称栏 == null)
            {
                Debug.LogError("找不到部门名称栏");
                return;
            }

            var 部门描述栏 = 弹出页面.transform.Find("部门描述栏");
            if (部门描述栏 == null)
            {
                Debug.LogError("找不到部门描述栏");
                return;
            }

            部门名称输入框 = 部门名称栏.Find("输入框")?.GetComponent<TMP_InputField>();
            部门描述输入框 = 部门描述栏.Find("输入框")?.GetComponent<TMP_InputField>();
            var 保存按钮 = 弹出页面.transform.Find("保存按钮")?.GetComponent<Button>();

            if (部门名称输入框 == null)
            {
                Debug.LogError("找不到部门名称输入框");
                return;
            }
            if (部门描述输入框 == null)
            {
                Debug.LogError("找不到部门描述输入框");
                return;
            }
            if (保存按钮 == null)
            {
                Debug.LogError("找不到保存按钮");
                return;
            }

            // 设置输入框光标颜色为黑色
            部门名称输入框.caretColor = Color.black;
            部门描述输入框.caretColor = Color.black;

            // 添加按钮监听
            保存按钮.onClick.AddListener(保存数据按钮监听);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"初始化组件时发生错误: {e.Message}");
            return;
        }

        OpenPanel();
    }

    void 保存数据按钮监听()
    {
        if (!验证输入情况())
        {
            return;
        }
        
        部门 新部门 = new 部门();
        新部门.部门名称 = 部门名称输入框.text;
        新部门.部门描述 = 部门描述输入框.text;
        新部门.单位名称 = 单位名称;  // 使用正确的属性名
        
        if (旧数据 != null)
        {
            // 编辑模式，保持原有编号
            var 原部门 = 旧数据 as 部门;
            新部门.部门编号 = 原部门.部门编号;
            this.SendCommand(new EditEntryCommand(旧数据, 新部门 as ICan2List, "部门数据Model"));
        }
        else
        {
            // 新增模式，自动生成编号
            新部门.部门编号 = 生成部门编号();
            this.SendCommand(new AddEntryCommand(新部门 as ICan2List, "部门数据Model"));
            Debug.Log("添加成功");
        }
        ClosePanel();
    }

    protected override bool 验证输入情况()
    {
        if (!base.验证输入情况())
        {
            return false;
        }
        return true;
    }
    
    string 生成部门编号()
    {
        var 部门模型 = this.GetModel<部门数据Model>();
        int 下一个编号 = 部门模型.部门列表.Count + 1;
        
        // 确保编号唯一性，如果存在重复则递增
        string 新编号;
        do
        {
            新编号 = $"DEPT{下一个编号:D3}"; // 格式：DEPT001, DEPT002...
            下一个编号++;
        }
        while (部门模型.根据编号查找部门(新编号) != null);
        
        return 新编号;
    }
    
    public override void 编辑条目(ICan2List ICan2List)
    {
        var 部门信息 = ICan2List as 部门;
        Debug.Log("编辑部门条目: " + 部门信息.部门名称);
        
        部门名称输入框.text = 部门信息.部门名称;
        部门描述输入框.text = 部门信息.部门描述;
        
        旧数据 = ICan2List;
    }

    // Start is called before the first frame update
  
}
