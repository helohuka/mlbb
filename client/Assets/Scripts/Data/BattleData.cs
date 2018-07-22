using UnityEngine;
using System.Collections.Generic;

public class BattleData {

    public int id_;

    public int battleBeginTalk_;

    public int battleEndTalk_;

    public string sceneName_;

    private static Dictionary<int, BattleData> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, BattleData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("BattleData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        BattleData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new BattleData();
            data.id_ = parser.GetInt(i, "ID");
            data.battleBeginTalk_ = parser.GetInt(i, "dialogue1");
            data.battleEndTalk_ = parser.GetInt(i, "dialogue2");
            data.sceneName_ = parser.GetString(i, "BattleName");
            if (metaData.ContainsKey(data.id_))
            {
                ClientLog.Instance.LogError("BattleData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id_] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static string GetSceneName(int battleid)
    {
        if (!metaData.ContainsKey(battleid))
            return "";
        return metaData[battleid].sceneName_;
    }

    public static int GetBeginTalk(int battleid)
    {
        if (!metaData.ContainsKey(battleid))
            return 0;
        return metaData[battleid].battleBeginTalk_;
    }

    public static int GetEndTalk(int battleid)
    {
        if (!metaData.ContainsKey(battleid))
            return 0;
        return metaData[battleid].battleEndTalk_;
    }
}
