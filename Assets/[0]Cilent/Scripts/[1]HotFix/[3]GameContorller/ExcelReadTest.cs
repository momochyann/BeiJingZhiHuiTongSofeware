using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using QFramework;
public class TestData
{
    public string Name;
    public string Age;
}
public class ExcelReadTest : MonoBehaviour, IController
{
    // Start is called before the first frame update
    
    void Start()
    {
        ReadData().Forget();
    }
    async UniTaskVoid ReadData()
    {
        var excelTool = this.GetUtility<ExcelReader>();
        excelTool.Init();
        var data = await excelTool.ReadDataAsync<TestData>("TestData");
        foreach (var item in data)
        {
            Debug.Log(item.Name);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
