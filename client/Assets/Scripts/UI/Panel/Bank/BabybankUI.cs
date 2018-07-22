using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BabybankUI : MonoBehaviour {

	public UILabel _ArrangeLbale;
	public UIGrid babyGrid;
	public GameObject babyItem;
	public UIGrid cGrid;
	public GameObject cItem;
	public GameObject tipsObj;
	public static bool isqu;
	public UILabel numabel;
	public UIButton zhenliBtn;

	public List<UIButton> babyTabBtn = new List<UIButton> ();
	private StorageBabyCell stCell;
	private List<Baby> babylist;
	private GameObject[] BabyCells = new GameObject[6];
	private bool bDouble = false;
	private bool sDouble = false;
	private int babyId;
	private int _selsctCTab;
	void Start () {
		_ArrangeLbale.text = LanguageManager.instance.GetValue ("bank_Arrange");
		for(int i =0;i<babyTabBtn.Count;i++)
		{
			UIManager.SetButtonEventHandler (babyTabBtn[i].gameObject, EnumButtonEvent.OnClick, OnClickLeftBtn,i, 0);
		}
		UIManager.SetButtonEventHandler (zhenliBtn.gameObject, EnumButtonEvent.OnClick, OnClickZhengliBtn,0, 0);

		babyItem.SetActive (false);
		cItem.SetActive (false);
		ShowStorageItem ();
		RefreshBabyListUI ();
		RequestStorageBaby ();
		TabsLeftSelect (0);
		GamePlayer.Instance.OnBabyUpdate += BankSystem.instance.UpdateUI;
		BankSystem.OnUpdateStoragebabyItemOk += RefreshBabyListUI;
		BankSystem.OnSortBabyStorageOk += SortBabyStorageOk;


	}
	void ClearStoragebaby()
	{
		for(int i =0;i<GamePlayer.Instance.Storagebaby.Length;i++)
		{
			GamePlayer.Instance.Storagebaby[i] = null;
		}

	}
	private void SortBabyStorageOk(List<COM_BabyInst> inst)
	{
		ClearStoragebaby ();
		for(int i =0;i<inst.Count;i++)
		{
			GamePlayer.Instance.Storagebaby[inst[i].slot_] = inst[i];
		}
		RemoveStorageItemAllEventHandler ();
		int num = _selsctCTabNum * 6 ;
		for (int i= 0; i<6; i++) 
		{
			BabyCells[i].GetComponent<StorageBabyCell>().BabyInst = GamePlayer.Instance.Storagebaby[num+i];
			
			if(GamePlayer.Instance.Storagebaby[num+i] != null)
			{
				UIManager.SetButtonEventHandler (BabyCells [i].gameObject, EnumButtonEvent.OnClick, OnClickCell, 0, 0);	
			}			
		}
	}
	void RemoveStorageItemAllEventHandler()
	{
		for(int i =0;i<BabyCells.Length;i++)
		{
			UIManager.RemoveButtonAllEventHandler (BabyCells[i].gameObject);
		}
	}


	void OnClickZhengliBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.sortStorage (StorageType.ST_Baby);
	}
	void OnClickLeftBtn(ButtonScript obj, object args, int param1, int param2)
	{

//		int num = BankSystem.instance.babyNum / BabyCells.Length;
//		if(param1<num)
//		{
			TabsLeftSelect (param1);
//		}else
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("shoulian") );
//		}


	}
	void TabsLeftSelect(int index)
	{
		for (int i = 0; i<babyTabBtn.Count; i++) 
		{
			if(i==index)
			{
				babyTabBtn[i].isEnabled = false;
				_selsctCTabNum = i;
			}
			else
			{
				babyTabBtn[i].isEnabled = true;
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
		int num = _selsctCTabNum * 6 ;
		for (int i= 0; i<6; i++) 
		{
			StorageBabyCell bagCell = BabyCells[i].GetComponent<StorageBabyCell>();
			bagCell.BabyInst = GamePlayer.Instance.Storagebaby[num+i];

//			if(GamePlayer.Instance.Storagebaby[num+i] != null)
//			{
				UIManager.SetButtonEventHandler (BabyCells [i], EnumButtonEvent.OnClick, OnClickCell, 0, 0);	
//			}
			
			int storNum = BankSystem.instance.babyNum;
			if(num+i >=storNum)  //已开启背包格子数.
			{
				BabyCells[i].GetComponent<StorageBabyCell>().Lock = false;
				UIManager.SetButtonParam(BabyCells [i].gameObject,1,0);

			}
			else
			{
				BabyCells[i].GetComponent<StorageBabyCell>().Lock = true;
				if(GamePlayer.Instance.Storagebaby[num+i] != null)
				{
					UIManager.SetButtonParam(BabyCells [i].gameObject,0,0);
				}else
				{
					UIManager.SetButtonParam(BabyCells [i].gameObject,2,0);
				}

			}
		}
	}
	void RefreshBabyListUI()
	{

		Refresh ();
		babylist =	GamePlayer.Instance.babies_list_;
		AddItems (babylist);
		RequestStorageBaby ();
		numabel.text = BankSystem.instance.GetBabySize () + "/" + BankSystem.instance.babyNum;
	}
//
	void RequestStorageBaby()
	{
		RemoveStorageItemAllEventHandler ();
		for(int i =0;i<GamePlayer.Instance.Storagebaby.Length;i++)
		{
			if(GamePlayer.Instance.Storagebaby[i] != null)
			{
				StorageBabyCell bagCe;
				if(i<BabyCells.Length)
				{
					bagCe = BabyCells[i].GetComponent<StorageBabyCell>();
					UIManager.SetButtonEventHandler (BabyCells[i], EnumButtonEvent.OnClick, OnClickCell, 0, 0);		
					UIEventListener.Get(BabyCells[i].gameObject).onDoubleClick = OnCellDoubleToBagClick;
					bagCe.BabyInst = GamePlayer.Instance.Storagebaby[i];
				}
				
				
			}
		}
		UpdataTabCangItems ();
	}
	void OnCellDoubleToBagClick(GameObject obj)
	{
		bDouble = true;
		StopCoroutine ("DelayOneClick");
		COM_BabyInst _baby = obj.GetComponentInParent<StorageBabyCell> ().BabyInst;
		if(BankSystem.instance.IsBabyListFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_BabyFull"));
			return;
		}
		if(_baby == null)
		{
			return;
		}
		StorageBabyCell scell = obj.GetComponentInParent<StorageBabyCell> ();
		babyId = (int)scell.BabyInst.instId_;
		scell.icon.gameObject.SetActive(false);
		scell.raceIcon.gameObject.SetActive(false);
		BabyTipsUI bt =	tipsObj.GetComponent<BabyTipsUI> ();
		if(scell.gameObject.name == "agCell")
		{
			bt.isbabyList = false;
		}else
		{
			bt.isbabyList = true;
		}
		bt.bcell = scell;
		bt.bcell.numsp.spriteName = "";
//		bt.bcell.iconBack.spriteName = "cw_chongwutouxiang1";
//		bt.bcell.iconBack.GetComponent<UIButton>().normalSprite = "cw_chongwutouxiang1";
		UIManager.RemoveButtonAllEventHandler (obj);
		NetConnection.Instance.storageBabyToPlayer(_baby.instId_);
	}
	private void OnClickCell(ButtonScript obj, object args, int param1, int param2)
	{
		if(param1 == 2)
		{
			return;	
		}
		if(param1 == 1)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("shoulian"));
		} else
		{

			bDouble = false;
			StorageBabyCell bCell = obj.GetComponentInParent<StorageBabyCell> ();
			babyId = (int)bCell.BabyInst.instId_ ;
			StartCoroutine (DelayOneClick (bCell));
		}
	}
	IEnumerator DelayOneClick(StorageBabyCell bCell)
	{
		yield return new WaitForSeconds(0.2f);
		if(!bDouble)
		{
			tipsObj.SetActive (true);
			COM_BabyInst binst = null;
			for(int i =0;i<GamePlayer.Instance.Storagebaby.Length;i++)
			{
				if(GamePlayer.Instance.Storagebaby[i]==null)continue;
				if(GamePlayer.Instance.Storagebaby[i].instId_==bCell.BabyInst.instId_)
				{
					binst = GamePlayer.Instance.Storagebaby[i];
					break;
				}
			}
			Baby baby = new Baby();
			baby.SetBaby(binst);

			BabyTipsUI bt =	tipsObj.GetComponent<BabyTipsUI> ();
			if(bCell.gameObject.name == "agCell")
			{
				bt.isbabyList = false;
			}else
			{
				bt.isbabyList = true;
			}
			bt.bcell = bCell;
			bt.baby =baby;

		}
	}
	public void AddItems(List<Baby> Entitylist)
	{
		for (int i = 0; i<Entitylist.Count; i++) {
			GameObject o = GameObject.Instantiate(babyItem)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = babyGrid.transform;
			StorageBabyCell mbCell = o.GetComponent<StorageBabyCell>();
			mbCell.BabyMainData = Entitylist[i];
//			if(Entitylist[i].isForBattle_)
//			{
//				mbCell.chuzhanButton.gameObject.SetActive(true);
//				mbCell.chuzhanButton.isEnabled = false;
//				mbCell.daimingButton.gameObject.SetActive(false);
//				mbCell.daimingButton.isEnabled = false;
//			}else
//			{
//				mbCell.chuzhanButton.isEnabled = false;
//				mbCell.daimingButton.isEnabled = false;
//				mbCell.chuzhanButton.gameObject.SetActive(false);
//				mbCell.daimingButton.gameObject.SetActive(true);
//
//			}
			o.transform.localScale= new Vector3(1,1,1);	
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, buttonClick,Entitylist[i].InstId,Entitylist[i].GetIprop(PropertyType.PT_AssetId));
			UIEventListener.Get(o).onDoubleClick = OnCellDoubleToSortClick;
			GlobalInstanceFunction.Instance.Invoke(()=>{
				babyGrid.Reposition();
			},1);
		}

	}
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{
		Baby b = null;
		b = GamePlayer.Instance.GetBabyInst (param1);
		if(b != null)
		{
			if(BankSystem.instance.IsBabyStorageFull())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("cankuman"));
				return;
			}
			if(b.isForBattle_)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("chuzhan"));
				return;
			}
			 if(b.GetInst().isShow_)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("showbaby"));
				return;
			}

				StorageBabyCell bCell = obj.GetComponentInParent<StorageBabyCell> ();
				sDouble = false;
				StartCoroutine (DelayOnesortClick (bCell));

		}

	}
	void OnCellDoubleToSortClick(GameObject obj)
	{

		StorageBabyCell bCell = obj.GetComponentInParent<StorageBabyCell> ();
		if(BankSystem.instance.IsBabyStorageFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("cankuman"));
			return;
		}
		if(bCell.BabyMainData.isForBattle_)
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("chuzhan"));
			return ;
		}
		if(bCell.BabyMainData.isShow_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("showbaby"));
			return ;
		}
		sDouble = true;
		StopCoroutine ("DelayOnesortClick");

		
		if(bCell == null)
		{
			return;
		}
	

		BabyTipsUI bt =	tipsObj.GetComponent<BabyTipsUI> ();
		bt.bcell = bCell;
		if(bCell.gameObject.name == "agCell")
		{
			bt.isbabyList = false;
		}else
		{
			bt.isbabyList = true;
		}
		UIManager.RemoveButtonAllEventHandler (obj);
		NetConnection.Instance.depositBabyToStorage((uint)bCell.BabyMainData.InstId);
	}
	IEnumerator DelayOnesortClick(StorageBabyCell bCell)
	{
		yield return new WaitForSeconds(0.2f);
		if(!sDouble)
		{
			tipsObj.SetActive (true);
			BabyTipsUI btp = tipsObj.GetComponent<BabyTipsUI> ();
			if(bCell.gameObject.name == "agCell")
			{
				btp.isbabyList = false;
			}else
			{
				btp.isbabyList = true;
			}
			btp.baby =bCell.BabyMainData;
			btp.bcell = bCell;


		}
	}
//
	void Refresh()
	{
		if(babyGrid == null)return;
		foreach(Transform tra in babyGrid.transform)
		{
			Destroy(tra.gameObject);
		}
	}
	void ShowStorageItem()
	{

		for(int i=0; i < BabyCells.Length;i++)
		{
			GameObject obj = Object.Instantiate(cItem) as GameObject;
			obj.SetActive(true);
			obj.name = "agCell";
			cGrid.AddChild(obj.transform);
			StorageBabyCell sbcell = obj.GetComponent<StorageBabyCell>();
			sbcell.numsp.spriteName = "";
			BabyCells[i] = obj;
			obj.transform.localScale = Vector3.one;
		}

	}

	void OnDestroy()
	{
		BankSystem.OnUpdateStoragebabyItemOk -= RefreshBabyListUI;
		BankSystem.OnSortBabyStorageOk -= SortBabyStorageOk;
		GamePlayer.Instance.OnBabyUpdate -= BankSystem.instance.UpdateUI;

	}
}
