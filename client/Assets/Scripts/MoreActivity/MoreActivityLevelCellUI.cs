using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoreActivityLevelCellUI : MonoBehaviour
{
	public List<UISprite> iconList = new List<UISprite>();
	public UIButton getBtn;
	public UILabel levelLab;
	public UISprite getImg;
	int level_;

	void Start ()
	{
		UIManager.SetButtonEventHandler (getBtn.gameObject, EnumButtonEvent.OnClick, OnGetBtn, 0, 0);
		GamePlayer.Instance.moreLevelEnvet += new RequestEventHandler<int> (RewardEnvet);

	}

	public int MoreLevel
	{
		set
		{
			level_ = value;
			if(level_ <= 0)
				return;
			levelLab.text = level_.ToString() + LanguageManager.instance.GetValue("jilibao");
			MoreLevelData data  = MoreLevelData.GetData(level_);
			for(int i=0;i<data.items_.Count;i++)
			{
				iconList[i].gameObject.SetActive(true);
				ItemCellUI itemcell = UIManager.Instance.AddItemCellUI(iconList[i],(uint)data.items_[i]);
				itemcell.showTips = true;
				itemcell.ItemCount = data.itemNum_[i];
			}
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= level_)
			{
				if(GamePlayer.Instance.levelgift.Contains((uint)level_))
				{
					getImg.gameObject.SetActive(true);
					getBtn.gameObject.SetActive(false);
				}
				else
				{
					getImg.gameObject.SetActive(false);
					getBtn.isEnabled = true;
				}
			}
			else
			{
				getImg.gameObject.SetActive(false);
				getBtn.isEnabled = false;
			}
		}
		get
		{
			return level_;
		}
	}

	private void OnGetBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.requestLevelGift (level_);
	}

	void RewardEnvet(int level)
	{
		if(level == level_)
		{
			if(GamePlayer.Instance.levelgift.Contains((uint)level_))
			{
				getImg.gameObject.SetActive(true);
				getBtn.gameObject.SetActive(false);
			}
			else
			{
				if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= level_)
				{
					getImg.gameObject.SetActive(false);
					getBtn.isEnabled = true;
				}
				else
				{
					getImg.gameObject.SetActive(false);
					getBtn.isEnabled = false;
				}
			}
		}
	}

	void OnDestroy()
	{
		GamePlayer.Instance.moreLevelEnvet -= RewardEnvet;
	}


}

