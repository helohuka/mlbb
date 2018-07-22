using UnityEngine;
using System.Collections.Generic;

public class MoreActivityData {

    static COM_ADCards cardsData_;
    static COM_ADHotRole hotRoleData_;
	COM_ADLoginTotal loginTotal;
	COM_ADChargeTotal selfRechargedata;  
	COM_ADChargeTotal sysRechargedata;
	COM_ADChargeEvery selfChargeEverydata;
	COM_ADChargeEvery sysChargeEverydata;
	COM_ADDiscountStore selfDiscountStore;
	COM_ADDiscountStore sysDiscountStore;
	COM_ADEmployeeTotal employeeTotal;
	COM_IntegralData IntegralData;

	public  RequestEventHandler<COM_ADLoginTotal> LoginTotalEvent;

	public  RequestEventHandler<COM_ADChargeTotal> SelfRechargeEvent;
	public  RequestEventHandler<COM_ADChargeTotal> SysRechargeEvent;

	public  RequestEventHandler<COM_ADChargeEvery> SelfChargeEveryEvent;
	public  RequestEventHandler<COM_ADChargeEvery> SysChargeEveryEvent;

	public  RequestEventHandler<COM_ADDiscountStore> SelfDiscountStoreEvent;
	public  RequestEventHandler<COM_ADDiscountStore> SysDiscountStoreEvent;
	public  RequestEventHandler<COM_ADEmployeeTotal> employeeTotalEvent;
	public  RequestEventHandler<COM_IntegralData> IntegralEvent;


	public  RequestEventHandler<int> MoreActivityRedEvent;


	public int[] redList = new int[(int)ADType.ADT_Max];


    static List<COM_Sevenday> sevenDayData_;

	public static bool onlineStart;
	public static float onlineTime;

    public static bool drawCardOk;
    public static bool hotRoleBuyNumDirty;
    public static bool sevenDaysDirty;


	private static MoreActivityData _instance;
	public static MoreActivityData instance
	{
		get
		{
			if(_instance == null)
				_instance = new MoreActivityData();
			return _instance;
		}
	}

    public static void TestHotRoleData()
    {
        if (hotRoleData_ != null)
            return;
        COM_ADHotRole role = new COM_ADHotRole();
        COM_ADHotRoleContent dd = new COM_ADHotRoleContent();
        dd.buyNum_ = 5;
        dd.type_ = EntityType.ET_Baby;
        dd.roleId_ = 29;
        dd.price_ = 300;
        role.contents_ = new COM_ADHotRoleContent[1];
        role.contents_[0] = dd;
        InitHotRoleData(role);
    }

	public static void InitCardsData(COM_ADCards cardsData)
    {
        cardsData_ = cardsData;
    }

    public static void UpdateCardsData(COM_ADCardsContent content)
    {
        if(cardsData_ == null)
            return;

        for (int i = 0; i < cardsData_.contents_.Length; ++i)
        {
            if(cardsData_.contents_[i].count_ == content.count_)
            {
                cardsData_.contents_[i] = content;
                return;
            }
        }

        COM_ADCardsContent[] newCards = new COM_ADCardsContent[cardsData_.contents_.Length + 1];
        for (int i = 0; i < cardsData_.contents_.Length; ++i)
        {
            newCards[i] = cardsData_.contents_[i];
        }
        newCards[newCards.Length - 1] = content;
        cardsData_.contents_ = newCards;

        drawCardOk = true;
    }

    public static int GetCardReward(int idx)
    {
        if(cardsData_ == null)
            return 0;

        for (int i = 0; i < cardsData_.contents_.Length; ++i)
        {
            if (cardsData_.contents_[i].count_ == idx)
            {
                return (int)cardsData_.contents_[i].rewardId_;
            }
        }
        return 0;
    }

    public static COM_ADCards GetCardsData()
    {
        return cardsData_;
    }

    public static void ClearCardsData()
    {
        if(cardsData_ != null)
            cardsData_.contents_ = new COM_ADCardsContent[0];
        drawCardOk = true;
    }

    public static void InitHotRoleData(COM_ADHotRole hotRoleData)
    {
        hotRoleData_ = hotRoleData;
    }

    public static void UpdateHotRoleBuyNum(int num)
    {
        if (hotRoleData_ == null)
            return;

        hotRoleData_.contents_[0].buyNum_ = (uint)num;
        hotRoleBuyNumDirty = true;
    }

    public static COM_ADHotRoleContent GetHotRoleData()
    {
        if (hotRoleData_ == null)
            return null;
        return hotRoleData_.contents_[0];
    }

    public static void Init7DaysData(COM_Sevenday[] datas)
    {
        sevenDayData_ = new List<COM_Sevenday>(datas);

		SevenDaysData sdd = null;
		for(int i =0;i<datas.Length;i++)
		{ 
			sdd = SevenDaysData.GetData((int)datas[i].quest_);
			if(datas[i].isfinish_&& !datas[i].isreward_ && sdd.day <= GamePlayer.Instance.DaysOld)
			{
				//MoreActivityData.instance.SetTypeRad((int)ADType.ADT_7Days,1);
				return;
			}
		}

		//MoreActivityData.instance.SetTypeRad((int)ADType.ADT_7Days,0);
    }

    public static void Update7Days(COM_Sevenday data)
    {
        for (int i = 0; i < sevenDayData_.Count; ++i)
        {
            if (sevenDayData_[i].quest_ == data.quest_)
            {
                sevenDayData_[i] = data;
                sevenDaysDirty = true;
				Check7DayRed();
				return;
            }
        }

        sevenDayData_.Add(data);
        sevenDaysDirty = true;
		Check7DayRed ();
    }

	static void Check7DayRed()
	{
		SevenDaysData sdd = null;
		for(int i =0;i<sevenDayData_.Count;i++)
		{ 
			sdd = SevenDaysData.GetData((int)sevenDayData_[i].quest_);
			if(sevenDayData_[i].isfinish_ && !sevenDayData_[i].isreward_ && sdd.day <= GamePlayer.Instance.DaysOld)
			{
				//MoreActivityData.instance.SetTypeRad((int)ADType.ADT_7Days,1);
				return;
			}
		}
		
		//MoreActivityData.instance.SetTypeRad((int)ADType.ADT_7Days,0);
	}

    public static COM_Sevenday Get7DaysData(int id)
    {
        if(sevenDayData_ == null)
            return null;
        for (int i = 0; i < sevenDayData_.Count; ++i)
        {
            if (sevenDayData_[i].quest_ == id)
                return sevenDayData_[i];
        }
        return null;
    }
	public static void verificationSMS(string num)
	{
		MoreActivityData.instance.SetTypeRad((int)ADType.ADT_PhoneNumber, 0);
		GamePlayer.Instance.adTypes.Remove (ADType.ADT_PhoneNumber);
	}
    //public static bool isBuy()
    //{
    //    if (hotRoleData_ == null)
    //        return true;
    //    return hotRoleData_.isBuy_;
    //}

    public static void ClearHotRoleData()
    {
        hotRoleData_.contents_ = new COM_ADHotRoleContent[0];
    }

	public COM_ADLoginTotal GetLoginTotal()
	{
		return loginTotal;
	}

	public void UpdateLoginTotal(COM_ADLoginTotal data)
	{
		loginTotal = data;
		redList [(int)ADType.ADT_LoginTotal] = 0;
		for(int i =0;i<data.contents_.Length;i++)
		{ 
			if(data.contents_[i].status_ == 1)
			{
				redList[(int)ADType.ADT_LoginTotal] =1;
				break;
			}
		}

		if (MoreActivityRedEvent != null)
		{
			MoreActivityRedEvent (1);
		}
		if(LoginTotalEvent !=null)
		{
			LoginTotalEvent(data);
		}

	}


	public COM_ADChargeTotal GetSelfRechargel()
	{
		return selfRechargedata;
	}
	
	public void UpdateSelfRecharge(COM_ADChargeTotal data)
	{
		selfRechargedata = data;

		if(SelfRechargeEvent != null)
		{
			SelfRechargeEvent(data);
		}

		for(int i =0;i<data.contents_.Length;i++)
		{ 
			if(data.contents_[i].status_ == 1)
			{
				SetTypeRad((int)ADType.ADT_SelfChargeTotal,1);
				return;
			}
		}
		SetTypeRad((int)ADType.ADT_SelfChargeTotal,0);
	}


	public COM_ADChargeTotal GetSysRechargel()
	{
		return sysRechargedata;
	}
	
	public void UpdateSysRecharge(COM_ADChargeTotal data)
	{
		sysRechargedata = data;
	
		if(SysRechargeEvent != null)
		{
			SysRechargeEvent(data);
		}

		for(int i =0;i<data.contents_.Length;i++)
		{ 
			if(data.contents_[i].status_ == 1)
			{
				SetTypeRad((int)ADType.ADT_ChargeTotal,1);
				return;
			}
		}
		SetTypeRad((int)ADType.ADT_ChargeTotal,0);

	}
	
	public COM_ADChargeEvery GetSysChargeEvery()
	{
		return sysChargeEverydata;
	}
	
	public  void UpdateSysChargeEvery(COM_ADChargeEvery data)
	{
		sysChargeEverydata = data;
	
		if(SysChargeEveryEvent != null)
		{
			SysChargeEveryEvent(data);
		}

		for(int i =0;i<data.contents_.Length;i++)
		{ 
			if(data.contents_[i].status_ == 1)
			{
				SetTypeRad((int)ADType.ADT_ChargeEvery,1);
				return;
			}
		}
		SetTypeRad((int)ADType.ADT_ChargeEvery,0);

	}

	public COM_ADEmployeeTotal GetEmployeeTotal()
	{
		return employeeTotal;
	}


	public void updateEmployeeActivity(COM_ADEmployeeTotal empData)
	{
		employeeTotal = empData;

		redList [(int)ADType.ADT_BuyEmployee] = 0;
		for(int i =0;i<empData.contents_.Length;i++)
		{ 
			if(empData.contents_[i].status_ == 1)
			{
				redList[(int)ADType.ADT_BuyEmployee] =1;
				break;
			}
		}
		if (MoreActivityRedEvent != null)
		{
			MoreActivityRedEvent (1);
		}


		if(employeeTotalEvent != null)
		{
			employeeTotalEvent(empData);
		}
	}

	public COM_ADChargeEvery GetSelfChargeEvery()
	{
		return selfChargeEverydata;
	}
	
	public void UpdateSelfChargeEvery(COM_ADChargeEvery data)
	{
		selfChargeEverydata = data;

		redList [(int)ADType.ADT_SelfChargeEvery] = 0;
		for(int i =0;i<data.contents_.Length;i++)
		{ 
			if(data.contents_[i].status_ == 1)
			{
				redList[(int)ADType.ADT_SelfChargeEvery] =1;
				break;
			}
		}

		if (MoreActivityRedEvent != null)
		{
			MoreActivityRedEvent (1);
		}

		if(SelfChargeEveryEvent != null)
		{
			SelfChargeEveryEvent(data);
		}
	}

	public COM_ADDiscountStore GetSelfDiscountStore()
	{
		return selfDiscountStore;
	}

	public void UpdateSelfDiscountStore(COM_ADDiscountStore data)
	{
		selfDiscountStore = data;
	
		if(SelfDiscountStoreEvent != null)
		{
			SelfDiscountStoreEvent(data);
		}
	}

	public COM_ADDiscountStore GetSysDiscountStore()
	{
		return sysDiscountStore;
	}

	public void UpdateSysDiscountStore(COM_ADDiscountStore data)
	{
		sysDiscountStore = data;
		if(SysDiscountStoreEvent != null)
		{
			SysDiscountStoreEvent(data);
		}
	}

	public void updateIntegralShop(COM_IntegralData data)
	{
		IntegralData = data;
		if(IntegralEvent != null)
		{
			IntegralEvent(data);
		}
	}


	public void UpdateOnlineTime(float time)
	{
		redList [(int)ADType.ADT_OnlineReward] = 0;
		foreach(TimerReawData td in TimerReawData.GetData().Values)
		{
			if((int)time >= td._time && !GamePlayer.Instance.onlineTimeRewards_.Contains( (uint)td._Id))
			{
				redList[(int)ADType.ADT_OnlineReward] =1;

				if (MoreActivityRedEvent != null)
				{
					MoreActivityRedEvent (1);
				}

				return;
			}
		}

		if (MoreActivityRedEvent != null)
		{
			MoreActivityRedEvent (1);
		}
	}


	public COM_IntegralData GetIntegralData()
	{
		return IntegralData;
	}


	public void UpdateLevelUpRad()
	{
		List<MoreLevelData> levelList = MoreLevelData.moreLevelList;
		for(int i =0;i<levelList.Count;i++)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= levelList[i].level_)
			{
				if( !GamePlayer.Instance.levelgift.Contains((uint)levelList[i].level_))
				{
					SetTypeRad((int)ADType.ADT_Level,1);
					return;
				}
			}
		}

		SetTypeRad ((int)ADType.ADT_Level, 0);
	}

	public void SetTypeRad(int type,int isShow)
	{
		redList [type] = isShow;

		if (MoreActivityRedEvent != null)
		{
			MoreActivityRedEvent (1);
		}

	}
}
