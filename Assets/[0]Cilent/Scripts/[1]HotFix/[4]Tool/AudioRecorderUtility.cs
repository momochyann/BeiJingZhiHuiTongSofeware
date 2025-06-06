using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.IO;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using System.Linq;

public class AudioRecorderUtility : IUtility
{
    private AudioClip recordedClip;
    private bool isRecording = false;
    private string microphoneName;
    public void Init()
    {
        // 获取默认麦克风
        Debug.Log("获取麦克风设备");
        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];

            Debug.Log($"找到麦克风设备: {microphoneName}");
        }
        else
        {
            Debug.LogError("未找到麦克风设备");
        }
    }
    
    /// <summary>
    /// 开始录音
    /// </summary>
    /// <param name="maxRecordTime">最大录音时长（秒），默认300秒</param>
    /// <param name="frequency">采样率，默认44100</param>
    public bool StartRecording(int maxRecordTime = 300, int frequency = 44100)
    {
        if (isRecording)
        {
            Debug.LogWarning("已经在录音中");
            return false;
        }
        
        if (string.IsNullOrEmpty(microphoneName))
        {
            Debug.LogError("没有可用的麦克风设备");
            return false;
        }
        
        try
        {
            // 开始录音：设备名，是否循环，录音时长（秒），采样率
            recordedClip = Microphone.Start(microphoneName, false, maxRecordTime, frequency);
            isRecording = true;
            Debug.Log("开始录音");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"开始录音失败: {e.Message}");
            return false;
        }
    }
    
    /// <summary>
    /// 停止录音并返回路径
    /// </summary>
    /// <param name="fileName">文件名（不包含扩展名），如果为空则自动生成</param>
    /// <returns>保存的文件路径</returns>
    public string StopRecordingAndSave(string fileName = "")
    {
        if (!isRecording)
        {
            Debug.LogWarning("当前没有在录音");
            return null;
        }
        
        try
        {
            Microphone.End(microphoneName);
            isRecording = false;
            Debug.Log("停止录音");
            
            // 保存音频文件
            string savedPath = SaveAudioToFile(fileName);
            return savedPath;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"停止录音失败: {e.Message}");
            isRecording = false;
            return null;
        }
    }
    
    /// <summary>
    /// 取消录音（不保存）
    /// </summary>
    public void CancelRecording()
    {
        if (isRecording)
        {
            Microphone.End(microphoneName);
            isRecording = false;
            recordedClip = null;
            Debug.Log("取消录音");
        }
    }
   
    
    /// <summary>
    /// 检查是否正在录音
    /// </summary>
    public bool IsRecording => isRecording;
   
    /// <summary>
    /// 获取当前录音时长
    /// </summary>
    public float GetRecordingTime()
    {
        if (isRecording && !string.IsNullOrEmpty(microphoneName))
        {
            return (float)Microphone.GetPosition(microphoneName) / 44100f; // 假设采样率为44100
        }
        return 0f;
    }
    

    private string SaveAudioToFile(string fileName)
    {
        if (recordedClip == null)
        {
            Debug.LogError("没有录音数据可保存");
            return null;
        }
        
        try
        {
            // 创建录音保存目录
            string recordingsDir = Path.Combine(Application.persistentDataPath, "Recordings");
            if (!Directory.Exists(recordingsDir))
            {
                Directory.CreateDirectory(recordingsDir);
            }
            
            // 生成文件名
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"recording_{DateTime.Now:yyyyMMdd_HHmmss}";
            }
            else
            {
                fileName = SanitizeFileName(fileName);
            }
            
            string filePath = Path.Combine(recordingsDir, fileName + ".wav");
            
            // 确保文件名唯一
            int counter = 1;
            string originalPath = filePath;
            while (File.Exists(filePath))
            {
                string nameWithoutExt = Path.GetFileNameWithoutExtension(originalPath);
                string directory = Path.GetDirectoryName(originalPath);
                filePath = Path.Combine(directory, $"{nameWithoutExt}_{counter}.wav");
                counter++;
            }
            
            // 保存为WAV文件
            SavWav.Save(filePath, recordedClip);
            
            Debug.Log($"录音已保存到: {filePath}");
            return filePath;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"保存录音文件失败: {e.Message}");
            return null;
        }
    }
    
   
    
    private string SanitizeFileName(string fileName)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            fileName = fileName.Replace(c, '_');
        }
        return fileName;
    }
    

    /// <summary>
    /// 使用UniTask异步加载音频文件并返回AudioClip
    /// </summary>
    /// <param name="audioPath">音频文件路径</param>
    /// <returns>加载的AudioClip，如果失败返回null</returns>
    public async UniTask<AudioClip> LoadAudioClipAsync(string audioPath)
    {
        if (string.IsNullOrEmpty(audioPath) || !File.Exists(audioPath))
        {
            Debug.LogError($"音频文件不存在: {audioPath}");
            return null;
        }
        
        try
        {
            string url = "file://" + audioPath;
            
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                // 使用UniTask等待请求完成
                await www.SendWebRequest().ToUniTask();
                
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"加载音频文件失败: {www.error}");
                    return null;
                }
                
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip == null)
                {
                    Debug.LogError($"无法创建AudioClip: {audioPath}");
                    return null;
                }
                
                Debug.Log($"成功加载音频文件: {audioPath}");
                return clip;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载音频文件异常: {e.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 使用UniTask异步加载音频文件（带超时）
    /// </summary>
    /// <param name="audioPath">音频文件路径</param>
    /// <param name="timeoutSeconds">超时时间（秒），默认30秒</param>
    /// <returns>加载的AudioClip，如果失败或超时返回null</returns>
    public async UniTask<AudioClip> LoadAudioClipAsync(string audioPath, float timeoutSeconds = 30f)
    {
        if (string.IsNullOrEmpty(audioPath) || !File.Exists(audioPath))
        {
            Debug.LogError($"音频文件不存在: {audioPath}");
            return null;
        }
        
        try
        {
            string url = "file://" + audioPath;
            
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                // 使用UniTask等待请求完成，带超时
                var timeoutToken = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
                await www.SendWebRequest().ToUniTask(cancellationToken: timeoutToken.Token);
                
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"加载音频文件失败: {www.error}");
                    return null;
                }
                
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip == null)
                {
                    Debug.LogError($"无法创建AudioClip: {audioPath}");
                    return null;
                }
                
                Debug.Log($"成功加载音频文件: {audioPath}");
                return clip;
            }
        }
        catch (System.OperationCanceledException)
        {
            Debug.LogError($"加载音频文件超时: {audioPath}");
            return null;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"加载音频文件异常: {e.Message}");
            return null;
        }
    }
    
    /// <summary>
    /// 批量异步加载多个音频文件
    /// </summary>
    /// <param name="audioPaths">音频文件路径列表</param>
    /// <returns>加载的AudioClip列表，失败的项为null</returns>
    public async UniTask<List<AudioClip>> LoadMultipleAudioClipsAsync(List<string> audioPaths)
    {
        if (audioPaths == null || audioPaths.Count == 0)
        {
            return new List<AudioClip>();
        }
        
        // 并行加载所有音频文件
        var loadTasks = audioPaths.Select(path => LoadAudioClipAsync(path)).ToArray();
        var results = await UniTask.WhenAll(loadTasks);
        
        return results.ToList();
    }
    
    /// <summary>
    /// 同步加载音频文件并返回AudioClip（不推荐，会阻塞主线程）
    /// </summary>
    /// <param name="audioPath">音频文件路径</param>
    /// <returns>加载的AudioClip，如果失败返回null</returns>
    public AudioClip LoadAudioClip(string audioPath)
    {
        // 使用异步方法的同步版本
        return LoadAudioClipAsync(audioPath).GetAwaiter().GetResult();
    }
 
}

// WAV文件保存工具类
public static class SavWav
{
    public static bool Save(string filepath, AudioClip clip)
    {
        if (!filepath.ToLower().EndsWith(".wav"))
        {
            filepath += ".wav";
        }

        var fileStream = CreateEmpty(filepath);
        ConvertAndWrite(fileStream, clip);
        WriteHeader(fileStream, clip);
        fileStream.Close();
        return true;
    }

    static FileStream CreateEmpty(string filepath)
    {
        var fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++) // 预留WAV文件头空间
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    static void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];
        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767;

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = System.BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }

        fileStream.Write(bytesData, 0, bytesData.Length);
    }

    static void WriteHeader(FileStream fileStream, AudioClip clip)
    {
        var hz = clip.frequency;
        var channels = clip.channels;
        var samples = clip.samples;

        fileStream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        fileStream.Write(riff, 0, 4);

        Byte[] chunkSize = System.BitConverter.GetBytes(fileStream.Length - 8);
        fileStream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        fileStream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        fileStream.Write(fmt, 0, 4);

        Byte[] subChunk1 = System.BitConverter.GetBytes(16);
        fileStream.Write(subChunk1, 0, 4);

        UInt16 one = 1;
        Byte[] audioFormat = System.BitConverter.GetBytes(one);
        fileStream.Write(audioFormat, 0, 2);

        Byte[] numChannels = System.BitConverter.GetBytes(channels);
        fileStream.Write(numChannels, 0, 2);

        Byte[] sampleRate = System.BitConverter.GetBytes(hz);
        fileStream.Write(sampleRate, 0, 4);

        Byte[] byteRate = System.BitConverter.GetBytes(hz * channels * 2);
        fileStream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        fileStream.Write(System.BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = System.BitConverter.GetBytes(bps);
        fileStream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        fileStream.Write(datastring, 0, 4);

        Byte[] subChunk2 = System.BitConverter.GetBytes(samples * channels * 2);
        fileStream.Write(subChunk2, 0, 4);
    }
} 