using UnityEditor;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SceneEditor : EditorWindow {

	[MenuItem("Tools/SceneEditor")]
	static void Open()
	{
		Init ();
		EditorWindow.GetWindow (typeof(SceneEditor));
	}

	static void Init()
	{
		scene_ = new Scene ();
        npcObjList_ = new List<GameObject>();
		zoneObjList_ = new List<GameObject> ();
		entryObjList_ = new List<GameObject> ();
        funcObjList_ = new List<GameObject>();
		root = GameObject.Find ("DynamicRoot");
		if(root == null)
			root = new GameObject ("DynamicRoot");

        funcPointMat_ = new Material(Shader.Find("Self-Illumin/Diffuse"));
        funcPointMat_.color = Color.green;
	}

    static Material funcPointMat_;

	static Scene scene_;

	static GameObject root = null;
	static GameObject bornPos = null;
    static List<GameObject> npcObjList_;
	static List<GameObject> zoneObjList_;
	static List<GameObject> entryObjList_;
    static List<GameObject> funcObjList_;
	static string xmlPath = "";

    static Vector2 NpcScrollPos_;
	static Vector2 ZoneScrollPos_;
	static Vector2 EntryScrollPos_;
    static Vector2 FuncPointScrollPos_;
    static Vector2 TotalScrollPos_;

	bool beginEdit = false;
	public void OnGUI()
	{
		if(beginEdit)
		{
			EditLayout();
			return;
		}
		if(GUILayout.Button ("Load"))
		{
			xmlPath = EditorUtility.OpenFilePanel("","","");
			beginEdit = true;
			Load();
		}
		if(GUILayout.Button("New"))
		{
			beginEdit = true;
		}
    }

	static void Save(Scene scene)
	{
		string data = SerializeObject (scene);
		StreamWriter writer;

		string savePath = string.Format ("{0}/../../../Config/Tables/Scene/Scene_{1}.xml", Application.dataPath, scene.ID);
		FileInfo file = new FileInfo (savePath);
		writer = file.CreateText ();
		writer.Write (data);
		writer.Close ();
	}

	static void Load()
	{
		FileInfo fi = new FileInfo (xmlPath);
		if(fi.Exists)
		{
			StreamReader sr = fi.OpenText();
			string content = sr.ReadToEnd();
			sr.Close();
			if(content.ToString() != "")
				scene_ = (Scene)DeserializeObject(content);
		}
	}

	static string SerializeObject(object o)
	{
		string XmlizedString = "";
		MemoryStream ms = new MemoryStream ();
		XmlSerializer xs = new XmlSerializer (typeof(Scene));
		XmlTextWriter tw = new XmlTextWriter (ms, new System.Text.UTF8Encoding(false));
		xs.Serialize (tw, o);
		ms = (MemoryStream)tw.BaseStream;
		XmlizedString = UTF8ByteArrayToString(ms.ToArray());
		return XmlizedString;
	}

	static object DeserializeObject(string pXmlizedString)   
	{
		XmlSerializer xs = new XmlSerializer(typeof(Scene));
		MemoryStream ms = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
//		XmlTextWriter tw = new XmlTextWriter(ms, new System.Text.UTF8Encoding(false));
		return xs.Deserialize(ms);
	}

	static string UTF8ByteArrayToString(byte[] characters)   
	{        
		UTF8Encoding encoding = new UTF8Encoding();   
		string constructedString = encoding.GetString(characters);   
		return (constructedString);   
	}   
		
	static byte[] StringToUTF8ByteArray(string pXmlString)   
	{   
		UTF8Encoding encoding = new UTF8Encoding();   
		byte[] byteArray = encoding.GetBytes(pXmlString);   
		return byteArray;   
	}  

	static void EditLayout()
	{
		// sceneInfo
        GUILayout.BeginVertical();
        GUILayout.Label("Chapter:");
        scene_.CID = GUILayout.TextField(scene_.CID);
        GUILayout.EndVertical();

		GUILayout.BeginVertical();
		GUILayout.Label("SceneID:");
		scene_.ID = GUILayout.TextField (scene_.ID);
		GUILayout.EndVertical ();

		GUILayout.BeginVertical();
		GUILayout.Label("SceneName:");
		scene_.Name = GUILayout.TextField (scene_.Name);
		GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("BattleName:");
        scene_.BattleName = GUILayout.TextField(scene_.BattleName);
        GUILayout.EndVertical();
		GUILayout.Space (5);

        TotalScrollPos_ = GUILayout.BeginScrollView(TotalScrollPos_, GUILayout.Width(600), GUILayout.Height(600));
        // npcInfo
        GUILayout.Box("NpcInfo");
        if (scene_.npcList.Count > 0)
        {
            NpcScrollPos_ = GUILayout.BeginScrollView(NpcScrollPos_, GUILayout.Width(300), GUILayout.Height(200));
            for (int i = 0; i < scene_.npcList.Count; ++i)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("ID");
                ((Npc)scene_.npcList[i]).ID = GUILayout.TextField(((Npc)scene_.npcList[i]).ID);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Position" + new Vector3(((Npc)scene_.npcList[i]).X, ((Npc)scene_.npcList[i]).Y, ((Npc)scene_.npcList[i]).Z));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Rotation" + new Quaternion(((Npc)scene_.npcList[i]).RX, ((Npc)scene_.npcList[i]).RY, ((Npc)scene_.npcList[i]).RZ, ((Npc)scene_.npcList[i]).RW));
                GUILayout.EndHorizontal();

                if (scene_.npcList.Count != npcObjList_.Count)
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    go.transform.parent = root.transform;
                    go.transform.position = new Vector3(((Npc)scene_.npcList[i]).X, ((Npc)scene_.npcList[i]).Y, ((Npc)scene_.npcList[i]).Z);
                    go.transform.rotation = new Quaternion(((Npc)scene_.npcList[i]).RX, ((Npc)scene_.npcList[i]).RY, ((Npc)scene_.npcList[i]).RZ, ((Npc)scene_.npcList[i]).RW);
                    npcObjList_.Add(go);
                }
                ((Npc)scene_.npcList[i]).X = npcObjList_[i].transform.position.x;
                ((Npc)scene_.npcList[i]).Y = npcObjList_[i].transform.position.y;
                ((Npc)scene_.npcList[i]).Z = npcObjList_[i].transform.position.z;
                ((Npc)scene_.npcList[i]).RX = npcObjList_[i].transform.rotation.x;
                ((Npc)scene_.npcList[i]).RY = npcObjList_[i].transform.rotation.y;
                ((Npc)scene_.npcList[i]).RZ = npcObjList_[i].transform.rotation.z;
                ((Npc)scene_.npcList[i]).RW = npcObjList_[i].transform.rotation.w;

                if (GUILayout.Button("Delete"))
                {
                    scene_.npcList.RemoveAt(i);
                    DestroyImmediate(npcObjList_[i]);
                    npcObjList_.RemoveAt(i);
                }
                GUILayout.Space(15);
            }
            GUILayout.EndScrollView();
            
        }
        else
        {
            GUILayout.Label("Empty");
        }
        GUILayout.Space(5);
        // monsterInfo
        GUILayout.Box("ZoneInfo");
        ZoneScrollPos_ = GUILayout.BeginScrollView(ZoneScrollPos_, GUILayout.Width(300), GUILayout.Height(200));
        for (int i = 0; i < scene_.zoneList.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID");
            GUILayout.Label(((Zone)scene_.zoneList[i]).ID);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius " + ((Zone)scene_.zoneList[i]).Radius);
            GUILayout.Label("Center" + new Vector3(((Zone)scene_.zoneList[i]).CenterX, 0f, ((Zone)scene_.zoneList[i]).CenterZ));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("MinCount");
            ((Zone)scene_.zoneList[i]).MonsterMin = GUILayout.TextField(((Zone)scene_.zoneList[i]).MonsterMin);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("MaxCount");
            ((Zone)scene_.zoneList[i]).MonsterMax = GUILayout.TextField(((Zone)scene_.zoneList[i]).MonsterMax);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rate");
            ((Zone)scene_.zoneList[i]).Rate = GUILayout.TextField(((Zone)scene_.zoneList[i]).Rate);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("MonsterGroup");
            ((Zone)scene_.zoneList[i]).MonsterGroup = GUILayout.TextField(((Zone)scene_.zoneList[i]).MonsterGroup);
            GUILayout.EndHorizontal();

            if (scene_.zoneList.Count != zoneObjList_.Count)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.localScale = new Vector3(1f, 0.1f, 1f);
                DestroyImmediate(go.GetComponent<SphereCollider>());
                go.transform.parent = root.transform;
                go.transform.position = new Vector3(((Zone)scene_.zoneList[i]).CenterX, ((Zone)scene_.zoneList[i]).CenterY, ((Zone)scene_.zoneList[i]).CenterZ);
                go.transform.localScale = new Vector3(((Zone)scene_.zoneList[i]).Radius * 2f, 0.1f, ((Zone)scene_.zoneList[i]).Radius * 2f);
                zoneObjList_.Add(go);
            }
            ((Zone)scene_.zoneList[i]).CenterX = zoneObjList_[i].transform.position.x;
            ((Zone)scene_.zoneList[i]).CenterY = zoneObjList_[i].transform.position.y;
            ((Zone)scene_.zoneList[i]).CenterZ = zoneObjList_[i].transform.position.z;

            ((Zone)scene_.zoneList[i]).Radius = zoneObjList_[i].transform.localScale.x / 2f;

            if (GUILayout.Button("Delete"))
            {
                scene_.zoneList.RemoveAt(i);
                DestroyImmediate(zoneObjList_[i]);
                zoneObjList_.RemoveAt(i);
            }
            GUILayout.Space(15);
        }
        GUILayout.EndScrollView();
        GUILayout.Space(10);

        // entryInfo
        GUILayout.Box("EntryInfo");
        EntryScrollPos_ = GUILayout.BeginScrollView(EntryScrollPos_, GUILayout.Width(300), GUILayout.Height(200));
        for (int i = 0; i < scene_.entryList.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID");
            GUILayout.Label(((SceneEntry)scene_.entryList[i]).ID);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            ((SceneEntry)scene_.entryList[i]).isBornPos = GUILayout.Toggle(((SceneEntry)scene_.entryList[i]).isBornPos, "是否当作出生点");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Radius " + ((SceneEntry)scene_.entryList[i]).Radius);
            GUILayout.Label("Center" + new Vector3(((SceneEntry)scene_.entryList[i]).CenterX, ((SceneEntry)scene_.entryList[i]).CenterY, ((SceneEntry)scene_.entryList[i]).CenterZ));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("AreaNum");
            ((SceneEntry)scene_.entryList[i]).areaNum = GUILayout.TextField(((SceneEntry)scene_.entryList[i]).areaNum);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("toSceneID");
            ((SceneEntry)scene_.entryList[i]).toSceneID = GUILayout.TextField(((SceneEntry)scene_.entryList[i]).toSceneID);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("toEntryID");
            ((SceneEntry)scene_.entryList[i]).toEntryID = GUILayout.TextField(((SceneEntry)scene_.entryList[i]).toEntryID);
            GUILayout.EndHorizontal();

            if (scene_.entryList.Count != entryObjList_.Count)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.transform.localScale = new Vector3(1f, 0.1f, 1f);
                DestroyImmediate(go.GetComponent<SphereCollider>());
                go.transform.parent = root.transform;
                go.transform.position = new Vector3(((SceneEntry)scene_.entryList[i]).CenterX, ((SceneEntry)scene_.entryList[i]).CenterY, ((SceneEntry)scene_.entryList[i]).CenterZ);
                go.transform.localScale = new Vector3(((SceneEntry)scene_.entryList[i]).Radius * 2f, 0.1f, ((SceneEntry)scene_.entryList[i]).Radius * 2f);
                entryObjList_.Add(go);
            }
            ((SceneEntry)scene_.entryList[i]).CenterX = entryObjList_[i].transform.position.x;
            ((SceneEntry)scene_.entryList[i]).CenterY = entryObjList_[i].transform.position.y;
            ((SceneEntry)scene_.entryList[i]).CenterZ = entryObjList_[i].transform.position.z;

            ((SceneEntry)scene_.entryList[i]).Radius = entryObjList_[i].transform.localScale.x / 2f;

            if (GUILayout.Button("Delete"))
            {
                scene_.entryList.RemoveAt(i);
                DestroyImmediate(entryObjList_[i]);
                entryObjList_.RemoveAt(i);
            }
            GUILayout.Space(15);
        }
        GUILayout.EndScrollView();
        GUILayout.Space(10);

        // mutifuncPointInfo
        GUILayout.Box("FunctionalPoints");
        FuncPointScrollPos_ = GUILayout.BeginScrollView(FuncPointScrollPos_, GUILayout.Width(500), GUILayout.Height(400));
        for (int i = 0; i < scene_.funcPointList.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("ID");
            ((FunctionalPoint)scene_.funcPointList[i]).ID = i.ToString();
            GUILayout.Label(i.ToString());
            GUILayout.EndHorizontal();

            GUI.color = Color.yellow;
            GUILayout.BeginHorizontal();
            GUILayout.Label("NpcId");
            GUILayout.Label(((FunctionalPoint)scene_.funcPointList[i]).NpcId);
            GUILayout.EndHorizontal();
            GUI.color = Color.white;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Position" + new Vector3(((FunctionalPoint)scene_.funcPointList[i]).X, ((FunctionalPoint)scene_.funcPointList[i]).Y, ((FunctionalPoint)scene_.funcPointList[i]).Z));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rotation" + new Quaternion(((FunctionalPoint)scene_.funcPointList[i]).RX, ((FunctionalPoint)scene_.funcPointList[i]).RY, ((FunctionalPoint)scene_.funcPointList[i]).RZ, ((FunctionalPoint)scene_.funcPointList[i]).RW));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Type");
            ((FunctionalPoint)scene_.funcPointList[i]).type = (FunctionalPointType)GUILayout.SelectionGrid((int)((FunctionalPoint)scene_.funcPointList[i]).type, new string[] { "FPT_None", "FPT_Tongji", "FPT_Mogu", "FPT_SinglePK", "FPT_TeamPK", "FPT_Npc", "FPT_Xiji" }, 5);
            GUILayout.EndHorizontal();

            if (scene_.funcPointList.Count != funcObjList_.Count)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                go.name = "FunctionalPoint";
                go.GetComponent<MeshRenderer>().material = funcPointMat_;
                go.transform.parent = root.transform;
                go.transform.position = new Vector3(((FunctionalPoint)scene_.funcPointList[i]).X, ((FunctionalPoint)scene_.funcPointList[i]).Y, ((FunctionalPoint)scene_.funcPointList[i]).Z);
                go.transform.rotation = new Quaternion(((FunctionalPoint)scene_.funcPointList[i]).RX, ((FunctionalPoint)scene_.funcPointList[i]).RY, ((FunctionalPoint)scene_.funcPointList[i]).RZ, ((FunctionalPoint)scene_.funcPointList[i]).RW);
                funcObjList_.Add(go);
            }
            ((FunctionalPoint)scene_.funcPointList[i]).X = funcObjList_[i].transform.position.x;
            ((FunctionalPoint)scene_.funcPointList[i]).Y = funcObjList_[i].transform.position.y;
            ((FunctionalPoint)scene_.funcPointList[i]).Z = funcObjList_[i].transform.position.z;
            ((FunctionalPoint)scene_.funcPointList[i]).RX = funcObjList_[i].transform.rotation.x;
            ((FunctionalPoint)scene_.funcPointList[i]).RY = funcObjList_[i].transform.rotation.y;
            ((FunctionalPoint)scene_.funcPointList[i]).RZ = funcObjList_[i].transform.rotation.z;
            ((FunctionalPoint)scene_.funcPointList[i]).RW = funcObjList_[i].transform.rotation.w;

            if (GUILayout.Button("Delete"))
            {
                scene_.funcPointList.RemoveAt(i);
                DestroyImmediate(funcObjList_[i]);
                funcObjList_.RemoveAt(i);
            }
            GUILayout.Space(15);
        }
        GUILayout.EndScrollView();
        GUILayout.Space(10);

        GUILayout.EndScrollView();

		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("AddMonster"))
		{
            GameObject monsterArea = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            monsterArea.transform.localScale = new Vector3(1f, 0.1f, 1f);
            DestroyImmediate(monsterArea.GetComponent<SphereCollider>());
			monsterArea.transform.parent = root.transform;
			Zone zone = new Zone();
            zone.ID = (scene_.zoneList.Count + 1).ToString();
			zoneObjList_.Add(monsterArea);
			scene_.Add(zone);
		}
		
		if(GUILayout.Button("AddEntry"))
		{
			GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.localScale = new Vector3(1f, 0.1f, 1f);
            DestroyImmediate(go.GetComponent<SphereCollider>());
			go.transform.parent = root.transform;
            SceneEntry entry = new SceneEntry();
            entry.ID = (scene_.entryList.Count + 1).ToString();
			entryObjList_.Add(go);
            scene_.Add(entry);
        }

        if (GUILayout.Button("AddFuncPoint"))
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            go.name = "FunctionalPoint";
            go.GetComponent<MeshRenderer>().material = funcPointMat_;
            go.transform.parent = root.transform;
            FunctionalPoint fp = new FunctionalPoint();
            funcObjList_.Add(go);
            scene_.Add(fp);
        }

        if (GUILayout.Button("CombieNpcPoint"))
        {
            for (int i = 0; i < scene_.npcList.Count; ++i)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                go.name = "FunctionalPoint";
                go.GetComponent<MeshRenderer>().material = funcPointMat_;
                go.transform.parent = root.transform;
                go.transform.position = new Vector3(((Npc)scene_.npcList[i]).X, ((Npc)scene_.npcList[i]).Y, ((Npc)scene_.npcList[i]).Z);
                go.transform.rotation = new Quaternion(((Npc)scene_.npcList[i]).RX, ((Npc)scene_.npcList[i]).RY, ((Npc)scene_.npcList[i]).RZ, ((Npc)scene_.npcList[i]).RW);
                funcObjList_.Add(go);
                FunctionalPoint fp = new FunctionalPoint();
                fp.NpcId = ((Npc)scene_.npcList[i]).ID;
                fp.type = FunctionalPointType.FPT_Npc;
                scene_.Add(fp);
            }
            scene_.npcList.Clear();
            npcObjList_.Clear();
        }
        
        // createXML
        if(GUILayout.Button("Save"))
        {
			if(EditorUtility.DisplayDialog("Save", "确定保存数据吗?", "似的", "不"))
			{
				Save(scene_);
				EditorUtility.DisplayDialog("Alert", "保存完毕", "确定");
			}
        }

		// update
		if(GUILayout.Button("Update"))
		{
			UpdateEnviroment();
        }
        
        GUILayout.EndHorizontal ();
        SceneView.RepaintAll();
	}

	static void UpdateEnviroment()
	{
		string content = File.ReadAllText(Application.dataPath + "/../../../Config/Tables/Npc.csv");
		NpcData.ParseData(content, "Npc.csv");
		content = File.ReadAllText(Application.dataPath + "/../../../Config/Tables/EntityAssets.csv");
		EntityAssetsData.ParseData(content, "EntityAssets.csv");
        Dictionary<int, NpcData> allNpc = NpcData.GetData();
        foreach (NpcData nData in allNpc.Values)
		{
            //if (nData.sceneID_ == int.Parse(scene_.ID))
            //{
            //    for (int i = 0; i < scene_.funcPointList.Count; ++i)
            //    {
            //        if (nData.pointID_ == int.Parse(((FunctionalPoint)scene_.funcPointList[i]).ID))
            //        {
            //            string assName = EntityAssetsData.GetData(NpcData.GetData(nData.id_).assetsID_).assetsName_;
            //            string path = "Assets/ResData/Role/Prefabs/" + assName + ".prefab";
            //            GameObject npc = Resources.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            //            npc = (GameObject)GameObject.Instantiate(npc) as GameObject;
            //            npc.name = "NPC_" + nData.id_.ToString();
            //            npc.transform.position = new Vector3(((FunctionalPoint)scene_.funcPointList[i]).X, ((FunctionalPoint)scene_.funcPointList[i]).Y, ((FunctionalPoint)scene_.funcPointList[i]).Z);
            //            npc.transform.rotation = new Quaternion(((FunctionalPoint)scene_.funcPointList[i]).RX, ((FunctionalPoint)scene_.funcPointList[i]).RY, ((FunctionalPoint)scene_.funcPointList[i]).RZ, ((FunctionalPoint)scene_.funcPointList[i]).RW);
            //            Transform parent = funcObjList_[i].transform.parent;
            //            DestroyImmediate(funcObjList_[i]);
            //            funcObjList_[i] = npc;
            //            npc.transform.parent = parent;
            //        }
            //    }
            //}
        }
    }
    
    void OnDestroy()
	{
		DestroyImmediate (root);
	}
}

public class Scene
{
    [XmlAttribute("ChapterID")]
    public string CID { set; get; }
	[XmlAttribute("SceneID")]
	public string ID { set; get; }
	[XmlAttribute("SceneName")]
	public string Name { set; get; }
    [XmlAttribute("BattleName")]
    public string BattleName { set; get; }
	
	[XmlArray("NPCInfo")]
	[XmlArrayItem("NPC", typeof(Npc))]
	public ArrayList npcList;
	
	[XmlArray("ZoneInfo")]
	[XmlArrayItem("Zone", typeof(Zone))]
	public ArrayList zoneList;

	[XmlArray("EntryInfo")]
    [XmlArrayItem("Entry", typeof(SceneEntry))]
	public ArrayList entryList;

    [XmlArray("FuncPInfo")]
    [XmlArrayItem("FunctionPoint", typeof(FunctionalPoint))]
    public ArrayList funcPointList;
	
	public void Add(Zone zone)
	{
		zoneList.Add(zone);
	}
	
	public void Add(Npc npc)
	{
		npcList.Add(npc);
	}

	public void Add(SceneEntry entry)
	{
		entryList.Add(entry);
    }

    public void Add(FunctionalPoint fp)
    {
        funcPointList.Add(fp);
    }
	
	public Scene()
	{
        CID = "";
		ID = "";
		Name = "";
        BattleName = "";
		zoneList = new ArrayList ();
		npcList = new ArrayList ();
		entryList = new ArrayList ();
        funcPointList = new ArrayList();
	}
}

public class Zone
{
	[XmlAttribute("ID")]
	public string ID { set; get; }
	[XmlAttribute("Rate")]
	public string Rate { set; get; } // 0~100
	[XmlAttribute("MonsterMin")]
	public string MonsterMin { set; get; }
	[XmlAttribute("MonsterMax")]
	public string MonsterMax { set; get; }
	[XmlAttribute("Monsters")]
	public string MonsterGroup { set; get; }
    [XmlAttribute("Radius")]
    public float Radius { set; get; }
    [XmlAttribute("CenterX")]
    public float CenterX { set; get; }
    [XmlAttribute("CenterY")]
    public float CenterY { set; get; }
    [XmlAttribute("CenterZ")]
    public float CenterZ { set; get; }

	public Zone()
	{
		ID = "";
		Rate = "";
		MonsterMin = "";
		MonsterMax = "";
		MonsterGroup = "";
	}
}

public class Npc
{
	[XmlAttribute("ID")]
	public string ID { set; get; }
	[XmlAttribute("PositionX")]
	public float X { set; get; }
	[XmlAttribute("PositionY")]
	public float Y { set; get; }
	[XmlAttribute("PositionZ")]
	public float Z { set; get; }
    [XmlAttribute("RotationX")]
    public float RX { set; get; }
    [XmlAttribute("RotationY")]
    public float RY { set; get; }
    [XmlAttribute("RotationZ")]
    public float RZ { set; get; }
    [XmlAttribute("RotationW")]
    public float RW { set; get; }

	public Npc()
	{
		ID = "";
	}
}

public class SceneEntry
{
	[XmlAttribute("ID")]
    public string ID { set; get; }
    [XmlAttribute("AreaNum")]
    public string areaNum { set; get; }
    [XmlAttribute("toSceneID")]
    public string toSceneID { set; get; }
    [XmlAttribute("toEntryID")]
    public string toEntryID { set; get; }
    [XmlAttribute("Radius")]
    public float Radius { set; get; }
    [XmlAttribute("CenterX")]
    public float CenterX { set; get; }
    [XmlAttribute("CenterY")]
    public float CenterY { set; get; }
    [XmlAttribute("CenterZ")]
    public float CenterZ { set; get; }
    [XmlAttribute("IsBornPos")]
    public bool isBornPos { set; get; }

    public SceneEntry()
    {
        ID = "";
        areaNum = "";
        toSceneID = "";
        toEntryID = "";
    }
}

public class FunctionalPoint
{
    [XmlAttribute("ID")]
    public string ID { set; get; }
    [XmlAttribute("PositionX")]
	public float X { set; get; }
	[XmlAttribute("PositionY")]
	public float Y { set; get; }
	[XmlAttribute("PositionZ")]
	public float Z { set; get; }
    [XmlAttribute("RotationX")]
    public float RX { set; get; }
    [XmlAttribute("RotationY")]
    public float RY { set; get; }
    [XmlAttribute("RotationZ")]
    public float RZ { set; get; }
    [XmlAttribute("RotationW")]
    public float RW { set; get; }
    [XmlAttribute("Type")]
    public FunctionalPointType type { set; get; }
    [XmlAttribute("NpcId")]
    public string NpcId { set; get; }

    public FunctionalPoint()
	{
        ID = "";
        NpcId = "";
	}
}
