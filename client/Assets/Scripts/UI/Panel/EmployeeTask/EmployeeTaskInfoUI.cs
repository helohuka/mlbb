using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeTaskInfoUI : MonoBehaviour
{
	public UIButton closeBtn;
	public UIButton startBtn;
	public UILabel nameLab;
	public UISprite reward0;
	public UISprite reward1;
	public UISprite reward2;
	public UILabel SuccessRate0;
	public UILabel SuccessRate1;
	public UILabel SuccessRate2;
	public GameObject monster0;
	public UITexture monsterIcon0;  
	public UITexture monsterSkill0;  
	public UITexture monsterSkill1;  

	public GameObject monster1;
	public UITexture monsterIcon1;
	public UITexture monsterSkill01;  
	public UITexture monsterSkill02;

	public GameObject monster2;
	public UITexture monsterIcon2;
	public UITexture monsterSkill11;  
	public UITexture monsterSkill12;
	public UIGrid employeeListGuid;
	public GameObject employeeListCell;
	private List<GameObject> CellList = new List<GameObject>();
	public List<GameObject> employeeBattleList = new List<GameObject> ();
	public GameObject startObj;
	public GameObject submitObj;
	public UIButton submitBtn;
	public GameObject runingObj;
	public UILabel timeLab;
	public UIButton closeBtn1;
	public UILabel empNumLab;
	public UISprite pzImg;
	public UISprite pzBack;
	public UILabel needMoneyLab;
	public UISprite skillTips;
	public UILabel tipsSkillName;
	public UILabel tipsSkillDesc;
	public UILabel canEmpNum;

	private List<int> empTaskList = new List<int> (); 
	private int _taskId;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (closeBtn1.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (startBtn.gameObject, EnumButtonEvent.OnClick, OnStart, 0, 0);
		UIManager.SetButtonEventHandler (submitBtn.gameObject, EnumButtonEvent.OnClick, OnSubmit, 0, 0);
		UIManager.SetButtonEventHandler (skillTips.gameObject, EnumButtonEvent.OnClick, OnTips, 0, 0);
		EmployeeTaskSystem.instance.UpdaEmployeeTaskEnven += new RequestEventHandler<COM_EmployeeQuestInst>(UpdateInfo);
		EmployeeTaskSystem.instance.SubmEmployeeTaskEnven += new  RequestEventHandler<int> (OnSubmUpdateList);


		for(int i=0;i<employeeBattleList.Count;i++)
		{
			UIManager.SetButtonEventHandler (employeeBattleList[i].gameObject, EnumButtonEvent.OnClick, OnEmpOff, 0, 0);
		}
		empNumLab.text = (GamePlayer.Instance.EmployeeList.Count - GamePlayer.Instance.GetBattleEmployees().Count - EmployeeTaskSystem.instance.GetTaskEmpNum ()) 
			+ "/" + GamePlayer.Instance.EmployeeList.Count;

	}

	void Update ()
	{
        int sec = EmployeeTaskSystem.instance.GetTaskInst (_taskId).timeout_;
        if (sec > 0)
            timeLab.text = FormatTimeHasHour(sec);
		else
			timeLab.text = FormatTimeHasHour(0);
	}

	public  string FormatTimeHasHour(int time)
	{
		int hour = time/3600;
		int min = (time%3600)/60;
		int second = time%60;
		return DoubleTime(hour) + ":" + DoubleTime(min) + ":" + DoubleTime(second);
	}  
	
	
	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0" + time);
	}


	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{ 
		gameObject.SetActive (false);
	} 

	private void OnSubmit(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.	submitEmployeeQuest(_taskId);
	}

	private void OnStart(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeQuestData qData = EmployeeQuestData.GetData(_taskId);
		if(qData == null)
			return;

		if(empTaskList.Count < qData.employeeRequier_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("renshubuzhu"));
			return; 
		}
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Money) < qData.needMoney_)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("nomoney"));
			return;
		}
		NetConnection.Instance.acceptEmployeeQuest (_taskId,empTaskList.ToArray());
	}
	private void OnTips(ButtonScript obj, object args, int param1, int param2)
	{
		skillTips.gameObject.SetActive (false);
	}

	private void OnMonsterSkill(ButtonScript obj, object args, int param1, int param2)
	{
		skillTips.gameObject.SetActive (true);
		skillTips.transform.localPosition = new Vector3 (obj.transform.parent.transform.localPosition.x, skillTips.transform.localPosition.y, 0);
		tipsSkillName.text = EmployeeMonsterSkillData.GetData (param1).name_;
		tipsSkillDesc.text = EmployeeMonsterSkillData.GetData (param1).desc;
	}


	public void UpdateEmpList()
	{
		for (int i = 0; i < CellList.Count; ++i ) 
		{
			employeeListGuid.RemoveChild(CellList[i].transform);
			CellList[i].transform.parent = null;
			CellList[i].gameObject.SetActive(false);
		}
		CellList.Clear ();
		
		List<Employee> employeeList = GamePlayer.Instance.EmployeeList;
		for(int i =0;i<employeeList.Count;i++)
		{
			//if(employeeList[i].isForBattle_)
			//	continue;
			if(EmployeeTaskSystem.instance.IsTaskEmp(employeeList[i].InstId))
				continue;
			GameObject objCell = Object.Instantiate(employeeListCell.gameObject) as GameObject;
			UIManager.SetButtonEventHandler (objCell.gameObject, EnumButtonEvent.OnClick, OnEmpOn, employeeList[i].InstId, 0);
			EmployeeTaskInfoCell cell = objCell.GetComponent<EmployeeTaskInfoCell>();
			cell.monster = EmployeeQuestData.GetData(_taskId).monsterList;
			cell.EmpData =employeeList[i]; 

			objCell.transform.parent = employeeListGuid.transform;
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);
		}
		employeeListGuid.Reposition ();
	}


	private void OnEmpOn(ButtonScript obj, object args, int param1, int param2)
	{
		EmployeeQuestData qData = EmployeeQuestData.GetData(_taskId);
		if(empTaskList.Count >= qData.employeeRequier_)
		{
			return;
		}
		if (empTaskList.Contains (param1))
		{
			return;
		}
		empTaskList.Add (param1);
		UpdateBattleEmp (empTaskList);
		SetSuccessRate (TaskId,empTaskList);
	}

	private void OnEmpOff(ButtonScript obj, object args, int param1, int param2)
	{
		COM_EmployeeQuestInst inst = EmployeeTaskSystem.instance.GetTaskInst(_taskId);
		if (inst.status_ == EmployeeQuestStatus.EQS_Running || inst.status_ == EmployeeQuestStatus.EQS_Complate)
			return;
		EmployeeTaskBattleCell  bCellUI = obj.gameObject.GetComponent<EmployeeTaskBattleCell>();
		if(empTaskList.Contains(bCellUI.EmployeeInstId))
		{
			empTaskList.Remove(bCellUI.EmployeeInstId);
		}
		UpdateBattleEmp (empTaskList);
		SetSuccessRate (TaskId,empTaskList);
	}

	public int TaskId
	{
		set
		{
			_taskId = value;
			EmployeeQuestData qData = EmployeeQuestData.GetData(_taskId);
			if(qData == null)
				return;
			nameLab.text = qData.name_;
			ItemCellUI cell0 =  UIManager.Instance.AddItemCellUI( reward0,(uint)qData.award1_);
			cell0.showTips = true;
			ItemCellUI cell1 =  UIManager.Instance.AddItemCellUI( reward1,(uint)qData.award2_);
			cell1.showTips = true;
			ItemCellUI cell2 =  UIManager.Instance.AddItemCellUI( reward2,(uint)qData.award3_);
			cell2.showTips = true;

			needMoneyLab.text = qData.needMoney_.ToString(); 

			if(qData.employeeQuestColor_ == EmployeeQuestColor.EQC_White)
			{
				pzImg.spriteName ="putong";
				pzBack.spriteName = "webzudilv";
			}
			else if(qData.employeeQuestColor_ == EmployeeQuestColor.EQC_Blue)
			{
				pzImg.spriteName ="youxiu";
				pzBack.spriteName = "webzudidilan";
			} 
			else
			{
				pzImg.spriteName ="shishi";
				pzBack.spriteName = "webzudihong";
			}
			canEmpNum.text= LanguageManager.instance.GetValue("employeeTaskCanNum").Replace("{n}",qData.employeeRequier_.ToString()) ;
			COM_EmployeeQuestInst inst = EmployeeTaskSystem.instance.GetTaskInst(_taskId);
			UpdateInfo(inst);

			EmployeeMonsterData mData = EmployeeMonsterData.GetData(qData.employeeMonster_);
			if(mData == null)
				return;
			HeadIconLoader.Instance.LoadIcon(mData.icon_, monsterIcon0);

			EmployeeMonsterSkillData mskillData = EmployeeMonsterSkillData.GetData((int)mData.skill_0+1);
			if(mskillData == null)
			{
				monsterSkill0.gameObject.SetActive(false);
			}
			else
			{
				monsterSkill0.gameObject.SetActive(true);
				UIManager.SetButtonEventHandler (monsterSkill0.gameObject, EnumButtonEvent.OnClick, OnMonsterSkill, mskillData.id_, 0);
				HeadIconLoader.Instance.LoadIcon(mskillData.icon, monsterSkill0);
			}

			EmployeeMonsterSkillData mskillData1 = EmployeeMonsterSkillData.GetData((int)mData.skill_1+1);
			if(mskillData1 == null)
			{
				monsterSkill1.gameObject.SetActive(false);
			}
			else
			{
				monsterSkill1.gameObject.SetActive(true);
				UIManager.SetButtonEventHandler (monsterSkill1.gameObject, EnumButtonEvent.OnClick, OnMonsterSkill, mskillData1.id_, 0);
				HeadIconLoader.Instance.LoadIcon(mskillData1.icon, monsterSkill1);
			}



			if(qData.employeeMonster_1 != 0)
			{
				mData = EmployeeMonsterData.GetData(qData.employeeMonster_1);
				if(mData == null)
					return;
				HeadIconLoader.Instance.LoadIcon(mData.icon_, monsterIcon1);
				
				mskillData = EmployeeMonsterSkillData.GetData((int)mData.skill_0+1);
				if(mskillData == null)
				{
					monsterSkill01.gameObject.SetActive(false);
				}
				else
				{
					monsterSkill01.gameObject.SetActive(true);
					UIManager.SetButtonEventHandler (monsterSkill01.gameObject, EnumButtonEvent.OnClick, OnMonsterSkill, mskillData.id_, 0);
					HeadIconLoader.Instance.LoadIcon(mskillData.icon, monsterSkill01);
				}
				monster1.SetActive(true);
				mskillData1 = EmployeeMonsterSkillData.GetData((int)mData.skill_1+1);
				if(mskillData1 == null)
				{
					monsterSkill02.gameObject.SetActive(false);
				}
				else
				{
					monsterSkill02.gameObject.SetActive(true);
					UIManager.SetButtonEventHandler (monsterSkill02.gameObject, EnumButtonEvent.OnClick, OnMonsterSkill, mskillData1.id_, 0);
					HeadIconLoader.Instance.LoadIcon(mskillData1.icon, monsterSkill02);
				}
			}
			else
			{
				monster1.SetActive(false);
			}

			if(qData.employeeMonster_2 != 0)
			{
				mData = EmployeeMonsterData.GetData(qData.employeeMonster_2);
				if(mData == null) 
					return;
				HeadIconLoader.Instance.LoadIcon(mData.icon_, monsterIcon2);
				
				mskillData = EmployeeMonsterSkillData.GetData((int)mData.skill_0+1);
				if(mskillData == null)
				{
					monsterSkill11.gameObject.SetActive(false);
				}
				else
				{
					monsterSkill11.gameObject.SetActive(true);
					UIManager.SetButtonEventHandler (monsterSkill11.gameObject, EnumButtonEvent.OnClick, OnMonsterSkill, mskillData.id_, 0);
					HeadIconLoader.Instance.LoadIcon(mskillData.icon, monsterSkill11);
				}
				monster2.SetActive(true);
				mskillData1 = EmployeeMonsterSkillData.GetData((int)mData.skill_1+1);
				if(mskillData1 == null)
				{
					monsterSkill12.gameObject.SetActive(false);
				}
				else
				{
					monsterSkill12.gameObject.SetActive(true);
					UIManager.SetButtonEventHandler (monsterSkill12.gameObject, EnumButtonEvent.OnClick, OnMonsterSkill, mskillData1.id_, 0);
					HeadIconLoader.Instance.LoadIcon(mskillData1.icon, monsterSkill12);
				}
			}
			else
			{
				monster2.SetActive(false);
			}


			UpdateEmpList ();
		} 
		get
		{
			return _taskId;
		}
	}


	public void UpdateInfo(COM_EmployeeQuestInst inst)
	{
		if(inst == null)
			return;
		if(inst.status_ == EmployeeQuestStatus.EQS_None)
		{
			startObj.gameObject.SetActive(true);
			submitObj.gameObject.SetActive(false);
			runingObj.gameObject.SetActive(false);
		}
		else if(inst.status_ == EmployeeQuestStatus.EQS_Running)
		{
			startObj.gameObject.SetActive(false);
			submitObj.gameObject.SetActive(false);
			runingObj.gameObject.SetActive(true);
		}
		else
		{
			startObj.gameObject.SetActive(false);
			submitObj.gameObject.SetActive(true);
			runingObj.gameObject.SetActive(false);
		}
		
		
		for(int j=0;j<employeeBattleList.Count;j++)
		{
			employeeBattleList[j].SetActive(false);
		}
		
		empTaskList.Clear();
		for(int i=0;i<inst.usedEmployees_.Length;i++)
		{
			if(inst.usedEmployees_[i] != 0)
			{
				EmployeeTaskBattleCell  bCellUI = employeeBattleList[i].GetComponent<EmployeeTaskBattleCell>();
				bCellUI.EmployeeInstId = inst.usedEmployees_[i];
				employeeBattleList[i].SetActive(true);
				empTaskList.Add(inst.usedEmployees_[i]);
			}
			
		}
		SetSuccessRate (inst.questId_,empTaskList); 

		UpdateEmpList ();
	}

	void OnSubmUpdateList(int id)
	{
		gameObject.SetActive (false);	
	}

	private void UpdateBattleEmp(List<int> empList)
	{
		for(int j=0;j<employeeBattleList.Count;j++)
		{
			employeeBattleList[j].SetActive(false);
		}
		for(int i=0;i<empList.Count;i++)
		{
			EmployeeTaskBattleCell  bCellUI = employeeBattleList[i].GetComponent<EmployeeTaskBattleCell>();
			bCellUI.EmployeeInstId = empList[i];
			employeeBattleList[i].SetActive(true);
		}
	}

	private void SetSuccessRate(int id ,List<int> emps)
	{

		if(emps.Count == 0)
		{
			EmployeeQuestData qData = EmployeeQuestData.GetData(_taskId);
			SuccessRate0.text = qData.successRate_+"%";
			SuccessRate1.text = "0%";
			SuccessRate2.text = "0%";
			return;
		}


		int rate = (int)EmployeeTaskSystem.CalcSuccessRate (id, emps);

		SuccessRate0.text = rate+"%";
		if(rate - 100 > 0)
			SuccessRate1.text = (rate - 100)+"%";
		else
			SuccessRate1.text = "0%";
		if(rate - 200 > 0)
			SuccessRate2.text = (rate-200)+"%";
		else
			SuccessRate2.text = "0%";

		empNumLab.text = (GamePlayer.Instance.EmployeeList.Count - GamePlayer.Instance.GetBattleEmployees().Count - EmployeeTaskSystem.instance.GetTaskEmpNum ()) 
			+ "/" + GamePlayer.Instance.EmployeeList.Count;

	}

	void OnDestroy()
	{
		EmployeeTaskSystem.instance.UpdaEmployeeTaskEnven -= UpdateInfo;
		EmployeeTaskSystem.instance.SubmEmployeeTaskEnven -= OnSubmUpdateList;
	}




}

