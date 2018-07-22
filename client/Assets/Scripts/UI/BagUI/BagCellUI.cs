using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BagCellUI : MonoBehaviour
{

	public UISprite pane;
	public UITexture itemIcon;
	public UILabel countLab;
	public UISprite blackImg;
	public UISprite debirsImg;
	public UISprite suoImg;
	//
	private COM_Item _Item;
	private bool _isLock;
	private List<string> _icons = new List<string>();
	void Start ()
	{

	}
	

	public COM_Item Item
	{
		set
		{
			if(value == null)
			{
				//UIManager.RemoveButtonAllEventHandler (pane.gameObject);
				itemIcon.gameObject.SetActive(false);
				countLab.gameObject.SetActive(false);
				if(blackImg != null)
					blackImg.gameObject.SetActive(false);
				if(debirsImg != null)
					debirsImg.gameObject.SetActive(false);
				if(suoImg != null)
					suoImg.gameObject.SetActive(false);
				pane.spriteName = "bb_daojukuang1";
				_Item = value;
				return;
			}
			_Item = value;
			ItemData data = ItemData.GetData((int)_Item.itemId_);
			if(data == null)
				return;
			itemIcon.gameObject.SetActive(true);
			if(_Item.stack_ > 1)
			{
				countLab.gameObject.SetActive(true);
				countLab.text = _Item.stack_.ToString();
			}
			else
				countLab.gameObject.SetActive(false);
			//itemIcon.spriteName = data.icon_;countLab.gameObject.SetActive(true);
			HeadIconLoader.Instance.LoadIcon(data.icon_,itemIcon);
			pane.spriteName = BagSystem.instance.GetQualityBack((int)data.quality_);
			if(!_icons.Contains(data.icon_))
			{
				_icons.Add(data.icon_);
			}
			if(data.mainType_ == ItemMainType.IMT_Debris)
			{
				if(debirsImg != null)
				{
					debirsImg.gameObject.SetActive(true);
				}
			}
			else
			{
				if(debirsImg != null)
				{
					debirsImg.gameObject.SetActive(false);
				}
			}
			if(_Item.isLock_)
			{
				if(suoImg != null)
					suoImg.gameObject.SetActive(true);
			}
			else
			{
				if(suoImg != null)
					suoImg.gameObject.SetActive(false);
			}
			if(data.maxCount_ > 1)
			 	countLab.text = _Item.stack_.ToString();
			else
				countLab.gameObject.SetActive(false);

			if(ItemData.GetData((int)_Item.itemId_).subType_ == ItemSubType.IST_Fashion)
			{
				if(_Item.usedTimeout_ > 0)
				{
					countLab.gameObject.SetActive(true);
					countLab.text = FormatTimeHasDay(_Item.usedTimeout_) + LanguageManager.instance.GetValue("Sign_Day");
				}
				else
				{
					countLab.gameObject.SetActive(false);
				}
			}
		}
		get
		{
			return _Item;
		}
	}

	public  string FormatTimeHasDay(int time)
	{
		int day = time/86400;
		if(time%86400 > 0)
			day++ ;
		return day.ToString ();
	}
	public bool isLock
	{
		set
		{
			if(!value)
			{
				pane.spriteName = "suo";
			}
			else
			{
				if(Item != null)
				{
					pane.spriteName = BagSystem.instance.GetQualityBack((int)ItemData.GetData((int)Item.itemId_).quality_);
				}
				else
				{
					pane.spriteName = "bb_daojukuang1";
				}
			}
			_isLock = value;
		}
		get
		{
			return _isLock;
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


