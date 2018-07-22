using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BagUI : UIBase
{
	public UIButton closeBtn;
	public UISprite itemsPane;
	public UIPanel heroPane;
	public GameObject bagCell;
	public Transform grid;
	public UIPanel	bagTipsPane;
	public UISprite[] EquipCells;
	public UISprite equipTips; 
	public UIButton demountBtn;
	public UIButton sortBtn;
	public UIButton fixBtn;
	public List<UIButton> tabBtn;
	public UILabel nameLab;
	public UISprite proImg;
	public UILabel levelLab;
	public UIButton bagBtn;
	public UIButton sellBtn;
	public UIButton equipeBtn;
	public UIButton debrisBtn;
	public UIButton xiaohaoBtn;
	public UISprite rolePane;
	public UIPanel sellPane;
	public UIButton sellOkBtn;
	public UIGrid  sellGrid;
	public Transform Mpos;
	public FriendMegBoxUI msgBoxPane;
	public BagTipsUI compTips;
	public UILabel sellMoneyLab;
	public BagUseItemUI bagUsePanel;
	public DebrisUI debrisPanel;
	public UIButton bankBtn;
	private COM_Item[] _BagItems = new COM_Item[100];
	private GameObject[] BagCells = new GameObject[20];
	private COM_Item[] _EquipList = new COM_Item[(int)EquipmentSlot.ES_Max];
	private COM_Item[] _FuWenList = new COM_Item[6];
	private COM_Item selectHeroEquip;
	private int _selsctTab;
	private bool bDouble = false;
	private bool bDoubleEquip = false;
	public List<GameObject> sellCellList = new List<GameObject> ();
	public List<GameObject> sellCellPool = new List<GameObject> ();
	public List<COM_Item> sellItemList = new List<COM_Item> ();

	public UILabel titleLab;
	public UILabel tabBtnBagLab;
	public UILabel tabBtnXiaohaoLab;
	public UILabel tabBtnEquipLab;
	public UILabel tabBtnDebrisLab;
	public UILabel sellLiftLab;
	public UILabel sellRightLab;
	public UILabel warehouseLab;
	public UILabel fixLab;
	public UILabel sortLab;
	public UIPanel CItemTips;
	public UISprite fashionBG;
	public UIButton wenhaoBtn;
	public UILabel fashionTime;
	public UIButton fuWenBtn; 
	public GameObject bagRoleObj;
	public GameObject fuwenRoleObj;
	public List<UISprite> fuwenList = new List<UISprite> ();
	public GameObject fuwenHeChengPanel;
	public Transform fuWenMpos;
	public UIGrid fuWengrid;
	public GameObject fuWenpropCell;
	public UILabel fuWenNameLab;
	public UISprite fuWenproImg;
	public UILabel fuWenlevelLab;
	public UIButton fuWemWenHao; 
	public UIButton roleEquipBtn; 
	private List<GameObject> fuWenCellList = new List<GameObject>();
	private List<GameObject> fuWenCellPool = new List<GameObject>();
	private Dictionary<PropertyType,COM_PropValue>  fuWenpropArr = new Dictionary<PropertyType, COM_PropValue>();
	private int _selectMianType;  
	private UIGrid  uigrid;
	private static BagUI _mainbag= null;
	public static BagUI Instance 
	{
		get{
			return _mainbag;
		}
	}
	
	void Awake()
	{
		_mainbag = this;
	}
	void Start ()
	{
		hasDestroy = false;
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (wenhaoBtn.gameObject, EnumButtonEvent.OnClick, OnWenhaoBtn, 0, 0);
		UIManager.SetButtonEventHandler (sortBtn.gameObject, EnumButtonEvent.OnClick, OnClickSortBtn, 0, 0);
		UIManager.SetButtonEventHandler (fixBtn.gameObject, EnumButtonEvent.OnClick, OnClickFixBtn, 0, 0);
		UIManager.SetButtonEventHandler (bagBtn.gameObject, EnumButtonEvent.OnClick, OnClickTabBag, 0, 0);
		UIManager.SetButtonEventHandler (sellBtn.gameObject, EnumButtonEvent.OnClick, OnClickTabSell, 0, 0);
		UIManager.SetButtonEventHandler (sellOkBtn.gameObject, EnumButtonEvent.OnClick, OnClickSellOk, 0, 0);
		UIManager.SetButtonEventHandler (bankBtn.gameObject, EnumButtonEvent.OnClick, OnClickbank, 0, 0);
		UIManager.SetButtonEventHandler (equipeBtn.gameObject, EnumButtonEvent.OnClick, OnClickEquip, 0, 0);
		UIManager.SetButtonEventHandler (debrisBtn.gameObject, EnumButtonEvent.OnClick, OnClickDebris, 0, 0);
		UIManager.SetButtonEventHandler (xiaohaoBtn.gameObject, EnumButtonEvent.OnClick, OnClickConsume, 0, 0);
		UIManager.SetButtonEventHandler (fashionBG.gameObject, EnumButtonEvent.OnClick, OnClickFashionBG, 0, 0);
		UIManager.SetButtonEventHandler (fuWenBtn.gameObject, EnumButtonEvent.OnClick, OnClickFuWen, 0, 0);
		UIManager.SetButtonEventHandler (roleEquipBtn.gameObject, EnumButtonEvent.OnClick, OnClickTabBag, 0, 0);

		titleLab.text = LanguageManager.instance.GetValue("bagtitle");
		tabBtnBagLab.text = LanguageManager.instance.GetValue("bagtabBtnBagLab");
		tabBtnXiaohaoLab.text = LanguageManager.instance.GetValue("bagtabBtnXiaohaoLab");
		tabBtnEquipLab.text = LanguageManager.instance.GetValue("bagtabBtnEquipLab");
		tabBtnDebrisLab.text = LanguageManager.instance.GetValue("bagtabBtnDebrisLab");
		sellLiftLab.text = LanguageManager.instance.GetValue("bagsellLiftLab");
		sellRightLab.text = LanguageManager.instance.GetValue("bagsellRightLab");
		warehouseLab.text = LanguageManager.instance.GetValue("bagwarehouseLab");
		fixLab.text = LanguageManager.instance.GetValue("bagfixLab");
		sortLab.text = LanguageManager.instance.GetValue("bagsortLab");

        GuideManager.Instance.RegistGuideAim(fuWenBtn.gameObject, GuideAimType.GAT_MainBagFuwenTab);

		//BagSystem.instance.ItemChanged += new ItemChangedEventHandler(OnItemChange);
		//BagSystem.instance.DelItemEvent += new ItemDelEventHandler(OnDelItem);
		//BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (OnUpdateItem); 
		BagSystem.instance.SortItemEvent += new ItemSortEventHandler (OnSortOk);
		GamePlayer.Instance.WearEquipEvent += new WearEquipEventHandler (OnWearEquip);
		GamePlayer.Instance.DelEquipEvent += new DelEquipEventHandler (OnDelEquip);
		BagSystem.instance.OpenBagGridEvent += new ItemSortEventHandler (OnOpenGridEvent);
		GamePlayer.Instance.DebrisItmeEnvet += new RequestEventHandler<int> (DebrisEventOk);
		GamePlayer.Instance.fuwenEnvet += new RequestEventHandler<int> (FuwenEnvet);
		bagTipsPane.GetComponent<BagTipsUI> ().closeCallback = OnHideTips;
		CItemTips.GetComponent<BagTipsUI> ().closeCallback = OnHideTips;

		bagTipsPane.GetComponent<BagTipsUI> ().openHCFuwen = OnOPenFuWen;

		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{ 

		uigrid = grid.GetComponent<UIGrid> ();
		_EquipList = GamePlayer.Instance.Equips; 
		_FuWenList = GamePlayer.Instance.fuwens_;
		_BagItems = BagSystem.instance.BagItems;
	
		nameLab.text = GamePlayer.Instance.InstName;
		levelLab.text =  GamePlayer.Instance.GetIprop(PropertyType.PT_Level).ToString();
		proImg.spriteName =((JobType) GamePlayer.Instance.GetIprop(PropertyType.PT_Profession)).ToString ();       
		
			fuWenNameLab.text = GamePlayer.Instance.InstName;
		fuWenproImg.spriteName =((JobType) GamePlayer.Instance.GetIprop(PropertyType.PT_Profession)).ToString ();      
		fuWenlevelLab.text =  GamePlayer.Instance.GetIprop(PropertyType.PT_Level).ToString();
		
		for(int t= 0;t<tabBtn.Count;t++)
		{
			UIManager.SetButtonEventHandler (tabBtn[t].gameObject, EnumButtonEvent.OnClick, OnClickTabBtn, 0, 0); 
		}
			// _EquipList.Length
		for(int j = 1;j <_EquipList.Length;j++) 
		{
				EquipCells[j].GetComponent<HeroEquipCellUI>().blackImg.gameObject.SetActive(false);
			if(_EquipList[j] != null) 
			{
				EquipCells[j].GetComponent<HeroEquipCellUI>().Equip = _EquipList[j];
				Profession profession = Profession.get((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), (int)GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
				
				if(ItemData.GetData((int)_EquipList[j].itemId_).slot_!= EquipmentSlot.ES_Ornament_0 && ItemData.GetData((int)_EquipList[j].itemId_).slot_ != EquipmentSlot.ES_Ornament_1)
				{
					if (! profession.canuseItem( ItemData.GetData((int)_EquipList[j].itemId_).subType_,ItemData.GetData((int)_EquipList[j].itemId_).level_))
					{
						EquipCells[j].GetComponent<HeroEquipCellUI>().blackImg.gameObject.SetActive(true);
					}
					else
					{
						EquipCells[j].GetComponent<HeroEquipCellUI>().blackImg.gameObject.SetActive(false);
					}
				}
			}
			UIManager.SetButtonEventHandler (EquipCells[j].gameObject, EnumButtonEvent.OnClick, OnClickEquipCell, 0, 0);
			UIEventListener.Get(EquipCells[j].gameObject).onDoubleClick = onDoubleEquip;
		}

			for(int i= 0;i<fuwenList.Count;i++)
			{
				fuwenList[i].GetComponent<HeroEquipCellUI>().blackImg.gameObject.SetActive(false);
				if(_FuWenList[i] != null)
				{
					fuwenList[i].GetComponent<HeroEquipCellUI>().Equip = _FuWenList[i];
				}
				UIManager.SetButtonEventHandler (fuwenList[i].gameObject, EnumButtonEvent.OnClick, OnClickFuWEnCell, i, 0);
				UIEventListener.Get(fuwenList[i].gameObject).onDoubleClick = onDoubleEquip;
			}
			SetFuwenProp ();
			ShowFuWenProp ();

			if(_EquipList[(int)EquipmentSlot.ES_Fashion] != null)
			{
				if(_EquipList[(int)EquipmentSlot.ES_Fashion].usedTimeout_ > 0)
				{
					fashionTime.gameObject.SetActive(true);
					fashionTime.text = FormatTimeHasDay(_EquipList[(int)EquipmentSlot.ES_Fashion].usedTimeout_);
				}
				else
				{
					fashionTime.gameObject.SetActive(false);
				}
			}
			else
			{
				fashionTime.gameObject.SetActive(false);
			}
		for(int i=0; i < BagCells.Length;i++)
		{
			GameObject obj = Object.Instantiate(bagCell) as GameObject;
			obj.SetActive(true);
			UIManager.SetButtonEventHandler (obj.GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);
			UIEventListener.Get(obj.GetComponent<BagCellUI>().pane.gameObject).onDoubleClick = OnCellDoubleClick;
			obj.GetComponent<BagCellUI>().Item = _BagItems[i];
			uigrid.AddChild(obj.transform);
			BagCells[i] = obj;
			obj.transform.localScale = Vector3.one;
			if(GamePlayer.Instance.isInBattle)
			{
				if(_BagItems[i]  != null)
				{
					if(ItemData.GetData((int)_BagItems[i].itemId_).usedFlag_ != ItemUseFlag.IUF_All && ItemData.GetData((int)_BagItems[i].itemId_).usedFlag_ != ItemUseFlag.IUF_Battle)
					{
						obj.GetComponent<BagCellUI>().blackImg.gameObject.SetActive(true);
					}
					else
					{
						obj.GetComponent<BagCellUI>().blackImg.gameObject.SetActive(false);
					}
				}
			}

			if(i > GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum))  //已开启背包格子数.
			{
				obj.GetComponent<BagCellUI>().isLock = false;
			}
			else
			{
				obj.GetComponent<BagCellUI>().isLock = true;
			}
		}
		tabBtn[0].isEnabled = false;
		bagBtn.isEnabled = false;
		roleEquipBtn.isEnabled = false;
		SelectTabNum = 0;
		_selectMianType = 0;
			
		if(role != null)
		{
			Destroy(role);
			role = null;
		}

		GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, EntityType.ET_Player,
                                          AssetLoadCallBack, new ParamData(GamePlayer.Instance.InstId, GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)), "UI", GamePlayer.Instance.DressID);

		GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, EntityType.ET_Player,
			                              FuWenAssetLoadCallBack, new ParamData(GamePlayer.Instance.InstId, GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)), "UI", GamePlayer.Instance.DressID);
			

		UIManager.Instance.LoadMoneyUI(this.gameObject);

		int fuWenOPen = 0;
		GlobalValue.Get(Constant.C_FuWenOpenLevel, out fuWenOPen);
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)< fuWenOPen)
		{
			fuWenBtn.gameObject.SetActive(false);
		}
		else
		{
			fuWenBtn.gameObject.SetActive(true);
		}
		
		wenhaoBtn.gameObject.SetActive(GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Guid));		

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ClickMainBag);

		});

    }

	public  string FormatTimeHasDay(int time)
	{
		int day = time/86400;
		int min = (time%86400)/3600;
		int second = ((time%86400)%3600)/60;
		return day + ":" + DoubleTime(min) + ":" + DoubleTime(second);
	}
	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}

	public GameObject GetRole ()
	{

	    if(role != null)
		{
			return role;
		}
			
		return null;
	}
    GameObject role;
	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset ((ENTITY_ID)data.iParam, false);
			return;
		}
		if(role != null)
		{ 
			Destroy(role);
			role = null;
		}
		role = ro; 
		ro.transform.parent = Mpos;
		//ro.transform.localPosition = Vector3.zero;
		float zoom = EntityAssetsData.GetData (GamePlayer.Instance.GetIprop (PropertyType.PT_AssetId)).zoom_;
		ro.transform.localPosition = Vector3.forward * -150f;
		ro.transform.localScale = new Vector3 (zoom, zoom, zoom);
		//ro.transform.localPosition = Vector3.forward * 800;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		//EffectLevel el = ro.AddComponent<EffectLevel>();
		//el.target = ro.transform.parent.parent.GetComponent<UISprite>();

		//role = ro;
	}
	GameObject fuwenRole;
	void FuWenAssetLoadCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset ((ENTITY_ID)data.iParam, false);
			return;
		}
		if(fuwenRole != null)
		{
			Destroy(fuwenRole);
			fuwenRole = null;
		}
		fuwenRole = ro; 
		ro.transform.parent = fuWenMpos;
		//ro.transform.localPosition = Vector3.zero;
		float zoom = EntityAssetsData.GetData (GamePlayer.Instance.GetIprop (PropertyType.PT_AssetId)).zoom_;
		ro.transform.localPosition = Vector3.forward * -150f;
		ro.transform.localScale = new Vector3 (450f, 450f, 450f);
		//ro.transform.localPosition = Vector3.forward * 800;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		//EffectLevel el = ro.AddComponent<EffectLevel>();
		//el.target = ro.transform.parent.parent.GetComponent<UISprite>();
		
		//role = ro;
	}





	#region Fixed methods for UIBase derived cass

	public static void SwithShowMe()
	{
		if(!BagSystem.instance.IsInit )
		{
			BagSystem.instance.isOpenInitBag = true;
			NetConnection.Instance.requestBag();
            NetWaitUI.ShowMe();
		}
		else
		{
			UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_BagPanel);
		}
	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_BagPanel);
	}

	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_BagPanel);
	}
	
	#endregion
	

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			if(BagSystem.instance.battleOpenBag ) 
			{
				BagSystem.instance.battleOpenBag = false;
			}
			Destroy(role);
			Hide ();	
		});
        
	}
 
	private void OnWenhaoBtn(ButtonScript obj, object args, int param1, int param2)
	{
		HelpUI.SwithShowMe (1,4);
	}

	protected override void DoHide ()
	{
		//BagSystem.instance.ItemChanged -= OnItemChange;
		//BagSystem.instance.DelItemEvent -= OnDelItem;
		//BagSystem.instance.UpdateItemEvent -= OnUpdateItem;
		BagSystem.instance.SortItemEvent -= OnSortOk;
		GamePlayer.Instance.WearEquipEvent -= OnWearEquip;
		GamePlayer.Instance.DelEquipEvent -= OnDelEquip;
		BagSystem.instance.OpenBagGridEvent -= OnOpenGridEvent;
		GamePlayer.Instance.DebrisItmeEnvet -= DebrisEventOk;
		GamePlayer.Instance.fuwenEnvet -= FuwenEnvet;


		for(int i= 0;i<sellCellPool.Count;i++)
		{
			GameObject.DestroyObject(sellCellPool[i]);
			sellCellPool[i] = null;
		}
		sellCellPool.Clear ();
		if(role != null)
		{
			Destroy(role);
			role = null;
		}
		if(fuwenRole != null)
		{
			Destroy(fuwenRole);
			fuwenRole = null;
		}

		base.DoHide ();
	}
	bool hasDestroy;
	void OnDestroy()
	{
		hasDestroy = true;
	}

	void onDoubleEquip(GameObject obj)
	{
		bDoubleEquip = true;
		StopCoroutine ("DelayClickEquip");

		COM_Item Item = obj.GetComponentInParent<HeroEquipCellUI> ().Equip;
		if (Item == null) 
		{
			return;
		}
		
		if(BagSystem.instance.GetBagSize() >= GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum))
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("bagfull"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			return;
		}

		if(ItemData.GetData((int)Item.itemId_).mainType_ == ItemMainType.IMT_FuWen)
		{
			NetConnection.Instance.takeoffFuwen(Item.slot_);
			return;
		}
		
		
		if (GamePlayer.Instance.isInBattle)
		{
			if(ItemData.GetData((int)Item.itemId_).slot_ != EquipmentSlot.ES_SingleHand && ItemData.GetData((int)Item.itemId_).slot_ != EquipmentSlot.ES_DoubleHand)
			{
				return;
			}

			Battle.Instance.UseItem((int)Item.instId_);
			BagSystem.instance.battleOpenBag = false;
			BagUI.HideMe();
		}
		else
		{
			NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId , Item.instId_);
			NetWaitUI.ShowMe();
		}


	}

	void OnCellDoubleClick(GameObject obj)
	{
		bDouble = true;
		StopCoroutine ("DelayOneClick");
		COM_Item Item = obj.GetComponentInParent<BagCellUI> ().Item;

		if(Item == null)
		{
			return;
		}

		int _ItemId = 0;
		GlobalValue.Get(Constant.C_WishItem, out _ItemId);
		if(Item.itemId_ == _ItemId)
		{
			if(!Prebattle.Instance.IsWishingAvailable())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("xuyuanre"));
				return;
			}
			WishingTreeUI.SwithShowMe();
			return;
		}

		if (sellPane.gameObject.activeSelf) 
		{
			if(Item.isLock_)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("cantsell"));
				return;
			}

			if(ItemData.GetData((int)Item.itemId_).price_ <= 0 || ItemData.GetData((int)Item.itemId_).mainType_ == ItemMainType.IMT_Quest)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("cantsell"));
				return;
			}

			if(sellItemList.Count>= 16)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("sellMax"));
				return;
			}
			
			for(int i = 0;i<sellItemList.Count;i++)
			{
				if(sellItemList[i].instId_ == Item.instId_)
				{
					return ;
				}
			}
			
			GameObject sellObj = null;
			
			obj.GetComponentInParent<BagCellUI> ().itemIcon.gameObject.SetActive(false);
			obj.GetComponentInParent<BagCellUI> ().countLab.gameObject.SetActive(false);
			obj.GetComponentInParent<BagCellUI> ().debirsImg.gameObject.SetActive(false);
			obj.GetComponentInParent<BagCellUI> ().suoImg.gameObject.SetActive(false);
			obj.GetComponentInParent<BagCellUI> ().pane.spriteName = "bb_daojukuang1";
			UIManager.RemoveButtonAllEventHandler ( obj.GetComponentInParent<BagCellUI> ().pane.gameObject);
			
			sellObj = Object.Instantiate(bagCell.gameObject) as GameObject;
			sellObj.GetComponent<BagCellUI>().Item =  obj.GetComponentInParent<BagCellUI> ().Item;
			sellObj.transform.parent = sellGrid.transform;
			sellObj.SetActive(true);
			sellItemList.Add( obj.GetComponentInParent<BagCellUI> ().Item);
			sellCellList.Add(sellObj);
			sellObj.transform.localScale = Vector3.one;
			UIManager.SetButtonEventHandler (sellObj.GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickSellCell, 0, 0);
			sellGrid.Reposition();
			
			
			int money = 0;
			for(int x=0;x<sellItemList.Count;x++)
			{
				money += ItemData.GetData((int)sellItemList[x].itemId_).price_ * sellItemList[x].stack_;
			}
			sellMoneyLab.text = money.ToString();
			
			return ;
		} 


		ItemData _itemData = ItemData.GetData ((int)Item.itemId_);

		if(_itemData == null)
		{
			return;
		}

		if(!_itemData.canUse_)
		{
			return;
		}
		else if(_itemData.usedFlag_ == ItemUseFlag.IUF_Scene)
		{
			if(GamePlayer.Instance.isInBattle)
				return;
		}
        else if (_itemData.usedFlag_ == ItemUseFlag.IUF_Battle)
		{
			if(!GamePlayer.Instance.isInBattle)
				return;
		}

		if(_itemData.mainType_ == ItemMainType.IMT_FuWen)
		{
			if(!fuwenRoleObj.activeSelf)
				return;
			NetConnection.Instance.wearFuwen ((int)Item.instId_);
		}
		else if(_itemData.mainType_ == ItemMainType.IMT_Equip)
		{

			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) /10 +1 < _itemData.level_)
			{
				//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("equipLevel"));
				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
				return;
			}
			
			JobType jt = (JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
			int level = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);
			Profession profession = Profession.get(jt, level);
			if (null == profession)
				return;
			
			if(_itemData.slot_ == EquipmentSlot.ES_Ornament_0 || _itemData.slot_ == EquipmentSlot.ES_Ornament_1)
			{
				
			}
			else
			{
				if (!profession.canuseItem(_itemData.subType_, _itemData.level_))
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("equipProfession"));
					return;
				}
			}

			if (GamePlayer.Instance.isInBattle)
			{
				if(_itemData.slot_ != EquipmentSlot.ES_SingleHand && _itemData.slot_ != EquipmentSlot.ES_DoubleHand)
				{
					return;
				}
			}

			if(!Item.isBind_ && _itemData.bindType_ == BindType.BIT_Use)
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("shifoubangding"),()=>{

					if (GamePlayer.Instance.isInBattle)
					{
						Battle.Instance.UseItem((int)Item.instId_);
						BagSystem.instance.battleOpenBag = false;
						BagUI.HideMe();
					}
					//else
					//NetConnection.Instance.wearEquipment((uint)GamePlayer.Instance.InstId, Item.instId_);
					
					
					if(_itemData.slot_ == EquipmentSlot.ES_SingleHand)
					{
						if(_itemData.subType_ == ItemSubType.IST_Shield)
						{
							if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
							}
						}
						else
						{
							if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null && ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].itemId_).subType_ != ItemSubType.IST_Shield)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
							}
						}
					}
					else if(_itemData.slot_ ==EquipmentSlot.ES_DoubleHand)
					{
						if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand].instId_);
						}
					}
					else if(_itemData.slot_ == EquipmentSlot.ES_Ornament_0 ||_itemData.slot_ == EquipmentSlot.ES_Ornament_1 )
					{
						
						if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null && GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
							}
							else if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
							}
							else
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
							}
						}
						else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
							}
							else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
							{
								if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
								{
									NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
								}
							}
						}
						else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null )
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
							}
							else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
							{
								if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
								{
									NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
								}
							}
						}
					}
					if (!GamePlayer.Instance.isInBattle)
					{
						NetConnection.Instance.wearEquipment((uint)GamePlayer.Instance.InstId, Item.instId_);
						NetWaitUI.ShowMe();
					}

				});
			}
			else
			{
				if (GamePlayer.Instance.isInBattle)
				{
					Battle.Instance.UseItem((int)Item.instId_);
					BagSystem.instance.battleOpenBag = false;
					BagUI.HideMe();
				}
				//else
				//NetConnection.Instance.wearEquipment((uint)GamePlayer.Instance.InstId, Item.instId_);
				
				
				if(_itemData.slot_ == EquipmentSlot.ES_SingleHand)
				{
					if(_itemData.subType_ == ItemSubType.IST_Shield)
					{
						if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
					else
					{
						if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null && ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].itemId_).subType_ != ItemSubType.IST_Shield)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
				}
				else if(_itemData.slot_ ==EquipmentSlot.ES_DoubleHand)
				{
					if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
					{
						NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand].instId_);
					}
				}
				else if(_itemData.slot_ == EquipmentSlot.ES_Ornament_0 ||_itemData.slot_ == EquipmentSlot.ES_Ornament_1 )
				{
					
					if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null && GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
					}
					else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
							}
						}
					}
					else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null )
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
							}
						}
					}
				}
				if (!GamePlayer.Instance.isInBattle)
				{
					NetConnection.Instance.wearEquipment((uint)GamePlayer.Instance.InstId, Item.instId_);
					NetWaitUI.ShowMe();
				}
			}
			
		}
		else 
		{
			if(!GamePlayer.Instance.isInBattle &&  ItemData.GetData((int) Item.itemId_).subType_ == ItemSubType.IST_Blood)
			{
				bagUsePanel._item = Item;
				bagUsePanel.stack = 1;
				bagUsePanel.Show();
				return;
			}
			if(GamePlayer.Instance.isInBattle)
			{
				Battle.Instance.UseItem((int)Item.instId_);
				BagSystem.instance.battleOpenBag = false;
				BagUI.HideMe();
			}
			else
			{
				if(_itemData.mainType_ == ItemMainType.IMT_Consumables)
				{
					/*if(_itemData.subType_ == ItemSubType.IST_BabyExp)
					{
						if(GamePlayer.Instance. MaxUseAllNum(Item,1) > 0)
						{
							NetConnection.Instance.useItem (Item.slot_,(uint)GamePlayer.Instance.InstId, 1);
						}else
						{
							PopText.Instance.Show(LanguageManager.instance.GetValue("expItem"));
						}
					}
					else */
					if(_itemData.subType_ == ItemSubType.IST_SkillExp)
					{
						if(!GamePlayer.Instance.IsCanUseSkillExpItem())
						{
							PopText.Instance.Show(LanguageManager.instance.GetValue("EN_NoUpSkill"));
						}
						else
						{
							NetConnection.Instance.useItem ((uint)Item.slot_,(uint)GamePlayer.Instance.InstId, 1);
						}
					}
					else
					{
						NetConnection.Instance.useItem ((uint)Item.slot_,(uint)GamePlayer.Instance.InstId, 1);
					}
				}
			}
		}

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagItemDoubleClick, (int)Item.itemId_);
	}
	
	IEnumerator DelayClickEquip(COM_Item bCell)
	{
		yield return new WaitForSeconds(0.3f);

		if(!bDoubleEquip)
		{
			bagTipsPane.gameObject.SetActive (true);
			bagTipsPane.GetComponent<BagTipsUI>().tipsImg.gameObject.SetActive(true);
			bagTipsPane.GetComponent<BagTipsUI> ().Item = bCell; 
			bagTipsPane.GetComponent<BagTipsUI>().PlayerInstId = (uint)GamePlayer.Instance.InstId;
			bagTipsPane.GetComponent<BagTipsUI> ().ShowDemountBtn ();

			if(bCell.isLock_)
			{
				bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(true);
				bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
				bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
			}
			else
			{
				bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(false);
				bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
				bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
			}
			if(bCell.lastSellTime_ > 0)
			{
				bagTipsPane.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(true);
				bagTipsPane.GetComponent<BagTipsUI>().lastTime.text = FormatTimeHasDay((int)bCell.lastSellTime_);
			}
			else
			{
				bagTipsPane.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(false);
			}
			if(GamePlayer.Instance.isInBattle)
			{
				if(ItemData.GetData((int)bCell.itemId_).slot_ != EquipmentSlot.ES_SingleHand && ItemData.GetData((int)bCell.itemId_).slot_ != EquipmentSlot.ES_DoubleHand)
				{
					bagTipsPane.GetComponent<BagTipsUI> ().DemountBtn.gameObject.SetActive(false);
				}
			}

			if(role != null)
				role.gameObject.SetActive(false);
			
			
			
		}
	}
	

	IEnumerator DelayOneClick(BagCellUI bCell)
	{
		yield return new WaitForSeconds(0.2f);

		if(!bDouble)
		{
			if (sellPane.gameObject.activeSelf) 
			{
				if(ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_Quest)
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("cantsell"));
					yield break;
				}

				if(ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_Equip || ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_EmployeeEquip || ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_FuWen ||ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_BabyEquip)
				{
					if(bCell.Item.isLock_)
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("cantsell"));
						yield break ;
					}
					bagTipsPane.gameObject.SetActive (true);
					bagTipsPane.GetComponent<BagTipsUI>().bagCell = bCell;
					bagTipsPane.GetComponent<BagTipsUI>().Item = bCell.Item; 
					bagTipsPane.GetComponent<BagTipsUI>().shoWSell = false;
					role.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().tipsImg.gameObject.SetActive(true);
					bagTipsPane.GetComponent<BagTipsUI>().PlayerInstId = (uint)GamePlayer.Instance.InstId;
					bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(false);
				}
				else
				{
					CItemTips.GetComponent<BagTipsUI>().bagCell = bCell;
					CItemTips.GetComponent<BagTipsUI>().Item = bCell.Item; 
					CItemTips.GetComponent<BagTipsUI>().shoWSell = false;
					CItemTips.gameObject.SetActive (true);
					role.gameObject.SetActive(false);  
					CItemTips.GetComponent<BagTipsUI>().tipsImg.gameObject.SetActive(true);
					CItemTips.GetComponent<BagTipsUI>().PlayerInstId = (uint)GamePlayer.Instance.InstId;
					CItemTips.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(false);
					CItemTips.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
					CItemTips.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
					CItemTips.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(false);

					if(bCell.Item.lastSellTime_ > 0)
					{
						CItemTips.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(true);
						CItemTips.GetComponent<BagTipsUI>().lastTime.text = FormatTimeHasDay((int)bCell.Item.lastSellTime_);
					}
					else
					{
						CItemTips.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(false);
					}
				}

				if(ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_FuWen)
				{
					bagTipsPane.GetComponent<BagTipsUI>().fuwenBtn.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().fuwenHCBtn.gameObject.SetActive(false);
				}


				yield break;  
			}

			if(ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_Debris)
			{
				debrisPanel.Debris = bCell.Item;
				debrisPanel.gameObject.SetActive(true);
				yield break;;
			}
			if(ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_Equip || ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_EmployeeEquip ||ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_FuWen ||ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_BabyEquip)
			{
				bagTipsPane.GetComponent<BagTipsUI>().shoWSell = true;
				bagTipsPane.gameObject.SetActive (true);
				role.gameObject.SetActive(false);
				bagTipsPane.GetComponent<BagTipsUI>().Item = bCell.Item; 
				bagTipsPane.GetComponent<BagTipsUI>().tipsImg.gameObject.SetActive(true);
				bagTipsPane.GetComponent<BagTipsUI>().PlayerInstId = (uint)GamePlayer.Instance.InstId;

				if(bCell.Item.isLock_)
				{
					bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(true);
					bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(true);
				}
				else
				{
					bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(true);
					bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
				}
				if(bCell.Item.lastSellTime_ > 0)
				{
					bagTipsPane.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(true);
					bagTipsPane.GetComponent<BagTipsUI>().lastTime.text = FormatTimeHasDay((int)bCell.Item.lastSellTime_);
				}
				else
				{
					bagTipsPane.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(false);
				}

				if(ItemData.GetData((int)bCell.Item.itemId_).mainType_ == ItemMainType.IMT_FuWen)
				{
					if(!fuwenRoleObj.activeSelf)
					{
						bagTipsPane.GetComponent<BagTipsUI>().fuwenBtn.gameObject.SetActive(false);
						bagTipsPane.GetComponent<BagTipsUI>().fuwenHCBtn.gameObject.SetActive(false);
						bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
						bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
					}
					else
					{
						if(ItemData.GetData((int)bCell.Item.itemId_).level_ >= 10)
						{
							bagTipsPane.GetComponent<BagTipsUI>().fuwenHCBtn.gameObject.SetActive(false);
						} 
						bagTipsPane.GetComponent<BagTipsUI>().excreteBtn.gameObject.SetActive(false);
						bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
						bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
					}
				}
			}
			else
			{
				CItemTips.GetComponent<BagTipsUI>().shoWSell = true;
				CItemTips.gameObject.SetActive (true);
				if(role != null)
					role.gameObject.SetActive(false);
				CItemTips.GetComponent<BagTipsUI>().Item = bCell.Item; 
				CItemTips.GetComponent<BagTipsUI>().tipsImg.gameObject.SetActive(true);
				CItemTips.GetComponent<BagTipsUI>().PlayerInstId = (uint)GamePlayer.Instance.InstId;
				if(bCell.Item.lastSellTime_ > 0)
				{
					CItemTips.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(true);
					CItemTips.GetComponent<BagTipsUI>().lastTime.text = FormatTimeHasDay((int)bCell.Item.lastSellTime_);
				}
				else
				{
					CItemTips.GetComponent<BagTipsUI>().lastTime.gameObject.SetActive(false);
				}
			}


			ItemData itemData  = ItemData.GetData((int)bCell.Item.itemId_);
			
			if(itemData.mainType_ == ItemMainType.IMT_Equip)
			{
				if(GamePlayer.Instance.Equips[(int)itemData.slot_] != null)
				{
					compTips.gameObject.SetActive(true);
					compTips.tipsImg.gameObject.SetActive(true);
					role.gameObject.SetActive(false);
					compTips.Item = GamePlayer.Instance.Equips[(int)itemData.slot_];
					//ApplicationEntry.Instance.ui3DCamera.depth = -1;
				}
			}
		}
	}


	private void OnClickCell(ButtonScript obj, object args, int param1, int param2)
	{
		BagCellUI bCell = obj.GetComponentInParent<BagCellUI> ();
		
		if(!bCell.isLock)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("buyitemopenbag").Replace("{n}",ItemData.GetData(5001).name_),()=>{StoreUI.SwithShowMe(2);});
			return;
		}
		if(bCell.Item == null)
		{
			return;
		}

		bDouble = false;
		StartCoroutine (DelayOneClick (bCell));
	}

	private void OnClickEquipCell(ButtonScript obj, object args, int param1, int param2)
	{
		COM_Item item = obj.GetComponent<HeroEquipCellUI> ().Equip; 
		if(item == null)
		{
			return;
		}
		item = GamePlayer.Instance.Equips [item.slot_];
		bDoubleEquip = false;
		StartCoroutine (DelayClickEquip(item));

	}

	private void OnClickFuWEnCell(ButtonScript obj, object args, int param1, int param2)
	{
		COM_Item item = obj.GetComponent<HeroEquipCellUI> ().Equip; 
		if(item == null)
		{
			return;
		}
		bDoubleEquip = false;
		StartCoroutine (DelayClickEquip(item));
	}


	private void OnClickSortBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.sortBagItem ();		
	}

	private void OnClickFixBtn(ButtonScript obj, object args, int param1, int param2)
	{
		FixAllUI.ShowMe ();	
	}

	private void OnClickTabBag(ButtonScript obj, object args, int param1, int param2)
	{
		equipeBtn.isEnabled = true;
		fuWenBtn.isEnabled = true;
		debrisBtn.isEnabled = true;
		xiaohaoBtn.isEnabled = true;
		bagBtn.isEnabled = false;
		roleEquipBtn.isEnabled = false;
		sellBtn.isEnabled = true;
		sellBtn.gameObject.SetActive(true);
		rolePane.gameObject.SetActive (true);
		sellPane.gameObject.SetActive (false);
		SetItemIsSell (false);
		sortBtn.gameObject.SetActive (true);
		fixBtn.gameObject.SetActive (true);
		bankBtn.gameObject.SetActive (true);
		SelectTabItem (0);
		bagRoleObj.gameObject.SetActive(true);
		fuwenRoleObj.gameObject.SetActive(false);
		return;

	}

	private void OnClickTabSell(ButtonScript obj, object args, int param1, int param2)
	{
		equipeBtn.isEnabled = true;
		fuWenBtn.isEnabled = true;
		debrisBtn.isEnabled = true;
		xiaohaoBtn.isEnabled = true;
		bagBtn.isEnabled = true;
		roleEquipBtn.isEnabled = false;
		sellBtn.isEnabled = false;
		rolePane.gameObject.SetActive (false);
		sellPane.gameObject.SetActive (true);
		sellMoneyLab.text = "0";
		SetItemIsSell (true);
		sortBtn.gameObject.SetActive (false);
		fixBtn.gameObject.SetActive (false);
		bankBtn.gameObject.SetActive (false);
		bagRoleObj.gameObject.SetActive(false);
		fuwenRoleObj.gameObject.SetActive(false);
	}

	private void OnClickTabBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<tabBtn.Count;i++)
		{
			if(obj.name == tabBtn[i].name)
			{
				tabBtn[i].isEnabled = false;
				SelectTabNum = i;
			}
			else
			{
				tabBtn[i].isEnabled  = true;
			}
		}
	}

	public void OnClickSellCell(ButtonScript obj, object args, int param1, int param2)
	{
		COM_Item item = obj.GetComponentInParent<BagCellUI> ().Item;

		if(item == null)
		{
			return;
		}
		sellItemList.Remove (item);
		obj.gameObject.transform.parent.parent = null;
		sellCellList.Remove (obj.gameObject.transform.parent.gameObject);
		Destroy (obj.gameObject.transform.parent.gameObject);
		sellGrid.Reposition ();


		int money = 0;
		for(int x = 0;x<sellItemList.Count;x++)
		{
			money += ItemData.GetData((int)sellItemList[x].itemId_).price_ * sellItemList[x].stack_;
		}
		sellMoneyLab.text = money.ToString();



		for(int c = 0;c<BagCells.Length;c++)
		{
			BagCellUI i = BagCells[c].GetComponent<BagCellUI> ();
			if(i == null || i.Item == null)
				continue;
			if(item.instId_ == i.Item.instId_)
			{
			
				//i.itemIcon.gameObject.SetActive(true);
				i.Item =  item;
				//i.pane.spriteName  = BagSystem.instance.GetQualityBack((int)ItemData.GetData( (int)i.Item.itemId_).quality_);
				//i.countLab.gameObject.SetActive(true);
				UIManager.SetButtonEventHandler (BagCells[c].GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);
				UIEventListener.Get(BagCells[c].GetComponent<BagCellUI>().pane.gameObject).onDoubleClick = OnCellDoubleClick;
				return;
			}
		}
	}

	private void OnClickEquip(ButtonScript obj, object args, int param1, int param2)
	{
		equipeBtn.isEnabled = false;
		fuWenBtn.isEnabled = true;
		debrisBtn.isEnabled = true;
		xiaohaoBtn.isEnabled = true;
		bagBtn.isEnabled = true;
		roleEquipBtn.isEnabled = false;
		sellBtn.isEnabled = true;
		sellBtn.gameObject.SetActive(true);
		rolePane.gameObject.SetActive (true);
		sellPane.gameObject.SetActive (false);
		SetItemIsSell (false);
		sortBtn.gameObject.SetActive (false);
		bankBtn.gameObject.SetActive (false);
		fixBtn.gameObject.SetActive (true);

		SelectTabItem ((int)ItemMainType.IMT_Equip);
		bagRoleObj.gameObject.SetActive(true);
		fuwenRoleObj.gameObject.SetActive(false);
	}
	private void OnClickbank(ButtonScript obj, object args, int param1, int param2)
	{
		BankControUI.ShowMe ();
	}
	private void OnClickDebris(ButtonScript obj, object args, int param1, int param2)
	{
		equipeBtn.isEnabled = true;
		debrisBtn.isEnabled = false;
		fuWenBtn.isEnabled = true;
		xiaohaoBtn.isEnabled = true;
		bagBtn.isEnabled = true;
		roleEquipBtn.isEnabled = false;
		sellBtn.isEnabled = true;
		sellBtn.gameObject.SetActive(true);
		rolePane.gameObject.SetActive (true);
		sellPane.gameObject.SetActive (false);
		SetItemIsSell (false);
		sortBtn.gameObject.SetActive (false);
		fixBtn.gameObject.SetActive (false);
		bankBtn.gameObject.SetActive (false);
		SelectTabItem ((int)ItemMainType.IMT_Debris);
		bagRoleObj.gameObject.SetActive(true);
		fuwenRoleObj.gameObject.SetActive(false);
	}

	private void OnClickFuWen(ButtonScript obj, object args, int param1, int param2)
	{
		fuWenBtn.isEnabled = false;
		equipeBtn.isEnabled = true;
		debrisBtn.isEnabled = true;
		xiaohaoBtn.isEnabled = true;
		bagBtn.isEnabled = true;
		roleEquipBtn.isEnabled = true;
		sellBtn.gameObject.SetActive(false);
		rolePane.gameObject.SetActive (true);
		sellPane.gameObject.SetActive (false);
		SetItemIsSell (false);
		sortBtn.gameObject.SetActive (false);
		fixBtn.gameObject.SetActive (false);
		bankBtn.gameObject.SetActive (false);
		SelectTabItem ((int)ItemMainType.IMT_FuWen);

		bagRoleObj.gameObject.SetActive(false);
		fuwenRoleObj.gameObject.SetActive(true);
		//UIManager.Instance.AdjustUIDepth (fuwenRoleObj.transform);

        GlobalInstanceFunction.Instance.Invoke(delegate
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagFuwenOpen);
        }, 1);
	}


	private void OnClickConsume(ButtonScript obj, object args, int param1, int param2)
	{
		equipeBtn.isEnabled = true;
		debrisBtn.isEnabled = true;
		fuWenBtn.isEnabled = true;
		xiaohaoBtn.isEnabled = false;
		bagBtn.isEnabled = true;
		roleEquipBtn.isEnabled = false;
		sellBtn.isEnabled = true;
		sellBtn.gameObject.SetActive(true);
		rolePane.gameObject.SetActive (true);
		sellPane.gameObject.SetActive (false);
		SetItemIsSell (false);
		sortBtn.gameObject.SetActive (false);
		fixBtn.gameObject.SetActive (false);
		bankBtn.gameObject.SetActive (false);

		SelectTabItem ((int)ItemMainType.IMT_Consumables);
		bagRoleObj.gameObject.SetActive(true);
		fuwenRoleObj.gameObject.SetActive(false);
	}

	private void OnClickFashionBG(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("goshop"),()=>{StoreUI.SwithShowMe(2);});
	}

	public void SelectTabItem(int type)
	{
		if(_selectMianType == type && sellCellList.Count == 0)
		{
			return;
		}
		_selectMianType = type;
		List<COM_Item> items = new List<COM_Item> ();

		
		for(int i=0;i<tabBtn.Count;i++)
		{
			if(i == 0)
			{
				tabBtn[i].isEnabled = false;
				SelectTabNum = 0;
			}
			else
			{
				tabBtn[i].isEnabled  = true;
			}
		}


		if(type == 0)
		{
			items = new List<COM_Item>(BagSystem.instance.BagItems);
		}
		else
		{
			items = BagSystem.instance.GetMainTypeItems ((ItemMainType)type);
		}

		for(int i=0; i < items.Count && i<BagCells.Length;i++)
		{
			BagCells[i].GetComponent<BagCellUI>().Item =items[i];
			UIManager.SetButtonEventHandler (BagCells[i].GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);
			UIEventListener.Get(BagCells[i].GetComponent<BagCellUI>().pane.gameObject).onDoubleClick = OnCellDoubleClick;

            if (i == 0 && (ItemMainType)type == ItemMainType.IMT_FuWen)
                GuideManager.Instance.RegistGuideAim(BagCells[i].GetComponent<BagCellUI>().pane.gameObject, GuideAimType.GAT_MainBagFuwenFirstItem);
		}

		if(items.Count < BagCells.Length)
		{
			for(int j=items.Count; j<BagCells.Length;j++)
			{
				BagCells[j].GetComponent<BagCellUI>().Item = null;
				UIManager.RemoveButtonAllEventHandler (BagCells [j].GetComponent<BagCellUI> ().pane.gameObject);
			}
		}

		for( int  g=0;g<sellCellList.Count;g++)
		{
			COM_Item item = sellCellList[g].GetComponent<BagCellUI> ().Item;
			if(item == null)
			{
				return;
			}
			foreach(GameObject c in BagCells)
			{
				BagCellUI i = c.GetComponent<BagCellUI> ();
				if(i == null || i.Item == null)
					continue;
				if(item.instId_ == i.Item.instId_)
				{
					i.itemIcon.gameObject.SetActive(true);
					if(item.stack_ > 1)
					{
						i.countLab.gameObject.SetActive(true);
						i.countLab.text =item.stack_.ToString();
					}
					else
						i.countLab.gameObject.SetActive(false);
					UIManager.SetButtonEventHandler (c.GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);
					UIEventListener.Get(c.GetComponent<BagCellUI>().pane.gameObject).onDoubleClick = OnCellDoubleClick;
					break;
				}
			}
			
			sellItemList.Remove (sellCellList[g].gameObject.GetComponent<BagCellUI> ().Item);
			//g.gameObject.transform.parent = null;
			Destroy(sellCellList[g].gameObject);
			sellCellList[g].gameObject.SetActive (false);
		}
		sellCellList.Clear();

		//UpdataTabBagItems ();

	}

	private void OnClickSellOk(ButtonScript obj, object args, int param1, int param2)
	{  
		if(sellItemList.Count <= 0)
		{
			return;
		}

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("bagsellall"), () => {

			foreach(COM_Item  i in sellItemList)
			{
                NetConnection.Instance.sellBagItem(i.instId_, (uint)i.stack_);
			}
			ClearSellList ();
			sellItemList.Clear ();
			sellMoneyLab.text = "0";

			PopText.Instance.Show(LanguageManager.instance.GetValue("shushouOk"));
			});

	}

	private void ClearSellList()
	{
		for( int i=0 ;i< sellCellList.Count;i++)
		{
			sellCellList[i].gameObject.transform.parent = null;
			Destroy(sellCellList[i]);
			sellCellList[i] = null;
		}
		sellCellList.Clear();
	}

    void UpdateItem()
    {
        //更新界面数据
        _BagItems = BagSystem.instance.BagItems;

        //更新tips
        for (int i = 0; i < _BagItems.Length; ++i)
        {
            if (_BagItems[i] == null)
                continue;

            if (bagTipsPane.gameObject.activeSelf)
            {
                if (bagTipsPane.GetComponent<BagTipsUI>().Item.instId_ == _BagItems[i].instId_)
                {
                    bagTipsPane.GetComponent<BagTipsUI>().Item = _BagItems[i];
                    if (_BagItems[i].isLock_)
                    {
                        bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(true);
                        bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
                        bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(true);
                    }
                    else
                    {
                        bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(false);
                        bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(true);
                        bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
                    }
                    if (ItemData.GetData((int)_BagItems[i].itemId_).mainType_ == ItemMainType.IMT_FuWen)
                    {
                        if (!fuwenRoleObj.activeSelf)
                        {
                            bagTipsPane.GetComponent<BagTipsUI>().fuwenBtn.gameObject.SetActive(false);
                            bagTipsPane.GetComponent<BagTipsUI>().fuwenHCBtn.gameObject.SetActive(false);
                        }
                    }

                }
            }
        }
        
        //更新界面
        UpdataTabBagItems();
    }

	private void OnItemChange(COM_Item item)
	{
		if (item.slot_ >= _BagItems.Length) 
		{
			return;
		}
		_BagItems [item.slot_] = item;

		if (BagCells.Length <= item.slot_) 
		{
			//OnSortOk();
			UpdataTabBagItems ();
			return;
	
		}
		//BagCells [item.slot_].GetComponent<BagCellUI>().Item = item;
	//	UIManager.SetButtonEventHandler (BagCells [item.slot_].GetComponent<BagCellUI> ().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0); 
		//UIEventListener.Get(BagCells [item.slot_].GetComponent<BagCellUI>().pane.gameObject).onDoubleClick = OnCellDoubleClick;
		UpdataTabBagItems ();
		//uigrid.repositionNow = true;
	}

	private void OnDelItem(uint slot)   
	{
		for(int i =0;i<BagCells.Length;i++)
		{
			if(BagCells [i].GetComponent<BagCellUI> ().Item == null)
				continue;
			if(BagCells [i].GetComponent<BagCellUI> ().Item.slot_ == slot )
			{
				BagCells [i].GetComponent<BagCellUI> ().Item = null;
			}
		}
	}

	private void OnUpdateItem(COM_Item item)
	{
		if(bagTipsPane.gameObject.activeSelf)
		{
			if(bagTipsPane.GetComponent<BagTipsUI>().Item.instId_ == item.instId_)
			{
				bagTipsPane.GetComponent<BagTipsUI>().Item = item;
				if(item.isLock_)
				{
					bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(true);
					bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(true);
				}
				else
				{
					bagTipsPane.GetComponent<BagTipsUI>().suoImg.gameObject.SetActive(false);
					bagTipsPane.GetComponent<BagTipsUI>().lockBtn.gameObject.SetActive(true);
					bagTipsPane.GetComponent<BagTipsUI>().cancelLockBtn.gameObject.SetActive(false);
				}
				if(ItemData.GetData((int)item.itemId_).mainType_ == ItemMainType.IMT_FuWen)
				{
					if(!fuwenRoleObj.activeSelf)
					{
						bagTipsPane.GetComponent<BagTipsUI>().fuwenBtn.gameObject.SetActive(false);
						bagTipsPane.GetComponent<BagTipsUI>().fuwenHCBtn.gameObject.SetActive(false);
					}
				}

			}
		}
		UpdataTabBagItems ();
	}

	private void OnSortOk()
	{
		PopText.Instance.Show (LanguageManager.instance.GetValue ("zhengliwancheng"));
		UpdataTabBagItems ();
	}

	private void OnWearEquip(uint target, COM_Item equip)
	{
//		if (target != 0) 
//		{
//			return;
//		}
		NetWaitUI.HideMe ();
		PopText.Instance.Show (LanguageManager.instance.GetValue ("zhaunbgeiOk"));
		_EquipList[equip.slot_] = equip;
		EquipCells[equip.slot_].GetComponent<HeroEquipCellUI> ().Equip = equip;
		EquipCells [equip.slot_].GetComponent<HeroEquipCellUI> ().blackImg.gameObject.SetActive (false);

		if(_EquipList[(int)EquipmentSlot.ES_Fashion] != null)
		{
			if(_EquipList[(int)EquipmentSlot.ES_Fashion].usedTimeout_ > 0)
			{
				fashionTime.gameObject.SetActive(true);
				fashionTime.text = FormatTimeHasDay(_EquipList[(int)EquipmentSlot.ES_Fashion].usedTimeout_);
			}
			else
			{
				fashionTime.gameObject.SetActive(false);
			}
		}
		else
		{
			fashionTime.gameObject.SetActive(false);
		}

	}

	private void OnDelEquip(uint target, uint slot)
	{
//		if(target != 0)
//		{
//			return;
		NetWaitUI.HideMe ();
		EquipCells[slot].GetComponent<HeroEquipCellUI>().Equip = null;
		EquipCells[slot].GetComponent<HeroEquipCellUI>().blackImg.gameObject.SetActive(false);
		_EquipList[slot] = null;


		if(_EquipList[(int)EquipmentSlot.ES_Fashion] != null)
		{
			if(_EquipList[(int)EquipmentSlot.ES_Fashion].usedTimeout_ > 0)
			{
				fashionTime.gameObject.SetActive(true);
				fashionTime.text = FormatTimeHasDay(_EquipList[(int)EquipmentSlot.ES_Fashion].usedTimeout_);
			}
			else
			{
				fashionTime.gameObject.SetActive(false);
			}
		}
		else
		{
			fashionTime.gameObject.SetActive(false);
		}


	}


	private void OnOpenGridEvent()
	{
		UpdataTabBagItems ();
	}

	void DebrisEventOk(int a)
	{
		if(debrisPanel.gameObject.activeSelf)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("hechengok"));
			COM_Item item = BagSystem.instance.GetItemByInstId((int)debrisPanel.Debris.instId_);
			debrisPanel.Debris = item;
		}
	}

	void FuwenEnvet(int slot)
	{
		_FuWenList = GamePlayer.Instance.fuwens_;
		for(int i= 0;i<fuwenList.Count;i++)
		{
			fuwenList[i].GetComponent<HeroEquipCellUI>().blackImg.gameObject.SetActive(false);
			if(_FuWenList[i] != null)
			{
				fuwenList[i].gameObject.SetActive(true);
				fuwenList[i].GetComponent<HeroEquipCellUI>().Equip = _FuWenList[i];
			}
			else
			{
				fuwenList[i].GetComponent<HeroEquipCellUI>().Equip = null;
			}
		}

		SetFuwenProp ();
		ShowFuWenProp ();

	}

	public int SelectTabNum
	{
		get
		{
			return _selsctTab;
		}
		set
		{
			if(_selsctTab  != value)
			{
				_selsctTab = value;
				UpdataTabBagItems();

			}
		}
	}


	private void UpdataTabBagItems()
	{
        //_BagItems = BagSystem.instance.BagItems;
		List<COM_Item> bagItems = new List<COM_Item> ();
		if(_selectMianType > 0)
		{
			bagItems = BagSystem.instance.GetMainTypeItems ((ItemMainType)_selectMianType);
		}
		else
		{
			bagItems = new List<COM_Item>(BagSystem.instance.BagItems);
		}

		int num = SelectTabNum * 20 ;
		for (int i= 0; i<20; i++) 
		{
			BagCellUI bagCell = BagCells[i].GetComponent<BagCellUI>();
			if(bagItems.Count > num+i)
			{
				if(bagItems[num+i] == null)
				{
					bagCell.Item = null;
				}
				else
				{
					bagCell.Item = _BagItems[bagItems[num+i].slot_];
				}
			}
			else
			{
				bagCell.Item = null;
			}

			if(_BagItems[num+i] != null)
			{
				UIManager.SetButtonEventHandler (BagCells [i].GetComponent<BagCellUI> ().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);	
			}
			
			if(num+i >= GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum))  //已开启背包格子数.
			{
				BagCells[i].GetComponent<BagCellUI>().isLock = false;
				
			}
			else
			{
				BagCells[i].GetComponent<BagCellUI>().isLock = true;
			}

			if(BagCells[i].GetComponent<BagCellUI>().Item != null)
			{
				foreach(COM_Item s in sellItemList)
				{
					if(s.instId_ == BagCells[i].GetComponent<BagCellUI>().Item.instId_)
					{
						BagCells[i].GetComponent<BagCellUI>().pane.spriteName = "bb_daojukuang1";
						BagCells[i].GetComponent<BagCellUI>().itemIcon.gameObject.SetActive(false);
						BagCells[i].GetComponent<BagCellUI>().countLab.gameObject.SetActive(false);
						UIManager.RemoveButtonAllEventHandler (BagCells [i].GetComponent<BagCellUI> ().pane.gameObject);	
					}
				}

				if(GamePlayer.Instance.isInBattle)
				{
					if(ItemData.GetData((int)bagCell.Item.itemId_).usedFlag_ != ItemUseFlag.IUF_All && ItemData.GetData((int)bagCell.Item.itemId_).usedFlag_ != ItemUseFlag.IUF_Battle)
					{
						bagCell.blackImg.gameObject.SetActive(true);
					}
					else
					{
						bagCell.blackImg.gameObject.SetActive(false);
					}
				}
			}




		}

		if(sellPane.gameObject.activeSelf)
		{
			SetItemIsSell (true);
		}
	}
	
	public GameObject GetItemObj(int itemId)
	{
		foreach( var x in BagCells)
		{
			BagCellUI cell = x.GetComponent<BagCellUI>();
			if(cell.Item == null)
			{
				continue;
			}
			if(cell.Item.itemId_ == itemId)
			{
				return x;
			}
		}
		return null;
	}

	void OnHideTips()
	{
		if(compTips.gameObject.activeSelf)
		{
			compTips.gameObject.SetActive(false);
			if(role != null)
				role.gameObject.SetActive(true);
			//ApplicationEntry.Instance.ui3DCamera.depth = 1; 
		}
		if(role != null)
			role.gameObject.SetActive(true);
	}


	void OnOPenFuWen(int instId)
	{
		fuwenHeChengPanel.gameObject.SetActive (true);
		UIManager.Instance.AdjustUIDepth (fuwenHeChengPanel.transform);
		fuwenHeChengPanel.GetComponent<fuwenCompoundUI> ().Item = BagSystem.instance.GetItemByInstId (instId);
	}

	private void SetItemIsSell( bool sell)
	{

		foreach( var x in BagCells)
		{
			BagCellUI cell = x.GetComponent<BagCellUI>();
			cell.itemIcon.color = Color.white;
			if(cell.Item == null)
				continue;
			if(ItemData.GetData((int)cell.Item.itemId_).price_ <= 0 || ItemData.GetData((int)cell.Item.itemId_).mainType_ == ItemMainType.IMT_Quest)
			{
				if(sell)
				{
					cell.itemIcon.color = Color.gray;
				}
				else
				{
					cell.itemIcon.color = Color.white;
				}
			}
		}

	}

	public void AddsellCell(GameObject obj)
	{
		sellCellList.Add(obj);
		UIManager.SetButtonEventHandler (obj.GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick,OnClickSellCell, 0, 0);
	}

    void Update()
    {
        if (GamePlayer.Instance.dressChanged_)
        {
            GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, EntityType.ET_Player,
                                          AssetLoadCallBack, new ParamData(GamePlayer.Instance.InstId, GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)), "UI", GamePlayer.Instance.DressID);
			GameManager.Instance.GetActorClone((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), (ENTITY_ID)GamePlayer.Instance.WeaponAssetID, EntityType.ET_Player,
			                                   FuWenAssetLoadCallBack, new ParamData(GamePlayer.Instance.InstId, GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)), "UI", GamePlayer.Instance.DressID);

			GamePlayer.Instance.dressChanged_ = false;

        }

		if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Fashion] != null)
		{
			if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Fashion].usedTimeout_ > 0)
			{
				fashionTime.gameObject.SetActive(true);
				fashionTime.text = FormatTimeHasDay( (int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Fashion].usedTimeout_);
			}
			else
			{
				fashionTime.gameObject.SetActive(false);
			}
		}
		else
		{
			fashionTime.gameObject.SetActive(false);
		}

        if (BagSystem.instance.isDirty_)
        {
            UpdateItem();
            BagSystem.instance.isDirty_ = false;
        }
    }

	private void SetFuwenProp()
	{
		fuWenpropArr.Clear();
		for(int i=0;i<_FuWenList.Length;i++)
		{
			if(_FuWenList[i]!= null)
			{
				for(int j= 0;j<_FuWenList[i].propArr.Length;j++)
				{
					if( !fuWenpropArr.ContainsKey(_FuWenList[i].propArr[j].type_) )
					{
						COM_PropValue prop = new COM_PropValue();
						prop.type_ = _FuWenList[i].propArr[j].type_;
						prop.value_ = _FuWenList[i].propArr[j].value_;
						//fuWenpropArr[_FuWenList[i].propArr[j].type_] = _FuWenList[i].propArr[j];
						fuWenpropArr[prop.type_] = prop;
					}
					else
					{
						fuWenpropArr[_FuWenList[i].propArr[j].type_].value_ += _FuWenList[i].propArr[j].value_;
					}
				}
			}
		}

	}

	void ShowFuWenProp()
	{
		for( int o = 0;o<fuWenCellList.Count;o++)
		{
			fuWengrid.RemoveChild(fuWenCellList[o].transform);
			fuWenCellList[o].transform.parent = null;
			fuWenCellList[o].gameObject.SetActive(false);
			fuWenCellPool.Add(fuWenCellList[o]);
		}
		fuWenCellList.Clear ();

		foreach(COM_PropValue x in fuWenpropArr.Values) 
		{
			GameObject objCell = null;
			if(fuWenCellPool.Count>0)
			{
				objCell = fuWenCellPool[0];
				fuWenCellPool.Remove(objCell);  
				UIManager.RemoveButtonAllEventHandler(objCell);
			}
			else  
			{
				objCell = Object.Instantiate(fuWenpropCell) as GameObject;
			}

			string sNum = "";
			if(x.value_ > 0)
			{
				sNum = " +" + ((int)x.value_).ToString();
			}
			else
			{
				sNum =  " "+((int)x.value_).ToString();
			}

			UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>();
			lable.text  =  LanguageManager.instance.GetValue(x.type_.ToString())+sNum;

			fuWengrid.AddChild(objCell.transform);
			objCell.transform.localScale = Vector3.one;
			objCell.gameObject.SetActive(true);
			fuWenCellList.Add(objCell);

		}
		fuWengrid.Reposition();

	}


	public override void Destroyobj ()
	{
		//AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TopMoneyPanel, AssetLoader.EAssetType.ASSET_UI), true);
		//AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_BagPanel, AssetLoader.EAssetType.ASSET_UI), true);
		PlayerAsseMgr.DeleteAsset ((ENTITY_ID)GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId), false);
		GameObject.Destroy (gameObject);
	
	}
}

