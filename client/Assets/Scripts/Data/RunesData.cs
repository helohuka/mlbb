using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunesData  {
	
	public int id;
	public int needItemId;
	public int needItemNum;
	public int resultId;
	
	private static Dictionary<int, RunesData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, RunesData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("RunesData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		RunesData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new RunesData ();
			data.id = parser.GetInt (i, "ID");
			string str = parser.GetString (i, "ItemID");
			if(string.IsNullOrEmpty(str))
			{
				data.needItemId  = 0;
				data.needItemNum = 0;
			}
			else
			{
				string[] strArr =  str.Split(';');
				data.needItemId = int.Parse(strArr[0]);
				data.needItemNum = int.Parse(strArr[1]);
			}

			data.resultId = parser.GetInt (i, "Result");

			if(metaData.ContainsKey(data.id))
			{
				ClientLog.Instance.LogError("ExpData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id] = data;
		}
		parser.Dispose ();
		parser = null;
	}


	public static RunesData getData(int id)
	{
		if(metaData.ContainsKey(id))
		{
			return metaData[id];
		}

		return null;
	}


}

