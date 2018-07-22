using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerTitleData {

	public int id_;
	
	public string desc_;

	public int MinReputation_;

	public int MaxReputation_;

	private static Dictionary<int, PlayerTitleData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, PlayerTitleData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("PlayerTitleData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		PlayerTitleData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new PlayerTitleData ();
			data.id_ = parser.GetInt (i, "TitleId");
			data.desc_ = parser.GetString (i, "Desc");
			data.MinReputation_ = parser.GetInt (i, "MinReputation");
			data.MaxReputation_ = parser.GetInt (i, "MaxReputation");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("PlayerTitleData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static PlayerTitleData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, PlayerTitleData> GetData()
	{
		
		return metaData;
	}
}
