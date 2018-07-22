using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TimerReawData  {

	public int _Id;
	public int _time;
	public int _reward;
	
	private static Dictionary<int, TimerReawData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, TimerReawData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("BabyData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		TimerReawData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new TimerReawData ();
			data._Id = parser.GetInt (i, "id");
			data._time = parser.GetInt (i, "time");
			data._reward = parser.GetInt (i, "reward");
			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("TimerReawData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static TimerReawData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, TimerReawData>  GetData()
	{
		return metaData;
	}

}
