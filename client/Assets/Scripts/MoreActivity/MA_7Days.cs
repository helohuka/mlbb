using UnityEngine;
using System.Collections.Generic;

public class MA_7Days : UIBase {

    public UIGrid dayGrid;
    public GameObject dayItem;
    public UIGrid questGrid;
    public GameObject questItem;
    public UILabel periodTime;
	public UIButton closeBtn;

    Dictionary<int, List<SevenDaysData>> allData;

    int crtDay;

    List<GameObject> questPool;
	List<GameObject> dayPool;

	// Use this for initialization
	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnCloseBtn, 0, 0);
        string from = "";
        string to = "";
        periodTime.text = string.Format(LanguageManager.instance.GetValue("7dayTime"), from, to);
        questPool = new List<GameObject>();
		dayPool = new List<GameObject> ();
        GameObject dayitem = null;
        int creatDays = GamePlayer.Instance.DaysOld;
        allData = SevenDaysData.dayData;
        UIEventListener listener;
        foreach(int day in allData.Keys)
        {
            dayitem = GameObject.Instantiate(dayItem) as GameObject;
            dayitem.transform.parent = dayGrid.transform;
            dayitem.transform.localScale = Vector3.one;
            dayitem.GetComponent<MA_7DaysDayItem>().SetData(day <= creatDays? day: 0, day);
            listener = UIEventListener.Get(dayitem);
            listener.parameter = day;
            listener.onClick += delegate(GameObject go)
            {
                for (int i = 0; i < dayGrid.transform.childCount; ++i)
                {
                    dayGrid.transform.GetChild(i).GetComponent<MA_7DaysDayItem>().Select(false);
                }
                crtDay = (int)UIEventListener.Get(go).parameter;
                go.GetComponent<MA_7DaysDayItem>().Select(true);
                UpdateQuest();
            };
            dayitem.SetActive(true);

            if(day == 1)
            {
                crtDay = day;
                dayitem.GetComponent<MA_7DaysDayItem>().Select(true);
            }
			dayPool.Add(dayitem);
        }
        dayGrid.Reposition();
        UpdateQuest();
	}

    void UpdateQuest()
    {
        GameObject questitem = null;
        List<SevenDaysData> quests = allData[crtDay];
        for (int i = 0; i < quests.Count; ++i)
        {
            if (i >= questPool.Count)
            {
                questitem = (GameObject)GameObject.Instantiate(questItem) as GameObject;
                questitem.transform.parent = questGrid.transform;
                questitem.transform.localScale = Vector3.one;
                questPool.Add(questitem);
            }
            else
            {
                questitem = questPool[i];
            }
            questitem.GetComponent<MA_7DaysQuestItem>().SetData(quests[i].id);
            questitem.SetActive(true);
        }
        for (int i = quests.Count; i < questPool.Count; ++i)
        {
            questPool[i].SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (MoreActivityData.sevenDaysDirty)
        {
            for (int i = 0; i < questPool.Count; ++i)
            {
                questPool[i].GetComponent<MA_7DaysQuestItem>().UpdateBtnStatus();
            }

			GameObject dayitem;
			for(int i=0; i < dayPool.Count; ++i)
			{
				bool isAllGet = true;
				List<SevenDaysData> quests = allData[i+1];
				for(int j=0;j<quests.Count;j++)
				{
					COM_Sevenday sd = MoreActivityData.Get7DaysData(quests[j].id);
					if(sd != null)
					{
						if(!sd.isreward_)
						{
							isAllGet  =false;
							break;
						}
					}
					else
					{
						isAllGet  =false;
						break;
					}
				}
				dayPool[i].GetComponent<MA_7DaysDayItem>().IsGet(isAllGet);
			}

            MoreActivityData.sevenDaysDirty = false;
        }
	}


	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_7DaysPanel);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_7DaysPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_7DaysPanel);
	}

	public override void Destroyobj ()
	{

	}
	
	#endregion

	private void OnCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Hide();		
	}

}
