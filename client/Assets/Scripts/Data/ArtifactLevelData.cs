using System;
using System.Collections;
using System.Collections.Generic;

public class ArtifactLevelData 
{
	public int _Id;
	public int  _Exp;
	public int  _ItemId;
	public string _Icon;
	public JobType _ProfessionType;

	private static Dictionary<int, List<ArtifactLevelData>> metaData;
	public  List<KeyValuePair<PropertyType,string>>  propArr = new List<KeyValuePair<PropertyType, string>>(); 

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, List<ArtifactLevelData>> ();

		CSVParser parser = new CSVParser ();
		if(!parser.Parse(content))
		{
			ClientLog.Instance.LogError("ArtifactLevelData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		int recordCounter = parser.GetRecordCounter();
		ArtifactLevelData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new ArtifactLevelData();
			data._Id = parser.GetInt (i, "Lv");
			data._ItemId = parser.GetInt (i, "Item");
			data._Exp = parser.GetInt (i, "Exp");
			data._Icon = parser.GetString (i, "Icon");
			string profession = parser.GetString(i, "JobType");
			if(!string.IsNullOrEmpty(profession))
				data._ProfessionType = (JobType)Enum.Parse(typeof(JobType), profession);

			//  prop 
			string propSre = parser.GetString(i, "HpMax");
			KeyValuePair<PropertyType,string> dataArr = new KeyValuePair<PropertyType, string>();
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_HpMax,propSre);
				data.propArr.Add(dataArr);
			}
			
			propSre = parser.GetString(i, "MpMax");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_MpMax,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "HpCurr");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_HpCurr,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "MpCurr");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_MpCurr,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "Attack");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Attack,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "Defense");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Defense,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "Agile");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Agile,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Spirit");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Spirit,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Reply");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Reply,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Hit");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Hit,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "Dodge");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Dodge,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Crit");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Crit,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "counterpunch");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_counterpunch,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Magicattack");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Magicattack,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "Magicdefense");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_Magicdefense,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoSleep");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_NoSleep,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoPetrifaction");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_NoPetrifaction,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoDrunk");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_NoDrunk,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "NoChaos");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_NoChaos,propSre);
				data.propArr.Add(dataArr);
			}

			propSre = parser.GetString(i, "NoForget");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_NoForget,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "PT_SneakAttack");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_SneakAttack,propSre);
				data.propArr.Add(dataArr);
			}
			propSre = parser.GetString(i, "Poison");
			if(!string.IsNullOrEmpty(propSre))
			{
				dataArr = new KeyValuePair<PropertyType, string>(PropertyType.PT_NoPoison,propSre);
				data.propArr.Add(dataArr);
			}

			
			if(metaData.ContainsKey(data._Id))
			{
				metaData[data._Id].Add(data);
				
			}
			else
			{
				List<ArtifactLevelData> dataList = new List<ArtifactLevelData>();
				dataList.Add(data);
				metaData[data._Id] = dataList;
			}

		}

		parser.Dispose ();
		parser = null;
	}


	public static ArtifactLevelData GetData(int lv,int job)
	{
		if(!metaData.ContainsKey(lv))
		{
			ClientLog.Instance.Log("magic ID: " + lv + "is not exist!");
			return null;
		}

		for(int i=0;i< metaData[lv].Count;i++)
		{
			if(metaData[lv][i]._ProfessionType == (JobType)job)
			{
				return metaData[lv][i];
			}
		}

		return null;
	}



}

