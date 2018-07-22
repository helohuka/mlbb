using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class ChatMaxCell : MonoBehaviour
{
	public UISprite _Background;
    public UISprite _RChatBackground;
    public UILabel _RChatContent;
    public UIButton _RAudioB;
    public UIButton _RIconB;
    public UITexture _RIconImage;
    public UILabel _RName;
    public UISprite _RChatKindBackground;
    public UILabel _RChatKind;
	public UILabel _rtimeLabel;
	public UISprite _reftDh;
    public UISprite _LSystemFlag;
    public UISprite _LChatBackground;
    public UILabel _LChatContent;
    public UIButton _LAudioB;
    public UIButton _LIconB;
    public UITexture _LIconImage;
    public UILabel _LName;
    public UISprite _LChatKindBackground;
    public UILabel _LChatKind;
	public UILabel _leftTime;
	public UISprite _leftDh;
	public GameObject tipsObj;
	bool isstrat;
	bool isOne = true;
	List<char> chars = new List<char> ();
	COM_ChatInfo _Right;
	COM_ChatInfo _Light;
	public GameObject chatObj;
	public int offset = 25;
	public int lwidthOffset = 20;
	string Lconte = "";
	string Rconte = "";
	void Awake()
	{
		LprefabCur = new List<GameObject> ();
		//Prefabs = new List<string> ();
		LprefabCeches = new List<GameObject>();
		LlabelCaches = new List<UILabel>();
		LlabelCur = new List<UILabel>();

		RprefabCur = new List<GameObject> ();
		//Prefabs = new List<string> ();
		RprefabCeches = new List<GameObject>();
		RlabelCaches = new List<UILabel>();
		RlabelCur = new List<UILabel>();


		maxLlist = new List<LabelType> ();
		maxRlist = new List<LabelType> ();
		Prefabs.AddRange( ChatSystem.faceStrl);
	}
	void Start()
	{
        UIManager.SetButtonEventHandler(_RAudioB.gameObject, EnumButtonEvent.OnClick, OnClickBroadcast, 1, 0);
        UIManager.SetButtonEventHandler(_LAudioB.gameObject, EnumButtonEvent.OnClick, OnClickBroadcast, 2, 0);
	}

    public COM_ChatInfo Right
    {
        set
        {
            //always show
			_Right = value;
            _RChatBackground.Show();
           // _RChatContent.Show();
            _RAudioB.Show();
            _RIconB.Show();
            _RIconImage.Show();
            _RName.Show();
            _RChatKindBackground.Show();
            _RChatKind.Show();
            _RName.text = value.playerName_;
			_RChatBackground.gameObject.SetActive(true);
            _RChatKind.text = LanguageManager.instance.GetValue(Enum.GetName(typeof(ChatKind), value.ck_));
            HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(value.assetId_).assetsIocn_, _RIconImage);
            _RIconImage.width = _RIconImage.height = 100;
            //value.audio_ ??
            if (!string.IsNullOrEmpty(value.content_))
            {
				isOne = true;
                _RAudioB.Hide();
                _RChatBackground.Show();

				_RChatContent.text ="";
				Reset();	
				maxRlist.Add(new LabelType("", InfoType.Text));
				if(value.teamId_ != 0)
				{
					Rconte = LanguageManager.instance.GetValue ("yijianhanhua").Replace ("{n}",LanguageManager.instance.GetValue(value.teamType_.ToString())).Replace ("{n1}", value.teamMinLevel_.ToString()).Replace ("{n2}", value.teamMaxLevel_.ToString()).Replace("{t1}","1").Replace("{t2}",value.teamId_.ToString());
				}else
				{
					Rconte = value.content_;
				}

				ParseSymbol(Rconte);				
				ShowSymbol(maxRlist);
			    _RChatBackground.height = rLine*27+offset;
				_RChatBackground.transform.localPosition = new Vector3(-_RChatBackground.width,_RChatBackground.transform.localPosition.y,_RChatBackground.transform.localPosition.z);
			    _Background.height =_RChatBackground.height+80;

            }
            else if (value.audioId_!=0)
            {
				_rtimeLabel.text =value.audioTime_.ToString();
				_RChatBackground.gameObject.SetActive(false);
                _RChatBackground.Hide();
                _RAudioB.Show();
				UIManager.SetButtonEventHandler (_RAudioB.gameObject, EnumButtonEvent.OnClick, OnClickBroadcast, 1, 0);
            }

            //always hide
            _LSystemFlag.Hide();
            _LChatBackground.Hide();
            _LChatContent.Hide();
            _LAudioB.Hide();
            _LIconB.Hide();
            _LIconImage.Hide();
            _LName.Hide();
            _LChatKindBackground.Hide();
            _LChatKind.Hide();

        }
		get
		{
			return _Right;
		}
    }
	void startAudio(int id)
	{
        UISpriteAnimation rspani = _reftDh.GetComponent<UISpriteAnimation>();
        if (rspani != null)
            rspani.enabled = true;

        ChatSystem.startAudioOk -= startAudio;
	}

	void OnClickBroadcast(ButtonScript obj, object args, int param1, int param2)
	{
		if(ChatSystem.isPlayingAudio)
			return;

		ChatSystem.startAudioOk += startAudio;
        ChatSystem.finishAudioOk += closeVecUI;
		if (param1 == 1)
		{
			NetConnection.Instance.requestAudio (Right.audioId_);
			Transform tr = _RAudioB.transform.FindChild ("dian");
			tr.gameObject.SetActive(false);
		} else
			if(param1 == 2)
		{
			NetConnection.Instance.requestAudio (Left.audioId_);
			Transform tr = _LAudioB.transform.FindChild ("dian");
			tr.gameObject.SetActive(false);
		}
	}
	public void closeVecUI()
	{
        ChatSystem.finishAudioOk -= closeVecUI;
		UISpriteAnimation rspani = _reftDh.GetComponent<UISpriteAnimation> ();
		UISpriteAnimation lspani = _leftDh.GetComponent<UISpriteAnimation> ();
		if (rspani != null)
			rspani.enabled = false;
		if (lspani != null)
			lspani.enabled = false;
		rspani.transform.GetComponent<UISprite> ().spriteName = "yuyin1";
		lspani.transform.GetComponent<UISprite> ().spriteName = "yuyin1";
	}
    public COM_ChatInfo Left
    {
        get
        {
			return _Light;
        }
        set
        {
			_Light = value;
            //always show
            _LChatBackground.Show();
            //_LChatContent.Show();
            _LAudioB.Show();
            _LIconB.Show();
            _LIconImage.Show();
            _LName.Show();
            _LChatKindBackground.Show();
            _LChatKind.Show();
            _LChatKind.text = LanguageManager.instance.GetValue(Enum.GetName(typeof(ChatKind), value.ck_));
           
            if (!string.IsNullOrEmpty(value.content_))
            {
                _LAudioB.Hide();
                _LChatBackground.Show();
				_LChatContent.gameObject.SetActive(false);
				Reset();
				maxLlist.Add(new LabelType("  ", InfoType.Text));
				if(value.teamId_ != 0)
				{
					Lconte = LanguageManager.instance.GetValue ("yijianhanhua").Replace ("{n}",LanguageManager.instance.GetValue(value.teamType_.ToString())).Replace ("{n1}", value.teamMinLevel_.ToString()).Replace ("{n2}", value.teamMaxLevel_.ToString()).Replace("{t1}","1").Replace("{t2}",value.teamId_.ToString());
				}else
				{
					Lconte = value.content_;
				}

				ParseSymbol(Lconte);				
				ShowSymbol(maxLlist);
				_LChatBackground.height = lLine*27+offset;
				_Background.height =_LChatBackground.height+80;

            }
			else if (value.audioId_!=0)
            {
                _LChatBackground.gameObject.SetActive(false);
				_leftTime.text =value.audioTime_.ToString();
                _LChatBackground.Hide();
                _LAudioB.Show();
                UIManager.SetButtonEventHandler(_LAudioB.gameObject, EnumButtonEvent.OnClick, OnClickBroadcast, 2, 0);
            }

            if (value.ck_ == ChatKind.CK_System){
                _LSystemFlag.Show();
                _LIconB.Hide();
                _LName.Hide();
            }
            else
            {
                _LName.Show();
                _LName.text = value.playerName_;
                _LSystemFlag.Hide();
                _LIconB.Show();
                HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(value.assetId_).assetsIocn_, _LIconImage);
                _LIconImage.width = _LIconImage.height = 100;
            }

            //always hide
            _RChatBackground.Hide();
            _RChatContent.Hide();
            _RAudioB.Hide();
            _RIconB.Hide();
            _RIconImage.Hide();
            _RName.Hide();
            _RChatKindBackground.Hide();
            _RChatKind.Hide();
			UIManager.SetButtonEventHandler(_LIconB.gameObject, EnumButtonEvent.OnClick, OnClickLIconB, 0, 0);

        }
    }

	void OnClickLIconB(ButtonScript obj, object args, int param1, int param2)
	{
		tipsObj.SetActive (true);
		HaoyouShezhi hs = tipsObj.GetComponent<HaoyouShezhi> ();
		tipsObj.transform.position =new Vector3(obj.transform.position.x+0.8f,obj.transform.position.y,obj.transform.position.z) ;
		hs.insetId = Left.instId_;
		hs.name = Left.playerName_;
	}
	private static string chatName;
    public COM_ChatInfo Info
    {
        set
        {
			chatName = value.playerName_;
            if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(value.playerName_))
            { //这是我在说 尼玛~
                Right = value;
            }
            else
            {
                Left = value;
            }
        }
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
		
		public LabelType(string text,InfoType tp)
		{
			info = text;
			type = tp;
		}
	}
	private static List<LabelType> maxLlist;
	private static List<LabelType> maxRlist;
	private int positionX = 0;
	private int positionY = -6;
	//public UISprite back;
	private List<GameObject> LprefabCeches;
	private List<UILabel> LlabelCaches;
	private List<UILabel> LlabelCur;
	private List<GameObject> LprefabCur;

	private List<GameObject> RprefabCeches;
	private List<UILabel> RlabelCaches;
	private List<UILabel> RlabelCur;
	private List<GameObject> RprefabCur;

	public int cellHeight = 25;
	private int labelNameWidth;
	private int MaxLineWidth = 450;
	public string infoColor;
	public List<string> Prefabs;
	public GameObject LLabel;
	public GameObject LFacePrefab;
	public GameObject RLabel;
	public GameObject RFacePrefab;
	private int rLine = 1;
	private int lLine = 1;
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
					{
						if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
						{
							maxRlist.Add(new LabelType(startString, InfoType.Text));
						}else
						{
							maxLlist.Add(new LabelType(startString, InfoType.Text));
						}
					}						
					
					if (!string.IsNullOrEmpty(str))
					{
						if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
						{
							maxRlist.Add(new LabelType(str, InfoType.Face));
						}else
						{
							maxLlist.Add(new LabelType(str, InfoType.Face));
						}
					}
						
					
					endString = endString.Substring(endIndex);
				}
			}
			
			if (!string.IsNullOrEmpty(endString))
			{
				if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
				{
					maxRlist.Add(new LabelType(endString, InfoType.Text));
				}else
				{
					maxLlist.Add(new LabelType(endString, InfoType.Text));
				}
			}
				
		}
		else
		{
			if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
			{
				maxRlist.Add(new LabelType(endString, InfoType.Text));
			}else
			{
				maxLlist.Add(new LabelType(endString, InfoType.Text));
			}
			//maxlist.Add(new LabelType(endString, InfoType.Text));
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
		List<UILabel > labelCaches = new List<UILabel> ();
		if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
		{
			labelCaches = RlabelCaches;
		}else
		{
			labelCaches = LlabelCaches;
		}
		if (labelCaches.Count > 0)
		{
			label = labelCaches[0];
			labelCaches.Remove(label);
			label.gameObject.SetActive(true);
		}
		else
		{
			GameObject go = null;
//			GameObject go = GameObject.Instantiate(LLabel) as GameObject;
//			go.SetActive(true);
			if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
			{
			    go = GameObject.Instantiate(LLabel) as GameObject;
				go.SetActive(true);
				go.transform.parent = _RChatBackground.transform;
			}else
			{
			    go = GameObject.Instantiate(RLabel) as GameObject;
				go.SetActive(true);
				go.transform.parent = _LChatBackground.transform;
			}

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
		//aaa (value, out sbstr);
		
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
			label.text = infoColor + text + "[-]";
		else
			label.text = text;
		
		label.gameObject.transform.localPosition = new Vector3(positionX, positionY, 0);

		   

		positionX += (label.width);
		if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
		{
		
			RlabelCur.Add (label);
		}else
		{

			LlabelCur.Add (label);
		}
		
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

		if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
		{
			if(rLine==1)
			{
				_RChatBackground.width += label.width;
			}
		}else
		{
			if(lLine == 1)
			{
				if(label.text !="  ")
				_LChatBackground.width += label.width;
			}


		}


		if (sbstr.Length > 0)
		{
			positionX =labelNameWidth;
			positionY -= (cellHeight+4);
			if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
			{
				rLine++;
				//_RChatBackground.height += (cellHeight+10);
			}else
			{
				lLine++;
				//_LChatBackground.height += (cellHeight+10);
			}

			
			sbstr = sbstr.Replace("\n", "");
			CreateTextLabel(sbstr);

		}



	}
	
	private void CreateFace(string value)
	{
		List<GameObject> prefabCeches = new List<GameObject> ();
		if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
		{
			prefabCeches = RprefabCeches;
		}else
		{
			prefabCeches = LprefabCeches;
		}
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
				if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
				{
					face = GameObject.Instantiate(RFacePrefab) as GameObject;
					face.transform.parent = _RChatBackground.transform;
				}else
				{
					face = GameObject.Instantiate(LFacePrefab) as GameObject;
					face.transform.parent = _LChatBackground.transform;
				}

				face.transform.localScale = Vector3.one;
				face.SetActive(true);
			}
			
			UISprite sprite = face.GetComponent<UISprite>();
			sprite.spriteName = value;
			widget = face.GetComponent<UIWidget>();
			widget.pivot = UIWidget.Pivot.TopLeft;
			

			if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
			{
				if(rLine==1)
				{
					_RChatBackground.width += sprite.width;
				}

			}else
			{
				if(lLine==1)
				{
					_LChatBackground.width += sprite.width;
				}
			}


			if (MaxLineWidth < (positionX + widget.width))
			{
				positionX =  labelNameWidth;
				positionY -= (cellHeight);
				if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
				{
					rLine++;
					//_RChatBackground.height += cellHeight;
				}else
				{
					lLine++;
					//_LChatBackground.height += cellHeight;
				}

			}
			
			face.transform.localPosition = new Vector3(positionX, positionY, 0);
			
			positionX += widget.width;
			if (GamePlayer.Instance != null && GamePlayer.Instance.InstName.Equals(chatName))
			{
				RprefabCur.Add(face);
			}else
			{
				LprefabCur.Add(face);
			}
			//prefabCur.Add(face);
			

		}
		else
		{
			CreateTextLabel(value);
		}
	}
	
	private void Reset()
	{
		positionX = 0;
		rLine = 1;
		lLine = 1;
		positionY = -10;
		labelNameWidth = 0;
		_RChatBackground.height = cellHeight;
		_LChatBackground.height = cellHeight;
		maxLlist.Clear();
		maxRlist.Clear ();
		_RChatBackground.width = 1;
		_LChatBackground.width = 1;
		_Background.height = cellHeight;

		while (RlabelCur.Count > 0)
		{
			UILabel lab = RlabelCur[0];
			
			RlabelCur.Remove(lab);
			RlabelCaches.Add(lab);
			lab.gameObject.SetActive(false);
		}
		
		while (RprefabCur.Count > 0)
		{
			GameObject go = RprefabCur[0];
			
			RprefabCur.Remove(go);
			RprefabCeches.Add(go);
			go.SetActive(false);
		}
		while (LlabelCur.Count > 0)
		{
			UILabel lab = LlabelCur[0];
			
			LlabelCur.Remove(lab);
			LlabelCaches.Add(lab);
			lab.gameObject.SetActive(false);
		}
		
		while (LprefabCur.Count > 0)
		{
			GameObject go = LprefabCur[0];
			
			LprefabCur.Remove(go);
			LprefabCeches.Add(go);
			go.SetActive(false);
		}
	}
	
}