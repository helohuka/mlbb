using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class PlayerStateUI : MonoBehaviour {

	public GameObject tipsObj;
	public UILabel decLable;
	public UIButton chucunBtn;
	public UIButton chucunCloseBtn;
	public UILabel chucunExpLable;
	public UILabel RaceLabel;
	public UILabel GradeLabel;
	public UILabel PrestigeLabel;
	public UILabel RecordLabel;
	public UILabel FamilyLabel;
	public UILabel OccupationLable;
	public UILabel nameLabel;

	public UILabel zhandouliLable;

	public UISlider HpSlider;
	public UISlider MoliSlider;
	public UISlider expSlider;

	public UISlider LandSlider;
	public UISlider waterSlider;
	public UISlider fireSlider;
	public UISlider windSlider;


	public UILabel hpLabel;
	public UILabel mpLabel;
	public UILabel expLabel;

	public UIButton genggaiBtn;
	public GameObject PromptObj;

	public UILabel qianLabel;
	public UILabel houLabel;

	public UISprite raceIcon;


	int front;

	public UIButton frontBtn;
	public UIButton BackRowBtn;
	public UISprite frontSp;
	public UISprite BackSp;


	private string feng = "cw_feng02";
	private string shui = "cw_shui02";
	private string huo = "cw_huo02";
	private string di = "cw_di02";
	string cur = "";
	string max = "";
	private long curExp;
	private long maxExp;

	public delegate void SetPlayerTitle();
	public static SetPlayerTitle SetPlayerTitleOk;
	GamePlayer playerInst;
	void Start () {
	    playerInst = GamePlayer.Instance;
		//SetPlayerTitleOk = SetTitle; 
		front = playerInst.GetIprop(PropertyType.PT_Front);

		nameLabel.text = playerInst.InstName;
		SetPlayerInfo ();
		//SetPlayerInfo ("无", /*PlayerData.GetData(playerInst.Properties [(int)PropertyType.PT_TableId]).Race_*/"", playerInst.Properties [(int)PropertyType.PT_Level], playerInst.Properties [(int)PropertyType.PT_Reputation], 0, "无", playerInst.Properties [(int)PropertyType.], playerInst.Properties [(int)PropertyType.PT_HpCurr], playerInst.Properties [(int)PropertyType.PT_MpCurr], playerInst.Properties [(int)PropertyType.PT_Land], playerInst.Properties [(int)PropertyType.PT_Water], playerInst.Properties [(int)PropertyType.PT_Fire], playerInst.Properties [(int)PropertyType.PT_Wind]);
		//raceIcon.spriteName = PlayerData.GetData (playerInst.GetIprop(PropertyType.PT_TableId)).RaceIcon_;
		InitPlayerFront ();

        UpdateInfo();
        //GamePlayer.Instance.OnIPropUpdate += UpdateInfo;
		//SetTitle ();
		GamePlayer.Instance.OnUpdateConvertExp = UpdateConvertExp;
		UIManager.SetButtonEventHandler (frontBtn.gameObject, EnumButtonEvent.OnClick, OnClickfront, 0, 0);
		UIManager.SetButtonEventHandler (BackRowBtn.gameObject, EnumButtonEvent.OnClick, OnClickBackRow, 0, 0);
		UIManager.SetButtonEventHandler (genggaiBtn.gameObject, EnumButtonEvent.OnClick, OnClickgenggai, 0, 0);
		UIManager.SetButtonEventHandler (chucunBtn.gameObject, EnumButtonEvent.OnClick, OnClickchucunBtn, 0, 0);
		UIManager.SetButtonEventHandler (chucunCloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickchucunCloseBtn, 0, 0);


        if (GamePlayer.Instance.titleHide_)
        {
            zhandouliLable.text = "";
        }
        else
        {
            TitleData tData = TitleData.GetTitleData(GamePlayer.Instance.GetIprop(PropertyType.PT_Title));
            if (tData == null)
                zhandouliLable.text = "";
            else
                zhandouliLable.text = tData.desc_;
        }
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			frontBtn.gameObject.SetActive(false);
			BackRowBtn.gameObject.SetActive(false);
		}
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerUIStatusSwitch);
		GameManager.Instance.UpdatePlayermake += UpdatePlayerTitle;
	}
	int oldLevel = 0;
	long oldExp = 0;
	float pcurExp =0; 
	void UpdatePlayerTitle()
	{
        if (GamePlayer.Instance.titleHide_)
        {
            zhandouliLable.text = "";
        }
        else
        {
            TitleData tData = TitleData.GetTitleData(GamePlayer.Instance.GetIprop(PropertyType.PT_Title));
            if (tData == null)
                zhandouliLable.text = "";
            else
                zhandouliLable.text = tData.desc_;
        }
	}
	void UpdateConvertExp(int exp)
	{
        chucunExpLable.text = GamePlayer.Instance.GetIprop(PropertyType.PT_ConvertExp).ToString();
	}
    public void UpdateInfo()
    {
        hpLabel.text = ((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_HpCurr]) + "/" + ((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_HpMax]);
		mpLabel.text = ((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_MpCurr]) + "/" + ((int)GamePlayer.Instance.Properties[(int)PropertyType.PT_MpMax]);
		curExp = (long)playerInst.Properties[(int)PropertyType.PT_Exp];
		pcurExp = playerInst.Properties[(int)PropertyType.PT_Exp];
		int l = playerInst.GetIprop (PropertyType.PT_Level);
		long m = ExpData.GetPlayerMaxExp(l);
        maxExp = ExpData.GetPlayerMaxExp(playerInst.GetIprop(PropertyType.PT_Level));
        chucunExpLable.text = GamePlayer.Instance.GetIprop(PropertyType.PT_ConvertExp).ToString();
		if(playerInst.GetIprop(PropertyType.PT_Level)>1)
		{
			oldLevel = (playerInst.GetIprop(PropertyType.PT_Level)-1);
		}else
		{
			oldExp = 0;
		}
		oldExp =  ExpData.GetPlayerMaxExp(oldLevel);
		long s = curExp - oldExp;
		if(s<0)
		{
			s=0;
		}
		expSlider.value = (s* 1f) / ((maxExp-oldExp)* 1f) ;
		expLabel.text = s + "/" + ((long)maxExp-(long)oldExp);

    }

	void OnClickchucunCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		tipsObj.SetActive (false);
	}
	void OnClickchucunBtn(ButtonScript obj, object args, int param1, int param2)
	{
		tipsObj.SetActive (true);
		decLable.text = LanguageManager.instance.GetValue ("chucunjingyan");
	}
	void OnClickgenggai(ButtonScript obj, object args, int param1, int param2)
	{
		PromptObj.SetActive (true);
		
	}
	void InitPlayerFront()
	{
		if (front == 0) {
			frontBtn.gameObject.SetActive(false);
			BackRowBtn.gameObject.SetActive(true);
			frontSp.spriteName = "webzudi";
			BackSp.spriteName = "webzudilv";
			
		} else 
		{
			frontBtn.gameObject.SetActive(true);
			BackRowBtn.gameObject.SetActive(false);
			frontSp.spriteName = "webzudilv";
			BackSp.spriteName = "webzudi";

		}
		
	}
	void SetPlayerInfo()
	{
		//TitleLabel.text = "";
		//RaceLabel.text = PlayerData.GetData(playerInst.Properties [(int)PropertyType.PT_TableId]).Race_;
		GradeLabel.text = /*Grade.ToString()*/ playerInst.Properties [(int)PropertyType.PT_Level].ToString();
		//PrestigeLabel.text = /*Prestige.ToString()*/ playerInst.Properties [(int)PropertyType.PT_Reputation].ToString();
		RecordLabel.text = playerInst.InstId.ToString();
		string guildName = "";
		if (GuildSystem.Mguild != null)
			guildName = GuildSystem.Mguild.guildName_;
		else
			guildName = LanguageManager.instance.GetValue("wu");
		FamilyLabel.text = guildName;
		OccupationLable.text = /*Occupation*/Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
		 
//		HpSlider.value = playerInst.Properties [(int)PropertyType.PT_HpCurr]/playerInst.Properties [(int)PropertyType.PT_HpMax];
//		MoliSlider.value = playerInst.Properties [(int)PropertyType.PT_MpCurr]/playerInst.Properties [(int)PropertyType.PT_MpMax];

		RaceLabel.text = LanguageManager.instance.GetValue(PlayerData.GetData (playerInst.GetIprop(PropertyType.PT_TableId)).Race_.ToString());
		curExp = (long)playerInst.Properties[(int)PropertyType.PT_Exp];
		pcurExp = playerInst.Properties[(int)PropertyType.PT_Exp];
		maxExp = ExpData.GetPlayerMaxExp(playerInst.GetIprop(PropertyType.PT_Level));
		expSlider.value = (pcurExp / maxExp) * 1f;
		LandSlider.value = /*LandValue/10f*/playerInst.Properties [(int)PropertyType.PT_Land]/10f;
		waterSlider.value = /*waterValue/10f*/playerInst.Properties [(int)PropertyType.PT_Water] / 10f;
		fireSlider.value = /*fireValue/10f*/playerInst.Properties [(int)PropertyType.PT_Fire] / 10f;
		windSlider.value=/*windValue/10f*/playerInst.Properties [(int)PropertyType.PT_Wind] / 10f;
	}

	
	void OnClickfront(ButtonScript obj, object args, int param1, int param2)
	{
		frontSp.spriteName = "webzudi";
		BackSp.spriteName = "webzudilv";
		frontBtn.gameObject.SetActive(false);
		BackRowBtn.gameObject.SetActive(true);
		NetConnection.Instance.setPlayerFront (false);
	
		
	}
	void OnClickBackRow(ButtonScript obj, object args, int param1, int param2)
	{
		frontSp.spriteName = "webzudilv";
		BackSp.spriteName = "webzudi";
		frontBtn.gameObject.SetActive(true);
		BackRowBtn.gameObject.SetActive(false);
		NetConnection.Instance.setPlayerFront (true);

		
	}
	void SetTitle()
	{
//		PlayerTitleData ptd = PlayerTitleData.GetData (playerInst.GetIprop (PropertyType.PT_Title));
//		if(ptd != null)
//			TitleLabel.text = ptd.desc_;
	}


    void OnDestroy()
    {
		GamePlayer.Instance.OnUpdateConvertExp = null;
		UIManager.RemoveButtonEventHandler (frontBtn.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (BackRowBtn.gameObject, EnumButtonEvent.OnClick);
		GameManager.Instance.UpdatePlayermake -= UpdatePlayerTitle;
		//UIManager.RemoveButtonEventHandler (genggaiBtn.gameObject, EnumButtonEvent.OnClick);
        //GamePlayer.Instance.OnIPropUpdate -= UpdateInfo;
    }
}
