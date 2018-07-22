using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BagUseItemUI : MonoBehaviour
{
	public UIButton closeBtn;
	public GameObject cell;
	public UIGrid grid;

	public uint stack;
	public COM_Item _item;
	public UILabel bagUseTitleLab;
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPoolList = new List<GameObject>();

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);
		bagUseTitleLab.text = LanguageManager.instance.GetValue ("bagUseTitleLab");
	}
	
	public void Show()
	{
		this.gameObject.SetActive (true);
		UpdateBaby();
	}
	
	public void Hide()
	{
		this.gameObject.SetActive (false);
	}

	private void UpdateBaby()
	{
		foreach(GameObject c in cellList)
		{
			grid.RemoveChild(c.transform);
			c.transform.parent = null;
			c.gameObject.SetActive(false);
			cellPoolList.Add(c);
		}
		cellList.Clear ();

		GameObject roleCell = null;
		if(cellPoolList.Count>0)
		{
			roleCell = cellPoolList[0];
			cellPoolList.Remove(roleCell);  
		}
		else
		{
			roleCell = Object.Instantiate(cell.gameObject) as GameObject;
		}
		UIManager.SetButtonEventHandler (roleCell, EnumButtonEvent.OnClick, OnClickCell,0, 0);
		bagUseCellUI rCell = roleCell.GetComponent<bagUseCellUI>();
		rCell.instId = GamePlayer.Instance.InstId;
		rCell.nameLab.text = GamePlayer.Instance.InstName;
		rCell.typeLab.text  = LanguageManager.instance.GetValue("role");
		rCell.hpLab.text = GamePlayer.Instance.GetIprop (PropertyType.PT_HpCurr) + "/" + GamePlayer.Instance.GetIprop (PropertyType.PT_HpMax);
		rCell.fpLab.text = GamePlayer.Instance.GetIprop (PropertyType.PT_MpCurr) + "/" + GamePlayer.Instance.GetIprop (PropertyType.PT_MpMax);
		roleCell.transform.parent = grid.transform;
		roleCell.SetActive(true);
		roleCell.transform.localScale = Vector3.one;
		cellList.Add(roleCell);

		foreach(Baby x in GamePlayer.Instance.babies_list_)
		{
			if(x == null)
				continue;
			GameObject babyCell = null;
			if(cellPoolList.Count>0)
			{
				babyCell = cellPoolList[0];
				cellPoolList.Remove(babyCell);  
			}
			else
			{
				babyCell = Object.Instantiate(cell.gameObject) as GameObject;
			}
			bagUseCellUI bCell = babyCell.GetComponent<bagUseCellUI>();
			bCell.instId = x.InstId;
			bCell.nameLab.text = x.InstName;
			bCell.typeLab.text  = LanguageManager.instance.GetValue("baobao");
			bCell.hpLab.text = x.GetIprop(PropertyType.PT_HpCurr) + "/" + x.GetIprop (PropertyType.PT_HpMax);
			bCell.fpLab.text = x.GetIprop (PropertyType.PT_MpCurr) + "/" + x.GetIprop (PropertyType.PT_MpMax);
			UIManager.SetButtonEventHandler (babyCell, EnumButtonEvent.OnClick, OnClickCell,0, 0);
			babyCell.transform.parent = grid.transform;
			babyCell.SetActive(true);
			babyCell.transform.localScale = Vector3.one;
			cellList.Add(babyCell);
		}
		grid.Reposition ();
	}

	private void OnClickCell(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.isInBattle)
		{
			Battle.Instance.UseItem((int)_item.instId_);
			BagSystem.instance.battleOpenBag = false;
			BagUI.HideMe();
		}
		else
		{
			//if(itemTabel.mainType_ == ItemMainType.IMT_Consumables)
			//{
			NetConnection.Instance.useItem ((uint)_item.slot_, (uint)obj.GetComponent<bagUseCellUI>().instId, stack);
				Hide();
			//}
		}
	}

	private void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Hide ();
	}
}

