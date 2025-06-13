using UnityEngine;

namespace CustomKeyboard
{
    /// <summary>
    /// Android键盘辅助类
    /// 用于在Android 13/14设备上更好地控制系统键盘行为
    /// </summary>
    public static class AndroidKeyboardHelper
    {
        /// <summary>
        /// 强制隐藏系统软键盘
        /// </summary>
        public static void ForceHideSystemKeyboard()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject inputMethodManager = activity.Call<AndroidJavaObject>("getSystemService", "input_method");
                    AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
                    AndroidJavaObject view = window.Call<AndroidJavaObject>("getDecorView");
                    
                    inputMethodManager.Call<bool>("hideSoftInputFromWindow", view.Call<AndroidJavaObject>("getWindowToken"), 0);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"AndroidKeyboardHelper: 隐藏系统键盘失败 - {e.Message}");
            }
#endif
        }
        
        /// <summary>
        /// 设置输入法标志
        /// </summary>
        /// <param name="flags">输入法标志</param>
        public static void SetSoftInputMode(int flags)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
                    
                    window.Call("setSoftInputMode", flags);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"AndroidKeyboardHelper: 设置输入法模式失败 - {e.Message}");
            }
#endif
        }
        
        /// <summary>
        /// 检查是否为Android 13及以上版本
        /// </summary>
        /// <returns>是否为Android 13+</returns>
        public static bool IsAndroid13Plus()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    int sdkInt = version.GetStatic<int>("SDK_INT");
                    return sdkInt >= 33; // Android 13 = API Level 33
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"AndroidKeyboardHelper: 获取Android版本失败 - {e.Message}");
                return false;
            }
#else
            return false;
#endif
        }
        
        /// <summary>
        /// 获取Android版本信息
        /// </summary>
        /// <returns>版本信息字符串</returns>
        public static string GetAndroidVersionInfo()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    int sdkInt = version.GetStatic<int>("SDK_INT");
                    string release = version.GetStatic<string>("RELEASE");
                    return $"Android {release} (API {sdkInt})";
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"AndroidKeyboardHelper: 获取版本信息失败 - {e.Message}");
                return "Unknown Android Version";
            }
#else
            return "Not Android Platform";
#endif
        }
        
        /// <summary>
        /// 禁用系统键盘（适用于Android 13+）
        /// </summary>
        public static void DisableSystemKeyboardForAndroid13Plus()
        {
            if (!IsAndroid13Plus()) return;
            
            // Android 13+ 特殊处理
            // SOFT_INPUT_STATE_ALWAYS_HIDDEN | SOFT_INPUT_ADJUST_PAN
            const int SOFT_INPUT_STATE_ALWAYS_HIDDEN = 0x03;
            const int SOFT_INPUT_ADJUST_PAN = 0x20;
            
            SetSoftInputMode(SOFT_INPUT_STATE_ALWAYS_HIDDEN | SOFT_INPUT_ADJUST_PAN);
            ForceHideSystemKeyboard();
        }
        
        /// <summary>
        /// 恢复系统键盘
        /// </summary>
        public static void RestoreSystemKeyboard()
        {
            // SOFT_INPUT_STATE_UNSPECIFIED | SOFT_INPUT_ADJUST_RESIZE
            const int SOFT_INPUT_STATE_UNSPECIFIED = 0x00;
            const int SOFT_INPUT_ADJUST_RESIZE = 0x10;
            
            SetSoftInputMode(SOFT_INPUT_STATE_UNSPECIFIED | SOFT_INPUT_ADJUST_RESIZE);
        }
        
        /// <summary>
        /// 初始化Android键盘设置
        /// </summary>
        public static void InitializeForCustomKeyboard()
        {
            Debug.Log($"AndroidKeyboardHelper: 当前系统版本 - {GetAndroidVersionInfo()}");
            
            if (IsAndroid13Plus())
            {
                Debug.Log("AndroidKeyboardHelper: 检测到Android 13+，启用自定义键盘模式");
                DisableSystemKeyboardForAndroid13Plus();
            }
            else
            {
                Debug.Log("AndroidKeyboardHelper: Android版本低于13，可能需要其他处理方式");
            }
        }
    }
} 