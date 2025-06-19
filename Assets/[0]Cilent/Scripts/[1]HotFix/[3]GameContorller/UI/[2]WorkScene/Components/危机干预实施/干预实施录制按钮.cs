using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class 干预实施录制按钮 : MonoBehaviour, IController
{
    [HideInInspector] public string 录音地址 = "";
    Button 录制按钮;
    Image 图标;
    bool 是否录制 = false;
    [SerializeField]
    Sprite[] 图标列表;
    AudioRecorderUtility audioRecorderUtility;
    void Start()
    {
        录制按钮 = GetComponent<Button>();
        图标 = GetComponent<Image>();
        录制按钮.onClick.AddListener(录制按钮点击);
        audioRecorderUtility = this.GetUtility<AudioRecorderUtility>();


    }
    private void 录制按钮点击()
    {
        是否录制 = !是否录制;
        图标.sprite = 是否录制 ? 图标列表[1] : 图标列表[0];
        if (是否录制)
        {
            audioRecorderUtility.StartRecording();
            //  audioRecorderUtility.CancelRecording();
            WorkSceneManager.Instance.加载通知("操作提示", "开始录制音频").Forget();
        }
        else
        {
            WorkSceneManager.Instance.加载确认提示("是否停止并保存录制？", "", () =>
             {
                 录音地址 = audioRecorderUtility.StopRecordingAndSave();
                 Debug.Log("录音地址: " + 录音地址);
                 //  this.GetSystem<InterventionSystem>().当前干预档案.录音地址.Add(地址);

             }).Forget(); ;
        }
    }
    void OnDestroy()
    {
        if (是否录制)
            audioRecorderUtility.StopRecordingAndSave();
    }
    // Update is called once per frame
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
    // Start is called before the first frame update

}
