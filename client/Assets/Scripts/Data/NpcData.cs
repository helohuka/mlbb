using UnityEngine;
using System.Collections.Generic;
using System;

public class NpcData {

	public int NpcId;
	
	public int AssetsID;

	public string  NpcName;

	public int NpcTalk;

	public string Name;

	public int[] Quests;

    public NpcType Type;

	public UIASSETS_ID AssetsId;
	//public string talk_;

	public string BabySkillLearn;

	public string Transfer;

    public int OpenLv;

    public int[] NeedQuests;

    public float PosX, PosY , PosZ, RotY;

	private static Dictionary<int, NpcData> MetaData;
	
	public static void ParseData(string content, string fileName)
	{
		MetaData = new Dictionary<int, NpcData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("NpcData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		NpcData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new NpcData ();
			data.NpcId = parser.GetInt (i, "NpcId");
			data.AssetsID = parser.GetInt (i, "AssetID");
            data.NpcTalk = parser.GetInt (i, "NpcTalk");
            data.PosX = parser.GetFloat(i, "PosX");
            data.PosY = parser.GetFloat(i, "PosY");
            data.PosZ = parser.GetFloat(i, "PosZ");
            data.RotY = parser.GetFloat(i, "RotY");
			data.Name = parser.GetString (i, "NpcName");
            string npcTypeStr = parser.GetString(i, "NpcType");
            if(!string.IsNullOrEmpty(npcTypeStr))
                data.Type = (NpcType)Enum.Parse(typeof(NpcType), npcTypeStr);
            string[] queststr = parser.GetString(i, "NpcQuest").Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            data.Quests = new int[queststr.Length];
            for (int j = 0; j < queststr.Length; ++j)
            {
                data.Quests[j] = int.Parse(queststr[j]);
            }
			//data.talk_ = parser.GetString (i, "npc_talk");

			data.BabySkillLearn = parser.GetString (i, "BabySkillLearn");

            string[] needquestids = parser.GetString(i, "QuestID").Split(new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
            data.NeedQuests = new int[needquestids.Length];
            for(int s=0; s<needquestids.Length; ++s)
            {
                data.NeedQuests[s] = int.Parse(needquestids[s]);
            }
            data.OpenLv = parser.GetInt(i, "OpenLv");

			data.Transfer = parser.GetString(i, "Transfer");

			if(parser.GetString(i, "UIAssetsId") != "")
			{
                data.AssetsId = (UIASSETS_ID)Enum.Parse(typeof(UIASSETS_ID), parser.GetString(i, "UIAssetsId"));
			}

			if(MetaData.ContainsKey(data.NpcId))
			{
				ClientLog.Instance.LogError("NpcData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			MetaData[data.NpcId] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static NpcData GetData(int id)
	{
		if(!MetaData.ContainsKey(id))
			return null;
		return MetaData[id];
	}
	public static Dictionary<int, NpcData> GetData()
	{

		return MetaData;
	}
}
