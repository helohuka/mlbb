using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoreDiscountStoreUI : MonoBehaviour
{
	public MoreDiscountStoreCellUI cell;
	public UIGrid grid;
	public GameObject Tips;
	public UIButton buyBtn;
	public UISprite buyIcon;
	public UILabel timeLab;
	private List<GameObject> CellList = new List<GameObject>();
	void Start ()
	{
		MoreActivityData.instance.SysDiscountStoreEvent += new RequestEventHandler<COM_ADDiscountStore> (OnUpdataEvent);
		UIManager.SetButtonEventHandler (buyBtn.gameObject, EnumButtonEvent.OnClick, OnBuyBtn, 0, 0);
		UpdateInfo ();
	}

	void  OnUpdataEvent(COM_ADDiscountStore data)
	{
		UpdateInfo ();
	}


	void UpdateInfo ()
	{
		COM_ADDiscountStore data = MoreActivityData.instance.GetSysDiscountStore ();
		//COM_ADDiscountStore data = MoreActivityData.GetSysDiscountStore (); 
		if (data == null)
			return;

		string sfmt = "yyyy/MM/dd";
		string efmt = "yyyy/MM/dd";
		Define.FormatUnixTimestamp(ref sfmt, (int)data.sinceStamp_); 
		Define.FormatUnixTimestamp(ref efmt, (int)data.endStamp_);
		
		timeLab.text = LanguageManager.instance.GetValue("huodongshijian") + sfmt + " - "+ efmt;

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
			MoreDiscountStoreCellUI cellUI = objCell.GetComponent<MoreDiscountStoreCellUI>();
			ItemCellUI cellItem = UIManager.Instance.AddItemCellUI(cellUI.item,data.contents_[i].itemId_);
			cellItem.showTips = true;
			cellUI.nameLab.text = ItemData.GetData((int)data.contents_[i].itemId_).name_;
			int price = (int)data.contents_[i].price_;
			decimal discount = (decimal)(data.contents_[i].discount_ * 10f);
			decimal needM = price*discount/10;
			cellUI.needMoney.text =needM.ToString();
			cellUI.buyNumLab.text = LanguageManager.instance.GetValue("leijikemainum").Replace("{n}",data.contents_[i].buyLimit_.ToString());
			cellUI.oldMoney.text = data.contents_[i].price_.ToString();
			cellUI.saleLab.text = (data.contents_[i].discount_*10).ToString()+LanguageManager.instance.GetValue("salezhe");
			cellUI.data = data.contents_[i];
			UIManager.SetButtonEventHandler (cellUI.gameObject, EnumButtonEvent.OnClick, OnGetBtn, (int)data.contents_[i].itemId_, (int)data.contents_[i].buyLimit_);
			objCell.transform.parent = grid.transform;
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);
		}
		grid.Reposition ();
	}

	private void OnGetBtn(ButtonScript obj, object args, int param1, int param2)
	{

		Tips.SetActive (true);
		UIManager.Instance.AdjustUIDepth(Tips.transform, false);
		StoreTips stips = Tips.GetComponent<StoreTips>();
		stips.nameLabel.text = ItemData.GetData (param1).name_;
		stips.count = 1;
		stips.buyNumLab.text = "1";
		Tips.name = param1.ToString ();
		stips.maxCount = param2;
		ItemCellUI cellui = UIManager.Instance.AddItemCellUI (buyIcon, (uint)param1);
		cellui.showTips = true;
		MoreDiscountStoreCellUI cellUI = obj.gameObject.GetComponent<MoreDiscountStoreCellUI>();

		int price = (int)cellUI.data.price_;
		decimal discount = (decimal)(cellUI.data.discount_ * 10f);
		decimal needM = price*discount/10;
		
		stips.jiageLabel.text =  needM.ToString();
		stips.DesLabel.text = ":"+ItemData.GetData (param1).desc_;
	}

	private void OnBuyBtn(ButtonScript obj, object args, int param1, int param2)
	{
		StoreTips stips = Tips.GetComponent<StoreTips>();
		int count = stips.count;

		MessageBoxUI.ShowMe (LanguageManager.instance.GetValue ("shopbuyitem").Replace ("{n}", count .ToString ()).Replace ("{n1}", stips.nameLabel.text).Replace ("{n2}", (count * int.Parse(stips.jiageLabel.text)).ToString ()
		                                                                                                                                                           + LanguageManager.instance.GetValue ("shuijing")), () => {

						NetConnection.Instance.buyDiscountStore (int.Parse (Tips.name), count);});
	}

	void OnDestroy()
	{
		MoreActivityData.instance.SysDiscountStoreEvent -= OnUpdataEvent;
	}
}

