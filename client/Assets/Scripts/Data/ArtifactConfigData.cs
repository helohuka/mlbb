using System;
using System.Collections;
using System.Collections.Generic;

public class ArtifactConfigData 
{
	public int _Number;
	public int _Diamonds;
	public int _ItemId_1;
	public int _ItemId_2;
	public int _ItemId_3;
	public int _ItemId_4;
	public int _ItemId_5;

	public int _ItemNum_1;
	public int _ItemNum_2;
	public int _ItemNum_3;
	public int _ItemNum_4;
	public int _ItemNum_5;

	public JobType _ProfessionType;


	private static Dictionary<int, List<ArtifactConfigData>> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, List<ArtifactConfigData>> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("ArtifactLevelData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}

		int recordCounter = parser.GetRecordCounter();
		ArtifactConfigData data = null;
		for(int i=0; i < recordCounter; ++i)
		{

			data = new ArtifactConfigData();
			data._Number = parser.GetInt (i, "Lv");
			string profession = parser.GetString(i, "JobType");
			if(!string.IsNullOrEmpty(profession))
				data._ProfessionType = (JobType)Enum.Parse(typeof(JobType), profession);

			data._Diamonds = parser.GetInt (i, "Diamonds");
			data._ItemId_1 = parser.GetInt (i, "Item_1");
			//data._ItemId_2 = parser.GetInt (i, "Item_2");
			//data._ItemId_3 = parser.GetInt (i, "Item_3");
			//data._ItemId_4 = parser.GetInt (i, "Item_4");
			//data._ItemId_5 = parser.GetInt (i, "Item_5");
			data._ItemNum_1 = parser.GetInt (i, "Item_1Num");
			//data._ItemNum_2 = parser.GetInt (i, "Item_2Num");
			//data._ItemNum_3 = parser.GetInt (i, "Item_3Num");
			//data._ItemNum_4 = parser.GetInt (i, "Item_4Num");
			//data._ItemNum_5 = parser.GetInt (i, "Item_5Num");

			if(metaData.ContainsKey(data._Number))
			{
				metaData[data._Number].Add(data);

			}
			else
			{
				List<ArtifactConfigData> dataList = new List<ArtifactConfigData>();
				dataList.Add(data);
				metaData[data._Number] = dataList;
			}
			
		}

		parser.Dispose ();
		parser = null;
	}


	public static ArtifactConfigData GetData(int lv,int job)
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

