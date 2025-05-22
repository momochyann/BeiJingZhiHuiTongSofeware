using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
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
        //    WorkSceneManager.Instance.加载提示("编辑成功");
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
            WorkSceneManager.Instance.加载提示("添加成功");
        }
        else
        {
            Debug.LogError("找不到" + entryModelName);
        }
    }
    // async UniTaskVoid 确认面板(string 提示文本内容, UnityAction 确认回调, string 跳转面板名称 = "")
    // {
    //     var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("确认提示弹窗");
    //     var 确认提示弹窗 = Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<P_TipPanel>();
    //     确认提示弹窗.显示面板(提示文本内容, 跳转面板名称, 确认回调);
    // }
}
