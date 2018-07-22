/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackControlEnitiy
{
	ParamData			m_ParamData;
	#region Singletion
	static private AttackControlEnitiy s_Instance = null;
	static public AttackControlEnitiy Instance
	{
		get
		{
			if(s_Instance == null)
			{
				s_Instance = new AttackControlEnitiy();
				s_Instance.Init();
			}
			return s_Instance;
		}
	}
	#endregion

	public enum E_ShowType 
	{
		MAX ,
		SHOWTYPE_MELEE , 
		SHOWTYPE_LONGRANGE ,
	}

	public enum E_ShowMeleeState
	{
		MAX , 
		SHOWMELEESTATE_PLAYSINGACTION,
		SHOWMELEESTATE_PLAYSINGACTIONFINISH,
		SHOWMELEESTATE_USESKILL , 
	}

	public enum E_ShowLongRangeState
	{
		MAX , 
		SHOWLONGRANGESTATE_PLAYSINGACTION ,
		SHOWLONGRANGESTATE_PLAYSINGACITONFINISH , 
		SHOWLONGRANGESTATE_USESKILL , 
	}

	private Entity					m_SourceEnitiy;
	private Entity					m_TargetEnitiy;
	private	E_ShowType				m_eShowType;
	private	E_ShowMeleeState		m_eShowMeleeState;
	private E_ShowLongRangeState	m_eShowLongRangeState;
	
	void Init()
	{
		m_eShowLongRangeState = E_ShowLongRangeState.MAX;
		m_eShowMeleeState = E_ShowMeleeState.MAX;
		m_eShowType = E_ShowType.MAX;
	}

	public void RegisterEnitiy( Entity	Source , E_ShowType E_ShowType , ParamData data )
	{
		if( null == Source ) return ;
		if( null == Source.ControlEntity ) return;
		if( null == Source.ControlEntity.TargetEntity ) return ;
		m_SourceEnitiy = Source;
		m_TargetEnitiy = Source.ControlEntity.TargetEntity;
		m_eShowType = E_ShowType;
		m_eShowLongRangeState = E_ShowLongRangeState.SHOWLONGRANGESTATE_PLAYSINGACTION;
		m_eShowMeleeState = E_ShowMeleeState.SHOWMELEESTATE_PLAYSINGACTION;
		m_ParamData = data;
	}

	void UseSkill()
	{
		int SkillID = AttackDemoMain.Instance.FindSelectSkillID( m_SourceEnitiy.entity_.id_ );
		SkillData data =  SkillData.GetData( SkillID );
		List<int> lstTaget = data.ExcuteEffectPos(m_SourceEnitiy.ControlEntity.TargetEntity.entity_.position_);
		List<Entity>	lstEnity = new List<Entity>();
		for( int i = 0; i <lstTaget.Count; ++i)
		{
			Entity tmp = AttackDemoMain.Instance.GetEntityByIdx( lstTaget[i] );
			if( null == tmp || tmp.isDead ) continue;
			lstEnity.Add( tmp );
		}
		SkillInst inst = new SkillInst(data);
		bool isMlee = m_eShowType == E_ShowType.SHOWTYPE_MELEE ? true : false;
		inst.Cast( m_SourceEnitiy , lstEnity.ToArray() , isMlee , AttackDemoMain.Instance.ActionFinishCallBack , m_ParamData );
	}

	void SetShowMeleeState( E_ShowMeleeState State )
	{
		m_eShowMeleeState = State;
	}

	public void SetShowLongStateState( E_ShowLongRangeState State )
	{
		m_eShowLongRangeState = State;
	}

	void PlayMeleeSingEffectFinishCallBack()
	{
		SetShowMeleeState( E_ShowMeleeState.SHOWMELEESTATE_USESKILL );
	}

	public void Update()
	{
		if( m_eShowType == E_ShowType.SHOWTYPE_MELEE )
		{
			switch( m_eShowMeleeState )
			{
			case E_ShowMeleeState.SHOWMELEESTATE_PLAYSINGACTION:
				{
				int SkillID = AttackDemoMain.Instance.FindSelectSkillID( m_SourceEnitiy.entity_.id_ );
					if( SkillID != GlobalValue.SKILL_ATTACKID )
					{
						SetShowMeleeState( E_ShowMeleeState.SHOWMELEESTATE_PLAYSINGACTIONFINISH );
						EffectAPI.Play( EFFECT_ID.EFFECT_WULIXVLI , m_SourceEnitiy.ControlEntity.EntityObj , null , null , PlayMeleeSingEffectFinishCallBack );
					}
					else
					{
						SetShowMeleeState( E_ShowMeleeState.SHOWMELEESTATE_USESKILL );
					}
				}
				break;
			case E_ShowMeleeState.SHOWMELEESTATE_USESKILL:
				{
					UseSkill();
					SetShowMeleeState(E_ShowMeleeState.MAX );
				}
				break;
			}
		}
		else
		{
			switch( m_eShowLongRangeState )
			{
			case E_ShowLongRangeState.SHOWLONGRANGESTATE_PLAYSINGACTION:
				{
					SetShowLongStateState( E_ShowLongRangeState.SHOWLONGRANGESTATE_PLAYSINGACITONFINISH );
					m_SourceEnitiy.ControlEntity.PlayEntityAction( GlobalValue.ActionName , GlobalValue.Action_Sing );
					EffectAPI.Play( EFFECT_ID.EFFECT_XVLI , m_SourceEnitiy.ControlEntity.EntityObj );		
				}
				break;
			case E_ShowLongRangeState.SHOWLONGRANGESTATE_USESKILL:
				{
					SetShowLongStateState( E_ShowLongRangeState.MAX);
					UseSkill();
				}
				break;
			}
		}
	}
}
*/











