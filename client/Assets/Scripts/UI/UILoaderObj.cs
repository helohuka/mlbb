using UnityEngine;
using System.Collections;

public class UILoaderObj : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIManager.Instance.DoDeActive();
        UIFactory.Instance.OpenUI(StageMgr.Scene_name, menuTypes.MAIN);
        //UIManager.Instance.DoDeActive();
        //UIFactory.Instance.LoadUIPanel(GetID(StageMgr.Scene_name), () =>
        //{
            //UIManager.Instance.ShowMainUI();
        //});
        //StageMgr.Scene_name = "";
	}

    //UIASSETS_ID GetID(string sceneName)
    //{
    //    if(GlobalValue.isBattleScene(sceneName))
    //        return UIASSETS_ID.UIASSETS_AttackPanel;

    //    if (GlobalValue.isFBScene(sceneName))
    //        return UIASSETS_ID.UIASSETS_MainPanel;

    //    switch (sceneName)
    //    {
    //        case GlobalValue.StageName_CreateRoleScene:
    //            return UIASSETS_ID.UIASSETS_PanelXuan;
    //        case GlobalValue.StageName_groupScene:
    //            return UIASSETS_ID.UIASSETS_TeamPanel;
    //        case GlobalValue.StageName_ReLoginScene:
    //            return UIASSETS_ID.UIASSETS_LoginPanel;
    //        default:
    //            return UIASSETS_ID.UIASSETS_MainPanel;
    //    }
    //}

}
