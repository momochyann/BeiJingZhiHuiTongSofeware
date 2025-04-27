using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public static class CommandExtensions
{
    /// <summary>
    /// 通过名称获取Model并转换为IListModel
    /// </summary>
    public static IListModel GetListModelByName(this AbstractCommand command, string modelName)
    {
        var model = command.GetSystem<GetCan2ListModelByStringSystem>().GetModel<IListModel>(modelName);
        return model;
    }
    public static IListModel GetListModelByName(this AbstractCommand<List<int>> command, string modelName)
    {
        var model = command.GetSystem<GetCan2ListModelByStringSystem>().GetModel<IListModel>(modelName);
        return model;
    }
    public static IListModel GetListModelByName(this IController controller, string modelName)
    {
        var model = controller.GetSystem<GetCan2ListModelByStringSystem>().GetModel<IListModel>(modelName);
        return model;
    }
  
}
