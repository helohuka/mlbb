using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeTaskUI : UIBase
{
	public UIButton CloseBtn;
	public UIGrid grid;
	public GameObject itemCell;
	public UIButton canTaskBtn;
	public UIButton haveTaskBtn;
	public GameObject infoPanel;
	public UILabel canBtnLab;
	public UILabel haveBtnLab;
	public UILabel empNum;
	public UISprite empNumBtn; 
	private int selecttype_;  

	private List<GameObject> CellList = new List<GameObject>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (canTaskBtn.gameObject, EnumButtonEvent.OnClick, OncanTaskBtn, 0, 0);
		UIManager.SetButtonEventHandler (haveTaskBtn.gameObject, EnumButtonEvent.OnClick, OnhaveTaskBtn, 0, 0);
		UIManager.SetButtonEventHandler (empNumBtn.gameObject, EnumButtonEvent.OnClick, OnEmpNumBtn, 0, 0);
		canTaskBtn.isEnabled = false;
		haveTaskBtn.isEnabled = true;
		updateTaskList ((int)EmployeeQuestStatus.EQS_None);
		EmployeeTaskSystem.instance.UpdaEmployeeTaskEnven += new RequestEventHandler<COM_EmployeeQuestInst> (OnUpdateList);
		EmployeeTaskSystem.instance.SubmEmployeeTaskEnven += new  RequestEventHandler<int> (OnSubmUpdateList);

		empNum.text = (GamePlayer.Instance.EmployeeList.Count - GamePlayer.Instance.GetBattleEmployees().Count - EmployeeTaskSystem.instance.GetTaskEmpNum ()) 
			+ "/" + GamePlayer.Instance.EmployeeList.Count;

	}
	 
	void Update ()
	{

	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_EmployeeTask);
	}
	public static void ShowMe()
	{
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_EmployeeTask);  
	}
	
	public static void HideMe()
	{ 
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_EmployeeTask);
	}

	public override void Destroyobj ()
	{

	}
	#endregion
	
	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();	
	}

	private void OncanTaskBtn(ButtonScript obj, object args, int param1, int param2)
	{
		canTaskBtn.isEnabled = false;
		haveTaskBtn.isEnabled = true;
		updateTaskList ((int)EmployeeQuestStatus.EQS_None);
	}

	private void OnhaveTaskBtn(ButtonScript obj, object args, int param1, int param2)
	{
		canTaskBtn.isEnabled = true;
		haveTaskBtn.isEnabled = false;
		updateTaskList ((int)EmployeeQuestStatus.EQS_Running);
	}

	private void OnEmpNumBtn(ButtonScript obj, object args, int param1, int param2)
	{
		EmployessControlUI.SwithShowMe ();
	}

	private void updateTaskList(int state)
	{
		selecttype_ = state;
		for (int i = 0; i < CellList.Count; ++i )
		{
			grid.RemoveChild(CellList[i].transform);
			CellList[i].transform.parent = null;
			CellList[i].gameObject.SetActive(false);
		}
		CellList.Clear ();

		List<COM_EmployeeQuestInst> questlist = EmployeeTaskSystem.instance._questlist;


		for(int i=0;i<questlist.Count;i++)
		{
			if(state == 0)
			{

				if(questlist[i].status_ != (EmployeeQuestStatus)state)
				{
					continue;
				}	
			}
			else
			{
				if(questlist[i].status_ == EmployeeQuestStatus.EQS_None)
				{
					continue;
				}
			}

			GameObject objCell = Object.Instantiate(itemCell.gameObject) as GameObject;
			EmployeeTaskCellUI cell = objCell.GetComponent<EmployeeTaskCellUI>();
			cell.TaskId = questlist[i].questId_;
			UIManager.SetButtonEventHandler (objCell.gameObject, EnumButtonEvent.OnClick, OnTaskCell, questlist[i].questId_, 0);
			objCell.transform.parent = grid.transform;
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);
		}
		grid.Reposition ();

		int runNum = EmployeeTaskSystem.instance.getTypeNum ((int)EmployeeQuestStatus.EQS_Running);
		int comNum = EmployeeTaskSystem.instance.getTypeNum ((int)EmployeeQuestStatus.EQS_Complate);
		int nNum = EmployeeTaskSystem.instance.getTypeNum ((int)EmployeeQuestStatus.EQS_None);
		canBtnLab.text = LanguageManager.instance.GetValue("Task_KeJie") + "(" + nNum+ ")";
		haveBtnLab.text = LanguageManager.instance.GetValue("Task_Picked") + "(" + (runNum+ comNum) + ")";
	}


	private void OnTaskCell(ButtonScript obj, object args, int param1, int param2)
	{
		infoPanel.gameObject.SetActive (true);
		EmployeeTaskInfoUI infoUi = infoPanel.GetComponent<EmployeeTaskInfoUI> ();
		infoUi.TaskId = param1;
	}


	void OnUpdateList(COM_EmployeeQuestInst inst)
	{
		updateTaskList (selecttype_);
		empNum.text = (GamePlayer.Instance.EmployeeList.Count - GamePlayer.Instance.GetBattleEmployees().Count - EmployeeTaskSystem.instance.GetTaskEmpNum ()) 
			+ "/" + GamePlayer.Instance.EmployeeList.Count;
	}
	void OnSubmUpdateList(int id)
	{
		updateTaskList (selecttype_);
		empNum.text = (GamePlayer.Instance.EmployeeList.Count - GamePlayer.Instance.GetBattleEmployees().Count - EmployeeTaskSystem.instance.GetTaskEmpNum ()) 
			+ "/" + GamePlayer.Instance.EmployeeList.Count;
	}

	void OnDestroy()
	{
		EmployeeTaskSystem.instance.UpdaEmployeeTaskEnven -= OnUpdateList;
		EmployeeTaskSystem.instance.SubmEmployeeTaskEnven -= OnSubmUpdateList;
	}
}

