using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class EmployeeContext
{
    public int skId_;
    public int skLevel_;
    public int star_;
    public int color_;
};
public class EmployeeTaskSystem 
{
	public List<COM_EmployeeQuestInst> _questlist = new List<COM_EmployeeQuestInst>();
	public bool openUIEmployeeTask;
	public event RequestEventHandler<COM_EmployeeQuestInst> UpdaEmployeeTaskEnven;
	public event RequestEventHandler<int> SubmEmployeeTaskEnven;

	private uint _remainTimeStart;		 //记录重置CD开始时间.	
	private float _remainCDTime; 		 //CD 时间.

	private static EmployeeTaskSystem _instance;
	public static EmployeeTaskSystem instance
	{
		get
		{
			if(_instance == null)
				_instance = new EmployeeTaskSystem();
			return _instance;
		}
	}

    public static float CalcEmployeeQuestSuccessRate(float skLevel, float star, float color, float rate, float monsterSize)
    {
        float r = 0.0F;
        if (rate > 0.0F)
        {
            r = (skLevel + star + color * color * 0.2F) * (rate / monsterSize);
        }
        else
        {
            r = color * color / (monsterSize * 5);
        }
        return r;
    }

    public static float CalcSuccessRate(int questId,List<int> employees){
	EmployeeQuestData quest = EmployeeQuestData.GetData(questId);
    if(null == quest)
        return 0.0F;

    List<EmployeeContext> contexts = new List<EmployeeContext>();
	for(int i=0; i<employees.Count; ++i){
		Employee employee = GamePlayer.Instance.GetEmployeeById(employees[i]);
		if(null == employee){
			//ACE_DEBUG((LM_ERROR,"EmployeeQuestSystem::CalcSuccessRate [[ player(%d) not haved employee(%d) at employee quest(%d)\n",playerId,questInst.usedEmployees_[i],questInst.questId_));
			continue;
		}
		
		for(int j=0; j<employee.SkillInsts.Count; ++j){
			if(employee.SkillInsts[j] == null)
				continue;
			EmployeeContext c = new EmployeeContext();
			c.star_ = (int)employee.star_;
			c.color_ = (int)employee.quality_;
			c.skId_ = (int)employee.SkillInsts[j].skillID_;
			c.skLevel_ = (int)employee.SkillInsts[j].skillLevel_;
			contexts.Add(c); 
		}	
	}

	if(contexts.Count == 0){
	//	ACE_DEBUG((LM_ERROR,"EmployeeQuestSystem::CalcSuccessRate [[ player(%d) empty employee at employee quest(%d)\n",playerId,questInst.questId_));
		return 0.0F;
	}
	
	float maxRate = 0.0F;
	
	for(int i=0; i<quest.monsterList.Count; ++i){
		EmployeeMonsterData monster = EmployeeMonsterData.GetData(quest.monsterList[i]);
		if(null == monster){
			continue;
		}
		
		for(int k=0; k<monster.skills.Count; ++k){
			int usedIndex = -1;
			float usedRate = 0.0F;
			for(int j=0; j<contexts.Count; ++j){
                int successRate = EmployeeQuestSkillData.GetData(contexts[j].skId_, (EmployeeSkillType)monster.skills[k]);
				float rate = CalcEmployeeQuestSuccessRate(contexts[j].skLevel_,contexts[j].star_,contexts[j].color_,successRate,quest.monsterList.Count);
				if(rate > usedRate){
					usedRate = rate;   
					usedIndex = j;
				}
			}
			
			if(usedIndex != -1){ 
                contexts.RemoveAt(usedIndex); 
			}
			maxRate += usedRate;
			
		}
	}
	
	return maxRate + quest.successRate_;
}
    

	public void  InitTaskList(COM_EmployeeQuestInst[] list)
	{   
		_questlist.Clear ();
		for(int i=0;i<list.Length;i++)
		{
			_questlist.Add(list[i]);
		} 
		if(!GamePlayer.Instance.isInitEmployees)
		{
			EmployessSystem.instance.openUIEmpTask = true;                                                                                     
			NetConnection.Instance.requestEmployees();
			NetWaitUI.ShowMe();
		}
		else
		{
			EmployeeTaskUI.SwithShowMe ();
			NetWaitUI.HideMe();
		}
		SetTaskTime ();
	}

	public void UpdateTaskInst(COM_EmployeeQuestInst inst)
	{
		for(int i=0;i<_questlist.Count;i++)
		{
			if(_questlist[i].questId_ == inst.questId_)
			{
				 _questlist[i] = inst;
				break;
			}
		}
		if (UpdaEmployeeTaskEnven != null)
			UpdaEmployeeTaskEnven (inst);
		SetTaskTime ();
	}

	public void SubmitEmployeeQuestOk(int id, bool success)
	{
		if(!success)
		{
			PopText.Instance.Show( LanguageManager.instance.GetValue("employeeTaskshibao"));
			//return;
		}
		for(int i=0;i<_questlist.Count;i++)
		{
			if(_questlist[i].questId_ == id)
			{
				_questlist.Remove(_questlist[i]);
			} 
		}
		if (SubmEmployeeTaskEnven != null) 
			SubmEmployeeTaskEnven (id);
	}

	public COM_EmployeeQuestInst GetTaskInst(int id)
	{
		for(int i=0;i<_questlist.Count;i++)
		{
			if(_questlist[i].questId_ == id)
				return _questlist[i];
		}

		return null;
	}

    static float DelayTime = 1.0F;
    
    public void Update()
    {
        if (_questlist == null)
            return;

        DelayTime -= RealTime.deltaTime;
        if (DelayTime <= 0)
        {
            DelayTime += 1;
        }
        else
        {
            return;
        }

        for (int i = 0; i < _questlist.Count; i++)
        {
			if(_questlist[i].timeout_ >= 1)
			{
            	_questlist[i].timeout_ -= 1;
				if( _questlist[i].timeout_<=0 && _questlist[i].status_ == EmployeeQuestStatus.EQS_Running)
				{
					_questlist[i].status_ = EmployeeQuestStatus.EQS_Complate;
					if(UpdaEmployeeTaskEnven != null)
						UpdaEmployeeTaskEnven(_questlist[i]);
				}
			}
        }
    }

	public int getTypeNum(int state)
	{
		int num = 0;
		for(int i=0;i<_questlist.Count;i++)
		{
			if(_questlist[i].status_ == (EmployeeQuestStatus)state)
			{ 
				num++; 
			}
		}

		return num;
	}
	   

	public void SetTaskTime()
	{
		_remainTimeStart = (uint)Time.realtimeSinceStartup;
	}

	public int GetTime(int t)
	{
		return (int)Mathf.Max(0, t - (Time.realtimeSinceStartup - _remainTimeStart));
	}

	 
	public int GetTaskEmpNum()
	{
		int num = 0;
		for(int i =0;i<_questlist.Count;i++)
		{
			num += _questlist[i].usedEmployees_.Length;
		}

		return num;
	} 

	public bool IsTaskEmp(int id)
	{
		int num = 0;
		for(int i =0;i<_questlist.Count;i++)
		{ 
			for(int j= 0;j <_questlist[i].usedEmployees_.Length;j++)
			{
				if(_questlist[i].usedEmployees_[j] == id)
					return true;
			}
		}
		
		return false;
	} 


}

