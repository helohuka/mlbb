using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CompoundUI : UIBase
{
	public UILabel _TitleLable;
	public UILabel _PromptLable;
	public UILabel _ProfessionEquLable;
	public UILabel _ArmsLable;
	public UILabel _HatLable;
	public UILabel _ArmorLable;
	public UILabel _ShoeLable;

	public GameObject skillCell;
	public UISprite InlayGem; 
	public UISprite gemIcon;
	public UIGrid skillGrid;

	public UIPanel gemPanel;
	public GameObject compoundList;
	public GameObject gemCell;
	public UIGrid  gemGrid;
	public UIButton gemCloseBtn;
	public UISprite compoundIcon;   
	public UILabel compoundItemName;
	public UILabel compoundItemLevel;
	public UILabel compoundItemProp;
	public UIButton compoundBtn;
	public UILabel gemNameLab; 
	public UILabel gemListNiGemLab;
	public UIButton closeBtn;
	public UILabel needMoneyLab;

	public UIButton wuQiBtn;
	public UIButton maoZiBtn;
	public UIButton kuiJiaBtn;
	public UIButton xieZiBtn;
	public UIButton dunPaiBtn;
	public UIButton jobEquipBtn;
	public GameObject CompoundInfoObj;
	public compoundNeedItemInfoUI infoTipsUI;

	public List<UISprite> needItemBack = new List<UISprite>();
	public List<UITexture> needItemsIcon = new List<UITexture>();
	public List<COM_Item> needItems = new List<COM_Item> ();
		 
	public GameObject chindItemCell;
	public UILabel noGemLab;
	public GameObject compoundOkPanel;
	public UIButton compoundOkBtn;

	public UIGrid gemInfoGrid;
	public GameObject gemInfoCell;
	public UILabel gemName;
	public GameObject gemInfoObj;
	public UISprite geamBack;
	public UIButton buyGemBtn;
	public UIScrollBar listBar;
	public GameObject propCell;
	public UIGrid propGrid;
	public UIGrid PBpropGrid;
	public UISprite topImg;
	public GameObject peifangInfo;
	public UILabel peifangLab;
	public UITexture peifangIcon;
	public UISprite pobiaoBtn;
	public UISprite BtnArrTop;
	public UISprite BtnArr;
	public UISprite PBPropImg;
	public UISprite PropImg;
	private bool isItemEnough;
	private CompoundSkillCellUI _selectSkillCell;
	private List<GameObject> chindCellList = new List<GameObject>();
	private List<GameObject> chindCellPoolList = new List<GameObject>();
	private List<GameObject> gemCellList = new List<GameObject>();
	private List<GameObject> gemCellPoolList = new List<GameObject>();
	private List<GameObject> propCellList = new List<GameObject>();
	private List<GameObject> propCellPoolList = new List<GameObject>();

	private GameObject _selectChindObj;
	private bool showPBprop = false;
	private List<GameObject> levelBtnCellList = new List<GameObject>();

	private int _gemId;
	private ItemCellUI _compoundItem;
	private ItemCellUI _gemItem;
	private ButtonScript _chindItemCellObj;
	private List<string> _icons = new List<string>();
	public static bool isOpen;
	private int _selectEquipType;
	private int _selectLevel;
	private bool _bJobEquip;
	private static bool _bTiSheng;
	void Start ()
	{
		//InitUIText ();
		isOpen = true;
		UIManager.SetButtonEventHandler (InlayGem.gameObject, EnumButtonEvent.OnClick, OnGem, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (gemCloseBtn.gameObject, EnumButtonEvent.OnClick, OnCloseGem, 0, 0);
		UIManager.SetButtonEventHandler (compoundBtn.gameObject, EnumButtonEvent.OnClick, OnCompound, 0, 0);

		UIManager.SetButtonEventHandler (wuQiBtn.gameObject, EnumButtonEvent.OnClick, OnEquipType, 2, 0);
		UIManager.SetButtonEventHandler (maoZiBtn.gameObject, EnumButtonEvent.OnClick, OnEquipType, 4, 0);
		UIManager.SetButtonEventHandler (kuiJiaBtn.gameObject, EnumButtonEvent.OnClick, OnEquipType, 6, 0);
		UIManager.SetButtonEventHandler (xieZiBtn.gameObject, EnumButtonEvent.OnClick, OnEquipType, 1, 0);
		UIManager.SetButtonEventHandler (dunPaiBtn.gameObject, EnumButtonEvent.OnClick, OnEquipType, 7, 0);
		UIManager.SetButtonEventHandler (compoundOkBtn.gameObject, EnumButtonEvent.OnClick, OnCompoundOk, 0, 0);
		UIManager.SetButtonEventHandler (geamBack.gameObject, EnumButtonEvent.OnClick, OnCloseGem, 0, 0);
		UIManager.SetButtonEventHandler (buyGemBtn.gameObject, EnumButtonEvent.OnClick, OnBuyGem, 0, 0);
		UIManager.SetButtonEventHandler (jobEquipBtn.gameObject, EnumButtonEvent.OnClick, OnEquipType, 8, 0);
		UIManager.SetButtonEventHandler (pobiaoBtn.gameObject, EnumButtonEvent.OnClick, OnEquipPoBiao, 8, 0);


		for(int i=0;i<needItemsIcon.Count;i++)
		{
			UIManager.SetButtonEventHandler (needItemsIcon[i].gameObject, EnumButtonEvent.OnClick, OnNeedItem, 0, 0);
		}

		UIManager.SetButtonEventHandler (peifangIcon.gameObject, EnumButtonEvent.OnClick, OnNeedItem, 0, 0);

        GatherSystem.instance.CompoundOkEvent += new RequestEventHandler<COM_Item> (MakeOk);
		GatherSystem.instance._GatherItemHandler += new GatherItemHandler(UpdateNeedMakeItem);
		BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (UpdateAddMakeItem);
		BagSystem.instance.ItemChanged += new ItemChangedEventHandler (UpdateAddMakeItem);
		GamePlayer.Instance.OnIPropUpdate += UpdateMoney;
		isItemEnough = true;
		//UpdateSkillList ();
		compoundBtn.isEnabled = false;
		InlayGem.gameObject.SetActive (false);

		OpenPanelAnimator.PlayOpenAnimation(this.panel,()=>{
			SetBtnEnabled (true);
			jobEquipBtn.isEnabled = false;
			_bJobEquip = true;
			_selectLevel = 1;
			_selectEquipType = (int)EquipmentSlot.ES_SingleHand;
			UpdateSkillList ();
		
			if(_bTiSheng)
			{
			//	SelectItem();
			}
		});
		UIManager.Instance.LoadMoneyUI (gameObject);
        GuideManager.Instance.RegistGuideAim(compoundBtn.gameObject, GuideAimType.GAT_MainMakeCompoundBtn);
        GuideManager.Instance.RegistGuideAim(InlayGem.gameObject, GuideAimType.GAT_MainMakeGemBtn);
        GuideManager.Instance.RegistGuideAim(gemCloseBtn.gameObject, GuideAimType.GAT_MainMakeGemClose);

	}
	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("forging_Title");
		_PromptLable.text = LanguageManager.instance.GetValue("forging_Prompt");
		_ProfessionEquLable.text = LanguageManager.instance.GetValue("forging_ProfessionEqu");
		_ArmsLable.text = LanguageManager.instance.GetValue("forging_Arms");
		_HatLable.text = LanguageManager.instance.GetValue("forging_Hat");
		_ArmorLable.text = LanguageManager.instance.GetValue("forging_Armor");
		_ShoeLable.text = LanguageManager.instance.GetValue("forging_Shoe");
	}


	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe(bool ts = false)
	{
		if(!CompoundSystem.instance.isInit)
		{
			CompoundSystem.instance.IsOpenInit = true;
			CompoundSystem.instance.bTiSheng = ts;
			NetConnection.Instance.requestCompound();
            NetWaitUI.ShowMe();
		}
		else
		{
			_bTiSheng = ts;
			UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_SkillTabPanel);
		}
	}
	
	public static void ShowMe(bool ts = false)
	{
		_bTiSheng = ts;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_SkillTabPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_SkillTabPanel);
	}

	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);

		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);

		}
	}

	#endregion


	public void Show()
	{
		if(_selectSkillCell != null)
		{
			updateCompoundInfo(_selectSkillCell);
		}
	}


	private void UpdateEquipType(int Level ,int type)
	{



	}

	public void UpdateSkillList()
	{
		int levelNum = GamePlayer.Instance.GetIprop (PropertyType.PT_Level) / 10 + 1;
        GameObject obj = null;
        Transform root = null;
		for(int i= 1;i<levelNum;i++)
		{
			obj = Object.Instantiate (skillCell.gameObject) as GameObject;
            root = obj.transform.FindChild("bg");
			if(i ==0)
			{
				UIManager.SetButtonEventHandler (obj, EnumButtonEvent.OnClick, OnSkillCell, 1, 0);

                root.FindChild("level").GetComponent<UILabel>().text = "1-9"; 
				obj.name  = "1";

			}
			else
			{
				UIManager.SetButtonEventHandler (obj, EnumButtonEvent.OnClick, OnSkillCell, i*10, 0);
				int num = i*10;
                root.FindChild("level").GetComponent<UILabel>().text = num + "-" + (num + 9); 
				obj.name  =(i*10)+"";
			}

            if(i == 1)
            {
                GuideManager.Instance.RegistGuideAim(obj, GuideAimType.GAT_MainMakeLevel10);
            }

			obj.transform.parent = skillGrid.transform;
			obj.SetActive (true);
			obj.transform.localScale = Vector3.one;

			levelBtnCellList.Add(obj);
		}
	
        skillGrid.Reposition ();
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMakeUIOpen);
	}

	private void UpdataGemList()
	{
	


		COM_Item[] bagItem = BagSystem.instance.BagItems;



		for(int k =0; k<gemCellList.Count;k++)
		{
			gemCellList[k].transform.parent = null;
			gemCellList[k].gameObject.SetActive(false);
			gemCellPoolList.Add(gemCellList[k]); 
		}
		gemCellList.Clear ();

		if(BagSystem.instance.GetGemList().Count<=0)
		{
			gemListNiGemLab.gameObject.SetActive(true);
			return;
		}
		else
		{
			gemListNiGemLab.gameObject.SetActive(false);
		}

		for(int i=0;i<bagItem.Length;i++)
		{
			if(bagItem[i] == null)
				continue;

			ItemData item = ItemData.GetData((int)bagItem[i].itemId_);
			if(item.mainType_ == ItemMainType.IMT_Consumables  &&  item.subType_ == ItemSubType.IST_Gem)
			{
				GameObject obj ;

				if(gemCellPoolList.Count > 0)
				{
					obj= gemCellPoolList[0];
					gemCellPoolList.Remove(gemCellPoolList[0]);
				}
				else
				{
					obj= Object.Instantiate(gemCell.gameObject) as GameObject;
				}
				CompoundGemCellUI cell =  obj.GetComponent<CompoundGemCellUI>();
				//cell.Item = item;
				cell.ItemInst = bagItem[i];
				cell.callBack = OnGemCell;
				obj.transform.parent = gemGrid.transform;
				obj.transform.localScale = Vector3.one;
				obj.SetActive(true);
				gemCellList.Add(obj);
                if (gemCellList.Count == 1)
                    GuideManager.Instance.RegistGuideAim(cell.inlayBtn.gameObject, GuideAimType.GAT_MainMakeGemFirst);
			}


		}
		gemGrid.Reposition ();
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMakeGemUI);
        }, 1);
    }
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();

	void OnGemCell(COM_Item gem)
	{
		if(gem == null)
		{
			return;
		}

		if(_gemItem == null)
		{
			_gemItem = UIManager.Instance.AddItemCellUI(gemIcon,gem.itemId_);
			UIManager.SetButtonEventHandler (_gemItem.cellPane.gameObject, EnumButtonEvent.OnClick, OnOutGem,0, 0);
		}
		else
		{
			_gemItem.itemId = gem.itemId_;
		}
		_gemItem.cellPane.gameObject.SetActive (true);
		_gemId = (int)gem.itemId_;
		//gemNameLab.text = ItemData.GetData ((int)gem.itemId_).name_;
		gemName.text = ItemData.GetData ((int)gem.itemId_).name_;
		gemName.gameObject.SetActive (true);
		gemInfoObj.gameObject.SetActive (true);

		ItemData gDate = ItemData.GetData ((int)gem.itemId_);

		foreach( GameObject o in cellList)
		{
			gemInfoGrid.RemoveChild(o.transform);
			o.transform.parent = null;
			o.gameObject.SetActive(false);
			cellPool.Add(o);
		}
		cellList.Clear ();

		List<KeyValuePair<PropertyType,string>> proplist = new List<KeyValuePair<PropertyType, string>> ();
		if (ItemData.GetData ((int)_selectSkillCell.MakeId).slot_ == EquipmentSlot.ES_SingleHand || ItemData.GetData ((int)_selectSkillCell.MakeId).slot_ == EquipmentSlot.ES_DoubleHand) 
		{
			if(ItemData.GetData ((int)_selectSkillCell.MakeId).subType_ == ItemSubType.IST_Shield)
			{
				proplist = gDate.GemArmorPropArr;
			}
			else
			{
				proplist = gDate.GemWeaponPropArr;
			}
		}
		else
		{
			proplist = gDate.GemArmorPropArr;
		}

		if(proplist.Count > 0)//gem.propArr.Length > 0)
		{
			for(int i=0;i<proplist.Count;i++)
			{
				GameObject objCell = null;
				if(cellPool.Count>0)
				{
					objCell = cellPool[0];
					cellPool.Remove(objCell);  
					//UIManager.RemoveButtonAllEventHandler(objCell);
				}
				else  
				{
					objCell = Object.Instantiate(gemInfoCell) as GameObject;
				}

				string sNum = "";
				if(float.Parse(proplist[i].Value) > 0)
				{
					if(float.Parse(proplist[i].Value) < 1)
					{
						sNum = " +" + (float.Parse(proplist[i].Value) * 100) + "%";
					}
					else
					{
						sNum = " +" + (float.Parse(proplist[i].Value)); 
					}
				}
				else
				{
					if(float.Parse(proplist[i].Value) > -1)
					{
						sNum = " " + (float.Parse(proplist[i].Value) * 100) + "%";
					}
					else
					{
						sNum = " " + (float.Parse(proplist[i].Value));
					}
				}
				UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>();
				lable.text  =  LanguageManager.instance.GetValue(proplist[i].Key.ToString())+sNum;

				objCell.transform.parent = gemInfoGrid.transform;
				objCell.gameObject.SetActive(true);	
				objCell.transform.localScale = Vector3.one;
				cellList.Add(objCell);
			}
			gemInfoGrid.Reposition();
		}

        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMakeGemOk);
        }, 1);

		gemPanel.gameObject.SetActive (false);
		compoundList.SetActive (true);
	}

	private void OnSkillCell(ButtonScript obj, object args, int param1, int param2)
	{
		if(_chindItemCellObj != null && _chindItemCellObj != obj)
		{
            _chindItemCellObj.transform.FindChild("bg").FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao";
			_chindItemCellObj = null;
		}

        if (obj.transform.FindChild("bg").FindChild("arrows").GetComponent<UISprite>().spriteName == "sanjiao2")
		{
            obj.transform.FindChild("bg").FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao";
			foreach(GameObject c in chindCellList)
			{
				skillGrid.RemoveChild(c.transform);
				c.transform.parent = null;
				c.gameObject.SetActive(false);
				chindCellPoolList.Add(c);
			}
			chindCellList.Clear ();
			_chindItemCellObj = null;
			return;
		}
		else
		{
            obj.transform.FindChild("bg").FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao2";

			foreach(GameObject c in chindCellList)
			{
				skillGrid.RemoveChild(c.transform);
				c.transform.parent = null;
				c.gameObject.SetActive(false);
				chindCellPoolList.Add(c);
			}
			chindCellList.Clear ();

			_chindItemCellObj = obj;
		}

		int indx = skillGrid.GetIndex (obj.transform);
		int pLevel = param1;// GamePlayer.Instance.GetIprop (PropertyType.PT_Level) / 10 + 1;
		if(pLevel== 1)
		{
			pLevel = 0;
		}
        int index = 0;
        bool jobFirst = false;


		Profession pro =  Profession.get((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession),GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
		ItemSubType[] items = pro.CanUsedItems(GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)*2+2);

		foreach(MakeData m in MakeData.metaData.Values)
		{
			if(_bJobEquip)
			{
				if((pLevel <= m.skillLevel && m.skillLevel <= pLevel +9) && m.type_ == "Player" )
				{
					for(int i =0 ;i<items.Length;i++)
					{
						if(ItemData.GetData(m.itemId_).subType_ == items[i])
						{

							GameObject objCell = null;
							if(chindCellPoolList.Count>0)
							{
								objCell = chindCellPoolList[0];
								chindCellPoolList.Remove(objCell);  
							}
							else  
							{
								objCell = Object.Instantiate(chindItemCell.gameObject) as GameObject;
							}
							
							UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickChindItem,0, 0);
							CompoundSkillCellUI cell = objCell.GetComponent<CompoundSkillCellUI>();
							cell.SkillId = m.skillId;
							cell.MakeId = m.itemId_;
                            cell.gameObject.transform.FindChild("bg").FindChild("recommend").GetComponent<UISprite>().gameObject.SetActive(true);
							skillGrid.AddChild(objCell.transform,++indx);
							objCell.SetActive(true);
							objCell.transform.localScale = Vector3.one;
							chindCellList.Add(objCell);

                            if (jobFirst == false && i == 0)
                            {
                                GuideManager.Instance.RegistGuideAim(objCell, GuideAimType.GAT_MainMakeSubFirst);
                            }
                            if (jobFirst == false)
                                jobFirst = true;
						}
					}
				}

			}
			else
			{

				if((ItemData.GetData(m.itemId_).slot_ == (EquipmentSlot)_selectEquipType ||
	                ((EquipmentSlot)_selectEquipType == EquipmentSlot.ES_SingleHand &&
	                    ItemData.GetData(m.itemId_).slot_ == EquipmentSlot.ES_DoubleHand)) &&
				   (pLevel <= m.skillLevel && m.skillLevel <= pLevel +9)&& m.type_ == "Player" )
				{
					GameObject objCell = null;
					if(chindCellPoolList.Count>0)
					{
						objCell = chindCellPoolList[0];
						chindCellPoolList.Remove(objCell);  
					}
					else  
					{
						objCell = Object.Instantiate(chindItemCell.gameObject) as GameObject;
					}

					UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickChindItem,0, 0);
					CompoundSkillCellUI cell = objCell.GetComponent<CompoundSkillCellUI>();
					cell.SkillId = m.skillId;
					cell.MakeId = m.itemId_;
					skillGrid.AddChild(objCell.transform,++indx);
					objCell.SetActive(true);
					objCell.transform.localScale = Vector3.one;
					chindCellList.Add(objCell);
	                if (index == 0)
	                {
	                    GuideManager.Instance.RegistGuideAim(objCell, GuideAimType.GAT_MainMakeSubFirst);
	                }

	                if (index == 2)
	                {
	                    GuideManager.Instance.RegistGuideAim(objCell, GuideAimType.GAT_MainMakeSubSecond);
	                }

	                if (index == 4)
	                {
	                    GuideManager.Instance.RegistGuideAim(objCell, GuideAimType.GAT_MainMakeSubThird);
	                }
	                index++;
				}
			}

		}
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMakeSub, GamePlayer.Instance.GetIprop(PropertyType.PT_Profession));
	}

	public void  updateCompoundInfo(CompoundSkillCellUI cell)
	{
		CompoundInfoObj.SetActive (true);
		//if(_selectSkillCell !=  null)
		//{
		//	_selectSkillCell.arrow.gameObject.SetActive(false);
		//}
		
		_selectSkillCell = cell;
	//	_selectSkillCell.arrow.gameObject.SetActive(true);
		//skillNameLab.text = cell.nameLab.text; 
		MakeData make = MakeData.GetData (cell.MakeId);
		if(make==null)
		{
			return;
		}
		ItemData itemData = ItemData.GetData (cell.MakeId);
		if(itemData == null)
		{
			return;
		}
		compoundIcon.gameObject.SetActive (true);
		if(_compoundItem == null)
		{
			_compoundItem = UIManager.Instance.AddItemCellUI(compoundIcon,(uint)itemData.id_);
			_compoundItem.showTips = true;

		}
		else
		{
			_compoundItem.itemId = (uint)itemData.id_;
		}
		InlayGem.gameObject.SetActive (true);
		compoundItemName.text = itemData.name_;
		compoundItemLevel.text = itemData.level_.ToString();
		needMoneyLab.text = make.needMoney.ToString ();


	/*	if(itemData.propArr.Count > 0)
		{
			compoundItemProp.text = "";
			for(int i=0;i<itemData.propArr.Count;i++)
			{
				compoundItemProp.text += LanguageManager.instance.GetValue(itemData.propArr[i].Key.ToString())+" "  
					+itemData.propArr[i].Value[0].ToString()+"-"+itemData.propArr[i].Value[1].ToString() + " ";
			}
		}
		else
		{
			compoundItemProp.gameObject.SetActive(false);
		}
	*/



		foreach(UITexture v in needItemsIcon )
		{
			v.gameObject.SetActive(false);
		}

		isItemEnough = true;
		compoundBtn.isEnabled = true;
		for(int i =0 ;i<make.needItems.Length;i++)
		{
			ItemData needItem = ItemData.GetData (int.Parse(make.needItems[i]));
			if(needItem == null)
			{
				return;
			}
			needItemsIcon[i].gameObject.SetActive(true);
			HeadIconLoader.Instance.LoadIcon(needItem.icon_, needItemsIcon[i]);

			if(!_icons.Contains(needItem.icon_))
			{
				_icons.Add(needItem.icon_);
			}

			needItemsIcon[i].name = needItem.id_.ToString();
			int itemCount = BagSystem.instance.GetItemMaxNum(uint.Parse(make.needItems[i]));
		
			needItemsIcon[i].gameObject.transform.Find("num").GetComponent<UILabel>().text = itemCount+"/"+make.needItemNum[i];
			needItemsIcon[i].gameObject.transform.Find("num").GetComponent<UILabel>().color = Color.white;
			needItemsIcon[i].gameObject.transform.Find("name").GetComponent<UILabel>().text = needItem.name_;
			
			if(itemCount < int.Parse(make.needItemNum[i]))
			{
				needItemsIcon[i].gameObject.transform.Find("num").GetComponent<UILabel>().color = Color.red;
				isItemEnough = false;
				compoundBtn.isEnabled = false;
			}
			if(make.skillLevel  > GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
			{
				compoundBtn.isEnabled = false;
			}
			if(make.needMoney > GamePlayer.Instance.GetIprop(PropertyType.PT_Money))
			{
				compoundBtn.isEnabled = false;
			}
		}


		if(make.skillLevel >= 40 &&!CompoundSystem.instance.GetIsOPenEquip((uint)cell.MakeId ))
		{
			peifangInfo.gameObject.SetActive(true);
			ItemData pItem = ItemData.GetData(make.needBook_);
			if(pItem != null)
			{
				peifangLab.text =  LanguageManager.instance.GetValue("xuyaopeifang").Replace("{n}",pItem.name_);
				peifangIcon.name = pItem.id_.ToString();
				HeadIconLoader.Instance.LoadIcon(pItem.icon_,peifangIcon);


				if(_gemId != 0)
				{
					_gemItem.cellPane.gameObject.SetActive(false);
					_gemId = 0;
					gemName.text = "";
					gemNameLab.text = LanguageManager.instance.GetValue("inlaygem");
					gemInfoObj.gameObject.SetActive (false);
					
				}


			}
			compoundBtn.isEnabled = false;
		}
		else
		{
			peifangInfo.gameObject.SetActive(false);
		}

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMakeSubDetail, compoundBtn.isEnabled == true? 1: 0);
	}

	public void  updateCompoundPropInfo(CompoundSkillCellUI cell)
	{

		MakeData make = MakeData.GetData (cell.MakeId);
		if(make==null)
		{
			return;
		}
		ItemData itemData = ItemData.GetData (cell.MakeId);

		for(int k =0; k<propCellList.Count;k++)
		{
			propCellList[k].transform.parent = null;
			propCellList[k].gameObject.SetActive(false);
			propCellPoolList.Add(propCellList[k]); 
		}
		propCellList.Clear ();

	
	
		GameObject djobj ;
		
		if(propCellPoolList.Count > 0)
		{
			djobj= propCellPoolList[0];
			propCellPoolList.Remove(propCellPoolList[0]);
		}
		else
		{
			djobj= Object.Instantiate(propCell.gameObject) as GameObject;
		}
		djobj.transform.FindChild ("skillName").GetComponent<UILabel> ().text = LanguageManager.instance.GetValue ("dengji") +" "+ itemData.level_;
		djobj.transform.FindChild("arrow").GetComponent<UISprite>().gameObject.SetActive(false);
		djobj.transform.parent = propGrid.transform;
		djobj.transform.localScale = Vector3.one;
		djobj.SetActive(true);
		propCellList.Add(djobj);

		for(int i=0;i<itemData.propArr.Count;i++)
		{
			GameObject obj ;
			
			if(propCellPoolList.Count > 0)
			{
				obj= propCellPoolList[0];
				propCellPoolList.Remove(propCellPoolList[0]);
			}
			else
			{
				obj= Object.Instantiate(propCell.gameObject) as GameObject;
			}
			obj.transform.FindChild("skillName").GetComponent<UILabel>().text =  LanguageManager.instance.GetValue(itemData.propArr[i].Key.ToString())+" "  
				+itemData.propArr[i].Value[0].ToString()+"-"+itemData.propArr[i].Value[1].ToString() + " ";
			obj.transform.FindChild("arrow").GetComponent<UISprite>().gameObject.SetActive(false);
			obj.transform.parent = propGrid.transform;
			obj.transform.localScale = Vector3.one;
			obj.SetActive(true);
			propCellList.Add(obj);
		
		}
		propGrid.Reposition ();


		ItemData itemD = ItemData.GetData (MakeData.GetData(_selectSkillCell.MakeId).specialID_);
		if (itemD == null)
			return;

		for(int i=0;i<itemD.propArr.Count;i++)
		{
			GameObject Pobj ;
			
			if(propCellPoolList.Count > 0)
			{
				Pobj= propCellPoolList[0];
				propCellPoolList.Remove(propCellPoolList[0]);
			}
			else
			{
				Pobj= Object.Instantiate(propCell.gameObject) as GameObject;
			}
			Pobj.transform.FindChild("skillName").GetComponent<UILabel>().text =  LanguageManager.instance.GetValue(itemD.propArr[i].Key.ToString())+" "  
				+itemD.propArr[i].Value[0].ToString()+"-"+itemD.propArr[i].Value[1].ToString() + " ";
			Pobj.transform.FindChild("arrow").GetComponent<UISprite>().gameObject.SetActive(true);
			Pobj.transform.parent = PBpropGrid.transform;
			Pobj.transform.localScale = Vector3.one;
			Pobj.SetActive(true);
			propCellList.Add(Pobj);
			
		}
		PBpropGrid.Reposition ();


	}

	private void OnClickChindItem(ButtonScript obj, object args, int param1, int param2)
	{
		CompoundSkillCellUI cell = obj.GetComponent<CompoundSkillCellUI> ();
		//if (_selectChindObj == obj.gameObject)
			//return;
		if(_selectChindObj == null)
		{
			_selectChindObj = obj.gameObject;
			obj.GetComponent<CompoundSkillCellUI> ().arrow.gameObject.SetActive(true);
		}
		else
		{
			_selectChindObj.GetComponent<CompoundSkillCellUI> ().arrow.gameObject.SetActive(false);	
			_selectChindObj = obj.gameObject;
			_selectChindObj.GetComponent<CompoundSkillCellUI> ().arrow.gameObject.SetActive(true);
		}

		if(cell == null)
		{
			return;
		}
		updateCompoundInfo (cell);
		updateCompoundPropInfo(cell);
		
	}

	private void OnOutGem(ButtonScript obj, object args, int param1, int param2) 
	{
		CompoundInfoObj.SetActive (true);
		//itemGridList.gameObject.SetActive (false);
		_gemItem.cellPane.gameObject.SetActive(false);
		_gemId = 0;
		//gemNameLab.text = LanguageManager.instance.GetValue("inlaygem");
		gemName.text = "";
		gemName.gameObject.SetActive (false);
		gemInfoObj.gameObject.SetActive (false);
	}


	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		isOpen = false;

		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			if(chindCellPoolList.Count >0 )
			{
				for(int i= 0;i< chindCellPoolList.Count;i++)
				{
					Destroy(chindCellPoolList[i]);
					chindCellPoolList[i] = null;
				}
				chindCellPoolList.Clear();
			}
			Hide ();	
		});
	}

	private void OnGem(ButtonScript obj, object args, int param1, int param2) 
	{

		if(_gemId != 0)
		{
			_gemItem.cellPane.gameObject.SetActive(false);
			_gemId = 0;
			gemName.text = "";
			gemNameLab.text = LanguageManager.instance.GetValue("inlaygem");

		}

		CompoundSkillCellUI cellUI = _selectChindObj.GetComponent<CompoundSkillCellUI> ();
		if (cellUI == null)
			return;
		if( MakeData.GetData (cellUI.MakeId).skillLevel >= 40 && !CompoundSystem.instance.GetIsOPenEquip((uint)cellUI.MakeId))
			return;
		if(gemPanel.gameObject.activeSelf)
		{
			return;
		}
		gemPanel.gameObject.SetActive (true);
		compoundList.SetActive (false);
		UpdataGemList ();
	}

	private void OnCloseGem(ButtonScript obj, object args, int param1, int param2)
	{
		gemPanel.gameObject.SetActive (false);
		compoundList.SetActive (true);
		//itemGridList.gameObject.SetActive (true);

        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMakeGemUIClose);
        }, 1);
	}

	private void OnNeedItem(ButtonScript obj, object args, int param1, int param2)
	{
		int itemId = int.Parse (obj.name);
		ItemData itemD = ItemData.GetData (itemId);
		if (itemD == null)
			return;

		if(TipsItemUI.instance != null)
			TipsItemUI.instance.HideTips();
		infoTipsUI.gameObject.SetActive (true);
		infoTipsUI.buyBtn.gameObject.SetActive(false);
		infoTipsUI.item = itemD;

		if(itemId == 5086)
		{
			infoTipsUI.getBtn.gameObject.SetActive(false);
			infoTipsUI.buyBtn.gameObject.SetActive(true);
			UIManager.SetButtonEventHandler (infoTipsUI.buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyItem, 0, 0);
		}
	


		/*GatherData gather = GatherData.GetData (itemId);
		if (gather == null)

			return;

		MessageBoxUI.ShowMe ( LanguageManager.instance.GetValue("gatherthis"), () => {

			NetConnection.Instance.startMining (itemId);

			GatherSystem.instance.IsMineing = true;
			SkillControlUI.Instance.makeGather();

		});
		*/

	}
	private void OnBuyItem(ButtonScript obj, object args, int param1, int param2)
	{
		int shopid = ShopData.GetShopId (5086);
		infoTipsUI.gameObject.SetActive (false);
		QuickBuyUI.ShowMe(shopid);
	}

	private void OnEquipPoBiao(ButtonScript obj, object args, int param1, int param2)
	{
		if(!showPBprop)
		{
			showPBprop = true;
			BtnArrTop.gameObject.SetActive(true);
			BtnArr.gameObject.SetActive(false);
			PBPropImg.gameObject.SetActive(false);

		}
		else
		{
			showPBprop = false;
			BtnArrTop.gameObject.SetActive(false);
			BtnArr.gameObject.SetActive(true);
			PBPropImg.gameObject.SetActive(true);
			updateCompoundPropInfo(_selectSkillCell);
		}

	
	}

	private void OnEquipType(ButtonScript obj, object args, int param1, int param2)
	{
		listBar.value = 0;
		SetBtnEnabled (true);
		obj.GetComponent<UIButton> ().isEnabled = false;
		_selectEquipType = param1;
		CompoundInfoObj.SetActive (false);

		if(_chindItemCellObj != null && _chindItemCellObj != obj)
		{
			_chindItemCellObj.transform.FindChild("bg").FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao";
			_chindItemCellObj = null;
		}
		
		foreach(GameObject c in chindCellList)
		{
			skillGrid.RemoveChild(c.transform);
			c.transform.parent = null;
			c.gameObject.SetActive(false);
			chindCellPoolList.Add(c);
		}
		chindCellList.Clear ();
		_chindItemCellObj = null;

		if(_selectChindObj != null)
		{
			_selectChindObj.GetComponent<CompoundSkillCellUI> ().arrow.gameObject.SetActive(false);	
			_selectChindObj = null;
		}
		if(param1 == 8)
		{
			_bJobEquip = true; 
		}
		else
		{
			_bJobEquip = false;
		}
	}

	private void OnCompoundOk(ButtonScript obj, object args, int param1, int param2)
	{
		compoundOkPanel.gameObject.SetActive (false);
	}

	private void OnCompound(ButtonScript obj, object args, int param1, int param2)
	{
		if(isItemEnough)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("shifouduanzao"),()=>{

			if(TipsItemUI.instance != null)
				TipsItemUI.instance.HideTips();

			NetConnection.Instance.compoundItem(_selectSkillCell.MakeId,_gemId);
			});
		}
		else
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("compoundNot"));
			return;
		}
	}
	
	private void OnBuyGem(ButtonScript obj, object args, int param1, int param2)
	{
		StoreUI.SwithShowMe (2);
	}

	private void OnJobBtn(ButtonScript obj, object args, int param1, int param2)
	{

	} 

	private void MakeOk(COM_Item item)
	{
        GuideManager.Instance.ClearMask();
		topImg.gameObject.SetActive (true);
		PBPropImg.gameObject.SetActive(false);
		PropImg.gameObject.SetActive(false);
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_chilun, transform,()=>{
			topImg.gameObject.SetActive (false);
			compoundOkPanel.gameObject.SetActive(true);
			PBPropImg.gameObject.SetActive(true);
			PropImg.gameObject.SetActive(true);
			compoundOkPanel.GetComponent<BagTipsUI>().Item = item;
			if(item.itemId_ == MakeData.GetData(_selectSkillCell.MakeId).specialID_)
			{
				EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_juepinItem, transform,null);//()=>{});
			}
		});

		MakeData make = MakeData.GetData (_selectSkillCell.MakeId);
		for(int i=0;i<make.needItems.Length;i++)
		{
			int itemCount = BagSystem.instance.GetItemMaxNum(uint.Parse(make.needItems[i]));
			needItemsIcon[i].gameObject.transform.Find("num").GetComponent<UILabel>().text = itemCount.ToString()+"/"+make.needItemNum[i];
			needItemsIcon[i].gameObject.transform.Find("num").GetComponent<UILabel>().color = Color.white;
			
			if(itemCount < int.Parse(make.needItemNum[i]))
			{
				needItemsIcon[i].gameObject.transform.Find("num").GetComponent<UILabel>().color = Color.red;
				isItemEnough = false;
			}
		}
		if(make.needMoney > GamePlayer.Instance.GetIprop(PropertyType.PT_Money))
		{
			compoundBtn.isEnabled = false;
		}

		//gemIcon.spriteName = "";
		//ItemIcon = 0;
		if(_gemItem != null && _gemItem.cellPane != null)
			_gemItem.cellPane.gameObject.SetActive (false);
		gemInfoObj.gameObject.SetActive (false);
		_gemId = 0;
		gemName.text = "";
		gemNameLab.text = LanguageManager.instance.GetValue("inlaygem");
		UpdataGemList ();

		foreach(var x in chindCellList)
		{
			x.GetComponent<CompoundSkillCellUI>().updateRed();
		}

      GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_MainMakeItemOk);
	}

	public void UpdateAddMakeItem(COM_Item item)
	{
		UpdateNeedMakeItem (null);
	}


	public void UpdateNeedMakeItem(List<COM_DropItem> items)
	{
		CompoundSkillCellUI cell = _selectChindObj.GetComponent<CompoundSkillCellUI> ();
		if(cell == null)
		{
			return;
		}
		updateCompoundInfo (cell);
	}

	void UpdateMoney()
	{
		MakeData make = MakeData.GetData (_selectSkillCell.MakeId);
		if(make.needMoney > GamePlayer.Instance.GetIprop(PropertyType.PT_Money))
		{
			compoundBtn.isEnabled = false;
		}
	}

	protected override void DoHide ()
	{
        GatherSystem.instance.CompoundOkEvent -= MakeOk;
		GamePlayer.Instance.OnIPropUpdate -= UpdateMoney;
		GatherSystem.instance._GatherItemHandler -= UpdateNeedMakeItem;
		BagSystem.instance.UpdateItemEvent -= UpdateAddMakeItem;
		BagSystem.instance.ItemChanged -= UpdateAddMakeItem;
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainMakeCompoundBtn);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainMakeGemBtn);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainMakeGemClose);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainMakeSubFirst);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainMakeSubSecond);
        GuideManager.Instance.RemoveGuideAim(GuideAimType.GAT_MainMakeSubThird);
		base.DoHide ();
	}

	private int SortMakes(MakeData  a,MakeData b)
	{
		if (a.skillId < b.skillId)
		{
			return -1;
		}
		else
		{
			return 1;
		}
	}

	private void SetBtnEnabled(bool show)
	{
		wuQiBtn.isEnabled = show;
		maoZiBtn.isEnabled = show;
		kuiJiaBtn.isEnabled = show;
		xieZiBtn.isEnabled = show;
		dunPaiBtn.isEnabled = show;
		jobEquipBtn.isEnabled = show;
	}

	private void SelectItem()
	{
		levelBtnCellList[0].gameObject.transform.FindChild("arrows").GetComponent<UISprite>().spriteName = "sanjiao2";
		int indx = skillGrid.GetIndex (levelBtnCellList[0].gameObject.transform);
		int pLevel = 1;// GamePlayer.Instance.GetIprop (PropertyType.PT_Level) / 10 + 1;
		if(pLevel== 1)
		{
			pLevel = 0;
		}

		Profession pro =  Profession.get((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession),GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
		ItemSubType[] items = pro.CanUsedItems(GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)*2+2);
		
		foreach(MakeData m in MakeData.metaData.Values)
		{
			if((pLevel <= m.skillLevel && m.skillLevel <= pLevel +9) && m.type_ == "Player" )
			{
				for(int i =0 ;i<items.Length;i++)
				{
					if(ItemData.GetData(m.itemId_).subType_ == items[i])
					{
						
						GameObject objCell = null;
						if(chindCellPoolList.Count>0)
						{
							objCell = chindCellPoolList[0];
							chindCellPoolList.Remove(objCell);  
						}
						else  
						{
							objCell = Object.Instantiate(chindItemCell.gameObject) as GameObject;
						}
						
						UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickChindItem,0, 0);
						CompoundSkillCellUI cell = objCell.GetComponent<CompoundSkillCellUI>();
						cell.SkillId = m.skillId;
						cell.MakeId = m.itemId_;
						cell.gameObject.transform.FindChild("recommend").GetComponent<UISprite>().gameObject.SetActive(true);
						skillGrid.AddChild(objCell.transform,++indx);
						objCell.SetActive(true);
						objCell.transform.localScale = Vector3.one;
						chindCellList.Add(objCell);
					}
				}
			}
		}


		CompoundSkillCellUI cell1 = chindCellList[0].gameObject.GetComponent<CompoundSkillCellUI> ();
		if (_selectChindObj == chindCellList[0].gameObject)
			return;
		if(_selectChindObj == null)
		{
			_selectChindObj = chindCellList[0].gameObject;
			chindCellList[0].GetComponent<CompoundSkillCellUI> ().arrow.gameObject.SetActive(true);
		}
		else
		{
			_selectChindObj.GetComponent<CompoundSkillCellUI> ().arrow.gameObject.SetActive(false);	
			_selectChindObj = chindCellList[0].gameObject;
			_selectChindObj.GetComponent<CompoundSkillCellUI> ().arrow.gameObject.SetActive(true);;
		}
		
		if(cell1 == null)
		{
			return;
		}
		updateCompoundInfo (cell1);
		updateCompoundPropInfo(cell1);

	}
}

