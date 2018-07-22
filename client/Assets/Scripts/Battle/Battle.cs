using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Battle
{
	static private Battle inst = null;
	static public Battle Instance
	{
		get
		{
			if(inst == null)
				inst = new Battle();
			return inst;
		}
	}
	
	public enum BattleStateType
	{
        BST_None,
		BST_InitData,
		BST_InitWait,
		BST_InitFinish,
		BST_SetPossition,
		BST_Ready,
		BST_WaitForShowTime,
		BST_ShowTime,
		BST_ShowTimeWait,
		BST_ShowTimeFinish,
		BST_Battlejustice,
		BST_Max,
	}

    public delegate void NeedUpdateHandler();
    public event NeedUpdateHandler OnNeedUpdateBabyInfo;

	BattleJudgeType battleJudgeType_ = BattleJudgeType.BJT_None;
	public BattleJudgeType BattleJudgeType
	{
		set
        {
            battleJudgeType_ = value;
        }
	}

	COM_BattleOverClearing battleReward_ = null;
	public COM_BattleOverClearing BattleReward
	{
		set { battleReward_ = value; }
		get { return battleReward_; }
	}

    public int BattleBabyExp
    {
        get {
            if (BattleReward == null) return 0;
            return (int)BattleReward.babyExp_; 
        }
    }

	List<int> skillAffectIndexs_;
	public List<int> SkillAffectIndexs
	{
		get { return skillAffectIndexs_; }
	}

	public delegate void BattleOverEvent ();
	public static event BattleOverEvent BattleOver;

    SneakAttackType sneakAtkType_ = SneakAttackType.SAT_None;

	public BattleType battleType = BattleType.BT_None;

	OperateType crtOperateType_ = OperateType.OT_0;

	BattleStateType battleState_;

    List<BattleActor> battleActorsLst_;

	List<Transform> battleStagePoints_;

	Queue<COM_ReportAction> reportActionQue_;

	List<COM_ReportTarget> stateProps_;

    BattleActor crtTargetActor_;

    BattleActor crtShowTimeActor_;

    public bool Ready_ = false;

	public float reportPlaySpeed_ = 1.5f;

    public bool speedUpOn_;

	bool waitUserOperate = false;
	
	bool needCacheSkill = false;

	bool thisTurnDoNothing = false;

    bool uiOpening = false;

    public COM_BattleReport preReport_ = null;

    bool reconnectWaitNextReport_ = false;
    public bool WaitNextReport
    {
        set { reconnectWaitNextReport_ = value; }
        get { return reconnectWaitNextReport_; }
    }

    bool returnMainScene_ = false;
    public bool ReturnMainScene
    {
        set { returnMainScene_ = value; }
        get { return returnMainScene_; }
    }
	
	int selectSkillId_;
	
	public int itemId_;

    public ItemData useItem_;
	
	public int P1SkillId_;

    public int P2SkillId_;

    public int BSkillId_;

    int battleTurn_;

    int enterShowCount_, enterShowMax_;

    int battleId_;
    public int BattleID
    {
        set { battleId_ = value; }
        get { return battleId_; }
    }

    public List<int> disableLst_ = new List<int>();

    BattleActor target_;
    BattleActor Target
	{
		set { target_ = value; SendOrder(GetCrtBattleActor(), selectSkillId_); }
	}
	
	bool selectTargetFlag_ = false;
	public bool SelectFlag
	{
		set { selectTargetFlag_ = value; }
	}
	public void SetBattleState( BattleStateType	state )
	{
		battleState_ = state;
	}
    public Queue<COM_BattleReport> reportQue_;

    public void PreLoadScene(string nextscene)
    {
        ScenePreloader.Instance.PreLoadScene(nextscene);
    }

    public bool CheckBattleState(COM_BattleReport actions)
    {
        if (battleState_ != Battle.BattleStateType.BST_Max)
        {
            if (reportQue_ == null)
                reportQue_ = new Queue<COM_BattleReport>();
            reportQue_.Enqueue(actions);
            return false;
        }
        return true;
    }

	public  OperateType  GetBattleOperateState()
	{
		return crtOperateType_;
	}


#region public interface

	public void UseItem(int instId)
	{
        if (crtOperateType_ != OperateType.OT_P1 && crtOperateType_ != OperateType.OT_P2)
            return;

        COM_Item inst = BagSystem.instance.GetItemByInstId(instId);
        if(inst == null)
            inst = GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand];
        if(inst == null)
            inst = GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand];
        if (inst == null)
            return;
		ItemData item = ItemData.GetData ((int)inst.itemId_);
        if (item.slot_ == EquipmentSlot.ES_SingleHand ||
            item.slot_ == EquipmentSlot.ES_DoubleHand)
        {
            ChangeWeapon((int)inst.instId_);
        }
        else
        {
            itemId_ = item.id_;
            useItem_ = item;
            SelectSkill(item.skillId_);
        }
	}

    public void ChangeWeapon(int instId)
    {
        COM_Order order = new COM_Order();

        order.target_ = GetCrtBattleActor().BattlePos;

        order.casterId_ = GetCrtBattleActor().InstId;

        order.weaponInstId_ = instId;

        NetConnection.Instance.syncOrder(order);
    }

	public BattleActor SelfActor
	{
		get
		{
            if (battleActorsLst_ == null)
                return null;
            for (int i = 0; i < battleActorsLst_.Count; ++i)
            {
                if (battleActorsLst_[i].InstId == GamePlayer.Instance.InstId)
                    return battleActorsLst_[i];
            }
            return null;
		}
	}

    public BattleActor SelfActorBattleBaby
	{
		get
		{
            if (GamePlayer.Instance.BattleBaby == null)
            {
                ClientLog.Instance.Log("GamePlayer.Instance.BattleBaby == null");
                return null;
            }

            if (battleActorsLst_ == null)
                return null;

            for (int i = 0; i < battleActorsLst_.Count; ++i)
            {
                if (battleActorsLst_[i].InstId == GamePlayer.Instance.BattleBaby.InstId)
                    return battleActorsLst_[i];
            }
            return null;
		}
	}

	public void ResetActor(int instId)
	{
		if( null == battleActorsLst_ || null == battleStagePoints_ ) return;
		for(int i=0; i < battleActorsLst_.Count; ++i)
		{
			if(battleActorsLst_[i].InstId == instId)
			{
				Transform	point = GetStagePointByIndex( battleActorsLst_[i].BattlePos );
				battleActorsLst_[i].ControlEntity.ActorObj.transform.localPosition = point.position;
				battleActorsLst_[i].ControlEntity.ActorObj.transform.localRotation = point.rotation;
				battleActorsLst_[i].ControlEntity.ActorObj.transform.localScale = point.localScale;
				battleActorsLst_[i].ControlEntity.ActorObj.SetActive( true );
				break;
			}
		}
	}

	// skip user operation of current turn.
	public void SkipUserOperation()
	{
        thisTurnDoNothing = true;
        OnCountDownTimeOut();
    }

    // skip user operation of current turn. 死亡专用
    public void SkipUserOperationDead()
    {
        thisTurnDoNothing = true;
    }

	public void DoSkill(int skillId)
	{
		COM_Order order = new COM_Order();
        order.target_ = GetCrtBattleActor().BattlePos;
        order.casterId_ = GetCrtBattleActor().InstId;
		order.skill_ = skillId;
		NetConnection.Instance.syncOrder (order);
	}

	// disable skill
	public void DisableSkillBtn(int index)
	{
        disableLst_.Add(index);
	}


#endregion

#region control ui fun
	
	public void ShowBattleUI() { AttaclPanel.Instance.ShowUI(); }
	
	public void HideBattleUI() { AttaclPanel.Instance.HideUI(); }
	
	public void OpenCountDownUI() { AttaclPanel.Instance.StartCountDown(); }
	
	public void CloseCountDownUI() { AttaclPanel.Instance.CloseCountDown(); } 
	
	public void OpenPetUI() { AttaclPanel.Instance.OpenPetWindow(); }
	
	public void ClosePetUI() { AttaclPanel.Instance.ClosePetWindow(); }
	
	public void OpenPetBackUI() { AttaclPanel.Instance.SetBackBtnVisible(true); }
	
	public void ClosePetBackUI() { AttaclPanel.Instance.SetBackBtnVisible(false); }
	
	public void CloseBabyUI() { AttaclPanel.Instance.CloseBabyWindow (); }
	
	public void OpenBabyUI() { AttaclPanel.Instance.ShowBabyWindow (); }
	
	public void ResetBattleBtnUIState() { AttaclPanel.Instance.RecoverButtonStateNormal(); }
	
	public void CloseAllUI()
	{
		AttaclPanel.Instance.CloseCountDown ();
		AttaclPanel.Instance.ClosePetWindow ();
		AttaclPanel.Instance.CloseBabyWindow ();
		AttaclPanel.Instance.CloseScrollView ();
		AttaclPanel.Instance.closeSkillWindow ();
		AttaclPanel.Instance.closeSkillTwoWindow ();
		AttaclPanel.Instance.HideUI ();
		
		//SetAttackCounts (false);
        uiOpening = false;
	}
	
	public void CloseAllUIAndResetForBattle()
	{
		AttaclPanel.Instance.CloseCountDown ();
		AttaclPanel.Instance.ClosePetWindow ();
		AttaclPanel.Instance.CloseBabyWindow ();
		AttaclPanel.Instance.CloseScrollView ();
		AttaclPanel.Instance.closeSkillWindow ();
		AttaclPanel.Instance.closeSkillTwoWindow ();
		
		//SetAttackCounts (false);
		ResetAllEntityAction ();
	}
	
    //public void SetAttackCounts(bool bShow, int num = 1)
    //{
    //    if( null != SelfActorBattleBaby ) return;
    //    AttaclPanel.Instance.DisplayNumberOfAttacks( SelfActor.ControlEntity.ActorObj.transform.position, bShow, num);
    //}

    public void SetAttackCounts(int casterId, sbyte sec)
    {
        BattleActor actor = GetActorByInstId(casterId);
        AttaclPanel.Instance.DisplayNumberOfAttacks(actor, sec != 0, (int)sec);
    }

    COM_BattleReport currentReport_ = null;
	public void InitBattleShowTimeQueue( COM_BattleReport report )
	{
        if (battleActorsLst_ == null)
            return;

        currentReport_ = report;
		COM_ReportState temp;
		int len = report.stateIds_.Length;
		for (int i = len; i > 0; i--)
		{
			for (int j = 0; j < i - 1; j++)
			{
				if (report.stateIds_[j].add_ && !report.stateIds_[j + 1].add_)
				{
					
					temp = report.stateIds_[j];
					report.stateIds_[j] = report.stateIds_[j + 1];
					report.stateIds_[j + 1] = temp;
				}
			}
		}
		
		/// change state
		for(int i=0; i < report.stateIds_.Length; ++i)
		{
            BattleActor actor = GetActorByInstId(report.stateIds_[i].ownerId_);
            if (actor == null)
                continue;

			if(report.stateIds_[i].add_)
				actor.ControlEntity.AddState(report.stateIds_[i]);
			else
				actor.ControlEntity.RemoveState((int)report.stateIds_[i].stateId_);
		}

//		/// excute state effects
		for(int j=0; j < battleActorsLst_.Count; ++j)
		{
			if(battleActorsLst_[j] == null)
				continue;

			if(battleActorsLst_[j].ControlEntity == null)
				continue;

			battleActorsLst_[j].ControlEntity.ExcuteState(EntityActionController.StateInst.ExcuteType.ET_Work);
		}

        BattleActor bActor = null;
		/// do action list
		for( int iLoop = 0; iLoop < report.actionResults_.Length; ++iLoop )
		{
			if(report.actionResults_[iLoop].targets_.Length != 0)
			{
                bActor = GetActorByInstId(report.actionResults_[iLoop].casterId_);
                if (bActor == null)
                {
                    ApplicationEntry.Instance.PostSocketErr(57557);
                    return;
                }
                if (bActor.ControlEntity == null)
                {
                    ApplicationEntry.Instance.PostSocketErr(57557);
                    return;
                }

				/// assign targetEntity
                bActor.ControlEntity.Target = GetActorByIdx((int)report.actionResults_[iLoop].targets_[0].position_);
			}
			/// add to show queue for ready.
			reportActionQue_.Enqueue( report.actionResults_[iLoop] );
		}
		stateProps_ = new List<COM_ReportTarget> (report.targets_);

        //设置行动图标
        for (int i = 0; i < battleActorsLst_.Count; ++i)
        {
            SetAttackCounts(battleActorsLst_[i].InstId, AtkCount(battleActorsLst_[i].InstId));
        }
	}

	public COM_ReportTarget FindStateProps(int pos)
	{
		if(stateProps_ == null)
			return null;

		for(int i=0; i < stateProps_.Count; ++i)
		{
            if ((int)stateProps_[i].position_ == pos)
			{
				return stateProps_[i];
			}
		}
		return null;
	}

    public void RemoveStateProps(COM_ReportTarget props)
	{
		if(stateProps_ == null)
			return;

		if(!stateProps_.Contains(props))
			return;

		stateProps_.Remove (props);
	}
	
	public void LoadBattleSceneFinish(string sceneName)
	{
        if (!GlobalValue.isBattleScene(sceneName))
            return;

        // 由于副本场景的资源加载异步 有可能会在切到战斗场景后 之前的资源才加载好 故而使副本场景逻辑不该执行而执行
        StageMgr.OnSceneLoaded -= LoadBattleSceneFinish;
		SetBattleState( BattleStateType.BST_InitData );
	}
	
	public Transform GetStagePointByIndex( int battlePosition )
	{
		int index = battlePosition - 1;
		if( null == battleStagePoints_ || 0 == battleStagePoints_.Count || index < 0 || index >= battleStagePoints_.Count)
		{
			return null;
		}
		return battleStagePoints_[index];
	}
	
    public BattleActor GetActorByInstId(int instId)
    {
        if (battleActorsLst_ == null) return null;
        for (int iCount = 0; iCount < battleActorsLst_.Count; ++iCount)
        {
            if (battleActorsLst_[iCount] != null && instId == battleActorsLst_[iCount].InstId)
            {
                return battleActorsLst_[iCount];
            }
        }
        return null;
    }

	public int GetPairPos(int pos)
	{
		if((pos >= (int)BattlePosition.BP_Down0 && pos <= (int)BattlePosition.BP_Down4) || 
		   (pos >= (int)BattlePosition.BP_Up0 && pos <= (int)BattlePosition.BP_Up4))
		{
			return pos + 5;
		}
		else if((pos >= (int)BattlePosition.BP_Down5 && pos <= (int)BattlePosition.BP_Down9) ||
		        (pos >= (int)BattlePosition.BP_Up5 && pos <= (int)BattlePosition.BP_Up9))
		{
			return pos - 5;
		}
		else
			return pos;
	}

    public BattleActor GetActorByName(string name)
    {
        if (null == battleActorsLst_) return null;
        for (int iCount = 0; iCount < battleActorsLst_.Count; ++iCount)
        {
            if (battleActorsLst_[iCount] != null && battleActorsLst_[iCount].battlePlayer.instName_.Equals(name))
            {
                return battleActorsLst_[iCount];
            }
        }
        return null;

    }

    public BattleActor GetActorByObjName(string name)
    {
        if (null == battleActorsLst_) return null;
        for (int iCount = 0; iCount < battleActorsLst_.Count; ++iCount)
        {
            if (battleActorsLst_[iCount] != null && battleActorsLst_[iCount].GameObjectName.Equals(name))
            {
                return battleActorsLst_[iCount];
            }
        }
        return null;
    }

    public BattleActor GetActorByIdx(int index)
	{
		if( null == battleActorsLst_ ) return null;
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
		{
            if (battleActorsLst_[iCount] != null && battleActorsLst_[iCount].BattlePos == index)
			{
				return battleActorsLst_[iCount];
			}
		}
		return null;
	}
	
	public void ResumeEntityAction( int id )
	{
        BattleActor ec = GetActorByInstId(id);
		if( null == ec ) return;
        if (ec.ControlEntity == null)
        {
            //ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
		ec.ControlEntity.Operating( false );
	}

    public void PauseEntityAction(int id)
    {
        BattleActor ec = GetActorByInstId(id);
        if (null == ec) return;
        if (ec.ControlEntity == null)
        {
            //ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
        ec.ControlEntity.Operating(true);
    }
	
	public void PauseSetEntityAction()
	{
		if( null == battleActorsLst_ ) return ;
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++iCount )
		{
			if( null == battleActorsLst_[iCount].ControlEntity ) continue;
            if (battleActorsLst_[iCount].IsAi()) continue;
            battleActorsLst_[iCount].ControlEntity.Operating(true);
		}
	}
	
	public void ResetAllEntityAction()
	{
		if( null == battleActorsLst_ ) return ;
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++iCount )
		{
			if( null == battleActorsLst_[iCount].ControlEntity ) continue;
			battleActorsLst_[iCount].ControlEntity.Operating( false );
		}
	}
	
	public void ResetBattleActorAction()
	{
		if( null == crtShowTimeActor_ ) 
		{
			return ;
		}
		if( crtShowTimeActor_.isDead )
		{
			return ;
		}
		//source
		Transform	t = GetStagePointByIndex( crtShowTimeActor_.BattlePos );
		if( null == t )
		{
			return ;
		}
		//
        if (crtShowTimeActor_.ControlEntity.ActorObj != null)
        {
            crtShowTimeActor_.ControlEntity.ActorObj.transform.localRotation = t.localRotation;
        }
//		if( !crtShowTimeActor_.isDead )
//		{
//			crtShowTimeActor_.ControlEntity.PlayEntityAction( GlobalValue.ActionName , GlobalValue.Action_Idle );
			AttaclPanel.Instance.SetEnityUIVisble( crtShowTimeActor_.InstId , true );
//		}
		
		//target
		if( null == crtShowTimeActor_.ControlEntity.Target )
		{
			return ;
		}
		if( null == crtShowTimeActor_.ControlEntity.Target.ControlEntity.ActorObj )
		{
			return ;
		}
		Transform t2 = GetStagePointByIndex( crtShowTimeActor_.ControlEntity.Target.BattlePos );
		if( null == t2 )
		{
			return ;
		}
		crtShowTimeActor_.ControlEntity.Target.ControlEntity.ActorObj.transform.localRotation = t2.localRotation;
	}

	public void ResetActorDirection()
	{
		Transform	t = GetStagePointByIndex( crtShowTimeActor_.BattlePos );
		if( null == t ) return ;
        if (crtShowTimeActor_.ControlEntity.ActorObj != null)
        {
            crtShowTimeActor_.ControlEntity.ActorObj.transform.localRotation = t.localRotation;
        }
//		crtShowTimeActor_.ControlEntity.PlayEntityAction( GlobalValue.ActionName , GlobalValue.Action_Idle );
		AttaclPanel.Instance.SetEnityUIVisble( crtShowTimeActor_.InstId , true );
	}
	
	public bool bBattleShowTimeEnd()
	{
		if( null == reportActionQue_ ) return true;
		if( reportActionQue_.Count > 0 )
		{
			return false;
		}
		return true;
	}

	public void SendOrderOk(int uid)
	{
        ResumeEntityAction(uid);
        if (Ready_ == false)
            return;
		
		if(autoBattle_)
		{
			return;
		}
		
		if(uid != GamePlayer.Instance.InstId && !GamePlayer.Instance.isMineBaby(uid))
			return;
		
		if(crtOperateType_ == OperateType.OT_P1)
		{
			if(needCacheSkill)
				P1SkillId_ = selectSkillId_;
			if(HasBaby)
			{
				if(BabyIsAlive)
				{
					crtOperateType_ = OperateType.OT_B;
                    int[] typs = GetCrtBattleActor().ControlEntity.StateTypes;
                    string err = "";
                    GameScript.Call(ScriptGameEvent.SGE_CheckState, new object[] { typs }, null, ref err);
					OpenPetUI();
					waitUserOperate = true;
                    if (BSkillId_ != 0)
                    {
                        COM_Skill skillInst = GamePlayer.Instance.BattleBaby.GetSkillCore(BSkillId_);
                        if (skillInst == null)
                        {
                            BSkillId_ = 0;
                            AttaclPanel.Instance.SetSelectedSkill(0);
                        }
                        else
                        {
                            AttaclPanel.Instance.SetWhoTurn(3);
                            //SetButtonStateBySkillId(BSkillId_, true);
                            //AttaclPanel.Instance.SetSelectedSkill(BSkillId_);
                        }
                    }
                    else
                        AttaclPanel.Instance.SetSelectedSkill(0);
				}
				else
				{
					CloseCountDownUI();
					waitUserOperate = false;
				}
				HideBattleUI();
			}
			else
			{
				ResetBattleBtnUIState();
				ShowBattleUI();
				//SetAttackCounts(true, 2);
				crtOperateType_ = OperateType.OT_P2;
				waitUserOperate = true;
				if(P2SkillId_ == 0)
				{
					P2SkillId_ = GlobalValue.GetAttackID(GamePlayer.Instance.GetWeaponType());
				}
                if (selectSkillId_ != GlobalValue.GetAttackID(GamePlayer.Instance.GetWeaponType()) &&
				   selectSkillId_ != GlobalValue.SKILL_DEFENSEID &&
				   selectSkillId_ != GlobalValue.SKILL_FLEEID)
				{
                    AttaclPanel.Instance.SetButtonState(false, AttaclPanel.SKILL_BTN, AttaclPanel.ARTICLE_BTN, AttaclPanel.PET_BTN, AttaclPanel.POSITION_BTN, AttaclPanel.CATCH_BTN);
				}
                AttaclPanel.Instance.SetWhoTurn(2);
				AttaclPanel.Instance.SetPlayerBackBtnVisible(false);
                //AttaclPanel.Instance.SetSelectedSkill(P2SkillId_);
                //SetButtonStateBySkillId(P2SkillId_);
			}
		}
		else
		{
			if(needCacheSkill)
			{
				if(crtOperateType_ == OperateType.OT_P2)
					P2SkillId_ = selectSkillId_;
				else if(crtOperateType_ == OperateType.OT_B)
					BSkillId_ = selectSkillId_;
			}
			CloseCountDownUI();
			HideBattleUI();
			ClosePetBackUI();
			waitUserOperate = false;
		}
		needCacheSkill = false;
	}

    void LoadedCallBack(string sceneName)
    {
        //StageMgr.OnSceneLoaded -= LoadedCallBack;
        //if (sceneName.Equals(GlobalValue.StageName_MainScene))
        //{
        //    GameManager.SceneID = 1;
        //    Prebattle.Instance.clickedQuestId_ = 0;
        //}
        //GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterMainScene);
    }
	
	public void ExitBattle( BattleJudgeType bjt )
	{
        SetBattleState(BattleStateType.BST_Max);
        NpcRenwuUI.talkFinishCallBack_ = () =>
        {
            DoExitBattle();
        };
        int endTalk = BattleData.GetEndTalk(battleId_);
        //bool ret = GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_Talk_BattleOver, battleId_, (int)bjt);
        //如果事件没有被脚本接收，则继续战斗
        if (endTalk == 0 || battleJudgeType_ != BattleJudgeType.BJT_Win)
        {
            DoExitBattle();
            NpcRenwuUI.talkFinishCallBack_ = null;
        }
        else
            NpcRenwuUI.ShowDialogByTalk(endTalk);
	}

    void DoExitBattle()
    {
        if (BattleOver != null)
            BattleOver();
        currentReport_ = null;
        Prebattle.Instance.judgeType_ = battleJudgeType_;
        Prebattle.Instance.battleType_ = battleType;

        ResetData();

        //reset ReportPlaySpeed
        GlobalInstanceFunction.Instance.setTimeScale(1f);
        BattleID = 0;

        GameManager.Instance.ResetBattlePropCache();

        if (GameManager.Instance.nextBattle_ != null)
        {
            ComboBattle();
            GameManager.Instance.nextBattle_ = null;
            return;
        }

        //if (battleType == BattleType.BT_Guild && GuildSystem.GetMyActionPoint() <= 0)
        //    beFlied_ = true;

        if (battleReward_.isFly_)
        {
            Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
            Prebattle.Instance.clickedQuestId_ = 0;
			if(SceneData.HomeID == GameManager.SceneID)
			{
				Prebattle.Instance.ExitFromBattle();
			}
			else
			{
            	NetConnection.Instance.transforScene(SceneData.HomeID);
				Prebattle.Instance.exitFromBattle_ = true;
				ArenaSystem.Instance.openPvP = false;
			}
        }
        else
        {
            Prebattle.Instance.ExitFromBattle();
        }

		if(battleType == BattleType.BT_PET)
		{
			BattleReward = null;
		}
    }

    public void ComboBattle()
    {
        if (GameManager.Instance.nextBattle_ == null)
            return;

        Prebattle.Instance.EnterBattle();

        BattleID = (int)GameManager.Instance.nextBattle_.battleId_;
        Battle.Instance.InitBattle(GameManager.Instance.nextBattle_);
        string battleScene = BattleData.GetSceneName(BattleID);
        if (string.IsNullOrEmpty(battleScene))
        {
            SceneData ssd = SceneData.GetData(GameManager.SceneID);
            if (ssd == null)
                ssd = SceneData.GetData(PlayerData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_TableId)).defaultSceneId_);
            battleScene = ssd.battleLevelName_;
        }
        StageMgr.OnSceneLoaded += Battle.Instance.LoadBattleSceneFinish;
		StageMgr.LoadingAsyncScene(battleScene, SwitchScenEffect.SMBlindsTransition);
    }

	public void DeleteDeadEntity( int EntityID )
	{
        // 暂时不处理ai死亡后消失
        return;
		if( null == battleActorsLst_) return;
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
		{
			if(battleActorsLst_[iCount].InstId == EntityID && battleActorsLst_[iCount].IsAi())
			{
				battleActorsLst_[iCount].ControlEntity.ActorObj.SetActive( false );
				battleActorsLst_[iCount].ControlEntity.PlayerInfoUI.SetActive( false );
				battleActorsLst_[iCount].ControlEntity.StartEffect.gameObject.SetActive( false );
			}
		}
	}
	
	public void DeleteBattleEntityItem(int instId)
	{
		if(battleActorsLst_ == null)
			return;

		for( int iLoop = 0; iLoop < battleActorsLst_.Count; ++ iLoop )
		{
            if (battleActorsLst_[iLoop] == null)
                continue;

			if( battleActorsLst_[iLoop].InstId == instId )
			{
                if (battleActorsLst_[iLoop].ControlEntity != null && battleActorsLst_[iLoop].ControlEntity.PlayerInfoUI != null)
                {
                    AttaclPanel.Instance.DeletePlayerInfoUI(battleActorsLst_[iLoop].ControlEntity.PlayerInfoUI);
                    PlayerAsseMgr.DeleteAsset((ENTITY_ID)battleActorsLst_[iLoop].AssetId, false);
                    battleActorsLst_[iLoop].ControlEntity.Destroy();
                    battleActorsLst_.Remove(battleActorsLst_[iLoop]);
                    break;
                }
			}
		}
	}
	
	public void AddBattleEntityItem( BattleActor	ec , AssetLoader.AssetLoadCallback	CallBack , bool rightPos = true)
	{
		if( ec == null ) return ;
		bool hideHud;
        GameManager.Instance.GetActorClone((ENTITY_ID)ec.AssetId, (ENTITY_ID)ec.WeaponAssetID(), ec.battlePlayer.type_, (GameObject go, ParamData data) =>
        {
            BattleActor entitycontroller = data.battleActor_;
            Transform point = GetStagePointByIndex(entitycontroller.BattlePos);
            if (null == point)
            {
                ClientLog.Instance.LogError("the point is null !");
                return;
            }
            battleActorsLst_.Add(entitycontroller);
            entitycontroller.ControlEntity = new EntityActionController(go, entitycontroller.InstId);
            //entitycontroller.ControlEntity.AddOriginState(entitycontroller.battlePlayer.stateids);
            entitycontroller.ControlEntity.ActorObj.name = GlobalValue.ActorObjName + entitycontroller.InstId.ToString();
            entitycontroller.ControlEntity.ActorObj.transform.localPosition = point.position;
            entitycontroller.ControlEntity.ActorObj.transform.localRotation = point.rotation;

            if (data.bParam)
                entitycontroller.ControlEntity.ActorObj.transform.localPosition = point.position;
            else
                entitycontroller.ControlEntity.ActorObj.transform.localPosition = entitycontroller.ControlEntity.ActorObj.transform.localPosition + entitycontroller.ControlEntity.ActorObj.transform.forward * -8f;
            entitycontroller.ControlEntity.ActorObj.transform.localRotation = point.rotation;
            entitycontroller.ControlEntity.ActorObj.transform.localScale = point.localScale;
            hideHud = false;
            if (battleType == BattleType.BT_PVP && isEnemy(entitycontroller.BattlePos))
                hideHud = true;
            entitycontroller.ControlEntity.PlayerInfoUI = AttaclPanel.Instance.CreatePlayerUI(entitycontroller, hideHud);
            go.name = entitycontroller.GameObjectName;

            if(CallBack != null)
                CallBack(null, null);

            loadedNewActor++;
            if(loadedNewActor >= totalNewActor)
            {
                loadedNewActor = 0;
                totalNewActor = 0;
                if (AddNewActorCallBack != null)
                    AddNewActorCallBack();
                AddNewActorCallBack = null;
            }
        }, new ParamData(ec, ec.InstId, rightPos), "Default");
	}
	
	public void DeleteEntityAssetsAndData()
	{
		if( null == battleActorsLst_ ) return ;
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
		{
            if (battleActorsLst_[iCount].ControlEntity != null)
			    battleActorsLst_[iCount].ControlEntity.Destroy();
			PlayerAsseMgr.DeleteAsset( (ENTITY_ID)battleActorsLst_[iCount].AssetId, true );
		}
		battleActorsLst_.Clear();
	}

	public void InitData()
	{
        timeStart = false;

        timer = 0f;

		SetBattleState( BattleStateType.BST_Max );
		
		battleActorsLst_ = new List<BattleActor>();
		
		battleStagePoints_ = new List<Transform>();
		
		reportActionQue_ = new Queue<COM_ReportAction>();

        reportQue_ = new Queue<COM_BattleReport>();

        AttaclEvent.getInstance.CatchEvent = OnClickCatchPet;
		
		AttaclEvent.getInstance.attackEvent = OnClickAttackBtn;
		
		AttaclEvent.getInstance.SkillShowEvent = OnClickSelectSkillBtn;
		
		AttaclEvent.getInstance.CountDownEvent = OnCountDownTimeOut;
		
		AttaclEvent.getInstance.PetOnEvent = OnClickPetSelectSkillBtn;
		
		AttaclEvent.getInstance.DefenseEvent = OnClickDefenseBtn;
		
		AttaclEvent.getInstance.PositionEvent = OnClickChangePossitionBtn;
		
		AttaclEvent.getInstance.FleeEvent = OnClickFleeBtn;
		
		AttaclEvent.getInstance.BackEvent = OnClickPetBackBtn;
		
		AttaclEvent.getInstance.BabyEvent = OnClickBabyBtn;
		
		AttaclEvent.getInstance.AUTOEvent = OnClickAutoBtn;

        LaunchSkillListSkill();
	}

    public void NewOne()
    {
        ResetData();
        autoBattle_ = false;
        battleReward_ = null;
    }
	
	public void ResetData()
	{
        Ready_ = false;

        nextWaveGoing_ = false;

		SetBattleState( BattleStateType.BST_Max );
		
        //autoBattle_ = false;
		
		//battleJudgeType_ = BattleJudgeType.BJT_None;

        returnMainScene_ = false;

		//battleReward_ = null;

        preReport_ = null;

        if (battleStagePoints_ != null)
        {
//            for (int i = 0; i < battleStagePoints_.Count; ++i)
//            {
//                if (battleStagePoints_[i] != null && battleStagePoints_[i].gameObject != null)
//                    GameObject.Destroy(battleStagePoints_[i].gameObject);
//            }
            battleStagePoints_.Clear();
            //battleStagePoints_ = null;
        }

        if (battleActorsLst_ != null)
        {
            for (int j = 0; j < battleActorsLst_.Count; ++j)
            {
                if (battleActorsLst_[j] != null && battleActorsLst_[j].ControlEntity != null)
                    battleActorsLst_[j].ControlEntity.ClearState();

            }
        }

        if(reportQue_ != null)
        {
            reportQue_.Clear();
            reportQue_ = null;
        }

        if (reportActionQue_ != null)
        {
            reportActionQue_.Clear();
            reportActionQue_ = null;
        }

        if (stateProps_ != null)
        {
            stateProps_.Clear();
            stateProps_ = null;
        }

        if (battleActorsLst_ != null)
        {
            battleActorsLst_.Clear();
            battleActorsLst_ = null;
        }
		
		crtTargetActor_ = null;
		
		crtShowTimeActor_ = null;
		
		EffectMgr.Instance.DeleteAll();
		
		DeleteEntityAssetsAndData();

		AttaclEvent.getInstance.CatchEvent = null;
		
		AttaclEvent.getInstance.SkillShowEvent = null;
		
		AttaclEvent.getInstance.CountDownEvent = null;
		
		AttaclEvent.getInstance.attackEvent = null;
		
		AttaclEvent.getInstance.SkillShowEvent = null;
		
		AttaclEvent.getInstance.CountDownEvent = null;
		
		AttaclEvent.getInstance.PetOnEvent = null;
		
		AttaclEvent.getInstance.DefenseEvent = null;
		
		AttaclEvent.getInstance.PositionEvent = null;
		
		AttaclEvent.getInstance.FleeEvent = null;
		
		AttaclEvent.getInstance.BackEvent = null;
		
		AttaclEvent.getInstance.BabyEvent = null;
		
		AttaclEvent.getInstance.AUTOEvent = null;

		GlobalInstanceFunction.Instance.setTimeScale(1f);

        selectTargetFlag_ = false;
	}
	
	public void InitBattle(COM_InitBattle initBattle)
	{
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BeforeEnterBattle, battleId_);
		InitData();
        sneakAtkType_ = initBattle.sneakAttack_;
		battleType = initBattle.bt_;
        crtOperateType_ = initBattle.opType_;
        battleTurn_ = (int)initBattle.roundCount_ + 1;
        battleJudgeType_ = BattleJudgeType.BJT_None;
        Ready_ = false;
		battleActorsLst_ = new List<BattleActor>();

        BattleActor bActor = null;
		for(int i=0; i < initBattle.actors_.Length; ++i)
		{
	        bActor = new BattleActor();
	        bActor.SetBattlePlayer(initBattle.actors_[i]);
	        bActor.BattlePos = (int)initBattle.actors_[i].battlePosition_;
	        battleActorsLst_.Add(bActor);
		}
	}

    public delegate void AddNewActorHandler();
    public AddNewActorHandler AddNewActorCallBack = null;
    int totalNewActor, loadedNewActor;
    public void AddNewActor(COM_BattleEntityInformation[] entity, AddNewActorHandler callback = null, int total = 0, bool rightPos = true)
    {
        for (int i = 0; i < entity.Length; ++i)
        {

            BattleActor actor = new BattleActor();
            actor.SetBattlePlayer(entity[i]);
            actor.BattlePos = (int)entity[i].battlePosition_;
            AddNewActorCallBack = callback;
            totalNewActor = total;
            AddBattleEntityItem(actor, null, rightPos);
        }
    }
	
#endregion

#region all battle state fun
	void UpdateBST_Init()
	{
		InitStagePointList();
	}

	void UpdateBST_InitWait()
	{
		if( ActorAssetsLoaded )
		{
			SetBattleState( BattleStateType.BST_InitFinish );
		}
	}

	void UpdateBST_InitFinish()
	{
		SetBattleState( BattleStateType.BST_SetPossition );
	}

	void UpdateBST_SetPossition()
	{
        AttaclPanel.Instance.ShowAttackType(sneakAtkType_);
        AttaclPanel.Instance.SetSelectedSkill(0);
        AttaclPanel.Instance.InitData(SelfActor.battlePlayer.hpCrt_, SelfActor.battlePlayer.mpCrt_, SelfActor.battlePlayer.hpMax_, SelfActor.battlePlayer.mpMax_);
        SetActorPosition();
        EnterShow();
        //ResetAllStatus ();
		SetBattleState( BattleStateType.BST_Max );
	}

	void UpdateBST_ShowTime()
	{
        if (uiOpening)
            CloseAllUI();
		if( !bShowTimeLstPlayDieAction() )
		{
			if(Battle.Instance.bBattleShowTimeEnd() )
			{
                ResetAllActorNum();
				Battle.Instance.ResetBattleActorAction();
				Battle.Instance.SetBattleState( Battle.BattleStateType.BST_ShowTimeFinish );
				return ;
			}
			ClosePetBackUI();
			SetBattleState( BattleStateType.BST_ShowTimeWait );
			ResetBattleActorAction();
			DoBattleShowTimeAction();
			AttaclPanel.Instance.UpdateEnityUIPosition( true );
		}
	}

	void UpdateBST_ShowTimeFinish()
	{
        if (battleActorsLst_ == null)
            return;
		// clear all entities' target
		for(int i=0; i < battleActorsLst_.Count; ++i)
		{
            if (battleActorsLst_[i].ControlEntity == null)
                continue;

			battleActorsLst_[i].ControlEntity.Target = null;
		}
		crtTargetActor_ = null;
		SetBattleState( BattleStateType.BST_Battlejustice );
	}

    bool nextWaveGoing_;
	void UpdateBST_BattleJustice()
	{
        if (reportQue_.Count > 0)
        {
            battleTurn_++;
            ResetAllStatus();
            SetBattleState(BattleStateType.BST_Max);
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BattleTurn, battleTurn_);

            Battle.Instance.CloseAllUIAndResetForBattle();
            currentReport_ = reportQue_.Dequeue();
            Battle.Instance.InitBattleShowTimeQueue(currentReport_);
            Battle.Instance.SetBattleState(Battle.BattleStateType.BST_ShowTime);
            return;
        }

		if(battleJudgeType_ != BattleJudgeType.BJT_None)
		{
			ExitBattle(battleJudgeType_);
            battleTurn_ = 0;
		}
		else
		{
            if (nextWaveGoing_)
                return;

            if (nextWaveGoing_ == false && currentReport_ != null && currentReport_.waveEntities_.Length > 0)//(有下一波)
            {
                nextWaveGoing_ = true;
                AddNewActor(currentReport_.waveEntities_, nextWaveCallback, currentReport_.waveEntities_.Length, false);
            }
            else
            {
                //这里不需要改状态，因为没有战报，且没有结算，是网络问题，应该等待下一场战报或结算消息。这里一直update该状态就好

                battleTurn_++;
                ResetAllStatus();
                SetBattleState(BattleStateType.BST_Max);
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BattleTurn, battleTurn_);
            }
		}
	}

#endregion

#region call back
	
	void LoadEntityAssetCallBack( GameObject go , ParamData paraData )
	{
        if (battleActorsLst_ == null)
            return;

        go.SetActive(false);
        Transform bp = null;
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
		{
            if (battleActorsLst_[iCount].GameObjectName == paraData.szParam)
			{
                battleActorsLst_[iCount].ControlEntity = new EntityActionController(go, battleActorsLst_[iCount].InstId);
                if (null != battleActorsLst_[iCount].ControlEntity.ActorObj)
                {
                    battleActorsLst_[iCount].ControlEntity.ActorObj.name = battleActorsLst_[iCount].GameObjectName;
                    bp = GetStagePointByIndex(battleActorsLst_[iCount].BattlePos);
                    if (bp == null)
                    {
                        ApplicationEntry.Instance.PostSocketErr(57558);
                        return;
                    }
                    battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition = bp.position;
                }
                //battleActorsLst_[iCount].ControlEntity.AddOriginState(battleActorsLst_[iCount].battlePlayer.stateids);
			}
		}
	}

    void LoadExtraEntityAssetCallBack(GameObject go, ParamData paraData)
    {
        if (battleActorsLst_ == null)
            return;
        for (int iCount = 0; iCount < battleActorsLst_.Count; ++iCount)
        {
            if (battleActorsLst_[iCount].GameObjectName == paraData.szParam)
            {
                battleActorsLst_[iCount].ControlEntity = new EntityActionController(go, battleActorsLst_[iCount].InstId);
                //battleActorsLst_[iCount].ControlEntity.AddOriginState(battleActorsLst_[iCount].battlePlayer.stateids);
                if (null != battleActorsLst_[iCount].ControlEntity.ActorObj)
                {
                    //battleActorsLst_[iCount].ControlEntity.ActorObj.SetActive(false);
                    battleActorsLst_[iCount].ControlEntity.ActorObj.name = battleActorsLst_[iCount].GameObjectName;
					battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition = GetStagePointByIndex(battleActorsLst_[iCount].BattlePos).position;
					battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localRotation = GetStagePointByIndex(battleActorsLst_[iCount].BattlePos).rotation;
                }
            }
        }
        loadedNewActor ++;
        if(totalNewActor == loadedNewActor)
        {
            if (AddNewActorCallBack != null)
            {
                AddNewActorCallBack();
                AddNewActorCallBack = null;
            }
            totalNewActor = 0;
            loadedNewActor = 0;
        }
    }

	bool bShowTimeLstPlayDieAction()
	{
        if (battleActorsLst_ == null)
            return false;

		for( int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
		{
			if( isCanDoAction( battleActorsLst_[iCount]) )
			{
				return true;
			}
		}
		return false;
	}

	public bool isCanDoAction(BattleActor actor)
	{
		if(actor != null && actor.ControlEntity != null)
            return /*(actor.isDead && actor.ControlEntity.m_bPlayDieAcitonFinish == false) ||*/ actor.ControlEntity.CheckBeattack && actor.ControlEntity.CheckDie;
		return false;
	}

    void OnClickCatchPet(BattleActor caser)
    {
        int itemID = 0;
        GlobalValue.Get(Constant.C_CatchPetItemID, out itemID);
        useItem_ = ItemData.GetData(itemID);
        itemId_ = itemID;
        SelectSkill(useItem_.skillId_);
    }

    void OnClickFleeBtn(Entity caser) { SelectSkill(GlobalValue.SKILL_FLEEID); }

    void OnClickChangePossitionBtn(Entity caser) { SelectSkill(GlobalValue.SKILL_CHANGEPOSID); }

    void OnClickDefenseBtn(Entity caser) { SelectSkill(GlobalValue.SKILL_DEFENSEID); }
	
	void OnClickPetSelectSkillBtn( int SkillID ) { SelectSkill (SkillID); }

    void OnClickAttackBtn(Entity caser) { SelectSkill(GlobalValue.GetAttackID(GetCrtBattleActor().GetWeaponType())); }
    
	void OnClickSelectSkillBtn( int SkillID ) { SelectSkill (SkillID); }
	
	public void OnCountDownTimeOut()
	{
		HideBattleUI();
		ClosePetUI();
		AttaclPanel.Instance.CloseScrollView();
		NetConnection.Instance.syncOrderTimeout ();
	}
	
	void OnClickPetBackBtn()
	{
		ClosePetBackUI();
		OpenPetUI();
		selectTargetFlag_ = false;
        selectSkillId_ = 0;
        AttaclPanel.Instance.SetSelectedSkill(0);
	}
	
	int BABY_OUT = -1;
	void OnClickBabyBtn(int uid)
	{
		CloseBabyUI ();
		BABY_OUT = uid;
		SelectSkill (GlobalValue.SKILL_BABYINNOUT);
	}
	
	public bool autoBattle_ = false;
    bool orderSended = false;
    public void OnClickAutoBtn(Entity caser)
	{
		autoBattle_ = !autoBattle_;
        if (orderSended == false && autoBattle_ && (battleState_ == BattleStateType.BST_Max || battleState_ == BattleStateType.BST_ShowTime))
		{
			CloseAllUI();
            
			AutoModule.Instance.SendAutoOrder();
            orderSended = true;
		}
        AttaclPanel.Instance.SetAUTOBtnVisible(!autoBattle_);
        AttaclPanel.Instance.SetCanAUTOBtnVisible(autoBattle_);

        if (autoBattle_)
        {
            if (AutoModule.Instance.GetP1Order() != null && GamePlayer.Instance.GetSkillById(AutoModule.Instance.GetP1Order().skill_) != null)
                AttaclPanel.Instance.AutoPlayerPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(AutoModule.Instance.GetP1Order().skill_, (int)GamePlayer.Instance.GetSkillById(AutoModule.Instance.GetP1Order().skill_).skillLevel_);
            else
                AttaclPanel.Instance.AutoPlayerPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(1, 1);

            if (GamePlayer.Instance.BattleBaby != null)
            {
                if (AutoModule.Instance.GetBOrder() != null && GamePlayer.Instance.BattleBaby.GetSkillCore(AutoModule.Instance.GetBOrder().skill_) != null)
                    AttaclPanel.Instance.AutoPetPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(AutoModule.Instance.GetBOrder().skill_, (int)GamePlayer.Instance.BattleBaby.GetSkillCore(AutoModule.Instance.GetBOrder().skill_).skillLevel_);
                else
                    AttaclPanel.Instance.AutoPetPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(1, 1);
            }
        }
	}

    void OnSpeedUpEvent(bool on)
    {
        speedUpOn_ = on;
        //SetReportPlaySpeed(on ? 2f : 1f, true);
    }

    public void ExcuteBabyUIUpdate()
    {
        if (OnNeedUpdateBabyInfo != null)
            OnNeedUpdateBabyInfo();
    }

#endregion
	
#region logic fun

    bool timeStart = false;
    float timer = 0f;
    float waitTime = 3f;

	void InitStagePointList()
	{
        if (timeStart)
        {
            timer += RealTime.deltaTime;
            if (timer > waitTime)
            {
                timeStart = false;
                timer = 0f;
                ApplicationEntry.Instance.PostSocketErr(57558);
                return;
            }
        }

        if (battleStagePoints_ == null)
            return;

        if (battleStagePoints_.Count > 0)
            battleStagePoints_.Clear();

        GameObject Obj = null;
		for( int iCount = 0; iCount < 20; ++ iCount )
		{
            try
            {
                Obj = GameObject.Find("point" + iCount);
            }
            catch (System.Exception ex)
            {
                return;
            }
			if( null == Obj )
			{
				ClientLog.Instance.Log("Cannot find point" + iCount);
				battleStagePoints_.Clear();
                timeStart = true;
				return;
			}
			battleStagePoints_.Add( Obj.transform );
		}
        SetCameraSize();
		SetBattleState( BattleStateType.BST_InitWait );
		CreateEntityAssets();
	}

    void SetCameraSize()
    {
        if (GameManager.Instance.IsPad)
        {
            Camera.main.orthographicSize = 3;
        }
    }

	void CreateEntityAssets()
	{
		if( null == battleActorsLst_ || 0 == battleActorsLst_.Count ) return ;

        int dressId = 0;
        ItemData dress = null;
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
		{
            dress = ItemData.GetData(battleActorsLst_[iCount].battlePlayer.fashionId_);
            if (dress != null)
                dressId = dress.weaponEntityId_;
            else
                dressId = 0;
            GameManager.Instance.GetActorClone(
                (ENTITY_ID)battleActorsLst_[iCount].AssetId,
                (ENTITY_ID)battleActorsLst_[iCount].WeaponAssetID(),
                battleActorsLst_[iCount].battlePlayer.type_,
                LoadEntityAssetCallBack,
                new ParamData(battleActorsLst_[iCount].InstId, battleActorsLst_[iCount].GameObjectName),
                "Default", battleActorsLst_[iCount].InstId == GamePlayer.Instance.InstId ? GamePlayer.Instance.DressID : dressId);
		}
	}

	bool ActorAssetsLoaded
	{
		get
		{
			if(null == battleActorsLst_)
				return false;
			for(int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
			{
				if( null == battleActorsLst_[iCount].ControlEntity || null == battleActorsLst_[iCount].ControlEntity.ActorObj )
				{
					return false;
				}
			}
			return true;
		}
	}

	bool SkillCanUseMe( int SkillID , int skillLv, BattleActor target )
	{
        SkillData data = SkillData.GetData(SkillID, skillLv);
		if( null == data ) return false;
        bool yes = false;
		SkillTargetType[] aims = ConvertToEnum( data, GetCrtBattleActor(), target.BattlePos );
		for(int i=0; i < aims.Length; ++i)
		{
            if (aims[i] == data._Target_type)
                yes = true;
		}

        if(yes)
        {
            // 如果在各方的第一排 要判断隔行
            if (NeedJusticeGapLine(GetCrtBattleActor()))
            {
                int weapon = GetCrtBattleActor().battlePlayer.weaponItemId_;
                ItemData weaponItem = ItemData.GetData(weapon);
                if ((data._IsMelee && weaponItem == null) ||
				    (data._IsMelee &&
                    (weaponItem.subType_ != ItemSubType.IST_Bow) &&
                    (weaponItem.subType_ != ItemSubType.IST_Knife)))
                {
                    if (NeedJusticeGapLine(target))
                    {
                        yes = !HasTwoPeople(GetCrtBattleActor().BattlePos, target.BattlePos);
                    }
                }
            }
        }

        //如果是抓宠 判断宠物是否可抓
        if (SkillID == GlobalValue.SKILL_ZhuaChong)
        {
            if (target.battlePlayer.type_ == EntityType.ET_Monster)
            {
                BabyData baby = BabyData.GetData(target.battlePlayer.tableId_);
                if (baby._Tpye != 1 || baby._Pet == 0)
                {
                    yes = false;
                    PopText.Instance.Show(LanguageManager.instance.GetValue("cannotCatchThisMonster"), PopText.WarningType.WT_Warning, true);
                }
            }
        }

        return yes;
	}

    // 是否需要判断隔行
    bool NeedJusticeGapLine(BattleActor self)
    {
        return (self.BattlePos >= (int)BattlePosition.BP_Down0 && self.BattlePos <= (int)BattlePosition.BP_Down4) || (self.BattlePos >= (int)BattlePosition.BP_Up0 && self.BattlePos <= (int)BattlePosition.BP_Up4);
    }

    bool HasTwoPeople(int self, int aim)
    {
        bool yes = false;
        BattleActor actor1 = GetActorByBattlePos(self + 5);
        BattleActor actor2 = GetActorByBattlePos(aim + 5);
        yes = (actor1 != null && actor2 != null && !actor1.isDead && !actor2.isDead);

        return yes;
    }

    BattleActor GetActorByBattlePos(int pos)
    {
        if (battleActorsLst_ == null)
            return null;
        for (int i = 0; i < battleActorsLst_.Count; ++i)
        {
            if (battleActorsLst_[i].BattlePos == pos)
                return battleActorsLst_[i];
        }
        return null;
    }

	SkillTargetType[] ConvertToEnum( SkillData data, BattleActor target, int position )
	{
		List<SkillTargetType> aims = new List<SkillTargetType>();
		int min, max;
        //确定当前角色的队伍
        if (target.BattlePos <= (int)BattlePosition.BP_Down9)
		{
            min = (int)BattlePosition.BP_Down0;
            max = (int)BattlePosition.BP_Down9;
		}
		else
		{
			min = (int)BattlePosition.BP_Up0;
			max = (int)BattlePosition.BP_Up9;
		}

        //目标位置在己方
		if(position >= min && position <= max)
		{
            //选的是自己
            if (target.BattlePos == position)
            {
                aims.Add(SkillTargetType.STT_All);
                aims.Add(SkillTargetType.STT_Team);
                aims.Add(SkillTargetType.STT_Self);
            }
            else
            {
                aims.Add(SkillTargetType.STT_All);
                aims.Add(SkillTargetType.STT_AllNoSelf);
                aims.Add(SkillTargetType.STT_TeamNoSelf);
                aims.Add(SkillTargetType.STT_TeamDead);
                aims.Add(SkillTargetType.STT_Team);
            }
		}
		else//目标位置在敌方
		{
			aims.Add(SkillTargetType.STT_All);
			aims.Add(SkillTargetType.STT_AllNoSelf);
		}

		return aims.ToArray();
	}
	
	void SetActorPosition()
	{
        if (null == battleActorsLst_ || null == battleStagePoints_)
        {
            ApplicationEntry.Instance.PostSocketErr(57558);
            return;
        }
		for( int iCount = 0; iCount < battleActorsLst_.Count; ++ iCount )
		{
            if (null == battleActorsLst_[iCount].ControlEntity || null == battleActorsLst_[iCount].ControlEntity.ActorObj)
            {
                ApplicationEntry.Instance.PostSocketErr(57557);
                return;
            }
			Transform	point = GetStagePointByIndex( battleActorsLst_[iCount].BattlePos );
            if (null == point)
            {
                ApplicationEntry.Instance.PostSocketErr(57558);
                return;
            }
			battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition = point.position;
			battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localRotation = point.rotation;
            switch(sneakAtkType_)
            {
                case SneakAttackType.SAT_SneakAttack:
                    if (isEnemy(battleActorsLst_[iCount].BattlePos))
                        battleActorsLst_[iCount].ControlEntity.ActorObj.transform.Rotate(point.transform.up, 180f);
                    else
                        battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition = battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition + battleActorsLst_[iCount].ControlEntity.ActorObj.transform.forward * -8f;
                    break;
                case SneakAttackType.SAT_SurpriseAttack:
                    if (!isEnemy(battleActorsLst_[iCount].BattlePos))
                        battleActorsLst_[iCount].ControlEntity.ActorObj.transform.Rotate(point.transform.up, 180f);
                    else
                        battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition = battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition + battleActorsLst_[iCount].ControlEntity.ActorObj.transform.forward * -8f;
                    break;
                default:
                    battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition = battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition + battleActorsLst_[iCount].ControlEntity.ActorObj.transform.forward * -8f;
                    break;
            }
			battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localScale = point.localScale;

            
			battleActorsLst_[iCount].ControlEntity.ActorObj.SetActive( true );
		}
	}

    void EnterShow()
    {
        AttaclPanel.Instance.SetSpeedUpBtnVisable(!TeamSystem.IsInTeam());

        if (TeamSystem.IsInTeam())
        {
            speedUpOn_ = false;
            //SetReportPlaySpeed(1f, true);
        }
        else
        {
            OnSpeedUpEvent(speedUpOn_);
        }

		if(ARPCProxy.resetLocker)
		{
			for (int i = 0; i < battleActorsLst_.Count; ++i)
			{
				battleActorsLst_[i].ControlEntity.ActorObj.transform.localPosition = GetStagePointByIndex( battleActorsLst_[i].BattlePos ).transform.position;
			}
			enterShowMax_ = 1;
			EnterShowFinish();
		}
		else
		{
			GlobalInstanceFunction.Instance.Invoke(() =>
			                                       {
				if (battleActorsLst_ == null)
					return;
				for (int i = 0; i < battleActorsLst_.Count; ++i)
				{
					if(battleActorsLst_[i].ControlEntity == null)
					{
						ApplicationEntry.Instance.PostSocketErr(57558);
						return;
					}
					if (sneakAtkType_ == SneakAttackType.SAT_SneakAttack)
					{
						if (isEnemy(battleActorsLst_[i].BattlePos))
							continue;
					}
					else if (sneakAtkType_ == SneakAttackType.SAT_SurpriseAttack)
					{
						if (!isEnemy(battleActorsLst_[i].BattlePos))
							continue;
					}
					enterShowMax_++;
					BabyData monster = BabyData.GetData(battleActorsLst_[i].battlePlayer.tableId_);
					if (battleActorsLst_[i].battlePlayer.type_ == EntityType.ET_Monster && monster._IsBoss && battleId_ <= 100) //is boss battleid ????100???boss??
					{
						battleActorsLst_[i].ControlEntity.SetAnimationParam(GlobalValue.BBossIn, AnimatorParamType.APT_Boolean, true);
						battleActorsLst_[i].ControlEntity.ActorObj.transform.localPosition = GetStagePointByIndex( battleActorsLst_[i].BattlePos ).transform.position;
					}
					else
					{
						battleActorsLst_[i].BackToOrigin(EnterShowFinish, 1f);
						battleActorsLst_[i].ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, 1f);
					}
				}
				
				//battleid ????100 ?????????
				if(battleId_ <= 100)
				{
					GlobalInstanceFunction.Instance.InvokeRepeat(() =>
					                                             {
						BabyData monster = null;
						for (int i = 0; i < battleActorsLst_.Count; ++i)
						{
							monster = BabyData.GetData(battleActorsLst_[i].battlePlayer.tableId_);
							if(battleActorsLst_[i].ControlEntity == null)
							{
								ApplicationEntry.Instance.PostSocketErr(57558);
								return;
							}
							if (battleActorsLst_[i].battlePlayer.type_ == EntityType.ET_Monster && monster._IsBoss && battleActorsLst_[i].ControlEntity.IsClipFinish(GlobalValue.BBossIn))
								EnterShowFinish();
						}
					}, 0.2f);
				}
			}, 1f);
		}
	}
	
	void EnterShowFinish()
	{
		enterShowCount_++;
		if (enterShowCount_ == enterShowMax_)
		{
			GlobalInstanceFunction.Instance.ClearInvokeRepeat();
			if (battleActorsLst_ == null)
				return;
			for (int i = 0; i < battleActorsLst_.Count; ++i)
            {
                if (battleActorsLst_[i].isDead)
                {
                    battleActorsLst_[i].ControlEntity.DealEntityDie();
                }
				else
				{
                    BabyData monster = BabyData.GetData(battleActorsLst_[i].battlePlayer.tableId_);
					if (battleActorsLst_[i].battlePlayer.type_ == EntityType.ET_Monster && monster._IsBoss && battleId_ <= 100) //is boss battleid ????100 ???boss????
                    {
                        battleActorsLst_[i].ControlEntity.SetAnimationParam(GlobalValue.BBossIn, AnimatorParamType.APT_Boolean, false);
                    }
                    else
                    {
                        battleActorsLst_[i].ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, 0f);
                    }
				}
            }
            enterShowCount_ = 0;
            enterShowMax_ = 0;
            AttaclPanel.Instance.SetSpeedUpBtnVisable(!TeamSystem.IsInTeam());
            if (TeamSystem.IsInTeam())
            {
                speedUpOn_ = false;
                //SetReportPlaySpeed(1f, true);
            }
            else
            {
                OnSpeedUpEvent(speedUpOn_);
            }

            AttaclPanel.Instance.OnSpeedUp += OnSpeedUpEvent;
            NpcRenwuUI.talkFinishCallBack_ = () =>
            {
                GlobalInstanceFunction.Instance.Invoke(() =>
                {
                    AttaclPanel.Instance.HideAttackType();
                    CreatePlayerInfoUI();
                    ResetAllStatus();
                    GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterBattle, battleId_);
                    //if (reconnectWaitNextReport_)
                    //{
                    //    CloseAllUI();
                    //    reconnectWaitNextReport_ = false;
                    //   // ErrorTipsUI.ShowMe("接收战报中...请耐心等待");
                    //    PopText.Instance.Show(LanguageManager.instance.GetValue("BattleReportReceiving"));
                    //}
                    //接续战报
                    if (preReport_ != null)
                    {
                        if (CheckBattleState(preReport_))
                        {
                            CloseAllUIAndResetForBattle();
                            InitBattleShowTimeQueue(preReport_);
                            SetBattleState(Battle.BattleStateType.BST_ShowTime);
                        }
                        preReport_ = null;
                    }
                }, 1);
            };
            int beginTalk = BattleData.GetBeginTalk(battleId_);
            //bool ret = GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_Talk_BattleReady, battleId_);
            //如果事件没有被脚本接收，则继续战斗
            if (beginTalk == 0 || ARPCProxy.resetLocker)
            {
                AttaclPanel.Instance.HideAttackType();
                CreatePlayerInfoUI();
                ResetAllStatus();
                GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterBattle, battleId_);
                NpcRenwuUI.talkFinishCallBack_ = null;
                //if (reconnectWaitNextReport_)
                //{
                //    CloseAllUI();
                //    reconnectWaitNextReport_ = false;
                //    //ErrorTipsUI.ShowMe("接收战报中...请耐心等待");
                //    PopText.Instance.Show(LanguageManager.instance.GetValue("BattleReportReceiving"));
                //}
                //接续战报
                if (preReport_ != null)
                {
                    if (CheckBattleState(preReport_))
                    {
                        CloseAllUIAndResetForBattle();
                        InitBattleShowTimeQueue(preReport_);
                        SetBattleState(Battle.BattleStateType.BST_ShowTime);
                    }
                    preReport_ = null;
                }
            }
            else
            {
                NpcRenwuUI.ShowDialogByTalk(beginTalk);
            }

            Ready_ = true;

			GlobalInstanceFunction.Instance.setTimeScale(reportPlaySpeed_);

            //StageMgr.SceneLoadedFinish();

            //注册聊天气泡事件
            ChatSystem.RegMakeDirtyFunc(ShowBubble);
        }
    }

    void ShowBubble()
    {
        COM_ChatInfo chatInfo = ChatSystem.GetLastestChat();
        if (chatInfo == null)
            return;

		if(chatInfo.ck_ != ChatKind.CK_World && chatInfo.ck_ != ChatKind.CK_Team)
			return;
		if (chatInfo.audioId_ != 0)
						return;
        BattleActor bActor = GetActorByName(chatInfo.playerName_);
        if (bActor != null)
        {
            if (chatInfo.ck_ == ChatKind.CK_Team && !TeamSystem.isTeamMember(bActor.InstId))
                return;
            bActor.ControlEntity.PlayerInfoUI.GetComponent<Roleui>().ChatBubble(chatInfo.content_);
        }
    }

	void ShowSelectEffect(int SkillID , int skillLv, int position)
	{
        SkillData data = SkillData.GetData(SkillID, skillLv);
		if( null == data )
			return;

		skillAffectIndexs_ = new List<int> ();
        if (string.IsNullOrEmpty(data._SelectedTarget))
            skillAffectIndexs_.Add(position);
        else
        {
            string err = "";
            GameScript.Call(data._SelectedTarget, new object[] { SkillID, position }, null, ref err);
        }
        List<GameObject> listAim = new List<GameObject>();
		for( int i=0; i < skillAffectIndexs_.Count; ++i )
		{
            BattleActor entity = GetActorByIdx(skillAffectIndexs_[i]);
			if(null == entity || entity.isDead)
				continue;
			listAim.Add(entity.ControlEntity.ActorObj);
		}
		EffectAPI.Play( EFFECT_ID.EFFECT_SELECTTARGET , null , listAim.ToArray() , null , null , null );
	}

	bool bSkillAimMySelf( int SkillID, int skillLv)
	{
        SkillData data = SkillData.GetData(SkillID, skillLv);
		if( null == data ) return false;
		return data._Target_type == SkillTargetType.STT_Self;
	}

    int nextWaveCurrentCount_;
    void nextWaveCallback()
    {
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            if (battleActorsLst_ == null)
                return;
            for (int i = 0; i < battleActorsLst_.Count; ++i)
            {
				if(battleActorsLst_[i].ControlEntity == null)
				{
					ApplicationEntry.Instance.PostSocketErr(57558);
					return;
				}
				BabyData monster = BabyData.GetData(battleActorsLst_[i].battlePlayer.tableId_);
				if (battleActorsLst_[i].battlePlayer.type_ == EntityType.ET_Monster && monster._IsBoss && battleId_ <= 100) //is boss battleid ????100???boss??
				{
					battleActorsLst_[i].ControlEntity.SetAnimationParam(GlobalValue.BBossIn, AnimatorParamType.APT_Boolean, true);
					battleActorsLst_[i].ControlEntity.ActorObj.transform.localPosition = GetStagePointByIndex( battleActorsLst_[i].BattlePos ).transform.position;
				}
				else
				{
					battleActorsLst_[i].BackToOrigin(BeginFight, 1f);
				}
			}

			if(battleId_ <= 100)
			{
				GlobalInstanceFunction.Instance.InvokeRepeat(() =>
				                                             {
					BabyData monster = null;
					for (int i = 0; i < battleActorsLst_.Count; ++i)
					{
						monster = BabyData.GetData(battleActorsLst_[i].battlePlayer.tableId_);
						if(battleActorsLst_[i].ControlEntity == null)
						{
							ApplicationEntry.Instance.PostSocketErr(57558);
							return;
						}
						if (battleActorsLst_[i].battlePlayer.type_ == EntityType.ET_Monster && monster._IsBoss && battleActorsLst_[i].ControlEntity.IsClipFinish(GlobalValue.BBossIn))
							BeginFight();
					}
				}, 0.2f);
			}

		}, 2f);
	}
	//
	
	void BeginFight()
	{
        if (battleActorsLst_ == null)
            return;
        nextWaveCurrentCount_++;
        if (nextWaveCurrentCount_ >= battleActorsLst_.Count)
        {
            nextWaveCurrentCount_ = 0;
            //SetBattleState(BattleStateType.BST_ShowTimeFinish);
            battleTurn_++;
            ResetAllStatus();
            SetBattleState(BattleStateType.BST_Max);
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BattleTurn, battleTurn_);
            nextWaveGoing_ = false;
        }
    }

	void DoBattleShowTimeAction()
	{
        //设置行动图标
        for (int i = 0; i < preActionActor_.Count; ++i)
        {
            sbyte atkCount = AtkCount(preActionActor_[i]);
            //如果是2动 结算1动的buff 或者正常1动中的1动
            if (atkCount == 2 || atkCount == -1)
            {
                GetActorByInstId(preActionActor_[i]).ControlEntity.UpdateStateTurn();
            }
            SetAttackCounts(preActionActor_[i], atkCount);
        }
		if( null == reportActionQue_ || 0 == reportActionQue_.Count )
		{
		    SetBattleState( BattleStateType.BST_ShowTimeFinish );
			return ;
		}

        //Label1
        NpcRenwuUI.talkFinishCallBack_ = () =>
        {
            //BackAllActor();
			DoOneActorAction();
        };
        int pos = GetActorByInstId(reportActionQue_.Peek().casterId_).BattlePos;
        bool ret = GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_Talk_ActorReady, battleId_, battleTurn_, pos);
        //如果事件没有被脚本接收，则继续战斗会调用Label1
        if (!ret)
        {
            NpcRenwuUI.talkFinishCallBack_ = null;
        }
	}

    int backCounter_;
    void BackAllActor()
    {
        if (battleActorsLst_ == null)
            return;
        backCounter_ = 0;
        for (int i = 0; i < battleActorsLst_.Count; ++i)
        {
            battleActorsLst_[i].BackToOrigin(() =>
            {
                BackAllActorCallBack();
            });
        }
    }

    void BackAllActorCallBack()
    {
        if (battleActorsLst_ == null)
            return;
        backCounter_++;
        if (backCounter_ >= battleActorsLst_.Count)
            DoOneActorAction();
    }

    void ResetAllActorNum()
    {
        if (battleActorsLst_ == null)
            return;

        for (int i = 0; i < battleActorsLst_.Count; ++i)
        {
            SetAttackCounts(battleActorsLst_[i].InstId, 0);
        }
    }

    List<int> preActionActor_ = new List<int>();

    void DoOneActorAction()
    {
        preActionActor_.Clear();
        int uniteNum = reportActionQue_.Peek().uniteNum_;
        if (uniteNum > 1)
        {
            BattleActor[] casters = new BattleActor[uniteNum];
            BattlePosition aim = BattlePosition.BP_None;
            COM_ReportTarget[] props = new COM_ReportTarget[uniteNum];
            COM_ReportAction action = null;
            COM_ReportAction[] actions = new COM_ReportAction[uniteNum];

            for (int i = 0; i < uniteNum; ++i)
            {
                if (reportActionQue_.Count == 0)
                {
                    ClientLog.Instance.Log("uniteNum is : " + uniteNum + " but reportAction is not enough!");
                }
                action = reportActionQue_.Dequeue();
                actions[i] = action;
                casters[i] = GetActorByInstId(action.casterId_);
                aim = (BattlePosition)action.target_;
                if (action.targets_ == null || action.targets_.Length == 0)
                    ClientLog.Instance.Log("tagets ?????");
                props[i] = action.targets_[0];
                //SetAttackCounts(action.casterId_, action.isSec_);
            }
            TogetherAction ta = new TogetherAction();
            ta.Action(casters, aim, props, () =>
            {
                SetBattleState(Battle.BattleStateType.BST_ShowTime);
            }, actions);

            for (int i = 0; i < casters.Length; ++i)
            {
                preActionActor_.Add(casters[i].InstId);
            }
        }
        else
        {
            COM_ReportAction report = reportActionQue_.Dequeue();
            crtShowTimeActor_ = GetActorByInstId(report.casterId_);
            //SetAttackCounts(report.casterId_, report.isSec_);
            crtShowTimeActor_.BackToOrigin(() =>
            {
                crtShowTimeActor_.ControlEntity.DoAction(report);
            });

            preActionActor_.Add(report.casterId_);
        }
    }

    sbyte AtkCount(int instId)
    {
        int count = 0;
        sbyte isSec = 0;
        foreach (COM_ReportAction report in reportActionQue_)
        {
            if(report.casterId_ == instId)
            {
                isSec = report.isSec_;
                count++;
            }
        }
        //如果有两个战报 则返回1动
        if (count == 2) return 1;
        //如果是一个战报 看是否二动
        if (count == 1 && isSec == 2) return 2;
        //如果是一个战报且不是二动
        if (count == 1 && isSec != 2) return -1;
        return 0;
    }

	void CreatePlayerInfoUI()
	{
		if(null == battleActorsLst_)
			return;

		for( int iCount = 0; iCount < battleActorsLst_.Count; ++iCount )
		{
			bool hideHud = false;
			if( null == battleActorsLst_[iCount].ControlEntity )
				continue;

			if(battleType == BattleType.BT_PVP && isEnemy(battleActorsLst_[iCount].BattlePos))
				hideHud = true;
			battleActorsLst_[iCount].ControlEntity.PlayerInfoUI = AttaclPanel.Instance.CreatePlayerUI( battleActorsLst_[iCount], hideHud );
		}
	}

	public bool isEnemy(int pos)
	{
		bool selfTeam = SelfActor.BattlePos >= (int)BattlePosition.BP_Down0 && 
			SelfActor.BattlePos <= (int)BattlePosition.BP_Down9;
		bool posTeam = pos >= (int)BattlePosition.BP_Down0 && pos <= (int)BattlePosition.BP_Down9;

		return selfTeam ^ posTeam;
    }

    public Actor GetCrtActor()
    {
        switch (crtOperateType_)
        {
            case OperateType.OT_P1:
            case OperateType.OT_P2:
                return GamePlayer.Instance;
            case OperateType.OT_B:
                return GamePlayer.Instance.BattleBaby;
            default:
                return null;
        }
    }

    public BattleActor GetCrtBattleActor()
	{
		switch(crtOperateType_)
		{
		case OperateType.OT_P1:
		case OperateType.OT_P2:
			return SelfActor;
		case OperateType.OT_B:
            return SelfActorBattleBaby;
		default:
			return null;
		}
	}

    bool NeedExcuteTarget(int skillId)
	{
		selectSkillId_ = skillId;
		return !bSkillAimMySelf(selectSkillId_, 1);
	}

	bool HasBaby
	{
		get { return SelfActorBattleBaby != null; }
	}

	bool BabyIsAlive
	{
		get
		{
			if(!HasBaby)
			{
				ClientLog.Instance.LogError("Player has not battleBaby!");
			}
			return !SelfActorBattleBaby.isDead;
		}
	}

    bool launchOrder_ = false;
	void SelectSkill(int skillId)
	{
		if(!waitUserOperate)
			return;

        COM_Skill skinst = GetCrtActor().GetSkillCore(skillId);
        if (skinst == null)
            return;

        SkillData data = SkillData.GetData((int)skinst.skillID_, (int)skinst.skillLevel_);
        if (data._SkillType != SkillType.SKT_DefaultActive &&
		    data._SkillType != SkillType.SKT_DefaultSecActive)
        {
            launchOrder_ = true;
        }
        else
        {
            launchOrder_ = false;
        }

		if(NeedExcuteTarget(skillId))
		{
            AttaclPanel.Instance.SetSelectedSkill(selectSkillId_);
			selectTargetFlag_ = true;
			if(crtOperateType_ == OperateType.OT_B)
			{
				HideBattleUI();
				OpenPetBackUI();
			}
            //needCacheSkill = true;
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_SelectTarget);
			needCacheSkill = ((data._SkillType == SkillType.SKT_Active) || (data._SkillType == SkillType.SKT_DefaultSecActive));
		}
		else
		{
			if(crtOperateType_ == OperateType.OT_0)
			{
				ClientLog.Instance.LogError("Current Operate Type is OT_None!");
			}
			if(skillId != GlobalValue.SKILL_CHANGEPOSID && skillId != GlobalValue.SKILL_BABYINNOUT)
				Target = (crtOperateType_ == OperateType.OT_B?SelfActorBattleBaby: SelfActor);
			else
				Target = null;
            GetCrtBattleActor().ControlEntity.Target = target_;
            needCacheSkill = false;
		}
	}

    void SendOrder(BattleActor control, int skillId)
	{
		COM_Order order = new COM_Order();

		if(target_ != null)
			order.target_ = target_.BattlePos;
		else
			order.target_ = GetPairPos(control.BattlePos);

		order.casterId_ = control.InstId;

		if(skillId == GlobalValue.SKILL_BABYINNOUT)
			order.babyId_ = BABY_OUT;

		order.itemId_ = itemId_;
		order.skill_ = skillId;
		NetConnection.Instance.syncOrder (order);
        switch (crtOperateType_)
        {
            case OperateType.OT_P1:
                AutoModule.Instance.LaunchP1Order(launchOrder_ ? order : null);
                break;
            case OperateType.OT_P2:
                AutoModule.Instance.LaunchP2Order(launchOrder_ ? order : null);
                break;
            case OperateType.OT_B:
                AutoModule.Instance.LaunchBOrder(launchOrder_ ? order : null);
                break;
        }
        
		itemId_ = 0;
        useItem_ = null;
	}

    void ResetObjStatus()
    {
        if (battleActorsLst_ == null)
            return;
        for (int iCount = 0; iCount < battleActorsLst_.Count; ++iCount)
        {
            if (null == battleActorsLst_[iCount].ControlEntity || null == battleActorsLst_[iCount].ControlEntity.ActorObj) continue;
            Transform point = GetStagePointByIndex(battleActorsLst_[iCount].BattlePos);
            if (null == point) continue;
            battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localPosition = point.position;
            battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localRotation = point.rotation;
            battleActorsLst_[iCount].ControlEntity.ActorObj.transform.localScale = point.localScale;
            battleActorsLst_[iCount].ControlEntity.ActorObj.SetActive(true);
        }
    }

    public int[] displayList = null;
    void LaunchSkillListSkill()
    {
        List<int> list = new List<int>();
        SkillData data;
        for (int i = 0; i < GamePlayer.Instance.SkillIds.Length; ++i)
        {
            data = SkillData.GetData(GamePlayer.Instance.SkillIds[i], 1);
			if (data != null && (data._SkillType == SkillType.SKT_Active || data._SkillType == SkillType.SKT_Passive))
                list.Add(GamePlayer.Instance.SkillIds[i]);
        }
        displayList = list.ToArray();
    }

	void ResetAllStatus()
	{
        if (battleActorsLst_ == null)
            return;

        orderSended = false; 
        string err = "";
        disableLst_.Clear();
        GameScript.Call(GlobalValue.PlayerSelectSkill, new object[] { displayList, (int)GamePlayer.Instance.GetWeaponType() }, null, ref err);
        
        AttaclPanel.Instance.BattleTurn = battleTurn_;
        //for (int i = 0; i < battleActorsLst_.Count; ++i)
        //{
        //    battleActorsLst_[i].ControlEntity.UpdateStateTurn();
        //}

		if(SelfActor.isDead)
		{
			SkipUserOperationDead();
		}

		if(thisTurnDoNothing)
		{
			thisTurnDoNothing = false;
			return;
		}

        if(sneakAtkType_ == SneakAttackType.SAT_SurpriseAttack)
        {
            ///被偷袭
            CloseAllUI();
            NetConnection.Instance.syncOrderTimeout();
            sneakAtkType_ = SneakAttackType.SAT_None;
            return;
        }
        else if (sneakAtkType_ == SneakAttackType.SAT_SneakAttack)
        {
            ///偷袭
        }
        else 
        {
            ResetObjStatus();
        }
        sneakAtkType_ = SneakAttackType.SAT_None;

        PauseSetEntityAction();
		if(autoBattle_)
		{
            SelectFlag = false;
			CloseAllUI();
            OpenCountDownUI();
			AttaclPanel.Instance.SetAUTOBtnVisible(false);
			AttaclPanel.Instance.SetCanAUTOBtnVisible(true);
            if (AutoModule.Instance.GetP1Order() != null && GamePlayer.Instance.GetSkillById(AutoModule.Instance.GetP1Order().skill_) != null)
                AttaclPanel.Instance.AutoPlayerPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(AutoModule.Instance.GetP1Order().skill_, (int)GamePlayer.Instance.GetSkillById(AutoModule.Instance.GetP1Order().skill_).skillLevel_);
            else
                AttaclPanel.Instance.AutoPlayerPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(1, 1);

            if (GamePlayer.Instance.BattleBaby != null)
            {
                if (AutoModule.Instance.GetBOrder() != null && GamePlayer.Instance.BattleBaby.GetSkillCore(AutoModule.Instance.GetBOrder().skill_) != null)
                    AttaclPanel.Instance.AutoPetPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(AutoModule.Instance.GetBOrder().skill_, (int)GamePlayer.Instance.BattleBaby.GetSkillCore(AutoModule.Instance.GetBOrder().skill_).skillLevel_);
                else
                    AttaclPanel.Instance.AutoPetPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(1, 1);
            }
			//AttaclPanel.Instance.Hidebtn(true, AttaclPanel.AUTO_BTN);
            AttaclPanel.Instance.SetCancelAutoTime("3");
            GlobalInstanceFunction.Instance.Invoke(() => 
            {
                if (GamePlayer.Instance.isInBattle == false)
                    return;

                if (battleState_ == BattleStateType.BST_ShowTimeWait)
                {
                    AttaclPanel.Instance.SetCancelAutoTime("");
                    return;
                }

                if (orderSended || reportQue_ == null || reportQue_.Count > 0)
                {
                    SelectFlag = true;
                    AttaclPanel.Instance.SetCancelAutoTime("");
                    return;
                }
                AttaclPanel.Instance.SetCancelAutoTime("2");
                GlobalInstanceFunction.Instance.Invoke(() =>
                {
                    if (GamePlayer.Instance.isInBattle == false)
                        return;

                    if (battleState_ == BattleStateType.BST_ShowTimeWait)
                    {
                        AttaclPanel.Instance.SetCancelAutoTime("");
                        return;
                    }

                    if (orderSended || reportQue_ == null || reportQue_.Count > 0)
                    {
                        SelectFlag = true;
                        AttaclPanel.Instance.SetCancelAutoTime("");
                        return;
                    }
                    AttaclPanel.Instance.SetCancelAutoTime("1");
                    GlobalInstanceFunction.Instance.Invoke(() =>
                    {
                        if (GamePlayer.Instance.isInBattle == false)
                            return;

                        if (battleState_ == BattleStateType.BST_ShowTimeWait)
                        {
                            AttaclPanel.Instance.SetCancelAutoTime("");
                            return;
                        }

                        if (orderSended || reportQue_ == null || reportQue_.Count > 0)
                        {
                            SelectFlag = true;
                            AttaclPanel.Instance.SetCancelAutoTime("");
                            return;
                        }
                        AttaclPanel.Instance.SetCancelAutoTime("");
                        if (autoBattle_)
                            AutoModule.Instance.SendAutoOrder();
                        else
                            ResetAllStatusStep2();
                    }, 1f);
                }, 1f);
            }, 1f);
			return;
		}
        else
        {
            AttaclPanel.Instance.SetAUTOBtnVisible(true);
            AttaclPanel.Instance.SetCanAUTOBtnVisible(false);
        }
        ResetAllStatusStep2();
	}

    void ResetAllStatusStep2()
    {
        if (SelfActor.ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
        string err = "";
        crtOperateType_ = OperateType.OT_P1;
        int[] typs = SelfActor.ControlEntity.StateTypes;
        GameScript.Call(ScriptGameEvent.SGE_CheckState, new object[] { typs }, null, ref err);
        waitUserOperate = true;
        ResetBattleBtnUIState();
        ShowBattleUI();
		//AttaclPanel.Instance.playerIcon.gameObject.SetActive (true);
		//AttaclPanel.Instance.babyIcon.gameObject.SetActive (true);
		if (autoBattle_)
            CloseCountDownUI();
        else
            OpenCountDownUI();
        //SetAttackCounts(true);
        //AttaclPanel.Instance.UpdateEnityUIPosition(false);
        if (P1SkillId_ == 0)
        {
            P1SkillId_ = GlobalValue.GetAttackID(GetCrtBattleActor().GetWeaponType());
        }
        AttaclPanel.Instance.SetWhoTurn(1);
        //SetButtonStateBySkillId(P1SkillId_);
        //AttaclPanel.Instance.SetSelectedSkill(P1SkillId_);
        AttaclPanel.Instance.SetButtonState(true, AttaclPanel.SKILL_BTN, AttaclPanel.ARTICLE_BTN, AttaclPanel.PET_BTN, AttaclPanel.POSITION_BTN, AttaclPanel.CATCH_BTN);

        uiOpening = true;
    }

	int RandomAEnemy(int skillId)
	{
        if (battleActorsLst_ == null)
            return 0;

		List<int> posLst = new List<int> ();
		bool enemy = false;
		for(int i=0; i < battleActorsLst_.Count; ++i)
		{
			if(battleActorsLst_[i].isDead)
				continue;
			enemy = isEnemy(battleActorsLst_[i].BattlePos);
			if(enemy && !TeamForce(skillId))
			{
				posLst.Add(battleActorsLst_[i].BattlePos);
			}
            else if (!enemy && TeamForce(skillId) && battleActorsLst_[i].InstId != GamePlayer.Instance.InstId)
			{
				posLst.Add(battleActorsLst_[i].BattlePos);
			}
            else if (IncludeSelf(skillId) && battleActorsLst_[i].InstId == GamePlayer.Instance.InstId)
            {
                posLst.Add(battleActorsLst_[i].BattlePos);
            }
		}
		if(posLst.Count == 0)
			return 0;

        COM_Skill skillinst = GetCrtActor().GetSkillCore(skillId);
        SkillData skill = SkillData.GetData((int)skillinst.skillID_, (int)skillinst.skillLevel_);
        int pos = 0;
		if (skill._Id == 2361 || skill._Id == 2371 || skill._Id == 2381)
        {
            pos = GetMinHpOne(posLst);
            return pos;
        }
        else
            pos = posLst[UnityEngine.Random.Range(0, posLst.Count)];
        BattleActor hasForeman = null;
        bool foreman = false;
        //如果都在需要判断隔行的位置
        if (NeedJusticeGapLine(GetCrtBattleActor()) && NeedJusticeGapLine(GetActorByBattlePos(pos)))
        {
            //如果是打不到后排
            int weapon = GetCrtBattleActor().battlePlayer.weaponItemId_;
            ItemData weaponItem = ItemData.GetData(weapon);
            if ((skill._IsMelee && weaponItem == null) ||
			    (skill._IsMelee &&
                (weaponItem.subType_ != ItemSubType.IST_Bow) &&
                (weaponItem.subType_ != ItemSubType.IST_Knife)))
            {
                //有两个人并且活着
                foreman = HasTwoPeople(GetCrtBattleActor().BattlePos, pos);
            }
        }

        // 如果在各方的第一排 要判断隔行
        //if (NeedJusticeGapLine(GetCrtActor()))
        //{
        //    hasForeman = GetActorByBattlePos(pos + 5);
        //    if ((pos >= (int)BattlePosition.BP_Down5 && pos <= (int)BattlePosition.BP_Down9) || (pos >= (int)BattlePosition.BP_Up5 && pos <= (int)BattlePosition.BP_Up9))
        //        hasForeman = null;

        //    if (hasForeman != null)
        //    {
        //        int weapon = GetCrtActor().WeaponID;
        //        ItemData weaponItem = ItemData.GetData(weapon);
        //        if ((skill.isMelee_ && weaponItem == null) ||
        //            (skill.isMelee_ &&
        //            (weaponItem.subType_ != ItemSubType.IST_Bow) &&
        //            (weaponItem.subType_ != ItemSubType.IST_Knife)))
        //        {
        //            if (NeedJusticeGapLine(hasForeman))
        //            {
        //                foreman = HasTwoPeople(GetCrtActor().BattlePos, hasForeman.BattlePos);
        //            }
        //        }
        //    }
        //}
        if (foreman)
            return GetActorByBattlePos(pos + 5).BattlePos;
        else
            return pos;
	}

    int GetMinHpOne(List<int> posList)
    {
        float minHpPercent = 1f;
        float hppercent = 0f;
        int pos = 0;
        BattleActor actor = null;
        for (int i = 0; i < posList.Count; ++i)
        {
            actor = Battle.Instance.GetActorByBattlePos(posList[i]);
            hppercent = actor.battlePlayer.hpCrt_ * 1f / actor.battlePlayer.hpMax_;
            if (minHpPercent > hppercent)
            {
                minHpPercent = hppercent;
                pos = actor.BattlePos;
            }
        }
        return pos;
    }

	bool IncludeSelf(int skillId)
	{
        COM_Skill skinst = GetCrtActor().GetSkillCore(skillId);
        SkillData data = SkillData.GetData((int)skinst.skillID_, (int)skinst.skillLevel_);
		return data._Target_type == SkillTargetType.STT_Team;
	}

	bool TeamForce(int skillId)
	{
        COM_Skill skinst = GetCrtActor().GetSkillCore(skillId);
        SkillData data = SkillData.GetData((int)skinst.skillID_, (int)skinst.skillLevel_);
		return data._Target_type == SkillTargetType.STT_Team || data._Target_type == SkillTargetType.STT_TeamNoSelf;
	}

	void SetButtonStateBySkillId(int skillId, bool isBaby = false)
	{
		if(NeedExcuteTarget(skillId))
		{
			if(!isBaby)
			{
                if (skillId == GlobalValue.GetAttackID(GetCrtBattleActor().GetWeaponType()))
				{
					AttaclPanel.Instance.SetButtonSelectState(AttaclPanel.ATTACK_BTN);
				}
				else
				{
					AttaclPanel.Instance.SetButtonSelectState(AttaclPanel.SKILL_BTN);
					//AttaclPanel.Instance.ButtonSelect(AttaclPanel.SKILL_BTN);
//                    AttaclPanel.Instance.openSkillWindow();
				}
			}
			else
			{
				ClosePetUI();
			}
			SelectSkill(skillId);
		}
	}

	void RayCollider()
	{
		if(!selectTargetFlag_)
			return;

        if (UICamera.hoveredObject == null || GuideManager.Instance.IsGuiding_)
		{
			if(Input.GetMouseButtonUp(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hitInfo;
				BattleActor target = null;
                if (Physics.Raycast(ray, out hitInfo, 10000f, 1 << LayerMask.NameToLayer("Guide") | 1 << LayerMask.NameToLayer("Default")))
				{
                    if (GuideManager.Instance.IsGuiding_)
                    {
                        if(GuideManager.Instance.crtGuideObj != null && !GuideManager.Instance.crtGuideObj.Equals(hitInfo.transform.gameObject))
                            return;
                    }

					target = GetActorByObjName(hitInfo.collider.gameObject.name);
					if(target == null)
						return;
					
					if(!SkillCanUseMe(selectSkillId_, 1, target))
						return;
					
					Target = target;
					
					ShowSelectEffect( selectSkillId_ , 1, target.BattlePos );
					GetCrtBattleActor().ControlEntity.Target = target_;
					selectTargetFlag_ = false;
                    GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_SelectTargetOk);
				}
			}
		}
	}

    public void SetAutoBattle(bool open)
    {
        autoBattle_ = open;
    }
	
	public void Update()
	{
		RayCollider();
		switch( battleState_ )
		{
		case BattleStateType.BST_InitData:
		{
			UpdateBST_Init();
		}
			break;
		case BattleStateType.BST_InitWait:
		{
			UpdateBST_InitWait();
		}
			break;
		case BattleStateType.BST_InitFinish:
		{
			UpdateBST_InitFinish();
		}
			break;
		case BattleStateType.BST_SetPossition:
		{
			UpdateBST_SetPossition();
		}
			break;
		case BattleStateType.BST_ShowTime:
		{
			UpdateBST_ShowTime();
		}
			break;
		case BattleStateType.BST_ShowTimeFinish:
		{
			UpdateBST_ShowTimeFinish();
		}
			break;
		case BattleStateType.BST_Battlejustice:
		{
			UpdateBST_BattleJustice();
		}
			break;
		default:
		{
			
		}
			break;
		}

        if (TeamSystem.MemberCount > 1)
        {
            if (reportQue_ != null)
            {
                float ax = reportQue_.Count == 0 ? (StageMgr.HasNextScene() ? 3f : 1f) : (reportQue_.Count + 1) * 1f;
                ax *= reportPlaySpeed_;

                GlobalInstanceFunction.Instance.setTimeScale(ax);
            }
        }
	}

	public class AutoModule
	{
		static private AutoModule inst = null;
		static public AutoModule Instance
		{
			get
			{
				if(inst == null)
					inst = new AutoModule();
				return inst;
			}
		}

		COM_Order P1Order = null;
		COM_Order P2Order = null;
		COM_Order BOrder = null;

		public void LaunchP1Order(COM_Order order) 
        {
            P1Order = order;
            if (order != null && GamePlayer.Instance.GetSkillById(order.skill_) != null)
                AttaclPanel.Instance.AutoPlayerPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(order.skill_, (int)GamePlayer.Instance.GetSkillById(order.skill_).skillLevel_);
            else
                AttaclPanel.Instance.AutoPlayerPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(1, 1);
        }

		public void LaunchP2Order(COM_Order order) { P2Order = order; }

        public void LaunchBOrder(COM_Order order)
        {
            BOrder = order;
            if (order != null && GamePlayer.Instance.BattleBaby.GetSkillCore(order.skill_) != null)
                AttaclPanel.Instance.AutoPetPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(order.skill_, (int)GamePlayer.Instance.BattleBaby.GetSkillCore(order.skill_).skillLevel_);
            else
                AttaclPanel.Instance.AutoPetPanel.GetComponent<ChangeAutoOrder>().UpdateIcon(1, 1);
        }

		public void SendAutoOrder()
		{
            bool hasSkill = false;
			if(!Battle.Instance.SelfActor.isDead)
			{
                Battle.Instance.crtOperateType_ = OperateType.OT_P1;
                ClientLog.Instance.Log("Player Send P1 Order " + Battle.Instance.battleTurn_);
                hasSkill = P1Order != null && GamePlayer.Instance.GetSkillCore(P1Order.skill_) != null;
				if(hasSkill && GamePlayer.Instance.GetSkillCost(P1Order.skill_) <= GamePlayer.Instance.GetIprop(PropertyType.PT_MpCurr))
					SendRandomAimOrder(P1Order);
				else
					SendDefaultOrder(Battle.Instance.SelfActor);
			}
            if (Battle.Instance.SelfActorBattleBaby != null)
			{
                Battle.Instance.crtOperateType_ = OperateType.OT_B;
				if(!Battle.Instance.SelfActorBattleBaby.isDead)
				{
                    ClientLog.Instance.Log("Player Send Baby Order " + Battle.Instance.battleTurn_);
                    hasSkill = BOrder != null && GamePlayer.Instance.BattleBaby.GetSkillCore(BOrder.skill_) != null;
                    if (hasSkill && GamePlayer.Instance.BattleBaby.GetSkillCost(BOrder.skill_) <= Battle.Instance.SelfActorBattleBaby.battlePlayer.mpCrt_)
						SendRandomAimOrder(BOrder);
					else
						SendDefaultOrder(Battle.Instance.SelfActorBattleBaby);
				}
			}
			else
			{
                Battle.Instance.crtOperateType_ = OperateType.OT_P2;
                ClientLog.Instance.Log("Player Send P2 Order " + Battle.Instance.battleTurn_);
                hasSkill = P2Order != null && GamePlayer.Instance.GetSkillCore(P2Order.skill_) != null;
                if (hasSkill && GamePlayer.Instance.GetSkillCost(P2Order.skill_) <= GamePlayer.Instance.GetIprop(PropertyType.PT_MpCurr))
					SendRandomAimOrder(P2Order);
				else
					SendDefaultOrder(Battle.Instance.SelfActor);
			}
		}

        public void ChangeOrder(OperateType type, int skillId)
        {
            switch (type)
            {
                case OperateType.OT_P1:
                    if (P1Order != null)
                    {
                        P1Order.skill_ = skillId;
                    }
                    else
                    {
                        if (skillId != 0)
                        {
                            P1Order = new COM_Order();
                            P1Order.skill_ = skillId;
                            P1Order.casterId_ = GamePlayer.Instance.InstId;
                        }
                    }
                    Battle.Instance.P1SkillId_ = skillId;
                    //AttaclPanel.Instance.SetSelectedSkill(skillId);
                    break;
                case OperateType.OT_P2:
                    if (P2Order != null)
                    {
                        P2Order.skill_ = skillId;
                    }
                    else
                    {
                        if (skillId != 0)
                        {
                            P2Order = new COM_Order();
                            P2Order.skill_ = skillId;
                            P2Order.casterId_ = GamePlayer.Instance.InstId;
                        }
                    }
                    Battle.Instance.P2SkillId_ = skillId;
                    break;
                case OperateType.OT_B:
                    if (GamePlayer.Instance.BattleBaby != null)
                    {
                        if (BOrder != null)
                        {
                            BOrder.skill_ = skillId;
                        }
                        else
                        {
                            if (skillId != 0)
                            {
                                BOrder = new COM_Order();
                                BOrder.skill_ = skillId;
                                BOrder.casterId_ = GamePlayer.Instance.BattleBaby.InstId;
                            }
                        }
                        Battle.Instance.BSkillId_ = skillId;
                    }
                    break;
            }
        }

		void SendRandomAimOrder(COM_Order order)
		{
			int skillId = order.skill_;
			if(order.itemId_ != 0)
				skillId = ItemData.GetData(order.itemId_).skillId_;
            COM_Skill skinst = Battle.Instance.GetCrtActor().GetSkillCore(skillId);
            if (skinst == null)
            {
                skillId = GlobalValue.GetAttackID(Battle.inst.GetCrtBattleActor().GetWeaponType());
                order.skill_ = skillId;
            }
            else
            {
                SkillData skdata = SkillData.GetData((int)skinst.skillID_, (int)skinst.skillLevel_);
                if (skdata._SkillType == SkillType.SKT_DefaultSecActive && skdata._Target_type != SkillTargetType.STT_Self)
                {
                    skillId = GlobalValue.GetAttackID(Battle.inst.GetCrtBattleActor().GetWeaponType());
                    order.skill_ = skillId;
                }
            }
            order.casterId_ = Battle.inst.GetCrtBattleActor().InstId;
			order.target_ = Battle.Instance.RandomAEnemy (skillId);
			NetConnection.Instance.syncOrder (order);
		}

		void SendDefaultOrder(BattleActor caster)
		{
			COM_Order order = new COM_Order();

            int aimPos = Battle.Instance.RandomAEnemy(GlobalValue.GetAttackID(Battle.inst.GetCrtBattleActor().GetWeaponType()));
			caster.ControlEntity.Target = Battle.Instance.GetActorByIdx(aimPos);
			order.target_ = aimPos;
			order.casterId_ = caster.InstId;
            order.skill_ = GlobalValue.GetAttackID(Battle.inst.GetCrtBattleActor().GetWeaponType());

			NetConnection.Instance.syncOrder (order);
		}

        public COM_Order GetP1Order()
        {
            return P1Order;
        }

        public COM_Order GetP2Order()
        {
            return P2Order;
        }

        public COM_Order GetBOrder()
        {
            return BOrder;
        }
	}
	#endregion
}




