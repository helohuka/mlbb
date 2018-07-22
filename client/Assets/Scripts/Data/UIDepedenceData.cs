using UnityEngine;
using System;
using LitJson;
using System.Collections.Generic;

public class UIDepedenceData {

    static Dictionary<string, List<string>> metaData;

    public static void ParseData(string content, string fileName)
    {
        content = content.Replace("\n", "");
        content = content.Replace("\\", "");
        metaData = JsonMapper.ToObject<Dictionary<string, List<string>>>(content);
    }

    public static string[] GetRefAtlas(string uiName)
    {
        if (metaData == null)
            return null;
        if (!metaData.ContainsKey(uiName))
            return null;
        return metaData[uiName].ToArray();
    }
}
