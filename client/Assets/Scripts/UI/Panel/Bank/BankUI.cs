using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BankUI : UIBase {

	public UILabel _ArrangeLbale;
	public UIButton leftZLBtn;
	public UIButton rightZLBtn;
	public List<UIButton> leftTabBtns = new List<UIButton> ();
	public List<UIButton> rightTabBtns = new List<UIButton> ();

	public UIPanel	bagTipsPane;
	public Tips compTips;
	public UILabel itemNunLabel;

	public GameObject PropsbackObj;
	private GameObject babyObj;

	public GameObject leftobj;
	public GameObject rightobj;
	public UIGrid baGrid;
	public GameObject bagcell;

	public UIGrid cGrid;
	public GameObject acell;

	private GameObject[] BagCells = new GameObject[20];
	private GameObject[] ACells = new GameObject[20];
	private COM_Item[] _BagItems = new COM_Item[100];
	public static List<COM_Item> sellItemList = new List<COM_Item> ();
	private int _selsctTab;
	private int _selsctCTab;
	private bool bDouble = false;
	void Awake()
	{
		if(MainbabyListUI.babyObj != null)
		{
			MainbabyListUI.babyObj.SetActive (false);
		}

	}
	void Start () {
		_ArrangeLbale.text = LanguageManager.instance.GetValue ("bank_Arrange");
		bagcell.SetActive (false);
		acell.SetActive (false);

//		daojuBtn.isEnabled = false;
//		babyBtn.isEnabled = true;
		_BagItems = BagSystem.instance.BagItems;
		for(int i =0;i<leftTabBtns.Count;i++)
		{
			UIManager.SetButtonEventHandler (leftTabBtns[i].gameObject, EnumButtonEvent.OnClick, OnClickLeftBtn,i, 0);
		}
		for(int i =0;i<rightTabBtns.Count;i++)
		{
			UIManager.SetButtonEventHandler (rightTabBtns[i].gameObject, EnumButtonEvent.OnClick, OnClickRightBtn,i, 0);
		}
//		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);
//		UIManager.SetButtonEventHandler (daojuBtn.gameObject, EnumButtonEvent.OnClick, OnClickdaoju,0, 0);
//		UIManager.SetButtonEventHandler (babyBtn.gameObject, EnumButtonEvent.OnClick, OnClickbaby,0, 0);

		UIManager.SetButtonEventHandler (leftZLBtn.gameObject, EnumButtonEvent.OnClick, OnClicLksort,0, 0);
		UIManager.SetButtonEventHandler (rightZLBtn.gameObject, EnumButtonEvent.OnClick, OnClicRksort,0, 0);

		//bagTipsPane.GetComponent<Tips> ().closeCallback = OnHideTips;
		ShowBagItem ();
		ShowCitems ();
		RequestStorageItem ();
		BankSystem.OnSortItem += OnSortOk;
		BankSystem.OnUpdateItemOk += RequestStorageItem;
		BankSystem.OnUpdateBagItemOk += UpdateBagItemdataNum;
		BankSystem.OnUpdateStorageItemOk += OnSorttorageOk;
		BagSystem.instance.ItemChanged += UpdateBagItemdata;
		TabsLeftSelect (0);
		TabsRightSelect (0);
	}
	void RequestStorageItem()
	{
		RemoveStorageItemAllEventHandler ();
		for(int i =0;i<GamePlayer.Instance.StorageItems.Length;i++)
		{
			if(GamePlayer.Instance.StorageItems[i] != null)
			{
				BagCellUI bagCe;
				if(i<ACells.Length)
				{
					bagCe = ACells[i].GetComponent<BagCellUI>();
					UIManager.SetButtonEventHandler (ACells[i].GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickBankCell, 1, 0);					
					bagCe.Item = GamePlayer.Instance.StorageItems[i];
				}
					

			}
		}
		UpdataTabCangItems();
		itemNunLabel.text = BankSystem.instance.GetBagSize () + "/" + BankSystem.instance.itemNum;
	}
	void RemoveStorageItemAllEventHandler()
	{
		for(int i =0;i<ACells.Length;i++)
		{
			UIManager.RemoveButtonAllEventHandler (ACells[i].GetComponent<BagCellUI>().pane.gameObject);
		}
	}
	void RemoveBagItemAllEventHandler()
	{
		for(int i =0;i<ACells.Length;i++)
		{
			UIManager.RemoveButtonAllEventHandler (BagCells[i].GetComponent<BagCellUI>().pane.gameObject);
		}
	}
	void UpdateBagItemdata(COM_Item item)
	{
		RemoveBagItemAllEventHandler ();
		_BagItems = BagSystem.instance.BagItems;
		for(int i =0;i<_BagItems.Length;i++)
		{
			if(_BagItems[i]!=null)
			{
				if(i<BagCells.Length)
				{
					BagCellUI bcell = BagCells[i].GetComponent<BagCellUI>();
					bcell.Item = _BagItems[i];
					UIManager.SetButtonEventHandler (BagCells[i].GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, (int)_BagItems[i].itemId_, 0);
				}

			}

		}

		UpdataTabBagItems();

	}
	void UpdateBagItemdataNum()
	{
		itemNunLabel.text = BankSystem.instance.GetBagSize () + "/" + BankSystem.instance.itemNum;
	}
	void ClearStorageItems()
	{
		for(int i =0;i<GamePlayer.Instance.StorageItems.Length;i++)
		{
			GamePlayer.Instance.StorageItems[i] = null;
		}
	}

	private void OnSorttorageOk()
	{
		//ClearStorageItems ();
//		for(int i =0;i<citem.Count;i++)
//		{
//			//citem[i].slot_ = (ushort)i;
//			GamePlayer.Instance.StorageItems[citem[i].slot_] = citem[i];
//		}


		RemoveStorageItemAllEventHandler ();
		int num = _selsctCTabNum * 20 ;
		for (int i= 0; i<20; i++) 
		{
			ACells[i].GetComponent<BagCellUI>().Item = GamePlayer.Instance.StorageItems[num+i];
			
			if(GamePlayer.Instance.StorageItems[num+i] != null)
			{
				UIManager.SetButtonEventHandler (ACells [i].GetComponent<BagCellUI> ().pane.gameObject, EnumButtonEvent.OnClick, OnClickBankCell, 0, 0);	
			}			
		}
		itemNunLabel.text = BankSystem.instance.GetBagSize () + "/" + BankSystem.instance.itemNum;
	}

	void ShowCitems()
	{
		for(int i=0; i < ACells.Length;i++)
		{
			GameObject obj = Object.Instantiate(acell) as GameObject;
			obj.SetActive(true);
			obj.name = "agCell";
			cGrid.AddChild(obj.transform);
			ACells[i] = obj;
			obj.transform.localScale = Vector3.one;
			UIEventListener.Get(obj.GetComponent<BagCellUI>().pane.gameObject).onDoubleClick = OnCellDoubleToBagClick;
		}
	}


	void OnHideTips()
	{
		if(compTips.gameObject.activeSelf)
		{
			compTips.gameObject.SetActive(false);

		}
	}
	private void OnSortOk()
	{
		_BagItems = BagSystem.instance.BagItems;
		int num = SelectTabNum * 20 ;
		for (int i= 0; i<20; i++) 
		{
			BagCells[i].GetComponent<BagCellUI>().Item = _BagItems[num+i];
			
			if(_BagItems[num+i] != null)
			{
				UIManager.SetButtonEventHandler (BagCells [i].GetComponent<BagCellUI> ().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);	
			}			
		}
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
	public int _selsctCTabNum
	{
		get
		{
			return _selsctCTab;
		}
		set
		{
			if(value != _selsctCTab)
			{
				_selsctCTab = value;
				UpdataTabCangItems();
			}
		}
	}


	private void UpdataTabCangItems()
	{
		RemoveStorageItemAllEventHandler ();
		int num = _selsctCTabNum * 20 ;
		for (int i= 0; i<20; i++) 
		{
			BagCellUI bagCell = ACells[i].GetComponent<BagCellUI>();
			bagCell.Item = GamePlayer.Instance.StorageItems[num+i];
			
			if(ACells[i].GetComponent<BagCellUI>().Item != null)
			{
//				foreach(COM_Item s in sellItemList)
//				{
//					if(s.instId_ == ACells[i].GetComponent<BagCellUI>().Item.instId_)
//					{
//						ACells[i].GetComponent<BagCellUI>().itemIcon.gameObject.SetActive(false);
//						ACells[i].GetComponent<BagCellUI>().countLab.gameObject.SetActive(false);
//						//ACells[i].GetComponent<BagCellUI>().debirsImg.gameObject.SetActive(false);
//						UIManager.RemoveButtonAllEventHandler (ACells [i].GetComponent<BagCellUI> ().pane.gameObject);	
//					}
//				}
				
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
			
			
			UIManager.SetButtonEventHandler (ACells [i].GetComponent<BagCellUI> ().pane.gameObject, EnumButtonEvent.OnClick, OnClickBankCell, 0, 0);	
			int storNum = BankSystem.instance.itemNum;
			if(num+i >=storNum)  //已开启背包格子数.
			{
				ACells[i].GetComponent<BagCellUI>().isLock = false;
				UIManager.SetButtonParam(ACells [i].GetComponent<BagCellUI> ().pane.gameObject,1,0);
			}
			else
			{
				ACells[i].GetComponent<BagCellUI>().isLock = true;
				if(GamePlayer.Instance.StorageItems[num+i] != null)
				{
					UIManager.SetButtonParam(ACells [i].GetComponent<BagCellUI> ().pane.gameObject,0,0);
				}else
				{
					UIManager.SetButtonParam(ACells [i].GetComponent<BagCellUI> ().pane.gameObject,2,0);
				}

			}

		}
	}







	private void UpdataTabBagItems()
	{
		int num = SelectTabNum * 20 ;
		for (int i= 0; i<20; i++) 
		{
			BagCellUI bagCell = BagCells[i].GetComponent<BagCellUI>();
			bagCell.Item = _BagItems[num+i];
			
			if(BagCells[i].GetComponent<BagCellUI>().Item != null)
			{
//				foreach(COM_Item s in sellItemList)
//				{
//					if(s.instId_ == BagCells[i].GetComponent<BagCellUI>().Item.instId_)
//					{
//						BagCells[i].GetComponent<BagCellUI>().itemIcon.gameObject.SetActive(false);
//						BagCells[i].GetComponent<BagCellUI>().countLab.gameObject.SetActive(false);
//						UIManager.RemoveButtonAllEventHandler (BagCells [i].GetComponent<BagCellUI> ().pane.gameObject);	
//					}
//				}
				
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
			
			
			if(_BagItems[num+i] != null)
			{
				UIManager.SetButtonEventHandler (BagCells [i].GetComponent<BagCellUI> ().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);	
			}
			int bumbag = GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum);
			if(num+i >= bumbag)  //已开启背包格子数.
			{
				BagCells[i].GetComponent<BagCellUI>().isLock = false;
				
			}
			else
			{
				BagCells[i].GetComponent<BagCellUI>().isLock = true;
			}
		}
		
//		if(sellPane.gameObject.activeSelf)
//		{
//			SetItemIsSell (true);
//		}
	}
	void OnClicLksort(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.sortStorage (StorageType.ST_Item);	
	}
	void OnClicRksort(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.sortBagItem ();	
	}
//	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
//	{
//		Hide ();
//		//NetConnection.Instance.sortBagItem ();	
//	}
//	void OnClickdaoju(ButtonScript obj, object args, int param1, int param2)
//	{
//		daojuBtn.isEnabled = false;
//		babyBtn.isEnabled = true;
//		PropsbackObj.SetActive (true);
//		if(babyObj != null)
//		babyObj.SetActive (false);
//	}
//	void OnClickbaby(ButtonScript obj, object args, int param1, int param2)
//	{
//		daojuBtn.isEnabled = true;
//		babyBtn.isEnabled = false;
//		PropsbackObj.SetActive (false);
//		if(babyObj != null)
//		{
//			babyObj.SetActive(true);
//		}else
//		{
//			LoadUI (UIASSETS_ID.UIASSETS_babyBankPanel);
//		}
//	}
	void OnClickLeftBtn(ButtonScript obj, object args, int param1, int param2)
	{
		//int num = BankSystem.instance.itemNum / ACells.Length;
//		if(param1<num)
//		{
			TabsLeftSelect (param1);
//		}else
//		{
//            PopText.Instance.Show(LanguageManager.instance.GetValue("useStorePaper"));
//		}
	
	}
	void OnClickRightBtn(ButtonScript obj, object args, int param1, int param2)
	{
		TabsRightSelect (param1);
	}
//	string subUiResPath;
//	bool hasDestroy = false;
//	GameObject panObj;
//	private void LoadUI(UIASSETS_ID id)
//	{
//		subUiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);
//		
//		AssetLoader.LoadAssetBundle(subUiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
//		                            {
//			if (hasDestroy)
//			{
//				AssetInfoMgr.Instance.DecRefCount(subUiResPath, true);
//				return;
//			}
//			if( null == Assets || null == Assets.mainAsset )
//			{
//				return ;
//			}			
//			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
//			if(this == null && !gameObject.activeSelf )
//			{
//				Destroy(go);
//			}
//
//			go.transform.parent = this.panel.transform;    
//			go.transform.localPosition = Vector3.zero;
//			go.transform.localScale = Vector3.one;
//			babyObj = go;
//			if(babyBtn.isEnabled)
//			{
//				go.SetActive(false);	
//			}
//
//		}
//		, null);
//	}
	void ShowBagItem()
	{
		//RemoveBagItemAllEventHandler ();
		for(int i=0; i < BagCells.Length;i++)
		{
			GameObject obj = Object.Instantiate(bagcell) as GameObject;
			obj.SetActive(true);

			UIEventListener.Get(obj.GetComponent<BagCellUI>().pane.gameObject).onDoubleClick = OnCellDoubleClick;
			obj.GetComponent<BagCellUI>().Item = _BagItems[i];
			if(_BagItems[i] != null)
			{
				UIManager.SetButtonEventHandler (obj.GetComponent<BagCellUI>().pane.gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);
			}
			baGrid.AddChild(obj.transform);
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
		ItemData idata = ItemData.GetData ((int)Item.itemId_);
		if(idata.mainType_ == ItemMainType.IMT_Quest)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("renwudaoju"));
			return;
		}
		if(BankSystem.instance.IsStorageFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("cankuman"));
			return ;
		}
		if(IsItemTypeIST_PVP((int)Item.itemId_))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("IST_PVP"));
			return;
		}

//		if(ItemData.GetData((int)Item.itemId_).price_ <= 0 || ItemData.GetData((int)Item.itemId_).mainType_ == ItemMainType.IMT_Quest)
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("cantsell"));
//			return;
//		}
		
//		if(sellItemList.Count>= 16)
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("sellMax"));
//			return;
//		}
		
//		foreach(COM_Item i in sellItemList)
//		{
//			if(i.instId_ == Item.instId_)
//			{
//				return ;
//			}
//		}
		
		GameObject sellObj = null;		
		obj.GetComponentInParent<BagCellUI> ().itemIcon.gameObject.SetActive(false);
		obj.GetComponentInParent<BagCellUI> ().countLab.gameObject.SetActive(false);
		//obj.GetComponentInParent<BagCellUI> ().debirsImg.gameObject.SetActive (false);
		obj.GetComponentInParent<BagCellUI> ().pane.spriteName = "bb_daojukuang1";
		UIManager.RemoveButtonAllEventHandler ( obj.GetComponentInParent<BagCellUI> ().pane.gameObject);
		NetConnection.Instance.depositItemToStorage (Item.instId_);

	}

	void OnCellDoubleToBagClick(GameObject obj)
	{
		bDouble = true;
		StopCoroutine ("DelayOneClick");
		COM_Item Item = obj.GetComponentInParent<BagCellUI> ().Item;
		
		if(Item == null)
		{
			return;
		}
		if(BagSystem.instance.BagIsFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));

			return;
		}
//		if(ItemData.GetData((int)Item.itemId_).price_ <= 0 || ItemData.GetData((int)Item.itemId_).mainType_ == ItemMainType.IMT_Quest)
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("cantsell"));
//			return;
//		}
		
		GameObject sellObj = null;		
		obj.GetComponentInParent<BagCellUI> ().itemIcon.gameObject.SetActive(false);
		obj.GetComponentInParent<BagCellUI> ().countLab.gameObject.SetActive(false);
		//obj.GetComponentInParent<BagCellUI> ().debirsImg.gameObject.SetActive (false);
		obj.GetComponentInParent<BagCellUI> ().pane.spriteName = "bb_daojukuang1";
		UIManager.RemoveButtonAllEventHandler ( obj.GetComponentInParent<BagCellUI> ().pane.gameObject);
		NetConnection.Instance.storageItemToBag(Item.instId_);
	}

	bool IsItemTypeIST_PVP(int itemid)
	{
		ItemData itemd = ItemData.GetData (itemid);
		if (itemd == null)
						return false;
		if(itemd.subType_ == ItemSubType.IST_PVP)
		{
			return true;
		}
		return false;
	}
	private void OnClickBankCell(ButtonScript obj, object args, int param1, int param2)
	{
		if(param1 == 2)
		{
			return;	
		}
		if(param1 == 1)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("useStorePaper"));
		} else
		{
			BagCellUI bCell = obj.GetComponentInParent<BagCellUI> ();
			if(BagSystem.instance.BagIsFull())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
				return;
			}
			if(IsItemTypeIST_PVP((int)bCell.Item.itemId_))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("IST_PVP"));
				return;
			}
			bDouble = false;
			StartCoroutine (DelayOneClick (bCell));
		}

	}
	private void OnClickCell(ButtonScript obj, object args, int param1, int param2)
	{
		BagCellUI bCell = obj.GetComponentInParent<BagCellUI> ();
		if(BankSystem.instance.IsStorageFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("cankuman"));
			return ;
		}
//		if(BagSystem.instance.BagIsFull())
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
//			return;
//		}
		if(IsItemTypeIST_PVP((int)bCell.Item.itemId_))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("IST_PVP"));
			return;
		}
//		if(!bCell.isLock)
//		{
//			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("buyitemopenbag").Replace("{n}",ItemData.GetData(5001).name_),()=>{StoreUI.SwithShowMe();});
//			return;
//		}
//		if(bCell.Item == null)
//		{
//			return;
//		}
		
		bDouble = false;
		StartCoroutine (DelayOneClick (bCell));
	}
	IEnumerator DelayOneClick(BagCellUI bCell)
	{
		yield return new WaitForSeconds(0.2f);
		
		if(!bDouble)
		{	if(bCell.gameObject.name=="agCell")
			{
				bagTipsPane.transform.position = leftobj.transform.position;
				bagTipsPane.GetComponent<Tips>().isleft = true;
				//bagTipsPane.GetComponent<Tips>().sellBtn.GetComponentInChildren<UILabel>().text = "取出";
			}else
			{
				bagTipsPane.transform.position = rightobj.transform.position;
				bagTipsPane.GetComponent<Tips>().isleft = false;
				//bagTipsPane.GetComponent<Tips>().sellBtn.GetComponentInChildren<UILabel>().text = "存入";
			}
			//bCell.pane.spriteName = "bb_daojukuang1";
			bagTipsPane.GetComponent<Tips>().bagCell = bCell;
			bagTipsPane.gameObject.SetActive (true);
			bagTipsPane.GetComponent<Tips>().tipsImg.gameObject.SetActive(true);
			bagTipsPane.GetComponent<Tips>().PlayerInstId = (uint)GamePlayer.Instance.InstId;
			bagTipsPane.GetComponent<Tips>().Item = bCell.Item; 
		}
	}
	void TabsLeftSelect(int index)
	{
		_selsctCTabNum = index;
		for (int i = 0; i<leftTabBtns.Count; i++) 
		{
			if(i==index)
			{
				leftTabBtns[i].isEnabled = false;

			}
			else
			{
				leftTabBtns[i].isEnabled = true;
			}
		}
	}

	void TabsRightSelect(int index)
	{
		for (int i = 0; i<rightTabBtns.Count; i++) 
		{
			if(i==index)
			{
				rightTabBtns[i].isEnabled = false;
				SelectTabNum = i;
			}
			else
			{
				rightTabBtns[i].isEnabled = true;
			}
		}
	}


	void OnDestroy()
	{
		BankSystem.OnSortItem -= OnSortOk;
		BankSystem.OnUpdateItemOk -= RequestStorageItem;
		BankSystem.OnUpdateBagItemOk -= UpdateBagItemdataNum;
		BankSystem.OnUpdateStorageItemOk -= OnSorttorageOk;
		BagSystem.instance.ItemChanged -= UpdateBagItemdata;

	}
	public override void Destroyobj ()
	{


	}
}
