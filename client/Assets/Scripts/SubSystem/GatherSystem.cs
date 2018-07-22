using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatherSystem 
{

    public GatherItemHandler _GatherItemHandler;
	public RequestEventHandler<COM_Item> CompoundOkEvent;
	public RequestEventHandler<int> GatheNumEvent;
	public RequestEventHandler<COM_Gather> UpdateGatheEvent;
	private static GatherSystem _instance;
	private List<COM_DropItem> _MineItems = new List<COM_DropItem>();
	private List<COM_Gather> OpenGatherItemList = new List<COM_Gather>();

	public bool isInitGatherList;
	public bool openInitGatherList;
	public int minType;
	public int minId;
	public uint maxNum;
	void Start ()
	{

	}
	
	void Update ()
	{

	}

	public static GatherSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new GatherSystem();
			return _instance;
        }
    }

	public List<COM_Gather> OpenGatherList
	{
		get
		{
			return OpenGatherItemList;
		}
	}

	public void InitOpenGatherList(uint num,COM_Gather[] list)
	{
		maxNum = num;
		for(int i=0;i<list.Length;i++)
		{
			OpenGatherItemList.Add(list[i]);
			if (UpdateGatheEvent != null)
				UpdateGatheEvent (list[i]);
		}
		isInitGatherList = true;
		if (openInitGatherList)
		{
			openInitGatherList = false;
            NetWaitUI.HideMe();
			if(minType != 0 && minId != 0)
			{
				//SkillViewUI.ShowMe(1,minType,minId);
			}
			else
			{
				//SkillViewUI.SwithShowMe(1,minType,minId);
			}
		}
	}

	public void AddOpenGather( COM_Gather gather)
	{
		for(int i =0;i<OpenGatherItemList.Count;i++)
		{
			if(OpenGatherItemList[i].gatherId_ == gather.gatherId_)
			{
				OpenGatherItemList[i] = gather;
				GatherData gd =GatherData.GetGather((int)gather.gatherId_);
				if(gd != null && gd._Level >= 40)
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("useopenGatherbook").Replace("{n}",gd._Title));
				}
				return;
			}
		}

		if( !OpenGatherItemList.Contains(gather))
		{
			OpenGatherItemList.Add(gather);
			GatherData gd =GatherData.GetGather((int)gather.gatherId_);
			if(gd != null && gd._Level >= 40)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("useopenGatherbook").Replace("{n}",gd._Title));
			}
		}
		


		if (UpdateGatheEvent != null)
			UpdateGatheEvent (gather);
	}


	public COM_Gather GetOpenGather( int id)
	{
		for(int i =0;i<OpenGatherItemList.Count;i++)
		{
			if(OpenGatherItemList[i].gatherId_ == id)
			{
				return OpenGatherItemList[i];
			}
		}

		return null;
	}


    public void InitMingTimes(ushort[] times)
    {
        //for (int i = 0; i < times.Length; ++i)
        //{
        //    _MingTimes[i] = (int)times[i];
        //}

		if (GatheNumEvent != null)
			GatheNumEvent (1);
    }

    public void MiningOk(COM_DropItem[] items)
    {
		_MineItems.Clear ();
        for (int i = 0; i < items.Length; ++i)
        {
            bool added = false;
            for (int j = 0; j < _MineItems.Count; ++j)
            {
                if (items[i].itemId_ == _MineItems[j].itemId_)
                {
                    _MineItems[j].itemNum_ += items[i].itemNum_;
                    added = true;
                    break;
                }
            }
            if (!added)
            {
                _MineItems.Add(items[i]);
            }
        }
        if (_GatherItemHandler != null)
            _GatherItemHandler(_MineItems);

    }

	public void CompoundOk(COM_Item item)
    {
        if (CompoundOkEvent != null)
        {
			CompoundOkEvent(item);
        }
        return;
    }

    public int GetGatherMoneyByType(MineType mt)
    {
        List<GatherData> tmp = GatherData.GetGatherList(mt);
        return tmp[0]._Money;
    }

    public bool Gather(int id,int num)
    {
        GatherData gd = GatherData.GetGather(id);
        if (null == id)
            return true;//没有找到相对应ID

        //if (_MingTimes[gd._Type] <= 0)
        //    return false; //这个类型采集次数不够

        if (gd._Level > GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
            return true; //等级不够

        NetConnection.Instance.mining(id,num);
        //--_MingTimes[gd._Type];

        return true;
    }

    public void Clear()
    {
        OpenGatherItemList.Clear();
        isInitGatherList = false;
    }
}

