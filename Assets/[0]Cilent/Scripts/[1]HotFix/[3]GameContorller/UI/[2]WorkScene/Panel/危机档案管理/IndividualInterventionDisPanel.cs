using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Michsky.MUIP;
using System;
public class IndividualInterventionDisPanel : MonoBehaviour, IController
{

    [SerializeField]
    Text nameText;
    [SerializeField]
    Text 性别框;
    [SerializeField]
    Text 部门框;
    [SerializeField]
    Text dateText;
    [SerializeField]
    Button 导出按钮;
    [SerializeField]
    Text 干预人员文本框;
    [SerializeField]
    Text 干预开始时间;
    [SerializeField]
    Text 干预结束时间;
    [SerializeField]
    CustomDropdown 干预实施详情选择框;
    [SerializeField]
    ButtonManager 干预实施详情查看按钮;
    IndividualInterventionArchive EntryRawValue;
    void Start()
    {

    }
    public void OpenPanel(IndividualInterventionArchive EntryRawValue)
    {
        this.EntryRawValue = EntryRawValue;
        填入数据();
    }
    void 填入数据()
    {
        if (EntryRawValue != null)
        {
            // 姓名
            if (nameText != null)
            {
                nameText.text = EntryRawValue.name;
            }

            // 性别
            if (性别框 != null)
            {
                性别框.text = EntryRawValue.gender;
            }

            // 部门
            if (部门框 != null)
            {
                部门框.text = EntryRawValue.category;
            }

            // 日期
            if (dateText != null)
            {
                dateText.text = EntryRawValue.createDate;
            }
            if (干预开始时间 != null)
            {
                干预开始时间.text = EntryRawValue.startDate;
            }
            if (干预结束时间 != null)
            {
                干预结束时间.text = EntryRawValue.endDate;
            }
            // 干预人员
            if (干预人员文本框 != null && EntryRawValue.Interveners != null)
            {
                干预人员文本框.text = EntryRawValue.Interveners;
            }
            if (干预实施详情选择框 != null)
            {
                添加咨询详情选项();
            }
            if (干预实施详情查看按钮 != null)
            {
                干预实施详情查看按钮.onClick.AddListener(查看干预实施详情);
            }
        }
    }

    async private void 查看干预实施详情()
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("干预实施查看面板");
        var 干预实施详情面板 = Instantiate(pfb, transform.parent);
        干预实施详情面板.GetComponent<干预实施查看面板>().设置干预实施问题(EntryRawValue, 干预实施详情选择框.selectedItemIndex);
    }

    void 添加咨询详情选项()
    {
        // 清空现有选项
        干预实施详情选择框.items.Clear();
        foreach (var 咨询详情 in EntryRawValue.干预实施咨询问答列表)
        {
            干预实施详情选择框.CreateNewItem(咨询详情.问题.Substring(咨询详情.问题.IndexOf("*") + 1));
        }
        干预实施详情选择框.SetupDropdown();
    }

    // Update is called once per frame
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
    // 启用输入框自动换行
    void 启用自动换行(InputField 输入框)
    {
        // 获取Text组件
        Text 文本组件 = 输入框.textComponent;
        if (文本组件 != null)
        {
            // 设置自动换行
            文本组件.horizontalOverflow = HorizontalWrapMode.Wrap;
            文本组件.verticalOverflow = VerticalWrapMode.Truncate;

            // 如果需要，可以调整行距
            文本组件.lineSpacing = 1.2f;
        }

        // 调整输入框的内容类型为多行文本
        输入框.lineType = InputField.LineType.MultiLineSubmit;

        // 可选：调整输入框大小以适应多行文本
        RectTransform rectTransform = 输入框.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 根据需要调整高度
            float 最小高度 = 60f; // 根据实际需求调整
            if (rectTransform.sizeDelta.y < 最小高度)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 最小高度);
            }
        }
    }

    // 将评价项目字典转换为格式化字符串
    string 格式化评价项目(Dictionary<string, string> 评价项目)
    {
        if (评价项目 == null || 评价项目.Count == 0)
            return string.Empty;

        List<string> 格式化项目 = new List<string>();

        foreach (var 项目 in 评价项目)
        {
            // 移除值中的所有空格
            string 处理后值 = 项目.Value.Replace(" ", "");

            // 添加格式化的项目
            格式化项目.Add($"{项目.Key}:{处理后值}");
        }

        // 用" - "连接所有项目
        return string.Join(" | ", 格式化项目);
    }
}
