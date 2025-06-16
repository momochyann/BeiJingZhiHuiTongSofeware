using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class WorkSceneManager : MonoSingleton<WorkSceneManager>, IController
{
    // Start is called before the first frame update
    WorkScenePanelSelect workScenePanelSelect;
    public string 干预者;
    public GameObject 界面生成节点;
    [SerializeField] string[] 界面名称;
    void Start()
    {
        界面生成节点 = GameObject.Find("界面生成节点");
        LoadPanel().Forget();
        var 当前干预人员名称 = PlayerPrefs.GetString("当前干预人员")        ;
        if (!string.IsNullOrEmpty(当前干预人员名称))
        {
            干预者 = 当前干预人员名称;
        }
        Debug.Log("WorkSceneManager Start");
    }

    // Update is called once per frame
    async UniTaskVoid LoadPanel()
    {
        var index = this.GetSystem<WorkSceneSystem>().WorkSceneIndex;
        Debug.Log("index: " + index);
        if (index <= 0)
            index = 1;
        加载界面(界面名称[index - 1]).Forget();
        //await UniTask.Delay(500);

        // workScenePanelSelect = FindObjectOfType<WorkScenePanelSelect>();
        // Debug.Log("workScenePanelSelect: " + workScenePanelSelect.gameObject.name);
        // workScenePanelSelect.LoadPanelByIndex(index - 1);
    }
    async public UniTaskVoid 跳转界面(string 跳转界面名称, GameObject 本来界面 = null)
    {
        // if (本来界面 == null)
        //     本来界面 = this.GetView<WorkScenePanelSelect>().gameObject;
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb(跳转界面名称);
        Instantiate(pfb, 界面生成节点.transform);
        if (本来界面 != null)
        {
            Destroy(本来界面);
        }
    }
    async public UniTaskVoid 加载界面(string 界面名称)
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb(界面名称);
        Instantiate(pfb, 界面生成节点.transform);
    }
    async public UniTaskVoid 加载通知(string 标题, string 内容)
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("通知");
        var 通知 = Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<通知控制>();
        await UniTask.Delay(10);
        通知.播送通知(标题, 内容);
    }

    async public UniTaskVoid 加载提示(string 提示文本内容)
    {
        try
        {
            Debug.Log($"开始加载提示: {提示文本内容}");
            var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("通知");
            if (pfb == null)
            {
                Debug.LogError("通知预制体加载失败");
                return;
            }
            Debug.Log("通知预制体加载成功");
            
            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("未找到Canvas");
                return;
            }
            
            var 通知 = Instantiate(pfb, canvas.transform).GetComponent<通知控制>();
            if (通知 == null)
            {
                Debug.LogError("未找到通知控制组件");
                return;
            }
            
            await UniTask.Delay(10);
            通知.播送通知("操作提示", 提示文本内容);
            Debug.Log($"提示已显示: {提示文本内容}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载提示时发生错误: {e.Message}\n{e.StackTrace}");
        }
    }
    async public UniTaskVoid 加载确认提示(string 提示文本内容, string 跳转面板名称, UnityAction 确认回调)
    {
        if (FindObjectOfType<P_TipPanel>() != null)
            return;
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("确认提示弹窗");
        var 确认提示弹窗 = Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<P_TipPanel>();
        确认提示弹窗.显示面板(提示文本内容, 跳转面板名称, 确认回调);
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
