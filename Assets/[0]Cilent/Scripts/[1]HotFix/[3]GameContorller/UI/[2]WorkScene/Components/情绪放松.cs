using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.IO;
using UnityEngine.Video;

public class 情绪放松 : PanelBase
{
    [SerializeField] private VideoPlayer videoPlayer;
    // 使用英文文件名，避免Android构建时的编码问题
    string[] videoFileNames = {"breathing_relaxation", "imagination_relaxation", "muscle_relaxation", "abdominal_relaxation", "nasal_relaxation"};
    // 对应的中文显示名称（如果需要在UI中显示）
    string[] videoDisplayNames = {"呼吸放松法", "想象放松法", "肌肉放松法", "腹式放松法", "鼻腔放松法"};
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
        播放视频(0).Forget();
    }
    public async UniTaskVoid 播放视频(int index)
    {
        await UniTask.Delay(500);
        
        // 构建视频文件名，假设视频文件命名为 video1.mp4, video2.mp4 等
       // string videoFileName = $"{videoFileNames[index]}.mp4";
        
        // 获取StreamingAssets中视频文件的路径
      //  string videoPath = GetStreamingAssetsPath(videoFileName);
        var video = await this.GetModel<YooAssetPfbModel>().LoadConfig<VideoClip>(videoFileNames[index],true);

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

