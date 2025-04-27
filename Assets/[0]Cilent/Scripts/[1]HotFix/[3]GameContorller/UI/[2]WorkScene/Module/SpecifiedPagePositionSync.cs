using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecifiedPagePositionSync : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float offsetX;
    [SerializeField]
    Transform PagePromptBoxLast;
    private Transform myTransform;
    EntryDisPanelNew entryDisPanel;
    [SerializeField]
    InputField PageInputField;
    [SerializeField]
    Button SpecifiedPageConfirmButton;
    float initialOffsetX;
    private Vector3[] myCorners = new Vector3[4];
    private Vector3[] lastCorners = new Vector3[4];

    void Start()
    {
        entryDisPanel = GetComponentInParent<EntryDisPanelNew>();
        SpecifiedPageConfirmButton.onClick.AddListener(OnSpecifiedPageConfirmButtonClick);
        myTransform = this.transform;
        // 计算左边缘的X轴偏移量（使用左下角点）
        initialOffsetX = myTransform.position.x - PagePromptBoxLast.position.x;
    }

    private void FixedUpdate()
    {
        // 保持相同的X轴偏移量，保留原始Y和Z位置
        Vector3 currentPos = myTransform.position;
        float newX = PagePromptBoxLast.position.x + initialOffsetX;

        // 更新位置
        myTransform.position = new Vector3(newX-offsetX, currentPos.y, currentPos.z);
    }
    void OnSpecifiedPageConfirmButtonClick()
    {
        if (int.TryParse(PageInputField.text, out int pageIndex))
        {
            entryDisPanel.OnSpecifiedPageConfirmButtonClick(pageIndex);
        }
    }
}
