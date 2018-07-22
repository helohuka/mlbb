using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GrowthFundData {

	public int _Iv;
	public int _renum;
	public int _reward;
	public string _des;
	private static Dictionary<int, GrowthFundData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, GrowthFundData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("GrowthFundData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		GrowthFundData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new GrowthFundData ();
			data._Iv = parser.GetInt (i, "lv");
			data._renum = parser.GetInt (i, "renum");
			data._reward = parser.GetInt (i, "reward");
			data._des = parser.GetString (i, "des");
			if(metaData.ContainsKey(data._Iv))
			{
				ClientLog.Instance.LogError("GrowthFundData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Iv] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static GrowthFundData GetData(int lv)
	{
		if(!metaData.ContainsKey(lv))
			return null;
		return metaData[lv];
	}
	public static Dictionary<int, GrowthFundData>  GetData()
	{
		return metaData;
	}
}
