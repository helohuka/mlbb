using System;
using UnityEngine;
using System.Collections.Generic;

public class DaliyActivityData {

    public int id_;

    public string activityName_;

    public int activityType_;

    public ActivityType activityKind_;

    public string joinInfo_;

    public int joinLv_;

    public string activityTime_;

    public string activityFrom_;

    public string desc_;

    public string award_;

    public string startTime_;

    public int itemid4Icon_;

    public int maxCount_;

	public int Active_;
	public string Icon_;
    private static Dictionary<int, DaliyActivityData> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, DaliyActivityData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("DaliyActivityData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        DaliyActivityData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new DaliyActivityData();
            data.id_ = parser.GetInt(i, "ID");
            data.activityName_ = parser.GetString(i, "ActivityName");
            data.activityType_ = parser.GetInt(i, "ActivitiesType");
            data.activityKind_ = (ActivityType)Enum.Parse(typeof(ActivityType), parser.GetString(i, "ActivitiesKind"));
            data.joinInfo_ = parser.GetString(i, "Join");
            data.joinLv_ = parser.GetInt(i, "JoinGrade");
            data.activityTime_ = parser.GetString(i, "ActivityTime");
            data.activityFrom_ = parser.GetString(i, "ActivityForm");
            data.desc_ = parser.GetString(i, "Desc");
            data.award_ = parser.GetString(i, "Award");
            data.startTime_ = parser.GetString(i, "StartTime");
            data.itemid4Icon_ = parser.GetInt(i, "ItemId");
            data.maxCount_ = parser.GetInt(i, "maxtime");
			data.Active_ = parser.GetInt(i, "Active");
			data.Icon_ = parser.GetString(i, "Icon");
            if (metaData.ContainsKey(data.id_))
            {
                ClientLog.Instance.LogError("DaliyActivityData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id_] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static Dictionary<int, DaliyActivityData> MetaData()
    {
        return metaData;
    }

    public static DaliyActivityData GetData(int id)
    {
        if (!metaData.ContainsKey(id))
            return null;
        return metaData[id];
    }

	public static DaliyActivityData GetData(ActivityType type)
	{
		foreach(DaliyActivityData da in metaData.Values)
		{
			if(da.activityKind_ == type)
			{
				return da;
			}
		}

		return null;
	}
	public static List<DaliyActivityData> GetDatas(ActivityType[] type)
	{
		List<DaliyActivityData> temps = new List<DaliyActivityData> ();
		foreach(DaliyActivityData da in metaData.Values)
		{
            for (int i = 0; i < type.Length; ++i)
            {
                if (da.activityKind_ == type[i])
                {
                    temps.Add(da);
                }
            }
		}
		
		return temps;
	}
    public static int GetActivityMaxCount(ActivityType type)
    {
        foreach (DaliyActivityData data in metaData.Values)
        {
            if(data.activityKind_ == type)
            {
                return data.maxCount_;
            }
        }
        return 0;
    }
}
