using UnityEngine;
using System.Collections;

public class HuoDongCell : MonoBehaviour {


	public UILabel _CountLable;
	public UILabel _ActiveLable;


	public UILabel nameLabel;
	public UILabel numLabel;
	public UILabel timeLabel;
    public UISprite completed;
	public GameObject DetailoBJ;
	//public UIButton jiaruBtn;
    public UISprite backGround_;
    public UISprite icon_;
	public UILabel huoyueduLabel;
    public DaliyActivityData daData_;
    bool displayLevel_;
	public UISprite huoyuedustate;
	public UIButton canjBtn;
	public UILabel timelabel;
    ActivityType activityType_;
	void Start () {
		_CountLable.text = LanguageManager.instance.GetValue("huodong_count");
		_ActiveLable.text = LanguageManager.instance.GetValue("huodong_active");
		InitbtnState ();
	}

    public void SetData(DaliyActivityData data, bool displayLv = false)
    {
		huoyuedustate.gameObject.SetActive(false);
        daData_ = data;
        displayLevel_ = displayLv;
        nameLabel.text = data.activityName_;
		huoyueduLabel.text =daData_.Active_.ToString ();
        if (data.maxCount_ != 0)
            numLabel.text = string.Format("{0}/{1}", ActivitySystem.Instance.GetCount(data.activityKind_), data.maxCount_);
        else
            numLabel.text = LanguageManager.instance.GetValue("notLimit");

        //backGround_.spriteName = displayLv ? "landiban" : "lvdiban";
        //GetComponent<UIButton>().normalSprite = displayLv ? "landiban" : "lvdiban";
        UIEventListener listner = UIEventListener.Get(gameObject);
        activityType_ = data.activityKind_;
        listner.onClick = OnShowDetail;
        listner.parameter = data.id_;
        completed.gameObject.SetActive(false);
        UIManager.Instance.AddItemCellUI(icon_, (uint)data.itemid4Icon_);
    }

    string GetNearestTime(DaliyActivityData data)
    {
        if (string.IsNullOrEmpty(data.startTime_))
            return "";

        string[] openTime = data.startTime_.Split(';');
        for(int i=0; i < openTime.Length; ++i)
        {
            string[] time = openTime[i].Split(':');
            if (System.DateTime.Now.Hour < int.Parse(time[0]))
            {
                if (time.Length > 1)
                {
                    if (System.DateTime.Now.Minute < int.Parse(time[1]))
                        return openTime[i];
                    else
                        return openTime[i];
                }
                else
                    return openTime[i];
            }
        }
        if(openTime.Length >= 1)
            return openTime[0];
        return "";
    }

    public void OnShowDetail(GameObject go)
    {
		DetailoBJ.SetActive (true);
        int id = (int)UIEventListener.Get(go).parameter;
        GameObject panel = GameObject.FindObjectOfType<HongDongPanel>().gameObject;
        HuoDongDetail[] arr = panel.GetComponentsInChildren<HuoDongDetail>(true);
        if (arr != null && arr.Length >= 1)
        {
            bool isOpen = ActivitySystem.Instance.GetInfoState(daData_.id_) == ActivitySystem.ActivityInfo.ActivityState.AS_Open;
            arr[0].SetData(id);
			//bool nearestTime = daData_.activityTime_;//string.IsNullOrEmpty(GetNearestTime(daData_));
			string timeOpen = displayLevel_ ? daData_.joinLv_.ToString() + LanguageManager.instance.GetValue("levelOpen") : daData_.activityTime_;
			// GetNearestTime(daData_) + (nearestTime ? LanguageManager.instance.GetValue("openDaylight") : LanguageManager.instance.GetValue("opening"));

            if (isOpen)
            {
                if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < daData_.joinLv_)
                    isOpen = false;
            }

            if (daData_.activityKind_ == ActivityType.ACT_Pet)
            {
				arr[0].SetJiaruBtnHandler(OnShowPet, daData_.id_, timeOpen, isOpen);
            }
			else if(daData_.activityKind_ == ActivityType.ACT_JJC)
			{
				arr[0].SetJiaruBtnHandler(OnJJC, daData_.joinInfo_, timeOpen, isOpen);
			}
			else if(daData_.activityKind_ == ActivityType.ACT_Challenge)
			{
				arr[0].SetJiaruBtnHandler(OnBRDC, daData_.joinInfo_, timeOpen, isOpen);
			}
			else if(daData_.activityKind_ == ActivityType.ACT_Exam)
			{
				arr[0].SetJiaruBtnHandler(OnExam, daData_.joinInfo_, timeOpen, isOpen);
			}
            else
            {
				arr[0].SetJiaruBtnHandler(OnJoin, daData_.joinInfo_, timeOpen, isOpen); 
            }
        }
    }
	public void InitbtnState()
	{
		bool isOpen = ActivitySystem.Instance.GetInfoState(daData_.id_) == ActivitySystem.ActivityInfo.ActivityState.AS_Open;
		
		//bool nearestTime = daData_.activityTime_;//string.IsNullOrEmpty(GetNearestTime(daData_));
		string timeOpen = displayLevel_ ? daData_.joinLv_.ToString () + LanguageManager.instance.GetValue ("levelOpen") : daData_.activityTime_;//
			//GetNearestTime(daData_) + (nearestTime ? LanguageManager.instance.GetValue("openDaylight") : LanguageManager.instance.GetValue("opening"));
		
		if (isOpen)
		{
			if (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < daData_.joinLv_)
				isOpen = false;
			if(actcount>=daData_.maxCount_&&daData_.maxCount_!= 0)
			{
				isOpen = false;
				timeOpen = "";
			}
		}
		
		if (daData_.activityKind_ == ActivityType.ACT_Pet)
		{
			SetJBtnHandler(OnShowPet, daData_.id_, timeOpen, isOpen);
		}
		else if(daData_.activityKind_ == ActivityType.ACT_JJC)
		{
            SetJBtnHandler(OnJJC, null, timeOpen, isOpen);
		}
		else if(daData_.activityKind_ == ActivityType.ACT_Challenge)
		{
            SetJBtnHandler(OnBRDC, null, timeOpen, isOpen);
		}
		else if(daData_.activityKind_ == ActivityType.ACT_Exam)
		{
            SetJBtnHandler(OnExam, null, timeOpen, isOpen);
		}
		else
		{
			SetJBtnHandler(OnJoin, daData_.joinInfo_, timeOpen, isOpen); 
		}

	}
	void OnExam(GameObject go)
	{
		DatiPanel.SwithShowMe ();
	}
	void OnJJC(GameObject go)
	{
		ArenaUI.SwithShowMe ();
	}
	void OnBRDC(GameObject go)
	{
		HundredUI.SwithShowMe();
	}
	public void SetJBtnHandler(UIEventListener.VoidDelegate handler, object parameter, string timeOpen, bool isOpen)
	{
		UIEventListener listener = UIEventListener.Get(canjBtn.gameObject);
		listener.onClick = handler;
		listener.parameter = parameter;		
		canjBtn.gameObject.SetActive(isOpen);
		timelabel.text = timeOpen;
		timelabel.gameObject.SetActive(!isOpen);
	}
    void OnShowPet(GameObject go)
    {
        //PopText.Instance.Show("宠物神殿正在抓紧修建中...", PopText.WarningType.WT_Tip);
        //    return;

        PetTemple.ShowMe();
    }

	public void OnSelJoin(GameObject go)
	{
		if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId) && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("onlyLeaderCanOperate"), PopText.WarningType.WT_Tip);
			return;
		}

		string joinInfoo = (string)UIEventListener.Get(go).parameter;
		
		GameManager.Instance.ParseNavMeshInfo (joinInfoo, ConvertNpcType (activityType_));
		HongDongPanel.HidePanelByName(UIASSETS_ID.UIASSETS_DailyActivityPanel);
	}


    public void OnJoin(GameObject go)
    {
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("querenlikai"),()=>{
					if(daData_.activityKind_ == ActivityType.ACT_Family_0 ||
					   daData_.activityKind_ == ActivityType.ACT_Family_1 ||
					   daData_.activityKind_ == ActivityType.ACT_Family_2 ||
					   daData_.activityKind_ == ActivityType.ACT_Family_3 ||
					   daData_.activityKind_ == ActivityType.ACT_Family_4)
					{
						if(!GuildSystem.IsInGuild())
						{
							PopText.Instance.Show(LanguageManager.instance.GetValue("guildCanJoin"), PopText.WarningType.WT_Warning);
							return;
						}
					}
					
					if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId) && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("onlyLeaderCanOperate"), PopText.WarningType.WT_Tip);
						return;
					}
					string joinInfo = (string)UIEventListener.Get(go).parameter;
					
					GameManager.Instance.ParseNavMeshInfo(joinInfo, ConvertNpcType(activityType_));
					HongDongPanel.HidePanelByName(UIASSETS_ID.UIASSETS_DailyActivityPanel);
				},false,null,null,"","",2000,true);
				return;
		}



        if(daData_.activityKind_ == ActivityType.ACT_Family_0 ||
            daData_.activityKind_ == ActivityType.ACT_Family_1 ||
            daData_.activityKind_ == ActivityType.ACT_Family_2 ||
            daData_.activityKind_ == ActivityType.ACT_Family_3 ||
            daData_.activityKind_ == ActivityType.ACT_Family_4)
        {
            if(!GuildSystem.IsInGuild())
            {
                PopText.Instance.Show(LanguageManager.instance.GetValue("guildCanJoin"), PopText.WarningType.WT_Warning);
                return;
            }
        }

        if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId) && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("onlyLeaderCanOperate"), PopText.WarningType.WT_Tip);
            return;
        }
        string joinInfos = (string)UIEventListener.Get(go).parameter;

		GameManager.Instance.ParseNavMeshInfo(joinInfos, ConvertNpcType(activityType_));
		HongDongPanel.HidePanelByName(UIASSETS_ID.UIASSETS_DailyActivityPanel);
        //if(string.IsNullOrEmpty(joinInfo))
        //{
            //NPCInfo npc = GameManager.Instance.GetGuaiWuGongChengNpc();
            //if (npc == null)
            //{
            //    ClientLog.Instance.LogError("没有找到怪物攻城NPC");
            //}
            //else
            //    joinInfo = string.Format("{0},{1};{2}", npc.sceneId_, npc.areaNum_, npc.id_);
        //    return;
        //}

        //string[] infos = joinInfo.Split(';');
        //string[] sceneInfo = infos[0].Split(',');
        //int sceneId = int.Parse(sceneInfo[0]);
        //int sceneAreaNum = 0;
        //bool tryGetAreaNumByNpc = false;
        //if (sceneInfo.Length > 1)
        //    sceneAreaNum = int.Parse(sceneInfo[1]);
        //else
        //    tryGetAreaNumByNpc = true;
        //int npcId = 0;
        //Vector2 destPos = Vector2.zero;
        //if (infos.Length > 1)
        //{
        //    if (infos[1].Contains(","))
        //    {
        //        string[] pos = infos[1].Split(',');
        //        destPos = new Vector2(float.Parse(pos[0]), float.Parse(pos[1]));
        //    }
        //    else
        //    {
        //        npcId = int.Parse(infos[1]);
        //    }
        //}
        //else
        //{
        //    string xml = SceneSimpleData.GetData(sceneId).sceneXml_;
        //    int bornId = SceneData.GetBornPosEntryID(xml);
        //    destPos = SceneData.GetEntryPos(xml, bornId);
        //}

        //bool APEOn = false;
        //if (npcId != 0)
        //{
        //    if (tryGetAreaNumByNpc)
        //    {
        //        //NPCInfo destNpc = GameManager.Instance.GetNpc(sceneId, npcId);
        //        //if (destNpc == null)
        //        //{
        //        //    // npc has not refresh finished.
        //        //    tryGetAreaNumByNpc = false;
        //        //    return;
        //        //}
        //        //else
        //        //{
        //        //    sceneAreaNum = destNpc.areaNum_;
        //        //}
        //        //tryGetAreaNumByNpc = false;
        //    }
        //    APEOn = Traveller.Instance.Launch().TravelTo(sceneId, sceneAreaNum, npcId);
        //}
        //else if (destPos != Vector2.zero)
        //    APEOn = Traveller.Instance.Launch().TravelTo(sceneId, sceneAreaNum, new Vector3(destPos.x, 0f, destPos.y));
        //if (APEOn)
        //{
        //    GamePlayer.Instance.IsNotAutoPathing = true;
        //    Prebattle.Instance.UpdateAutoPathingAction();
        //    Prebattle.Instance.ExcuteOnePathAction();
        //    Prebattle.Instance.SwitchAFPEffect(true);
        //}

       
    }
	public int actcount;
	public void RefreshFinishProgress(int count)
	{
		actcount = count;

		if (daData_.activityKind_ == ActivityType.ACT_JJC)
		{
			numLabel.text = string.Format("{0}/{1}",count, daData_.maxCount_);
		}
		if (daData_.activityKind_ == ActivityType.ACT_Challenge)
		{
			numLabel.text = string.Format("{0}/{1}",count, daData_.maxCount_);
		}

		if (daData_.activityKind_ == ActivityType.ACT_Tongji)
		{
			numLabel.text = string.Format("{0}/{1}",count, daData_.maxCount_);
		}
		if(count>=daData_.maxCount_&&daData_.maxCount_!= 0)
		{
			canjBtn.gameObject.SetActive(false);
			huoyuedustate.gameObject.SetActive(true);
		}else
		{
			canjBtn.gameObject.SetActive(true);
			huoyuedustate.gameObject.SetActive(false);
		}
	}

    NpcType ConvertNpcType(ActivityType aType)
    { 
        switch(aType)
        {
            case ActivityType.ACT_Mogu:
                return NpcType.NT_Mogu;
            //case ActivityType.ACT_Xiji:
            //    return NpcType.NT_Xiji;
            default:
                return NpcType.NT_None;
        }
    }
	

}
