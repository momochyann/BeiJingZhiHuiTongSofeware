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
    public DeleteEntryCommand(IEntry entry, string entryModelName)
    {
        this.entry = entry;
        this.entryModelName = entryModelName;
    }
    IEntry entry;
    string entryModelName;

    protected override void OnExecute()
    {
        var model = this.GetListModelByName(entryModelName);
        if (model != null)
        {
            model.RemoveItem(entry.can2ListValue);
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
    public EditEntryCommand(IEntry oldEntry, IEntry newEntry, string entryModelName)
    {
        this.oldEntry = oldEntry;
        this.newEntry = newEntry;
        this.entryModelName = entryModelName;
    }
    IEntry oldEntry;
    IEntry newEntry;
    string entryModelName;
    protected override void OnExecute()
    {
        var model = this.GetListModelByName(entryModelName);
        if (model != null)
        {
            model.EditItem(oldEntry.can2ListValue, newEntry.can2ListValue);
            Debug.Log("编辑" + entryModelName);
        }
    }
}

public class AddEntryCommand : AbstractCommand
{
    public AddEntryCommand(IEntry entry, string entryModelName)
    {
        this.entry = entry;
        this.entryModelName = entryModelName;
    }
    IEntry entry;
    string entryModelName;
    protected override void OnExecute()
    {
        var model = this.GetListModelByName(entryModelName);
        if (model != null)
        {
            model.AddItem(entry.can2ListValue);
            Debug.Log("添加" + entryModelName);
        }
        else
        {
            Debug.LogError("找不到" + entryModelName);
        }
    }
}
