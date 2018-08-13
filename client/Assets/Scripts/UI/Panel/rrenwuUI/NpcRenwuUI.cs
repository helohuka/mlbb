using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class NpcRenwuUI : UIBase {

	public GameObject Duihua;
	public GameObject NpcTask;
    public UILabel NpcTaskName;
	public UILabel DialogueLabel;
	public UISprite dialogueBox;
	public Transform[] model;
    public UISprite[] namessp;
    public UILabel[] nameslbl;
	public GameObject item;
	public UIButton taskbtn;
	public UIGrid grid;

	public UISprite blackBack;
	public GameObject topBack;

    public delegate void TalkFinishCallBack();
    public static TalkFinishCallBack talkFinishCallBack_;

    public delegate void NpcRenwuUIHide();
    public static event NpcRenwuUIHide OnNpcRenwuHide;

	private UIEventListener Listener;

    bool hasDestroy_;

	static GameObject _NpcObj;
    static NpcData _NpcData;
    static int TalkId_;

    static int TalkIndex = 0;
    static List<Pair<int, string>> TalkList;
    static public int BattleId;
    static public int _NpcId;
    static int QuestId;

    public static bool talking_;

    class UniqueAsset
    {
        public UniqueAsset(int assId, GameObject go)
        {
            assetId_ = assId;
            go_ = go;
        }
        public int assetId_;
        public GameObject go_;
    }

    UniqueAsset uniqueAss_;

    void Start () 
    {
        talking_ = true;
		item.SetActive (false);
        hasDestroy_ = false;



		UIManager.SetButtonEventHandler (Duihua.gameObject, EnumButtonEvent.OnClick, OnClickTalk, 0, 0);
		UIManager.SetButtonEventHandler (blackBack.gameObject, EnumButtonEvent.OnClick, OnClickTalk, 0, 0);
		GuideManager.Instance.RegistGuideAim(Duihua.gameObject, GuideAimType.GAT_DialogUI);
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_NpcDialogBegin, TalkId_);
		topBack.GetComponent<TweenAlpha> ().SetOnFinished (OnBlackFinish);
		UIManager.SetButtonEventHandler (taskbtn.gameObject, EnumButtonEvent.OnClick, OnClicktaskbtn, 0, 0);
		GamePlayer.Instance.OpenSystemEnvetString += new RequestEventHandler<int> (UpdateOpenSystemStr);
		if (TurnOnUIPlanel.quesid != 0)
		{
//			QuestId = TurnOnUIPlanel.quesid;
//			QuestData quest = QuestData.GetData(QuestId);
			GiveupBabyQuest ();
			TurnOnUIPlanel.quesid = 0;
			return;
		}

		ShowNpcQuest();


	}
	void OnClicktaskbtn(ButtonScript obj, object args, int param1, int param2)
	{
		HideMe ();
	}
    void InitTalk(int talkId)
    {
        TalkData tdata = TalkData.GetData(talkId);
        if (null == tdata)
        {
            Hide();
            return;
        }
        TalkList = tdata.Content;
        BattleId = tdata.BattleId;
        TalkIndex = 0;
		if(Duihua !=null)
        Duihua.gameObject.SetActive(true);
        NpcTask.SetActive(false);
        Talking();
    }

    GameObject pos1;
    GameObject pos2;

    ENTITY_ID preNpcAssId = 0;
    bool Talking()
    {
        if (TalkIndex >= TalkList.Count)
            return false;

        string txt = TalkList[TalkIndex].second;

        

        NpcData npcData = NpcData.GetData(TalkList[TalkIndex].first);
        bool isSelf = (npcData == null);
        ENTITY_ID assetId = npcData == null ? (ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId) : (ENTITY_ID)npcData.AssetsID;
        if(!isSelf)
        {
            if (preNpcAssId != assetId)
            {
                if (pos2 != null)
                    DestroyImmediate(pos2);
            }
            preNpcAssId = assetId;
        }

        bool hasGameObject = isSelf ? pos1 != null : pos2 != null;

        string actorname = isSelf? GamePlayer.Instance.InstName: npcData.Name;

        string playerName = GamePlayer.Instance.InstName;
        StringTool.RepColor(ref playerName, "00FF00");
        StringTool.RepName(ref txt, playerName);

        ClearName();
        if (pos1 != null)
            pos1.SetActive(isSelf);
        if (pos2 != null)
            pos2.SetActive(!isSelf);
        ShowName(isSelf, actorname);

        if (!hasGameObject)
        {
            GameManager.Instance.GetActorClone(assetId, isSelf ? (ENTITY_ID)GamePlayer.Instance.WeaponAssetID : (ENTITY_ID)0, EntityType.ET_Player, (GameObject go, ParamData data) => 
            {
                if (hasDestroy_)
                {
                    Destroy(go);
                    return;
                }

                int dialogScale = 0;
                GlobalValue.Get(Constant.C_DialogScale, out dialogScale);
                if (data.bParam == true)
                {
                    pos1 = go;
                    pos1.transform.parent = model[0];
                    pos1.transform.localPosition = /*ector3.zero*/new Vector3(0f, 0f, -200f);
                    pos1.transform.Rotate(Vector3.up, 180f);
                    float zoom = EntityAssetsData.GetData((int)assetId).zoom_;
                    pos1.transform.localScale = new Vector3(zoom + dialogScale, zoom + dialogScale, zoom + dialogScale);
                }
                else
                {
                    pos2 = go;
                    pos2.transform.parent = model[1];
                    pos2.transform.localPosition = /*Vector3.zero*/new Vector3(0f, 0f, -200f);
                    pos2.transform.Rotate(Vector3.up, 180f);
                    float zoom = EntityAssetsData.GetData((int)assetId).zoom_;
                    pos2.transform.localScale = new Vector3(zoom + dialogScale, zoom + dialogScale, zoom + dialogScale);
                }

                //PlayerAsseMgr.DeleteAsset(assetId, false);
            }, new ParamData((int)assetId, isSelf), "UI", isSelf ? GamePlayer.Instance.DressID : 0);
        }

        NpcDialogue = txt;
        TalkIndex++;
        return true;
    }

    public void ClearName()
    {
        for (int i = 0; i < namessp.Length; ++i)
        {
            namessp[i].gameObject.SetActive(false);
        }
    }

    public void ShowName(bool self, string name)
    {
        int i = self ? 0 : 1;
        namessp[i].gameObject.SetActive(true);
        nameslbl[i].text = name;
    }

    bool firstDisplayQuest_;
	void OnClickTalk(ButtonScript obj, object args, int param1, int param2)
	{
        if (!Talking())
        {
            bool battleEntered = false;
            if (QuestId != 0) ///任务相关
            {
                if (QuestSystem.IsQuestFinish(QuestId))
                {
					QuestData qda = QuestData.GetData(QuestId);
					if(qda.questType_ == QuestType.QT_GiveItem || qda.questType_ == QuestType.QT_GiveBaby)
					{
						//TurnOnUIPlanel.SwithShowMe(QuestId,_NpcId);
					}else
					{
						NetConnection.Instance.submitQuest2(_NpcId, QuestId,0);
						QuestSystem.LocalSubmitQuest(QuestId);
					}
                    
                  
                    ///这里做任务连续操作
                    if (LinkQuest())
                    {
                        return;
                    }
                }
                else if (QuestSystem.IsQuestAcceptable(QuestId))
                {
					if(!IsJobQuestSame(QuestId))
					{
						//PopText.Instance.Show(LanguageManager.instance.GetValue( "bunengjie"));
						QuestSystem.TryAcceptQuest(QuestId);
					}
					//else
//					{
//						QuestSystem.TryAcceptQuest(QuestId);
//					}
                   
                }
                QuestId = 0;
                HideMe();
            }
            else if(NpcId == 0)
            {
                if (BattleId != 0) ///如果有战斗直接进战斗
                {
					if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < 23)
                    {
                        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterNPCBattle, BattleId);
                        GamePlayer.Instance.wait4EnterBattleId = BattleId;
                        NetConnection.Instance.enterBattle(BattleId);
                        if (BattleId == GlobalValue.GuideBattleID)
                            GuideManager.Instance.InBattleGuide_ = true;
                        HideMe();
                    }
                    else
                    {
                        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("WillEnterBattle"), () =>
                        {
                            battleEntered = true;
                            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterNPCBattle, BattleId);
                            GamePlayer.Instance.wait4EnterBattleId = BattleId;
                            NetConnection.Instance.enterBattle(BattleId);
                            if (BattleId == GlobalValue.GuideBattleID)
                                GuideManager.Instance.InBattleGuide_ = true;
                        });
                    }
                }
                HideMe();
				if(TeamSystem.IsTeamLeader())
					NetConnection.Instance.leaderCloseDialog();
            }
            else if (QuestSystem.IsQuestAcceptableNpc(_NpcId) || QuestSystem.IsQuestFinishNpc(_NpcId)) ///如果没有战斗 但是有任务 那就显示任务小框
            {
				bool hideQuest = false;
				//如果可接没有东西
				//排除不可接职业任务
				int[] hideJobQuestIdx = CutJobQuest();
                if (firstDisplayQuest_)
                {
                    firstDisplayQuest_ = false;
                    HideMe();
                }                
				
                NpcTaskName.text = _NpcData.Name;

				List<int> aQuestList = QuestSystem.GetQuestAcceptableNpc(_NpcId);
				for(int i=0; i < hideJobQuestIdx.Length; ++i)
				{
					aQuestList.RemoveAt(hideJobQuestIdx[i]);
					for(int j = i + 1; j < hideJobQuestIdx.Length; ++j)
					{
						hideJobQuestIdx[j] -= 1;
					}
				}
				if(QuestSystem.IsFDailyQuest())
				{
					for(int i=aQuestList.Count-1; i >=0; i--)
				    {
						QuestData qdd = QuestData.GetData(aQuestList[i]);
						if(qdd.questKind_ == QuestKind.QK_Daily)
						{

							aQuestList.RemoveAt(i);
						}
					}
				}

                List<int> qlist = QuestSystem.GetQuestByFinishNpc(_NpcId);
				qlist.AddRange(aQuestList);
				RefreshItems(qlist.ToArray());
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_NpcRenwuUIOpen, qlist.Count > 0? qlist[0]: 0);
                firstDisplayQuest_ = true;
				Duihua.gameObject.SetActive(false);
				if(qlist.Count == 0)
					HideMe();
//				if(IsKinDaily(aQuestList))
//				{
//					HideMe();
//				}
				NpcTask.SetActive(qlist.Count != 0);

            }
            else if (_NpcData != null && _NpcData.AssetsId != 0)
            {
                    //UIBase.AsyncLoad(_NpcData.AssetsId);
				GlobalInstanceFunction.Instance.NpcOpenUI(_NpcData.AssetsId);
                HideMe();
            }
            else
            {
                HideMe();
            }

            if (!battleEntered)
            {
                if (BattleId != 0) ///如果有战斗直接进战斗
                {
                    //新手直接进
					if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < 23)
                    {
                        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterNPCBattle, BattleId);
                        GamePlayer.Instance.wait4EnterBattleId = BattleId;
                        NetConnection.Instance.enterBattle(BattleId);
                        if (BattleId == GlobalValue.GuideBattleID)
                            GuideManager.Instance.InBattleGuide_ = true;
                        HideMe();
                    }
                    else
                    {
                        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("WillEnterBattle"), () =>
                        {
                            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterNPCBattle, BattleId);
                            GamePlayer.Instance.wait4EnterBattleId = BattleId;
                            NetConnection.Instance.enterBattle(BattleId);
                            if (BattleId == GlobalValue.GuideBattleID)
                                GuideManager.Instance.InBattleGuide_ = true;
                            HideMe();
                        });
                    }
                }
            }

            bool scriptDeal = false;
            //如果没有battle 就把npcid抛给脚本
            if(BattleId == 0)
            {
                scriptDeal = GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_NpcTalked, _NpcId, TalkId_);
            }

            if(!scriptDeal)
            {
                if (_NpcId != 0)
                    NetConnection.Instance.talkedNpc(_NpcId);
            }
        }
	}
						
	bool IsKinDaily(List<int> qlist)
	{
		for(int i =0;i<qlist.Count;i++)
		{
			QuestData qda = QuestData.GetData(qlist[i]);
			if(qda.questKind_ == QuestKind.QK_Daily)
			{
				continue;
			}else
			{
				return true;
			}
		}
		return false;
	}
	int[] CutJobQuest()
	{
		Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
		if(pro==null)return null;
		List<int> questids = new List<int> ();
		questids = QuestSystem.GetQuestAcceptableNpc (_NpcId);

		List<int> acceptQuestIdx_ = new List<int> ();
		for(int i =0;i<questids.Count;i++)
		{
			QuestData qd = QuestData.GetData (questids[i]);
			if(qd==null)return null;
			if(qd.questKind_ == QuestKind.QK_Profession)
			{
				if(qd.JobLevel_ != 1&& qd.JobLevel_ != 0)
				{
					int typeP = (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession);
					int levelP = GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel);
					if(qd.jobtype_ != typeP || qd.JobLevel_ - levelP != 1)
					{
						acceptQuestIdx_.Add(i);
					}
				}
			}
		}
		return acceptQuestIdx_.ToArray();
	}

	bool IsJobQuestSame(int quid)
	{
		QuestData qd = QuestData.GetData (quid);
		if(qd==null)return false;
		Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
		if(pro==null)return false;
		if(qd.questKind_ == QuestKind.QK_Profession)
		{
			if(qd.JobLevel_ == GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel)&&qd.jobtype_ == (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue( "bunengjie"));
				return true;
			}
//			if(qd.JobLevel_ != 1)
//			{
//				if(qd.jobtype_ != (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession) || qd.JobLevel_ - GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel) != 1)
//				{
//					return true;
//				}
//			}
		}
		return false;
	}
    bool LinkQuest()
    {
        if (QuestId == 0)
            return false;
        QuestData qdata = QuestData.GetData(QuestId);
        if (null == qdata)
            return false;
        if (qdata.postQuest_ == 0) //|| QuestSystem.IsQuestAcceptable(qdata.postQuest_))
            return false;
        for (int i = 0; i < _NpcData.Quests.Length; ++i)
        {
            if (qdata.postQuest_ == _NpcData.Quests[i])
            {
                QuestId = qdata.postQuest_;
                qdata = QuestData.GetData(QuestId);
                InitTalk(qdata.startTalk_);
                return true;
            }
        }
        return false;
    }

    //private void SetIcon(string spName)
    //{
    //    spIcon.spriteName = spName;
    //}

	public void ShowNpcQuest()
	{
        InitTalk(TalkId_);
	}

	public void RefreshItems(int[] qlist)
    {
        if (grid.transform == null)
            return;

		foreach(Transform tran in grid.transform)
		{
			Destroy(tran.gameObject);
		}

        for (int i = 0; i < qlist.Length; ++i)
        {
            QuestData qdata = QuestData.GetData(qlist[i]);
			Profession pro = Profession.get ((JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession), GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel));
			if(qdata.questKind_ == QuestKind.QK_Profession)
			{
				if(qdata.JobLevel_ != 1&& qdata.JobLevel_ != 0)
				{
					if(qdata.jobtype_ != (int)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession) || qdata.JobLevel_ - GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel) != 1)
					{
						continue;
					}
				}
			}
		
			if(qdata.questKind_ == QuestKind.QK_Daily)
			{
				if(IsQuestDaily(qdata)&&!QuestSystem.IsQuestFinish(qdata.id_)&&!QuestSystem.IsQuestDoing(qlist[i]))
				{
					continue;
				}
				if(!IsQuestDailyLevel(qdata)&& !QuestSystem.IsQuestFinish(qdata.id_)&&!QuestSystem.IsQuestDoing(qlist[i]))
				{
					continue;
				}
			}
            string stateStr = "";
            if (QuestSystem.IsQuestFinish(qlist[i]))
            {
                stateStr = "(已完成)";
            }
            else if (QuestSystem.IsQuestDoing(qlist[i]))
            {
                stateStr = "(进行中)";
            }
            else if (QuestSystem.IsQuestAcceptable(qlist[i]))
            {
                stateStr = "(可接)";
                QuestData data = QuestData.GetData(qlist[i]);
                if (data.questKind_ == QuestKind.QK_Tongji && QuestSystem.HasQuestByType(QuestKind.QK_Tongji))
                    continue;
				if (data.questKind_ == QuestKind.QK_Rand && QuestSystem.HasQuestByType(QuestKind.QK_Rand))
					continue;
            }
            else continue;


            GameObject o = GameObject.Instantiate(item) as GameObject;
            o.SetActive(true);
            o.name = o.name + i;

			QrestACell qcell =  o.GetComponent<QrestACell>();
			qcell.Qdata = qdata;
            if (stateStr.Equals(LanguageManager.instance.GetValue("Quest_Finish")))
			{
				qcell.background.spriteName = "rw_wancheng";
			}
			else
			{
				qcell.background.spriteName = "rw_weiwancheng";
			}       
			qcell.decLabel.text = qdata.questName_ + "" + stateStr;
            o.transform.parent = grid.transform;
            o.transform.localPosition = new Vector3(0, 0, 0);
            o.transform.localScale = new Vector3(1, 1, 1);
            Listener = UIEventListener.Get(o);
            Listener.onClick += OnQuestClick;
            Listener.parameter = qdata.id_;
            grid.repositionNow = true;
            if (i == 0)
            {
                GuideManager.Instance.RegistGuideAim(o, GuideAimType.GAT_FirstQuest);
            }
        }
    }
	bool IsQuestDaily( QuestData qdata)
	{
		for(int i =0;i<QuestSystem.CurrentList.Count;i++)
		{
			QuestData qdataa = QuestData.GetData(QuestSystem.CurrentList[i].questId_);
			if(qdata.questKind_ == qdataa.questKind_)
			{
				return true;
			}
		}
		return false;
	}
	bool IsQuestDailyLevel(QuestData qdata)
	{
		List<QuestData> temp = new List<QuestData> ();
		foreach(KeyValuePair<int,QuestData> par in QuestData.GetMetaData ())
		{
			if(par.Value.questKind_ == QuestKind.QK_Daily)
			{
				if(par.Value.needLevel_<=GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
				{
					temp.Add(par.Value);
				}
			}
		}
		if(temp.Count==0)
			return true;
		QuestData qdat = temp[0];
		for(int j =0;j<temp.Count;j++)
		{
			qdat = qdat.needLevel_>temp[j].needLevel_?qdat:temp[j];
		}
		if(qdata.needLevel_>=qdat.needLevel_)
		{
			return true;
		}
		return false;
	}
    void boxCallBack0()
    {
        //NetConnection.Instance.exitScene();
        NetConnection.Instance.jointLobby();
    }

    void boxCallBack1()
    {
        //NetConnection.Instance.exitScene();
        NetConnection.Instance.jointLobby();
    }
	void GiveupBabyQuest()
	{
		int talkId = 0;
		//QuestId = TurnOnUIPlanel.quesid;
		QuestData quest = QuestData.GetData(TurnOnUIPlanel.quesid);
		if (quest.questType_ == QuestType.QT_GiveBaby || quest.questType_ == QuestType.QT_GiveItem) {						
			talkId = quest.finishTalk_;
			QuestId = quest.postQuest_;
			InitTalk (talkId);
			//QuestId = 0;
		}

	}
    void OnQuestClick(GameObject sender)
    {
        QuestId = (int)UIEventListener.Get(sender).parameter;

        bool lvCheckOk = true;
        int talkId = 0;
        QuestData quest = QuestData.GetData(QuestId);
        if (quest.questKind_ == QuestKind.QK_Tongji)
        {
            if (QuestSystem.IsQuestFinish(QuestId))
            {
                talkId = quest.finishTalk_;
                InitTalk(talkId);
                return;
            }
//            int minLv = 0;
//            GlobalValue.Get(Constant.C_TongjiTeamSizeMin, out minLv);
//            if (!TeamSystem.IsInTeam())
//            {
//                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("noTeam"), () =>
//                {
//					if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
//					{
//                        PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
//					}else
//					{
//						TeamUI.ShowMe();
//					}
//                   
//                });
//                QuestId = 0;
//            }
           // else 
//				if (TeamSystem.IsInTeam() && TeamSystem.IsTeamLeader())
//            {
//                int crtCount = ActivitySystem.Instance.GetCount(ActivityType.ACT_Tongji);
//                int maxCount = DaliyActivityData.GetActivityMaxCount(ActivityType.ACT_Tongji);
//                if (crtCount / maxCount == 0)
//                {
//                    int tongjiteam = 5;
//                    if (TeamSystem.RealTeamCount() < tongjiteam)
//                    {
//                        int temQuestId = QuestId;
//                        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("suggestTeamNumber"), () =>
//                        {
//							if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
//							{
//                                PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
//							}else
//							{
//								TeamUI.ShowMe();
//							}
//                            
//                        }, false, () =>
//                        {
//							if(!IsJobQuestSame(QuestId))
//							{
								//PopText.Instance.Show(LanguageManager.instance.GetValue( "bunengjie"));
//				             QuestSystem.TryAcceptQuest(QuestId);
							//}
                            
                       // }, null, LanguageManager.instance.GetValue("gotosee"), LanguageManager.instance.GetValue("ignore"));
//                        QuestId = 0;
                    //}
               // }
           // }
//            else if (TeamSystem.IsInTeam())
//            {
//                COM_SimplePlayerInst[] infos = TeamSystem.GetTeamMembers();
//                for (int i = 0; i < infos.Length; ++i)
//                {
//                    minLv = 0;
//                    GlobalValue.Get(Constant.C_TongjiTeamMemberLevelMin, out minLv);
//                    if (infos[i].properties_[(int)PropertyType.PT_Level] < minLv)
//                    {
//
//                        PopText.Instance.Show(string.Format(LanguageManager.instance.GetValue("levelNotEnough"), infos[i].instName_));
//                        lvCheckOk = false;
//                        break;
//                    }
//                }
//                if(lvCheckOk == false)
//                    QuestId = 0;
//            }
            if (lvCheckOk && QuestId != 0)
            {
				if(!IsJobQuestSame(QuestId))
				{
					QuestSystem.TryAcceptQuest(QuestId);
				}
				QuestId = 0;
            }
            HideMe();
        }
        else if (quest.questKind_ == QuestKind.QK_Rand)
		{
			if(quest.questType_ == QuestType.QT_GiveBaby || quest.questType_ == QuestType.QT_GiveItem)
			{
				TurnOnUIPlanel.SwithShowMe(QuestId,_NpcId);
			}else
			{
				int randMaxcount = 0;
				GlobalValue.Get(Constant.C_AccecptRandQuestLimit, out randMaxcount);
				if (QuestSystem.randCount >= randMaxcount)
				{
					HideMe();
				}
				if (QuestSystem.IsQuestFinish(QuestId))
				{
					talkId = quest.finishTalk_;
					InitTalk(talkId);
				}
				else if (QuestSystem.IsQuestDoing(QuestId))
				{
					talkId = quest.proTalk_;
					InitTalk(talkId);
				}
				else if (QuestSystem.IsQuestAcceptable(QuestId))
				{
					talkId = quest.startTalk_;
					InitTalk(talkId);
				}
				else
				{
					QuestId = 0;
				}
			}
           
		}
		else if (quest.questKind_ == QuestKind.QK_Daily)
		{
			if(quest.questType_ == QuestType.QT_GiveBaby || quest.questType_ == QuestType.QT_GiveItem)
			{
				TurnOnUIPlanel.SwithShowMe(QuestId,_NpcId);
			}else
			{
				if (QuestSystem.IsQuestFinish(QuestId))
				{
					talkId = quest.finishTalk_;
					InitTalk(talkId);
				}
				else if (QuestSystem.IsQuestDoing(QuestId))
				{
					talkId = quest.proTalk_;
					InitTalk(talkId);
				}
				else if (QuestSystem.IsQuestAcceptable(QuestId))
				{
					talkId = quest.startTalk_;
					InitTalk(talkId);
				}
				else
				{
					QuestId = 0;
				}
			}
		}
		else 
        {
        	   if (QuestSystem.IsQuestFinish(QuestId))
				{
					talkId = quest.finishTalk_;
					InitTalk(talkId);
				}
				else if (QuestSystem.IsQuestDoing(QuestId))
				{
					talkId = quest.proTalk_;
					InitTalk(talkId);
				}
				else if (QuestSystem.IsQuestAcceptable(QuestId))
				{
					talkId = quest.startTalk_;
					InitTalk(talkId);
				}
				else
				{
					QuestId = 0;
				}
			}


        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_NpcRenwuPreAccept, talkId);
    }

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_NpcTaskPanel, false);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_NpcTaskPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_NpcTaskPanel);
	}

	public static void ShowDialog(int nid)
	{
        UIManager.Instance.DoDeActive();
        if(MainPanle.Instance != null)
            MainPanle.Instance.gameObject.SetActive(false);
        NpcId = nid;
		SwithShowMe();
		if(AttaclEvent.getInstance.OnSetPanelActive != null)
		{
			AttaclEvent.getInstance.OnSetPanelActive(false);
		}
		GameManager.Instance.hidechatobj ();
	}

    public static void ShowDialogByTalk(int talkId)
    {
        if (GamePlayer.Instance.leaderSkipTalk || talkId == 0)
        {
            GamePlayer.Instance.leaderSkipTalk = false;
            if (talkFinishCallBack_ != null)
            {
                talkFinishCallBack_();
                talkFinishCallBack_ = null;
            }
            return;
        }
        TalkId_ = talkId;
		SwithShowMe ();
		if(AttaclEvent.getInstance.OnSetPanelActive != null)
		{
			AttaclEvent.getInstance.OnSetPanelActive(false);
		}
		GameManager.Instance.hidechatobj ();
    }

	public static int  NpcId
	{
		set
		{
			_NpcId = value;
            _NpcData = NpcData.GetData(_NpcId);
            TalkId_ = _NpcData.NpcTalk;
            //GameObject[] npc = GameObject.FindGameObjectsWithTag("NPC");
            //for (int i = 0; i < npc.Length; ++i )
            //{
            //    if (npc[i].name.Equals(_NpcId.ToString()))
            //    {
            //        _NpcObj = npc[i]; 
            //    }
            //}
            //if(_NpcObj != null)
            //{
                //Vector3 npcPos = new Vector3(_npcObj.transform.position.x, _npcObj.transform.position.y + _npcObj.transform.GetComponent<BoxCollider>().size.y, _npcObj.transform.position.z);
				//Duihua.transform.position = GlobalInstanceFunction.WorldToUI(_npcObj.transform.position);
                //Vector3 pos = GlobalInstanceFunction.WorldToUI(npcPos);
				//Vector3 pos = GlobalInstanceFunction.WorldToUI(_npcObj.transform.position);
                //Duihua.transform.localPosition = pos;
				//pos = new Vector3(pos.x,pos.y + _npcObj.transform.GetComponent<BoxCollider>().size.y);
				//Duihua.transform.position = pos;
            //}
		}
		get
		{
			return _NpcId;
		}
	}

	public string NpcDialogue
	{
		set
		{
			DialogueLabel.text = value;
			//dialogueBox.height = DialogueLabel.height + 80;
		//	DialogueLabel.transform.localPosition = new Vector3(DialogueLabel.transform.localPosition.x,dialogueBox.transform.localPosition.y + 32f,0);
		}
		get
		{
			return DialogueLabel.text;
		}
	}

	private void OnBlackFinish()
	{
		dialogueBox.gameObject.SetActive (true);
	}

	void Refresh()
	{
		foreach(Transform tan in grid.transform)
		{
			Destroy(tan.gameObject); 
		}
	}

    public override void Destroyobj()
    {
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_NpcTaskPanel, AssetLoader.EAssetType.ASSET_UI), true);
        hasDestroy_ = true;
        talking_ = false;
    }

    public void ExcuteEvent()
    {
        if (OnNpcRenwuHide != null)
        {
            OnNpcRenwuHide();
        }
        if (talkFinishCallBack_ != null)
        {
            talkFinishCallBack_();
            talkFinishCallBack_ = null;
        }
    }

    void OnDestroy()
    {
        ExcuteEvent();

        hasDestroy_ = true;
        talking_ = false;
        if (pos1 != null)
            Destroy(pos1);
        if (pos2 != null)
            Destroy(pos2);
		GamePlayer.Instance.OpenSystemEnvetString -= UpdateOpenSystemStr;

		if(AttaclEvent.getInstance.OnSetPanelActive != null)
		{
			AttaclEvent.getInstance.OnSetPanelActive(true);
		}
		GameManager.Instance.showchatobj ();
    }

	public void UpdateOpenSystemStr(int str)
	{
        if (MainPanle.Instance == null)
            return;

		if(MainPanle.Instance.gameObject.activeSelf)
		{
			return;
		}
		if(str == (int)OpenSubSystemFlag.OSSF_EmployeePos10 || str == (int)OpenSubSystemFlag.OSSF_EmployeePos15 ||str == (int)OpenSubSystemFlag.OSSF_EmployeePos20)
		{
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_kaiqihuoban, gameObject.transform,()=>{});
		}
	}
}