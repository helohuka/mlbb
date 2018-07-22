using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ACT_RewardData {

	public int _Id;
	
	public int _ItemID;
	
	public int _Price;
	

	
	private static Dictionary<int, ACT_RewardData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ACT_RewardData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("AchieveData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		ACT_RewardData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ACT_RewardData ();
			data._Id = parser.GetInt (i, "ID");
			data._ItemID = parser.GetInt (i, "itemID");
			data._Price = parser.GetInt (i, "price");
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
	
	public static ACT_RewardData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, ACT_RewardData> GetData()
	{
		
		return metaData;
	}

	public static int minprice_()
	{
	    List<ACT_RewardData> actRewData = new List<ACT_RewardData>();
		foreach(KeyValuePair<int, ACT_RewardData> pair in ACT_RewardData.GetData())
		{
			actRewData.Add(pair.Value);
		}

		int min = 0;
		for(int i =0;i<actRewData.Count;i++)
		{
			min = actRewData[0]._Price;
			if(min>actRewData[i]._Price)
			{
				min = actRewData[i]._Price;
			}
		}
		return min;
	}
}
