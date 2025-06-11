using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TextToTMPConverter : EditorWindow
{
    private bool includeInactive = true;
    private bool showPreview = false;
    private List<Text> foundTextComponents = new List<Text>();
    private Vector2 scrollPosition;
    private ConversionMode conversionMode = ConversionMode.Scene;

    public enum ConversionMode
    {
        Scene,
        SelectedObjects,
        PrefabStage
    }

    [MenuItem("MomoTools/Text to TMP Converter")]
    public static void ShowWindow()
    {
        GetWindow<TextToTMPConverter>("Text to TMP Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Text to TextMeshPro Converter", EditorStyles.boldLabel);
        GUILayout.Space(10);

        // 选择转换模式
        conversionMode = (ConversionMode)EditorGUILayout.EnumPopup("Conversion Mode", conversionMode);

        GUILayout.Space(5);

        // 根据模式显示不同的说明
        switch (conversionMode)
        {
            case ConversionMode.Scene:
                EditorGUILayout.HelpBox("Convert all Text components in the current scene.", MessageType.Info);
                break;
            case ConversionMode.SelectedObjects:
                EditorGUILayout.HelpBox("Convert Text components in selected GameObjects and their children.", MessageType.Info);
                break;
            case ConversionMode.PrefabStage:
                EditorGUILayout.HelpBox("Convert Text components in the currently opened prefab.", MessageType.Info);
                break;
        }

        includeInactive = EditorGUILayout.Toggle("Include Inactive Objects", includeInactive);

        GUILayout.Space(10);

        if (GUILayout.Button("Scan for Text Components"))
        {
            ScanForTextComponents();
        }

        if (foundTextComponents.Count > 0)
        {
            GUILayout.Space(10);
            GUILayout.Label($"Found {foundTextComponents.Count} Text components:", EditorStyles.boldLabel);

            showPreview = EditorGUILayout.Foldout(showPreview, "Preview Components");

            if (showPreview)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
                foreach (var text in foundTextComponents)
                {
                    if (text != null)
                    {
                        EditorGUILayout.ObjectField(text.gameObject, typeof(GameObject), true);
                    }
                }
                EditorGUILayout.EndScrollView();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Convert All to TextMeshPro"))
            {
                ConvertAllTextToTMP();
            }
        }
        else if (foundTextComponents.Count == 0 && GUILayout.Button("Refresh"))
        {
            ScanForTextComponents();
        }
    }

    private void ScanForTextComponents()
    {
        foundTextComponents.Clear();

        switch (conversionMode)
        {
            case ConversionMode.Scene:
                ScanScene();
                break;
            case ConversionMode.SelectedObjects:
                ScanSelectedObjects();
                break;
            case ConversionMode.PrefabStage:
                ScanPrefabStage();
                break;
        }

        Debug.Log($"Found {foundTextComponents.Count} Text components");
    }

    private void ScanScene()
    {
        Text[] allTexts = FindObjectsOfType<Text>(includeInactive);
        foundTextComponents.AddRange(allTexts);
    }

    private void ScanSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "Please select one or more GameObjects to scan.", "OK");
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            // 获取选中对象及其子对象中的所有Text组件
            Text[] texts = obj.GetComponentsInChildren<Text>(includeInactive);
            foundTextComponents.AddRange(texts);
        }
    }

    private void ScanPrefabStage()
    {
        // 检查是否在预制体编辑模式
        var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();

        if (prefabStage == null)
        {
            EditorUtility.DisplayDialog("Not in Prefab Mode", "Please open a prefab for editing to use this mode.", "OK");
            return;
        }

        // 获取预制体根对象
        GameObject prefabRoot = prefabStage.prefabContentsRoot;

        if (prefabRoot != null)
        {
            Text[] texts = prefabRoot.GetComponentsInChildren<Text>(includeInactive);
            foundTextComponents.AddRange(texts);
        }
    }

    private void ConvertAllTextToTMP()
    {
        if (foundTextComponents.Count == 0)
        {
            EditorUtility.DisplayDialog("No Components", "No Text components found to convert.", "OK");
            return;
        }

        bool proceed = EditorUtility.DisplayDialog(
            "Convert Text Components",
            $"This will convert {foundTextComponents.Count} Text components to TextMeshPro. This action cannot be undone. Continue?",
            "Convert",
            "Cancel"
        );

        if (!proceed) return;

        int convertedCount = 0;

        // 记录Undo操作
        Undo.SetCurrentGroupName("Convert Text to TMP");
        int undoGroup = Undo.GetCurrentGroup();

        try
        {
            for (int i = foundTextComponents.Count - 1; i >= 0; i--)
            {
                Text textComponent = foundTextComponents[i];
                if (textComponent != null)
                {
                    ConvertTextToTMP(textComponent);
                    convertedCount++;
                }
            }

            Undo.CollapseUndoOperations(undoGroup);

            // 如果在预制体模式，标记预制体为已修改
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorUtility.SetDirty(prefabStage.prefabContentsRoot);
            }

            EditorUtility.DisplayDialog(
                "Conversion Complete",
                $"Successfully converted {convertedCount} Text components to TextMeshPro.",
                "OK"
            );

            // 清空列表
            foundTextComponents.Clear();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error during conversion: {e.Message}");
            EditorUtility.DisplayDialog("Error", $"An error occurred during conversion: {e.Message}", "OK");
        }
    }

    private void ConvertTextToTMP(Text originalText)
    {
        GameObject gameObject = originalText.gameObject;

        // 记录原始属性
        string text = originalText.text;
        Font font = originalText.font;
        int fontSize = originalText.fontSize;
        FontStyle fontStyle = originalText.fontStyle;
        Color color = originalText.color;
        TextAnchor alignment = originalText.alignment;
        bool raycastTarget = originalText.raycastTarget;
        Material material = originalText.material;
        bool supportRichText = originalText.supportRichText;
        HorizontalWrapMode horizontalOverflow = originalText.horizontalOverflow;
        VerticalWrapMode verticalOverflow = originalText.verticalOverflow;
        float lineSpacing = originalText.lineSpacing;

        // 记录RectTransform属性
        RectTransform rectTransform = originalText.rectTransform;
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;
        Vector2 pivot = rectTransform.pivot;
        Vector3 localScale = rectTransform.localScale;
        Quaternion localRotation = rectTransform.localRotation;

        // 记录Undo
        Undo.RegisterCompleteObjectUndo(gameObject, "Convert Text to TMP");

        // 删除原始Text组件
        Undo.DestroyObjectImmediate(originalText);

        // 添加TextMeshProUGUI组件
        TextMeshProUGUI tmpText = Undo.AddComponent<TextMeshProUGUI>(gameObject);

        // 设置文本内容
        tmpText.text = text;

        // 设置字体（尝试找到对应的TMP字体）
        TMP_FontAsset tmpFont = GetTMPFont(font);
        if (tmpFont != null)
        {
            tmpText.font = tmpFont;
        }
    
        // 设置字体大小
        tmpText.fontSize = fontSize;

        // 设置字体样式
        tmpText.fontStyle = ConvertFontStyle(fontStyle);

        // 设置颜色
        tmpText.color = color;

        // 设置对齐方式
        tmpText.alignment = ConvertTextAlignment(alignment);

        // 设置其他属性
        tmpText.raycastTarget = raycastTarget;
        tmpText.richText = supportRichText;
        tmpText.overflowMode = ConvertOverflowMode(horizontalOverflow, verticalOverflow);
        tmpText.lineSpacing = lineSpacing;

        if (material != null)
        {
            tmpText.material = material;
        }

        // 恢复RectTransform属性
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.pivot = pivot;
        rectTransform.localScale = localScale;
        rectTransform.localRotation = localRotation;

        // 标记对象为已修改
        EditorUtility.SetDirty(gameObject);

        Debug.Log($"Converted Text component on {gameObject.name} to TextMeshPro");
    }

    private TextOverflowModes ConvertOverflowMode(HorizontalWrapMode horizontal, VerticalWrapMode vertical)
    {
        if (horizontal == HorizontalWrapMode.Overflow && vertical == VerticalWrapMode.Overflow)
            return TextOverflowModes.Overflow;
        else if (vertical == VerticalWrapMode.Truncate)
            return TextOverflowModes.Truncate;
        else
            return TextOverflowModes.Overflow;
    }

    private TMP_FontAsset GetTMPFont(Font originalFont)
    {
        if (originalFont == null)
        {
            // 返回TMP默认字体
            return GetDefaultTMPFont();
        }

        // 尝试在项目中找到同名的TMP字体
        string[] guids = AssetDatabase.FindAssets($"{originalFont.name} t:TMP_FontAsset");

        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            return AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
        }

        // 如果找不到，返回默认TMP字体
        return GetDefaultTMPFont();
    }

    private TMP_FontAsset GetDefaultTMPFont()
    {
        // 方法1：尝试加载TMP默认字体
        // TMP_FontAsset defaultFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        // if (defaultFont != null) return defaultFont;
        TMP_FontAsset defaultFont = null;
        // 方法2：尝试从TMP设置中获取默认字体
        TMP_Settings tmpSettings = TMP_Settings.instance;
        if (tmpSettings != null && TMP_Settings.defaultFontAsset != null)
        {
            return TMP_Settings.defaultFontAsset;
        }

        // 方法3：查找项目中的第一个TMP字体
        string[] fontGuids = AssetDatabase.FindAssets("t:TMP_FontAsset");
        if (fontGuids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(fontGuids[0]);
            return AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(path);
        }

        // 方法4：尝试加载Arial字体（如果存在）
        defaultFont = Resources.Load<TMP_FontAsset>("Fonts & Materials/Arial SDF");
        if (defaultFont != null) return defaultFont;

        Debug.LogWarning("Could not find any TMP_FontAsset. Please ensure TextMeshPro is properly imported.");
        return null;
    }

    private FontStyles ConvertFontStyle(FontStyle fontStyle)
    {
        switch (fontStyle)
        {
            case FontStyle.Normal:
                return FontStyles.Normal;
            case FontStyle.Bold:
                return FontStyles.Bold;
            case FontStyle.Italic:
                return FontStyles.Italic;
            case FontStyle.BoldAndItalic:
                return FontStyles.Bold | FontStyles.Italic;
            default:
                return FontStyles.Normal;
        }
    }

    private TextAlignmentOptions ConvertTextAlignment(TextAnchor textAnchor)
    {
        switch (textAnchor)
        {
            case TextAnchor.UpperLeft:
                return TextAlignmentOptions.TopLeft;
            case TextAnchor.UpperCenter:
                return TextAlignmentOptions.Top;
            case TextAnchor.UpperRight:
                return TextAlignmentOptions.TopRight;
            case TextAnchor.MiddleLeft:
                return TextAlignmentOptions.MidlineLeft;
            case TextAnchor.MiddleCenter:
                return TextAlignmentOptions.Midline;
            case TextAnchor.MiddleRight:
                return TextAlignmentOptions.MidlineRight;
            case TextAnchor.LowerLeft:
                return TextAlignmentOptions.BottomLeft;
            case TextAnchor.LowerCenter:
                return TextAlignmentOptions.Bottom;
            case TextAnchor.LowerRight:
                return TextAlignmentOptions.BottomRight;
            default:
                return TextAlignmentOptions.Center;
        }
    }
}