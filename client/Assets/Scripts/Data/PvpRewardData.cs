using System;
using System.Collections.Generic;

public class PvpRewardData
{
	public int id_;
	public int min_;
	public int max_;
	public int times_;
	public int day_;
	public int senson_;
	public string rankName_;
	
	
	public static Dictionary<int, PvpRewardData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, PvpRewardData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("PvRrewardData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		PvpRewardData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new PvpRewardData ();
			data.id_ = parser.GetInt (i, "runk");
			data.min_ = parser.GetInt (i, "min");
			data.max_ = parser.GetInt (i, "max");
			data.times_ = parser.GetInt (i, "dropID_senson");
			data.day_ = parser.GetInt (i, "dropID_day");
			data.senson_ = parser.GetInt (i, "dropID_senson");
			data.rankName_ = parser.GetString (i, "rankName");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("PvRrewardData" + ConfigLoader.Instance.csvext + "ID重复 " + data.id_);
				return;
			}
			metaData[data.id_] = data;
			
		}
		parser.Dispose ();
		parser = null;
		
	}

	
	public static int GetRewardData(int rank)
	{
		foreach(PvpRewardData x in metaData.Values )
		{
			if(rank >= x.min_&& rank <= x.max_)
				return x.times_;
		}
		return 0;
	}


}

