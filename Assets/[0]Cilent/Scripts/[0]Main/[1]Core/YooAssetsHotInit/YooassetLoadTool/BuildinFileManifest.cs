using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 内置资源清单
/// </summary>
public class BuildinFileManifest : ScriptableObject
{
    [Serializable]
    public class Element
    {
        public string PackageName;
        public string FileName;
        public string FileCRC32;
    }
    public List<Element> BuildinFiles = new List<Element>();
}