using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class PlayerProperty : UIBase {

	public UILabel _ResistanceLable;
	public UILabel _AntiDrunkLable;
	public UILabel _AntiChaosLable;
	public UILabel _AntiForgottenLable;
	public UILabel _AntiSleepingLable;
	public UILabel _AntitoxicLable;
	public UILabel _AntiPetrochemical;

	public UILabel _AttributesLable;
	public UILabel _AttackLable;
	public UILabel _DefenseLable;
	public UILabel _AgileLable;
	public UILabel _SpiritLable;
	public UILabel _ReplyLable;
	public UILabel _KillLable;
	public UILabel _HitLable;
	public UILabel _FightBackLable;
	public UILabel _DodgeLable;

	public UILabel _SurplusLable;
	public UILabel _PhysicalLable;
	public UILabel _PowerLable;
	public UILabel _StrengthLable;
	public UILabel _SpeedLable;
	public UILabel _MagicLable;
	public UILabel _AKeyPlusPointLable;
	public UILabel _ConfirmAddLable;



	public UISprite [] tuijianSp;

	public UILabel nameLabel;
	public UILabel zhiyeLabel;
	public UILabel accatkLabel;
	public UILabel defensekLabel;
	public UILabel agilitykLabel;
	public UILabel spiritLabel;
	public UILabel replyLabel;
	public UILabel slayLabel;
	public UILabel hitLabel;
	public UILabel counterattackLabel;
	public UILabel dodgingLabel;
	public UILabel lethargyLabel;
	public UILabel KangduLabel;
	public UILabel AntidrunkLabel;
	public UILabel AntichaosLabel;
	public UILabel AntiamnesticLabel;
	public UILabel AntiPetrochemicalLabel;
    public UILabel gongjiSim;
    public UILabel fangyuSim;
    public UILabel minjieSim;
    public UILabel jingshenSim;
    public UILabel huifuSim;

    public GameObject ContainerPanel;

	public static int []idss;
	
	public UIButton tiliJa;
	public UIButton tiliJan;
	public UILabel tili;
    public UILabel tiliSim;
	
	public UIButton liliangJa;
	public UIButton liliangJan;
	public UILabel liliang;
    public UILabel liliangSim;
	
	public UIButton QiangduJa;
	public UIButton QiangduJan;
	public UILabel Qiangdu;
    public UILabel QiangduSim;
	
	public UIButton suduJa;
	public UIButton suduJan;
	public UILabel sudu;
    public UILabel suduSim;
	
	public UIButton mofaJa;
	public UIButton mofaJan;
	public UILabel mofa;
    public UILabel mofaSim;
	
	public UILabel Shengyu;

	public UIButton XidianBtn;

	private int curDian;

    bool hasDestroyed_ = false;

	public Transform mpos;
	private int itemid;
	List<UILabel>Attributes = new List<UILabel> ();
	public UIButton queding;
	public UIButton yijianBtn;
	GamePlayer playerInst;
	List<COM_Addprop> propList = new List<COM_Addprop>();
	Dictionary<string,int> AttributeFree = new Dictionary<string, int>();
	Dictionary<UIButton,PropertyType> jiaBtns = new Dictionary<UIButton, PropertyType>();
	Dictionary<UIButton,PropertyType> jianBtns = new Dictionary<UIButton, PropertyType>();
	void InitUIText()
	{
		_ResistanceLable.text = LanguageManager.instance.GetValue("playerPro_Resistance");
		_AntiDrunkLable.text = LanguageManager.instance.GetValue("playerPro_AntiDrunk");
		_AntiChaosLable.text = LanguageManager.instance.GetValue("playerPro_AntiChaos");
		_AntiForgottenLable.text = LanguageManager.instance.GetValue("playerPro_AntiForgotten");
		_AntiSleepingLable.text = LanguageManager.instance.GetValue("playerPro_AntiSleeping");
		_AntitoxicLable.text = LanguageManager.instance.GetValue("playerPro_Antitoxic");
		_AntiPetrochemical.text = LanguageManager.instance.GetValue("playerPro_AntiPetrochemical");

		_AttributesLable.text = LanguageManager.instance.GetValue("playerPro_Por");
		_AttackLable.text = LanguageManager.instance.GetValue("playerPro_Attack");
		_DefenseLable.text = LanguageManager.instance.GetValue("playerPro_Defense");
		_AgileLable.text = LanguageManager.instance.GetValue("playerPro_Agile");
		_SpiritLable.text = LanguageManager.instance.GetValue("playerPro_Spirit");
		_ReplyLable.text = LanguageManager.instance.GetValue("playerPro_Reply");
		_KillLable.text = LanguageManager.instance.GetValue("playerPro_Kill");
		_HitLable.text = LanguageManager.instance.GetValue("playerPro_Hit");
		_FightBackLable.text = LanguageManager.instance.GetValue("playerPro_FightBack");
		_DodgeLable.text = LanguageManager.instance.GetValue("playerPro_Dodge");

		_SurplusLable.text = LanguageManager.instance.GetValue("playerPro_Surplus");
		_PhysicalLable.text = LanguageManager.instance.GetValue("playerPro_Physical");
		_PowerLable.text = LanguageManager.instance.GetValue("playerPro_Power");
		_StrengthLable.text = LanguageManager.instance.GetValue("playerPro_Strength");
		_SpeedLable.text = LanguageManager.instance.GetValue("playerPro_Speed");
		_MagicLable.text = LanguageManager.instance.GetValue("playerPro_Magic");
		_AKeyPlusPointLable.text = LanguageManager.instance.GetValue("playerPro_AKeyPlusPoint");
		_ConfirmAddLable.text = LanguageManager.instance.GetValue("playerPro_ConfirmAdd");
	}
	void Start () {
		InitUIText();
        hasDestroyed_ = false;
	    playerInst = GamePlayer.Instance;
		GlobalValue.Get(Constant.C_ResetPlayerPay, out itemid);
        UpdateUI();

		if(playerInst.GetIprop(PropertyType.PT_Free) == 0)
		{
			for(int i =0;i<tuijianSp.Length ;i++)
			{
				tuijianSp[i].gameObject.SetActive(false);
			}

		}else
		{
			for(int i =0;i<tuijianSp.Length ;i++)
			{
				tuijianSp[i].gameObject.SetActive(false);
			}
		}
		initFree ();
		jiaBtns.Add (tiliJa,PropertyType.PT_Stama);
		jiaBtns.Add (liliangJa,PropertyType.PT_Strength);
		jiaBtns.Add (QiangduJa,PropertyType.PT_Power);
		jiaBtns.Add (suduJa,PropertyType.PT_Speed);
		jiaBtns.Add (mofaJa,PropertyType.PT_Magic);
		
		jianBtns.Add (tiliJan,PropertyType.PT_Stama);
		jianBtns.Add (liliangJan,PropertyType.PT_Strength);
		jianBtns.Add (QiangduJan,PropertyType.PT_Power);
		jianBtns.Add (suduJan,PropertyType.PT_Speed);
		jianBtns.Add (mofaJan,PropertyType.PT_Magic);

		foreach(UIButton bt in jiaBtns.Keys)
		{
			UIEventListener.Get(bt.gameObject).onPress = myonPress;
		}
		foreach(UIButton bt in jianBtns.Keys)
		{
			UIEventListener.Get(bt.gameObject).onPress = myonPressJian;
		}
//		nameLabel.text = GamePlayer.Instance.InstName;
//		zhiyeLabel.text = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel)).jobName_;
//
        GamePlayer.Instance.OnIPropUpdate += UpdateUI;
		
		UIManager.SetButtonEventHandler (queding.gameObject, EnumButtonEvent.OnClick, OnClickqueding, 0, 0);
		UIManager.SetButtonEventHandler (yijianBtn.gameObject, EnumButtonEvent.OnClick, OnClickyijian, 0, 0);

		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)>=20)
		{
			XidianBtn.gameObject.SetActive(true);
			UIManager.SetButtonEventHandler(XidianBtn.gameObject, EnumButtonEvent.OnClick, OnClicXidian, 0, 0);
		}else
		{
			XidianBtn.gameObject.SetActive(false);
		}


        UIManager.SetButtonEventHandler(tiliJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Stama, 0);
        UIManager.SetButtonEventHandler(tiliJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Stama, 0);

        UIManager.SetButtonEventHandler(liliangJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Strength, 0);
        UIManager.SetButtonEventHandler(liliangJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Strength, 0);

        UIManager.SetButtonEventHandler(QiangduJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Power, 0);
        UIManager.SetButtonEventHandler(QiangduJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Power, 0);

        UIManager.SetButtonEventHandler(suduJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Speed, 0);
        UIManager.SetButtonEventHandler(suduJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Speed, 0);

        UIManager.SetButtonEventHandler(mofaJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Magic, 0);
        UIManager.SetButtonEventHandler(mofaJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Magic, 0);

		//GameManager.Instance.GetActorClone((ENTITY_ID)playerInst.Properties[(int)PropertyType.PT_AssetId], (ENTITY_ID)playerInst.WeaponAssetID, AssetLoadCallBack, new ParamData(playerInst.InstId,playerInst.GetIprop(PropertyType.PT_AssetId)), "UI");

        GuideManager.Instance.RegistGuideAim(ContainerPanel, GuideAimType.GAT_MainPlayerInfoPropertyContainer);
        GuideManager.Instance.RegistGuideAim(queding.gameObject, GuideAimType.GAT_MainPlayerInfoPropertyConfirm);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerUIPropertySwitch);

		if (GlobalValue.isBattleScene(StageMgr.Scene_name)) {
			SetBtnState(false);
		}
	}
	void SetBtnState(bool issate)
	{
		
		foreach(UIButton bt in jiaBtns.Keys)
		{
			bt.gameObject.SetActive(issate);
		}
		foreach(UIButton bt in jianBtns.Keys)
		{
			bt.gameObject.SetActive(issate);
		}
		yijianBtn.gameObject.SetActive (issate);
		XidianBtn.gameObject.SetActive(issate);
		for(int i =0;i<tuijianSp.Length ;i++)
		{
			tuijianSp[i].gameObject.SetActive(false);
		}
	}
	void initFree()
	{
		string [] Attribute = Profession.get((JobType)playerInst.GetIprop(PropertyType.PT_Profession), playerInst.GetIprop(PropertyType.PT_ProfessionLevel)).Recommand_.Split(';');
		if(playerInst.GetIprop(PropertyType.PT_Free) != 0)
		{
			for(int i =0;i<tuijianSp.Length;i++)
			{
				if(tuijianSp[i]==null)continue;
				for(int j =0;j<Attribute.Length;j++)
				{
					string [] ss = Attribute[j].Split(':');
					if(tuijianSp[i].name.Equals(ss[0]))
					{
						tuijianSp[i].gameObject.SetActive(true);
					}
				}
				
			}


		}
		for(int i =0;i<Attribute.Length;i++)
		{
			string[] strs = Attribute[i].Split(':');
			if(!AttributeFree.ContainsKey(strs[0]))
			AttributeFree.Add(strs[0],int.Parse(strs[1]));
		}

	}
	bool isPrass;
	UIButton tempBtn;
	bool istargetPrassJia = false;
	bool istargetPrassJian = false;
	private void myonPress(GameObject sender,bool isPress)
	{
		tempBtn = sender.GetComponent<UIButton>();
        if (!isPress)
        {
            istargetPrassJia = isPress;
            pressTimer = 0f;
        }
        beginPlusTimer = isPress;
	}

    private void myonPressJian(GameObject sender, bool isPress)
	{
		tempBtn = sender.GetComponent<UIButton>();
        if (!isPress)
        {
            istargetPrassJian = isPress;
            pressTimer = 0f;
        }
        beginReduceTimer = isPress;
	}
	float curTime = 0.01f;
	float nextTime = 0.0f;

    bool beginPlusTimer, beginReduceTimer = false;
    float pressTimer = 0f;
	void Update () {

        if (beginPlusTimer || beginReduceTimer)
        {
            pressTimer += Time.deltaTime;
            if (pressTimer > 0.6f)
            {
                pressTimer = 0f;
                istargetPrassJia = beginPlusTimer;
                istargetPrassJian = beginReduceTimer;
                beginPlusTimer = false;
                beginReduceTimer = false;
            }
        }

		if(istargetPrassJia)
		{
			if(nextTime < Time.time)
			{
				nextTime = curTime+Time.time;
                if (!jiaBtns.ContainsKey(tempBtn))
                    return;
				OnClicktiliJa(null, null, (int)jiaBtns[tempBtn], 0);
                if (OverLoad(GamePlayer.Instance.GetIprop(jiaBtns[tempBtn]) + GetSimPropPoint(jiaBtns[tempBtn])) == false)
					istargetPrassJia = false;
			}
		}
        //else
        //{
        //    SetBtnState();
        //}
		
		if(istargetPrassJian)
		{
			if(nextTime < Time.time)
			{
				nextTime = curTime+Time.time;
                if (!jianBtns.ContainsKey(tempBtn))
                    return;
                OnClictiliJan(null, null, (int)jianBtns[tempBtn], 0);
                if (curDian >= playerInst.GetIprop(PropertyType.PT_Free))
                    istargetPrassJian = false;
			}
		}
        //else
        //{
        //    SetBtnState();
        //}
	}

    void UpdateUI()
    {
        curDian = playerInst.GetIprop(PropertyType.PT_Free);
        propList.Clear();
        tiliSim.text = "";
        liliangSim.text = "";
        QiangduSim.text = "";
        suduSim.text = "";
        mofaSim.text = "";
        gongjiSim.text = "";
        fangyuSim.text = "";
        minjieSim.text = "";
        jingshenSim.text = "";
        huifuSim.text = "";
        SetBtnState();
        ShowPlayerProperty();
    }

    void SetBtnState()
    {
        // 界面销毁后 事件貌似还没有注销 i属性更新过来后会报错
        if (queding == null)
            return;

        queding.gameObject.SetActive(propList.Count > 0);
        SetJiaBtnState(curDian > 0);
		 
        SetJianBtnState(false);
        for (int i = 0; i < propList.Count; ++i)
        {
            switch (propList[i].type_)
            {
                case PropertyType.PT_Stama:
                    tiliJan.gameObject.SetActive(true);
                    break;
                case PropertyType.PT_Strength:
                    liliangJan.gameObject.SetActive(true);
                    break;
                case PropertyType.PT_Power:
                    QiangduJan.gameObject.SetActive(true);
                    break;
                case PropertyType.PT_Speed:
                    suduJan.gameObject.SetActive(true);
                    break;
                case PropertyType.PT_Magic:
                    mofaJan.gameObject.SetActive(true);
                    break;
            }
        }
    }

    GameObject playerObj;
    void AssetLoadCallBack(GameObject ro, ParamData data)
    {
        if (hasDestroyed_)
        {
            Destroy(ro);
            return;
        }
        playerObj = ro;
        ro.transform.parent = mpos;
        ro.transform.localPosition = Vector3.forward * -1000f;
        ro.transform.localRotation = new Quaternion(ro.transform.localRotation.x, -180, ro.transform.localRotation.z, ro.transform.localRotation.w);
		ro.transform.localScale = new Vector3(EntityAssetsData.GetData(data.iParam2).zoom_ ,EntityAssetsData.GetData(data.iParam2).zoom_,EntityAssetsData.GetData(data.iParam2).zoom_ );
        EffectLevel el = ro.AddComponent<EffectLevel>();
        el.target = ro.transform.parent.parent.GetComponent<UISprite>();
    }

	void OnClickqueding(ButtonScript obj, object args, int param1, int param2)
	{
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerUIPropertyConfirmClick);
		NetConnection.Instance.changProp ((uint)playerInst.InstId,propList.ToArray());

	}

	void OnClickyijian(ButtonScript obj, object args, int param1, int param2)
	{
		while (curDian>0)
		{		
			foreach(KeyValuePair<string,int> pair in AttributeFree)
			{
				//GamePlayer.Instance.GetIprop(PropertyType.PT_Stama) + GetSimPropPoint(PropertyType.PT_Stama);
				//


				if(OverLoad(GamePlayer.Instance.GetIprop((PropertyType)Enum.Parse(typeof(PropertyType),pair.Key)) + GetSimPropPoint((PropertyType)Enum.Parse(typeof(PropertyType),pair.Key)))&&curDian>=pair.Value)
				{
						Setprop((PropertyType)Enum.Parse(typeof(PropertyType),pair.Key),(uint)pair.Value);


				}
//				if(pair.Key == PropertyType.PT_Stama.ToString())
//				{
//					Setprop(PropertyType.PT_Stama,(uint)pair.Value);
//				}else
//					if(pair.Key == PropertyType.PT_Strength.ToString())
//				{
//					Setprop(PropertyType.PT_Strength,(uint)pair.Value);
//				}else 
//					if(pair.Key == PropertyType.PT_Power.ToString())
//				{
//					Setprop(PropertyType.PT_Power,(uint)pair.Value);
//				}else 
//					if(pair.Key == PropertyType.PT_Speed.ToString())
//				{
//					Setprop(PropertyType.PT_Speed,(uint)pair.Value);
//				}else 
//					if(pair.Key == PropertyType.PT_Magic.ToString())
//				{
//					Setprop(PropertyType.PT_Magic,(uint)pair.Value);
//				}
				
			}
		}


	}
	int sum;
	void Setprop(PropertyType prop ,uint free)
	{
//		float a = (float)free / 4;
//		int b = (int)(curDian * a);
//		sum += b;
//		if(sum == curDian)
//		{
//			curDian = 0;
//		}
		curDian -= (int)free;
		if(curDian<=0)
		{
			curDian = 0;
		}
		if (isContain (prop))
		{
			for (int i = 0; i<propList.Count; i++)
			{
				if(propList[i].type_ == prop)
				{
					propList[i].uVal_ +=free;
					SetTiLi(propList[i].type_,propList[i]);
				}
			}
		}else
		{
			COM_Addprop comprop = new COM_Addprop ();
			comprop.type_ = prop;
			comprop.uVal_ += free;
			propList.Add(comprop);
			SetTiLi(comprop.type_, comprop);
		}
		SetBtnState();
	}

	void OnClicXidian(ButtonScript obj, object args, int param1, int param2)
	{
        int shopid = ShopData.GetShopId(itemid);
        if (shopid == 0)
            return;

		if(BagSystem.instance.GetItemMaxNum((uint)itemid)<=0)
		{
			QuickBuyUI.ShowMe(shopid);
		}else
		{
			COM_Item item =	BagSystem.instance.GetItemByItemId((uint)itemid);
			ItemData idata = ItemData.GetData(itemid);
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("chongzhidianshu").Replace("{n}",idata.name_),()=>{
				
				NetConnection.Instance.useItem((uint)item.slot_,(uint)GamePlayer.Instance.InstId,(uint)1);
			});
		}
	}
	void OnClicktiliJa(ButtonScript obj, object args, int param1, int param2)
	{
        if (curDian == 0)
            return;

        curDian--;
        if (curDian <= 0)
        {
            curDian = 0;
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PlayerUIAddPoint, curDian);
        }

		if (isContain ((PropertyType)param1))
		{
			for (int i = 0; i<propList.Count; i++)
			{
				if(propList[i].type_ == (PropertyType)param1)
				{
					propList[i].uVal_ +=1;
					SetTiLi(propList[i].type_,propList[i]);
				}
			}
		}else
		{
			COM_Addprop comprop = new COM_Addprop ();
			comprop.type_ = (PropertyType) param1;
			comprop.uVal_ += 1;
			propList.Add(comprop);
            SetTiLi(comprop.type_, comprop);
		}
	  

        SetBtnState();
	}

	bool isContain(PropertyType type)
	{
		for (int i = 0; i<propList.Count; i++)
		{
			if(propList[i].type_ == type)
			{
				return true;
			}
		}
		return false;
	}


	void OnClictiliJan(ButtonScript obj, object args, int param1, int param2)
	{
        if (curDian == playerInst.GetIprop(PropertyType.PT_Free))
            return;

        if (isContain((PropertyType)param1))
        {
            curDian++;
            if (curDian >= playerInst.GetIprop(PropertyType.PT_Free))
            {
                curDian = playerInst.GetIprop(PropertyType.PT_Free);
            }
            for (int i = 0; i < propList.Count; i++)
            {
                if (propList[i].type_ == (PropertyType)param1)
                {
                    if (propList[i].uVal_ > 0)
                    {
                        propList[i].uVal_ -= 1;
                        SetTiLi(propList[i].type_, propList[i]);
                        if (propList[i].uVal_ <= 0)
                            propList.RemoveAt(i--);
                    }
                }
            }
        }
        //else
        //{
        //    COM_Addprop comprop = new COM_Addprop();
        //    comprop.type_ = (PropertyType)param1;
        //    if (comprop.uVal_ > 0)
        //    {
        //        comprop.uVal_ -= 1;
        //        propList.Add(comprop);
        //        SetTiLi(comprop.type_, comprop);
        //    }
        //}
        SetBtnState();
	}

	void ShowPlayerProperty()
	{
		if(playerInst.GetIprop(PropertyType.PT_Free) == 0)
		{
			for(int i =0;i<tuijianSp.Length ;i++)
			{
				if(tuijianSp[i] == null)continue;
				tuijianSp[i].gameObject.SetActive(false);
			}
			
		}else
		{
			for(int i =0;i<tuijianSp.Length ;i++)
			{
				if(tuijianSp[i] == null)continue;
				tuijianSp[i].gameObject.SetActive(false);
			}
		}
		initFree ();
        Shengyu.text = playerInst.GetIprop(PropertyType.PT_Free).ToString();
        tili.text = playerInst.GetIprop(PropertyType.PT_Stama).ToString();
        liliang.text = playerInst.GetIprop(PropertyType.PT_Strength).ToString();
        Qiangdu.text = playerInst.GetIprop(PropertyType.PT_Power).ToString();
        sudu.text = playerInst.GetIprop(PropertyType.PT_Speed).ToString();
        mofa.text = playerInst.GetIprop(PropertyType.PT_Magic).ToString();

		accatkLabel.text   = playerInst.GetIprop(PropertyType.PT_Attack).ToString();
        defensekLabel.text = playerInst.GetIprop(PropertyType.PT_Defense).ToString();
        agilitykLabel.text = playerInst.GetIprop(PropertyType.PT_Agile).ToString();
        spiritLabel.text = playerInst.GetIprop(PropertyType.PT_Spirit).ToString();
        replyLabel.text = playerInst.GetIprop(PropertyType.PT_Reply).ToString();
		slayLabel.text = playerInst.GetIprop(PropertyType.PT_Crit).ToString();
        hitLabel.text = playerInst.GetIprop(PropertyType.PT_Hit).ToString();
        counterattackLabel.text = playerInst.GetIprop(PropertyType.PT_counterpunch).ToString();
        dodgingLabel.text = playerInst.GetIprop(PropertyType.PT_Dodge).ToString();
        lethargyLabel.text = playerInst.GetIprop(PropertyType.PT_NoSleep).ToString();
        KangduLabel.text = playerInst.GetIprop(PropertyType.PT_NoPoison).ToString();
        AntidrunkLabel.text = playerInst.GetIprop(PropertyType.PT_NoDrunk).ToString();
        AntichaosLabel.text = playerInst.GetIprop(PropertyType.PT_NoChaos).ToString();
        AntiamnesticLabel.text = playerInst.GetIprop(PropertyType.PT_NoForget).ToString();
        AntiPetrochemicalLabel.text = playerInst.GetIprop(PropertyType.PT_NoPetrifaction).ToString();
	}

    float SimAddPoint(PropertyType pointType, PropertyType effectType, COM_Addprop prop)
    {
        switch (pointType)
        {
            case PropertyType.PT_Stama:
                switch(effectType)
                {
                    case PropertyType.PT_HpMax:
                        return (prop.uVal_ * 8f);
                    case PropertyType.PT_MpMax:
                        return (prop.uVal_ * 1f);
                    case PropertyType.PT_Attack:
                        return (prop.uVal_ * 0.1f);
                    case PropertyType.PT_Defense:
                        return (prop.uVal_ * 0.1f);
                    case PropertyType.PT_Agile:
                        return (prop.uVal_ * 0.1f);
                    case PropertyType.PT_Reply:
                        return (prop.uVal_ * 0.8f);
                    case PropertyType.PT_Spirit:
                        return (prop.uVal_ * -0.3f);
                }
                return 0;
            case PropertyType.PT_Strength:
                switch (effectType)
                {
                    case PropertyType.PT_HpMax:
                        return (prop.uVal_ * 2f);
                    case PropertyType.PT_MpMax:
                        return (prop.uVal_ * 2f);
                    case PropertyType.PT_Attack:
                        return (prop.uVal_ * 2f);
                    case PropertyType.PT_Defense:
                        return (prop.uVal_ * 0.2f);
                    case PropertyType.PT_Agile:
                        return (prop.uVal_ * 0.2f);
                    case PropertyType.PT_Reply:
                        return (prop.uVal_ * -0.1f);
                    case PropertyType.PT_Spirit:
                        return (prop.uVal_ * -0.1f);
                }
                return 0;
            case PropertyType.PT_Power:
                switch (effectType)
                {
                    case PropertyType.PT_HpMax:
                        return (prop.uVal_ * 3f);
                    case PropertyType.PT_MpMax:
                        return (prop.uVal_ * 2f);
                    case PropertyType.PT_Attack:
                        return (prop.uVal_ * 0.2f);
                    case PropertyType.PT_Defense:
                        return (prop.uVal_ * 2f);
                    case PropertyType.PT_Agile:
                        return (prop.uVal_ * 0.2f);
                    case PropertyType.PT_Reply:
                        return (prop.uVal_ * -0.1f);
                    case PropertyType.PT_Spirit:
                        return (prop.uVal_ * 0.2f);
                }
                return 0;
            case PropertyType.PT_Speed:
                switch (effectType)
                {
                    case PropertyType.PT_HpMax:
                        return (prop.uVal_ * 3f);
                    case PropertyType.PT_MpMax:
                        return (prop.uVal_ * 2f);
                    case PropertyType.PT_Attack:
                        return (prop.uVal_ * 0.2f);
                    case PropertyType.PT_Defense:
                        return (prop.uVal_ * 0.2f);
                    case PropertyType.PT_Agile:
                        return (prop.uVal_ * 2f);
                    case PropertyType.PT_Reply:
                        return (prop.uVal_ * 0.2f);
                    case PropertyType.PT_Spirit:
                        return (prop.uVal_ * -0.1f);
                }
                return 0;
            case PropertyType.PT_Magic:
                switch (effectType)
                {
                    case PropertyType.PT_HpMax:
                        return (prop.uVal_ * 1f);
                    case PropertyType.PT_MpMax:
                        return (prop.uVal_ * 10f);
                    case PropertyType.PT_Attack:
                        return (prop.uVal_ * 0.1f);
                    case PropertyType.PT_Defense:
                        return (prop.uVal_ * 0.1f);
                    case PropertyType.PT_Agile:
                        return (prop.uVal_ * 0.1f);
                    case PropertyType.PT_Reply:
                        return (prop.uVal_ * -0.3f);
                    case PropertyType.PT_Spirit:
                        return (prop.uVal_ * 0.8f);
                }
                return 0;
        }
        return 0;
    }

    float[] addPoints;

	void SetTiLi(PropertyType type,COM_Addprop prop)
	{
        addPoints = new float[5];
        
        for (int i = 0; i < propList.Count; ++i)
        {
            addPoints[0] += SimAddPoint(propList[i].type_, PropertyType.PT_Attack, propList[i]);
            addPoints[1] += SimAddPoint(propList[i].type_, PropertyType.PT_Defense, propList[i]);
            addPoints[2] += SimAddPoint(propList[i].type_, PropertyType.PT_Agile, propList[i]);
            addPoints[3] += SimAddPoint(propList[i].type_, PropertyType.PT_Reply, propList[i]);
            addPoints[4] += SimAddPoint(propList[i].type_, PropertyType.PT_Spirit, propList[i]);

            accatkLabel.text = ((int)(playerInst.GetProperty(PropertyType.PT_Attack))).ToString();
            defensekLabel.text = ((int)(playerInst.GetProperty(PropertyType.PT_Defense))).ToString();
            agilitykLabel.text = ((int)(playerInst.GetProperty(PropertyType.PT_Agile))).ToString();
            replyLabel.text = ((int)(playerInst.GetProperty(PropertyType.PT_Reply))).ToString();
            spiritLabel.text = ((int)(playerInst.GetProperty(PropertyType.PT_Spirit))).ToString();
            string content = "";
            Color color = Color.green;
            UILabel lbl = null;
            int roundToint = 0;
            int adjust = 0;
            for (int j = 0; j < addPoints.Length; ++j)
            {
                roundToint = (int)addPoints[j];

                switch (j)
                {
                    case 0:
                        lbl = gongjiSim;
                        adjust = (int)(addPoints[j] + playerInst.GetProperty(PropertyType.PT_Attack)) - ((int)addPoints[j] + (int)playerInst.GetProperty(PropertyType.PT_Attack));
                        break;
                    case 1:
                        lbl = fangyuSim;
                        adjust = (int)(addPoints[j] + playerInst.GetProperty(PropertyType.PT_Defense)) - ((int)addPoints[j] + (int)playerInst.GetProperty(PropertyType.PT_Defense));
                        break;
                    case 2:
                        lbl = minjieSim;
                        adjust = (int)(addPoints[j] + playerInst.GetProperty(PropertyType.PT_Agile)) - ((int)addPoints[j] + (int)playerInst.GetProperty(PropertyType.PT_Agile));
                        break;
                    case 3:
                        lbl = huifuSim;
                        adjust = (int)(addPoints[j] + playerInst.GetProperty(PropertyType.PT_Reply)) - ((int)addPoints[j] + (int)playerInst.GetProperty(PropertyType.PT_Reply));
                        break;
                    case 4:
                        lbl = jingshenSim;
                        adjust = (int)(addPoints[j] + playerInst.GetProperty(PropertyType.PT_Spirit)) - ((int)addPoints[j] + (int)playerInst.GetProperty(PropertyType.PT_Spirit));
                        break;
                }

                roundToint += adjust;

                if (roundToint > 0)
                {
                    content = string.Format("+{0}", roundToint);
                    color = Color.green;
                }
                else if (roundToint < 0)
                {
                    content = string.Format("-{0}", Mathf.Abs(roundToint));
                    color = Color.red;
                }
                else
                    content = "";

                lbl.text = content;
                lbl.color = color;
            }
        }
        
        //if (type == PropertyType.PT_Stama)
        //{
        //    accatkLabel.text = (playerInst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (playerInst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (playerInst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (playerInst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (playerInst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}
        //else
        //    if(type == PropertyType.PT_Strength)
        //{
        //    accatkLabel.text = (playerInst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (playerInst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (playerInst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (playerInst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (playerInst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}
        //else
        //    if(type == PropertyType.PT_Power)
        //{
        //    accatkLabel.text = (playerInst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (playerInst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (playerInst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (playerInst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();

        //}
        //else
        //    if(type == PropertyType.PT_Speed)
        //{
        //    accatkLabel.text = (playerInst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (playerInst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (playerInst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (playerInst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (playerInst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}
        //else
        //    if(type == PropertyType.PT_Magic)
        //{
        //    accatkLabel.text = (playerInst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (playerInst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (playerInst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (playerInst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (playerInst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}

        Shengyu.text = curDian.ToString();

        for (int i = 0; i < propList.Count; ++i)
        {
            switch (propList[i].type_)
            {
                case PropertyType.PT_Stama:
                    //tili.text = ((int)playerInst.Properties [(int)PropertyType.PT_Stama] + propList[i].uVal_).ToString();
                    tili.text = ((int)playerInst.Properties[(int)PropertyType.PT_Stama]).ToString();
                    if (propList[i].uVal_ == 0)
                        tiliSim.text = "";
                    else
                        tiliSim.text = string.Format("+{0}", propList[i].uVal_);
                    break;
                case PropertyType.PT_Strength:
                    liliang.text = ((int)playerInst.Properties[(int)PropertyType.PT_Strength]).ToString();
                    if (propList[i].uVal_ == 0)
                        liliangSim.text = "";
                    else
                        liliangSim.text = string.Format("+{0}", propList[i].uVal_);
                    break;
                case PropertyType.PT_Power:
                    Qiangdu.text = ((int)playerInst.Properties[(int)PropertyType.PT_Power]).ToString();
                    if (propList[i].uVal_ == 0)
                        QiangduSim.text = "";
                    else
                        QiangduSim.text = string.Format("+{0}", propList[i].uVal_);
                    break;
                case PropertyType.PT_Speed:
                    sudu.text = ((int)playerInst.Properties[(int)PropertyType.PT_Speed]).ToString();
                    if (propList[i].uVal_ == 0)
                        suduSim.text = "";
                    else
                        suduSim.text = string.Format("+{0}", propList[i].uVal_);
                    break;
                case PropertyType.PT_Magic:
                    mofa.text = ((int)playerInst.Properties[(int)PropertyType.PT_Magic]).ToString();
                    if (propList[i].uVal_ == 0)
                        mofaSim.text = "";
                    else
                        mofaSim.text = string.Format("+{0}", propList[i].uVal_);
                    break;
            }
        }
	}
	void SetJiaBtnState(bool isState)
	{
        tiliJa.gameObject.SetActive(isState);
        liliangJa.gameObject.SetActive(isState);
        QiangduJa.gameObject.SetActive(isState);
        suduJa.gameObject.SetActive(isState);
        mofaJa.gameObject.SetActive(isState);
		yijianBtn.gameObject.SetActive(isState);
        if (isState == false)
		{

			return;
		}
            

        int total;
        total = GamePlayer.Instance.GetIprop(PropertyType.PT_Stama) + GetSimPropPoint(PropertyType.PT_Stama);
        tiliJa.gameObject.SetActive(OverLoad(total));
        total = GamePlayer.Instance.GetIprop(PropertyType.PT_Strength) + GetSimPropPoint(PropertyType.PT_Strength);
        liliangJa.gameObject.SetActive(OverLoad(total));
        total = GamePlayer.Instance.GetIprop(PropertyType.PT_Power) + GetSimPropPoint(PropertyType.PT_Power);
        QiangduJa.gameObject.SetActive(OverLoad(total));
        total = GamePlayer.Instance.GetIprop(PropertyType.PT_Speed) + GetSimPropPoint(PropertyType.PT_Speed);
        suduJa.gameObject.SetActive(OverLoad(total));
        total = GamePlayer.Instance.GetIprop(PropertyType.PT_Magic) + GetSimPropPoint(PropertyType.PT_Magic);
        mofaJa.gameObject.SetActive(OverLoad(total));
	}

    public int GetSimPropPoint(PropertyType type)
    {
        for (int i = 0; i < propList.Count; ++i)
        {
            if (propList[i].type_ == type)
                return (int)propList[i].uVal_;
        }
        return 0;
    }

    bool OverLoad(int point)
    {
        // level * 2 + 13
        //等于 加号就已经不显示了
        return point < (GamePlayer.Instance.GetIprop(PropertyType.PT_Level) * 2 + 13);
    }

	void SetJianBtnState(bool isState)
	{
		tiliJan.gameObject.SetActive (isState);
		liliangJan.gameObject.SetActive (isState);
		QiangduJan.gameObject.SetActive (isState);
		suduJan.gameObject.SetActive (isState);
		mofaJan.gameObject.SetActive (isState);
	}

	public override void Destroyobj ()
	{
        GamePlayer.Instance.OnIPropUpdate -= UpdateUI;
		UIManager.RemoveButtonEventHandler (queding.gameObject, EnumButtonEvent.OnClick);
		
		UIManager.RemoveButtonEventHandler (tiliJa.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (tiliJan.gameObject, EnumButtonEvent.OnClick);
		
		UIManager.RemoveButtonEventHandler (liliangJa.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (liliangJan.gameObject, EnumButtonEvent.OnClick);
		
		UIManager.RemoveButtonEventHandler (QiangduJa.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (QiangduJan.gameObject, EnumButtonEvent.OnClick);
		
		UIManager.RemoveButtonEventHandler (suduJa.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (suduJan.gameObject, EnumButtonEvent.OnClick);
		
		UIManager.RemoveButtonEventHandler (mofaJa.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (mofaJan.gameObject, EnumButtonEvent.OnClick);
        if (playerObj != null)
            Destroy(playerObj);
        hasDestroyed_ = true;
	}
}
