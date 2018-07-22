using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BagSystem  {

	public event ItemChangedEventHandler ItemChanged;
	public event ItemDelEventHandler DelItemEvent;
    public event ItemDelInstEventHandler DelItemInstEvent;
	public event ItemUpdateEventHandler UpdateItemEvent;
	public event ItemSortEventHandler SortItemEvent;
	public event ItemSortEventHandler OpenBagGridEvent;
	public RequestEventHandler<bool> BattleOpenBagEvent;
    public uint recentlyTakeoffEquip;
    public bool addItemIsFromBody;

	public static bool isUse;
	private Dictionary<uint, COM_Item> _Items =  new Dictionary<uint, COM_Item>();
	private COM_Item[]   _BagItems ;//= new COM_Item[100];

	private static BagSystem _instance;

    public List<COM_Item> _BabyEquips = new List<COM_Item>();

	public int _openBagNum = 20;
	public bool isBattlebagfull;
	public bool isBattleOpenBag;
	private bool isInitBag; 
	public bool isOpenInitBag;
    public bool isDirty_;
    public bool isBabyEquipDirty_;

	public uint _remainTimeStart;

	public static BagSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new BagSystem();
			return _instance;
		}
	}
	
    public bool IsInit
    {
        get { return isInitBag; }
    }

    public void Clean()
    {
        isInitBag = false;
    }

	public void InitBag(COM_Item[] items)
	{
		_BagItems = new COM_Item[100];
        _BabyEquips.Clear();
		for(int j= 0;j<_BagItems.Length;j++)
		{
			_BagItems[j] = null;
		}
        ItemData iData = null;
		for (int i = 0; i<items.Length; i++) 
		{
			_BagItems[items[i].slot_] = items[i];
            iData = ItemData.GetData((int)items[i].itemId_);
            if (iData.mainType_ == ItemMainType.IMT_BabyEquip)
                _BabyEquips.Add(items[i]);
            QuestSystem.CheckItemQuest(items[i].itemId_);
		}
        isBabyEquipDirty_ = true;
		isInitBag = true;
		if(isOpenInitBag)
		{
			isOpenInitBag = false;
            NetWaitUI.HideMe();
			BagUI.SwithShowMe ();
		}
        isDirty_ = true;
	}
	public void BagClear()
	{
		if (_BagItems == null)
			return;
		for(int j= 0;j<_BagItems.Length;j++)
		{
			_BagItems[j] = null;
		}
        _BabyEquips.Clear();
	}

	public bool battleOpenBag
	{
		set
		{
			isBattleOpenBag = value;
			if(!isBattleOpenBag)
			{
				if(BattleOpenBagEvent != null)
				{
					BattleOpenBagEvent(false);
				}
			}
		}
		get
		{
			return isBattleOpenBag;
		}
	}


	public COM_Item[] BagItems
	{
		get
		{
			return _BagItems;
		}
	}

	public COM_Item GetItemInstBySlot(int slot)
	{
		return _BagItems[slot];
	}

	public void AddItem(COM_Item item) 
	{
        ItemData iData = ItemData.GetData((int)item.itemId_);
        if (iData == null) 
		{
			return;
		}

		_BagItems[item.slot_] = item;

        if (iData.mainType_ == ItemMainType.IMT_BabyEquip)
        {
            _BabyEquips.Add(item);
            isBabyEquipDirty_ = true;
        }
        if (item.instId_ == recentlyTakeoffEquip)
            addItemIsFromBody = true;
		if (ItemChanged != null) 
		{
			ItemChanged.Invoke(item);
//			if(ItemData.GetData((int)item.itemId_).mainType_ == ItemMainType.IMT_Equip)
//			{
//				FastUpload.Instance.OnItemAdd(item);
//			}
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_GainItem, (int)item.itemId_);
		}
        isDirty_ = true;
        addItemIsFromBody = false;
        recentlyTakeoffEquip = 0;
		//COM_Chat info = new COM_Chat ();
//		COM_ChatInfo info = new COM_ChatInfo ();
//		info.ck_ = ChatKind.CK_System;
//		info.content_ = LanguageManager.instance.GetValue("getitem").Replace("{n}",ItemData.GetData((int)item.itemId_).name_);
		//NetConnection.Instance.sendChat(info, "");
		string itemName = iData.name_; 
		string itemColor = "";
		if ((int)iData.quality_ >= (int)QualityColor.QC_Orange2)
		{
			itemColor ="fb851c";
		} 
		if((int)iData.quality_ <= (int)QualityColor.QC_White)
		{
			itemName =  "";
		}
		else if ((int)iData.quality_ <= (int)QualityColor.QC_Green)
		{
			itemColor ="5efd52";
		}
		else if((int)iData.quality_ <= (int)QualityColor.QC_Blue1)
		{
			itemColor ="4594ff";
		}
		else if ((int)iData.quality_ <= (int)QualityColor.QC_Purple2)
		{
			itemColor ="ad52fa";
		}
		else if ((int)iData.quality_ <= (int)QualityColor.QC_Golden2)
		{
			itemColor ="eef001";
		}
		else if ((int)iData.quality_ <= (int)QualityColor.QC_Orange2)
		{
			itemColor ="fb851c";
		}
		else if ((int)iData.quality_ <= (int)QualityColor.QC_Pink)
		{
			itemColor ="f66099";
		}

		if(!LotteryPanelUI.isOpen &&!CompoundUI.isOpen)
			PopText.Instance.Show (LanguageManager.instance.GetValue("getitem").Replace("{n}", ItemData.GetData((int)item.itemId_).name_),PopText.WarningType.WT_None, false,itemColor);
        ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("getitem").Replace("{n}", ItemData.GetData((int)item.itemId_).name_));

		EmployessSystem.instance.UpdateEmployeeRed ();
        QuestSystem.CheckItemQuest(item.itemId_);
		SoundTools.PlaySound (SOUND_ID.SOUND_HUODEWUQIN);
	}

	public void DelItem(uint instId)
	{
	
		if (_BagItems [instId] == null) 
		{
			return;
		}

        COM_Item tmpDelInst = _BagItems[instId];
		QuestSystem.CheckItemQuest(tmpDelInst.itemId_);
        _BagItems[instId] = null;

        for (int i = 0; i < _BabyEquips.Count; ++i)
        {
            if (_BabyEquips[i].slot_ == instId)
            {
                _BabyEquips.RemoveAt(i);
                isBabyEquipDirty_ = true;
                break;
            }
        }

        if (DelItemInstEvent != null)
            DelItemInstEvent(tmpDelInst);

		if (DelItemEvent != null) 
		{
			DelItemEvent.Invoke(instId);
		}

        isDirty_ = true;
		EmployessSystem.instance.UpdateEmployeeRed ();


	}

	public void UpdateItem(COM_Item item)
	{
		if(BagItems[item.slot_] == null)
		{
			return;
		}


		COM_Item bagItem = GetItemByInstId ((int)item.instId_);
		if (bagItem != null)
		{
			ItemData iData = ItemData.GetData((int)item.itemId_);
			string itemName = iData.name_; 
			string itemColor = "";
			if ((int)iData.quality_ >= (int)QualityColor.QC_Orange2)
			{
				itemColor ="fb851c";
			} 
			if((int)iData.quality_ <= (int)QualityColor.QC_White)
			{
				itemName =  "";
			}
			else if ((int)iData.quality_ <= (int)QualityColor.QC_Green)
			{
				itemColor ="5efd52";
			}
			else if((int)iData.quality_ <= (int)QualityColor.QC_Blue1)
			{
				itemColor ="4594ff";
			}
			else if ((int)iData.quality_ <= (int)QualityColor.QC_Purple2)
			{
				itemColor ="ad52fa";
			}
			else if ((int)iData.quality_ <= (int)QualityColor.QC_Golden2)
			{
				itemColor ="eef001";
			}
			else if ((int)iData.quality_ <= (int)QualityColor.QC_Orange2)
			{
				itemColor ="fb851c";
			}
			else if ((int)iData.quality_ <= (int)QualityColor.QC_Pink)
			{
				itemColor ="f66099";
			}



			int num = bagItem.stack_;
			int curNum = num - item.stack_;
			if(curNum>0)
			{
				if(!LotteryPanelUI.isOpen &&!CompoundUI.isOpen )
				{
					PopText.Instance.Show (LanguageManager.instance.GetValue("xiaohaoitemwupin").Replace("{n}", ItemData.GetData((int)item.itemId_).name_).Replace("{n1}", Mathf.Abs(curNum).ToString()), PopText.WarningType.WT_Tip, GamePlayer.Instance.isInBattle);
				}
				ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("xiaohaoitemwupin").Replace("{n}", ItemData.GetData((int)item.itemId_).name_).Replace("{n1}", Mathf.Abs(curNum).ToString()));
			}else if(curNum<0)
			{

				if(!LotteryPanelUI.isOpen && !CompoundUI.isOpen)
				{
					PopText.Instance.Show (LanguageManager.instance.GetValue("getitemwupin").Replace("{n}", ItemData.GetData((int)item.itemId_).name_).Replace("{n1}", Mathf.Abs(curNum).ToString()),PopText.WarningType.WT_None,false,itemColor);
					//PopText.Instance.Show (LanguageManager.instance.GetValue("getitem").Replace("{n}", ItemData.GetData((int)item.itemId_).name_));
				}
				ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("getitemwupin").Replace("{n}", ItemData.GetData((int)item.itemId_).name_).Replace("{n1}", Mathf.Abs(curNum).ToString()));

			}
		}
		BagItems [item.slot_] = item;

		if (UpdateItemEvent != null) 
		{
			UpdateItemEvent.Invoke(item);
		}
        isDirty_ = true;

		ItemData idata = ItemData.GetData ((int)item.itemId_);
		if(idata == null)return;
		if(idata.mainType_ == ItemMainType.IMT_Consumables)
		{
			isUse = true;
			//ChatSystem.instance.AddchatInfo( LanguageManager.instance.GetValue("delitem").Replace("{n}",idata.name_),ChatKind.CK_System,false);
		}
		EmployessSystem.instance.UpdateEmployeeRed ();
        QuestSystem.CheckItemQuest(item.itemId_);
		SoundTools.PlaySound (SOUND_ID.SOUND_HUODEWUQIN);
	}

	public void sortBagItemOk()
	{
		if (SortItemEvent != null) 
		{
			SortItemEvent ();
		}
	}

    static int UpdateUseTimeout = 5;
    float CurrentUseTimeout = 0.0F;
    public void UpdateUsetime()
    {
        CurrentUseTimeout += Time.deltaTime;
        if (CurrentUseTimeout > UpdateUseTimeout)
        {
			CurrentUseTimeout -= UpdateUseTimeout;
            UpdateUsetime(UpdateUseTimeout);
        }
    }

    public void UpdateUsetime(int dt)
    {
		if (_BagItems == null)
			return;
        for (int i = 0; i < _BagItems.Length; ++i)
        {
            if (null == _BagItems[i])
                continue;
			if (_BagItems[i].usedTimeout_ > 0 )
			{
				_BagItems[i].usedTimeout_ -= dt;
			}
			if (_BagItems[i].lastSellTime_ > 0 )
			{
				_BagItems[i].lastSellTime_ -= dt;
			}
        }
		if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Fashion] != null)
		{
			if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Fashion].usedTimeout_ > 0)
			{
				GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Fashion].usedTimeout_ -= dt;
			}
		}
    }

	public void OnBagItemUpdate()
	{
        

	}
	
	public void UpdateItemCount(uint id, uint count)
	{
	
	}

	public int GetItemCount(uint instId)
	{
		return 0;
	}

	public int GetBagSize()
	{
		if(_BagItems == null)
		{
			return 0;
		}
		int size = 0;
		for(int i=0;i<_BagItems.Length;i++ )
		{
			if(_BagItems[i] != null)
			{
				++size;
			}
		}
		
		return size;
	}


	public COM_Item GetItemByItemId(uint id)
	{
		COM_Item item = null;
		for(int i=0;i<_BagItems.Length;i++ )
		{
			if(_BagItems[i] == null)
				continue;
			if(_BagItems[i].itemId_ == id)
			{
				item = _BagItems[i];
				break;
			}
		}

		return item;
	}

    public COM_Item GetItemByInstId(int instId)
    {
        COM_Item item = null;
        for (int i = 0; i < _BagItems.Length; i++)
        {
            if (_BagItems[i] == null)
                continue;
            if (_BagItems[i].instId_ == instId)
            {
                item = _BagItems[i];
                break;
            }
        }
        return item;
    }
	
	public List<COM_Item> GetBagTypeItems(uint kind)
	{
		List<COM_Item> list = new List<COM_Item>();
		foreach(KeyValuePair<uint, COM_Item> kv in _Items)
		{

		}
		return list;
	}


	public int GetItemMaxNum(uint id)
	{
        if (!isInitBag || _BagItems == null)
            return 0;
		int num = 0;
		for(int i=0;i<_BagItems.Length;i++ )
		{
			if(_BagItems[i] == null)
				continue;
			if(_BagItems[i].itemId_ == id) 
			{
				num += (int)_BagItems[i].stack_;
			}
		}
		return num;
	}

	public void OpenBugGridOk(int num)
	{
		GamePlayer.Instance.SetIprop (PropertyType.PT_BagNum, num);
		_openBagNum = GamePlayer.Instance.GetIprop (PropertyType.PT_BagNum);
		if (OpenBagGridEvent != null)
		{
			OpenBagGridEvent ();
		}

	}
	
	public bool CheckHaveItem(uint id)
	{

		return false;
	}

	public bool BagIsFull()
	{
		return  GetBagSize () >= GamePlayer.Instance.GetIprop (PropertyType.PT_BagNum); 
	}

	public List<COM_Item> GetGemList()
	{
		List<COM_Item> gemList = new List<COM_Item> ();
		for(int i=0;i<_BagItems.Length;i++)
		{
			if(_BagItems[i] == null)
				continue;
			
			ItemData item = ItemData.GetData((int)_BagItems[i].itemId_);
			if(item.mainType_ == ItemMainType.IMT_Consumables  &&  item.subType_ == ItemSubType.IST_Gem)
			{
				gemList.Add(_BagItems[i]);
			}
		}

		return gemList;
	}

	public List<COM_Item> GetMainTypeItems(ItemMainType type )
	{
		List<COM_Item> list = new List<COM_Item> ();
		for(int i=0;i<_BagItems.Length;i++)
		{
			if(_BagItems[i] == null)
				continue;
			
			ItemData item = ItemData.GetData((int)_BagItems[i].itemId_);
			if(item.mainType_ == type)
			{
				list.Add(_BagItems[i]);
			}
		}
		
		return list;
	}


	public string GetQualityBack(int quality)
	{
		if ((int)quality >= (int)QualityColor.QC_Pink)
		{
			return "bb_daojukuang7";
		} 
		if((int)quality <= (int)QualityColor.QC_White)
		{
			return "bb_daojukuang1";
		}
		else if ((int)quality <= (int)QualityColor.QC_Green)
		{
			return "bb_daojukuang2";
		}
		else if((int)quality <= (int)QualityColor.QC_Blue1)
		{
			return "bb_daojukuang3";
		}
		else if ((int)quality <= (int)QualityColor.QC_Purple2)
		{
			return "bb_daojukuang4";
		}
		else if ((int)quality <= (int)QualityColor.QC_Golden2)
		{
			return "bb_daojukuang5";
		}
		else if ((int)quality <= (int)QualityColor.QC_Orange2)
		{
			return "bb_daojukuang6";
		}
		else if ((int)quality <= (int)QualityColor.QC_Pink)
		{
			return "bb_daojukuang7";
		}
		return "bb_daojukuang1";
	}

    public int GetEmptySlotNum()
    {
        int totalslot = GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum);
        int total = 0;
        for (int i = 0; i < totalslot; ++i)
        {
            if (_BagItems[i] == null)
                total++;
        }
        return total;
    }

	public uint GetCDTime(int time)
	{
		return (uint)Mathf.Max(0, (float)time - (Time.realtimeSinceStartup -(float)_remainTimeStart));
	} 


}
