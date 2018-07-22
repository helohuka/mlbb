using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompoundSystem 
{
	public bool isInit;
	public bool isOpenInit;
	public bool bTiSheng;
	private  List<uint> openEquipList = new List<uint> ();

	private static CompoundSystem _instance;
	public static CompoundSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new CompoundSystem();
			return _instance;
		}
	}

	public bool IsOpenInit
	{
		set
		{
			isOpenInit = value;
		}
		get
		{
			return isOpenInit;
		}
	}

	public List<uint> GetOpenEquipList
	{
		get
		{
			return openEquipList;
		}	
	}

	public void InitOpenEquip(uint[] arr)
	{
		openEquipList.Clear ();
		for(int i =0;i<arr.Length;i++)
		{
			openEquipList.Add(arr[i]);
		}
		isInit = true;
		if(isOpenInit)
		{
			isOpenInit = false;
			NetWaitUI.HideMe();
			CompoundUI.SwithShowMe(bTiSheng);
            
		}
	}

	public void AddOpenEquip(uint id)
	{
		if (openEquipList.Contains (id))
			return;
		openEquipList.Add (id);
		ItemData iData = ItemData.GetData ((int)id);
		PopText.Instance.Show (LanguageManager.instance.GetValue ("xuehuifengfang").Replace("{n}",iData.name_));
	}

	public bool GetIsOPenEquip( uint id)
	{
		if (openEquipList.Contains (id))
		{
		   return true;
		}
			
		return false;
	}



		
}

