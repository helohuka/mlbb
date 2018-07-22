using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpBookCellUI : MonoBehaviour
{
	public UILabel nameLab;
	public UILabel descLab;
	public UISprite nameBack;
	public UIButton goBtn;
	public UISprite dabiaoImg;

	private CheatsData _cheatsData; 
	private string[] backImg = {"webzudidilan","webzudihuang","webzudilv"}; 

	void Start ()
	{

		UIManager.SetButtonEventHandler (goBtn.gameObject, EnumButtonEvent.OnClick, OnGoBtn, 6, 0);
	}

	
	public CheatsData Cheats
	{
		set
		{
			if(value != null)
			{
				_cheatsData = value;
				nameLab.text= _cheatsData.name_;
				descLab.text= _cheatsData.desc_;
				int i = Random.Range (0, 3);
				nameBack.spriteName = backImg[i];
				dabiaoImg.gameObject.SetActive(false);
				if(_cheatsData.uiType_ != HelpRaiseType.HRT_None || !string.IsNullOrEmpty(_cheatsData.find_))
				{
					goBtn.gameObject.SetActive (true);
					SetFinish();
				}
				else
				{
					goBtn.gameObject.SetActive (false);
				}
				if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < _cheatsData.level_)
				{
					goBtn.gameObject.SetActive (false);
				}


			}
		}
		get
		{
			return _cheatsData;
		}
	}

	private void OnGoBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (Cheats == null)
			return;
		if(!string.IsNullOrEmpty(_cheatsData.find_))
		{

			GameManager.Instance.ParseNavMeshInfo(_cheatsData.find_);
			return;
		}


		if(Cheats.uiType_ == HelpRaiseType.HRT_None)
			return;

		if(Cheats.uiType_ == HelpRaiseType.HRT_PlayerAddProp)
		{
			PlayerPropertyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_PlayerResetProp)
		{
			PlayerPropertyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_PlayerAddEvolve)
		{
			PlayerPropertyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyAddProp)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyReset)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyStr)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabySkill)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyChange)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Baby))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_SkillAuto)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Skill))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			SkillViewUI.SwithShowMe ();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_SkillItem)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Skill))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			SkillViewUI.SwithShowMe ();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EquipCompound)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Make))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			CompoundUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EquipGem)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_Make))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			CompoundUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeBuy)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_EmployeeGet))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			EmployessControlUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeePos)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_EmployeeGet))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			EmployessControlUI.SwithShowMe(2);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeSkill)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_EmployeeGet))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			EmployessControlUI.SwithShowMe(1);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeEquip)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_EmployeeGet))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			EmployessControlUI.SwithShowMe(3);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeEvolve)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_EmployeeGet))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			EmployessControlUI.SwithShowMe(3);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_MagicLevelUp)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_MagicItem))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			magicItemUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_MagicEvolve)
		{
			if(!GamePlayer.Instance.GetOpenSubSystemFlag(OpenSubSystemFlag.OSSF_MagicItem))
			{
				PopText.Instance.Show (LanguageManager.instance.GetValue("gongnengweikaiqi"));
				return;
			}
			magicItemUI.SwithShowMe();
		}

	}


	private void SetFinish()
	{
		if (Cheats == null)
			return;
		if(Cheats.uiType_ == HelpRaiseType.HRT_PlayerAddProp)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Free) <=0)
			{
				dabiaoImg.gameObject.SetActive(true);
				goBtn.gameObject.SetActive(false);
			}
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_PlayerResetProp)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Free) <=0)
			{
				//dabiaoImg.gameObject.SetActive(true);
				//goBtn.gameObject.SetActive(false);
			}
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_PlayerAddEvolve)
		{
			if(GamePlayer.Instance.GetIprop(PropertyType.PT_Free) <=0)
			{
				//dabiaoImg.gameObject.SetActive(true);
				//goBtn.gameObject.SetActive(false);
			}
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyAddProp)
		{
			foreach(Baby x in GamePlayer.Instance.babies_list_)
			{
				if(x.GetIprop(PropertyType.PT_Free) < 0)
				{
					dabiaoImg.gameObject.SetActive(false);
					goBtn.gameObject.SetActive(true);
					return;
				}
			}

			dabiaoImg.gameObject.SetActive(true);
			goBtn.gameObject.SetActive(false);

		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyReset)
		{
			//MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyStr)
		{//
			//MainbabyUI.ShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabySkill)
		{
			//MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_BabyChange)
		{
			//MainbabyUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_SkillAuto)
		{
			//MainSkillUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_SkillItem)
		{
			//MainSkillUI.SwithShowMe();
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EquipCompound)
		{
			//CompoundUI.SwithShowMe(true);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EquipGem)
		{
			//CompoundUI.SwithShowMe(true);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeBuy)
		{
			if(GamePlayer.Instance.EmployeeList.Count >= 100)
			{
				dabiaoImg.gameObject.SetActive(true);
				goBtn.gameObject.SetActive(false);
			}
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeePos)
		{
			if(!EmployessSystem.instance.GetBattleEmpty())
			{
				dabiaoImg.gameObject.SetActive(true);
				goBtn.gameObject.SetActive(false);

			}
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeSkill)
		{
			//EmployessControlUI.SwithShowMe(1);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeEquip)
		{
			//EmployessControlUI.SwithShowMe(3);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_EmployeeEvolve)
		{
			//EmployessControlUI.SwithShowMe(3);
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_MagicLevelUp)
		{
			if(GamePlayer.Instance.MagicItemLevel >= 30)
			{
				dabiaoImg.gameObject.SetActive(true);
				goBtn.gameObject.SetActive(false);
			}
		}
		else if(Cheats.uiType_ == HelpRaiseType.HRT_MagicEvolve)
		{
			if(GamePlayer.Instance.MagicItemLevel >= 30)
			{
				dabiaoImg.gameObject.SetActive(true);
				goBtn.gameObject.SetActive(false);
			}
		}
		
	}


}

