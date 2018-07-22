using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class ChatMax : MonoBehaviour
{
    bool _IsInit = false; //初始化一次
    bool _IsDirty = true; //是否需要更新
    int _MakedIndex = 0;
    ChatKind _SendChatKind = ChatKind.CK_World;
    bool[] _OpenChannels = new bool[(int)ChatKind.CK_Max];
    List<COM_ChatInfo> _ChannelInfo = new List<COM_ChatInfo>();
	public ChatGrid cGrid;
	//public UIFont _font;
	public GameObject yuyinTips;
    public UIScrollView _Sv;
    public UIScrollBar _Sb;
    public UIButton _AllButton;
	public UIButton _faceBtn;
	public UIButton _VoiceBtn;
	public UIButton _lockBtn;
   // public UILabel _Input; //输入框
	public UIPanel maxPanel;
	public UIInput _uinput;
    //public UIGrid _Grid; //显示列表
    public GameObject _GridItem; //列表元素
	public GameObject faceObj;
	public UIFont faFont;
	private bool isLock = false;
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

		_GridItem.SetActive (false);
        //undo
        if (!_IsInit)
        {
            for (int i = 0; i < _OpenChannels.Length; ++i)
            {
                _OpenChannels[i] = true; //默认全开
            }
            _AllButton.Disable(); //扯淡的设置
            ChatSystem.RegMakeDirtyFunc(MakeDirty);
            _IsInit = true;
        }
		UIManager.SetButtonEventHandler (_faceBtn.gameObject, EnumButtonEvent.OnClick, OnClickface, 0, 0);
		UIManager.SetButtonEventHandler (_lockBtn.gameObject, EnumButtonEvent.OnClick, OnClicklockBtn, 0, 0);
		UIEventListener.Get (_VoiceBtn.gameObject).onPress = OnPrassVoice;
		_lockBtn.GetComponent<UIButton> ().zoom = true;
		_uinput.characterLimit = 30;
		//StageMgr.OnSceneLoaded += SetDeth;
		InitFace ();
		//UIManager.SetButtonEventHandler (_VoiceBtn.gameObject, EnumButtonEvent.OnClick, OnClickVoice, 0, 0);
    }
	void OnEnable()
	{
		ChatSystem.CloseAudioUIOk += closeAudio;
	}
	void OnDestroy()
	{
		//StageMgr.OnSceneLoaded -= SetDeth;
	}
	void InitFace()
	{
		for(int i =0;i<ChatSystem.faceStrl.Length;i++)
		{
			faFont.AddSymbol(ChatSystem.faceStrl[i],ChatSystem.faceStrl[i]);
		}
	}

	void OnClicklockBtn(ButtonScript obj, object args, int param1, int param2)
	{
		isLock = !isLock;
		if(isLock)
		{
			obj.GetComponentInChildren<UISprite>().spriteName = "jiesuo";
			obj.GetComponentInChildren<UIButton>().normalSprite = "jiesuo";
		}else
		{
			obj.GetComponentInChildren<UISprite>().spriteName = "suoding";
			obj.GetComponentInChildren<UIButton>().normalSprite = "suoding";
		}

	}


	void OnClickface(ButtonScript obj, object args, int param1, int param2)
	{
		faceObj.SetActive (true);

	}
	float PrassTime = 0;
	float maxTime = 15;
	bool istargetPrass = false;
	float tt;
	bool ischaoshi;
    COM_Chat audioChat_ = null;
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
	float mousePosition_Y;
	bool isCanel;
	int index;
    void Update()
    {
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
                int audiolen = Mathf.RoundToInt(Time.realtimeSinceStartup - PrassTime);
				ChatSystem.StopRecord();
                if (ChatSystem.AsyncGetCallBack == null)
                {
                    ChatSystem.AsyncGetCallBack = delegate(byte[] datas)
                    {
                        audioChat_ = new COM_Chat();
                        audioChat_.audio_ = datas;
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

//        GlobalInstanceFunction.Instance.Invoke(() =>
//        {
		cGrid.Reposition();
		if(!isLock)
		{

            _Sb.value = 1;
		}
//        }, 2);

       
        
        _IsDirty = false;
    }

    ///<初始化一个ITEM如果不够长往上加
	/// 


    void _MakedGridItem(int idx, COM_ChatInfo p)
    {
		Transform tf = cGrid.GetChild(idx);
        if (null == tf)
        {
            tf = _Alloc().transform;
            if (null == tf)
            {
            }
			cGrid.AddChild(tf);
            tf.localPosition = Vector3.zero;
            tf.localScale = Vector3.one;
        }
        tf.gameObject.SetActive(true);
        ChatMaxCell cmi = tf.gameObject.GetComponent<ChatMaxCell>();
        cmi.Info = p;

    }

    ///<从IDX(包含)往后的所有ITEM扔到池子里去
    void _CachedFreeGridItem(int idx)
    {
		int gsize = cGrid.transform.childCount;
        if (idx > gsize)
        {
            return; //
        }
        for (int i = 0, c = gsize - idx; i < c; ++i)
        {
			Transform tf = cGrid.RemoveChild(idx);
            if (tf)
            {
                _Free(tf.gameObject);
            }
        }

    }

	public void closeAudio()
	{
		foreach(Transform tr in cGrid.transform)
		{
			ChatMaxCell catc = tr.GetComponent<ChatMaxCell>();
			catc.closeVecUI();
		}
	}
	void OnDisable()
	{
		closeAudio ();
		ChatSystem.CloseAudioUIOk -= closeAudio;
	}
    public void SwapSwitchSystemChannel()
    {
        for (int i = 0; i < _OpenChannels.Length; ++i)
        {
            _OpenChannels[i] = false;
        }
        _OpenChannels[(int)ChatKind.CK_System] = true;
		_SendChatKind = ChatKind.CK_System;
        //_Sb.value = 0;
        _IsDirty = true;
//		_uinput.enabled = true;
//		_uinput.label.text = "";
//		_uinput.GetComponent<BoxCollider> ().enabled = true;
    }

    public void SwapSwitchWorldChannel()
    {
        for (int i = 0; i < _OpenChannels.Length; ++i)
        {
            _OpenChannels[i] = false;
        }
        _OpenChannels[(int)ChatKind.CK_World] = true;
        _SendChatKind = ChatKind.CK_World;
        //_Sb.value = 0;
        _IsDirty = true;
//		_uinput.enabled = true;
//		_uinput.label.text = "";
//		_uinput.GetComponent<BoxCollider> ().enabled = true;
    }

    public void SwapSwitchTeamChannel()
    {
        for (int i = 0; i < _OpenChannels.Length; ++i)
        {
            _OpenChannels[i] = false;
        }
        _OpenChannels[(int)ChatKind.CK_Team] = true;
        _SendChatKind = ChatKind.CK_Team;
        //_Sb.value = 0;
        _IsDirty = true;
//		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team))
//		{
//			//PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
//			_uinput.enabled = false;
//			_uinput.label.text = LanguageManager.instance.GetValue("duiwuweikai");
//			_uinput.GetComponent<BoxCollider> ().enabled = false;
//		}else
//		{
//			_uinput.enabled = true;
//			_uinput.label.text = "";
//			_uinput.GetComponent<BoxCollider> ().enabled = true;
//		}
    }
    public void SwapSwitchGuildChannel()
    {
        for (int i = 0; i < _OpenChannels.Length; ++i)
        {
            _OpenChannels[i] = false;
        }
        _OpenChannels[(int)ChatKind.CK_Guild] = true;
        _SendChatKind = ChatKind.CK_Guild;
        //_Sb.value = 0;
        _IsDirty = true;
//		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Family)|| !GuildSystem.IsInGuild())
//		{
//			//PopText.Instance.Show(LanguageManager.instance.GetValue("jiazuweikaiqi"));
//			_uinput.enabled = false;
//			_uinput.label.text = LanguageManager.instance.GetValue("jiazuweikaiqi");
//			_uinput.GetComponent<BoxCollider> ().enabled = false;
//
//		}else
//		{
//			_uinput.enabled = true;
//			_uinput.label.text = "";
//			_uinput.GetComponent<BoxCollider> ().enabled = true;
//		}
    }

    public void OpenAllChannel()
    {
        for (int i = 0; i < _OpenChannels.Length; ++i)
        {
            _OpenChannels[i] = true;
        }
        _SendChatKind = ChatKind.CK_World;
        //_Sb.value = 0;
        _IsDirty = true; //这个需要去查询吗，俄
//		_uinput.enabled = true;
//		_uinput.label.text = "";
//		_uinput.GetComponent<BoxCollider> ().enabled = true;
    }

    public void OnInputChange()
    {//输入回调 更改屏蔽字
        
    }
	public void SetDeth(string name)
	{
		//maxPanel.depth = 102;
	}
    public void OnInputEnter()
    {//输入完毕
//        if (_Input.text.Length == 0)
//            return;
//        ChatSystem.SendToServer(_SendChatKind,"",_Input.text.Clone()as string);
//        _Input.text = "";

		if (_uinput.value == "")
		{
			PopText.Instance.Show (LanguageManager.instance.GetValue("bunengweikong"));
			return;
		}
						
		chatStr = _uinput.value;
		if(!chatStr[0].Equals('^'))
		{
			Filt (ref chatStr, 0, 1);
			if(IschatChannel())
			{
				ChatSystem.SendToServer(_SendChatKind,"",chatStr);
				
			}
		}else
		{
			ChatSystem.SendToServer(_SendChatKind,"",chatStr);
		}

		_uinput.value = "";

    }
	bool IschatChannel()
	{
		if( _SendChatKind != ChatKind.CK_World)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Family)&& _OpenChannels[(int)ChatKind.CK_Guild])
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("jiazuweikaiqi"));
				return false;
			}
			if(!GuildSystem.IsInGuild()&& _OpenChannels[(int)ChatKind.CK_Guild])
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("meiyoujiazu"));
				return false;
			}
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Team)&& _OpenChannels[(int)ChatKind.CK_Team])
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("duiwuweikai"));
				return false;
			}
			if(!TeamSystem.IsInTeam()&& _OpenChannels[(int)ChatKind.CK_Team])
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("meiyouduiwu"));
				return false;
			}
			if(_OpenChannels[(int)ChatKind.CK_System])
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("bunengfasong"));
				return false;
			}
		}

		return true;
	}
	string chatStr = "";
	List<int> Repindex = new List<int>();
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
}