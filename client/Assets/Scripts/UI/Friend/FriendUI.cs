// File for panel in friend-system
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class FriendUI : UIBase
{
	public UIButton close_;

    public GameObject	friendBtn;
	public GameObject blackBtn;
	public UIGrid friendGrild;
	public GameObject  friendCell;

	public UIButton findFriendName;
	public UIButton addFriendBtn;
	public GameObject recomPane;
	public GameObject recomCell;
	public UIGrid   recomGrid;
	public UISprite chatLisPane;
	public UIPanel funPane;
	public UISprite funBtns;
	public UIButton funCloseBtn;
	public UIButton addBlackBtn;
	public UISprite blackPane;
	public UIButton findBlackBtn;
	public UIGrid findBlackGrid; 
	public UILabel inputLab;
	public UILabel inputBlackLab;
	public UILabel friendChatLab;
	public UIButton sendChatBtn;
	public GameObject chatCell;
	public GameObject chatSelfCell;
	public UIGrid chatGrid;
	public UIInput chatInput;

	public UIButton friendTabBtn;
	public UIButton offTabBtn;
	public UIPanel leftPane;
	public UISprite offPane;
	public UIGrid offGrid;
	public UIPanel findPane;
	public UIButton closeFindBtn;
	//public TeamPlayerInfo infoPanel;

	public UIButton faceBtn;
	public UIButton huanBtn;
	public UISprite offenRed;
	public UISprite friendRed;

	//选择好友操作按钮.
	public UIButton ctlSendBtn;
	public UIButton ctlLookBtn;
	public UIButton ctlTeamBtn;
	public UIButton ctlDelBtn;
	public UIButton ctlBlackBtn;
	public UISprite funCloseBack;
	public UIButton funAddFriend;
	public GameObject faceObj;
	public UIFont _font;
	public GameObject yuyinTips;
	public static AudioSource audioSO; 
	public UIButton _VoiceBtn;
	public UILabel selectChatLab;


	public ChatGrid cGrid;
	public GameObject _GridItem;
	//
	//
	public UIScrollBar scrollBar;
	private List<GameObject> friendCellList = new List<GameObject>();
	private List<GameObject> blackCellList = new List<GameObject>();
	private List<GameObject> chatCellList = new List<GameObject>();
	private List<GameObject> _findFriendCellList = new List<GameObject>();
	private List<GameObject> _findFriendCellPool = new List<GameObject>();
	private List<GameObject> _offCellList = new List<GameObject>();
	private COM_ContactInfo _selectFriend;  //选中好友.
	private GameObject       _selectFriendBtn;  
	private  COM_Chat chat_com;//= new COM_Chat();
	private List<string> _icons = new List<string>();
	private List<GameObject> chindCellPoolList = new List<GameObject>();
  
	void Start ()
	{
		UIManager.SetButtonEventHandler (close_.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (closeFindBtn.gameObject, EnumButtonEvent.OnClick, OnCloseFind, 0, 0);

		UIManager.SetButtonEventHandler (blackBtn.gameObject, EnumButtonEvent.OnClick, OnInitBlack, 0, 0);
		UIManager.SetButtonEventHandler (addFriendBtn.gameObject, EnumButtonEvent.OnClick, OnAddFriend, 0, 0);
		UIManager.SetButtonEventHandler (addBlackBtn.gameObject, EnumButtonEvent.OnClick, OnAddBlack, 0, 0);
		UIManager.SetButtonEventHandler (funCloseBtn.gameObject, EnumButtonEvent.OnClick, OnCloseFun, 0, 0);
		UIManager.SetButtonEventHandler (findFriendName.gameObject, EnumButtonEvent.OnClick, OnfindFriend, 0, 0);
		UIManager.SetButtonEventHandler (findBlackBtn.gameObject, EnumButtonEvent.OnClick, OnFindBlack, 0, 0);
		UIManager.SetButtonEventHandler (sendChatBtn.gameObject, EnumButtonEvent.OnClick, OnSendChat, 0, 0);

		UIManager.SetButtonEventHandler (friendTabBtn.gameObject, EnumButtonEvent.OnClick, OnFriendTabBtn, 0, 0);
		UIManager.SetButtonEventHandler (offTabBtn.gameObject, EnumButtonEvent.OnClick, OnOffTabBtn, 0, 0);
		UIManager.SetButtonEventHandler (huanBtn.gameObject, EnumButtonEvent.OnClick, OnHuanBtn, 0, 0);
		UIManager.SetButtonEventHandler (funCloseBack.gameObject, EnumButtonEvent.OnClick, OnCloseFun, 0, 0);
		UIManager.SetButtonEventHandler (funAddFriend.gameObject, EnumButtonEvent.OnClick, OnAddFriendFun, 0, 0);
		UIManager.SetButtonEventHandler (faceBtn.gameObject, EnumButtonEvent.OnClick, OnfaceBtn, 0, 0);

			

		//选择好友操作按钮.
		UIManager.SetButtonEventHandler (ctlSendBtn.gameObject, EnumButtonEvent.OnClick, OnCtlSendBtn, 0, 0);
		UIManager.SetButtonEventHandler (ctlLookBtn.gameObject, EnumButtonEvent.OnClick, OnCtlLookBtn, 0, 0);
		UIManager.SetButtonEventHandler (ctlTeamBtn.gameObject, EnumButtonEvent.OnClick, OnCtlTeamBtn, 0, 0);

		UIManager.SetButtonEventHandler (ctlDelBtn.gameObject, EnumButtonEvent.OnClick, OnCtlDelBtn, 0, 0);
		UIManager.SetButtonEventHandler (ctlBlackBtn.gameObject, EnumButtonEvent.OnClick, OnCtlBlackBtn, 0, 0);

		UIEventListener.Get (_VoiceBtn.gameObject).onPress = OnPrassVoice;
		//事件.
		FriendSystem.Instance().UpdateFriend += new UpdateFriendHandler(OnUpdataFriendList);
        FriendSystem.Instance().UpdateBlack += new UpdateBlackHandler(OnUpdataBlackList);
		FriendSystem.Instance().UpdateOffen += new UpdateOffenHandler(OnUpdateOffens);
		FriendSystem.Instance ().FriendChat += new FriendChatHandler (OnFriendChat);
		FriendSystem.Instance ().FindFriendOkEvent += new RequestEventHandler<COM_ContactInfo> (FindFriendOk);	
		FriendSystem.Instance ().FindFriendFail += new FindFriendFailHandler(FindFriendFail);
		FriendSystem.Instance ().recommendEvent += new RequestEventHandler<int> (OnRecommendEvent);
		FriendSystem.Instance ().friendOnLineEvent += new RequestEventHandler<bool> (OnLineEvent);

		NetConnection.Instance.requestFriendList ();
		ChartsSystem.QueryPlayerEventOk += OnQueryPlayer;

        GuideManager.Instance.RegistGuideAim(addFriendBtn.gameObject, GuideAimType.GAT_FriendAddBtn);
		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{

			friendGrild.AddChild(friendBtn.transform,0);
			//friendBtn.transform.parent = friendGrild.transform;
			friendBtn.SetActive (true);
			friendBtn.transform.localScale = Vector3.one;
			UIManager.SetButtonEventHandler (friendBtn.gameObject, EnumButtonEvent.OnClick, OnClickFirend, 0, 0);
			friendGrild.AddChild(blackBtn.transform,1);
			//blackBtn.transform.parent = friendGrild.transform;
			blackBtn.SetActive (true);
			blackBtn.transform.localScale = Vector3.one;
			UIManager.SetButtonEventHandler (blackBtn.gameObject, EnumButtonEvent.OnClick, OnInitBlack, 0, 0);

			//friendGrild.Reposition();

			friendTabBtn.isEnabled = false;
			//UpdataFriendList ();
			FriendSystem.Instance ().isOpenFried = true;
			if(FriendSystem.Instance().offens_.Count > 0)
			{
				foreach(COM_ContactInfo f in FriendSystem.Instance().offens_)
				{
					if(FriendSystem.Instance().NoChatDict.ContainsKey(f.name_))
					{
						//offTabBtn.GetComponentInChildren<UISprite>().MarkOff();
						offTabBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-10,-10);
						break;
					}
				}
			}

            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickMainFriend);
		});
		audioSO = gameObject.GetComponentInParent<AudioSource> ();
		InitFace ();
		if(FriendSystem.Instance().friends_.Count <= 0)
		{
			selectChatLab.gameObject.SetActive(false);
		}

	}

	void InitFace()
	{
		for(int i =0;i<ChatSystem.faceStrl.Length;i++)
		{
			_font.AddSymbol(ChatSystem.faceStrl[i],ChatSystem.faceStrl[i]);
		}
	}

	bool isCanel;
	float PrassTime = 0;
	float maxTime = 20;
	bool istargetPrass = false;
	float tt;
	bool ischaoshi;

	private void OnPrassVoice(GameObject sender,bool isPrass)
	{
#if UNITY_ANDROID
        if(!XyskAndroidAPI.hasMicrophoneAuth())
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("microphoneforbidden"), PopText.WarningType.WT_Warning);
            return;
        }
#elif UNITY_IOS
        if(!XyskIOSAPI.HasMicrophoneAuth() && isPrass)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("microphoneforbidden"), PopText.WarningType.WT_Warning);
            return;
        }
#endif
        if (Microphone.devices.Length == 0)
        {
            ClientLog.Instance.Log("No Record Device!");
            return;
        }
		if(isPrass)
		{
			isCanel = false;
			ischaoshi = false;
			PrassTime = Time.realtimeSinceStartup;
			istargetPrass = true;
			ChatSystem.StartRecord();
			yuyinTips.SetActive(true);
		}
		else
		{
			istargetPrass = false;
			yuyinTips.SetActive(false);
			chat_com = new COM_Chat();
			if(ischaoshi)return;
			float tm = Time.realtimeSinceStartup - PrassTime;
			if(tm<1)
			{
				
				PopText.Instance.Show(LanguageManager.instance.GetValue("chatVeTime"));
			}else
			{
				if(isCanel)return;
				ChatSystem.StopRecord();
                if (ChatSystem.AsyncGetCallBack == null)
                {
                    ChatSystem.AsyncGetCallBack = delegate(byte[] datas)
                    {
                        chat_com.audio_ = datas;
                        NetConnection.Instance.queryOnlinePlayerbyName (_selectFriend.name_);
                    };
                    ChatSystem.AsyncGet();
                }
                //ChatSystem.GetClip(ref chat_com.audio_);
                //NetConnection.Instance.queryOnlinePlayerbyName (_selectFriend.name_);

				//ChatSystem.SendToServer(_SendChatKind,"","",chat_com.audio_);
				
			}
			
		}
	}
	

	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_Friend);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_Friend);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_Friend);
	}
	#endregion

	public void UpdataBlackList()
	{ 
		if (!leftPane.gameObject.activeSelf)
			return;
		foreach (GameObject o in friendCellList)
		{
			friendGrild.RemoveChild(o.transform);
			o.transform.parent = null;
			GameObject.Destroy(o);
		}
		friendCellList.Clear();


		friendGrild.RemoveChild(blackBtn.transform);
		blackBtn.transform.parent = null;


		friendGrild.AddChild(blackBtn.transform,1);
		//blackBtn.transform.parent = friendGrild.transform;
		blackBtn.SetActive (true);
		blackBtn.transform.localScale = Vector3.one;
		UIManager.SetButtonEventHandler (blackBtn.gameObject, EnumButtonEvent.OnClick, OnInitBlack, 0, 0);
		
		
		int indx = friendGrild.GetIndex (blackBtn.transform);

		foreach (COM_ContactInfo f in FriendSystem.Instance().blacks_)
        {
			GameObject obj = Object.Instantiate(friendCell.gameObject) as GameObject;
			obj.SetActive(true);

			FriendItem cellUI = obj.GetComponent<FriendItem>();
			cellUI.ContactInfo = f;
		
			if(FriendSystem.Instance().ChatDict.ContainsKey(f.name_))
			{
				cellUI.red.gameObject.SetActive(true);
			}
			else
			{
				cellUI.red.gameObject.SetActive(false);
			}
			cellUI.IsBlack(true);
			cellUI.delCallBack = OnCellDelBlack;
			friendGrild.AddChild(obj.transform,++indx);
			friendCellList.Add(obj);
			obj.transform.localScale = Vector3.one;
        }
		friendGrild.Reposition ();
	}


	public void UpdataFriendList()
	{
		if (!leftPane.gameObject.activeSelf)
			return;
		foreach (GameObject o in friendCellList)
		{
			friendGrild.RemoveChild(o.transform);
			o.transform.parent = null;
			GameObject.Destroy(o);
		}
		friendCellList.Clear();

		int indx = friendGrild.GetIndex (friendBtn.transform);
	

		foreach (COM_ContactInfo f in FriendSystem.Instance().friends_)
		{
			if(f.isLine_)  
			{
				GameObject obj = Object.Instantiate(friendCell.gameObject) as GameObject;
				obj.SetActive(true);
				FriendItem cellUI = obj.GetComponent<FriendItem>();
				cellUI.ContactInfo = f;
				if(FriendSystem.Instance().ChatDict.ContainsKey(f.name_))
				{
					cellUI.red.gameObject.SetActive(true);
				}
				else
				{
					cellUI.red.gameObject.SetActive(false);
				}
				cellUI.IsBlack(false);
				UIManager.SetButtonEventHandler (obj, EnumButtonEvent.OnClick, OnFriendCell, 0, 0);
				cellUI.callBack = OnFunBtn;
				friendGrild.AddChild(obj.transform,++indx);

				friendCellList.Add(obj);
				obj.transform.localScale = Vector3.one;
			}
		}


		foreach (COM_ContactInfo f in FriendSystem.Instance().friends_)
		{
			if(!f.isLine_)
			{
				GameObject obj = Object.Instantiate(friendCell.gameObject) as GameObject;
				obj.SetActive(true);
				FriendItem cellUI = obj.GetComponent<FriendItem>();
				cellUI.ContactInfo = f;
				if(FriendSystem.Instance().ChatDict.ContainsKey(f.name_))
				{
					cellUI.red.gameObject.SetActive(true);
				}
				else
				{
					cellUI.red.gameObject.SetActive(false);
				}
				cellUI.IsBlack(false);
				UIManager.SetButtonEventHandler (obj, EnumButtonEvent.OnClick, OnFriendCell, 0, 0);
				cellUI.callBack = OnFunBtn;
				friendGrild.AddChild(obj.transform,++indx);
				
				friendCellList.Add(obj);
				obj.transform.localScale = Vector3.one;
			}
		}

	}


	private void UpdateLatelyList()
	{
		foreach (GameObject o in _offCellList)
		{
			offGrid.RemoveChild(o.transform);
		//	o.transform.transform = null;
			GameObject.Destroy(o);
		}

		_offCellList.Clear();

		foreach(COM_ContactInfo f in FriendSystem.Instance().offens_)
		{
			GameObject obj = Object.Instantiate(friendCell.gameObject) as GameObject;
			obj.SetActive(true);
			
			FriendItem cellUI = obj.GetComponent<FriendItem>();
			cellUI.ContactInfo = f;
			
			if(FriendSystem.Instance().NoChatDict.ContainsKey(f.name_))
			{
				//cellUI.red.gameObject.SetActive(true);
				cellUI.GetComponent<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-20,-130);
			}
			else
			{
				cellUI.GetComponent<UISprite>().MarkOff();
				//cellUI.red.gameObject.SetActive(false);
			}
			cellUI.IsBlack(false);
			UIManager.SetButtonEventHandler (obj, EnumButtonEvent.OnClick, OnFriendCell, 0, 0);
			cellUI.callBack = OnFunBtn;
			offGrid.AddChild(obj.transform);
			_offCellList.Add(obj);
			obj.transform.localScale = Vector3.one;
		}
		offGrid.Reposition ();

	}


	public void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			FriendSystem.Instance ().isOpenFried = false;
			Hide ();	
		});
	}

	public void OnCloseFind(ButtonScript obj, object args, int param1, int param2)
	{
		findPane.gameObject.SetActive (false);
	}


	public void OnClickFirend(ButtonScript obj, object args, int param1, int param2)
    { 
		if(obj.transform.FindChild("arrows").GetComponent<UISprite>().spriteName == "sanjiao2")
		{

			friendBtn.transform.FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao";
			addFriendBtn.gameObject.SetActive (false);
			chatLisPane.gameObject.SetActive(false);
			foreach (GameObject o in friendCellList)
			{
				friendGrild.RemoveChild(o.transform);
				o.transform.parent = null;
				GameObject.Destroy(o);
			}
			friendCellList.Clear();
		}
		else
		{
			obj.transform.FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao2";
			blackBtn.transform.FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao";
			addFriendBtn.gameObject.SetActive (true);
			addBlackBtn.gameObject.SetActive (false);
			UpdataFriendList ();


			blackPane.gameObject.SetActive (false);
			chatLisPane.gameObject.SetActive(false);
			//recomPane.gameObject.SetActive (false);
		}

	}

    public void OnInitBlack(ButtonScript obj, object args, int param1, int param2)
    {
		if(obj.transform.FindChild("arrows").GetComponent<UISprite>().spriteName == "sanjiao2")
		{
			blackBtn.transform.FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao";
			addFriendBtn.gameObject.SetActive (false);
			addBlackBtn.gameObject.SetActive (false);
			
			blackPane.gameObject.SetActive (false);
			chatLisPane.gameObject.SetActive(false);
			foreach (GameObject o in friendCellList)
			{
				friendGrild.RemoveChild(o.transform);
				o.transform.parent = null;
				GameObject.Destroy(o);
			}
			friendCellList.Clear();

		}
		else
		{
			obj.transform.FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao2";
			friendBtn.transform.FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao";
			addFriendBtn.gameObject.SetActive (false);
			//addBlackBtn.gameObject.SetActive (true);	
			UpdataBlackList (); 


			blackPane.gameObject.SetActive (true);
			chatLisPane.gameObject.SetActive(false);
			//recomPane.gameObject.SetActive (false);

		}
	}



	public void OnAddFriend(ButtonScript obj, object args, int param1, int param2)
	{
		chatLisPane.gameObject.SetActive(false);
		blackPane.gameObject.SetActive (false);
		//recomPane.gameObject.SetActive (true);
		findPane.gameObject.SetActive (true);
		UIManager.Instance.AdjustUIDepth (findPane.transform);
		NetConnection.Instance.requestReferrFriend ();
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickAddFriendBtn);
	}

	private void OnAddBlack(ButtonScript obj, object args, int param1, int param2)
	{
		blackPane.gameObject.SetActive (true);
		chatLisPane.gameObject.SetActive(false);
		//recomPane.gameObject.SetActive (false);
	} 


	public void OnFriendCell(ButtonScript obj, object args, int param1, int param2)
	{
		obj.GetComponent<UISprite>().MarkOff();

		//recomPane.gameObject.SetActive (false);
		blackPane.gameObject.SetActive (false);
		chatLisPane.gameObject.SetActive (true);
		if(_selectFriendBtn != null)
		{
			_selectFriendBtn.GetComponent<UISprite> ().spriteName = "jn_jinlan";
		}
		_selectFriendBtn = obj.gameObject;
		_selectFriendBtn.GetComponent<UISprite> ().spriteName = "jn_jinlanliang";
		if(obj.GetComponent<FriendItem> ().ContactInfo == null)
		{
			return;
		}
		_selectFriend = obj.GetComponent<FriendItem> ().ContactInfo;
		if(FriendSystem.Instance ().NoChatDict.ContainsKey(_selectFriend.name_))
		{
			offTabBtn.GetComponentInChildren<UISprite>().MarkOff();
			FriendSystem.Instance ().NoChatDict.Remove (_selectFriend.name_);
		}
		//if(obj.GetComponent<FriendItem> ().red.gameObject.activeSelf)
		//{
			//obj.GetComponent<FriendItem> ().red.gameObject.SetActive(false);
			UpdateChatList(_selectFriend);
		//}
	}

	private void OnFunBtn( GameObject obj, COM_ContactInfo info)
	{
		if(info == null)
		{
			return;
		}
		if(_selectFriendBtn != null)
		{
			_selectFriendBtn.GetComponent<UISprite> ().spriteName = "jn_jinlan";
		}
		_selectFriendBtn = obj;
		_selectFriendBtn.GetComponent<UISprite> ().spriteName = "jn_jinlanliang";
		_selectFriend = info;


		if (FriendSystem.Instance ().offens_.Contains (_selectFriend) && !FriendSystem.Instance ().friends_.Contains (_selectFriend)) 
		{
			funAddFriend.gameObject.SetActive(true);
		}
		else
		{
			funAddFriend.gameObject.SetActive(false);
		}

		funPane.gameObject.SetActive (true);

		if(TeamSystem.IsInTeam()&&TeamSystem.IsTeamLeader())
		{
			ctlTeamBtn.gameObject.GetComponentInChildren<UILabel>().text = LanguageManager.instance.GetValue("yaoqingrudui");
		}else
		{
			if(TeamSystem.IsInTeam())
			{
				ctlTeamBtn.gameObject.GetComponentInChildren<UILabel>().text = LanguageManager.instance.GetValue("haoyoushenqingrudui");;
			}
		}

		funBtns.transform.position = new Vector3 (funBtns.transform.position.x,obj.gameObject.transform.position.y , 0f);
	}

	void OnCellDelBlack(GameObject obj, COM_ContactInfo info)
	{
		if(info == null)
		{
			return;
		}
		MessageBoxUI.ShowMe( LanguageManager.instance.GetValue("delback"), () => {
			NetConnection.Instance.delBlacklist(info.instId_);
		});
	}


	private void OnAddFriendList(ButtonScript obj, COM_ContactInfo info)
	{
		if(info == null)
		{
			return;
		}
		if(FriendSystem.Instance ().GetFriend (info.instId_) != null)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("alreadyhave"),null,true);
			return;
		}
		if(FriendSystem.Instance().friends_.Count >= 100)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}
		int fMax = 0;
		GlobalValue.Get(Constant.C_FriendMax, out fMax);
		if(FriendSystem.Instance().friends_.Count>= fMax)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("haoyoumax"));
			return;
		}
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue( "addfriend").Replace("{n}",info.name_) ,()=>{
			NetConnection.Instance.addFriend(info.instId_);
		});
	}

	private void OnAddBlackList(ButtonScript obj, COM_ContactInfo info)
	{
		if(info == null)
		{
			return;
		}
		if(FriendSystem.Instance ().GetBlack (info.instId_) != null)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("alreadyhaveback"),null,true);
			return;
		}
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("addback").Replace("{n}",info.name_) ,()=>{
			NetConnection.Instance.addBlacklist(info.instId_);
		});
	}

	void OnFriendChat(COM_ContactInfo contact,COM_Chat msg)
	{


		if( _selectFriend != null && contact.instId_ == _selectFriend.instId_ )
		{
			/*GameObject obj = Object.Instantiate(chatCell.gameObject) as GameObject;
			obj.SetActive(true);
			FriendItem cellUI = obj.GetComponent<FriendItem>();
			cellUI._chatInfo = msg;
			if(msg.audio_ != null && msg.audio_.Length > 0)
			{
			//	obj.transform.Find ("info").GetComponent<UILabel> ().gameObject.SetActive(false);
				obj.transform.Find ("vButton").GetComponent<UIButton> ().gameObject.SetActive(true);
			}
			else
			{
				obj.transform.Find ("vButton").GetComponent<UIButton> ().gameObject.SetActive(false);
			}
			//obj.transform.Find ("info").GetComponent<UILabel> ().text = msg.content_;

			//cellUI.SetInfoBack(msg.content_);
			cellUI.ParseSymbol(msg.content_);

			HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData((int)contact.assetId_).assetsIocn_, obj.transform.Find ("icon").Find ("Sprite").GetComponent<UITexture>());
			if(!_icons.Contains(EntityAssetsData.GetData((int)contact.assetId_).assetsIocn_))
			{
				_icons.Add(EntityAssetsData.GetData((int)contact.assetId_).assetsIocn_);
			}

			cellUI.name_.text = contact.name_;
			//cellUI.level_.text = contact.level_.ToString ();
			chatGrid.AddChild(obj.transform);
			obj.transform.localScale = Vector3.one;
			*/



			GameObject tf = null;
			tf = GameObject.Instantiate(_GridItem) as GameObject;
			cGrid.AddChild(tf.transform);
			tf.transform.localScale = Vector3.one;
			tf.gameObject.SetActive(true);
			ChatMaxCell cmi = tf.gameObject.GetComponent<ChatMaxCell>();
			COM_ChatInfo info = new COM_ChatInfo();
			Filt(ref msg.content_,0,1);
			info.content_ = msg.content_;
			info.ck_ =  ChatKind.CK_Friend;
			info.playerName_ = contact.name_;
			info.assetId_ = (ushort)contact.assetId_;
			cmi.Info = info;
			cmi._LChatKindBackground.gameObject.SetActive(false);
			cmi._RChatKindBackground.gameObject.SetActive(false);
			chatCellList.Add(tf);



		}
		else
		{
			foreach(var x in friendCellList)
			{
				if(x.GetComponent<FriendItem>().ContactInfo.instId_ == contact.instId_)
				{
					//x.GetComponent<FriendItem>().red.gameObject.SetActive(true);
					x.GetComponent<UISprite>().MarkOn();//UISprite.MarkAnthor.MA_RightTop,0,0);
					break;
				}
			}
			foreach (COM_ContactInfo f in FriendSystem.Instance().offens_)
			{
				if(f.instId_ == contact.instId_)
				{
					offTabBtn.GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-10,-10);
					break;
				}
			}
		}
	}

	void OnUpdataFriendList(COM_ContactInfo contact,bool isNew)
	{
	
		if(isNew)
		{
		//	PopText.Instance.Show(LanguageManager.instance.GetValue("addfriendok"));
		}
		else
		{
			//PopText.Instance.Show(LanguageManager.instance.GetValue("delfriendOk"));
		}
		UpdataFriendList ();
	}

	void OnUpdataBlackList(COM_ContactInfo contact,bool isNew)
	{

		if(isNew)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("addbackok"));
		}
		else
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("delbackok"));
		}
		UpdataBlackList ();
	}
		
	void FindFriendFail()
	{
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("nofindplayer"),null,true);
	}


	void OnRecommendEvent(int id)
	{
		if(_findFriendCellList.Count>0)
		{
			foreach(GameObject i in _findFriendCellList)
			{
				recomGrid.RemoveChild(i.transform);
				i.gameObject.transform.parent = null;
				i.gameObject.SetActive(false);
				_findFriendCellPool.Add(i);
			}
			_findFriendCellList.Clear();
		}
		COM_ContactInfo[] rcmfriends = FriendSystem.Instance().RecommendFriends;
		foreach(COM_ContactInfo req in rcmfriends)
		{
			GameObject objCell = null;
			if(_findFriendCellPool.Count>0)
			{
				objCell = _findFriendCellPool[0];
				//objCell.SetActive(true);
				FriendRecommendCell cellUI = objCell.GetComponent<FriendRecommendCell>();
				_findFriendCellPool.Remove(objCell);
			}
			else
			{
				objCell = Object.Instantiate(recomCell.gameObject) as GameObject;

			}
			
			if(objCell != null)
			{
				FriendRecommendCell cellUI1 = objCell.GetComponent<FriendRecommendCell>();
				cellUI1.ContactInfo = req;
				cellUI1.level_.text = req.level_.ToString();
				//cellUI1.icon.spriteName = req
				//recomGrid.AddChild(objCell.transform);
				objCell.transform.parent = recomGrid.transform;
				objCell.SetActive(true);
				cellUI1.callBack = OnAddFriendList;
				objCell.transform.localScale = Vector3.one;
				_findFriendCellList.Add(objCell);
			}
		}
		recomGrid.Reposition();
	}

	void OnQueryPlayer(COM_SimplePlayerInst Inst)
	{
		//infoPanel.gameObject.SetActive (true);
		//infoPanel.SetSimplePlayerInst (Inst);
		TeamPlayerInfo.ShowMe (Inst);
	}

	void FindFriendOk(COM_ContactInfo req)
	{
		GameObject objCell = null;
		if(_findFriendCellList.Count>0)
		{
			foreach(GameObject i in _findFriendCellList)
			{
				recomGrid.RemoveChild(i.transform);
				i.gameObject.transform.parent = null;
				i.gameObject.SetActive(false);
				_findFriendCellPool.Add(i);
			}
			_findFriendCellList.Clear();

		}
		
		if(_findFriendCellPool.Count>0)
		{
			objCell = _findFriendCellPool[0];
			objCell.SetActive(true);
			FriendRecommendCell cellUI = objCell.GetComponent<FriendRecommendCell>();
			_findFriendCellPool.Remove(objCell);
		}
		else
		{
			objCell = Object.Instantiate(recomCell.gameObject) as GameObject;
			objCell.SetActive(true);
		}

		if(objCell != null)
		{
			FriendRecommendCell cellUI1 = objCell.GetComponent<FriendRecommendCell>();
			cellUI1.ContactInfo = req;
			cellUI1.level_.text = req.level_.ToString();
			//cellUI1.icon.spriteName = req
		
			if(findPane.gameObject.activeSelf)
			{
				recomGrid.AddChild(objCell.transform);
				objCell.transform.parent = recomGrid.transform;
				cellUI1.callBack = OnAddFriendList;
				recomGrid.gameObject.SetActive(false);
				recomGrid.gameObject.SetActive(true);
				recomGrid.repositionNow = true;
			}
			else if(blackPane.gameObject.activeSelf)
			{
				findBlackGrid.AddChild(objCell.transform);
				objCell.transform.parent = findBlackGrid.transform;
				cellUI1.callBack = OnAddBlackList;
				findBlackGrid.gameObject.SetActive(false);
				findBlackGrid.gameObject.SetActive(true);
				findBlackGrid.repositionNow = true;
			}
			objCell.transform.localScale = Vector3.one;
			_findFriendCellList.Add(objCell);
		}

	

	}

	void OnUpdateOffens(COM_ContactInfo contact,bool isNew)
	{
	
		UpdateLatelyList ();
	}

	private void OnCloseFun(ButtonScript obj, object args, int param1, int param2)
	{
		funPane.gameObject.SetActive (false);
	}

	private void OnAddFriendFun(ButtonScript obj, object args, int param1, int param2)
	{
		funPane.gameObject.SetActive (false);
		if (_selectFriend == null)
			return;
		if(FriendSystem.Instance ().GetFriend (_selectFriend.instId_) != null)
		{ 

			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("alreadyhave"),null,true);
			return;
		}

		int fMax = 0;
		GlobalValue.Get(Constant.C_FriendMax, out fMax);
		if(FriendSystem.Instance().friends_.Count >= fMax)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}
		MessageBoxUI.ShowMe(LanguageManager.instance.GetValue( "addfriend").Replace("{n}",_selectFriend.name_) ,()=>{
			NetConnection.Instance.addFriend(_selectFriend.instId_);
		});
	}

	private void OnfaceBtn(ButtonScript obj, object args, int param1, int param2)
	{
		faceObj.gameObject.SetActive (true);
	}

	private void OnfindFriend(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.requestContactInfoByName(inputLab.text);
		inputLab.text = "";
	}

	private void OnFindBlack(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.requestContactInfoByName(inputBlackLab.text);
	}

	private void OnSendChat(ButtonScript obj, object args, int param1, int param2)
	{
		if(_selectFriend == null)
		{
			return;
		}

		NetConnection.Instance.queryOnlinePlayerbyName (_selectFriend.name_);
	}


	void OnLineEvent(bool line)
	{
		if(string.IsNullOrEmpty(friendChatLab.text.Trim()))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bunengweikong"));
			return;
		}
		if(!line)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("frinedonline"));
			return;
		}

		else if(chat_com != null)
		{
            chat_com.ck_ = ChatKind.CK_Friend;
			Filt(ref chat_com.content_,0,1);
			NetConnection.Instance.sendChat(chat_com,_selectFriend.name_);
			if(chat_com.content_.Length> 30)
			{
				//PopText.Instance.Show(LanguageManager.instance.GetValue("chatLineMax"));
				//return;
			}

			//COM_Chat comC = new COM_Chat();
			chat_com.ck_ = ChatKind.CK_Friend;
			chat_com.content_ = friendChatLab.text;
			chat_com.isMe = true;
		//	NetConnection.Instance.sendChat (comC,_selectFriend.name_);
			
			GameObject tf = null;
			tf = GameObject.Instantiate(_GridItem) as GameObject;
			cGrid.AddChild(tf.transform);
			tf.transform.localScale = Vector3.one;
			tf.gameObject.SetActive(true);
			ChatMaxCell cmi = tf.gameObject.GetComponent<ChatMaxCell>();
			COM_ChatInfo info = new COM_ChatInfo();// (COM_ChatInfo)comC ;
			info.playerName_ = GamePlayer.Instance.InstName;

			info.content_ = friendChatLab.text;
			Filt(ref info.content_,0,1);
			info.ck_ =  ChatKind.CK_Friend;
			info.assetId_ =(ushort)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId);
			cmi.Info = info;
			cmi._LChatKindBackground.gameObject.SetActive(false);
			cmi._RChatKindBackground.gameObject.SetActive(false);
			chat_com = null;
			FriendSystem.Instance().addMyChat(_selectFriend.name_,chat_com);
			chatCellList.Add(tf);

			friendChatLab.text = "";
			chatInput.value = "";
		}
		else
		{
			if(string.IsNullOrEmpty(friendChatLab.text.Trim()))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("bunengweikong"));
				return;
			}
			if(friendChatLab.text.Length> 30)
			{
				//PopText.Instance.Show(LanguageManager.instance.GetValue("chatLineMax"));
				//return;
			}
			COM_Chat comC = new COM_Chat();
            comC.ck_ = ChatKind.CK_Friend;
			comC.content_ = friendChatLab.text;
			Filt(ref comC.content_,0,1);
			NetConnection.Instance.sendChat (comC,_selectFriend.name_);

			GameObject tf = null;
			tf = GameObject.Instantiate(_GridItem) as GameObject;
			cGrid.AddChild(tf.transform);
			tf.transform.localScale = Vector3.one;
			tf.gameObject.SetActive(true);
			ChatMaxCell cmi = tf.gameObject.GetComponent<ChatMaxCell>();
			COM_ChatInfo info = new COM_ChatInfo(); 
			info.playerName_ = GamePlayer.Instance.InstName;
			info.content_ = friendChatLab.text;
			Filt(ref info.content_,0,1);
			info.ck_ =  ChatKind.CK_Friend;
			info.assetId_ =(ushort)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId);
			cmi.Info = info;
			cmi._LChatKindBackground.gameObject.SetActive(false);
			cmi._RChatKindBackground.gameObject.SetActive(false);
			comC.isMe = true;
			friendChatLab.text = "";
			chatInput.value = "";
			FriendSystem.Instance().addMyChat(_selectFriend.name_,comC);
			chatCellList.Add(tf);
		}
		cGrid.Reposition ();
		scrollBar.value = 1;
	}

	private void OnFriendTabBtn(ButtonScript obj, object args, int param1, int param2)
	{
		friendTabBtn.isEnabled = false;
		offTabBtn.isEnabled = true;
		leftPane.gameObject.SetActive(true);
		offPane.gameObject.SetActive(false);

		if(friendBtn.transform.FindChild("arrows").GetComponent<UISprite>().spriteName == "sanjiao2")
		{
			UpdataFriendList();
		}
		else if(blackBtn.transform.FindChild("arrows").GetComponent<UISprite>().spriteName == "sanjiao2")
		{
			UpdataBlackList();
		}


	}

	private void OnOffTabBtn(ButtonScript obj, object args, int param1, int param2)
	{
		friendTabBtn.isEnabled = true;
		offTabBtn.isEnabled = false;
		leftPane.gameObject.SetActive(false);
		offPane.gameObject.SetActive(true);
		UpdateLatelyList ();
	}

	private void OnHuanBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.requestReferrFriend ();
	}

	private void OnCtlSendBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_selectFriend == null)
		{
			return; 
		}
		funPane.gameObject.SetActive (false);
		///recomPane.gameObject.SetActive (false);
		blackPane.gameObject.SetActive (false);
		chatLisPane.gameObject.SetActive (true);
		UpdateChatList(_selectFriend);
	}

	private void OnCtlLookBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_selectFriend == null)
		{
			return;
		}
		NetConnection.Instance.queryPlayerbyName(_selectFriend.name_);

		funPane.gameObject.SetActive (false);
	}
	private void OnCtlTeamBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_selectFriend == null)
		{
			return;
		}
		funPane.gameObject.SetActive (false);
		if(TeamSystem.IsInTeam()&&TeamSystem.IsTeamLeader())
		{
			NetConnection.Instance.inviteTeamMember(_selectFriend.name_);
		}else
		{
			if(!TeamSystem.IsInTeam())
			{
				NetConnection.Instance.requestJoinTeam(_selectFriend.name_);
			}else
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("yizaiduiwuqong"));
			}

		}
	}
	
	private void OnCtlDelBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_selectFriend == null)
		{
			return;
		}
		funPane.gameObject.SetActive (false);
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("delfriend"), () => {

			NetConnection.Instance.delFriend(_selectFriend.instId_);
		});
	}

	private void OnCtlBlackBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_selectFriend == null)
		{
			return;
		}
		if(FriendSystem.Instance().blacks_.Count >= 100)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}
		if(FriendSystem.Instance().blacks_.Count >= 100)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("addback").Replace("{n}",_selectFriend.name_),()=>{
			NetConnection.Instance.addBlacklist(_selectFriend.instId_);
		});


		funPane.gameObject.SetActive (false);
	}


	private void UpdateChatList(COM_ContactInfo contact)
	{
		List<COM_Chat> strArr = FriendSystem.Instance ().ChatCache (contact);

			for (int i=0;i<chatCellList.Count;i++)
			{
				//chatGrid.RemoveChild(chatCellList[i].transform);
				cGrid.RemoveChild(chatCellList[i].transform);
				chatCellList[i].transform.parent = null;
				chatCellList[i].gameObject.SetActive(false);
				GameObject.Destroy(chatCellList[i]);
			}
			chatCellList.Clear();
		//	return;
		//}
		if (strArr != null && strArr.Count > 0)
		{
		for(int i=0;i<strArr.Count;i++)
		{
			if(!strArr[i].isMe)
			{
				GameObject tf = null;
				tf = GameObject.Instantiate(_GridItem) as GameObject;
				cGrid.AddChild(tf.transform);
				tf.transform.localScale = Vector3.one;
				tf.gameObject.SetActive(true);
				ChatMaxCell cmi = tf.gameObject.GetComponent<ChatMaxCell>();
				COM_ChatInfo info = new COM_ChatInfo();
				Filt(ref strArr[i].content_,0,1);
				info.content_ = strArr[i].content_;
				info.ck_ =  ChatKind.CK_Friend;
				info.playerName_ = contact.name_;
				info.assetId_ = (ushort)contact.assetId_;
				cmi.Info = info;
				cmi._LChatKindBackground.gameObject.SetActive(false);
				cmi._RChatKindBackground.gameObject.SetActive(false);
				chatCellList.Add(tf);
			}
			else
			{
					GameObject tf = null;
					tf = GameObject.Instantiate(_GridItem) as GameObject;
					cGrid.AddChild(tf.transform);
					tf.transform.localScale = Vector3.one;
					tf.gameObject.SetActive(true);
					ChatMaxCell cmi = tf.gameObject.GetComponent<ChatMaxCell>();
					COM_ChatInfo info = new COM_ChatInfo();
					Filt(ref strArr[i].content_,0,1);
					info.content_ = strArr[i].content_;
					info.ck_ =  ChatKind.CK_Friend;
					info.playerName_ = GamePlayer.Instance.InstName;
					info.assetId_ =(ushort)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId);
					cmi.Info = info;
					cmi._LChatKindBackground.gameObject.SetActive(false);
					cmi._RChatKindBackground.gameObject.SetActive(false);
					chatCellList.Add(tf);
			}
		}
		}
		cGrid.Reposition ();
		scrollBar.value = 1;
		//FriendSystem.Instance ().ChatDict.Remove(contact.name_);
	}



	protected override void DoHide()
	{
		FriendSystem.Instance().UpdateFriend -= OnUpdataFriendList;
		FriendSystem.Instance().UpdateBlack -= OnUpdataBlackList;
		FriendSystem.Instance ().FriendChat -= OnFriendChat;
		FriendSystem.Instance ().FindFriendOkEvent -= FindFriendOk;
		FriendSystem.Instance ().FindFriendFail -= FindFriendFail;
		FriendSystem.Instance().UpdateOffen -= OnUpdateOffens;
		FriendSystem.Instance ().recommendEvent -= OnRecommendEvent;
		ChartsSystem.QueryPlayerEventOk -= OnQueryPlayer;
		FriendSystem.Instance ().friendOnLineEvent -= OnLineEvent;
		base.DoHide();
	}

	void Filt(ref string matchStr, int start, int len)
	{
		if(len <= 0 || len > matchStr.Length)
			return;
		
		//????????? ???
		string tStr = matchStr.Substring(start, len);
		if(tStr.Contains("*"))
		{
			if(start + len + 1 <= matchStr.Length)
				Filt(ref matchStr, start + 1, len);
			else
				Filt(ref matchStr, 0, len + 1);
			return;
		}
		
		//??????????? ???
		List<string> fdata = FilterwordData.GetData (len);
		if(fdata == null)
		{
			if(start + len + 1 <= matchStr.Length)
				Filt(ref matchStr, start + 1, len);
			else
				Filt(ref matchStr, 0, len + 1);
			return;
		}
		
		
		//??
		for(int i=0; i < fdata.Count; ++i)
		{
			//????
			if(tStr.Equals(fdata[i]))
			{
				// dsdfsdf
				
				Regex reg=new Regex(tStr);
				string partten = "";
				for(int j=0; j < len; ++j)
				{
					partten += "*";
				}
				matchStr=reg.Replace(matchStr,partten,len,matchStr.IndexOf(tStr));
				//?????
				if(start + len + 1 <= matchStr.Length)
					Filt(ref matchStr, start + 1, len);
				else
					Filt(ref matchStr, 0, len + 1);
				return;
			}
		}
		
		//???? ?????
		if(start + len + 1 <= matchStr.Length)
			Filt(ref matchStr, start + 1, len);
		else
			Filt(ref matchStr, 0, len + 1);
	}



	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}
