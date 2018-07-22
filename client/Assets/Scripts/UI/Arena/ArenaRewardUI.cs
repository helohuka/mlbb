using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaRewardUI : UIBase
{
	public UIButton closeBtn;
	public UIButton dayBtn;
	public UIButton weekBtn;
	public UIGrid   rewardGrid;
	public GameObject listCell;
	public UILabel titleLab;
	public UILabel	btnLab;
	private List<GameObject> CellList = new List<GameObject>();
	private List<GameObject> CellPoolList = new List<GameObject>();
	private static int panelId;

	void Start ()
	{
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);
		UIManager.SetButtonEventHandler (dayBtn.gameObject, EnumButtonEvent.OnClick, OnDay, 0, 0);
		UIManager.SetButtonEventHandler (weekBtn.gameObject, EnumButtonEvent.OnClick, OnWeek, 0, 0);
		dayBtn.isEnabled = false;
		if(panelId == 1)
		{
			titleLab.text = LanguageManager.instance.GetValue("lixianarena");
			btnLab.text = LanguageManager.instance.GetValue("lixianarenari");
			UpdataLIst (1);
		}
		else
		{
			titleLab.text = LanguageManager.instance.GetValue("zaixianarena");
			btnLab.text = LanguageManager.instance.GetValue("zaixianarenaji");
			UpdataPvpList(2);
		}
	}

	#region Fixed methods for UIBase derived cass
	
	public static void SwithShowMe()
	{
		UIBase.SwitchShowPanelByName (UIASSETS_ID.UIASSETS_ArenaRewardPanel);
	}
	
	public static void ShowMe(int pid)
	{
		panelId = pid;
		UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_ArenaRewardPanel);
	}
	
	public static void HideMe()
	{
		UIBase.HidePanelByName (UIASSETS_ID.UIASSETS_ArenaRewardPanel);
	}
	
	#endregion
	
	public override void Destroyobj ()
	{
		GameObject.Destroy (gameObject);
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
	 	Hide ();	
	}

	private void OnDay(ButtonScript obj, object args, int param1, int param2)
	{
		dayBtn.isEnabled = false;
		weekBtn.isEnabled = true;

		if(panelId == 1)
		{
			UpdataLIst (1);
		}
		else
		{
			UpdataPvpList(1);
		}
	}

	private void OnWeek(ButtonScript obj, object args, int param1, int param2)
	{
		dayBtn.isEnabled = true;
		weekBtn.isEnabled = false;
		if(panelId == 1)
		{
			UpdataLIst (2);
		}
		else
		{
			UpdataPvpList(2);
		}
	}

	private void UpdataLIst(int time)
	{
		for(int i=0; i < CellList.Count; ++i)
		{
			rewardGrid.RemoveChild(CellList[i].transform);
			CellList[i].transform.parent = null;
			CellList[i].gameObject.SetActive(false);
            CellPoolList.Add(CellList[i]);
		}
		CellList.Clear ();


		foreach(PvRrewardData p in PvRrewardData.metaData.Values)
		{
			if(p==null || p.id_ == 0)
				continue;
			GameObject objCell = null;
			if(CellPoolList.Count>0)
			{
				objCell = CellPoolList[0];
				CellPoolList.Remove(objCell);  
			}
			else  
			{
				objCell = Object.Instantiate(listCell.gameObject) as GameObject;
			}

			objCell.transform.parent = rewardGrid.transform;
			objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankLab.text = p.ranking_[0] + "-" + p.ranking_[1];
			objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankImg.gameObject.SetActive(true);
			objCell.gameObject.GetComponent<ArenaRewardCellUI>().fenLab.gameObject.SetActive(false);
			if(p.id_ == 1)
			{
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankImg.spriteName  = "paimingjiangli";
			}
			else if(p.id_ == 2)
			{
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankImg.spriteName  = "paimingjiangli2";
			}
			else if(p.id_ == 3)
			{
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankImg.spriteName  = "paimingjiangli3";
			}
			else
			{
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankImg.spriteName  = "paimingjiangli4";
			}
			if(time == 1)
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().PvPDayReward(p.day_);
			else
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().PvPDayReward(p.times_);
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);

		}
		rewardGrid.Reposition ();

	}


	private void UpdataPvpList(int time)
	{
        for (int i = 0; i < CellList.Count; ++i )
        {
            rewardGrid.RemoveChild(CellList[i].transform);
            CellList[i].transform.parent = null;
            CellList[i].gameObject.SetActive(false);
            CellPoolList.Add(CellList[i]);
        }
		CellList.Clear ();



		int len = PvpRewardData.metaData.Values.Count;
		int num = 0;
		for(int i = len;i>=1;i-- )
		{ 
			num++;
			PvpRewardData p =PvpRewardData.metaData[i];
			if(p ==null || p.id_ == 0)
				continue;
			GameObject objCell = null;
			if(CellPoolList.Count>0)
			{
				objCell = CellPoolList[0];
				CellPoolList.Remove(objCell);  
			}
			else  
			{
				objCell = Object.Instantiate(listCell.gameObject) as GameObject;
			}
			
			objCell.transform.parent = rewardGrid.transform;
			objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankLab.text = p.rankName_;//.min_ + "-" + p.max_;
			objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankImg.gameObject.SetActive(true);
			objCell.gameObject.GetComponent<ArenaRewardCellUI>().fenLab.gameObject.SetActive(false);
			objCell.gameObject.GetComponent<ArenaRewardCellUI>().rankImg.spriteName  = "py_" +num;//p.id_;
			
			if(time == 1)
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().PvPDayReward(p.day_);
			else
				objCell.gameObject.GetComponent<ArenaRewardCellUI>().PvPDayReward(p.times_);
			objCell.SetActive(true);
			objCell.transform.localScale = Vector3.one;
			CellList.Add(objCell);
		}
		rewardGrid.Reposition ();
	}

}

