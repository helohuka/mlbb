using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GatherUI : MonoBehaviour
{
    public UIGrid _KindList; //ÀàÐÍÁÐ±í

    public UIGrid _ItemList; //»ñµÃÎïÆ·

    public UILabel oneNeedMoneyLab;
	public UILabel tenNeedMoneyLab;
	public UIButton tenBtn;
	public UIButton oneBtn;

    public GatherKindCell _KindCellTmplate;
    public GatherItemCell _ItemCellTmplate;

    public UIButton _Gather;
    public UIButton _BuyTimes;

	private MineType _mineType;
    int _GatherId;

	int _KindItemSize = 0;
	List<GatherKindCell> _KindItems = new List<GatherKindCell>();

	int _ItemSize = 0;
	List<GatherItemCell> _Items = new List<GatherItemCell>();

	public static int minType;
	public static int minId;
	public UIButton itemBtn0;
	public UIButton itemBtn1;
	public UIButton itemBtn2;
	public UISprite itemBtnImg0;
	public UISprite itemBtnImg1;
	public UISprite itemBtnImg2;

	public UILabel gatherTitleLab;
	public UILabel gatherMnieLab;
	public UILabel gatherTreeLab;
	public UILabel gatherClothLab;
	public UILabel gatherOneBtnLab;
	public UILabel gatherTenBtnLab;
	public UILabel gatherNumInfoLab;
	public UILabel gatherGetItemLab;
	public UILabel gatherNunLab;

	void Start ()
	{
		//this.gameObject.transform.localPosition = new Vector3 (0, 0, -1000);
		//OpenPanelAnimator.PlayOpenAnimation (this.panel, () =>{

			UIManager.SetButtonEventHandler (tenBtn.gameObject, EnumButtonEvent.OnClick, OnTenBtn, 0, 0);
			UIManager.SetButtonEventHandler (oneBtn.gameObject, EnumButtonEvent.OnClick, OnOneBtn, 0, 0);

			GatherSystem.instance._GatherItemHandler += new GatherItemHandler(UpdateItemList);
			GatherSystem.instance.GatheNumEvent += new RequestEventHandler<int>(UpdateNumEvent);
			//UIManager.Instance.LoadMoneyUI (this.gameObject);

			//gatherTitleLab.text = LanguageManager.instance.GetValue("gatherTitleLab");
			gatherMnieLab.text = LanguageManager.instance.GetValue("gatherMnieLab");
			gatherTreeLab.text = LanguageManager.instance.GetValue("gatherTreeLab");
			gatherClothLab.text = LanguageManager.instance.GetValue("gatherClothLab");
			gatherOneBtnLab.text = LanguageManager.instance.GetValue("gatherOneBtnLab");
			gatherTenBtnLab.text = LanguageManager.instance.GetValue("gatherTenBtnLab");
			gatherNumInfoLab.text = LanguageManager.instance.GetValue("gatherNumInfoLab");
			gatherGetItemLab.text = LanguageManager.instance.GetValue("gatherGetItemLab");
			
			if(!GatherSystem.instance.isInitGatherList)
			{
				NetConnection.Instance.initminig();
			}
			
		//});
	}

	
	public void SwithShowMe(int type = 1 , int id = 0)
	{
		if(!GatherSystem.instance.isInitGatherList)
		{
			GatherSystem.instance.minType = type;
			GatherSystem.instance.minId = id;
			GatherSystem.instance.openInitGatherList = true;
			NetConnection.Instance.initminig();
            NetWaitUI.ShowMe();
		}
		else
		{
			minType = type;
			minId = id;
			//UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_GatherPanel);
		}
	}
	
	public  void ShowMe(int type = 1 , int id = 0)
	{
		if(!GatherSystem.instance.isInitGatherList)
		{
			GatherSystem.instance.minType = type;
			GatherSystem.instance.minId = id;
			GatherSystem.instance.openInitGatherList = true;
			NetConnection.Instance.initminig();
            NetWaitUI.ShowMe();
		}

			minType = type;
			minId = id;
			//UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_GatherPanel);



		if(minType == (int)MineType.MT_JinShu)
		{
			itemBtnImg0.gameObject.SetActive(true);
			itemBtnImg1.gameObject.SetActive(false);
			itemBtnImg2.gameObject.SetActive(false);
			SelectCaiKuang ();
		}
		else if(minType == (int)MineType.MT_BuLiao)
		{
			itemBtnImg0.gameObject.SetActive(false);
			itemBtnImg1.gameObject.SetActive(false);
			itemBtnImg2.gameObject.SetActive(true);
			SelectZhibu ();
		}
		else if(minType == (int)MineType.MT_MuCai)
		{
			itemBtnImg0.gameObject.SetActive(false);
			itemBtnImg1.gameObject.SetActive(true);
			itemBtnImg2.gameObject.SetActive(false);
			SelectFamu ();
		}

	}
	

    public void FlashGatherTimes(MineType mt)
    {
        _BuyTimes.Hide();
		_mineType = mt;
        
        _Gather.Show();
            
    }

    /// <summary>
    /// ²É¼¯
    /// </summary>
    public void SelectCaiKuang()
    {
		if(_mineType == MineType.MT_JinShu)
			return;
        //FlashGatherTimes(MineType.MT_JinShu);
        UpdateKindCurrent(GatherData.GetGatherList(MineType.MT_JinShu));
    }

    /// <summary>
    /// ·¥Ä¾
    /// </summary>
    public void SelectFamu()
	{
		if(_mineType == MineType.MT_MuCai)
		return;
        //FlashGatherTimes(MineType.MT_MuCai);
        UpdateKindCurrent(GatherData.GetGatherList(MineType.MT_MuCai));
    }

    /// <summary>
    /// Ö¯²¼
    /// </summary>
    public void SelectZhibu()
    {
		if(_mineType == MineType.MT_BuLiao)
			return;
		//FlashGatherTimes(MineType.MT_BuLiao);
		UpdateKindCurrent(GatherData.GetGatherList(MineType.MT_BuLiao));
    }


    void UpdateKindCurrent(List<GatherData> li)
    {
        _GatherId = 0;

		oneNeedMoneyLab.text = "0";
		tenNeedMoneyLab.text = "0";


        _KindList.transform.DetachChildren();
        foreach (GatherKindCell gkc in _KindItems)
        {
            gkc._SelectHandler = null;
            gkc.active = false;
            //GameObject.Destroy(gkc.gameObject);
        }
        //_KindItems.Clear();

        _KindItemSize = 0;
        foreach(var g in li){
            GatherKindCell gkc = null;
            if (_KindItemSize >= _KindItems.Count)
            {
                gkc = GameObject.Instantiate(_KindCellTmplate) as GatherKindCell;
                
                 _KindItems.Add(gkc);
            }
            else
            {
                gkc = _KindItems[_KindItemSize];
            }
            gkc.active = true;
            gkc.Data = g;
            gkc._SelectHandler = _SelectKind;

            _KindList.AddChild(gkc.transform);
			gkc.transform.localScale = Vector3.one;
            ++_KindItemSize;

			if(minId == g._Show0 || minId == g._Show1)
			{
				gkc.GetComponent<GatherKindCell>().OnClick();
			}
        }

        _KindList.Reposition();
        _KindList.Hide();
        _KindList.Show();
    }

	void UpdateNumEvent(int a)
	{
		PopText.Instance.Show (LanguageManager.instance.GetValue("EN_MallBuyOk"));
		//FlashGatherTimes(_mineType);
		//UpdateKindCurrent(GatherData.GetGatherList(_mineType));
	}

    void UpdateItemList(List<COM_DropItem> items)
    {
        _ItemList.transform.DetachChildren();
        foreach (GatherItemCell gkc in _Items)
        {
            gkc.gameObject.SetActive(false);
            //GameObject.Destroy(gkc.gameObject);
        }
        //_KindItems.Clear();

        _ItemSize = 0;
        foreach (var g in items)
        {
            GatherItemCell gkc = null;
            if (_ItemSize >= _Items.Count)
            {
                gkc = GameObject.Instantiate(_ItemCellTmplate) as GatherItemCell;
                gkc.transform.localScale = Vector3.one;

                _Items.Add(gkc);
            }
            else
            {
                gkc = _Items[_ItemSize];
            }

				gkc.Data = g;
            _ItemList.AddChild(gkc.transform);
			gkc.gameObject.SetActive(true);
			gkc.transform.localScale = Vector3.one;
            ++_ItemSize;
        }

        _ItemList.Reposition();

		GatherData gd = GatherData.GetGather(_GatherId);
		COM_Gather  gatherD = GatherSystem.instance.GetOpenGather(_GatherId);
		int maxNum = 0;
		GlobalValue.Get(Constant.C_GatherNumMax, out maxNum);
		if(gatherD == null)
		{
			gatherNunLab.gameObject.SetActive(true);
			gatherNunLab.text = LanguageManager.instance.GetValue ("gatherCanNum").Replace("{n}",(maxNum-GatherSystem.instance.maxNum).ToString() +"/"+maxNum);
		}
		else
		{
			gatherNunLab.text = LanguageManager.instance.GetValue ("gatherCanNum").Replace("{n}",(maxNum -GatherSystem.instance.maxNum).ToString()+"/"+maxNum);
		}
		if(gatherD == null)
		{
			oneNeedMoneyLab.text = (((int)(0 +1- 1))/5*gd._AddMoney+gd._Money).ToString ();
			int tenMoney = 0;
			for(int i =1;i<=10;i++)
			{
				tenMoney += ((int)(0 + i -1))/5*gd._AddMoney+gd._Money;
			}
			
			tenNeedMoneyLab.text = tenMoney.ToString (); 
		}
		else
		{
			oneNeedMoneyLab.text = (((int)gatherD.num_+1  -1)/5*gd._AddMoney+gd._Money).ToString ();
			int tenMoney1 = 0;
			for(int i =1;i<=10;i++)
			{
				tenMoney1 += ((int)gatherD.num_ + i - 1)/5*gd._AddMoney+gd._Money;
			}
			
			tenNeedMoneyLab.text = tenMoney1.ToString (); 
		}


		//FlashGatherTimes (_mineType);
    }

    void _SelectKind(GameObject obj, int id)
    {
        for(int i=0; i<_KindItemSize; ++i){
            if(_KindItems[i].gameObject == obj){
                _KindItems[i]._Selected.Show();
            }
            else{
                _KindItems[i]._Selected.Hide();
            }
        }
        _GatherId = id;
        GatherData gd = GatherData.GetGather(_GatherId);
		COM_Gather  gatherD = GatherSystem.instance.GetOpenGather(_GatherId);
		int maxNum = 0;
		GlobalValue.Get(Constant.C_GatherNumMax, out maxNum);
		if(gatherD == null)
		{
			if(gd ._Level <= GamePlayer.Instance.GetIprop(PropertyType.PT_Level))
			{	
				gatherNunLab.gameObject.SetActive(true);
				gatherNunLab.text = LanguageManager.instance.GetValue ("gatherCanNum").Replace("{n}",(maxNum-GatherSystem.instance.maxNum).ToString()+"/"+maxNum);
			}
			else
			{
				gatherNunLab.gameObject.SetActive(false);
			}
		}
		else
		{
			gatherNunLab.gameObject.SetActive(true);
			gatherNunLab.text = LanguageManager.instance.GetValue ("gatherCanNum").Replace("{n}",(maxNum -GatherSystem.instance.maxNum).ToString()+"/"+maxNum);
		}


		if(gatherD == null)
		{
			oneNeedMoneyLab.text = (((int)(0 +1- 1))/5*gd._AddMoney+gd._Money).ToString ();
			int tenMoney = 0;
			for(int i =1;i<=10;i++)
			{
				tenMoney += (0 + i -1)/5*gd._AddMoney+gd._Money;
			}

			tenNeedMoneyLab.text = tenMoney.ToString (); 
		}
		else
		{
			oneNeedMoneyLab.text = (((int)gatherD.num_ +1 -1)/5*gd._AddMoney+gd._Money).ToString ();
			int tenMoney1 = 0;
			for(int i =1;i<=10;i++)
			{
				tenMoney1 += ((int)gatherD.num_ + i - 1)/5*gd._AddMoney+gd._Money;
			}

			//for (size_t i = 0; i < times; ++i)
			//{
			//	needmoney += (pnewdate->num_+i -1)/5*pG->addmoney_+pG->money_;
			//}


			tenNeedMoneyLab.text = tenMoney1.ToString (); 
		}
        //
    }

    /// <summary>
    /// ²É¼¯°´Å¥°´ÏÂ
    /// </summary>
	public void OnOneBtn(ButtonScript obj, object args, int param1, int param2)
    {
		GatherData gd = GatherData.GetGather(_GatherId);
		COM_Gather  gatherD = GatherSystem.instance.GetOpenGather(_GatherId);
		int maxNum = 0;
		GlobalValue.Get(Constant.C_GatherNumMax, out maxNum);
		if(gatherD != null)
		{
			if(gatherD.num_ >= maxNum)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_GatherTimesLess"));
				return;
			}
		}

		//if(BagSystem.instance.BagIsFull())
		//{
		//	PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
		//	return;
		//}
        if (_GatherId == 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qingxuanzhecaiji"));
            return;
		}
		//GatherData gd = GatherData.GetGather(_GatherId);
		GatherSystem.instance.Gather(_GatherId,1);
      
    }

	private void OnTenBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (_GatherId == 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("qingxuanzhecaiji"));
			return;
		}
		GatherData gd = GatherData.GetGather(_GatherId);
		COM_Gather  gatherD = GatherSystem.instance.GetOpenGather(_GatherId);

		int maxNum = 0;
		GlobalValue.Get(Constant.C_GatherNumMax, out maxNum);
		if(gatherD != null)
		{
			if( maxNum - gatherD.num_  < 10)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("gathertennoNum"));
				return;
			}
			if(gatherD.num_ >= maxNum)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_GatherTimesLess"));
				return;
			}
		}


		int tenMoney = 0;
		if(gatherD == null)
		{

			for(int i =1;i<=10;i++)
			{
				tenMoney += (0 + i -1)/5*gd._AddMoney+gd._Money;
			}
		}
		else
		{
			for(int i =1;i<=10;i++)
			{
				tenMoney += ((int)gatherD.num_ + i - 1)/5*gd._AddMoney+gd._Money;
			}



		}
		if (tenMoney > GamePlayer.Instance.GetIprop (PropertyType.PT_Money))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nomoney"),PopText.WarningType.WT_Tip);
			return;
		}
		MessageBoxUI.ShowMe ( LanguageManager.instance.GetValue("chouxiaohanjq").Replace("{n}", tenMoney.ToString ()), () => {   GatherSystem.instance.Gather(_GatherId,10);});
	}

    /// <summary>
    /// ¹ºÂò²É¼¯´ÎÊý
    /// </summary>
    public void BuyGatherTimes()
    {

        //MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("caijimaicishu"),()=>{

        //    if(_mineType == MineType.MT_BuLiao)
        //    {
        //        NetConnection.Instance.diamondBuy (DiamondConfigClass.DBT_Type_Mine_Zhibu);
        //    }
        //    else if(_mineType == MineType.MT_JinShu)
        //    {
        //        NetConnection.Instance.diamondBuy (DiamondConfigClass.DBT_Type_Mine_Caikuang);
        //    }
        //    else
        //    {
        //        NetConnection.Instance.diamondBuy (DiamondConfigClass.DBT_Type_Mine_Famu);
        //    }
        //});

        ///Õâ¸ö±»²ß»®¸ÉµôÁËÏêÇéÎÊÃÎÁú
    }    

	void OnDestroy()
	{
		GatherSystem.instance._GatherItemHandler -= UpdateItemList;
		GatherSystem.instance.GatheNumEvent -= UpdateNumEvent;
	}
	public void OnClose()
	{
		//OpenPanelAnimator.CloseOpenAnimation (this.panel, () => {Hide ();});
	}
}

