using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpWayUI : MonoBehaviour
{
	public GameObject cell;
	public List<GameObject> levelBtns = new List<GameObject>();
	public UIGrid grid;

	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();
	private int _selectLevel;

	void Start ()
	{
		for(int i =0;i<levelBtns.Count;i++)
		{
			UIManager.SetButtonEventHandler (levelBtns[i].gameObject, EnumButtonEvent.OnClick, OnLevelBtns, i+1, 0);
		}
		UpdateWayList (10);
		levelBtns[0].GetComponent<UIButton>().isEnabled = true;
	}
	

	private void OnLevelBtns(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<levelBtns.Count;i++)
		{
			levelBtns[i].GetComponent<UIButton>().isEnabled = true;
		}
		obj.gameObject.GetComponent<UIButton>().isEnabled = false;

		UpdateWayList (param1*10);
	}

	public void UpdateCellList()
	{
		foreach( GameObject o in cellList)
		{
			grid.RemoveChild(o.transform);
			o.transform.parent = null;
			o.gameObject.SetActive(false);
			cellPool.Add(o);
		}
		cellList.Clear ();
	}

	private void UpdateWayList(int level)
	{

		if (_selectLevel == level)
			return;
		UpdateCellList ();
		_selectLevel = level;
		foreach(CourseData x in CourseData.GetData().Values)
		{
			if(x.level_ >= level - 9 && x.level_ <= level) 
			{
				GameObject objCell = null;
				if(cellPool.Count>0)
				{
					objCell = cellPool[0];
					cellPool.Remove(objCell);  
					UIManager.RemoveButtonAllEventHandler(objCell);
				}
				else  
				{
					objCell = Object.Instantiate(cell) as GameObject;
				}
				

				HelpWayCellUI cellUI = objCell.GetComponent<HelpWayCellUI>();
				cellUI.Course  = x;
				//UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickSkill, 0, 0);
				objCell.transform.parent = grid.transform;
				objCell.gameObject.SetActive(true);
				objCell.transform.localScale = Vector3.one;
				cellList.Add(objCell);
			}
		}
		grid.Reposition ();
	}

}

