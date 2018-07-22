using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PetIntensiveData {
	
	public int level_;
	public int item_;
	public int itemnum_;
	public float grow_;
	public int probability_;
	private static Dictionary<int, PetIntensiveData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, PetIntensiveData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("CheatsData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		PetIntensiveData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new PetIntensiveData ();
			data.level_ = parser.GetInt (i, "Lv");
			data.item_ = parser.GetInt (i, "item");
			data.itemnum_ = parser.GetInt (i, "itemnum"); 
			data.grow_ = parser.GetFloat (i, "grow");
			data.probability_ = parser.GetInt (i, "probability");
			if(metaData.ContainsKey(data.level_))
			{
				ClientLog.Instance.LogError("PetIntensiveData" + ConfigLoader.Instance.csvext + "ID重复"); 
				return;
			}
			metaData[data.level_] = data;
		}
		
		parser.Dispose ();
		parser = null;
	}
	
	public static PetIntensiveData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	
	public static Dictionary<int, PetIntensiveData> GetData()
	{
		return metaData;
	}
}
