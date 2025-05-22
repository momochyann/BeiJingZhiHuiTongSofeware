using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public enum 提示移动方向
{
    经,
    纬
}
[Serializable]
public class 切换栏目
{
    public GameObject panelObj;
    public Button button;
    public string panelName;
}
public class WorkScenePanelSelect : MonoBehaviour, IController
{
    public bool 是否删除其他界面 = false;
    public List<切换栏目> 栏目列表;
    public Image selectImageBack;
    public GameObject 父节点;
    public string 界面生成节点名称 = "界面生成节点";
    public 提示移动方向 移动方向 = 提示移动方向.经;
    [HideInInspector] public UnityAction<int> 触发动画;
    // Start is called before the first frame update
    void Start()
    {
        Init().Forget();
    }
    async protected virtual UniTaskVoid Init()
    {
        foreach (var item in 栏目列表)
        {
            item.button.onClick.AddListener(OnButtonClick);
            if (!string.IsNullOrEmpty(item.panelName))
            {
                item.panelObj = await this.GetModel<YooAssetPfbModel>().LoadPfb(item.panelName);
            }
        }
        if(父节点 == null)
        {
            父节点 = GameObject.Find(界面生成节点名称);
        }
    }

    
    public void LoadPanelByIndex(int index)
    {
        if (index < 0 || index >= 栏目列表.Count)
        {
            Debug.LogWarning("索引超出栏目列表范围");
            return;
        }
        
        切换栏目 选中栏目 = 栏目列表[index];
        List<GameObject> 已显示面板 = new List<GameObject>();
      //  Debug.Log("LoadPanelByIndex: " + index);
        // 查找并收集所有已显示的面板
        foreach (var item in 栏目列表)
        {
            if (string.IsNullOrEmpty(item.panelName))
            {
                continue;
            }
            var 查至面板1 = 父节点.transform.Find(item.panelName + "(Clone)");
            var 查至面板2 = 父节点.transform.Find(item.panelName);

            if (查至面板1 != null)
            {
                已显示面板.Add(查至面板1.gameObject);
            }
            if (查至面板2 != null)
            {
                已显示面板.Add(查至面板2.gameObject);
            }
        }
        
        // 销毁已显示的面板
        if (已显示面板.Count > 0)
        {
            foreach (var item in 已显示面板)
            {
                Destroy(item);
            }
        }
        
        // 如果需要删除其他界面
        if (是否删除其他界面)
        {
            // 查找名为"界面生成节点"的游戏对象
            GameObject 界面生成节点 = GameObject.Find("界面生成节点");
            if (界面生成节点 != null)
            {
                // 删除该节点下的所有子物体
                for (int i = 界面生成节点.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(界面生成节点.transform.GetChild(i).gameObject);
                }
             //   Debug.Log("已清空界面生成节点物体下的所有子物体");
            }
            else
            {
                Debug.LogWarning("未找到名为'界面生成节点'的游戏对象");
            }
        }
        
        // 实例化选中栏目的面板
        if (选中栏目.panelObj != null)
        {
            Instantiate(选中栏目.panelObj, 父节点.transform);
        }
        
        // 触发动画效果
        Animation(index).Forget();
    }
    
    private void OnButtonClick()
    {
   //     Debug.Log("OnButtonClick");
        var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        切换栏目 选中栏目 = null;
        List<GameObject> 已显示面板 = new List<GameObject>();
      
        foreach (var item in 栏目列表)
        {
            if (item.button == button)
            {
                选中栏目 = item;
            }
            if (string.IsNullOrEmpty(item.panelName))
            {
                continue;
            }
            var 查至面板1 = 父节点.transform.Find(item.panelName + "(Clone)");
            var 查至面板2 = 父节点.transform.Find(item.panelName);

            if (查至面板1 != null)
            {
                已显示面板.Add(查至面板1.gameObject);
            }
            if (查至面板2 != null)
            {
                已显示面板.Add(查至面板2.gameObject);
            }
        }
        if (已显示面板.Count > 0)
        {
            foreach (var item in 已显示面板)
            {
                Destroy(item);
            }
        }
        if (选中栏目 != null)
        {
            if (选中栏目.panelObj != null)
            {
                if (是否删除其他界面)
                {
                    // 查找名为"界面生成节点物体"的游戏对象
                    GameObject 界面生成节点 = GameObject.Find("界面生成节点");
                    if (界面生成节点 != null)
                    {
                        // 删除该节点下的所有子物体
                        for (int i = 界面生成节点.transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(界面生成节点.transform.GetChild(i).gameObject);
                        }
               //         Debug.Log("已清空界面生成节点物体下的所有子物体");
                    }
                    else
                    {
                        Debug.LogWarning("未找到名为'界面生成节点物体'的游戏对象");
                    }
                }
              
                Instantiate(选中栏目.panelObj, 父节点.transform);
            }
            Animation(栏目列表.IndexOf(选中栏目)).Forget();
        }
    }
    
    async protected virtual UniTaskVoid Animation(int index)
    {
        await UniTask.Yield();
        if (移动方向 == 提示移动方向.纬)
        {
            selectImageBack.transform.DOMoveX(栏目列表[index].button.transform.position.x, 0.3f);
            for (int i = 0; i < 栏目列表.Count; i++)
            {
                if (i == index)
                {
                    栏目列表[i].button.GetComponentInChildren<Text>().DOColor(Color.white, 0.3f);
                }
                else
                {
                    栏目列表[i].button.GetComponentInChildren<Text>().DOColor(Color.black, 0.3f);
                }
            }
        }
        else if (移动方向 == 提示移动方向.经)
        {
            selectImageBack.transform.DOMoveY(栏目列表[index].button.transform.position.y, 0.3f);

        }
        触发动画?.Invoke(index);
    }
    // Update is called once per frame
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
