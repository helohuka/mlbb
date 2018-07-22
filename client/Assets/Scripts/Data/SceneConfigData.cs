using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class SceneConfigData {

    public int chapterId_;

	public string fileName_;

	public string sceneName_;

    public string battleName_;



    public List<NPCInfo> npcList_;

    public List<ZoneInfo> monsterList_;

    public List<EntryInfo> entryList_;

	private static Dictionary<string, SceneConfigData> metaData_ = new Dictionary<string, SceneConfigData>();

	public static void ParseData(string content, string fileName)
	{
        if (!fileName.EndsWith(".xml"))
            return;

		SceneConfigData data = new SceneConfigData ();
		data.Init ();

		fileName = fileName.Substring (fileName.LastIndexOf("/") + 1, fileName.IndexOf(".xml") - 1 - fileName.LastIndexOf("/"));

		ParseXML (content, data, fileName);

		if(metaData_.ContainsKey(data.fileName_))
		{
			ClientLog.Instance.LogError("Same id in SceneData :" + data.fileName_);
			return;
		}
        metaData_.Add (data.fileName_, data);
	}

	public void Init()
	{
		npcList_ = new List<NPCInfo> ();
        monsterList_ = new List<ZoneInfo>();
        entryList_ = new List<EntryInfo>();
	}

	public static SceneConfigData GetData(string fileName)
	{
		if(!metaData_.ContainsKey(fileName))
		{
			ClientLog.Instance.LogError("SceneData by " + fileName + " is null");
			return null;
		}
		return metaData_[fileName];
	}

    public static Vector3 GetEntryPos(string fileName, int entryId)
    {
        SceneConfigData data = GetData(fileName);
        for (int i=0; i < data.entryList_.Count; ++i)
        {
            if (data.entryList_[i].id_ == entryId)
                return data.entryList_[i].position_;
        }
        return Vector3.zero;
    }

    public static EntryInfo GetEntryInfo(string fileName, int entryId)
    {
        SceneConfigData data = GetData(fileName);
        for (int i = 0; i < data.entryList_.Count; ++i)
        {
            if (data.entryList_[i].id_ == entryId)
                return data.entryList_[i];
        }
        return null;
    }

    public static int GetBornPosEntryID(string fileName)
    {
        SceneConfigData data = GetData(fileName);
        for (int i = 0; i < data.entryList_.Count; ++i)
        {
            if (data.entryList_[i].isBornPos_)
                return data.entryList_[i].id_;
        }
        return 0;
    }

    public static Vector3 GetNpcPos(string fileName, int npcId)
    {
        SceneConfigData data = GetData(fileName);
        for (int i = 0; i < data.npcList_.Count; ++i)
        {
            if (data.npcList_[i].id_ == npcId)
                return data.npcList_[i].position_;
        }
        return Vector3.zero;
    }


	static void ParseXML(string content, SceneConfigData data, string fileName)
	{
		XmlDocument doc = new XmlDocument ();
        try
        {
            doc.LoadXml(content);
        }
        catch (System.Xml.XmlException ex)
        {
            ClientLog.Instance.LogError(" xml file : " + fileName + " has exception.");
        }

		data.fileName_ = fileName;

        XmlNode rootNode = doc.SelectSingleNode("/Scene");
        foreach (XmlAttribute attr in rootNode.Attributes)
        {
            if (attr.Name.Equals("ChapterID") && !string.IsNullOrEmpty(attr.Value))
                data.chapterId_ = int.Parse(attr.Value);

            if (attr.Name.Equals("SceneName"))
                data.sceneName_ = attr.Value;

            if (attr.Name.Equals("BattleName"))
                data.battleName_ = attr.Value;
        }
    }

    public int InWhitchZone(Vector2 pos)
    {
        for (int i = 0; i < monsterList_.Count; ++i)
        {
            if (InRange(monsterList_[i].center_, monsterList_[i].radius_, pos))
                return monsterList_[i].id_;
        }
        return 0;
    }

    public ZoneInfo GetZone(int zoneId)
    {
        for (int i = 0; i < monsterList_.Count; ++i)
        {
            if (monsterList_[i].id_ == zoneId)
                return monsterList_[i];
        }
        return null;
    }

    public Vector2 GetValidPosInZone(int zoneId)
    {
        ZoneInfo zone = GetZone(zoneId);
        float angle = UnityEngine.Random.Range(0f, 360f);
        float angleHud = angle * Mathf.PI / 180;
        float radius = UnityEngine.Random.Range(0f, zone.radius_);
        Vector2 dest = new Vector2(radius * Mathf.Cos(angleHud) + zone.center_.x, radius * Mathf.Sin(angleHud) + zone.center_.y);
        return dest;
    }

    public Vector2 GetZoneCenter(int zoneId)
    {
        ZoneInfo zone = GetZone(zoneId);
        return zone.center_;
    }

    public bool OutOfZoneRange(int zoneId, Vector2 pos)
    {
        ZoneInfo zone = GetZone(zoneId);
        return !InRange(zone.center_, zone.radius_, pos);
    }

    bool InRange(Vector2 center, float r, Vector2 dest)
    {
        float len = (dest - center).magnitude;
        return len < r;
    }
}

public class NPCInfo
{
    public int id_;
    public int sceneId_;
    public Vector3 position_;
    public Quaternion rotation_;
    public int areaNum_;
}

public class ZoneInfo
{
    public int id_;
    public Vector2 center_;
	public float radius_;
}

public class EntryInfo
{
	public int id_;
    public int areaNum_;
    public SceneInfo toScene_;
    public int toEntryId_;
    public Vector3 position_;
    public bool isBornPos_;

    public struct  SceneInfo 
    {
        public int toSceneId_;
        public int sceneAreaNum_;
    }
}