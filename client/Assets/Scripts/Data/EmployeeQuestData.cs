using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class EmployeeQuestData 
{
	public int id_;
	public string name_;
	public int employeeMonster_;
	public int employeeMonster_1;
	public int employeeMonster_2;
	public EmployeeQuestColor employeeQuestColor_;
	public int timeRequier_;
	public int employeeRequier_;
	public int successRate_;
	public int award1_;
	public int award2_;
	public int award3_;
	public int needMoney_;
	public List<int> monsterList = new List<int>();
	static Dictionary<int, EmployeeQuestData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EmployeeQuestData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("EmployeeQuest" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EmployeeQuestData data = null;

		for(int i=0; i < recordCounter; ++i)
		{
			data = new EmployeeQuestData ();
			data.id_ = parser.GetInt (i, "EQID");    
			data.name_ = parser.GetString (i, "EQName");

			string[] monsters = parser.GetString(i, "EmployeeMonster").Split(new string[]{";"}, StringSplitOptions.RemoveEmptyEntries);

			if(monsters.Length == 1)
			{
				data.employeeMonster_ = int.Parse(monsters[0]);
				data.employeeMonster_1 =0;
				data.employeeMonster_2 =0;
			}
			else if(monsters.Length == 2)
			{
				data.employeeMonster_ = int.Parse(monsters[0]);
				data.employeeMonster_1 =int.Parse(monsters[1]);
				data.employeeMonster_2 =0;
			}
			else if(monsters.Length == 3)
			{
				data.employeeMonster_ = int.Parse(monsters[0]);
				data.employeeMonster_1 = int.Parse(monsters[1]);
				data.employeeMonster_2 = int.Parse(monsters[2]);
			}

			data.monsterList.Add(data.employeeMonster_);
			if(data.employeeMonster_1 != 0)
				data.monsterList.Add(data.employeeMonster_1);
			if(data.employeeMonster_2 != 0)
				data.monsterList.Add(data.employeeMonster_2);
			data.employeeQuestColor_ = (EmployeeQuestColor)Enum.Parse(typeof(EmployeeQuestColor), parser.GetString(i, "EmployeeQuestColor"));
			data.timeRequier_ = parser.GetInt (i, "TimeRequier");
			data.employeeRequier_ = parser.GetInt (i, "EmployeeRequier");
			data.successRate_ = parser.GetInt (i, "SuccessRate");
			data.award1_ = parser.GetInt (i, "award1");
			data.award2_ = parser.GetInt (i, "award2");
			data.award3_ = parser.GetInt (i, "award3");
			data.needMoney_= parser.GetInt (i, "cost");
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

	public static EmployeeQuestData GetData(int id)
	{
		if(!metaData.ContainsKey(id)) 
			return null;
		return metaData[id];
	}

}

