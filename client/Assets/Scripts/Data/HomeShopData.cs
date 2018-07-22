using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HomeShopData  {

	public int id_;

	public string name_;

	public int  Num_;

	public int Price_;

	public int needLv_;

	public int timeLimit_;

	public int Itemid_;

	private static Dictionary<int, HomeShopData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, HomeShopData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("HomeShopData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		HomeShopData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new HomeShopData ();
			data.id_ = parser.GetInt (i, "ID");
			data.Num_ = parser.GetInt (i, "Num");
			data.needLv_ = parser.GetInt (i, "needlv");
			data.timeLimit_ = parser.GetInt (i, "Timelimit");
			data.Price_ = parser.GetInt (i, "Price");
			data.name_ = parser.GetString (i, "Name");
			data.Itemid_ = parser.GetInt (i, "Itemid");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("HomeShopData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static HomeShopData GetHomeShopData(int id)
	{
		return metaData[id];
	}
}
