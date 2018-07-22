using System;
using UnityEngine;
using System.Collections.Generic;

public class SevenDaysData {

    public int id;
    public string desc;
    public COM_Item[] rewardItem;
    public int day;

    public static Dictionary<int, SevenDaysData> metaData;
    public static Dictionary<int, List<SevenDaysData>> dayData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, SevenDaysData>();
        dayData = new Dictionary<int, List<SevenDaysData>>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("SevenDaysData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        SevenDaysData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new SevenDaysData();
            data.id = parser.GetInt(i, "ID");
            data.desc = parser.GetString(i, "AtName");
            data.rewardItem = new COM_Item[3];
            string rewardStr = parser.GetString(i, "Item");
            string[] comItem = rewardStr.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries);
            string[] comItemDetail;
            COM_Item item;
            for (int j = 0; j < comItem.Length; ++j)
            {
                comItemDetail = comItem[j].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                item = new COM_Item();
                item.itemId_ = uint.Parse(comItemDetail[0]);
                item.stack_ = short.Parse(comItemDetail[1]);
                data.rewardItem[j] = item;
            }
            data.day = parser.GetInt(i, "Day");

            if (!dayData.ContainsKey(data.day))
                dayData.Add(data.day, new List<SevenDaysData>());
            dayData[data.day].Add(data);

            if (metaData.ContainsKey(data.id))
            {
                ClientLog.Instance.LogError("SevenDaysData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static SevenDaysData GetData(int id)
    {
        if (!metaData.ContainsKey(id))
            return null;
        return metaData[id];
    }
}
