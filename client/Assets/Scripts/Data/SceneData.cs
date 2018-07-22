using System;
using System.Collections.Generic;

public class SceneData {

    static int HomeSceneId_;

    public int id_;

    public SceneType sceneType_;

    public string sceneName_;

    public string sceneIcon_;

    public string sceneXml_;
    public string scene_icon;
    public int xmlNameId_;

    public int UnLockQuestID_;

    public string nameEffectId_;
    public int M_ID;

    public string sceneLevelName_;
    public string battleLevelName_;

    public float dir_;
    public float zoom_;
    public float offsetX_;

    public float offsetY_;
    public string minmap;

    public class SceneEntry
    {
        public int toSceneId_;
        public UnityEngine.Vector2 pos_;
    }

    public List<SceneEntry> entrys_ = new List<SceneEntry>();

    private static Dictionary<int, SceneData> metaData;

    public static void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, SceneData>();

        CSVParser parser = new CSVParser();
        if (!parser.Parse(content))
        {
            ClientLog.Instance.LogError("SceneData" + ConfigLoader.Instance.csvext + "解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        SceneData data = null;
        for (int i = 0; i < recordCounter; ++i)
        {
            data = new SceneData();
            data.id_ = parser.GetInt(i, "sceneId");
            data.sceneType_ = (SceneType)Enum.Parse(typeof(SceneType), parser.GetString(i, "SceneType"));
            data.sceneName_ = parser.GetString(i, "scene_name");
            data.sceneIcon_ = parser.GetString(i, "scene_icon");
            data.sceneXml_ = parser.GetString(i, "scene_xml");
            data.sceneXml_ = data.sceneXml_.Substring(data.sceneXml_.LastIndexOf("/") + 1, data.sceneXml_.IndexOf(".xml") - 1 - data.sceneXml_.LastIndexOf("/"));
            data.UnLockQuestID_ = parser.GetInt(i, "UnLockQuestID");
            data.nameEffectId_ = parser.GetString(i, "EffectId");
            data.M_ID = parser.GetInt(i, "MUSIC_ID");
            data.scene_icon = parser.GetString(i, "scene_icon");
            data.xmlNameId_ = int.Parse(data.sceneXml_.Substring(data.sceneXml_.LastIndexOf("_") + 1));
            data.sceneLevelName_ = parser.GetString(i, "SceneName");
            data.battleLevelName_ = parser.GetString(i, "BattleName");
            data.offsetX_ = parser.GetFloat(i, "offsetx");
            data.offsetY_ = parser.GetFloat(i, "offsety");
            data.zoom_ = parser.GetFloat(i, "zoom");
            data.dir_ = parser.GetFloat(i, "dir");
            data.minmap = parser.GetString(i, "minimap");
            string entrysStr = parser.GetString(i, "EntryInfo");
            string[] entryAry = entrysStr.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < entryAry.Length; ++j)
            {
                string[] entryInfo = entryAry[j].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                string[] entryPosStr = entryInfo[0].Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                SceneEntry se = new SceneEntry();
                se.pos_ = new UnityEngine.Vector2(float.Parse(entryPosStr[0]), float.Parse(entryPosStr[1]));
                se.toSceneId_ = int.Parse(entryInfo[1]);
                if (data.entrys_ == null)
                    data.entrys_ = new List<SceneEntry>();
                data.entrys_.Add(se);
            }
            if (data.sceneType_ == SceneType.SCT_Home)
            {
                HomeSceneId_ = data.id_;
            }
            //			{
            //				string str =  parser.GetString(i,"UnLockQuestID");
            //				if(str.Contains(";"))
            //				{
            //					string [] strs = str.Split(';');
            //					data.UnLockQuestID_ = strs;
            //				}
            //				else
            //				{
            //					data.UnLockQuestID_ [0]=str;
            //				}
            //			}

            if (metaData.ContainsKey(data.id_))
            {
                ClientLog.Instance.LogError("SceneSimpleData" + ConfigLoader.Instance.csvext + "ID重复");
                return;
            }
            metaData[data.id_] = data;
        }
        parser.Dispose();
        parser = null;
    }

    public static SceneData GetData(int id)
    {
        if (!metaData.ContainsKey(id))
        {
            ClientLog.Instance.Log("SceneData ID: " + id + "is not exist!");
            return null;
        }
        return metaData[id];
    }
    public static Dictionary<int, SceneData> GetData()
    {
        if (metaData.Count == 0)
        {
            ClientLog.Instance.Log("SceneSimpleData has not valid data!");
            return null;
        }
        return metaData;
    }

    public static int HomeID
    {
        get { return HomeSceneId_; }
    }
}
