using System;
using System.Collections.Generic;

public class ChallengeData 
{
	public int id_;
	public int assetsID_;
	public int senceId_;
	public int battleId_;
	public int exp_;
	public string[] reward_;
		
	private static Dictionary<int, ChallengeData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, ChallengeData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ChallengeData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}


		int recordCounter = parser.GetRecordCounter();
		ChallengeData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ChallengeData ();
			data.id_ = parser.GetInt (i, "ID");
			data.assetsID_ = parser.GetInt (i, "AssetID");
			data.senceId_ = parser.GetInt (i, "SenceName");

			data.battleId_ = parser.GetInt (i, "BattleID");
			data.exp_ = parser.GetInt (i, "Exp");

			data.reward_ = parser.GetString(i, "Reward").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);


			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("ChallengeData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;

	}

	public static ChallengeData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}

	public static Dictionary<int, ChallengeData> GetData()
	{
		
		return metaData;
	}


	public static int getHundredNum(int id)
	{
		foreach(ChallengeData c in metaData.Values)
		{
			if(c.senceId_ == id)
				return c.id_;
		}


		return 0;
	}



}

