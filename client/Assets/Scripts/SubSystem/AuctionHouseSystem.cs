using UnityEngine;
using System;
using System.Collections.Generic;

public class AuctionHouseSystem {

    public static List<COM_SellItem> mySellingList_;

    public static List<COM_SelledItem> mySellRecordList_;

    public static List<COM_SellItem> otherSellingList_;

    public static int currentPage_ = 1, totalPage_;

    public static int otherCurrentPage = 1, otherTotalPage_;

    public static int otherCurrentStartIndex = 0;

    public static int currentStartIndex_ = 0;

    public const int CountPerPage_item = 15;
    public const int CountPerPage_baby = 8;

    public const int CountPerPage = 8;

    public const int CollectionMax = 20;
    public const int SellingMax = 20;
    public const int SellRecordMax = 20;

    public const int OtherSellingMax = 3;

    public static List<COM_SellItem> crtDisplayList_;

    public static List<string> collectionList_;

    public delegate void SellListUpdate();
    public static event SellListUpdate OnSellListUpdate;

    public delegate void MySellingListUpdate();
    public static event MySellingListUpdate OnMySellingListUpdate;

    public delegate void MySelledListUpdate();
    public static event MySelledListUpdate OnMySelledListUpdate;

    public delegate void OtherSellingListUpdate();
    public static event OtherSellingListUpdate OnOtherSellingListUpdate;

    public static bool Open_;

    public static int GoodProtectDay;

    public static void InitMySelling(COM_SellItem[] items)
    {
        Open_ = true;
        mySellingList_ = new List<COM_SellItem>(items);
        collectionList_ = new List<string>(LoadCollection());
        GlobalValue.Get(Constant.C_AucGoodProtect, out GoodProtectDay);
    }

    public static void InitMySelled(COM_SelledItem[] items)
    {
        mySellRecordList_ = new List<COM_SelledItem>(items);
    }

    public static void AddMySelled(COM_SelledItem item)
    {
        if (mySellRecordList_.Count >= SellRecordMax)
            mySellRecordList_.RemoveAt(0);
        mySellRecordList_.Add(item);
        if (OnMySelledListUpdate != null)
            OnMySelledListUpdate();

        DelMySelling(item.guid_);
    }

    public static void AddMySelling(COM_SellItem item)
    {
        mySellingList_.Add(item);
        if (OnMySellingListUpdate != null)
            OnMySellingListUpdate();
    }

    public static void DelMySelling(int sellId)
    {
        for (int i = 0; i < mySellingList_.Count; ++i)
        {
            if (mySellingList_[i].guid_ == sellId)
            {
                mySellingList_.RemoveAt(i);
                break;
            }
        }
        if (OnMySellingListUpdate != null)
            OnMySellingListUpdate();
    }

    public static void UpdateOtherSelling(COM_SellItem[] items)
    {
        otherSellingList_ = new List<COM_SellItem>(items);
        if (OnOtherSellingListUpdate != null)
            OnOtherSellingListUpdate();
    }

    public static bool AddCollection(int id, bool isItem = true)
    {
        string tmp = id.ToString() + "|" + isItem.ToString();
        if(collectionList_.Contains(tmp))
            return false;
        collectionList_.Add(tmp);
        SaveCollection();
        return true;
    }

    public static void RemoveCollection(int index)
    {
        if (collectionList_.Count <= index) return;

        collectionList_.RemoveAt(index);
        SaveCollection();
    }

    static void SaveCollection()
    {
        string val = "";
        for (int i = 0; i < collectionList_.Count; ++i)
        {
            val += "," + collectionList_[i].ToString();
        }
        PlayerPrefs.SetString("xyskCollection1", val);
    }

    static public string GetFirstCollection()
    {
        if (collectionList_.Count == 0) return "";

        return collectionList_[0];
    }

    static string[] LoadCollection()
    {
        string[] sArr = PlayerPrefs.GetString("xyskCollection1").Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
        if(sArr == null)
            return null;

        return sArr;
    }

    public static void CacheOnePageItems4Display(COM_SellItem[] items)
    {
        crtDisplayList_ = new List<COM_SellItem>(items);
        if (OnSellListUpdate != null)
            OnSellListUpdate();
    }

    public static void UpdatePage(int total)
    {
        totalPage_ = total / CountPerPage + (total % CountPerPage == 0? 0 : 1);
        currentPage_ = currentStartIndex_ / CountPerPage + 1;
    }

    public static void UpdateOtherPage(int total)
    {
        otherTotalPage_ = total / OtherSellingMax + (total % OtherSellingMax == 0 ? 0 : 1);
        otherCurrentPage = otherCurrentStartIndex / OtherSellingMax + 1;
    }
}
