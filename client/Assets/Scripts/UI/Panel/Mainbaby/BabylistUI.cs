using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BabylistUI : MonoBehaviour {

	public GameObject item;
	public UIGrid grid;
	public UIButton close;
	void Start () {
		item.SetActive (false);
		UIManager.SetButtonEventHandler (close.gameObject, EnumButtonEvent.OnClick, OnClickclose,0,0);
	 
	}
	void addItem(List<Baby> babys)
	{
		Refresh ();
		for(int i=0;i<babys.Count;i++)
		{
			GameObject o = GameObject.Instantiate(item)as GameObject;
			o.SetActive(true);
			o.name = o.name+i;
			o.transform.parent = grid.transform;
			MainBabyListCell mbCell = o.GetComponent<MainBabyListCell>();
			Baby inst = GamePlayer.Instance.GetBabyInst (babys[i].InstId);
			mbCell.BabyMainData = babys[i];
			BabyData bab = BabyData.GetData (inst.GetIprop(PropertyType.PT_TableId));
			string ReformItem = bab._ReformItem;
//			if(MainbabyReformUI.isMainbabyReformUI)
//			{
//				if(babys[i].isForBattle_)
//				{
//					mbCell.chuzhanButton.gameObject.SetActive(true);
//					mbCell.chuzhanButton.isEnabled = false;
//					mbCell.daimingButton.gameObject.SetActive(false);
//				}else
//				{
//					mbCell.chuzhanButton.gameObject.SetActive(false);
//					mbCell.daimingButton.gameObject.SetActive(true);
//					mbCell.daimingButton.isEnabled = false;
//				}
//			}
//				if(ReformItem == "")
//				{
//					mbCell.ban.gameObject.SetActive(true);
//
//					
//				}else
//					if(inst.GetIprop(PropertyType.PT_Level)>1)
//				{
//					mbCell.ban.gameObject.SetActive(true);
//
//				}else
//				{
//					mbCell.ban.gameObject.SetActive(false);
//				}
			

			o.transform.localScale= new Vector3(1,1,1);	
			UIManager.SetButtonEventHandler (o, EnumButtonEvent.OnClick, buttonClick,babys[i].InstId,babys[i].GetIprop(PropertyType.PT_AssetId));
			grid.repositionNow = true;

		}
	}
	void Refresh()
	{
		foreach(Transform tr in grid.transform)
		{
			Destroy(tr.gameObject);
		}
	}
	private void buttonClick(ButtonScript obj, object args, int param1, int param2)
	{
	
		Baby inst = GamePlayer.Instance.GetBabyInst (param1);
		BabyData bab = BabyData.GetData (inst.GetIprop(PropertyType.PT_TableId));
		string ReformItem = bab._ReformItem;
		if(ReformItem == "")
		{

			PopText.Instance.Show(LanguageManager.instance.GetValue("babygaizao1"));

		}else
			if(inst.GetIprop(PropertyType.PT_Level)>1)
		{
		
			PopText.Instance.Show(LanguageManager.instance.GetValue("babygaizao2"));
		}else
		{
		
			if(MainbabyReformUI.ShowbabyInfoOk != null)
			{
				MainbabyReformUI.ShowbabyInfoOk(inst,ReformItem,bab._ReformMonster);
			}
			gameObject.SetActive(false);
		}


	}

	void UpdateBabyState()
	{
		GlobalInstanceFunction.Instance.Invoke(() => { addItem(GamePlayer.Instance.babies_list_); }, 0.1f);

	}


	void OnEnable()
	{
		UpdateBabyState ();
	}
	private void OnClickclose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}

}
