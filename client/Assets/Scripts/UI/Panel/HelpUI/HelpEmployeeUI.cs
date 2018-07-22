using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpEmployeeUI : MonoBehaviour
{

	public UIGrid grid;
	public GameObject cell;

	private bool _showEmployee;  

	void Start ()
	{
		
	}


	public void UpdataEmployees()
	{
		if(_showEmployee)
			return;

		foreach(EmployeeData e in EmployeeData.metaData.Values)
		{
			GameObject objCell = Object.Instantiate(cell) as GameObject;
			HelpEmployeeCellUI cellUI = objCell.GetComponent<HelpEmployeeCellUI>();
			cellUI.Employee = e;
			UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickEmployee, 0, 0);
			objCell.gameObject.SetActive(true);
			objCell.transform.parent = grid.transform;
			objCell.transform.localScale = Vector3.one;
		}
		grid.Reposition ();
		_showEmployee = true;
	}

	private void OnClickEmployee(ButtonScript obj, object args, int param1, int param2)
	{
		HelpEmployeeInfoUI.ShowMe (obj.GetComponent<HelpEmployeeCellUI>().Employee);
	}
}

