using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrystalUpData
{
	public int level;
	public int DebrisNum;
	public int GodNum;
	public int Mission;
	static Dictionary<int, CrystalUpData> metaData;
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, CrystalUpData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("CrystalUpData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		CrystalUpData data = null;
		
		for(int i=0; i < recordCounter; ++i)
		{
			data = new CrystalUpData ();
			data.level = parser.GetInt (i, "levelup");    
			data.DebrisNum = parser.GetInt (i, "DebrisNum");    
			data.GodNum = parser.GetInt (i, "GodNum");    
			data.Mission = parser.GetInt (i, "Mission");  

			if(metaData.ContainsKey(data.level))
			{
				ClientLog.Instance.LogError("EmployeeData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.level] = data;

		}
		parser.Dispose ();
		parser = null;
	}

	public static CrystalUpData GetData(int id)
	{
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}

}

