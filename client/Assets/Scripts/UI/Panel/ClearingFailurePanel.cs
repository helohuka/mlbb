using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ClearingFailurePanel : UIBase {

	private COM_DropItem[] items;
	public UIGrid grid;
	public GameObject item;
	// Use this for initialization
	public UIButton closeBtn;
	void Start () {
		item.SetActive (false);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		items = Battle.Instance.BattleReward.items_;
		//BagSystem.instance.ItemChanged += OnItemAdd;
		//AddPropsItems (items);
		ShowBtn ();
		GlobalInstanceFunction.Instance.Invoke(() => { OnClickclose(null, null, 0, 0); }, 5f);
	}
	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void ShowBtn()
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Free)>0)
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			UILabel[] las = o.GetComponentsInChildren<UILabel>();
			//la.text = ida[i].itemNum_.ToString();
			UISprite sp = o.GetComponentInChildren<UISprite>();
			sp.spriteName = "";

			UITexture sps = o.GetComponentInChildren<UITexture>();
			sps.enabled = true;
			HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)).assetsIocn_, sps);
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("itemnameLabel"))
				{
					la.text = "分配属性";
				}
				
			}
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickbtn, 0, 0);
			o.transform.parent = grid.transform;
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			grid.repositionNow = true;

		}
		if(IsForBattle())
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			UILabel[] las = o.GetComponentsInChildren<UILabel>();
			UITexture spt = o.GetComponentInChildren<UITexture>();
			spt.enabled = false;
			UISprite sps = o.GetComponentInChildren<UISprite>();

			sps.enabled = true;
			sps.spriteName = "chongwu";
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("itemnameLabel"))
				{
					la.text = "分配宠物属性";
				}
				
			}
			
			o.transform.parent = grid.transform;
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickbtn, 1, 0);
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			grid.repositionNow = true;
		}
		if(isNewEquip())
		{
			addEquipsBtn();
		}
		if(EmployessSystem.instance.GetBattleEmpty())
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			UILabel[] las = o.GetComponentsInChildren<UILabel>();
			UITexture spt = o.GetComponentInChildren<UITexture>();
			spt.enabled = false;
			UISprite sps = o.GetComponentInChildren<UISprite>();
			sps.enabled = true;
			sps.spriteName = "huoban";
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("itemnameLabel"))
				{
					la.text = "上阵新伙伴";
				}
				
			}
			
			o.transform.parent = grid.transform;
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickbtn, 3, 0);
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			grid.repositionNow = true;
		}
		//else
		if(EmployessNewItem())
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			UILabel[] las = o.GetComponentsInChildren<UILabel>();
			UITexture spt = o.GetComponentInChildren<UITexture>();
			spt.enabled = false;
			UISprite sps = o.GetComponentInChildren<UISprite>();
			sps.enabled = true;
			sps.spriteName = "huoban";
			foreach(UILabel la in las)
			{
				if(la.gameObject.name.Equals("itemnameLabel"))
				{
					la.text = "伙伴装备";
				}
				
			}
			
			o.transform.parent = grid.transform;
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickbtn, 4, 0);
			o.transform.localPosition = new Vector3(0,0,0);
			o.transform.localScale= new Vector3(1,1,1);	
			grid.repositionNow = true;
		}
	}
    

	bool EmployessNewItem()
	{

		uint[] bemps = GamePlayer.Instance.GetEmployeesBattles((int)GamePlayer.Instance.CurEmployeesBattleGroup);
		if(bemps == null)
			return false;

		for(int i =0;i<bemps.Length;i++)
		{
			if(bemps[i] == 0)
			{
				continue;
			}
			//index=i;
			Employee emp = GamePlayer.Instance.GetEmployeeById((int)bemps[i]);
			if(emp == null)
				continue;
			int newStart = emp.GetIprop(PropertyType.PT_Level)/10+2;
			if((int)emp.star_<newStart)
			{
				return true;
			}
		}
		return false;
	}

	void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(param1 ==0)
		{
			PlayerPropertyUI.SwithShowMe();
		}else
			if(param1 == 1)
		{
			MainbabyUI.SwithShowMe();
		}
		else
			if(param1 == 2)
		{

		}else
			if(param1 == 3)
		{
			EmployessControlUI.SwithShowMe(3);
		}else
			if(param1 == 4)
		{
			EmployessControlUI.SwithShowMe();
		}

		//Hide ();
	}
//	void addbtn()
//	{
//		GameObject o = GameObject.Instantiate(item)as GameObject;
//		o.SetActive(true);
//		UILabel[] las = o.GetComponentsInChildren<UILabel>();
//		UITexture spt = o.GetComponentInChildren<UITexture>();
//		spt.enabled = false;
//		UISprite sps = o.GetComponentInChildren<UISprite>();
//		sps.enabled = true;
//		sps.spriteName = "zjm_huoban";
//		foreach(UILabel la in las)
//		{
//			if(la.gameObject.name.Equals("itemnameLabel"))
//			{
//				la.text = "上阵新伙伴";
//			}
//			
//		}
//		o.transform.parent = grid.transform;
//		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickbtn, 3, 0);
//		o.transform.localPosition = new Vector3(0,0,0);
//		o.transform.localScale= new Vector3(1,1,1);	
//		grid.repositionNow = true;
//
//	}
//	void OnBetterPartnerEquip(COM_Item item)
//	{
//		ItemData data = ItemData.GetData((int)item.itemId_);
//		if (data.mainType_ != ItemMainType.IMT_Equip)
//			return;
//		
//		List<Employee> emp = GamePlayer.Instance.GetBattleEmployees();
//		float itemForce = Define.CALC_BASE_FightingForce(item);
//		for (int i = 0; i < emp.Count; ++i)
//		{
//			float equiForce = Define.CALC_BASE_FightingForce(emp[i].Equips[(int)data.slot_]);
//			if (itemForce > equiForce)
//			{
//				addbtn();
//				break;
//			}
//		}
//	}
	bool IsForBattle()
	{
		for(int i =0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(GamePlayer.Instance.babies_list_[i].isForBattle_)
			{
				if(GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_Free)>0)
				{
					return true;
				}
			}
		}
		return false;
	}
//	public  void AddPropsItems(COM_DropItem [] ida)
//	{
//		for (int i = 0; i<ida.Length; i++) {
//			GameObject o = GameObject.Instantiate(item)as GameObject;
//			o.SetActive(true);
//			o.name = o.name+i;
//			UILabel[] las = o.GetComponentsInChildren<UILabel>();
//			//la.text = ida[i].itemNum_.ToString();
//			UITexture sps = o.GetComponentInChildren<UITexture>();
//			HeadIconLoader.Instance.LoadIcon (ItemData.GetData((int)ida[i].itemId_).icon_, sps);
//			foreach(UILabel la in las)
//			{
//				if(la.gameObject.name.Equals("itemnumLabel"))
//				{
//					la.text = ida[i].itemNum_.ToString();
//				}
//				if(la.gameObject.name.Equals("itemnameLabel"))
//				{
//					la.text = ItemData.GetData((int)ida[i].itemId_).name_;
//				}
//				
//			}
//			
//			o.transform.parent = grid.transform;
//			o.transform.localPosition = new Vector3(0,0,0);
//			o.transform.localScale= new Vector3(1,1,1);	
//			grid.repositionNow = true;
//			
//		}
//	}

	bool isNewEquip()
	{
		for(int i =0;i<BagSystem.instance.BagItems.Length;i++)
		{
			if(OnItemAdd(BagSystem.instance.BagItems[i]))
			{
				return true;
			}
		}
		return false;
	}

	bool OnItemAdd(COM_Item item)
	{
		if(item == null)return false;
		ItemData data = ItemData.GetData((int)item.itemId_);
		if(data == null)return false;
		if (data.mainType_ == ItemMainType.IMT_Equip)
		{
			JobType jt = (JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
			int level = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);
			Profession profession = Profession.get(jt, level);
			ItemData _itemData = ItemData.GetData((int)item.itemId_);
			if (!profession.canuseItem(_itemData.subType_, _itemData.level_))
			{
				return false;
			}
			
			if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) / 10 + 1 < _itemData.level_)
			{
				return false;
			}
			
			if (data.slot_ == EquipmentSlot.ES_SingleHand)
			{
				if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
					return false;
			}
			
			if (data.slot_ == EquipmentSlot.ES_DoubleHand)
			{
				if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
					return false;
			}
			
			float newForce = Define.CALC_BASE_FightingForce(item);
			float oldForce = Define.CALC_BASE_FightingForce(GamePlayer.Instance.Equips[(int)data.slot_]);
			if (newForce > oldForce)
			{
				return true;

			}
		}

		return false;
	}
	void addEquipsBtn()
	{
		GameObject o = GameObject.Instantiate(item)as GameObject;
		o.SetActive(true);
		UILabel[] las = o.GetComponentsInChildren<UILabel>();
		UITexture spt = o.GetComponentInChildren<UITexture>();
		spt.enabled = false;
		UISprite sps = o.GetComponentInChildren<UISprite>();
		sps.enabled = true;
		sps.spriteName = "zhuangbei";
		foreach(UILabel la in las)
		{
			if(la.gameObject.name.Equals("itemnameLabel"))
			{
				la.text = "打造新装备";
			}
			
		}
		
		o.transform.parent = grid.transform;
		UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, OnClickbtn, 2, 0);
		o.transform.localPosition = new Vector3(0,0,0);
		o.transform.localScale= new Vector3(1,1,1);	
		grid.repositionNow = true;
	}

	public static void ShowMe()
	{
		if (Battle.Instance.BattleReward == null)
			return;
		
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_Failure);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_Failure);
	}
	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_Failure, AssetLoader.EAssetType.ASSET_UI), true);
		//BagSystem.instance.ItemChanged -= OnItemAdd;

	}
	void OnDestroy()
	{
		if(TeamSystem.isBattleOpen)
		{
			TeamSystem.BackTeam();
		}
		if(TeamSystem.isYQ)
		{
			NetConnection.Instance.jointLobby ();
			TeamSystem.isYQ = false;
		}
	}
}
