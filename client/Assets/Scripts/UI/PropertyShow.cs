using UnityEngine;
using System;
using System.Collections;

public class PropertyShow : MonoBehaviour {

    public PropertyType propType;
	
    void OnPress(bool press)
    {
        string desc = "";
        string typestr = string.Format("C_{0}", propType);
        Constant type = (Constant)Enum.Parse(typeof(Constant), typestr);
        GlobalValue.Get(type, out desc);
        StringTool.UTF8String(ref desc);
        if (press)
            TipsPropertyUI.ShowMe(desc, transform.position);
        else
            TipsPropertyUI.HideMe();
    }

    void Update()
    {
        if (Input.touchCount > 1)
        {
            TipsPropertyUI.HideMe();
        }
    }
}
