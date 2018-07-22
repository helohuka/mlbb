using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TalkData  {

	public int TalkId;
    public List<Pair<int, string>> Content = new List<Pair<int, string>>();
    public int BattleId;

	private static Dictionary<int, TalkData> Meta;

	public static void ParseData(string content, string fileName)
	{
        Meta = new Dictionary<int, TalkData>();
		
		CSVParser parser = new CSVParser ();
		if(!parser.Parse (content))
		{
			ClientLog.Instance.LogError("TalkData" + ConfigLoader.Instance.csvext + "解析错误");
			return;
		}
		
		int recordCounter = parser.GetRecordCounter();
		TalkData data = null;
		for(int i=0; i < recordCounter; ++i)
		{
			data = new TalkData ();

			data.TalkId = parser.GetInt (i, "talk");

            if (Meta.ContainsKey(data.TalkId))
            {
                ClientLog.Instance.LogError("TalkData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }

            string npcid = "npcid";
            string contex = "content";

            for(int j=1; j<=10; ++j)
            {
                int id = parser.GetInt(i, npcid + j);
                if(id == 0)
                    break;
                Pair<int, string> pair = new Pair<int, string>(id, parser.GetString(i, contex + j));
                data.Content.Add(pair);
            }

            data.BattleId = parser.GetInt(i, "BattleID");

            Meta[data.TalkId] = data;
		}
		parser.Dispose ();
		parser = null;
	}
	
	public static TalkData GetData(int id)
	{
        if (!Meta.ContainsKey(id))
			return null;
        return Meta[id];
	}
	
	public static Dictionary<int, TalkData> GetMetaData()
	{
        return Meta;
	}
}
