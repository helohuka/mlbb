using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatherItemCell :  MonoBehaviour{

    public UITexture _Icon;
    public UILabel _Desc;
    public UILabel _Stack;


    public COM_DropItem Data
    {
        set
        {
            ItemData d = ItemData.GetData((int)value.itemId_);
            if (d == null)
                return;
            HeadIconLoader.Instance.LoadIcon(d.icon_, _Icon);
            _Desc.text = d.name_;
            _Stack.text = value.itemNum_.ToString();
        }
    }
}