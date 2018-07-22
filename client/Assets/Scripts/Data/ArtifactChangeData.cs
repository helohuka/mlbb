using System;
using System.Collections;
using System.Collections.Generic;

public class ArtifactChangeData
{
	public int _Id;
	public JobType _JobType;
	public string _Icon ;
	public int _Diamonds;

	public static Dictionary<int, ArtifactChangeData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ArtifactChangeData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ArtifactChangeData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		ArtifactChangeData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ArtifactChangeData();

			string profession = parser.GetString(i, "Type");
			if(!string.IsNullOrEmpty(profession))
				data._JobType = (JobType)Enum.Parse(typeof(JobType), profession);
			data._Id = (int)data._JobType;
			data._Icon = parser.GetString(i, "Icon");
			data._Diamonds = parser.GetInt(i, "Diamonds");
			metaData[i] = data;

		}
		parser.Dispose ();
		parser = null;
	}


	public ArtifactChangeData GetData(int id)
	{
		if(metaData.ContainsKey(id))
			return metaData[id];
		return null;
	}

}

