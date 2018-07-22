using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoreActivityBuyEmployeeUI : MonoBehaviour
{
	public MoreSignInCellUI cell;
	public UIGrid grid;
	public UILabel timeLab;
	public UITexture back;
	private List<GameObject> CellList = new List<GameObject>();
	void Start ()
	{
		HeadIconLoader.Instance.LoadIcon("leijichongzhi1", back);
		MoreActivityData.instance.employeeTotalEvent += new RequestEventHandler<COM_ADEmployeeTotal> (OnUpdataEvent);
		UpdateInfo ();
	}
	
	void  OnUpdataEvent(COM_ADEmployeeTotal data)
	{
		UpdateInfo ();
	}
	void UpdateInfo()
	{
		COM_ADEmployeeTotal data = MoreActivityData.instance.GetEmployeeTotal ();
		if (data == null)
			return;
		string sfmt = "yyyy/MM/dd";
		string efmt = "yyyy/MM/dd";
		Define.FormatUnixTimestamp(ref sfmt, (int)data.sinceStamp_);
		Define.FormatUnixTimestamp(ref efmt, (int)data.endStamp_);
		
		timeLab.text = LanguageManager.instance.GetValue("zaomuchongzhitime").Replace("{n}", sfmt +" - "+ efmt);
		
		for (int i = 0; i < CellList.Count; ++i )
		{
			grid.RemoveChild(CellList[i].transform);
			CellList[i].transform.parent = null;
			CellList[i].gameObject.SetActive(false);
		}
		CellList.Clear ();
		
		for(int i =0;i<data.contents_.Length;i++)
		{
			GameObject objCell = Object.Instantiate(cell.gameObject) as GameObject;
			MoreSignInCellUI cellUI = objCell.GetComponent<MoreSignInCellUI>();
			ItemCellUI item = UIManager.Instance.AddItemCellUI(cellUI.icon,data.contents_[i].itemIds_[0]);
			item.showTips =true;
			item.ItemCount = (int)data.contents_[i].itemStacks_[0];
			UIManager.SetButtonEventHandler (cellUI.sgignInBtn.gameObject, EnumButtonEvent.OnClick, OnGetBtn, i, 0);
			cellUI.descLab.text = LanguageManager.instance.GetValue("morebuyEmpcell").Replace("{n}",((data.contents_[i].outputCount_).ToString()));
			if(data.contents_[i].status_ == 1)
			{
				cellUI.sgignInBtn.gameObject.SetActive(true);
				cellUI.sgignInBtn.isEnabled = true;
				cellUI.haveImg.gameObject.SetActive(false);
			}
			else if(data.contents_[i].status_ == 2)
			{
				cellUI.sgignInBtn.gameObject.SetActive(false);
				cellUI.haveImg.gameObject.SetActive(true);
			}
			else
			{
				cellUI.sgignInBtn.gameObject.SetActive(true);
				cellUI.sgignInBtn.isEnabled = false;
				cellUI.haveImg.gameObject.SetActive(false);
			}
			objCell.transform.parent = grid.transform;
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);
		}
		grid.Reposition ();
	}
	private void OnGetBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.requestEmployeeActivityReward((uint)param1);
	}
	
	
	void OnDestroy()
	{
		MoreActivityData.instance.employeeTotalEvent -= OnUpdataEvent;
	}
}

