using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class familyMonsterData
{
	public int _Id;
	public int _Level;
	public int _LevelExp;
	public int _AssetsID;
	public string _Name;
	public string _Desc;
	public string _Icon;

	public int _Fire;
	public int _Water;
	public int _Wind;
	public int _Ground;

	public int _PT_Attack;
	public int _PT_Defense;
	public int _PT_Agile;
	public int _PT_Spirit;
	public int _PT_Reply;

	public List<string[]> _Skills = new List<string[]>();
	public List<string> myNPCId = new List<string>();
	public List<string> otherNPCId = new List<string>();
	private static Dictionary<int, List<familyMonsterData>> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, List<familyMonsterData>>();
		CSVParser parser = new CSVParser();
		if (!parser.Parse(content))
		{
			ClientLog.Instance.LogError("familyMonsterData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		familyMonsterData data = null;
		for (int i = 0; i < recordCounter; ++i)
		{
			data = new familyMonsterData();
			data._Id = parser.GetInt (i, "ID");
			data._Level = parser.GetInt (i, "Level");
			data._LevelExp = parser.GetInt (i, "LevelExp");
			data._AssetsID = parser.GetInt (i, "AssetsID");
			data._Name = parser.GetString (i, "Name");
			data._Desc = parser.GetString (i, "Desc");
			data._Icon = parser.GetString (i, "Icon");
			data._Fire = parser.GetInt (i, "Fire");
			data._Water = parser.GetInt (i, "Water");
			data._Wind = parser.GetInt (i, "Wind");
			data._Ground = parser.GetInt (i, "Ground");
			string[]  skill = parser.GetString(i, "DefalutSkill").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
			for(int j = 0;j<skill.Length;j++)
			{
				string[] s = skill[j].Split(':');
				data._Skills.Add(s);
			}
			string[] npcId = parser.GetString(i, "NpcID").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
			string[] mynpc = npcId[0].Split(',');
			string[] othernpc = npcId[1].Split(',');
			data.myNPCId.AddRange(mynpc);
			data.otherNPCId.AddRange(othernpc);
			data._PT_Agile = parser.GetInt (i, "PT_Agile");
			data._PT_Attack = parser.GetInt (i, "PT_Attack");
			data._PT_Defense = parser.GetInt (i, "PT_Defense");
			data._PT_Spirit = parser.GetInt (i, "PT_Spirit");
			data._PT_Reply = parser.GetInt (i, "PT_Reply");

			if (!metaData.ContainsKey(data._Id))
			{
				metaData.Add(data._Id, new List<familyMonsterData>());
			}
			metaData[data._Id].Add(data);
		}
		parser.Dispose();
		parser = null;
	}

	public static familyMonsterData GetData(int id, int lv)
	{
		if (!metaData.ContainsKey(id))
			return null;
		
		for (int i = 0; i < metaData[id].Count; ++i)
		{
			if(metaData[id][i]._Level == lv)
				return metaData[id][i];
		}
		return null;
	}

	public static int GetNpcId(int nId)
	{
		if(metaData[1][1].myNPCId.Contains(nId.ToString()))
		    return 1;
		if(metaData[1][1].otherNPCId.Contains(nId.ToString()))
			return 2;

		if(metaData[2][1].myNPCId.Contains(nId.ToString()))
			return 1;
		if(metaData[2][1].otherNPCId.Contains(nId.ToString()))
			return 2;

		if(metaData[3][1].myNPCId.Contains(nId.ToString()))
			return 1;
		if(metaData[3][1].otherNPCId.Contains(nId.ToString()))
			return 2;

		if(metaData[4][1].myNPCId.Contains(nId.ToString()))
			return 1;
		if(metaData[4][1].otherNPCId.Contains(nId.ToString()))
			return 2;
		 
		return 0;
	}

    //左true;右false
    public static bool isEnemyGuildMonster(bool flag, int npcid)
    {
        bool yes = false;
        if (flag)
        {
            foreach (List<familyMonsterData> datas in metaData.Values)
            {
                //所有id相同的 等级不同的守护兽npcid一样 所以只判第一个
                if(datas.Count > 0)
                    yes = datas[0].otherNPCId.Contains(npcid.ToString());
                if (yes)
                    return true;
            }
            return false;
        }
        else
        {
            foreach (List<familyMonsterData> datas in metaData.Values)
            {
                if (datas.Count > 0)
                    yes = datas[0].myNPCId.Contains(npcid.ToString());
                if (yes)
                    return true;
            }
            return false;
        }
    }

}

