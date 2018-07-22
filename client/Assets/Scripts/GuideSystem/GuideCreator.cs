using UnityEngine;
using System.Collections.Generic;

public class GuideCreator {

    public const int GuideDepth = 3000;

    Camera guideCamera_;

    GameObject crtGuideObj_;

    int sourceLayer_ = -1;

    GameObject guideRoot_;

    Object pointerPrefab_;

    bool needRemovePanel_;

    int panelDepth_, panelSortOrder_;

    GameObject uiClone_;

    public delegate void ClearGuideEvent();
    public event ClearGuideEvent OnClearGuide;

	int iStep;

    public void Init()
    {
        GlobalInstanceFunction.Instance.MakeMask();
    }

    public void InitArrow()
    {
        UIAssetMgr.LoadUI("jiantou", (AssetBundle bundle, ParamData data) =>
        {
            pointerPrefab_ = bundle.mainAsset;
        }, null);
    }

	public void CreateInScene(GameObject guideObj, float offsetX, float offsetY, GuidePointerRotateType rotateType, string str,int step, float alpha = 0.7f,bool mask = false)
    {
        if (guideObj == null)
            return;

        if (crtGuideObj_ != null && crtGuideObj_.Equals(guideObj))
            return;

        ClearGuide();
		if(ChatUI.Instance.BigChatOpen())
			ChatUI.Instance.SwitchChatObjActive();
        guideRoot_ = new GameObject("GuideMask");
		if (mask)
		{
			ShowBlack(alpha,step);
			/*UIPanel panel = guideRoot_.AddComponent<UIPanel>();
			panel.depth = GuideDepth;
			panel.sortingOrder = GuideDepth;
			BoxCollider bc = guideRoot_.AddComponent<BoxCollider>();
			Vector3 size = new Vector3(ApplicationEntry.Instance.UIWidth, ApplicationEntry.Instance.UIHeight, 0);
			bc.size = size;
			GameObject maskSub = new GameObject("sub");
			UITexture texture = maskSub.AddComponent<UITexture>();
			texture.mainTexture = GlobalInstanceFunction.Instance.maskTex_;
			texture.color = new Color(0f, 0f, 0f, alpha);
			texture.MakePixelPerfect();
			texture.gameObject.AddComponent<BoxCollider>();
			UIManager.SetButtonEventHandler(texture.gameObject, EnumButtonEvent.OnClick,OnClickBalck , 0, 0);


			//
			maskSub.transform.parent = guideRoot_.transform;
			*/
		}
        guideRoot_.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        guideRoot_.transform.localScale = Vector3.one;
        NGUITools.SetLayer(guideRoot_, LayerMask.NameToLayer("UI"));
        NGUITools.SetChildLayer(guideRoot_.transform, LayerMask.NameToLayer("UI"));

        guideCamera_ = ((GameObject)GameObject.Instantiate(Camera.main.gameObject) as GameObject).GetComponent<Camera>();
        guideCamera_.cullingMask = 1 << LayerMask.NameToLayer("Guide");
        guideCamera_.depth = 1000;
        guideCamera_.clearFlags = CameraClearFlags.Nothing;
        guideCamera_.tag = "GuideCam";
        GameObject.Destroy(guideCamera_.GetComponent<AudioListener>());
        guideCamera_.transform.parent = guideRoot_.transform;
        guideCamera_.transform.position = Camera.main.gameObject.transform.position;
        guideCamera_.transform.rotation = Camera.main.gameObject.transform.rotation;

        crtGuideObj_ = guideObj;
        sourceLayer_ = crtGuideObj_.layer;
        NGUITools.SetLayer(crtGuideObj_, LayerMask.NameToLayer("Guide"));
        NGUITools.SetChildLayer(crtGuideObj_.transform, LayerMask.NameToLayer("Guide"));
        crtGuideObj_.SetActive(false);
        crtGuideObj_.SetActive(true);

        GameObject pointer = (GameObject)GameObject.Instantiate(pointerPrefab_);
        pointer.transform.parent = guideRoot_.transform;
        pointer.transform.localScale = Vector3.one;
        pointer.GetComponent<GuidePointer>().BeginInScene(crtGuideObj_, offsetX, offsetY, rotateType, str);

        //处理npc的名字高亮
        if (crtGuideObj_.CompareTag("NPC"))
        {
            TaskNpc tn = crtGuideObj_.GetComponent<TaskNpc>();
            if (tn != null)
            {
                uiClone_ = (GameObject)GameObject.Instantiate(tn.Name.gameObject);
                GameObject uiCloneParent = new GameObject("GuideNpcName");
                uiClone_.transform.parent = uiCloneParent.transform;
                uiCloneParent.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
                uiCloneParent.transform.localScale = Vector3.one;
                NGUITools.SetLayer(uiCloneParent, LayerMask.NameToLayer("UI"));
                NGUITools.SetChildLayer(uiCloneParent.transform, LayerMask.NameToLayer("UI"));
                UIPanel guidePanel = uiCloneParent.AddComponent<UIPanel>();
                panelDepth_ = guidePanel.depth;
                panelSortOrder_ = guidePanel.sortingOrder;
                guidePanel.depth = GuideDepth + 1;
                guidePanel.sortingOrder = GuideDepth + 1;
                uiCloneParent.SetActive(false);
                uiCloneParent.SetActive(true);
            }
        }
    }

    public void Update()
    {
        if (crtGuideObj_ == null || !IsVisable(crtGuideObj_))
        {
            GuideManager.Instance.ClearMask();
            return;
        }

        TaskNpc tn = crtGuideObj_.GetComponent<TaskNpc>();
        if (tn != null)
            uiClone_.transform.position = tn.Name.transform.position;
    }

    bool IsVisable(GameObject go)
    {
        if (go.activeSelf == true)
        {
            if(go.transform.parent != null)
                return IsVisable(go.transform.parent.gameObject);
            return true;
        }

        return false;
    }

	public void Create(GameObject guideObj, float offsetX, float offsetY, GuidePointerRotateType rotateType,string str,int step, float alpha = 0.7f,bool mask = false)
    {
        if (guideObj == null)
            return;

        if (crtGuideObj_ != null && crtGuideObj_.Equals(guideObj))
            return;

        ClearGuide();
		if(ChatUI.Instance.BigChatOpen())
			ChatUI.Instance.SwitchChatObjActive();
        guideRoot_ = new GameObject("GuideMask");
		if(mask)
		{
			ShowBlack(alpha,step);
		}
		guideRoot_.transform.parent = ApplicationEntry.Instance.uiRoot.transform;
        guideRoot_.transform.localScale = Vector3.one;
        NGUITools.SetLayer(guideRoot_, LayerMask.NameToLayer("UI"));
        NGUITools.SetChildLayer(guideRoot_.transform, LayerMask.NameToLayer("UI"));

        crtGuideObj_ = guideObj;
        UIPanel guide = guideObj.GetComponent<UIPanel>();
        if (guide == null)
        {
            guide = guideObj.AddComponent<UIPanel>();
            needRemovePanel_ = true;
        }
        panelDepth_ = guide.depth;
        panelSortOrder_ = guide.sortingOrder;
        guide.depth = GuideDepth + 1;
        guide.sortingOrder = GuideDepth + 1;
        guideObj.SetActive(false);
        guideObj.SetActive(true);

        GameObject pointer = (GameObject)GameObject.Instantiate(pointerPrefab_);
        pointer.GetComponent<GuidePointer>().Begin(crtGuideObj_, offsetX, offsetY, rotateType,str);
        pointer.transform.parent = guideRoot_.transform;
        pointer.transform.localScale = Vector3.one;
    }

    public void ClearGuide()
    {
        if (guideRoot_ != null)
        {
            GameObject.Destroy(guideRoot_);
            guideRoot_ = null;
        }
		CinemaUI.HideMe ();
        if (crtGuideObj_ != null)
        {
            UIPanel panel = crtGuideObj_.GetComponent<UIPanel>();
            if (panel != null && panel.sortingOrder == GuideDepth + 1 && needRemovePanel_)
                GameObject.Destroy(panel);
            else if (panel != null && panel.sortingOrder == GuideDepth + 1 && !needRemovePanel_)
            {
                panel.depth = panelDepth_;
                panel.sortingOrder = panelSortOrder_;
                panelDepth_ = 0;
                panelSortOrder_ = 0;
            }
            if (sourceLayer_ != -1)
            {
                NGUITools.SetLayer(crtGuideObj_, sourceLayer_);
                NGUITools.SetChildLayer(crtGuideObj_.transform, sourceLayer_);
                sourceLayer_ = -1;
            }
            //crtGuideObj_.SetActive(false);
            //crtGuideObj_.SetActive(true);
            crtGuideObj_ = null;
        }

        GameObject npcName = (GameObject)GameObject.Find("GuideNpcName");
        if(npcName != null)
            GameObject.Destroy(npcName);

        if (OnClearGuide != null)
            OnClearGuide();
    }

    public GameObject crtObj
    {
        get
        {
            return crtGuideObj_;
        }
    }


	private void ShowBlack(float alpha,int step)
	{
		iStep = step;
		UIPanel panel = guideRoot_.AddComponent<UIPanel>();
		panel.depth = GuideDepth;
		panel.sortingOrder = GuideDepth;
		BoxCollider bc = guideRoot_.AddComponent<BoxCollider>();
		Vector3 size = new Vector3(ApplicationEntry.Instance.UIWidth, ApplicationEntry.Instance.UIHeight, 0);
		bc.size = size;
		GameObject maskSub = new GameObject("sub");
		UITexture texture = maskSub.AddComponent<UITexture>();
		texture.mainTexture = GlobalInstanceFunction.Instance.maskTex_;
		texture.color = new Color(0f, 0f, 0f, alpha);
		//texture.MakePixelPerfect();
        texture.width = (int)size.x;
        texture.height = (int)size.y;
		texture.gameObject.AddComponent<BoxCollider>();
		UIManager.SetButtonEventHandler(guideRoot_.gameObject, EnumButtonEvent.OnClick,OnClickBalck , 0, 0);
		//
		maskSub.transform.parent = guideRoot_.transform;
	}


	private void OnClickBalck(ButtonScript obj, object args, int param1, int param2)
	{
		CinemaUI.ShowMe ((GameObject go)=>{
			GuideManager.Instance.SetFinish(iStep);
			GuideManager.Instance.ClearMask();
		});
	}
}
