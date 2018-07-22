using UnityEngine;
using System.Collections;

public class EquipitemCell : MonoBehaviour {

	public UISprite iconbBack;
	public UISprite numberSprite;
	public UILabel nameLabel;
	public UILabel numberLabel;
	private ShopData _shopData;
	public ShopData EquipshopData
	{
		set
		{
			if(value!=null)
			{
				_shopData = value;
				nameLabel.text = _shopData._Name;
				numberLabel.text = _shopData._Price.ToString();
				UIManager.Instance.AddItemCellUI(iconbBack,(uint)_shopData._Itemid).showTips = true;
				if(_shopData._ShopPayType == ShopPayType.SPT_Diamond)
				{

				}else
					if(_shopData._ShopPayType == ShopPayType.SPT_Gold)
				{
					
				}else
					if(_shopData._ShopPayType == ShopPayType.SPT_MagicCurrency)
				{
					
				}else 
					if(_shopData._ShopPayType == ShopPayType.SPT_RMB)
				{
					
				}
			}
		}
		get
		{
			return _shopData;
		}
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
