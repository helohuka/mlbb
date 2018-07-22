using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
public class FanilyBankUI : UIBase {

	public UILabel levelLable;
	public UILabel curFamilyMoneyLable;
	public UILabel maxFamilyMoneyLable;
	public UILabel weihuLable;
	public UIButton closeBtn;
	public UIButton juanXBtn;
	public GameObject tipsObj;
	public UILabel moneyLable;
	public UIButton leftBtn;
	public UIButton rightBtn;
	public UIButton duihuanBtn;
	public UIButton tipsCloseBtn;
	public UIInput moneyinput;
	public UILabel inputLable;
	public UISprite levelSp;
	private int money;
	private int maxMoney;
	private int temp;
	FamilyData fads;
	int weihu;
	void Start () {
		UIManager.SetButtonEventHandler (juanXBtn.gameObject, EnumButtonEvent.OnClick, buttonClick,0,0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick,onClickClose,0,0);
		UIManager.SetButtonEventHandler (duihuanBtn.gameObject, EnumButtonEvent.OnClick,onClickduihua,0,0);
		UIManager.SetButtonEventHandler (leftBtn.gameObject, EnumButtonEvent.OnClick,onClickLeft,0,0);
		UIManager.SetButtonEventHandler (rightBtn.gameObject, EnumButtonEvent.OnClick,onClickRight,0,0);
		UIManager.SetButtonEventHandler (tipsCloseBtn.gameObject, EnumButtonEvent.OnClick,onClicktipsCloseBtn,0,0);
		maxMoney = GamePlayer.Instance.GetIprop (PropertyType.PT_Money);
		moneyinput.value = "0";
		FamilyData fa = FamilyData.GetData ((int)GuildBuildingType.GBT_Bank,(int)GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Bank)-1].level_);
		fads = fa;
		levelLable.text = GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Bank)-1].level_.ToString ();
		maxFamilyMoneyLable.text = fa.number_.ToString();
		levelSp.spriteName = GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Bank)-1].level_.ToString();
		GlobalValue.Get (Constant.C_FamilyOneDayFundzLose, out weihu);
		weihuLable.text = (GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Main)-1].level_ *weihu).ToString();
		curFamilyMoneyLable.text = GuildSystem.Mguild.fundz_.ToString ();
		GuildSystem.InitGuildDataOk += UpdateGuild;
		GameManager.Instance.UpdatePlayermake += UpdatePlayerMoneyOk;
		UpdateGuild (GuildSystem.Mguild);
		GuildSystem.updateGuildfndzOk += updatefndz;
		GuildSystem.UpdateGuildLevelok += updateguildLevel;
	}
	void UpdatePlayerMoneyOk()
	{
		moneyLable.text = GamePlayer.Instance.GetIprop (PropertyType.PT_Money).ToString();
	}
	void updateguildLevel(GuildBuildingType type, COM_GuildBuilding building)
	{
		if(type == GuildBuildingType.GBT_Bank)
		{
			FamilyData fa = FamilyData.GetData ((int)type,(int)building.level_);
			fads = fa;
			levelLable.text = building.level_.ToString ();
			maxFamilyMoneyLable.text = fa.number_.ToString();
			levelSp.spriteName = building.level_.ToString();
			GlobalValue.Get (Constant.C_FamilyOneDayFundzLose, out weihu);
			curFamilyMoneyLable.text = GuildSystem.Mguild.fundz_.ToString ();
		}

	}
	void OnEnable()
	{

	}
	void OnDisable()
	{

	}
	void updatefndz(int num)
	{
		curFamilyMoneyLable.text = num.ToString ();
		moneyinput.value = "";
		money = 0;
	}
	void UpdateGuild(COM_Guild guild)
	{
		//GuildSystem.Mguild = guild;
		FamilyData fad = FamilyData.GetData ((int)GuildBuildingType.GBT_Bank,(int)guild.buildings_[((int)GuildBuildingType.GBT_Bank)-1].level_);
		fads = fad;
		levelLable.text = guild.buildings_[((int)GuildBuildingType.GBT_Bank)-1].level_.ToString ();
		maxFamilyMoneyLable.text = fad.number_.ToString();
		levelSp.spriteName = guild.buildings_[((int)GuildBuildingType.GBT_Bank)-1].level_.ToString();
		weihuLable.text = (GuildSystem.Mguild.buildings_[((int)GuildBuildingType.GBT_Main)-1].level_ *weihu).ToString();
		curFamilyMoneyLable.text = guild.fundz_.ToString ();
		moneyinput.value = "";
		money = 0;

	}
	// Update is called once per frame
	void Update () {
	
	}
	private void onClickClose(ButtonScript obj, object args, int param1, int param2)
	{ 

		Hide ();
	}
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{ 	
		tipsObj.SetActive (true);
		moneyLable.text = GamePlayer.Instance.GetIprop (PropertyType.PT_Money).ToString ();

	}
	private void onClicktipsCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{ 
		tipsObj.SetActive (false);
	}
	private void onClickduihua(ButtonScript obj, object args, int param1, int param2)
	{ 	
		Regex reg = new Regex("^[0-9]*[1-9][0-9]*$");
		if (!reg.IsMatch (money.ToString()))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bunengfushu"));
			return;
		}
		if(money<GuildSystem.defMoney)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("jiazuzijin"));
			return;
		}
		if(money%GuildSystem.defMoney !=0&&money>=GuildSystem.defMoney)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("duihuanbeishu"));
			return;
		}
//		if(GuildSystem.Mguild.fundz_>=fads.number_)
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("dayusgangxian"));
//			return;
//		}
			NetConnection.Instance.addGuildMoney (money);
	}
	private void onClickLeft(ButtonScript obj, object args, int param1, int param2)
	{ 	
		if (GamePlayer.Instance.GetIprop (PropertyType.PT_Money)<GuildSystem.defMoney)
		{
			money = 0;
			moneyinput.value = money.ToString ();
			return;
		}
			
		if (money <=10000)
		{
			money = 10000;
			moneyinput.value = "10000";
			return;
		}
		money -= GuildSystem.defMoney;
//		temp += GuildSystem.defMoney;
//		moneyLable.text = temp.ToString ();
		moneyinput.value = money.ToString ();
	}
	private void onClickRight(ButtonScript obj, object args, int param1, int param2)
	{ 	
//		if (money >= GamePlayer.Instance.GetIprop (PropertyType.PT_Money))
//		{
//			money = GamePlayer.Instance.GetIprop (PropertyType.PT_Money);
//		}
		if(GamePlayer.Instance.GetIprop (PropertyType.PT_Money)<=GuildSystem.defMoney)
		{
			money = GamePlayer.Instance.GetIprop (PropertyType.PT_Money);
			moneyinput.value = money.ToString();
		}else
		{
			money += GuildSystem.defMoney;
			moneyinput.value = money.ToString();
		}


	}
	public void InputNum()
	{
		if(moneyinput.value == "")
		{
			moneyinput.value ="0";
		}
		//if(moneyinput.value != "")
			money = int.Parse(moneyinput.value);
//		moneyLable.text = (maxMoney - money).ToString ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FanilyBank);
	}
	public static void SwithShowMe()
	{
		
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FanilyBank);
	}
	public static void HideMe()
	{
		
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FanilyBank);
	}
	public override void Destroyobj ()
	{
		GuildSystem.InitGuildDataOk -= UpdateGuild;
		GameManager.Instance.UpdatePlayermake -= UpdatePlayerMoneyOk;
		GuildSystem.updateGuildfndzOk -= updatefndz;
		GuildSystem.UpdateGuildLevelok -= updateguildLevel;
	}
}
