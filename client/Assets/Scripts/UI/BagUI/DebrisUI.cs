using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebrisUI : MonoBehaviour
{

	public UITexture debrisIcon;
	public UITexture itemIcon;
	public UIButton okBtn;
	public UIButton cancelBtn;
	public UILabel dNumLab;
	public UILabel itemUnmLab;
	public UILabel itemNameLab;
	public UILabel debrisNameLab;
	public UISprite itemBack0;
	public UISprite itemBack1;
	public UILabel bagDebrisTitleLab;
	public UILabel bagDebrisOkBtnLab;
	public UILabel bagDebrisCancelLab;

	private List<string> _icons = new List<string>();

	private COM_Item _item;
	void Start ()
	{
		UIManager.SetButtonEventHandler (okBtn.gameObject, EnumButtonEvent.OnClick, OnOKBtn, 0, 0);
		UIManager.SetButtonEventHandler (cancelBtn.gameObject, EnumButtonEvent.OnClick, OnCancelBtn, 0, 0);

		bagDebrisTitleLab.text = LanguageManager.instance.GetValue("bagDebrisTitleLab");
		bagDebrisOkBtnLab.text = LanguageManager.instance.GetValue("bagDebrisOkBtnLab");;
		bagDebrisCancelLab.text = LanguageManager.instance.GetValue("bagDebrisCancelLab");;
	}
	
	public COM_Item Debris
	{
		set
		{
			if(value != null)
			{
				_item = value;
				DebrisData dData = DebrisData.GetData((int)_item.itemId_);
				HeadIconLoader.Instance.LoadIcon(ItemData.GetData((int)_item.itemId_).icon_,debrisIcon);
				if(!_icons.Contains(ItemData.GetData((int)_item.itemId_).icon_))
				{
					_icons.Add(ItemData.GetData((int)_item.itemId_).icon_);
				}
				debrisNameLab.text = ItemData.GetData((int)_item.itemId_).name_;
				dNumLab.text =_item.stack_ + "/" + dData.needNum_;
				if(_item.stack_ < dData.needNum_)
				{
					dNumLab.color = Color.red;
					okBtn.isEnabled = false;
				}
				else
				{
					dNumLab.color = Color.black;
					okBtn.isEnabled = true;
				}
				itemBack0.spriteName = BagSystem.instance.GetQualityBack((int)ItemData.GetData((int)_item.itemId_).quality_);
				itemBack1.spriteName = BagSystem.instance.GetQualityBack((int)ItemData.GetData((int)_item.itemId_).quality_);
			
				if(dData.subType_== ItemSubType.IST_BabyDebris)
				{

					HeadIconLoader.Instance.LoadIcon( EntityAssetsData.GetData( BabyData.GetData((int)dData.itemId_)._AssetsID).assetsIocn_,itemIcon);
					if(!_icons.Contains(EntityAssetsData.GetData( BabyData.GetData((int)dData.itemId_)._AssetsID).assetsIocn_))
					{
						_icons.Add(EntityAssetsData.GetData( BabyData.GetData((int)dData.itemId_)._AssetsID).assetsIocn_);
					}
					itemNameLab.text = BabyData.GetData((int)dData.itemId_)._Name;
				}
				else if(dData.subType_== ItemSubType.IST_EmployeeDebris)
				{
					//HeadIconLoader.Instance.LoadIcon( EntityAssetsData.GetData( EmployeeData.GetData((int)dData.itemId_).asset_id).assetsIocn_,itemIcon);
					HeadIconLoader.Instance.LoadIcon(ItemData.GetData((int)_item.itemId_).icon_,itemIcon);
					//if(!_icons.Contains(EntityAssetsData.GetData( EmployeeData.GetData((int)dData.itemId_).asset_id).assetsIocn_))
					//{
					//	_icons.Add(EntityAssetsData.GetData( EmployeeData.GetData((int)dData.itemId_).asset_id).assetsIocn_);
					//}

					itemNameLab.text =EmployeeData.GetData((int)dData.itemId_).name_;
				}
				else if(dData.subType_== ItemSubType.IST_ItemDebris)
				{
					HeadIconLoader.Instance.LoadIcon(ItemData.GetData(dData.itemId_).icon_,itemIcon);
					
					if(!_icons.Contains(ItemData.GetData(dData.itemId_).icon_))
					{
						_icons.Add(ItemData.GetData(dData.itemId_).icon_);
					}
					
					itemNameLab.text =ItemData.GetData(dData.itemId_).name_;
				}
				itemUnmLab.text = dData.itemNum_.ToString();

			}
			else
			{
				if(_item != null)
				{
					DebrisData dData = DebrisData.GetData((int)_item.itemId_);
					debrisNameLab.text = ItemData.GetData((int)_item.itemId_).name_;
					dNumLab.text =  "0/" + dData.needNum_;
					dNumLab.color = Color.red;
					okBtn.isEnabled = false;
				}
			}
		}
		get
		{
			return _item;
		}
	}
		 

	public void UpdateDebrisItem()
	{
		PopText.Instance.Show("OK  ");
		COM_Item item = BagSystem.instance.GetItemByInstId ((int)Debris.instId_);
		if(item == null)
		{
			dNumLab.text =  "0/" + DebrisData.GetData((int)_item.itemId_).needNum_;
			okBtn.isEnabled = false;
			dNumLab.color = Color.red;
		}
		else
		{
			Debris = item;
		}
	}


	private void OnOKBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.makeDebirsItem ((int)_item.instId_, 1);
	}

	private void OnCancelBtn(ButtonScript obj, object args, int param1, int param2)
	{

		gameObject.SetActive (false);
	}
	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}

