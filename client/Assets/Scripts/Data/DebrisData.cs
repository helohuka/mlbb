using System;
using System.Collections;
using System.Collections.Generic;

public class DebrisData 
{
	public int id_;
	public int debrisId_;
	public int needNum_;
	public int itemId_;
	public int itemNum_;
	public ItemSubType subType_;


	private static Dictionary<int, DebrisData> metaData;
	private static Dictionary<int, DebrisData> empData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, DebrisData> ();
		empData = new Dictionary<int, DebrisData> ();
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("debrisData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		DebrisData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new DebrisData ();
			data.id_ = parser.GetInt (i, "ID");
			data.needNum_ = parser.GetInt (i, "Number");
			data.itemId_ = parser.GetInt (i, "Item");
			data.itemNum_ = parser.GetInt (i, "ItemNumber");
			data.subType_ = (ItemSubType)Enum.Parse(typeof(ItemSubType), parser.GetString(i, "ItemSubType"));

			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("debrisData" + ConfigLoader.Instance.csvext + "ID重复 " + data.id_);
				return;
			}
			metaData[data.id_] = data;
			if(empData.ContainsKey(data.itemId_))
			{
				ClientLog.Instance.LogError("debrisData" + ConfigLoader.Instance.csvext + "ID重复 " + data.id_);
				return;
			}
			empData[data.itemId_] = data;
		}
		parser.Dispose ();
		parser = null;

	}

	public static DebrisData GetData(int id)
	{
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}

	public static DebrisData GetEmpData(int id)
	{
		if(!empData.ContainsKey(id)) 
			return null;
		return empData[id];
	}

}

