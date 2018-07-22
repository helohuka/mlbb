using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ActivityData  {

	public int _Id;
	
	public int _Target;
	
	public string _Desc;

	public int _Reward;


	private static Dictionary<int, ActivityData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ActivityData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("AchieveData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		ActivityData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ActivityData ();
			data._Id = parser.GetInt (i, "ID");
			data._Target = parser.GetInt (i, "target");
			data._Reward = parser.GetInt (i, "reward");
			data._Desc = parser.GetString (i, "desc");
			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("AchieveData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static ActivityData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, ActivityData> GetData()
	{
		
		return metaData;
	}
}
