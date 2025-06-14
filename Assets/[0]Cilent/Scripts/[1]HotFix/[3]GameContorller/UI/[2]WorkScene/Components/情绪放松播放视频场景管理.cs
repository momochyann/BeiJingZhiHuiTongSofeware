using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine.Video;
using TMPro;
public class 情绪放松播放视频场景管理 : PanelBase
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] TMP_Text 简介文本框;
    // 使用英文文件名，避免Android构建时的编码问题
    string[] 简介 ={
    "放松训练是指身体和精神由紧张状态朝向松弛状态的过程。放松主要是消除肌肉的紧张。呼吸放松训练通过对呼吸的调控使肌肉放松下来，从而达到放松情绪的效果。当压力事件出现时，紧张不断积累，压力体验逐渐增强。此刻，持续几分钟的完全放松比一小时睡眠效果更好。"
    ,"想象放松法主要通过唤起宁静、轻松、舒适情景的想象和体验，来减少紧张、焦虑，控制唤醒水平，引发注意集中的状态，增强内心的愉悦感和自信心。"
    ,"放松训练对于应付紧张、焦虑、不安、气愤的情绪与情境非常有用，可以帮助人们振作精神，恢复体力，消除疲劳，镇定情绪。这与中国的气功、太极拳、站桩功、坐禅等很相似，有助于全身肌肉放松，造成自我抑制状态，促进血液循环，平稳呼吸，增强个体应付紧张事件的能力。"
    ,"缓慢的腹式呼吸，可以调节自主神经，进一步调节清醒、焦虑的唤起机制，从而让头脑和身体感到放松。缓慢的腹式呼吸可以使肺部有充足的时间做气体交换，吸入的氧量高于正常情况下的两到三倍，使身体获得更多的氧气。"
    ,"请你在一个舒适的位置上坐好，姿势摆正，将右手的食指和中指放在前额上，用大拇指按压住右鼻孔，然后用左鼻孔缓慢地轻轻吸气，再用无名指按压住左鼻孔，同时将大拇指移开打开右鼻孔，由右鼻孔缓慢地尽量彻底地将气体呼出，再用右鼻孔吸气，用大拇指按压住右鼻孔，同时打开无名指，再用左鼻孔呼气。"
   };
    // 对应的中文显示名称（如果需要在UI中显示）
    string[] videoDisplayNames = { "呼吸放松法", "想象放松法", "肌肉放松法", "腹式放松法", "鼻腔放松法" };
    // Start is called before the first frame update
    protected override void Start()
    {
        //base.OnInit();
        base.Start();

        // 如果没有手动指定VideoPlayer，尝试从当前GameObject或子对象中查找
        if (videoPlayer == null)
        {
            videoPlayer = GetComponentInChildren<VideoPlayer>();
        }
        //播放视频(0).Forget();
    }
    public async UniTaskVoid 播放视频(int index)
    {
        简介文本框.text = 简介[index];
        await UniTask.Delay(500);

        // 构建视频文件名，假设视频文件命名为 video1.mp4, video2.mp4 等
        // string videoFileName = $"{videoFileNames[index]}.mp4";

        // 获取StreamingAssets中视频文件的路径
        //  string videoPath = GetStreamingAssetsPath(videoFileName);
        var video = await this.GetModel<YooAssetPfbModel>().LoadConfig<VideoClip>(videoDisplayNames[index], true);
        if (video == null)
        {
            return;
        }
        if (videoPlayer != null)
        {
            // 设置视频源为URL
            // videoPlayer.source = UnityEngine.Video.VideoSource.Url;
            // videoPlayer.url = videoPath;
            videoPlayer.source = UnityEngine.Video.VideoSource.VideoClip;
            videoPlayer.clip = video;
            // 准备视频
            videoPlayer.Prepare();

            // 等待视频准备完成
            while (!videoPlayer.isPrepared)
            {
                await UniTask.Yield();
            }

            // 播放视频
            videoPlayer.Play();

            //  Debug.Log($"正在播放视频: {videoPath}");
        }
        else
        {
            Debug.LogError("VideoPlayer组件未找到！");
        }
    }

    /// <summary>
    /// 获取StreamingAssets中文件的完整路径，支持不同平台
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <returns>完整的文件路径</returns>
    private string GetStreamingAssetsPath(string fileName)
    {
        string path = "";

#if UNITY_ANDROID && !UNITY_EDITOR
        // Android平台
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        // PC平台和编辑器
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#else
        // 其他平台
        path = Path.Combine(Application.streamingAssetsPath, fileName);
#endif

        // 对于本地文件，需要添加 file:// 前缀
        if (!path.StartsWith("jar:") && !path.StartsWith("http"))
        {
            path = "file://" + path;
        }

        return path;
    }
    // Update is called once per frame
    void Update()
    {

    }
}

