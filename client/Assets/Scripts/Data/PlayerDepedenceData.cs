using UnityEngine;
using System.Collections.Generic;
using LitJson;

public class PlayerDepedenceData : MonoBehaviour {

    static Dictionary<string, List<string>> metaData;

    public static void ParseData(string content, string fileName)
    {
        content = content.Replace("\n", "");
        content = content.Replace("\\", "");
        metaData = JsonMapper.ToObject<Dictionary<string, List<string>>>(content);
    }

    public static string[] GetRefAssets(string uiName)
    {
        if (metaData == null)
            return null;
        if (!metaData.ContainsKey(uiName))
            return null;
        return metaData[uiName].ToArray();
    }
}
