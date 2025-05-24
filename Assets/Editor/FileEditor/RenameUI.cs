using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class RenameUI : EditorWindow
{
    [System.Serializable]
    public class FolderConfig
    {
        public string targetFolder = "Assets/";
        public string suffix = "_PC";
        public bool includeSubfolders = true;
        public bool isRemoveSuffix = false;
    }

    private List<FolderConfig> folderConfigs = new List<FolderConfig>();
    private Vector2 scrollPosition;

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
        LoadConfigs();
        currentPlatform = (TestPlatform)EditorPrefs.GetInt("TestPlatform", 0);
    }

    void OnDisable()
    {
        SaveConfigs();
    }

    void LoadConfigs()
    {
        string json = EditorPrefs.GetString("UIToolConfigs", "");
        if (!string.IsNullOrEmpty(json))
        {
            folderConfigs = JsonUtility.FromJson<Wrapper>("{\"configs\":" + json + "}").configs;
        }
        
        if (folderConfigs.Count == 0)
        {
            folderConfigs.Add(new FolderConfig());
        }
    }

    void SaveConfigs()
    {
        string json = JsonUtility.ToJson(new Wrapper { configs = folderConfigs });
        json = json.Substring(11, json.Length - 12); // 移除包装器
        EditorPrefs.SetString("UIToolConfigs", json);
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<FolderConfig> configs;
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

        // 文件夹配置部分
        EditorGUILayout.Space(20);
        GUILayout.Label("UI预制体批量重命名工具", EditorStyles.boldLabel);

        // 开始滚动视图
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(position.height - 200));

        for (int i = 0; i < folderConfigs.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            
            var config = folderConfigs[i];
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"文件夹配置 {i + 1}");
            if (GUILayout.Button("删除配置", GUILayout.Width(80)))
            {
                folderConfigs.RemoveAt(i);
                SaveConfigs();
                break;
            }
            EditorGUILayout.EndHorizontal();

            config.targetFolder = EditorGUILayout.TextField("目标文件夹:", config.targetFolder);
            if (GUILayout.Button("选择文件夹"))
            {
                string folder = EditorUtility.OpenFolderPanel("选择预制体所在文件夹", "Assets", "");
                if (!string.IsNullOrEmpty(folder))
                {
                    config.targetFolder = "Assets" + folder.Substring(Application.dataPath.Length);
                    SaveConfigs();
                }
            }
            
            config.suffix = EditorGUILayout.TextField("后缀:", config.suffix);
            config.includeSubfolders = EditorGUILayout.Toggle("包含子文件夹", config.includeSubfolders);
            config.isRemoveSuffix = EditorGUILayout.Toggle("删除后缀", config.isRemoveSuffix);

            if (GUILayout.Button(config.isRemoveSuffix ? "开始删除后缀" : "开始添加后缀"))
            {
                RenamePrefabs(config);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        EditorGUILayout.EndScrollView();

        // 添加新配置按钮放在滚动视图外面
        EditorGUILayout.Space(10);
        if (GUILayout.Button("添加新配置", GUILayout.Height(30)))
        {
            folderConfigs.Add(new FolderConfig());
            SaveConfigs();
        }
    }

    void RenamePrefabs(FolderConfig config)
    {
        if (!AssetDatabase.IsValidFolder(config.targetFolder))
        {
            EditorUtility.DisplayDialog("错误", "无效的文件夹路径!", "确定");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { config.targetFolder });
        int count = 0;

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            
            if (!config.includeSubfolders && Path.GetDirectoryName(assetPath) != config.targetFolder)
                continue;

            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string newFileName;

            if (config.isRemoveSuffix)
            {
                if (!fileName.EndsWith(config.suffix))
                    continue;
                
                newFileName = fileName.Substring(0, fileName.Length - config.suffix.Length);
            }
            else
            {
                if (fileName.EndsWith(config.suffix))
                    continue;
                
                newFileName = fileName + config.suffix;
            }

            string newPath = Path.Combine(
                Path.GetDirectoryName(assetPath),
                newFileName + ".prefab"
            ).Replace("\\", "/");

            AssetDatabase.MoveAsset(assetPath, newPath);
            count++;
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", 
            $"成功{(config.isRemoveSuffix ? "删除" : "添加")}后缀 {count} 个预制体!", 
            "确定");
    }

    public static TestPlatform GetCurrentPlatform()
    {
        return currentPlatform;
    }
}
