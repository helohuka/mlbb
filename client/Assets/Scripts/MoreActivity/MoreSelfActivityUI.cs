using UnityEngine;
using System.Collections.Generic;

public class MoreSelfActivityUI : UIBase {
	
	public UIGrid grid_;
	public GameObject itemPrefab_;
	public GameObject closeBtn_;
	public Transform rightContent_;
	
	List<GameObject> itemPool_;
	
	ADType crtType_;
	
	void Awake()
	{
		itemPool_ = new List<GameObject>();
	}
	
	ADType[] types = new ADType[] {ADType.ADT_SelfChargeTotal,ADType.ADT_SelfChargeEvery,ADType.ADT_SelfDiscountStore };
	
	// Use this for initialization
	void Start () {
		GameObject item = null;
		UIEventListener listener = null;
		for (int i = 0; i < types.Length; ++i)
		{
			if (i >= itemPool_.Count)
			{
				item = (GameObject)GameObject.Instantiate(itemPrefab_) as GameObject;
				item.GetComponent<MoreActivityItem>().SetData(LanguageManager.instance.GetValue(types[i].ToString()));
				item.transform.parent = grid_.transform;
				item.transform.localScale = Vector3.one;
				listener = UIEventListener.Get(item);
				listener.onClick += OnClickItem;
				itemPool_.Add(item);
			}
			else
			{
				item = itemPool_[i];
				listener = UIEventListener.Get(item);
			}
			listener.parameter = types[i];
			item.SetActive(true);
		}
		grid_.Reposition();
		
		for (int i = types.Length; i < itemPool_.Count; ++i)
		{
			itemPool_[i].SetActive(false);
		}
		
		UIEventListener.Get(closeBtn_).onClick += delegate { HideMe(); };
		
		if (types.Length > 0)
		{
			crtType_ = types[0];
			ResetBtnAndSubPanel();
			UpdateRight();
			itemPool_[0].GetComponent<MoreActivityItem>().SetSelected(true);
		}
	}
	
	bool loadingSub_ = false;
	void OnClickItem(GameObject go)
	{
		ADType type = (ADType)UIEventListener.Get(go).parameter;
		if (crtType_ == type)
			return;
		
		if (loadingSub_)
			return;
		
		crtType_ = type;
		ResetBtnAndSubPanel();
		UpdateRight();
		go.GetComponent<MoreActivityItem>().SetSelected(true);
	}
	
	void UpdateRight()
	{
		UIASSETS_ID uiId = GlobalValue.GetMoreActivityID(crtType_);
		if (uiId != (UIASSETS_ID)0)
		{
			loadingSub_ = true;
			UIAssetMgr.LoadUI(uiId, (AssetBundle Asset, ParamData Param) =>
			                  {
				if (rightContent_ == null)
				{
					UIAssetMgr.DeleteAsset(uiId, true);
					return;
				}
				
				GameObject subUi = (GameObject)GameObject.Instantiate(Asset.mainAsset) as GameObject;
				Asset.Unload(false);
				subUi.transform.parent = rightContent_;
				subUi.transform.localScale = Vector3.one;
				subUi.transform.localPosition = Vector3.zero;
				UIManager.Instance.AdjustUIDepth(subUi.transform, false);
				loadingSub_ = false;
			}, null);
		}
	}
	
	void ResetBtnAndSubPanel()
	{
		for (int i = 0; i < itemPool_.Count; ++i)
		{
			itemPool_[i].GetComponent<MoreActivityItem>().SetSelected(false);
		}
		for (int i = 0; i < rightContent_.childCount; ++i)
		{
			Destroy(rightContent_.GetChild(i).gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_MoreSelfActivity);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_MoreSelfActivity);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_MoreSelfActivity);
	}
	
	public override void Destroyobj()
	{
		GameObject.Destroy(gameObject);
	}
	
	#endregion
}
