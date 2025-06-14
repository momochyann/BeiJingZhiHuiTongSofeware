using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using Michsky.MUIP;
public class 情绪放松选择界面管理 : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] List<ButtonManager> 情绪放松按钮组;

    void Start()
    {
       foreach (var item in 情绪放松按钮组)
       {
            item.onClick.AddListener(() =>
            {
                OnButtonClick(情绪放松按钮组.IndexOf(item));
            });
       }
    }

    void OnButtonClick(int index)
    {
        Debug.Log("点击了" + index);
        跳转并设置视频源(index).Forget();

    }
    async UniTaskVoid 跳转并设置视频源(int index)
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("情绪放松界面");
        var 情绪放松界面 = Instantiate(pfb, transform.parent);
        var _情绪放松播放视频场景管理 = 情绪放松界面.GetComponent<情绪放松播放视频场景管理>();
        _情绪放松播放视频场景管理.播放视频(index).Forget();
        Destroy(gameObject,0.1f);
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
