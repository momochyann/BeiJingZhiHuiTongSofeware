using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;

public class ButtonChangeScene : MonoBehaviour, IController
{
    [SerializeField] private string 面板名称;
    [SerializeField] private Transform 生成父物体;
    [SerializeField] private GameObject 要删除的面板;

    private Button 按钮;

    void Start()
    {
        // 获取按钮组件
        按钮 = GetComponent<Button>();

        if (按钮 == null)
        {
            Debug.LogError("ButtonChangeScene: 未找到按钮组件，请添加到带有Button组件的游戏对象上");
            return;
        }

        // 添加按钮点击监听
        按钮.onClick.AddListener(() =>
        {
            加载面板().Forget();
        });

        // 如果没有指定生成父物体，默认使用Canvas
        if (生成父物体 == null)
        {
            生成父物体 = GameObject.Find("界面生成节点").transform;
        }
    }

    async UniTaskVoid 加载面板()
    {
        Debug.Log("加载面板");
        if (string.IsNullOrEmpty(面板名称))
        {
            // 如果面板名称为空，直接删除指定的面板
            if (要删除的面板 != null)
            {
                Destroy(要删除的面板);
            }
            return;
        }

        try
        {
            // 获取YooAssetPfbModel
            var model = this.GetModel<YooAssetPfbModel>();

            // 加载面板预制体
            var 面板预制体 = await model.LoadPfb(面板名称);

            if (面板预制体 == null)
            {
                Debug.LogError($"无法加载面板: {面板名称}，请确认面板名称正确且已添加到资源系统中");
                return;
            }

            // 实例化面板预制体
            Instantiate(面板预制体, 生成父物体);

            // 如果指定了要删除的面板，则删除它
            if (要删除的面板 != null)
            {
                Destroy(要删除的面板);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载面板时发生错误: {e.Message}");
        }
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }


}
