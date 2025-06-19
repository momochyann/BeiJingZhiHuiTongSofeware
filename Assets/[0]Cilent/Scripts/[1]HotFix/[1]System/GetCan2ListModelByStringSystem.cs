using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
public class ModelCreateEvent
{
    public IModel model;
}
// public class ICanListModel : AbstractModel
// {
//     public Dictionary<string, ICan2List> canListDict;
//     protected override void OnInit()
//     {
//         canListDict = new Dictionary<string, ICan2List>();
//         this.RegisterEvent<ModelCreateEvent>(OnModelCreateEvent);
//     }

//     private void OnModelCreateEvent(ICan2List model)
//     {
//         canListDict[model.GetType().Name] = model;
//     }
// }

public class GetCan2ListModelByStringSystem : AbstractSystem
{
    public Dictionary<string, IModel> canListDict;
    protected override void OnInit()
    {
       // canListDict = new Dictionary<string, IModel>();
        
    }

    public void Register2System(IModel model)
    {
        if(canListDict==null)
        {
            canListDict = new Dictionary<string, IModel>();
        }
        canListDict.Add(model.GetType().Name, model);
      //  Debug.Log("注册了" + model.GetType().Name);
    }
     public T GetModel<T>(string modelName) where T : class, IModel
    {
        if (canListDict.TryGetValue(modelName, out var model))
        {
           // Debug.Log("找到了" + model.GetType().Name);
            return model as T;
        }
        
     //   Debug.LogWarning($"找不到Model: {modelName}");
        return null;
    }

}