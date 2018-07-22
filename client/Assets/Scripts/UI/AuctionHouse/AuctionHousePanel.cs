using UnityEngine;
using System.Collections.Generic;

public class AuctionHousePanel : UIBase {

    // buygroup
    public GameObject buyGroup_;

    public GameObject hasItemPage_;
    public GameObject nothingPage_;

    public UIButton buyTab_;

    public UIInput searchPrex_;
    public UIButton searchBtn_;

    public UIGrid typeListGrid_;
    public GameObject mainItem_;
    public GameObject collectItem_;
    public GameObject subItem_; // contained baby and item.

    public UIGrid sellListGrid_;
    public GameObject sellItem_;

    public UILabel diamond_;

    public UIButton collectBtn_;

    public UIButton buyBtn_;

    public UIButton pageUpBtn_;

    public UIButton pageDownBtn_;

    public UILabel pageInfo_;

    // sellgroup
    public GameObject sellGroup_;

    public UIButton sellTab_;

    public UIButton sellRecordBtn_;

    public UIGrid mySellingListGrid_;
    public GameObject mySellingItem_;

    public UIButton itemSubTab_;
    public GameObject itemSubContent_;
    public GameObject itemsInBagPre_;
    public UIGrid itemsInBagGrid;
    int itemSubTab_CrtPage_;

    public UIButton babySubTab_;
    public GameObject babySubContent_;
    public GameObject babiesInBagPre_;
    public UIGrid babiesInBagGrid;
    int babySubTab_CrtPage_;

    COM_SellItem crtSelectSellItem_;

    public SellConfirmBaby sellConfirmBabyPanel_;
    public SellConfirmItem sellConfirmItemPanel_;
    public Sellrecord sellRecordPanel_;

    public UILabel mySellingCount_;

    // commongroup
    public UIButton closeBtn_;

    public UIScrollView scroView;
    UIPanel listPanel_;
    BoxCollider listDragArea_;

    DisplayType crtDisplayType_ = DisplayType.DT_Buy;

    enum DisplayType
    {
        DT_Sell,
        DT_Buy,
    }

    enum BagType
    {
        BT_Item,
        BT_Baby,
    }

    enum TypeType
    {
        TT_Collection,
        TT_Equip,
        TT_Baby,
        TT_Item,
    }

    GameObject collectionTab_, equipTab_, babyTab_, itemTab_;
    bool[] typeFlag = { false, false, false, false };

	// Use this for initialization
	void Start () {

        listPanel_ = scroView.gameObject.GetComponent<UIPanel>();
        listDragArea_ = scroView.gameObject.GetComponent<BoxCollider>();

        AuctionHouseSystem.OnSellListUpdate += OnSellListUpdate;
        AuctionHouseSystem.OnMySellingListUpdate += OnMySellingUpdate;
        GamePlayer.Instance.OnBabyUpdate += UpdateSubContent;
        GamePlayer.Instance.OnIPropUpdate += OnDiamondUpdate;

        UIManager.SetButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
        UIManager.SetButtonEventHandler(searchBtn_.gameObject, EnumButtonEvent.OnClick, OnSearch, 0, 0);
        UIManager.SetButtonEventHandler(buyBtn_.gameObject, EnumButtonEvent.OnClick, OnBuy, 0, 0);
        UIManager.SetButtonEventHandler(pageUpBtn_.gameObject, EnumButtonEvent.OnClick, OnPageUpdate, -1, 0);
        UIManager.SetButtonEventHandler(pageDownBtn_.gameObject, EnumButtonEvent.OnClick, OnPageUpdate, 1, 0);
        UIManager.SetButtonEventHandler(buyTab_.gameObject, EnumButtonEvent.OnClick, OnDisplay, (int)DisplayType.DT_Buy, 0);
        UIManager.SetButtonEventHandler(sellTab_.gameObject, EnumButtonEvent.OnClick, OnDisplay, (int)DisplayType.DT_Sell, 0);
        UIManager.SetButtonEventHandler(collectBtn_.gameObject, EnumButtonEvent.OnClick, OnCollect, 0, 0);
        UIManager.SetButtonEventHandler(sellRecordBtn_.gameObject, EnumButtonEvent.OnClick, OnRecord, 0, 0);
        UIManager.SetButtonEventHandler(itemSubTab_.gameObject, EnumButtonEvent.OnClick, OnBagTab, (int)BagType.BT_Item, itemSubTab_CrtPage_);
        UIManager.SetButtonEventHandler(babySubTab_.gameObject, EnumButtonEvent.OnClick, OnBagTab, (int)BagType.BT_Baby, babySubTab_CrtPage_);
        
		OpenPanelAnimator.PlayOpenAnimation (this.panel, () => {

        SelectDisplayGroup(DisplayType.DT_Buy);
        DisplaySellTab();

        OnDiamondUpdate();

        //selectedItemSubTabBtnForIndex(itemSubTab_CrtPage_);
		//selectedbabySubTabBtnForIndex (babySubTab_CrtPage_);
		});
    }

    void LaunchContext(string searchStr = "", ItemSubType searchItem = ItemSubType.IST_None, RaceType searchBaby = RaceType.RT_None, int itemId = 0, int babyId = 0)
    {
        searchContext_.title_ = searchStr;
        searchContext_.ist_ = searchItem;
        searchContext_.rt_ = searchBaby;
        searchContext_.itemId_ = itemId;
        searchContext_.babyId_ = babyId;
        searchContext_.begin_ = AuctionHouseSystem.currentStartIndex_;
        searchContext_.limit_ = AuctionHouseSystem.CountPerPage;
    }

    void OnDiamondUpdate()
    {
        diamond_.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond).ToString();
    }

    void SelectDisplayGroup(DisplayType type)
    {
        crtDisplayType_ = type;
        buyGroup_.SetActive(type == DisplayType.DT_Buy);
        sellGroup_.SetActive(type == DisplayType.DT_Sell);

        string buyTabName = type == DisplayType.DT_Buy ? "hb_qieyefu_liang" : "hb_qieyefu_an";
        string sellTabName = type == DisplayType.DT_Sell ? "hb_qieyefu_liang" : "hb_qieyefu_an";

        buyTab_.GetComponentInChildren<UISprite>().spriteName = buyTabName;
        sellTab_.GetComponentInChildren<UISprite>().spriteName = sellTabName;

        if (type == DisplayType.DT_Buy)
        {
            string coll = AuctionHouseSystem.GetFirstCollection();
            if (!string.IsNullOrEmpty(coll))
            {
                string[] infos = coll.Split('|');
                int id = int.Parse(infos[0]);
                bool isItem = bool.Parse(infos[1]);
                if (isItem)
                    LaunchContext("", ItemSubType.IST_None, RaceType.RT_None, id);
                else
                    LaunchContext("", ItemSubType.IST_None, RaceType.RT_None, 0, id);
            }
            else
                LaunchContext();
            NetConnection.Instance.fetchSelling(searchContext_);
        }
        else
        {
            OnMySellingUpdate();
            OnBagTab(null, null, (int)type, 0);
        }
    }

    void DisplaySellTab()
    {
        // 收藏夹
        if (collectionTab_ == null)
        {
            collectionTab_ = GameObject.Instantiate(mainItem_) as GameObject;
            collectionTab_.transform.parent = typeListGrid_.transform;
            collectionTab_.transform.localScale = Vector3.one;
			collectionTab_.GetComponent<MainGood>().SetData(LanguageManager.instance.GetValue("shouchangjian"));
            UIManager.SetButtonEventHandler(collectionTab_.gameObject, EnumButtonEvent.OnClick, OnClickTypeBtn, (int)TypeType.TT_Collection, 0);
        }
        collectionTab_.SetActive(true);

        // 装备
        if (equipTab_ == null)
        {
            equipTab_ = GameObject.Instantiate(mainItem_) as GameObject;
            equipTab_.transform.parent = typeListGrid_.transform;
            equipTab_.transform.localScale = Vector3.one;
			equipTab_.GetComponent<MainGood>().SetData(LanguageManager.instance.GetValue("IMT_Equip"));
            UIManager.SetButtonEventHandler(equipTab_.gameObject, EnumButtonEvent.OnClick, OnClickTypeBtn, (int)TypeType.TT_Equip, 0);
        }
        equipTab_.SetActive(true);

        // 宠物
        if (babyTab_ == null)
        {
            babyTab_ = GameObject.Instantiate(mainItem_) as GameObject;
            babyTab_.transform.parent = typeListGrid_.transform;
            babyTab_.transform.localScale = Vector3.one;
			babyTab_.GetComponent<MainGood>().SetData(LanguageManager.instance.GetValue("mainbaby_tit"));
            UIManager.SetButtonEventHandler(babyTab_.gameObject, EnumButtonEvent.OnClick, OnClickTypeBtn, (int)TypeType.TT_Baby, 0);
        }
        babyTab_.SetActive(true);

        // 消耗品
        if (itemTab_ == null)
        {
            itemTab_ = GameObject.Instantiate(mainItem_) as GameObject;
            itemTab_.transform.parent = typeListGrid_.transform;
            itemTab_.transform.localScale = Vector3.one;
			itemTab_.GetComponent<MainGood>().SetData(LanguageManager.instance.GetValue("IST_Consumables"));
            UIManager.SetButtonEventHandler(itemTab_.gameObject, EnumButtonEvent.OnClick, OnClickTypeBtn, (int)TypeType.TT_Item, 0);
        }
        itemTab_.SetActive(true);

        typeListGrid_.repositionNow = true;
    }

    /*
     * 1.收藏
     * 2.装备
     * 3.宠物
     * 4.消耗品
     */

    GameObject[] collectItemCache = new GameObject[AuctionHouseSystem.CollectionMax];
    GameObject[] equipItemCache = new GameObject[(int)ItemSubType.IST_EquipMax - 1];
    GameObject[] babyItemCache = new GameObject[(int)RaceType.RT_Max - 1];
    GameObject[] consumItemCache = new GameObject[(int)ItemSubType.IST_ConsEnd - (int)ItemSubType.IST_ConsBegin - 1];
    void OnClickTypeBtn(ButtonScript obj, object args, int param1, int param2)
    {
        switch((TypeType)param1)
        {
            case TypeType.TT_Collection:
                int index = typeListGrid_.GetIndex(obj.transform);
                if (typeFlag[(int)TypeType.TT_Collection] && param2 != 1)
                {
                    for (int i = 0; i < collectItemCache.Length; ++i)
                    {
                        if (collectItemCache[i] != null)
                        {
                            //typeListGrid_.RemoveChild(index + 1);
							typeListGrid_.RemoveChild(collectItemCache[i].transform);
                            UIManager.RemoveButtonEventHandler(collectItemCache[i], EnumButtonEvent.OnClick);
                            collectItemCache[i].SetActive(false);
                        }
                    }
                    typeFlag[(int)TypeType.TT_Collection] = false;
                }
                else
                {
                    GameObject colItem = null;
                    for (int i = 0; i < AuctionHouseSystem.CollectionMax; ++i)
                    {
                        if (i < AuctionHouseSystem.collectionList_.Count)
                        {
                            string[] infos = AuctionHouseSystem.collectionList_[i].Split('|');
                            int id = int.Parse(infos[0]);
                            int isItem = bool.Parse(infos[1]) == true? 1: 0;
                            if (collectItemCache[i] == null)
                            {
                                colItem = GameObject.Instantiate(collectItem_) as GameObject;
								typeListGrid_.AddChild(colItem.transform, index + 1 + i);
								colItem.SetActive(true);
								colItem.transform.localScale = Vector3.one;
                                colItem.GetComponent<CollectGood>().SetData(AuctionHouseSystem.collectionList_[i]);
                                UIManager.SetButtonEventHandler(colItem, EnumButtonEvent.OnClick, OnCollectionSearch, id, isItem);
								UIManager.SetButtonEventHandler(colItem.GetComponent<CollectGood>().cancelCollectBtn_.gameObject, EnumButtonEvent.OnClick, OnUnCollect, i, isItem);

                                
                                collectItemCache[i] = colItem;
                            }
                            else
                            {
                                typeListGrid_.AddChild(collectItemCache[i].transform, index + 1 + i);
								collectItemCache[i].SetActive(true);
								collectItemCache[i].GetComponent<CollectGood>().SetData(AuctionHouseSystem.collectionList_[i]);
							UIManager.SetButtonEventHandler(collectItemCache[i].GetComponent<CollectGood>().cancelCollectBtn_.gameObject, EnumButtonEvent.OnClick, OnUnCollect, i, isItem);
                                UIManager.SetButtonEventHandler(collectItemCache[i], EnumButtonEvent.OnClick, OnCollectionSearch, id, isItem);
                                
                            }
                        }
                        else
                        {
                            if (collectItemCache[i] != null)
                            {
                                UIManager.RemoveButtonEventHandler(collectItemCache[i], EnumButtonEvent.OnClick);
                                collectItemCache[i].SetActive(false);
                            }
                        }
                        
                    }
                    typeFlag[(int)TypeType.TT_Collection] = true;
                }
                //typeListGrid_.Reposition();
                obj.GetComponent<MainGood>().SetTriangle(typeFlag[(int)TypeType.TT_Collection]);
                break;
            case TypeType.TT_Equip:
                index = typeListGrid_.GetIndex(obj.transform);
                if (typeFlag[(int)TypeType.TT_Equip])
                {
                    for (int i = 0; i < equipItemCache.Length; ++i)
                    {
                        if (equipItemCache[i] != null)
                        {
                            typeListGrid_.RemoveChild(index + 1);
                            UIManager.RemoveButtonEventHandler(equipItemCache[i], EnumButtonEvent.OnClick);
                            equipItemCache[i].SetActive(false);
                        }
                    }
                    typeFlag[(int)TypeType.TT_Equip] = false;
                }
                else
                {
                    GameObject subItem = null;
                    for (int i = (int)ItemSubType.IST_None; i < (int)ItemSubType.IST_EquipMax - 1; ++i)
                    {
                        if (equipItemCache[i] == null)
                        {
                            subItem = GameObject.Instantiate(subItem_) as GameObject;
                            typeListGrid_.AddChild(subItem.transform, index + 1 + i);
                            subItem.transform.localScale = Vector3.one;
                            SubGood sg = subItem.GetComponent<SubGood>();
                            sg.SetType(SubGood.GoodType.GT_Item);
                            sg.SetData(LanguageManager.instance.GetValue(((ItemSubType)i + 1).ToString()));
                            UIManager.SetButtonEventHandler(subItem, EnumButtonEvent.OnClick, OnSubTypeSearch, i + 1, 1);
                            subItem.SetActive(true);
                            equipItemCache[i] = subItem;
                        }
                        else
                        {
                            typeListGrid_.AddChild(equipItemCache[i].transform, index + 1 + i);
                            UIManager.SetButtonEventHandler(equipItemCache[i], EnumButtonEvent.OnClick, OnSubTypeSearch, i + 1, 1);
                            equipItemCache[i].SetActive(true);
                        }
                    }
                    typeFlag[(int)TypeType.TT_Equip] = true;
                }
                typeListGrid_.Reposition();
                obj.GetComponent<MainGood>().SetTriangle(typeFlag[(int)TypeType.TT_Equip]);
                break;
            case TypeType.TT_Baby:
                index = typeListGrid_.GetIndex(obj.transform);
                if (typeFlag[(int)TypeType.TT_Baby])
                {
                    for (int i = 0; i < babyItemCache.Length; ++i)
                    {
                        if (babyItemCache[i] != null)
                        {
                            typeListGrid_.RemoveChild(index + 1);
                            UIManager.RemoveButtonEventHandler(babyItemCache[i], EnumButtonEvent.OnClick);
                            babyItemCache[i].SetActive(false);
                        }
                    }
                    typeFlag[(int)TypeType.TT_Baby] = false;
                }
                else
                {
                    GameObject subItem = null;
                    for (int i = (int)RaceType.RT_None; i < (int)RaceType.RT_Max - 1; ++i)
                    {
                        if (babyItemCache[i] == null)
                        {
                            subItem = GameObject.Instantiate(subItem_) as GameObject;
                            typeListGrid_.AddChild(subItem.transform, index + 1 + i);
                            subItem.transform.localScale = Vector3.one;
                            SubGood sg = subItem.GetComponent<SubGood>();
                            sg.SetType(SubGood.GoodType.GT_Baby);
						    string nameType = ((RaceType)i + 1).ToString();
                            sg.SetData(LanguageManager.instance.GetValue(((RaceType)i + 1).ToString()), BabyData.GetRaceIconByType((RaceType)i + 1));
                            UIManager.SetButtonEventHandler(subItem, EnumButtonEvent.OnClick, OnSubTypeSearch, i + 1, 0);
                            subItem.SetActive(true);
                            babyItemCache[i] = subItem;
						if(RaceType.RT_Human.ToString().Equals(nameType)||RaceType.RT_Metal.ToString().Equals(nameType))
						{
							sg.dikuangSp.spriteName = "webzudidilan";
						}
						if(RaceType.RT_Insect.ToString().Equals(nameType)||RaceType.RT_Animal.ToString().Equals(nameType))
						{
							sg.dikuangSp.spriteName = "webzudihuang";
						}
						
						if(RaceType.RT_Dragon.ToString().Equals(nameType)||RaceType.RT_Extra.ToString().Equals(nameType)||RaceType.RT_Undead.ToString().Equals(nameType))
						{
							sg.dikuangSp.spriteName = "webzudihong";
						}
						if(RaceType.RT_Fly.ToString().Equals(nameType)||RaceType.RT_Plant.ToString().Equals(nameType))
						{
							sg.dikuangSp.spriteName = "webzudilv";
						}
                        }
                        else
                        {
                            typeListGrid_.AddChild(babyItemCache[i].transform, index + 1 + i);
                            UIManager.SetButtonEventHandler(babyItemCache[i], EnumButtonEvent.OnClick, OnSubTypeSearch, i + 1, 0);
                            babyItemCache[i].SetActive(true);
                        }
                    }
                    typeFlag[(int)TypeType.TT_Baby] = true;
                }
                typeListGrid_.Reposition();
                obj.GetComponent<MainGood>().SetTriangle(typeFlag[(int)TypeType.TT_Baby]);
                break;
            case TypeType.TT_Item:
                index = typeListGrid_.GetIndex(obj.transform);
                if (typeFlag[(int)TypeType.TT_Item])
                {
                    for (int i = 0; i < consumItemCache.Length; ++i)
                    {
                        if (consumItemCache[i] != null)
                        {
                            typeListGrid_.RemoveChild(index + 1);
                            UIManager.RemoveButtonEventHandler(consumItemCache[i], EnumButtonEvent.OnClick);
                            consumItemCache[i].SetActive(false);
                        }
                    }
                    typeFlag[(int)TypeType.TT_Item] = false;
                }
                else
                {
                    GameObject subItem = null;
                    for (int i = (int)ItemSubType.IST_None; i < (int)ItemSubType.IST_ConsEnd - (int)ItemSubType.IST_ConsBegin - 1; ++i)
                    {
                        if (consumItemCache[i] == null)
                        {
                            subItem = GameObject.Instantiate(subItem_) as GameObject;
                            typeListGrid_.AddChild(subItem.transform, index + 1 + i);
                            subItem.transform.localScale = Vector3.one;
                            SubGood sg = subItem.GetComponent<SubGood>();
                            sg.SetType(SubGood.GoodType.GT_Item);
                            sg.SetData(LanguageManager.instance.GetValue(((ItemSubType)i + (int)ItemSubType.IST_ConsBegin + 1).ToString()));
                            UIManager.SetButtonEventHandler(subItem, EnumButtonEvent.OnClick, OnSubTypeSearch, i + (int)ItemSubType.IST_ConsBegin + 1, 1);
                            subItem.SetActive(true);
                            consumItemCache[i] = subItem;
                        }
                        else
                        {
                            typeListGrid_.AddChild(consumItemCache[i].transform, index + 1 + i);
                            UIManager.SetButtonEventHandler(consumItemCache[i], EnumButtonEvent.OnClick, OnSubTypeSearch, i + (int)ItemSubType.IST_ConsBegin + 1, 1);
                            consumItemCache[i].SetActive(true);
                        }
                    }
                    typeFlag[(int)TypeType.TT_Item] = true;
                }
                typeListGrid_.Reposition();
                obj.GetComponent<MainGood>().SetTriangle(typeFlag[(int)TypeType.TT_Item]);
                break;
            default:
                break;
        }
    }



	private void OnUnCollect(ButtonScript obj, object args, int param1, int param2)
	{
		AuctionHouseSystem.RemoveCollection(param1);

		int index = 0;//typeListGrid_.GetIndex(obj.transform);
			for (int i = 0; i < collectItemCache.Length; ++i)
			{
				if (collectItemCache[i] != null)
				{
					typeListGrid_.RemoveChild(collectItemCache[i].transform);
					collectItemCache[i].transform.parent = null;
					UIManager.RemoveButtonEventHandler(collectItemCache[i], EnumButtonEvent.OnClick);
					collectItemCache[i].SetActive(false);
				}
			}
			GameObject colItem = null;
			for (int i = 0; i < AuctionHouseSystem.CollectionMax; ++i)
			{
				if (i < AuctionHouseSystem.collectionList_.Count)
				{
					string[] infos = AuctionHouseSystem.collectionList_[i].Split('|');
					int id = int.Parse(infos[0]);
					int isItem = bool.Parse(infos[1]) == true? 1: 0;
					if (collectItemCache[i] == null)
					{
						colItem = GameObject.Instantiate(collectItem_) as GameObject;
						typeListGrid_.AddChild(colItem.transform, index + 1 + i);
						colItem.SetActive(true);
						colItem.transform.localScale = Vector3.one;
						colItem.GetComponent<CollectGood>().SetData(AuctionHouseSystem.collectionList_[i]);
						UIManager.SetButtonEventHandler(colItem, EnumButtonEvent.OnClick, OnCollectionSearch, id, isItem);
						UIManager.SetButtonEventHandler(colItem.GetComponent<CollectGood>().cancelCollectBtn_.gameObject, EnumButtonEvent.OnClick, OnUnCollect, i, isItem);
						collectItemCache[i] = colItem;
					}
					else
					{
						typeListGrid_.AddChild(collectItemCache[i].transform, index + 1 + i);
						collectItemCache[i].SetActive(true);
						collectItemCache[i].GetComponent<CollectGood>().SetData(AuctionHouseSystem.collectionList_[i]);
						UIManager.SetButtonEventHandler(collectItemCache[i], EnumButtonEvent.OnClick, OnCollectionSearch, id, isItem);
					UIManager.SetButtonEventHandler(collectItemCache[i].GetComponent<CollectGood>().cancelCollectBtn_.gameObject, EnumButtonEvent.OnClick, OnUnCollect, i, isItem);
						
					}
				}
				else
				{
					if (collectItemCache[i] != null)
					{
						UIManager.RemoveButtonEventHandler(collectItemCache[i], EnumButtonEvent.OnClick);
						collectItemCache[i].SetActive(false);
					}
				}
				
			}

	}

	
    
    void OnClose(ButtonScript obj, object args, int param1, int param2)
    {
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide();
			Destroyobj();
		});
        
    }

    COM_SearchContext searchContext_ = new COM_SearchContext();

    void OnSearch(ButtonScript obj, object args, int param1, int param2)
    {
        string content = searchPrex_.label.text;
        if (!string.IsNullOrEmpty(content))
        {
            AuctionHouseSystem.currentStartIndex_ = 0;
            LaunchContext(content);
            NetConnection.Instance.fetchSelling(searchContext_);
        }
    }

    void OnSubTypeSearch(ButtonScript obj, object args, int param1, int param2)
    {
        ResetItem4Buy();
        // param2 == 1 is item. else is baby.
        AuctionHouseSystem.currentStartIndex_ = 0;
        if (param2 == 1)
        {
            LaunchContext("", (ItemSubType)param1);
            NetConnection.Instance.fetchSelling(searchContext_);
        }
        else
        {
            LaunchContext("", ItemSubType.IST_None, (RaceType)param1);
            NetConnection.Instance.fetchSelling(searchContext_);
        }
    }

    void OnCollectionSearch(ButtonScript obj, object args, int param1, int param2)
    {
        // param2 == 1 is item. else is baby.
        AuctionHouseSystem.currentStartIndex_ = 0;
        if (param2 == 1)
        {
            LaunchContext("", ItemSubType.IST_None, RaceType.RT_None, param1);
        }
        else
        {
            LaunchContext("", ItemSubType.IST_None, RaceType.RT_None, 0, param1);
        }
        NetConnection.Instance.fetchSelling(searchContext_);
    }

    void OnDisplay(ButtonScript obj, object args, int param1, int param2)
    {
        SelectDisplayGroup((DisplayType)param1);
        if (param1 == (int)DisplayType.DT_Buy)
        {
            buyTab_.isEnabled = false;
            sellTab_.isEnabled = true;
        }
        else
        {
            buyTab_.isEnabled = true;
            sellTab_.isEnabled = false;
        }
    }

    void OnBuy(ButtonScript obj, object args, int param1, int param2)
    {
        if (crtSelectSellItem_ == null)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("NoGoodSelected"), PopText.WarningType.WT_Warning);
            return;
        }

        string name = "";
        bool isItem = false;
        if (crtSelectSellItem_.item_.itemId_ != 0)
        {
            name = ItemData.GetData((int)crtSelectSellItem_.item_.itemId_).name_;
            isItem = true;
        }
        else
            name = BabyData.GetData(crtSelectSellItem_.baby_.tableId_)._Name;

        //超级会员才能买
        if (GamePlayer.Instance.vipLevel_ != 2)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("superVIPCanBuy"), PopText.WarningType.WT_Warning);
            return;
        }

        if (GamePlayer.Instance.GetIprop(PropertyType.PT_Diamond) < crtSelectSellItem_.sellPrice)
        {
            if (GamePlayer.Instance.Properties[(int)PropertyType.PT_MagicCurrency] < crtSelectSellItem_.sellPrice)
            {
                PopText.Instance.Show(LanguageManager.instance.GetValue("NoMoreDiamond"), PopText.WarningType.WT_Warning);
            }
            else
            {
                MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaotimoli").Replace("{n}", crtSelectSellItem_.sellPrice.ToString()), delegate
                {
                    if (BagSystem.instance.BagIsFull() && isItem)
                    {
                        PopText.Instance.Show(LanguageManager.instance.GetValue("EN_BagFull"), PopText.WarningType.WT_Warning);
                        return;
                    }

                    NetConnection.Instance.buy(crtSelectSellItem_.guid_);
                    NetConnection.Instance.fetchSelling(searchContext_);
                    if (crtSelectSellItem_.item_ != null)
                        CommonEvent.ExcutePurchase((int)crtSelectSellItem_.item_.itemId_, (int)crtSelectSellItem_.item_.stack_, crtSelectSellItem_.sellPrice);
                });
            }
            return;
        }

        if (BagSystem.instance.BagIsFull() && isItem)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("EN_BagFull"), PopText.WarningType.WT_Warning);
            return;
        }

        MessageBoxUI.ShowMe(string.Format(LanguageManager.instance.GetValue("CostDiamondTip"), crtSelectSellItem_.sellPrice, name), () =>
        {
            NetConnection.Instance.buy(crtSelectSellItem_.guid_);
            NetConnection.Instance.fetchSelling(searchContext_);
			if(crtSelectSellItem_.item_ != null)
				CommonEvent.ExcutePurchase((int)crtSelectSellItem_.item_.itemId_, (int)crtSelectSellItem_.item_.stack_, crtSelectSellItem_.sellPrice);
        });
    }

    void OnPageUpdate(ButtonScript obj, object args, int param1, int param2)
    {
        int toPage = AuctionHouseSystem.currentPage_ + param1;
        if (toPage < 1)
            toPage = 1;
        if (toPage > AuctionHouseSystem.totalPage_)
            toPage = AuctionHouseSystem.totalPage_;
        AuctionHouseSystem.currentStartIndex_ = (toPage - 1) * AuctionHouseSystem.CountPerPage;
        searchContext_.begin_ = AuctionHouseSystem.currentStartIndex_;
        NetConnection.Instance.fetchSelling(searchContext_);
    }

    void OnCollect(ButtonScript obj, object args, int param1, int param2)
    {
        if (crtSelectSellItem_ == null)
            return;

        if (AuctionHouseSystem.collectionList_.Count == AuctionHouseSystem.CollectionMax)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("CollectionFull"), PopText.WarningType.WT_Warning);
            return;
        }

        int id = 0;
        bool isItem = false;
        if (crtSelectSellItem_.item_.itemId_ != 0)
        {
            id = (int)crtSelectSellItem_.item_.itemId_;
            isItem = true;
        }
        else if (crtSelectSellItem_.baby_.instId_ != 0)
            id = (int)crtSelectSellItem_.baby_.properties_[(int)PropertyType.PT_TableId];
        if (AuctionHouseSystem.AddCollection(id, isItem))
        {
            if (typeFlag[(int)TypeType.TT_Collection])
                OnClickTypeBtn(collectionTab_.gameObject.GetComponent<ButtonScript>(), null, 0, 1);

            PopText.Instance.Show(LanguageManager.instance.GetValue("CollectionSucc"), PopText.WarningType.WT_Tip);
        }
        else
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("CollectionSame"), PopText.WarningType.WT_Warning);
        }
    }

    void OnRecord(ButtonScript obj, object args, int param1, int param2)
    {
        sellRecordPanel_.SetUp();
    }

    GameObject[] sellItemCache = new GameObject[AuctionHouseSystem.CountPerPage];
    void OnSellListUpdate()
    {
        if (crtDisplayType_ != DisplayType.DT_Buy)
            return;

        if (AuctionHouseSystem.crtDisplayList_.Count == 0)
        {
            if (searchContext_.begin_ != 0)
            {
                searchContext_.begin_ -= AuctionHouseSystem.CountPerPage;
                NetConnection.Instance.fetchSelling(searchContext_);
                return;
            }

            nothingPage_.SetActive(true);
            hasItemPage_.SetActive(false);
        }
        else
        {
            nothingPage_.SetActive(false);
            hasItemPage_.SetActive(true);
        }

        GameObject sItemGo = null;
        for (int i = 0; i < AuctionHouseSystem.CountPerPage; ++i)
        {
            if (i < AuctionHouseSystem.crtDisplayList_.Count)
            {
                if (sellItemCache[i] == null)
                {
                    sItemGo = GameObject.Instantiate(sellItem_) as GameObject;
                    sItemGo.transform.parent = sellListGrid_.transform;
                    sItemGo.transform.localScale = Vector3.one;
                    sItemGo.GetComponent<SellGood>().SetData(AuctionHouseSystem.crtDisplayList_[i]);
                    sItemGo.SetActive(true);
                    UIManager.SetButtonEventHandler(sItemGo, EnumButtonEvent.OnClick, OnClickItem4Buy, i, 0);
                    sellItemCache[i] = sItemGo;
                }
                else
                {
                    sellItemCache[i].GetComponent<SellGood>().SetData(AuctionHouseSystem.crtDisplayList_[i]);
                    sellItemCache[i].SetActive(true);
                    UIManager.SetButtonEventHandler(sellItemCache[i], EnumButtonEvent.OnClick, OnClickItem4Buy, i, 0);
                }
            }
            else
            {
                if (sellItemCache[i] != null)
                {
                    sellItemCache[i].SetActive(false);
                    UIManager.RemoveButtonEventHandler(sellItemCache[i], EnumButtonEvent.OnClick);
                }
            }
        }
        sellListGrid_.Reposition();
        ResetItem4Buy();
        pageInfo_.text = string.Format("{0}/{1}", AuctionHouseSystem.currentPage_ > AuctionHouseSystem.totalPage_ ? AuctionHouseSystem.totalPage_ : AuctionHouseSystem.currentPage_, AuctionHouseSystem.totalPage_);
    }

    void ResetItem4Buy()
    {
        for (int i = 0; i < sellListGrid_.transform.childCount; ++i)
        {
            sellListGrid_.transform.GetChild(i).GetComponent<UIButton>().isEnabled = true;
        }
        crtSelectSellItem_ = null;
    }

    void OnClickItem4Buy(ButtonScript obj, object args, int param1, int param2)
    {
        ResetItem4Buy();
        COM_SellItem realItem = AuctionHouseSystem.crtDisplayList_[param1];
        crtSelectSellItem_ = new COM_SellItem();
        crtSelectSellItem_.baby_ = realItem.baby_;
        crtSelectSellItem_.guid_ = realItem.guid_;
        crtSelectSellItem_.item_ = realItem.item_;
        crtSelectSellItem_.guid_ = realItem.guid_;
        crtSelectSellItem_.sellPlayerId_ = realItem.sellPlayerId_;
        crtSelectSellItem_.sellPrice = realItem.sellPrice;
        crtSelectSellItem_.title_ = realItem.title_;
        obj.GetComponent<UIButton>().isEnabled = false;
    }

    BagType crtBagType;
    void OnBagTab(ButtonScript obj, object args, int param1, int param2)
    {
        listPanel_.clipOffset = Vector2.zero;
        listPanel_.transform.localPosition = Vector3.zero;
        listPanel_.GetComponent<UIScrollView>().ResetPosition();

        crtBagType = (BagType)param1;
        if (crtBagType == BagType.BT_Item)
        {
            itemSubTab_.isEnabled = false;
            babySubTab_.isEnabled = true;
            //selectedItemSubTabBtnForIndex(param2);
            itemSubTab_CrtPage_ = param2;
        }
        else
        {
            itemSubTab_.isEnabled = true;
            babySubTab_.isEnabled = false;
            //selectedbabySubTabBtnForIndex(param2);
            babySubTab_CrtPage_ = param2;
        }

        itemSubContent_.SetActive(crtBagType == BagType.BT_Item);
        babySubContent_.SetActive(crtBagType == BagType.BT_Baby);

        UpdateSubContent();
    }

    void Update()
    {
        listDragArea_.center = listPanel_.clipOffset;
    }

    List<GameObject> itemObjPool = new List<GameObject>();
    List<GameObject> babyObjPool = new List<GameObject>();

    void ClearSub()
    {
        for (int i = 0; i < itemObjPool.Count; ++i)
        {
            itemObjPool[i].SetActive(false);
        }
        for (int i = 0; i < babyObjPool.Count; ++i)
        {
            babyObjPool[i].SetActive(false);
        }
    }
    void UpdateSubContent()
    {
        ClearSub();
        if (crtBagType == BagType.BT_Item)
        {
            ItemCellUI cell;
            int count = 0;
            GameObject item = null;
            for (int i = 0; i < BagSystem.instance.BagItems.Length; ++i)
            {
                if (BagSystem.instance.BagItems[i] != null)
                {
                    if (itemObjPool.Count > count)
                        item = itemObjPool[count];
                    else
                    {
                        item = (GameObject)GameObject.Instantiate(itemsInBagPre_) as GameObject;
                        item.transform.parent = itemsInBagGrid.transform;
                        item.transform.localScale = Vector3.one;
                        itemObjPool.Add(item);
                    }
                    cell = UIManager.Instance.AddItemInstCellUI(item.GetComponent<UISprite>(), BagSystem.instance.BagItems[i]);
                    cell.ItemCount = (int)BagSystem.instance.BagItems[i].stack_;
                    cell.gameObject.AddComponent<UIDragScrollView>();
                    item.SetActive(true);

                    cell.enable = !BagSystem.instance.BagItems[i].isLock_ && !BagSystem.instance.BagItems[i].isBind_ && GlobalInstanceFunction.Instance.DayPass((int)BagSystem.instance.BagItems[i].lastSellTime_) >= AuctionHouseSystem.GoodProtectDay;//ItemData.GetData((int)BagSystem.instance.BagItems[j].itemId_).price_ != 0;
                    cell.collideEnable = true;
                    if (cell.enable)
                        UIManager.SetButtonEventHandler(cell.cellPane.gameObject, EnumButtonEvent.OnClick, OnClickBagSomthing, (int)crtBagType, (int)BagSystem.instance.BagItems[i].instId_);
                    else
                        UIManager.SetButtonEventHandler(cell.cellPane.gameObject, EnumButtonEvent.OnClick, OnClickDontSellSomthing, 0, 0);
                    count ++;
                }
            }
            itemsInBagGrid.repositionNow = true;

        }
        else
        {
            BabyCellUI cell = null;
            int count = 0;
            GameObject babyobj = null;
            for (int i = 0; i < GamePlayer.Instance.babies_list_.Count; ++i)
            {
                if (GamePlayer.Instance.babies_list_[i] != null)
                {
                    if (babyObjPool.Count > count)
                        babyobj = babyObjPool[count];
                    else
                    {
                        babyobj = (GameObject)GameObject.Instantiate(babiesInBagPre_) as GameObject;
                        babyobj.transform.parent = babiesInBagGrid.transform;
                        babyobj.transform.localScale = Vector3.one;
                        babyObjPool.Add(babyobj);
                    }
                    cell = UIManager.Instance.AddBabyCellUI(babyobj.GetComponent<UISprite>(), GamePlayer.Instance.babies_list_[i]);
                    UIManager.SetButtonEventHandler(cell.gameObject, EnumButtonEvent.OnClick, OnClickIcon, i, 0);
                    babyobj.SetActive(true);

                    cell.enable = !GamePlayer.Instance.babies_list_[i].GetInst().isLock_ && !GamePlayer.Instance.babies_list_[i].GetInst().isBind_ && GlobalInstanceFunction.Instance.DayPass((int)GamePlayer.Instance.babies_list_[i].GetInst().lastSellTime_) >= AuctionHouseSystem.GoodProtectDay;//ItemData.GetData((int)BagSystem.instance.BagItems[j].itemId_).price_ != 0;
                    cell.collideEnable = true;
                    cell.gameObject.AddComponent<UIDragScrollView>();
                    if (cell.enable)
                        UIManager.SetButtonEventHandler(cell.cellPane.gameObject, EnumButtonEvent.OnClick, OnClickBagSomthing, (int)crtBagType, GamePlayer.Instance.babies_list_[i].InstId);
                    else
                        UIManager.SetButtonEventHandler(cell.cellPane.gameObject, EnumButtonEvent.OnClick, OnClickDontSellSomthing, 0, 0);
                    count++;
                }
            }
            babiesInBagGrid.repositionNow = true;
        }
    }
    void OnClickIcon(ButtonScript obj, object args, int param1, int param2)
    {
        ChatBabytips.ShowMe((COM_BabyInst)GamePlayer.Instance.babies_list_[param1].GetInst());
    }

    void OnClickBagSomthing(ButtonScript obj, object args, int param1, int param2)
    {
        if ((BagType)param1 == BagType.BT_Item)
        {
            sellConfirmItemPanel_.SetData(BagSystem.instance.GetItemByInstId(param2));
        }
        else
        {
            sellConfirmBabyPanel_.SetData(GamePlayer.Instance.GetBabyInst(param2));
        }
    }

    void OnClickDontSellSomthing(ButtonScript obj, object args, int param1, int param2)
    {
        PopText.Instance.Show(LanguageManager.instance.GetValue("dontSell"));
    }

    GameObject[] mySellingCache = new GameObject[AuctionHouseSystem.SellingMax];
    void OnMySellingUpdate()
    {
        if (crtDisplayType_ != DisplayType.DT_Sell)
            return;

        GameObject go = null;
        for (int i = 0; i < AuctionHouseSystem.SellingMax; ++i)
        {
            if (i < AuctionHouseSystem.mySellingList_.Count)
            {
                if (mySellingCache[i] == null)
                {
                    go = GameObject.Instantiate(mySellingItem_) as GameObject;
                    go.transform.parent = mySellingListGrid_.transform;
                    go.transform.localScale = Vector3.one;
                    go.GetComponent<SellingGood>().SetData(AuctionHouseSystem.mySellingList_[i]);
                    UIManager.SetButtonEventHandler(go, EnumButtonEvent.OnClick, OnCancelSelling, AuctionHouseSystem.mySellingList_[i].guid_, 0);
                    go.SetActive(true);
                    mySellingCache[i] = go;
                }
                else
                {
                    mySellingCache[i].GetComponent<SellingGood>().SetData(AuctionHouseSystem.mySellingList_[i]);
					UIManager.SetButtonEventHandler(mySellingCache[i], EnumButtonEvent.OnClick, OnCancelSelling, AuctionHouseSystem.mySellingList_[i].guid_, 0);
                    mySellingCache[i].SetActive(true);
                }
            }
            else
            {
                if (mySellingCache[i] != null)
                {
                    UIManager.RemoveButtonEventHandler(mySellingCache[i], EnumButtonEvent.OnClick);
                    mySellingCache[i].SetActive(false);
                }
            }
        }
        mySellingListGrid_.Reposition();
        // 请求刷新商会出售列表
        LaunchContext();
        NetConnection.Instance.fetchSelling(searchContext_);
        // 刷新背包
        UpdateSubContent();

        mySellingCount_.text = string.Format("{0}/{1}", AuctionHouseSystem.mySellingList_.Count, AuctionHouseSystem.SellingMax);
    }

    void OnCancelSelling(ButtonScript obj, object args, int param1, int param2)
    {
        MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("cancelSelling"), () =>
        {
            NetConnection.Instance.unselling(param1);
        });
    }

    public static void ShowMe()
    {
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_AuctionHousePanel);
    }
    public static void SwithShowMe()
    {
        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_AuctionHousePanel);
    }

    public override void Destroyobj()
    {
        AuctionHouseSystem.OnSellListUpdate -= OnSellListUpdate;
        AuctionHouseSystem.OnMySellingListUpdate -= OnMySellingUpdate;
        GamePlayer.Instance.OnBabyUpdate -= UpdateSubContent;
        GamePlayer.Instance.OnIPropUpdate -= OnDiamondUpdate;

        UIManager.RemoveButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(searchBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(buyBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(buyTab_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(sellTab_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(collectBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(pageUpBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(pageDownBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(sellRecordBtn_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(itemSubTab_.gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(babySubTab_.gameObject, EnumButtonEvent.OnClick);
	
        GameObject.Destroy(gameObject);
    }

    //void selectedItemSubTabBtnForIndex(int index)
    //{
    //    for(int i =0;i<itemSubTab_Group_.Length;i++)
    //    {
    //        if(i==index)
    //        {
    //            itemSubTab_Group_[i].isEnabled = false;
    //        }else
    //        {
    //            itemSubTab_Group_[i].isEnabled = true;
    //        }
    //    }
    //}
    //void selectedbabySubTabBtnForIndex(int index)
    //{
    //    for(int i =0;i<babySubTab_Group_.Length;i++)
    //    {
    //        if(i==index)
    //        {
    //            babySubTab_Group_[i].isEnabled = false;
    //        }else
    //        {
    //            babySubTab_Group_[i].isEnabled = true;
    //        }
    //    }
    //}

}
