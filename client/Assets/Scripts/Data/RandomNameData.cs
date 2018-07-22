using UnityEngine;
using System;
using System.Collections.Generic;

public class RandomNameData {
	
	public string lastName_;

	public string firstName_;

	public SexType sexType_ = SexType.ST_Unknown;
	
	private static Dictionary<int, RandomNameData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, RandomNameData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("RandomNameData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		RandomNameData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new RandomNameData ();
			data.lastName_ = parser.GetString (i, "lastname");
			data.firstName_ = parser.GetString (i, "name");
			data.sexType_ = (SexType)Enum.Parse(typeof(SexType), parser.GetString (i, "namesex"));

			metaData[i] = data;
		}
		parser.Dispose ();
		parser = null;
	}

	public static string RandomName()
	{
		int lastNameIdx = UnityEngine.Random.Range (0, metaData.Count);
		int firstNameIdx = UnityEngine.Random.Range (0, metaData.Count);
		string finalName = metaData [lastNameIdx].lastName_ + metaData [firstNameIdx].firstName_;
		return finalName;
	}
}
