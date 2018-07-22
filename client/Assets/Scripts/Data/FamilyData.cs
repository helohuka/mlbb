using System;
using UnityEngine;
using System.Collections.Generic;

public class FamilyData {

    public int id_;

    public int level_;

    public int needMoney_;

    public string name_;

    public string icon_;

    public string desc_;

	public int number_;

	public int rewrod_;

	public GuildBuildingType  type_;

    private static Dictionary<int, List<FamilyData>> metaData;

	private static List<FamilyData> allL1Data = new List<FamilyData> ();

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, List<FamilyData>>();
		allL1Data = new List<FamilyData> ();
        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("FamilyData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        FamilyData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new FamilyData();
            data.id_ = parser.GetInt(i, "ArchitectureID");
            data.level_ = parser.GetInt(i, "Level");
            data.needMoney_ = parser.GetInt(i, "NeedMoney");
            data.name_ = parser.GetString(i, "Name");
            data.icon_ = parser.GetString(i, "ICON");
            data.desc_ = parser.GetString(i, "Desc");
			data.number_ = parser.GetInt(i, "Number");
			data.rewrod_ = parser.GetInt(i, "Reward");
			data.type_ = (GuildBuildingType)Enum.Parse(typeof(GuildBuildingType), parser.GetString(i, "Type"));
            if (!metaData.ContainsKey(data.id_))
            {
                allL1Data.Add(data);
                metaData.Add(data.id_, new List<FamilyData>());
            }
            metaData[data.id_].Add(data);
        }
        parser.Dispose();
        parser = null;
    }

    public static FamilyData GetData(int id, int lv)
    {
        if (!metaData.ContainsKey(id))
            return null;

        for (int i = 0; i < metaData[id].Count; ++i)
        {
            if(metaData[id][i].level_ == lv)
                return metaData[id][i];
        }
        return null;
    }

    public static List<FamilyData> AllL1Data()
    {
        return allL1Data;
    }
}
