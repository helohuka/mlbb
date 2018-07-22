using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoreActivitySignInUI : MonoBehaviour
{
	public MoreSignInCellUI cell;
	public UIGrid grid;
	public UILabel timeLab;
	public UITexture back;
	private List<GameObject> CellList = new List<GameObject>();
	void Start ()
	{
		HeadIconLoader.Instance.LoadIcon("leijichongzhi1", back);
		MoreActivityData.instance.LoginTotalEvent += new RequestEventHandler<COM_ADLoginTotal> (OnUpdataEvent);
		UpdateInfo ();
	}

	void  OnUpdataEvent(COM_ADLoginTotal data)
	{
		UpdateInfo ();
	}

	void UpdateInfo ()
	{
		COM_ADLoginTotal loginData = MoreActivityData.instance.GetLoginTotal ();
		if (loginData == null)
			return;
		string sfmt = "yyyy/MM/dd";
		string efmt = "yyyy/MM/dd";
		Define.FormatUnixTimestamp(ref sfmt, (int)loginData.sinceStamp_);
		Define.FormatUnixTimestamp(ref efmt, (int)loginData.endStamp_);
		
		timeLab.text =timeLab.text = LanguageManager.instance.GetValue("huodongshijian") + sfmt + " - "+ efmt;
		for (int i = 0; i < CellList.Count; ++i )
		{
			grid.RemoveChild(CellList[i].transform);
			CellList[i].transform.parent = null;
			CellList[i].gameObject.SetActive(false);
		}
		CellList.Clear ();
		for(int i =0;i<loginData.contents_.Length;i++)
		{
			GameObject objCell = Object.Instantiate(cell.gameObject) as GameObject;
			MoreSignInCellUI cellUI = objCell.GetComponent<MoreSignInCellUI>();
			ItemCellUI cellItem = UIManager.Instance.AddItemCellUI(cellUI.icon,loginData.contents_[i].itemIds_[0]);
			cellItem.showTips = true;

			ItemCellUI item1 = UIManager.Instance.AddItemCellUI(cellUI.icon1,loginData.contents_[i].itemIds_[1]);
			item1.showTips = true;
			
			ItemCellUI item2 = UIManager.Instance.AddItemCellUI(cellUI.icon2,loginData.contents_[i].itemIds_[2]);
			item2.showTips = true;


			cellUI.descLab.text = LanguageManager.instance.GetValue("leijidenglutian").Replace("{n}",(i+1).ToString());
			UIManager.SetButtonEventHandler (cellUI.sgignInBtn.gameObject, EnumButtonEvent.OnClick, OnGetBtn, i, 0);
			if(loginData.contents_[i].status_ == 1)
			{
				cellUI.sgignInBtn.gameObject.SetActive(true);
				cellUI.sgignInBtn.isEnabled = true;
				cellUI.haveImg.gameObject.SetActive(false);
			}
			else if(loginData.contents_[i].status_ == 2)
			{
				cellUI.haveImg.gameObject.SetActive(true);
				cellUI.sgignInBtn.gameObject.SetActive(false);
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
		NetConnection.Instance.requestLoginTotal((uint)param1);
	}

	void OnDestroy()
	{
		MoreActivityData.instance.LoginTotalEvent -= OnUpdataEvent;
	}
}

