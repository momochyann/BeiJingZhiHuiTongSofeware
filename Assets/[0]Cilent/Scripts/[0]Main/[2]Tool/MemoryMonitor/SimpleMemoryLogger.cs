using UnityEngine;
using Unity.Profiling;
using System.Collections;

namespace MemoryMonitor
{
    /// <summary>
    /// 精简内存监控脚本 - 每5秒打印一次内存使用量
    /// </summary>
    public class SimpleMemoryLogger : MonoBehaviour
    {
        [Header("设置")]
        [SerializeField] private float logInterval = 5f;  // 打印间隔（秒）
        
        // Profiler记录器
        private ProfilerRecorder gcUsedMemoryRecorder;
        private ProfilerRecorder systemUsedMemoryRecorder;
        private ProfilerRecorder textureMemoryRecorder;
        
        private void Start()
        {
            // 初始化内存记录器
            gcUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory");
            systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
            textureMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Texture Memory");
            
            // 开始定时打印
            StartCoroutine(LogMemoryUsage());
            
            Debug.Log("内存监控已启动，每" + logInterval + "秒打印一次");
        }
        
        /// <summary>
        /// 定时打印内存使用情况
        /// </summary>
        private IEnumerator LogMemoryUsage()
        {
            while (true)
            {
                yield return new WaitForSeconds(logInterval);
                PrintMemoryInfo();
            }
        }
        
        /// <summary>
        /// 打印内存信息
        /// </summary>
        private void PrintMemoryInfo()
        {
            string memoryInfo = "=== 内存使用情况 ===\n";
            
            // GC内存
            if (gcUsedMemoryRecorder.Valid)
            {
                long gcMemory = gcUsedMemoryRecorder.LastValue;
                memoryInfo += $"GC内存: {FormatMB(gcMemory)}\n";
            }
            
            // 系统内存
            if (systemUsedMemoryRecorder.Valid)
            {
                long systemMemory = systemUsedMemoryRecorder.LastValue;
                memoryInfo += $"系统内存: {FormatMB(systemMemory)}\n";
            }
            
            // 纹理内存
            if (textureMemoryRecorder.Valid)
            {
                long textureMemory = textureMemoryRecorder.LastValue;
                memoryInfo += $"纹理内存: {FormatMB(textureMemory)}\n";
            }
            
            // 添加FPS信息
            memoryInfo += $"FPS: {(1f / Time.deltaTime):F1}";
            
            Debug.Log(memoryInfo);
            
            // 如果GC内存过高，发出警告
            if (gcUsedMemoryRecorder.Valid && gcUsedMemoryRecorder.LastValue > 20 * 1024 * 1024)
            {
                Debug.LogWarning("⚠️ GC内存使用过高! 当前: " + FormatMB(gcUsedMemoryRecorder.LastValue));
            }
        }
        
        /// <summary>
        /// 格式化为MB显示
        /// </summary>
        private string FormatMB(long bytes)
        {
            return $"{bytes / (1024.0 * 1024.0):F1} MB";
        }
        
        private void OnDestroy()
        {
            // 释放记录器
            gcUsedMemoryRecorder.Dispose();
            systemUsedMemoryRecorder.Dispose();
            textureMemoryRecorder.Dispose();
        }
    }
} 