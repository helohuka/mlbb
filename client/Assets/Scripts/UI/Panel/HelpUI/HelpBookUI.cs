using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpBookUI : MonoBehaviour
{
	public UIButton moneeyBtn;
	public UIButton diamondBtn;
	public UIButton roleBtn;
	public UIButton babyBtn;
	public UIButton empoyeeBtn;
	public UIButton magicBtn;
	public UIButton skillBtn;
	public UIButton expBtn;
	public UIButton equipBtn;
	public UIGrid grid;
	public GameObject cell;
	public UIGrid leftGrid;
	public List<UIButton> bookBtns = new List<UIButton> ();
	public List<UIButton> stroBtns = new List<UIButton> ();

	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();
	private HelpType _type; 
	void Start ()
	{
		UIManager.SetButtonEventHandler (moneeyBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 1, 0);
		UIManager.SetButtonEventHandler (diamondBtn.gameObject, EnumButtonEvent.OnClick,OnSelectBtn, 2, 0);
		UIManager.SetButtonEventHandler (roleBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 3, 0);
		UIManager.SetButtonEventHandler (babyBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 4, 0);
		UIManager.SetButtonEventHandler (empoyeeBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 5, 0);
		UIManager.SetButtonEventHandler (skillBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 6, 0);
		UIManager.SetButtonEventHandler (expBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 7, 0);
		UIManager.SetButtonEventHandler (magicBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 8, 0);
		UIManager.SetButtonEventHandler (equipBtn.gameObject, EnumButtonEvent.OnClick, OnSelectBtn, 9, 0);

	//	foreach( UIButton s in bookBtns)
	//	{
	//		s.gameObject.SetActive(false);
	//	}
	//	//
		//foreach(UIButton b in stroBtns)
		//{
		//	b.gameObject.SetActive(false);
		//}

	}
	

	private void OnSelectBtn(ButtonScript obj, object args, int param1, int param2)
	{
		for(int i=0;i<stroBtns.Count;i++)
		{
			stroBtns[i].isEnabled = true;
		}
		for(int i=0;i<bookBtns.Count;i++)
		{
			bookBtns[i].isEnabled = true;
		}
		UpdateList ((HelpType)param1);
		obj.gameObject.GetComponent<UIButton> ().isEnabled = false;
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

	private void UpdateList(HelpType type)
	{
		if (_type == type)
			return;
		_type = type; 
		
		UpdateCellList ();
		foreach(CheatsData x in CheatsData.GetData().Values)
		{
			if(x.type_ == type) 
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
				
				
				HelpBookCellUI cellUI = objCell.GetComponent<HelpBookCellUI>();
				cellUI.Cheats  = x;
				//UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickSkill, 0, 0);
				objCell.transform.parent = grid.transform;
				objCell.gameObject.SetActive(true);
				objCell.transform.localScale = Vector3.one;
				cellList.Add(objCell);
			}
		}
		grid.Reposition ();
	}

	public void UpdateBookBtns()
	{

		foreach( UIButton s in stroBtns)
		{
			if(grid.GetIndex(s.transform) != -1)
			{
				leftGrid.RemoveChild(s.transform);
				s.transform.parent = this.gameObject.transform;
			}
				s.gameObject.SetActive(false);
			
		}

		foreach(UIButton b in bookBtns)
		{
			b.transform.parent = leftGrid.transform;
			b.gameObject.SetActive(true);
			b.transform.localScale = Vector3.one;
		}
		leftGrid.Reposition ();
		UpdateList (HelpType.HT_Money);
		for(int i=0;i<stroBtns.Count;i++)
		{
			stroBtns[i].isEnabled = true;
		}
		for(int i=0;i<bookBtns.Count;i++)
		{
			bookBtns[i].isEnabled = true;
		}
		moneeyBtn.isEnabled = false;
	}

	public void UpdateStrBtns()
	{
		
		foreach( UIButton s in bookBtns)
		{
			if(grid.GetIndex(s.transform) != -1)
			{
				leftGrid.RemoveChild(s.transform);
				s.transform.parent = this.gameObject.transform;
			}
			s.gameObject.SetActive(false);
		}
		
		foreach(UIButton b in stroBtns)
		{
			b.transform.parent = leftGrid.transform;
			b.gameObject.SetActive(true);
			b.transform.localScale = Vector3.one;
		}
		leftGrid.Reposition ();
		UpdateList (HelpType.HT_Role);
		for(int i=0;i<stroBtns.Count;i++)
		{
			stroBtns[i].isEnabled = true;
		}
		for(int i=0;i<bookBtns.Count;i++)
		{
			bookBtns[i].isEnabled = true;
		}
		roleBtn.isEnabled = false;
	}




}

