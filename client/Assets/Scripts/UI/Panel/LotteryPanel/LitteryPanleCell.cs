using UnityEngine;
using System.Collections;

public class LitteryPanleCell : MonoBehaviour {
	
	public UILabel name;
	public UISprite back;
	private COM_ZhuanpanContent zpdata;
	public COM_ZhuanpanContent Data
	{
		set
		{
			if(value != null)
			{
				zpdata = value;
				ItemCellUI itemc =  UIManager.Instance.AddItemCellUI(back,zpdata.item_);
				itemc.showTips=true;
				itemc.ItemCount = (int)zpdata.itemNum_;
				name.text = ItemData.GetData((int)zpdata.item_).name_;

				//num.gameObject.SetActive(false);
//				if(zpdata._Number >1)
//				{
//					num.text = zpdata._Number.ToString();
//				}else
//				{
//					num.text ="";
//				}
//				back.spriteName =BagSystem.instance.GetQualityBack((int)ItemData.GetData(zpdata._ItemID).quality_);
//				HeadIconLoader.Instance.LoadIcon (ItemData.GetData(zpdata._ItemID).icon_, icon);
			}
		}
		get
		{
			return zpdata;
		}
	}

    void OnDestroy()
    {
        ItemData data = null;
//        if(zpdata != null)
//			data = ItemData.GetData(zpdata._ItemID);
        if (data != null)
            HeadIconLoader.Instance.Delete(data.icon_);
    }

	void Start () {
	
	}

}
