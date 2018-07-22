using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CopyData  {
	public int _CopyID;
	public int _StartTaskID;
	public int _EndID;
	public int _SceneID;
	public int _NextSceneID;
	public string _PicName;
	public string[] _Reward;

	private static Dictionary<int, CopyData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, CopyData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
            ClientLog.Instance.LogError("CopyData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		CopyData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new CopyData ();
			data._CopyID = parser.GetInt (i, "CopyID");
			data._StartTaskID = parser.GetInt (i, "StartTaskID");
			data._EndID = parser.GetInt (i, "EndID");
			data._SceneID = parser.GetInt (i, "SceneID");
			data._NextSceneID = parser.GetInt (i, "NextSceneID");
			data._PicName = parser.GetString (i, "picname");
			data._Reward = parser.GetString (i, "reward").Split(';');

            if (metaData.ContainsKey(data._SceneID))
			{
				ClientLog.Instance.LogError("BabyData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
            metaData[data._SceneID] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static CopyData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, CopyData> GetData()
	{

		return metaData;
	}
	public static bool IsCopyScene(int sceneId)
	{
		foreach(KeyValuePair<int, CopyData> Pair in metaData)
		{
			if(sceneId == Pair.Value._SceneID||sceneId == Pair.Value._NextSceneID)
			{
				return true;
			}
		}
		return false;
	}

}
