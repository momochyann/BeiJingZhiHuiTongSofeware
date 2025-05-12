using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
public class SubjectiveAssessmentArchiveStartPanel2 : PanelBase
{
    // Start is called before the first frame update
    [SerializeField] InputField 描述文本;
    void Start()
    {

    }

    protected override void 返回按钮监听Virtual()
    {
        Destroy(gameObject);
    }
    protected override void 下一步按钮监听Virtual()
    {
        if (描述文本.text == "")
        {
            return;
        }
        var 之前文本 = this.GetSystem<ObjectiveSelectSystem>().当前主观评估.stressEventDescription;
        this.GetSystem<ObjectiveSelectSystem>().当前主观评估.stressEventDescription = 之前文本 + 描述文本.text;
        下一步().Forget();
    }
    async UniTaskVoid 下一步()
    {

        var model = this.GetModel<YooAssetPfbModel>();
        var pfb = await model.LoadPfb("3-4-1-3评估页面");
        var subjectiveAssessmentStartPanel3 = Instantiate(pfb, transform.parent.parent);
        Destroy(gameObject);
    }
}
