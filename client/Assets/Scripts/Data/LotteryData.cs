using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LotteryData {
	public int id_;
	public int LotteryID_;
	public int RewardLv_;
	public string Win_symbol;
	//public int CouponID_;
	//public int Rewarditem_;
	//public int Rewarditem_Num_;
	public string RewardName_;
	public int RewardRate_;
	private static Dictionary<int, LotteryData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, LotteryData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("TalkData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		LotteryData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new LotteryData ();
			data.id_ = parser.GetInt (i, "ID");
			data.LotteryID_ = parser.GetInt (i, "LotteryID");
			data.RewardLv_ = parser.GetInt (i, "RewardLv");
			data.Win_symbol = parser.GetString (i, "Win_symbol");
//			data.CouponID_ = parser.GetInt (i, "CouponID");
//			data.Rewarditem_ = parser.GetInt (i, "Rewarditem");
//			data.Rewarditem_Num_ = parser.GetInt (i, "Rewarditem_Num");
			data.RewardName_ = parser.GetString (i, "RewardName");
			data.RewardRate_ = parser.GetInt (i, "RewardRate");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("TalkData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static LotteryData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, LotteryData> GetData()
	{
		if(metaData.Count == 0)
			return null;
		return metaData;
	}
}
