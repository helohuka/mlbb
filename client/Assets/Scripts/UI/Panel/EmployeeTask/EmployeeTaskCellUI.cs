using UnityEngine;
using System.Collections;

public class EmployeeTaskCellUI : MonoBehaviour
{
	public UISprite icon;
	public UISprite icon1;
	public UISprite icon2;
	public UILabel timeLab;
	public UISprite pzImg;
	public UISprite pzBack;


	private int _taskId;

	void Start ()
	{

	}

	public int TaskId
	{
		set
		{
			_taskId = value;
			EmployeeQuestData qData = EmployeeQuestData.GetData(_taskId);
			if(qData ==null)
				return;
			COM_EmployeeQuestInst eQuest =  EmployeeTaskSystem.instance.GetTaskInst(_taskId);

			if(eQuest != null)
			{
				if(eQuest.status_ == EmployeeQuestStatus.EQS_Complate)
				{
					timeLab.text= qData.name_ + " " + LanguageManager.instance.GetValue("Quest_Finish");
				}
				else if(eQuest.status_ == EmployeeQuestStatus.EQS_Running)
				{
					timeLab.text= qData.name_ + " ("+FormatTimeHasHour(eQuest.timeout_)+")";
				}
				else
				{
					timeLab.text= qData.name_ + " ("+qData.timeRequier_  + LanguageManager.instance.GetValue("emplouyeetaskhour")+")";
				}
			}
			else
			{
				timeLab.text= qData.name_ + " ("+qData.timeRequier_  + LanguageManager.instance.GetValue("emplouyeetaskhour")+")";
			}

			ItemCellUI iCell =  UIManager.Instance.AddItemCellUI(icon,(uint)qData.award1_);
			iCell.showTips = true;

			ItemCellUI iCell1 =  UIManager.Instance.AddItemCellUI(icon1,(uint)qData.award2_);
			iCell1.showTips = true;


			ItemCellUI iCell2 =  UIManager.Instance.AddItemCellUI(icon2,(uint)qData.award3_);
			iCell2.showTips = true;

			if(qData.employeeQuestColor_ == EmployeeQuestColor.EQC_White)
			{
				pzImg.spriteName ="putong";
				pzBack.spriteName = "putongdi";
			}
			else if(qData.employeeQuestColor_ == EmployeeQuestColor.EQC_Blue)
			{
				pzImg.spriteName ="youxiu";
				pzBack.spriteName = "youxiudi";
			}
			else
			{
				pzImg.spriteName ="shishi";
				pzBack.spriteName = "shishidi";
			}
		}
		get
		{
			return _taskId;
		}
	}


	public  string FormatTimeHasHour(int time)
	{
		int hour = time/3600;
		int min = (time%3600)/60;
		int second = time%60;
		return DoubleTime (hour) + LanguageManager.instance.GetValue("emplouyeetaskhour") + DoubleTime (min)+LanguageManager.instance.GetValue("emplouyeetaskmin");//+ ":" + DoubleTime(second);
	}  
	
	
	public string DoubleTime(int time)
	{
		return (time > 9)?time.ToString():("0"+time);
	}

}

