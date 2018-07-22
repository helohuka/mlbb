using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void ClickAdd(ButtonScript obj, COM_ContactInfo Info);
public class FriendRecommendCell : MonoBehaviour
{
	public ClickAdd callBack;
	public UILabel name_;
	public UILabel level_;
	public UIButton addBtn;
	public UITexture icon;
	public UILabel professLab;
	
	private COM_ContactInfo _friend;
	private List<string> _icons = new List<string>();
	void Start()
	{ 
		UIManager.SetButtonEventHandler (addBtn.gameObject, EnumButtonEvent.OnClick, OnAddFriend, 0, 0);
	}
	
	void Update()
	{
	}
	
	public COM_ContactInfo  ContactInfo
	{
		get		{
			return _friend;
		}
		set
		{
			if(value!= null)
			{
				_friend = value;
				
				name_.text = _friend.name_;
				level_.text = _friend.level_.ToString();
				EntityAssetsData assetsData =EntityAssetsData.GetData((int)_friend.assetId_);
				if(assetsData == null)
					return;
				HeadIconLoader.Instance.LoadIcon(assetsData.assetsIocn_,icon);
				if(!_icons.Contains(assetsData.assetsIocn_))
				{
					_icons.Add(assetsData.assetsIocn_);
				}
				if(Profession.get((JobType)_friend.job_,(int)_friend.jobLevel_) == null)
					return;
				professLab.text = Profession.get((JobType)_friend.job_,(int)_friend.jobLevel_).jobName_;
			}
		}
	}
	
	private void OnAddFriend(ButtonScript obj, object args, int param1, int param2)
	{
		if(callBack!= null)
		{
			callBack(obj,_friend);
		}
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}

