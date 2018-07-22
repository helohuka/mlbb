using UnityEngine;
using System;
using System.Collections.Generic;

public class EmployeeData {
	
	public int id_;
		
	public string name_;
	
	public string desc_;
	
	public string resIcon_;

	public int asset_id;

	public JobType professionType_;

	public int jobLevel_;

	public QualityColor quality_;

	public int stama_ ;
	public int strength_;
	public int power_ ;
	public int speed_;
	public int magic_;
	public int hp_;
	public int mp_;
	public int attack_;
	public int defense_;
	public int agile_;



	public  List<KeyValuePair<int,int>>  advancedList1 = new List<KeyValuePair<int, int>> (); 
	public  List<KeyValuePair<int,int>>  advancedList2 = new List<KeyValuePair<int, int>> ();
	public  List<KeyValuePair<int,int>>  advancedList3 = new List<KeyValuePair<int, int>> ();
	public  List<KeyValuePair<int,int>>  advancedList4 = new List<KeyValuePair<int, int>> ();
	public  List<KeyValuePair<int,int>>  advancedList5 = new List<KeyValuePair<int, int>> ();


    public List<COM_Skill> skills_ = new List<COM_Skill>();

	public string[] evolutionNum;

	public string[] skillLevelNeedNum;

	public static Dictionary<int, EmployeeData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EmployeeData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EmployeeData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EmployeeData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EmployeeData ();
			data.id_ = parser.GetInt (i, "ID");
			data.name_ = parser.GetString (i, "Name");
			data.asset_id = parser.GetInt (i, "AssetID");
			data.desc_ = parser.GetString(i,"Decs");
			data.jobLevel_ = parser.GetInt (i, "JobLv");
			data.stama_ = parser.GetInt(i,"PT_Stama");
			data.strength_ = parser.GetInt(i,"PT_Strength");
			data.power_ = parser.GetInt(i,"PT_Power");
			data.speed_ = parser.GetInt(i,"PT_Speed");
			data.magic_ = parser.GetInt(i,"PT_Magic");
			data.hp_ = parser.GetInt(i,"PT_Hp");
			data.mp_ = parser.GetInt(i,"PT_Mp");
			data.attack_ = parser.GetInt(i,"PT_Attack");
			data.defense_ = parser.GetInt(i,"PT_Defense");
			data.agile_ = parser.GetInt(i,"PT_Agile");

			data.quality_ = (QualityColor)Enum.Parse(typeof(QualityColor), parser.GetString(i, "Quality"));
			string profession = parser.GetString(i, "JobType");
			if(!string.IsNullOrEmpty(profession))
				data.professionType_ = (JobType)Enum.Parse(typeof(JobType), profession);

		
			data.evolutionNum = parser.GetString(i, "EvolutionNum").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
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

			data.skillLevelNeedNum = parser.GetString(i, "Skill_levelup").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);

			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("EmployeeData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static EmployeeData GetData(int id)
	{
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}

}