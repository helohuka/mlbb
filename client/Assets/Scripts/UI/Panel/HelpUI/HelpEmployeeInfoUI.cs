using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpEmployeeInfoUI : UIBase
{

	public UIButton closeBtn;
	public UILabel nameLab;
	public UILabel decslab;
	public UILabel hpLab;
	public UILabel spiritLab;
	public UILabel magicLab;
	public UILabel critLab;
	public UILabel attLab;
	public UILabel hitLab;
	public UILabel defenseLab;
	public UILabel dodgeLab;
	public UILabel agileLab;
	public UILabel counterpunchLab;
	public UILabel replyLab;
	public UISprite qualityBg;
	public List<UISprite> starList = new List<UISprite> ();
	public Transform mpos;
	public UILabel qAddLab;
	public UISprite pinzhiImg;
	public UILabel profssLab;
	public UISprite jobImg;
	public UILabel battleNumLAb;

	public List<UISprite> skillIcon = new List<UISprite> ();
	private static EmployeeData _employeeData;

	private List<string> _icons = new List<string>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnCloseBtn, 0, 0);


		UpdataEmployeeInfo ();
	}
	
	#region Fixed methods for UIBase derived cass
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_HelpEmployeePanel);
	}
	
	public static void ShowMe(EmployeeData data)
	{
		_employeeData = data;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_HelpEmployeePanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_HelpEmployeePanel);
	}
	
	public override void Destroyobj ()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
	#endregion



	private void OnCloseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}

	private void OnSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		SkillData _skillData = SkillData.GetData (param1, 1);
		if (TipsSkillUI.instance != null)
		{
			TipsSkillUI.instance.setData(_skillData, transform);
		}
	}

	private void UpdataEmployeeInfo()
	{

		if (_employeeData == null)
			return;
		nameLab.text = _employeeData.name_;
		profssLab.text =  Profession.get(_employeeData.professionType_, _employeeData.jobLevel_).jobName_;
		GameManager.Instance.GetActorClone ((ENTITY_ID)_employeeData.asset_id, 
		                                    (ENTITY_ID)0, EntityType.ET_Emplyee, AssetLoadCallBack);//,"UI");
		pinzhiImg.spriteName = EmployessSystem.instance.GetQualityYuanBack ((int)_employeeData.quality_);
		jobImg.spriteName = _employeeData.professionType_.ToString ();
		decslab.text = _employeeData.desc_;


		int level = GamePlayer.Instance.GetIprop (PropertyType.PT_Level);
		int stama = level * _employeeData.stama_;
		int str = level * _employeeData.strength_;
		int power = level * _employeeData.power_;
		int speed = level * _employeeData.speed_;
		int magic = level * _employeeData.magic_;

		hpLab.text =  ((int)(stama *8 + str * 2 + power * 3 + speed * 3 + magic * 1 + _employeeData.hp_)).ToString();
		spiritLab.text = ((int)(stama *-0.3   + str * -0.1  + power * 0.2  + speed * -0.1   + magic * 0.8 + _employeeData.hp_)).ToString(); 
		magicLab.text = ((int)(stama *1 + str * 2 + power * 2 + speed * 2 + magic * 10 + _employeeData.magic_)).ToString(); 
		critLab.text = "0";
		attLab.text = ((int)(stama *0.1 + str * 2 + power *0.2 + speed * 0.2 + magic * 10 )).ToString();
		hitLab.text = "0";
		defenseLab.text = ((int)(stama *0.1 + str * 0.2 + power *2 + speed * 0.2 + magic * 0.1 )).ToString();
		dodgeLab.text = "0";
		agileLab.text = ((int)(stama *0.1 + str * 0.2 + power *0.2 + speed * 2 + magic * 0.1 )).ToString();;
		counterpunchLab.text = "0";
		replyLab.text ="100";
		battleNumLAb.text = ((int)(int.Parse (hpLab.text) * 1.25 + int.Parse (magicLab.text) * 1 + int.Parse (attLab.text) * 7.5 + int.Parse (defenseLab.text) * 5
						+ int.Parse (agileLab.text) * 5 + int.Parse (spiritLab.text) * 12.5)).ToString();
	


		for(int i =0;i< _employeeData.skills_.Count && i< 6;i++)
		{
			SkillData sData  =SkillData.GetData((int)_employeeData.skills_[i].skillID_,1);
			if(sData == null)
				continue;
			HeadIconLoader.Instance.LoadIcon("skillSuo",skillIcon[i].transform.Find("Sprite").GetComponent<UITexture>()) ;
			if(!_icons.Contains("skillSuo"))
			{
				_icons.Add("skillSuo");
			}
			skillIcon[i].gameObject.SetActive(true);
            HeadIconLoader.Instance.LoadIcon(SkillData.GetData((int)_employeeData.skills_[i].skillID_, 1)._ResIconName, skillIcon[i].transform.Find("Sprite").GetComponent<UITexture>());
            if (!_icons.Contains(SkillData.GetData((int)_employeeData.skills_[i].skillID_, 1)._ResIconName))
			{
                _icons.Add(SkillData.GetData((int)_employeeData.skills_[i].skillID_, 1)._ResIconName);
			}

			skillIcon[i].transform.Find("Sprite").GetComponent<UITexture>().gameObject.SetActive(true);

		//	UIManager.SetButtonEventHandler (skillIcon[i].gameObject, EnumButtonEvent.OnClick, OnSkillBtn, _employeeData.skills_[i], 0);
		}
	
	}

	void AssetLoadCallBack(GameObject ro, ParamData data)
	{

		ro.transform.parent = mpos;
		ro.transform.localPosition = Vector3.forward * 100f;
		ro.transform.localScale = new Vector3(400f,400f,400f);
		ro.transform.localRotation = Quaternion.Euler (0f, 180f, 0f);
		EffectLevel el =ro.AddComponent<EffectLevel>();
		el.target =ro.transform.parent.parent.GetComponent<UISprite>();
		
	}

}

