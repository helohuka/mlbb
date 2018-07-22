using UnityEngine;
using System.Collections;

public class BoxSystem
{

	private uint _remainTimeStart;		 //记录重置CD开始时间.	
	private float _remainCDTime; 		 //CD 时间.

	private uint _remainBlueTimeStart;		 //记录重置CD开始时间.	
	private float _blueCDTime; 		 //CD 时间.
	
	private int _freeNum;			 //挑战次数.	


	private static BoxSystem _instance;
	public static BoxSystem Instance
	{
		get
		{
			if(_instance == null)
				_instance = new BoxSystem();
			return _instance;
		}
	}


	public float GreenCDTime
	{
		set
		{
			_remainTimeStart = (uint)Time.realtimeSinceStartup;
			_remainCDTime  = value;
			//_remainCDTime  = 10000;
		}
		get
		{ 
			return (uint)Mathf.Max(0, _remainCDTime - (Time.realtimeSinceStartup - _remainTimeStart));
		}
	}


	public float BlueCDTime
	{
		set
		{
			_remainBlueTimeStart = (uint)Time.realtimeSinceStartup;
			_blueCDTime  = value;
			//_remainCDTime  = 10000;
		}
		get
		{ 
			return (uint)Mathf.Max(0, _blueCDTime - (Time.realtimeSinceStartup - _remainBlueTimeStart));
		}
	}


	public int FreeNum
	{
		set
		{
			if(_freeNum != value)
			{
				_freeNum = value;
			}
		}
		get
		{
			return _freeNum; 
		}
	}

	public void requestOpenBuyBox(float greenTime,float blueTime,int greenNum)
	{
		GreenCDTime = greenTime;
		BlueCDTime = blueTime;
		FreeNum = greenNum;
	}
}

