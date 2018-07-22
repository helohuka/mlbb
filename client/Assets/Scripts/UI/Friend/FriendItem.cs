using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public delegate void ClickFun(GameObject obj, COM_ContactInfo Info);

public class FriendItem : MonoBehaviour
{
	public ClickFun callBack;
	public ClickFun delCallBack;

    public UILabel name_;
    public UILabel level_;
	public UIButton funBtn;
	public UIButton delBtn;
	public UITexture icon;
	public UISprite red;
	public UILabel professLab;
	public UIButton vButton;
	public UISprite chatInfoback;
	public COM_Chat _chatInfo;
	private COM_ContactInfo _friend;
	private List<string> _icons = new List<string>();
	void Start()
	{ 
		UIManager.SetButtonEventHandler (delBtn.gameObject, EnumButtonEvent.OnClick, OnDelBlack, 0, 0);
		UIManager.SetButtonEventHandler (funBtn.gameObject, EnumButtonEvent.OnClick, OnFunBtn, 0, 0);
		if(vButton != null)
		{
			UIManager.SetButtonEventHandler (vButton.gameObject, EnumButtonEvent.OnClick, OnVBtn, 0, 0);
		}
	}
	
	void Update()
	{
	}

	public COM_ContactInfo  ContactInfo
	{
		get
		{
			return _friend;
		}
		set
		{
			if(value!= null)
			{
				_friend = value;

				name_.text = _friend.name_;
				level_.text = _friend.level_.ToString();


                EntityAssetsData assetsData = EntityAssetsData.GetData((int)_friend.assetId_);
                if (assetsData == null)
                    return;
                HeadIconLoader.Instance.LoadIcon(assetsData.assetsIocn_, icon);
                if (!_icons.Contains(assetsData.assetsIocn_))
                {
                    _icons.Add(assetsData.assetsIocn_);
                }
				if(!_friend.isLine_)
				{
					icon.color = Color.gray;//(0f,1f,1f);
				}
				else
				{
					icon.color = Color.white;
				}
                if (Profession.get((JobType)_friend.job_, (int)_friend.jobLevel_) == null)
                    return;
                professLab.text = Profession.get((JobType)_friend.job_, (int)_friend.jobLevel_).jobName_;

			}

		}
	}

	public void IsBlack(bool black)
	{
		if(black)
		{
			delBtn.gameObject.SetActive(true);
			funBtn.gameObject.SetActive(false);
		}
		else
		{
			delBtn.gameObject.SetActive(false);
			funBtn.gameObject.SetActive(true);
		}
	}

	private void OnDelBlack(ButtonScript obj, object args, int param1, int param2)
	{
		if(delCallBack != null)
		{
			delCallBack(this.gameObject,_friend);
		}
	}

	private void OnFunBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(callBack != null)
		{
			callBack(this.gameObject,_friend);
		}
	}

	private void OnVBtn(ButtonScript obj, object args, int param1, int param2)
	{
		ChatSystem.PlayRecord (_chatInfo.audio_,FriendUI.audioSO);
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
	

}

