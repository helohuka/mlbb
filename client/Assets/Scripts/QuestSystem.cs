using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public static class QuestSystem
{
	static List<COM_QuestInst>[] QuestListKind = new List<COM_QuestInst>[(int)QuestKind.QK_Max] { new List<COM_QuestInst>(), new List<COM_QuestInst>(), new List<COM_QuestInst>(), new List<COM_QuestInst>(), new List<COM_QuestInst>(),new List<COM_QuestInst>() ,new List<COM_QuestInst>() ,new List<COM_QuestInst>(),new List<COM_QuestInst>(),new List<COM_QuestInst>(),new List<COM_QuestInst>()};

	static List<COM_QuestInst> QuestList = new List<COM_QuestInst>();
    
    static List<int> CompletedQuestList = new List<int>();
    static List<int> AcceptableQuestList = new List<int>();
    static List<int> TracingQuestList = new List<int>();
	//private static List<COM_QuestInst> TrackQues = new List<COM_QuestInst>();
    public delegate void QuestUdpateEvent();
    public static event QuestUdpateEvent OnQuestUpdate;
    
	public delegate void QuestEffectFinishEvent(int qid);
	public static event QuestEffectFinishEvent OnQuestEffectFinish;
	public static int randCount;
	public delegate void GiveupQuest();
	public static event GiveupQuest GiveupQuests;

	public static int  aqid = 0;
    static bool NewQuest = false;
	static public bool isEffectF = false;
	static public bool isTaskF = false;
	static public int sqid = 0;
    static public bool isDirty;
//	public static List<COM_QuestInst> TrackQuests
//	{
//		set{
//			TrackQues = value;
//		}
//		get{
//			return TrackQues;
//		}
//	}

//	public static void UpDateTrackQuesLabel()
//	{
//		if (OnQuestUpdate != null)
    //			OnQuestUpdate();
    //	}
    static int _QuestId = 0;
    public static void TryAcceptQuest(int questId,bool isTeam=false)
    {
        QuestData qdata = QuestData.GetData(questId);
        if (null == qdata)
        {
            return;
        }
        if (qdata.questKind_ == QuestKind.QK_Profession)
        { //Ö°Òµ×ªÖ°
            int tmp = GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
            if (tmp != (int)JobType.JT_Newbie && qdata.jobtype_ != tmp)
            {
                _QuestId = questId;
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("TransProfTips"), __AcceptQuest);
                return;
            }
        }

        if (qdata.questKind_ == QuestKind.QK_Guild)
        {
            if (TeamSystem.IsInTeam())
            {
                PopText.Instance.Show(LanguageManager.instance.GetValue("GuildQuestNoTeam"));
                return;
            }
        }
        
        NetConnection.Instance.acceptQuest(questId);
    }

    static void __AcceptQuest()
    {
        NetConnection.Instance.acceptQuest(_QuestId);
    }
    
    public static void InitQuest(ref COM_QuestInst[] qinst, ref int[] list)
	{
        Clear();
        for (int i = 0; i < qinst.Length; ++i)
        {
            InsertQuestInst(qinst[i]);
//			if(qinst[i].questId_ == PlayerPrefs.GetInt(qinst[i].questId_.ToString()))
//			{
//				TrackQues.Add(qinst[i]);
//			}
        }

        CompletedQuestList = new List<int>(list);
        UpdateAcceptableQuests();
        if(OnQuestUpdate != null)
            OnQuestUpdate();
        isDirty = true;
    }

    public static void Clear()
    {
        foreach (var a in QuestListKind)
        {
            a.Clear();
        }
        QuestList.Clear();
        CompletedQuestList.Clear();
        AcceptableList.Clear();
        TracingQuestList.Clear();
    }

    public static void UpdateAcceptableQuests()
    {
        Dictionary<int, QuestData> metaData = QuestData.GetMetaData();
        if (metaData == null)
            return;
        AcceptableQuestList.Clear();
        foreach (QuestData quest in metaData.Values)
        {
            if (quest.needLevel_ > GamePlayer.Instance.GetProperty(PropertyType.PT_Level))
                continue;

            if (IsQuestDoing(quest.id_))
                continue;

            if (IsComplate(quest.id_))
                continue;

            ///Èç¹ûÊÇÈÕ³£ÈÎÎñ ²¢ÇÒÊÇÈÎÎñÁ´ÖÐµÄµÚÒ»¸öÈÎÎñ ²¢ÇÒ ½ñÌìÒÑ¾­×öÁËÒ»¸öÈÕ³£
            //if (quest.questKind_ == QuestKind.QK_Daily){
            //  if(quest.preQuest_ == 0 && (IsDailyComplate() || IsDailyDoing()))
            //        continue;
            //}
            if (quest.preQuest_!=0 && !IsComplate(quest.preQuest_))
                continue;

            //if (quest.questKind_ == QuestKind.QK_Profession)
            //{
            //    if (quest.preQuest_ == 0)
            //        continue;
            //}

            AcceptableQuestList.Add((int)quest.id_);
        }
    }

    public static List<int> CompletedList
    {
        get
        {
            return CompletedQuestList;
        }
    }

    public static List<int> AcceptableList
    {
        get
        {
            return AcceptableQuestList;
        }
    }

    public static List<COM_QuestInst> CurrentList
    {
        get
        {
            return QuestList;
        }
    }

    public static bool IsMainKindEmpty()
    {
        if (QuestListKind == null)
            return false;

        return (QuestListKind[(int)QuestKind.QK_Main].Count == 0);
    }

    public static int GetFirstComplateMainKindId()
    {
        if (CompletedList == null)
            return 0;

        int ret = 0;
        for (int i = 0; i < CompletedList.Count; ++i)
        {
            QuestData qdata = QuestData.GetData(CompletedList[i]);
            if (null == qdata)
            {
                continue;
            }

            if (qdata.questKind_ == QuestKind.QK_Main)
            {
                ret = CompletedList[i];
            }
        }

        return ret;
    }

    public static COM_QuestInst GetDoingMainKind()
    {
        if (QuestListKind == null)
            return null;

        foreach (COM_QuestInst qi in QuestListKind[(int)QuestKind.QK_Main])
        {
            return qi;
        }
        return null;
    }

    public static int GetLastComplateMainKindId()
    {
        int current = GetFirstComplateMainKindId();

        while (true)
        {
            QuestData qdata = QuestData.GetData(current);
            if (qdata == null)
                return current;

            if (!IsComplate(qdata.postQuest_))
            {
                break;
            }
            current = qdata.postQuest_;
        }
        return current;
    }
	public static bool IsFDailyQuest()
	{
        if (CompletedList == null)
            return false;

		int count = 0;
		for(int i =0;i<CompletedList.Count;i++)
		{
			QuestData qd = QuestData.GetData (CompletedList[i]);
			if(qd.questKind_ == QuestKind.QK_Daily)
			{
				count++;
			}
		}
		if(count == 10)
		{
			return true;
		}
		return false;
	}
    public static int GetFirstAcceptableMainKindId()
    {
        if (AcceptableList == null)
            return 0;

        for (int i = 0; i < AcceptableList.Count; ++i)
        {
            QuestData qdata = QuestData.GetData(AcceptableList[i]);
            if (null == qdata)
            {
                continue;
            }

            if (qdata.questKind_ == QuestKind.QK_Main)
            {
                return AcceptableList[i];
            }
        }

        return 0;
    }

    public static List<COM_QuestInst> GetKindList(QuestKind kind)
    {
        if (QuestListKind == null)
            return null;

        return QuestListKind[(int)kind];
    }

    public static bool IsDailyComplate()
    {
        if (CompletedQuestList == null)
            return false;

        for (int i = 0; i < CompletedQuestList.Count; ++i) 
        {
            if (QuestData.GetData( CompletedQuestList[i]).questKind_ == QuestKind.QK_Daily)
                return true;
        }
        return false;
    }

    public static bool IsDailyDoing()
    {
        if (QuestList == null)
            return false;

        for (int i = 0; i < QuestList.Count; ++i)
        {
            if (QuestData.GetData(QuestList[i].questId_).questKind_ == QuestKind.QK_Daily)
                return true;
        }
        return false;
    
    }

    public static bool IsComplate(int id)
    {
        if (QuestList == null)
            return false;

        for (int i = 0; i < CompletedQuestList.Count; ++i)
        {
            if (CompletedQuestList[i] == id)
                return true;
        }
        return false;
    }

    public static void InsertQuestInst(COM_QuestInst quest)
    {
        if (QuestList == null)
            return;

        QuestData qdata0 = QuestData.GetData(quest.questId_);
        int i;
        for (i = 0; i < QuestList.Count; ++i)
        {
			QuestData qdata1 = QuestData.GetData(QuestList[i].questId_);
            if (qdata0.questKind_ > qdata1.questKind_)
            {
                break;
            }
        }
        QuestListKind[(int)qdata0.questKind_].Add(quest);
        QuestList.Insert(i, quest);

        UpdateAcceptableQuests();
    }

    public static void EraseQuestInst(int questId)
    {
        if (QuestList == null)
            return;

        for (int i = 0; i < QuestList.Count; ++i)
        {
            if (QuestList[i].questId_ == questId)
            {
                QuestList.RemoveAt(i);
                break;
            }
        }

        QuestData qdata = QuestData.GetData(questId);
        for (int i = 0; i < QuestListKind[(int)qdata.questKind_].Count; ++i)
        {
            if (QuestListKind[(int)qdata.questKind_][i].questId_ == questId)
            {
                QuestListKind[(int)qdata.questKind_].RemoveAt(i);
                break;
            }
        }

        UpdateAcceptableQuests();
    }

    public static bool HasNewQuest()
    {
        if (false == NewQuest) return false;
        
        NewQuest = false;
        return true;
    }

    public static void AcceptQuestOk(COM_QuestInst quest)
    {
        NewQuest = true;
        InsertQuestInst(quest);
        UpdateAcceptableQuests();
		//AddTrackQues(quest);
        if (OnQuestUpdate != null)
            OnQuestUpdate();
        isDirty = true;

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_NpcRenwuAccept, quest.questId_);
        EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_AcceptQuest, ApplicationEntry.Instance.uiRoot.transform);
    }

    public static void LocalSubmitQuest(int id)
    {
        QuestData q = QuestData.GetData(id);
        if (q.questKind_ == QuestKind.QK_Profession)
            return;

        DropData dd = DropData.GetData(q.DropID_);
        if (dd.itemList.Count > BagSystem.instance.GetEmptySlotNum())
        {
            return;
        }

        if (OnQuestEffectFinish != null)
        {
            OnQuestEffectFinish(id);
        }

        EraseQuestInst(id);

        if (q.questKind_ != QuestKind.QK_Tongji)
        {
            if (!CompletedQuestList.Contains(id))
            {
                CompletedQuestList.Add(id);
                UpdateAcceptableQuests();
                if (OnQuestUpdate != null)
                    OnQuestUpdate();
                isDirty = true;
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_NpcRenwuSubmit, id);
                EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_ComplishQuest, ApplicationEntry.Instance.uiRoot.transform);
            }
        }
    }

    public static void SubmitQuestOk(int id)
    {
        QuestData q = QuestData.GetData(id);

		if(OnQuestEffectFinish != null)
		{
			OnQuestEffectFinish(id);
		}
		//DelTrackQues (id);
        EraseQuestInst(id);

        if (q.questKind_ == QuestKind.QK_Profession)
        {
            for (int i = 0; i < CompletedList.Count; ++i )
            {
                QuestData tmpQ = QuestData.GetData(CompletedList[i]);
                if (tmpQ.questKind_ == QuestKind.QK_Profession)
                {
                    CompletedList.RemoveAt(i);
                    break;
                }
            }
        }

        if (q.questKind_ != QuestKind.QK_Tongji)
        {
            if (!CompletedQuestList.Contains(id))
            {
                CompletedQuestList.Add(id);
                UpdateAcceptableQuests();
                if (OnQuestUpdate != null)
                    OnQuestUpdate();
                isDirty = true;
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_NpcRenwuSubmit, id);
                EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_ComplishQuest, ApplicationEntry.Instance.uiRoot.transform);
            }
        }
		if(q.questType_ == QuestType.QT_GiveBaby || q.questType_ == QuestType.QT_GiveItem)
		{
			TurnOnUIPlanel.HideMe();
			NpcRenwuUI.SwithShowMe();

		}
		if(IsCopyEndId(id))
		{
			CopyTipsUI.SwithShowMe();
//			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("gongxitongguan"),()=>{
//				NetConnection.Instance.exitCopy();
//			},true,null,null,LanguageManager.instance.GetValue("likai"));
		}
    }

	static bool IsCopyEndId(int qid)
	{
	
		foreach(KeyValuePair<int, CopyData> pair in CopyData.GetData())
		{
			if(qid == pair.Value._EndID)
			{
				return true;
			}
		}
		return false;
	}


	public static void UpdateQuest()
	{
		if (OnQuestUpdate != null)
			OnQuestUpdate();
        isDirty = true;
	}
    public static void GiveupQuestOk(int id)
    {
        if (AcceptableQuestList == null)
            return;

		QuestData data = QuestData.GetData (id);
		if (data != null && data.questKind_ == QuestKind.QK_Rand) {
			AcceptableQuestList.Add(QuestData.defaultXuanshangQuestId_); //??????? ????2b?????????????
		}else {
        AcceptableQuestList.Add(id);
		}

      
        for (int i = 0; i < QuestList.Count; ++i)
        {
            if (QuestList[i].questId_ == id)
            {
                QuestList.RemoveAt(i);
                break;
            }
        }
		if(GiveupQuests != null)
		{
			GiveupQuests();
		}
//		for(int j =0;j<TrackQues.Count;j++)
//		{
//			if (TrackQues[j].questId_ == id)
//			{
//				TrackQues.RemoveAt(j);
//				PlayerPrefs.DeleteKey(TrackQues[j].questId_.ToString());
//			}
//				
//
//		}
        if (OnQuestUpdate != null)
            OnQuestUpdate();
        isDirty = true;
    }

    public static void UpdateQuest(COM_QuestInst questInst)
    {
        if (QuestListKind == null)
            return;

		QuestData qdata = QuestData.GetData(questInst.questId_);
		int i;
		for (i = 0; i < QuestListKind[(int)qdata.questKind_].Count; ++i)
		{

			if (QuestListKind[(int)qdata.questKind_][i].questId_ == qdata.id_)
			{
				QuestListKind[(int)qdata.questKind_][i] = questInst;
				break;
			}
		}

        for (i = 0; i < QuestList.Count; ++i)
        {
            if (QuestList[i].questId_ == questInst.questId_)
			{
				QuestList[i] = questInst;
				break;
			}
        }
		if(IsQuestFinish(questInst.questId_))
		{
			isEffectF = true;
			isTaskF = true;
			sqid = questInst.questId_;
            //Èç¹ûÍê³ÉµÄÈÎÎñÊÇµ±Ç°Ñ²ÂßµÄÈÎÎñ Í£Ö¹
            if (Prebattle.Instance.clickedQuestId_ == sqid)
            {
                NetConnection.Instance.stopMove();
                Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
                Prebattle.Instance.ResetCurrentQuest();
            }
		}


//		for(int j =0;j<TrackQues.Count;j++)
//		{
//			if (TrackQues[j].questId_ == questInst.questId_)
//				TrackQues[j] = questInst;
//		}
        isDirty = true;
        if (OnQuestUpdate != null)
            OnQuestUpdate();
    }

    public static bool IsQuestDoing(int questId)
    {
        if (QuestList == null)
            return false;

        for (int j = 0; j < QuestList.Count; j++)
        {
            if (questId == QuestList[j].questId_)
                return true;
        }
        return false;
    }

    public static bool IsQuestFinish(int questId)
    {
        QuestData quest = QuestData.GetData(questId);
        COM_QuestInst inst = GetQuestInst(questId);
        if (null == inst)
            return false;
        switch (quest.questType_)
        {
            case QuestType.QT_Battle:
			for(int i =0;i<inst.targets_.Length;i++)
			{
				if(inst.targets_[i].targetNum_>=1)
				{
					return true;
				}
			}
//                if (inst.questNum_ >= 1)
//                    return true;
                break;
            case QuestType.QT_Dialog:        
                    return true;
            case QuestType.QT_Item:
            case QuestType.QT_Other:
		    case QuestType.QT_GiveBaby:
            case QuestType.QT_Kill:
            case QuestType.QT_KillAI:
		   {
			if(isAll((int)questId))
			{
				return true;
			}
		   }
                break;
		case QuestType.QT_GiveItem:
			if(isGiveItemAll((int)questId))
			{
				return true;
			}
			break;
            default:
                return false;
        }
        return false;
    }

	static	bool isGiveItemAll(int questId)
	{
		QuestData quest = QuestData.GetData(questId);
		if (quest == null)
						return false;
		int itemcount =  BagSystem.instance.GetItemMaxNum ((uint)quest.targetId_);
		if( itemcount>= int.Parse(quest.targetNum_[0]))
		{
			return true;
		}
		return false;
	}

    static	bool isAll(int questId)
	{
		QuestData quest = QuestData.GetData(questId);
		COM_QuestInst inst = GetQuestInst(questId);
        if (inst == null)
            return false;

        if (quest == null)
            return false;

		for(int i =0;i<inst.targets_.Length;i++)
		{
			//QuestData questd = QuestData.GetData((int)inst.targets_[i].targetId_);
			if(inst.targets_[i].targetNum_<int.Parse(quest.targetNum_[i]))
			{
				return false;
			}
		}
		return true;
	}

    //static bool isAllItem(int questId)
    //{
    //    QuestData quest = QuestData.GetData(questId);
    //    COM_QuestInst inst = GetQuestInst(questId);
    //    if (inst == null)
    //        return false;

    //    if (quest == null)
    //        return false;

    //    for (int i = 0; i < inst.targets_.Length; i++)
    //    {
    //        int itemCountInBag = BagSystem.instance.GetItemMaxNum(inst.targets_[i].targetId_);
    //        //QuestData questd = QuestData.GetData((int)inst.targets_[i].targetId_);
    //        if (itemCountInBag < int.Parse(quest.targetNum_[i]))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    public static void CheckItemQuest(uint itemId)
    {
		int itemStack = BagSystem.instance.GetItemMaxNum (itemId);
        for (int i = 0; i < CurrentList.Count; ++i)
        {
            QuestData quest = QuestData.GetData(CurrentList[i].questId_);
            if (null == quest)
            {
                continue;
            }

			if (quest.questType_ == QuestType.QT_Item || quest.questType_ == QuestType.QT_GiveItem)
            {
                for (int j = 0; j < CurrentList[i].targets_.Length; ++j)
                {
                    if (CurrentList[i].targets_[j].targetId_ == itemId)
                    {
                        CurrentList[i].targets_[j].targetNum_ = itemStack;
						QuestSystem.isDirty = true;
                    }
                }
            }
        }
    }

    public static bool IsQuestAcceptable(int id)
    {
        if (AcceptableQuestList == null)
            return false;

        for (int j = 0; j < AcceptableQuestList.Count; j++)
        {
            if (id == AcceptableQuestList[j])
                return true;
        }
        return false;
    }

    public static bool IsQuestAcceptableNpc(int npcid)
    {
        NpcData ndata = NpcData.GetData(npcid);
        if (ndata == null)
            return false;

        for (int i = 0; i < ndata.Quests.Length; ++i)
        {
            if (IsQuestAcceptable(ndata.Quests[i]))
                return true;
        }

        return false;
    }

    

    public static List<int> GetQuestAcceptableNpc(int npcid)
    {
        List<int> qlist = new List<int>();
        NpcData ndata = NpcData.GetData(npcid);
        for (int i = 0; i < ndata.Quests.Length; ++i)
        {
            if (IsQuestAcceptable(ndata.Quests[i]))
                qlist.Add(ndata.Quests[i]);
        }
        return qlist;
    }

    public static bool IsQuestFinishNpc(int npcid)
    {
        if (QuestList == null)
            return false;

        for (int i = 0; i < QuestList.Count; i++)
        {
            if (IsQuestFinish(QuestList[i].questId_))
            {
                QuestData quest = QuestData.GetData(QuestList[i].questId_);
                if (npcid == quest.finishNpcId_)
                    return true;
            }
        }
        return false;
    }

    public static bool IsProfessionQuestDoing()
    {
        if (QuestList == null)
            return false;

        for (int j = 0; j < QuestList.Count; j++)
        {
            if (QuestKind.QK_Profession == QuestData.GetData(QuestList[j].questId_).questKind_)
                return true;
        }
        return false;
    }

    public static List<int> GetQuestByFinishNpc(int npcid)
    {
        if (QuestList == null)
            return null;

        List<int> qlist = new List<int>();
        for (int i = 0; i < QuestList.Count; i++)
        {
            QuestData quest = QuestData.GetData(QuestList[i].questId_);
            if (npcid == quest.finishNpcId_)
            {
                qlist.Add(quest.id_);
            }
        }

        return qlist;
    }

    public static COM_QuestInst GetQuestInst(int id)
    {
        if (QuestList == null)
            return null;

        for (int i = 0; i < QuestList.Count; i++)
        {
            if (id == QuestList[i].questId_)
            {
                return QuestList[i];
            }
        }
        return null;
    }

    public static bool HasQuestByType(QuestKind type)
    {
        if (QuestList == null)
            return false;

        QuestData data = null;
        for (int i = 0; i < QuestList.Count; ++i)
        {
            data = QuestData.GetData(QuestList[i].questId_);
            if (type == data.questKind_)
                return true;
        }
        return false;
    }

    public static PointInfo GetPointInfo()
    {
        if (QuestListKind == null)
            return null;

        if (QuestListKind[(int)QuestKind.QK_Main].Count > 0)
        {
            PointInfo info = null;
            int questId = QuestListKind[(int)QuestKind.QK_Main][0].questId_;
            QuestData data = QuestData.GetData(questId);
            if (IsQuestFinish(questId))
            {
                Vector2 dest = Vector2.zero;
                GameObject[] npcLst = GameObject.FindGameObjectsWithTag("NPC");
                for(int i=0; i < npcLst.Length; ++i)
                    if(npcLst[i].name.Equals(data.finishNpcId_.ToString()))
                        dest = new Vector2(npcLst[i].transform.position.x, npcLst[i].transform.position.z);

                info = new PointInfo(data.finishSceneId_, dest);
            }
            else
            {
                info = new PointInfo(data.doingSceneId_, data.doingLocation_);
            }
            return info;
        }
        return null;
    }

    public static bool IsQuestNeedNpc(int npcid)
    {
        NpcData ndata = NpcData.GetData(npcid);
        if (ndata.NeedQuests.Length == 0)
            return true;
        for (int i = 0; i < ndata.NeedQuests.Length; ++i)
        {
            if (IsQuestDoing(ndata.NeedQuests[i]))
                return true;
        }
        return false;
    }

    public static bool IsPointNpc(int npcId)
    {
        if (QuestListKind == null)
            return false;

        if (QuestListKind[(int)QuestKind.QK_Main].Count > 0)
        {
            int questId = QuestListKind[(int)QuestKind.QK_Main][0].questId_;
            QuestData data = QuestData.GetData(questId);
            if (IsQuestFinish(questId))
            {
                GameObject[] npcLst = GameObject.FindGameObjectsWithTag("NPC");
                for (int i = 0; i < npcLst.Length; ++i)
                    if (npcLst[i].name.Equals(data.finishNpcId_.ToString()))
                        return true;
            }
        }
        return false;
    }

    public class PointInfo
    {
        public PointInfo(int sceneId, Vector2 location)
        {
            sceneId_ = sceneId;
            location_ = location;
        }

        public int sceneId_;
        public Vector2 location_;
    }

	public static List<QuestData> GetQuestDataForQuestKind(QuestKind questKind)
	{
        if (AcceptableList == null)
            return null;

		List<QuestData> qdatas = new List<QuestData> ();
		for(int i =0;i<AcceptableList.Count;i++)
		{
			if(QuestData.GetData(AcceptableList[i]).questKind_ == questKind)
			{
				qdatas.Add(QuestData.GetData(AcceptableList[i]));
			}
		}
		return qdatas;
	}

}