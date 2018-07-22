using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public static class ZhuanPanSystem  {

	public delegate void GozhuanpanOK(uint[] items);
	public static event  GozhuanpanOK Gozhuanpan;

	public delegate void updateZhuanpanNoticeOk(COM_Zhuanpan Zhuanpan);
	public static event  updateZhuanpanNoticeOk UpdateZhuanpanNoticeOk;

	public delegate void sycnZhuanpanNoticOk(List<COM_Zhuanpan> zhans);
	public static event  sycnZhuanpanNoticOk ZhuanpanNotOk;

	public static List<COM_Zhuanpan> ZhuanpanList = new List<COM_Zhuanpan> ();
	public static List<uint>itemsList = new List<uint> ();
	public static List<COM_ZhuanpanContent > zdata = new List<COM_ZhuanpanContent> ();
	public static COM_ZhuanpanData zhuanData;
	static public void sycnZhuanpanNotice(COM_ZhuanpanData data)
	{
		zhuanData = data;
		ZhuanpanList.Clear ();
		zdata.Clear ();
		ZhuanpanList.AddRange (data.rarity_);
		zdata.Clear ();
		zdata.AddRange (data.contents_);
		if(ZhuanpanNotOk != null)
		{
			ZhuanpanNotOk(ZhuanpanList);
		}
	}
	static public void updateZhuanpanNotice(COM_Zhuanpan Zhuanpan)
	{
		ZhuanpanList.Add (Zhuanpan);
		if(UpdateZhuanpanNoticeOk != null)
		{
			UpdateZhuanpanNoticeOk(Zhuanpan);
		}
	}
	static public void zhuanpanOK(uint[] items)
	{
		int cost = 0;
		if(items.Length == 1)
			GlobalValue.Get(Constant.C_ZhuanPanOneGo, out cost);
		else
			GlobalValue.Get(Constant.C_ZhuanPanTenGo, out cost);
		for(int i=0; i < items.Length; ++i)
		{
			CommonEvent.ExcutePurchase((int)items[i], 1, cost / items.Length);
		}
		itemsList.Clear ();
		itemsList.AddRange (items);
		if(Gozhuanpan != null)
		{
			Gozhuanpan(items);
		}
	}

	static public COM_ZhuanpanContent GetData(int lootId)
	{
		for(int i=0; i < zdata.Count; ++i)
		{
			if(zdata[i].id_ == lootId)
				return zdata[i];
		}
		return null;
	}
}
