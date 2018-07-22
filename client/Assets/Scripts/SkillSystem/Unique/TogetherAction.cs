using UnityEngine;
using System.Collections;

/*
 * special skill realize --- Guard
 */

public class TogetherAction {

	public delegate void TogetherCallBack();
	TogetherCallBack togetherFinCallBack_;

    int resetBackCount_;

	int memberCount_;

	int finishCount_;

    BattleActor[] members_;

    COM_ReportTarget[] props_;

    BattleActor aim_;

	int CurrentHp_;

    int dealStateCount_;

    public void Action(BattleActor[] member, BattlePosition aim, COM_ReportTarget[] props, TogetherCallBack finishCallBack, COM_ReportAction[] acitons)
	{
        GlobalInstanceFunction.Instance.OnDeadFinish += OnDealDeadFinish;
        resetBackCount_ = 0;
        dealStateCount_ = 0;
        simpleHitCount = 0;
        includeChange = 0;
        AttaclPanel.Instance.togetherHitCrl_.Open();
		togetherFinCallBack_ = finishCallBack;
		members_ = member;
		props_ = props;
		aim_ = Battle.Instance.GetActorByIdx ((int)aim);
		CurrentHp_ = aim_.battlePlayer.hpCrt_;
        ResetBack();
	}

    void OnDealDeadFinish()
    {
        GlobalInstanceFunction.Instance.openTimer_ = false;
        GlobalInstanceFunction.Instance.OnDeadFinish -= OnDealDeadFinish;
        memberCount_ = 0;
        finishCount_ = members_.Length - 1;
        MoveBack();
    }

    void ResetBack()
    {
        for (int i = 0; i < members_.Length; ++i)
        {
            members_[i].BackToOrigin(ResetBackCallBack);
        }
    }

    void ResetBackCallBack()
    {
        resetBackCount_++;
        if (resetBackCount_ >= members_.Length)
        {
            for (int i = 0; i < members_.Length; ++i)
            {
                members_[i].ControlEntity.ExcuteState(EntityActionController.StateInst.ExcuteType.ET_Action, DealStateCallBack);
            }
        }
    }

    void DealStateCallBack(COM_ReportActionCounter counter)
    {
        dealStateCount_++;
        if (dealStateCount_ == members_.Length)
        {
            for (int i = 0; i < members_.Length; ++i)
            {
                members_[i].ControlEntity.MoveTo(aim_.ControlEntity.ActorObj, ReadyAttack);
            }
        }
    }

	void ReadyAttack(int iVal)
	{
		memberCount_ ++;
		if(memberCount_ == members_.Length)
		{
			memberCount_ = 0;
			for(int i=0; i < members_.Length; ++i)
			{
                members_[i].ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
				members_[i].attackAnim_ = members_[i].GetWeaponAction() + GlobalValue.TAttack;
			}
			Attack(memberCount_);
		}
	}

	void Attack(int idx)
	{
        if (members_[idx].ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
        GlobalInstanceFunction.Instance.openTimer_ = true;
        GlobalInstanceFunction.Instance.deadTimer_ = 0f;
		if(idx != members_.Length - 1)
            members_[idx].ControlEntity.Attack(MoveBack, HitHim);
		else
            members_[idx].ControlEntity.Attack(MoveBack, HitHim, null, null, ShowValueChange);
    }

	void MoveBack()
	{
        if (members_ == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }

        if (memberCount_ >= members_.Length)
        {
            OnDealDeadFinish();
            return;
        }

        if (members_[memberCount_].ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
		members_[memberCount_].ControlEntity.MoveTo(Battle.Instance.GetStagePointByIndex(members_[memberCount_].BattlePos).gameObject, FinishOne, true, true, 0.6f, memberCount_);
		memberCount_ ++;
		if(memberCount_ < members_.Length)
			Attack (memberCount_);
	}
    
    void FinishOne(int idx)
	{
        if (members_[idx].ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
		finishCount_ ++;
        members_[idx].ControlEntity.SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
		Battle.Instance.ResetActor (members_[idx].InstId);
		if(finishCount_ == members_.Length)
		{
			if(togetherFinCallBack_ != null)
				togetherFinCallBack_();
            AttaclPanel.Instance.togetherHitCrl_.Close();
            GlobalInstanceFunction.Instance.OnDeadFinish -= OnDealDeadFinish;
		}
	}

	int skipIdx = -1;

    int simpleHitCount = 0;

    int includeChange = 0;
	void HitHim(int idx)
	{
        if (members_[idx].ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
        simpleHitCount ++;
        AttaclPanel.Instance.togetherHitCrl_.SetNum(simpleHitCount);
        SkillData skill = SkillData.GetMinxiLevelData(GlobalValue.GetAttackID(members_[idx].GetWeaponType()));
        bool playSkillEffect = aim_.ControlEntity.PlaySkillBeattackEffect(skill._Id);
		if(playSkillEffect)
            EffectAPI.Play((EFFECT_ID)skill._Beattack_effectID, aim_.ControlEntity.ActorObj);
        else
        {
            int effId = aim_.ControlEntity.GetStateBeattackEffectID(skill._Id);
            EffectAPI.Play((EFFECT_ID)effId, aim_.ControlEntity.ActorObj);
        }

		int change = (int)props_[memberCount_].prop_.value_ - CurrentHp_;
		CurrentHp_ += change;
        aim_.ControlEntity.hitOver_ = props_[memberCount_].fly_;

        int[] typs = aim_.ControlEntity.StateTypes;
        int rtst = 0;
        string err = "";
        GameScript.Call(ScriptGameEvent.SGE_TogetherState, new object[] { typs }, new object[] { rtst }, ref err);

        TogetherStateType tst = (TogetherStateType)rtst;//GameScript.TogetherStateEvent (ScriptGameEvent.SGE_TogetherState, aim_.ControlEntity.StateTypes);
		if(tst == TogetherStateType.TST_Self)
		{
            skipIdx = memberCount_;
            aim_.ControlEntity.PopValueChange(change, PropertyType.PT_HpCurr, aim_.battlePlayer.hpMax_, aim_.battlePlayer.mpMax_);
            includeChange += change;
		}
        else if (tst == TogetherStateType.TST_Enemy)
        {
            members_[memberCount_].ControlEntity.PopValueChange(change, PropertyType.PT_HpCurr, aim_.battlePlayer.hpMax_, aim_.battlePlayer.mpMax_);
        }

        bool hasAInvalid = hasActionInvalid();
        if (idx != members_.Length - 1)
            aim_.ControlEntity.Beattack_1(skill._Id, skill._Level, !hasAInvalid, false, false, members_[memberCount_]);
        else
			aim_.ControlEntity.Beattack_1(skill._Id, skill._Level, !hasAInvalid, CurrentHp_ <= 0, false, members_[memberCount_]);
        aim_.ControlEntity.LookAt(members_[memberCount_].ControlEntity.ActorObj);
        aim_.ControlEntity.UpdateStateTick(true);
	}

    bool hasActionInvalid()
    {
        StateData sd = null;
        for (int i = 0; i < aim_.ControlEntity.stateList_.Count; ++i)
        {
            sd = StateData.GetData(aim_.ControlEntity.stateList_[i].stateId_);
            if ((StateType)sd._StateType == StateType.ST_ActionInvalid)
                return true;
        }
        return false;
    }

	void ShowValueChange(int idx)
	{
        //最终总血量： 当前正常减少的hp值 + 状态期间已经扣除的hp值
        int realCurrentHp = CurrentHp_ - includeChange;
        int dmg = realCurrentHp - aim_.battlePlayer.hpCrt_;

        aim_.ChangeAttributeType = PropertyType.PT_HpCurr;
        aim_.ChangeAttributeValue = dmg;
        if (CurrentHp_ > aim_.battlePlayer.hpMax_)
            CurrentHp_ = aim_.battlePlayer.hpMax_;
        if (CurrentHp_ < 0)
            CurrentHp_ = 0;

        aim_.battlePlayer.hpCrt_ = realCurrentHp;
        if (aim_.ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return;
        }
        aim_.ControlEntity.PopValueChange(aim_.ChangeAttributeValue, aim_.ChangeAttributeType, aim_.battlePlayer.hpMax_, aim_.battlePlayer.mpMax_);

        if (aim_.isDead)
        {
			GlobalInstanceFunction.Instance.setTimeScale(Battle.Instance.reportPlaySpeed_ * 0.7f);
        }
	}
}
