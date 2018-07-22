using System;
using System.Collections.Generic;

public class MakeData 
{

	public int itemId_;
	
	public int skillId;
	
	public int skillLevel;
	
	public string[] needItems;
	public string[] needItemNum;
	public int needMoney;
	public string type_;
	public int needBook_;
	public int specialID_;
	public static Dictionary<int, MakeData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, MakeData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("GatherData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		MakeData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new MakeData ();
			data.itemId_ = parser.GetInt (i, "item_id");
			data.skillId = parser.GetInt (i, "skill_kind");
			data.skillLevel = parser.GetInt (i, "skill_lv");
			
			data.needItems = parser.GetString(i, "need_item").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
			data.needItemNum = parser.GetString(i, "need_itemnum").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
			data.needMoney =parser.GetInt (i, "need_money");
			data.type_ = parser.GetString (i, "type");
			data.needBook_ = parser.GetInt (i, "need_book");
			data.specialID_ = parser.GetInt (i, "SpecialID");
			if(metaData.ContainsKey(data.itemId_))
			{
				ClientLog.Instance.LogError("MakeData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.itemId_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static MakeData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}

}

