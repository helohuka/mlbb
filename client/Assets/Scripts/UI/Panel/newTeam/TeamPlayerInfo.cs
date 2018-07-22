using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TeamPlayerInfo : UIBase
{

	public UILabel _PInfoTitleLable;
	public UILabel _PInfoBabyLable;
	public UILabel _PInfoHuoLable;
	public UILabel _PInfoNameLable;
	public UILabel _PInfoProLable;
	public UILabel _PInfoLevelLable;
	public UILabel _PInfoLookSkillLable;
	public UILabel _PInfoAddFLable;
	public UILabel _PInfoInvLable;
	public UILabel _BabyLevel1;
	public UILabel _BabyLevel2;
	public UILabel _BabyLevel3;


	public List<GameObject> eql = new List<GameObject> ();
	public UIButton haoyouBtn;
	public UIButton yaoqingBtn;

	public UIButton closeBtn;
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel zhiyeb;
	public List<UITexture> equips = new List<UITexture>();
	public List<UISprite> employes = new List<UISprite> ();
	public List<UISprite> babeis = new List<UISprite> ();
	public Transform Mpos; 
	public UIButton skillBtn;
	public UIPanel skilList;
	public UIButton skilListCloseBtn;
	public GameObject  skillCell;
	public UIGrid skillGrid;
	private static COM_SimplePlayerInst _splayerInst;

	private GameObject babyObj;
	private List<GameObject> skillCellList = new List<GameObject>();
	
	void Start ()
	{
		InitUIText ();
		skilList.gameObject.SetActive (false);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (skillBtn.gameObject, EnumButtonEvent.OnClick, OnSkillBtn, 0, 0);
		UIManager.SetButtonEventHandler (skilListCloseBtn.gameObject, EnumButtonEvent.OnClick, OnSkillCloseBtn, 0, 0);
		UIManager.SetButtonEventHandler (haoyouBtn.gameObject, EnumButtonEvent.OnClick, OnhaoyouBtn, 0, 0);
		UIManager.SetButtonEventHandler (yaoqingBtn.gameObject, EnumButtonEvent.OnClick, OnyaoqingBtn, 0, 0);
		InitPlayerInfo ();
		skillCell.gameObject.SetActive (false);
	}

	void InitUIText()
	{
		_PInfoTitleLable.text = LanguageManager.instance.GetValue("Team_PInfoTitle");
		_PInfoBabyLable.text = LanguageManager.instance.GetValue("Team_PInfoBaby");
		_PInfoHuoLable.text = LanguageManager.instance.GetValue("Team_PInfoHuo");
		_PInfoNameLable.text = LanguageManager.instance.GetValue("Team_PInfoName");
		_PInfoProLable.text = LanguageManager.instance.GetValue("Team_PInfoPro");
		_PInfoLevelLable.text = LanguageManager.instance.GetValue("Team_PInfoLevel");
		_PInfoLookSkillLable.text = LanguageManager.instance.GetValue("Team_PInfoLookSkill");
		_PInfoAddFLable.text = LanguageManager.instance.GetValue("Team_PInfoAddF");
		_PInfoInvLable.text = LanguageManager.instance.GetValue("Team_PInfoInv");
		_BabyLevel1.text = LanguageManager.instance.GetValue("Team_BabyLevel");
		_BabyLevel2.text = LanguageManager.instance.GetValue("Team_BabyLevel");
		_BabyLevel3.text = LanguageManager.instance.GetValue("Team_BabyLevel");
	}


	private void OnhaoyouBtn(ButtonScript obj, object args, int param1, int param2)
	{
		int fMax = 0;
		GlobalValue.Get(Constant.C_FriendMax, out fMax);
		if(FriendSystem.Instance().friends_.Count >= fMax)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue( "haoyoumax"));
			return;
		}
		NetConnection.Instance.addFriend (_splayerInst.instId_);
	}
	private GuildJob gjob;
	private void OnyaoqingBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GuildSystem.IsInGuild())
		{
			gjob = (GuildJob)Enum.ToObject (typeof(GuildJob), GuildSystem.GetGuildMemberSelf(GamePlayer.Instance.InstId).job_);
			if(gjob == GuildJob.GJ_VicePremier )
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("yaoqingchenggong"));

			}else if(gjob == GuildJob.GJ_SecretaryHead)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("yaoqingchenggong"));

			}else
				if(gjob == GuildJob.GJ_Premier)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("yaoqingchenggong"));

			}
			NetConnection.Instance.inviteJoinGuild (_splayerInst.instName_);
		}else
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nojiazu"));
		}

	}


	
	public void ShowPanel(COM_SimplePlayerInst inst)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = -1;
		this.gameObject.SetActive (true);
		_splayerInst = inst;
	}

	public void SetSimplePlayerInst(COM_SimplePlayerInst playInst)
	{
		_splayerInst = playInst;
	}

	
	void InitPlayerInfo()
	{
		nameLab.text = _splayerInst.instName_;
		levelLab.text = _splayerInst.properties_[(int)PropertyType.PT_Level]+"";
		zhiyeb.text = Profession.get ((JobType)_splayerInst.properties_[(int)PropertyType.PT_Profession], (int)_splayerInst.properties_[(int)PropertyType.PT_ProfessionLevel]).jobName_;
        int weapon = GlobalInstanceFunction.Instance.WeaponID(_splayerInst);
        int weaponAssetid = 0;
        ItemData wep = ItemData.GetData(weapon);
        if (wep != null)
            weaponAssetid = wep.weaponEntityId_;
        GameManager.Instance.GetActorClone((ENTITY_ID)_splayerInst.properties_[(int)PropertyType.PT_AssetId], (ENTITY_ID)weaponAssetid, EntityType.ET_Player, AssetLoadCallBack, null, "UI", GlobalInstanceFunction.Instance.GetDressId(_splayerInst.equips_));
		float idd = _splayerInst.properties_ [(int)PropertyType.PT_AssetId];
		for(int i =0;i<eql.Count;i++)
		{
			if(eql[i] != null)
			{
				eql[i].SetActive(false);

//				UITexture te = equips[i].GetComponent<UITexture>();
//				te.mainTexture = null;
			}
		}
		
		for(int i =0;i<_splayerInst.equips_.Length;i++)
		{
			if(_splayerInst.equips_[i] != null)
			{
				HeroEquipCellUI hec = eql[_splayerInst.equips_[i].slot_].GetComponent<HeroEquipCellUI>();
				hec.Equip = _splayerInst.equips_[i];
//				UITexture te = equips[_splayerInst.equips_[i].slot_].GetComponent<UITexture>();
//				HeadIconLoader.Instance.LoadIcon (ItemData.GetData((int)_splayerInst.equips_[i].itemId_).icon_, te);
			}
			else
			{
				//equips[i].gameObject.SetActive(false);
			}
		}
		
		for(int i =0;i<employes.Count;i++)
		{
			employes[i].gameObject.SetActive(false);
		}
		
		for(int i =0;i<babeis.Count;i++)
		{
			babeis[i].gameObject.SetActive(false);
		}
		
		for(int i =0 ;i<_splayerInst.battleEmps_.Length;i++)
		{   //nas =  =  

			employes[i].gameObject.SetActive(true);

			employes[i].transform.Find("icon").GetComponent<UISprite>().spriteName = EmployessSystem.instance.GetQualityBack((int)_splayerInst.battleEmps_[i].quality_);
			employes[i].transform.Find("name").GetComponent<UILabel>().text = _splayerInst.battleEmps_[i].instName_;
			UISprite sp = employes[i].transform.FindChild("icon").FindChild("zhiye").GetComponent<UISprite>();
			sp.spriteName =Profession.get ((JobType)_splayerInst.battleEmps_[i].properties_[(int)PropertyType.PT_Profession],(int)_splayerInst.battleEmps_[i].properties_[(int)PropertyType.PT_ProfessionLevel]).jobtype_.ToString();
			//employes[i].transform.Find("pross").GetComponent<UILabel>().text =  LanguageManager.instance.GetValue(((JobType)_splayerInst.battleEmps_[i].properties_[(int)PropertyType.PT_Profession]).ToString());
			UITexture tex1 = employes[i].transform.Find("icon").Find("icon").GetComponent<UITexture>(); 
			HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)_splayerInst.battleEmps_[i].properties_[(int)PropertyType.PT_AssetId]).assetsIocn_, tex1);
			
			
		}
		
		for(int i=0;i<_splayerInst.babies1_.Length;i++)
		{
			babeis[i].gameObject.SetActive(true);
			UITexture tex = babeis[i].transform.FindChild("icon").GetComponent<UITexture>();
			HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)_splayerInst.babies1_[i].properties_[(int)PropertyType.PT_AssetId]).assetsIocn_, tex);
			
			UITexture tex1 = babeis[i].transform.FindChild("zhongzuIocn").GetComponent<UITexture>();
			HeadIconLoader.Instance.LoadIcon ( BabyData.GetData((int)_splayerInst.babies1_[i].properties_[(int)PropertyType.PT_TableId])._RaceIcon, tex1);
			babeis[i].transform.FindChild("Label").GetComponent<UILabel>().text = _splayerInst.babies1_[i].properties_[(int)PropertyType.PT_Level].ToString();
		}

		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Friend))
		{
			haoyouBtn.gameObject.SetActive(false);
		}

		if(_splayerInst.instId_ == GamePlayer.Instance.InstId  )
		{
			haoyouBtn.gameObject.SetActive(false);
		}
		if(FriendSystem.Instance().IsmyFriend((int)_splayerInst.instId_))
		{
			haoyouBtn.gameObject.SetActive(false);
		}
	}


	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		//NGUITools.SetChildLayer(ro.transform, LayerMask.NameToLayer("3D"));
		//ro.transform.parent = Mpos;
		//ro.transform.localScale = new Vector3(300,300,300);
		//ro.transform.localPosition = Vector3.zero;
		//ro.transform.localRotation = Quaternion.Euler (10f, 180f, 0f);
		
		
		ro.transform.parent = Mpos;
		ro.transform.localPosition = Vector3.forward * -300f;
		ro.transform.localScale = new Vector3(400f,400f,400f);
		ro.transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
		EffectLevel el =ro.AddComponent<EffectLevel>();
		el.target =ro.transform.parent.parent.GetComponent<UISprite>();
		
		
		if(babyObj != null)
		{
			Destroy (babyObj);
			babyObj = null;
		}
		babyObj = ro;
	}
	
	
	private void UpdateSkillList()
	{
		for(int i =0;i<skillCellList.Count;i++)
		{
			skillGrid.RemoveChild(skillCellList[i].transform);
			skillCellList[i].transform.parent = null;
			GameObject.Destroy(skillCellList[i]);
			skillCellList[i]= null;
			
		}
		skillCellList.Clear ();
		for(int i =0;i<_splayerInst.skill_.Length;i++)
		{

			SkillData skdata = SkillData.GetData((int)_splayerInst.skill_[i].skillID_, (int)_splayerInst.skill_[i].skillLevel_);
			if (skdata._SkillType != SkillType.SKT_Active && skdata._SkillType != SkillType.SKT_Passive&& skdata._SkillType != SkillType.SKT_CannotUse)
				continue;
			GameObject obj = GameObject.Instantiate(skillCell.gameObject) as GameObject;
			obj.SetActive(true);
			HeadIconLoader.Instance.LoadIcon(skdata._ResIconName, obj.transform.Find("Sprite").transform.Find("icon").GetComponent<UITexture>());
			//obj.transform.Find("name").GetComponent<UILabel>().text = skdata._Name;
			obj.transform.Find("name").GetComponent<UILabel>().text = skdata._Name;
			obj.transform.parent =skillGrid.transform;
			obj.transform.localPosition = Vector3.zero;
			skillCellList.Add(obj);
			obj.transform.localScale = Vector3.one;
		}
		skillGrid.Reposition();
	}
	
	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
		//ApplicationEntry.Instance.ui3DCamera.depth = 1;
	}
	
	private void OnSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		skilList.gameObject.SetActive (true);
		Mpos.gameObject.SetActive (false);
		UpdateSkillList ();
	}
	
	private void OnSkillCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		skilList.gameObject.SetActive (false);
		Mpos.gameObject.SetActive (true);
	}
	public static void ShowMe(COM_SimplePlayerInst player)
	{
        if (player == null)
            return;

		_splayerInst = player;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_chenkPlayerPanle, false);
	}
	public static void SwithShowMe(COM_SimplePlayerInst player)
	{
		if (player == null)
			return;
		
		_splayerInst = player;
		UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_chenkPlayerPanle);
	}
	public static void HideMe()
	{
		//ross.Clear();
		UIBase.HidePanelByName(UIASSETS_ID.UIASSETS_chenkPlayerPanle);
	}
	public override void Destroyobj ()
	{

	}
	
}

