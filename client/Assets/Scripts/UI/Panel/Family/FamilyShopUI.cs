using UnityEngine;
using System.Collections;
using System;
public class FamilyShopUI : UIBase {

	public GameObject item;
	public UIGrid grid;
	public GameObject familyShopObj;
	public UIButton shuaxinBtn;
	public UILabel gongxianLable;
	public UILabel xiaohaoLable;
	public UIButton CloseBtn;
	public UISprite back;
	int xiaohao = 0;
	float refXiaohao=0;
	public static bool isShowMe;
	void Start () {
		item.SetActive (false);
		//GuildSystem.updateGuildShopOk += UpdateGuildShopOK;
		gongxianLable.text =GuildSystem.GetGuildMemberSelf (GamePlayer.Instance.InstId).contribution_.ToString();
		GlobalValue.Get (Constant.C_FamilyShopConsume, out xiaohao);
		xiaohaoLable.text = xiaohao.ToString();
		UIManager.SetButtonEventHandler (shuaxinBtn.gameObject, EnumButtonEvent.OnClick, onClickRef,0,0);
		COM_GuildShopItem [] shopitem = GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).shopItems_;
		refXiaohao = Mathf.Pow (2, GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).shopRefreshTimes_) * xiaohao;
		xiaohaoLable.text = refXiaohao.ToString();
		if(IsRefShopCountDown(GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId)))
		{
			shuaxinBtn.isEnabled = false;
		}
		UpdateGuildShop (shopitem);
		if(isShowMe)
		{
			CloseBtn.gameObject.SetActive(true);
			UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, onClickClose,0,0);
			back.gameObject.SetActive(true);
		}else
		{
			CloseBtn.gameObject.SetActive(false);
			back.gameObject.SetActive(false);
		}
		updateRefGuildShopCount (GuildSystem.GetGuildMemberSelf (GamePlayer.Instance.InstId));
	}
	void OnEnable()
	{
		GuildSystem.UpdateGuildMemberOk += UpdateGuildShopOK;
		GuildSystem.UpdateGuildShopCountOk += updateRefGuildShopCount;
	}
	void OnDisable()
	{
		GuildSystem.UpdateGuildMemberOk -= UpdateGuildShopOK;
		GuildSystem.UpdateGuildShopCountOk -= updateRefGuildShopCount;
	}
	// Update is called once per frame
	void Update () {
	
	}
	bool IsRefShopCountDown(COM_GuildMember Member)
	{
		GuildJob go = (GuildJob)Enum.ToObject (typeof(GuildJob), Member.job_);
		if(go == GuildJob.GJ_Minister && Member.shopRefreshTimes_==3)
		{
			return true;
		}else
			if(go == GuildJob.GJ_Premier && Member.shopRefreshTimes_==6)
		{
			return true;
			
		}else if(go == GuildJob.GJ_SecretaryHead&& Member.shopRefreshTimes_==4)
		{
			return true;
			
		}else if(go == GuildJob.GJ_VicePremier && Member.shopRefreshTimes_==5)
		{
			return true;
		}else if(go == GuildJob.GJ_People&& Member.shopRefreshTimes_==2)
		{
			return true;
		}
		return false;
	}
	public void UpdateGuildShopOK(COM_GuildMember Member)
	{
		refXiaohao = (Mathf.Pow (2, Member.shopRefreshTimes_) * xiaohao);
		xiaohaoLable.text = refXiaohao.ToString();
		gongxianLable.text = GuildSystem.GetGuildMemberSelf (GamePlayer.Instance.InstId).contribution_.ToString();
		foreach(Transform tr in grid.transform )
		{
			Destroy(tr.gameObject);
		}
		UpdateGuildShop (Member.shopItems_);
		if(IsRefShopCountDown(Member))
		{
			shuaxinBtn.isEnabled = false;
		}
	}
	public void updateRefGuildShopCount(COM_GuildMember Member)
	{
		if(IsRefShopCountDown(Member))
		{
			shuaxinBtn.isEnabled = false;
		}
	}
	public void UpdateGuildShop(COM_GuildShopItem [] itemids)
	{

		for(int i =0;i<6;i++)
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			FamilyShopCell fcell = o.GetComponent<FamilyShopCell>();
			o.SetActive(true);
			o.transform.parent = grid.transform;
			o.transform.localScale= new Vector3(1,1,1);	
			if(i<itemids.Length)
			{
				HomeShopData hdata = HomeShopData.GetHomeShopData(itemids[i].shopId_);			
				fcell.HShopItem = itemids[i];
				UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, buttonClick,itemids[i].shopId_,itemids[i].buyLimit_);
			}else
			{
				fcell.HideUI(i);
			}

			grid.repositionNow = true;
		}
	}
	private void onClickRef(ButtonScript obj, object args, int param1, int param2)
	{
		if(GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).contribution_<xiaohao)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("familyGongxian"));
			return ;
		}
		NetConnection.Instance.refreshGuildShop ();
	}
	private void onClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{ 
		HomeShopData hdata = HomeShopData.GetHomeShopData(param1);	
		if(param2 == 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("goumaicishu"));
			return;
		}
		familyShopObj.SetActive (true);
		FamilyShopBuyUI fsb = familyShopObj.GetComponent<FamilyShopBuyUI>();
		fsb.Hdata = hdata;
		
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilShopPanel);
	}
	public static void SwithShowMe(bool isshow = false)
	{
		isShowMe = isshow;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilShopPanel);
	}
	public static void HideMe()
	{
		
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilShopPanel);
	}
	public override void Destroyobj ()
	{

	}
}
