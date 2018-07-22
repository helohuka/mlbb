using System;
using System.Collections;
using System.Collections.Generic;

public class EmployeeQuestSkillData 
{

	public int id_;
	public string name_;
	public Dictionary<EmployeeSkillType,int> SkillTypeArr = new Dictionary<EmployeeSkillType, int> ();

	static Dictionary<int, EmployeeQuestSkillData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EmployeeQuestSkillData> ();
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EmployeeQuestSkillData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EmployeeQuestSkillData data = null;
		
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EmployeeQuestSkillData ();
			data.id_ = parser.GetInt (i, "SkillID");    
			data.name_ = parser.GetString (i, "SkillName");

			data.SkillTypeArr[EmployeeSkillType.EKT_GroupDamage] = parser.GetInt (i, "EKT_GroupDamage");
			data.SkillTypeArr[EmployeeSkillType.EKT_DeadlyDamage] = parser.GetInt (i, "EKT_DeadlyDamage");
			data.SkillTypeArr[EmployeeSkillType.EKT_BattleTimeLimit] = parser.GetInt (i, "EKT_BattleTimeLimit");
			data.SkillTypeArr[EmployeeSkillType.EKT_Thump] = parser.GetInt (i, "EKT_Thump");
			data.SkillTypeArr[EmployeeSkillType.EKT_SiegeDamage] = parser.GetInt (i, "EKT_SiegeDamage");
			data.SkillTypeArr[EmployeeSkillType.EKT_SuperMagic] = parser.GetInt (i, "EKT_SuperMagic");
			data.SkillTypeArr[EmployeeSkillType.EKT_Debuff] = parser.GetInt (i, "EKT_Debuff");
			data.SkillTypeArr[EmployeeSkillType.EKT_PhysicalDefense] = parser.GetInt (i, "EKT_PhysicalDefense");


			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("EmployeeQuestSkillData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
			
		}
		parser.Dispose ();
		parser = null;
		
	}
	
	public static int GetData(int id, EmployeeSkillType type)
	{
		if(!metaData.ContainsKey(id)) 
			return 0;
		if(!metaData[id].SkillTypeArr.ContainsKey(type)) 
			return 0;

		return metaData[id].SkillTypeArr[type];
	}


}

