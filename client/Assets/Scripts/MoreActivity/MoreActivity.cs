using UnityEngine;
using System.Collections.Generic;

public class MoreActivity : UIBase {

    public UIGrid grid_;
    public GameObject itemPrefab_;
    public GameObject closeBtn_;
    public Transform rightContent_;

    List<GameObject> itemPool_;

    ADType crtType_;
	List<ADType> _adTypes = new List<ADType>();
    int oldCount;

    void Awake()
    {
        itemPool_ = new List<GameObject>();
        oldCount = GamePlayer.Instance.adTypes.Count;
		_adTypes = GamePlayer.Instance.adTypes;
    }

	// Use this for initialization
	void Start () {
        UIEventListener.Get(closeBtn_).onClick += delegate { HideMe(); };
        UIManager.Instance.LoadMoneyUI(this.gameObject);
		MoreActivityData.instance.MoreActivityRedEvent += new RequestEventHandler<int> (MoreActivityRedEvent);
		UpdateLeft();
	}

    void UpdateLeft()
    {


        GameObject item = null;
        UIEventListener listener = null;
		for (int i = 0; i < _adTypes.Count; ++i)
        {
			if(_adTypes[i] == ADType.ADT_7Days)
				continue;
            if (i >= itemPool_.Count)
            {
                item = (GameObject)GameObject.Instantiate(itemPrefab_) as GameObject;
				if(MoreActivityData.instance.redList[(int)GamePlayer.Instance.adTypes[i]] == 1)
				{
					item.GetComponent<MoreActivityItem>().bgSp_.MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
				}
				else
				{
					item.GetComponent<MoreActivityItem>().bgSp_.MarkOff();
				}
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
            item.GetComponent<MoreActivityItem>().SetData(LanguageManager.instance.GetValue(GamePlayer.Instance.adTypes[i].ToString()));
            listener.parameter = GamePlayer.Instance.adTypes[i];
            item.SetActive(true);
        }
        grid_.Reposition();

        for (int i = oldCount; i < itemPool_.Count; ++i)
        {
            itemPool_[i].SetActive(false);
        }

        if (oldCount > 0)
        {
            crtType_ = GamePlayer.Instance.adTypes[0];
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
        if (oldCount != GamePlayer.Instance.adTypes.Count)
        {
            oldCount = GamePlayer.Instance.adTypes.Count;
            UpdateLeft();
        }
	}

    #region Fixed methods for UIBase derived cass

    public static void SwithShowMe()
    {
        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_MoreActivity);
    }

    public static void ShowMe()
    {
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_MoreActivity);
    }

    public static void HideMe()
    {
        UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_MoreActivity);
    }


	void MoreActivityRedEvent(int num )
	{
		for (int i = 0; i < itemPool_.Count; ++i)
		{
			if(MoreActivityData.instance.redList[(int)GamePlayer.Instance.adTypes[i]] == 1)
			{
				itemPool_[i].GetComponent<MoreActivityItem>().bgSp_.MarkOn(UISprite.MarkAnthor.MA_RightTop,-12,-25);
			}
			else
			{
				itemPool_[i].GetComponent<MoreActivityItem>().bgSp_.MarkOff();
			}
		}
	}

    public override void Destroyobj()
    {
        GameObject.Destroy(gameObject);
    }

	void OnDestroy()
	{
		MoreActivityData.instance.MoreActivityRedEvent -= MoreActivityRedEvent;
	}

    #endregion
}
