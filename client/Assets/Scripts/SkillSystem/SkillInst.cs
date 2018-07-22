using System.Collections.Generic;
using UnityEngine;

public class SkillInst {

//	static SkillInst inst = null;
//	static public SkillInst Instance
//	{
//		get
//		{
//			if(inst == null)
//				inst = new SkillInst();
//			return inst;
//		}
//	}
	
	public delegate void FinishCallBack ();
	public FinishCallBack finish_call_back_;

    public delegate void AttackFinishCallBack(BattleActor aim);
	public AttackFinishCallBack attack_finish_call_back_;

    private Queue<BattleActor> aimsQue_;
    private Queue<COM_ReportTarget> propQue_;
	private bool isMelee_;
	private bool isPhysic_;
	private BattleActor caster_;
    private BattleActor aim_;
    private BattleActor[] aims_;
    private COM_ReportTarget prop_;
    private COM_ReportTarget[] props_;
	private GameObject originObj_;
	private ParamData param_data_;
	private SkillData crt_data_;
	private Dictionary<int, EffectInst> effectInsts_;

    bool realAimPos_ = false;
	
    void OnDealDeadFinish()
    {
        ClientLog.Instance.Log("OnDealDeadFinish");
        if(caster_ != null)
           caster_.ControlEntity.FinishShow();
        GlobalInstanceFunction.Instance.OnDeadFinish -= OnDealDeadFinish;
    }

    public void Cast(int skillId, int skillLv, BattleActor caster, BattleActor[] aims, COM_ReportTarget[] propertyVals, FinishCallBack finCallback, AttackFinishCallBack attackfinishCallback = null, bool realAimPos = false)
	{
		GlobalInstanceFunction.Instance.openTimer_ = true;
		GlobalInstanceFunction.Instance.deadTimer_ = 0f;
         GlobalInstanceFunction.Instance.OnDeadFinish += OnDealDeadFinish;
		if(crt_data_ != null)
		{
			ClientLog.Instance.LogError("a skill is casting");
			return;
		}

		effectInsts_ = new Dictionary<int, EffectInst> ();

        crt_data_ = SkillData.GetData(skillId, skillLv);

        if (crt_data_ == null)
        {
            ClientLog.Instance.LogError("skill id :" + skillId + "has not data");
            return;
        }

        realAimPos_ = realAimPos;
        if(caster.InstId == GamePlayer.Instance.InstId || TeamSystem.isTeamMember(caster.InstId))
        {

			AttaclPanel.Instance.SetSkillIcon(skillId,skillLv,caster.AssetId);
            //caster.ControlEntity.PlayerInfoUI.GetComponent<Roleui>().ShowSkill(crt_data_.singEffectId_, caster.SkillNamePos());
        }

		isPhysic_ = crt_data_._IsPhysic;
        if (isPhysic_)
        {
            if (caster.battlePlayer.weaponItemId_ != 0)
            {
                isMelee_ = !caster.rangeWeapon();
            }
            else
                isMelee_ = crt_data_._IsMelee;
        }
        else
			isMelee_ = crt_data_._IsMelee;

		Transform t = Battle.Instance.GetStagePointByIndex(caster.BattlePos);
		if(t == null)
			return;
		caster_ = caster;
		aims_ = TrimNull(aims);
        props_ = propertyVals;
		originObj_ =  t.gameObject;
		finish_call_back_ = finCallback;
		attack_finish_call_back_ = attackfinishCallback;
        //caster_.ResetPos();
        Battle.Instance.ResetActorDirection();

        aimsQue_ = new Queue<BattleActor>();
        propQue_ = new Queue<COM_ReportTarget>();
        for (int i = 0; i < aims_.Length; ++i)
        {
            if (aims_[i] == null)
                continue;
            aimsQue_.Enqueue(aims_[i]);
            propQue_.Enqueue(props_[i]);
        }

 		if(isMelee_)
			caster_.attackAnim_ = caster_.GetWeaponAction() + GlobalValue.TAttack;
		else
		{
			if(isPhysic_)
				caster_.castAnim_ = caster_.GetWeaponAction() + GlobalValue.TCast;
			else
				caster_.castAnim_ = GlobalValue.TCast;
		}

        caster.battlePlayer.mpCrt_ -= crt_data_._Cost_mana;

        if(caster.InstId == GamePlayer.Instance.InstId)
        {
            AttaclPanel.Instance.ChangeValue(PropertyType.PT_MpCurr, crt_data_._Cost_mana * -1, caster.battlePlayer.hpMax_, caster.battlePlayer.mpMax_);
            ScrollViewPanel.curMp = caster.battlePlayer.mpCrt_;
        }

        if (Battle.Instance.SelfActorBattleBaby != null)
        {
            if (caster.InstId == Battle.Instance.SelfActorBattleBaby.InstId)
            {
                AttaclPanel.Instance.ChangeValueBaby(PropertyType.PT_MpCurr, crt_data_._Cost_mana * -1, caster.battlePlayer.hpMax_, caster.battlePlayer.mpMax_);
            }
        }

        if (caster.ControlEntity.PlayerInfoUI != null &&
               caster.ControlEntity.PlayerInfoUI.GetComponent<Roleui>() != null)
        {
            Roleui roleinfoUI = caster.ControlEntity.PlayerInfoUI.GetComponent<Roleui>();
			roleinfoUI.ValueChange(PropertyType.PT_MpCurr, crt_data_._Cost_mana * -1, caster.battlePlayer.hpMax_, caster.battlePlayer.mpMax_, false);
        }

        //EffectAPI.Load (() => {
        //    if (crt_data_._Cast_effectID != -1)
        //        EffectAPI.Play((EFFECT_ID)crt_data_._Cast_effectID, caster_.ControlEntity.ActorObj, null, null, AfterCastEffect);
        //    else
        //        AfterCastEffect();
        //}, crt_data_._EffectID, crt_data_._Cast_effectID, crt_data_._Cast_effectID);
        if (crt_data_._Cast_effectID != -1)
            EffectAPI.Play((EFFECT_ID)crt_data_._Cast_effectID, caster_.ControlEntity.ActorObj, null, null, AfterCastEffect);
        else
            AfterCastEffect();
	}

    int oboIdx = 0;
    bool playCast = false;
    void AfterCastEffect(int iVal = 0)
    {
        //如果是乱射 则设置完成回调代理
		EffectAssetsData ead = EffectAssetsData.GetData(crt_data_._EffectID);
        EffectBehaviourData ebd = null;
        if(ead != null)
            ebd = EffectBehaviourData.GetData(ead.behaviour_id_);
        //注意！！！ 只有乱射才是onebyone
        if (ebd != null && !isMelee_ && ebd.cast_type_ == EffectBehaviourData.CASTTYPE.OneByOne)
        {
            //设置远程类技能的触发点回调
            caster_.ControlEntity.SetSingBrustCallBack(Brust);
            //设置远程类技能的完成回调
            caster_.ControlEntity.SetSingCallBack(PlayBrustAnim);
        }
        //else
        //{
        //    caster_.ControlEntity.SetSingCallBack(Brust);
        //}
        PlayBrustAnim();
    }

    void PlayBrustAnim()
    {
        //如果是乱射
		EffectAssetsData ead = EffectAssetsData.GetData(crt_data_._EffectID);
        EffectBehaviourData ebd = null;
        if (ead != null)
            ebd = EffectBehaviourData.GetData(ead.behaviour_id_);
        bool needTimerCallback = false;
        if (ebd != null && !isMelee_ && ebd.cast_type_ == EffectBehaviourData.CASTTYPE.OneByOne)
        {
            //没有目标了
            if (aimsQue_.Count == 0)
            {
                return;
            }
        }
        else
        {
            needTimerCallback = true;
            //Brust();
        }
        if (!isMelee_)
        {
            caster_.ControlEntity.SetAnimationParam(caster_.castAnim_, AnimatorParamType.APT_Trigger);
            if(needTimerCallback)
                GlobalInstanceFunction.Instance.Invoke(() => { Brust(); }, 0.5f);
        }
        else
            Brust();
    }

    void Brust()
    {
        // show cast effect
		EffectAssetsData ead = EffectAssetsData.GetData(crt_data_._EffectID);
        EffectBehaviourData ebd = null;
        if (ead != null)
            ebd = EffectBehaviourData.GetData(ead.behaviour_id_);
        if (ebd != null && !isMelee_ && ebd.cast_type_ == EffectBehaviourData.CASTTYPE.OneByOne)
        {
            if (aimsQue_.Count > 0)
            {
                bool final = aimsQue_.Count == 1;
                BattleActor actor = aimsQue_.Dequeue();
				actor.ControlEntity.DealLagState();
                OneByOneExcute(actor, final);
                
                //if (!final)
                //    GlobalInstanceFunction.Instance.Invoke(Brust, 0.2f);
                //if (!playCast)
                //{
                    
                    //caster_.ControlEntity.StartRecord();
                //    playCast = true;
                //}
                //else
                //{
                //    caster_.ControlEntity.PlayBack();
                //}
            }
        }
        else
        {
            //if (!isMelee_)
            //{
            //    if (!playCast)
            //    {
            //        caster_.ControlEntity.SetAnimationParam(caster_.castAnim_, AnimatorParamType.APT_Trigger);
            //        caster_.ControlEntity.StartRecord();
            //        playCast = true;
            //    }
            //    else
            //    {
            //        caster_.ControlEntity.PlayBack();
            //    }

            //}
            CastEffectAfter();
        }
    }

    void OneByOneExcute(BattleActor aim, bool final = false)
    {
        Vector3 casterPos = caster_.ControlEntity.ActorObj.transform.position;
        // show skill effect
        if (final)
        {
			EffectAPI.Play((EFFECT_ID)crt_data_._EffectID, casterPos, ConvertV3(new BattleActor[] { aim }), GetPack(HitMotion_1, HitMotion_2, ShowHitEffect, PopChangeValue), ActionOver, SaveEffectInst, new ParamData(oboIdx++));
            caster_.ControlEntity.ClearSingBrustCallBack();
            caster_.ControlEntity.ClearSingCallBack();
        }
        else
			EffectAPI.Play((EFFECT_ID)crt_data_._EffectID, casterPos, ConvertV3(new BattleActor[] { aim }), GetPack(HitMotion_1, HitMotion_2, ShowHitEffect, PopChangeValue), null, SaveEffectInst, new ParamData(oboIdx++));
    }
	
	void CastEffectAfter(int iVal = 0)
	{
        if (caster_.ControlEntity.reportAction_ != null && caster_.ControlEntity.reportAction_.status_ == OrderStatus.OS_Summon)
        {
            caster_.ControlEntity.Summon();
            return;
        }
		if(isMelee_)
		{
			OneMotion();
		}
		else
		{
			Vector3 casterPos = caster_.ControlEntity.ActorObj.transform.position;
			// show skill effect
			EffectAPI.Play((EFFECT_ID)crt_data_._EffectID, casterPos, ConvertV3(aims_), GetPack(HitMotion_1, HitMotion_2, ShowHitEffect, PopChangeValue), ActionOver, SaveEffectInst, null, ConvertTrans(aims_));
		}
	}

	void SaveEffectInst(EffectInst inst, ParamData data)
	{
        if (data == null)
            return;

		if(effectInsts_.ContainsKey(data.iParam))
		{
			effectInsts_[data.iParam].DestorySelf();
			effectInsts_[data.iParam] = inst;
		}
		else
			effectInsts_.Add (data.iParam, inst);

        //if (inst.needRotate())
        //    inst.transform.Rotate(Vector3.up, 180f);
	}
	
	EffectAPI.TakeDmgCallBackPack GetPack(EffectInst.ReachCallBack_1 hit_1, EffectInst.ReachCallBack_2 hit_2, EffectInst.ReachCallBack_Eff eff, EffectInst.ReachCallBack_Pop changeVal)
	{
		EffectAPI.TakeDmgCallBackPack tdPack = new EffectAPI.TakeDmgCallBackPack ();
		tdPack.hit_1 = hit_1;
		tdPack.hit_2 = hit_2;
		tdPack.effect = eff;
		tdPack.changeVal = changeVal;
		return tdPack;
	}

    Vector3[] ConvertV3(BattleActor[] arr)
	{
        if (arr == null)
            return null;

		List<Vector3> tl = new List<Vector3> ();
		for(int i = 0; i < arr.Length; ++i)
		{
            if (arr[i] != null)
                tl.Add(arr[i].ControlEntity.ActorObj.transform.position);
            }
		return tl.ToArray ();
	}

    Transform[] ConvertTrans(BattleActor[] arr)
    {
        List<Transform> tl = new List<Transform>();
        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i] != null)
                tl.Add(arr[i].ControlEntity.ActorObj.transform);
        }
        return tl.ToArray();
    }

    BattleActor[] TrimNull(BattleActor[] arr)
	{
        if(arr == null)
            return new BattleActor[0];
        List<BattleActor> tl = new List<BattleActor>();
		for(int i = 0; i < arr.Length; ++i)
		{
			if(arr[i] != null)
				tl.Add(arr[i]);
		}
		return tl.ToArray ();
	}
	
	void OneMotion()
	{
		if(aim_ != null)
		{
            if (aim_.ControlEntity.m_bPlayingBeattackAction)
            {
                GlobalInstanceFunction.Instance.Invoke(OneMotion, 2);
                return;
            }
            caster_.ControlEntity.hasActionCallBackToCall = false;
			if(attack_finish_call_back_ != null)
			{
                //aim_.ControlEntity.UpdateStateTick();
				attack_finish_call_back_(aim_);
			}
            //if(aimsQue_.Count <= 0)
            //{
            //    MoveToOriginPos();
            //}
		}
		else
		{
            caster_.ControlEntity.hasActionCallBackToCall = false;
			ContinueAttack();
		}
	}

	public void ContinueAttack()
	{
		if(aimsQue_.Count <= 0)
		{
			MoveToOriginPos();
			return;
		}
		if(aim_ != null && !aim_.isDead)
			aim_.BackToOrigin();
		aim_ = aimsQue_.Dequeue ();
		prop_ = propQue_.Dequeue ();
        aim_.ControlEntity.DealLagState();
		MoveToOtherAim ();
	}
	
	void MoveToOtherAim()
	{
        if (caster_.ControlEntity == null)
		{
			ApplicationEntry.Instance.PostSocketErr(57557);
            return;
		}

		Transform crossTrans = null;
		int aimForGuard = aim_.ForGuardPos;
		if(aimForGuard != (int)BattlePosition.BP_None)
			crossTrans = Battle.Instance.GetStagePointByIndex(aimForGuard);
        if (crossTrans != null)
        {
            BattleActor actor = Battle.Instance.GetActorByIdx((int)aimForGuard);
            if (actor.ControlEntity == null)
            {
                ApplicationEntry.Instance.PostSocketErr(57557);
                return;
            }

            actor.battlePlayer.mpCrt_ -= crt_data_._Cost_mana;
            if (actor.InstId == GamePlayer.Instance.InstId)
            {
                AttaclPanel.Instance.ChangeValue(PropertyType.PT_MpCurr, crt_data_._Cost_mana * -1, actor.battlePlayer.hpMax_, actor.battlePlayer.mpMax_);
                ScrollViewPanel.curMp = actor.battlePlayer.mpCrt_;
            }

            if (Battle.Instance.SelfActorBattleBaby != null)
            {
                if (actor.InstId == Battle.Instance.SelfActorBattleBaby.InstId)
                {
                    AttaclPanel.Instance.ChangeValueBaby(PropertyType.PT_MpCurr, crt_data_._Cost_mana * -1, actor.battlePlayer.hpMax_, actor.battlePlayer.mpMax_);
                }
            }

            if (actor.ControlEntity.PlayerInfoUI != null &&
                   actor.ControlEntity.PlayerInfoUI.GetComponent<Roleui>() != null)
            {
                Roleui roleinfoUI = actor.ControlEntity.PlayerInfoUI.GetComponent<Roleui>();
                roleinfoUI.ValueChange(PropertyType.PT_MpCurr, crt_data_._Cost_mana * -1, actor.battlePlayer.hpMax_, actor.battlePlayer.mpMax_, false);
            }

            Vector3 crossPos = (crossTrans.position + crossTrans.forward);
            caster_.ControlEntity.MoveTo(crossPos, AttackMotion);
			EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_huwei, Vector3.zero, aim_.ControlEntity.ActorObj.transform, null, true);
            aim_.ControlEntity.MoveTo(crossPos, (int data) =>
            {
                if (aim_.ControlEntity == null)
                {
                    ApplicationEntry.Instance.PostSocketErr(57557);
                    return;
                }
                //				aim_.ControlEntity.PlayEntityAction( GlobalValue.ActionName , GlobalValue.Action_Idle );
                aim_.ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
            }, false, true, 0.3f);
        }
        else
        {
            if (realAimPos_)
                caster_.ControlEntity.MoveTo(aim_.ControlEntity.ActorObj, AttackMotion);
            else
                caster_.ControlEntity.MoveTo(Battle.Instance.GetStagePointByIndex(aim_.BattlePos).gameObject, AttackMotion);
        }
	}

	public void MoveToOriginPos()
	{
		float moveTime = EntityActionController.MOVETOTAGETTIME;
		if(caster_.isDead)
			moveTime = EntityActionController.DEAD_MOVETOTAGETTIME;
		caster_.ControlEntity.MoveTo (originObj_, ActionOver , false, true, moveTime);
		//如果目标没被打飞
        if(aim_ != null)
            aim_.BackToOrigin ();
	}

	void ActionOver(int data)
	{
        caster_.ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
		caster_.ControlEntity.ResetState ();
        if (finish_call_back_ != null)
        {
            finish_call_back_();
            for (int i = 0; i < aims_.Length; ++i)
            {
                aims_[i].ControlEntity.ExcuteState(EntityActionController.StateInst.ExcuteType.ET_Work);
            }
        }
        else
        {
            //最终回调没掉过就没了？？ 草 手动调！！
            ClientLog.Instance.LogError("WTF!!!!");
            caster_.ControlEntity.FinishShow();
        }
		finish_call_back_ = null;
        GlobalInstanceFunction.Instance.OnDeadFinish -= OnDealDeadFinish;
        GlobalInstanceFunction.Instance.openTimer_ = false;
		//crt_data_ = null;
	}
	
	void AttackMotion(int ival)
	{
        ExcuteAttack();
	}
	
	void ExcuteAttack()
	{
        caster_.ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
        //if (caster_.ControlEntity.hasActionCallBackToCall)
        //    GlobalInstanceFunction.Instance.Invoke(ExcuteAttack, 2);
        //else
		    caster_.ControlEntity.Attack (OneMotion, HitMotion_1, HitMotion_2, ShowHitEffect, PopChangeValue);
	}

    bool hasWuxiaoBuff(BattleActor aim)
    {
        StateData sd = null;
        for (int i = 0; i < aim.ControlEntity.stateList_.Count; ++i)
        {
            sd = StateData.GetData(aim.ControlEntity.stateList_[i].stateId_);
            if ((StateType)sd._StateType == StateType.ST_ActionInvalid && isPhysic_)
                return true;

			if ((StateType)sd._StateType == StateType.ST_MagicInvalid && !isPhysic_)
                return true;
        }
        return false;
    }
	
    
	void HitMotion_1(int idx)
	{
		if(idx == -1)
			return;

		if(isMelee_)
		{
			if(prop_.prop2_.Length != 0)
			{
                COM_ReportTargetBase prop = prop_.prop2_[0];
                int chageVal = 0;
                switch(prop.prop_.type_)
                {
                    case PropertyType.PT_HpCurr:
                        chageVal = (int)prop.prop_.value_ - aim_.battlePlayer.hpCrt_;
                        break;
                    case PropertyType.PT_MpCurr:
                        chageVal = (int)prop.prop_.value_ - aim_.battlePlayer.mpCrt_;
                        break;
                }
                aim_.ControlEntity.Beattack_1(crt_data_._Id, crt_data_._Level, hasWuxiaoBuff(aim_), false, false, caster_);
                //反弹伤害不播受击
				caster_.ControlEntity.Beattack_1(crt_data_._Id, crt_data_._Level, false, false, chageVal == 0, null, false);
                //caster_.ControlEntity.UpdateStateTick();
			}
			else
			{
                COM_ReportTarget prop = prop_;
                int chageVal = 0;
                switch (prop.prop_.type_)
                {
                    case PropertyType.PT_HpCurr:
                        chageVal = (int)prop.prop_.value_ - aim_.battlePlayer.hpCrt_;
                        break;
                    case PropertyType.PT_MpCurr:
                        chageVal = (int)prop.prop_.value_ - aim_.battlePlayer.mpCrt_;
                        break;
                }
				aim_.ControlEntity.LookAt (caster_.ControlEntity.ActorObj);
                bool hasWuxiao = hasWuxiaoBuff(aim_);
                if (hasWuxiao == false)
                    hasWuxiao = (chageVal > 0);
                if (prop.prop_.type_ == PropertyType.PT_HpCurr)
					aim_.ControlEntity.Beattack_1(crt_data_._Id, crt_data_._Level, !hasWuxiao, (aim_.battlePlayer.hpCrt_ + chageVal) <= 0, chageVal == 0, caster_);
                else if(prop.prop_.type_ == PropertyType.PT_MpCurr)
					aim_.ControlEntity.Beattack_1(crt_data_._Id, crt_data_._Level, !hasWuxiao, false, false, caster_);
                //aim_.ControlEntity.UpdateStateTick();
			}
		}
		else
		{
			if(props_[idx].prop2_.Length != 0)
			{
				if(effectInsts_.ContainsKey(idx))
					effectInsts_[idx].DestorySelf();
				aims_[idx].ControlEntity.Beattack_1(crt_data_._Id, crt_data_._Level, hasWuxiaoBuff(aims_[idx]), false, false, caster_);
				Vector3 casterPos = aims_[idx].ControlEntity.ActorObj.transform.position;
				Vector3 aim = caster_.ControlEntity.ActorObj.transform.position;
				EffectAPI.Play((EFFECT_ID)crt_data_._EffectID, casterPos, new Vector3[]{aim}, GetPack(RangeAntiHit, null, null, RangeAntiPop), ActionOver, SaveEffectInst, new ParamData(idx));
            }
            else
            {
                COM_PropValue prop = props_[idx].prop_;
                int chageVal = 0;
                switch (prop.type_)
                {
                    case PropertyType.PT_HpCurr:
                        chageVal = (int)prop.value_ - aims_[idx].battlePlayer.hpCrt_;
                        break;
                    case PropertyType.PT_MpCurr:
                        chageVal = (int)prop.value_ - aims_[idx].battlePlayer.mpCrt_;
                        break;
                }
                aims_[idx].ControlEntity.LookAt (caster_.ControlEntity.ActorObj);
                bool hasWuxiao = hasWuxiaoBuff(aims_[idx]);
                if (hasWuxiao == false)
                    hasWuxiao = (chageVal > 0);
                aims_[idx].ControlEntity.Beattack_1(crt_data_._Id, crt_data_._Level, !hasWuxiao, aims_[idx].battlePlayer.hpCrt_ + chageVal <= 0f, chageVal == 0, caster_);
                //aims_[idx].ControlEntity.UpdateStateTick();
            }
        }
	}
	
	void HitMotion_2(int idx)
	{
		if(idx == -1)
			return;
	}

	void ShowHitEffect(int idx)
	{
		if(idx == -1)
			return;
		
		BattleActor aim = isMelee_? aim_: aims_[idx];
        if (aim == null)
            return;

		bool playSkillEffect = aim.ControlEntity.PlaySkillBeattackEffect (crt_data_._Id);
        if (playSkillEffect)
            EffectAPI.Play((EFFECT_ID)crt_data_._Beattack_effectID, aim.ControlEntity.ActorObj, null, null, null, (EffectInst ei, ParamData data) =>
            {
                if (aim == null || aim.ControlEntity == null || aim.ControlEntity.ActorObj == null)
                    return;
                ei.transform.parent = aim.ControlEntity.ActorObj.transform;
                ei.transform.localScale = Vector3.one;
            });
        else
            aim.ControlEntity.UpdateStateTick(crt_data_._IsPhysic);
	}
	
	void PopChangeValue(int idx)
	{
		if(idx == -1)
			return;

		if(props_ == null || props_.Length == 0)
			return;

		if(isMelee_)
		{
			if(prop_.prop2_.Length != 0)
			{
				COM_ReportTargetBase prop = prop_.prop2_[0];
                int chageVal = 0;
                switch (prop.prop_.type_)
                {
                    case PropertyType.PT_HpCurr:
                        chageVal = (int)prop.prop_.value_ - caster_.battlePlayer.hpCrt_;
                        caster_.battlePlayer.hpCrt_ = (int)prop.prop_.value_;
                        if (caster_.battlePlayer.hpCrt_ < 0)
                            caster_.battlePlayer.hpCrt_ = 0;
                        break;
                    case PropertyType.PT_MpCurr:
                        chageVal = (int)prop.prop_.value_ - caster_.battlePlayer.mpCrt_;
                        caster_.battlePlayer.mpCrt_ = (int)prop.prop_.value_;
                        if (caster_.battlePlayer.mpCrt_ < 0)
                            caster_.battlePlayer.mpCrt_ = 0;
                        break;
                }
                caster_.ChangeAttributeType = prop.prop_.type_;
				caster_.ChangeAttributeValue = chageVal;
				if(caster_.battlePlayer.hpCrt_ > caster_.battlePlayer.hpMax_)
                    caster_.battlePlayer.hpCrt_ = caster_.battlePlayer.hpMax_;
                if (caster_.battlePlayer.mpCrt_ > caster_.battlePlayer.mpMax_)
					caster_.battlePlayer.mpCrt_ = caster_.battlePlayer.mpMax_;
                caster_.ControlEntity.hitOver_ = prop.fly_;
                caster_.ControlEntity.PopValueChange(caster_.ChangeAttributeValue, caster_.ChangeAttributeType, caster_.battlePlayer.hpMax_, caster_.battlePlayer.mpMax_, null, prop.bao_);
			}
			else
			{
				COM_ReportTarget prop = prop_;
                int chageVal = 0;
                switch (prop.prop_.type_)
                {
                    case PropertyType.PT_HpCurr:
                        chageVal = (int)prop.prop_.value_ - aim_.battlePlayer.hpCrt_;
                        aim_.battlePlayer.hpCrt_ = (int)prop.prop_.value_;
                        if (aim_.battlePlayer.hpCrt_ < 0)
                            aim_.battlePlayer.hpCrt_ = 0;
                        break;
                    case PropertyType.PT_MpCurr:
                        chageVal = (int)prop.prop_.value_ - aim_.battlePlayer.mpCrt_;
                        aim_.battlePlayer.mpCrt_ = (int)prop.prop_.value_;
                        if (aim_.battlePlayer.mpCrt_ < 0)
                            aim_.battlePlayer.mpCrt_ = 0;
                        break;
                }
                aim_.ChangeAttributeType = prop.prop_.type_;
				aim_.ChangeAttributeValue = chageVal;

                if (aim_.battlePlayer.hpCrt_ > aim_.battlePlayer.hpMax_)
                    aim_.battlePlayer.hpCrt_ = aim_.battlePlayer.hpMax_;
                if (aim_.battlePlayer.mpCrt_ > aim_.battlePlayer.mpMax_)
                    aim_.battlePlayer.mpCrt_ = aim_.battlePlayer.mpMax_;
                aim_.ControlEntity.hitOver_ = prop.fly_;
                aim_.ControlEntity.PopValueChange(aim_.ChangeAttributeValue, aim_.ChangeAttributeType, aim_.battlePlayer.hpMax_, aim_.battlePlayer.mpMax_, null, prop.bao_);
			}
            if (aim_.isDead)
            {
                GlobalInstanceFunction.Instance.setTimeScale(Battle.Instance.reportPlaySpeed_ * 0.7f);
            }
	    }
	    else
	    {
            bool deadBefore = aims_[idx].isDead;
			COM_ReportTarget prop = props_[idx];
            int chageVal = 0;
            switch (prop.prop_.type_)
            {
                case PropertyType.PT_HpCurr:
                    chageVal = (int)prop.prop_.value_ - aims_[idx].battlePlayer.hpCrt_;
                    aims_[idx].battlePlayer.hpCrt_ = (int)prop.prop_.value_;
                    if (aims_[idx].battlePlayer.hpCrt_ < 0)
                        aims_[idx].battlePlayer.hpCrt_ = 0;
                    break;
                case PropertyType.PT_MpCurr:
                    chageVal = (int)prop.prop_.value_ - aims_[idx].battlePlayer.mpCrt_;
                    aims_[idx].battlePlayer.mpCrt_ = (int)prop.prop_.value_;
                    if (aims_[idx].battlePlayer.mpCrt_ < 0)
                        aims_[idx].battlePlayer.mpCrt_ = 0;
                    break;
            }
            aims_[idx].ChangeAttributeType = prop.prop_.type_;
			aims_[idx].ChangeAttributeValue = chageVal;
            aims_[idx].ControlEntity.PopValueChange(aims_[idx].ChangeAttributeValue, aims_[idx].ChangeAttributeType, aims_[idx].battlePlayer.hpMax_, aims_[idx].battlePlayer.mpMax_, null, prop.bao_);
            aims_[idx].ControlEntity.hitOver_ = prop.fly_;
            if (aims_[idx].battlePlayer.hpCrt_ > aims_[idx].battlePlayer.hpMax_)
                aims_[idx].battlePlayer.hpCrt_ = aims_[idx].battlePlayer.hpMax_;
            if (aims_[idx].battlePlayer.mpCrt_ > aims_[idx].battlePlayer.mpMax_)
                aims_[idx].battlePlayer.mpCrt_ = aims_[idx].battlePlayer.mpMax_;
            if (aims_[idx].isDead && deadBefore == false)
            {
                GlobalInstanceFunction.Instance.setTimeScale(Battle.Instance.reportPlaySpeed_ * 0.7f);
            }
        }

        
	}

	void RangeAntiPop(int idx)
	{
		COM_ReportTargetBase prop = props_[idx].prop2_[0];
        int chageVal = 0;
        switch (prop.prop_.type_)
        {
            case PropertyType.PT_HpCurr:
                chageVal = (int)prop.prop_.value_ - caster_.battlePlayer.hpCrt_;
                caster_.battlePlayer.hpCrt_ = (int)prop.prop_.value_;
                if (caster_.battlePlayer.hpCrt_ < 0)
                    caster_.battlePlayer.hpCrt_ = 0;
                break;
            case PropertyType.PT_MpCurr:
                chageVal = (int)prop.prop_.value_ - caster_.battlePlayer.mpCrt_;
                caster_.battlePlayer.mpCrt_ = (int)prop.prop_.value_;
                if (caster_.battlePlayer.mpCrt_ < 0)
                    caster_.battlePlayer.mpCrt_ = 0;
                break;
        }
        caster_.ChangeAttributeType = prop.prop_.type_;
		caster_.ChangeAttributeValue = chageVal;
        caster_.ControlEntity.hitOver_ = prop.fly_;
        caster_.ControlEntity.PopValueChange(caster_.ChangeAttributeValue, caster_.ChangeAttributeType, caster_.battlePlayer.hpMax_, caster_.battlePlayer.mpMax_, null, prop.bao_);
    }
    
    void RangeAntiHit(int idx)
	{
        //魔法反弹 不播放受击 注意卡死？？？
        aims_[idx].ControlEntity.Beattack_1(crt_data_._Id, crt_data_._Level, false);
        //caster_.ControlEntity.UpdateStateTick();
    }
}
