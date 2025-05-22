using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"DOTween.dll",
		"EasySave3.dll",
		"LitJson.dll",
		"Main.dll",
		"QFramework.CoreKit.dll",
		"QFramework.dll",
		"System.Core.dll",
		"UniTask.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<ExcelReader.<GetAllExcelFilesAsync>d__2,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<ExcelReader.<ReadFileManifestAsync>d__3,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<ExcelReader.<ReadObjectiveAssessmentDataAsync>d__1,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<GameStartAfterLoad.<WaitLoadAnimation>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<GroupCrisisIncidentPerSonSelect.<InitAsync>d__8>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<ObjectiveAssessmentStartPanel.<计时>d__16>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<YooAssetPfbModel.<LoadConfig>d__3<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask.<>c<YooAssetPfbModel.<LoadPfb>d__2,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<ExcelReader.<GetAllExcelFilesAsync>d__2,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<ExcelReader.<ReadFileManifestAsync>d__3,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<ExcelReader.<ReadObjectiveAssessmentDataAsync>d__1,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<GameStartAfterLoad.<WaitLoadAnimation>d__2>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<GroupCrisisIncidentPerSonSelect.<InitAsync>d__8>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<ObjectiveAssessmentStartPanel.<计时>d__16>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<YooAssetPfbModel.<LoadConfig>d__3<object>,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTask<YooAssetPfbModel.<LoadPfb>d__2,object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ButtonChangeScene.<加载面板>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<EntryDisPanelNew.<InitAsync>d__19>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<EntryEditorButton.<添加条目按钮监听>d__6>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<EntryEditorButton.<确认面板>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<EntryEditorButton.<编辑条目>d__8>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<EntryEditorSelect.<Init>d__3>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<GroupCrisisIncidentEntry.<LoadImage>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<IndividualInterventionArchiveEntry.<查看按钮监听>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<IndividualInterventionSelectPanel.<开始评估>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<MimaxiangIntervention1.<显示提示面板>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ObjectiveAssessmentArchiveEntry.<查看按钮监听>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ObjectiveAssessmentModel.<LoadObjectiveAssessmentData>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ObjectiveAssessmentPanel.<LoadObjectiveAssessmentPage>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ObjectiveAssessmentSelectPanel.<Init>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ObjectiveAssessmentSelectPanel.<开始评估>d__12>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<ObjectiveAssessmentStartPanel.<Init>d__13>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddGroupCrisisIncident.<加载增加事件面板2>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddGroupCrisisIncident_2.<上一步按钮监听async>d__16>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddGroupCrisisIncident_2.<下一步按钮监听async>d__18>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddGroupCrisisIncident_2.<初始化面板>d__14>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddGroupCrisisIncident_3.<上一步按钮监听async>d__15>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddGroupCrisisIncident_3.<初始化面板>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddIntervener.<添加应激事件属性>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<P_AddPerson.<添加应激事件属性>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<PanelBase.<AsyncStart>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<PerSonGroupSelect.<Init>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<SelectBaseUI.<Animation>d__13>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<SelectBaseUI.<Init>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<SubjectiveAssessmentArchiveEntry.<查看按钮监听>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<SubjectiveAssessmentArchiveSelectPanel.<开始评估>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<SubjectiveAssessmentArchiveStartPanel.<下一步>d__7>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<SubjectiveAssessmentSelectCom.<AsyncInit>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<WorkSceneManager.<LoadPanel>d__4>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<WorkSceneManager.<加载提示>d__6>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<WorkSceneManager.<加载确认提示>d__7>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<WorkSceneManager.<跳转界面>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<WorkScenePanelSelect.<Animation>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid.<>c<WorkScenePanelSelect.<Init>d__8>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ButtonChangeScene.<加载面板>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<EntryDisPanelNew.<InitAsync>d__19>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<EntryEditorButton.<添加条目按钮监听>d__6>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<EntryEditorButton.<确认面板>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<EntryEditorButton.<编辑条目>d__8>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<EntryEditorSelect.<Init>d__3>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<GroupCrisisIncidentEntry.<LoadImage>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<IndividualInterventionArchiveEntry.<查看按钮监听>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<IndividualInterventionSelectPanel.<开始评估>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<MimaxiangIntervention1.<显示提示面板>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ObjectiveAssessmentArchiveEntry.<查看按钮监听>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ObjectiveAssessmentModel.<LoadObjectiveAssessmentData>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ObjectiveAssessmentPanel.<LoadObjectiveAssessmentPage>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ObjectiveAssessmentSelectPanel.<Init>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ObjectiveAssessmentSelectPanel.<开始评估>d__12>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<ObjectiveAssessmentStartPanel.<Init>d__13>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddGroupCrisisIncident.<加载增加事件面板2>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddGroupCrisisIncident_2.<上一步按钮监听async>d__16>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddGroupCrisisIncident_2.<下一步按钮监听async>d__18>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddGroupCrisisIncident_2.<初始化面板>d__14>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddGroupCrisisIncident_3.<上一步按钮监听async>d__15>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddGroupCrisisIncident_3.<初始化面板>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddIntervener.<添加应激事件属性>d__22>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<P_AddPerson.<添加应激事件属性>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<PanelBase.<AsyncStart>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<PerSonGroupSelect.<Init>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<SelectBaseUI.<Animation>d__13>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<SelectBaseUI.<Init>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<SubjectiveAssessmentArchiveEntry.<查看按钮监听>d__10>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<SubjectiveAssessmentArchiveSelectPanel.<开始评估>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<SubjectiveAssessmentArchiveStartPanel.<下一步>d__7>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<SubjectiveAssessmentSelectCom.<AsyncInit>d__9>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<WorkSceneManager.<LoadPanel>d__4>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<WorkSceneManager.<加载提示>d__6>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<WorkSceneManager.<加载确认提示>d__7>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<WorkSceneManager.<跳转界面>d__5>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<WorkScenePanelSelect.<Animation>d__11>
	// Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoid<WorkScenePanelSelect.<Init>d__8>
	// Cysharp.Threading.Tasks.CompilerServices.IStateMachineRunnerPromise<object>
	// Cysharp.Threading.Tasks.ITaskPoolNode<object>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.IUniTaskSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.IUniTaskSource<object>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.Awaiter<object>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.IsCanceledSource<object>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask.MemoizeSource<object>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// Cysharp.Threading.Tasks.UniTask<System.ValueTuple<byte,object>>
	// Cysharp.Threading.Tasks.UniTask<object>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<Cysharp.Threading.Tasks.AsyncUnit>
	// Cysharp.Threading.Tasks.UniTaskCompletionSourceCore<object>
	// DG.Tweening.Core.DOGetter<UnityEngine.Vector3>
	// DG.Tweening.Core.DOGetter<float>
	// DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.Options.VectorOptions>
	// DG.Tweening.Core.TweenerCore<float,float,DG.Tweening.Plugins.Options.FloatOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.Options.VectorOptions>
	// DG.Tweening.Plugins.Core.ABSTweenPlugin<float,float,DG.Tweening.Plugins.Options.FloatOptions>
	// QFramework.Architecture.<>c<object>
	// QFramework.Architecture<object>
	// QFramework.MonoSingleton<object>
	// System.Action<int>
	// System.Action<object,object>
	// System.Action<object>
	// System.Collections.Generic.ArraySortHelper<int>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.Comparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.Comparer<byte>
	// System.Collections.Generic.Comparer<int>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<object,ES3Internal.ES3Data>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,ES3Internal.ES3Data>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,ES3Internal.ES3Data>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,ES3Internal.ES3Data>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,ES3Internal.ES3Data>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<object,ES3Internal.ES3Data>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<ES3Internal.ES3Data>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.EqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.EqualityComparer<byte>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.HashSet.Enumerator<object>
	// System.Collections.Generic.HashSet<object>
	// System.Collections.Generic.HashSetEqualityComparer<object>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,ES3Internal.ES3Data>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<int>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<int>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<object,LitJson.ArrayMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.ObjectMetadata>
	// System.Collections.Generic.IDictionary<object,LitJson.PropertyMetadata>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,ES3Internal.ES3Data>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<int>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,ES3Internal.ES3Data>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<int>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<int>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<object,ES3Internal.ES3Data>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.List.Enumerator<int>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<int>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectComparer<byte>
	// System.Collections.Generic.ObjectComparer<int>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<ES3Internal.ES3Data>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.Collections.Generic.ObjectEqualityComparer<System.ValueTuple<byte,object>>
	// System.Collections.Generic.ObjectEqualityComparer<byte>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<int>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<int>
	// System.Comparison<object>
	// System.Func<int>
	// System.Func<object,byte>
	// System.Func<object,object,object>
	// System.Func<object,object>
	// System.Func<object>
	// System.Linq.Enumerable.<CastIterator>d__99<object>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Linq.Enumerable.WhereSelectArrayIterator<object,object>
	// System.Linq.Enumerable.WhereSelectEnumerableIterator<object,object>
	// System.Linq.Enumerable.WhereSelectListIterator<object,object>
	// System.Predicate<int>
	// System.Predicate<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable.ConfiguredTaskAwaiter<object>
	// System.Runtime.CompilerServices.ConfiguredTaskAwaitable<object>
	// System.Runtime.CompilerServices.TaskAwaiter<object>
	// System.Threading.Tasks.ContinuationTaskFromResultTask<object>
	// System.Threading.Tasks.Task<object>
	// System.Threading.Tasks.TaskFactory.<>c__DisplayClass35_0<object>
	// System.Threading.Tasks.TaskFactory<object>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,System.ValueTuple<byte,object>>>
	// System.ValueTuple<byte,System.ValueTuple<byte,object>>
	// System.ValueTuple<byte,object>
	// UnityEngine.Events.InvokableCall<byte>
	// UnityEngine.Events.InvokableCall<float>
	// UnityEngine.Events.InvokableCall<int>
	// UnityEngine.Events.InvokableCall<object>
	// UnityEngine.Events.UnityAction<byte>
	// UnityEngine.Events.UnityAction<float>
	// UnityEngine.Events.UnityAction<int>
	// UnityEngine.Events.UnityAction<object>
	// UnityEngine.Events.UnityEvent<byte>
	// UnityEngine.Events.UnityEvent<float>
	// UnityEngine.Events.UnityEvent<int>
	// UnityEngine.Events.UnityEvent<object>
	// }}

	public void RefMethods()
	{
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,GameStartAfterLoad.<WaitLoadAnimation>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter&,GameStartAfterLoad.<WaitLoadAnimation>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,ObjectiveAssessmentStartPanel.<计时>d__16>(Cysharp.Threading.Tasks.UniTask.Awaiter&,ObjectiveAssessmentStartPanel.<计时>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,GroupCrisisIncidentPerSonSelect.<InitAsync>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,GroupCrisisIncidentPerSonSelect.<InitAsync>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ExcelReader.<GetAllExcelFilesAsync>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ExcelReader.<GetAllExcelFilesAsync>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,YooAssetPfbModel.<LoadConfig>d__3<object>>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,YooAssetPfbModel.<LoadConfig>d__3<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,YooAssetPfbModel.<LoadPfb>d__2>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,YooAssetPfbModel.<LoadPfb>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ExcelReader.<ReadFileManifestAsync>d__3>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ExcelReader.<ReadFileManifestAsync>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.AwaitUnsafeOnCompleted<System.Runtime.CompilerServices.TaskAwaiter<object>,ExcelReader.<ReadObjectiveAssessmentDataAsync>d__1>(System.Runtime.CompilerServices.TaskAwaiter<object>&,ExcelReader.<ReadObjectiveAssessmentDataAsync>d__1&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<GameStartAfterLoad.<WaitLoadAnimation>d__2>(GameStartAfterLoad.<WaitLoadAnimation>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<GroupCrisisIncidentPerSonSelect.<InitAsync>d__8>(GroupCrisisIncidentPerSonSelect.<InitAsync>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder.Start<ObjectiveAssessmentStartPanel.<计时>d__16>(ObjectiveAssessmentStartPanel.<计时>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<ExcelReader.<GetAllExcelFilesAsync>d__2>(ExcelReader.<GetAllExcelFilesAsync>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<ExcelReader.<ReadFileManifestAsync>d__3>(ExcelReader.<ReadFileManifestAsync>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<ExcelReader.<ReadObjectiveAssessmentDataAsync>d__1>(ExcelReader.<ReadObjectiveAssessmentDataAsync>d__1&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<LoadYooAssetsTool.<LoadAsset>d__4<object>>(LoadYooAssetsTool.<LoadAsset>d__4<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<YooAssetPfbModel.<LoadConfig>d__3<object>>(YooAssetPfbModel.<LoadConfig>d__3<object>&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskMethodBuilder<object>.Start<YooAssetPfbModel.<LoadPfb>d__2>(YooAssetPfbModel.<LoadPfb>d__2&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,EntryDisPanelNew.<InitAsync>d__19>(Cysharp.Threading.Tasks.UniTask.Awaiter&,EntryDisPanelNew.<InitAsync>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,ObjectiveAssessmentSelectPanel.<Init>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter&,ObjectiveAssessmentSelectPanel.<Init>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,SelectBaseUI.<Animation>d__13>(Cysharp.Threading.Tasks.UniTask.Awaiter&,SelectBaseUI.<Animation>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,WorkSceneManager.<LoadPanel>d__4>(Cysharp.Threading.Tasks.UniTask.Awaiter&,WorkSceneManager.<LoadPanel>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ButtonChangeScene.<加载面板>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ButtonChangeScene.<加载面板>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,EntryDisPanelNew.<InitAsync>d__19>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,EntryDisPanelNew.<InitAsync>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,EntryEditorButton.<添加条目按钮监听>d__6>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,EntryEditorButton.<添加条目按钮监听>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,EntryEditorButton.<确认面板>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,EntryEditorButton.<确认面板>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,EntryEditorButton.<编辑条目>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,EntryEditorButton.<编辑条目>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,EntryEditorSelect.<Init>d__3>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,EntryEditorSelect.<Init>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,GroupCrisisIncidentEntry.<LoadImage>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,GroupCrisisIncidentEntry.<LoadImage>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,IndividualInterventionArchiveEntry.<查看按钮监听>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,IndividualInterventionArchiveEntry.<查看按钮监听>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,IndividualInterventionSelectPanel.<开始评估>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,IndividualInterventionSelectPanel.<开始评估>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,MimaxiangIntervention1.<显示提示面板>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,MimaxiangIntervention1.<显示提示面板>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ObjectiveAssessmentArchiveEntry.<查看按钮监听>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ObjectiveAssessmentArchiveEntry.<查看按钮监听>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ObjectiveAssessmentModel.<LoadObjectiveAssessmentData>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ObjectiveAssessmentModel.<LoadObjectiveAssessmentData>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ObjectiveAssessmentPanel.<LoadObjectiveAssessmentPage>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ObjectiveAssessmentPanel.<LoadObjectiveAssessmentPage>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ObjectiveAssessmentSelectPanel.<开始评估>d__12>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ObjectiveAssessmentSelectPanel.<开始评估>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ObjectiveAssessmentStartPanel.<Init>d__13>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ObjectiveAssessmentStartPanel.<Init>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddGroupCrisisIncident.<加载增加事件面板2>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddGroupCrisisIncident.<加载增加事件面板2>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddGroupCrisisIncident_2.<上一步按钮监听async>d__16>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddGroupCrisisIncident_2.<上一步按钮监听async>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddGroupCrisisIncident_2.<下一步按钮监听async>d__18>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddGroupCrisisIncident_2.<下一步按钮监听async>d__18&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddGroupCrisisIncident_2.<初始化面板>d__14>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddGroupCrisisIncident_2.<初始化面板>d__14&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddGroupCrisisIncident_3.<上一步按钮监听async>d__15>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddGroupCrisisIncident_3.<上一步按钮监听async>d__15&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddGroupCrisisIncident_3.<初始化面板>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddGroupCrisisIncident_3.<初始化面板>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddIntervener.<添加应激事件属性>d__22>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddIntervener.<添加应激事件属性>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_AddPerson.<添加应激事件属性>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_AddPerson.<添加应激事件属性>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,PanelBase.<AsyncStart>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,PanelBase.<AsyncStart>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,PerSonGroupSelect.<Init>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,PerSonGroupSelect.<Init>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,SelectBaseUI.<Init>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,SelectBaseUI.<Init>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,SubjectiveAssessmentArchiveEntry.<查看按钮监听>d__10>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,SubjectiveAssessmentArchiveEntry.<查看按钮监听>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,SubjectiveAssessmentArchiveSelectPanel.<开始评估>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,SubjectiveAssessmentArchiveSelectPanel.<开始评估>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,SubjectiveAssessmentArchiveStartPanel.<下一步>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,SubjectiveAssessmentArchiveStartPanel.<下一步>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,SubjectiveAssessmentSelectCom.<AsyncInit>d__9>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,SubjectiveAssessmentSelectCom.<AsyncInit>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,WorkSceneManager.<加载提示>d__6>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,WorkSceneManager.<加载提示>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,WorkSceneManager.<加载确认提示>d__7>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,WorkSceneManager.<加载确认提示>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,WorkSceneManager.<跳转界面>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,WorkSceneManager.<跳转界面>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,WorkScenePanelSelect.<Init>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,WorkScenePanelSelect.<Init>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.YieldAwaitable.Awaiter,WorkScenePanelSelect.<Animation>d__11>(Cysharp.Threading.Tasks.YieldAwaitable.Awaiter&,WorkScenePanelSelect.<Animation>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ButtonChangeScene.<加载面板>d__5>(ButtonChangeScene.<加载面板>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<EntryDisPanelNew.<InitAsync>d__19>(EntryDisPanelNew.<InitAsync>d__19&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<EntryEditorButton.<添加条目按钮监听>d__6>(EntryEditorButton.<添加条目按钮监听>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<EntryEditorButton.<确认面板>d__10>(EntryEditorButton.<确认面板>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<EntryEditorButton.<编辑条目>d__8>(EntryEditorButton.<编辑条目>d__8&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<EntryEditorSelect.<Init>d__3>(EntryEditorSelect.<Init>d__3&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<GroupCrisisIncidentEntry.<LoadImage>d__11>(GroupCrisisIncidentEntry.<LoadImage>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<IndividualInterventionArchiveEntry.<查看按钮监听>d__11>(IndividualInterventionArchiveEntry.<查看按钮监听>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<IndividualInterventionDisPanel.<填入数据>d__13>(IndividualInterventionDisPanel.<填入数据>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<IndividualInterventionSelectPanel.<开始评估>d__11>(IndividualInterventionSelectPanel.<开始评估>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<MimaxiangIntervention1.<显示提示面板>d__5>(MimaxiangIntervention1.<显示提示面板>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ObjectiveAssessmentArchiveEntry.<查看按钮监听>d__11>(ObjectiveAssessmentArchiveEntry.<查看按钮监听>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ObjectiveAssessmentDisPanel.<填入数据>d__11>(ObjectiveAssessmentDisPanel.<填入数据>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ObjectiveAssessmentModel.<LoadObjectiveAssessmentData>d__5>(ObjectiveAssessmentModel.<LoadObjectiveAssessmentData>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ObjectiveAssessmentPanel.<LoadObjectiveAssessmentPage>d__5>(ObjectiveAssessmentPanel.<LoadObjectiveAssessmentPage>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ObjectiveAssessmentSelectPanel.<Init>d__10>(ObjectiveAssessmentSelectPanel.<Init>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ObjectiveAssessmentSelectPanel.<开始评估>d__12>(ObjectiveAssessmentSelectPanel.<开始评估>d__12&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<ObjectiveAssessmentStartPanel.<Init>d__13>(ObjectiveAssessmentStartPanel.<Init>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddGroupCrisisIncident.<加载增加事件面板2>d__11>(P_AddGroupCrisisIncident.<加载增加事件面板2>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddGroupCrisisIncident_2.<上一步按钮监听async>d__16>(P_AddGroupCrisisIncident_2.<上一步按钮监听async>d__16&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddGroupCrisisIncident_2.<下一步按钮监听async>d__18>(P_AddGroupCrisisIncident_2.<下一步按钮监听async>d__18&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddGroupCrisisIncident_2.<初始化面板>d__14>(P_AddGroupCrisisIncident_2.<初始化面板>d__14&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddGroupCrisisIncident_3.<上一步按钮监听async>d__15>(P_AddGroupCrisisIncident_3.<上一步按钮监听async>d__15&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddGroupCrisisIncident_3.<初始化面板>d__10>(P_AddGroupCrisisIncident_3.<初始化面板>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddIntervener.<添加应激事件属性>d__22>(P_AddIntervener.<添加应激事件属性>d__22&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<P_AddPerson.<添加应激事件属性>d__10>(P_AddPerson.<添加应激事件属性>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<PanelBase.<AsyncStart>d__11>(PanelBase.<AsyncStart>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<PerSonGroupSelect.<Init>d__10>(PerSonGroupSelect.<Init>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<SelectBaseUI.<Animation>d__13>(SelectBaseUI.<Animation>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<SelectBaseUI.<Init>d__10>(SelectBaseUI.<Init>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<SubjectiveAssessmentArchiveDisPanel.<填入数据>d__13>(SubjectiveAssessmentArchiveDisPanel.<填入数据>d__13&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<SubjectiveAssessmentArchiveEntry.<查看按钮监听>d__10>(SubjectiveAssessmentArchiveEntry.<查看按钮监听>d__10&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<SubjectiveAssessmentArchiveSelectPanel.<开始评估>d__9>(SubjectiveAssessmentArchiveSelectPanel.<开始评估>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<SubjectiveAssessmentArchiveStartPanel.<下一步>d__7>(SubjectiveAssessmentArchiveStartPanel.<下一步>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<SubjectiveAssessmentSelectCom.<AsyncInit>d__9>(SubjectiveAssessmentSelectCom.<AsyncInit>d__9&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<WorkSceneManager.<LoadPanel>d__4>(WorkSceneManager.<LoadPanel>d__4&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<WorkSceneManager.<加载提示>d__6>(WorkSceneManager.<加载提示>d__6&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<WorkSceneManager.<加载确认提示>d__7>(WorkSceneManager.<加载确认提示>d__7&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<WorkSceneManager.<跳转界面>d__5>(WorkSceneManager.<跳转界面>d__5&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<WorkScenePanelSelect.<Animation>d__11>(WorkScenePanelSelect.<Animation>d__11&)
		// System.Void Cysharp.Threading.Tasks.CompilerServices.AsyncUniTaskVoidMethodBuilder.Start<WorkScenePanelSelect.<Init>d__8>(WorkScenePanelSelect.<Init>d__8&)
		// object DG.Tweening.TweenExtensions.Pause<object>(object)
		// object DG.Tweening.TweenExtensions.Play<object>(object)
		// DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.Options.VectorOptions> DG.Tweening.TweenSettingsExtensions.From<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.Options.VectorOptions>(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3,UnityEngine.Vector3,DG.Tweening.Plugins.Options.VectorOptions>,UnityEngine.Vector3,bool,bool)
		// DG.Tweening.Core.TweenerCore<float,float,DG.Tweening.Plugins.Options.FloatOptions> DG.Tweening.TweenSettingsExtensions.From<float,float,DG.Tweening.Plugins.Options.FloatOptions>(DG.Tweening.Core.TweenerCore<float,float,DG.Tweening.Plugins.Options.FloatOptions>,float,bool,bool)
		// object DG.Tweening.TweenSettingsExtensions.OnComplete<object>(object,DG.Tweening.TweenCallback)
		// object DG.Tweening.TweenSettingsExtensions.SetEase<object>(object,DG.Tweening.Ease)
		// object DG.Tweening.TweenSettingsExtensions.SetLoops<object>(object,int,DG.Tweening.LoopType)
		// object ES3.Deserialize<object>(byte[],ES3Settings)
		// object ES3.Load<object>(string)
		// object ES3.Load<object>(string,ES3Settings)
		// System.Void ES3.Save<object>(string,object)
		// System.Void ES3.Save<object>(string,object,ES3Settings)
		// object ES3File.Load<object>(string)
		// System.Void ES3File.Save<object>(string,object)
		// object ES3Reader.Read<object>(ES3Types.ES3Type)
		// object ES3Reader.Read<object>(string)
		// object ES3Reader.ReadObject<object>(ES3Types.ES3Type)
		// System.Type ES3Reader.ReadTypeFromHeader<object>()
		// object ES3Types.ES3Type.Read<object>(ES3Reader)
		// System.Void ES3Writer.Write<object>(string,object)
		// object LitJson.JsonMapper.ToObject<object>(string)
		// Cysharp.Threading.Tasks.UniTask<object> LoadYooAssetsTool.LoadAsset<object>(string,bool)
		// System.Void QFramework.Architecture<object>.RegisterModel<object>(object)
		// System.Void QFramework.Architecture<object>.RegisterSystem<object>(object)
		// System.Void QFramework.Architecture<object>.RegisterUtility<object>(object)
		// QFramework.IUnRegister QFramework.CanRegisterEventExtension.RegisterEvent<object>(QFramework.ICanRegisterEvent,System.Action<object>)
		// System.Void QFramework.CanSendCommandExtension.SendCommand<object>(QFramework.ICanSendCommand,object)
		// System.Void QFramework.CanSendEventExtension.SendEvent<object>(QFramework.ICanSendEvent,object)
		// QFramework.IUnRegister QFramework.IArchitecture.RegisterEvent<object>(System.Action<object>)
		// System.Void QFramework.IArchitecture.SendCommand<object>(object)
		// System.Void QFramework.IArchitecture.SendEvent<object>(object)
		// System.Void QFramework.IOCContainer.Register<object>(object)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Cast<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.CastIterator<object>(System.Collections.IEnumerable)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Select<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,object>)
		// System.Collections.Generic.List<object> System.Linq.Enumerable.ToList<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Iterator<object>.Select<object>(System.Func<object,object>)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,GameStartAfterLoad.<GameLoadedInit>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter&,GameStartAfterLoad.<GameLoadedInit>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,P_AddGroupCrisisIncident_2.<>c__DisplayClass16_0.<<上一步按钮监听async>b__0>d>(Cysharp.Threading.Tasks.UniTask.Awaiter&,P_AddGroupCrisisIncident_2.<>c__DisplayClass16_0.<<上一步按钮监听async>b__0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter,P_TipPanel.<跳转面板>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter&,P_TipPanel.<跳转面板>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,GameStartAfterLoad.<GameLoadedInit>d__1>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,GameStartAfterLoad.<GameLoadedInit>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,IndividualInterventionSelectPanel.<情绪放松按钮监听>d__8>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,IndividualInterventionSelectPanel.<情绪放松按钮监听>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,LoadNewGameVesionUI.<OnClickConfimButton>d__5>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,LoadNewGameVesionUI.<OnClickConfimButton>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,MimaxiangIntervention1.<保存干预数据>d__6>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,MimaxiangIntervention1.<保存干预数据>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,ObjectiveAssessmentStartPanel.<跳转面板>d__18>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,ObjectiveAssessmentStartPanel.<跳转面板>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.AwaitUnsafeOnCompleted<Cysharp.Threading.Tasks.UniTask.Awaiter<object>,P_TipPanel.<跳转面板>d__11>(Cysharp.Threading.Tasks.UniTask.Awaiter<object>&,P_TipPanel.<跳转面板>d__11&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<GameStartAfterLoad.<GameLoadedInit>d__1>(GameStartAfterLoad.<GameLoadedInit>d__1&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<IndividualInterventionSelectPanel.<情绪放松按钮监听>d__8>(IndividualInterventionSelectPanel.<情绪放松按钮监听>d__8&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<LoadNewGameVesionUI.<OnClickConfimButton>d__5>(LoadNewGameVesionUI.<OnClickConfimButton>d__5&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<MimaxiangIntervention1.<保存干预数据>d__6>(MimaxiangIntervention1.<保存干预数据>d__6&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<ObjectiveAssessmentStartPanel.<跳转面板>d__18>(ObjectiveAssessmentStartPanel.<跳转面板>d__18&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<P_AddGroupCrisisIncident_2.<>c__DisplayClass16_0.<<上一步按钮监听async>b__0>d>(P_AddGroupCrisisIncident_2.<>c__DisplayClass16_0.<<上一步按钮监听async>b__0>d&)
		// System.Void System.Runtime.CompilerServices.AsyncVoidMethodBuilder.Start<P_TipPanel.<跳转面板>d__11>(P_TipPanel.<跳转面板>d__11&)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>(bool)
		// object UnityEngine.Component.GetComponentInParent<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>()
		// object[] UnityEngine.Component.GetComponentsInChildren<object>(bool)
		// object UnityEngine.GameObject.AddComponent<object>()
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>(bool)
		// object UnityEngine.GameObject.GetComponentInParent<object>()
		// object UnityEngine.GameObject.GetComponentInParent<object>(bool)
		// object[] UnityEngine.GameObject.GetComponents<object>()
		// object[] UnityEngine.GameObject.GetComponentsInChildren<object>(bool)
		// object UnityEngine.Object.FindObjectOfType<object>()
		// object[] UnityEngine.Object.FindObjectsOfType<object>()
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object[] UnityEngine.Resources.ConvertObjects<object>(UnityEngine.Object[])
	}
}