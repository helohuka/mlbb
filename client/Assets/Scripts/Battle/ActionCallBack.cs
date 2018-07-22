using UnityEngine;
using System.Collections;

public class ActionCallBack : MonoBehaviour
{
	public delegate	void PlayBeAttackOneStep( int idx = 0 );
	public delegate void PlayBeAttackTwoStep( int idx = 0 );
	public delegate	void ShowChangePlayerInfo( int idx = 0 );
	public delegate void PlayBeAttackEffect( int idx = 0 );
	public delegate void AttackFinishCallBack();
    public delegate void PlaySingBrustCallBack();
	public delegate void PlaySingAcitonCallBack();
	public delegate void BeAttackFinishCallBack();
	public delegate void PlayerDieFinishCallBack();
	public delegate	void MoveFinishCallBack(int data);
	public delegate void LookAtFinishCallBack ();
	public delegate void UpdateCallBack ();




	public PlayBeAttackOneStep		m_PlayBeAttackOneStep;
	public PlayBeAttackTwoStep		m_PlayBeAttackTwoStep;
	public ShowChangePlayerInfo		m_ShowChangePlayerInfo;
	public PlayBeAttackEffect		m_PlayBeAttackEffect;
	public AttackFinishCallBack 	m_AttackFinishCallBack;
	public MoveFinishCallBack		m_MoveFinishCallBack;
	public PlaySingAcitonCallBack	m_PlaySingAcitonCallBack;
    public PlaySingBrustCallBack    m_PlaySingBrustCallBack;
	public LookAtFinishCallBack		m_LookAtFinishCallBack;
	public BeAttackFinishCallBack	m_BeAttackFinishCallBack;
	public PlayerDieFinishCallBack	m_PlayerDieFinishCallBack;
	public UpdateCallBack			m_UpdateCallBack;

	public void PlayBeAttackOneStepCallBack()
	{
		if( null == m_PlayBeAttackOneStep ) return ;
		m_PlayBeAttackOneStep();
		m_PlayBeAttackOneStep = null;
	}

	public void PlayBeAttackTwoStepCallBack()
	{
		if( null == m_PlayBeAttackTwoStep ) return ;
		m_PlayBeAttackTwoStep();
		m_PlayBeAttackTwoStep = null;
	}

	public void ShowChangePlayerInfoCallBack()
	{
		if( null == m_ShowChangePlayerInfo ) return ;
		m_ShowChangePlayerInfo();
		m_ShowChangePlayerInfo = null;
	}	

	public void PlayBeAttackEffectCallBack()
	{
		if( null == m_PlayBeAttackEffect ) return;
		m_PlayBeAttackEffect();
		m_PlayBeAttackEffect = null;
	}

	public void ActionMoveFinishCallBack(int data)
	{
		if( null == m_MoveFinishCallBack ) return;
		m_MoveFinishCallBack(data);
        //m_MoveFinishCallBack = null;
	}

    public void MoveUpdateCallBack()
    {
        XShake shake = gameObject.GetComponent<XShake>();
        if (shake != null)
            GameObject.Destroy(shake);
    }

	public void ActionLookAtFinishCallBack()
	{
		if( null == m_LookAtFinishCallBack ) return;
		m_LookAtFinishCallBack();
		m_LookAtFinishCallBack = null;
	}
	
	public void AttackActionFinishCallBack()
	{
		if( null == m_AttackFinishCallBack ) return;
        // this function can Call AttackFinish Setter. so it cause killed.
		m_AttackFinishCallBack();
        m_AttackFinishCallBack = tAttackFinishCallBack;
        tAttackFinishCallBack = null;
	}

    AttackFinishCallBack tAttackFinishCallBack;
    public AttackFinishCallBack AttackFinish
    {
        set
        {
            if (m_AttackFinishCallBack != null)
            {
                tAttackFinishCallBack = value;
                ClientLog.Instance.Log("m_AttackFinishCallBack != null");
            }
            else
                m_AttackFinishCallBack = value;
        }
    }

    public void SingBustCallBack()
    {
        if (m_PlaySingBrustCallBack == null) return;
        m_PlaySingBrustCallBack();
    }

	public void SingActionCallBack()
	{
		if( null == m_PlaySingAcitonCallBack ) return;
		m_PlaySingAcitonCallBack();
	}

	public void BeAttackActionCallBack()
	{
		if( null == m_BeAttackFinishCallBack ) return;
		m_BeAttackFinishCallBack();
	}

	public void DieActionFinishCallBack()
	{
		if( null == m_PlayerDieFinishCallBack ) return ;
		m_PlayerDieFinishCallBack();
	}
	void Update()
	{
		if(m_UpdateCallBack != null)
			m_UpdateCallBack();
	}


}
