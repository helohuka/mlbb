using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class familyZhufuUI : MonoBehaviour
{
	public UITexture icon;
	public UILabel nameLab;
	public UILabel levelLab;
	public UILabel maxLevelLab;
	public UIProgressBar bar;
	public UILabel barExpLab;
	public UIButton levelUpBtn;
	public List<familyZhufuCellUI> cellList = new List<familyZhufuCellUI>();
	public UILabel nowpropLab;
	public UILabel nextpropLab;
	public UILabel moneyLab;
	public UILabel maxMoneyLab;
	public UISprite barTop;
	private familyZhufuCellUI selectCellUI;
	private int _familyLearnSkillPay;
	private List<string> _icons = new List<string>();

	void Start ()
	{
		FamilySystem.instance.FamilyMyDataEvent += new RequestEventHandler<int> (OnSkillExpEnevt);
		GamePlayer.Instance.OnIPropUpdate += UpdateMoney;
		UIManager.SetButtonEventHandler (levelUpBtn.gameObject, EnumButtonEvent.OnClick, OnClickLevelUp, 0, 0);
		for(int i=0;i<cellList.Count;i++)
		{
			UIManager.SetButtonEventHandler (cellList[i].gameObject, EnumButtonEvent.OnClick, OnClickItemCell, 0, 0);
		}

		GlobalValue.Get(Constant.C_FamilyLearnSkillPay, out _familyLearnSkillPay);
		selectCellUI = cellList[0].gameObject.GetComponent<familyZhufuCellUI> ();
		selectCellUI.selectImg.gameObject.SetActive(true);
		UpdateInfo (1);
	}

	void Update ()
	{

	}

	private void OnClickItemCell(ButtonScript obj, object args, int param1, int param2)
	{
		familyZhufuCellUI cellUI = obj.gameObject.GetComponent<familyZhufuCellUI> ();
		if (cellUI == null)
			return;
		if (selectCellUI == null)
		{
			selectCellUI = cellUI;
		} 
		else
		{
			selectCellUI.selectImg.gameObject.SetActive(false);
			selectCellUI = cellUI;
		}
		cellUI.selectImg.gameObject.SetActive (true);
		UpdateInfo (cellUI.id);
	}

	private void OnClickLevelUp(ButtonScript obj, object args, int param1, int param2)
	{
		if (selectCellUI == null)
			return;
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money) < _familyLearnSkillPay)
		{
			return;
		}

	//	if(GamePlayer.Instance.GetSkillCore(selectCellUI.skillId_) == null)
		//{
		//	NetConnection.Instance.learnGuildSkill(selectCellUI.skillId_);
		//}
		//else
		//{
			NetConnection.Instance.levelupGuildSkill(selectCellUI.skillId_);
		EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_familyZhufu, gameObject.transform, () =>{});

		
		//}

	}

	private void UpdateInfo(int id)
	{
		BlessingData bData = BlessingData.GatData (id);
		if (bData == null)
			return;
		nameLab.text = bData._Name;
		HeadIconLoader.Instance.LoadIcon (bData._Icon, icon);
		if(!_icons.Contains(bData._Icon))
		{
			_icons.Add(bData._Icon);
		}

		int goddessLevel = FamilySystem.instance.GuildData.buildings_[(int)GuildBuildingType.GBT_Goddess -1].level_;
		maxLevelLab.text = (goddessLevel*2).ToString();
		COM_Skill skillInst = FamilySystem.instance.GetZhuFuSkill(bData._SkillId);
		if(skillInst == null)
		{
			SkillData sData = SkillData.GetData(bData._SkillId,1);
			if(sData == null)
				return;
			levelLab.text = LanguageManager.instance.GetValue("mainbaby_Level")+ "1";
			barExpLab.text = "0/"+sData._Proficiency;
			bar.value = 0;
			barTop.gameObject.SetActive(false);
			nowpropLab.text = LanguageManager.instance.GetValue("dangqianprop")+ sData._Desc;
			SkillData nextSkillData = SkillData.GetData(bData._SkillId,2);
			if(nextSkillData == null)
				nextpropLab.text ="";
			else
				nextpropLab.text =LanguageManager.instance.GetValue("xiajiprop")+nextSkillData._Desc;

		}
		else
		{
			SkillData sData = SkillData.GetData(bData._SkillId,(int)skillInst.skillLevel_);
			if(sData == null)
			{
				levelUpBtn.isEnabled = false; 
				return;
			}
			levelLab.text = LanguageManager.instance.GetValue("mainbaby_Level")+skillInst.skillLevel_;

			barExpLab.text = skillInst.skillExp_ +"/"+sData._Proficiency;
			barTop.gameObject.SetActive(true);
			if(skillInst.skillExp_ == 0)
			{
				barTop.gameObject.SetActive(false);
			}
			bar.value = (float)skillInst.skillExp_/(float)sData._Proficiency;
			nowpropLab.text = LanguageManager.instance.GetValue("dangqianprop")+sData._Desc;
			SkillData nextSkillData = SkillData.GetData(bData._SkillId,(int)(skillInst.skillLevel_ +1));
			if(nextSkillData == null)
				nextpropLab.text ="";
			else
				nextpropLab.text = LanguageManager.instance.GetValue("xiajiprop")+nextSkillData._Desc;


		}
		moneyLab.text  = _familyLearnSkillPay.ToString();
		maxMoneyLab.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Money).ToString();
	
		levelUpBtn.isEnabled = true;

		if(skillInst != null)
		{
			int level = FamilySystem.instance.GuildData.buildings_[(int)GuildBuildingType.GBT_Goddess -1].level_;
			if(level * 2 <= skillInst.skillLevel_)
			{
				levelUpBtn.isEnabled = false; 
			}
		}

		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money) < _familyLearnSkillPay)
		{
			levelUpBtn.isEnabled = false;
		}

	}

	private void OnSkillExpEnevt(int id)
	{
		UpdateInfo (selectCellUI.id);
	}

	public void UpdateMoney()
	{
		moneyLab.text  = _familyLearnSkillPay.ToString();
		maxMoneyLab.text = GamePlayer.Instance.GetIprop(PropertyType.PT_Money).ToString();
		UpdateInfo (selectCellUI.id);
	}

	void OnDestroy()
	{
		FamilySystem.instance.FamilyMyDataEvent -= OnSkillExpEnevt;
		GamePlayer.Instance.OnIPropUpdate -= UpdateMoney;
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}

