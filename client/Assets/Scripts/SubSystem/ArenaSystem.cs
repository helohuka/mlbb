using UnityEngine;
using System.Collections;

public class ArenaSystem
{
	public event ArenaRivalHandler arenaRivalEvent;
	public event RequestEventHandler<COM_EndlessStair> UpdateArenaEnven;
	public event RequestEventHandler<COM_SimplePlayerInst> checkPlayerEnven;
	public event RequestEventHandler<COM_JJCBattleMsg> newBattleMsgEnven;

	private COM_EndlessStair[] _rivals;  //可挑战者.		
	private COM_EndlessStair[] _ranks;   //排行榜.
	private COM_JJCBattleMsg[] _battleMsgs;   //对战记录.

	private uint _remainTimeStart;		 //记录重置CD开始时间.	
	private float _remainCDTime; 		 //CD 时间.
	private int _challengeNum;			 //挑战次数.
	private COM_EndlessStair _myInfo;	 //	

 	public  bool openPvP;
	public  bool openPvE;

	private static ArenaSystem _instance;
	public static ArenaSystem Instance
	{
		get
		{
			if(_instance == null)
				_instance = new ArenaSystem();
			return _instance;
		}
	}



	public float RemainCDTime
	{
		set
		{
			_remainTimeStart = (uint)Time.realtimeSinceStartup;
			_remainCDTime  = value/1000;
			//_remainCDTime  = 10000;
		}
		get
		{ 
			return (uint)Mathf.Max(0, _remainCDTime - (Time.realtimeSinceStartup - _remainTimeStart));
		}
	}



	public int ChallengeNum
	{
		set
		{
			if(_challengeNum != value)
			{
				_challengeNum = value;
			}
		}
		get
		{
			return _challengeNum; 
		}
	}


	public COM_EndlessStair[] Rivals
	{
		set
		{
			_rivals = value;

			if(arenaRivalEvent!= null)
			{
				arenaRivalEvent();
			}
		}
		get
		{
			return _rivals;
		}
	}

	public COM_EndlessStair[] Ranks
	{
		set
		{
			_ranks = value;
			if(arenaRivalEvent!= null)
			{
				arenaRivalEvent();
			}
		}
		get
		{
			return _ranks;
		}
	}

	public void NewBattleMsg(COM_JJCBattleMsg msg)
	{
		/*if(_battleMsgs.Length >= 10)
		{
			_battleMsgs[0] = msg;
		}
		else
		{
			_battleMsgs[0] = msg;
		}
		*/

		if(newBattleMsgEnven != null)
		{
			newBattleMsgEnven(msg);
		}
	}

	public COM_JJCBattleMsg[] BattleMsgs
	{

		set
		{
			_battleMsgs = value; 
		}
		get
		{
			return _battleMsgs;
		}
	}

	public  COM_EndlessStair MyInfo
	{
		set
		{
			if(value != null)
			{
				_myInfo  = value;
				ChallengeNum = _myInfo.rivalNum_;
				RemainCDTime = _myInfo.rivalTime_;
			
				if(UpdateArenaEnven != null)
				{
					UpdateArenaEnven(_myInfo);
				}
			}
		}
		get
		{
			return _myInfo;
		}
	}

	public void checkArenaPlayer( COM_SimplePlayerInst inst)
	{
		if(checkPlayerEnven != null)
		{
			checkPlayerEnven(inst);
		}
	}

	public void IsOPenArena()
	{
		if (openPvE)
		{
			ArenaUI.SwithShowMe ();
		}
		if(openPvP )
		{
			ArenaPvpPanelUI.SwithShowMe();
			//NetConnection.Instance.requestpvprank();
		}
	}

}

