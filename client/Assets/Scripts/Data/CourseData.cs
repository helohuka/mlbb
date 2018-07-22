using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CourseData
{
	public int id_;
	public int level_;
	public string name_;
	public string desc_;

	private static Dictionary<int, CourseData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, CourseData> ();

		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("CourseData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}

		int recordCounter = parser.GetRecordCounter();
		CourseData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new CourseData ();
			data.id_ = parser.GetInt (i, "ID");
			data.name_ = parser.GetString (i, "Name");
			data.desc_ = parser.GetString (i, "Desc"); 
			data.level_ = parser.GetInt (i, "Level");

			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("CourseData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}

		parser.Dispose ();
		parser = null;
	}

	public static CourseData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}

	public static Dictionary<int, CourseData> GetData()
	{
		return metaData;
	}
}

