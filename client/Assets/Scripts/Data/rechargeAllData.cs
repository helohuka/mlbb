using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rechargeAllData 
{
	public int num_;
	public string[] reward;
	public string[] renum;
	public string picname;
	public static Dictionary<int, rechargeAllData> metaData;
	public static List<rechargeAllData> rechargeList = new List<rechargeAllData> (); 

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, rechargeAllData> ();
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("rechargeAllData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		rechargeAllData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new rechargeAllData ();
			data.num_ = parser.GetInt (i, "num");
			data.picname = parser.GetString (i, "picname");
			if(metaData.ContainsKey(data.num_))
			{
				ClientLog.Instance.LogError("debrisData" + ConfigLoader.Instance.csvext + "ID重复 " + data.num_);
				return;
			}
			metaData[data.num_] = data;
			rechargeList.Add(data);
		
		}
		parser.Dispose ();
		parser = null;
	}

	public static rechargeAllData GetData(int id)
	{
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}

}

