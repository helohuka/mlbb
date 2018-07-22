using UnityEngine;
using System.Collections.Generic;

public class TitleData {

    public int id_;

    public string desc_;

    public int minValue_;

    public int maxValue_;

    private static Dictionary<int, TitleData> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, TitleData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("TitleData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        TitleData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new TitleData();
            data.id_ = parser.GetInt(i, "TitleId");
            data.desc_ = parser.GetString(i, "Desc");
            data.minValue_ = parser.GetInt(i, "MinReputation");
            data.maxValue_ = parser.GetInt(i, "MaxReputation");

            metaData.Add(data.id_, data);
        }
        parser.Dispose();
        parser = null;
    }

    public static string GetTitleByValue(int value)
    {
        foreach (TitleData data in metaData.Values)
        {
            if (value >= data.minValue_ && value <= data.maxValue_)
                return data.desc_;
        }

        return "";
    }
	public static TitleData GetTitleData(int tid)
	{
		if(!metaData.ContainsKey(tid))
			return null;
		return metaData[tid];
	}
}
