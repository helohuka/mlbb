using System;
using System.Collections.Generic;

public class PvRrewardData
{
	public int id_;
	public string[] ranking_;
	public int times_;
	public int day_;
	public int senson_;
	
	
	public static Dictionary<int, PvRrewardData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, PvRrewardData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("PvRrewardData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		PvRrewardData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new PvRrewardData ();
			data.id_ = parser.GetInt (i, "ID");
			data.times_ = parser.GetInt (i, "dropID_times");
			data.day_ = parser.GetInt (i, "dropID_day");
			data.senson_ = parser.GetInt (i, "dropID_senson");
			data.ranking_ = parser.GetString(i, "Ranking").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
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
		foreach(PvRrewardData x in metaData.Values )
		{
			if(rank >= int.Parse(x.ranking_[0]) && rank <= int.Parse(x.ranking_[1]))
				return x.day_;
		}

		return 0;
	}
}

