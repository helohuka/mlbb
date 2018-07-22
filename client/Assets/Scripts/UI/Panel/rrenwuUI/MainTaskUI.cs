
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainTaskUI : MonoBehaviour {
/// <summary>
/// 主面板 UI 
/// </summary>
	/// 
    public UIButton RightButton;
	public UIButton TaskButton;
	public UIButton TeamButton;
	public UIButton creataTeamBtn;
	public GameObject tasksObj;
	public GameObject teamObj;
	public GameObject[] taskObjs;
	public UILabel[] mubiaoLabels;
	public GameObject item;
	public UIGrid grid;
	public GameObject questitem;
	public UIGrid questgrid;
    public UILabel[] Tasks;
    UIPlayTween PlayTween;
    static bool IsOpen = false;
	public UISprite raSp;
	public UIScrollBar svb;
	public GameObject chenkPlayerObj;

	public GameObject tipsObj;
	public UIButton ZSBtn;
	public UIButton LKbtn;
	public UIButton ZHBtn;
	public UIButton JHYBtn;
	public UIButton CKBtn;
	public UIButton GDBtn;
	public UIButton tipsCloseBtn;

	public UIPanel teamPanle;

    const int MAIN_TaskLableIndex = 0;
    const int SUB_TaskLableIndex = 1;
    const int MAX_LableLength =3;
	List<GameObject> listObj = new List<GameObject>();
	void Start () {
		item.SetActive (false);
		SwitchBtnSprite (false);
		teamObj.SetActive (false);
		tasksObj.SetActive (true);

		//questitem.SetActive(false);
        PlayTween = RightButton.GetComponent<UIPlayTween>();
		UIManager.SetButtonEventHandler (TaskButton.gameObject, EnumButtonEvent.OnClick, OnClickPTaskBtn, 0, 0);

		UIManager.SetButtonEventHandler (TeamButton.gameObject, EnumButtonEvent.OnClick, OnClickPTeamButton, 0, 0);

        UIManager.SetButtonEventHandler(RightButton.gameObject, EnumButtonEvent.OnClick, OnRightButtonDown, 0, 0);
        //QuestSystem.OnQuestUpdate += OnQuestUpdateOk;
		if(QuestSystem.CurrentList.Count == 0)
		{
			PlayerPrefs.SetInt("xysk_miniQuest_open", 0);
		}else
		{
			PlayerPrefs.SetInt("xysk_miniQuest_open", 1);
		}
        IsOpen = PlayerPrefs.GetInt("xysk_miniQuest_open") == 1;
        if (IsOpen)
        {
            PlayTween.Play(IsOpen);
        }
		if(IsOpen)
		{
			//raSp.transform.rotation = new Quaternion(raSp.transform.rotation.x,180,raSp.transform.rotation.z,raSp.transform.rotation.w);
			TweenRotation rot = raSp.GetComponent<TweenRotation>();
			rot.from = new Vector3(0,0,0);
			rot.to = new Vector3(0,180,0);

		}else
		{
			TweenRotation rot = raSp.GetComponent<TweenRotation>();

			rot.from = new Vector3(0,180,0);
			rot.to = new Vector3(0,0,0);
		}
        GuideManager.Instance.RegistGuideAim(TeamButton.gameObject, GuideAimType.GAT_MainTeamBtn);
        GuideManager.Instance.RegistGuideAim(TaskButton.gameObject, GuideAimType.GAT_MainTaskBtn);
        questitem.SetActive(false);
		//GuideManager.Instance.RegistGuideAim(questitem.transform.parent.gameObject, GuideAimType.GAT_QuestMiniFirst);
//		GuideManager.Instance.RegistGuideAim(questItems[1].transform.parent.gameObject, GuideAimType.GAT_QuestMiniSecond);
//		GuideManager.Instance.RegistGuideAim(questItems[2].transform.parent.gameObject, GuideAimType.GAT_QuestMiniThird);

       
		if(TeamSystem.IsInTeam())
		{
			if (grid == null)
				return;
			foreach(Transform tr in grid.transform)
			{
				Destroy(tr.gameObject);
			}
//			teamPanle.clipOffset = Vector2.zero;
//			teamPanle.transform.localPosition = Vector3.zero;
//			teamPanle.GetComponent<UIScrollView>().ResetPosition();
//			teamPanle.GetComponent<UIPanel>().clipOffset = Vector2.zero;
//			teamPanle.transform.localPosition = Vector3.zero;
			AddTeamItem(TeamSystem.GetTeamMembers());
			creataTeamBtn.gameObject.SetActive(false);
		}else
		{
			creataTeamBtn.gameObject.SetActive(true);
			UIManager.SetButtonEventHandler (creataTeamBtn.gameObject, EnumButtonEvent.OnClick, OnClickcreataTeamBtn, 0, 0);
		}
		TeamSystem.OnUpdateMainteamList += UpdateMainteamList;
		QuestSystem.OnQuestEffectFinish += TaskLabe;
		TeamSystem.OnTeamDirtyProps += updatePros;

		if(QuestSystem.isTaskF)
		{
			ResetTaskLabe();
			QuestSystem.isTaskF = false;
		}
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Castle))
		{
			TeamButton.pressedSprite = "duiwuan";
			TeamButton.normalSprite = "duiwuan";
		}
		OnQuestUpdateOk();
    }

	void OnEnable()
	{
		if(TeamSystem.IsInTeam())
		{
			updatePros(TeamSystem.GetTeamMembers());
		}
		if(TeamSystem.IsInTeam())
		{
			if (grid == null)
				return;
			foreach(Transform tr in grid.transform)
			{
				Destroy(tr.gameObject);
			}

			AddTeamItem(TeamSystem.GetTeamMembers());
			creataTeamBtn.gameObject.SetActive(false);
		}else
		{
			creataTeamBtn.gameObject.SetActive(true);
			UIManager.SetButtonEventHandler (creataTeamBtn.gameObject, EnumButtonEvent.OnClick, OnClickcreataTeamBtn, 0, 0);
		}
	}
	void updatePros(COM_SimplePlayerInst [] PlayerInsts)
	{
		updatecellsInfo (PlayerInsts);
	}
	void UpdateMainteamList()
	{
		if(TeamSystem.GetTeamMembers().Length==0)
		{
			creataTeamBtn.gameObject.SetActive(true);
		}else
		{
			creataTeamBtn.gameObject.SetActive(false);
		}
		if (grid == null)
						return;
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
//		teamPanle.clipOffset = Vector2.zero;
//		teamPanle.transform.localPosition = Vector3.zero;
//		teamPanle.GetComponent<UIScrollView>().ResetPosition();
//		teamPanle.GetComponent<UIPanel>().clipOffset = Vector2.zero;
//		teamPanle.transform.localPosition = Vector3.zero;
		AddTeamItem (TeamSystem.GetTeamMembers());
	}
    void Update()
    {
        if(QuestSystem.isDirty)
        {
            OnQuestUpdateOk();
            QuestSystem.isDirty = false;
        }
    }

    void OnRightButtonDown(ButtonScript obj, object args, int param1, int param2)
    {
        PlayTween.Play(IsOpen = !IsOpen);
        PlayerPrefs.SetInt("xysk_miniQuest_open", IsOpen? 1: 0);
    }
	int sor(COM_QuestInst q1,COM_QuestInst q2)
	{
		QuestData qdata = QuestData.GetData (q1.questId_);
		if(qdata.questKind_ == QuestKind.QK_Main)
			return 1;

		if(QuestSystem.IsQuestFinish(q1.questId_) && !QuestSystem.IsQuestFinish(q2.questId_))
		{
			return -1;
		}else
		{
			return 1;
		}

		return 0;
	}
    List<GameObject> _QuestItems = new List<GameObject>();
    void RefreshTaskItemData()
    {
        questgrid.transform.DetachChildren();
        foreach (GameObject go in _QuestItems)
        {
            GameObject.Destroy(go);
        }
        _QuestItems.Clear();
		MainTaskUICell mcell = null;
		//第一个永远显示主线任务 不要乱动
		GameObject clone = null;
		if(!CopyData.IsCopyScene(GameManager.SceneID))
		{

			clone = GameObject.Instantiate(questitem) as GameObject;
			_QuestItems.Add(clone);
			questgrid.AddChild(clone.transform);
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			clone.SetActive(true);
		    mcell = clone.GetComponent<MainTaskUICell>();

        
        if (QuestSystem.IsMainKindEmpty())
        {///没有主线任务
                
            int next = QuestSystem.GetFirstAcceptableMainKindId();
            if (next == 0)
            {
                next = QuestSystem.GetFirstComplateMainKindId();
                if (next != 0)
                {
                    QuestData qdata = QuestData.GetData(next);
                    qdata = QuestData.GetData(qdata.postQuest_);
					if(qdata != null)
					{
	                    mcell.targetLabel.text = "";
	                    mcell.descLabel.text = string.Format("需要升到{0}级才能接任务！", qdata.needLevel_);
					}
                }
            }
            else
            {
                QuestData qdata = QuestData.GetData(next);
                mcell.QData = qdata;
                //mcell.targetLabel.text = "";
                //mcell.descLabel.text =  "接任务！";
                GetInfoOnClick gioc = clone.GetComponent<GetInfoOnClick>();
                if (gioc == null)
                    gioc = clone.gameObject.AddComponent<GetInfoOnClick>();
                gioc.param_ = qdata.id_;
            }
        }
        else
        {
//			if(!CopyData.IsCopyScene(GameManager.SceneID))
//			{
				mcell.QuestInst = QuestSystem.GetDoingMainKind();
				GetInfoOnClick gioc = clone.GetComponent<GetInfoOnClick>();
				if (gioc == null)
					gioc = clone.gameObject.AddComponent<GetInfoOnClick>();
				gioc.param_ = mcell.QuestInst.questId_;
//			}
//           
            
        }
		}




		for(int i =0;i<QuestSystem.CurrentList.Count;i++)
		{
            QuestData qdata = QuestData.GetData((int)QuestSystem.CurrentList[i].questId_);
            
			if(CopyData.IsCopyScene(GameManager.SceneID))
			{
				if(qdata.questKind_ != QuestKind.QK_Copy)
					continue;
			}else
			{
				if (qdata.questKind_ == QuestKind.QK_Main)
					continue; ///主线任务 去你妈的
			}

			clone = GameObject.Instantiate(questitem)as GameObject;
            _QuestItems.Add(clone);
			clone.SetActive(true);
			
			UIManager.SetButtonEventHandler (clone, EnumButtonEvent.OnClick, OnClickReceive,qdata.id_, 0);
            questgrid.AddChild(clone.transform);
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			mcell = clone.GetComponent<MainTaskUICell>();
			mcell.QuestInst = QuestSystem.CurrentList[i];
			GetInfoOnClick gioc = clone.GetComponent<GetInfoOnClick>();
			if(gioc == null)
				gioc = clone.gameObject.AddComponent<GetInfoOnClick>();
			gioc.param_ = QuestSystem.CurrentList[i].questId_;
			questgrid.repositionNow = true;
		}


			if (_QuestItems.Count> 0)
				GuideManager.Instance.RegistGuideAim(_QuestItems[0], GuideAimType.GAT_QuestMiniFirst);
			if (_QuestItems.Count > 1)
				GuideManager.Instance.RegistGuideAim(_QuestItems[1], GuideAimType.GAT_QuestMiniSecond);


            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainTaskFlushOk);
	}
	
	void OnClickReceive(ButtonScript obj, object args, int param1, int param2)
	{
//		GetInfoOnClick gioc = obj.GetComponent<GetInfoOnClick>();
//		if(gioc == null)
//			gioc = obj.gameObject.AddComponent<GetInfoOnClick>();
//		gioc.param_ = param1;
	}

	void OnQuestUpdateOk()
    {
		QuestSystem.CurrentList.Sort (sor);
		RefreshTaskItemData ();
//        for (int i = 0; i < Tasks.Length; i++)
//        {
//            Tasks[i].text = "";
//			mubiaoLabels[i].text = "";
//			taskObjs[i].SetActive(false);
//        }

//        string goal_str = "";
//
//
//        List<COM_QuestInst> qlist = QuestSystem.TrackQuests;
//		if(qlist.Count == 0)
//		{
//
//			Tasks[MAIN_TaskLableIndex].text = "当前没有追踪任务";
//			taskObjs[MAIN_TaskLableIndex].SetActive(true);
//		}
//		if (QuestSystem.IsMainKindEmpty())
//        {///没有主线任务
//
//            int next = QuestSystem.GetFirstAcceptableMainKindId();
//            if (next == 0)
//            {
//                next = QuestSystem.GetFirstComplateMainKindId();
//                if (next != 0)
//                {
//                    QuestData qdata = QuestData.GetData(next);
//                    qdata = QuestData.GetData(qdata.postQuest_);
//					goal_str = string.Format("需要升到{0}级才能接任务！", qdata.needLevel_);
//                }
//            }
//            else
//            {
//                QuestData qdata = QuestData.GetData(next);
//                goal_str = "接任务！";
//            }
//
//            Tasks[MAIN_TaskLableIndex].text = goal_str;
//			taskObjs[MAIN_TaskLableIndex].SetActive(true);
//        }
//        

//        for (int i = 0; i < MAX_LableLength  && i < qlist.Count; i++)
//        {
//
//            if (qlist[i] == null) 
//			{
//				UISprite sp = Tasks[i].GetComponentInParent<UISprite>();
//				sp.spriteName = "renwutiao";
//				continue;
//			}
//			taskObjs[i].SetActive(true);
//            QuestData qdata = QuestData.GetData((int)qlist[i].questId_);           
//            if (QuestSystem.IsQuestFinish(qdata.id_))
//			{
//
//				if(qdata.questKind_ == QuestKind.QK_Main)
//				{
//					//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("mainQuset"),qdata.questName_+"\n"+qdata.miniFinDesc_);
//					Tasks[i].text = string.Format("{0}    {1}",LanguageManager.instance.GetValue("mainQuset"),qdata.questName_);
//					mubiaoLabels[i].text = qdata.miniFinDesc_;
//				}
//				else//(qdata.questKind_ == QuestKind.QK_Sub)
//				{
//					if(qdata.questKind_ == QuestKind.QK_Profession)
//					{
//						//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Profession"),qdata.questName_+"\n"+qdata.miniFinDesc_);
//						Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Profession"),qdata.questName_);
//						mubiaoLabels[i].text = qdata.miniFinDesc_;
//					}else
//						if(qdata.questKind_ == QuestKind.QK_Tongji)
//					{
//						//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Tongji"),qdata.questName_+"\n"+qdata.miniFinDesc_);
//						Tasks[i].text = string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Tongji"),qdata.questName_);
//						mubiaoLabels[i].text = qdata.miniFinDesc_;
//					}else
//						if(qdata.questKind_ == QuestKind.QK_Sub)
//					{
//						//Tasks[i].text =string.Format("{0}      {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
//						Tasks[i].text = string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_);
//						mubiaoLabels[i].text = qdata.miniFinDesc_;
//					}
//
//				}
//
//			}
//                
//            else
//			{
//				if(qdata.questKind_ == QuestKind.QK_Main)
//				{
//					//Tasks[i].text = string.Format("{0}     {1}({2}/{3})",LanguageManager.instance.GetValue("mainQuset") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
//					Tasks[i].text = string.Format("{0}     {1}",LanguageManager.instance.GetValue("mainQuset") ,qdata.questName_);
//					mubiaoLabels[i].text = string.Format("{0}({1}/{2})",qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
//				}
//                else
//				{
//					if(qdata.questKind_ == QuestKind.QK_Profession)
//					{
//					
//						//Tasks[i].text = string.Format("{0}    {1}({2}/{3})",LanguageManager.instance.GetValue("QK_Profession") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
//						Tasks[i].text = string.Format("{0}     {1}",LanguageManager.instance.GetValue("QK_Profession") ,qdata.questName_);
//						mubiaoLabels[i].text = string.Format("{0}({1}/{2})",qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
//					}else
//						if(qdata.questKind_ == QuestKind.QK_Tongji)
//					{
//						//Tasks[i].text = string.Format("{0}    {1}({2}/{3})",LanguageManager.instance.GetValue("QK_Tongji") ,qdata.questName_+"\n"+qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
//						Tasks[i].text = string.Format("{0}     {1}",LanguageManager.instance.GetValue("QK_Tongji") ,qdata.questName_);
//						mubiaoLabels[i].text = string.Format("{0}({1}/{2})",qdata.miniDesc_, qlist[i].questNum_, qdata.targetNum_);
//					}else
//						if(qdata.questKind_ == QuestKind.QK_Sub)
//					{
//						//Tasks[i].text =string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_+"\n"+qdata.miniFinDesc_);
//						Tasks[i].text = string.Format("{0}    {1}",LanguageManager.instance.GetValue("QK_Sub"),qdata.questName_);
//						mubiaoLabels[i].text = qdata.miniFinDesc_;
//					}
//
//				}
//
//			}      
//			GetInfoOnClick gioc = taskObjs[i].GetComponent<GetInfoOnClick>();
//			if(gioc == null)
//				gioc = taskObjs[i].gameObject.AddComponent<GetInfoOnClick>();
//			gioc.param_ = qdata.id_;

//			GetInfoOnClick gioc = mubiaoLabels[i].GetComponent<GetInfoOnClick>();
//            if(gioc == null)
//				gioc = mubiaoLabels[i].gameObject.AddComponent<GetInfoOnClick>();
//            gioc.param_ = qdata.id_;
//        }

   
        if (QuestSystem.HasNewQuest() && !IsOpen)
        {
            PlayTween.Play(IsOpen = true);
            PlayerPrefs.SetInt("xysk_miniQuest_open", IsOpen ? 1 : 0);
        }
    }
	void TaskLabe(int qid)
	{
		for(int i=0;i<QuestSystem.CurrentList.Count &&i<Tasks.Length;i++)
		{
			if(qid == QuestSystem.CurrentList[i].questId_)
			{
				UISprite sps = Tasks[i].GetComponentInParent<UISprite>();
				if(sps != null)
					sps.spriteName = "renwutiao";
			}
		}
	}
	void ResetTaskLabe()
	{
		for(int i=0;i<QuestSystem.CurrentList.Count &&i<Tasks.Length;i++)
		{
			if(QuestSystem.sqid == QuestSystem.CurrentList[i].questId_)
			{
				UISprite sps = Tasks[i].GetComponentInParent<UISprite>();
				if(sps != null)
					sps.spriteName = "renwutiaozhuxian";
			}
		}
	}
	List<MianteamListCell> cells = new List<MianteamListCell>();
	void AddTeamItem(COM_SimplePlayerInst[] infos)
	{
		for(int i =0;i<infos.Length;i++)
		{
			GameObject clone = GameObject.Instantiate(item)as GameObject;
			clone.SetActive(true);
			clone.transform.parent = grid.transform;
			clone.transform.localPosition = Vector3.zero;
			clone.transform.localScale = Vector3.one;
			MianteamListCell mcell = clone.GetComponent<MianteamListCell>();
			mcell.SimpleInformation = infos[i];
			if(mcell.SimpleInformation.isLeavingTeam_)
			{
				mcell.heiSp.gameObject.SetActive(true);
			}else
			{
				mcell.heiSp.gameObject.SetActive(false);
			}
			cells.Add(mcell);

			//svb.value = 1;
			if (TeamSystem.IsTeamLeader ((int)infos[i].instId_))
			{
				mcell.LeadersP.spriteName = "duizhang(1)";
			}else
			{
				mcell.LeadersP.spriteName = "duiyuan(1)";
			}
			UIManager.SetButtonEventHandler (clone.gameObject, EnumButtonEvent.OnClick, OnClickShowTips, 0, 0);
			GlobalInstanceFunction.Instance.Invoke (()=>{
				grid.repositionNow = true;
			},1);
		}

//		teamPanle.clipOffset = Vector2.zero;
//		teamPanle.transform.localPosition = Vector3.zero;
//		teamPanle.GetComponent<UIScrollView>().ResetPosition();
//		teamPanle.GetComponent<UIPanel>().clipOffset = Vector2.zero;
//		teamPanle.transform.localPosition = Vector3.zero;
		if(infos.Length==0)
		{
			creataTeamBtn.gameObject.SetActive(true);
			UIManager.SetButtonEventHandler (creataTeamBtn.gameObject, EnumButtonEvent.OnClick, OnClickcreataTeamBtn, 0, 0);
		}
	}
	COM_SimplePlayerInst SimplePInst;
	public UIGrid gridTips;
	private void OnClickShowTips(ButtonScript obj, object args, int param1, int param2)
	{
		tipsObj.SetActive (true);
		MianteamListCell mcell = obj.GetComponent<MianteamListCell> ();
		if(TeamSystem.IsTeamLeader())
		{
			if(TeamSystem.IsTeamLeader((int)mcell.SimpleInformation.instId_))
			{
				ZHBtn.gameObject.SetActive(true);
				JHYBtn.gameObject.SetActive(false);
				CKBtn.gameObject.SetActive(false);
				ZSBtn.gameObject.SetActive(false);
				LKbtn.gameObject.SetActive(true);
				GDBtn.gameObject.SetActive(false);
			}else
			{
				if(mcell.SimpleInformation.isLeavingTeam_)
				{
					ZHBtn.gameObject.SetActive(false);
					JHYBtn.gameObject.SetActive(true);
					CKBtn.gameObject.SetActive(true);
					ZSBtn.gameObject.SetActive(false);
					LKbtn.gameObject.SetActive(false);
					GDBtn.gameObject.SetActive(false);
				}else
				{
					ZHBtn.gameObject.SetActive(false);
					JHYBtn.gameObject.SetActive(true);
					CKBtn.gameObject.SetActive(true);
					ZSBtn.gameObject.SetActive(false);
					LKbtn.gameObject.SetActive(false);
					GDBtn.gameObject.SetActive(false);
				}
			}

		}else
		{
			if(mcell.SimpleInformation.instId_ == GamePlayer.Instance.InstId)
			{
				if(mcell.SimpleInformation.isLeavingTeam_)
				{
					GDBtn.gameObject.SetActive(true);
					ZSBtn.gameObject.SetActive(false);
				}else
				{
					GDBtn.gameObject.SetActive(false);
					ZSBtn.gameObject.SetActive(true);
				}

				LKbtn.gameObject.SetActive(true);
				ZHBtn.gameObject.SetActive(false);
				JHYBtn.gameObject.SetActive(false);
				CKBtn.gameObject.SetActive(false);
			}else
			{
				ZSBtn.gameObject.SetActive(false);
				LKbtn.gameObject.SetActive(false);
				ZHBtn.gameObject.SetActive(false);
				JHYBtn.gameObject.SetActive(true);
				CKBtn.gameObject.SetActive(true);
				GDBtn.gameObject.SetActive(false);
			}
		}

		SimplePInst = mcell.SimpleInformation;
		gridTips.Reposition ();
		UISprite sp = tipsObj.GetComponent<UISprite>();
		sp.height = (int)(btnCount () * gridTips.cellHeight)+30;
		UIManager.SetButtonEventHandler (ZSBtn.gameObject, EnumButtonEvent.OnClick, OnClickZS, 0, 0);
		UIManager.SetButtonEventHandler (LKbtn.gameObject, EnumButtonEvent.OnClick, OnClickLK, 0, 0);
		UIManager.SetButtonEventHandler (ZHBtn.gameObject, EnumButtonEvent.OnClick, OnClickzh, 0, 0);
		UIManager.SetButtonEventHandler (JHYBtn.gameObject, EnumButtonEvent.OnClick, OnClickJHY, 0, 0);
		UIManager.SetButtonEventHandler (CKBtn.gameObject, EnumButtonEvent.OnClick, OnClickCK, 0, 0);
		UIManager.SetButtonEventHandler (GDBtn.gameObject, EnumButtonEvent.OnClick, OnClickGDBtn, 0, 0);
		UIManager.SetButtonEventHandler (tipsCloseBtn.gameObject, EnumButtonEvent.OnClick, OnClicktipsClose, 0, 0);
	}
	int btnCount()
	{
		int size = 0;
		foreach( Transform tr in gridTips.transform)
		{
			if(tr.gameObject.activeSelf)
			{
				size++;
			}
		}
		return size;
	}
	private void OnClickGDBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.backTeam ();
		tipsObj.SetActive (false);
	}
	private void OnClickJHY(ButtonScript obj, object args, int param1, int param2)
	{
		if(FriendSystem.Instance().IsmyFriend((int)SimplePInst.instId_))
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("AlreadyUrFirend"));
		}else
		{
			int fMax = 0;
			GlobalValue.Get(Constant.C_FriendMax, out fMax);
			if(FriendSystem.Instance().friends_.Count >= fMax)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
				return;
			}
			NetConnection.Instance.addFriend (SimplePInst.instId_);
		}

	}
	private void OnClickCK(ButtonScript obj, object args, int param1, int param2)
	{
		//chenkPlayerObj.SetActive (true);
//		TeamPlayerInfo tinfo = chenkPlayerObj.GetComponent<TeamPlayerInfo> ();
//		tinfo.SPlayerInfo = SimplePInst;
		TeamPlayerInfo.ShowMe (SimplePInst);
		tipsObj.SetActive (false);
	}
	private void OnClickZS(ButtonScript obj, object args, int param1, int param2)
	{

		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_Instance)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
				NetConnection.Instance.exitCopy();
			});
		}else
		{
			NetConnection.Instance.leaveTeam ();
			tipsObj.SetActive (false);
		}



//		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
//		if(ssdata.sceneType_ == SceneType.SCT_Instance)
//		{
//			return;
//		}
//		NetConnection.Instance.leaveTeam ();
//		tipsObj.SetActive (false);
	}
	private void OnClickLK(ButtonScript obj, object args, int param1, int param2)
	{
        SceneData ssdata = SceneData.GetData(GameManager.SceneID);
        if (ssdata.sceneType_ == SceneType.SCT_Instance)
        {
            if (CopyData.IsCopyScene(GameManager.SceneID))
            {
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"), () =>
                {
                    NetConnection.Instance.exitCopy();

                });
            }
            else
            {
                NetConnection.Instance.exitTeam();
                NetConnection.Instance.exitLobby();
            }

        }
        else
        {
            NetConnection.Instance.exitTeam();
            NetConnection.Instance.exitLobby();
        }
		tipsObj.SetActive (false);
	}
	private void OnClickzh(ButtonScript obj, object args, int param1, int param2)
	{

		if(CopyData.IsCopyScene(GameManager.SceneID))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bunengzhaohuan"));
			return;
		}

		for(int i =0;i<TeamSystem.GetTeamMembers().Length;i++)
		{
			if(!TeamSystem.IsTeamLeader((int)TeamSystem.GetTeamMembers()[i].instId_))
			{
				if(TeamSystem.GetTeamMembers()[i].isLeavingTeam_)
				{
					NetConnection.Instance.teamCallMember ((int)TeamSystem.GetTeamMembers()[i].instId_);
				}

			}
		}



		tipsObj.SetActive (false);
	}
	private void OnClicktipsClose(ButtonScript obj, object args, int param1, int param2)
	{
		tipsObj.SetActive (false);
	}
	void updatecellsInfo(COM_SimplePlayerInst[] infos)
	{
		for(int i =0;i< infos.Length&&i<cells.Count;i++)
		{
			cells[i].SimpleInformation = infos[i];

		}
	}

	int passCount = 1;
	bool isquestbtn;
	private void OnClickPTaskBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
		{
			teampassCount --;
			isteamquestbtn = true;
		}
		 
		SwitchBtnSprite (false);
		teamObj.SetActive (false);
		tasksObj.SetActive (true);
		if(isquestbtn)
		{
			passCount = 0;
			isquestbtn = false;
		}
		passCount++;
		if(passCount>=2)
		{
			TaskUI.SwithShowMe();
		}
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainTaskUI);
	}
	private void OnClickcreataTeamBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
		}else
		{

		    TeamUI.ShowMe ();
			TeamSystem.isYqEnd = false;
		}
        //Prebattle.Instance.OnReturnMainScene();
        //TeamSystem.wantToEnterTeamScene_ = true;
	}
	int teampassCount = 0;
	bool isteamquestbtn;
	private void OnClickPTeamButton(ButtonScript obj, object args, int param1, int param2)
	{
		passCount--;
		isquestbtn = true;
		if(isteamquestbtn)
		{
			teampassCount = 0;
			isteamquestbtn = false;
		}
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
		}else
		{
			SceneData ssd = SceneData.GetData (GameManager.SceneID);
//			if(ssd.sceneType_ == SceneType.SCT_AlonePK||ssd.sceneType_ == SceneType.SCT_TeamPK)
//			{
//				PopText.Instance.Show(LanguageManager.instance.GetValue("bunengzudui"));
//				return;
//			}


//			if (TeamSystem.IsInTeam() && TeamSystem.IsTeamLeader())
//			{   
//				NetConnection.Instance.joinTeamRoom();
//			}else
//			{
				//TeamUI.ShowMe();
//			}
			teampassCount++;
			if(teampassCount>=2)
			{
				TeamUI.ShowMe();
			}
			TeamSystem.isYqEnd = false;
		    TeamButton.pressedSprite = "duiwuliang";
		    TeamButton.normalSprite = "duiwuliang";
			teamObj.SetActive (true);
			tasksObj.SetActive (false);
			SwitchBtnSprite (true);
		    
			
		}
	}
	void SwitchBtnSprite(bool isTeam)
	{
		if(isTeam)
		{
			TeamButton.pressedSprite = "duiwuliang";
			TeamButton.normalSprite = "duiwuliang";
			TaskButton.pressedSprite = "renwuan";
			TaskButton.normalSprite = "renwuan";
		}else
		{
			TaskButton.pressedSprite = "renwuliang";
			TaskButton.normalSprite = "renwuliang";
			TeamButton.pressedSprite = "duiwuan";
			TeamButton.normalSprite = "duiwuan";
		}

	}

    void OnDestroy()
    {
        UIManager.RemoveButtonAllEventHandler(TaskButton.gameObject);
        UIManager.RemoveButtonAllEventHandler(RightButton.gameObject);
		//QuestSystem.OnQuestUpdate -= OnQuestUpdateOk;
		QuestSystem.OnQuestEffectFinish -= TaskLabe;
		TeamSystem.OnTeamDirtyProps -= updatePros;
		TeamSystem.OnUpdateMainteamList -= UpdateMainteamList;

    }
}
