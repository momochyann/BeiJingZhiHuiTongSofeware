using UnityEngine;
using UnityEditor;
using System.IO;

public class RenameUI : EditorWindow
{
    private string targetFolder = "Assets/";
    private string suffix = "_PC";
    private bool includeSubfolders = true;
    private bool isRemoveSuffix = false;

    public enum TestPlatform
    {
        PC,
        Mobile
    }
    private static TestPlatform currentPlatform = TestPlatform.PC;
    
    [MenuItem("MomoTools/UI Tools")]
    static void ShowWindow()
    {
        GetWindow<RenameUI>("UI工具");
    }

    void OnEnable()
    {
        // 加载保存的平台设置
        currentPlatform = (TestPlatform)EditorPrefs.GetInt("TestPlatform", 0);
    }

    void OnGUI()
    {
        // 平台选择部分
        EditorGUILayout.Space(10);
        GUILayout.Label("测试平台选择", EditorStyles.boldLabel);
        EditorGUI.BeginChangeCheck();
        currentPlatform = (TestPlatform)EditorGUILayout.EnumPopup("测试平台:", currentPlatform);
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetInt("TestPlatform", (int)currentPlatform);
        }

        // UI重命名部分
        EditorGUILayout.Space(20);
        GUILayout.Label("UI预制体批量重命名工具", EditorStyles.boldLabel);
        
        targetFolder = EditorGUILayout.TextField("目标文件夹:", targetFolder);
        if (GUILayout.Button("选择文件夹"))
        {
            string folder = EditorUtility.OpenFolderPanel("选择预制体所在文件夹", "Assets", "");
            if (!string.IsNullOrEmpty(folder))
            {
                // 转换为相对路径
                targetFolder = "Assets" + folder.Substring(Application.dataPath.Length);
            }
        }
        
        suffix = EditorGUILayout.TextField("后缀:", suffix);
        includeSubfolders = EditorGUILayout.Toggle("包含子文件夹", includeSubfolders);
        isRemoveSuffix = EditorGUILayout.Toggle("删除后缀", isRemoveSuffix);

        if (GUILayout.Button(isRemoveSuffix ? "开始删除后缀" : "开始添加后缀"))
        {
            RenamePrefabs();
        }
    }

    // 供运行时代码获取当前测试平台
    public static TestPlatform GetCurrentPlatform()
    {
        return currentPlatform;
    }

    void RenamePrefabs()
    {
        if (!AssetDatabase.IsValidFolder(targetFolder))
        {
            EditorUtility.DisplayDialog("错误", "无效的文件夹路径!", "确定");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { targetFolder });
        int count = 0;

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            
            // 如果不包含子文件夹且文件不在目标文件夹直接下级，则跳过
            if (!includeSubfolders && Path.GetDirectoryName(assetPath) != targetFolder)
                continue;

            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string newFileName;

            if (isRemoveSuffix)
            {
                // 如果文件名不包含后缀，跳过
                if (!fileName.EndsWith(suffix))
                    continue;
                
                // 删除后缀
                newFileName = fileName.Substring(0, fileName.Length - suffix.Length);
            }
            else
            {
                // 如果文件名已经包含后缀，跳过
                if (fileName.EndsWith(suffix))
                    continue;
                
                // 添加后缀
                newFileName = fileName + suffix;
            }

            // 构建新路径
            string newPath = Path.Combine(
                Path.GetDirectoryName(assetPath),
                newFileName + ".prefab"
            ).Replace("\\", "/");

            // 重命名资源
            AssetDatabase.MoveAsset(assetPath, newPath);
            count++;
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", 
            $"成功{(isRemoveSuffix ? "删除" : "添加")}后缀 {count} 个预制体!", 
            "确定");
    }
}
