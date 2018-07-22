using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaUI : UIBase
{
	public UIButton closeBtn; 
	public UIButton rollBtn;
	public UIButton rankBtn;
	public UIButton buyBtn;
	public UILabel myRankLab;
	public UILabel myNameLab;
	public UILabel myLevelLab;
	public UITexture myIcon;
	public UISprite myJob;
	public UISprite myJobBack;
	public UIPanel buyPane; 
	public UILabel CDTimeLab;
	public UILabel ChallengeLab;
	public UISprite recordListBtn;
	//public UIPanel  recordListPane;
	public UISprite recordListPane;
	public List<UISprite> challengePlayers = new List<UISprite>(); 
	public ArenaCheckPlayerUI checkPlayerUI;
	public UILabel recordLab;
	public UISprite rewardIcon1;
	public UISprite rewardIcon2;
	public UISprite rewardIcon;

	public UILabel arenaMyRankLab;
	public UILabel arenaRollBtnLab;
	public UILabel arenaMyRankInfoLab;
	public UILabel arenaFigthInfoLab;
	public UILabel arenaRankBtnLab;
	public UISprite No1Item0;
	public UISprite No1Item1;
	public UISprite No1Item2;
	public UILabel No1ItemLab0;
	public UILabel No1ItemLab1;
	public UILabel No1ItemLab2;
	private int _timeCd;
	private List<string> _icons = new List<string>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (rollBtn.gameObject, EnumButtonEvent.OnClick, OnRoll, 0, 0);
		UIManager.SetButtonEventHandler (rankBtn.gameObject, EnumButtonEvent.OnClick, OnRank, 0, 0);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuy, 0, 0);
		UIManager.SetButtonEventHandler (rewardIcon.gameObject, EnumButtonEvent.OnClick, OnrewardIcon, 0, 0);
		UIManager.SetButtonEventHandler (recordListBtn.gameObject, EnumButtonEvent.OnClick, OnRecordList, 0, 0);

		arenaMyRankLab.text = LanguageManager.instance.GetValue("arenaMyRankLab");
		arenaRollBtnLab.text = LanguageManager.instance.GetValue("arenaRollBtnLab");
		arenaMyRankInfoLab.text = LanguageManager.instance.GetValue("arenaMyRankInfoLab");
		arenaFigthInfoLab.text = LanguageManager.instance.GetValue("arenaFigthInfoLab");
		arenaRankBtnLab.text= LanguageManager.instance.GetValue("arenaRankBtnLab");
		myNameLab.text = GamePlayer.Instance.InstName;
		myLevelLab.text = GamePlayer.Instance.GetIprop (PropertyType.PT_Level).ToString ();

		for(int i = 0;i<challengePlayers.Count;i++)
		{
            if (i == challengePlayers.Count - 1)
            {
                ArenaPlayerCellUI apcUI = challengePlayers[i].gameObject.GetComponent<ArenaPlayerCellUI>();
                GuideManager.Instance.RegistGuideAim(apcUI.challengeBtn.gameObject, GuideAimType.GAT_OfflineJJC4);
            }
			UIManager.SetButtonEventHandler (challengePlayers[i].gameObject, EnumButtonEvent.OnClick, OnCheckPlayer, 0, 0);
		}

		checkPlayerUI.callBack = closeCheckPlayer;

		ArenaSystem.Instance.arenaRivalEvent += new ArenaRivalHandler(OnReqsetRival);
		ArenaSystem.Instance.UpdateArenaEnven += new RequestEventHandler<COM_EndlessStair> (OnUpdateMyInfo);
		ArenaSystem.Instance.checkPlayerEnven += new RequestEventHandler<COM_SimplePlayerInst> (CheckPlayer);
		ArenaSystem.Instance.newBattleMsgEnven += new RequestEventHandler<COM_JJCBattleMsg> (OnNewbattleMsg);
		NetConnection.Instance.requestMyAllbattleMsg ();
		NetConnection.Instance.requestRival ();
		NetConnection.Instance.requestMySelfJJCData();

		ArenaSystem.Instance.openPvE = true;
		OpenPanelAnimator.PlayOpenAnimation(this.panel);

		DropData dropData = DropData.GetData (PvRrewardData.GetRewardData (1));
		if (dropData == null)
			return;
		ItemCellUI itemcell =  UIManager.Instance.AddItemCellUI (No1Item0,(uint)dropData.item_1_);
		itemcell.showTips = true;
		ItemCellUI itemcell1 =  UIManager.Instance.AddItemCellUI (No1Item1,(uint)dropData.item_2);
		itemcell1.showTips = true;
		No1Item2.gameObject.SetActive (false);
		//ItemCellUI itemcell2 =  UIManager.Instance.AddItemCellUI (No1Item2,(uint)dropData.item_3);
		//itemcell2.showTips = true;

		No1ItemLab0.text = dropData.item_num_1_.ToString();
		No1ItemLab1.text = dropData.item_num_2.ToString();
		No1ItemLab2.text = dropData.item_num_3.ToString();

		
		GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_OfflineJJCUI);
	}

	void Update ()
	{
		if(ArenaSystem.Instance.RemainCDTime > 0)
		{
			CDTimeLab.gameObject.SetActive(true);
			CDTimeLab.text = FormatTime((int)ArenaSystem.Instance.RemainCDTime);
		}
		else
		{
			CDTimeLab.gameObject.SetActive(false);
		}
	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS__Arena);
	}

	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS__Arena);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS__Arena);
	}
	
	#endregion


	void OnReqsetRival()
	{
		COM_EndlessStair[] rivals = ArenaSystem.Instance.Rivals;
		if (rivals.Length <= 0)
		{
			return;
		}

		for(int i = 0; i<rivals.Length; i++ )
		{
			challengePlayers[i].GetComponent<ArenaPlayerCellUI>().Player = rivals[i];
		}

	}

	void OnUpdateMyInfo(COM_EndlessStair info)
	{
		myRankLab.text = info.rank_.ToString ();
		ChallengeLab.text = ArenaSystem.Instance.ChallengeNum.ToString ();

		HeadIconLoader.Instance.LoadIcon(EntityAssetsData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)).assetsIocn_,myIcon);
		
		if(!_icons.Contains(EntityAssetsData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)).assetsIocn_))
		{
			_icons.Add(EntityAssetsData.GetData(GamePlayer.Instance.GetIprop(PropertyType.PT_AssetId)).assetsIocn_);
		}

		myJobBack.spriteName = ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession)).ToString();
		/*DropData dropData = DropData.GetData (PvRrewardData.GetRewardData ((int)info.rank_));

		ItemCellUI itemcell1 =  UIManager.Instance.AddItemCellUI (rewardIcon1,(uint)dropData.item_1_);
		itemcell1.ItemCount = dropData.item_num_1_;
		itemcell1.showTips = true;
		ItemCellUI itemcell2 =  UIManager.Instance.AddItemCellUI (rewardIcon2,(uint)dropData.item_2);
		itemcell2.ItemCount = dropData.item_num_2;
		itemcell2.showTips = true;
		*/
		UpdateBattleMsg ();
	}

	void CheckPlayer(COM_SimplePlayerInst inst)
	{


		for(int i = 0; i<challengePlayers.Count; i++ )
		{
			challengePlayers[i].GetComponent<ArenaPlayerCellUI>().Mpos.gameObject.SetActive(false);
		}
	

		checkPlayerUI.ShowPanel (inst);
	}

	void OnNewbattleMsg(COM_JJCBattleMsg msg)
	{
		UpdateBattleMsg ();
	}

	private void UpdateBattleMsg()
	{
		COM_JJCBattleMsg[] msgs  = ArenaSystem.Instance.BattleMsgs;
		for (int i=0; i<msgs.Length&&i<3; i++) 
		{
			recordLab.text += msgs[i].defier_ + " VS "+ msgs[i].bydefier_  + "\n"; 
		}
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		ArenaSystem.Instance.openPvE = false;
		OpenPanelAnimator.CloseOpenAnimation(this.panel, ()=>{
			Hide ();	
		});
	}

	private void OnRank(ButtonScript obj, object args, int param1, int param2)
	{
		//ArenaRankUI.ShowMe();
		ChartsPanelUI.ShowMe ();
	}

	private void OnRoll(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.requestRival ();
	}


	private void OnBuy(ButtonScript obj, object args, int param1, int param2)
	{
		buyPane.gameObject.SetActive (true);
	}

	private void OnrewardIcon(ButtonScript obj, object args, int param1, int param2)
	{
		ArenaRewardUI.ShowMe (1);
	}

	private void OnRecordList(ButtonScript obj, object args, int param1, int param2)
	{
		UIManager.Instance.AdjustUIDepth (recordListPane.gameObject.transform);
		recordListPane.gameObject.SetActive(true);
		recordListPane.GetComponent<ArenaRecordListUI>().UpdateList();
	}

	private void OnCheckPlayer(ButtonScript obj, object args, int param1, int param2)
	{
		ArenaPlayerCellUI cell = obj.GetComponent<ArenaPlayerCellUI> ();
		if(cell.Player == null)
		{
			return;
		}

		NetConnection.Instance.requestCheckMsg (cell.Player.name_);
	}

	public string FormatTime(int time)
	{
		int min = time/60;
		int second = time%60;
		return DoubleTime(min) + ":" + DoubleTime(second);
	}
	
	public static string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}


	protected override void DoHide ()
	{
		ArenaSystem.Instance.arenaRivalEvent -= OnReqsetRival;
		ArenaSystem.Instance.UpdateArenaEnven -= OnUpdateMyInfo;
		ArenaSystem.Instance.checkPlayerEnven -= CheckPlayer;
		ArenaSystem.Instance.newBattleMsgEnven -= OnNewbattleMsg;
		base.DoHide ();
	}


	void closeCheckPlayer()
	{
		for(int i = 0; i<challengePlayers.Count; i++ )
		{
			challengePlayers[i].GetComponent<ArenaPlayerCellUI>().Mpos.gameObject.SetActive(true);
		}

	}

	public override void Destroyobj ()
	{
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS__Arena, AssetLoader.EAssetType.ASSET_UI), true);

		COM_EndlessStair[] rivals = ArenaSystem.Instance.Rivals;
		for(int i = 0; i<rivals.Length; i++ )
		{
			PlayerAsseMgr.DeleteAsset ((ENTITY_ID)rivals[i].assetId_, false);
		}

		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}

		GameObject.Destroy (gameObject);
	}





}

