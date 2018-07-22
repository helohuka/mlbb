using System;
using UnityEngine;
using System.Collections;

public class RecordItem : MonoBehaviour {

    public UILabel date_;

    public UILabel name_;

    public UILabel gainDiamond_;

    public UILabel tax_;

	// Use this for initialization
	void Start () {
	
	}

    public void SetData(COM_SelledItem item)
    {
        string fmt = "MM月dd日HH:mm";
        Define.FormatUnixTimestamp( ref fmt, item.selledTime_);
        date_.text = fmt;
        string name = "";
        if (item.itemId_ != 0)
        {
            name = ItemData.GetData(item.itemId_).name_;
            name_.text = name + string.Format("({0})", item.itemStack_);
        }
        else
        {
            name = BabyData.GetData(item.babyId_)._Name;
            name_.text = name;
        }
        gainDiamond_.text = item.price_.ToString();
        tax_.text = item.tax_.ToString();
    }

}
