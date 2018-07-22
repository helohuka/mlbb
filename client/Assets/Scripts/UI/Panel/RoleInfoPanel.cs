using UnityEngine;
using System.Collections;

public class RoleInfoPanel : UIBase {

	public UILabel nameLabel;
	public UILabel titleLabel;
	public UILabel jobLabel;
	public UILabel LevlLabel;
	public UILabel raceLabel;
	public UILabel expLabel;
	public UILabel NextLevlLabel;
	public UILabel hpLabel;
	public UILabel mpLabel;
	public UISlider LandSlider;
	public UISlider waterSlider;
	public UISlider fireSlider;
	public UISlider windSlider;


	public UILabel GJLabel;
	public UILabel FYLabel;
	public UILabel MJLabel;
	public UILabel JSLabel;
	public UILabel HFLabel;
	public UILabel BSLabel;
	public UILabel MZLabel;
	public UILabel FJLabel;
	public UILabel SBLabel;
	public UILabel TLLabel;
	public UILabel LLLabel;
	public UILabel QDLabel;
	public UILabel SDLabel;
	public UILabel MFLabel;



	public UIButton closeBtn;
	public UIButton frontBtn;
	public UIButton BackRowBtn;
	int front=0;

	void Start () {
	
		GamePlayer playerInst = GamePlayer.Instance;
		front = playerInst.GetIprop(PropertyType.PT_Front);
		nameLabel.text = "名字 " + playerInst.InstName;
		titleLabel.text = "称号 ";
		jobLabel.text = "职业 ";
		LevlLabel.text = "等级 "+playerInst.Properties[(int)PropertyType.PT_Level];
		raceLabel.text = "种  族: ";
		expLabel.text = "经验值: "+playerInst.Properties[(int)PropertyType.PT_Exp];
		NextLevlLabel.text = "下一级: ";
		hpLabel.text = "生   命: "+playerInst.Properties[(int)PropertyType.PT_HpMax];
		mpLabel.text = "魔   力: "+playerInst.Properties[(int)PropertyType.PT_MpMax];
		LandSlider.value = playerInst.Properties[(int)PropertyType.PT_Land] / 100f;
		waterSlider.value = playerInst.Properties[(int)PropertyType.PT_Water] / 100f;
		fireSlider.value = playerInst.Properties[(int)PropertyType.PT_Fire] / 100f;
		windSlider.value = playerInst.Properties[(int)PropertyType.PT_Wind] / 100f;


		TLLabel.text = "体力： " + playerInst.GetIprop(PropertyType.PT_Stama);
		LLLabel.text = "力量: " + playerInst.GetIprop(PropertyType.PT_Strength);
		QDLabel.text = "强度: " + playerInst.Properties[(int)PropertyType.PT_Power];
		SDLabel.text = "速度: " + playerInst.Properties[(int)PropertyType.PT_Speed];
		MFLabel.text = "魔法: " + playerInst.Properties[(int)PropertyType.PT_Magic];
		GJLabel.text = "攻击: " + playerInst.Properties[(int)PropertyType.PT_Attack];
		FYLabel.text = "防御: " + playerInst.Properties[(int)PropertyType.PT_Defense];
		MJLabel.text = "敏捷: " + playerInst.Properties[(int)PropertyType.PT_Agile];
		JSLabel.text = "精神: ";
		HFLabel.text = "回复: " + playerInst.Properties[(int)PropertyType.PT_Reply];
		BSLabel.text = "必杀: ";
		MZLabel.text = "命中: ";
		FJLabel.text = "反击: ";
		SBLabel.text = "闪避: ";

		InitPlayerFront ();
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (frontBtn.gameObject, EnumButtonEvent.OnClick, OnClickfront, 0, 0);
		UIManager.SetButtonEventHandler (BackRowBtn.gameObject, EnumButtonEvent.OnClick, OnClickBackRow, 0, 0);
	}

	void InitPlayerFront()
	{
		if (front == 0) {
			frontBtn.defaultColor = Color.white;
			BackRowBtn.defaultColor = Color.gray;

		} else 
		{
			frontBtn.defaultColor = Color.gray;
			BackRowBtn.defaultColor = Color.white;
		}

	}
	
	#region Fixed methods for UIBase derived cass

	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_PlayerInfoPanel);
	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad (UIASSETS_ID.UIASSETS_PlayerInfoPanel);			
	}

	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_PlayerInfoPanel);
	}

	#endregion

	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	void OnClickfront(ButtonScript obj, object args, int param1, int param2)
	{
		frontBtn.defaultColor = Color.gray;
		BackRowBtn.defaultColor = Color.white;
		NetConnection.Instance.setPlayerFront (true);

	}
	void OnClickBackRow(ButtonScript obj, object args, int param1, int param2)
	{
		frontBtn.defaultColor = Color.white;
		BackRowBtn.defaultColor = Color.gray;
		NetConnection.Instance.setPlayerFront (false);

	}

	
	public override void Destroyobj ()
	{
		Destroy (gameObject);
	}


}
