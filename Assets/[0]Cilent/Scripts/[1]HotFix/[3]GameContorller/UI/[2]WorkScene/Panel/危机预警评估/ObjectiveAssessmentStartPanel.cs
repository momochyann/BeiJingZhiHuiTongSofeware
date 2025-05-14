using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;
using System;
public class ObjectiveAssessmentStartPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField]
    Button 返回按钮;
    [SerializeField]
    Text 时间显示;
    [SerializeField]
    Text 题目;
    [SerializeField]
    Text 题序;
    [SerializeField]
    GameObject 选项客观评估选项;
    [SerializeField]
    Button 下一题按钮;
    [SerializeField]
    GameObject 评估选项布局;
    ObjectiveAssessment 当前量表 => this.GetSystem<ObjectiveSelectSystem>().当前量表;
    int 当前题序 => this.GetSystem<ObjectiveSelectSystem>().当前题序;
    ObjectiveSelectSystem 得分系统;
    void Start()
    {
        Init().Forget();
    }
    async UniTaskVoid Init()
    {
        得分系统 = this.GetSystem<ObjectiveSelectSystem>();
        选项客观评估选项 = await this.GetModel<YooAssetPfbModel>().LoadPfb("客观评估选项");
        下一题按钮.onClick.AddListener(OnNextButtonClick);
        计时().Forget();
        得分系统.当前题序 = 0;
        更新题目();
        返回按钮.onClick.AddListener(返回按钮点击);
    }
    void 返回按钮点击()
    {
        if (得分系统.当前题序 > 0)
        {
            得分系统.当前题序--;
            更新题目();
        }
        else
        {
            //TODO: 提示没有题目
        }
    }
    void 更新题目()
    {
        for (int i = 0; i < 评估选项布局.transform.childCount; i++)
        {
            Destroy(评估选项布局.transform.GetChild(i).gameObject);
        }
        题目.text = 当前量表.题目列表[当前题序].题目名称;
        题序.text = "第" + (当前题序 + 1).ToString() + "题" + "/共" + 当前量表.题目列表.Count + "题";
        for (int i = 0; i < 当前量表.题目列表[当前题序].选项.Count; i++)
        {
            var 选项 = Instantiate(选项客观评估选项, 评估选项布局.transform);
            选项.GetComponentInChildren<Text>().text = 当前量表.题目列表[当前题序].选项[i];
            选项.transform.GetComponentInChildren<Text>().color = Color.blue;
            选项.gameObject.name = 当前量表.题目列表[当前题序].选项[i];
            选项.GetComponent<Button>().onClick.AddListener(OnButtonClick);
            选项.GetComponent<ProceduralImage>().BorderWidth = 1;

        }
    }
    async UniTask 计时()
    {
        var 计时Token = this.GetCancellationTokenOnDestroy();
        int time = 0;
        while (true)
        {
            // await UniTask.Delay(1000, cancellationToken: 倒计时Token);
            await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: 计时Token);
            time++;
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            时间显示.text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
    private void OnNextButtonClick()
    {
        bool 是否回答 = false;
        for (int i = 0; i < 评估选项布局.transform.childCount; i++)
        {
            if (评估选项布局.transform.GetChild(i).GetComponent<ProceduralImage>().BorderWidth == 0)
            {
                Debug.Log("当前题序:" + 当前题序 + "当前量表:" + 当前量表.题目列表[当前题序].分值.Count);
                得分系统.当前量表得分.Add(int.Parse(当前量表.题目列表[当前题序].分值[i]));
                是否回答 = true;
            }
        }
        if (是否回答)
        {
            得分系统.当前题序++;
            if (得分系统.当前题序 < 当前量表.题目列表.Count)
            {
                更新题目();
            }
            else
            {
                var 客观评估存档 = new ObjectiveAssessmentArchive();
                客观评估存档.name = 得分系统.当前人员.name;
                客观评估存档.gender = 得分系统.当前人员.gender;
                客观评估存档.category = 得分系统.当前人员.category;
                客观评估存档.Interveners = 得分系统.当前干预人员;
                客观评估存档.scaleName = 当前量表.量表名称;
                // 客观评估存档.description = 得分系统.当前人员.description;
                客观评估存档.isCreateReport = false;
                客观评估存档.FormIntroduction = 当前量表.量表简介;
                int 得分 = 0;
                foreach (var item in 得分系统.当前量表得分)
                {
                    得分 += item;
                }
                var 记分题目 = 当前量表.记分规则.报告[0].记分题目;
                if (记分题目[0] == 0)
                {
                    if (记分题目.Count == 1)
                    {
                        for (int i = 0; i < 得分系统.当前量表得分.Count; i++)
                        {
                            得分 += 得分系统.当前量表得分[i];
                        }
                    }
                    else
                    {
                        for (int i = 记分题目[1]; i < 记分题目[2]; i++)
                        {
                            得分 += 得分系统.当前量表得分[i];
                        }
                    }
                }
                else
                {
                    foreach (var item in 记分题目)
                    {
                        得分 = 得分系统.当前量表得分[item];
                    }
                }

                for (int i = 0; i < 当前量表.记分规则.报告[0].结果类别.Count; i++)
                {
                    if (得分 >= 当前量表.记分规则.报告[0].结果类别[i].数始 && 得分 <= 当前量表.记分规则.报告[0].结果类别[i].数至)
                    {
                        客观评估存档.ScoreSituation = 当前量表.记分规则.报告[0].结果类别[i].结果文案;
                        break;
                    }
                }
                客观评估存档.createDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                this.SendCommand<AddEntryCommand>(new AddEntryCommand(客观评估存档, "ObjectiveAssessmentArchiveModel"));
                //TODO: 显示结果
            }
        }
        else
        {
            //TODO: 提示未回答
        }
    }

    private void OnButtonClick()
    {
        var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        Debug.Log("button.gameObject.name: " + button.gameObject.name);
        for (int i = 0; i < 评估选项布局.transform.childCount; i++)
        {
            var _button = 评估选项布局.transform.GetChild(i).GetComponent<Button>();
            if (_button.gameObject.name == button.gameObject.name)
            {
                _button.gameObject.GetComponent<ProceduralImage>().BorderWidth = 0;
                _button.transform.GetComponentInChildren<Text>().color = Color.white;
            }
            else
            {
                _button.gameObject.GetComponent<ProceduralImage>().BorderWidth = 1;
                _button.transform.GetComponentInChildren<Text>().color = Color.blue;
            }
        }
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
