using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class PlayerData {

	public int id_;

	public int lookID_;

	public int level_;

	public int exp_;

	public int reputation_;

	public int stama_;

	public int strength_;

	public int power_;

	public int speed_;

	public string race_des_;

	public string des_;

	public int magic_;

	public int defaultSceneId_;

	public string Race_;

	public string RaceIcon_;

	public string DefalutSkill_;

	public string [] DefalutSkills_;

	private static Dictionary<int, PlayerData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, PlayerData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("PlayerData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		PlayerData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new PlayerData ();
			data.id_ = parser.GetInt (i, "ID");
			data.level_ = parser.GetInt (i, "PT_Level");
			data.exp_ = parser.GetInt (i, "PT_Exp");
			data.reputation_ = parser.GetInt (i, "PT_Reputation");
			data.stama_ = parser.GetInt (i, "PT_Stama");
			data.strength_ = parser.GetInt (i, "PT_Strength");
			data.power_ = parser.GetInt (i, "PT_Power");
			data.speed_ = parser.GetInt (i, "PT_Speed");
			data.magic_ = parser.GetInt (i, "PT_Magic");
			data.lookID_ = parser.GetInt (i, "PT_AssetId");
			data.defaultSceneId_ = parser.GetInt(i, "DefaultSceneId");
			data.Race_ = parser.GetString(i, "Race");
			data.RaceIcon_ = parser.GetString(i, "RaceIcon");
			data.DefalutSkill_ = parser.GetString(i, "DefalutSkill");
			data.race_des_ = parser.GetString(i, "race_decs");
			data.des_ = parser.GetString(i, "decs");
			data.DefalutSkills_ = data.DefalutSkill_.Split(';');
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("PlayerData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static PlayerData GetData(int id)
	{
		return metaData[id];
	}
	public static Dictionary<int, PlayerData> GetMetaData()
	{
		return metaData;
	}
	public static int GetMetaDataId(int assId)
	{
		foreach(KeyValuePair<int,PlayerData> pai in metaData)
		{
			if(pai.Value.lookID_ == assId)
			{
				return pai.Key;
			}
		
		}
		return -1;
	}

}
