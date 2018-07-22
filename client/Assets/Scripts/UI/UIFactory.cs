using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum menuTypes
{
	MAIN,
	Message,
	Popup
}
public class UIFactory:MonoBehaviour
{
	private GameObject uiPre;
    private AssetBundle uiBundlePre;
	private GameObject currentPanel;
	private Vector3 playerUIPosition;
	private static UIFactory instance;
	private menuTypes mType;
	private GameObject mainPanelObj;
	private GameObject popPanelObj;
	public delegate void SceneUICallBack();
	public  SceneUICallBack UICallBack;
	Queue <UIASSETS_ID>priousUI = new Queue<UIASSETS_ID> ();

	public AnimationCurve openPanelAnimCurve;   // 打开面板的动画曲线.
	public AnimationCurve closePanelAnimCurve;  // 关闭面板的动画曲线..


	//Stack<UIASSETS_ID> priousUI;
	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		DontDestroyOnLoad(this);
	}

	public static UIFactory Instance
	{
		get
		{
			return instance;	
		}
	}
	public void OpenUI (UIASSETS_ID id, menuTypes type)
	{
		if(uiPre == null)
			return;

	    mType = type;
		if(type == menuTypes.MAIN)
		{
			priousUI.Enqueue(id);
		}			
		ShowUIPanel (uiPre);
	}

	public void OpenUI (string sceneName,menuTypes type)
	{
		if(sceneName.Equals(""))
			return;
		UIASSETS_ID id = (UIASSETS_ID)0;
		switch(sceneName)
		{
            case GlobalValue.StageName_LoginScene:
            case GlobalValue.StageName_ReLoginScene:
                id = UIASSETS_ID.UIASSETS_LoginPanel;
                break;
            case GlobalValue.StageName_CreateRoleScene:
                id = UIASSETS_ID.UIASSETS_PanelXuan;
                break;
            //case GlobalValue.StageName_AttackScene:
            //case GlobalValue.StageName_AttackScene_Maze:
                
            //    break;
            case GlobalValue.StageName_groupScene:
                id = UIASSETS_ID.UIASSETS_TeamPanel;
                break;
            default:
                if(GlobalValue.isBattleScene(sceneName))
                    id = UIASSETS_ID.UIASSETS_AttackPanel;
                else
                    id = UIASSETS_ID.UIASSETS_MainPanel;
                break;
		}
        OpenUI(id, type);
	}

	private void ShowUIPanel(GameObject panelObj)
	{
		GameObject tempPanel = InitUIPanel (panelObj,ApplicationEntry.Instance.uiRoot);
//		UIPanel [] panels = gameObject.GetComponentsInChildren<UIPanel> ();
//		int icount = 0;
//		for (int i = 0; i<panels.Length; i++) 
//		{
//			panels[i].depth = i;
//			icount = panels[i].depth;
//		}
//		UIPanel objPanel = tempPanel.GetComponent<UIPanel> ();
//		if(objPanel!=null)
//		{
//			objPanel.depth = icount + 1;
//		}

        
		if(mType == menuTypes.MAIN)
		{
			
			mainPanelObj = tempPanel;
		}else if(mType == menuTypes.Popup)
		{
			//popPanelObj = tempPanel;
		}

	}
	private GameObject InitUIPanel(GameObject nameObj,GameObject sRoot)
	{
		GameObject obj = GameObject.Instantiate(nameObj)as GameObject;
        //if (uiBundlePre != null)
        //{
        //    uiBundlePre.Unload(false);
        //    uiBundlePre = null;
        //}
		obj.name = nameObj.name;
		if (obj != null) {
			obj.transform.parent = sRoot.transform;
			obj.transform.position = Vector3.zero;
			obj.transform.localScale =new Vector3(1,1,1);
			obj.transform.rotation = new Quaternion(0,0,0,0);
            if (obj.name.Equals("AttackPanel"))
            {
                AttaclPanel panel = obj.GetComponent<AttaclPanel>();
                if(panel != null)
                    panel.HideUI();
            }
		}
		return obj;
	}

	public void LoadUIPanel (UIASSETS_ID assetID, SceneUICallBack callback = null)
	{
		UICallBack = callback;
        UIAssetMgr.LoadUI(assetID, LoadUICallBack, null);
	}

	public void LoadUIPanel (string	assetsName, SceneUICallBack callback = null)
	{
		UICallBack = callback;
        UIAssetMgr.LoadUI(assetsName, LoadUICallBack, null);
	}

	public void LoadUICallBack( AssetBundle	Assets , ParamData	paramData )
	{
		if( null == Assets || null == Assets.mainAsset )
		{
			return ;
		}

        if (uiPre != null)
            DestroyImmediate(uiPre, true);
        //uiBundlePre = Assets;
		uiPre = Assets.mainAsset as GameObject;
		if (UICallBack != null) 
		{
		  UICallBack ();
		}

	}
	public void CloseUI (UIASSETS_ID assetID, bool unLoadAllLoadedObjects)
	{
		UIBase ba = null;
        if (mainPanelObj != null && mType == menuTypes.MAIN)
        {
			ba = mainPanelObj.GetComponent<UIBase> ();
		}
        else
		{
		    //ba = popPanelObj.GetComponent<UIBase> ();
		}

        if (ba != null)
        {
            ba.Destroyobj();
            //GlobalInstanceFunction.Instance.ReleaseTexture(ba.transform);
        }
        else
        {
            if (mainPanelObj != null && mType == menuTypes.MAIN)
            {
                //GlobalInstanceFunction.Instance.ReleaseTexture(mainPanelObj.transform);
                Destroy(mainPanelObj);
            }
            else
            {
                //ReleaseTexture(popPanelObj.transform);
                //Destroy(popPanelObj);
            }
        }
		UIAssetMgr.DeleteAsset (assetID, unLoadAllLoadedObjects);
	}

    public void CloseCurrentUI()
    {
		if(priousUI.Count == 0)
			return;

        CloseUI(priousUI.Dequeue(), true);
    }

	public void ClearLoadedUI()
	{
		if(uiPre != null)
			uiPre = null;
	}

	string gmorder = "";
	static public bool joined = false;

}
