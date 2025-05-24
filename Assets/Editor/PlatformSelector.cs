using UnityEngine;
using UnityEditor;

public class PlatformSelector : EditorWindow
{
    public enum TestPlatform
    {
        PC,
        Mobile
    }

    // 静态属性供运行时获取
    public static TestPlatform CurrentPlatform = TestPlatform.PC;

    [MenuItem("MomoTools/Platform Selector")]
    static void ShowWindow()
    {
        GetWindow<PlatformSelector>("平台选择器");
    }

    void OnGUI()
    {
        GUILayout.Label("编辑器测试平台选择", EditorStyles.boldLabel);
        
        CurrentPlatform = (TestPlatform)EditorGUILayout.EnumPopup("测试平台:", CurrentPlatform);
        
        if (GUI.changed)
        {
            EditorPrefs.SetInt("TestPlatform", (int)CurrentPlatform);
        }
    }

    // 在编辑器启动时加载保存的设置
    [InitializeOnLoadMethod]
    static void Initialize()
    {
        CurrentPlatform = (TestPlatform)EditorPrefs.GetInt("TestPlatform", 0);
    }
} 