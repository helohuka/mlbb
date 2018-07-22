using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moreActivitySelfOneUI : MonoBehaviour
{

	public MoreSignInCellUI cell;
	public UIGrid grid;
	public UILabel timeLab;
	private List<GameObject> CellList = new List<GameObject>();
	void Start ()
	{
		MoreActivityData.instance.SelfChargeEveryEvent += new RequestEventHandler<COM_ADChargeEvery> (OnUpdataEvent);
		UpdateInfo ();
	}

	void  OnUpdataEvent(COM_ADChargeEvery data)
	{
		UpdateInfo ();
	}
	void UpdateInfo()
	{
		COM_ADChargeEvery data = MoreActivityData.instance.GetSelfChargeEvery ();
		if (data == null)
			return;
		string sfmt = "yyyy/MM/dd";
		string efmt = "yyyy/MM/dd";
		Define.FormatUnixTimestamp(ref sfmt, (int)data.sinceStamp_);
		Define.FormatUnixTimestamp(ref efmt, (int)data.endStamp_);
		
		timeLab.text = LanguageManager.instance.GetValue("danbichongzhitime").Replace("{n}", sfmt +" - "+  efmt);

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
			item.showTips = true;

			ItemCellUI item1 = UIManager.Instance.AddItemCellUI(cellUI.icon1,data.contents_[i].itemIds_[1]);
			item1.ItemCount = (int)data.contents_[i].itemStacks_[1];
			item1.showTips = true;
			
			ItemCellUI item2 = UIManager.Instance.AddItemCellUI(cellUI.icon2,data.contents_[i].itemIds_[2]);
			item2.ItemCount = (int)data.contents_[i].itemStacks_[2];
			item2.showTips = true;



			//item.ItemCount = (int)data.contents_[i].itemStacks_[0];
			item.showTips = true;
			cellUI.descLab.text = LanguageManager.instance.GetValue ("ADT_ChargeEvery")+" "+data.contents_[i].currencyCount_.ToString();
			UIManager.SetButtonEventHandler (cellUI.sgignInBtn.gameObject, EnumButtonEvent.OnClick, OnGetBtn, i, 0);
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
		NetConnection.Instance.requestChargeEverySingleReward((uint)param1);
	}

	void OnDestroy()
	{
		MoreActivityData.instance.SelfChargeEveryEvent -= OnUpdataEvent;
	}
}

