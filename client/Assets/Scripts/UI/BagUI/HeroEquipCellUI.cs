using UnityEngine;
using System.Collections;

public class HeroEquipCellUI : MonoBehaviour
{
	public GameObject Pane;
	public UITexture Icon;
	public UISprite blackImg;
	public UISprite qImg;
	//
	private COM_Item equipItem;


	void Start ()
	{

	}
	

	public COM_Item Equip
	{
		set
		{
			if(value == null)
			{
				Pane.SetActive(false);
				return;
			}
			equipItem = value;
			ItemData equipData = ItemData.GetData((int)equipItem.itemId_);
			if(equipData == null)
				return;

			Pane.SetActive(true);
			HeadIconLoader.Instance.LoadIcon( equipData.icon_,Icon);
			qImg.spriteName = BagSystem.instance.GetQualityBack((int)equipData.quality_);

		}
		get
		{
			return equipItem;
		}
	}

}

