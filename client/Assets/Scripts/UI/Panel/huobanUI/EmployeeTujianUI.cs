using UnityEngine;
using System.Collections;

public class EmployeeTujianUI : MonoBehaviour
{
	public GameObject empCell;
	public UIGrid grid;
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
			GameObject objCell = Object.Instantiate(empCell) as GameObject;
			EmployeeTujianCellUI cellUI = objCell.GetComponent<EmployeeTujianCellUI>();
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
		HelpEmployeeInfoUI.ShowMe (obj.GetComponent<EmployeeTujianCellUI>().Employee);
	}
}


