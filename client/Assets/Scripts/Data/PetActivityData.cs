using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetActivityData {

    public int id_;

    public int monsterID_;

    public string[] skillIDs_;

    public string[] openTimeDesc_;

    public string[] difficults_;

    public string[] levels_;

    private static Dictionary<int, PetActivityData> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, PetActivityData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("PetActivityData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        PetActivityData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new PetActivityData();
            data.id_ = parser.GetInt(i, "ID");
            data.monsterID_ = parser.GetInt(i, "MonsterID");
            string metaStr = parser.GetString(i, "Skills");
            data.skillIDs_ = metaStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            metaStr = parser.GetString(i, "OpenTimeDesc");
            data.openTimeDesc_ = metaStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            metaStr = parser.GetString(i, "Difficults");
            data.difficults_ = metaStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            metaStr = parser.GetString(i, "Levels");
            data.levels_ = metaStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            
            if (metaData.ContainsKey(data.id_))
            {
                ClientLog.Instance.LogError("PetActivityData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id_] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static PetActivityData GetData(int id)
    {
        if (!metaData.ContainsKey(id))
            return null;
        return metaData[id];
    }

    public static Dictionary<int, PetActivityData> GetMetaData()
    {
        return metaData;
    }
}
