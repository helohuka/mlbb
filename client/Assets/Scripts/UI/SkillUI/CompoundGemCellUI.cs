using UnityEngine;
using System.Collections;

public delegate void inlayGem(COM_Item item);
public class CompoundGemCellUI : MonoBehaviour
{
	public inlayGem callBack;
	public UILabel nameLab;
	public UIButton inlayBtn;
	public UISprite itemIcon;
	private ItemData _itemData;
	private COM_Item _itemInst;

	void Start ()
	{
		UIManager.SetButtonEventHandler (inlayBtn.gameObject, EnumButtonEvent.OnClick, OnInlayBtn, 0, 0);
	}


	public ItemData Item
	{
		set
		{
			if(value != null)
			{
				_itemData = value;
			}
		}
		get
		{
			return _itemData;
		}
	}

	public COM_Item ItemInst
	{
		set
		{
			if(value  != null)
			{
				_itemInst = value;
				Item = ItemData.GetData((int)_itemInst.itemId_);
				nameLab.text  = Item.name_;
				ItemCellUI  cell =  UIManager.Instance.AddItemCellUI(itemIcon,(uint)Item.id_);
				cell.ItemCount = _itemInst.stack_;
				cell.showTips = true;
			}
		}
		get
		{
			return _itemInst;
		}
	}
	private void OnInlayBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(callBack!= null)
		{
			callBack(_itemInst);
		}
	}

}

