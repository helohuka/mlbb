using UnityEngine;
using System.Collections.Generic;

public class HelpLevelData {

    public int id;
    public int level;
    public string icon;
    public string name;
    public string desc;
    public string openui;
    public string npc;

    public static Dictionary<int, HelpLevelData> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, HelpLevelData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("HelpLevelData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        HelpLevelData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new HelpLevelData();
            data.id = parser.GetInt(i, "id");
            data.icon = parser.GetString(i, "icon");
            data.name = parser.GetString(i, "name");
            data.desc = parser.GetString(i, "desc");
            data.level = parser.GetInt(i, "level");
            data.openui = parser.GetString(i, "openui");
            data.npc = parser.GetString(i, "npc");

            if (metaData.ContainsKey(data.id))
            {
                ClientLog.Instance.LogError("HelpLevelData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static Dictionary<int, HelpLevelData> GetMetaData()
    {
        return metaData;
    }

    public static HelpLevelData NextData(int crtlevel)
    {
        HelpLevelData data = null;
        int nextLv = int.MaxValue;
        foreach (HelpLevelData hlData in metaData.Values)
        {
            if(hlData.level > crtlevel)
            {
                if(hlData.level < nextLv)
                {
                    nextLv = hlData.level;
                    data = hlData;
                }
            }
        }
        return data;
    }
}
