using UnityEngine;
using System.Collections.Generic;

public class MusicAssetsData
{
	public int id_;
	
	public string assetsName_;
	
	private static Dictionary<int, MusicAssetsData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, MusicAssetsData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("UIAssetsData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		MusicAssetsData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new MusicAssetsData ();
			data.id_ = parser.GetInt (i, "ID");
			data.assetsName_ = parser.GetString (i, "AssetName");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("UIAssetsData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static MusicAssetsData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
		
}

