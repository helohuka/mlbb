using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public class RoundMonster : MonoBehaviour
{
	[Serializable]
	public class RoundMonsterInfo
	{
		//
		[SerializeField]
		public GameObject		m_MonsterObj = null;
		//
		[SerializeField]
		public GameObject[]		m_path;
		//
		[NonSerialized]
		public	bool			m_bOpeationing = false;
		//
		[NonSerialized]
		public	int				m_PathCount = 0;
		//
		[NonSerialized]
		public string 			m_actStandName = "stand";
		//
		[NonSerialized]
		public string 			m_actRunName = "stand";
		//
		[NonSerialized]
		public int				m_actStandState = 0;
		//
		[NonSerialized]
		public int 				m_actRunState = 1;
		//
		[NonSerialized]
		public GameObject		m_curPath = null;	
	}
	[SerializeField]
	public List<RoundMonsterInfo>	m_lstRoundMonsterInfo = new List<RoundMonsterInfo>();
	[NonSerialized]
	public const float				MOVETIME = 10f;
	[NonSerialized]
	public float					m_fStandLimitTime = 3f;
	[NonSerialized]
	public float					m_fStandTime = 0f;
	[NonSerialized]
	public bool						m_bStanding = false;
	//
	void Start () 
	{
		for( int iLoop = 0; iLoop < m_lstRoundMonsterInfo.Count; ++ iLoop )
		{
			RoundMonsterInfo	data = m_lstRoundMonsterInfo[iLoop];
			data.m_MonsterObj.GetComponent<CharacterController>().detectCollisions = false;
		}
	}

	void Update () 
	{
		if( null == m_lstRoundMonsterInfo || 0 == m_lstRoundMonsterInfo.Count ) return ;
		for( int iLoop = 0; iLoop < m_lstRoundMonsterInfo.Count; ++ iLoop )
		{
			RoundMonsterInfo	data = m_lstRoundMonsterInfo[iLoop];
			if( null == data ) continue;
			if( null == data.m_path ) continue;
		
			if( data.m_PathCount >= data.m_path.Length )
			{
				data.m_PathCount = 0;
			}

			if( bStand() && Time.realtimeSinceStartup - m_fStandTime >= m_fStandLimitTime )
			{
				m_fStandTime = Time.realtimeSinceStartup;
				m_bStanding = true;
				PlayMonsterAction( data.m_MonsterObj , data.m_actStandName , data.m_actStandState );
			}
			
			if( m_bStanding && Time.realtimeSinceStartup - m_fStandTime >= m_fStandLimitTime )
			{
				m_bStanding = false;
				m_fStandTime = 0f;
				PlayMonsterAction( data.m_MonsterObj , data.m_actRunName , data.m_actRunState );
			}
			
			if( m_bStanding ) return;

			if( !data.m_bOpeationing )
			{
				for( ; data.m_PathCount < data.m_path.Length;  )
				{
					data.m_curPath = data.m_path[data.m_PathCount];
					data.m_bOpeationing = true;
					++ data.m_PathCount;
					PlayMonsterAction( data.m_MonsterObj , data.m_actRunName , data.m_actRunState );
					break;
				}
			}


			MoveTo( data.m_MonsterObj , data.m_curPath , data.m_actRunName , data.m_actStandState );

			if( Vector3.Distance( data.m_MonsterObj.transform.position , data.m_curPath.transform.position ) < 0.3 )
			{
				data.m_bOpeationing = false;
				PlayMonsterAction( data.m_MonsterObj , data.m_actStandName , data.m_actStandState );
			}
		}
	}

	void MoveTo( GameObject Obj , GameObject targetPath , string ActionName , int Integer )
	{
		CharacterController		cc = Obj.GetComponent<CharacterController>();
		if( null == cc ) return;
		Vector3	 v3 = targetPath.transform.position - Obj.transform.position;
		Obj.transform.LookAt(targetPath.transform.position);
		cc.Move(v3.normalized * Time.deltaTime * 0.5f );
	}

	void PlayMonsterAction( GameObject	Obj , string ActionName , int integer )
	{
		if( null == Obj ) return ;
		Animator	Ani = Obj.GetComponent<Animator>();
		Ani.SetInteger( ActionName , integer );
	}

	bool	bStand()
	{
		int value = UnityEngine.Random.Range( 0 , 1000 );
		if( value == 5 )
		{
			return true;
		}
		return false;
	}
}





