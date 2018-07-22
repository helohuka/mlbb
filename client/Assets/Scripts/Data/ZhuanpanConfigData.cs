using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ZhuanpanConfigData  {

	public int _Id;
	public int _ItemID;
	public int _Number;
	public int _Probability;
	public int _DailyOutput;
	public int _RewardRate;
	private static Dictionary<int, ZhuanpanConfigData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ZhuanpanConfigData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ZhuanpanConfigData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		ZhuanpanConfigData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ZhuanpanConfigData ();
			data._Id = parser.GetInt (i, "ID");
			data._ItemID = parser.GetInt (i, "Item");
			data._Number = parser.GetInt (i, "Number");
			data._Probability = parser.GetInt (i, "Probability");
			data._DailyOutput = parser.GetInt (i, "DailyOutput");
			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("ZhuanpanConfigData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	public static ZhuanpanConfigData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, ZhuanpanConfigData> GetData()
	{

		return metaData;
	}
}
