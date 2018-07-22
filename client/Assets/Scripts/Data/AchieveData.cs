using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class AchieveData {
	public int _Id;
	
	public int _AssetsID;
	
	public string _Name;
	
	public string _Desc;

	public int _Num;

	public int _DropId;

	public string _AtName;
	
	public CategoryType _Category;

	public AchievementType _AchieveType;

	public static Dictionary<int, AchieveData> metaData;

	public static void ParseData(string content, string fileName)
	{
		metaData = new Dictionary<int, AchieveData> ();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("AchieveData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		AchieveData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new AchieveData ();
			data._Id = parser.GetInt (i, "ID");
			data._Desc = parser.GetString (i, "desc");
			data._Num = parser.GetInt (i, "target");
			data._DropId = parser.GetInt(i, "DropID");
			data._AtName = parser.GetString (i, "AtName");
			data._Category = (CategoryType)Enum.Parse(typeof(CategoryType), parser.GetString(i, "Category"));
			data._AchieveType = (AchievementType)Enum.Parse(typeof(AchievementType), parser.GetString(i, "type"));
			if(metaData.ContainsKey(data._Id))
			{
				ClientLog.Instance.LogError("AchieveData" + ConfigLoader.Instance.csvext + "ID重复");
				return;
			}
			metaData[data._Id] = data;

            SuccessSystem.SetData(data);
		}
		parser.Dispose ();
		parser = null;

		//ParseBase ();
	}

	static Dictionary<AchievementType, List<int>> _achievementType =new Dictionary<AchievementType, List<int>>();

    static List<AchieveData> displayList = new List<AchieveData>();
	public static List<AchieveData> GetDisplayList()
	{
		foreach(List<int> list in _achievementType.Values)
		{
			if(list.Count > 0)
			{
				if(metaData[list[0]]._AchieveType == AchievementType.AT_CatchPet)
				{
					for(int i =0;i<list.Count;i++)
					{
						displayList.Add(metaData[list[i]]);
					}
				}else
				{
					displayList.Add(metaData[list[0]]);
				}

			}
				
		}
		return displayList;
	}

	public static List<AchieveData> GetAchievementList(List<COM_Achievement> Achievements)
	{

		List<AchieveData> AchievementList = new List<AchieveData> ();
		for(int i = 0;i<Achievements.Count;i++)
		{
			if(GetData((int)Achievements[i].achId_)!= null)
			{
				AchievementList.Add(GetData((int)Achievements[i].achId_));
			}
		}
		return AchievementList;
	}


	static void ParseBase()
	{
		foreach(int id in metaData.Keys)
		{
			if(!_achievementType.ContainsKey(metaData[id]._AchieveType))
				_achievementType[metaData[id]._AchieveType] = new List<int>();
			_achievementType[metaData[id]._AchieveType].Add(id);
		}
	}
	public static List<int> FinishList = new List<int> ();
	public static void ForDisplay(COM_Achievement[] achievements)
	{

		for(int i=0; i < achievements.Length; ++i)
		{
			if(_achievementType[metaData[(int)achievements[i].achId_]._AchieveType].Count > 0)
			{
				if(_achievementType[metaData[(int)achievements[i].achId_]._AchieveType][0] == achievements[i].achId_ && achievements[i].isAch_ && achievements[i].isAward_)
				{
					if(!FinishList.Contains(_achievementType[metaData[(int)achievements[i].achId_]._AchieveType][0]))
						FinishList.Add(_achievementType[metaData[(int)achievements[i].achId_]._AchieveType][0]);
					if(_achievementType[metaData[(int)achievements[i].achId_]._AchieveType].Count>1)
						_achievementType[metaData[(int)achievements[i].achId_]._AchieveType].RemoveAt(0);
				}
					
			}
		}
	}
//	public static void ff()
//	{
//		AchieveData temp;
//		foreach(AchieveData ad in metaData.Values)
//		{
//			if(!_achievementType.ContainsKey(ad.AchieveType_))
//				_achievementType.Add(ad.AchieveType_,null);
//			AchieveDatas.Add(ad);
//		}
//
//		for(int i =0;i<AchieveDatas.Count-1;i++)
//		{
//			for(int j =0;j<AchieveDatas.Count-i -1;j++)
//			{
//				if(AchieveDatas[j+1].AchieveType_ == AchieveDatas[j].AchieveType_ )
//				{
//					if(AchieveDatas[j+1].id_<AchieveDatas[j].id_)
//					{
//						temp = AchieveDatas[j+1];
//						AchieveDatas[j+1] = AchieveDatas[j];
//						AchieveDatas[j] = temp;
//					}
//
//				}
//
//			}
//		}
//		ClientLog.Instance.Log (AchieveDatas);
//
//	}
	public static AchieveData GetData(int id)
	{
		if(!metaData.ContainsKey(id))
			return null;
		return metaData[id];
	}
	public static Dictionary<int, AchieveData> GetData()
	{
		
		return metaData;
	}
}