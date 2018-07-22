using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class EntityActionController
{
	private GameObject						m_EntityObj;
	private GameObject						m_WeaponObj;
	private Animator						m_EntityAnimator;
	public const float 						MOVETOTAGETTIME = 0.2f;
	public const float 						DEAD_MOVETOTAGETTIME = 1f;
	private	GameObject						m_PlayerInfoUI;
    public bool                            hitOver_ = false;
	public	EffectInst						m_StarEffect;
    public bool hasActionCallBackToCall = false;
    private SkinnedMeshRenderer[] renderer_;

    StateInst.ExcuteType type_;
    DealStateCallBack dealStateCallBack_;
    COM_ReportActionCounter counter_;
    int stateIndex_;
    StateData crtStateData_;
    BattleActor selfActor_;

//	int 									defaultBeattackAnim1 = GlobalValue.Action_BeAttack_1;
//	int 									defaultBeattackAnim2 = GlobalValue.Action_BeAttack_2;
	string takeDmgAction = GlobalValue.TTakeDmg;

	public delegate void DealStateCallBack (COM_ReportActionCounter counter);

	public GameObject PlayerInfoUI
	{
		set
		{
			m_PlayerInfoUI = value;
		}
		get
		{
			return m_PlayerInfoUI;
		}
	}

	public EffectInst StartEffect
	{
		set
		{
			m_StarEffect = value;
		}
		get
		{
			return m_StarEffect;
		}
	}

	void RegisterActionCallBack()
	{
		ActionCallBack	acb = ActorObj.GetComponent<ActionCallBack>();
		acb.m_BeAttackFinishCallBack = BeAttackActionFinishCallBack;
        acb.m_PlaySingBrustCallBack = SingActionBrustCallBack;
		acb.m_PlaySingAcitonCallBack = SingActionFinishCallBack;
		acb.m_PlayerDieFinishCallBack = DieActionFinishCallBack;
		acb.m_UpdateCallBack = UpdateHandler;
	}

	public float GetCurPlayActionTime()
	{
		if( null == m_EntityAnimator ) return 0f;
		return m_EntityAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}

	public void MoveTo( GameObject TargetObj , ActionCallBack.MoveFinishCallBack	CallBack = null , bool bUseOffset = true , bool bPlayAnim = true, float speed = MOVETOTAGETTIME, int iVal = 0, bool lookTarget = true)
	{
		if( null == TargetObj ) return ;
		
		MoveTo (TargetObj.transform.position, CallBack, bUseOffset, bPlayAnim, MOVETOTAGETTIME, iVal, lookTarget);
	}

    public void MoveTo(Vector3 TargetPos, ActionCallBack.MoveFinishCallBack CallBack = null, bool bUseOffset = true, bool bPlayAnim = true, float moveTime = MOVETOTAGETTIME, int iVal = 0, bool lookTarget = true)
	{
		// one position to another is 0.5f
		float distance = Vector3.Distance (TargetPos, ActorObj.transform.position);
		if(distance < GlobalValue.MoveMinGap || Mathf.Approximately(distance, GlobalValue.MoveMinGap))
		{
			if(CallBack != null)
				CallBack(iVal);
			return;
		}
		if(bPlayAnim)
			SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, distance);
//			PlayEntityAction( GlobalValue.ActionName , GlobalValue.Action_Run);
		ActionCallBack	callback = ActorObj.GetComponent<ActionCallBack>();
		callback.m_MoveFinishCallBack = CallBack;
		Hashtable args = new Hashtable();
		args.Add("easeType", iTween.EaseType.linear);
		args.Add("time",moveTime);
		args.Add("delay", 0.0f);
        if (lookTarget)
        {
            args.Add("looktarget", TargetPos);
            m_EntityObj.transform.LookAt(TargetPos);
        }
		args.Add("loopType", "none");
		//args.Add("onstart", "MoveStartCallBack");
		//args.Add("onstarttarget", m_EntityObj);
		args.Add("oncomplete", "ActionMoveFinishCallBack");
		args.Add("oncompleteparams", iVal);
		args.Add("oncompletetarget", m_EntityObj);
		args.Add("onupdate", "MoveUpdateCallBack");
		//args.Add("onupdatetarget", m_EntityObj);
		args.Add("onupdateparams", true);
		if( bUseOffset )
		{
			Vector3 shortPos = (TargetPos - m_EntityObj.transform.position).normalized;
			args.Add("position", TargetPos - shortPos * 0.5f );
		}
		else
		{
			args.Add("position", TargetPos );
		}
		
		iTween.MoveTo(m_EntityObj,args);
	}
	
	public void LookAt(GameObject target, ActionCallBack.LookAtFinishCallBack callback = null, float time = -1)
	{
		if( null == target ) return ;
        if (ActorObj == null) return;
		
		ActionCallBack	acb = ActorObj.GetComponent<ActionCallBack>();
        if(ActorObj != null && acb != null)
		    acb.m_LookAtFinishCallBack = callback;
		Hashtable args = new Hashtable();
		args.Add("time", time < 0? CalcRotateTime(ActorObj.transform, target.transform): time);
		args.Add("delay", 0.0f);
		args.Add("looktarget",target.transform.position);
		args.Add("oncomplete", "ActionLookAtFinishCallBack");
		args.Add("oncompleteparams", "end");
		args.Add("oncompletetarget", m_EntityObj);
		iTween.LookTo(ActorObj, args);
	}

	// 根据origin target 角度和旋转一圈所需时间aroundTime计算 本次旋转所需时间
	float CalcRotateTime(Transform origin, Transform target, float aroundTime = 0.5f)
	{
		float clv = Mathf.Clamp01(Vector3.Dot(origin.forward, (target.position - origin.position).normalized));
		return aroundTime * (1f - clv);
	}

	public void PopValueChange( int Value , PropertyType chgType , int hpMax, int mpMax, InvokePkg.CallBack callback = null, bool isCritical = false)
	{
		if(Value == 0)
		{
			if(callback != null)
				callback();
			return;
		}
		if( null == PlayerInfoUI ) return;
		Roleui	roleinfoUI = PlayerInfoUI.GetComponent<Roleui>();
		if( null == roleinfoUI ) return;
		roleinfoUI.ValueChange( chgType , Value , hpMax, mpMax,true,false);
        if(isCritical)
            EffectAPI.Play((EFFECT_ID)GlobalValue.EFFECT_Critical, ActorObj.transform.position + ActorObj.transform.forward, null, null, null, (EffectInst inst, ParamData pData) =>
            {
                inst.gameObject.SetActive(true);
                //Battle.BattleOver -= inst.GetComponent<EffectInst>().DestorySelf;
            });
		if( PlayerID == GamePlayer.Instance.InstId )
		{
			AttaclPanel.Instance.ChangeValue(chgType,Value,hpMax,mpMax);
		}
        if (Battle.Instance.SelfActorBattleBaby != null)
        {
            if (PlayerID == Battle.Instance.SelfActorBattleBaby.InstId)
            {
                AttaclPanel.Instance.ChangeValueBaby(chgType, Value, hpMax, mpMax);
            }
        }
		if(callback != null)
			GlobalInstanceFunction.Instance.Invoke(callback, 1f);
	}

	public void DestroyEntity()
	{
		GameObject.Destroy( m_EntityObj );
		GameObject.Destroy( m_EntityAnimator );
		GameObject.Destroy( PlayerInfoUI );
	}

    bool hitoverEffectPlayed = false;
    public void DealEntityDie(DeathMaster.DieActionType dieType = DeathMaster.DieActionType.DAT_Normal)
	{
        BattleActor self = Battle.Instance.GetActorByInstId(PlayerID);
        if (self != null)
            self.SetIprop(PropertyType.PT_HpCurr, 0);
        if (hitOver_)
        {
            dieType = Random.Range(0f, 1f) > 0.5f ? DeathMaster.DieActionType.DAT_Fly : DeathMaster.DieActionType.DAT_Knock;
            EffectAPI.Play((EFFECT_ID)GlobalValue.EFFECT_Dead, self.ControlEntity.ActorObj.transform.position, null, null, null, (EffectInst inst, ParamData pData) =>
            {
                if (hitoverEffectPlayed)
                {
                    GameObject.Destroy(inst.gameObject);
                    return;
                }
                hitoverEffectPlayed = true;
                inst.gameObject.AddComponent<Destroy>().lifetime = 0.5f;
            });

            //飞的是自己 回主城
            //if (PlayerID == GamePlayer.Instance.InstId)
            //{
            //    Battle.Instance.beFlied_ = true;
            //}
        }

        DeathMaster dm = ActorObj.GetComponent<DeathMaster>();
		GlobalInstanceFunction.Instance.setTimeScale(Battle.Instance.reportPlaySpeed_);
        if(dm == null)
            dm = ActorObj.AddComponent<DeathMaster>();
        dm.Do(dieType, self);
		AttaclPanel.Instance.SetEnityUIVisble( m_PlayerID , false );
        ClearState();
	}
	
	public void StarEffectInstCallBack( EffectInst	starInst, ParamData data )
	{
        // 之前是如果有新的死亡星星加载 则销毁当前的再赋值。现在改为直接抛弃新的 就用之前的。
        DestroyEffectAssets();
		if (GamePlayer.Instance.isInBattle == false)
		{
            GameObject.Destroy(starInst.gameObject);
			ClientLog.Instance.LogError("********############&@@@@@&&&&&&&&&&&&&&&&&&&");
		}
        StartEffect = starInst;
        StartEffect.transform.parent = ActorObj.transform;
	}
	
	public void DestroyEffectAssets()
	{
		if( null == m_StarEffect ) return ;
		m_StarEffect.DestorySelf();
		m_StarEffect = null;
	}
	
	public void Operating( bool bStatic )
	{
        if (PlayerInfoUI == null)
            return;

        Roleui uiInfo = PlayerInfoUI.GetComponent<Roleui>();
        if (uiInfo != null)
            uiInfo.SetReady(bStatic);
	}

	public void SetEntityActionTime( float speed )
	{
		if (m_EntityAnimator == null)
						return;
		m_EntityAnimator.speed = speed;
	}

#region animation

	public void Attack( ActionCallBack.AttackFinishCallBack FinishCallBack = null,
	                   ActionCallBack.PlayBeAttackOneStep hit_1 = null,
	                   ActionCallBack.PlayBeAttackTwoStep hit_2 = null,
	                   ActionCallBack.PlayBeAttackEffect eff = null, 
	                   ActionCallBack.ShowChangePlayerInfo changeVal = null)
	{
		if( null == ActorObj ) return;
//		PlayEntityAction( GlobalValue.ActionName , GlobalValue.Action_Attack );
        BattleActor self = Battle.Instance.GetActorByInstId(PlayerID);
        if (self == null)
            return;

		SetAnimationParam (self.attackAnim_, AnimatorParamType.APT_Trigger);
		ActionCallBack	callback = ActorObj.GetComponent<ActionCallBack>();
		if( null == callback ) return;
        callback.AttackFinish = FinishCallBack;
		callback.m_PlayBeAttackOneStep = hit_1;
		callback.m_PlayBeAttackTwoStep = hit_2;
		callback.m_PlayBeAttackEffect = eff;
		callback.m_ShowChangePlayerInfo = changeVal;
        hasActionCallBackToCall = true;
	}

	public void DieActionFinishCallBack()
	{
		m_bPlayDieAcitonFinish = true;
		Battle.Instance.DeleteDeadEntity(PlayerID);
	}

	public void BeAttackActionFinishCallBack()
	{
        m_EntityAnimator.StopRecording();
        AnimatorStateInfo state = m_EntityAnimator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName("Base Layer.beattack02"))
            return;

        BattleActor self = Battle.Instance.GetActorByInstId(PlayerID);
        if (self != null && self.isDead)
        {
            if (isOnOriginPos(self))
            {
                DealEntityDie();
                m_bPlayingBeattackAction = false;
            }
            else
            {
                if (hitOver_)
                {
                    DealEntityDie();
                    m_bPlayingBeattackAction = false;
                }
                else
                {
                    //move to origin position.
                    self.BackToOrigin(() =>
                    {
                        DealEntityDie();
                        m_bPlayingBeattackAction = false;
                    }, 0.7f);
                }
            }
        }
        else
            m_bPlayingBeattackAction = false;
	}

    bool isOnOriginPos(BattleActor actor)
	{
        if (actor.ControlEntity == null)
        {
            ApplicationEntry.Instance.PostSocketErr(57557);
            return true;
        }
		return Vector3.Distance (actor.ControlEntity.ActorObj.transform.position, Battle.Instance.GetStagePointByIndex (actor.BattlePos).position) <= 0.1f;
	}

    public delegate void SingCallBack();
    public delegate void SingBrustCallBack();

    SingCallBack SCallback_;
    SingBrustCallBack SBCallBack_;

    public void SetSingCallBack(SingCallBack callback)
    {
        SCallback_ = callback;
    }

    public void ClearSingCallBack()
    {
        SCallback_ = null;
    }

    public void SetSingBrustCallBack(SingBrustCallBack callback)
    {
        SBCallBack_ = callback;
    }

    public void ClearSingBrustCallBack()
    {
        SBCallBack_ = null;
    }

    void SingActionBrustCallBack()
    {
        if (SBCallBack_ != null)
            SBCallBack_();
    }

    //远程特效类的释放完成回调
	void SingActionFinishCallBack()
	{
        if (SCallback_ != null)
            SCallback_();
	}

	void Reborn()
	{
		if(StartEffect != null)
		{
            StartEffect.GetComponent<EffectInst>().DestorySelf();
		}

		SetAnimationParam (GlobalValue.BDead, AnimatorParamType.APT_Boolean, false);
		AttaclPanel.Instance.SetEnityUIVisble( PlayerID , true );
		m_bPlayingBeattackAction = false;
	}

	bool isPlayBack = false;
	public void Beattack_1(int skillId, int skillLv, bool playAnim = true, bool isDead = false, bool isDodge = false, BattleActor attacker = null, bool playEffect = true)
	{
        //SkillData skill = SkillData.GetData(skillId);
        BattleActor entity = Battle.Instance.GetActorByInstId(PlayerID);
        if (entity == null)
            return;

		if(entity.isDead)
		{
			Reborn();
		}
        else if(skillId == 1031)//崩击
        {
            int effId = GetStateBeattackEffectID(skillId);
            if (effId != -1)
            {
                // 如果是防御特效
                ExcuteState(StateInst.ExcuteType.ET_Beattack, null, null, true);

				//die
				if (isDead)
				{
					GlobalInstanceFunction.Instance.setTimeScale(Battle.Instance.reportPlaySpeed_ * 0.3f);
					m_EntityAnimator.Play("Base Layer.beattack02", -1, 0.1f);
					SetEntityActionTime(0f);
					XShake shake = ActorObj.GetComponent<XShake>();
					if (shake != null)
                        GameObject.Destroy(shake);
					shake = ActorObj.AddComponent<XShake>();
					shake.battlePos = Battle.Instance.GetActorByInstId(PlayerID).BattlePos;
					shake.originPos_ = ActorObj.transform.position;
					shake.OnBeattackActionFinish += OnBeattackActionFinish;
					shake.OnMoveBackActionFinish += OnMoveBackActionFinish;
					m_bPlayingBeattackAction = true;
				}
				else
				{
					if (playAnim)
					{
						if (takeDmgAction.Equals(GlobalValue.TTakeDmg))
						{
                            //if (!m_bPlayingBeattackAction)
                            //{
                            //    m_EntityAnimator.Play("Base Layer.beattack02", -1, 0.1f);
								SetAnimationParam(takeDmgAction, AnimatorParamType.APT_Trigger);
                            //    m_EntityAnimator.StartRecording(0);
                            //    m_bPlayingBeattackAction = true;
                            //}
                            //else
                            //{
                            //    m_EntityAnimator.StopRecording();
                            //    m_EntityAnimator.StartPlayback();
                            //    m_EntityAnimator.playbackTime = 0f;
                            //    isPlayBack = true;
                            //}
                            //SetEntityActionTime(0);
                            //GlobalInstanceFunction.Instance.Invoke(() =>
                            //{
                                XShake shake = ActorObj.GetComponent<XShake>();
                                if (shake != null)
                                    GameObject.Destroy(shake);
                                shake = ActorObj.AddComponent<XShake>();
                                shake.OnlyBack();
                                selfActor_ = Battle.Instance.GetActorByInstId(m_PlayerID);
                                shake.battlePos = selfActor_.BattlePos;
                                shake.originPos_ = ActorObj.transform.position;//Battle.Instance.GetStagePointByIndex(selfActor_.BattlePos).position;
                                shake.OnBeattackActionFinish += OnBeattackActionFinish;
                                shake.OnMoveBackActionFinish += OnMoveBackActionFinish;
                                //SetEntityActionTime(1);
                            //}, 1);
						}
						else
						{
                            //if (!m_bPlayingBeattackAction)
                            //{
								SetAnimationParam(takeDmgAction, AnimatorParamType.APT_Trigger);
                            //    m_EntityAnimator.StartRecording(0);
                            //    m_bPlayingBeattackAction = true;
                            //}
                            //else
                            //{
                            //    m_EntityAnimator.StopRecording();
                            //    m_EntityAnimator.StartPlayback();
                            //    m_EntityAnimator.playbackTime = 0f;
                            //    isPlayBack = true;
                            //}
						}
					}
				}
            }
            else
            {
                m_bPlayingBeattackAction = false;
                EffectAPI.PlaySceneEffect(EFFECT_ID.EFFECT_BengjiMiss, Vector3.zero, ActorObj.transform, null, true);
            }
        }
        else if (/*skill != null &&*/ skillId != 2391/*复活技能不调用受击动作*/)
		{
            if (playEffect)
            {
                int effId = GetStateBeattackEffectID(skillId);
                if (effId != -1)
                {
                    SkillData skilldata = null;
                    if (attacker != null)
                    {
                        skilldata = SkillData.GetData(skillId, skillLv);
                        if (skilldata != null)
                        {
                            //如果该闪避但没有闪避状态 强制闪避一下。。。
                            if (isDodge && !HasState(StateType.ST_Dodge))
                            {
                                if (crtStateData_._BeattackPkg.effectId_ != 0)
                                {
                                    EffectAPI.Play(EFFECT_ID.EFFECT_DODGE, ActorObj, null, null, null, (EffectInst ei, ParamData data) =>
                                    {
                                        ei.transform.parent = ActorObj.transform;
                                        ei.transform.localScale = Vector3.one;
                                    });
                                }
                            }
                            ExcuteState(StateInst.ExcuteType.ET_Beattack, null, null, skilldata._IsPhysic, isDodge);
                            takeDmgAction = GlobalValue.TDodge;
                        }
                    }
                }
            }

            //die
            if (isDead)
            {
				GlobalInstanceFunction.Instance.setTimeScale(Battle.Instance.reportPlaySpeed_ * 0.7f);
                m_EntityAnimator.Play("Base Layer.beattack02", -1, 0.1f);
                SetEntityActionTime(0f);
                XShake shake = ActorObj.GetComponent<XShake>();
                if (shake != null)
                    GameObject.Destroy(shake);
                shake = ActorObj.AddComponent<XShake>();
                shake.battlePos = Battle.Instance.GetActorByInstId(PlayerID).BattlePos;
                shake.originPos_ = ActorObj.transform.position;
                shake.OnBeattackActionFinish += OnBeattackActionFinish;
                shake.OnMoveBackActionFinish += OnMoveBackActionFinish;
                m_bPlayingBeattackAction = true;
            }
            else
            {
                if (playAnim)
                {
                    if (takeDmgAction.Equals(GlobalValue.TTakeDmg))
                    {
                        //if (!m_bPlayingBeattackAction)
                        //{
                            SetAnimationParam(takeDmgAction, AnimatorParamType.APT_Trigger);
                            //m_EntityAnimator.StartRecording(0);
                            //m_bPlayingBeattackAction = true;
                        //}
                        //else
                        //{
                        //    m_EntityAnimator.StopRecording();
                        //    m_EntityAnimator.StartPlayback();
                        //    m_EntityAnimator.playbackTime = 0f;
                        //    isPlayBack = true;
                        //}
                        //SetEntityActionTime(0);
                        //GlobalInstanceFunction.Instance.Invoke(() =>
                        //{
                            XShake shake = ActorObj.GetComponent<XShake>();
                            if (shake != null)
                                GameObject.Destroy(shake);
                            shake = ActorObj.AddComponent<XShake>();
                            shake.OnlyBack();
                            selfActor_ = Battle.Instance.GetActorByInstId(m_PlayerID);
                            shake.battlePos = selfActor_.BattlePos;
                            shake.originPos_ = ActorObj.transform.position;//Battle.Instance.GetStagePointByIndex(selfActor_.BattlePos).position;
                            shake.OnBeattackActionFinish += OnBeattackActionFinish;
                            shake.OnMoveBackActionFinish += OnMoveBackActionFinish;
                            //SetEntityActionTime(1);
                        //}, 1);
                    }
                    else
                    {
                        //if (!m_bPlayingBeattackAction)
                        //{
                            SetAnimationParam(takeDmgAction, AnimatorParamType.APT_Trigger);
                        //    m_EntityAnimator.StartRecording(0);
                        //    m_bPlayingBeattackAction = true;
                        //}
                        //else
                        //{
                        //    m_EntityAnimator.StopRecording();
                        //    m_EntityAnimator.StartPlayback();
                        //    m_EntityAnimator.playbackTime = 0f;
                        //    isPlayBack = true;
                        //}
                    }
                }
            }
		}
        takeDmgAction = GlobalValue.TTakeDmg;
	}

    void OnMoveBackActionFinish()
    {
        //SetAnimationParam(GlobalValue.FMove, AnimatorParamType.APT_Float, GlobalValue.MoveMinGap);
        //m_EntityAnimator.Play("Base Layer.run");
        SetEntityActionTime(1f);
    }

    void OnBeattackActionFinish()
    {
		GlobalInstanceFunction.Instance.setTimeScale(Battle.Instance.reportPlaySpeed_);
        BattleActor self = Battle.Instance.GetActorByInstId(PlayerID);
        if (self == null)
        {
            m_bPlayingBeattackAction = false;
            return;
        }
        if (self.isDead)
        {
            if (isOnOriginPos(self))
            {
                DealEntityDie();
                m_bPlayingBeattackAction = false;
            }
            else
            {
                if (hitOver_)
                {
                    DealEntityDie();
                    m_bPlayingBeattackAction = false;
                }
                else
                {
                    //move to origin position.
                    self.BackToOrigin(() =>
                    {
                        DealEntityDie();
                        m_bPlayingBeattackAction = false;
                    }, 0.7f);
                }
            }
        }
        else
            m_bPlayingBeattackAction = false;
    }
	
	public void UpdateHandler()
	{
        //AnimatorStateInfo animatorState = m_EntityAnimator.GetCurrentAnimatorStateInfo(0);
        //if(animatorState.normalizedTime >= 1f)
            
		if(isPlayBack)
		{
			float newPlaybackTime = m_EntityAnimator.playbackTime + Time.deltaTime;
			if(newPlaybackTime > m_EntityAnimator.recorderStopTime)
			{
				newPlaybackTime = 0f;
				m_EntityAnimator.StopPlayback();
				isPlayBack = false;
				return;
			}
			m_EntityAnimator.playbackTime = newPlaybackTime;
		}

        if(stateList_ != null)
        {
            for (int i = 0; i < stateList_.Count; ++i)
            {
                if (stateList_[i].workingInst_ != null)
                {
                    BoxCollider box = ActorObj.GetComponent<BoxCollider>();
                    Vector3 pos = new Vector3(ActorObj.transform.position.x, ActorObj.transform.position.y, ActorObj.transform.position.z + box.center.z + box.size.z);
                    stateList_[i].workingInst_.transform.localPosition = pos;
                }
            }
        }
	}
	
	public void Beattack_2()
	{
//		PlayEntityAction(GlobalValue.ActionName, defaultBeattackAnim2);
	}

	public void PlayEntityAction( string state , int value )
	{
		if( null == m_EntityAnimator ) return ;
		m_EntityAnimator.SetInteger( state , value );
	}

    public void StartRecord()
    {
        m_EntityAnimator.StartRecording(0);
    }

    public void PlayBack()
    {
        m_EntityAnimator.StopRecording();
        m_EntityAnimator.StartPlayback();
        m_EntityAnimator.playbackTime = 0f;
        isPlayBack = true;
    }

    int crtClipHash;
	public void SetAnimationParam(string paramName, AnimatorParamType paramType, object param = null)
	{
		if(null == m_EntityAnimator)
			return;

		switch(paramType)
		{
		case AnimatorParamType.APT_Boolean:
			m_EntityAnimator.SetBool(paramName, (bool)param);
			break;
		case AnimatorParamType.APT_Float:
                //小刀和没拿武器都是空手的移动动作
            paramName = paramName.Replace("WT_None", "");
            paramName = paramName.Replace("WT_Knife", "");
			m_EntityAnimator.SetFloat(paramName, (float)param);
			break;
		case AnimatorParamType.APT_Integer:
			m_EntityAnimator.SetInteger(paramName, (int)param);
			break;
		case AnimatorParamType.APT_Trigger:
			m_EntityAnimator.SetTrigger(paramName);
			break;
		}
	}

    public bool IsClipFinish(string boolAnim)
    {
		if(m_EntityAnimator == null)
		{
			ApplicationEntry.Instance.PostSocketErr(57558);
			return false;
		}
        AnimatorStateInfo info = m_EntityAnimator.GetCurrentAnimatorStateInfo(0);
        // 判断动画是否播放完成
        if (info.IsName("Base Layer." + boolAnim) && info.normalizedTime >= 1.0f)
        {
            //播放完毕，要执行的内容
            return true;
        }
        return false;
    }

    public void DirectoryPlayAnimation(string stateName, float normalizeTime)
    {
        m_EntityAnimator.Play(stateName, -1, normalizeTime);
    }

	public void ResetState()
	{
		for(int i=0; i < stateList_.Count;)
		{
			if(stateList_[i].once_)
				stateList_.RemoveAt(i);
			else
				i++;
		}
		takeDmgAction = GlobalValue.TTakeDmg;
	}

    bool notPlayDef_;
    bool isPhysic_;
    bool realDodge_;
	public void ExcuteState(StateInst.ExcuteType type, DealStateCallBack callback = null, COM_ReportActionCounter counter = null, bool isPhysic = true, bool realDodge = false)
	{
        type_ = type;
        dealStateCallBack_ = callback;
        realDodge_ = realDodge;
        counter_ = counter;
        stateIndex_ = 0;
        isPhysic_ = isPhysic;

        if (isPhysic_)
            notPlayDef_ = DontPlayDefState();
        else
            notPlayDef_ = true;

        ExcuteStateInner();
	}

    bool DontPlayDefState()
    {
        for (int i = 0; i < stateList_.Count; ++i)
        {
            StateData data = StateData.GetData(stateList_[i].stateId_);
            if (data._StateType == (int)StateType.ST_ActionAbsorb ||
			    data._StateType == (int)StateType.ST_ActionBounce ||
			    data._StateType == (int)StateType.ST_ActionInvalid)
            {
                return true;
            }
        }
        return false;
    }

    void ExcuteStateInner()
    {
        if (stateIndex_ >= stateList_.Count)
        {
            if (dealStateCallBack_ != null)
            {
                dealStateCallBack_(counter_);
                dealStateCallBack_ = null;
            }
            return;
        }

        crtStateData_ = StateData.GetData(stateList_[stateIndex_].stateId_);
		if (!isPhysic_ && (crtStateData_._StateType == (int)StateType.ST_ActionAbsorb ||
            crtStateData_._StateType == (int)StateType.ST_ActionBounce ||
            crtStateData_._StateType == (int)StateType.ST_ActionInvalid))
        {
            stateIndex_++;
            ExcuteStateInner();
            return;
        }

		if (isPhysic_ && (crtStateData_._StateType == (int)StateType.ST_MagicAbsorb ||
            crtStateData_._StateType == (int)StateType.ST_MagicBounce ||
            crtStateData_._StateType == (int)StateType.ST_MagicInvalid))
        {
            stateIndex_++;
            ExcuteStateInner();
            return;
        }

        //如果不需要播放防御特效 并且这次要播放防御特效 则跳过
		if (notPlayDef_ && (crtStateData_._StateType == (int)StateType.ST_Defense || crtStateData_._StateType == (int)StateType.ST_Shield))
        {
            stateIndex_++;
            ExcuteStateInner();
            return;
        }

        switch (type_)
        {
            case StateInst.ExcuteType.ET_Work:
                if (stateList_[stateIndex_].workingInst_ == null && crtStateData_._WorkPkg.effectId_ != 0)
                {
                    EffectAPI.Play((EFFECT_ID)crtStateData_._WorkPkg.effectId_, ActorObj, null, null, null, (EffectInst ei, ParamData pData) =>
                    {
                        if (pData.iParam < stateList_.Count)
                        {
                            if (stateList_[pData.iParam].workingInst_ != null)
                                GameObject.Destroy(stateList_[pData.iParam].workingInst_.gameObject);
                            stateList_[pData.iParam].workingInst_ = ei;

                            if (renderer_ == null)
                                renderer_ = GetBody(ActorObj);

                            if (renderer_ == null)
                                renderer_ = new SkinnedMeshRenderer[0];

                            if (renderer_ != null)
                            {
                                for (int i = 0; i < renderer_.Length; ++i)
                                {
                                    renderer_[i].material.SetColor("_Color", crtStateData_._WorkPkg.mainColor_);
                                    renderer_[i].material.SetColor("_RimColor", crtStateData_._WorkPkg.rimColor_);
                                    renderer_[i].material.SetFloat("_RimWidth", crtStateData_._WorkPkg.rimWidth_);
                                }
                            }
                            ExcuteStateInner();
                        }
                        else
                        {
                            ei.DestorySelf();
                        }
                    }, new ParamData(stateIndex_));
                    stateIndex_++;
                }
                else
                {
                    stateIndex_++;
                    ExcuteStateInner();
                }
                break;
            case StateInst.ExcuteType.ET_Beattack:
                //如果是闪避且没有hp变化就不触发状态
                if (crtStateData_._StateType == (int)StateType.ST_Dodge && !realDodge_)
                {

                }
                else
                {
                    if (crtStateData_._BeattackPkg.effectId_ != 0)
                    {
                        EffectAPI.Play((EFFECT_ID)crtStateData_._BeattackPkg.effectId_, ActorObj, null, null, null, (EffectInst ei, ParamData data) =>
                        {
                            ei.transform.parent = ActorObj.transform;
                            ei.transform.localScale = Vector3.one;
                        });
                    }
                    if (!string.IsNullOrEmpty(crtStateData_._BeattackPkg.action_))
                        takeDmgAction = crtStateData_._BeattackPkg.action_;
                }
                stateIndex_++;
                ExcuteStateInner();
                break;
            case StateInst.ExcuteType.ET_Action:
                selfActor_ = Battle.Instance.GetActorByInstId(PlayerID);
                // mutiple state in this case would be cause some error. call mutiple times.
                if (crtStateData_._ActionPkg.effectId_ != 0)
                {
                    EffectAPI.Play((EFFECT_ID)crtStateData_._ActionPkg.effectId_, ActorObj, null, null, (int iVal) =>
                    {
                        ExcuteStateInnerPopVal();
                    });
                }
                else if (!string.IsNullOrEmpty(crtStateData_._ActionPkg.action_))
                {
                    ExcuteStateInnerPopVal();
                }
                else
                {
                    stateIndex_++;
                    ExcuteStateInner();
                }
                break;
            default:
                break;
        }
    }

    void ExcuteStateInnerPopVal()
    {
		if (!string.IsNullOrEmpty(crtStateData_._ActionPkg.action_))
			takeDmgAction = crtStateData_._ActionPkg.action_;

        COM_ReportTarget props = Battle.Instance.FindStateProps(selfActor_.BattlePos);
        if (props != null && props.prop_ != null)
        {
            int crtVal = 0;
            int chageVal = 0;
            switch (props.prop_.type_)
            {
                case PropertyType.PT_HpCurr:
                    crtVal = selfActor_.battlePlayer.hpCrt_;
                    chageVal = (int)props.prop_.value_ - crtVal;
                    selfActor_.battlePlayer.hpCrt_ = (int)props.prop_.value_;
                    if (selfActor_.battlePlayer.hpCrt_ < 0)
                        selfActor_.battlePlayer.hpCrt_ = 0;
                    break;
                case PropertyType.PT_MpCurr:
                    Debug.Log(" MP Before: " + selfActor_.battlePlayer.mpCrt_);
                    crtVal = selfActor_.battlePlayer.mpCrt_;
                    Debug.Log(" MP After: " + (int)props.prop_.value_);
                    chageVal = (int)props.prop_.value_ - crtVal;
                    Debug.Log(" MP Change: " + chageVal.ToString());
                    selfActor_.battlePlayer.mpCrt_ = (int)props.prop_.value_;
                    if (selfActor_.battlePlayer.mpCrt_ < 0)
                        selfActor_.battlePlayer.mpCrt_ = 0;
                    break;
            }
            
			if(selfActor_.battlePlayer.hpCrt_ > selfActor_.battlePlayer.hpMax_)
				selfActor_.battlePlayer.hpCrt_ = selfActor_.battlePlayer.hpMax_;
			if(selfActor_.battlePlayer.mpCrt_ > selfActor_.battlePlayer.mpMax_)
                selfActor_.battlePlayer.mpCrt_ = selfActor_.battlePlayer.mpMax_;
            hitOver_ = props.fly_;
            PopValueChange(chageVal, props.prop_.type_, selfActor_.battlePlayer.hpMax_, selfActor_.battlePlayer.mpMax_, () =>
            {
                Battle.Instance.RemoveStateProps(props);
                if (selfActor_.isDead)
                    selfActor_.ControlEntity.DealEntityDie();
                stateIndex_++;
                ExcuteStateInner();
            }, props.bao_);
            //状态不可能有闪避
            Beattack_1(0, 0, chageVal < 0);
        }
        else
        {
            stateIndex_++;
            ExcuteStateInner();
        }
    }

    public bool HasState(StateType sType)
    {
        StateData sd = null;
        for (int i = 0; i < stateList_.Count; ++i)
        {
            sd = StateData.GetData(stateList_[i].stateId_);
            if (sd._StateType == (int)sType)
                return true;
        }
        return false;
    }
    
    public void UpdateStateTick(bool isPhysic)
	{
        List<int> tickedStates = new List<int>();
        StateData sData = null;
		for(int i=0; i < stateList_.Count; )
		{
            if (!tickedStates.Contains(stateList_[i].stateId_))
            {
				if (stateList_[i].tick_ == 0)
				{
					sData = StateData.GetData( stateList_[i].stateId_ );
					if( null == sData )
					{
						ClientLog.Instance.Log("stateData == nulll");
						continue;
					} 
                    //如果是防御或圣盾 不计tick数
					if (sData._StateType == (int)StateType.ST_Defense || sData._StateType == (int)StateType.ST_Shield)
                    {
                        i++;
                    }
                    else
                    {
                        tickedStates.Add(stateList_[i].stateId_);
                        //RemoveState(stateList_[i].stateId_);
                    }
					continue;
				}

                sData = StateData.GetData(stateList_[i].stateId_);
				if (!isPhysic && (sData._StateType == (int)StateType.ST_MagicAbsorb ||
                    sData._StateType == (int)StateType.ST_MagicBounce ||
                    sData._StateType == (int)StateType.ST_MagicInvalid ||
                    sData._StateType == (int)StateType.ST_Sleep))
                {
                    stateList_[i].tick_--;
                }

				if (isPhysic && (sData._StateType == (int)StateType.ST_ActionAbsorb ||
                    sData._StateType == (int)StateType.ST_ActionBounce ||
                    sData._StateType == (int)StateType.ST_ActionInvalid ||
                    sData._StateType == (int)StateType.ST_Dodge ||
                    sData._StateType == (int)StateType.ST_Sleep))
                {
                    stateList_[i].tick_--;
                }
                
                if (stateList_[i].tick_ == 0)
                {
                    //如果是防御或圣盾 不计tick数
					if (sData._StateType == (int)StateType.ST_Defense || sData._StateType == (int)StateType.ST_Shield)
                    {
                        i++;
                    }
                    else
                    {
                        tickedStates.Add(stateList_[i].stateId_);
                        RemoveState(stateList_[i].stateId_);
                    }
                }
                else
                    i++;
            }
            else
            {
                i++;
            }
		}
        tickedStates.Clear();
	}
//
    public void UpdateStateTurn()
    {
        for (int i = 0; i < stateList_.Count; ++i)
        {
            stateList_[i].turn_--;
            if (stateList_[i].turn_ == 0)
                RemoveState(stateList_[i].stateId_);
        }
    }

	public int GetStateBeattackEffectID(int skillId)
	{
		SkillData skill = SkillData.GetMinxiLevelData (skillId);
		StateData data = null;
		if(skill != null)
		{
			// stateType
			for(int i=0; i < skill._StateIds.Length; ++i)
			{
				for(int j=0; j < stateList_.Count; ++j)
				{
					data = StateData.GetData(stateList_[j].stateId_);
					if(data._StateType == skill._StateIds[i])
					{
						return data._BeattackPkg.effectId_;
					}
				}
			}
		}
		return -1;
	}

	public bool PlaySkillBeattackEffect(int skillId)
	{
        SkillData skill = SkillData.GetMinxiLevelData(skillId);
		StateData data = null;
		if(skill != null)
		{
			for(int i=0; i < skill._StateIds.Length; ++i)
			{
				for(int j=0; j < stateList_.Count; ++j)
				{
                    data = StateData.GetData(stateList_[j].stateId_);
					if (data._StateType == skill._StateIds[i])
					{
						return false;
					}
				}
			}
		}
		return true;
	}

    [System.Serializable]
	public class StateInst
	{
		public enum ExcuteType
		{
			ET_None,
			ET_Work,
			ET_Beattack,
			ET_Action,
			ET_Max,
		}
		public int stateId_;
		public int tick_;
		public int turn_;
		public bool once_;
        [System.NonSerialized]
		public EffectInst workingInst_;
	}
#endregion
}
