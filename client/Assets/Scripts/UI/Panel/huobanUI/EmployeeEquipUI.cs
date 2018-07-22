using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EmployeeEquipUI : MonoBehaviour
{
	public GameObject itemCell; 
	public List<UITexture> equipList = new List<UITexture> ();
	public List<UISprite> equipListBack = new List<UISprite> ();
	private Employee curEmployee;
	public Transform mpos;
	private GameObject babyObj;
	public UILabel employessNumLab;


	public UISprite itemIcon;
	public UILabel itemName;
	public UILabel itemLevel;
	public UILabel itemNum;
	public UILabel needMoneyLab;
	public UIButton starUpBtn;
	public UIButton makeBtn;
	public UIButton equipBtn;
	public UILabel jobLab;
	public UILabel nameLab;
	public UISprite jobIcon;
	public List<UISprite> star = new List<UISprite> ();
	public List<UISprite> makeItem = new List<UISprite> ();
	public UIGrid grid;
	public compoundNeedItemInfoUI infoTipsUI;
	public GameObject propCell;
	public GameObject equipInfoPane;
	public UIButton closeBtn;
	public UISprite blackImg;
	public List<UISprite> canArrowEquipList = new List<UISprite> ();
	private int selectEquipId;
	private EmployeeConfigData _employeeConfigData;
	private bool _isequipFull;
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();
	private List<string> _icons = new List<string>();
	bool hasDestroy;

	void Start()
	{
		GamePlayer.Instance.EmployeeEquipEvent += new WearEquipEventHandler (OnWearEquip);
		GamePlayer.Instance.EmployeeStarOkEnvent += new RequestEventHandler<Employee> (OnEmployeeStarOk);
		GatherSystem.instance.CompoundOkEvent += new RequestEventHandler<COM_Item> (MakeOk);
		BagSystem.instance.DelItemEvent += new ItemDelEventHandler(OnDelItem);

		UIManager.SetButtonEventHandler (starUpBtn.gameObject, EnumButtonEvent.OnClick, OnStarUp, 0, 0);
		UIManager.SetButtonEventHandler (makeBtn.gameObject, EnumButtonEvent.OnClick, OnMake, 0, 0);
		UIManager.SetButtonEventHandler (equipBtn.gameObject, EnumButtonEvent.OnClick, OnEquip, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		GatherSystem.instance._GatherItemHandler += new GatherItemHandler(UpdateNeedMakeItem);
		BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (UpdateAddMakeItem);
		BagSystem.instance.ItemChanged += new ItemChangedEventHandler (UpdateAddMakeItem);
		for(int i=1;i<equipList.Count;i++)
		{
            if (i == 6)
            {
                // 注册衣服的槽位
                GuideManager.Instance.RegistGuideAim(equipList[i].gameObject, GuideAimType.GAT_PartnerDetailBodySlot);
            }
			UIManager.SetButtonEventHandler (equipList[i].gameObject, EnumButtonEvent.OnClick, OnPlayerEquip, 0, 0);
		}

		//for(int m=0;m<makeItem.Count;m++)
	//	{
	//		UIManager.SetButtonEventHandler (makeItem[m].gameObject, EnumButtonEvent.OnClick, OnMakeItem, 0, 0);
	//	}

		UpdataPanel ();

        // 注册装备按钮
       

		//GuideManager.Instance.RegistGuideAim(equipList[6].gameObject, GuideAimType.GAT_PartnerDetailBodySlot);
        ///伙伴操作界面打开事件
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerDetailUIOpen);
	}



	public void UpdataPanel()
	{
		curEmployee =  EmployessSystem.instance.CurEmployee;
		if(curEmployee == null)
		{
			return;
		}



		employessNumLab.text = curEmployee.GetIprop (PropertyType.PT_FightingForce).ToString ();
		nameLab.text = curEmployee.InstName;
		jobIcon.spriteName = ((JobType)curEmployee.GetIprop (PropertyType.PT_Profession)).ToString ();
		jobLab.text =  Profession.get((JobType)curEmployee.GetIprop(PropertyType.PT_Profession), 
		                                 curEmployee.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;

		for(int i =0;i<star.Count;i++)
		{
			star[i].gameObject.SetActive(false);
		}
		
		for(int j =0;j<curEmployee.star_ && j<5;j++)
		{
			star[j].gameObject.SetActive(true);
		}

		//if(babyObj != null)
		//{
			//Destroy (babyObj);

			//babyObj = null;
		//}

		//GameManager.Instance.GetActorClone((ENTITY_ID) EmployeeData.GetData(curEmployee.GetIprop(PropertyType.PT_TableId)).asset_id, 
		                           //     (ENTITY_ID)curEmployee.WeaponAssetID, EntityType.ET_Emplyee, AssetLoadCallBack, new ParamData(curEmployee.InstId),"UI");

		EmployeeConfig = EmployeeConfigData.GetData (curEmployee.GetIprop(PropertyType.PT_TableId),(int)curEmployee.star_-1);
		if (curEmployee.star_ >= 6)
			starUpBtn.gameObject.SetActive (false);

		UpdateEquips();
	}

	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		if (gameObject == null || !this.gameObject.activeSelf)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}

		if(babyObj != null)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		//NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		ro.transform.parent = mpos;
		ro.transform.localScale = new Vector3(400f,400f,400f);
		//ro.transform.localPosition = Vector3.zero;
		ro.transform.localPosition = Vector3.forward * -40;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		//EffectLevel el =ro.AddComponent<EffectLevel>();
		//el.target =ro.transform.parent.parent.GetComponent<UISprite>();
		babyObj = ro;
	}

	private void UpdateEquips()
	{
		_isequipFull = true;
		for(int i=0;i<EmployeeConfig.items.Count;i++ )
		{
			ItemData edata = ItemData.GetData(EmployeeConfig.items[i]);

			if(ISHaveEquip(EmployeeConfig.items[i]))
			{
				//equipList[(int)edata.slot_].color = Color.white;
				equipList[(int)edata.slot_].gameObject.SetActive(true);
				HeadIconLoader.Instance.LoadIcon(edata.icon_, equipList[(int)edata.slot_]);
				
				if(!_icons.Contains(edata.icon_))
				{
					_icons.Add(edata.icon_);
				}
				equipList[(int)edata.slot_].gameObject.transform.parent.GetComponent<UISprite>().spriteName = "bb_daojukuang1";
				canArrowEquipList[(int)edata.slot_].gameObject.SetActive(false);
			}
			else
			{
				//equipList[(int)edata.slot_].color = new Color(0f,1f,1f);
				//equipList[(int)edata.slot_].gameObject.SetActive(false);
				equipList[(int)edata.slot_].mainTexture = null;
				equipList[(int)edata.slot_].gameObject.transform.parent.GetComponent<UISprite>().spriteName = "bb_weixide";
				if(BagSystem.instance.GetItemCount((uint)EmployeeConfig.items[i]) > 0)
				{
					canArrowEquipList[(int)edata.slot_].gameObject.SetActive(true);
				}
				else
				{
					if(EquipISCanMake(EmployeeConfig.items[i]))
					{
						canArrowEquipList[(int)edata.slot_].gameObject.SetActive(true);
					}
					else
					{
						canArrowEquipList[(int)edata.slot_].gameObject.SetActive(false);
					}
				}

				_isequipFull = false;
			}
		}

		if(_isequipFull)
		{
			starUpBtn.isEnabled = true;
		}
		else
		{
			starUpBtn.isEnabled = false;
		}
	}


	private bool ISHaveEquip(int itemId)
	{
		for(int j=1;j<curEmployee.Equips.Length;j++)
		{
			if(curEmployee.Equips[j]!= null && curEmployee.Equips[j].itemId_ == itemId)
			{
				return true;
			}
		}

		return false;
	}

	



	public int EquipItemid
	{
		set
		{
			ItemData idata = ItemData.GetData(value);
			if(idata != null)
			{
				selectEquipId = value;
				ItemCellUI item =  UIManager.Instance.AddItemCellUI( itemIcon,(uint)value);
				itemName.text = idata.name_ ;
				itemLevel.text= idata.level_.ToString();
				itemNum.text = BagSystem.instance.GetItemMaxNum((uint)value).ToString();
			
					foreach( GameObject o in cellList)
					{
						grid.RemoveChild(o.transform);
						o.transform.parent = null;
						o.gameObject.SetActive(false);
						cellPool.Add(o);
					}
					cellList.Clear ();
					
					for(int i=0;i<idata.propArr.Count;i++)
					{
						
						if(idata.propArr[i].Key == PropertyType.PT_Durability)
							continue;

						GameObject objCell = null;
						if(cellPool.Count>0)
						{
							objCell = cellPool[0];
							cellPool.Remove(objCell);  
							UIManager.RemoveButtonAllEventHandler(objCell);
						}
						else  
						{
							objCell = Object.Instantiate(propCell) as GameObject;
						}
						string sNum ="";
						string sNum1 ="";
						if(int.Parse(idata.propArr[i].Value[0]) > 0)
						{
							sNum =" +"+ int.Parse(idata.propArr[i].Value[0]);
						}
						else
						{
							sNum = " "+ int.Parse(idata.propArr[i].Value[0]);
						}

					objCell.transform.FindChild("name").GetComponent<UILabel>().text  =
					LanguageManager.instance.GetValue(idata.propArr[i].Key.ToString())+ sNum;// + " - "+sNum1;
						
					if(int.Parse(idata.propArr[i].Value[0]) < 0)
					{
						objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.grey;
					}
					else if(int.Parse(idata.propArr[i].Value[0]) >= 15 && int.Parse(idata.propArr[i].Value[0]) <= 17)
					{
						objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.black;
					}
					else if(int.Parse(idata.propArr[i].Value[0]) >= 18 && int.Parse(idata.propArr[i].Value[0]) <= 19)
					{
						objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.green;
					}
					else if(int.Parse(idata.propArr[i].Value[0]) >= 20 && int.Parse(idata.propArr[i].Value[0]) <= 21)
					{
						objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.blue;
					}
					else if(int.Parse(idata.propArr[i].Value[0]) >= 22 && int.Parse(idata.propArr[i].Value[0]) <= 23)
					{
						objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.magenta;
					}
					else 
					{
						objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.white;
						objCell.transform.FindChild("name").GetComponent<UILabel>().text = "[FECE29]" + objCell.transform.FindChild("name").GetComponent<UILabel>().text;
					}



						/*if(int.Parse(idata.propArr[i].Value[1]) > 0)
						{
							sNum1 = " +"+ int.Parse(idata.propArr[i].Value[1]);
						}
						else
						{
							sNum1 = " "+ int.Parse(idata.propArr[i].Value[1]);
						}
						*/
						
						objCell.transform.parent = grid.transform;
						objCell.gameObject.SetActive(true);	
						objCell.transform.localScale = Vector3.one;
						cellList.Add(objCell);
					}
					grid.Reposition();


				MakeData mData = MakeData.GetData(idata.id_);
				needMoneyLab.text  =  mData.needMoney.ToString();
				for(int s = 0; s<makeItem.Count;s++)
				{
					makeItem[s].gameObject.SetActive(false);
				}

				makeBtn.isEnabled = true;
				for(int m = 0; m<mData.needItems.Length;m++)
				{
					makeItem[m].gameObject.SetActive(true);
					ItemCellUI iCell =  UIManager.Instance.AddItemCellUI(makeItem[m],uint.Parse(mData.needItems[m]));
					makeItem[m].name  = mData.needItems[m];
					UIManager.SetButtonEventHandler (iCell.gameObject, EnumButtonEvent.OnClick, OnMakeItem, int.Parse(mData.needItems[m]), 0);
				//	iCell.showTips = true;
					makeItem[m].transform.FindChild("name").GetComponent<UILabel>().text = ItemData.GetData(int.Parse(mData.needItems[m])).name_;
					int hNum = BagSystem.instance.GetItemMaxNum(uint.Parse(mData.needItems[m]));
					int nNum =  int.Parse(mData.needItemNum[m]);
					if(hNum < nNum)
					{
						makeItem[m].transform.FindChild("Label").GetComponent<UILabel>().color = Color.red;
						makeBtn.isEnabled = false;
					}
					else
					{
						makeItem[m].transform.FindChild("Label").GetComponent<UILabel>().color = Color.white;
					}
					makeItem[m].transform.FindChild("Label").GetComponent<UILabel>().text  = hNum +"/" + nNum;
				}

				if(ISHaveEquip(EquipItemid))
				{
					equipBtn.isEnabled = false;
				}
				else
				{
					equipBtn.isEnabled = true;
				}

				COM_Item bItem = BagSystem.instance.GetItemByItemId ((uint)EquipItemid);
				if(bItem == null)
				{
					equipBtn.isEnabled = false;
				}
				else
				{
					equipBtn.isEnabled = true;
				}

				if(ISHaveEquip(selectEquipId))
				{
					equipBtn.isEnabled = false;
				}
				if(ISHaveEquip(selectEquipId) || BagSystem.instance.GetItemByItemId ((uint)selectEquipId)!= null || GamePlayer.Instance.GetIprop(PropertyType.PT_Money) < mData.needMoney)
				{
					makeBtn.isEnabled = false;
				}
			}
		}
		get
		{
			return selectEquipId;
		}
	}

	public void UpdateAddMakeItem(COM_Item item)
	{
		UpdateNeedMakeItem (null);
		UpdateEquips();
	}

	public void UpdateNeedMakeItem(List<COM_DropItem> items)
	{
		ItemData idata = ItemData.GetData(EquipItemid);
		MakeData mData = MakeData.GetData(idata.id_);
		makeBtn.isEnabled = true;
		for(int m = 0; m<mData.needItems.Length;m++)
		{
			makeItem[m].gameObject.SetActive(true);
			ItemCellUI iCell =  UIManager.Instance.AddItemCellUI(makeItem[m],uint.Parse(mData.needItems[m]));
			makeItem[m].name  = mData.needItems[m];
			UIManager.SetButtonEventHandler (iCell.gameObject, EnumButtonEvent.OnClick, OnMakeItem, int.Parse(mData.needItems[m]), 0);
			//	iCell.showTips = true;
			makeItem[m].transform.FindChild("name").GetComponent<UILabel>().text = ItemData.GetData(int.Parse(mData.needItems[m])).name_;
			int hNum = BagSystem.instance.GetItemMaxNum(uint.Parse(mData.needItems[m]));
			int nNum =  int.Parse(mData.needItemNum[m]);
			if(hNum < nNum)
			{
				makeItem[m].transform.FindChild("Label").GetComponent<UILabel>().color = Color.red;
				makeBtn.isEnabled = false;
			}
			else
			{
				makeItem[m].transform.FindChild("Label").GetComponent<UILabel>().color = Color.white;
			}
			makeItem[m].transform.FindChild("Label").GetComponent<UILabel>().text  = hNum +"/" + nNum;
		}
	}

	private EmployeeConfigData EmployeeConfig
	{
		set
		{
			if(value != null)
			{
				_employeeConfigData = value;

			/*	for(int e= 0;e<value.items.Count;e++)
				{
					ItemData edata = ItemData.GetData(value.items[e]);
					equipList[(int)edata.slot_].gameObject.SetActive(false);
					HeadIconLoader.Instance.LoadIcon(ItemData.GetData(value.items[e]).icon_, equipList[(int)edata.slot_]);

					if(!_icons.Contains(ItemData.GetData(value.items[e]).icon_))
					{
						_icons.Add(ItemData.GetData(value.items[e]).icon_);
					}
					equipList[(int)edata.slot_].gameObject.SetActive(false);
				}
				*/
				EquipItemid = value.items [0];
			}
		}
		get
		{
			return _employeeConfigData;
		}
	}


	public void OnWearEquip(uint target, COM_Item equip)
	{
		if(target == curEmployee.InstId)
		{
			UpdateEquips();
			curEmployee = GamePlayer.Instance.GetEmployeeById ((int)target);
			EmployessSystem.instance.CurEmployee = curEmployee;
			EquipItemid = EquipItemid;
		}
	}


	void OnEmployeeStarOk(Employee inst)
	{
		curEmployee = inst;
		EmployessSystem.instance.CurEmployee = curEmployee;
		PopText.Instance.Show (LanguageManager.instance.GetValue("shengxingok"));
	//	EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_JinjieSuccess, gameObject.transform);
		blackImg.gameObject.SetActive (true);
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_employeeStarOk, gameObject.transform,()=>{blackImg.gameObject.SetActive (false);});
		UpdataPanel ();
	}
	private void MakeOk(COM_Item item)
	{
		//EffectAPI.PlayUIEffect ((EFFECT_ID)GlobalValue.EFFECT_EquipCombie, transform);
		PopText.Instance.Show (LanguageManager.instance.GetValue ("huanbanEquipok"));

		EquipItemid = EquipItemid; 
	}

	public void ClosePanel()
	{
		GamePlayer.Instance.EmployeeEquipEvent -= OnWearEquip;
		GamePlayer.Instance.EmployeeStarOkEnvent -= OnEmployeeStarOk;
		GatherSystem.instance.CompoundOkEvent -= MakeOk;
		BagSystem.instance.DelItemEvent -= OnDelItem;
		GatherSystem.instance._GatherItemHandler -= UpdateNeedMakeItem;
		BagSystem.instance.UpdateItemEvent -= UpdateAddMakeItem;
		BagSystem.instance.ItemChanged -= UpdateAddMakeItem;
		Destroy (babyObj);
		babyObj = null;
	}

	private void OnStarUp(ButtonScript obj, object args, int param1, int param2)
	{
		if (curEmployee.star_ >= 6)
			return;
		NetConnection.Instance.requestUpStar ((uint)curEmployee.InstId);
	}

	private void OnMake(ButtonScript obj, object args, int param1, int param2)
	{
		//CompoundUI.SwithShowMe ();
		NetConnection.Instance.compoundItem(EquipItemid,0);
	}

	private void OnEquip(ButtonScript obj, object args, int param1, int param2)
	{
		if (_isequipFull)
			return;

		COM_Item bItem = BagSystem.instance.GetItemByItemId ((uint)EquipItemid); 
		if(bItem == null)
		{
			return;
		}

		if(ISHaveEquip((int)bItem.itemId_))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("ciwupinyizhuangbei"));
			return;
		}

		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) /10 +1 <  ItemData.GetData((int)bItem.itemId_).level_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
			return;
		}
		
		/*JobType jt = (JobType)curEmployee.GetIprop(PropertyType.PT_Profession);
		int level = curEmployee.GetIprop(PropertyType.PT_ProfessionLevel);
		Profession profession = Profession.get(jt, level);
		if (null == profession)
			return;
		if (!profession.canuseItem(ItemData.GetData((int)bItem.itemId_).subType_, ItemData.GetData((int)bItem.itemId_).level_))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("noprofession"));
			return;
		}
		*/
		NetConnection.Instance.wearEquipment ((uint)curEmployee.InstId, bItem.instId_);

        // 点击此装备成功
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerDetailEquipSucc);
	}

	private void OnMakeItem(ButtonScript obj, object args, int param1, int param2)
	{
		infoTipsUI.gameObject.SetActive (true);
		ItemData itemD = ItemData.GetData (param1);
		if(itemD == null)
		{
			infoTipsUI.gameObject.SetActive (false);
			return;
		}
		infoTipsUI.item = itemD;
		infoTipsUI.buyBtn.gameObject.SetActive(false);
		if(param1 == 5086)
		{
			infoTipsUI.getBtn.gameObject.SetActive(false);
			infoTipsUI.buyBtn.gameObject.SetActive(true);
			UIManager.SetButtonEventHandler (infoTipsUI.buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyItem, 0, 0);
		}
	}
	
	private void OnBuyItem(ButtonScript obj, object args, int param1, int param2)
	{
		int shopid = ShopData.GetShopId (5086);
		infoTipsUI.gameObject.SetActive (false);
		QuickBuyUI.ShowMe(shopid);
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		equipInfoPane.gameObject.SetActive (false);
	}
	
	private void OnPlayerEquip(ButtonScript obj, object args, int param1, int param2)
	{
		GuideManager.Instance.ClearMask ();
		equipInfoPane.gameObject.SetActive (true);
		GuideManager.Instance.RegistGuideAim(equipBtn.gameObject, GuideAimType.GAT_PartnerDetailEquipBtn);
		int indx = equipList.IndexOf (obj.gameObject.GetComponent<UITexture> ());
		if (indx < 0)
			return;

		for(int i= 0;i< EmployeeConfig.items.Count;i++)
		{
			if((int)ItemData.GetData(EmployeeConfig.items[i]).slot_ == indx)
			{
				EquipItemid = EmployeeConfig.items[i];
				break;
			}
		}


        // 抛是否可装备 
        //equipBtn.isEnabled
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerDetailEquipClick, equipBtn.isEnabled? 1: 0);
	}
	private void OnDelItem(uint slot)   
	{
		EquipItemid = EquipItemid;
	}
	void OnDestroy()
	{

		hasDestroy = true;
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
		//PlayerAsseMgr.DeleteAsset ((ENTITY_ID)EmployeeData.GetData(curEmployee.GetIprop(PropertyType.PT_TableId)).asset_id, false);
	}



	bool EquipISCanMake(int id)
	{

		MakeData mData = MakeData.GetData (id);
		bool bCan = true;
		for(int m = 0; m<mData.needItems.Length;m++)
		{
			int hNum = BagSystem.instance.GetItemMaxNum(uint.Parse(mData.needItems[m]));
			int nNum =  int.Parse(mData.needItemNum[m]);
			if(hNum < nNum)
			{
				return  false;
			}
		}
		return bCan;
	}
}

