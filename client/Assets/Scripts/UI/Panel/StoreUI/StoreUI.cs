using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreUI : UIBase {
	public UILabel _TitleLable;
	public UILabel _FirstLable;
	public UILabel _RechargeLable;
	public UILabel _PropsLable;
	public UILabel _VIPLable;
	public UILabel _HaveReceivedLable;
	public UILabel _ReceivedLable;
	public UILabel _ChongzhiLable;

    public UITexture background;
    public UITexture fanliicon;

	public UIButton CloseBtn;
    public	List<UIButton> tabBtns = new List<UIButton>();
	public List<GameObject> tabObjs = new List<GameObject>();
	public static StoreUI StoreIns;
	public GameObject Tips;
	public GameObject listViewObj;
	UIPanel listPanel_;
	private BoxCollider listDragArea_;
	public storeItemTipsUI itemInfoTips;
	public GameObject grid;

    string background_icon = "storebackground_icon";
    string fanli_icon = "fanli_icon";
    static int idxWithShow;
	void Awake()
	{
		StoreIns = this;
	}
	public static StoreUI Instance
	{
		get
		{
			return StoreIns;	
		}
	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("Store_Title");
		_FirstLable.text = LanguageManager.instance.GetValue("Store_First");
		_RechargeLable.text = LanguageManager.instance.GetValue("Store_Recharge");
		_PropsLable.text = LanguageManager.instance.GetValue("Store_Props");
		_VIPLable.text = LanguageManager.instance.GetValue("Store_VIP");
		_HaveReceivedLable.text = LanguageManager.instance.GetValue("Store_HaveReceived");
		_ReceivedLable.text = LanguageManager.instance.GetValue("Store_Received");
		_ChongzhiLable.text = LanguageManager.instance.GetValue("Store_Chongzhi");
	}

	void Start () 
	{
		InitUIText ();
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		for(int i = 0;i<tabBtns.Count;i++)
		{
			UIManager.SetButtonEventHandler (tabBtns[i].gameObject, EnumButtonEvent.OnClick, OnClickTabBtns, i, 0);
		}
//		listPanel_ = listViewObj.gameObject.GetComponent<UIPanel>();
//		listDragArea_ = grid.gameObject.GetComponent<BoxCollider>();

		OpenPanelAnimator.PlayOpenAnimation (this.panel, () => {
            TabsSelect(idxWithShow);
            TabsSelectTab(idxWithShow);
			UIManager.Instance.LoadMoneyUI(this.gameObject);
			if(GamePlayer.Instance.isGetShouchong)
			{
				tabBtns[0].gameObject.SetActive(false);
			}
		});
        
        HeadIconLoader.Instance.LoadIcon(background_icon, background);
        HeadIconLoader.Instance.LoadIcon(fanli_icon, fanliicon);
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation (this.panel, () => {
				Hide ();
		});
	}
	void OnClickTabBtns(ButtonScript obj, object args, int param1, int param2)
	{
		TabsSelect (param1);
        //if(param1==1)
        //{
        //    PopText.Instance.Show (LanguageManager.instance.GetValue("zanweikaiqi"));
        //    return;
        //}
		TabsSelectTab (param1);
	}
 public	void TabsSelect(int index)
	{
		for (int i = 0; i<tabBtns.Count; i++) 
		{
			if(i==index)
			{
				tabBtns[i].isEnabled = false;
			}
			else
			{
				tabBtns[i].isEnabled = true;
			}
		}
	} 

	void TabsSelectTab(int index)
	{
		for (int i = 0; i<tabObjs.Count; i++) 
		{
			if(i==index)
			{
				tabObjs[i].SetActive(true);
			}
			else
			{
				tabObjs[i].SetActive(false);
			}
		}
	} 

    public void SwitchTab(int idx)
    {
        TabsSelect(idx);
        TabsSelectTab(idx);
    }

    public static void ShowMe(int idx = 3)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Shop))
		{
			PopText.Instance.Show( LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
        idxWithShow = idx;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS__StoreUI);
	}
	public static void SwithShowMe(int idx = 3)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Shop))
		{
			PopText.Instance.Show( LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
        idxWithShow = idx;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS__StoreUI);
	}

	public override void Destroyobj ()
	{
        HeadIconLoader.Instance.Delete(background_icon);
        HeadIconLoader.Instance.Delete(fanli_icon);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS__StoreUI, AssetLoader.EAssetType.ASSET_UI), true);
	}
}
