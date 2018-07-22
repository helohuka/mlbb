using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;



	
public delegate void OnTouchButtonHandler (ButtonScript obj, object args, int param1, int param2);// touch button

public delegate void ItemChangedEventHandler(COM_Item item);
public delegate void ItemDelEventHandler(uint instId);
public delegate void ItemDelInstEventHandler(COM_Item item);
public delegate void ItemUpdateEventHandler(COM_Item item);
public delegate void ItemSortEventHandler();

public delegate void WearEquipEventHandler(uint target,COM_Item item);
public delegate void DelEquipEventHandler(uint target,uint slot);

public delegate void GatherKindSelectHandler(GameObject obj,int id); ///选择采集类型
public delegate void GatherItemHandler(List<COM_DropItem> items); //完成采集获得的道具

public delegate void UpdateFriendHandler(COM_ContactInfo contact, bool isNewFrend);
public delegate void UpdateBlackHandler(COM_ContactInfo contact, bool isNewBlack);
public delegate void UpdateOffenHandler(COM_ContactInfo contact,bool isNewOff);
public delegate void FriendChatHandler(COM_ContactInfo contact,COM_Chat msg);

public delegate void EmpOnBattleEvent(Employee inst,int group);

public delegate void RequestEventHandler<T>(T value );
public delegate void FindFriendFailHandler();
public delegate void ArenaRivalHandler();
public delegate void GatherTimeOut();
public delegate void CompoundOkEventHandler();

public enum EnumButtonEvent
{
	OnClick,
	CheckOn,
	CheckOff,
	OnPress,
	TouchDown,
	TouchUp,
	OnDrop,
}

public class Define
{

    public static float CALC_BASE_FightingForce(COM_Item item)
    {
        if (item == null)
            return -1000f;

        float sumForce = 0f;
        for (int i = 0; i < item.propArr.Length; ++i)
        {
            switch (item.propArr[i].type_)
            {
                case PropertyType.PT_HpMax:
                    sumForce += item.propArr[i].value_ * 1.25f;
                    break;
                case PropertyType.PT_MpMax:
                    sumForce += item.propArr[i].value_ * 1.0f;
                    break;
                case PropertyType.PT_Attack:
                    sumForce += item.propArr[i].value_ * 7.5f;
                    break;
                case PropertyType.PT_Defense:
                    sumForce += item.propArr[i].value_ * 5.0f;
                    break;
                case PropertyType.PT_Agile:
                    sumForce += item.propArr[i].value_ * 5.0f;
                    break;
                case PropertyType.PT_Spirit:
                    sumForce += item.propArr[i].value_ * 12.5f;
                    break;
                case PropertyType.PT_Reply:
                    sumForce += item.propArr[i].value_ * 12.5f;
                    break;
                default:
                    break;
            }
        }
        return sumForce;
    }

    public static DateTime TransUnixTimestamp(long tt)
    {
        return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0)).AddSeconds(tt).ToLocalTime();
    }

    public static void FormatUnixTimestamp(ref string inout, long tt)
    {
        inout = TransUnixTimestamp(tt).ToString(inout, DateTimeFormatInfo.InvariantInfo);
    }

    public static long GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return (long)ts.TotalSeconds;
    }
}

public static class ArrayTool
{
    public static T[] NewArray<T>(int size ,bool batch = true) where T : new()
    {
        T[] r = new T[size];
        if (batch)
        {
            for (int i = 0; i < size; ++i)
            {
                r[i] = new T();
            }
        }
        return r;
    }
}
