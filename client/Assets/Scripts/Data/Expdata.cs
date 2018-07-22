using UnityEngine;
using System.Collections.Generic;

public class ExpData {
	
	public int level_;

	public long exp_;

	public long petExp_;
	
	private static Dictionary<int, ExpData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ExpData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ExpData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		ExpData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ExpData ();
			data.level_ = parser.GetInt (i, "Lv");
			string str = parser.GetString (i, "Exp");
			data.exp_ = long.Parse(str);
			string s = parser.GetString (i, "PetExp");
			data.petExp_ = long.Parse(s);
			
			if(metaData.ContainsKey(data.level_))
			{
				ClientLog.Instance.LogError("ExpData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.level_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static long GetPlayerMaxExp(int level)
	{
		if(!metaData.ContainsKey(level))
			return 0;
		return metaData[level].exp_;
	}

	public static long GetBabyMaxExp(int level)
	{
		if(!metaData.ContainsKey(level))
			return 0;
		return metaData[level].petExp_;
	}
}
