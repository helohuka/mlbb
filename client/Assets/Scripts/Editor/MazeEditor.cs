using UnityEditor;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class MazeEditor : EditorWindow {
	[MenuItem("Tools/MazeEditor")]
   
	static void Open()
	{
		Init ();
		EditorWindow.GetWindow (typeof(MazeEditor));
	}
	static GameObject root = null;
	static Maze maze; 
	static MazeTerrain mTerrain;
	static string sfloor="";
	static string sEntrySceneId = "";
	static string sEntryId = "";
    static  bool isCreate = false;
	static bool isCreateMazeZone = false;
	static bool isbegin = true;
	static bool isXMLInfoEditor = false;
	static string xmlPath = "";
	static string MazeMonster = "";
	static string MazeMonsterMaxNum = "";
	static string MazeMonsterMinNum = "";
	static string rate = "";
	static Vector2 ZoneScrollPos_;
	public int toolbarInt = 0;
	public string[] toolbarStrings = new string[] {"Maze", "MazeElement"};
	public static List<GameObject> lightobjs_ = new List<GameObject> ();
	public  static List<GameObject> Cubeobjs_ = new List<GameObject> ();
	static void Init()
	{
	
		maze = new Maze ();
		mTerrain = new MazeTerrain ();
		mTerrain.cubes_ = new ArrayList ();
		mTerrain.lights_ = new ArrayList ();
		//lightobjs_ = new List<GameObject> ();
		//Cubeobjs_ = new List<GameObject> ();
		maze.zoneList = new ArrayList ();
		isCreate = false;
		isbegin = true;
		isXMLInfoEditor = false;
		root = GameObject.Find ("MazeRoot");
		if (root == null)
			root = new GameObject ("MazeRoot");

	}

	void CreateLight()
	{
		GUILayout.BeginVertical();
		if(GUILayout.Button("AddDirectional"))
		{
			GameObject Directional = new GameObject();
			Directional.name = LightType.Directional.ToString();
			Directional.transform.parent = root.transform;
			Directional.transform.position = Vector3.zero;
			Directional.AddComponent<Light>();
			Light light = Directional.GetComponent<Light>();
			light.type = LightType.Directional;
			lightobjs_.Add(Directional);


		}
		if(GUILayout.Button("AddPointLight"))
		{
			GameObject Point = new GameObject();
			Point.name = LightType.Point.ToString();
			Point.transform.parent = root.transform;
			Point.transform.position = Vector3.zero;
			Point.AddComponent<Light>();
			Light light = Point.GetComponent<Light>();
			light.type = LightType.Point;
			lightobjs_.Add(Point);

		}
		if(GUILayout.Button("AddSpotLight"))
		{
			GameObject SpotLight = new GameObject();
			SpotLight.name = LightType.Spot.ToString();
			SpotLight.transform.parent = root.transform;
			SpotLight.transform.position = Vector3.zero;
			SpotLight.AddComponent<Light>();
			Light light = SpotLight.GetComponent<Light>();
			light.type = LightType.Spot;
			lightobjs_.Add(SpotLight);


		}

		GUILayout.EndVertical ();
	}
	List<GameObject>clones = new List<GameObject>();
	List<GameObject>prs = new List<GameObject>();

	public void huifu()
	{
		for (int j = 0; j<mTerrain.cubes_.Count; j++) {
			
			if (root == null)
				root = new GameObject ("MazeRoot");
			string name = ((MCube)mTerrain.cubes_[j]).Name;

			if(name.Contains("_"))
			{
				string []strs = name.Split('_');

				if(strs.Length==3)
				{
					string path = string.Format ("Assets/ResData/Maze/{0}/{1}/pre/{2}.prefab",strs[0],strs[1],name);
					GameObject go = Resources.LoadAssetAtPath(path,typeof(GameObject))as GameObject;
					GameObject clone = GameObject.Instantiate(go)as GameObject;
					clone.name = ((MCube)mTerrain.cubes_[j]).Name;
					clone.transform.parent = root.transform;				    
					clone.transform.position = new Vector3(((MCube)mTerrain.cubes_[j]).x,((MCube)mTerrain.cubes_[j]).y,((MCube)mTerrain.cubes_[j]).z);
					clone.transform.rotation = new Quaternion(((MCube)mTerrain.cubes_[j]).rx,((MCube)mTerrain.cubes_[j]).ry,((MCube)mTerrain.cubes_[j]).rz,((MCube)mTerrain.cubes_[j]).rw);
				}

				
			}
			
		}


		for (int i = 0; i<mTerrain.lights_.Count; i++) {
			GameObject go = new GameObject();
			go.AddComponent<Light>();
			go.transform.parent = root.transform;
			go.name = ((Mlight)mTerrain.lights_[i]).Name;
			go.transform.position = new Vector3(((Mlight)mTerrain.lights_[i]).x,((Mlight)mTerrain.lights_[i]).y,((Mlight)mTerrain.lights_[i]).z);
			go.transform.rotation = new Quaternion(((Mlight)mTerrain.lights_[i]).rx,((Mlight)mTerrain.lights_[i]).ry,((Mlight)mTerrain.lights_[i]).rz,0);
			Light lg = go.GetComponent<Light>();
			lg.color = new Color(((Mlight)mTerrain.lights_[i]).r,((Mlight)mTerrain.lights_[i]).g,((Mlight)mTerrain.lights_[i]).b,((Mlight)mTerrain.lights_[i]).a);
			lg.intensity = ((Mlight)mTerrain.lights_[i]).Intensity;
			lg.range = ((Mlight)mTerrain.lights_[i]).Range;
			lg.spotAngle = ((Mlight)mTerrain.lights_[i]).SpotAngle;
			lg.cookieSize = ((Mlight)mTerrain.lights_[i]).CookieSize;
		}
		
	}
	bool isShowEnemyInfo;
	public void OnGUI()
	{
		GUILayout.BeginVertical();
		toolbarInt = GUILayout.Toolbar (toolbarInt,toolbarStrings);
		if (toolbarInt == 0)
		{
			if (isCreate) 
			{
				XMLInfoEditor();
			}
			if(isbegin)
			{
				if (GUILayout.Button ("Create")) 
				{
					isCreate = true;
					isbegin = false;
				}
				if (GUILayout.Button ("Load")) 
				{
					string path =	string.Format ("{0}/../../../Config/Tables/Maze/", Application.dataPath);
					isbegin = false;
					isCreate = true;
					xmlPath = EditorUtility.OpenFilePanel("",path,"");
					Load();
				}
			}
		
		}
		else 
		if(toolbarInt==1)
		{


			if (GUILayout.Button ("Load")) 
			{
				string path =	string.Format ("{0}/../../../Config/Tables/Maze/", Application.dataPath);
				isbegin = false;
				isCreate = true;
				xmlPath = EditorUtility.OpenFilePanel("",path,"");
				LoadEnemy();
				huifu();
			}
			CreateLight();	
			scrollPosition2 = GUILayout.BeginScrollView(scrollPosition2, GUILayout.Width(200), GUILayout.Height(200));
			AssetModelsEditor();
			GUILayout.EndScrollView ();
			if(GUILayout.Button("save"))
			{
				mTerrain.cubes_.Clear();
				mTerrain.lights_.Clear();
				Traversal(root.transform);
				CreateCubeXml(mTerrain);
			}


		}
	
		GUILayout.EndVertical ();
	}
	void OnDestroy()
	{
		lightobjs_.Clear ();
		Cubeobjs_.Clear ();

		DestroyImmediate (root);
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
				maze = (Maze)DeserializeObject(content);
		}
	}

	static void CreateXml(Maze smaze)
	{		
		string data = SerializeObject (smaze);
		StreamWriter writer;

		string savePath = string.Format ("{0}/../../../Config/Tables/Maze/Maze_{1}.xml", Application.dataPath, smaze.MazeID);
		FileInfo file = new FileInfo (savePath);
		writer = file.CreateText ();
		writer.Write (data);
		writer.Close ();
		isXMLInfoEditor = true;
	}
	
	static void XMLInfoEditor()
	{
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("MazeID");
		maze.MazeID = GUILayout.TextField (maze.MazeID);
		GUILayout.Label ("MazeName");
		maze.MazeName = GUILayout.TextField (maze.MazeName);
        GUILayout.Label("BattleName");
        maze.BattleName = GUILayout.TextField(maze.BattleName);
		GUILayout.Label ("MazeStyle");
		maze.MazeStyle = GUILayout.TextField (maze.MazeStyle);
		GUILayout.Label ("MazeNpc");
		maze.MazeNpc = GUILayout.TextField (maze.MazeNpc);


		GUILayout.EndHorizontal ();
		
		for(int i = 0;i<maze.zoneList.Count;i++)
		{
			GUILayout.BeginVertical();
			ZoneScrollPos_ = GUILayout.BeginScrollView (ZoneScrollPos_, GUILayout.Width(200), GUILayout.Height(100));
			GUILayout.Label("mfloor");
			((MazeZone)maze.zoneList[i]).Level = GUILayout.TextField(((MazeZone)maze.zoneList[i]).Level);
			GUILayout.Label("mEntrySceneID");
			((MazeZone)maze.zoneList[i]).EntrySceneID = GUILayout.TextField(((MazeZone)maze.zoneList[i]).EntrySceneID);
			GUILayout.Label("mEntryID");
			((MazeZone)maze.zoneList[i]).EntryID = GUILayout.TextField(((MazeZone)maze.zoneList[i]).EntryID);
			GUILayout.Label("MazeMonster");
			((MazeZone)maze.zoneList[i]).MazeMonster = GUILayout.TextField(((MazeZone)maze.zoneList[i]).MazeMonster);
			GUILayout.Label("MazeMonsterMaxNum");
			((MazeZone)maze.zoneList[i]).MazeMonsterMaxNum = GUILayout.TextField(((MazeZone)maze.zoneList[i]).MazeMonsterMaxNum);
			GUILayout.Label("MazeMonsterMinNum");
			((MazeZone)maze.zoneList[i]).MazeMonsterMinNum = GUILayout.TextField(((MazeZone)maze.zoneList[i]).MazeMonsterMinNum);
			GUILayout.Label("Rate");
			((MazeZone)maze.zoneList[i]).Rate = GUILayout.TextField(((MazeZone)maze.zoneList[i]).Rate);

			if(GUILayout.Button("Delete"))
			{
				maze.zoneList.RemoveAt(i);
			}
			GUILayout.EndScrollView ();
			GUILayout.EndVertical ();
		}
		
		GUILayout.BeginVertical();
		if (GUILayout.Button ("AddMzone")) {
			MazeZone mzone = new MazeZone();
			mzone.EntrySceneID = sEntrySceneId;
			mzone.EntryID = sEntryId;
			mzone.Level = sfloor;
			mzone.MazeMonster = MazeMonster;
			mzone.MazeMonsterMaxNum = MazeMonsterMaxNum;
			mzone.MazeMonsterMinNum = MazeMonsterMinNum;
			mzone.Rate = rate;

			maze.Add(mzone);
		}

		if (GUILayout.Button ("Save")) {


			CreateXml(maze);				
		}
		GUILayout.EndVertical ();
	}
	string pr = "";
	void Traversal(Transform t)
	{
		for (int i = 0; i<t.childCount; i++) {
			Transform tt = t.GetChild(i);
			if(tt.GetComponent<Light>()!=null)
			{
				Light l = tt.GetComponent<Light>();	
				Mlight mlight = new Mlight();
				mlight.Name = l.type.ToString();
				mlight.Intensity = l.intensity;
				mlight.Range = l.range;
				mlight.SpotAngle = l.spotAngle;
				mlight.CookieSize = l.cookieSize;
				mlight.x = l.transform.position.x;
				mlight.y = l.transform.position.y;
				mlight.z = l.transform.position.z;
				mlight.r = l.color.r;
				mlight.g = l.color.g;
				mlight.b = l.color.b;
				mlight.a = l.color.a;
				mlight.rx = l.transform.rotation.x;
				mlight.ry = l.transform.rotation.y;
				mlight.rz = l.transform.rotation.z;
				mlight.rw = l.transform.rotation.w;
				mTerrain.Add(mlight);
			}else
			{
				MCube mcube = new MCube();
				mcube.Name = tt.name;
				mcube.x = tt.position.x;
				mcube.y = tt.position.y;
				mcube.z = tt.position.z;
				mcube.rx = tt.rotation.x;
				mcube.ry = tt.rotation.y;
				mcube.rz = tt.rotation.z;
				mcube.rw = tt.rotation.w;
				mcube.parent =tt.parent.name;
				mTerrain.Add(mcube);
			}

			Traversal(tt);
		}
	}

	static string SerializeObject(object o)
	{
		string XmlizedString = "";
		MemoryStream ms = new MemoryStream ();
		XmlSerializer xs = new XmlSerializer (typeof(Maze));
		XmlTextWriter tw = new XmlTextWriter (ms, new System.Text.UTF8Encoding(false));
		xs.Serialize (tw, o);
		ms = (MemoryStream)tw.BaseStream;
		XmlizedString = UTF8ByteArrayToString(ms.ToArray());
		return XmlizedString;
	}
	
	static object DeserializeObject(string pXmlizedString)   
	{
		XmlSerializer xs = new XmlSerializer(typeof(Maze));
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

	 Vector2 scrollPosition = Vector2.zero;
	Vector2 scrollPosition1 = Vector2.zero;
	Vector2 scrollPosition2 = Vector2.zero;
	 float vSbarValue;

	static void AssetModelsEditor()
	{

		Object [] objs = CreateAssetModels.GetAssetModels();
		foreach(Object o in objs)
		{

			if(GUILayout.Button(o.ToString()))
			{				 
				GameObject cloneModel =	(GameObject)GameObject.Instantiate(o);
				string []sps = cloneModel.name.Split('(');
				cloneModel.name = sps[0];
				cloneModel.transform.parent = root.transform;
//				MCube mcube = new MCube();
//				mTerrain.Add(mcube);			
//				Cubeobjs_.Add(cloneModel);
			}
		}


	}




	/// <summary>
	/// CUbe
	/// </summary>
	/// <param name="smaze">Smaze.</param>
	/// 
	/// 
	/// 
	/// 
	static string fileName = "";
	static string fileNum = "";
	static void LoadEnemy()
	{
		FileInfo fi = new FileInfo (xmlPath);
		if(fi.Exists)
		{
			string [] name = fi.Name.Split ('.');
			string [] nums = name [0].Split ('_');
			fileNum = nums[1];
			StreamReader sr = fi.OpenText();
			string content = sr.ReadToEnd();
			sr.Close();
			if(content.ToString() != "")
				mTerrain = (MazeTerrain)DeserializeEnemyObject(content);
		}
	}

	static object DeserializeEnemyObject(string pXmlizedString)   
	{
		XmlSerializer xs = new XmlSerializer(typeof(MazeTerrain));
		MemoryStream ms = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		//		XmlTextWriter tw = new XmlTextWriter(ms, new System.Text.UTF8Encoding(false));
		return xs.Deserialize(ms);
	}

	static void CreateCubeXml(MazeTerrain smaze)
	{		
	    string data = SerializeCubeObject (smaze);
		StreamWriter writer;
		fileName = "Element_";
		if (maze.MazeID == "") {
			maze.MazeID = fileNum;	
		}

		string savePath = string.Format ("{0}/../../../Config/Tables/Maze/{1}{2}.xml", Application.dataPath,fileName,maze.MazeID);
		FileInfo file = new FileInfo (savePath);

		writer = file.CreateText ();
		writer.Write (data);
		writer.Close ();

	}
	static string SerializeCubeObject(object o)
	{
		string XmlizedString = "";
		MemoryStream ms = new MemoryStream ();
		XmlSerializer xs = new XmlSerializer (typeof(MazeTerrain));
		XmlTextWriter tw = new XmlTextWriter (ms, new System.Text.UTF8Encoding(false));
		xs.Serialize (tw, o);
		ms = (MemoryStream)tw.BaseStream;
		XmlizedString = UTF8ByteArrayToString(ms.ToArray());
			return XmlizedString;
	}
		
    static object DeserializeCubeObject(string pXmlizedString)   
	{
	  XmlSerializer xs = new XmlSerializer(typeof(MazeTerrain));
	  MemoryStream ms = new MemoryStream(StringToUTF8ByteArray(pXmlizedString));
		//		XmlTextWriter tw = new XmlTextWriter(ms, new System.Text.UTF8Encoding(false));
	  return xs.Deserialize(ms);
	}
	/// <summary>
	/// CUbeEND
	/// </summary>
	/// <param name="smaze">Smaze.</param>





}

public class Maze
{
	[XmlAttribute("MazeID")]
	public string MazeID { set; get; }
	[XmlAttribute("MazeName")]
	public string MazeName { set; get; }
    [XmlAttribute("BattleName")]
    public string BattleName { set; get; }
	[XmlAttribute("MazeStyle")]
	public string MazeStyle{ set; get; }
	[XmlAttribute("MazeNpc")]
	public string MazeNpc  { set; get; }


	[XmlArray("MazeZoneInfo")]
	[XmlArrayItem("MazeZone", typeof(MazeZone))]
	public ArrayList zoneList;

	public void Add(MazeZone zone)
	{
		zoneList.Add(zone);
	}

	public Maze()
	{
		MazeID = "";
		MazeName = "";
        BattleName = "";
		MazeStyle = "";
		MazeNpc = "";

	}
}
public class MazeZone
{
	[XmlAttribute("Level")]
	public string Level{ set; get; }
	[XmlAttribute("EntrySceneID")]
	public string EntrySceneID{ set; get; }
	[XmlAttribute("EntryID")]
	public string EntryID{ set; get; }
	[XmlAttribute("MazeMonster")]
	public string MazeMonster  { set; get; }

	[XmlAttribute("MazeMonsterMaxNum")]
	public string MazeMonsterMaxNum  { set; get; }
	[XmlAttribute("Rate")]
	public string Rate  { set; get; }
	[XmlAttribute("MazeMonsterMinNum")]
	public string MazeMonsterMinNum  { set; get; }

	public MazeZone()
	{
		Level = "";
		EntrySceneID = "";
		EntryID = "";
		MazeMonster = "";
		MazeMonsterMaxNum = "";
		MazeMonsterMinNum = "";
		Rate = "";
	}

}
/// <summary>
/// MCube
///// </summary>
public class MazeTerrain
{
	[XmlArray("cubes")]
	[XmlArrayItem("MCube", typeof(MCube))]
	public ArrayList cubes_;



	[XmlArray("lights")]
	[XmlArrayItem("Mlight", typeof(Mlight))]
	public ArrayList lights_;







	public void Add(Mlight zone)
	{
		lights_.Add(zone);
	}

	public void Add(MCube zone)
	{
		cubes_.Add(zone);
	}

}

public class MCube
{

	[XmlAttribute("name")]
	public string Name{ set; get; }
	[XmlAttribute("x")]
	public float x{ set; get; }
	[XmlAttribute("y")]
	public float y{ set; get; }
	[XmlAttribute("z")]
	public float z{ set; get; }
	[XmlAttribute("rx")]
	public float rx{ set; get; }
	[XmlAttribute("ry")]
	public float ry{ set; get; }
	[XmlAttribute("rz")]
	public float rz{ set; get; }
	[XmlAttribute("rw")]
	public float rw{ set; get; }
	[XmlAttribute("parent")]
	public string parent{ set; get; }

}

public class Mlight
{
	[XmlAttribute("name")]
	public string Name{ set; get; }
	[XmlAttribute("Intensity")]
	public float Intensity{ set; get; }
	[XmlAttribute("x")]
	public float x{ set; get; }
	[XmlAttribute("y")]
	public float y{ set; get; }
	[XmlAttribute("z")]
	public float z{ set; get; }
	[XmlAttribute("Range")]
	public float Range{ set; get; }
	[XmlAttribute("SpotAngle")]
	public float SpotAngle{ set; get; }
	[XmlAttribute("CookieSize")]
	public float CookieSize{ set; get; }
	[XmlAttribute("r")]
	public float r{ set; get; }
	[XmlAttribute("g")]
	public float g{ set; get; }
	[XmlAttribute("b")]
	public float b{ set; get; }
	[XmlAttribute("a")]
	public float a{ set; get; }
	[XmlAttribute("rx")]
	public float rx{ set; get; }
	[XmlAttribute("ry")]
	public float ry{ set; get; }
	[XmlAttribute("rz")]
	public float rz{ set; get; }
	[XmlAttribute("rw")]
	public float rw{ set; get; }
}


public class CreateAssetModels
{
	public static Object[] GetAssetModels()
	{
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		return SelectedAsset;
	}
}