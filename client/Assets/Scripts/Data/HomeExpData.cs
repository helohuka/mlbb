using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HomeExpData {

	public int Lv_;
	
	public int Exp_;
	

	
	private static Dictionary<int, HomeExpData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, HomeExpData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("HomeExpData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		HomeExpData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new HomeExpData ();
			data.Lv_ = parser.GetInt (i, "lv");
			data.Exp_ = parser.GetInt (i, "Exp");

			if(metaData.ContainsKey(data.Lv_))
			{
				ClientLog.Instance.LogError("HomeExpData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.Lv_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static HomeExpData GetHomeExpData(int lv)
	{
		if(!metaData.ContainsKey(lv))
		{
			return null;
		}
		return metaData[lv];
	}
}
