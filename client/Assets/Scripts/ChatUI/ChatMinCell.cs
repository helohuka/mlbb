using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class ChatMinCell : MonoBehaviour
{
    public UIButton _Audio;
    public UILabel _Text;
	public UILabel _namelabel;
	public UILabel _timeLabel;
	
	public GameObject chatObj;
	public UISprite _reftDh;
	COM_ChatInfo catInfo;
	public UISprite back;
	public Transform chatPos;
	private int audioHight = 36;
	//public SymbolLabel _symbolLabel;
	//public UILabel namelabel;
	public List<string> lineList = new List<string>();
	void Awake()
	{

		prefabCur = new List<GameObject> ();
		//Prefabs = new List<string> ();
	    prefabCeches = new List<GameObject>();
		labelCaches = new List<UILabel>();
		labelCur = new List<UILabel>();
		list = new List<LabelType> ();
		Prefabs .AddRange(ChatSystem.faceStrl);
		//?????  
//		FontSize = 35;  
//		UIPanel panel=gameObject.GetComponent<UIPanel>();  
//		Depth = panel.depth + 1;  
		//MaxWidth = (int)panel.width;  
//		PositionX = 0;  
//		LablePositionY = 0;  
//		ImagePositionY = 0;  
		//ImageHeight = Prefab.GetComponent<UISprite>().height;  
		
//		LableCaches = new List<UILabel>();  
//		ImageCaches = new List<UISprite>();  	
	
	}

	void Start()
	{
		UIManager.SetButtonEventHandler (_Audio.gameObject, EnumButtonEvent.OnClick, OnClickBroadcast, 0, 0);
	}
	string co = "";
	string content = "";
    public COM_ChatInfo Info
    {
        set
        {
			catInfo = value;
			string nameStr = "";
			string text = "";

			_namelabel.gameObject.SetActive(false);
			if(value.ck_ == ChatKind.CK_System)
			{
				co = "[ff4646]";
				_namelabel.color = new Color(255/255.0f,70/255.0f,70/255.0f);
			}else if(value.ck_ == ChatKind.CK_World)
			{
				co = "[18ff00]";
				_namelabel.color = new Color(24/255.0f,255/255.0f,0);
			}else if(value.ck_ == ChatKind.CK_Team)
			{
				co = "[52c0ff]";
				_namelabel.color = new Color(82/255.0f,192/255.0f,255/255.0f);
			}else if(value.ck_ == ChatKind.CK_Friend)
			{
				co = "[ffd200]";
				_namelabel.color = new Color(250/255.0f,222/255.0f,0);
			}
			nameStr = "["+ LanguageManager.instance.GetValue(value.ck_.ToString())+"]";
			//_namelabel.text = "["+ LanguageManager.instance.GetValue(value.ck_.ToString())+"]";
            if (!string.IsNullOrEmpty(value.content_))
            {

                _Audio.gameObject.SetActive(false);
                _Text.gameObject.SetActive(false);
				if(catInfo.teamId_ != 0)
				{
					content = LanguageManager.instance.GetValue ("yijianhanhua").Replace ("{n}",LanguageManager.instance.GetValue(catInfo.teamType_.ToString())).Replace ("{n1}", catInfo.teamMinLevel_.ToString()).Replace ("{n2}", catInfo.teamMaxLevel_.ToString()).Replace("{t1}","1").Replace("{t2}",catInfo.teamId_.ToString());
				}else
				{
					content = value.content_;
				}
				text = nameStr + LanguageManager.instance.GetValue("chatmin").Replace("{t1}","4").Replace("{t2}",value.instId_.ToString()).Replace("{t3}",value.playerName_).Replace("{t4}","[29fff2]"+"["+value.playerName_+"]"+"[-]")+content ;
				//_Text.text = LanguageManager.instance.GetValue("chatName").Replace("{t1}","4").Replace("{t2}",value.instId_.ToString()).Replace("{t3}",value.playerName_).Replace("{t4}",value.playerName_);
				Reset();
				//list.Add(new LabelType(co +nameStr +"[-]", InfoType.Text));
				//list.Add(new LabelType(LanguageManager.instance.GetValue("chatName").Replace("{t1}","4").Replace("{t2}",value.instId_.ToString()).Replace("{t3}",value.playerName_).Replace("{t4}",value.playerName_), InfoType.Text));
				list.Add(new LabelType("", InfoType.Text));
				ParseSymbol(text);				
				ShowSymbol(list);				
				NGUITools.UpdateWidgetCollider(back.gameObject);		
				chatPos.localPosition = new Vector3(0,0,chatPos.localPosition.z);
				back.height = line*_Text.fontSize+offset;
            }
            else if (value.audioId_ != 0)
            {
				Reset();
//				string str = "["+ LanguageManager.instance.GetValue(value.ck_.ToString())+"]"+LanguageManager.instance.GetValue("chatName").Replace("{t1}","4").Replace("{t2}",value.instId_.ToString()).Replace("{t3}",value.playerName_).Replace("{t4}",value.playerName_);
//				list.Add(new LabelType("", InfoType.Text));
//				ParseSymbol(str);				
//				ShowSymbol(list);
				_namelabel.text = "["+ LanguageManager.instance.GetValue(value.ck_.ToString())+"]";
				_Text.text = LanguageManager.instance.GetValue("chatName").Replace("{t1}","4").Replace("{t2}",value.instId_.ToString()).Replace("{t3}",value.playerName_).Replace("{t4}",value.playerName_);
				//audios = gameObject.GetComponent<AudioSource>();

//				if (audios.clip != null)
//                {
				    _timeLabel.text = value.audioTime_.ToString();
                    _Audio.gameObject.SetActive(true);
                    _Text.gameObject.SetActive(true);
				    _namelabel.gameObject.SetActive(true);
				_Audio.transform.localPosition = new Vector3(_Text.width+_namelabel.width+100,_Audio.transform.localPosition.y,_Audio.transform.localPosition.z);
					//_Text.text = "[29fff2]"+"["+value.playerName_+"]"+"[-]";
//                }
				chatPos.localPosition = new Vector3(0,0,chatPos.localPosition.z);
				back.height = audioHight+offset;
                ///≤›¡À–¥ ≤√¥
            }
        }
		get
		{
			return catInfo;
		}
    }
	public void closeVecUI()
	{
        ChatSystem.finishAudioOk -= closeVecUI;
		UISpriteAnimation rspani = _reftDh.GetComponent<UISpriteAnimation> ();
		if (rspani != null)
			rspani.enabled = false;
		rspani.transform.GetComponent<UISprite> ().spriteName = "yuyin1";
	}
	void startAudio(int id)
	{
		UISpriteAnimation rspani = _reftDh.GetComponent<UISpriteAnimation> ();
		if (rspani != null)
			rspani.enabled = true;

		ChatSystem.startAudioOk -= startAudio;
	}
	void OnClickBroadcast(ButtonScript obj, object args, int param1, int param2)
	{
        if (ChatSystem.isPlayingAudio)
            return;

		ChatSystem.startAudioOk += startAudio;
        ChatSystem.finishAudioOk += closeVecUI;
		NetConnection.Instance.requestAudio (Info.audioId_);
		Transform tr = _Audio.transform.FindChild ("newvec");
		tr.gameObject.SetActive (false);
	}
	public enum InfoType
	{
		Text,
		Face,
	}
	
	public class LabelType
	{
		public string info;
		public InfoType type;
		public int labelWidth;
		
		public LabelType(string text,InfoType tp)
		{
			info = text;
			type = tp;
		}
	}
	private static List<LabelType> list;
	private int positionX = 0;
	private int positionY = -6;
	private List<GameObject> prefabCeches;
	private List<UILabel> labelCaches;
	private List<UILabel> labelCur;
	public int cellHeight = 28;
	private int labelNameWidth;
	private int MaxLineWidth = 400;
	public string infoColor;
	public List<string> Prefabs;
	public GameObject Label;
	public GameObject FacePrefab;
	private int offset = 10;
	public int line = 1;
	private List<GameObject> prefabCur;
	private static void ParseSymbol(string value)
	{
		if (string.IsNullOrEmpty(value))
			return;
		
		int startIndex = 0;
		int endIndex = 0;
		string startString;
		string endString = value;
		
		string pattern = "\\{\\w\\w\\}";
		MatchCollection matchs = Regex.Matches(value, pattern);
		string str;
		
		if (matchs.Count > 0)
		{
			foreach (Match item in matchs)
			{
				str = item.Value;
				startIndex = endString.IndexOf(str);
				endIndex = startIndex + str.Length;
				
				if (startIndex > -1)
				{
					startString = endString.Substring(0, startIndex);
					
					if (!string.IsNullOrEmpty(startString))
						list.Add(new LabelType(startString, InfoType.Text));
					
					if (!string.IsNullOrEmpty(str))
						list.Add(new LabelType(str, InfoType.Face));
					
					endString = endString.Substring(endIndex);
				}
			}
			
			if (!string.IsNullOrEmpty(endString))
				list.Add(new LabelType(endString, InfoType.Text));
		}
		else
		{
			list.Add(new LabelType(endString, InfoType.Text));
		}
	}
	
	private void ShowSymbol(List<LabelType> list)
	{
		foreach (LabelType item in list)
		{
			switch (item.type)
			{
			case InfoType.Text :
				CreateTextLabel(item.info);
				break;
			case  InfoType.Face :
				CreateFace(item.info);
				break;
			}
		}
	}
	
	private void CreateTextLabel(string value)
	{
		
		UILabel label;
		if (labelCaches.Count > 0)
		{
			label = labelCaches[0];
			labelCaches.Remove(label);
			label.gameObject.SetActive(true);
		}
		else
		{
			GameObject go = GameObject.Instantiate(Label) as GameObject;
			go.SetActive(true);
			go.transform.parent = chatPos.transform;
			label = go.GetComponent<UILabel>();
			go.transform.localScale = Vector3.one;
		}

		string sbstr = "";
		string text = "";
		
		NGUIText.fontSize = 27;
		NGUIText.finalSize = 27;
		NGUIText.dynamicFont = label.trueTypeFont;
		NGUIText.rectWidth = MaxLineWidth - positionX;
		NGUIText.maxLines = 10000;
		NGUIText.rectHeight = 10000;

		NGUIText.WrapText(value, out sbstr);
		int index = sbstr.IndexOf("\n");
		
		if (index > -1)
		{
			text = sbstr.Substring(0, index);
		}
		else
		{
			text = sbstr;
		}

		if(!string.IsNullOrEmpty(infoColor))
			label.text = co + text + "[-]";
		else
			label.text = co + text + "[-]";
		
		label.gameObject.transform.localPosition = new Vector3(positionX, positionY, 0);
		
		positionX += (label.width);
		
		labelCur.Add(label);
		
		sbstr = sbstr.Remove(0, text.Length);
		
		if (labelNameWidth == 0)
			labelNameWidth = label.width;

		if(label.text.Contains("[url=")||label.text.StartsWith("[url="))
		{
			if(label.GetComponent<BoxCollider>() == null)
			{
				label.gameObject.AddComponent<BoxCollider>();
				NGUITools.UpdateWidgetCollider(label.gameObject);
			}

		}

		if (sbstr.Length > 0)
		{
			line++;
			positionX =labelNameWidth;
			positionY -= (cellHeight+4);
			//back.height += (cellHeight+offset);			
			sbstr = sbstr.Replace("\n", "");
			CreateTextLabel(sbstr);
		}

	}
	
	private void CreateFace(string value)
	{

		int index = Prefabs.IndexOf(value);
		if (index > -1)
		{
			GameObject face;
			UIWidget widget;
			
			if (prefabCeches.Count > 0)
			{
				face = prefabCeches[0];
				prefabCeches.Remove(face);
				face.SetActive(true);
			}
			else
			{
				face = GameObject.Instantiate(FacePrefab) as GameObject;
				face.transform.parent = chatPos.transform;
				face.transform.localScale = FacePrefab.transform.localScale;
				face.SetActive(true);
			}
			
			UISprite sprite = face.GetComponent<UISprite>();
			sprite.spriteName = value;
			widget = face.GetComponent<UIWidget>();
			widget.pivot = UIWidget.Pivot.TopLeft;
			
			
			if (MaxLineWidth < (positionX + widget.width))
			{
				positionX =labelNameWidth;
				positionY -= (cellHeight+14);
				line++;
				//back.height += cellHeight+offset;
			}
			
			face.transform.localPosition = new Vector3(positionX, positionY, 0);
			
			positionX += widget.width;
			//back.height = widget.height+30;
			prefabCur.Add(face);
		}
		else
		{
			CreateTextLabel(value);
		}
	}
	
	private void Reset()
	{
		positionX = 0;
		positionY = 0;
		line = 1;
		labelNameWidth = 0;
		back.height = cellHeight;
		list.Clear();
		
		while (labelCur.Count > 0)
		{
			UILabel lab = labelCur[0];
			
			labelCur.Remove(lab);
			labelCaches.Add(lab);
			lab.gameObject.SetActive(false);
		}
		
		while (prefabCur.Count > 0)
		{
			GameObject go = prefabCur[0];
			
			prefabCur.Remove(go);
			prefabCeches.Add(go);
			go.SetActive(false);
		}
	}

	void aaa(string value,out string str)
	{
		str = "";
		string temp = "";
		float chaarWidth = 0;
		for(int i=0;i<value.Length;i++)
		{
			chaarWidth += NGUIText.GetGlyphWidth(value[i],0);
			temp+=value[i].ToString();

			if(chaarWidth>(460-78))
			{
				temp+="\n";
			}
		}
		str = temp;

	}


}