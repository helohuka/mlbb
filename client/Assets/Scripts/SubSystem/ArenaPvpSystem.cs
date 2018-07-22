using UnityEngine;
using System.Collections;

public class ArenaPvpSystem 
{
	private COM_PlayerVsPlayer _myInfo; 
	private static ArenaPvpSystem _instance;
	private bool _isMatching;
	private float _matchingStartTime;
	private float _matchingTime;
	private  COM_SimpleInformation[] _pvpPlayerTeam;
	private  COM_SimpleInformation _pvpPlayer;
	private uint _battleTeamId;

	public event RequestEventHandler<bool> PvpMatchingEnven;
	public event RequestEventHandler<COM_SimpleInformation[]> playerTeamEnven;
	public event RequestEventHandler<COM_SimpleInformation> playerSingleEnven;
	public event RequestEventHandler<bool> OpenPvpUIEnven;

	public COM_PlayerVsPlayer[]  teamPlayes;

	public static ArenaPvpSystem Instance
	{
		get
		{
			if(_instance == null)
				_instance = new ArenaPvpSystem();
			return _instance;
		}
	}


	public COM_PlayerVsPlayer MyInfo
	{
		set
		{
			_myInfo = value;
		}
		get
		{
			return _myInfo;
		}
	}

	public bool PvpMatching
	{
		set
		{
			if(value)
			{
				_matchingTime = 300f;
				_matchingStartTime = Time.realtimeSinceStartup;
			}
			_isMatching = value;
			if(PvpMatchingEnven != null)
			{
				PvpMatchingEnven(_isMatching);
			}
		}
		get
		{
			return _isMatching;
		}
	}
	public void StopMatching(float time)
	{
		if(time == 0)
		{
			if(!ArenaSystem.Instance.openPvP)
				return;
			//MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("resetMatching"),()=>{NetConnection.Instance.warriorStart();});
		}
	}

	public uint GetMatchingTimeOut
	{
		get
		{
			return  (uint)Mathf.Max(0, (Time.realtimeSinceStartup - _matchingStartTime));
		}
	}

	public COM_SimpleInformation[] PvpPlayerTeam
	{
		set
		{
			_pvpPlayerTeam = value;
			if(playerTeamEnven != null)
				playerTeamEnven(_pvpPlayerTeam);
		}
		get
		{
			return _pvpPlayerTeam;
		}
	}


	public COM_SimpleInformation playerSingle
	{
		set
		{
			_pvpPlayer = value;
			if(playerSingleEnven != null)
				playerSingleEnven(value);
		}
		get
		{
			return _pvpPlayer;
		}
	}


	public uint TeamId
	{
		set
		{
			_battleTeamId = value;
		}
		get
		{
			return _battleTeamId;
		}
	}

	public bool openPvpUI
	{
		set
		{
			if(OpenPvpUIEnven != null)
				OpenPvpUIEnven(value);
		}
		get
		{
			return false;
		}
	}


}

