using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainbabyListUI : MonoBehaviour {

//	public UISprite spZhongzu;
//	public UILabel bNameLabel;
	public GameObject item;
	public Transform TopLeft;
	public Transform BottomRight;
	public Transform modesPos;
    public UILabel modelName_;
    public UITexture modelRace_;
	public UILabel raceLabel;
	private UIGrid grid;
	private UIEventListener Listener;
	private List<Baby> babylist;
	private List<GameObject> itemsList = new List<GameObject>();
	private List<UIButton> chuzhanBtnList = new List<UIButton>();
	private List<UIButton> daimingBtnList = new List<UIButton>();
	public static GameObject babyObj;
	private int asssid;
	private bool isBattle;
	public delegate void changeBabyName(int uid,string newname);
	public static changeBabyName changeBabyNameOk;
	public delegate void FightingStandby(int uid,bool isf);
	public static FightingStandby BabyFightingStandby;
	public delegate void RefreshBabyList (int binsId);
	public static RefreshBabyList RefreshBabyListOk;

	public delegate void UpdateBabyMarkOn (int binsId);
	public static UpdateBabyMarkOn UpdateBabyMarkOnOk;

	public delegate void SetBabyListLockUI (int binsId,bool islock);
	public static SetBabyListLockUI SetBabyListLockUIOk;

	public delegate void RefreshBaby (COM_BabyInst binst);
	public static RefreshBaby RefreshBabyOk;


	public delegate void UpdateBabyListUIEvent (int binsId);
	public static UpdateBabyListUIEvent UpdateBabyListUIOk;
	private string babyName = "";

    public static bool SelectDirty = false;
    static int crtSelectIdx = 0;
    public static int CrtSelectIdx
    {
        set
        {
            crtSelectIdx = value;
            SelectDirty = true;
        }
        get
        {
            return crtSelectIdx;
        }
    }

	private List<string> _icons = new List<string>();
	// Use this for initialization
	void Start () {
		MainbabyUI.Instance.listObj.SetActive (true);
		item.SetActive (false);
		isBattle = GamePlayer.Instance.isInBattle;
		grid = GetComponentInChildren<UIGrid> ();
		babylist =	GamePlayer.Instance.babies_list_;
        changeBabyNameOk = NameBabyChange;
        Refresh();
        CrtSelectIdx = 0;
        AddItems(babylist);
        BabyFightingStandby = BabyFightingState;
        RefreshBabyListOk += RefreshBabyListUI;
		MainbabyUI.OnoffmakeOk += NewBabyMakeOff;
		UpdateBabyListUIOk = UpdateBabyListUI;
		RefreshBabyOk = RefreshBabyUIOk;
		SetBabyListLockUIOk = BabyListLockUIOk;
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			modelRace_.mainTexture = null;
			raceLabel.text = "";
		}
		GamePlayer.Instance.OnShowBaby = ShowBaby;
       // hasDestroy = true;
	}
	void ShowBaby(int babyid,bool isshow)
	{
		for(int i=0;i<listCells.Count;i++)
		{
			if(listCells[i] == null)
				continue;
			MainBabyListCell blc = listCells[i].GetComponent<MainBabyListCell>();
			if(blc==null)
				continue;
			if(babyid == blc.BabyMainData.InstId)
			{
				if(isshow)
				{
					blc.gensuiSp.gameObject.SetActive(true);
				}else
				{
					blc.gensuiSp.gameObject.SetActive(false);
				}
				
			}else
			{
				if( blc.BabyMainData.isShow_)
				{
					blc.gensuiSp.gameObject.SetActive(true);
				}else
				{
					blc.gensuiSp.gameObject.SetActive(false);
				}
			}
			
		}
	}
	void BabyListLockUIOk(int inid,bool slock)
	{
		for(int i=0;i<listCells.Count;i++)
		{
			MainBabyListCell blc = listCells[i].GetComponent<MainBabyListCell>();
			if(inid == blc.BabyMainData.InstId)
			{
				if(slock)
				{
					blc.suodingSp.gameObject.SetActive(true);
				}else
				{
					blc.suodingSp.gameObject.SetActive(false);
				}

			}
			
		}
	}
	void RefreshBabyListUI()
	{
		Refresh ();
		babylist =	GamePlayer.Instance.babies_list_;
		AddItems (babylist);
		if(babyObj != null)
		babyObj.SetActive (false);
		PopText.Instance.Show (LanguageManager.instance.GetValue("shiquchongwu").Replace("{n}",":"+babyName));
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			modelRace_.mainTexture = null;
			raceLabel.text = "";
		}
		
	}


	void RefreshBabyUIOk(COM_BabyInst binst)
	{
		Baby ba = new Baby ();
		ba.SetBaby (binst);
		for(int i =0;i<itemsList.Count;i++)
		{
			MainBabyListCell mbcell = itemsList[i].GetComponent<MainBabyListCell>();
			if(mbcell.BabyMainData.InstId == binst.instId_)
			{
				mbcell.BabyMainData = ba;

				return;
			}
		}
	}
	void UpdateBabyListUI(int binsId)
	{
		Baby ba = GamePlayer.Instance.GetBabyInst (binsId);
		if(ba == null)return;
		for(int i =0;i<itemsList.Count;i++)
		{
			MainBabyListCell mbcell = itemsList[i].GetComponent<MainBabyListCell>();
			if(mbcell.BabyMainData.InstId == binsId)
			{
				mbcell.BabyMainData = ba;
				return;
			}
		}
//		GlobalInstanceFunction.Instance.Invoke(()=>{
//			grid.Reposition();
//		},1);
	}
	void OnEnable()
	{
		if (grid == null)
			return;
		grid.Reposition ();
//		Refresh();
//		AddItems(GamePlayer.Instance.babies_list_);
	}
	void RefreshBabyListUI(int binsId)
	{
		Refresh ();
		babylist =	GamePlayer.Instance.babies_list_;
		AddItems (babylist);
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			modelRace_.mainTexture = null;
			raceLabel.text = "";
		}
		//PopText.Instance.Show (LanguageManager.instance.GetValue("shiquchongwu").Replace("{n}",":"+babyName));

	}
	void NameBabyChange(int uid,string newname)
	{
		Refresh ();
//		for (int i=0; i<babylist.Count; i++)
//		{
//			if(babylist[i].InstId == uid)
//			{
//				babylist[i].InstName = newname;
//			}
//		}
//		


		AddItems (babylist);
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			modelRace_ = null;
			raceLabel.text = "";
		}
	}
	void Refresh()
	{
		itemsList.Clear ();
		if (grid == null)
						return;
		foreach(Transform tra in grid.transform)
		{
			Destroy(tra.gameObject);
		}
	}

	List<MainBabyListCell> listCells = new List<MainBabyListCell> ();

	public void AddItems(List<Baby> Entitylist)
	{
		if (Entitylist == null)
						return;
		for (int i = 0; i<Entitylist.Count; i++) {
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = Entitylist[i].InstName;
			o.transform.parent = grid.transform;
			o.transform.localPosition = Vector3.zero;
			MainBabyListCell mbCell = o.GetComponent<MainBabyListCell>();
			mbCell.BabyMainData = Entitylist[i];

			listCells.Add(mbCell);
			o.transform.localScale= new Vector3(1,1,1);	
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, buttonClick,i, 0);
			//grid.repositionNow = true;


			itemsList.Add(o);
		}
		GlobalInstanceFunction.Instance.Invoke(()=>{
			grid.Reposition();
		},1);

		if(itemsList != null && itemsList.Count > 0)
		{
            int[] ids = { Entitylist[0].InstId, Entitylist[0].GetIprop(PropertyType.PT_AssetId) };
            GameManager.Instance.GetActorClone((ENTITY_ID)Entitylist[0].Properties[(int)PropertyType.PT_AssetId], (ENTITY_ID)0, Entitylist[0].type_, AssetLoadCallBack, new ParamData(Entitylist[0].InstId, Entitylist[0].GetIprop(PropertyType.PT_AssetId)), "UI");
			MainbabyProperty.idss = ids;
			//modelName_.text = GamePlayer.Instance.GetBabyInst(Entitylist[0].InstId).InstName;
			//modelRace_.spriteName = BabyData.GetData(GamePlayer.Instance.GetBabyInst(Entitylist[0].InstId).GetIprop(PropertyType.PT_TableId)).RaceIcon_;
			HeadIconLoader.Instance.LoadIcon (BabyData.GetData(GamePlayer.Instance.GetBabyInst(Entitylist[0].InstId).GetIprop(PropertyType.PT_TableId))._RaceIcon, modelRace_);

			if(!_icons.Contains(BabyData.GetData(GamePlayer.Instance.GetBabyInst(Entitylist[0].InstId).GetIprop(PropertyType.PT_TableId))._RaceIcon))
			{
				_icons.Add(BabyData.GetData(GamePlayer.Instance.GetBabyInst(Entitylist[0].InstId).GetIprop(PropertyType.PT_TableId))._RaceIcon);
			}

			raceLabel.text = LanguageManager.instance.GetValue( BabyData.GetData(GamePlayer.Instance.GetBabyInst(Entitylist[0].InstId).GetIprop(PropertyType.PT_TableId))._RaceType.ToString());
			if (MainbabyState.babyInfo != null)
			{
				MainbabyState.babyInfo(ids);		
			}
	
		}
		NewBabyMakeOn ();	
	}
	void NewBabyMakeOn()
	{
		for(int i =0;i<itemsList.Count;i++)
		{
			MainBabyListCell mbCell = itemsList[i].GetComponent<MainBabyListCell>();
			if(mbCell.BabyMainData.InstId == GamePlayer.newBabyId)
			{
				itemsList[i].GetComponent<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-10,-10);
			}
		}
		GamePlayer.newBabyId = 0;
	}

	void NewBabyMakeOff()
	{
		for(int i =0;i<itemsList.Count;i++)
		{
				itemsList[i].GetComponent<UISprite>().MarkOff();
		}
	}
	MainBabyListCell curCell;
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{
		babyName = obj.name;
		MainBabyListCell lCell = obj.GetComponent<MainBabyListCell>();

		if(curCell != null )
		{
			curCell.backSp.spriteName = "zd_chongwukuang";
		}

        int[] ids = { GamePlayer.Instance.babies_list_[param1].InstId, GamePlayer.Instance.babies_list_[param1].GetIprop(PropertyType.PT_AssetId) };
        CrtSelectIdx = param1;

		curCell = lCell;
		lCell.backSp.spriteName = "zd_chongwukuangxuanz";
        if (asssid != ids[1])
		{
			if (asssid != 0 && babyObj != null) {
				DestroyBaby((ENTITY_ID)asssid,true,babyObj);
			}
		}

        
		if (MainbabyState.babyInfo != null) {
			MainbabyState.babyInfo(ids);		
		}

		MainbabyProperty.idss = ids;
        int uId = ids[0];
        int asseId = ids[1];
		asssid = asseId;

		GameManager.Instance.GetActorClone((ENTITY_ID)asseId, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, new ParamData(uId,asseId), "UI");
        HeadIconLoader.Instance.LoadIcon(BabyData.GetData(GamePlayer.Instance.GetBabyInst(ids[0]).GetIprop(PropertyType.PT_TableId))._RaceIcon, modelRace_);
        raceLabel.text = LanguageManager.instance.GetValue(BabyData.GetData(GamePlayer.Instance.GetBabyInst(ids[0]).GetIprop(PropertyType.PT_TableId))._RaceType.ToString());
        if (!_icons.Contains(BabyData.GetData(GamePlayer.Instance.GetBabyInst(ids[0]).GetIprop(PropertyType.PT_TableId))._RaceIcon))
		{
            _icons.Add(BabyData.GetData(GamePlayer.Instance.GetBabyInst(ids[0]).GetIprop(PropertyType.PT_TableId))._RaceIcon);
		}

		if(MainbabyProperty.BabyProperty !=null)
		{
			MainbabyProperty.BabyProperty(uId);
		}
		if(MainbabyReductionUI.RefreshGrowingUpOk != null)
		{
            MainbabyReductionUI.RefreshGrowingUpOk(ids[0]);
		}
		if(Mainbabystrengthen.RefreshstrengthenOk != null)
		{
            Mainbabystrengthen.RefreshstrengthenOk(ids[0]);
		}
		if(MainbabyListUI.UpdateBabyMarkOnOk != null)
		{
			MainbabyListUI.UpdateBabyMarkOnOk(uId);
		}
	}
//	private void OnClickCZ(ButtonScript obj, object args, int param1, int param2)
//	{
//		SetBabyFightingState (param2,true);
//		GamePlayer.Instance.BabyState (param1, true);
//		NetConnection.Instance.setBattlebaby((uint)param1,true);
//	}
//	private void OnClickDM(ButtonScript obj, object args, int param1, int param2)
//	{
//		SetBabyFightingState (param2,false);
//		GamePlayer.Instance.BabyState (param1, false);
//		NetConnection.Instance.setBattlebaby((uint)param1,false);
//	}

	private void SetBabyFightingState(int index,bool isChuzhan)
	{
		if (isChuzhan) 
		{
			for(int i = 0;i<chuzhanBtnList.Count;i++)
			{
				if(index == i)
				{
					chuzhanBtnList[i].GetComponentInChildren<UILabel>().color = new Color(60f,60f,60f);
					chuzhanBtnList[i].isEnabled = false;
					daimingBtnList[i].isEnabled = true;
					daimingBtnList[i].GetComponentInChildren<UILabel>().color = new Color(9f,94f,0f);
				}
			}	
		}
		else
		{
			for(int j = 0;j<chuzhanBtnList.Count;j++)
			{
				if(index == j)
				{
					daimingBtnList[j].GetComponentInChildren<UILabel>().color = new Color(60f,60f,60f);
					daimingBtnList[j].isEnabled = false;
					chuzhanBtnList[j].isEnabled = true;
					chuzhanBtnList[j].GetComponentInChildren<UILabel>().color = new Color(9f,94f,0f);
				}
			}
		}


	}

	private void IsBabyFightingState(bool isForBattle)
	{
		if (isForBattle) 
		{
			for(int i = 0;i<chuzhanBtnList.Count;i++)
			{
				chuzhanBtnList[i].gameObject.SetActive(true);
				daimingBtnList[i].gameObject.SetActive(false);
			}	
		}
		else
		{
			for(int j = 0;j<chuzhanBtnList.Count;j++)
			{
					daimingBtnList[j].gameObject.SetActive(true);
					chuzhanBtnList[j].gameObject.SetActive( false);
			}
		}

	}
	public void BabyFightingState(int inId,bool isF)
	{
		for(int i = 0;i<listCells.Count;i++)
		{
			if(listCells[i]==null)continue;
			if(listCells[i].BabyMainData.InstId == inId)
			{
				if(isF)
				{
					listCells[i].chuzhanSp.gameObject.SetActive(true);
				}else
				{

					listCells[i].chuzhanSp.gameObject.SetActive(false);
				}
			}else
			{
				listCells[i].chuzhanSp.gameObject.SetActive(false);
			}
					



		}
	}

//	void buttonClick(GameObject sender)
//	{
//		if (asssid != 0 && babyObj != null) {
//			DestroyBaby((ENTITY_ID)asssid,true,babyObj);
//		}
//		int []ids = (int [])UIEventListener.Get (sender).parameter;
//		if (MainbabyState.babyInfo != null) {
//			MainbabyState.babyInfo(ids);		
//		}
//		MainbabyProperty.idss = ids;
//		int	uId = ids [0];
//		int	asseId = ids[1];
//		asssid = asseId;
//        GameManager.Instance.GetActorClone((ENTITY_ID)asseId, (ENTITY_ID)0, AssetLoadCallBack, new ParamData(uId));
//
//		for (int i =0; i<babylist.Count; i++)
//		{
//			if(babylist[i].InstId == uId)
//			{
//				spZhongzu.spriteName = BabyData.GetData( babylist[i].Properties[(int)PropertyType.PT_TableId]).RaceIcon_;
//				bNameLabel.text = babylist[i].InstName;
//			}
//		}
//		if(MainbabyProperty.BabyProperty !=null)
//		{
//			MainbabyProperty.BabyProperty(uId);
//		}
//
//
//	}
	void DestroyBaby(ENTITY_ID eId,bool unLoadAllLoadedObjects,GameObject obj)
	{
		PlayerAsseMgr.DeleteAsset (eId, unLoadAllLoadedObjects);
		Destroy (obj);
	}

    bool hasDestroy = false;
    void AssetLoadCallBack(GameObject ro, ParamData data)
    {
        if (hasDestroy)
        {
            DestroyBaby((ENTITY_ID)data.iParam, true, ro);
            return;
        }
//        Baby inst = GamePlayer.Instance.GetBabyInst(data.iParam);
//        modelName_.text = inst.InstName;
//        modelRace_.spriteName = BabyData.GetData(inst.GetIprop(PropertyType.PT_TableId)).RaceIcon_;
        ro.transform.parent = modesPos;
        modesPos.transform.localScale = Vector3.one;
        ro.transform.localPosition = Vector3.forward * -700;
		ro.transform.localScale = new Vector3(EntityAssetsData.GetData(data.iParam2).zoom_ ,EntityAssetsData.GetData(data.iParam2).zoom_,EntityAssetsData.GetData(data.iParam2).zoom_ );
		ro.transform.rotation = new Quaternion (ro.transform.rotation.x,180,ro.transform.rotation.z,ro.transform.rotation.w);
			// ro.transform.LookAt(ApplicationEntry.Instance.ui3DCamera.transform.position);
        EffectLevel el = ro.AddComponent<EffectLevel>();
        el.target = ro.transform.parent.parent.GetComponent<UISprite>();
        if(babyObj != null)
		{
			Destroy(babyObj);
		}
        babyObj = ro;
		if(MainbabyUI.Instance.isState&&!BankSystem.instance.isopen)
		{
			babyObj.SetActive(true);
		}else
		{
			babyObj.SetActive(false);
		}
//		if(!BankSystem.isopen)
//		{
//			babyObj.SetActive(true);
//		}else
//		{
//			babyObj.SetActive(false);
//		}
    }
	bool isHid;
	void OnDestroy()
	{
        hasDestroy = true;
		isHid = true;
        changeBabyNameOk = null;
        BabyFightingStandby = null;
		SetBabyListLockUIOk = null;
		GamePlayer.Instance.OnShowBaby = null;
		RefreshBabyListOk -= RefreshBabyListUI;
		MainbabyUI.OnoffmakeOk -= NewBabyMakeOff;
        itemsList.Clear();
		GamePlayer.Instance.OnShowBaby = null;
		PlayerAsseMgr.DeleteAsset((ENTITY_ID)asssid, true);
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}




}
