using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using QFramework;
public class SearchByNameCommand : AbstractCommand
{
    public SearchByNameCommand(string keyword, string modelName)
    {
        this.keyword = keyword;
        this.modelName = modelName;
    }
    string keyword;
    string modelName;

    protected override void OnExecute()
    {
        var model = this.GetListModelByName(modelName);
        var indexList = model.SearchByName(keyword);
        this.GetSystem<SearchEntrySystem>().SetShowEntryIndexList(indexList);
    }
}
