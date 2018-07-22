using UnityEngine;
using System.Collections;
[RequireComponent (typeof(AudioSource))]
public class CellChat : MonoBehaviour {

	public GameObject rightLabelDi;
	public GameObject rightSysDi;
	public UILabel rightLabel;
	public UILabel rightSystemLabel;
	public UILabel rightnameLabel;
	public UIButton rightVecBtn;
	public UIButton rightIconbtn;
	public UILabel rightNamelabel;
	public UITexture righticontx;

	public GameObject leftLabelDi;
	public GameObject leftSysDi;
	public UILabel leftLabel;
	public UILabel leftSystemLabel;
	public UILabel leftnameLabel;
	public UIButton leftVecBtn;
	public UIButton leftIconbtn;
	public UILabel leftNamelabel;
	public UITexture lefticontx;


	public UISprite systemSp;
	public bool isSelf;
	public GameObject objSet;

	public bool isVec = false;
	private COM_ChatInfo ChatInfo_;
	AudioSource audioSource;
	public COM_ChatInfo ChatInfo
	{
		set
		{
			if(value != null)
			{
				ChatInfo_ = value;
				if(ChatInfo_.ck_ == ChatKind.CK_World)
				{
					rightSystemLabel.text = LanguageManager.instance.GetValue("CK_World");
					leftSystemLabel.text = LanguageManager.instance.GetValue("CK_World");
					rightnameLabel.text = ChatInfo_.playerName_;	
					leftnameLabel.text = ChatInfo_.playerName_;	

					setArActivity(false);
				}else
					if(ChatInfo_.ck_ == ChatKind.CK_System)
				{
					rightSystemLabel.text = LanguageManager.instance.GetValue("CK_System");
					leftSystemLabel.text = LanguageManager.instance.GetValue("CK_System");
					rightnameLabel.text = ChatInfo_.playerName_;	
					leftnameLabel.text = ChatInfo_.playerName_;
					setArActivity(true);
				}else
					if(ChatInfo_.ck_ == ChatKind.CK_Team)
				{
					rightSystemLabel.text = LanguageManager.instance.GetValue("CK_Team");
					leftSystemLabel.text = LanguageManager.instance.GetValue("CK_Team");
					rightnameLabel.text = ChatInfo_.playerName_;	
					leftnameLabel.text = ChatInfo_.playerName_;
					//nameLabel.text = ChatInfo_.playerName_;		
					setArActivity(false);
				}
				else
					if(ChatInfo_.ck_ == ChatKind.CK_Guild)
				{
					rightSystemLabel.text = LanguageManager.instance.GetValue("CK_Guild");
					leftSystemLabel.text = LanguageManager.instance.GetValue("CK_Guild");
					rightnameLabel.text = ChatInfo_.playerName_;	
					leftnameLabel.text = ChatInfo_.playerName_;
					//nameLabel.text = ChatInfo_.guildName_;
					setArActivity(false);
				}
			}
		}
		get
		{
			return ChatInfo_;
		}
	}
	void setArActivity(bool issys)
	{

		if(isSelf)
		{
			rightLabelDi.SetActive(true);
			rightSysDi.SetActive(true);
			rightIconbtn.gameObject.SetActive(true);
			rightNamelabel.gameObject.SetActive(true);
			righticontx.gameObject.SetActive(true);
			HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(ChatInfo.assetId_).assetsIocn_, righticontx);
			if(ChatInfo.audio_.Length>0)
			{
				rightVecBtn.gameObject.SetActive(true);
				rightLabelDi.SetActive(false);
			}else
			{
				rightVecBtn.gameObject.SetActive(false);
				rightLabelDi.SetActive(true);
				//rightLabel.text = ChatInfo.content_;
                rightLabel.text = ChatInfo.content_;
                bool beginSkip = false;
                int realCount = 0;
                for (int i = 0; i < ChatInfo.content_.Length; ++i)
                {
                    if (ChatInfo.content_[i].Equals(']'))
                    {
                        beginSkip = false;
                        continue;
                    }

                    if (ChatInfo.content_[i].Equals('['))
                        beginSkip = true;

                    if (beginSkip)
                        continue;

                    realCount++;
                }
                // 15个字符一行
                int line = realCount / 15 + 1;
                if (realCount > 15)
                {
                    rightLabel.overflowMethod = UILabel.Overflow.ResizeHeight;
                    rightLabel.alignment = NGUIText.Alignment.Left;
                    rightLabel.width = 20 * 15;
                    rightLabel.height = 28 * line;
                }
                rightLabelDi.GetComponent<UISprite>().width = rightLabel.width + 50;
                rightLabelDi.GetComponent<UISprite>().height = rightLabel.height + 20;
			}

			leftLabelDi.SetActive(false);
			leftSysDi.SetActive(false);
			leftVecBtn.gameObject.SetActive(false);
			leftIconbtn.gameObject.SetActive(false);
			leftNamelabel.gameObject.SetActive(false);
			lefticontx.gameObject.SetActive(false);
			systemSp.gameObject.SetActive(false);
		}else
		{
			if(issys)
			{
				leftLabelDi.SetActive(true);
				leftSysDi.SetActive(true);
				leftVecBtn.gameObject.SetActive(false);
				leftIconbtn.gameObject.SetActive(false);
				leftNamelabel.gameObject.SetActive(false);
				systemSp.gameObject.SetActive(true);
				lefticontx.gameObject.SetActive(false);
				righticontx.gameObject.SetActive(false);
				rightLabelDi.SetActive(false);
				rightSysDi.SetActive(false);
				rightVecBtn.gameObject.SetActive(false);
				rightIconbtn.gameObject.SetActive(false);
				rightNamelabel.gameObject.SetActive(false);
				if(ChatInfo.audio_.Length>0)
				{
					leftVecBtn.gameObject.SetActive(true);
					leftLabelDi.SetActive(false);
				}else
				{
                    rightVecBtn.gameObject.SetActive(false);
                    leftLabelDi.SetActive(true);
                    //rightLabel.text = ChatInfo.content_;
                    leftLabel.text = ChatInfo.content_;
                    bool beginSkip = false;
                    int realCount = 0;
                    for (int i = 0; i < ChatInfo.content_.Length; ++i)
                    {
                        if (ChatInfo.content_[i].Equals(']'))
                        {
                            beginSkip = false;
                            continue;
                        }

                        if (ChatInfo.content_[i].Equals('['))
                            beginSkip = true;

                        if (beginSkip)
                            continue;

                        realCount++;
                    }
                    // 15个字符一行
                    int line = realCount / 15 + 1;
                    if (realCount > 15)
                    {
                        leftLabel.overflowMethod = UILabel.Overflow.ResizeHeight;
                        leftLabel.alignment = NGUIText.Alignment.Left;
                        leftLabel.width = 20 * 15;
                        leftLabel.height = 28 * line;
                    }
                    leftLabelDi.GetComponent<UISprite>().width = leftLabel.width + 50;
                    leftLabelDi.GetComponent<UISprite>().height = leftLabel.height + 20;
				}
			}else
			{
				rightLabelDi.SetActive(false);
				rightSysDi.SetActive(false);
				rightVecBtn.gameObject.SetActive(false);
				rightIconbtn.gameObject.SetActive(false);
				rightNamelabel.gameObject.SetActive(false);
				righticontx.gameObject.SetActive(false);
				leftLabelDi.SetActive(true);
				leftSysDi.SetActive(true);
				leftVecBtn.gameObject.SetActive(true);
				leftIconbtn.gameObject.SetActive(true);
				leftNamelabel.gameObject.SetActive(true);
				lefticontx.gameObject.SetActive(true);
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(ChatInfo.assetId_).assetsIocn_, lefticontx);
				if(ChatInfo.audio_.Length>0)
				{
					leftVecBtn.gameObject.SetActive(true);
					leftLabelDi.SetActive(false);
				}else
				{
                    leftVecBtn.gameObject.SetActive(false);
                    leftLabelDi.SetActive(true);
                    //rightLabel.text = ChatInfo.content_;
                    leftLabel.text = ChatInfo.content_;
                    bool beginSkip = false;
                    int realCount = 0;
                    for (int i = 0; i < ChatInfo.content_.Length; ++i)
                    {
                        if (ChatInfo.content_[i].Equals(']'))
                        {
                            beginSkip = false;
                            continue;
                        }

                        if (ChatInfo.content_[i].Equals('['))
                            beginSkip = true;

                        if (beginSkip)
                            continue;

                        realCount++;
                    }
                    // 15个字符一行
                    int line = realCount / 15 + 1;
                    if (realCount > 15)
                    {
                        leftLabel.overflowMethod = UILabel.Overflow.ResizeHeight;
                        leftLabel.alignment = NGUIText.Alignment.Left;
                        leftLabel.width = 20 * 15;
                        leftLabel.height = 28 * line;
                    }
                    leftLabelDi.GetComponent<UISprite>().width = leftLabel.width + 50;
                    leftLabelDi.GetComponent<UISprite>().height = leftLabel.height + 20;
				}
				systemSp.gameObject.SetActive(false);
			}
		}


	}

	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.enabled = true;
		UIManager.SetButtonEventHandler (rightIconbtn.gameObject, EnumButtonEvent.OnClick, OnClickIocnBtn, 0, 0);
		UIManager.SetButtonEventHandler (leftIconbtn.gameObject, EnumButtonEvent.OnClick, OnClickIocnBtn, 0, 0);
		UIManager.SetButtonEventHandler (rightVecBtn.gameObject, EnumButtonEvent.OnClick, OnClickPlayVec, 0, 0);
		UIManager.SetButtonEventHandler (leftVecBtn.gameObject, EnumButtonEvent.OnClick, OnClickPlayVec, 0, 0);

	}
	private void OnClickPlayVec(ButtonScript obj, object args, int param1, int param2)
	{
		//ChatSystem.instance.PlayRecord(ChatInfo.audio_,audioSource);
	}
	private void OnClickIocnBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (ChatInfo.playerName_ == GamePlayer.Instance.InstName)
		{
			return;
		}
		objSet.SetActive (true);
//		UISprite sp = obj.GetComponent<UIButton> ().tweenTarget.GetComponent<UISprite>();
//		UISprite sps = objSet.transform.FindChild ("kuang").GetComponent<UISprite>();
		objSet.transform.position = new Vector3 (objSet.transform.position.x, obj.gameObject.transform.position.y , 0f);

	}
}
