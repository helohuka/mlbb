using System;
using System.Collections.Generic;

public class EquipColorData 
{
	public int id_;
	public  List<KeyValuePair<PropertyType,string[]>>  propArr = new List<KeyValuePair<PropertyType, string[]>>();
	private static Dictionary<int, EquipColorData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, EquipColorData> ();
		CSVParser parser = new CSVParser ();

		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ItemData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		EquipColorData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new EquipColorData ();
			data.id_ = parser.GetInt (i, "EquipLv");


			string propSre = parser.GetString(i, "HpMax");
			string[] propValues;
			KeyValuePair<PropertyType,string[]> dataArr = new KeyValuePair<PropertyType, string[]>();
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "HpMax").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_HpMax,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "MpMax");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "MpMax").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_MpMax,propValues);
				data.propArr.Add(dataArr);
			}
		
			propSre = parser.GetString(i, "Magicattack");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Magicattack").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Magicattack,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Magicdefense");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Magicdefense").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Magicdefense,propValues);
				data.propArr.Add(dataArr);
			}
			

			
			propSre = parser.GetString(i, "Attack");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Attack").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Attack,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Defense");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Defense").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Defense,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Agile");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Agile").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Agile,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Spirit");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Spirit").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Spirit,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Reply");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Reply").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Reply,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Hit");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Hit").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Hit,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Dodge");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Dodge").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Dodge,propValues);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "Crit");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "Crit").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_Crit,propValues);
				data.propArr.Add(dataArr);
			}
			
		
			propSre = parser.GetString(i, "counterpunch");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "counterpunch").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_counterpunch,propValues);
				data.propArr.Add(dataArr);
			}


			propSre = parser.GetString(i, "NoSleep");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoSleep").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoSleep,propValues);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "NoPetrifaction");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoPetrifaction").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoPetrifaction,propValues);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "NoDrunk");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoDrunk").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoDrunk,propValues);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoForget");
			if(!string.IsNullOrEmpty(propSre))
			{
				propValues = parser.GetString(i, "NoForget").Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
				dataArr = new KeyValuePair<PropertyType, string[]>(PropertyType.PT_NoForget,propValues);
				data.propArr.Add(dataArr);
			}

			if(metaData.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("equipColor" + ConfigLoader.Instance.csvext + "ID重复 " + data.id_);
				return;
			}
			metaData[data.id_] = data;

		}

		parser.Dispose ();
		parser = null;
	}


	public static EquipColorData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
		{
			ClientLog.Instance.Log("Equiplevel: " + id + "is not exist!");
			return null;
		}
		return metaData[id];
	}


	public static float GetEquipPerNum(int level,PropertyType type ,float num )
	{
		EquipColorData data = GetData (level);
		if(data == null)
			return -1;
		foreach(var x in data.propArr)
		{
			if(x.Key == type)
			{
				return (((float)num - float.Parse(x.Value[0])) /  (float.Parse(x.Value[1])- float.Parse(x.Value[0])) )*100;
			}
		}


		return -1;
	}
		
}

