using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MyFamilyInfo : UIBase {

	public UILabel _FnameLbel;
	public UILabel _FamilyLeveLabel;
	public UILabel _FamilyNumLabel;
	public UILabel _FamilymemLabel;
	public UILabel _FamilyExpLabel;
	public UILabel _FamilyGongxianLabel;
	public UILabel _FamilyGonggAOLabel;
	public UILabel _FamilyGENGGAILabel;
	public UILabel _FamilyWeihuLbel;

	public UILabel _MemNumLabel;
	public UILabel _ProcessLabel;
	public UILabel _FamilyOtherILabel;

	public UILabel _PlayerNameLable;
	public UILabel _PlayerLevelLable;
	public UILabel _PlayerProLable;
	public UILabel _PlayerzhiweiLable;
	public UILabel _PlayergongxianLable;
	public UILabel _PlayerTimeLable;

	public UILabel _SPlayerNameLable;
	public UILabel _SPlayerLevelLable;
	public UILabel _SPlayerProLable;
	public UILabel _STimeLable;
	public UILabel _SClLable;



	public UILabel nameLbel;
	public UILabel familyLeveLabel;
	public UILabel familyNumLabel;
	public UILabel familymemLabel;
	public UILabel familyExpLabel;
	public UILabel familyGongxianLabel;
	//public UILabel familyKYLabel;
	public UILabel noticeLabel;
	public UIButton changeBtn;
	public UIButton transferBtn;
	public UIButton disbandBtn;
	public UIButton closeBtn;
	public UIButton departureBtn;
	public UIButton OtherFamilyBtn;
	public UIButton qiandaoBtn;
	public UITexture qiandaoT;

	public GameObject gonggaoObj;
	public GameObject transferObj;
	public GameObject RenmingObj;
	public GameObject tipsObj;

	public GameObject[] memberItems;
	public GameObject[] messageItems;

	public UIButton juanzhuBtn;
	public UIButton tanheBtn;

	public GameObject guildInfoObj;
	public GameObject familyShopObj;
	public GameObject back;

	public UILabel weihuLable;
	public GameObject diBiao;
	public UILabel numLabel;
	public UIButton leftBtn;
	public UIButton rightBtn;
	private string []names = {"Level1fuliName","Level2fuliName","Level3fuliName","Level4fuliName","Level5fuliName"};
	private string []neirong = {"Level1fuliNeirong","Level2fuliNeirong","Level3fuliNeirong","Level4fuliNeirong","Level5fuliNeirong"};
	private int curPage;
	private int maxPage;

	private int curResPage;
	private int maxResPage;

	private int bIndex = 0;

	public List<UIButton>Guildbtns = new List<UIButton> ();

	int weihu = 0;
	public List<UIButton>btns = new List<UIButton> ();
	public List<GameObject>tabels = new List<GameObject> ();

	private COM_Guild _guild;
	public COM_Guild Guild
	{
		set
		{
			if(value != null)
			{
				_guild = value;
				nameLbel.text = _guild.guildName_;
				familyLeveLabel.text = _guild.buildings_[((int)GuildBuildingType.GBT_Main)-1].level_.ToString();
				familyNumLabel.text = _guild.guildId_.ToString();
				noticeLabel.text = _guild.notice_;
			    familyExpLabel.text =_guild.fundz_.ToString();
				familymemLabel.text =GuildSystem.GuildMembers.Count.ToString();
			}
		}
		get
		{
			return _guild;
		}
	}
	void Start () {
		InitUIText ();
		//item.SetActive (false);
		UIManager.SetButtonEventHandler(changeBtn.gameObject, EnumButtonEvent.OnClick, OnClickchange, 0, 0);
		UIManager.SetButtonEventHandler(transferBtn.gameObject, EnumButtonEvent.OnClick, OnClicktransfer, 0, 0);
		UIManager.SetButtonEventHandler(disbandBtn.gameObject, EnumButtonEvent.OnClick, OnClickdisband, 0, 0);
		UIManager.SetButtonEventHandler(closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		UIManager.SetButtonEventHandler(departureBtn.gameObject, EnumButtonEvent.OnClick, OnClickdeparture, 0, 0);
		UIManager.SetButtonEventHandler(OtherFamilyBtn.gameObject, EnumButtonEvent.OnClick, OnClickOtherFamily, 0, 0);
		UIManager.SetButtonEventHandler(leftBtn.gameObject, EnumButtonEvent.OnClick,OnClickright , 0, 0);
		UIManager.SetButtonEventHandler(rightBtn.gameObject, EnumButtonEvent.OnClick, OnClickLeft, 0, 0);
		UIManager.SetButtonEventHandler(juanzhuBtn.gameObject, EnumButtonEvent.OnClick, OnClickjuanzhuBtn, 0, 0);
		UIManager.SetButtonEventHandler(tanheBtn.gameObject, EnumButtonEvent.OnClick, OnClicktanheBtn, 0, 0);
		UIManager.SetButtonEventHandler(qiandaoBtn.gameObject, EnumButtonEvent.OnClick, OnClickqiandaoBtn, 0, 0);

		for(int i =0;i<btns.Count;i++)
		{
			UIManager.SetButtonEventHandler(btns[i].gameObject, EnumButtonEvent.OnClick, OnClickbtnTabels, i, 0);
		}
		for(int i =0;i<Guildbtns.Count;i++)
		{
			UIManager.SetButtonEventHandler(Guildbtns[i].gameObject, EnumButtonEvent.OnClick, OnClickGuildbtns, i, 0);
		}
		GuildSystem.InitGuildDataOk += RefreshFamilyInfo;
		GuildSystem.InitmemberDataOk += RefreshFamilyMembers;
		GuildSystem.UpdateGuildInfo += UpdateUI;
		RefreshFamilyMembers (GuildSystem.GuildMembers.ToArray());
		RefreshFamilyInfo (GuildSystem.Mguild);
		familyGongxianLabel.text = GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).contribution_.ToString();

		GlobalValue.Get (Constant.C_FamilyOneDayFundzLose, out weihu);
		weihuLable.text = (GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Main)-1].level_ *weihu).ToString ();
		SelectTabelBtn (0);
		SelectTabelView (0);
		SelectGuildTabelBtn (0);
		UpdateUI ();
		UpdatePlayerMoneyOk ();
		if(bIndex ==0)
		{
			if(CurPage == 0)
			{
				numLabel.text = "1" + "/" +MaxPage;
				CurPage = 1;
			}else
			{
				numLabel.text = CurPage + "/" +MaxPage;
			}
		}else
		{
			if(CurResPage == 0)
			{
				numLabel.text = "1" + "/" +MaxResPage;
				CurResPage = 1;
			}else
			{
				numLabel.text = CurResPage + "/" +MaxResPage;
			}
		}
		GlobalValue.Get (Constant.C_FamilySignDrop, out SignDrop);
		ItemData ida = ItemData.GetData (DropData.GetData (SignDrop).item_1_);
		UIManager.Instance.AddItemCellUI (spobj, (uint)ida.id_).showTips = true;
		//HeadIconLoader.Instance.LoadIcon (ida.icon_, qiandaoT);
		if(GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).signflag_)
		{
			qiandaoBtn.isEnabled = false;
			yiobj.SetActive(true);
		}else
		{
			qiandaoBtn.isEnabled = true;
			yiobj.SetActive(false);
		}
		GameManager.Instance.UpdatePlayermake += UpdatePlayerMoneyOk;
		GuildSystem.UpdateGuildMemberOk += updateGuildMember;
		GuildSystem.UpdateGuildLevelok += updateguildLevel;
	}
	int SignDrop;
	public UISprite spobj;
	public GameObject yiobj;
	void updateguildLevel(GuildBuildingType type, COM_GuildBuilding building)
	{
		if(type == GuildBuildingType.GBT_Main)
		familyLeveLabel.text = building.level_.ToString();
	}
	void updateGuildMember(COM_GuildMember GuildMember)
	{
		if(GamePlayer.Instance.InstId == GuildMember.roleId_&&GuildMember.signflag_)
		{
			yiobj.SetActive(true);
			qiandaoBtn.isEnabled = false;
		}
		UpdataTabBagItems ();
		//RefreshFamilyMembers (GuildSystem.GuildMembers.ToArray());
	}
	void OnEnable()
	{
		if (_guild == null)
			return;
		familyExpLabel.text =_guild.fundz_.ToString();
	}
	void UpdatePlayerMoneyOk()
	{
		if (_guild == null)
						return;
		familyExpLabel.text =_guild.fundz_.ToString();
		if(GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId) != null)
			familyGongxianLabel.text =GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).contribution_.ToString();

		familyLeveLabel.text = _guild.buildings_[((int)GuildBuildingType.GBT_Main)-1].level_.ToString();
	}

	void InitUIText()
	{
		_FnameLbel.text = LanguageManager.instance.GetValue("Guild_Gname");
		_FamilyLeveLabel.text= LanguageManager.instance.GetValue("Guild_FamilyLeve");
		_FamilyNumLabel.text= LanguageManager.instance.GetValue("Gulid_FamilyNum");
		_FamilymemLabel.text= LanguageManager.instance.GetValue("Guild_Familymem");
		_FamilyExpLabel.text= LanguageManager.instance.GetValue("Guild_FamilyExp");
		_FamilyGongxianLabel.text= LanguageManager.instance.GetValue("Guild_FamilyGongxian");
		_FamilyGonggAOLabel.text= LanguageManager.instance.GetValue("Guild_FamilyGonggAO");
		_FamilyGENGGAILabel.text= LanguageManager.instance.GetValue("Guild_FamilyGENGGA");		
		_MemNumLabel.text= LanguageManager.instance.GetValue("Guild_MemNum");
		_ProcessLabel.text= LanguageManager.instance.GetValue("Guild_Process");
		_FamilyOtherILabel.text= LanguageManager.instance.GetValue("Guild_FamilyOther");
		_PlayerNameLable.text= LanguageManager.instance.GetValue("Guild_PlayerName");
		_PlayerLevelLable.text= LanguageManager.instance.GetValue("Guild_PlayerLevel");
		_PlayerProLable.text= LanguageManager.instance.GetValue("Guild_PlayerPro");
		_PlayerzhiweiLable.text= LanguageManager.instance.GetValue("Guild_Playerzhiwei");
		_PlayergongxianLable.text= LanguageManager.instance.GetValue("Guild_Playergongxian");
		_PlayerTimeLable.text= LanguageManager.instance.GetValue("Guild_PlayerTime");
		_FamilyWeihuLbel.text =  LanguageManager.instance.GetValue("Guild_Familyweihu");
		_SPlayerNameLable.text= LanguageManager.instance.GetValue("Guild_PlayerName");
		_SPlayerLevelLable.text= LanguageManager.instance.GetValue("Guild_PlayerLevel");
		_SPlayerProLable.text= LanguageManager.instance.GetValue("Guild_PlayerPro");
		_STimeLable.text= LanguageManager.instance.GetValue("Guild_STime");
		_SClLable.text= LanguageManager.instance.GetValue("Guild_SCl");

	}

	void Update()
	{
		if(GuildSystem.IsGuildMessage())
		{
			if(btns[1].GetComponentInChildren<UISprite>() != null)
			btns[1].GetComponentInChildren<UISprite>().MarkOn(UISprite.MarkAnthor.MA_RightTop,-5,-5);
		}else
		{
			if(btns[1].GetComponentInChildren<UISprite>() != null)
			btns[1].GetComponentInChildren<UISprite>().MarkOff();
		}
	}


	private void OnClickqiandaoBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.guildsign ();
	}
	private void OnClickjuanzhuBtn(ButtonScript obj, object args, int param1, int param2)
	{
		FanilyBankUI.ShowMe ();
	}
	private void OnClicktanheBtn(ButtonScript obj, object args, int param1, int param2)
	{
		int OffineTimeMax = 0;
		int itemid = 0;
		GlobalValue.Get(Constant.C_FamilyLeaderOffineTimeMax, out OffineTimeMax);
		GlobalValue.Get(Constant.C_FamilyLoseLeaderItem, out itemid);
		uint offlineTime =  GuildSystem.GetPremier ().offlineTime_;
		if(GetTime(offlineTime)>OffineTimeMax)
		{
			ItemData idata = ItemData.GetData(itemid);
			if(BagSystem.instance.GetItemCount((uint)itemid)==0)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("shangchenggoumai").Replace("{n}",idata.name_));
			}else
			{
				MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("tanhezuzhang"),()=>{
					NetConnection.Instance.familyLoseLeader();
				});
			}
		}else
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("tanhecuowu"));
		}


	}
	private long GetTime(uint timeStamp)
	{
		if (timeStamp == 0)
			return 0;
		System.DateTime dtStart = System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
		long curTime = Define.GetTimeStamp ();
		long gapSec = curTime - (long)timeStamp;
		return gapSec;
	}
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{
		/// <summary>
		/// 买道具 
		/// </summary>
		/// 
		/// 
		HomeShopData hdata = HomeShopData.GetHomeShopData(param1);

		familyShopObj.SetActive (true);
		FamilyShopBuyUI fsb = familyShopObj.GetComponent<FamilyShopBuyUI>();
		fsb.Hdata = hdata;
		
	}
	bool IsTeamMemberInGuild()
	{
		if (!TeamSystem.IsInTeam ())
			return false;
		for(int i =0;i< TeamSystem.GetTeamMembers().Length;i++)
		{
			if(GuildSystem.IsMyGuildMember((int)TeamSystem.GetTeamMembers()[i].instId_))
			{
				continue;
			}else
			{
				return false;
			}
		}
		return true;
	}
	bool IsleaveTeam()
	{
		if(!TeamSystem.IsInTeam())return false;
		for(int i =0;i<TeamSystem.GetTeamMembers().Length;i++)
		{
			if(TeamSystem.AwayTeam((int)TeamSystem.GetTeamMembers()[i].instId_))
			{
				return true;
			}
		}
		return false;
	}
	bool IsGTeamLeader()
	{
		if(TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))
		{
			return true;
		}
		return false;
	}



	private void OnClickLeft(ButtonScript obj, object args, int param1, int param2)
	{
		if(bIndex == 0)
		{
			if(CurPage == 1)
			{
				return;
			}
			if(CurPage>1)
			{
				CurPage--;
				
			}else
			{
				CurPage=1;
				return;
			}

		}else
		{
			if(CurResPage == 1)
			{
				return;
			}
			if(CurResPage>1)
			{
				CurResPage--;
				
			}else
			{
				CurResPage=1;
				return;
			}
		}



	}
	private void OnClickright(ButtonScript obj, object args, int param1, int param2)
	{
		if(bIndex == 0)
		{
			if (MaxPage == 1)
			{
				numLabel.text = "1" + "/" +MaxPage;
				return;
			}
			
			if(CurPage<MaxPage)
			{
				CurPage++;
				
			}else
			{
				CurPage = MaxPage;
				return;
			}
		}else
		{
			if (MaxResPage == 1)
			{
				numLabel.text = "1" + "/" +MaxResPage;
				return;
			}
			
			if(CurResPage<MaxResPage)
			{
				CurResPage++;
				
			}else
			{
				CurResPage = MaxResPage;
				return;
			}
		}


	}

	int CurResPage
	{
		get
		{
			return curResPage;
		}
		set
		{
			if(curResPage  != value)
			{
				curResPage = value;
				UpdataTabResItems();
				numLabel.text =curResPage + "/" + maxResPage;
			}
		}
	}
	int MaxResPage
	{
		get
		{
			if(GuildSystem.GuildRequestDatas.Count<7)
			{
				maxResPage = 1;
			}else
			{
				if(GuildSystem.GuildRequestDatas.Count/7==0)
				{
					maxResPage= GuildSystem.GuildRequestDatas.Count/7;
				}else
				{
					maxResPage= (GuildSystem.GuildRequestDatas.Count/7)+1;
				}
			}
			
			
			numLabel.text =curResPage + "/" + maxResPage;
			return maxResPage;
		}
		set
		{
			if(maxResPage  != value)
			{
				maxResPage = value;
				
			}
		}
	}



	int CurPage
	{
		get
		{
			return curPage;
		}
		set
		{
			if(curPage  != value)
			{
				curPage = value;
				UpdataTabBagItems();
				numLabel.text =curPage + "/" + maxPage;
			}
		}
	}
	int MaxPage
	{
		get
		{
			if(GuildSystem.GuildMembers.Count<7)
			{
				maxPage = 1;
			}else
			{
				if(GuildSystem.GuildMembers.Count/7==0)
				{
					maxPage= GuildSystem.GuildMembers.Count/7;
				}else
				{
					maxPage= (GuildSystem.GuildMembers.Count/7)+1;
				}
			}


			numLabel.text =curPage + "/" + maxPage;
			return maxPage;
		}
		set
		{
			if(maxPage  != value)
			{
				maxPage = value;

			}
		}
	}
	private void UpdataTabResItems()
	{
		for(int i =0;i<messageItems.Length;i++)
		{
			FamilyMesageCell bagCell = messageItems[i].GetComponent<FamilyMesageCell>();
			bagCell.MemRequestDataber = null;
			messageItems[i].SetActive(false);
		}
		int num = (CurResPage-1) * 7 ;
		int index = 0;
		for (int i= 0; i<7; i++) 
		{
			index = num+i;
			if(index< GuildSystem.GuildRequestDatas.Count)
			{
				messageItems[i].SetActive(true);
				FamilyMesageCell bagCell = messageItems[i].GetComponent<FamilyMesageCell>();
				bagCell.MemRequestDataber = GuildSystem.GuildRequestDatas[num+i];
				UIManager.SetButtonEventHandler(memberItems[i].gameObject, EnumButtonEvent.OnClick, OnClickmember, (int)bagCell.MemRequestDataber.roleId_, 0);
			}
			
			
			
			
		}
	}
	private void UpdataTabBagItems()
	{
		int num = 0;
		for(int i =0;i<memberItems.Length;i++)
		{
			MemberCell bagCell = memberItems[i].GetComponent<MemberCell>();
			bagCell.Member = null;
			memberItems[i].SetActive(false);
		}
		if(CurPage> 0)
			num= (CurPage-1) * 7 ;
		else
			num= (CurPage) * 7 ;
		//int index = 0;
		for (int i= 0; i<7; i++) 
		{
			//index = num+i;
			if((num+i)< GuildSystem.GuildMembers.Count)
			{
				memberItems[i].SetActive(true);
				MemberCell bagCell = memberItems[i].GetComponent<MemberCell>();
				bagCell.Member = GuildSystem.GuildMembers[num+i];
				UIManager.SetButtonEventHandler(memberItems[i].gameObject, EnumButtonEvent.OnClick, OnClickmember, bagCell.Member.roleId_, 0);
			}

				
		

		}
	
	}
	void RefreshFamilyMembers(COM_GuildMember[] members)
	{
		UpdataTabBagItems ();
//		for(int i =0;i<memberItems.Length;i++)
//		{
//			if(memberItems[i]==null)
//			{
//				continue;
//			}
//			MemberCell mcell = memberItems[i].GetComponent<MemberCell>();
//			mcell.Member = null;
//			memberItems[i].SetActive(false);
//		}
//		for(int j =0;j<members.Length;j++)
//		{
//		
//			if(j<memberItems.Length)
//			{
//				if(memberItems[j]==null)
//				{
//					continue;
//				}
//				memberItems[j].SetActive(true);
//				UIManager.SetButtonEventHandler(memberItems[j].gameObject, EnumButtonEvent.OnClick, OnClickmember, members[j].roleId_, 0);
//				MemberCell mcell = memberItems[j].GetComponent<MemberCell>();
//				mcell.Member = members[j];
//				memberItems[j].name = members[j].roleId_.ToString();
//			}
//
//		}

	}
	void RefreshFamilyInfo(COM_Guild guild)
	{
		if(guild == null)return;
		Guild = guild;
		UpdateApplyForMessage (guild.requestList_);
	}
	private void OnClickOtherFamily(ButtonScript obj, object args, int param1, int param2)
	{
		Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
		Prebattle.Instance.StopSelfActorMove();
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
    	if(CopyData.IsCopyScene(GameManager.SceneID))
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaifuben"),()=>{
				NetConnection.Instance.exitCopy();
				NetConnection.Instance.transforScene (1100);
			});
		}
		else if(ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("querenlikai"),()=>{
				NetConnection.Instance.transforScene (1100);
			});
		}
		else
		{
			NetConnection.Instance.transforScene (1100);
		}
	}

	private void OnClickGuildbtns(ButtonScript obj, object args, int param1, int param2)
	{
		if(param1==0)
		{
			if (_guild == null)
				return;
			familyExpLabel.text =_guild.fundz_.ToString();
		}
		LoadFamilyUI (param1);
		SelectGuildTabelBtn (param1);
	}
	private void OnClickbtnTabels(ButtonScript obj, object args, int param1, int param2)
	{
		bIndex = param1;
		if(param1==2||param1==3||param1==4)
		{
			diBiao.SetActive(false);
			OtherFamilyBtn.gameObject.SetActive(false);
		}else
		{
			diBiao.SetActive(true);
			OtherFamilyBtn.gameObject.SetActive(true);
		}
		if(param1==1)
		{
			if(CurResPage == 0)
			{
				numLabel.text = "1" + "/" +MaxResPage;
			}else
			{
				numLabel.text = CurResPage + "/" +MaxResPage;
			}
		}else
		if(param1==0)
		{
			if(CurPage == 0)
			{
				numLabel.text = "1" + "/" +MaxPage;
			}else
			{
				numLabel.text = CurPage + "/" +MaxPage;
			}
		}
		SelectTabelBtn (param1);
		SelectTabelView (param1);
	}
	private void OnClickdeparture(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("tuichujiazu"),()=>
		                     {
			NetConnection.Instance.leaveGuild ();
			
		});

	}
	private void OnClickmember(ButtonScript obj, object args, int param1, int param2)
	{
		if (GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).roleId_ == param1)
						return;
		tipsObj.SetActive (true);
		OperatingTips opt = tipsObj.GetComponent<OperatingTips> ();
		MemberCell mcell = obj.GetComponent<MemberCell>();
		opt.Member = mcell.Member;
		opt.jiantou.transform.position = new Vector3 (opt.jiantou.transform.position.x, obj.gameObject.transform.position.y , 0f);
	}
	private void OnClickchange(ButtonScript obj, object args, int param1, int param2)
	{
		gonggaoObj.SetActive (true);
	}
	private void OnClicktransfer(ButtonScript obj, object args, int param1, int param2)
	{
		transferObj.SetActive (true);
	}
	private void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	private void OnClickdisband(ButtonScript obj, object args, int param1, int param2)
	{

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("jiesanjiazu"),()=>
		{
			NetConnection.Instance.delGuild (Guild.guildId_);

		});
	}

	private void UpdateApplyForMessage(COM_GuildRequestData[] requestData)
	{
		for(int i =0;i<messageItems.Length;i++)
		{
			messageItems[i].SetActive(false);
		}
		for(int j =0;j<requestData.Length;j++)
		{
			if(j<messageItems.Length)
			{
				messageItems[j].SetActive(true);
				UIManager.SetButtonEventHandler(messageItems[j].gameObject, EnumButtonEvent.OnClick, OnClickmember,(int)requestData[j].roleId_, 0);
				FamilyMesageCell fcell = messageItems[j].GetComponent<FamilyMesageCell>();
				fcell.MemRequestDataber = requestData[j];
			}

		}
	}
	void UpdateUI()
	{

		for(int i =0;i<GuildSystem.GuildMembers.Count;i++)
		{
			if(GuildSystem.GuildMembers[i].roleId_ == GamePlayer.Instance.InstId)
			{
				if(GuildSystem.GuildMembers[i].job_== (sbyte)GuildJob.GJ_SecretaryHead)
				{
					departureBtn.gameObject.SetActive(true);
					disbandBtn.gameObject.SetActive(false);
					transferBtn.gameObject.SetActive(false);
					changeBtn.gameObject.SetActive(false);
					tanheBtn.gameObject.SetActive(true);
				}else if(GuildSystem.GuildMembers[i].job_== (sbyte)GuildJob.GJ_VicePremier)
				{
					
					departureBtn.gameObject.SetActive(true);
					disbandBtn.gameObject.SetActive(false);
					transferBtn.gameObject.SetActive(false);
					changeBtn.gameObject.SetActive(true);
					tanheBtn.gameObject.SetActive(true);
				}else if(GuildSystem.GuildMembers[i].job_== (sbyte)GuildJob.GJ_Premier)
				{
					departureBtn.gameObject.SetActive(false);
					disbandBtn.gameObject.SetActive(true);
					transferBtn.gameObject.SetActive(true);
					changeBtn.gameObject.SetActive(true);
					tanheBtn.gameObject.SetActive(false);
				}else
				{
					departureBtn.gameObject.SetActive(true);
					disbandBtn.gameObject.SetActive(false);
					transferBtn.gameObject.SetActive(false);
					changeBtn.gameObject.SetActive(false);
					tanheBtn.gameObject.SetActive(true);
				}

			}

		}
		if(GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId) != null)
			familyGongxianLabel.text =GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).contribution_.ToString();
	}
	private void SelectGuildTabelBtn(int index)
	{
		for(int i=0;i<Guildbtns.Count;i++)
		{
			if(i == index)
			{
				Guildbtns[i].isEnabled = false;
			}else
			{
				Guildbtns[i].isEnabled = true;
			}
		}
	}
	private void SelectTabelBtn(int index)
	{
		for(int i=0;i<btns.Count;i++)
		{
			if(i == index)
			{
				btns[i].isEnabled = false;
			}else
			{
				btns[i].isEnabled = true;
			}
		}
	}
	private void SelectTabelView(int index)
	{
		for(int i =0;i<tabels.Count;i++)
		{

			if(i == index)
			{
				tabels[i].SetActive(true);
			}else
			{
				tabels[i].SetActive(false);
			}
		}
	}
	public GameObject familyInfoObj;
	GameObject familyShopingObj;
	GameObject familyMessageObj;
	GameObject familyZhufuObj;
	GameObject familyZhufuOHD;
	GameObject familyConstructionObj;
	void LoadFamilyUI(int index)
	{
		if(index ==0)
		{
			familyInfoObj.SetActive(true);
			if(familyShopingObj != null)
				familyShopingObj.SetActive(false);
			if(familyMessageObj != null)
				familyMessageObj.SetActive(false);
			if(familyZhufuObj != null)
				familyZhufuObj.gameObject.SetActive(false);
			if(familyConstructionObj != null)
				familyConstructionObj.SetActive(false);
			if(familyZhufuOHD != null)
				familyZhufuOHD.SetActive(false);
		}else
			if(index ==1)
		{
			if(familyShopingObj != null)
				familyShopingObj.SetActive(false);
			if(familyInfoObj != null)
				familyInfoObj.SetActive(false);
			if(familyMessageObj != null)
				familyMessageObj.SetActive(false);
			if(familyZhufuObj != null)
				familyZhufuObj.gameObject.SetActive(false);
			if(familyZhufuOHD != null)
				familyZhufuOHD.SetActive(false);
			if(familyConstructionObj != null)
			{
				familyConstructionObj.SetActive(true);
				
			}else
			{
				LoadUI(UIASSETS_ID.UIASSETS_FamilyConstruction,1);
			}

			
		}else
			if(index ==2)
		{
			if(familyInfoObj != null)
				familyInfoObj.SetActive(false);
			if(familyMessageObj != null)
				familyMessageObj.SetActive(false);
			if(familyShopingObj != null)
				familyShopingObj.SetActive(false);
			if(familyConstructionObj != null)
				familyConstructionObj.SetActive(false);
			if(familyZhufuObj != null)
				familyZhufuObj.gameObject.SetActive(false);
			if(familyZhufuOHD != null)
				familyZhufuOHD.SetActive(false);
			if(familyShopingObj != null)
			{
				familyShopingObj.SetActive(true);

			}else
			{
				LoadUI(UIASSETS_ID.UIASSETS_FamilShopPanel,2);
			}

		}else
			if(index ==3)
		{

			if(familyInfoObj != null)
				familyInfoObj.SetActive(false);
			if(familyMessageObj != null)
				familyMessageObj.SetActive(false);
			if(familyShopingObj != null)
				familyShopingObj.SetActive(false);
			if(familyConstructionObj != null)
				familyConstructionObj.SetActive(false);
			if(familyZhufuOHD != null)
				familyZhufuOHD.SetActive(false);
			if(familyZhufuObj != null)
			{
				familyZhufuObj.gameObject.SetActive(true);
			}
			else
			{
				LoadUI(UIASSETS_ID.UIASSETS_FamilyZhufu,3);
			}
				
				
		}else
			if(index ==4)
		{
			if(familyInfoObj != null)
				familyInfoObj.SetActive(false);
			if(familyShopingObj != null)
				familyShopingObj.SetActive(false);
			if(familyZhufuObj != null)
				familyZhufuObj.SetActive(false);
			if(familyMessageObj != null)
				familyMessageObj.SetActive(false);
			if(familyConstructionObj != null)
				familyConstructionObj.SetActive(false);
			if(familyZhufuOHD != null)
			{
				familyZhufuOHD.SetActive(true);
			}else
			{
				LoadUI(UIASSETS_ID.UIASSETS_FamilyHD,4);
			}
		}else
			if(index ==5)
		{
			if(familyInfoObj != null)
				familyInfoObj.SetActive(false);
			if(familyShopingObj != null)
				familyShopingObj.SetActive(false);
			if(familyZhufuObj != null)
				familyZhufuObj.gameObject.SetActive(false);
			if(familyConstructionObj != null)
				familyConstructionObj.SetActive(false);
			if(familyZhufuOHD != null)
			   familyZhufuOHD.SetActive(false);
			if(familyMessageObj != null)
			{
				familyMessageObj.SetActive(true);
			}else
			{
				LoadUI(UIASSETS_ID.UIASSETS_FamilMesagePanel,5);
			}
		}
	}
	bool hasDestroy = false;
	string subUiResPath;
	List<string> subUIs_;
	private void LoadUI(UIASSETS_ID id,int num)
	{
		subUiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)id, AssetLoader.EAssetType.ASSET_UI);
		if (subUIs_ == null)
		{
			subUIs_ = new List<string>();
			subUIs_.Add(subUiResPath);
		}
		else
		{
			if(!subUIs_.Contains(subUiResPath))
				subUIs_.Add(subUiResPath);
		}
		UIAssetMgr.LoadUI(subUiResPath, (Assets, paramData) =>
		                            {
			if (hasDestroy)
			{
                UIAssetMgr.DeleteAsset(subUiResPath);
				return;
			}
			if( null == Assets || null == Assets.mainAsset )
			{
				return ;
			}			
			GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset)as GameObject;
			if(this == null && !gameObject.activeSelf)
			{
				Destroy(go);
			}
			go.transform.parent = back.transform;    
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one;
			if(num == 1)
			{
				familyConstructionObj = go;
				if(Guildbtns[num].isEnabled)
				{
					familyConstructionObj.SetActive(false);
				}
			}
			else if(num ==2)
			{
				familyShopingObj = go;
				if(Guildbtns[num].isEnabled)
				{
					familyShopingObj.SetActive(false);
				}
			}
			else if(num == 3)
			{
				familyZhufuObj = go;
				if(Guildbtns[num].isEnabled)
				{
					familyZhufuObj.SetActive(false);
				}
			}
			else
				if(num == 4)
			{
				familyZhufuOHD = go;
				if(Guildbtns[num].isEnabled)
				{
					familyZhufuOHD.SetActive(false);
				}
			}
				if(num == 5)
			{
				familyMessageObj = go;
				if(Guildbtns[num].isEnabled)
				{
					familyMessageObj.SetActive(false);
				}
			}


			UIManager.Instance.AdjustUIDepth(go.transform);
			NetWaitUI.HideMe();
			UIManager.Instance.AdjustUIDepth(go.transform);
		}
		, null);
	}
	
	void ShowGuildTab(bool isShow)
	{

	}
	
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilinfoPanel);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilinfoPanel);
	}
	public static void HideMe()
	{

		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilinfoPanel);
	}
	void OnDestroy()
	{
		GuildSystem.InitGuildDataOk -= RefreshFamilyInfo;
		GuildSystem.InitmemberDataOk -= RefreshFamilyMembers;
		GuildSystem.UpdateGuildInfo -= UpdateUI;
		GameManager.Instance.UpdatePlayermake -= UpdatePlayerMoneyOk;
		GuildSystem.UpdateGuildMemberOk -= updateGuildMember;
		GuildSystem.UpdateGuildLevelok -= updateguildLevel;
	}
	public override void Destroyobj ()
	{
        hasDestroy = true;
	}
}
