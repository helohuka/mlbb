using UnityEngine;
using System.Xml;
using System.IO;
using System.Collections.Generic;

public class MazeData {
	
	public int id_;
	
	public string mazeName_;

    public string battleName_;
	
	public string mazeStyle_;

	public int mazeFileId_;
	
	public List<string> npcList_;
	
	public List<Level> levelList_;
	
	private static Dictionary<int, MazeData> metaData_ = new Dictionary<int, MazeData>();

	private static Dictionary<int, MazeElement> mazeElements_ = new Dictionary<int, MazeElement> ();

	public static void ParseData(string content, string fileName)
	{
		if(fileName.Contains("Element"))
		{
			MazeElement element = new MazeElement();
			element.elementId_ = int.Parse(fileName.Substring(fileName.LastIndexOf("_") + 1, fileName.IndexOf(".xml") - 1 - fileName.LastIndexOf("_")));
			MazeElement.ParseXML(content, element);
			mazeElements_.Add(element.elementId_, element);
		}
		else
		{
			MazeData data = new MazeData ();
			data.mazeFileId_ = int.Parse(fileName.Substring(fileName.LastIndexOf("_") + 1, fileName.IndexOf(".xml") - 1 - fileName.LastIndexOf("_")));
			data.Init ();
			
			ParseXML (content, data);
			
			if(metaData_.ContainsKey(data.id_))
			{
				ClientLog.Instance.LogError("Same id in SceneData :" + data.id_);
				return;
			}
			metaData_.Add (data.mazeFileId_, data);
		}
	}
	
	public void Init()
	{
		npcList_ = new List<string> ();
		levelList_ = new List<Level> ();
	}

	public int[] GetSceneEntryByLevel(int level)
	{
		int[] arr = null;
		for(int i=0; i < levelList_.Count; ++i)
		{
			if(levelList_[i].level_ == level)
			{
				arr = new int[]{levelList_[i].entrySceneId_, levelList_[i].entryId_};
				break;
			}
		}
		return arr;
	}

	public bool isMinLevel(int level)
	{
		int min = levelList_.Count;
		for(int i=0; i < levelList_.Count; ++i)
		{
			if(levelList_[i].level_ < min)
				min = levelList_[i].level_;
		}
		return min == level;
	}
	
	public bool isMaxLevel(int level)
	{
		int max = 0;
		for(int i=0; i < levelList_.Count; ++i)
		{
			if(levelList_[i].level_ > max)
				max = levelList_[i].level_;
		}
		return max == level;
	}

	public static MazeData GetData(int id)
	{
		if(!metaData_.ContainsKey(id))
		{
			ClientLog.Instance.LogError("MazeData by " + id + " is null");
			return null;
		}
		return metaData_[id];
	}

	public static MazeData GetDataByStyle(string style)
	{
		foreach(MazeData data in metaData_.Values)
		{
			if(!data.mazeStyle_.Equals(style))
				continue;
			return data;
		}
		return null;
	}

	static void ParseXML(string content, MazeData data)
	{
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (content);
		
		XmlNode rootNode = doc.SelectSingleNode ("/Maze");
		foreach(XmlAttribute attr in rootNode.Attributes)
		{
			if(attr.Name.Equals("MazeID"))
				data.id_ = int.Parse(attr.Value);
			
			if(attr.Name.Equals("MazeName"))
				data.mazeName_ = attr.Value;

            if (attr.Name.Equals("BattleName"))
                data.battleName_ = attr.Value;
			
			if(attr.Name.Equals("MazeStyle"))
				data.mazeStyle_ = attr.Value;

			if(attr.Name.Equals("MazeNpc"))
				data.npcList_ = new List<string>(attr.Value.Split(new char[]{';'}, System.StringSplitOptions.RemoveEmptyEntries));
		}
		
		foreach(XmlNode subnode in rootNode.ChildNodes)
		{
			if(subnode.Name.Equals("MazeZoneInfo"))
			{
				Level level = null;
				foreach(XmlNode node in subnode.ChildNodes)
				{
					level = new Level();
					foreach(XmlAttribute attr in node.Attributes)
					{
						if(attr.Name.Equals("EntrySceneID") && !string.IsNullOrEmpty(attr.Value))
							level.entrySceneId_ = int.Parse(attr.Value);

						if(attr.Name.Equals("EntryID") && !string.IsNullOrEmpty(attr.Value))
							level.entryId_ = int.Parse(attr.Value);
						
						if(attr.Name.Equals("Level"))
							level.level_ = int.Parse(attr.Value);
						
						if(attr.Name.Equals("Monsters"))
							level.monsterIds_ = attr.Value;
						
						if(attr.Name.Equals("Npc"))
							level.npc_ = int.Parse(attr.Value);
					}
					data.levelList_.Add(level);
				}
			}
		}
	}

	public static MazeElement GetElement(int eleId)
	{
		if(!mazeElements_.ContainsKey(eleId))
			return null;
		return mazeElements_ [eleId];
	}

	public static int ElementSize
	{
		get { return mazeElements_.Count; }
	}
}

public class Level
{
	public int level_;
	public int entrySceneId_;
	public int entryId_;
	public string monsterIds_;
	public int npc_;
}

public class Mazelight
{
    public string name_;
    public float intensity_;
    public float x_;
    public float y_;
    public float z_;
    public float range_;
    public float spotAngle_;
    public float cookieSize_;
    public float r_;
    public float g_;
    public float b_;
    public float a_;
    public float rx_;
    public float ry_;
    public float rz_;
    public float rw_;
}

public class MazeElement
{
	public int elementId_;
	public string parent_;
	public float rotateX_;
	public float rotateY_;
	public float rotateZ_;
	public float rotateW_;
	public float x_;
	public float y_;
	public float z_;
	public string name_;
    public List<Mazelight> lights_;

	public void Clone(MazeElement ele)
	{
		this.elementId_ = ele.elementId_;
		this.parent_ = ele.parent_;
		this.rotateX_ = ele.rotateX_;
		this.rotateY_ = ele.rotateY_;
		this.rotateZ_ = ele.rotateZ_;
		this.rotateW_ = ele.rotateW_;
		this.x_ = ele.x_;
		this.y_ = ele.y_;
		this.z_ = ele.z_;
		this.name_ = ele.name_;
        if(ele.lights_ != null)
            this.lights_ = new List<Mazelight>(ele.lights_);
		for(int i=0; i < ele.elements_.Count; ++i)
		{
			elements_.Add(ele.elements_[i]);
		}
	}

	public List<MazeElement> elements_ = new List<MazeElement>();

	public static void ParseXML(string content, MazeElement data)
	{
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (content);

		XmlNode rootNode = doc.SelectSingleNode ("/MazeTerrain");
		MazeElement element = null;
		foreach(XmlNode subNode in rootNode.ChildNodes)
		{
			if(subNode.Name.Equals("cubes"))
			{
				foreach(XmlNode cubeNode in subNode.ChildNodes)
				{
					if(cubeNode.Name.Equals("MCube"))
					{
						element = new MazeElement();
						foreach(XmlAttribute attr in cubeNode.Attributes)
						{
							if(attr.Name.Equals("parent"))
								element.parent_ = attr.Value;
							
							if(attr.Name.Equals("rw"))
								element.rotateW_ = float.Parse(attr.Value);
							
							if(attr.Name.Equals("rx"))
								element.rotateX_ = float.Parse(attr.Value);
							
							if(attr.Name.Equals("ry"))
								element.rotateY_ = float.Parse(attr.Value);
							
							if(attr.Name.Equals("rz"))
								element.rotateZ_ = float.Parse(attr.Value);
							
							if(attr.Name.Equals("x"))
								element.x_ = float.Parse(attr.Value);
							
							if(attr.Name.Equals("y"))
								element.y_ = float.Parse(attr.Value);
							
							if(attr.Name.Equals("z"))
								element.z_ = float.Parse(attr.Value);
							
							if(attr.Name.Equals("name"))
								element.name_ = attr.Value;
						}
					}
					data.elements_.Add(element);
				}

				if(subNode.Name.Equals("lights"))
				{
                    if (data.lights_ == null)
                        data.lights_ = new List<Mazelight>();
                    Mazelight light = new Mazelight();
                    foreach (XmlAttribute attr in subNode.Attributes)
                    {
                        if (attr.Name.Equals("name"))
                            light.name_ = attr.Value;

                        if (attr.Name.Equals("Intensity"))
                            light.intensity_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("Range"))
                            light.range_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("SpotAngle"))
                            light.spotAngle_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("CookieSize"))
                            light.cookieSize_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("x"))
                            light.x_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("y"))
                            light.y_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("z"))
                            light.z_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("a"))
                            light.a_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("r"))
                            light.r_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("g"))
                            light.g_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("b"))
                            light.b_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("rx"))
                            light.rx_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("ry"))
                            light.ry_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("rz"))
                            light.rz_ = float.Parse(attr.Value);

                        if (attr.Name.Equals("rw"))
                            light.rw_ = float.Parse(attr.Value);
                    }
                    data.lights_.Add(light);
				}
			}
		}
	}
}