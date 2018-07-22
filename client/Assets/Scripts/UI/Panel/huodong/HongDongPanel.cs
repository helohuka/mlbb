using UnityEngine;
using System.Collections.Generic;

public class HongDongPanel : UIBase {

	public UILabel _TitleLable;
	public UILabel _LimitedLable;
	public UILabel _DailyLable;
	public UILabel _UpcomingLable;
	public UILabel _Active20Lable;
	public UILabel _Active40Lable;
	public UILabel _Active60Lable;
	public UILabel _Active80Lable;
	public UILabel _Active100Lable;

    public UIPanel contentPanel;
	public UISprite yuandianSp;
	public UILabel actCountLab;
	public GameObject item;
	public UIGrid grid;
	public UIButton closeBtn;
	public UIButton timebtn;
	public UIButton weikaibtn;
	public UIButton meiribtn;
	public UISlider pBar;
	public UITexture [] icons;
	public GameObject [] rewObj;
	public UISprite []sps;
    List<ActivitySystem.ActivityInfo> dataList_;
    List<GameObject> dataObj_;
	List<UIButton> btns = new List<UIButton>();
	public List<UISprite> spslist = new List<UISprite> ();
	List<ACT_RewardData> adata = new List<ACT_RewardData> ();
    // 1 is limited . 2 is daily
    int crtType_;
    int crtTab_;
	UIEventListener Listener ;
	void Start () {
		InitUIText ();
        item.SetActive(false);
		btns.Add (timebtn);
		btns.Add (weikaibtn);
		btns.Add (meiribtn);
		InitRewardIcon ();
        ActivitySystem.Instance.OnActivityUpdate += UpdateUI;
        dataList_ = ActivitySystem.Instance.GetAll();
        dataObj_ = new List<GameObject>();
        GameObject go;
        for (int i = 0; i < dataList_.Count; ++i)
        {
            go = Instantiate(item) as GameObject;
            go.name = (dataList_.Count - i).ToString();
            go.SetActive(true);
            go.transform.parent = grid.transform;
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            dataObj_.Add(go);
        }
		for(int i=0;i<spslist.Count;i++)
		{
			spslist[i].gameObject.SetActive(false);
		}
        OnClicTime(null, null, 0, 0);

        SetType(GamePlayer.activityType_);
        GamePlayer.activityType_ = 2;
        UpdateUI();
		for(int i =0;i<rewObj.Length;i++)
		{
			UIButton btn = rewObj[i].GetComponent<UIButton>();
			//btn.isEnabled = false;
			UISprite sp = rewObj[i].GetComponentInChildren<UISprite>();
			sp.spriteName = "daojuan";
			UIManager.SetButtonEventHandler(rewObj[i], EnumButtonEvent.OnClick, OnClicrew,i, 0);
		}
		UIManager.SetButtonEventHandler(timebtn.gameObject, EnumButtonEvent.OnClick, OnClicTime,0, 0);
		UIManager.SetButtonEventHandler(weikaibtn.gameObject, EnumButtonEvent.OnClick, OnClicweikai,0, 0);
		UIManager.SetButtonEventHandler(meiribtn.gameObject, EnumButtonEvent.OnClick, OnClicMeiri,0, 0);
        UIManager.SetButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick, (ButtonScript obj, object args, int param1, int param2) =>
        {
            Hide();
        }, 0, 0);
		openButton ();
		ActivitySystem.UpdateactivitiOk += openButton;
		ActivitySystem.ReceiveActivityOK += ReceiveOK;
        TabsSelect(crtTab_);
		PlayerActivityEventOk ();

	}

	void InitUIText()
	{
		_TitleLable.text = LanguageManager.instance.GetValue("huodong_Title");
		_LimitedLable.text = LanguageManager.instance.GetValue("huodong_Limited");
		_DailyLable.text = LanguageManager.instance.GetValue("huodong_Daily");
		_UpcomingLable.text = LanguageManager.instance.GetValue("huodong_Upcoming");
		_Active20Lable.text = LanguageManager.instance.GetValue("huodong_Active20");
		_Active40Lable.text = LanguageManager.instance.GetValue("huodong_Active40");
		_Active60Lable.text = LanguageManager.instance.GetValue("huodong_Active60");
		_Active80Lable.text = LanguageManager.instance.GetValue("huodong_Active80");
		_Active100Lable.text = LanguageManager.instance.GetValue("huodong_Active100");
	}

	List<ItemCellUI> cells = new List<ItemCellUI>();
	void InitRewardIcon()
	{
		Dictionary<int, ACT_RewardData> actdata = ACT_RewardData.GetData ();

		foreach(KeyValuePair<int, ACT_RewardData> par in actdata)
		{
			adata.Add(par.Value);
		}
		for(int i=0;i<adata.Count;i++)
		{
			//if(adata[i]!= null)
			ItemCellUI ic = UIManager.Instance.AddItemCellUI(sps[i],(uint)adata[i]._ItemID);
			ic.cellPane.spriteName="";
			ic.showTips = true;
			cells.Add(ic);
			//HeadIconLoader.Instance.LoadIcon (ItemData.GetData(adata[i]._ItemID).icon_, icons[i]);
		}
	}
    void SetType(int activityType)
    {
        crtType_ = activityType;
        crtTab_ = crtType_ == 1 ? 0 : (crtType_ == 0 ? 1 : 2);
    }
	void OnClicrew(ButtonScript obj, object args, int param1, int param2)
	{

		UISprite sp = rewObj[param1].GetComponentInChildren<UISprite>();
		if(sp.spriteName.Equals("daojuliang"))
		{
			NetConnection.Instance.requestActivityReward (param1);
		}
	}
	void openButton()
	{
		//

		if(GamePlayer.Instance.ActivityTable.reward_>=20)
		{
			UIButton btn = rewObj[0].GetComponent<UIButton>();
			UISprite sp = rewObj[0].GetComponentInChildren<UISprite>();

			cells[0].cellPane.GetComponent<BoxCollider>().enabled = false;//.gameObject.SetActive(false);
			if(ContainIndex(0))
			{
				sp.spriteName = "daojuliang";
				spslist[0].gameObject.SetActive(false);
			}else
			{
				sp.spriteName = "daojuan";
				spslist[0].gameObject.SetActive(true);
			}

			//btn.isEnabled = ContainIndex(0);
		} 
		if(GamePlayer.Instance.ActivityTable.reward_>=40)
		{
			UIButton btn = rewObj[1].GetComponent<UIButton>();
			UISprite sp = rewObj[1].GetComponentInChildren<UISprite>();

			cells[1].cellPane.GetComponent<BoxCollider>().enabled = false;
			if(ContainIndex(1))
			{
				sp.spriteName = "daojuliang";
				spslist[1].gameObject.SetActive(false);

			}else
			{
				sp.spriteName = "daojuan";
				spslist[1].gameObject.SetActive(true);
			}

		} 
		if(GamePlayer.Instance.ActivityTable.reward_>=60)
		{
			UIButton btn = rewObj[2].GetComponent<UIButton>();
			UISprite sp = rewObj[2].GetComponentInChildren<UISprite>();
			spslist[2].gameObject.SetActive(true);
			cells[2].cellPane.GetComponent<BoxCollider>().enabled = false;
			if(ContainIndex(2))
			{
				sp.spriteName = "daojuliang";
				spslist[2].gameObject.SetActive(false);
			}else
			{
				sp.spriteName = "daojuan";
				spslist[2].gameObject.SetActive(true);
			}

		} 
		if(GamePlayer.Instance.ActivityTable.reward_>=80)
		{
			UIButton btn = rewObj[3].GetComponent<UIButton>();
			UISprite sp = rewObj[3].GetComponentInChildren<UISprite>();
			spslist[3].gameObject.SetActive(true);
			cells[3].cellPane.GetComponent<BoxCollider>().enabled = false;
			if(ContainIndex(3))
			{
				sp.spriteName = "daojuliang";
				spslist[3].gameObject.SetActive(false);
			}else
			{
				sp.spriteName = "daojuan";
				spslist[3].gameObject.SetActive(true);
			}

		} 
		if(GamePlayer.Instance.ActivityTable.reward_>=100)
		{
			UIButton btn = rewObj[4].GetComponent<UIButton>();
			UISprite sp = rewObj[4].GetComponentInChildren<UISprite>();

			cells[4].cellPane.GetComponent<BoxCollider>().enabled = false;
			if(ContainIndex(4))
			{
				sp.spriteName = "daojuliang";
				spslist[4].gameObject.SetActive(false);
			}else
			{
				sp.spriteName = "daojuan";
				spslist[4].gameObject.SetActive(true);
			}

		}

	}

	bool ContainIndex(int index)
	{
		for(int i=0; i < ActivitySystem.flags.Count; ++i)
		{
			if(ActivitySystem.flags[i] == index)
				return false;
		}
		return true;
	}
	void ReceiveOK(uint idx)
	{
		UIButton btn = rewObj[idx].GetComponent<UIButton>();
		btn.isEnabled = false;
		UISprite sp = rewObj[idx].GetComponentInChildren<UISprite>();
		sp.spriteName = "daojuan";
		ItemData idd = ItemData.GetData ((int)adata[(int)idx]._ItemID);
		spslist [(int)idx].gameObject.SetActive (true);
		PopText.Instance.Show (LanguageManager.instance.GetValue("lingqujiangli").Replace("{n}",idd.name_));
	}

	void OnClicTime(ButtonScript obj, object args, int param1, int param2)
	{
        crtTab_ = 0;
        crtType_ = 1;
        TabsSelect(crtTab_);
        UpdateUI();
		PlayerActivityEventOk ();
		InitCellInfo ();
	}
	void OnClicweikai(ButtonScript obj, object args, int param1, int param2)
	{
        if(WeikaiNothing())
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("UnOpenIsNothing"), PopText.WarningType.WT_Warning);
            return;
        }
        crtTab_ = 1;
        crtType_ = 0;
        TabsSelect(crtTab_);
        UpdateUI();
		PlayerActivityEventOk ();
		InitCellInfo ();
	}
	void OnClicMeiri(ButtonScript obj, object args, int param1, int param2)
	{
        crtTab_ = 2;
        crtType_ = 2;
        TabsSelect(crtTab_);
        UpdateUI();
		PlayerActivityEventOk ();
		InitCellInfo ();
	}

    bool WeikaiNothing()
    {
        int openCount = 0;
        DaliyActivityData data = null;
        for (int i = 0; i < dataList_.Count; ++i)
        {
            data = DaliyActivityData.GetData(dataList_[i].id_);
            if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= data.joinLv_)
            {
                openCount++;
            }
        }
        return dataList_.Count == openCount;
    }

    int Compare(GameObject go1, GameObject go2)
    {
        if (int.Parse(go1.name) > int.Parse(go2.name))
            return -1;
        else if (int.Parse(go1.name) < int.Parse(go2.name))
            return 1;
        return 0;
    }

    void UpdateUI(int aid = 1)
    {
        dataObj_.Sort(Compare);
        HuoDongCell cell;
        DaliyActivityData data;
        int disableCount = 0;
        for (int i=0; i < dataList_.Count; ++i)
        {
            data = DaliyActivityData.GetData(dataList_[i].id_);
            if (crtType_ == 0)
            {
                if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) >= data.joinLv_)
                {
                    disableCount++;
                    continue;
                }
            }
            else
            {
                if (crtType_ != data.activityType_)
                {
                    disableCount++;
                    continue;
                }

                if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < data.joinLv_)
                {
                    disableCount++;
                    continue;
                }
            }
            cell = dataObj_[i - disableCount].GetComponent<HuoDongCell>();
            cell.SetData(data, crtType_ == 0);
            if (!dataObj_[i - disableCount].activeSelf)
            {
                cell.gameObject.SetActive(true);
            }
        }
        for ( ; disableCount > 0; --disableCount)
        {
            dataObj_[dataObj_.Count - disableCount].SetActive(false);
        }
        contentPanel.clipOffset = Vector2.zero;
        contentPanel.transform.localPosition = Vector3.zero;
        contentPanel.GetComponent<UIScrollView>().ResetPosition();
        grid.Reposition();

       // Transform firstItem = grid.GetChild(0);
      //  bool hasItem = firstItem == null? false: firstItem.gameObject.activeSelf;
//        if (hasItem == false)
//            ClearDetail();
//        else
//            firstItem.GetComponent<HuoDongCell>().OnShowDetail(firstItem.gameObject);
    }

    void ClearDetail()
    {
        HuoDongDetail[] arr = panel.GetComponentsInChildren<HuoDongDetail>(true);
        if (arr != null && arr.Length >= 1)
        {
            arr[0].Clear();
        }
    }

    public static void ShowMe()
    {
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_DailyActivityPanel);
    }
    public static void SwithShowMe()
    {
        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_DailyActivityPanel);
    }
	void PlayerActivityEventOk()
	{
		ClearActivity ();
		int gb = 0;
		COM_ActivityTable table = GamePlayer.Instance.ActivityTable;
		if (table == null)
			return;
		pBar.value =GamePlayer.Instance.ActivityTable.reward_ / 100f;
		if(yuandianSp.transform.localPosition.x>=1158)
		{
			yuandianSp.transform.localPosition = new Vector3 (1158,0,0);
		}else
		{
			yuandianSp.transform.localPosition = new Vector3 (1158*GamePlayer.Instance.ActivityTable.reward_ / 100f,0,0);
		}

		actCountLab.text = GamePlayer.Instance.ActivityTable.reward_.ToString ();
		for(int i =0;i<dataObj_.Count;i++)
		{
			//if(!dataObj_[i].activeSelf)continue;
			HuoDongCell hcell = dataObj_[i].GetComponent<HuoDongCell>();
			if(hcell == null)continue;
			for(int j =0;j<table.activities_.Length;j++)
			{
				if(hcell.daData_ == null)continue;
				if(hcell.daData_.id_ == table.activities_[j].actId_)
				{
					hcell.RefreshFinishProgress(table.activities_[j].counter_);
				}
			}
		}

	}
	void InitCellInfo()
	{
		for(int i =0;i<dataObj_.Count;i++)
		{

			HuoDongCell hcell = dataObj_[i].GetComponent<HuoDongCell>();
			if(hcell == null)continue;
		    if(hcell.daData_ == null)continue;				
			hcell.InitbtnState();
		}
	}
	public void ClearActivity()
	{
		for(int i =0;i<dataObj_.Count;i++)
		{
			HuoDongCell hcell = dataObj_[i].GetComponent<HuoDongCell>();
			if(hcell == null)continue;
			if(hcell.daData_ == null)continue;				
			hcell.RefreshFinishProgress(0);
		}
	}
    public void OnDestroy()
    {
        Destroyobj();
    }

	public override void Destroyobj ()
	{
        for (int i = 0; i < dataObj_.Count; ++i)
        {
			if(dataObj_[i]!= null)
            Destroy(dataObj_[i]);
        }
        ActivitySystem.Instance.OnActivityUpdate -= UpdateUI;
		ActivitySystem.UpdateactivitiOk -= openButton;
		ActivitySystem.ReceiveActivityOK -= ReceiveOK;
        UIManager.RemoveButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick);
	}
	void TabsSelect(int index)
	{
		for (int i = 0; i<btns.Count; i++) 
		{
			if(i==index)
			{
				btns[i].isEnabled = false;
			}
			else
			{
				btns[i].isEnabled = true;
			}
		}
	}
}
