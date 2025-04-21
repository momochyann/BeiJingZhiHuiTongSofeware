using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;

public class Can2ListModelChangeEvent
{
    public string ModelTypeName;
}

public class Can2ListModelFramework
{

}
public interface ICan2List
{

}
/// <summary>
/// 提供通用列表操作的接口，不依赖泛型参数
/// </summary>
public interface IListModel : IModel
{
    /// <summary>
    /// 添加一个项到列表
    /// </summary>
    /// <param name="item">要添加的项</param>
    /// <param name="insertAtStart">是否插入到列表开头</param>
    void AddItem(ICan2List item, bool insertAtStart = true);

    /// <summary>
    /// 从列表中移除一个项
    /// </summary>
    /// <param name="item">要移除的项</param>
    void RemoveItem(ICan2List item);

    /// <summary>
    /// 清空列表
    /// </summary>
    void ClearItems();
    bool EditItem(ICan2List oldItem, ICan2List newItem);
    // /// <summary>
    // /// 获取所有项
    // /// </summary>
    // /// <returns>列表中的所有项</returns>
    // List<ICan2List> GetAllItems();
}
[Serializable]
public abstract class CrisisIncidentBaseModel<T> : AbstractModel, ICanGetSystem, IListModel where T : ICan2List
{
    protected List<T> dataList;
    protected abstract string GetStorageKey();
    // 保存数据的方法
    protected override void OnInit()
    {
        this.GetSystem<GetCan2ListModelByStringSystem>().Register2System(this);
        LoadData();
    }
    protected void SaveData()
    {
        var storage = this.GetUtility<Storage>();
        this.SendEvent<Can2ListModelChangeEvent>(new Can2ListModelChangeEvent() { ModelTypeName = this.GetType().Name });
        storage.SaveValue(GetStorageKey(), dataList);
    }
    // 加载数据的方法
    protected void LoadData()
    {
        dataList = new List<T>();
        var storage = this.GetUtility<Storage>();
        List<T> loadedData = new List<T>();
        if (storage.GetValue(GetStorageKey(), ref loadedData))
        {
            dataList = loadedData;
        }
    }
    // 提供操作列表的方法，自动调用保存
    public void AddItem(T item, bool InsertOfStart = true)
    {
        if (InsertOfStart)
        {
            dataList.Insert(0, item);
        }
        else
        {
            dataList.Add(item);
        }
        SaveData();
    }
    public void RemoveItem(T item)
    {
        dataList.Remove(item);
        SaveData();
    }
    public void ClearItems()
    {
        dataList.Clear();
        SaveData();
    }
    public bool EditItem(T oldItem, T newItem)
    {
        int index = dataList.IndexOf(oldItem);
        if (index != -1)
        {
            dataList[index] = newItem;
            SaveData();
            return true;
        }
        return false;
    }
    public List<T> GetDataList()
    {
        return dataList;
    }

    /// <summary>
    /// 添加一个ICan2List项到列表
    /// </summary>
    void IListModel.AddItem(ICan2List item, bool insertAtStart)
    {
        if (item is T typedItem)
        {
            AddItem(typedItem, insertAtStart);
        }
        else
        {
            Debug.LogError($"类型不匹配: 期望 {typeof(T).Name}，实际 {item.GetType().Name}");
        }
    }

    /// <summary>
    /// 从列表中移除一个ICan2List项
    /// </summary>
    void IListModel.RemoveItem(ICan2List item)
    {
        if (item is T typedItem)
        {
            RemoveItem(typedItem);
        }
        else
        {
            Debug.LogError($"类型不匹配: 期望 {typeof(T).Name}，实际 {item.GetType().Name}");
        }
    }



    public bool EditItem(ICan2List oldItem, ICan2List newItem)
    {
        if (oldItem is T typedOldItem && newItem is T typedNewItem)
        {
            return EditItem(typedOldItem, typedNewItem);
        }
        else
        {
            Debug.LogError($"类型不匹配: 期望 {typeof(T).Name}，实际 oldItem:{oldItem.GetType().Name}, newItem:{newItem.GetType().Name}");
            return false;
        }
    }

    // /// <summary>
    // /// 获取所有项作为ICan2List列表
    // /// </summary>
    // List<ICan2List> IListModel.GetAllItems()
    // {
    //     return dataList.Cast<ICan2List>().ToList();
    // }
}
public abstract class CrisisIncidentFileBaseModel<T> : CrisisIncidentBaseModel<T> where T : ICan2List
{
    protected void ExportFile()
    {

    }
}