using UnityEngine;
using System.Collections;

public class BabyInfo : UIBase {

	public UILabel _BabyInfoTitle;
	public UILabel _BabyInfo;
	public UILabel _BabyInfoName;
	public UILabel _BabyInfoNum;
	public UILabel _BabyInfoTiL;
	public UILabel _BabyInfoNL;
	public UILabel _BabyInfoZL;
	public UILabel _BabyInfoRace;
	public UILabel _BabyInfoLL;
	public UILabel _BabyInfoMJ;
	public UILabel _BabyInfoSkillL;
	public UILabel _BabyInfoDI;
	public UILabel _BabyInfoSHUI;
	public UILabel _BabyInfoHUO;
	public UILabel _BabyInfoF;
	public UILabel _BabyInfoBZBtn;
	public UILabel _BabyInfoBZLand;
	public UILabel _BabyInfoNoBZ;

	public UILabel paimaihang;
	public UILabel goumaibtn;

	public Transform modelPos;

	public UILabel buzuoLabel;

	public UIButton goumaiBtn;
	public UIButton leftBtn;
	public UIButton rightBtn;
	public UIButton closeBtn;
	public UIButton BuZzhuoBtn;
	public UIButton lookSkillbtn;
	public UILabel lookskill;

	public UILabel nameLabel;
	public UILabel catchPosLabel;
	public UILabel numLabel;
	public UILabel mNameLabel;
	public UILabel PhysicalLabel;
	public UILabel EnduranceLabel;
	public UILabel intellectLabel;
	public UILabel RaceLabel;
	public UILabel skillNumLabel;
	public UILabel PowerLabel;
	public UILabel AgilityLabel;
	public UILabel raceLabel;
	public UITexture raceIcon;
	public UISprite StateSp;

	public UISlider landSlider;
	public UISlider waterSlider;
	public UISlider fireSlider;
	public UISlider windSlider;
	public UISprite[] nadusp;

	public bool isLefPre;
	public bool isRightPre;
	public GameObject[] items;
	public UIButton skillClosebtn;
	public GameObject skillObj;
	public GameObject robaby;
	private GameObject modelObj;
	private BabyData _bdata;	
	public BabyData Bdata
	{
		set
		{
			if(value != null)
			{
				_bdata = value;
				nameLabel.text = _bdata._Name;
                catchPosLabel.text = LanguageManager.instance.GetValue("Nothing");
				numLabel.text = _bdata._Id.ToString();
				mNameLabel.text = _bdata._Name;
				PhysicalLabel.text = _bdata._BIG_Stama.ToString();
				EnduranceLabel.text = _bdata._BIG_Power.ToString();
				intellectLabel.text =  _bdata._BIG_Magic.ToString();
				RaceLabel.text = LanguageManager.instance.GetValue(_bdata._RaceType.ToString());
				skillNumLabel.text = _bdata._SkillNum.ToString();
				PowerLabel.text = _bdata._BIG_Strength.ToString();
				AgilityLabel.text = _bdata._BIG_Speed.ToString();
				//raceIcon.spriteName = _bdata.RaceIcon_;
				HeadIconLoader.Instance.LoadIcon (_bdata._RaceIcon, raceIcon);
				raceLabel.text = LanguageManager.instance.GetValue(_bdata._RaceType.ToString());
			
				if(TuJianUI.IsCaptureBaby(_bdata._Id))
				{
					StateSp.gameObject.SetActive(true);
				}else
				{
					StateSp.gameObject.SetActive(false);
				}
				if(_bdata._Tpye == 2|| string.IsNullOrEmpty(_bdata._Location))
				{
					buzuoLabel.gameObject.SetActive(true);
                    buzuoLabel.text = LanguageManager.instance.GetValue("CannotCatch");
					BuZzhuoBtn.gameObject.SetActive(false);
				}else
				{
                    catchPosLabel.text = GetSceneName(_bdata._Location);
					BuZzhuoBtn.gameObject.SetActive(true);
					buzuoLabel.gameObject.SetActive(false);
					UIManager.SetButtonEventHandler (BuZzhuoBtn.gameObject, EnumButtonEvent.OnClick, OnClickBuZhuo,0, 0);
				}
				landSlider.value = _bdata._Ground/10f;
				waterSlider.value =_bdata._Water/10f;
				fireSlider.value = _bdata._Fire/10f;
				windSlider.value = _bdata._Wind/10f;;
				GameManager.Instance.GetActorClone((ENTITY_ID)Bdata._AssetsID, (ENTITY_ID)0, EntityType.ET_Baby, AssetLoadCallBack, null, "UI");




				if(GetPetProbability(_bdata._PetProbability) == 1)
				{
					nadusp[0].spriteName = "lvdian";
				}else
					if(GetPetProbability(_bdata._PetProbability) == 2)
				{
					nadusp[0].spriteName = "lvdian";
					nadusp[1].spriteName = "lvdian";
				}else
					if(GetPetProbability(_bdata._PetProbability) == 3)
				{
					nadusp[0].spriteName = "lvdian";
					nadusp[1].spriteName = "lvdian";
					nadusp[2].spriteName = "huangdian";
				}else
					if(GetPetProbability(_bdata._PetProbability) == 4)
				{
					nadusp[0].spriteName = "lvdian";
					nadusp[1].spriteName = "lvdian";
					nadusp[2].spriteName = "huangdian";
					nadusp[3].spriteName = "huangdian";

				}else
					if(GetPetProbability(_bdata._PetProbability) == 5)
				{
					nadusp[0].spriteName = "lvdian";
					nadusp[1].spriteName = "lvdian";
					nadusp[2].spriteName = "huangdian";
					nadusp[3].spriteName = "huangdian";
					nadusp[4].spriteName = "hongdian";
				}
			}
		}
		get
		{
			return _bdata;
		}
	}
	int GetPetProbability(int pro)
	{
		if(pro <101)
		{
			return 5;
		}else
			if(pro >100 && pro <1001)
		{
			return 4;
		}else
			if(pro >1000 && pro <2001)
		{
			return 3;
		}else
			if(pro >2000 && pro <5001)
		{
			return 2;
		}else
			if(pro >5000)
		{
			return 1;
		}
		return -1;
	}
    string GetSceneName(string location)
    {
        string[] parse = null;
        int sceneId;
        if (location.Contains("|"))
        {
            parse = location.Split('|');
            sceneId = int.Parse(parse[0]);
        }
        else
        {
            parse = location.Split(';');
            sceneId = int.Parse(parse[0]);
        }
        SceneData ssd = SceneData.GetData(sceneId);
        if (ssd != null)
            return ssd.sceneName_;
        return LanguageManager.instance.GetValue("Nothing");
    }

	void Start () {
		hasDestroy_ = false;
		InitUIText ();
		Bdata = BabyData.GetData (TuJianUI.babyId);
		UIManager.SetButtonEventHandler (skillClosebtn.gameObject, EnumButtonEvent.OnClick, OnClickskillClosebtn,0, 0);
		UIManager.SetButtonEventHandler (goumaiBtn.gameObject, EnumButtonEvent.OnClick, OnClickgoumai,0, 0);
		UIManager.SetButtonEventHandler (lookSkillbtn.gameObject, EnumButtonEvent.OnClick, OnClicklookSkill,0, 0);
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickclose,0, 0);
		UIEventListener.Get (leftBtn.gameObject).onPress = leftBtnPress;
		UIEventListener.Get (rightBtn.gameObject).onPress = rightBtnPress;
	}

	void InitUIText()
	{
		lookskill.text = LanguageManager.instance.GetValue ("BabyInfo_BabyInfoLookSkill");
		paimaihang.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoPaimaihang");
		goumaibtn.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoqugoumai");
		_BabyInfoTitle.text = LanguageManager.instance.GetValue("BabyInfo_Title");
		_BabyInfo.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfo");
		_BabyInfoName.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoName");
		_BabyInfoNum.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoNum");
		_BabyInfoTiL.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoTiL");
		_BabyInfoNL.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoNL");
		_BabyInfoZL.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoZL");
		_BabyInfoRace.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoRace");
		_BabyInfoLL.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoLL");
		_BabyInfoMJ.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoMJ");
		_BabyInfoSkillL.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoSkillL");
		_BabyInfoDI.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoDI");
		_BabyInfoSHUI.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoSHUI");
		_BabyInfoHUO.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoHUO");
		_BabyInfoF.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoF");
		_BabyInfoBZBtn.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoBZBtn");
		_BabyInfoBZLand.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoBZLand");
		_BabyInfoNoBZ.text = LanguageManager.instance.GetValue("BabyInfo_BabyInfoNoBZ");
	}

	private bool hasDestroy_;
	void AssetLoadCallBack(GameObject ro, ParamData data)
	{
		if(hasDestroy_)
		{
			Destroy(ro);
			return;
		}
		ro.transform.parent = modelPos;
		ro.transform.localPosition = new Vector3(0f, 0f, -400f);
		ro.transform.rotation = new Quaternion (ro.transform.rotation.x,-180,ro.transform.rotation.z,ro.transform.rotation.w);

		ro.transform.localScale =   new Vector3 (EntityAssetsData.GetData(Bdata._AssetsID).zoom_,EntityAssetsData.GetData(Bdata._AssetsID).zoom_,EntityAssetsData.GetData(Bdata._AssetsID).zoom_);
		if(robaby != null)
		{
			Destroy (robaby);
			robaby = null;
		}
		robaby = ro;
	}
	void leftBtnPress(GameObject sender, bool pre)
	{
		isLefPre = pre;
	}
	void rightBtnPress(GameObject sender,bool pre)
	{
		isRightPre = pre;
	}
	public void Update()
	{
		if(isLefPre)
		{
			robaby.transform.Rotate(Vector3.up, Mathf.Rad2Deg * 0.05f * Mathf.PI );
		}
		if(isRightPre)
		{
			robaby.transform.Rotate(Vector3.up, -(Mathf.Rad2Deg * 0.05f) * Mathf.PI );
		}
	}
	void OnClickgoumai(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_AuctionHouse))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
		AuctionHousePanel.SwithShowMe ();
	}

	void OnClicklookSkill(ButtonScript obj, object args, int param1, int param2)
	{
		skillObj.SetActive (true);
		for(int i =0;i<_bdata.skills_.Count;i++)
		{
			//UITexture tex = items[i].GetComponentInChildren<UITexture>();
			UISprite sp = items[i].GetComponent<UISprite>();
			SkillData sd = SkillData.GetMinxiLevelData((int)_bdata.skills_[i].skillID_);
			SkillCellUI scell =  UIManager.Instance.AddSkillCellUI(sp,sd);
			scell.showTips = true;
			//HeadIconLoader.Instance.LoadIcon (SkillData.GetMinxiLevelData((int)_bdata.skills_[i].skillID_)._ResIconName, tex);
		}
	}
	void OnClickskillClosebtn(ButtonScript obj, object args, int param1, int param2)
	{
		skillObj.SetActive (false);
	}
	void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
	void OnClickBuZhuo(ButtonScript obj, object args, int param1, int param2)
	{
		if(Bdata._PetProbability==0)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bunengbuzhuo"));
		}else
		{
            if (GameManager.Instance.ParseNavMeshInfo(Bdata._Location))
            {
                Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_AFP);
            }
		}

		//Traveller.Instance.ParseInfomation ();
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_BabyInfo);
	}
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_BabyInfo);
	}

	public override void Destroyobj ()
	{
		hasDestroy_ = true;
	}
}
