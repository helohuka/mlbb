using System;
using System.Collections;
using System.Collections.Generic;

public class CourseGiftData
{

	public int id_;
	public int shopId_;
	public int level_;
	public int time_;
	public string[] itemIds_;
	public int price_;
	public int oldPrice_;
	static Dictionary<int, CourseGiftData> metaData;
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, CourseGiftData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("CrystalData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		CourseGiftData data = null;
		
		for(int i=0; i < recordCounter; ++i)
		{
			data = new CourseGiftData ();
			data.id_ = parser.GetInt (i, "ID");    
			data.shopId_ = parser.GetInt (i, "ShopID");    
			data.level_ = parser.GetInt (i, "Lv");  
			data.time_ = parser.GetInt (i, "Time");  
			data.price_ = parser.GetInt (i, "Price");
			data.oldPrice_ = parser.GetInt (i, "Oldprice");
			data.itemIds_ = parser.GetString(i, "ItemID").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
			if(metaData.ContainsKey(data.shopId_))
			{
				ClientLog.Instance.LogError("CrystalData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.shopId_] = data;
			
		}
		parser.Dispose ();
		parser = null;
	}


	public static CourseGiftData GetData(int shopid)
	{
		if(!metaData.ContainsKey(shopid))
			return null;
		return metaData[shopid];
	}



		
}

