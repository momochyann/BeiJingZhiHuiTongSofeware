using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.IO;
using ExcelDataReader;
using System.Reflection;
using Cysharp.Threading.Tasks;
using System.Text;
using YooAsset;
public class ExcelReader : IUtility
{
    public void Init()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    async public UniTask<List<T>> ReadDataAsync<T>(string excelFileName) where T : new()
    {
        var datas = new List<T>();
        var handle = await LoadYooAssetsTool.LoadRawFile_DP(excelFileName);
        var stream = new MemoryStream(handle);
        bool titleRead = false;
        using (stream)
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                while (reader.Read())
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
                        if (reader.GetString(index) == null)
                        {
                            //Debug.Log("readdate is null");
                            field.SetValue(data, "");
                            index++;
                        }
                        else
                        {
                            //Debug.Log(reader.GetString(index));
                            field.SetValue(data, reader.GetString(index++));
                        }
                    }
                    datas.Add(data);
                }
            }
            return datas;
        }
    }

}
