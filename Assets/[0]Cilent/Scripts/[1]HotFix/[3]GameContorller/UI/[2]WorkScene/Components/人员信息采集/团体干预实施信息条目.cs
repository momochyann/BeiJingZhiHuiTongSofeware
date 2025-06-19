using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using TMPro;
using Cysharp.Threading.Tasks;

public class 团体干预实施信息条目 : MonoBehaviour, IController, IEntry
{
    // Start is called before the first frame update

    [SerializeField] TMP_Text 组别名称;
    [SerializeField] TMP_Text 主干预人员;
    [SerializeField] TMP_Text 助理人员;
    [SerializeField] Button 人员详情按钮;
    [SerializeField] TMP_Text 人员数量;
    [SerializeField] Button 备注详情按钮;
    [SerializeField] Toggle 选择;
    public bool IsChoose { get => 选择.isOn; set => 选择.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    干预实施团队 EntryRawValue;
    GameObject 备注;
    GameObject 人员;
    void Start()
    {
        InitAsync().Forget();
        选择.onValueChanged.AddListener((bool isOn) => {
            if (isOn && IsChoose)
            {
                // 获取所有条目
                EntryDisPanelNew entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
                if (entryDisPanel != null)
                {
                    IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
                    foreach (IEntry entry in entries)
                    {
                        if (!Object.ReferenceEquals(entry, this) && entry.IsChoose)
                        {
                            entry.IsChoose = false;
                        }
                    }
                }
                WorkSceneManager.Instance.加载提示("组别选择成功").Forget();
            }
            else
            {
                WorkSceneManager.Instance.加载提示("组别去除成功").Forget();
            }
        });
    }
    async UniTask InitAsync()
    {
        备注 = await this.GetModel<YooAssetPfbModel>().LoadPfb("备注");
        Debug.Log("备注加载完成");
        人员 = await this.GetModel<YooAssetPfbModel>().LoadPfb("人员");
        Debug.Log("人员加载完成");
    }
    
    public void DisEntry(int index)
    {
        // 实现接口方法
        var model = this.GetModel<干预实施Model>();
        var message = model.干预实施列表[index];
        EntryRawValue = message;
        _can2List = message;
        Debug.Log("message: " + message);
        组别名称.text = message.组别名称;
        主干预人员.text = message.主干预人员;
        助理人员.text = message.助理人员;
        人员数量.text = message.人员列表?.Count.ToString() ?? "0";

        人员详情按钮.onClick.AddListener(() =>
        {
            var 人员详情实例 = Instantiate(人员, FindObjectOfType<Canvas>().transform);
            人员详情实例.GetComponent<H_显示备注>().显示人员(message.人员列表);
            WorkSceneManager.Instance.加载提示("人员详情").Forget();
        });

        备注详情按钮.onClick.AddListener(() =>
        {
            var 备注详情实例 = Instantiate(备注, FindObjectOfType<Canvas>().transform);
            备注详情实例.GetComponent<H_显示备注>().显示备注(message.备注);
            WorkSceneManager.Instance.加载提示("备注详情").Forget();
        });
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }

    // Update is called once per frame
}
