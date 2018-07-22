using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FamilyPanelUI : UIBase {


	public UILabel _NumberingLable;
	public UILabel _LevelLable;
	public UILabel _NameLable;
	public UILabel _LeaderLable;
	public UILabel _MemberLable;
	public UILabel _OperatingLable;

	public UIButton oneKeyBtn;
	public UIButton LeftBtn;
	public UIButton rightBtn;
	public UIButton CloseBtn;
	public UILabel numLbel;
	public UIButton createBtn;
	public GameObject CreateObj;
	public GameObject lookObj;
	public int curPage;
	public int maxPage;
	public List<GameObject> items = new List<GameObject>();
	void Start () {
		InitUITex ();
		NetConnection.Instance.queryGuildList (0);
		UIManager.SetButtonEventHandler(oneKeyBtn.gameObject, EnumButtonEvent.OnClick, OnClickoneKeyBtn, 0, 0);
		UIManager.SetButtonEventHandler(LeftBtn.gameObject, EnumButtonEvent.OnClick, OnClickLeft, 0, 0);
		UIManager.SetButtonEventHandler(rightBtn.gameObject, EnumButtonEvent.OnClick, OnClickright, 0, 0);
		UIManager.SetButtonEventHandler(createBtn.gameObject, EnumButtonEvent.OnClick, OnClickcreate, 0, 0);
		UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		GuildSystem.QueryGuildListResultOk += RefreshItemDatas;
		GuildSystem.UpdateGuildmenbersok += UpdateGuildmenbers;
		for(int i = 0;i<items.Count;i++)
		{
			items[i].SetActive(false);
		}

		numLbel.text = 1+"/"+1;
		OpenPanelAnimator.PlayOpenAnimation (this.panel);
	}
	void UpdateGuildmenbers(COM_GuildMember guildMember)
	{
		if(guildMember.roleId_ == GamePlayer.Instance.InstId)
		{
			MyFamilyInfo.SwithShowMe();
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongxijiaru"));
			Hide();
		}
	}
	void InitUITex()
	{
		_NumberingLable.text = LanguageManager.instance.GetValue("Guild_Numbering");
		_LevelLable.text = LanguageManager.instance.GetValue("Guild_Level");
		_NameLable.text = LanguageManager.instance.GetValue("Guild_Name");
		_LeaderLable.text = LanguageManager.instance.GetValue("Guild_Leader");
		_MemberLable.text = LanguageManager.instance.GetValue("Guild_Member");
		_OperatingLable.text = LanguageManager.instance.GetValue("Guild_Operating");
	}
	private void OnClickoneKeyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		FamilyCell fcell1 = items[0].GetComponent<FamilyCell>();
		if(fcell1.GuildViewerData == null)
		{
			return;
		}
		for(int i =0;i< items.Count;i++)
		{
			FamilyCell fcell = items[i].GetComponent<FamilyCell>();
			if(fcell.GuildViewerData ==null)
				continue;
//			if(!fcell.requestBtn.isEnabled)
//				continue;
			NetConnection.Instance.requestJoinGuild (fcell.GuildViewerData.guid_);
			if(GuildSystem.battleState==0&&IsExitGuild24())
			{
				fcell.SetrequestBtn();
			}
		}

//		for(int i =0;i< GuildSystem.ViewerDatas.Count;i++)
//		{
//			NetConnection.Instance.requestJoinGuild (GuildSystem.ViewerDatas[i].guid_);
//		}
	}
	bool IsExitGuild24()
	{
		if (GamePlayer.Instance.exitguildtime == 0)
			return true;
		System.TimeSpan ts = System.DateTime.Now - Define.TransUnixTimestamp(GamePlayer.Instance.exitguildtime);
		if(ts.Hours>=24)
		{
			return true;
		}
		return false;
	}
	private void OnClickLeft(ButtonScript obj, object args, int param1, int param2)
	{
		if(curPage<0)
		{
			return;
		}
 		if(curPage>0)
		{
			curPage--;

		}else
		{
			curPage=0;
			return;
		}
		NetConnection.Instance.queryGuildList ((short)curPage);
	}
	private void OnClickright(ButtonScript obj, object args, int param1, int param2)
	{

		if(curPage<maxPage-1)
		{
			curPage++;

		}else
		{
			curPage = maxPage-1;
			return;
		}
		NetConnection.Instance.queryGuildList ((short)curPage);
	}
	private void OnClickcreate(ButtonScript obj, object args, int param1, int param2)
	{
		CreateObj.SetActive (true);
	}
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		OpenPanelAnimator.CloseOpenAnimation (this.panel, () => {
			Hide ();
		});
	}
	void RefreshItemDatas(short page, short pageNum, COM_GuildViewerData[] Datas)
	{
		if(pageNum==0)
		{
			numLbel.text = page + "/" + pageNum;
		}else
		{
			numLbel.text = (page+1) + "/" + pageNum;
		}
		maxPage = pageNum;
		curPage = page;
		for(int i = 0;i<items.Count;i++)
		{
			items[i].SetActive(false);
		}

			for(int j =0;j<Datas.Length;j++)
			{
				if(Datas.Length>items.Count)
					break;
				items[j].SetActive(true);
				FamilyCell fcell = items[j].GetComponent<FamilyCell>();
				fcell.Index = j;
				fcell.Page = page;
				fcell.GuildViewerData = Datas[j];
				if(GuildSystem.isExt)
				{
				  GuildSystem.UpdateRequest (page, j, false);
				}
			}


	}


	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilyPanel);
	}
	public static void SwithShowMe()
	{

		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilyPanel);
	}
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilyPanel);
	}
	void OnDestroy()
	{
		GuildSystem.QueryGuildListResultOk -= RefreshItemDatas;
		GuildSystem.UpdateGuildmenbersok -= UpdateGuildmenbers;
	}
	public override void Destroyobj ()
	{

	}
}
