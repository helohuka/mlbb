using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public  class BankSystem {

	public delegate void ItemSortHandler();
	public static event ItemSortHandler OnSortItem;
	
	public delegate void UpdateitemHandler();
	public static event UpdateitemHandler OnUpdateItemOk;
	public delegate void UpdateitemBagHandler();
	public static event UpdateitemBagHandler OnUpdateBagItemOk;

	public delegate void UpdateStorageHandler();
	public static event UpdateStorageHandler OnUpdateStorageItemOk;

	public delegate void UpdateStorageBabyHandler();
	public static event UpdateStorageBabyHandler OnUpdateStoragebabyItemOk;


	public delegate void SortBabyStorageHandler(List<COM_BabyInst> binst);
	public static event SortBabyStorageHandler OnSortBabyStorageOk;

	public static List<COM_BabyInst> babyinsts = new List<COM_BabyInst>();

	public  int itemNum = 20;
	public  int babyNum = 20;
	public  bool isopen;

	public bool isOpeninitBank;
	public int isOpenBankType;

	private static BankSystem _instance;
	public static BankSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new BankSystem();
			return _instance;
		}
	}


	public  void sortBagItemOk()
	{
		if (OnSortItem != null) 
		{
			OnSortItem ();
		}
	}

	public void DepositItem(COM_Item item)
	{
		GamePlayer.Instance.StorageItems [item.slot_] = item;
		if (OnUpdateItemOk != null) 
		{
			OnUpdateItemOk ();
		}
	}
	public  void GetoutItemOK(ushort slot)
	{
		if(GamePlayer.Instance.StorageItems[slot] != null)
		{
			GamePlayer.Instance.StorageItems[(int)slot] = null;

		}
		if (OnUpdateBagItemOk != null) 
		{
			OnUpdateBagItemOk ();
		}

		//NetConnection.Instance.sortBagItem ();
	}

	public  void  sortItemStorage(COM_Item[] itemids)
	{
		for(int i =0;i<GamePlayer.Instance.StorageItems.Length;i++)
		{
			GamePlayer.Instance.StorageItems[i] = null;
		}
		for(int i =0;i<itemids.Length;i++)
		{
			GamePlayer.Instance.StorageItems[itemids[i].slot_] = itemids[i];
		}

		if (OnUpdateStorageItemOk != null) 
		{
			OnUpdateStorageItemOk ();
		}
	}

	public  void DepositBabyOK(COM_BabyInst inst)
	{
		for(int i=0;i<GamePlayer.Instance.babies_list_.Count;i++)
		{
			if(GamePlayer.Instance.babies_list_[i].InstId == inst.instId_)
			{
				GamePlayer.Instance.babies_list_.RemoveAt(i);
			}
		}
		GamePlayer.Instance.Storagebaby [inst.slot_] = inst;
		if(MainbabyListUI.RefreshBabyListOk != null)
		{
			MainbabyListUI.RefreshBabyListOk((int)inst.instId_);
		}
		UpdateUI();
	}
	public  void GetoutBabyOK(ushort slot)
	{

		if(GamePlayer.Instance.Storagebaby[slot] != null)
		{
			GamePlayer.Instance.Storagebaby[(int)slot] = null;

			UpdateUI();
			if(MainbabyListUI.RefreshBabyListOk != null)
			{
				MainbabyListUI.RefreshBabyListOk(0);
			}
		}
	}

	public  void UpdateUI()
	{
		if (OnUpdateStoragebabyItemOk != null) 
		{
			OnUpdateStoragebabyItemOk ();
		}
	}

	public void SortBabyStorage(uint[] babyid)
	{
		List<COM_BabyInst> temps = new List<COM_BabyInst> ();
		COM_BabyInst item = null;
		for(int i =0;i<babyid.Length;i++)
		{
			item = GamePlayer.Instance.GetBabyInStoreById((int)babyid[i]);
			item.slot_ = (ushort)i;
			temps.Add(item);
		}
		if(OnSortBabyStorageOk != null)
		{
			OnSortBabyStorageOk(temps);
		}
	}
	public  int GetBagSize()
	{
		int size = 0;
		for(int i=0;i<GamePlayer.Instance.StorageItems.Length;i++ )
		{
			if(GamePlayer.Instance.StorageItems[i] != null)
			{
				size++;
			}
		}
		
		return size;
	}
	public  bool IsStorageFull()
	{
		if(GetBagSize()==BankSystem.instance.itemNum)
		{

			return true;
		}
		return false;
	}

	public bool IsBabyStorageFull()
	{
		if(GetBabySize()==BankSystem.instance.babyNum)
		{
			
			return true;
		}
		return false;
	}

	public bool IsBabyListFull()
	{
		if(GamePlayer.Instance.babies_list_.Count>=3)
		{
			return true;
		}
		return false;
	}

	public  int GetBabySize()
	{
		int size = 0;
		for(int i=0;i<GamePlayer.Instance.Storagebaby.Length;i++ )
		{
			if(GamePlayer.Instance.Storagebaby[i] != null)
			{
				size++;
			}
		}
		
		return size;
	}
}
