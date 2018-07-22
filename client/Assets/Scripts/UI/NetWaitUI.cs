using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetWaitUI : UIBase
{
    public static void ShowMe()
    {
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_NetWaitPanel);
    }

    public static void HideMe()
    {
        UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_NetWaitPanel);
    }

    public override void Destroyobj()
    {

    }
}

