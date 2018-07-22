using UnityEngine;
using System.Collections;

using System.Collections.Generic;

public class MainbabyProperty : MonoBehaviour {

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

	public UILabel _XkLable;
	public UILabel _EnterLable;

	public delegate void ShowInfoBaby(int uid);
	public static  ShowInfoBaby BabyProperty;

	public GameObject GrowingUpObj;
	public UIButton GrowingUpBtn;
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

    public GameObject propertyContainer;

	public static int []idss;
	public UIButton shuxingClose;
	public UIButton shuxingBtn;
	public GameObject shuxingObj;
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

	public UIButton queding;
	public UISprite tuijian;
	private int MaxDian;
	private int curDian;
	Baby Inst;
	List<UILabel>Attributes = new List<UILabel> ();
	List<COM_Addprop> propList = new List<COM_Addprop>();
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

		_XkLable.text = LanguageManager.instance.GetValue("mainbaby_shxk");
		_EnterLable.text = LanguageManager.instance.GetValue("mainbaby_queren");


	}




	void Start () 
	{
		InitUIText ();
		UpdateUI ();
		BabyProperty = UpdateUI;
		Attributes.Add (tili);
		Attributes.Add (liliang);
		Attributes.Add (Qiangdu);
		Attributes.Add (sudu);
		Attributes.Add (mofa);

        GamePlayer.Instance.babyUpdateIpropEvent += new RequestEventHandler<int>(UpdateUI);
		if(Inst != null)
		{
			if(Inst.GetIprop(PropertyType.PT_Free) == 0)
			{
				tuijian.gameObject.SetActive(false);
			}else
			{
				tuijian.gameObject.SetActive(true);
			}
			for(int i =0;i<Attributes.Count;i++)
			{
				if(i==BabyData.StrongestAttribute(Inst.GetIprop(PropertyType.PT_TableId)))
				{
					tuijian.transform.position = new Vector3(tuijian.transform.position.x,Attributes[i].transform.position.y,tuijian.transform.position.z);
				}
			}
		}else
		{
			tuijian.gameObject.SetActive(false);
		}


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

		UIManager.SetButtonEventHandler (queding.gameObject, EnumButtonEvent.OnClick, OnClickqueding, 0, 0);

		UIManager.SetButtonEventHandler (tiliJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Stama, 0);
//		UIManager.SetButtonEventHandler (tiliJa.gameObject, EnumButtonEvent.TouchDown, OnClickTouchDown, (int)PropertyType.PT_Stama, 0);

		UIManager.SetButtonEventHandler (tiliJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Stama, 0);

		UIManager.SetButtonEventHandler (liliangJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Strength, 0);
		UIManager.SetButtonEventHandler (liliangJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Strength, 0);

		UIManager.SetButtonEventHandler (QiangduJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Power, 0);
		UIManager.SetButtonEventHandler (QiangduJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Power, 0);

		UIManager.SetButtonEventHandler (suduJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Speed, 0);
		UIManager.SetButtonEventHandler (suduJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Speed, 0);

		UIManager.SetButtonEventHandler (mofaJa.gameObject, EnumButtonEvent.OnClick, OnClicktiliJa, (int)PropertyType.PT_Magic, 0);
		UIManager.SetButtonEventHandler (mofaJan.gameObject, EnumButtonEvent.OnClick, OnClictiliJan, (int)PropertyType.PT_Magic, 0);

		//UIManager.SetButtonEventHandler (GrowingUpBtn.gameObject, EnumButtonEvent.OnClick, OnClickGrowingUp,0, 0);
		queding.gameObject.SetActive(false);

		UIManager.SetButtonEventHandler (shuxingBtn.gameObject, EnumButtonEvent.OnClick, OnClickshuxingBtn,0, 0);
		UIManager.SetButtonEventHandler (shuxingClose.gameObject, EnumButtonEvent.OnClick, OnClickshuxingClose,0, 0);

        GuideManager.Instance.RegistGuideAim(propertyContainer, GuideAimType.GAT_MainBabyPropertyContainer);
        GuideManager.Instance.RegistGuideAim(queding.gameObject, GuideAimType.GAT_MainBabyPropertyConfirm);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyUIPropertySwitch);
		if (GlobalValue.isBattleScene(StageMgr.Scene_name)) {
			SetattackBtnState(false);
		}
    }
	void SetattackBtnState(bool issate)
	{
		
		foreach(UIButton bt in jiaBtns.Keys)
		{
			bt.gameObject.SetActive(issate);
		}
		foreach(UIButton bt in jianBtns.Keys)
		{
			bt.gameObject.SetActive(issate);
		}
		tuijian.gameObject.SetActive(false);
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

			}
		}
		if(istargetPrassJian)
		{
			if(nextTime < Time.time)
			{
				nextTime = curTime+Time.time;
				if (!jianBtns.ContainsKey(tempBtn))
					return;
				OnClictiliJan(null, null, (int)jianBtns[tempBtn], 0);
				if (curDian >= Inst.GetIprop(PropertyType.PT_Free))
					istargetPrassJian = false;
			}
		}
		//else
		//{
		//    SetBtnState();
		//}
	}
	void OnEnable()
	{
		UpdateUI(0);
	}

	void UpdateUI(int id = 0)
	{
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
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText();
		}else
		{
			if(id != 0)
				Inst = GamePlayer.Instance.GetBabyInst (id);
			else
				Inst = GamePlayer.Instance.GetBabyInst (idss[0]);
			curDian = Inst.GetIprop(PropertyType.PT_Free);
			propList.Clear();
			SetBtnState();
			ShowBabyProperty();
		}
		if (GlobalValue.isBattleScene(StageMgr.Scene_name)) {
			SetattackBtnState(false);
		}
	}
	void ShowBabyProperty()
	{

		if(Inst.GetIprop(PropertyType.PT_Free) == 0)
		{
			tuijian.gameObject.SetActive(false);
		}else
		{
			tuijian.gameObject.SetActive(true);
		}


			Shengyu.text = Inst.GetIprop(PropertyType.PT_Free).ToString();
			tili.text = Inst.GetIprop(PropertyType.PT_Stama).ToString();
			liliang.text = Inst.GetIprop(PropertyType.PT_Strength).ToString();
			Qiangdu.text = Inst.GetIprop(PropertyType.PT_Power).ToString();
			sudu.text = Inst.GetIprop(PropertyType.PT_Speed).ToString();
			mofa.text = Inst.GetIprop(PropertyType.PT_Magic).ToString();
			
			accatkLabel.text = Inst.GetIprop(PropertyType.PT_Attack).ToString();
			defensekLabel.text = Inst.GetIprop(PropertyType.PT_Defense).ToString();
			agilitykLabel.text = Inst.GetIprop(PropertyType.PT_Agile).ToString();
			spiritLabel.text = Inst.GetIprop(PropertyType.PT_Spirit).ToString();
			replyLabel.text = Inst.GetIprop(PropertyType.PT_Reply).ToString();
			slayLabel.text = Inst.GetIprop(PropertyType.PT_Sex).ToString();
			hitLabel.text = Inst.GetIprop(PropertyType.PT_Hit).ToString();
			counterattackLabel.text = Inst.GetIprop(PropertyType.PT_counterpunch).ToString();
			dodgingLabel.text = Inst.GetIprop(PropertyType.PT_Dodge).ToString();
			lethargyLabel.text = Inst.GetIprop(PropertyType.PT_NoSleep).ToString();
			KangduLabel.text = Inst.GetIprop(PropertyType.PT_NoPoison).ToString();
			AntidrunkLabel.text = Inst.GetIprop(PropertyType.PT_NoDrunk).ToString();
			AntichaosLabel.text = Inst.GetIprop(PropertyType.PT_NoChaos).ToString();
			AntiamnesticLabel.text = Inst.GetIprop(PropertyType.PT_NoForget).ToString();
			AntiPetrochemicalLabel.text = Inst.GetIprop(PropertyType.PT_NoPetrifaction).ToString();


		if(Inst.GetIprop(PropertyType.PT_Free) == 0)
		{
			tuijian.gameObject.SetActive(false);
		}else
		{
			tuijian.gameObject.SetActive(true);
		}
		for(int i =0;i<Attributes.Count;i++)
		{
			if(i==BabyData.StrongestAttribute(Inst.GetIprop(PropertyType.PT_TableId)))
			{
				tuijian.transform.position = new Vector3(tuijian.transform.position.x,Attributes[i].transform.position.y,tuijian.transform.position.z);
			}
		}
	}
	void ClearText()
	{
		Shengyu.text = "";
		tili.text = "";
		liliang.text = "";
		Qiangdu.text ="";
		sudu.text = "";
		mofa.text = "";		
		accatkLabel.text = "";
		defensekLabel.text = "";
		agilitykLabel.text ="";
		spiritLabel.text ="";
		replyLabel.text = "";
		slayLabel.text = "";
		hitLabel.text = "";
		counterattackLabel.text = "";
		dodgingLabel.text = "";
		lethargyLabel.text = "";
		KangduLabel.text = "";
		AntidrunkLabel.text = "";
		AntichaosLabel.text = "";
		AntiamnesticLabel.text = "";
		AntiPetrochemicalLabel.text = "";
	}

	void OnClickshuxingBtn(ButtonScript obj, object args, int param1, int param2)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = -1;
		MainbabyListUI.babyObj.SetActive (false);
		shuxingObj.SetActive (true);
		
	}
	void OnClickshuxingClose(ButtonScript obj, object args, int param1, int param2)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = -1;
		shuxingObj.SetActive (false);
		
	}

	void OnClickqueding(ButtonScript obj, object args, int param1, int param2)
	{
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyUIPropertyConfirmClick);
		NetConnection.Instance.changProp ((uint)idss[0],propList.ToArray());
		queding.gameObject.SetActive (false);

	}
	void OnClickGrowingUp(ButtonScript obj, object args, int param1, int param2)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = -1;
		MainbabyListUI.babyObj.SetActive (false);
		GrowingUpObj.gameObject.SetActive (true);

	}
	void OnClicktiliJa(ButtonScript obj, object args, int param1, int param2)
	{
        if (curDian == 0)
            return;

		curDian--;
		if (curDian <= 0)
		{
			curDian = 0;
			GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyUIAddPoint, curDian);
			istargetPrassJia = false;
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
		}
		else
		{
			COM_Addprop comprop = new COM_Addprop ();
			comprop.type_ = (PropertyType) param1;
			comprop.uVal_ += 1;
			propList.Add(comprop);
			SetTiLi(comprop.type_, comprop);
		}
		
		SetBtnState();

	}
	void OnClictiliJan(ButtonScript obj, object args, int param1, int param2)
	{

		curDian++;
        if (curDian >= Inst.GetIprop(PropertyType.PT_Free))
		{
			curDian = Inst.GetIprop(PropertyType.PT_Free);
		}
		
		if (isContain((PropertyType)param1))
		{
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
//		else
//		{
//			COM_Addprop comprop = new COM_Addprop();
//			comprop.type_ = (PropertyType)param1;
//			if (comprop.uVal_ > 0)
//			{
//				comprop.uVal_ -= 1;
//			}
//			propList.Add(comprop);
//			SetTiLi(comprop.type_, comprop);
//		}
		
		SetBtnState();


//		queding.gameObject.SetActive(true);
//		//SetJiaBtnState(true);
//		if (isContain ((PropertyType)param1))
//		{
//			for (int i = 0; i<propList.Count; i++)
//			{
//				if(propList[i].type_ == (PropertyType)param1)
//				{
//					if(propList[i].uVal_>0)
//					{
//						propList[i].uVal_ -=1;
//						curDian++;
//						if (curDian >= MaxDian)
//						{
//							curDian = MaxDian;
//							//SetJianBtnState(false);
//						}
//					}
//					SetTiLi(propList[i].type_,propList[i]);
//				}
//			}
//		}else
//		{
//			COM_Addprop comprop = new COM_Addprop ();
//			comprop.type_ = (PropertyType) param1;
//			if(comprop.uVal_>0)
//			{
//				comprop.uVal_ -= 1;
//			}
//			propList.Add(comprop);
//			SetTiLi(comprop.type_,comprop);
//		}
//
//		SetBabyPropertyText (Inst);
//		SetBtnState ();
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




	int DisplayValue(PropertyType type)
	{
		for(int i=0; i < propList.Count; ++i)
		{
			if(type == propList[i].type_)
			{
				if((int)propList[i].uVal_>0)
				{
					return (int)propList[i].uVal_;
				}else
				{
					return 0;
				}
			}
			
		}
		return 0;
	}






	void SetJiaBtnState(bool isState)
	{
		tiliJa.gameObject.SetActive (isState);
		liliangJa.gameObject.SetActive (isState);
		QiangduJa.gameObject.SetActive (isState);
		suduJa.gameObject.SetActive (isState);
		mofaJa.gameObject.SetActive (isState);
	}
	
	void SetJianBtnState(bool isState)
	{
		tiliJan.gameObject.SetActive (isState);
		liliangJan.gameObject.SetActive (isState);
		QiangduJan.gameObject.SetActive (isState);
		suduJan.gameObject.SetActive (isState);
		mofaJan.gameObject.SetActive (isState);
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

            accatkLabel.text = ((int)(Inst.GetProperty(PropertyType.PT_Attack))).ToString();
            defensekLabel.text = ((int)(Inst.GetProperty(PropertyType.PT_Defense))).ToString();
            agilitykLabel.text = ((int)(Inst.GetProperty(PropertyType.PT_Agile))).ToString();
            replyLabel.text = ((int)(Inst.GetProperty(PropertyType.PT_Reply))).ToString();
            spiritLabel.text = ((int)(Inst.GetProperty(PropertyType.PT_Spirit))).ToString();

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
                        adjust = (int)(addPoints[j] + Inst.GetProperty(PropertyType.PT_Attack)) - ((int)addPoints[j] + (int)Inst.GetProperty(PropertyType.PT_Attack));
                        break;
                    case 1:
                        lbl = fangyuSim;
                        adjust = (int)(addPoints[j] + Inst.GetProperty(PropertyType.PT_Defense)) - ((int)addPoints[j] + (int)Inst.GetProperty(PropertyType.PT_Defense));
                        break;
                    case 2: 
                        lbl = minjieSim;
                        adjust = (int)(addPoints[j] + Inst.GetProperty(PropertyType.PT_Agile)) - ((int)addPoints[j] + (int)Inst.GetProperty(PropertyType.PT_Agile));
                        break;
                    case 3: 
                        lbl = huifuSim;
                        adjust = (int)(addPoints[j] + Inst.GetProperty(PropertyType.PT_Reply)) - ((int)addPoints[j] + (int)Inst.GetProperty(PropertyType.PT_Reply));
                        break;
                    case 4: 
                        lbl = jingshenSim;
                        adjust = (int)(addPoints[j] + Inst.GetProperty(PropertyType.PT_Spirit)) - ((int)addPoints[j] + (int)Inst.GetProperty(PropertyType.PT_Spirit));
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
        //    accatkLabel.text = (Inst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (Inst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (Inst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (Inst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (Inst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}
        //else
        //    if(type == PropertyType.PT_Strength)
        //{
        //    accatkLabel.text = (Inst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (Inst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (Inst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (Inst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (Inst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}
        //else
        //    if(type == PropertyType.PT_Power)
        //{
        //    accatkLabel.text = (Inst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (Inst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (Inst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (Inst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
			
        //}
        //else
        //    if(type == PropertyType.PT_Speed)
        //{
        //    accatkLabel.text = (Inst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (Inst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (Inst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (Inst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (Inst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}
        //else
        //    if(type == PropertyType.PT_Magic)
        //{
        //    accatkLabel.text = (Inst.Properties [(int)PropertyType.PT_Attack] + SimAddPoint(type, PropertyType.PT_Attack, prop)).ToString ();
        //    defensekLabel.text = (Inst.Properties[(int)PropertyType.PT_Defense] + SimAddPoint(type, PropertyType.PT_Defense, prop)).ToString();
        //    agilitykLabel.text = (Inst.Properties[(int)PropertyType.PT_Agile] + SimAddPoint(type, PropertyType.PT_Agile, prop)).ToString();
        //    replyLabel.text = (Inst.Properties[(int)PropertyType.PT_Reply] + SimAddPoint(type, PropertyType.PT_Reply, prop)).ToString();
        //    spiritLabel.text = (Inst.Properties[(int)PropertyType.PT_Spirit] + SimAddPoint(type, PropertyType.PT_Spirit, prop)).ToString();
        //}
		
		Shengyu.text = curDian.ToString();
		
		for (int i = 0; i < propList.Count; ++i)
		{
			switch (propList[i].type_)
			{
			case PropertyType.PT_Stama:
                tili.text = ((int)Inst.Properties[(int)PropertyType.PT_Stama]).ToString();
                if (propList[i].uVal_ == 0)
                    tiliSim.text = "";
                else
                    tiliSim.text = string.Format("+{0}", propList[i].uVal_);
				break;
			case PropertyType.PT_Strength:
                liliang.text = ((int)Inst.Properties[(int)PropertyType.PT_Strength]).ToString();
                if (propList[i].uVal_ == 0)
                    liliangSim.text = "";
                else
                    liliangSim.text = string.Format("+{0}", propList[i].uVal_);
				break;
			case PropertyType.PT_Power:
                Qiangdu.text = ((int)Inst.Properties[(int)PropertyType.PT_Power]).ToString();
                if (propList[i].uVal_ == 0)
                    QiangduSim.text = "";
                else
                    QiangduSim.text = string.Format("+{0}", propList[i].uVal_);
				break;
			case PropertyType.PT_Speed:
                sudu.text = ((int)Inst.Properties[(int)PropertyType.PT_Speed]).ToString();
                if (propList[i].uVal_ == 0)
                    suduSim.text = "";
                else
                    suduSim.text = string.Format("+{0}", propList[i].uVal_);
				break;
			case PropertyType.PT_Magic:
                mofa.text = ((int)Inst.Properties[(int)PropertyType.PT_Magic]).ToString();
                if (propList[i].uVal_ == 0)
                    mofaSim.text = "";
                else
                    mofaSim.text = string.Format("+{0}", propList[i].uVal_);
				break;
			}
		}
	}



	float SimAddPoint(PropertyType pointType, PropertyType effectType, COM_Addprop prop)
	{
		switch (pointType)
		{
		case PropertyType.PT_Stama:
			switch(effectType)
			{
			case PropertyType.PT_HpMax:
				return prop.uVal_ * 8f;
			case PropertyType.PT_MpMax:
				return prop.uVal_ * 1f;
			case PropertyType.PT_Attack:
				return prop.uVal_ * 0.1f;
			case PropertyType.PT_Defense:
				return prop.uVal_ * 0.1f;
			case PropertyType.PT_Agile:
				return prop.uVal_ * 0.1f;
			case PropertyType.PT_Reply:
				return prop.uVal_ * 0.8f;
			case PropertyType.PT_Spirit:
				return prop.uVal_ * -0.3f;
			}
			return 0f;
		case PropertyType.PT_Strength:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return prop.uVal_ * 2f;
			case PropertyType.PT_MpMax:
				return prop.uVal_ * 2f;
			case PropertyType.PT_Attack:
				return prop.uVal_ * 2f;
			case PropertyType.PT_Defense:
				return prop.uVal_ * 0.2f;
			case PropertyType.PT_Agile:
				return prop.uVal_ * 0.2f;
			case PropertyType.PT_Reply:
				return prop.uVal_ * -0.1f;
			case PropertyType.PT_Spirit:
				return prop.uVal_ * -0.1f;
			}
			return 0f;
		case PropertyType.PT_Power:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return prop.uVal_ * 3f;
			case PropertyType.PT_MpMax:
				return prop.uVal_ * 2f;
			case PropertyType.PT_Attack:
				return prop.uVal_ * 0.2f;
			case PropertyType.PT_Defense:
				return prop.uVal_ * 2f;
			case PropertyType.PT_Agile:
				return prop.uVal_ * 0.2f;
			case PropertyType.PT_Reply:
				return prop.uVal_ * -0.1f;
			case PropertyType.PT_Spirit:
				return prop.uVal_ * 0.2f;
			}
			return 0f;
		case PropertyType.PT_Speed:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return prop.uVal_ * 3f;
			case PropertyType.PT_MpMax:
				return prop.uVal_ * 2f;
			case PropertyType.PT_Attack:
				return prop.uVal_ * 0.2f;
			case PropertyType.PT_Defense:
				return prop.uVal_ * 0.2f;
			case PropertyType.PT_Agile:
				return prop.uVal_ * 2f;
			case PropertyType.PT_Reply:
				return prop.uVal_ * 0.2f;
			case PropertyType.PT_Spirit:
				return prop.uVal_ * -0.1f;
			}
			return 0f;
		case PropertyType.PT_Magic:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return prop.uVal_ * 1f;
			case PropertyType.PT_MpMax:
				return prop.uVal_ * 10f;
			case PropertyType.PT_Attack:
				return prop.uVal_ * 0.1f;
			case PropertyType.PT_Defense:
				return prop.uVal_ * 0.1f;
			case PropertyType.PT_Agile:
				return prop.uVal_ * 0.1f;
			case PropertyType.PT_Reply:
				return prop.uVal_ * -0.3f;
			case PropertyType.PT_Spirit:
				return prop.uVal_ * 0.8f;
			}
			return 0f;
		}
		return 0f;
	}
	void OnDestroy()
	{
		BabyProperty = null;
		GamePlayer.Instance.babyUpdateIpropEvent -= new RequestEventHandler<int>(UpdateUI);
	}

}
