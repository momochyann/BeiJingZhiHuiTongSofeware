using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

public class SearchEntrySystem : AbstractSystem
{
    List<int> 展示条目序号列表;
    public bool isSearch = false;
    protected override void OnInit()
    {
        展示条目序号列表 = new List<int>();
    }


    public List<int> GetShowEntryIndexList()
    {
        return 展示条目序号列表;
    }
    public void SetShowEntryIndexList(List<int> indexList)
    {
        展示条目序号列表 = indexList;
    }
}
