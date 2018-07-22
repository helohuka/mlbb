using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoreRechargeUI : MonoBehaviour
{
	public UIProgressBar bar;
	public UILabel nowMoneyLab;
	public UILabel nowMoneyLab1;
	public List<UISprite> boxList = new List<UISprite>();
	public List<UILabel> boxlabList = new List<UILabel>();
	public List<UISprite> reword = new List<UISprite>();
	public List<UILabel> rewordName = new List<UILabel>();
	public UIButton getBtn;
	public UIButton leftBtn;
	public UIButton rightBtn;
	public UISprite barImg;
	public UISprite needMoneyImg;
	public UISprite rewardImg;

	int nowBoxNum ;
	private List<rechargeAllData> rechargeData = new List<rechargeAllData> ();
	private COM_ADChargeTotal chargeTotalData = null;

	void Start ()    
	{

		UIManager.SetButtonEventHandler (leftBtn.gameObject, EnumButtonEvent.OnClick, OnLeftBtn, 0, 0);
		UIManager.SetButtonEventHandler (rightBtn.gameObject, EnumButtonEvent.OnClick, OnRightBtn, 0, 0);
		UIManager.SetButtonEventHandler (getBtn.gameObject, EnumButtonEvent.OnClick, OnGetBtn, 0, 0);
		GamePlayer.Instance.getFanliEnvet += new RequestEventHandler<COM_ADChargeTotal> (fanliEnvet);

		rechargeData = rechargeAllData.rechargeList;
		nowBoxNum = 0;
		UpdateInfo ();
	}

	private void UpdateInfo()
	{
		chargeTotalData = GamePlayer.Instance.myselfRecharge_;
		for(int i=0;i<rechargeData.Count && i<7;i++)
		{
			boxlabList[i].text = rechargeData[i].num_.ToString();
			if(chargeTotalData.recharge_ >= rechargeData[i].num_)
			{
				nowBoxNum = i;
			}
			if(chargeTotalData.contents_[i].status_ == 2)
			{
				boxList[i].spriteName =  "baoxiangxiao2";
			}
			else
			{
				boxList[i].spriteName =  "baoxiangxiao";
			}
			
		}
		
		if(nowBoxNum >= 1)
		{
			barImg.gameObject.SetActive(true);
			bar.value = (float)nowBoxNum/6;
		}
		else
		{
			barImg.gameObject.SetActive(false);
		}
		nowMoneyLab.text = chargeTotalData.recharge_.ToString ();

		
		SetNowBaxInfo (nowBoxNum);
	}



	private void  SetNowBaxInfo(int num)
	{
		leftBtn.gameObject.SetActive(true);
		rightBtn.gameObject.SetActive(true);
		if(nowBoxNum == 0)
		{
			leftBtn.gameObject.SetActive(false);
			rightBtn.gameObject.SetActive(true);
		}

		if(nowBoxNum == 6)
		{
			rightBtn.gameObject.SetActive(false);
			leftBtn.gameObject.SetActive(true);
		}



		for(int i=0;i<reword.Count;i++)
		{
			reword[i].gameObject.SetActive(false);
		}

		for(int j= 0;j<chargeTotalData.contents_[num].itemIds_.Length;j++)
		{
			reword[j].gameObject.SetActive(true);
			ItemCellUI itemUI = UIManager.Instance.AddItemCellUI(reword[j],chargeTotalData.contents_[num].itemIds_[j]);
			itemUI.showTips = true;
			itemUI.ItemCount = (int)chargeTotalData.contents_[num].itemStacks_[j];
			rewordName[j].text = ItemData.GetData((int)chargeTotalData.contents_[num].itemIds_[j]).name_;
		}
		needMoneyImg.spriteName =  "czfl_"+rechargeData[num].num_.ToString () ;
		rewardImg.spriteName = rechargeData [num].picname;
		if(chargeTotalData.contents_[num].status_ == 1)
		{
			getBtn.isEnabled = true;
		}
		else
		{
			getBtn.isEnabled = false;
		}
	}

	private void OnLeftBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(nowBoxNum == 0)
		{
			return;
		}

		SetNowBaxInfo (--nowBoxNum);
		
	}


	private void OnRightBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(nowBoxNum >= 6)
		{
			return;
		}
		
		SetNowBaxInfo (++nowBoxNum);

	}

	private void OnGetBtn(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.requestmyselfrechargeleReward ((uint)nowBoxNum);
	}

	void fanliEnvet(COM_ADChargeTotal total)
	{
		UpdateInfo ();
	}

	void OnDestroy()
	{
		GamePlayer.Instance.getFanliEnvet -= fanliEnvet;
	}
}

