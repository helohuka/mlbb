using UnityEngine;
using System;
using System.Collections.Generic;

public class BabyData {

	public int _Id;
	
	public int _AssetsID;

	public string _Name;

	public string _Desc;

	public string _ResIcon;

	public int _BIG_Stama;
	public int _BIG_Strength;
	public int _BIG_Power;
	public int _BIG_Speed;
	public int _BIG_Magic;

	public int _Tpye;
    public bool _IsBoss;
	public int _Fire;
	public int _Water;
	public int _Wind;
	public int _Ground;

	public int _PT_Hp;
	public int _PT_Mp;
	public int _PT_Attack;
	public int _PT_Defense;
	public int _PT_Agile;

	public string _RaceIcon;
	public int _SkillNum;
	public int _Pet;
	public int _PetProbability;
	public RaceType _RaceType;
	public string _Location;

	public int _ReformMonster;
	public string _ReformItem;
	public PetQuality _PetQuality;
    public List<COM_Skill> skills_ = new List<COM_Skill>();
    
	public static int babyReId;
	public static int intensifyLevel;
	private static Dictionary<int, BabyData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, BabyData> ();

		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("BabyData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}

		int recordCounter = parser.GetRecordCounter();
		BabyData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new BabyData ();
			data._Id = parser.GetInt (i, "ID");
			data._AssetsID = parser.GetInt (i, "AssetsID");
			data._Name = parser.GetString (i, "Name");
			data._Desc = parser.GetString (i, "Desc");
			data._ResIcon = parser.GetString (i, "Icon");
			data._BIG_Strength = parser.GetInt (i, "BIG_Strength");
			data._BIG_Magic = parser.GetInt (i, "BIG_Magic");
			data._BIG_Power = parser.GetInt (i, "BIG_Power");
			data._BIG_Speed = parser.GetInt (i, "BIG_Speed");
			data._BIG_Stama = parser.GetInt (i, "BIG_Stama");
			data._RaceIcon = parser.GetString (i, "RaceIcon");
			data._SkillNum = parser.GetInt (i, "SkillNum");
			data._Fire = parser.GetInt (i, "Fire");
			data._Water = parser.GetInt (i, "Water");
			data._Wind = parser.GetInt (i, "Wind");
			data._Ground = parser.GetInt (i, "Ground");
			data._Tpye = parser.GetInt (i, "Type");
            data._IsBoss = (parser.GetInt(i, "TwiceAction") == 2);
			data._Pet = parser.GetInt (i, "Pet");
			data._PetProbability = parser.GetInt (i, "PetProbability");
			data._Location = parser.GetString (i, "Location");
			data._ReformMonster = parser.GetInt (i, "ReformMonster");
			data._ReformItem = parser.GetString (i, "ReformItem");
			data._PT_Agile = parser.GetInt (i, "PT_Agile");
			data._PT_Attack = parser.GetInt (i, "PT_Attack");
			data._PT_Defense = parser.GetInt (i, "PT_Defense");
			data._PT_Mp = parser.GetInt (i, "PT_Mp");
			data._PT_Hp = parser.GetInt (i, "PT_Hp");
			data._RaceType = (RaceType)Enum.Parse(typeof(RaceType), parser.GetString(i, "Race"));
			data._PetQuality = (PetQuality)Enum.Parse(typeof(PetQuality), parser.GetString(i, "PetQuality"));
            string skill = parser.GetString(i, "DefalutSkill");
            string[] skill1 = skill.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            COM_Skill skillInst = null;
            for (int j = 0; j < skill1.Length; ++j)
            {
                string[] skill2 = skill1[j].Split(':');
                skillInst = new COM_Skill();
                skillInst.skillID_ = uint.Parse(skill2[0]);
                skillInst.skillLevel_ = uint.Parse(skill2[1]);
                data.skills_.Add(skillInst);
            }
			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("BabyData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;
		}
		parser.Dispose ();
		parser = null;
	}

	public static BabyData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}

    public static string GetRaceIconByType(RaceType raceType)
    {
        foreach (BabyData data in metaData.Values)
        {
            if (data._RaceType == raceType)
                return data._RaceIcon;
        }
        return "";
    }

	public static Dictionary<int, BabyData> GetData()
	{

		return metaData;
	}
	static List<int> Attributes = new List<int> ();
	public static int StrongestAttribute(int babyId)
	{
		Attributes.Clear ();
		BabyData bdata = BabyData.GetData (babyId);

		Attributes.Add (bdata._BIG_Stama);
		Attributes.Add (bdata._BIG_Strength);
		Attributes.Add (bdata._BIG_Power);
		Attributes.Add (bdata._BIG_Speed);
		Attributes.Add (bdata._BIG_Magic);
		int max = Attributes[0];
		int index = 0;
		for(int i =0;i<Attributes.Count;i++)
		{
			if(max <Attributes[i])
			{
				max = Attributes[i];
				index = i;
			}
		}
		return index;
	}

	public static string GetPetQuality(PetQuality Quality)
	{
		if(Quality == PetQuality.PE_Blue)
		{
			return "cw_chongwutouxiang3";
		}else
			if(Quality == PetQuality.PE_Golden)
		{
			return "cw_chongwutouxiang5";
		}else
			if(Quality == PetQuality.PE_Green)
		{
			return "cw_chongwutouxiang2";
		}
		else
			if(Quality == PetQuality.PE_Orange)
		{
			return "cw_chongwutouxiang6";
		}
		else
			if(Quality == PetQuality.PE_Pink)
		{
			return "cw_chongwutouxiang7";
		}
		else
			if(Quality == PetQuality.PE_Purple)
		{
			return "cw_chongwutouxiang4";
		}
		else
			if(Quality == PetQuality.PE_White)
		{
			return "cw_chongwutouxiang1";
		}
		return "cw_chongwutouxiang1";
	}
	public static string GetBabyLeveSp(int num)
	{
		if(num<=1 && num>=0)
		{
			return "S";
		}else
			if(num<=6 && num>1)
		{
			return "A";
		}
		else
			if(num<=13 && num>6)
		{
			return "B";
		}else
			if(num<=20 && num>13)
		{
			return "C";
		}


		return "";
	}
	static uint UpdateUsebTimeout = 5;
	static float  CurrentUseTimeout = 0.0F;
	public static void UpdateUsetime()
	{
		CurrentUseTimeout += Time.deltaTime;
		if (CurrentUseTimeout > UpdateUsebTimeout)
		{
			CurrentUseTimeout -= UpdateUsebTimeout;
			UpdateUsetime(UpdateUsebTimeout);
		}
	}
	public static void UpdateUsetime(uint dt)
	{
		if (GamePlayer.Instance.babies_list_.Count == 0)
			return;
		for (int i = 0; i <GamePlayer.Instance.babies_list_.Count; ++i)
		{
			if (null == GamePlayer.Instance.babies_list_[i])
				continue;

			if (GamePlayer.Instance.babies_list_[i].lastSellTime_ > 0 )
			{
				GamePlayer.Instance.babies_list_[i].lastSellTime_ -= (int)dt;
			}
		}
	}
}
