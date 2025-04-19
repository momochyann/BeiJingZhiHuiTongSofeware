using NPOI.XWPF.UserModel;

using System.IO;
using QFramework;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;

public class WordTableRead : IUtility
{
    async public UniTask<List<T>> ReadDataAsync<T>(string filePath) where T : new()
    {
        var datas = new List<T>();
        await UniTask.Yield();
        bool titleRead = false;
        Debug.Log("ReadWord");
        try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                XWPFDocument doc = new XWPFDocument(fs);
                var tables = doc.Tables;
            //    Debug.Log("ReadWord1");
                if (tables.Count > 0)
                {
                    var table = tables[0]; // Assuming we're reading the first table
                    var rows = table.Rows;
                    // Debug.Log("ReadWord2");
                    foreach (var row in rows)
                    {
                        if (!titleRead)
                        {
                            titleRead = true;
                            continue;
                        }
                     
                        int index = 0;
                        var data = new T();
                        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                        {
                            string cellValue = row.GetCell(index)?.GetText();
                            if (string.IsNullOrEmpty(cellValue))
                            {
                                field.SetValue(data, "");
                            }
                            else
                            {
                                field.SetValue(data, cellValue);
                            }
                            index++;
                        }
                        //   Debug.Log("ReadWord3-"+index);
                        datas.Add(data);
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error reading Word document: {e.Message}");
        }

        return datas;
    }
}