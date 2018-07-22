using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public delegate void levelUpEvent(int instId);
public class EmployeeSkillLevelUpUI : MonoBehaviour
{
	public GameObject levelUpPane;
	public UILabel skillNameLab;
	public UITexture skillIcon;
	public UISprite itemIcon;
	public UILabel skillLevelLab;
	public UILabel itemNameLab;
	public UILabel getWayLab;
	public UILabel getWayText;
	public UIButton closeBtn;
	public UIButton levelUpBtn;
	public UIButton buyBtn;
	public UILabel skillDesc;
	public UILabel itemDesc;

	public UISprite nextDeac;
	public UILabel needText;
	public UILabel allMaoneyLab;
	public UILabel needMaoneyLab;

	private int _skillId;
	private int _itemId;
	private int _empInstId;
	
	public levelUpEvent callBack;
	private List<string> _icons = new List<string>();
	void Start ()
	{

		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (levelUpBtn.gameObject, EnumButtonEvent.OnClick, OnLevelUpBtn, 0, 0);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, 0, 0);

		GamePlayer.Instance.UpdateEmployeeSkillEnvent += new  EmpOnBattleEvent (OnEmployeeOk);
		BagSystem.instance.UpdateItemEvent += new ItemUpdateEventHandler (OnUpdateItem);
		BagSystem.instance.ItemChanged += new ItemChangedEventHandler (OnChangedItem);
        // 注册升级按钮   levelUpBtn.isenable
        GuideManager.Instance.RegistGuideAim(levelUpBtn.gameObject, GuideAimType.GAT_PartnerDetailBaseSkillLvUpBtn);
        //
        //打开技能升级界面事件
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_PartnerDetailBaseSkillOpen, levelUpBtn.isEnabled? 1: 0);
	}

    void Update()
    {
        if (GamePlayer.Instance.MyEmpCurrencyIsDirty)
        {
            int employeeCurrency = GamePlayer.Instance.GetIprop(PropertyType.PT_EmployeeCurrency);
            allMaoneyLab.text = employeeCurrency.ToString();
            GamePlayer.Instance.MyEmpCurrencyIsDirty = false;
        }
    }

	 public int ItemId
	{
		set
		{
			_itemId = value;
			ItemData data  = ItemData.GetData(_itemId);
			if(data == null)
			{   
				itemIcon.gameObject.SetActive(false);
				//buyBtn.gameObject.SetActive(false);
				//getWayText.gameObject.SetActive(false);
				itemNameLab.gameObject.SetActive(false);
				levelUpBtn.isEnabled = false;
				itemDesc.text = "";
				nextDeac.gameObject.SetActive(false);
				needText.gameObject.SetActive(false);
				return;
			}
			levelUpBtn.gameObject.SetActive(true);
			itemIcon.gameObject.SetActive(true);
			//buyBtn.gameObject.SetActive(true);
			//getWayText.gameObject.SetActive(true);
			nextDeac.gameObject.SetActive(true);
			needText.gameObject.SetActive(true);
			ItemCellUI cell = UIManager.Instance.AddItemCellUI(itemIcon,(uint)data.id_);
			cell.showTips = true;
			itemNameLab.gameObject.SetActive(true);
			itemNameLab.text = data.name_;
		//	getWayLab.text = data.acquiringWay_;
            //升级 怎么办
            //if(SkillData.GetData(_skillId).nextId_ > 0);
            //{
            //    itemDesc.text = SkillData.GetData(SkillData.GetData(_skillId).nextId_).desc_;
            //}

			if(BagSystem.instance.GetItemByItemId((uint)_itemId)!= null)
			{
				levelUpBtn.isEnabled = true;
			}
			else
			{
				levelUpBtn.isEnabled = false;
			}
		}
		get
		{
			return _itemId;
		}
	}

	

	public int SkillId
	{
		set
		{
			_skillId = value;
			int lv = 1;
			EmployeeData employeeD =  EmployeeData.GetData( GamePlayer.Instance.GetEmployeeById(_empInstId).GetIprop(PropertyType.PT_TableId));
			if(employeeD == null)
				return;
			List<COM_Skill> cskills = GamePlayer.Instance.GetEmployeeById(_empInstId).SkillInsts;
			for( int i=0; i<cskills.Count;i++)
			{
				if(cskills[i].skillID_ == value)
				{
					lv = (int)cskills[i].skillLevel_;
					skillLevelLab.text = cskills[i].skillLevel_.ToString();
					break;
				}
			}
		
			SkillData data  = SkillData.GetData(_skillId,lv);
			if(data == null)
				return;


			HeadIconLoader.Instance.LoadIcon(data._ResIconName,skillIcon);		
			if(!_icons.Contains(data._ResIconName))
			{
				_icons.Add(data._ResIconName);
			}
			skillNameLab.text =data._Name;
			skillDesc.text =data._Desc;
			int employeeCurrency = GamePlayer.Instance.GetIprop(PropertyType.PT_EmployeeCurrency);
			if(lv > employeeD.skillLevelNeedNum.Length)
			{
				levelUpBtn.isEnabled = false;
				itemDesc.text = "";
				nextDeac.gameObject.SetActive(false);
				levelUpBtn.gameObject.SetActive(false);
				needMaoneyLab.text ="";
				return;
			}

			allMaoneyLab.text = employeeCurrency.ToString();
			int needCurrency = int.Parse(employeeD.skillLevelNeedNum[lv-1]);
			needMaoneyLab.text = needCurrency.ToString(); 
			if(needCurrency <=  employeeCurrency)
			{
				levelUpBtn.gameObject.SetActive(true);
				levelUpBtn.isEnabled = true;
			}
			else
			{
				levelUpBtn.gameObject.SetActive(true);
				levelUpBtn.isEnabled = false;
			}

			SkillData nextData  = SkillData.GetData(_skillId,lv +1);
			if(nextData != null)
			{
				nextDeac.gameObject.SetActive(true);
				levelUpBtn.gameObject.SetActive(true);
				itemDesc.text = nextData._Desc;
			}
			else
			{
				itemDesc.text = "";
				nextDeac.gameObject.SetActive(false);
				getWayText.gameObject.SetActive(false);
				levelUpBtn.gameObject.SetActive(false);
				needMaoneyLab.text ="";
			}



			int slevel = int.Parse (skillLevelLab.text);
			if((GamePlayer.Instance.GetIprop(PropertyType.PT_Level)/10)+1  < slevel +1)
			{
				levelUpBtn.isEnabled = false;
			}

		}
		get
		{
			return _skillId;
		}
	}


	public int EmpInstId
	{
		set
		{
			_empInstId = value;
		}
		get
		{
			return _empInstId;
		}
	}

	public void Show()
	{

		levelUpPane.SetActive (true);
	}

	public void Hide()
	{
		if(callBack != null)
		{
			callBack(_empInstId);
		}
		levelUpPane.SetActive (false);

	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2) 
	{
		Hide ();
	}

	private void OnLevelUpBtn(ButtonScript obj, object args, int param1, int param2) 
	{
		Profession pData = Profession.get ((JobType)EmployessSystem.instance.CurEmployee.GetIprop (PropertyType.PT_Profession),
		                                   EmployessSystem.instance.CurEmployee.GetIprop (PropertyType.PT_ProfessionLevel));
		if(pData == null)
		{
			return;
		}

		int slevel = int.Parse (skillLevelLab.text);

		if (slevel >= pData.getSkilMaxLevel(SkillId)) 
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("dadazhiyegaoji"));
			return;
		}
		if((GamePlayer.Instance.GetIprop(PropertyType.PT_Level)/10)+1  < slevel +1)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("dengjibuzujineng").Replace("{n}",((slevel+1)*10-10).ToString()));
			return;
		}

		NetConnection.Instance.empSkillLevelUp((uint)EmpInstId,SkillId);

        //抛事件升成功
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_ParnterDetailBaseSkillLvUpSucc);
	}

	private void OnBuyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeBuySkillUI.ShowMe ();
	}

	void OnEmployeeOk(Employee inst,int grop)
	{
		PopText.Instance.Show (LanguageManager.instance.GetValue("employesslevelup"));
		SkillId = SkillId;
		//Hide ();
	}

	private void OnUpdateItem(COM_Item item)
	{
		SkillId = SkillId;
	}

	private void OnChangedItem(COM_Item item)
	{
		SkillId = SkillId;
	}


	void OnDestroy()
	{
        GamePlayer.Instance.UpdateEmployeeSkillEnvent -= OnEmployeeOk;
        BagSystem.instance.UpdateItemEvent -= OnUpdateItem;
		BagSystem.instance.ItemChanged -= OnChangedItem;
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}

