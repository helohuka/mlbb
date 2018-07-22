using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChatBabytips : UIBase {

	public UILabel _CahtTitleLable;
	public UILabel _SkillLable;
	public UILabel _RaceLable;
	public UILabel _NameLable;
	public UILabel _LevelLable;
	public UILabel _HpLable;
	public UILabel _CMpLable;
	public UILabel _DiLable;
	public UILabel _ShuiLable;
	public UILabel _HuoLable;
	public UILabel _FengLable;
	public UILabel _ShuxingLable;
	public UILabel _ChengzhangLable;
	public UILabel _AttackLable;
	public UILabel _FangyuLable;
	public UILabel _MinjieLable;
	public UILabel _JinshenLable;
	public UILabel _HuifuLable;
	public UILabel _BiSLable;
	public UILabel _MiZhLable;
	public UILabel _FJLable;
	public UILabel _SDfuLable;
	public UILabel _TLLable;
	public UILabel _LLLable;
	public UILabel _QDLable;
	public UILabel _MFLable;
	public UILabel _SDLable;
	public UILabel _SyLable;


	public UILabel raceLable;
	public UILabel shengyuLable;
	public Transform modes;
	public UIButton closeBtn;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UISlider diSlider;
	public UISlider shuiSlider;
	public UISlider huoSlider;
	public UISlider fengSlider;
	public UISprite [] skillIcons;
	public List<SkillData> skillDatas = new List<SkillData>();
	public UILabel gongjiLabel;
	public UILabel fangyuLabel;
	public UILabel minjieLabel;
	public UILabel jingshenLabel;
	public UILabel huifuLabel;
	public UILabel bishaLabel;
	public UILabel mingzhongLabel;
	public UILabel fanjiLabel;
	public UILabel shanduoLabel;
	public UITexture raceIcon;
	public UILabel hpLabel;
	public UILabel mpLabel;
	public UILabel shengyuLab;
	public UILabel tiLiLabel;
	public UILabel QiangduLabel;
	public UILabel mofaLabel;
	public UILabel LiliangLabel;
	public UILabel SuduLabel;

	public UILabel tiLiPLabel;
	public UILabel QiangduPLabel;
	public UILabel mofaPLabel;
	public UILabel LiliangPLabel;
	public UILabel SuduPLabel;
	public GameObject TipsObj;
	public UILabel TipsLabel;
	public UILabel skillLabel;
	public UILabel xiaohaoLabel;
	public UILabel levelSkillLabel;
    static COM_BabyInst Inst;

	void Start () {
		InitUIText ();
		//Inst = GamePlayer.Instance.GetBabyInst (id);
        UIPanel panel = GetComponent<UIPanel>();
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickcloseBtn, 0, 0);
        //panel.sortingOrder = panel.depth;
        ShowBabyProperty ();
	}
	void InitUIText()
	{
		_CahtTitleLable.text = LanguageManager.instance.GetValue("Caht_Title");
		_SkillLable.text = LanguageManager.instance.GetValue("Caht_Skill");
		_RaceLable.text = LanguageManager.instance.GetValue("Caht_Race");
		_NameLable.text = LanguageManager.instance.GetValue("Caht_Name");
		_LevelLable.text = LanguageManager.instance.GetValue("Caht_Level");
		_HpLable.text = LanguageManager.instance.GetValue("Caht_Hp");
		_CMpLable.text = LanguageManager.instance.GetValue("Caht_CMp");
		_DiLable.text = LanguageManager.instance.GetValue("Caht_Di");
		_ShuiLable.text = LanguageManager.instance.GetValue("Caht_Shui");
		_HuoLable.text = LanguageManager.instance.GetValue("Caht_Huo");
		_FengLable.text = LanguageManager.instance.GetValue("Caht_Feng");
		_ShuxingLable.text = LanguageManager.instance.GetValue("Caht_Shuxing");
		_ChengzhangLable.text = LanguageManager.instance.GetValue("Caht_Chengzhang");
		_AttackLable.text = LanguageManager.instance.GetValue("Caht_Attack");
		_FangyuLable.text = LanguageManager.instance.GetValue("Caht_fangyu");
		_MinjieLable.text = LanguageManager.instance.GetValue("Caht_minjie");
		_JinshenLable.text = LanguageManager.instance.GetValue("Caht_jingshen");
		_HuifuLable.text = LanguageManager.instance.GetValue("Caht_huifu");
		_BiSLable.text = LanguageManager.instance.GetValue("Caht_bisha");
		_MiZhLable.text = LanguageManager.instance.GetValue("Caht_mingzhong");
		_FJLable.text = LanguageManager.instance.GetValue("Caht_fanji");
		_SDfuLable.text = LanguageManager.instance.GetValue("Caht_shanduo");
		_TLLable.text = LanguageManager.instance.GetValue("Caht_tili");
		_LLLable.text = LanguageManager.instance.GetValue("Caht_liliang");
		_QDLable.text = LanguageManager.instance.GetValue("Caht_qiangdu");
		_MFLable.text = LanguageManager.instance.GetValue("Caht_mofa");
		_SDLable.text = LanguageManager.instance.GetValue("Caht_sudu");
		_SyLable.text = LanguageManager.instance.GetValue("Caht_shengyu");
	}
	void OnClickcloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
		
	}
	UIEventListener Listener;
	void ShowBabyProperty()
	{
		GameManager.Instance.GetActorClone((ENTITY_ID)Inst.properties_[(int)PropertyType.PT_AssetId], (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadSelfCallBack,new ParamData((int)Inst.instId_,(int)Inst.properties_[(int)PropertyType.PT_AssetId]),"UI");
		nameLabel.text = Inst.instName_;
		raceLable.text = LanguageManager.instance.GetValue( BabyData.GetData((int)Inst.properties_[(int)PropertyType.PT_TableId])._RaceType.ToString());
		shengyuLable.text = Inst.properties_ [(int)PropertyType.PT_Free].ToString ();
		levelLabel.text = Inst.properties_[(int)PropertyType.PT_Level].ToString();
		gongjiLabel.text = Inst.properties_[(int)PropertyType.PT_Attack].ToString();
		fangyuLabel.text = Inst.properties_[(int)PropertyType.PT_Defense].ToString();
		minjieLabel.text = Inst.properties_[(int)PropertyType.PT_Agile].ToString();
		jingshenLabel.text = Inst.properties_[(int)PropertyType.PT_Spirit].ToString();
		huifuLabel.text = Inst.properties_[(int)PropertyType.PT_Reply].ToString();
		bishaLabel.text = Inst.properties_[(int)PropertyType.PT_Sex].ToString();
		mingzhongLabel.text = Inst.properties_[(int)PropertyType.PT_Hit].ToString();
		fanjiLabel.text = Inst.properties_[(int)PropertyType.PT_counterpunch].ToString();
		shanduoLabel.text = Inst.properties_[(int)PropertyType.PT_Dodge].ToString();
        int id = (int)Inst.properties_[(int)PropertyType.PT_TableId];
		shengyuLab.text = Inst.properties_[(int)PropertyType.PT_Free].ToString();
        BabyData bsdata = BabyData.GetData(id);
		diSlider.value = bsdata._Ground/10f;
		fengSlider.value = bsdata._Wind / 10f;
		shuiSlider.value = bsdata._Water / 10f;
		huoSlider.value = bsdata._Fire / 10f;

		
		mofaLabel.text =   /*"[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Magic].ToString()+"[-][33FF66]"+*/"  -"+(bsdata._BIG_Magic - Inst.gear_[(int)BabyInitGear.BIG_Magic]);
		tiLiLabel.text =/*"[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Stama].ToString()+"[-][33FF66]"+*/"  -"+(bsdata._BIG_Stama - Inst.gear_[(int)BabyInitGear.BIG_Stama]);
		SuduLabel.text =/*"[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Speed].ToString()+"[-][33FF66]"+*/"  -"+(bsdata._BIG_Speed - Inst.gear_[(int)BabyInitGear.BIG_Speed]);
		QiangduLabel.text =/*"[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Power].ToString()+"[-][33FF66]"+*/"  -"+(bsdata._BIG_Power - Inst.gear_[(int)BabyInitGear.BIG_Power]);
		LiliangLabel.text =/*"[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Strength].ToString()+"[-][33FF66]"+*/"  -"+(bsdata._BIG_Strength - Inst.gear_[(int)BabyInitGear.BIG_Strength]);

		tiLiPLabel.text = Inst.properties_[(int)PropertyType.PT_Stama].ToString();
		QiangduPLabel.text = Inst.properties_[(int)PropertyType.PT_Power].ToString();
		mofaPLabel.text = Inst.properties_[(int)PropertyType.PT_Magic].ToString();
		LiliangPLabel.text = Inst.properties_[(int)PropertyType.PT_Strength].ToString();
		SuduPLabel.text = Inst.properties_[(int)PropertyType.PT_Speed].ToString();



		hpLabel.text = Inst.properties_ [(int)PropertyType.PT_HpCurr].ToString ();
		mpLabel.text = Inst.properties_ [(int)PropertyType.PT_MpCurr].ToString ();
		HeadIconLoader.Instance.LoadIcon(BabyData.GetData((int)Inst.properties_[(int)PropertyType.PT_TableId])._RaceIcon, raceIcon);      
		for (int i = 0; i < skillIcons.Length; ++i)
		{ //初始化
			Listener = UIEventListener.Get(skillIcons[i].GetComponent<UIButton>().gameObject);
			Listener.onPress = null;
			UIManager.RemoveButtonEventHandler(skillIcons[i].gameObject,EnumButtonEvent.OnClick);
			Transform ssp = skillIcons[i].transform.FindChild("suo000");
			ssp.gameObject.SetActive(false);
			ssp = skillIcons[i].transform.FindChild("skillicon");
			ssp.gameObject.SetActive(false);
		}
		
		for (int i = 0; i<Inst.skill_.Length; i++)
		{
            SkillData sdata = SkillData.GetMinxiLevelData((int)Inst.skill_[i].skillID_);
            if (sdata._Name.Equals(LanguageManager.instance.GetValue("playerPro_FightBack")))
			{
				continue;
			}
            if (sdata._Name.Equals(LanguageManager.instance.GetValue("playerPro_Dodge")))
			{
				continue;
			}
			skillDatas.Add(sdata);
		}
		
		for (int i = 0; i < skillDatas.Count; ++i)
		{
			if (i > skillIcons.Length)
				break; /// rongcuo 
            if (i > bsdata._SkillNum)
				break; ///错误
			Transform ssp = skillIcons[i].transform.FindChild("suo000");
			ssp.gameObject.SetActive(false);
			ssp = skillIcons[i].transform.FindChild("skillicon");
			ssp.gameObject.SetActive(true);
			UITexture sp = skillIcons[i].GetComponentInChildren<UITexture>();
			HeadIconLoader.Instance.LoadIcon(skillDatas[i]._ResIconName, sp);          
			Listener = UIEventListener.Get(skillIcons[i].GetComponent<UIButton>().gameObject);
			Listener.parameter = skillDatas[i]._Id;
			Listener.onPress = buttonPress; 
		}
		for(int i =0;i<skillIcons.Length;i++)
		{
			if(i< skillDatas.Count)
			{
				Transform lab = skillIcons[i].transform.FindChild("Label");
				lab.gameObject.SetActive(false);
			}
			if (i < bsdata._SkillNum && i >= skillDatas.Count)
			{
				Transform lab = skillIcons[i].transform.FindChild("Label");
				lab.gameObject.SetActive(true);
				
				//UIManager.SetButtonEventHandler (skillIcons[i].gameObject, EnumButtonEvent.OnClick, OnClickSkillNpc,0, 0);
				
			}
			if (i >= bsdata._SkillNum)
			{
				skillIcons[i].gameObject.SetActive(false);
			}

		}


	}
	void buttonPress(GameObject sender,bool isPressed)
	{
		if (isPressed)
		{
			int str = (int)UIEventListener.Get (sender).parameter;

			TipsObj.SetActive(true);
			UIManager.Instance.AdjustUIDepth(TipsObj.transform.transform);
			//MainbabyListUI.babyObj.SetActive(false);
			//TipsObj.transform.localPosition = sender.transform.localPosition;
			SkillData sdata = SkillData.GetMinxiLevelData(str);
			TipsLabel.text = sdata._Desc;
			skillLabel.text = sdata._Name;
			xiaohaoLabel.text = sdata._Cost_mana.ToString()+LanguageManager.instance.GetValue("PT_Magic");
			levelSkillLabel.text = sdata._Level.ToString();
			//ApplicationEntry.Instance.ui3DCamera.depth = -1;
			ClientLog.Instance.Log (str);	
		}
		else
		{
			//ApplicationEntry.Instance.ui3DCamera.depth = 1.2f;
			TipsObj.SetActive(false);
			//MainbabyListUI.babyObj.SetActive(true);
		}
	}
	bool hasDestroy = false;
	void AssetLoadSelfCallBack(GameObject ro, ParamData data)
	{
		if (hasDestroy)
		{
			DestroyBaby((ENTITY_ID)data.iParam, true, ro);
			return;
		}
		
		ro.transform.parent = modes.transform;
		ro.transform.localPosition = Vector3.forward * -150f;
		ro.transform.rotation = new Quaternion(ro.transform.rotation.x, -180, ro.transform.rotation.z, ro.transform.rotation.w);
		ro.transform.localScale = new Vector3 (EntityAssetsData.GetData(data.iParam2).zoom_,EntityAssetsData.GetData(data.iParam2).zoom_,EntityAssetsData.GetData(data.iParam2).zoom_);
	}
	void DestroyBaby(ENTITY_ID eId,bool unLoadAllLoadedObjects,GameObject obj)
	{
		PlayerAsseMgr.DeleteAsset (eId, unLoadAllLoadedObjects);
		Destroy (obj);
	}
	void OnDestroy()
	{
		hasDestroy = true;
	}
	public static void ShowMe(COM_BabyInst babyinst)
	{
		Inst = babyinst;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ChatBabyUI);
	}
	public static void SwithShowMe(COM_BabyInst babyinst)
	{
		Inst = babyinst;
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ChatBabyUI);
	}
	public override void Destroyobj ()
	{
		PlayerAsseMgr.DeleteAsset((ENTITY_ID)Inst.properties_[(int)PropertyType.PT_AssetId], true);
	}
}
