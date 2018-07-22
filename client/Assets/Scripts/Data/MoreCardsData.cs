using UnityEngine;
using System.Collections.Generic;

public class MoreCardsDrawData {
    public int times_;
    public int cost_;

    private static Dictionary<int, MoreCardsDrawData> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, MoreCardsDrawData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("MoreCardsDrawData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        MoreCardsDrawData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new MoreCardsDrawData();
            data.times_ = parser.GetInt(i, "time");
            data.cost_ = parser.GetInt(i, "cost");
            if (metaData.ContainsKey(data.times_))
            {
                ClientLog.Instance.LogError("MoreCardsDrawData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.times_] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static MoreCardsDrawData GetData(int id)
    {
        if (!metaData.ContainsKey(id))
            return null;
        return metaData[id];
    }
}

public class MoreCardsRewardData
{
    public int id_;
    public int itemid_;
    public int itemnum_;
	public int type;
    private static Dictionary<int, MoreCardsRewardData> metaData;
    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, MoreCardsRewardData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("MoreCardsRewardData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        MoreCardsRewardData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new MoreCardsRewardData();
            data.id_ = parser.GetInt(i, "id");
            data.itemid_ = parser.GetInt(i, "reward");
            data.itemnum_ = parser.GetInt(i, "num");
			data.type =  parser.GetInt(i, "type");
            if (metaData.ContainsKey(data.id_))
            {
                ClientLog.Instance.LogError("MoreCardsRewardData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id_] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static MoreCardsRewardData GetData(int id)
    {
        if (!metaData.ContainsKey(id))
            return null;
        return metaData[id];
    }
	public static List<MoreCardsRewardData> GetRewardData()
	{
		List<MoreCardsRewardData> datas = new List<MoreCardsRewardData> ();
		foreach(MoreCardsRewardData crd in metaData.Values)
		{
			datas.Add(crd);
		}
		datas.Sort (CardsRewardDataSort);
		return datas;
	}
	static int CardsRewardDataSort(MoreCardsRewardData crd,MoreCardsRewardData crd1)
	{
		if(crd.type>crd1.type)
		{
			return -1;
		}else
		{
			return 0;
		}

	}
}
