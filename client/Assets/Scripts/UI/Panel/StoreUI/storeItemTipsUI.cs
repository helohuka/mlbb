using UnityEngine;
using System.Collections;

public class storeItemTipsUI : MonoBehaviour
{
	public UISprite tipsPane;
	public UILabel itemNameLab;
	public UILabel itemDescLab;
	public UISprite itemIcon;

	private ItemData _itemData;

	void Start ()
	{

	}



	public ItemData Item
	{
		set
		{
			if(value != null)
			{
				_itemData = value;
				//ItemData data  = ItemData.GetData((int)_itemData.instId_);
				itemNameLab.text =  _itemData.name_;
				itemIcon.spriteName = _itemData.icon_;
				itemDescLab.text = _itemData.desc_;
			}
		}
		get
		{
			return _itemData;
		}
	}
}

