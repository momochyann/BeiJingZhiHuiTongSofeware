using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Linq;
public class EntryOptionCommand : AbstractCommand
{
    protected override void OnExecute()
    {

    }

}

public class DeleteEntryCommand : AbstractCommand
{
    public DeleteEntryCommand(ICan2List deleteValue, string entryModelName)
    {
        this.deleteValue = deleteValue;
        this.entryModelName = entryModelName;
    }
    ICan2List deleteValue;
    string entryModelName;

    protected override void OnExecute()
    {
        var model = this.GetListModelByName(entryModelName);
        if (model != null)
        {
            model.RemoveItem(deleteValue);
            Debug.Log("删除" + entryModelName);
        }
        else
        {
            Debug.LogError("找不到" + entryModelName);
        }
    }

}
public class EditEntryCommand : AbstractCommand
{
    public EditEntryCommand(ICan2List oldValue, ICan2List newValue, string entryModelName)
    {
        this.oldValue = oldValue;
        this.newValue = newValue;
        this.entryModelName = entryModelName;
    }
    ICan2List oldValue;
    ICan2List newValue;
    string entryModelName;
    protected override void OnExecute()
    {
        var model = this.GetListModelByName(entryModelName);
        if (model != null)
        {
            model.EditItem(oldValue, newValue);
            Debug.Log("编辑" + entryModelName);
        }
    }
}

public class AddEntryCommand : AbstractCommand
{
    public AddEntryCommand(ICan2List entry, string entryModelName)
    {
        this.entry = entry;
        this.entryModelName = entryModelName;
    }
    ICan2List entry;
    string entryModelName;
    protected override void OnExecute()
    {
        var model = this.GetListModelByName(entryModelName);
        if (model != null)
        {
            model.AddItem(entry);
            Debug.Log("添加" + entryModelName);
        }
        else
        {
            Debug.LogError("找不到" + entryModelName);
        }
    }
}
