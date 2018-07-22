using System.Collections.Generic;

public class EntityAssetsData {

	public int id_;

	public string assetsName_;

	public string assetsIocn_;

    public string bindPoint_;

	public float zoom_;	

	public string _skillhard;

	private static Dictionary<int, EntityAssetsData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EntityAssetsData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EntityAssetsData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EntityAssetsData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EntityAssetsData ();
			data.id_ = parser.GetInt (i, "ID");
			data.assetsName_ = parser.GetString (i, "AssetName");
			data.assetsIocn_ = parser.GetString (i, "Icon");
            data.bindPoint_ = parser.GetString(i, "BindPoint");
			data.zoom_  = parser.GetFloat(i,"Zoom");
			data._skillhard = parser.GetString (i, "skillhard");
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("EntityAssetsData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}

	public static EntityAssetsData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
}
