using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeMonsterSkillData 
{

	public int id_;
	public string name_;
	public string icon;
	public string desc;
	static Dictionary<int, EmployeeMonsterSkillData> metaData;
	
	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EmployeeMonsterSkillData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EmployeeQuest" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EmployeeMonsterSkillData data = null;
		
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EmployeeMonsterSkillData ();
			data.id_ = parser.GetInt (i, "SkillID");    
			data.name_ = parser.GetString (i, "SkillName");
			data.icon = parser.GetString (i, "SkillIcon");
			data.desc = parser.GetString (i, "SkillTips");
			
			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("EmployeeSkillData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data.id_] = data;
			
		}
		parser.Dispose ();
		parser = null;
		
	}
	
	public static EmployeeMonsterSkillData GetData(int id)
	{
		//if (id == (int)EmployeeSkillType.EKT_Max)
				//return null;
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}


}

