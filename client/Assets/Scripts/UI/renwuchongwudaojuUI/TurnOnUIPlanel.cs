using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurnOnUIPlanel : UIBase {

	public UILabel titleLable;
	public UIButton closeBtn;
	public UIButton enterBtn;
	public GameObject babyObj;
	public GameObject itemObj;
	public GameObject babyItemObj;
	public UIGrid grid;
	public UISprite raceIcon;
	public UITexture babyIcon;
	public UISprite pinjiSp;
	public UILabel gongjiLable;
	public UILabel fangyuLable;
	public UILabel minjieLable;
	public UILabel jingshenLable;
	public UILabel huifuLable;
	public UILabel bishaLable;
	public UILabel mingzhongLable;
	public UILabel fanjiLable;
	public UILabel shanduoLable;
	public UILabel jinengLable;
	public UISprite itemSp;
	public UIGrid itemGrid;
	public UISprite itemBack;
	public GameObject suoObj;
	public UILabel itemlevelLable;
	public UILabel zhongleiLable;
	public UILabel itemDecLable;
	public GameObject babysuo;
	public UIGrid proGrid;
	public GameObject proObj;
	UIEventListener babyListener;
	UIEventListener itemListener;
	private List<Baby> giveBabys;
	private List<COM_Item> giveItems;
	public static int quesid = 0;
	public static int Npcid = 0;
	int instId = 0;
	void Start () {
		giveBabys = new List<Baby> ();
		giveItems = new List<COM_Item> ();
		babyItemObj.SetActive (false);
		itemSp.gameObject.SetActive (false);
		proObj.SetActive (false);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenter, 0, 0);
		InitData ();
		GamePlayer.Instance.OnBabyUpdate += InitData;
	}
	void InitData()
	{
		ClearObj ();
		giveBabys.Clear ();
		giveItems.Clear ();
		QuestData qdata = QuestData.GetData (quesid);
		if(qdata.questType_== QuestType.QT_GiveItem)
		{
			titleLable.text = LanguageManager.instance.GetValue("xuanzedaoju");
			babyObj.SetActive(false);
			itemObj.SetActive(true);
			InitItemData(qdata);
			isBaby=false;
		}else if(qdata.questType_== QuestType.QT_GiveBaby)
		{
			titleLable.text = LanguageManager.instance.GetValue("xuanzechongwu");
			babyObj.SetActive(true);
			itemObj.SetActive(false);
			InitbabyData(qdata);
			isBaby=true;
		}
	}
	void InitbabyData(QuestData qda)
	{

		for(int i =0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{

			if(GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_TableId) == qda.targetId_ && !GamePlayer.Instance.babies_list_[i].isLock &&!GamePlayer.Instance.babies_list_[i].isForBattle_)
			{
				giveBabys.Add(GamePlayer.Instance.babies_list_[i]);
			}
		}
		addBabyList (giveBabys);
	}
	void addBabyList(List<Baby> bas)
	{
		for(int i =0;i<bas.Count;i++)
		{
			GameObject bClone = GameObject.Instantiate(babyItemObj)as GameObject;
			bClone.SetActive (true);
			bClone.transform.parent = grid.transform;
			bClone.transform.localScale = Vector3.one;
			TurnOnbabyListCell tbcell = bClone.GetComponent<TurnOnbabyListCell>();
			tbcell.babyData = bas[i];
			babyListener = UIEventListener.Get(bClone);
			babyListener.onClick = OnClickBaby;
			babyListener.parameter = bas[i];
			grid.repositionNow = true;
		}
		if(bas.Count>0)
		{
			SetBabyData(bas[0]);
		}else
		{
			ClearBabytext();
		}
	}
	void SetBabyData(Baby ba)
	{
		BabyData bdata = BabyData.GetData(ba.GetIprop(PropertyType.PT_TableId));
		gongjiLable.text = ba.GetIprop(PropertyType.PT_Attack).ToString();
		fangyuLable.text = ba.GetIprop(PropertyType.PT_Defense).ToString();
		minjieLable.text = ba.GetIprop(PropertyType.PT_Agile).ToString();
		jingshenLable.text = ba.GetIprop(PropertyType.PT_Spirit).ToString();
		huifuLable.text = ba.GetIprop(PropertyType.PT_Reply).ToString();
		bishaLable.text = ba.GetIprop(PropertyType.PT_Crit).ToString();
	    mingzhongLable.text = ba.GetIprop(PropertyType.PT_Hit).ToString();
		fanjiLable.text = ba.GetIprop(PropertyType.PT_counterpunch).ToString();
		shanduoLable.text = ba.GetIprop(PropertyType.PT_Dodge).ToString();
		jinengLable.text = bdata._SkillNum.ToString();
		babysuo.SetActive (ba.GetInst().isLock_);
		instId = ba.InstId;
		HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(BabyData.GetData(ba.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_, babyIcon);
		//iconkuang.spriteName = BabyData.GetPetQuality(bdata._PetQuality);
		
		int Magic =   bdata._BIG_Magic - ba.gear_[(int)BabyInitGear.BIG_Magic];
		int Stama =   bdata._BIG_Stama - ba.gear_[(int)BabyInitGear.BIG_Stama];
		int Speed =   bdata._BIG_Speed - ba.gear_[(int)BabyInitGear.BIG_Speed];
		int Power =   bdata._BIG_Power - ba.gear_[(int)BabyInitGear.BIG_Power];
		int Strength =   bdata._BIG_Strength - ba.gear_[(int)BabyInitGear.BIG_Strength];
		int num = Magic+Stama+Speed+Power+Strength;
		pinjiSp.spriteName = BabyData.GetBabyLeveSp(num);

	}
	void InitItemData(QuestData qda)
	{

		for(int i =0;i<BagSystem.instance.BagItems.Length;i++)
		{
			if(BagSystem.instance.BagItems[i] ==null)continue;
			if(BagSystem.instance.BagItems[i].itemId_ == qda.targetId_)
			{
				giveItems.Add(BagSystem.instance.BagItems[i]);
			}
		}
		addItemList (giveItems);
	}

	void addItemList(List<COM_Item> ites)
	{
		for(int i =0;i<ites.Count;i++)
		{
			GameObject iClone = GameObject.Instantiate(itemSp.gameObject)as GameObject;
			iClone.SetActive (true);
			iClone.transform.parent = itemGrid.transform;
			iClone.transform.localScale = Vector3.one;
			UISprite sp = iClone.GetComponent<UISprite>();
			UISprite []sps = iClone.GetComponentsInChildren<UISprite>(true);
			foreach(UISprite s in sps)
			{
				if(s.name == "suo")
				{
					s.gameObject.SetActive(ites[i].isLock_);
				}
			}
			ItemCellUI tcell = UIManager.Instance.AddItemCellUI(sp,ites[i].itemId_);
			tcell.ItemCount = (int)ites[i].stack_;
			itemListener = UIEventListener.Get(tcell.gameObject);
			itemListener.onClick = OnClickitem;
			itemListener.parameter = ites[i];
			itemGrid.repositionNow = true;
		}
		if(ites.Count >0)
		{
			SetItemData(ites[0]);
		}
	}
	void SetItemData(COM_Item cit)
	{
		ItemData ida = ItemData.GetData ((int)cit.itemId_);
		UIManager.Instance.AddItemCellUI (itemBack, cit.itemId_).ItemCount = (int)cit.stack_;
		itemDecLable.text = ida.desc_;
		suoObj.SetActive (cit.isLock_);
		instId = (int)cit.instId_;
		itemlevelLable.text = ida.level_.ToString ();
		zhongleiLable.text = LanguageManager.instance.GetValue(ida.subType_.ToString());
		ShowPro (ida,cit);

	}

	void ShowPro(ItemData idat,COM_Item c)
	{
		if(c.propArr.Length > 0)
		{
			for( int o = 0;o<cellList.Count;o++)
			{
				grid.RemoveChild(cellList[o].transform);
				cellList[o].transform.parent = null;
				cellList[o].gameObject.SetActive(false);
				cellPool.Add(cellList[o]);
			}
			cellList.Clear ();
			for(int i=0;i<c.propArr.Length;i++)
			{
				if(c.propArr[i].type_ == PropertyType.PT_Durability)
				{
					continue;
				}
				GameObject objCell = null;
				if(cellPool.Count>0)
				{
					objCell = cellPool[0];
					cellPool.Remove(objCell);  
					UIManager.RemoveButtonAllEventHandler(objCell);
				}
				else  
				{
					objCell = Object.Instantiate(proObj) as GameObject;
					objCell.SetActive(true);
				}
				string sNum = "";
				if(c.propArr[i].value_ > 0)
				{
					sNum = " +" + ((int)c.propArr[i].value_);
				}
				else
				{
					sNum =  " "+((int)c.propArr[i].value_).ToString();
				}
				UILabel lable =	objCell.GetComponent<UILabel>();
				lable.text  =  LanguageManager.instance.GetValue(c.propArr[i].type_.ToString())+sNum;
				if(c.durability_ < c.durabilityMax_/2)
				{
					lable.color = Color.red;
				}
				else if(ItemData.GetData((int)c.itemId_).mainType_ == ItemMainType.IMT_FuWen)
				{
					lable.color = Color.grey;
				}
				else
				{
					float perNum = EquipColorData.GetEquipPerNum(idat.level_, c.propArr[i].type_,c.propArr[i].value_);
					
					if(perNum < 0)
					{
						lable.color = Color.grey;
					}
					else if(perNum < 24)
					{
						lable.color = Color.grey;
					} 
					else if(perNum >= 25 && perNum <= 49)
					{
						lable.color = Color.green;
					}
					else if(perNum >=50 && perNum <= 74)
					{
						lable.color = Color.blue;
					}
					else if(perNum >= 75 && perNum <= 84)
					{
						lable.color = Color.magenta;
					}
					else if(perNum >= 85 && perNum <= 94)
					{
						lable.color = Color.yellow;
					}
					else 
					{ 
						lable.color = Color.white;
						lable.text = "[FECE29]" + lable.text;
					}
				}
				proGrid.AddChild(objCell.transform);
				objCell.transform.localScale = Vector3.one;
				objCell.gameObject.SetActive(true);
				cellList.Add(objCell);
				
			}
			proGrid.Reposition();
		}
	}
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();
	TurnOnbabyListCell curCell;
	bool isBaby;
	void OnClickBaby(GameObject sender)
	{
		Baby b = (Baby)UIEventListener.Get (sender).parameter;
		TurnOnbabyListCell lCell = sender.GetComponent<TurnOnbabyListCell>();
		if(curCell != null )
		{
			curCell.backsp.spriteName = "ln_jinlan";
		}
		SetBabyData(b);
		instId = b.InstId;
		curCell = lCell;
		lCell.backsp.spriteName = "jn_jinlanliang";
	}
	UISprite cursp;
	void OnClickitem(GameObject sender)
	{
		UISprite sp =sender.transform.parent.FindChild ("scl").GetComponent<UISprite>();
		if(cursp !=null)
		{
			cursp.gameObject.SetActive(false);
		}

		COM_Item ci = (COM_Item)UIEventListener.Get (sender).parameter;
		SetItemData(ci);
		instId = (int)ci.instId_;
		cursp =sp;
		sp.gameObject.SetActive (true);
	}
	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickenter(ButtonScript obj, object args, int param1, int param2)
	{
		if(isBaby)
		{
			if(!Isbattle())
			{
				return;
			}
		}
		NetConnection.Instance.submitQuest2(Npcid, quesid,instId);
		QuestSystem.LocalSubmitQuest(quesid);
	}
	bool Isbattle()
	{
		QuestData qdd = QuestData.GetData(quesid);
		for(int i =0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			
			if(GamePlayer.Instance.babies_list_[i].GetIprop(PropertyType.PT_TableId) == qdd.targetId_)
			{
				if(GamePlayer.Instance.babies_list_[i].isLock)
				{
					//PopText.Instance.Show(LanguageManager.instance.GetValue("suodingchongwuno"));
					return false;
				}
				if(GamePlayer.Instance.babies_list_[i].isForBattle_)
				{
					//PopText.Instance.Show(LanguageManager.instance.GetValue("chuzhanchongwuno"));
					return false;
				}
			}
		}
		return true;
	}
	// Update is called once per frame
	void Update () {
	
	}
	public static void ShowMe(int qid,int npcid)
	{
		quesid = qid;
		Npcid = npcid;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_TurnOnUIPlanel);
	}
	public static void SwithShowMe(int qid,int npcid)
	{
		quesid = qid;
		Npcid = npcid;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_TurnOnUIPlanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_TurnOnUIPlanel);
	}

	void ClearBabytext()
	{
		gongjiLable.text = "";
		fangyuLable.text ="";
		minjieLable.text = "";
		jingshenLable.text = "";
		huifuLable.text ="";
		bishaLable.text = "";
		mingzhongLable.text ="";
		fanjiLable.text ="";
		shanduoLable.text ="";
		jinengLable.text = "";
		babysuo.SetActive (false);
		babyIcon.mainTexture = null;
		pinjiSp.spriteName = "";
		raceIcon.spriteName = "";
	}
	void ClearObj()
	{
		if(grid !=null)
		{
			foreach(Transform ta in grid.transform)
			{
				Destroy(ta.gameObject);
			}
		}
		if(itemGrid != null)
		{
			foreach(Transform tai in itemGrid.transform)
			{
				Destroy(tai.gameObject);
			}
		}
	}
	void OnDestroy()
	{
		GamePlayer.Instance.OnBabyUpdate -= InitData;
	}
	public override void Destroyobj ()
	{

	}
}
