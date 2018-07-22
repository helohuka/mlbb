using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChatWindow : MonoBehaviour {

	public bool isOpen;
	public UIScrollBar sBar;
	public GameObject chatminPanel;
	//public GameObject chatmaxPanel;
	public GameObject chatBackPanel;
	public UIButton AreBtn;
	public GameObject Item;
	public GameObject meItem;
	public GameObject sysItem;
	public UIGrid grid;
	public UIButton allBtn;
	public UIButton SystemBtn;
	public UIButton WorldBtn;
	public UIButton GuildBtn;
	public GameObject TipsObj;
	public UIButton ContingentBtn;
	public UIButton VoiceBtn;
	public UIButton ExpressionBtn;
	public UIButton SendBtn;
	public UIButton SetBtn;
	public UIInput chatInput;
	private ChatKind chatType;
    public ChatKind ChatType
    {
        get { return chatType; }
    }
	private List<COM_ChatInfo> SysChat = new List<COM_ChatInfo> ();
	private List<COM_ChatInfo> WorldChat = new List<COM_ChatInfo> ();
	private List<COM_ChatInfo> GonghuiChat = new List<COM_ChatInfo> ();
	private List<COM_ChatInfo> TeamChat = new List<COM_ChatInfo> ();
	private List<UIButton>btns = new List<UIButton>();

	private List<GameObject> chatCellPool = new List<GameObject>();
	private List<GameObject> chatCellList = new List<GameObject>();



	private UIPanel SV;

	private static ChatWindow _chatWindowPanle = null;
	public static ChatWindow Instance 
	{
		get{
			return _chatWindowPanle;
		}
	}

	void Awake()
	{
		_chatWindowPanle = this;
	}

	void Start () {
		btns.Add (SystemBtn);
		btns.Add (WorldBtn);
		btns.Add (GuildBtn);
		btns.Add (ContingentBtn);
		btns.Add (allBtn);
		Item.SetActive (false);
		meItem.SetActive (false);
		sysItem.SetActive (false);
		gameObject.GetComponent<UIPanel> ().depth = 10;
		//dikuang.transform.localPosition = new Vector3 (749,226,0);
		UIManager.SetButtonEventHandler (SystemBtn.gameObject, EnumButtonEvent.OnClick, OnClickSystem, 0, 0);
		UIManager.SetButtonEventHandler (WorldBtn.gameObject, EnumButtonEvent.OnClick, OnClickWorld, 1, 0);
		UIManager.SetButtonEventHandler (GuildBtn.gameObject, EnumButtonEvent.OnClick, OnClickGuild, 2, 0);
		UIManager.SetButtonEventHandler (ContingentBtn.gameObject, EnumButtonEvent.OnClick, OnClickContingent,3, 0);
		UIEventListener.Get (VoiceBtn.gameObject).onPress = OnPrassVoice;
		UIManager.SetButtonEventHandler (ExpressionBtn.gameObject, EnumButtonEvent.OnClick, OnClickExpression, 0, 0);
		UIManager.SetButtonEventHandler (SendBtn.gameObject, EnumButtonEvent.OnClick, OnClickSend, 0, 0);
		UIManager.SetButtonEventHandler (SetBtn.gameObject, EnumButtonEvent.OnClick, OnClickSet, 0, 0);
		UIManager.SetButtonEventHandler (AreBtn.gameObject, EnumButtonEvent.OnClick, OnClickAre, 0, 0); 
		UIManager.SetButtonEventHandler (allBtn.gameObject, EnumButtonEvent.OnClick, OnClicall, 4, 0); 
		//UIEventListener.Get (chatminPanel).onClick = ShowSelfWindow;
		//UIEventListener.Get (chatmaxPanel).onClick = ShowSelfWindow;
		chatType = ChatKind.CK_World;
		BtnChoose (1);
		//ChatokOk (ChatSystem.ChatInfos);
		AddEvent ();
		chatminPanel.GetComponent<ChatMinPanel> ().AddEvent ();
		//chatmaxPanel.GetComponent<ChatMaxPanel> ().AddEvent ();
		SV = NGUITools.FindInParents<UIPanel>(gameObject);
	    Vector2 size =	SV.GetViewSize ();
		 nums = size.y/grid.cellHeight;


	}
	float nums ;
	public void AddEvent()
	{
		//ChatSystem.instance.ChatPanelOk += ChatokOk;
	}

	//void ShowSelfWindow(GameObject sender)
	//{
		//isOpen = true;
		//chatminPanel.transform.parent.gameObject.SetActive (false);
		//chatmaxPanel.SetActive (false);
		//chatBackPanel.SetActive (true);
	//}
public	void ChatokOk(List<COM_ChatInfo> chatInfos)
	{
		SysChat.Clear ();
		WorldChat.Clear ();
		GonghuiChat.Clear ();
		TeamChat.Clear ();
		for(int i =0;i<chatInfos.Count;i++)
		{
			if(chatInfos[i].ck_ == ChatKind.CK_System)
			{

				SysChat.Add(chatInfos[i]);
			}else
				if(chatInfos[i].ck_ == ChatKind.CK_World)
			{

				WorldChat.Add(chatInfos[i]);
			}else
				if(chatInfos[i].ck_ == ChatKind.CK_Guild)
			{

				GonghuiChat.Add(chatInfos[i]);
			}else
				if(chatInfos[i].ck_ == ChatKind.CK_Team)
			{

				TeamChat.Add(chatInfos[i]);

			}
		}
		switch (ChatType)
		{
		case ChatKind.CK_System:
			UpdataChatItem (SysChat);
			break;
		case ChatKind.CK_World:
			UpdataChatItem (WorldChat);
			break;
		case ChatKind.CK_Guild:
			UpdataChatItem (GonghuiChat);
			break;
		case ChatKind.CK_Team:
			UpdataChatItem (TeamChat);
			break;
		case ChatKind.CK_Max:
			UpdataChatItem (chatInfos);
			break;
		}

	}
	
	public	void UpdataChatItem(List<COM_ChatInfo> chat)
	{
		for(int i=0;i<chatCellList.Count;i++)
		{
			chatCellList[i].transform.parent = null;
			chatCellList[i].SetActive(false);
			chatCellPool.Add(chatCellList[i]);
		}
		chatCellList.Clear ();
		for(int i = 0;i<chat.Count;i++)
		{
			//if(!ChatSystem.instance.GetTypeIsOPen(chat[i].ck_))
			//	continue;
			GameObject clone = null;
			if(chatCellPool.Count>0)
			{
				clone = chatCellPool[0];
				chatCellPool.Remove(clone);
			}else
			{
				clone = GameObject.Instantiate (meItem)as GameObject;
			}
			clone.transform.parent = grid.transform;
			clone.SetActive (true);
			clone.transform.position = Vector3.zero;
			clone.transform.localScale = Vector3.one;

			chatCellList.Add(clone);
			CellChat cell = clone.GetComponent<CellChat> ();
			if(chat[i].playerName_ == GamePlayer.Instance.InstName)
			{
				cell.isSelf = true;
			}else
			{
				cell.isSelf = false;
			}
			cell.ChatInfo = chat[i];
		}
		grid.Reposition ();

		if (grid.transform.childCount >= 6)
		{
			sBar.value = 1;
		}
	}




	private void OnClickAre(ButtonScript obj, object args, int param1, int param2)
	{
		isOpen = false;
	
		//ChatSystem.instance.BackPanelOk -= ChatokOk;
		chatminPanel.transform.parent.gameObject.SetActive (true);
		//chatmaxPanel.SetActive (false);
		chatBackPanel.SetActive(false);

	}
	private void OnClickSystem(ButtonScript obj, object args, int param1, int param2)
	{
		chatInput.GetComponent<BoxCollider> ().enabled = false;
		chatInput.GetComponentInChildren<UILabel> ().text = "不能在此频道发言";
		BtnChoose (param1);
		chatType = ChatKind.CK_System;
		Refresh ();
		UpdataChatItem (SysChat);
	}
	private void OnClickWorld(ButtonScript obj, object args, int param1, int param2)
	{
		chatInput.GetComponent<BoxCollider> ().enabled = true;
		chatInput.GetComponentInChildren<UILabel> ().text = "";
		BtnChoose (param1);
		chatType = ChatKind.CK_World;
		Refresh ();
		UpdataChatItem (WorldChat);
	}
	private void OnClickGuild(ButtonScript obj, object args, int param1, int param2)
	{
		chatInput.GetComponent<BoxCollider> ().enabled = true;
		chatInput.GetComponentInChildren<UILabel> ().text = "";
		BtnChoose (param1);
		chatType = ChatKind.CK_Guild;
		Refresh ();
		UpdataChatItem (GonghuiChat);
	}
	private void OnClickContingent(ButtonScript obj, object args, int param1, int param2)
	{
		chatInput.GetComponentInChildren<UILabel> ().text = "";
		chatInput.GetComponent<BoxCollider> ().enabled = true;
		BtnChoose (param1);
		chatType = ChatKind.CK_Team;
		Refresh ();
		UpdataChatItem (TeamChat);
	}

	private void OnClicall(ButtonScript obj, object args, int param1, int param2)
	{
		chatInput.GetComponentInChildren<UILabel> ().text = "";
		chatInput.GetComponent<BoxCollider> ().enabled = true;
		BtnChoose (param1);
		chatType = ChatKind.CK_World;
		Refresh ();
		//UpdataChatItem (ChatSystem.ChatInfos);
	}
	private void OnPrassVoice(GameObject sender,bool isPrass)
	{
		COM_Chat chat_com = new COM_Chat();
		if(isPrass)
		{
		//	ChatSystem.instance.StartRecord();
		}
        else
		{
           // ChatSystem.instance.StopRecord();
           // ChatSystem.instance.GetClip(ref chat_com.audio_);
			chat_com.ck_ = ChatType;
            NetConnection.Instance.sendChat(chat_com, "");
		}
	}
	bool isSend;
   
//	private void OnClickVoice(ButtonScript obj, object args, int param1, int param2)
//	{
//	}
	private void OnClickExpression(ButtonScript obj, object args, int param1, int param2)
	{
	}

	private void OnClickSend(ButtonScript obj, object args, int param1, int param2)
	{

		string text = NGUIText.StripSymbols(chatInput.value);
		COM_Chat chat_com = new COM_Chat();
		if (!string.IsNullOrEmpty(text))
		{


			if(chatInput.value[0] == '^')
			{
				chat_com.ck_ = ChatKind.CK_GM;
				chat_com.content_ = chatInput.value.Substring(1);
				NetConnection.Instance.sendChat (chat_com,"");
			}				
			else
			{
				if(!TeamSystem.IsInTeam()&&chatType == ChatKind.CK_Team)
				{
					//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("teamMessage"));
					PopText.Instance.Show(LanguageManager.instance.GetValue("teamMessage"));
				}
				 else
					if(chatType == ChatKind.CK_Guild)
				{
					//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("gonghui"));
					PopText.Instance.Show(LanguageManager.instance.GetValue("gonghui"));
				}
				else
				{
					chat_com.ck_ = chatType;
					chat_com.content_ = chatInput.value;
					NetConnection.Instance.sendChat (chat_com,"");
				}

			}
				
			chatInput.value = "";
			chatInput.isSelected = false;
		}

	}
	private void OnClickSet(ButtonScript obj, object args, int param1, int param2)
	{
		TipsObj.SetActive (true);
	}
	private void BtnChoose(int index)
	{
		for(int i=0;i<btns.Count;i++)
		{
			if(i==index)
			{
				btns[i].isEnabled = false;
			}else
			{
				btns[i].isEnabled = true;
			}
		}
	}
	void Refresh()
	{

		if(grid == null)return;
		foreach(Transform tr in grid.transform)
		{
			tr.gameObject.SetActive(false);
		}
	}
	void OnDestroy()
	{
		//ChatSystem.instance.ChatPanelOk -= ChatokOk;
		ChatMinPanel minPanel = chatminPanel.GetComponent<ChatMinPanel> ();
		minPanel.Hide ();
//		ChatMaxPanel maxPanel = chatmaxPanel.GetComponent<ChatMaxPanel> ();
//		maxPanel.Hide ();
	}

}
