using UnityEngine;
using System.Collections;

public class FeaturesUIPanel : UIBase {

	public UILabel expDesLable;
	public UILabel guaijiLable;
	public UILabel shengyuTime;
	public UIButton ExpBtn;
	public UIButton guajiBtn;
	public UIButton CloseExpBtn;
	public UIButton CloseguajiBtn;
	public UIButton xuyuanBtn;
	public UILabel xuyuandecLable;
	public static bool isAuto = false;
	public UIButton closeBtn;
	void Start () {
		guaijiLable.text = LanguageManager.instance.GetValue ("zidongguaji");
		expDesLable.text = LanguageManager.instance.GetValue ("shuangbeijingyan");
		xuyuandecLable.text = LanguageManager.instance.GetValue ("xuyuandec");

		UIManager.SetButtonEventHandler (ExpBtn.gameObject, EnumButtonEvent.OnClick, OnClickExp, 0, 0);
		UIManager.SetButtonEventHandler (guajiBtn.gameObject, EnumButtonEvent.OnClick, OnClickguajiBtn, 0, 0);
		UIManager.SetButtonEventHandler (CloseExpBtn.gameObject, EnumButtonEvent.OnClick, OnClickCloseExp, 0, 0);
		UIManager.SetButtonEventHandler (CloseguajiBtn.gameObject, EnumButtonEvent.OnClick, OnClickClsoeguajiBtn, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose, 0, 0);
		UIManager.SetButtonEventHandler (xuyuanBtn.gameObject, EnumButtonEvent.OnClick, OnClickxuyuanBtn, 0, 0);
		GamePlayer.Instance.OnAutoMeetEnemy += UpdateAME;
		if(GamePlayer.Instance.isOpenDoubleTime) 
		{
			ExpBtn.gameObject.SetActive(false);
			CloseExpBtn.gameObject.SetActive(true);
		}else
		{
			ExpBtn.gameObject.SetActive(true);
			CloseExpBtn.gameObject.SetActive(false);
		}
		if(Prebattle.Instance.walkState_ != Prebattle.WalkState.WS_AME )
		{
			guajiBtn.gameObject.SetActive(true);
			CloseguajiBtn.gameObject.SetActive(false);
		}else
		{
			guajiBtn.gameObject.SetActive(false);
			CloseguajiBtn.gameObject.SetActive(true);
		}
//		if(isAuto)
//		{
//			guajiBtn.gameObject.SetActive(false);
//			CloseguajiBtn.gameObject.SetActive(true);
//		}else
//		{
//			guajiBtn.gameObject.SetActive(true);
//			CloseguajiBtn.gameObject.SetActive(false);
//		}
	}
	void Update () 
	{
		shengyuTime.text = LanguageManager.instance.GetValue ("shuangbejingyanshengyu").Replace("{n}",FormatTimeHasHour (GamePlayer.Instance.GetIprop(PropertyType.PT_DoubleExp)));
	}
	public  string FormatTimeHasHour(int time)
	{
		int hour = time/3600;
		int min = (time%3600)/60;
		int second = time%60;
		return DoubleTime (hour) + ":" + DoubleTime (min);//+ ":" + DoubleTime(second);
	}
	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}
	void UpdateAME(bool bOk)
	{
		isAuto = bOk;
		if (bOk)
		{
			guajiBtn.gameObject.SetActive(false);
			CloseguajiBtn.gameObject.SetActive(true);
		}
		else
		{
			guajiBtn.gameObject.SetActive(true);
			CloseguajiBtn.gameObject.SetActive(false);
		}
	}
	private void OnClickxuyuanBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)<25)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
		SceneData ssdata = SceneData.GetData (GameManager.SceneID);
		if(ssdata.sceneType_ == SceneType.SCT_GuildBattleScene)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("querenlikai"),()=>{
				NetConnection.Instance.moveToNpc (9572);
			});
			return;
		}

		NetConnection.Instance.moveToNpc (9572);
	}

	private void OnClickExp(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_DoubleExp))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
	    NetConnection.Instance.setOpenDoubleTimeFlag (true);
		ExpBtn.gameObject.SetActive(false);
		CloseExpBtn.gameObject.SetActive(true);
	}

	private void OnClickCloseExp(ButtonScript obj, object args, int param1, int param2)
	{			
	    NetConnection.Instance.setOpenDoubleTimeFlag (false);
		ExpBtn.gameObject.SetActive(true);
		CloseExpBtn.gameObject.SetActive(false);
	}
	private void OnClickguajiBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader() && !TeamSystem.AwayTeam(GamePlayer.Instance.InstId))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("onlyLeader"));
			return;
		}
		NetConnection.Instance.autoBattle();
		Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_Normal);
		guajiBtn.gameObject.SetActive(false);
		CloseguajiBtn.gameObject.SetActive(true);
	}
	private void OnClickClsoeguajiBtn(ButtonScript obj, object args, int param1, int param2)
	{
	   NetConnection.Instance.stopAutoBattle();
		guajiBtn.gameObject.SetActive(true);
		CloseguajiBtn.gameObject.SetActive(false);
		Prebattle.Instance.ChangeWalkEff (Prebattle.WalkState.WS_Normal);
	}
	private void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_GuaJiPanel);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_GuaJiPanel);
	}

	public override void Destroyobj ()
	{
		GamePlayer.Instance.OnAutoMeetEnemy -= UpdateAME;
	}
}
