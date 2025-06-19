using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;

public class ExpandableText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private Button expandButton;
    [SerializeField] private Image arrowIcon;
    [SerializeField] private RectTransform textContainer;
    [SerializeField] private int maxVisibleLines = 2;
    
    private bool isExpanded = false;
    private float collapsedHeight;
    private float expandedHeight;
    private string fullText;
    
    private void Start()
    {
        expandButton.onClick.AddListener(ToggleExpansion);
        // 初始隐藏展开按钮
        expandButton.gameObject.SetActive(false);
    }
    
    public void SetText(string text)
    {
        fullText = text;
        StartCoroutine(SetupTextCoroutine());
    }
    
    private IEnumerator SetupTextCoroutine()
    {
        if (string.IsNullOrEmpty(fullText))
        {
            textComponent.text = "";
            expandButton.gameObject.SetActive(false);
            yield break;
        }
        
        // 设置文本内容
        textComponent.text = fullText;
        
        // 等待一帧确保布局更新
        yield return null;
        
        // 先获取完整文本的高度
        textComponent.overflowMode = TextOverflowModes.Overflow;
        textComponent.enableWordWrapping = true;
        
        // 强制重新计算布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform);
        yield return null;
        
        expandedHeight = textComponent.preferredHeight;
        
        // 设置限制行数
        textComponent.maxVisibleLines = maxVisibleLines;
        textComponent.overflowMode = TextOverflowModes.Truncate;
        
        // 再次强制重新计算布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(textComponent.rectTransform);
        yield return null;
        
        collapsedHeight = textComponent.preferredHeight;
        
        // 检查文本是否被截断
        bool isTextTruncated = textComponent.isTextTruncated;
        bool heightDifference = expandedHeight > collapsedHeight + 5f; // 5f 的容差
        
        // Debug.Log($"Text: {fullText.Substring(0, Mathf.Min(20, fullText.Length))}...");
        // Debug.Log($"Is Truncated: {isTextTruncated}");
        // Debug.Log($"Expanded Height: {expandedHeight}");
        // Debug.Log($"Collapsed Height: {collapsedHeight}");
        // Debug.Log($"Height Difference: {heightDifference}");
        
        bool needsExpansion = isTextTruncated || heightDifference;
        expandButton.gameObject.SetActive(needsExpansion);
        
        // 设置容器高度
        if (textContainer != null)
        {
            textContainer.sizeDelta = new Vector2(textContainer.sizeDelta.x, collapsedHeight);
        }
        
        // 确保文本垂直居中
        textComponent.verticalAlignment = VerticalAlignmentOptions.Middle;
        
        if (needsExpansion)
        {
            UpdateArrowRotation();
        }
    }
    
    private void ToggleExpansion()
    {
        isExpanded = !isExpanded;
        
        if (isExpanded)
        {
            textComponent.maxVisibleLines = int.MaxValue;
            textComponent.overflowMode = TextOverflowModes.Overflow;
            if (textContainer != null)
            {
                textContainer.DOSizeDelta(new Vector2(textContainer.sizeDelta.x, expandedHeight), 0.3f);
            }
        }
        else
        {
            textComponent.maxVisibleLines = maxVisibleLines;
            textComponent.overflowMode = TextOverflowModes.Truncate;
            if (textContainer != null)
            {
                textContainer.DOSizeDelta(new Vector2(textContainer.sizeDelta.x, collapsedHeight), 0.3f);
            }
        }
        
        UpdateArrowRotation();
    }
    
    private void UpdateArrowRotation()
    {
        if (arrowIcon != null)
        {
            float targetRotation = isExpanded ? 180f : 0f;
            arrowIcon.transform.DORotate(new Vector3(0, 0, targetRotation), 0.3f);
        }
    }
    
    // 添加调试方法
    [ContextMenu("Debug Text Info")]
    private void DebugTextInfo()
    {
        if (textComponent != null)
        {
            // Debug.Log($"Text: {textComponent.text}");
            // Debug.Log($"Max Visible Lines: {textComponent.maxVisibleLines}");
            // Debug.Log($"Is Text Truncated: {textComponent.isTextTruncated}");
            // Debug.Log($"Preferred Height: {textComponent.preferredHeight}");
            // Debug.Log($"Rect Height: {textComponent.rectTransform.rect.height}");
        }
    }
}