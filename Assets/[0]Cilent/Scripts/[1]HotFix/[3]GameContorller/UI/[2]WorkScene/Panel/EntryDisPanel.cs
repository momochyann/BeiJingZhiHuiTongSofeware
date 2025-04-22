using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;

enum RefreshTiming
{
    OnClickpageLeftListLast,
    OnClickpageLeftListStart,
    OnSpecifiedPageIndex,
    other,
}

public class EntryDisPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    const int pageSize = 10;
    private int pageIndex = 0;
    List<int> pageLeftList = new List<int>();
    bool isLastPageBoxDis = false;

    [SerializeField] Button BeforePageButton;
    [SerializeField] Button NextPageButton;

    [SerializeField] GameObject pageDisBox;
    [SerializeField] GameObject entryBox;
    [SerializeField] string pfbName;
    GameObject entryPfb;
    GameObject pagePromptBoxPfb;
    int pageCount => GetPageCount();
    int entryCount;
    void Start()
    {
        InitAsync().Forget();
    }
    async UniTaskVoid InitAsync()
    {
        entryPfb = await this.GetModel<YooAssetPfbModel>().LoadPfb(pfbName);
        pagePromptBoxPfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("PagePromptBox");
        BeforePageButton.onClick.AddListener(OnBeforePageButtonClick);
        NextPageButton.onClick.AddListener(OnNextPageButtonClick);
        entryCount = entryPfb.GetComponent<IEntry>().GetListCount();
        this.RegisterEvent<Can2ListModelChangeEvent>(OnModelChange);
        pageLeftList = new List<int>();
        for (int i = 1; i < 5; i++)
        {
            pageLeftList.Add(i);
        }
        await UniTask.Delay(100);
        OnPageIndexButtonClick(1);
    }

    private void OnModelChange(Can2ListModelChangeEvent _event)
    {
        Debug.Log("修改数据并显示");
        UpdatePagePromptBox(RefreshTiming.other);
        DisPage();
    }

    private void OnNextPageButtonClick()
    {
        Debug.Log("OnNextPageButtonClick");
        if (pageIndex < pageCount)
        {
            OnPageIndexButtonClick(pageIndex + 1);
        }
    }

    private void OnBeforePageButtonClick()
    {
        Debug.Log("OnBeforePageButtonClick");
        if (pageIndex > 1)
            OnPageIndexButtonClick(pageIndex - 1);
    }

    void DisPage()
    {
        entryCount = entryPfb.GetComponent<IEntry>().GetListCount();
        for (int i = entryBox.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(entryBox.transform.GetChild(i).gameObject);
        }
        for (int i = (pageIndex - 1) * pageSize; i < pageIndex * pageSize; i++)
        {
            if (i >= entryCount)
                return;
            var entry = Instantiate(entryPfb, entryBox.transform);
            entry.GetComponent<IEntry>().DisEntry(i);
        }
    }

    int GetPageCount()
    {
        float _entryCount = entryCount;
        float _pageSize = pageSize;
        return (int)Mathf.Ceil(_entryCount / _pageSize);
    }
    void UpdatePagePromptBox(RefreshTiming refreshTiming)
    {
        if (pageCount <= 7) // 页码小于7 全部显示
        {
            CompleteStartPagePromptBox(pageCount);
        }
        else
        {
            if (pageDisBox.transform.childCount < 9)  // 页码大于7  先补全7个格子
            {
                CompleteStartPagePromptBox(7);
            }
            if (isLastPageBoxDis) // 显示最后几页的目录
            {
                if (refreshTiming == RefreshTiming.OnClickpageLeftListStart) // 
                {
                    isLastPageBoxDis = false;
                    UpdateMiddlePagePromptBox(-1);
                }
                else if (refreshTiming == RefreshTiming.OnSpecifiedPageIndex) // 
                {
                    if (pageIndex < pageCount - 5)
                    {
                        isLastPageBoxDis = false;
                        var pageLeftListStartIndex = pageIndex - 2;
                        for (int i = 0; i < 4; i++)
                        {
                            pageLeftList[i] = pageLeftListStartIndex + i;
                            pageDisBox.transform.GetChild(i + 1).GetComponent<PagePromptBox>().SetPageIndex(pageLeftList[i], pageLeftList[i] == pageIndex);
                        }
                        UpdateMiddlePagePromptBoxTheLastThree();
                    }
                    else
                    {
                        UpdateLastPagePromptBox();
                    }
                }
                else
                {
                    UpdateLastPagePromptBox();
                }
            }
            else if (isLastPageBoxDis == false) // 页码大于7  且显示中间目录
            {
                if (pageIndex >= pageCount - 4)
                {
                    isLastPageBoxDis = true;
                    UpdateLastPagePromptBox();
                }
                else
                {
                    if (refreshTiming == RefreshTiming.OnClickpageLeftListStart) // 
                    {
                        var changeValue = pageIndex == 1 ? 0 : -1;
                        UpdateMiddlePagePromptBox(changeValue);
                    }
                    else if (refreshTiming == RefreshTiming.OnClickpageLeftListLast)
                    {
                        UpdateMiddlePagePromptBox(1);
                    }
                    else if (refreshTiming == RefreshTiming.OnSpecifiedPageIndex)
                    {
                        var pageLeftListStartIndex = pageIndex - 2;
                        if (pageLeftListStartIndex < 1)
                        {
                            pageLeftListStartIndex = 1;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            pageLeftList[i] = pageLeftListStartIndex + i;
                            pageDisBox.transform.GetChild(i + 1).GetComponent<PagePromptBox>().SetPageIndex(pageLeftList[i], pageLeftList[i] == pageIndex);
                        }
                        UpdateMiddlePagePromptBoxTheLastThree();
                    }
                    else if (refreshTiming == RefreshTiming.other)
                    {
                        UpdateMiddlePagePromptBox(0);
                    }
                }
            }
        }
    }
    void UpdateMiddlePagePromptBox(int changeValue)
    {
        for (int i = 0; i < 4; i++)
        {
            pageLeftList[i] = pageLeftList[i] + changeValue;
            pageDisBox.transform.GetChild(i + 1).GetComponent<PagePromptBox>().SetPageIndex(pageLeftList[i], pageLeftList[i] == pageIndex);
        }
        UpdateMiddlePagePromptBoxTheLastThree();
    }
    void UpdateMiddlePagePromptBoxTheLastThree()
    {
        pageDisBox.transform.GetChild(5).GetComponent<PagePromptBox>().SetPageIndex("...", false);
        pageDisBox.transform.GetChild(6).GetComponent<PagePromptBox>().SetPageIndex(pageCount - 1, false);
        pageDisBox.transform.GetChild(7).GetComponent<PagePromptBox>().SetPageIndex(pageCount, false);
    }
    void UpdateLastPagePromptBox()
    {
        for (int i = 0; i < 7; i++)
        {
            pageDisBox.transform.GetChild(7 - i).GetComponent<PagePromptBox>().SetPageIndex(pageCount - i, pageCount - i == pageIndex);
        }
        pageLeftList[0] = pageCount - 6;
        pageLeftList[1] = pageCount - 5;
        pageLeftList[2] = pageCount - 4;
        pageLeftList[3] = pageCount - 3;
    }
    void CompleteStartPagePromptBox(int pageCnt)
    {
        DestroyMiddleChildrenReverse(pageDisBox.transform);
        for (int i = 1; i <= pageCnt; i++)
        {
            CreatePagePromptBox(i);
            if (i < 4)
            {
                pageLeftList[i] = i + 1;
            }
        }

    }


    private void OnPageIndexButtonClick(int _pageIndex)
    {
        if (_pageIndex == this.pageIndex)
            return;
        this.pageIndex = _pageIndex;
        if (_pageIndex == pageLeftList[0])
        {
            UpdatePagePromptBox(RefreshTiming.OnClickpageLeftListStart);
        }
        else if (_pageIndex == pageLeftList[3])
        {
            UpdatePagePromptBox(RefreshTiming.OnClickpageLeftListLast);
        }
        else
        {
            UpdatePagePromptBox(RefreshTiming.other);
        }
        DisPage();
    }

    public void OnSpecifiedPageConfirmButtonClick(int _pageIndex)
    {
        if (_pageIndex == this.pageIndex || _pageIndex > pageCount || _pageIndex < 1)
            return;
        this.pageIndex = _pageIndex;
        UpdatePagePromptBox(RefreshTiming.OnSpecifiedPageIndex);
        DisPage();
    }
    void CreatePagePromptBox(int _pageIndex)
    {
        var pagePromptBoxObj = Instantiate(pagePromptBoxPfb, pageDisBox.transform);
        pagePromptBoxObj.transform.SetSiblingIndex(pageDisBox.transform.childCount - 2);
        var pagePromptBox = pagePromptBoxObj.GetComponent<PagePromptBox>();
        pagePromptBox.SetPageIndex(_pageIndex, _pageIndex == this.pageIndex);
        pagePromptBox.OnPageIndexButtonClickEvent.AddListener(OnPageIndexButtonClick);
    }

    void CreatePagePromptBox(string character)
    {
        var pagePromptBoxObj = Instantiate(pagePromptBoxPfb, pageDisBox.transform);
        pagePromptBoxObj.transform.SetSiblingIndex(pageDisBox.transform.childCount - 2);
        pagePromptBoxObj.GetComponent<PagePromptBox>().SetPageIndex(character, false);
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
    public void DestroyMiddleChildrenReverse(Transform parent)
    {
        int childCount = parent.childCount;

        if (childCount <= 2) return;

        // 从后向前遍历，避免索引变化问题
        for (int i = childCount - 2; i > 0; i--)
        {
            Destroy(parent.GetChild(i).gameObject);
        }
    }
}
