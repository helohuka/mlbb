using UnityEngine;
using System.Collections.Generic;
using LitJson;

public class EffectDepedenceData : MonoBehaviour
{
    static Dictionary<string, List<string>> metaData;

    public static void ParseData(string content, string fileName)
    {
        content = content.Replace("\n", "");
        content = content.Replace("\\", "");
        metaData = JsonMapper.ToObject<Dictionary<string, List<string>>>(content);
    }

    public static string[] GetRefAssets(string effName)
    {
        if (metaData == null)
            return null;
        if (!metaData.ContainsKey(effName))
            return null;
        return metaData[effName].ToArray();
    }
}
