using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UI.ProceduralImage;
using Cysharp.Threading.Tasks;
public class 干预实施查看面板 : PopPanelBase
{
    // Start is called before the first frame update
    [SerializeField] Button 播放录音按钮;
    [SerializeField] TMP_Text 干预实施问题;
    [SerializeField] TMP_Text 干预实施回答;
    bool 是否播放录音 = false;
    void Start()
    {
        OpenPanel();
    }

    protected override void OpenPanel()
    {
        base.OpenPanel();
    }
    public void 设置干预实施问题(IndividualInterventionArchive 个人干预实施档案, int 问题索引)
    {
        干预实施问题.text = 个人干预实施档案.干预实施咨询问答列表[问题索引].问题;
        干预实施回答.text = 个人干预实施档案.干预实施咨询问答列表[问题索引].文字回答;
        if (!string.IsNullOrEmpty(个人干预实施档案.干预实施咨询问答列表[问题索引].录音地址))
        {
            播放录音按钮.onClick.AddListener(() => 播放录音(个人干预实施档案.干预实施咨询问答列表[问题索引].录音地址).Forget());
        }
        else
        {
            录音按钮失效样式();
        }
    }
    void 录音按钮失效样式()
    {
        播放录音按钮.GetComponent<ProceduralImage>().BorderWidth = 1;
        播放录音按钮.GetComponentInChildren<TMP_Text>().color = Color.gray;
        播放录音按钮.GetComponentInChildren<TMP_Text>().text = "暂无录音";
    }
    async UniTaskVoid 播放录音(string 录音地址)
    {
        if (是否播放录音)
            return;
        是否播放录音 = true;
        var 音频 = await this.GetUtility<AudioRecorderUtility>().LoadAudioClipAsync(录音地址);
        AudioKit.PlaySound(音频);
    }
}
