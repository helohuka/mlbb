using System;
using System.Collections;
using System.Collections.Generic;

public class CrystalData 
{
	
	public int Id;
	public int Quality;
	public PropertyType type;
	public string property;
	static Dictionary<int, CrystalData> metaData;
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, CrystalData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("CrystalData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		CrystalData data = null;
		
		for(int i=0; i < recordCounter; ++i)
		{
			data = new CrystalData ();
			data.Id = parser.GetInt (i, "ID");    
			data.Quality = parser.GetInt (i, "Quality");    
			data.type = (PropertyType)Enum.Parse(typeof(PropertyType), parser.GetString(i, "type"));
			data.property = parser.GetString (i, "property");  
			
			if(metaData.ContainsKey(data.Id))
			{
				ClientLog.Instance.LogError("CrystalData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.Id] = data;
			
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static CrystalData GetData(int type,int quality)
	{
		foreach(CrystalData x in metaData.Values)
		{
			if((int)x.type == type && x.Quality == quality)
				return x;
		}
		return null;
	}

}

