using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoreLevelData 
{
	public int level_;
	public List<int> items_ = new List<int>();
	public List<int> itemNum_ = new List<int>();

	public static Dictionary<int, MoreLevelData> metaData;
	public static List<MoreLevelData> moreLevelList = new List<MoreLevelData> (); 

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, MoreLevelData> ();
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("rechargeAllData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		MoreLevelData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new MoreLevelData ();
			data.level_ = parser.GetInt (i, "lv");
			string[] items =  parser.GetString (i, "reward").Split(';');
			for(int j= 0;j<items.Length;j++)
			{
				string[] str =items[j].Split(':');
				data.items_.Add(int.Parse(str[0]));
				data.itemNum_.Add(int.Parse(str[1]));
			}

			if(metaData.ContainsKey(data.level_))
			{
				ClientLog.Instance.LogError("debrisData" + ConfigLoader.Instance.csvext + "ID重复 " + data.level_);
				return;
			}
			metaData[data.level_] = data;
			moreLevelList.Add(data);
		}
		parser.Dispose ();
		parser = null;
	}

	public static MoreLevelData GetData(int level)
	{
		if(!metaData.ContainsKey(level)) 
			return null;
		return metaData[level];
	}

}

