using UnityEngine;
using System.Collections;

public class hundredSystem
{
	private uint _challengeNum;
	private uint _useNum;
	private uint _resetNum;
	public RequestEventHandler<COM_HundredBattle> HundredBattleEnvet;

	private static hundredSystem _instance;
	private COM_HundredBattle _hundredBattle;

	public static hundredSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new hundredSystem();
			return _instance;
		}
	}

    public int currentFightLevel_;

	public uint ChallengeNum
	{
		get
		{
			return _challengeNum;
		}
		set
		{
			_challengeNum = value;
		}
	}

	
	public uint ResetNum
	{
		get
		{
			return _resetNum;
		}
		set
		{
			_resetNum = value;
		}
	}

	public uint UseNum
	{
		set
		{
			_useNum = value;
		}
		get
		{
			return _useNum;
		}
	}


	public COM_HundredBattle HundredBattle
	{
		set
		{
			if(value != null)
			{
				_hundredBattle = value;
				currentFightLevel_ = (int)_hundredBattle.curTier_;           
				ChallengeNum  = _hundredBattle.tier_;
				UseNum = _hundredBattle.surplus_;
				ResetNum = _hundredBattle.resetNum_;

				if(HundredBattleEnvet!= null)
				{
					HundredBattleEnvet(_hundredBattle);
				}
			}
		}
		get
		{
			return _hundredBattle;
		}
	}

	
}

