using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MainbabyState : MonoBehaviour {

	public delegate void ShowBabyInfo(int []ids);
	public static ShowBabyInfo babyInfo;

	public delegate void ShowBabyChangeName(int uid,string bName);
	public static ShowBabyChangeName ShowBabyNewName;

	public delegate void ResBabyInfo(int bid);
	public static ResBabyInfo ResBabyInfoOk;

	public delegate void SetKJBtnstateEvent(bool isA);
	public static SetKJBtnstateEvent SetKJBtnstateEventOk;

	public GameObject npcObj;

	public UIButton changeNameBtn;
	public GameObject changeNameObj;

	public UISlider Expl;

	public UIButton zhanshiBtn;
	public UIButton shouhuiBtn;
	public UIButton suodingBtn;
	public UIButton jiesuoBtn;
	public GameObject dongjieObj;
	public UILabel dongjieSp;

	public GameObject objp;

	public GameObject babyMessageObj;

	public UIButton babyDel;
	//public UIButton babyHuiS;
	public UIButton babyBank;

	public UILabel  exptextLbel;
	public UILabel nameLabel;
	public UILabel loyaltyLabel;
	public UILabel levelLabel;
	//public UISlider expSlider;

	public UISprite leixngSp;

	public UILabel zhandouliLabel;

	string cur = "";
	string max = "";
	public UISlider diSlider;
	public UISlider fengSlider;
	public UISlider shuiSlider;
	public UISlider huoSlider;
	public static int babyInId;
//	public List<UISprite> fengSps = new List<UISprite>();
//	public List<UISprite> shuiSps = new List<UISprite>();
//	public List<UISprite> huoSps = new List<UISprite>();
//	public List<UISprite> diSps = new List<UISprite>();

	public List<SkillData> skillDatas = new List<SkillData>();
	public UISprite[] skillIcons;

	public UIButton kjBtn;
	public UIButton chuzhanBtn;
	public UIButton daimingBtn;

	public GameObject TipsObj;
	public UILabel TipsLabel;
	public UILabel skillLabel;
	public UILabel xiaohaoLabel;
	public UILabel levelSkillLabel;
	private string feng = "cw_feng02";
	private string shui = "cw_shui02";
	private string huo = "cw_huo02";
	private string di = "cw_di02";
	private int itemid;

	List<string> iconNames_= new List<string>();
		// Use this for initializationitemid
	void Start () {
     
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText(0);
		}
		UIManager.SetButtonEventHandler (daimingBtn.gameObject, EnumButtonEvent.OnClick, OnClickDM,0,0);
		UIManager.SetButtonEventHandler (chuzhanBtn.gameObject, EnumButtonEvent.OnClick, OnClickCZ,0,0);
		GlobalValue.Get(Constant.C_BabyExpItem, out itemid);
		UIManager.SetButtonEventHandler (babyBank.gameObject, EnumButtonEvent.OnClick, OnClickbabyBank,0, 0);
		UIManager.SetButtonEventHandler (babyDel.gameObject, EnumButtonEvent.OnClick, OnClickbabyDel,0, 0);
		UIManager.SetButtonEventHandler (kjBtn.gameObject, EnumButtonEvent.OnClick, OnClickbabykjBtn,0, 0);
		//UIManager.SetButtonEventHandler (babyHuiS.gameObject, EnumButtonEvent.OnClick, OnClickbabyHuiS,0, 0);
		UIManager.SetButtonEventHandler (changeNameBtn.gameObject, EnumButtonEvent.OnClick, OnClickchangeName,0, 0);
		UIManager.SetButtonEventHandler (zhanshiBtn.gameObject, EnumButtonEvent.OnClick, OnClickzhanshiBtn,0, 0);
		UIManager.SetButtonEventHandler (suodingBtn.gameObject, EnumButtonEvent.OnClick, OnClicksuodingBtn,0, 0);
		UIManager.SetButtonEventHandler (shouhuiBtn.gameObject, EnumButtonEvent.OnClick, OnClickshouhuiBtn,0, 0);
		UIManager.SetButtonEventHandler (jiesuoBtn.gameObject, EnumButtonEvent.OnClick, OnClickjiesuoBtn,0, 0);

		ShowBabyNewName = ChangeBabyName;
		SetKJBtnstateEventOk = SetKJBtnstate;
		babyInfo = BabyInfoShow;
		MainbabyListUI.RefreshBabyListOk += ClearText;

        shouhuiBtn.gameObject.SetActive(binst.isShow_);
        zhanshiBtn.gameObject.SetActive(!binst.isShow_);
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			kjBtn.gameObject.SetActive(false);
			chuzhanBtn.gameObject.SetActive(false);
			daimingBtn.gameObject.SetActive(false);
			babyBank.gameObject.SetActive(false);
			babyDel.gameObject.SetActive(false);
			zhanshiBtn.gameObject.SetActive(false);
			shouhuiBtn.gameObject.SetActive(false);
			changeNameBtn.gameObject.SetActive(false);
			suodingBtn.gameObject.SetActive(false);
			jiesuoBtn.gameObject.SetActive(false);
		}
	}
	private void OnClickCZ(ButtonScript obj, object args, int param1, int param2)
	{

		if((binst.GetIprop(PropertyType.PT_Level) - GamePlayer.Instance.GetIprop(PropertyType.PT_Level))>5)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("babyLvGreaterThanMy"));
			
		}else
		{
			GamePlayer.Instance.BabyState (binst.InstId, true);
			NetConnection.Instance.setBattlebaby((uint)binst.InstId, true);
			if(MainbabyState.SetKJBtnstateEventOk != null)
			{
				MainbabyState.SetKJBtnstateEventOk(true);
			}
			if(MainbabyListUI.BabyFightingStandby != null)
			{
				MainbabyListUI.BabyFightingStandby(binst.InstId,true);
			}
			daimingBtn.gameObject.SetActive(true);
			chuzhanBtn.gameObject.SetActive(false);
		}
	}
	private void OnClickDM(ButtonScript obj, object args, int param1, int param2)
	{


			GamePlayer.Instance.BabyState (binst.InstId, false);
			NetConnection.Instance.setBattlebaby((uint)binst.InstId, false);
			if(MainbabyListUI.BabyFightingStandby != null)
			{
				MainbabyListUI.BabyFightingStandby(binst.InstId,false);
			}
			if(MainbabyState.SetKJBtnstateEventOk != null)
			{
				MainbabyState.SetKJBtnstateEventOk(false);
			}
			chuzhanBtn.gameObject.SetActive(true);
			daimingBtn.gameObject.SetActive(false);
		
		
	}
	void OnEnable()
	{
		babyInfo = BabyInfoShow;
		ResBabyInfoOk = UpDateBabyInfo;
		GamePlayer.Instance.babyUpdateIpropEvent += UpDateBabyInfo;

		if(MainbabyProperty.idss!=null)
		UpDateBabyInfo (MainbabyProperty.idss[0]);
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			kjBtn.gameObject.SetActive(false);
			chuzhanBtn.gameObject.SetActive(false);
			daimingBtn.gameObject.SetActive(false);
			babyBank.gameObject.SetActive(false);
			babyDel.gameObject.SetActive(false);
			zhanshiBtn.gameObject.SetActive(false);
			shouhuiBtn.gameObject.SetActive(false);
			changeNameBtn.gameObject.SetActive(false);
			suodingBtn.gameObject.SetActive(false);
			jiesuoBtn.gameObject.SetActive(false);
		}
	}
	void OnDisable()
	{
		babyInfo = null;
		ResBabyInfoOk = null;
		GamePlayer.Instance.babyUpdateIpropEvent -= UpDateBabyInfo;
	}
	void OnClickbabyBank(ButtonScript obj, object args, int param1, int param2)
	{
		BankSystem.instance.isopen = true;
		BankControUI.ShowMe (2);
	}
	void OnClickchangeName(ButtonScript obj, object args, int param1, int param2)
	{
		//ApplicationEntry.Instance.ui3DCamera.depth = -1f;
		MainbabyListUI.babyObj.SetActive (false);
		changeNameObj.SetActive (true);
		
	}
	void OnClickshouhuiBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.showBaby(0);
        GamePlayer.Instance.ShowBaby(0);
        shouhuiBtn.gameObject.SetActive(false);
        zhanshiBtn.gameObject.SetActive(true);
		if(GamePlayer.Instance.OnShowBaby !=null)
		{
			GamePlayer.Instance.OnShowBaby(binst.InstId,false);
		}
	}
	void OnClickzhanshiBtn(ButtonScript obj, object args, int param1, int param2)
	{
	  /// <summary>
	  /// Rzhanshi	  /// </summary>
	  /// <param name="obj">Object.</param>
	  /// <param name="args">Arguments.</param>
	  /// <param name="param1">Param1.</param>
	  /// <param name="param2">Param2.</param>
        NetConnection.Instance.showBaby(binst.InstId);
        GamePlayer.Instance.ShowBaby(binst.InstId);
        shouhuiBtn.gameObject.SetActive(true);
        zhanshiBtn.gameObject.SetActive(false);
		if(GamePlayer.Instance.OnShowBaby !=null)
		{
			GamePlayer.Instance.OnShowBaby(binst.InstId,true);
		}
	}
	void OnClicksuodingBtn(ButtonScript obj, object args, int param1, int param2)
	{
		/// <summary>
		/// Rzhanshi	  /// </summary>
		/// <param name="obj">Object.</param>
		/// <param name="args">Arguments.</param>
		/// <param name="param1">Param1.</param>
		/// <param name="param2">Param2.</param>
		NetConnection.Instance.lockBaby (binst.InstId,true);
		if(MainbabyListUI.SetBabyListLockUIOk!=null)
		{
			MainbabyListUI.SetBabyListLockUIOk(binst.InstId,true);
		}
		jiesuoBtn.gameObject.SetActive(true);
		suodingBtn.gameObject.SetActive(false);
	}
	void OnClickjiesuoBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.lockBaby (binst.InstId,false);
		if(MainbabyListUI.SetBabyListLockUIOk!=null)
		{
			MainbabyListUI.SetBabyListLockUIOk(binst.InstId,false);
		}
		jiesuoBtn.gameObject.SetActive(false);
		suodingBtn.gameObject.SetActive(true);
	}
	
	void OnClickbabykjBtn(ButtonScript obj, object args, int param1, int param2)
	{

		if(binst.GetIprop(PropertyType.PT_Level)-GamePlayer.Instance.GetIprop(PropertyType.PT_Level)>5)
		{

			PopText.Instance.Show(LanguageManager.instance.GetValue("expItem"));
			return ; 
		}
		if (ShopData.GetShopId (itemid) == 0)
		{
			//PopText.Instance.Show(LanguageManager.instance.GetValue("商店没有此物品"));
			return;
		}
		int shopid = ShopData.GetShopId (itemid);
		if(BagSystem.instance.GetItemMaxNum((uint)itemid)<=0)
		{
			QuickBuyUI.ShowMe(shopid);
		}else
		{
			COM_Item item =	BagSystem.instance.GetItemByItemId((uint)itemid);
			ItemData idata = ItemData.GetData(itemid);
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaoitemwupin").Replace("{n1}","1").Replace("{n}",idata.name_),()=>{

				NetConnection.Instance.useItem((uint)item.slot_,(uint)binst.InstId,(uint)1);
			});
		}

	}
	void ClearText(int binsId)
	{
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			exptextLbel.text = "";
			nameLabel.text = "";
			loyaltyLabel.text = "";
			levelLabel.text = "";
			diSlider.value = 0;
			fengSlider.value = 0;
			shuiSlider.value = 0;
			huoSlider.value = 0;
			zhandouliLabel.text = "";
			changeNameBtn.gameObject.SetActive(false);
			babyDel.isEnabled = false;
			kjBtn.gameObject.SetActive (false);
			for (int i = 0; i < skillIcons.Length; ++i)
			{ //初始化
				skillIcons[i].gameObject.SetActive(false);
			}
		}

	}
	void OnClickbabyDel(ButtonScript obj, object args, int param1, int param2)
	{

		if (GamePlayer.Instance.GetBabyInst (babyInId).isForBattle_)
		{
      	  PopText.Instance.Show(LanguageManager.instance.GetValue("EN_BattleBabyCannotRelease"));

		}else if(GamePlayer.Instance.GetBabyInst (babyInId).isLock)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("suodingchongwu"));
		}else
		{
			babyMessageObj.SetActive(true);
			MainbabyListUI.babyObj.SetActive (false);
		}


	}
	void OnClickbabyHuiS(ButtonScript obj, object args, int param1, int param2)
	{

		objp.SetActive (true);
		
	}
	void BabyInfoShow(int []ids)
	{
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText(0);
		}else
		{
			changeNameBtn.gameObject.SetActive(true);
			babyDel.isEnabled = true;
			kjBtn.gameObject.SetActive(true);
			int uId = ids[0];
			int assId = ids [1];
			skillDatas.Clear ();
			Baby Inst = GamePlayer.Instance.GetBabyInst (uId);
			if(Inst == null)return;
			if(Inst.isForBattle_)
			{
				kjBtn.gameObject.SetActive(true);
			}else
			{
				kjBtn.gameObject.SetActive(false);
			}
			binst = Inst;
			SetBabyInfo (Inst);
			babyInId = uId;
			if(binst.isForBattle_)
			{
				chuzhanBtn.gameObject.SetActive(false);
				daimingBtn.gameObject.SetActive(true);
			}else
			{
				chuzhanBtn.gameObject.SetActive(true);
				daimingBtn.gameObject.SetActive(false);
			}
			if(binst.GetInst().isShow_)
			{
				shouhuiBtn.gameObject.SetActive(true);
				zhanshiBtn.gameObject.SetActive(false);
			}else
			{
				shouhuiBtn.gameObject.SetActive(false);
				zhanshiBtn.gameObject.SetActive(true);
			}
			if(binst.GetInst().isLock_)
			{
				jiesuoBtn.gameObject.SetActive(true);
				suodingBtn.gameObject.SetActive(false);
			}else
			{
				jiesuoBtn.gameObject.SetActive(false);
				suodingBtn.gameObject.SetActive(true);
			}
		}
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			kjBtn.gameObject.SetActive(false);
			chuzhanBtn.gameObject.SetActive(false);
			daimingBtn.gameObject.SetActive(false);
			babyBank.gameObject.SetActive(false);
			babyDel.gameObject.SetActive(false);
			zhanshiBtn.gameObject.SetActive(false);
			shouhuiBtn.gameObject.SetActive(false);
			changeNameBtn.gameObject.SetActive(false);
			suodingBtn.gameObject.SetActive(false);
			jiesuoBtn.gameObject.SetActive(false);
		}
	}




	private Baby binst;
    public void UpDateBabyInfo(int id)
    {

		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText(0);
		}else
		{  
			Baby inst = GamePlayer.Instance.GetBabyInst(id);
			if(inst == null)return;
			binst = inst;

			SetBabyInfo(inst);
			if(binst.isForBattle_)
			{
				chuzhanBtn.gameObject.SetActive(false);
				daimingBtn.gameObject.SetActive(true);
			}else
			{
				chuzhanBtn.gameObject.SetActive(true);
				daimingBtn.gameObject.SetActive(false);
			}
			if(binst.GetInst().isShow_)
			{
				shouhuiBtn.gameObject.SetActive(true);
				zhanshiBtn.gameObject.SetActive(false);
			}else
			{
				shouhuiBtn.gameObject.SetActive(false);
				zhanshiBtn.gameObject.SetActive(true);
			}
			if(binst.GetInst().isLock_)
			{
				jiesuoBtn.gameObject.SetActive(true);
				suodingBtn.gameObject.SetActive(false);
			}else
			{
				jiesuoBtn.gameObject.SetActive(false);
				suodingBtn.gameObject.SetActive(true);
			}
		}
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			kjBtn.gameObject.SetActive(false);
			chuzhanBtn.gameObject.SetActive(false);
			daimingBtn.gameObject.SetActive(false);
			babyBank.gameObject.SetActive(false);
			babyDel.gameObject.SetActive(false);
			zhanshiBtn.gameObject.SetActive(false);
			shouhuiBtn.gameObject.SetActive(false);
			changeNameBtn.gameObject.SetActive(false);
			suodingBtn.gameObject.SetActive(false);
			jiesuoBtn.gameObject.SetActive(false);
		}
    }

	UIEventListener Listener;
	private long curExp;
	private long maxExp;
	private long oldexp;
	int oldLevel;
	bool isPress;
	long oldExp = 0;
	void SetBabyInfo(Baby inst)
	{
        skillDatas.Clear();
		if(inst ==null)return;
		nameLabel.text = inst.InstName;
		loyaltyLabel.text = inst.GetIprop(PropertyType.PT_Glamour).ToString();
		levelLabel.text = inst.GetIprop(PropertyType.PT_Level).ToString();
		 
		diSlider.value = BabyData.GetData( inst.GetIprop(PropertyType.PT_TableId))._Ground/10f;
        fengSlider.value = BabyData.GetData(inst.GetIprop(PropertyType.PT_TableId))._Wind / 10f;
        shuiSlider.value = BabyData.GetData(inst.GetIprop(PropertyType.PT_TableId))._Water / 10f;
        huoSlider.value = BabyData.GetData(inst.GetIprop(PropertyType.PT_TableId))._Fire / 10f;
		leixngSp.spriteName = BabyData.GetData (inst.GetIprop (PropertyType.PT_TableId))._Tpye.ToString ();
		zhandouliLabel.text = inst.GetIprop (PropertyType.PT_FightingForce).ToString();
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
		BabyData bdata = BabyData.GetData (inst.GetIprop(PropertyType.PT_TableId));
		for (int i = 0; i<inst.SkillInsts.Count; i++)
		{
            SkillData sdata = SkillData.GetMinxiLevelData((int)inst.SkillInsts[i].skillID_);
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
            if (i > bdata._SkillNum)
                break; ///错误
			skillIcons[i].gameObject.SetActive(true);
            Transform ssp = skillIcons[i].transform.FindChild("suo000");
            ssp.gameObject.SetActive(false);
            ssp = skillIcons[i].transform.FindChild("skillicon");
            ssp.gameObject.SetActive(true);
            UITexture sp = skillIcons[i].GetComponentInChildren<UITexture>();
            iconNames_.Add(skillDatas[i]._ResIconName);
			HeadIconLoader.Instance.LoadIcon(skillDatas[i]._ResIconName, sp);
            Listener = UIEventListener.Get(skillIcons[i].GetComponent<UIButton>().gameObject);
            Listener.parameter = skillDatas[i]._Id;
            Listener.onPress = buttonPress; 
        }
		for (int i =0; i<skillIcons.Length; i++) {
			if (i < skillDatas.Count) {
					Transform ps = skillIcons [i].transform.FindChild ("Ps");
					ps.gameObject.SetActive (false);
			}
			if (i < bdata._SkillNum && i >= skillDatas.Count) {
					Transform ps = skillIcons [i].transform.FindChild ("Ps");
					ps.gameObject.SetActive (true);
					skillIcons [i].gameObject.SetActive (true);
					UIManager.SetButtonEventHandler (skillIcons [i].gameObject, EnumButtonEvent.OnClick, OnClickSkillNpc, 0, 0);

			}
			if (i >= bdata._SkillNum) {
					skillIcons [i].gameObject.SetActive (false);
			}
		}
		int Protect = 0;
		oldLevel = 0;
		GlobalValue.Get(Constant.C_AucGoodProtect, out Protect);
		//FormatTimeHasDay ((int)inst.GetInst ().lastSellTime_);
		//int day = (Protect - GlobalInstanceFunction.Instance.DayPass ((int)inst.GetInst ().lastSellTime_));
		dongjieSp.text = FormatTimeHasDay ((int)inst.GetInst ().lastSellTime_);//LanguageManager.instance.GetValue ("dongjie").Replace ("{n}",day.ToString()); 
		if (inst.GetInst ().lastSellTime_ <= 0) 
		{
			dongjieObj.SetActive(false);	
		}else
		{
			dongjieObj.SetActive(true);	
		}
	
		curExp = (long)inst.Properties[(int)PropertyType.PT_Exp];
		maxExp = ExpData.GetBabyMaxExp(inst.GetIprop(PropertyType.PT_Level));
		if(inst.GetIprop(PropertyType.PT_Level)>1)
		{
			oldLevel = (inst.GetIprop(PropertyType.PT_Level)-1);
		}else
		{
			oldExp = 0;
		}
		oldExp =  ExpData.GetBabyMaxExp(oldLevel);
		long s = curExp - oldExp;
		if(s<0)
		{
			s=0;
		}
		Expl.value = (s* 1f) / ((maxExp-oldExp)* 1f) ;
		exptextLbel.text = s + "/" + (maxExp-oldExp);


//		if(inst.GetIprop(PropertyType.PT_Level)>1)
//		{
//			oldLevel = (inst.GetIprop(PropertyType.PT_Level)-1);
//		}else
//		{
//			oldexp = 0;
//		}
//		oldexp = ExpData.GetBabyMaxExp(oldLevel);
//
//        int fExp = 0;
//        int bExp = 0;
//		if(inst.GetIprop(PropertyType.PT_Level)>1)
//		{
//            fExp = (curExp-oldexp);
//            bExp = (maxExp-oldexp);
//			exptextLbel.text = fExp + "/" + bExp;
//		}
//        else
//		{
//            fExp = curExp;
//            bExp = maxExp;
//			exptextLbel.text = curExp + "/" + maxExp;
//		}
//        Expl.value = (float)fExp / (float)bExp;
//		Expl.value = (curExp * 1f) / (maxExp * 1f);
		//Debug.Log ("curexp ==" + curExp + "   maxExp==" + maxExp + "  oldExp==" + oldexp + "  (maxExp-oldExp)" + (maxExp - oldexp));


	}
	void OnClickSkillNpc(ButtonScript obj, object args, int param1, int param2)
	{
//		if(MainbabyListUI.babyObj !=null)
//			MainbabyListUI.babyObj.SetActive (false);
		MainbabySkillNpc.ShowMe ();
		//npcObj.SetActive (true);
	}
	void buttonPress(GameObject sender,bool isPressed)
	{
		if (isPressed)
		{
			int str = (int)UIEventListener.Get (sender).parameter;
			TipsObj.SetActive(true);
			MainbabyListUI.babyObj.SetActive(false);
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
			MainbabyListUI.babyObj.SetActive(true);
		}
	}
	void ChangeBabyName(int uid,string name)
	{
		if (uid == MainbabyProperty.idss [0])
		{
			nameLabel.text = name;
		}
	
	}
	void SetKJBtnstate(bool isActive)
	{
		kjBtn.gameObject.SetActive (isActive);
	}
	void OnDestroy()
	{
		//babyInfo = null;
		ShowBabyNewName = null;
       // GamePlayer.Instance.babyUpdateIpropEvent -= UpDateBabyInfo;
		ResBabyInfoOk = null;
		UIManager.RemoveButtonEventHandler (babyDel.gameObject, EnumButtonEvent.OnClick);
		//UIManager.RemoveButtonEventHandler (babyHuiS.gameObject, EnumButtonEvent.OnClick);
		UIManager.RemoveButtonEventHandler (changeNameBtn.gameObject, EnumButtonEvent.OnClick);
		MainbabyListUI.RefreshBabyListOk -= ClearText;
        for (int i = 0; i < iconNames_.Count; ++i)
        {
            HeadIconLoader.Instance.Delete(iconNames_[i]);
        }
        iconNames_.Clear();
	}
	public  string FormatTimeHasDay(int time)
	{
		int day = time/86400;
		int min = (time%86400)/3600;
		int second = ((time%86400)%3600)/60;
		return day + ":" + DoubleTime(min) + ":" + DoubleTime(second);
	}
	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}


}
