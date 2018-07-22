using System;
using System.Collections.Generic;

public class SkillData {
	
	// 技能id
	public int _Id;
	
	// 技能名称
	public string _Name;
	
	// 技能描述
	public string _Desc;
	
	// 技能熟练度
	public int _Proficiency;
	
	// 技能等级
	public int _Level;

	// 技能类型
	public SkillType _SkillType;

	// 影响范围
	public string _SelectedTarget;

	// 伤害表达式
	public string _Expression;

	// 作用属性
	public string[] _EffectAttr;

	// 释放特效id
	public int _Cast_effectID;

	// 特效资源id
	public int _EffectID;

	// 受击特效资源id
	public int _Beattack_effectID;

	// 是否物理
	public bool _IsPhysic;

	// 是否近战
	public bool _IsMelee;

	// 消耗魔法
	public int _Cost_mana;

	// 适用目标
	public SkillTargetType _Target_type;

	// 被动技能类型
	public PassiveType _Passive_type;

    // 技能名称特效
    public int _SingEffectId;

	public int _LearnGroup;

	public string _SkillBack;

	public string _SkillIconTex;

	// 所有可触发的状态受击特效ID
	public int[] _StateIds;

	public string _ResIconName;

	public string _ResTextName;

	public int _LearnQuestID;
	public int _LearnCoin;

	public int _UpdateItem;
	public int _NextId;
	public int _LearnLv;
	
	public static Dictionary<int, SkillData[] > metaData;
	
	public static void ParseData(string content, string fileName)
	{
        metaData = new Dictionary<int, SkillData[]>();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("SkillData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		SkillData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new SkillData ();
			data._Id = parser.GetInt (i, "ID");
			data._Name = parser.GetString (i, "SkillName");
			data._Desc = parser.GetString (i, "SkillDesc");
			data._Proficiency = parser.GetInt (i, "Pr");
			data._Level = parser.GetInt (i, "Lv");
			//data.nextId_ = parser.GetInt (i, "NextId");
			data._SkillType = (SkillType)Enum.Parse(typeof(SkillType), parser.GetString(i, "SkillType"));
			data._SelectedTarget = parser.GetString (i, "SelectedTarget");
			data._Cast_effectID = parser.GetInt(i, "CastEffectAssetID");
			data._EffectID = parser.GetInt(i, "EffectAssetID");
			data._Beattack_effectID = parser.GetInt(i, "BeAttackAssetID");
			data._IsPhysic = parser.GetBool(i, "isPhysic");
			data._IsMelee = parser.GetBool(i, "isMelee");
			data._LearnGroup = parser.GetInt(i,"LearnGroup");
			data._Cost_mana = parser.GetInt(i, "cost");
			data._Target_type = (SkillTargetType)Enum.Parse(typeof(SkillTargetType), parser.GetString(i, "SkillTargetType"));
			data._Passive_type = (PassiveType)Enum.Parse(typeof(PassiveType), parser.GetString(i, "PassiveName"));
			string[] stateids = parser.GetString(i, "StateIDs").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
            data._StateIds = new int[stateids.Length];
            for(int j=0; j < stateids.Length; ++j)
            {
				data._StateIds[j] = (int)((StateType)Enum.Parse(typeof(StateType), stateids[j]));
            }
			data._ResIconName = parser.GetString(i, "resIconName");
			data._ResTextName = parser.GetString(i, "resTextName");
			data._LearnQuestID = parser.GetInt (i, "LearnQuestID");
			data._LearnCoin = parser.GetInt(i,"LearnCoin");
            data._UpdateItem = parser.GetInt(i, "LevelUpNeedItem");
			data._LearnLv = parser.GetInt(i, "LearnLv");
            data._SingEffectId = parser.GetInt(i, "NameEffectID");

			data._SkillBack = parser.GetString(i, "SkillBack");
			data._SkillIconTex = parser.GetString(i, "SkillIconTex");

            //if(metaData.ContainsKey(data.id_))
            //{
            //    ClientLog.Instance.LogError("SkillData" + ConfigLoader.Instance.csvext + "ID重复");
            //    return;
            //}

			if (metaData.ContainsKey(data._Id) == false)
            {
				metaData[data._Id] = new SkillData[21];
            }

			metaData[data._Id][data._Level] = data;
		}
		parser.Dispose ();
		parser = null;
	}

	List<int> GetRow(int aimPos)
	{
		int min = 0;
		int max = 0;
		List<int> lst = new List<int> ();
		if(aimPos >= (int)BattlePosition.BP_Down0 && aimPos <= (int)BattlePosition.BP_Down4)
		{
			min = (int)BattlePosition.BP_Down0;
			max = (int)BattlePosition.BP_Down5 + 1;
		}
		else if(aimPos >= (int)BattlePosition.BP_Down5 && aimPos <= (int)BattlePosition.BP_Down9)
		{
			min = (int)BattlePosition.BP_Down5;
			max = (int)BattlePosition.BP_Down9 + 1;
		}
		else if(aimPos >= (int)BattlePosition.BP_Up0 && aimPos <= (int)BattlePosition.BP_Up4)
		{
			min = (int)BattlePosition.BP_Up0;
			max = (int)BattlePosition.BP_Up4 + 1;
		}
		else if(aimPos >= (int)BattlePosition.BP_Up5 && aimPos <= (int)BattlePosition.BP_Up9)
		{
			min = (int)BattlePosition.BP_Up5;
			max = (int)BattlePosition.BP_Up9 + 1;
		}
		for(int i=min; i < max; ++i)
		{
			if(i != 0)
				lst.Add(i);
		}
		return lst;
	}

	List<int> GetCol(int aimPos)
	{
		int other = 0;
		List<int> lst = new List<int> ();
		if(aimPos >= (int)BattlePosition.BP_Down0 && aimPos <= (int)BattlePosition.BP_Down4)
		{
			other = aimPos + 5;
		}
		else if(aimPos >= (int)BattlePosition.BP_Down5 && aimPos <= (int)BattlePosition.BP_Down9)
		{
			other = aimPos - 5;
		}
		else if(aimPos >= (int)BattlePosition.BP_Up0 && aimPos <= (int)BattlePosition.BP_Up4)
		{
			other = aimPos + 5;
		}
		else if(aimPos >= (int)BattlePosition.BP_Up5 && aimPos <= (int)BattlePosition.BP_Up9)
		{
			other = aimPos - 5;
		}
		if(other != 0)
			lst.Add (other);
		lst.Add (aimPos);
		return lst;
	}

	List<int> GetCross(int aimPos)
	{
		int left = 0;
		int right = 0;
		int plus = 0;
		List<int> lst = new List<int> ();
		if(aimPos >= (int)BattlePosition.BP_Down0 && aimPos <= (int)BattlePosition.BP_Down4)
		{
			if(aimPos == (int)BattlePosition.BP_Down0)
			{
				right = aimPos + 1;
			}
			else if(aimPos == (int)BattlePosition.BP_Down4)
			{
				left = aimPos - 1;
			}
			else
			{
				right = aimPos + 1;
				left = aimPos - 1;
			}
			plus = aimPos + 5;
		}
		else if(aimPos >= (int)BattlePosition.BP_Down5 && aimPos <= (int)BattlePosition.BP_Down9)
		{
			if(aimPos == (int)BattlePosition.BP_Down5)
			{
				right = aimPos + (int)BattlePosition.BP_Down0;
			}
			else if(aimPos == (int)BattlePosition.BP_Down9)
			{
				left = aimPos - (int)BattlePosition.BP_Down0;
			}
			else
			{
				right = aimPos + 1;
				left = aimPos - 1;
			}
			plus = aimPos - 5;
		}
		else if(aimPos >= (int)BattlePosition.BP_Up0 && aimPos <= (int)BattlePosition.BP_Up4)
		{
			if(aimPos == (int)BattlePosition.BP_Up0)
			{
				right = aimPos + 1;
			}
			else if(aimPos == (int)BattlePosition.BP_Up4)
			{
				left = aimPos - 1;
			}
			else
			{
				right = aimPos + 1;
				left = aimPos - 1;
			}
			plus = aimPos + 5;
		}
		else if(aimPos >= (int)BattlePosition.BP_Up5 && aimPos <= (int)BattlePosition.BP_Up9)
		{
			if(aimPos == (int)BattlePosition.BP_Up5)
			{
				right = aimPos + 1;
			}
			else if(aimPos == (int)BattlePosition.BP_Up9)
			{
				left = aimPos - 1;
			}
			else
			{
				right = aimPos + 1;
				left = aimPos - 1;
			}
			plus = aimPos - 5;
		}
		if(left != 0)
			lst.Add (left);
		if(right != 0)
			lst.Add (right);
		if(plus != 0)
			lst.Add (plus);
		lst.Add (aimPos);
		return lst;
	}

	List<int> GetAll(int aimPos)
	{
		List<int> lst = new List<int> ();
		if(aimPos > (int)BattlePosition.BP_Down9)
		{
			for(int i=(int)BattlePosition.BP_Up0; i <= (int)BattlePosition.BP_Up9; ++i)
			{
				lst.Add(i);
			}
		}
		else
		{
			for(int i=(int)BattlePosition.BP_Down0; i <= (int)BattlePosition.BP_Down9; ++i)
			{
				lst.Add(i);
			}
		}
		return lst;
	}
	
	public static SkillData GetData(int id, int lev)
	{
        //ClientLog.Instance.Log("Skill ID: " + id + " LEV:" + lev);
		if(!metaData.ContainsKey(id))
		{
			return null;
		}
        if (lev >= metaData[id].Length)
            return null;
        else if (lev < 0)
            return null;
        if (metaData[id][lev] == null)
        {
            SkillData[] skills = metaData[id];
            if(skills == null)
                return null;
            for (int i = 0; i < skills.Length; ++i)
            {
                if(skills[i] != null)
                    return skills[i];
            }
            return null;
        }
		return metaData[id][lev];
	}

    public static SkillData GetMinxiLevelData(int id)
    {
        if (!metaData.ContainsKey(id))
        {
            return null;
        }
        for (int i = 0; i < metaData[id].Length; ++i)
        {
            if (metaData[id][i] != null)
                return metaData[id][i];
        }
        return null;
    }
	public static Dictionary<int, SkillData[] > GetAllData()
	{
		return metaData;
	}

	public static SkillData GetMinxiLevelData(SkillData[] rhs){
		for (int i = 0; i < rhs.Length; ++i)
		{
			if (rhs[i] != null)
				return rhs[i];
		}
		return null;
	}

	public static SkillData[] GetOneSkillList(int id)
	{
		if (metaData[id] != null)
			return metaData[id];
	
		return null;
	}
}
