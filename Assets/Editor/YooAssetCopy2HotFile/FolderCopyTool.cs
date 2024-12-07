using UnityEditor;
using UnityEngine;
using System.IO;

public class FolderCopyTool : EditorWindow
{
    private string sourceFolderPath1 = "";
    private string targetFolderPath1 = "";
    private string sourceFolderPath2 = "";
    private string targetFolderPath2 = "";

    [MenuItem("Tools/Folder Copy Tool")]
    public static void ShowWindow()
    {
        GetWindow<FolderCopyTool>("Folder Copy Tool");
    }

  private void OnEnable()
    {
        sourceFolderPath1 = EditorPrefs.GetString(GetProjectFolderNamePath("SourceFolderPath1"), "");
        targetFolderPath1 = EditorPrefs.GetString(GetProjectFolderNamePath("TargetFolderPath1"), "");
        sourceFolderPath2 = EditorPrefs.GetString(GetProjectFolderNamePath("SourceFolderPath2"), "");
        targetFolderPath2 = EditorPrefs.GetString(GetProjectFolderNamePath("TargetFolderPath2"), "");
    }

    private void OnDisable()
    {
        EditorPrefs.SetString(GetProjectFolderNamePath("SourceFolderPath1"), sourceFolderPath1);
        EditorPrefs.SetString(GetProjectFolderNamePath("TargetFolderPath1"), targetFolderPath1);
        EditorPrefs.SetString(GetProjectFolderNamePath("SourceFolderPath2"), sourceFolderPath2);
        EditorPrefs.SetString(GetProjectFolderNamePath("TargetFolderPath2"), targetFolderPath2);
    }
    private string GetProjectFolderNamePath(string path)
    {
        string projectPath = Application.dataPath;
        return new DirectoryInfo(projectPath).Parent.Name + path;
    }
    private void OnGUI()
    {
        GUILayout.Label("Folder Copy Tool", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("Copy Operation 1");
        EditorGUILayout.BeginHorizontal();
        sourceFolderPath1 = EditorGUILayout.TextField("Source Folder", sourceFolderPath1);
        if (GUILayout.Button("Browse"))
        {
            string startPath = string.IsNullOrEmpty(sourceFolderPath1) ? "" : Path.GetDirectoryName(sourceFolderPath1);
            sourceFolderPath1 = EditorUtility.OpenFolderPanel("Select Source Folder", startPath, "");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        targetFolderPath1 = EditorGUILayout.TextField("Target Folder", targetFolderPath1);
        if (GUILayout.Button("Browse"))
        {
            string startPath = string.IsNullOrEmpty(targetFolderPath1) ? "" : Path.GetDirectoryName(targetFolderPath1);
            targetFolderPath1 = EditorUtility.OpenFolderPanel("Select Target Folder", startPath, "");
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Copy and Clear Target Folder"))
        {
            ClearAndCopyFolder(sourceFolderPath1, targetFolderPath1);
        }
        if (GUILayout.Button("Copy Without Clearing"))
        {
            CopyFolder(sourceFolderPath1, targetFolderPath1);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Copy Operation 2");
        EditorGUILayout.BeginHorizontal();
        sourceFolderPath2 = EditorGUILayout.TextField("Source Folder", sourceFolderPath2);
        if (GUILayout.Button("Browse"))
        {
            string startPath = string.IsNullOrEmpty(sourceFolderPath2) ? "" : Path.GetDirectoryName(sourceFolderPath2);
            sourceFolderPath2 = EditorUtility.OpenFolderPanel("Select Source Folder", startPath, "");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        targetFolderPath2 = EditorGUILayout.TextField("Target Folder", targetFolderPath2);
        if (GUILayout.Button("Browse"))
        {
            string startPath = string.IsNullOrEmpty(targetFolderPath2) ? "" : Path.GetDirectoryName(targetFolderPath2);
            targetFolderPath2 = EditorUtility.OpenFolderPanel("Select Target Folder", startPath, "");
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Copy and Clear Target Folder"))
        {
            ClearAndCopyFolder(sourceFolderPath2, targetFolderPath2);
        }
        if (GUILayout.Button("Copy Without Clearing"))
        {
            CopyFolder(sourceFolderPath2, targetFolderPath2);
        }
    }

    private void ClearAndCopyFolder(string sourceFolder, string targetFolder)
    {
        if (Directory.Exists(targetFolder))
        {
            Directory.Delete(targetFolder, true);
        }
        Debug.Log("Is Delet");
        CopyFolder(sourceFolder, targetFolder);
    }

    private void CopyFolder(string sourceFolder, string targetFolder)
    {
        if (!Directory.Exists(sourceFolder))
        {
            Debug.LogError("Source folder does not exist: " + sourceFolder);
            return;
        }
        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        foreach (var file in Directory.GetFiles(sourceFolder))
        {
            var destFile = Path.Combine(targetFolder, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }

        foreach (var directory in Directory.GetDirectories(sourceFolder))
        {
            var destDirectory = Path.Combine(targetFolder, Path.GetFileName(directory));
            CopyFolder(directory, destDirectory);
        }
        Debug.Log("Is Copy");
    }
}
