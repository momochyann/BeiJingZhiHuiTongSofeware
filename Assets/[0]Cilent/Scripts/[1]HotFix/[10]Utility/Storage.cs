using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using LitJson;
using System.Runtime.CompilerServices;

public class Storage : IUtility
{
    public void SaveValue<T>(string key, T value)
    {
        var _value = JsonMapper.ToJson(value);
        ES3.Save(key, _value);
        Debug.Log("SaveValue: " + key);
    }
    public bool GetValue<T>(string key, ref T value)
    {
        if (ES3.KeyExists(key))
        {
            var _value = ES3.Load<string>(key);
            value = JsonMapper.ToObject<T>(_value);
            Debug.Log("GetValue: " + key + " " + value);
            return true;
        }
        return false;
    }
    public bool GetValue<T>(string key, ref List<T> value)
    {
        if (ES3.KeyExists(key))
        {
            var _value = ES3.Load<string>(key);
            value = JsonMapper.ToObject<List<T>>(_value);
            Debug.Log("GetValue: " + key + " " + value);
            return true;
        }
        return false;
    }
    public static string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

}

