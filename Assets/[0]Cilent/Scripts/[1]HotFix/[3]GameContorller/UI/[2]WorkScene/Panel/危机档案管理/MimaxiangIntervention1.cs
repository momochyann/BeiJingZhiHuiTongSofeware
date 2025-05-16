using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;

public class MimaxiangIntervention1 : PanelBase
{
    // Start is called before the first frame update
    public List<InputField> 密码箱描述 = new List<InputField>();
    [SerializeField] int index;
    protected override void Start()
    {
        base.Start();

    }

    protected override void 下一步按钮监听Virtual()
    {
        foreach (var 描述 in 密码箱描述)
        {
            if (描述.text == "")
            {
                return;
            }
        }
        foreach (var 描述 in 密码箱描述)
        {
            this.GetSystem<InterventionSystem>().interventionDescription[index] += 描述.text;
        }
        base.下一步按钮监听Virtual();
    }
    protected override void 保存按钮监听Virtual()
    {
        var 当前人员 = this.GetSystem<InterventionSystem>().当前人员;
        this.GetSystem<InterventionSystem>().当前干预档案.name = 当前人员.name;
        this.GetSystem<InterventionSystem>().当前干预档案.gender = 当前人员.gender;
        this.GetSystem<InterventionSystem>().当前干预档案.category = 当前人员.category;
        this.GetSystem<InterventionSystem>().当前干预档案.isCreateReport = false;
        this.GetSystem<InterventionSystem>().当前干预档案.FangAnName = "密码箱";
        this.GetSystem<InterventionSystem>().当前干预档案.interventionDescription = "";
        string interventionDescription = "";
        foreach (var 描述 in this.GetSystem<InterventionSystem>().interventionDescription)
        {
            interventionDescription += 描述;
        }

        this.GetSystem<InterventionSystem>().当前干预档案.interventionDescription = interventionDescription;
        显示提示面板().Forget();
    }
    async UniTaskVoid 显示提示面板()
    {
        var 提示面板pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("确认提示弹窗");
        var P_TipPanel = Instantiate(提示面板pfb, FindObjectOfType<Canvas>().transform);
        P_TipPanel.GetComponent<P_TipPanel>().显示面板("是否保存干预数据");
        P_TipPanel.GetComponent<P_TipPanel>().确认事件 += 保存干预数据;
    }
    async void 保存干预数据()
    {
        this.SendCommand(new AddEntryCommand(this.GetSystem<InterventionSystem>().当前干预档案, "IndividualInterventionArchiveModel"));
        var 危机干预实施选择界面pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("危机干预实施选择界面");
        var 危机干预实施选择界面 = Instantiate(危机干预实施选择界面pfb, transform.parent);
        Destroy(gameObject);
    }
}
