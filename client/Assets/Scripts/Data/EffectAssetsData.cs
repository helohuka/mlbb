using System.Collections.Generic;

public class EffectAssetsData {

	public int id_;

	public string assetsName_;

	public int behaviour_id_;

	public int 	SoundID;

	private static Dictionary<int, EffectAssetsData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EffectAssetsData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EffectAssetsData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EffectAssetsData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EffectAssetsData ();
			data.id_ = parser.GetInt (i, "ID");
			data.assetsName_ = parser.GetString (i, "AssetName");
			data.behaviour_id_ = parser.GetInt (i, "EffectBehavioursID");
			data.SoundID =  parser.GetInt (i, "SoundID");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("EffectAssetsData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}

	public static EffectAssetsData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
}
