using System;
using System.Collections.Generic;

public class LoadingTipsData
{
    public string tips_;
    private static List<string> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new List<string>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("LoadingTipsData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        for (int i = 0; i < recordCounter; ++i)
        {
            metaData.Add(parser.GetString(i, "Tips"));
        }
        parser.Dispose();
        parser = null;
    }

    public static string RandomTips()
    {
        if (metaData == null)
            return "";

        return metaData[UnityEngine.Random.Range(0, metaData.Count)];
    }
}


