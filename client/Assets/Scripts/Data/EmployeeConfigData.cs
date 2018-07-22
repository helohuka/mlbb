using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeConfigData 
{
	// 伙伴id
	public int id_;
	public int star_;
	public int money_;

	public List<int> items = new List<int>();

	public static Dictionary<int, EmployeeConfigData[] > metaData;
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EmployeeConfigData[]>();

		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EmployeeData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}

		int recordCounter = parser.GetRecordCounter();
		EmployeeConfigData data = null;

		for(int i=0; i < recordCounter; ++i)
		{
			data = new EmployeeConfigData();
			data.id_ = parser.GetInt (i, "EmployeeId");
			data.money_ = parser.GetInt (i, "money");
			data.star_ = parser.GetInt (i, "star");
			data.items.Add(parser.GetInt (i, "item-1"));
			data.items.Add(parser.GetInt (i, "item-2"));
			data.items.Add(parser.GetInt (i, "item-3"));
			data.items.Add(parser.GetInt (i, "item-4"));
			data.items.Add(parser.GetInt (i, "item-5"));

			if (metaData.ContainsKey(data.id_) == false)
			{
				metaData[data.id_] = new EmployeeConfigData[6];
			}
			
			metaData[data.id_][data.star_-1] = data;
		}

	}

	public static EmployeeConfigData GetData(int id, int star)
	{
		ClientLog.Instance.Log("EmployeeID: " + id + " LEV:" + star);
		if(!metaData.ContainsKey(id))
		{
			return null;
		}
		if (star >= metaData[id].Length)
			return null;
		else if (star < 0)
			return null;
		return metaData[id][star];
	}

}

