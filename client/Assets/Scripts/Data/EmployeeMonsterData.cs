using System;
using System.Collections.Generic;


public class EmployeeMonsterData 
{
	public int id_;
	public string name_;
	public int assteId_;
	public EmployeeSkillType skill_0;
	public EmployeeSkillType skill_1;
	public string icon_;
	public List<int> skills = new List<int>();
 
	static Dictionary<int, EmployeeMonsterData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EmployeeMonsterData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EmployeeQuest" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EmployeeMonsterData data = null;
		
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EmployeeMonsterData ();
			data.id_ = parser.GetInt (i, "ID");    
			data.name_ = parser.GetString (i, "Name");
			data.assteId_ = parser.GetInt (i, "AssetsID");
			data.icon_ = parser.GetString (i, "Icon");
			string[] skill = parser.GetString(i, "Skill").Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries);
			if(skill.Length > 1)
			{
				data.skill_0 = (EmployeeSkillType)Enum.Parse(typeof(EmployeeSkillType), skill[0]);
				data.skill_1 = (EmployeeSkillType)Enum.Parse(typeof(EmployeeSkillType), skill[1]);
			}
			else
			{
				data.skill_0 = (EmployeeSkillType)Enum.Parse(typeof(EmployeeSkillType), skill[0]);
				data.skill_1 = EmployeeSkillType.EKT_Max;
			}
			data.skills.Add((int)data.skill_0);
			data.skills.Add((int)data.skill_1);

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
	
	public static EmployeeMonsterData GetData(int id)
	{
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}


}

