using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmployeeStarUpUI : MonoBehaviour
{
	public UIButton starUpBtn;
	public UITexture skillIcon;
	public List<GameObject> starItemList = new List<GameObject> ();
	private Employee employeeInst;
	private List<COM_ExpendItem> _starNeedItem = new List<COM_ExpendItem>();
	private bool _isCanStarUp  = true; 
	public UISprite nextText;
	public UILabel nextTextLab;

	void Start ()
	{
		UIManager.SetButtonEventHandler (starUpBtn.gameObject, EnumButtonEvent.OnClick, OnJinjieBtn, 0, 0);
		UpdateStarInfo ();
	}



	private void OnJinjieBtn(ButtonScript obj, object args, int param1, int param2)
	{
		COM_ExpendItem[] expendItems = new COM_ExpendItem[_starNeedItem.Count];
		for(int i =0;i<_starNeedItem.Count;i++)
		{
			expendItems[i] = _starNeedItem[i];
		}
	}

	public void UpdateStarInfo()
	{
		employeeInst = EmployessSystem.instance.CurEmployee;
		if (employeeInst == null)
			return;
		for(int s=0;s<starItemList.Count;s++)
		{
			starItemList[s].gameObject.SetActive(false);
		}
		if(employeeInst.star_ >=5)
		{
			return;
		}
		nextTextLab.text = LanguageManager.instance.GetValue ("huobanshengxing").Replace("{n}",starUpNum((int)employeeInst.star_));
		_starNeedItem.Clear();
		
		List<KeyValuePair<int,int>> starItems = GetStarItem(employeeInst.GetIprop(PropertyType.PT_TableId),(int)employeeInst.star_-1);
		if(starItems.Count <=0)
		{
			starUpBtn.isEnabled = false;
			return;
		}
		
		starUpBtn.isEnabled = true;
		for(int i= 0;i<starItems.Count;i++)
		{
			ItemData  data = ItemData.GetData(starItems[i].Key);
			if(data == null)
			{
				return;
			}
			starItemList[i].gameObject.SetActive(true);
			ItemCellUI cell = UIManager.Instance.AddItemCellUI(starItemList[i].transform.Find("icon").GetComponent<UISprite>(),(uint)data.id_);
			cell.showTips = true;
			
			COM_Item sItem = BagSystem.instance.GetItemByItemId((uint)data.id_);
			COM_ExpendItem eItem = new COM_ExpendItem();
			//starItemList[i].transform.Find("num").GetComponent<UILabel>().text = starItems[i].Value.ToString();

			int needNum = BagSystem.instance.GetItemMaxNum((uint)data.id_);
			starItemList[i].transform.Find("num").GetComponent<UILabel>().text = needNum+"/"+ starItems[i].Value.ToString();

			if(needNum >= starItems[i].Value)
			{
				eItem.itemInstId_ = sItem.instId_;
				eItem.num_ = (uint)starItems[i].Value;
				_starNeedItem.Add(eItem);
				starItemList[i].transform.Find("num").GetComponent<UILabel>().color = Color.green;
			}
			else
			{
				_starNeedItem.Add(eItem);
				_isCanStarUp = false;
				starItemList[i].transform.Find("num").GetComponent<UILabel>().color = Color.red;
			}
			
		}
		
		for(int i= 0;i<starItems.Count;i++)
		{
			COM_Item nitem = BagSystem.instance.GetItemByItemId((uint)starItems[i].Key);	
			if(nitem == null)
			{
				starUpBtn.isEnabled = false;	
				break;
			}
			if(BagSystem.instance.GetItemMaxNum((uint)starItems[i].Key) < starItems[i].Value)
			{
				starUpBtn.isEnabled = false;	
				break;
			}
		}
	}

	public List<KeyValuePair<int,int>> GetStarItem(int eqmployeeId, int star)
	{
		if(star > 5)  //最高星级.
		{
			return null;
		}
		
		EmployeeData employee = EmployeeData.GetData(eqmployeeId);
		if(employee == null)
		{
			return null;
		}
		
		string[] items;
		
		switch (star) 
		{
		case 1:
		{
			return employee.advancedList1;
		}
			break;
		case 2:
		{
			return employee.advancedList2;
		}
			break;	
		case 3:
		{
			return employee.advancedList3;
		}
			break;
		case 4:
		{
			return employee.advancedList4;
		}
			break;
		case 5:
		{
			return employee.advancedList5;
		}
			break;
		}
		return null;

	}
	public string starUpNum( int star)
	{
		switch(star)
		{

		case 0:
			return "30%";
				break;
		case 1:
			return "60%";
				break;
		case 2:
			return "90%";
				break;
		case 3:
			return "120%";
				break;
		case 4:
			return "150%";
				break;
		}
		return "";
	}
}

