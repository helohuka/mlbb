using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class UIManager
{
	private static UIManager mManager = null;

	private UIPanel  _RootPanel = null;
	private List<UIBase> _UIList = new List<UIBase>();
	private GameObject mainPanelObj;

	public RequestEventHandler<bool> showMainPanelEnvent;

    private List<string> _NeedLoadingUI = new List<string>();
	private List<string> _NeedzhizuoUI = new List<string>();

	public static UIManager Instance 
	{
		get
        {
            if (mManager == null)
                mManager = new UIManager();
            return mManager;
		}
	}

    Dictionary<UIASSETS_ID, OpenSubSystemFlag> panelDic_;
	
	private UIManager ()
	{
        panelDic_ = new Dictionary<UIASSETS_ID, OpenSubSystemFlag>();
        panelDic_.Add(UIASSETS_ID.UIASSETS__BabySkillLearning, OpenSubSystemFlag.OSSF_BabyLeranSkill);

        AtlasLoader.Instance.OnAtlasLoaded += UIAssetMgr.LoadRefAtlasFin;
	}

    public OpenSubSystemFlag GetPanelOpenFlag(UIASSETS_ID assetid)
    {
        if(panelDic_.ContainsKey(assetid))
            return panelDic_[assetid];
        return OpenSubSystemFlag.OSSF_None;
    }
	
	public static void SetButtonEventHandler (GameObject button, EnumButtonEvent buttonEvent, OnTouchButtonHandler handler, int param1, int param2)
	{
		ButtonScript buttonScript = button.GetComponent<ButtonScript> ();
		if (buttonScript == null) {
			buttonScript = button.gameObject.AddComponent<ButtonScript> ();
		}
		buttonScript.SetButtonScriptHandler (buttonEvent, handler, param1, param2);
	}
	
	public static void RemoveButtonEventHandler (GameObject button, EnumButtonEvent buttonEvent)
	{
		ButtonScript buttonScript = button.GetComponent<ButtonScript> ();
		if (buttonScript == null) {
			buttonScript = button.gameObject.AddComponent<ButtonScript> ();
		}
		buttonScript.RemoveButtonScriptHandler (buttonEvent);
	}
	
	public static void RemoveButtonAllEventHandler (GameObject button)
	{
		ButtonScript buttonScript = button.GetComponent<ButtonScript> ();
		if (buttonScript == null) {
			buttonScript = button.gameObject.AddComponent<ButtonScript> ();
		}
		buttonScript.RemoveButtonScriptAllHandler ();
	}

	public static void SetButtonParam(GameObject button, int param1, int param2)
	{
		ButtonScript buttonScript = button.GetComponent<ButtonScript> ();
		if (buttonScript == null) {
			buttonScript = button.gameObject.AddComponent<ButtonScript> ();
		}
		buttonScript.SetButtonScriptParam (param1, param2);
	}

	public void DoActive(UIPanel rootPanel)
	{
		//_IsActivated = true;
		
		//如果相同，应该是隐藏后显示的.
		if(_RootPanel != rootPanel)
		{
			#if UNITY_EDITOR
			if(_RootPanel != null)
			{
				//UnityEngine.ClientLog.Instance.LogError("Wrong in UIManager.DoActive() .");
			}
			#endif
			_RootPanel = rootPanel;
		}
	}

	public void DoDeActive()
	{
		//_IsActivated = false;
		
		for(int i=0; i < _UIList.Count; i++)
		{
			_UIList[i].Hide ();
            i--;
		}
		_UIList.Clear();
		//TipsItemUI.instance.HideTips ();

		//_RootPanel = null;
	}

    int topDepth_;

	/// <summary>
	/// UIBase调用来把一个ui挂到场景中. 其他地方不要调用.
	/// </summary>
	/// <param name="uiPanel">User interface panel.</param>
	public void ShowUI(UIBase wnd)
	{
		//if(!_IsActivated)
		//{
		//	return;
		//}
		wnd.panel.gameObject.SetActive (true);
		// 先移除，再添加，可以把wnd放到最顶层...
		if(_UIList.Contains(wnd))
		{
			wnd.panel.transform.parent = null;
			_UIList.Remove(wnd);
		}

		UIPanel rootPane = ApplicationEntry.Instance.uiRoot.GetComponent<UIPanel>();

		wnd.panel.transform.parent = rootPane.transform;
		wnd.panel.transform.localScale = Vector3.one;
		//后开的层在最上...
		if(wnd.UIName != "Cinema")
		{
			AdjustUIDepth(wnd.transform);
			if (showMainPanelEnvent != null)
				showMainPanelEnvent (false);
		}
		_UIList.Add(wnd);

	}

    public bool isOpen(UIASSETS_ID uiid)
    {
        string AssetsName = GlobalInstanceFunction.Instance.GetAssetsName( (int)uiid , AssetLoader.EAssetType.ASSET_UI );
        for (int i = 0; i < _UIList.Count; ++i)
        {
            if (_UIList[i].UIName.Equals(AssetsName))
                return true;
        }
        return false;
    }

    List<UIPanel> panels_ = new List<UIPanel>();
    public void AdjustUIDepth(Transform subui, bool posPlus = true, float customPos = 0f, int defaultStartDepth = 1)
    {
        panels_.Clear();
        int listCountReduce = 0;
        if (_UIList.Count > 0)
        {
            int maxDep = 0;
            CollectChildPanel(_UIList[_UIList.Count - 1].transform);
            for (int i = 0; i < panels_.Count; ++i)
            {
                if (panels_[i].depth == GuideCreator.GuideDepth + 1)
                {
                    listCountReduce++;
                    continue;
                }
                if (maxDep < panels_[i].depth)
                    maxDep = panels_[i].depth;
            }
            topDepth_ = maxDep + 2;
        }
        else
			topDepth_ = defaultStartDepth;

        panels_.Clear();

        if (posPlus)
            subui.transform.localPosition = new Vector3(0f, 0f, (_UIList.Count - listCountReduce) * -500f + customPos);
        CollectChildPanel(subui);
        for (int i = 0; i < panels_.Count; ++i)
        {
            panels_[i].depth = topDepth_;
            panels_[i].sortingOrder = topDepth_;
            topDepth_ += 2;
        }
    }

    void CollectChildPanel(Transform trans)
    {
        UIPanel panel = trans.GetComponent<UIPanel>();
        if (panel != null)
            panels_.Add(panel);
        for (int i = 0; i < trans.childCount; ++i)
        {
            CollectChildPanel(trans.GetChild(i));
        }
    }

	public void HideUI(UIBase wnd)
	{

		wnd.transform.parent = null;
		// 从自己列表中移除.
		_UIList.Remove(wnd);

        if (_UIList.Count == 0)
        {
            if (showMainPanelEnvent != null)
                showMainPanelEnvent(true);
        }
	}

    public void InitIconCell()
    {
        if (goRes_ == null)
        {
            UIAssetMgr.LoadUI("ItemCell", (AssetBundle bundle, ParamData data) => {
                goRes_ = bundle.mainAsset;
            }, null);
        }

        if (goStateRes_ == null)
        {
            UIAssetMgr.LoadUI("StateCell", (AssetBundle bundle, ParamData data) =>
            {
                goStateRes_ = bundle.mainAsset;
            }, null);
        }

        if (goSkillRes_ == null)
        {
            UIAssetMgr.LoadUI("SkillCell", (AssetBundle bundle, ParamData data) =>
            {
                goSkillRes_ = bundle.mainAsset;
            }, null);
        }

        if (goBabyRes_ == null)
        {
            UIAssetMgr.LoadUI("BabyCell", (AssetBundle bundle, ParamData data) =>
            {
                goBabyRes_ = bundle.mainAsset;
            }, null);
        }
    }

	public ItemCellUI AddItemCellUI(UISprite parent, uint id = 0, float offsetX = 0f, float offsetY = 0f)
	{
		if (ItemData.GetData ((int)id) == null)
			return null;
        BabyCellUI[] cellbs = parent.gameObject.GetComponentsInChildren<BabyCellUI>(true);
        for (int i = 0; i < cellbs.Length; ++i)
        {
            GameObject.Destroy(cellbs[i].gameObject);
        }
        ItemCellUI[] cells = parent.gameObject.GetComponentsInChildren<ItemCellUI>(true);
        ItemCellUI cell = null;
        if (cells == null || cells.Length == 0)
        {
            cell = InstantiateBagCellUIObj();
            if (id != 0)
            {
				if(parent == null)
				{
					GameObject.Destroy(cell.gameObject);
					return null;
				}
                cell.cellPane.transform.parent = parent.gameObject.transform;
				cell.cellPane.width = parent.width;
				cell.cellPane.height = parent.height;
                cell.cellPane.depth = parent.depth + 1;
                cell.cellPane.transform.localPosition = new Vector2(offsetX, offsetY);
                cell.cellPane.transform.localScale = Vector3.one;
            }
        }
        else
            cell = cells[0];
        cell.itemId = id;
		cell.ItemInst = null;
		return cell;
	}




	public ItemCellUI AddItemInstCellUI(UISprite parent, object value, float offsetX = 0f, float offsetY = 0f)
	{
		if (value == null)
			return null;
		BabyCellUI[] cellbs = parent.gameObject.GetComponentsInChildren<BabyCellUI>(true);
		for (int i = 0; i < cellbs.Length; ++i)
		{
			GameObject.Destroy(cellbs[i].gameObject);
		}
		ItemCellUI[] cells = parent.gameObject.GetComponentsInChildren<ItemCellUI>(true);
		ItemCellUI cell = null;
		if (cells == null || cells.Length == 0)
		{
			cell = InstantiateBagCellUIObj();
			if (value != null)
			{
				if(parent == null)
				{
					GameObject.Destroy(cell.gameObject);
					return null;
				}
				cell.cellPane.transform.parent = parent.gameObject.transform;
				cell.cellPane.width = parent.width;
				cell.cellPane.height = parent.height;
				cell.cellPane.depth = parent.depth + 1;
				cell.cellPane.transform.localPosition = new Vector2(offsetX, offsetY);
				cell.cellPane.transform.localScale = Vector3.one;
			}
		}
		else
			cell = cells[0];
		if(value is ItemData)
		{
			//cell.ItemInst = (COM_Item)value;
		}
		else if(value is COM_Item)
		{
			cell.ItemInst = (COM_Item)value;
		}
		return cell;
	}




    Object goRes_;
	public ItemCellUI InstantiateBagCellUIObj()
	{
        var go = (GameObject)Object.Instantiate(goRes_);
		return go.GetComponent<ItemCellUI> ();
	}

    public BabyCellUI AddBabyCellUI(UISprite parent, object value, float offsetX = 0f, float offsetY = 0f)
    {
        ItemCellUI[] cellis = parent.gameObject.GetComponentsInChildren<ItemCellUI>(true);
        for (int i = 0; i < cellis.Length; ++i)
        {
            GameObject.Destroy(cellis[i].gameObject);
        }
        BabyCellUI[] cells = parent.gameObject.GetComponentsInChildren<BabyCellUI>(true);
        BabyCellUI cell = null;
        if (cells == null || cells.Length == 0)
        {
            cell = InstantiateBabyCellUIObj();
            if (value != null)
            {
                cell.cellPane.transform.parent = parent.gameObject.transform;
                cell.cellPane.depth = parent.depth + 1;
                cell.cellPane.transform.localPosition = new Vector2(offsetX, offsetY);
                cell.cellPane.transform.localScale = Vector3.one;
            }
        }
        else
            cell = cells[0];
        if (value is BabyData)
            cell.data = (BabyData)value;
        else
            cell.inst = (Baby)value;
        return cell;
    }

    Object goBabyRes_ = null;
    public BabyCellUI InstantiateBabyCellUIObj()
    {
        var go = (GameObject)Object.Instantiate(goBabyRes_);
        return go.GetComponent<BabyCellUI>();
    }

    public StateCellUI AddStateCellUI(UISprite parent, COM_State stateInst = null, float offsetX = 0f, float offsetY = 0f)
    {
        StateCellUI[] cells = parent.gameObject.GetComponentsInChildren<StateCellUI>(true);
        StateCellUI cell = null;
        if (cells == null || cells.Length == 0)
        {
            cell = InstantiateStateCellUIObj();
            if (stateInst != null)
            {
                cell.cellPane.transform.parent = parent.gameObject.transform;
                cell.cellPane.depth = parent.depth + 1;
                cell.cellPane.transform.localPosition = new Vector2(offsetX, offsetY);
                cell.cellPane.transform.localScale = Vector3.one;
            }
        }
        else
            cell = cells[0];
        cell.stateInst = stateInst;
        return cell;
    }

    Object goStateRes_ = null;
    public StateCellUI InstantiateStateCellUIObj()
    {
        var go = (GameObject)Object.Instantiate(goStateRes_);
        return go.GetComponent<StateCellUI>();
    }

    public SkillCellUI AddSkillCellUI(UISprite parent, SkillData data, float offsetX = 0f, float offsetY = 0f, float scale = 1f)
    {
        SkillCellUI[] cells = parent.gameObject.GetComponentsInChildren<SkillCellUI>(true);
        SkillCellUI cell = null;
        if (cells == null || cells.Length == 0)
        {
            cell = InstantiateSkillCellUIObj();
            if (data._Id != 0)
            {
                cell.cellPane.transform.parent = parent.gameObject.transform;
                cell.cellPane.depth = parent.depth + 1;
                cell.cellPane.transform.localPosition = new Vector2(offsetX, offsetY);
                cell.cellPane.transform.localScale = Vector3.one;
            }
        }
        else
            cell = cells[0];
        cell.data = data;
        cell.scale = scale;
        return cell;
    }

    Object goSkillRes_;
    public SkillCellUI InstantiateSkillCellUIObj()
    {
        var go = (GameObject)Object.Instantiate(goSkillRes_);
        return go.GetComponent<SkillCellUI>();
    }

    public void RegNeedLoading(string uiName)
    {
        if (_NeedLoadingUI.Contains(uiName))
            return;
        _NeedLoadingUI.Add(uiName);
    }

	public void Regzhizuo(string uiName)
	{
		if (_NeedzhizuoUI.Contains(uiName))
			return;
		_NeedzhizuoUI.Add(uiName);
	}

	public bool ContainsUI4zhizuo(string uiName)
	{
		return _NeedzhizuoUI.Contains(uiName);
	}

    public bool ContainsUI4Loading(string uiName)
    {
        return _NeedLoadingUI.Contains(uiName);
    }

	public void LoadMoneyUI(GameObject obj)
	{
		string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName( (int)UIASSETS_ID.UIASSETS_TopMoneyPanel , AssetLoader.EAssetType.ASSET_UI );
		
		AssetLoader.LoadAssetBundle (uiResPath, AssetLoader.EAssetType.ASSET_UI,(Assets,paramData)=> {
            
			if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}
			if(obj == null)
				return;
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(obj == null)
				return;
			go.transform.parent = obj.transform;    
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			UIManager.Instance.AdjustUIDepth(go.transform, false);
            AssetInfoMgr.Instance.DecRefCount(Assets, false);
		}
		, null);
	}


}

