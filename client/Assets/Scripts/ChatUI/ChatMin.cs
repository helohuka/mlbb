using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class ChatMin : MonoBehaviour
{
    bool _IsInit = false; //初始化一次
    bool _IsDirty = true; //是否需要更新

    bool[] _OpenChannels = new bool[(int)ChatKind.CK_Max];
    List<COM_ChatInfo> _ChannelInfo;
	//public UIFont _font;
    public UIGrid _Grid; //显示列表
    public GameObject _GridItem; //列表元素
    public UIScrollView _Sv;
    public UIScrollBar _Sb;
	public ChatGrid cGrid;
	public UIFont facefont;
	public UIButton VecBtn;
	public UIButton haoyouB;
	public GameObject yuyinTips;
	private bool isShowTeamLevel;
	ChatKind _SendChatKind = ChatKind.CK_World;
    Stack<GameObject> _GridItemCache = new Stack<GameObject>(); //缓存
    GameObject _Alloc()
    {
        GameObject o = null;
        if (_GridItemCache.Count != 0)
            o = _GridItemCache.Pop();
        else
        {
            o = GameObject.Instantiate(_GridItem) as GameObject;
           
        }
        return o;
    }
	float PrassTime = 0;
	float maxTime = 15;
	bool istargetPrass = false;
	float tt;
	bool ischaoshi;
	COM_Chat audioChat_ = null;
	float mousePosition_Y;
	bool isCanel;
	int index;
	public UIScrollView ListView_;
	UIPanel listPanel_;
	BoxCollider listDragArea_;
	public GameObject maxObj;
	private void OnPrassVoice(GameObject sender,bool isPrass)
    {
#if UNITY_ANDROID
        if(!XyskAndroidAPI.hasMicrophoneAuth() && isPrass)
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
			if(ischaoshi)return;
			float tm = Time.realtimeSinceStartup - PrassTime;
			ChatSystem.StopRecord();
			if(tm<1)
			{
				
				PopText.Instance.Show(LanguageManager.instance.GetValue("chatVeTime"));
			}else
			{
				if(isCanel)return;
                if (ChatSystem.AsyncGetCallBack == null)
                {
                    ChatSystem.AsyncGetCallBack = delegate(byte[] datas)
                    {
						audioChat_ = new COM_Chat();
                        audioChat_.audio_ = datas;
                        audioChat_.isAudio_ = true;
                        audioChat_.audioTime_ = (int)tm;
                        //int le = Mathf.RoundToInt(ChatSystem.GetClipLength(audioChat_.audio_));
                        ChatSystem.SendToServer(_SendChatKind, "", "", audioChat_.audio_, audioChat_.audioTime_);
                        ClientLog.Instance.Log("audioChat_.audio_=====" + audioChat_.audio_.Length);
                        audioChat_ = null;
                    };
                    ChatSystem.AsyncGet();
                }
			}
		}
	}

    void _Free(GameObject o)
    {
        if (null == o)
            return;
        o.transform.parent = null;
        _GridItemCache.Push(o);
        o.SetActive(false);
    }

    public void MakeDirty()
    {
        _IsDirty = true;
    }

    void Start()
    {
        //undo
        if (!_IsInit)
        {
            for (int i = 0; i < _OpenChannels.Length; ++i)
            {
                _OpenChannels[i] = true; //默认全开
            }
            ChatSystem.RegMakeDirtyFunc(MakeDirty);
            _IsInit = true;
        }
		_OpenChannels[(int)ChatKind.CK_System] = false;
		//InitFace ();
		for(int i =0;i<ChatSystem.faceStrl.Length;i++)
		{
			facefont.AddSymbol(ChatSystem.faceStrl[i],ChatSystem.faceStrl[i]);
		}
		listPanel_ = ListView_.gameObject.GetComponent<UIPanel>();
		listDragArea_ = ListView_.gameObject.GetComponent<BoxCollider>();
		UIEventListener.Get (VecBtn.gameObject).onPress = OnPrassVoice;
		UIManager.SetButtonEventHandler (haoyouB.gameObject, EnumButtonEvent.OnClick,OnClickhaoyouB, 0, 0);
		UIEventListener.Get (ListView_.gameObject).onClick = ShowChatMaxObj;
		GamePlayer.Instance.OpenSubSystemEnvet += new RequestEventHandler<ulong> (UpdateOpenSystem);
		ChatSystem.ShowItemInstResOk += showInsetItem;
		ChatSystem.ShowBabyInstResOk += showbabyInst;
		UpdateOpenSystem (0);

        GuideManager.Instance.RegistGuideAim(haoyouB.gameObject, GuideAimType.GAT_MainFriend);
    }
	public void UpdateOpenSystem(ulong open)
	{
		haoyouB.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Friend));

	}
	void OnClickhaoyouB(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Friend))
		{
			PopText.Instance.Show (LanguageManager.instance.GetValue("cannotopen"));
			return;
		}
		FriendUI.SwithShowMe();
		if(GamePlayer.Instance.OpenFunEffectBtns.Contains((int)OpenSubSystemFlag.OSSF_Friend))
		{
			GamePlayer.Instance.OpenFunEffectBtns.Remove((int)OpenSubSystemFlag.OSSF_Friend);
		}
		
		Transform txObj = obj.gameObject.transform.FindChild ("lizixuanzhuan(Clone)");
		if(txObj != null)
		{
			txObj.gameObject.SetActive(false);
			Destroy(txObj.gameObject);
		}	
		NetConnection.Instance.uiBehavior (UIBehaviorType.UBT_Friend);
	}
	void ShowChatMaxObj(GameObject sender)
	{
		gameObject.SetActive (false);
		maxObj.SetActive (true);
	}
	void showInsetItem(COM_ShowItemInst ItemInst)
	{
		if(TipsItemUI.instance != null )
		{
			UIPanel pa =  ChatUI.Instance.maxChatObj.GetComponent<UIPanel>();
			TipsItemUI.instance.setData (ItemInst.itemInst_, pa.depth+1);
		}

		///ItemsTips.SwithShowMe((int)ItemInst.itemInst_.itemId_);
	}
	void showbabyInst(COM_ShowbabyInst babyInst)
	{
		ChatBabytips.SwithShowMe(babyInst.babyInst_);
		TipsItemUI.instance.HideTips ();
	}
	//int index;
    void Update()
    {
		listDragArea_.center = listPanel_.clipOffset;
		if(istargetPrass)
		{
			if(Input.GetMouseButtonDown(0))
			{
				mousePosition_Y = Input.mousePosition.y;
			}
			if(Input.GetMouseButton(0))
			{
				if(Input.mousePosition.y - mousePosition_Y>80)
				{
					isCanel = true;
				}
			}
			tt = Mathf.Max(0, maxTime - (Time.realtimeSinceStartup - PrassTime));
			if(tt==0)
			{
				yuyinTips.SetActive(false);
				ChatSystem.StopRecord();
                int audiolen = Mathf.RoundToInt(Time.realtimeSinceStartup - PrassTime);
                if (ChatSystem.AsyncGetCallBack == null)
                {
                    ChatSystem.AsyncGetCallBack = delegate(byte[] datas)
                    {
                        audioChat_ = new COM_Chat();
                        audioChat_.isAudio_ = true;
                        audioChat_.audioTime_ = audiolen;
                        //int le = Mathf.RoundToInt(ChatSystem.GetClipLength(audioChat_.audio_));
                        ChatSystem.SendToServer(_SendChatKind, "", "", audioChat_.audio_, audioChat_.audioTime_);
                        ClientLog.Instance.Log("audioChat_.audio_====+++++=" + audioChat_.audio_.Length);
                        ischaoshi = true;
                        istargetPrass = false;
                        audioChat_ = null;
                    };
                    ChatSystem.AsyncGet();
                }
			}
		}


        if (!_IsDirty)
            return;

		if (!ChatSystem.GetChannelInfo(_OpenChannels, ref _ChannelInfo,ref index))
            return;
        if (_ChannelInfo.Count == 0)
        { //木有
            _CachedFreeGridItem(0);
            _IsDirty = false;
            return;
        }

        for (int i = 0; i < _ChannelInfo.Count; ++i)
        {
		   _MakedGridItem(i, _ChannelInfo[i]); //链表下标访问 是不是很
        }
        _CachedFreeGridItem(_ChannelInfo.Count); //清理多余物件 

        //_Grid.Reposition();
//        GlobalInstanceFunction.Instance.Invoke(() =>
//        {
			cGrid.Reposition ();
            _Sb.value = 1;
//        }, 2);
        _IsDirty = false;
    }

	public void closeAudio()
	{
		foreach(Transform tr in _Grid.transform)
		{
			ChatMinCell catc = tr.GetComponent<ChatMinCell>();
			catc.closeVecUI();
            //AudioSource cmc = tr.GetComponent<AudioSource>();
            //cmc.Stop();
		}
	}
	void OnDisable()
	{
		closeAudio ();
	}

    ///<初始化一个ITEM如果不够长往上加
    void _MakedGridItem(int idx, COM_ChatInfo p)
    {

        //Transform tf = _Grid.GetChild(idx);
		Transform tf = cGrid.GetChild (idx);
		if(p.teamId_ != 0)
		{
			if(!IsTeamLevelChannel(p)&&isShowTeamLevel)
			{
				if(tf==null)return;
				_Free(tf.gameObject);
				return;
			}
		}

        if (null == tf)
        {
            tf = _Alloc().transform;
			cGrid.AddChild(tf);
            //_Grid.AddChild(tf);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
        }
        tf.gameObject.SetActive(true);
        ChatMinCell cmi = tf.GetComponentInChildren<ChatMinCell>();
        cmi.Info = p;
    }

    ///<从IDX(包含)往后的所有ITEM扔到池子里去
    void _CachedFreeGridItem(int idx)
    {
        //int gsize = _Grid.transform.childCount;
		int gsize = cGrid.transform.childCount;
        if (idx > gsize)
        {
            return; //
        }
        for (int i = 0, c = gsize - idx; i < c; ++i)
        {
            //Transform tf = _Grid.RemoveChild(idx);
			Transform tf = cGrid.RemoveChild(idx);
            if (tf)
            {
                _Free(tf.gameObject);
            }
        }
        
	} 

    public void SwapSwitchWorldChannel()
	{
       _OpenChannels[(int)ChatKind.CK_World] = !_OpenChannels[(int)ChatKind.CK_World];

        _IsDirty = true;
    }

    public void SwapSwitchTeamChannel()
    {
        _OpenChannels[(int)ChatKind.CK_Team] = !_OpenChannels[(int)ChatKind.CK_Team];
        _IsDirty = true;
    }
    public void SwapSwitchGuildChannel()
    {
        _OpenChannels[(int)ChatKind.CK_Guild] = !_OpenChannels[(int)ChatKind.CK_Guild];
        _IsDirty = true;
    }
	public bool IsTeamLevelChannel(COM_ChatInfo chatinfo)
	{

		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)>chatinfo.teamMinLevel_ && GamePlayer.Instance.GetIprop(PropertyType.PT_Level)<=chatinfo.teamMaxLevel_)
		{
			return true;
		}
		return false;
	}
	public void SwapSwitchLevelChannel(UIToggle toggle)
	{
		isShowTeamLevel = toggle.value;
		_IsDirty = true;
	}
}