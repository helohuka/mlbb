using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TaskUI : UIBase {
    /// <summary>
    /// 任务界面
    /// </summary>

	public UILabel _TaskTitleLable;
	public UILabel _TaskTargetLable;
	public UILabel _TaskReceiveLable;
	public UILabel _TaskCompleteLable;
	public UILabel _TaskPromptLable;
	public UILabel _TaskDescriptionLable;
	public UILabel _TaskAbandonLable;
	public UILabel _TaskDeliverLable;
	public UILabel _TaskPickedLable;
	public UILabel _TaskKeJieLable;


	public UIPanel jianliPanle;


	public UILabel taskgoal;
	public UILabel taskDescription;
	public UILabel ReceiveNpc;
	public UILabel completeNpc;

	public UISprite[] RewardSps;
	public UIButton chuansongBtn;

	public GameObject infoObj;
	public UILabel backLabel;

	public GameObject item;
	public UIButton MainTaskBtn;
//	public UIButton ExtensionTaskBtn;
//	public UIButton DailyTaskBtn;
	public UIButton kejieTaskBtn;
	public UIButton closeBtn;
	public UIButton abnegateBtn;
//	public UIButton StopTrackingBtn;
//	public UIButton StartTrackingBtn;
	public GameObject KJListObj;
	public GameObject ListObj;


	public UIGrid JGrid;
	public GameObject JItem;


	private List<UIButton> btns = new List<UIButton>();
	public UIGrid grid;
	//private UIEventListener Listener;

	public static int CurrentId;

	public bool isKj = true;

	private List<GameObject> QuestsList_ = new List<GameObject>();
	private static Dictionary<int, NpcData> medata;
	private List<int>npcIds = new List<int>();
	private List<NpcData>npcdatas = new List<NpcData>();

	void InitUIText()
	{
		_TaskTitleLable.text = LanguageManager.instance.GetValue("Task_Title");
		_TaskTargetLable.text = LanguageManager.instance.GetValue("Task_Target");
		_TaskReceiveLable.text = LanguageManager.instance.GetValue("Task_Receive");
		_TaskCompleteLable.text = LanguageManager.instance.GetValue("Task_Complete");
		_TaskPromptLable.text = LanguageManager.instance.GetValue("Task_Prompt");
		_TaskDescriptionLable.text = LanguageManager.instance.GetValue("Task_Description");
		_TaskAbandonLable.text = LanguageManager.instance.GetValue("fangqiQuest");
		_TaskDeliverLable.text = LanguageManager.instance.GetValue("Task_Abandon");
		_TaskPickedLable.text = LanguageManager.instance.GetValue("Task_Picked");
		_TaskKeJieLable.text = LanguageManager.instance.GetValue("Task_KeJie");
	}

    void Start () 
    {
		InitUIText ();
		JItem.SetActive (false);
		item.SetActive (false);
		btns.Add (MainTaskBtn);
//		btns.Add (ExtensionTaskBtn);
//		btns.Add (DailyTaskBtn);
		btns.Add (kejieTaskBtn);	
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickTColse, 0, 0);
		UIManager.SetButtonEventHandler (MainTaskBtn.gameObject, EnumButtonEvent.OnClick, OnClickTabsBtn, 0, 0);
		//UIManager.SetButtonEventHandler (ExtensionTaskBtn.gameObject, EnumButtonEvent.OnClick, OnClickTabsBtn, 1, 0);
		//UIManager.SetButtonEventHandler (DailyTaskBtn.gameObject, EnumButtonEvent.OnClick, OnClickTabsBtn, 2, 0);
		UIManager.SetButtonEventHandler (kejieTaskBtn.gameObject, EnumButtonEvent.OnClick, OnClickTabsBtn, 1, 0);
		UIManager.SetButtonEventHandler (chuansongBtn.gameObject, EnumButtonEvent.OnClick, OnClickChuan, 1, 0);
		UIManager.SetButtonEventHandler (abnegateBtn.gameObject, EnumButtonEvent.OnClick, OnClickabnegate, 0, 0);
//        UIManager.SetButtonEventHandler (StopTrackingBtn.gameObject, EnumButtonEvent.OnClick, OnClickStopTracking, 0, 0);
//        UIManager.SetButtonEventHandler (StartTrackingBtn.gameObject, EnumButtonEvent.OnClick, OnClickStartTracking, 0, 0);
		QuestSystem.GiveupQuests += Reset;
        Reset();

        AcceptableRenwuUI accTab = KJListObj.GetComponent<AcceptableRenwuUI>();
        accTab.JGrid = JGrid;
        accTab.JItem = JItem;
		questBackLabel ();

	}
	void questBackLabel()
	{
		if(!isKj)
		{
			if(QuestSystem.CurrentList.Count==0)
			{
				infoObj.SetActive(false);
				backLabel.gameObject.SetActive(true);
			}else
			{
				infoObj.SetActive(true);
				backLabel.gameObject.SetActive(false);
			}
		}else
		{
			
			if(QuestSystem.AcceptableList.Count==0)
			{
				infoObj.SetActive(false);
				backLabel.gameObject.SetActive(true);
			}else
			{
				infoObj.SetActive(true);
				backLabel.gameObject.SetActive(false);
			}
			
			
		}

	}
	NpcData StartNpc(int qid)
	{
		medata = NpcData.GetData ();
		foreach (int nid in medata.Keys)
		{
			npcIds.Add(nid);
		}
		for(int i=0;i<npcIds.Count;i++)
		{
			NpcData ndata = NpcData.GetData(npcIds[i]);
			npcdatas.Add(ndata);
		}
		for (int i = 0; i<npcdatas.Count; i++)
		{
            for (int j = 0; j < npcdatas[i].Quests.Length; j++)
			{
				if(qid == npcdatas[i].Quests[j])
				{
					return npcdatas[i];
				}

			}
		}
		return null;
	}
	void OnClickChuan(ButtonScript obj, object args, int param1, int param2)
	{
		QuestData qdata = QuestData.GetData (CurrentId);
		if(qdata.questKind_ == QuestKind.QK_Guild)
		{
			if(!GuildSystem.IsInGuild())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("gonghui"));
				return;
			}
		}
        if(GameManager.Instance.ParseNavMeshInfo(qdata.xunlu))
        {
            Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_AFP);
        }
	}
	void OnClickabnegate(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("fangqiQuest"), () => {

			NetConnection.Instance.giveupQuest(CurrentId);

		});

	}
//	void OnClickStopTracking(ButtonScript obj, object args, int param1, int param2)
//	{
//        //CahngeTrackingState (false);
//	
////		QuestSystem.DelTrackQues (CurrentId);
////		QuestSystem.UpDateTrackQuesLabel ();
//		SetStartTrackingBtnDisplay (true);
//		SetStopTrackingBtnDisplay (false);
//	}
//	void OnClickStartTracking(ButtonScript obj, object args, int param1, int param2)
//	{
//        //CahngeTrackingState (true);
//
////		QuestSystem.AddTrackQues(questInset());
////		QuestSystem.UpDateTrackQuesLabel ();
//		SetStartTrackingBtnDisplay (false);
//		SetStopTrackingBtnDisplay (true);
//	}
	COM_QuestInst questInset()
	{
		for(int i=0;i<QuestSystem.CurrentList.Count;i++)
		{
			if(CurrentId==QuestSystem.CurrentList[i].questId_)
			{
				return QuestSystem.CurrentList[i];
			}
		}
		return null;
	}
	void OnClickTColse(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickTabsBtn(ButtonScript obj, object args, int param1, int param2)
	{
        qidd = 0;
		Refresh ();

		if (param1 == 0)
		{
			KJListObj.SetActive(false);
			ListObj.SetActive(true);
			isKj = false;
			sortQuest ();
			AddItems(qudes);
		}
		else if(param1 == 1)
		{
			isKj = true;
			KJListObj.SetActive(true);
			ListObj.SetActive(false);
		}
		TabsSelect (param1);
		questBackLabel ();
	}

    void Reset()
    {
        foreach (Transform tr in grid.transform)
        {
            Destroy(tr.gameObject);
        }
        TabsSelect(0);
        SetStartTrackingBtnDisplay(false);
        SetStopTrackingBtnDisplay(false);
        SetAbnegateBtnDisplay(false);

        UpdateData();

    }

	void UpdateData()
	{
		sortQuest ();
		AddItems(qudes);
	}
	List<QuestData> qudes = new List<QuestData> ();
	void sortQuest()
	{
		qudes.Clear ();
		for (int i =0; i<QuestSystem.CurrentList.Count; i++)
		{
			QuestData qdata = QuestData.GetData(QuestSystem.CurrentList[i].questId_);
			if(qdata.questKind_ == QuestKind.QK_Main)
			{
				qudes.Insert(0,qdata);
			}else
			{
				qudes.Add(qdata);
			}
		}
	}
	void Refresh()
	{
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
		taskDescription.text = "";
		ReceiveNpc.text = "";
		completeNpc.text = "";
		taskgoal.text = "";
		

	}

	public void AddItems(List<QuestData> qudes)
	{
		for (int i = 0; i < qudes.Count; i++)
        {
            //QuestData qdata = QuestData.GetData(qlist[i].questId_);
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = grid.transform;
			TaskuiCell tcell = o.GetComponent<TaskuiCell>();
			tcell.Qdata = qudes[i];
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickbtn, qudes[i].id_, 0);

			grid.repositionNow = true;
			QuestsList_.Add(o);
		}

		if (QuestSystem.CurrentList.Count > 0&&qudes.Count>0)
        {
			ShowTaskIonf(CurrentId = qudes[0].id_);
			etTrackingState (qudes[0].id_);
        }
	} 

	private void IsMainQuest(int qid)
	{
		for(int i =0;i<QuestsList_.Count;i++)
		{
            if (QuestsList_[i] == null)
                continue;

			TaskuiCell tcell = QuestsList_[i].GetComponent<TaskuiCell>();
			if(tcell.Qdata.id_ == qid && tcell.Qdata.questKind_ == QuestKind.QK_Main)
			{
				abnegateBtn.gameObject.SetActive(false);
			}else if(tcell.Qdata.id_ == qid && tcell.Qdata.questKind_ != QuestKind.QK_Main)
			{
				abnegateBtn.gameObject.SetActive(true);
			}
		}
	}

	private void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
	{
		ShowTaskIonf(CurrentId = param1);
		IsMainQuest (param1);
	}
    //void TrackingInfo(List<QuestInfo> Quests)
    //{
    //    for (int i=0; i<Quests.Count; i++)
    //    {
    //        for(int j = 0;j<NpcRenwuUI.TrackingQuests_.Count;j++)
    //        {
    //            if(Quests[i].questData_.id_ == NpcRenwuUI.TrackingQuests_[j].questData_.id_)
    //            {
    //                Quests[i].questState_ = QuestInfo.QuestState.QS_Tracking;
    //            }
    //        }
    //    }
    //}

//	void buttonClick(GameObject sender)
//	{
//		int qid = (int )UIEventListener.Get (sender).parameter;
//        ShowTaskIonf(CurrentId = qid);
//        //SetTrackingState (questId);
//	}
	
	void etTrackingState(int qid)
	{
		for(int i =0;i<QuestSystem.CurrentList.Count;i++)
		{
//			if(i<QuestSystem.CurrentList.Count)
//			{
//				if(QuestSystem.CurrentList[i].questId_ == qid && QuestSystem.CurrentList[i].questId_ == qid)
//				{
//					SetStopTrackingBtnDisplay(true);
//					SetStartTrackingBtnDisplay(false);
//				}else
//				{
//					SetStopTrackingBtnDisplay(false);
//					SetStartTrackingBtnDisplay(true);
//				}
//			}else
//			{
//				SetStopTrackingBtnDisplay(false);
//				SetStartTrackingBtnDisplay(true);
//			}
//			if(QuestData.GetData(QuestSystem.CurrentList[i].questId_).questKind_ != QuestKind.QK_Main)
//			{
//				SetAbnegateBtnDisplay (true);
//			}else
//			{
//				SetAbnegateBtnDisplay (false);
//			}
		}
	}
    //void SetTrackingState(int qid)
    //{
    //    for (int i =0; i<YQuests_.Count; i++)
    //    {
    //        if(YQuests_[i].questData_.id_ == qid)
    //        {
    //            if(NpcRenwuUI.TrackingQuests_.Count<TrackingCount)
    //            {
    //                if(YQuests_[i].questState_ == QuestInfo.QuestState.QS_Tracking)
    //                {
    //                    SetStopTrackingBtnDisplay(true);
    //                    SetStartTrackingBtnDisplay(false);
    //                }else
    //                {
    //                    SetStopTrackingBtnDisplay(false);
    //                    SetStartTrackingBtnDisplay(true);
    //                }
    //            }
    //            else
    //            {
    //                SetStopTrackingBtnDisplay(false);
    //                SetStartTrackingBtnDisplay(false);
    //            }

    //            if(YQuests_[i].questData_.questKind_ == QuestKind.QK_Sub)
    //            {
    //                SetAbnegateBtnDisplay (true);
    //            }else
    //            {
    //                SetAbnegateBtnDisplay (false);
    //            }
    //        }
    //    }

    //}
    //void CahngeTrackingState(bool isTracking)
    //{
    //    string str = "";
    //    for(int i=0;i<YQuests_.Count&&i<QuestsList_.Count;i++)
    //    {
    //        if(YQuests_[i].questData_.id_ == questId)
    //        {

    //            switch(YQuests_[i].questState_)
    //            {
    //            case QuestInfo.QuestState.QS_Doing:
    //                str = "追踪中";
    //                break;
    //            case QuestInfo.QuestState.QS_Finishable:
    //                str = "完成";
    //                break;
    //            }
    //            UILabel []las = QuestsList_[i].GetComponentsInChildren<UILabel>(true);
    //            foreach(UILabel la in las)
    //            {
    //                if(la.gameObject.name.Equals("TaskLabel"))
    //                {
    //                    if(isTracking)
    //                    {
    //                        if(NpcRenwuUI.TrackingQuests_.Count<TrackingCount)
    //                        {
    //                            NpcRenwuUI.TrackingQuests_.Add(YQuests_[i]);
    //                        }
    //                        la.text =string.Format("{0}\n{1}",YQuests_[i].questData_.questName_,str);
    //                        //if (MainTaskUI.TrackingQuestsInfo != null)
    //                        //{
    //                        //    MainTaskUI.TrackingQuestsInfo(NpcRenwuUI.TrackingQuests_);	
    //                        //}
    //                    }
    //                    else
    //                    {
    //                        for(int j = 0;j<NpcRenwuUI.TrackingQuests_.Count;j++)
    //                        {
    //                            if(NpcRenwuUI.TrackingQuests_[j].questData_.id_ == YQuests_[i].questData_.id_)
    //                            {
    //                                    la.text =YQuests_[i].questData_.questName_;
    //                                    NpcRenwuUI.TrackingQuests_.RemoveAt(j);
    //                                    //if (MainTaskUI.TrackingQuestsInfo != null)
    //                                    //{
    //                                    //    MainTaskUI.TrackingQuestsInfo(NpcRenwuUI.TrackingQuests_);	
    //                                    //}
    //                                }
    //                            }
    //                        }
    //                }
    //            }
    //        }
    //    }
    //}
    //void giveupTaskQe(int qid)
    //{
    //    for (int i=0; i<YQuests_.Count&&i<QuestsList_.Count; i++)
    //    {
    //        if(YQuests_[i].questData_.id_ == qid)
    //        {
    //            QuestsList_.RemoveAt(i);
    //        }
    //    }
    //}
	List<GameObject> listObj = new List<GameObject>();
	List<QuestData> qustDatas = new List<QuestData>();
	int qidd = 0;
    List<GameObject> rewardPool = new List<GameObject>();
	public void ShowTaskIonf(int qid)
	{
		if (qidd == qid)
			return;
		qidd = qid;
		string goal_str = "";
		QuestData quest = QuestData.GetData(qid);
		DropData droData = DropData.GetData (quest.DropID_);

 		taskDescription.text = quest.QuestDes_;
		if(StartNpc(qid) != null)
		ReceiveNpc.text = StartNpc(qid).Name;
		completeNpc.text = NpcData.GetData(quest.finishNpcId_).Name;
        goal_str = string.Format("{0}", quest.miniDesc_);
        taskgoal.text = goal_str;

        if (droData == null)
        {
            for (int i = 0; i < rewardPool.Count; ++i)
            {
                rewardPool[i].SetActive(false);
            }
            return;
        }

        int totalReward = 0;
        Dictionary<int, int> itemIconDic = new Dictionary<int, int>();
        if (droData.exp_ != 0)
        {
            totalReward += 1;
            itemIconDic.Add(5036, droData.exp_);
        }
        if (droData.money_ != 0)
        {
            totalReward += 1;
            itemIconDic.Add(5035, droData.money_);
        }
        totalReward += droData.itemList.Count;
        for (int i = 0; i < droData.itemList.Count; ++i)
        {
            if (droData.itemList[i] == 0 || itemIconDic.ContainsKey(droData.itemList[i]))
                continue;

            itemIconDic.Add(droData.itemList[i], droData.itemNumList[i]);
        }

        int count = 0;
        GameObject item = null;
        foreach (int itemId in itemIconDic.Keys)
        {
            if (count >= rewardPool.Count)
            {
                item = GameObject.Instantiate(JItem) as GameObject;
                item.transform.parent = JGrid.transform;
                item.transform.localScale = Vector3.one;
                rewardPool.Add(item);
            }
            else
            {
                item = rewardPool[count];
            }
            item.SetActive(true);
            RewardCell rc = item.GetComponent<RewardCell>();
            rc.RewardIcon.gameObject.SetActive(true);
            rc.RewardLabel.text = itemIconDic[itemId].ToString();
            rc.RewardIcon.gameObject.SetActive(false);
            rc.icont.gameObject.SetActive(true);

            UISprite sp = item.GetComponent<UISprite>();
            ItemCellUI ic = UIManager.Instance.AddItemCellUI(sp, (uint)itemId);
            ic.showTips = true;
            JGrid.repositionNow = true;
            rc.RewardIcon.MakePixelPerfect();
            count++;
        }
        for (int i = count - 1; i < rewardPool.Count; ++i)
        {
            rewardPool[i].SetActive(false);
        }
		chuansongBtn.gameObject.SetActive (true);
        JGrid.Reposition();
	}
	public void closeItem()
	{
		for (int i = 0; i < rewardPool.Count; ++i)
		{
			rewardPool[i].SetActive(false);
		}
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_renwuPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_renwuPanel);
	}
	void TabsSelect(int index)
	{
		for (int i = 0; i<btns.Count; i++) 
		{
			if(i==index)
			{
				btns[i].isEnabled = false;
			}
			else
			{
				btns[i].isEnabled = true;
			}
		}
	}

	void SetAbnegateBtnDisplay(bool isDisplay)
	{
		abnegateBtn.gameObject.SetActive (isDisplay);
	}
	void SetStopTrackingBtnDisplay(bool isDisplay)
	{
		//StopTrackingBtn.gameObject.SetActive (isDisplay);
	}
	void SetStartTrackingBtnDisplay(bool isDisplay)
	{
		//StartTrackingBtn.gameObject.SetActive (isDisplay);
	}

	void OnDestroy()
	{
		QuestSystem.GiveupQuests -= Reset;
	}
	public override void Destroyobj ()
	{

	}
}
