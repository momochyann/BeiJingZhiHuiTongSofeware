using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;
using Michsky.MUIP;


public class P添加部门界面1 : PopPanelBase
{
    [SerializeField] CustomDropdown 单位选择下拉框;
    [SerializeField] GameObject 单位名称输入框预制体;
    
    ICan2List 旧数据;
    private TMP_InputField 单位名称输入框;
    private GameObject 单位名称输入框实例;
    private TextMeshProUGUI 单位名称文本;
    private string 当前输入的单位名称 = "";

    async UniTask InitAsync()
    {
        单位名称输入框预制体 = await LoadYooAssetsTool.LoadAsset<GameObject>("单位名称_Mobile");
    }
    void Start()
    {
        InitAsync().Forget();
    }
    protected override void Awake()
    {
        base.Awake();
        
        // 检查弹出页面是否存在
        if (弹出页面 == null)
        {
            Debug.LogError("弹出页面未找到");
            return;
        }

        // 检查并获取单位选择下拉框
        var 下拉框对象 = 弹出页面.transform.Find("单位选择栏/下拉选择框");
        if (下拉框对象 == null)
        {
            Debug.LogError("找不到单位选择下拉框");
            return;
        }
        单位选择下拉框 = 下拉框对象.GetComponent<CustomDropdown>();
        if (单位选择下拉框 == null)
        {
            Debug.LogError("单位选择下拉框上没有CustomDropdown组件");
            return;
        }

        // 检查并获取保存按钮
        var 保存按钮对象 = 弹出页面.transform.Find("保存按钮");
        if (保存按钮对象 == null)
        {
            Debug.LogError("找不到保存按钮");
            return;
        }
        var 保存按钮 = 保存按钮对象.GetComponent<Button>();
        if (保存按钮 == null)
        {
            Debug.LogError("保存按钮上没有Button组件");
            return;
        }
        保存按钮.onClick.AddListener(下一个界面按钮监听);
        
        // 初始化下拉框
        初始化下拉框();
        
        // 设置下拉框选择事件
        单位选择下拉框.onValueChanged.AddListener(下拉框选择事件);
        
        OpenPanel();
    }

    void 初始化下拉框()
    {
        var 部门模型 = this.GetModel<部门数据Model>();
        var 单位列表 = 部门模型.部门列表.Select(d => d.部门名称).ToList();
        
        // 清空现有选项
        单位选择下拉框.items.Clear();
        
        // 添加部门选项
        foreach (var 单位 in 单位列表)
        {
            单位选择下拉框.CreateNewItem(单位);
        }
        
        // 添加"新建单位"选项
        单位选择下拉框.CreateNewItem("新建单位");
        
        // 设置默认选项
        if (单位选择下拉框.items.Count > 0)
        {
            单位选择下拉框.selectedItemIndex = 0;
            单位选择下拉框.SetupDropdown();
        }
    }

    async void 下拉框选择事件(int index)
    {
        // 检查下拉框是否为空
        if (单位选择下拉框 == null || 单位选择下拉框.selectedText == null)
        {
            Debug.LogError("下拉框或选中文本为空");
            return;
        }

        // 如果选择了"新建单位"
        if (单位选择下拉框.selectedText.text == "新建单位")
        {
            if (单位名称输入框实例 == null)
            {
                try
                {
                    // 使用YooAsset加载预制体
                    var 输入框预制体 = await LoadYooAssetsTool.LoadAsset<GameObject>("单位名称_Mobile");
                    if (输入框预制体 == null)
                    {
                        Debug.LogError("找不到单位名称输入框预制体");
                        return;
                    }
                    
                    单位名称输入框实例 = Instantiate(输入框预制体, 弹出页面.transform);
                    if (单位名称输入框实例 == null)
                    {
                        Debug.LogError("实例化输入框预制体失败");
                        return;
                    }

                    // 获取TextMeshPro组件
                    var 标题对象 = 单位名称输入框实例.transform.Find("单位名称");
                    if (标题对象 == null)
                    {
                        Debug.LogError("找不到标题对象");
                        return;
                    }

                    单位名称文本 = 标题对象.GetComponent<TextMeshProUGUI>();
                    if (单位名称文本 == null)
                    {
                        Debug.LogError("找不到TextMeshPro组件");
                        return;
                    }

                    // 设置文本属性
                    单位名称文本.color = Color.black;
                    单位名称文本.text = "请输入单位名称";

                    // 添加输入事件监听
                    var inputField = 单位名称输入框实例.AddComponent<TMP_InputField>();
                    inputField.textComponent = 单位名称文本;
                    inputField.onValueChanged.AddListener(On单位名称输入);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"创建输入框时发生错误: {e.Message}");
                    return;
                }
            }
        }
        else
        {
            if (单位名称输入框实例 != null)
            {
                Destroy(单位名称输入框实例);
                单位名称输入框实例 = null;
                单位名称文本 = null;
                当前输入的单位名称 = "";
            }
        }
    }

    private void On单位名称输入(string value)
    {
        当前输入的单位名称 = value;
        if (单位名称文本 != null)
        {
            单位名称文本.text = value;
        }
        Debug.Log($"输入的单位名称: {当前输入的单位名称}");
    }

    async void 下一个界面按钮监听()
    {
        if (!验证输入情况())
        {
            return;
        }
        
        // 加载增加部门面板预制体
        var 增加部门面板 = await LoadYooAssetsTool.LoadAsset<GameObject>("增加部门面板_Mobile");
        if (增加部门面板 != null)
        {
            var 面板实例 = Instantiate(增加部门面板, FindObjectOfType<Canvas>().transform);
            var 界面2 = 面板实例.GetComponent<P添加部门界面2>();
            if (界面2 != null)
            {
                string 要传递的单位名称;
                // 如果选择了"新建单位"，使用新输入的单位名称
                if (单位选择下拉框.selectedText.text == "新建单位")
                {
                    要传递的单位名称 = 当前输入的单位名称;
                }
                else
                {
                    // 如果选择了已有单位，使用选中的单位名称
                    要传递的单位名称 = 单位选择下拉框.selectedText.text;
                }

                // 确保单位名称不为空
                if (string.IsNullOrEmpty(要传递的单位名称))
                {
                    WorkSceneManager.Instance.加载通知("提示", "请选择或输入单位名称").Forget();
                    return;
                }

                界面2.设置单位名称(要传递的单位名称);
                ClosePanel();
            }
            else
            {
                Debug.LogError("找不到P添加部门界面2组件");
            }
        }
        else
        {
            Debug.LogError("找不到增加部门面板预制体");
        }
    }

    protected override bool 验证输入情况()
    {
        if (!base.验证输入情况())
        {
            return false;
        }

        // 如果选择了"新建单位"，需要验证单位名称输入
        if (单位选择下拉框.selectedText.text == "新建单位")
        {
            if (string.IsNullOrEmpty(当前输入的单位名称))
            {
                WorkSceneManager.Instance.加载通知("提示", "请输入单位名称").Forget();
                return false;
            }
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
        
        旧数据 = ICan2List;
    }
}
