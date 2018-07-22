using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HundredUI : UIBase
{
	public UIButton closeBtn;
	public UIButton changellBtn;
	public UIButton createTeamBtn;
	public UIButton resetBtn;
	public UIButton rightBtn;
	public UIButton leftBtn;
	public UILabel nameLab;
	public UILabel curNumLab;
	public UIButton teamBtn;
	public UILabel  surplusLab;

	public GameObject itemCell;
	public UIGrid grid;  
	public Transform Mpos;

	private List<GameObject> cellList = new List<GameObject> ();
	private List<GameObject> cellPoolList = new List<GameObject> ();
	private uint _curId;

    List<string> iconName_;

	void Start ()
	{
        iconName_ = new List<string>();
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (changellBtn.gameObject, EnumButtonEvent.OnClick, OnChallengeBtn, 0, 0);
		UIManager.SetButtonEventHandler (rightBtn.gameObject, EnumButtonEvent.OnClick, OnRightBtn, 0, 0);
		UIManager.SetButtonEventHandler (leftBtn.gameObject, EnumButtonEvent.OnClick, OnLeftBtn, 0, 0);
		UIManager.SetButtonEventHandler (resetBtn.gameObject, EnumButtonEvent.OnClick, OnResetBtn, 0, 0);
		UIManager.SetButtonEventHandler (teamBtn.gameObject, EnumButtonEvent.OnClick, OnTeamBtn, 0, 0);


		hundredSystem.instance.HundredBattleEnvet += new RequestEventHandler<COM_HundredBattle> (OnHundredBattle);

		curId = hundredSystem.instance.ChallengeNum;
		UpdateChallengeInfo (curId);

	}
	

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_HundredUI);
	}
	
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_HundredUI);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_HundredUI);
	}
	
	#endregion



	private void UpdateChallengeInfo( uint id)
	{

		if( hundredSystem.instance.ChallengeNum <= 1)
		{
			resetBtn.isEnabled = false;
		}
		else
		{
			resetBtn.isEnabled = true;
		}
		ChallengeData cData = ChallengeData.GetData ((int)id);//hundredSystem.instance.ChallengeNum);
		if (cData == null)
			return;
		nameLab.text = LanguageManager.instance.GetValue ("hundrednum").Replace("{n}",id.ToString());
		surplusLab.text = hundredSystem.instance.UseNum.ToString()+"/1";

		if(hundredSystem.instance.ResetNum == 0)
		{
			resetBtn.isEnabled = false;
		}
		//int num = EntityAssetsData.GetData (cData.assetsID_).assetsName_;
		GameManager.Instance.GetActorClone((ENTITY_ID)cData.assetsID_,0, EntityType.ET_Player, AssetLoadCallBack, new ParamData(0),"UI");

		foreach(GameObject c in cellList)
		{
			grid.RemoveChild(c.transform);
			c.transform.parent = null;
			c.gameObject.SetActive(false);
			cellPoolList.Add(c);
		}
		cellList.Clear ();

		for(int i=0;i<cData.reward_.Length;i++)
		{
			ItemData item  = ItemData.GetData(int.Parse(cData.reward_[i]));
			GameObject obj = null;
			if(cellPoolList.Count>0)
			{
				obj = cellPoolList[0];
				cellPoolList.Remove(obj);  
			}
			else  
			{
				obj =  Object.Instantiate(itemCell) as GameObject;
			}

			ItemCellUI cellUI =  UIManager.Instance.AddItemCellUI(obj.GetComponent<BagCellUI>().pane,(uint)item.id_);
			cellUI.showTips = true;
			obj.GetComponent<BagCellUI>().countLab.text =item.name_; 
			//HeadIconLoader.Instance.LoadIcon(item.icon_,obj.GetComponent<BagCellUI>().itemIcon);
            //if (!iconName_.Contains(item.icon_))
              //  iconName_.Add(item.icon_);
			grid.AddChild(obj.transform);
			obj.SetActive(true);
			obj.transform.localScale = Vector3.one;
			cellList.Add(obj);
		}

	}
	
	GameObject role;
	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if (role != null)
		{
			Destroy (role);
			role = null;
		}

		role = ro;
		ro.transform.parent = Mpos;
		float zoom = EntityAssetsData.GetData ( ChallengeData.GetData((int)curId).assetsID_).zoom_;
		ro.transform.localPosition = Vector3.forward * -300f;
		ro.transform.localScale = new Vector3(zoom,zoom,zoom);
		ro.transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
		//EffectLevel el =ro.AddComponent<EffectLevel>();
		//el.target =ro.transform.parent.parent.GetComponent<UISprite>();
	}


	public uint curId
	{
		set
		{
			_curId = value;
			curNumLab.text = (_curId).ToString();
		}
		get
		{
			return _curId;
		}
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		foreach(GameObject x in cellPoolList)
		{
			if(x != null)
			{
				Destroy(x);
			}
		}
		cellPoolList.Clear ();
		Destroy(role);

		Hide ();
	}

	private void OnResetBtn(ButtonScript obj, object args, int param1, int param2)
	{
		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue("shifouchongzhi"), () => {
			NetConnection.Instance.resetHundredTier ();
		});
	}

	private void OnTeamBtn(ButtonScript obj, object args, int param1, int param2)
	{
		/*TeamSystem._teamType = TeamType.TT_Daochang;
		int hundreLevel = 0;
		GlobalValue.Get(Constant.C_HundredBattle, out hundreLevel);
		VIPPackageItemManager.level = hundreLevel;
		TeamUIPanel.OpenFastTeam ();
		StageMgr.LoadingAsyncScene(GlobalValue.StageName_groupScene);
		*/
		TeamUI.SwithShowMe ();
	}

	private void OnLeftBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_curId <= 1)
			return;
		UpdateChallengeInfo (--curId);
	}

	private void OnRightBtn(ButtonScript obj, object args, int param1, int param2)
	{

		if(_curId+1 > hundredSystem.instance.ChallengeNum)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("hundredMax"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("hundredMax"));
			return;
		}

		UpdateChallengeInfo (++curId);
	}

	private void OnChallengeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(_curId != hundredSystem.instance.ChallengeNum)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("hundredMax"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("hundredMax"));
			return;
		}

		if(hundredSystem.instance.UseNum == 0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("tioazhancishumei"));
			return;
		}

		if(TeamSystem.IsInTeam() && !TeamSystem.IsTeamLeader(GamePlayer.Instance.InstId))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("zuduizhongduichang"));
			return;
		}

		if(TeamSystem.IsInTeam())
		{
			COM_SimplePlayerInst[] team = TeamSystem.GetTeamMembers();
			if(team == null )
			{
				return;
			}
			//int level = 0;
			//GlobalValue.Get(Constant.C_PVPJJCOpenlevel, out level);
			for (int i = 0; i < team.Length; i++)
			{
				if (team[i].properties_[(int)PropertyType.PT_Level] < 35)
				{
					PopText.Instance.Show(LanguageManager.instance.GetValue("duiyuandengji"));
					return;
				}
			}
		}




		NetConnection.Instance.enterHundredScene((int)_curId); 
        //ChallengeData cData = ChallengeData.GetData ((int)_curId);
        //if(cData == null)
        //    return;
        //Prebattle.Instance.EnterNewScene (cData.senceId_);
	}



	protected override void DoHide ()
	{
		hundredSystem.instance.HundredBattleEnvet -= OnHundredBattle;
		base.DoHide ();
	}

	void  OnHundredBattle(COM_HundredBattle HundredBattle)
	{
		curId = HundredBattle.tier_;
		UpdateChallengeInfo (curId);

	}

	public override void Destroyobj ()
	{
        for (int i = 0; i < iconName_.Count; ++i)
        {
            HeadIconLoader.Instance.Delete(iconName_[i]);
        }
        iconName_.Clear();
        //AssetInfoMgr.Instance.DecRefCount(GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_HundredUI, AssetLoader.EAssetType.ASSET_UI), true);
		GameObject.Destroy (gameObject);
	}


}

