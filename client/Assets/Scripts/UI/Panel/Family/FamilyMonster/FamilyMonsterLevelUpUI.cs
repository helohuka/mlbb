using UnityEngine;
using System.Collections;

public class FamilyMonsterLevelUpUI : UIBase
{
	public UIButton closeBtn;
	public UIButton levelUpBtn;
	public UIButton superLevelUpBtn;
	public UILabel xiaohaoLab;
	public UILabel superXiaohaoLab;
	public UILabel levelLab;
	public UILabel nameLab;
	public UILabel familyMoneyLab;
	public UILabel expLab;
	public UIProgressBar expbar;
	public Transform mpos;
	private GameObject babyObj;
	public static COM_GuildProgen _progen;
	private  COM_GuildProgen _progenM;

	private int pay;
	private int superPay;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
		UIManager.SetButtonEventHandler (levelUpBtn.gameObject, EnumButtonEvent.OnClick, OnClickLevelUp, 0, 0);
		UIManager.SetButtonEventHandler (superLevelUpBtn.gameObject, EnumButtonEvent.OnClick, OnClickSuperLevelUp, 0, 0);
		FamilySystem.instance.UpdateProgenitusEvent += new RequestEventHandler<COM_GuildProgen> (OnProgenitusEvent);
		UIPanel panel = GetComponent<UIPanel>();
		if (panel != null)
		{
			panel.renderQueue = UIPanel.RenderQueue.StartAt;
			panel.startingRenderQueue = 3000;
		}
		Monster = _progen;
		GlobalValue.Get(Constant.C_FamilyProgenitusAddExpPay, out pay);
		GlobalValue.Get(Constant.C_FamilyProgenitusAddExpSuperPay, out superPay);
		xiaohaoLab.text = pay.ToString ();
		superXiaohaoLab.text = superPay.ToString ();
	}

	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass

	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_FamilyMonsterLevelUp);
	}
	
	public static void ShowMe(COM_GuildProgen progen)
	{
		_progen = progen;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_FamilyMonsterLevelUp );
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_FamilyMonsterLevelUp );
	}
	
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}
	
	#endregion
	
	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{ 
		Hide ();
	}

	private void OnClickLevelUp(ButtonScript obj, object args, int param1, int param2)
	{

		FamilyData fData = FamilyData.GetData((int)GuildBuildingType.GBT_Progenitus,FamilySystem.instance.GuildData.guildLevel_);
		if (fData == null)
		{ 
			return;
		}
		if(fData.number_ <= Monster.lev_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("dangqianzuida"));
			return;
		}
		if (FamilySystem.instance.GuildData.fundz_ < pay)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_LevelupGuildBuildingMoneyLess"));
			return;
		}
		NetConnection.Instance.progenitusAddExp (Monster.mId_,false);
	}

	private void OnClickSuperLevelUp(ButtonScript obj, object args, int param1, int param2)
	{

		FamilyData fData = FamilyData.GetData((int)GuildBuildingType.GBT_Progenitus,FamilySystem.instance.GuildData.guildLevel_);
		if (fData == null)
		{ 
			return;
		}
		if(fData.number_ <= Monster.lev_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("dangqianzuida"));
			return;
		}
	
		if (FamilySystem.instance.GuildData.fundz_ < superPay)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_LevelupGuildBuildingMoneyLess"));
			return;
		}
		NetConnection.Instance.progenitusAddExp (Monster.mId_,true);
	}

	public COM_GuildProgen Monster
	{
		set
		{
		
			if(value != null)
			{
				familyMonsterData bData = familyMonsterData.GetData(value.mId_,value.lev_);
				if(bData == null)
					return;
				if(_progenM == null)
				{
					_progenM = value;
					GameManager.Instance.GetActorClone((ENTITY_ID)bData._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, null, "UI");
				}
				_progen = value;
				nameLab.text = bData._Name;
				levelLab.text = _progen.lev_.ToString();
				expLab.text = _progen.exp_+"/"+bData._LevelExp;
				expbar.value = (float)_progen.exp_/(float)bData._LevelExp;
				familyMoneyLab.text = FamilySystem.instance.GuildData.fundz_.ToString();
			}
		}
		get
		{
			return _progen;
		}
	}
	
	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if (gameObject == null || !this.gameObject.activeSelf)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		if(babyObj != null)
		{
			Destroy(ro);
			PlayerAsseMgr.DeleteAsset((ENTITY_ID)data.iParam, false);
			return;
		}
		ro.transform.parent = mpos;
		ro.transform.localScale = new Vector3(250f,250f,250f);
		ro.transform.localPosition = Vector3.forward * -40;
		ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		babyObj = ro;
	}

	private void OnProgenitusEvent(COM_GuildProgen progen )
	{
		if (progen.mId_ == Monster.mId_)
			Monster = progen;
	}

	void OnDestroy()
	{
		FamilySystem.instance.UpdateProgenitusEvent -= OnProgenitusEvent;
	}
}

