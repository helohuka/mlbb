using UnityEngine;
using System.Collections;

public class SellGood : MonoBehaviour {

    public UILabel name_;

    public UILabel price_;

    public UISprite icon_;

    COM_SellItem sellItem_;

	// Use this for initialization
	void Start () {
	
	}

    public void SetData(COM_SellItem sellItem)
    {
        sellItem_ = sellItem;
        ItemData idata = null;
        BabyData bData = null;
        if(sellItem.item_.itemId_ != 0)
        {
            idata = ItemData.GetData((int)sellItem.item_.itemId_);
            name_.text = idata.name_;
            ItemCellUI icell = UIManager.Instance.AddItemInstCellUI(icon_, sellItem_.item_);
			if(icell == null)
				return;
            icell.ItemCount = (int)sellItem.item_.stack_;
            icell.showTips = true;
        }
        else
        {
            bData = BabyData.GetData((int)sellItem.baby_.properties_[(int)PropertyType.PT_TableId]);
            name_.text = bData._Name;
            Baby baby = new Baby();
            baby.SetBaby(sellItem.baby_);
            BabyCellUI cell = UIManager.Instance.AddBabyCellUI(icon_, baby);
            UIManager.SetButtonEventHandler(cell.gameObject, EnumButtonEvent.OnClick, OnClickIcon, 0, 0);
        }
        price_.text = sellItem_.sellPrice.ToString();
    }

    void OnClickIcon(ButtonScript obj, object args, int param1, int param2)
    {
        ChatBabytips.ShowMe(sellItem_.baby_);
    }

}
