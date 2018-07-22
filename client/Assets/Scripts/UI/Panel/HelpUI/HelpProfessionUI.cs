using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class HelpProfessionUI : MonoBehaviour
{

	public UIGrid grid;
	public GameObject cell;
	public UILabel numLab;
	public UIButton addBtn;
	public UIButton subbtn;
	private bool isInit;
	private int selectPorfess  = 2;
	private List<GameObject> listCell = new List<GameObject>();

	public HelpProfessionInfoUI infoUI;

	void Start ()
	{
		UIManager.SetButtonEventHandler (addBtn.gameObject, EnumButtonEvent.OnClick, OnAddBtn, 0, 0);
		UIManager.SetButtonEventHandler (subbtn.gameObject, EnumButtonEvent.OnClick, OnSubBtn, 0, 0);
	}


	public void UpdataProfession()
	{
		if (isInit)
			return;
		isInit = true;
		for(int i= 0;i<3;i++)
		{
			GameObject objCell = Object.Instantiate(cell) as GameObject;
			HelpProfessionCellUI cellUI= objCell.GetComponent<HelpProfessionCellUI>();
			cellUI.Porfession = selectPorfess + i;
			objCell.gameObject.SetActive(true);
			objCell.transform.parent = grid.transform;
			objCell.transform.localScale = Vector3.one;
			listCell.Add(objCell);

			for(int j = 0;j<cellUI.porfessCell.Count;j++)
			{
				UIManager.SetButtonEventHandler (cellUI.porfessCell[j].gameObject, EnumButtonEvent.OnClick, OnPorfessCell, j, 0);
			}

		}
		grid.Reposition ();
		numLab.text = (selectPorfess/3+1)+"/4";//+(int)JobType.JT_Word /3; 
	}


	private void OnAddBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(selectPorfess+3 <= (int)JobType.JT_Word)
			selectNum += 3;

		
	}
 
	private void OnSubBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (selectPorfess - 3 >= (int)JobType.JT_Axe)
			selectNum -= 3;

	}

	private int selectNum
	{

		set
		{
			selectPorfess = value;
			if( selectPorfess >= 10)
			{
				numLab.text = "4/4";
				for(int i = 0;i< listCell.Count;i++)
				{
					HelpProfessionCellUI cellUI= listCell[0].GetComponent<HelpProfessionCellUI>();
					cellUI.Porfession = 11;
				}
				listCell[1].gameObject.SetActive(false);
				listCell[2].gameObject.SetActive(false);
				return;
			}

			numLab.text = (selectPorfess/3+1)+"/4";//(int)JobType.JT_Word /3; 
			listCell[1].gameObject.SetActive(true);
			listCell[2].gameObject.SetActive(true);
			for(int i = 0;i< listCell.Count;i++)
			{
				HelpProfessionCellUI cellUI= listCell[i].GetComponent<HelpProfessionCellUI>();
				cellUI.Porfession = selectPorfess + i;
			}
		}
		get
		{
			return selectPorfess;
		}
	}


	private void OnPorfessCell(ButtonScript obj, object args, int param1, int param2)
	{
		infoUI.gameObject.SetActive (true);
		Profession  pData = Profession.get((JobType)int.Parse(obj.name),param1+1);
		infoUI.professData = pData;
	}




}

