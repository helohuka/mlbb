using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class EntityActionController
{

#region mem value

	private	int								m_PlayerID;
    private BattleActor                     m_TargetEntity;
	public	bool							m_bPlayDieAcitonFinish;
	public	bool							m_bPlayingBeattackAction;
	private AssetBundle						weaponAssetBundle_;

	public delegate void UpdateWeaponHandler();
	public UpdateWeaponHandler updateWeaponCallBack_;

	public COM_ReportAction						reportAction_;
	List<COM_ReportActionCounter>			counterLst_;

	public List<StateInst>					stateList_ = null;
	public int[] StateTypes
	{
		get
		{
			if(stateList_ == null)
				return null;

			List<int> states = new List<int>();
			for(int i=0; i < stateList_.Count; ++i)
			{
				int type = StateData.GetData(stateList_[i].stateId_)._StateType;
				if(states.Contains(type))
					continue;
				states.Add(type);
			}
			return states.ToArray();
		}
	}

	private SkillInst						mainSkillInst = null;

	public void AddState(COM_ReportState state, bool excuteOnce = false)
	{
		if(stateList_ == null)
			return;
        //for(int i=0; i < stateList_.Count; ++i)
        //{
        //    if(stateList_[i].stateId_ == state.stateId_)
        //        return;
        //}
		stateList_.Add (GetStateInst(state, excuteOnce));
	}

    public void AddOriginState(COM_ReportState[] stateids)
    {
        if (stateList_ == null)
            return;

        for (int i = 0; i < stateids.Length; ++i)
        {
            stateList_.Add(GetStateInst(stateids[i]));
        }
    }

	StateInst GetStateInst(COM_ReportState state, bool once = false)
	{
		StateInst tstate = new StateInst ();
		tstate.stateId_ = (int)state.stateId_;
		tstate.tick_ = state.tick_;
		tstate.turn_ = state.turn_;
		tstate.once_ = once;
		return tstate;
	}

	public void RemoveState(int stateId)
	{
		if(stateList_ == null)
			return;

        if (renderer_ == null)
            renderer_ = GetBody(ActorObj);

        if(renderer_ == null)
            renderer_ = new SkinnedMeshRenderer[0];

		for(int i=0; i < stateList_.Count; ++i)
		{
			if(stateList_[i].stateId_ == stateId)
			{
                if (stateList_[i].workingInst_ != null)
                {
                    stateList_[i].workingInst_.DestorySelf();
                    if (renderer_ != null)
                    {
                        for (int j = 0; j < renderer_.Length; ++j)
                        {
                            renderer_[j].material.SetColor("_Color", Color.white);
                            renderer_[j].material.SetColor("_RimColor", Color.black);
                            renderer_[j].material.SetFloat("_RimWidth", 0f);
                        }
                    }
                }
                stateList_.RemoveAt(i);
				return;
			}
		}
	}

	public void ClearState()
	{
		if(stateList_ == null)
			return;

        if (renderer_ == null)
            renderer_ = GetBody(ActorObj);

        if (renderer_ == null)
            renderer_ = new SkinnedMeshRenderer[0];

		for(int i=0; i < stateList_.Count; ++i)
		{
            if (stateList_[i].workingInst_ != null)
            {
                stateList_[i].workingInst_.DestorySelf();
                if (renderer_ != null)
                {
                    for (int j = 0; j < renderer_.Length; ++j)
                    {
                        renderer_[j].material.SetColor("_Color", Color.white);
                        renderer_[j].material.SetColor("_RimColor", Color.black);
                        renderer_[j].material.SetFloat("_RimWidth", 0f);
                    }
                }
            }
		}
		stateList_.Clear ();
		//stateList_ = null;
	}

	public BattleActor	Target 
	{
		set { m_TargetEntity = value; }
		get { return m_TargetEntity; }
	}
	
	public int PlayerID
	{
		set { m_PlayerID = value; }
		get { return m_PlayerID; }
	}

	public GameObject ActorObj
	{
		set { m_EntityObj = value; }
		get { return m_EntityObj; }
	}

	public GameObject WeaponObj
	{
		set { m_WeaponObj = value; }
		get { return m_WeaponObj; }
	}

#endregion


	public EntityActionController( GameObject	obj , int playerID )
	{
		stateList_ = new List<StateInst> ();
		counterLst_ = new List<COM_ReportActionCounter> ();
		m_EntityObj = obj;
        //GlobalInstanceFunction.Instance.Invoke(() => { renderer_ = GetBody(m_EntityObj); }, 1);
        
		PlayerID = playerID;
		if( null == m_EntityObj ) return ;
		m_EntityAnimator = m_EntityObj.GetComponent<Animator>();
		RegisterActionCallBack();
		ResetParam ();
        //LoadWeaponAsset ();
        hitoverEffectPlayed = false;
	}

    SkinnedMeshRenderer[] GetBody(GameObject go)
    {
        if (go == null)
            return null;
        SkinnedMeshRenderer[] smr = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        if(smr == null || smr.Length == 0)
            return null;

        List<SkinnedMeshRenderer> renders = new List<SkinnedMeshRenderer>();
        for (int i = 0; i < smr.Length; ++i)
        {
            if (smr[i].CompareTag("Body"))
                renders.Add(smr[i].GetComponent<SkinnedMeshRenderer>());
        }
        return renders.ToArray();
    }

	void LoadWeaponAsset()
	{
		BattleActor actor = Battle.Instance.GetActorByInstId (PlayerID);
		if(actor.battlePlayer.weaponItemId_ != 0)
		{
            string weaponName = EntityAssetsData.GetData(actor.WeaponAssetID()).assetsName_;
            string resPath = string.Format("{0}_{1}", actor.AssetId, weaponName);
			AssetLoader.LoadAssetBundle(resPath, AssetLoader.EAssetType.ASSET_WEAPON, (AssetBundle asset, ParamData data) => {
				weaponAssetBundle_ = asset;
				WeaponObj = (GameObject)GameObject.Instantiate(asset.mainAsset) as GameObject;
				WeaponObj.transform.parent = m_EntityObj.transform;
			}, null);
		}
	}

	public void UpdateWeaponOutLook(int itemId, UpdateWeaponHandler callback = null)
	{
		updateWeaponCallBack_ = callback;
		if(itemId == 0)
		{
			if(weaponAssetBundle_ != null)
			{
				AssetInfoMgr.Instance.DecRefCount( weaponAssetBundle_, false );
			}
			if(updateWeaponCallBack_ != null)
			{
				updateWeaponCallBack_();
				updateWeaponCallBack_ = null;
			}
			return;
		}
        BattleActor actor = Battle.Instance.GetActorByInstId(PlayerID);
        string weaponName = EntityAssetsData.GetData(ItemData.GetData(itemId).weaponEntityId_).assetsName_;
        string resPath = string.Format("{0}_{1}", actor.AssetId, weaponName);
        ItemData weapon = ItemData.GetData(itemId);
		AssetLoader.LoadAssetBundle(resPath, AssetLoader.EAssetType.ASSET_WEAPON, (AssetBundle asset, ParamData data) => {
			if(weaponAssetBundle_ != null)
			{
				AssetInfoMgr.Instance.DecRefCount( weaponAssetBundle_, false );
			}
			WeaponObj = (GameObject)GameObject.Instantiate(asset.mainAsset) as GameObject;
            WeaponObj.transform.parent = m_EntityObj.transform.FindChild(EntityAssetsData.GetData(weapon.weaponEntityId_).bindPoint_);
			if(updateWeaponCallBack_ != null)
			{
				updateWeaponCallBack_();
				updateWeaponCallBack_ = null;
			}
		}, null);
	}

	public void Destroy()
	{
		DestroyEntity();
		DestroyEffectAssets();
	}

	public COM_ReportActionCounter GetAimCounter(int pos)
	{
		COM_ReportActionCounter counter = null;
        BattleActor actor = Battle.Instance.GetActorByIdx(pos);
        if (actor == null)
            return null;
        int casterId = actor.InstId;
		for(int i=0; i < counterLst_.Count; ++i)
		{
			if(casterId == counterLst_[i].casterId_)
			{
				counter = counterLst_[i];
				counterLst_.RemoveAt(i);
				break;
			}
		}
		return counter;
	}

    void RefuseAction()
    {
        ClientLog.Instance.Log("宠物 No！");
        EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_ChongwuNo, ApplicationEntry.Instance.uiRoot.transform, () =>
        {
            
        }, (GameObject go) =>
        {
            if (go != null && ActorObj != null)
            {
                go.transform.parent = ActorObj.transform;
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
            }
			//ClientLog.Instance.Log("宠物 No！  ContinueAction");
        });
		ContinueAction();
    }

	public void DoAction(COM_ReportAction action)
	{
        //lagStateList.Clear();
        reportAction_ = action;
        if (reportAction_.isNo_)
            RefuseAction();
        else
            ContinueAction();
	}

    void OnStateFinish()
    {
        FinishShow();
    }

    public List<COM_ReportState> lagStateList = new List<COM_ReportState>();

    public void DealLagState()
    {
        for (int i = 0; i < lagStateList.Count; )
        {
            lagStateList[i].addQueue_ -= 1;
            if (lagStateList[i].addQueue_ < 0)
            {
                BattleActor actor = Battle.Instance.GetActorByInstId(lagStateList[i].ownerId_);
                if (lagStateList[i].add_)
                    actor.ControlEntity.AddState(lagStateList[i]);
                else
                    actor.ControlEntity.RemoveState((int)lagStateList[i].stateId_);
                lagStateList.RemoveAt(i);
            }
            else
                i++;
        }
    }

    public List<COM_ReportState> afterActionBuff = new List<COM_ReportState>();

    public void DealAfterActionBuff()
    {
        for (int i = 0; i < afterActionBuff.Count; ++i)
        {
            if (afterActionBuff[i].add_)
                AddState(afterActionBuff[i]);
            else
                RemoveState((int)afterActionBuff[i].stateId_);
        }
        afterActionBuff.Clear();
    }

    void ContinueAction()
    {
        // make counter list
        for (int j = 0; j < reportAction_.counters_.Length; ++j)
        {
            counterLst_.Add(reportAction_.counters_[j]);
        }

        /// add state
        for (int i = 0; i < reportAction_.stateIds_.Length; ++i)
        {
            BattleActor actor = Battle.Instance.GetActorByInstId(reportAction_.stateIds_[i].ownerId_);
            //如果大于1 则延迟加buff
            if(reportAction_.stateIds_[i].addQueue_ > 0)
            {
                //如果目标是自己 加到行动后state列表，否则加到延迟state列表
                if (reportAction_.stateIds_[i].ownerId_ == m_PlayerID)
                    afterActionBuff.Add(reportAction_.stateIds_[i]);
                else
                    actor.ControlEntity.lagStateList.Add(reportAction_.stateIds_[i]);
                continue;
            }
            if (reportAction_.stateIds_[i].add_)
                actor.ControlEntity.AddState(reportAction_.stateIds_[i]);
            else
                actor.ControlEntity.RemoveState((int)reportAction_.stateIds_[i].stateId_);
        }

        /// deal self state.
        ExcuteState(StateInst.ExcuteType.ET_Action, (COM_ReportActionCounter counter) =>
        {

            /// do action
            BattleActor caster = Battle.Instance.GetActorByInstId(reportAction_.casterId_);
            if (caster == null)
            {
                //DealAfterActionState();
                return;
            }
            SkillData data = null;
            data = SkillData.GetData((int)reportAction_.skill_, (int)reportAction_.skillLevel_);
            /// extra skill
            if (data == null)
            {
                if (reportAction_.status_ == OrderStatus.OS_Weapon)
                {
                    COM_Item inst = new COM_Item();
                    inst.itemId_ = (uint)reportAction_.itemId_;
                    inst.instId_ = (uint)reportAction_.weaponInstId_;
                    if (inst.itemId_ != 0)
                    {
                        Battle.Instance.GetActorByInstId(reportAction_.casterId_).wearEquip(inst);
                    }
                    else
                    {
                        Battle.Instance.GetActorByInstId(reportAction_.casterId_).demontWeapon();
                    }
                }
                DealAfterActionState();
                return;
            }
            else
            {
				if(data._SkillType == SkillType.SKT_DefaultSecPassive || data._SkillType == SkillType.SKT_Passive)
                {
                    DealAfterActionState();
                    return;
                }
                switch (data._Passive_type)
                {
                    case PassiveType.PAT_Runaway:
                        RunAway ra = new RunAway();
                        ra.Run(caster, Battle.Instance.GetActorByInstId(reportAction_.babyId_), reportAction_.status_.Equals(OrderStatus.OS_RunawayOk), (ParamData pdata) =>
                        {
                            DealAfterActionState();
                        });
                        return;
                    case PassiveType.PAT_Change:
                        ChangePosition cp = new ChangePosition();
                        cp.ChangePos((int)reportAction_.bp0_, (int)reportAction_.bp1_, (ParamData pdata) =>
                        {
                            DealAfterActionState();
                        });
                        return;
                    case PassiveType.PAT_BabyInnout:
                        BabyInnOut bio = new BabyInnOut();
                        bio.Excute(caster, reportAction_.status_, reportAction_.babyId_, reportAction_.baby_, DealAfterActionState);
                        ClearState();
                        return;
                    case PassiveType.PAT_SecKill:
                        Seckill sk = new Seckill();
                        SkillData sdata = SkillData.GetData((int)reportAction_.skill_, (int)reportAction_.skillLevel_);
                        if (sdata._Cast_effectID != -1)
                        {
						EffectAPI.Play((EFFECT_ID)sdata._Cast_effectID, caster.ControlEntity.ActorObj, null, null, (int iVal) =>
                            {
                                if (reportAction_.targets_ == null || reportAction_.targets_.Length == 0)
                                {
                                    ClientLog.Instance.Log("Sec Kill has no target!");
                                    return;
                                }
                                sk.Do(caster, Battle.Instance.GetActorByIdx((int)reportAction_.targets_[0].position_), DealAfterActionState);
                            });
                        }
                        else
                        {
                            if (reportAction_.targets_ == null || reportAction_.targets_.Length == 0)
                            {
                                ClientLog.Instance.Log("Sec Kill has no target!");
                                return;
                            }
                            sk.Do(caster, Battle.Instance.GetActorByIdx((int)reportAction_.targets_[0].position_), DealAfterActionState);
                        }
                        return;
                    default:
                        if (reportAction_.status_ == OrderStatus.OS_None)
                        {
                            DealAfterActionState();
                            return;
                        }
                        else if (reportAction_.status_ == OrderStatus.OS_Zhuachong)
                        {
                            CatchBaby cb = new CatchBaby();
                            cb.Catch(reportAction_, DealAfterActionState);
                            return;
                        }
						else if(reportAction_.status_ == OrderStatus.OS_Summon)
						{
//                        List<COM_Entity> entities = new List<COM_Entity>(reportAction_.dynamicEntities_);
//                        Battle.Instance.AddNewActor(entities.ToArray());//, () =>
////							 {
////								DealAfterActionState();
////							 }, reportAction_.dynamicEntities_.Length);
//                        DealAfterActionState();
//                            return;
                            break;
						}
                        else
                            break;
                }
            }

            if (reportAction_.targets_.Length == 0)
            {
                if (reportAction_.status_ == OrderStatus.OS_Summon)
                {
                    mainSkillInst = new SkillInst();
                    mainSkillInst.Cast(reportAction_.skill_, reportAction_.skillLevel_, caster, null, reportAction_.targets_, FinishShow, DealCounterAction);
                }
                else
                {
                    DealAfterActionState();
                }
                return;
            }

            /// normal skill
            List<BattleActor> aims = new List<BattleActor>();
            for (int i = 0; i < reportAction_.targets_.Length; ++i)
            {
                BattleActor item = Battle.Instance.GetActorByIdx((int)reportAction_.targets_[i].position_);
                //				if( null == item || item.isDead ) continue;
                aims.Add(item);
            }

            //TODO for temp
            if (aims.Count == 0)
            {
                aims.Add(Battle.Instance.GetActorByInstId(reportAction_.casterId_));
            }
            ///////
            for (int i = 0; i < aims.Count; ++i)
            {
                if (aims[i] == null)
                    continue;
                aims[i].ForGuardPos = (int)reportAction_.huweiPosition_;
            }
            mainSkillInst = new SkillInst();
			//ClientLog.Instance.Log("宠物 No！  Cast: " + reportAction_.skill_ + "  caster : " + caster.InstName + " aim: " + aims[0].InstName + " len:" + aims.Count + " target: " + reportAction_.targets_[0].position_ + " len:" + reportAction_.targets_.Length);
            mainSkillInst.Cast(reportAction_.skill_, reportAction_.skillLevel_, caster, aims.ToArray(), reportAction_.targets_, FinishShow, DealCounterAction);
        });

        //// 处理需要除掉的actor
        //DealEraseActor();
    }

    public void Summon()
    {
        List<COM_BattleEntityInformation> entities = new List<COM_BattleEntityInformation>(reportAction_.dynamicEntities_);
        Battle.Instance.AddNewActor(entities.ToArray());
        DealAfterActionState();
    }

    bool EraseOnce_;
    // 处理需要除掉的actor
    void DealEraseActor()
    {
        EraseOnce_ = false;
        BattleActor actor = null;
        if (reportAction_ == null || reportAction_.eraseEntities_ == null)
        {
            ResetParam();
            Battle.Instance.SetBattleState(Battle.BattleStateType.BST_ShowTime);
            return;
        }

        bool needBack = true;
        for (int i = 0; i < reportAction_.eraseEntities_.Length; ++i)
        {
            actor = Battle.Instance.GetActorByInstId(reportAction_.eraseEntities_[i]);
            if (actor != null)
            {
                needBack = false;
                actor.ControlEntity.ActorObj.transform.Rotate(actor.ControlEntity.ActorObj.transform.up, 180f);
                actor.ControlEntity.MoveTo(actor.ControlEntity.ActorObj.transform.localPosition + actor.ControlEntity.ActorObj.transform.forward * 5f, (int val) => 
                {
                    if(reportAction_ != null)
                        Battle.Instance.DeleteBattleEntityItem(reportAction_.eraseEntities_[val]);

                    if (EraseOnce_)
                        return;

                    EraseOnce_ = true;

                    ResetParam();
                    Battle.Instance.SetBattleState(Battle.BattleStateType.BST_ShowTime);
                }, false, true, EntityActionController.MOVETOTAGETTIME, i);
            }
        }
        if (needBack)
        {
            ResetParam();
            Battle.Instance.SetBattleState(Battle.BattleStateType.BST_ShowTime);
        }
    }

    void DealAfterActionState()
    {
        // 添加行动后state列表并结算typeWork类型状态
        // 之后再finishMove();
        ExcuteState(StateInst.ExcuteType.ET_Work);
        FinishShow();
    }

    void FinishMove()
    {
        ResetParam();
        Battle.Instance.SetBattleState(Battle.BattleStateType.BST_ShowTime);
    }

	void DealCounterAction(BattleActor aim)
	{
		// reset action state
		aim.ControlEntity.ResetState();
		COM_ReportActionCounter counter = GetAimCounter (aim.BattlePos);
		if(counter == null)
		{
//			if(counterLst_.Count == 0)
//			{
				mainSkillInst.ContinueAttack();
                return;
//			}
		}

        BattleActor actor = null;
		if(counter.states_ != null)
		{
			for(int i=0; i < counter.states_.Length; ++i)
			{
				// add counter state
				actor = Battle.Instance.GetActorByInstId(counter.states_[i].ownerId_);
				actor.ControlEntity.AddState(counter.states_[i]);
			}
		}

		ExcuteState (StateInst.ExcuteType.ET_Beattack, ExcuteCounter, counter, true);
	}

	void ExcuteCounter(COM_ReportActionCounter counter)
	{
        BattleActor caster = Battle.Instance.GetActorByInstId(counter.casterId_);
        BattleActor[] aim = new BattleActor[] { Battle.Instance.GetActorByIdx((int)counter.targetPosition_) };
		SkillInst skill = new SkillInst();
        EffectAPI.Play((EFFECT_ID)GlobalValue.EFFECT_Fanji, caster.ControlEntity.ActorObj.transform.position + caster.ControlEntity.ActorObj.transform.forward, null, null, null, (EffectInst inst, ParamData pData) =>
        {
            EffectAPI.Play((EFFECT_ID)GlobalValue.EFFECT_FanjiCast, caster.ControlEntity.ActorObj, null, null, (int iVal) =>
            {
                skill.Cast(GlobalValue.GetAttackID(caster), 1, caster, aim, new COM_ReportTarget[] { counter.props_ }, null, DealCounterAction, true);
            });
        });
	}

	public void FinishShow()
	{
        BattleActor self = Battle.Instance.GetActorByInstId(PlayerID);
        if (self != null && self.isDead)
		{
			Battle.Instance.ResetActorDirection();
			self.ControlEntity.DealEntityDie();
		}
        DealEraseActor();
        //处理行动后buff
        DealAfterActionBuff();
	}

	void ResetParam()
	{
        reportAction_ = null;
        takeDmgAction = GlobalValue.TTakeDmg;
	}

	public bool CheckBeattack
	{
		get
		{
			return m_bPlayingBeattackAction;
		}
	}

    public bool CheckDie
    {
        get
        {
            BattleActor self = Battle.Instance.GetActorByInstId(PlayerID);
            if (self != null && self.isDead)
                return !m_bPlayDieAcitonFinish;
            else
                return false;
        }
    }

	private bool EffectAttr(Entity[] aims, COM_PropValue[] prop)
	{
		for(int i=0; i < aims.Length; ++i)
		{
			if(aims[i] == null)
				continue;

			// define for ui display.
			aims[i].ChangeAttributeType = prop[i].type_;
			aims[i].ChangeAttributeValue = (int)prop[i].value_;

			// change property value.
			aims[i].Properties[(int)prop[i].type_] += prop[i].value_;
            if (aims[i].Properties[(int)prop[i].type_] < 0)
                aims[i].Properties[(int)prop[i].type_] = 0;
		}
		return true;
	}
	//
//#endregion
}
