using System;
using System.Collections.Generic;

public class CheatsData
{
	public int id_;
	public int level_;
	public string name_;
	public string desc_;
	public HelpType type_;
	public HelpRaiseType uiType_;
	public int group_;
	public string find_;
	private static Dictionary<int, CheatsData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, CheatsData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("CheatsData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		CheatsData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new CheatsData ();
			data.id_ = parser.GetInt (i, "ID");
			data.name_ = parser.GetString (i, "Name");
			data.desc_ = parser.GetString (i, "Desc"); 
			data.level_ = parser.GetInt (i, "Level");
			data.group_ = parser.GetInt (i, "Group");
			data.type_  = (HelpType)Enum.Parse(typeof(HelpType), parser.GetString(i, "Type"));
			data.find_ = parser.GetString (i, "Find");
			if(!string.IsNullOrEmpty( parser.GetString(i, "UI")))
			{
				data.uiType_  = (HelpRaiseType)Enum.Parse(typeof(HelpRaiseType), parser.GetString(i, "UI"));
			}
			else
			{
				data.uiType_ = HelpRaiseType.HRT_None;
			}

			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("CheatsData" + ConfigLoader.Instance.csvext + "ID重复"); 
				return;
			}
			metaData[data.id_] = data;
		}
		
		parser.Dispose ();
		parser = null;
	}
	
	public static CheatsData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	
	public static Dictionary<int, CheatsData> GetData()
	{
		return metaData;
	}
}

