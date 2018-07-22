using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpEquipAndSkillUI : MonoBehaviour
{
	public GameObject panel;
	public UIGrid grid;
	public GameObject itemCell;
	public UISprite icon;
	public UIButton selectBtn;
	public UIButton selectSkillBtn;
	public UIPanel equipType;
	public UIPanel skillTypes;
	public UILabel equipTypeLab;
	public UILabel skillTypeLab;

	public List<UIButton> equipTypeBtns = new List<UIButton>();
	public List<UISprite> weaponTypeBtns = new List<UISprite>();
	public List<UIButton> skillTypeBtns = new List<UIButton>();
	public GameObject weaponTypes;
	public HelpEquipInfoUI equipInfoUI;
	public HelpSkillInfoUI skillInfoUI;
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();

	HelpEquipCellUI selectObj;

	
	private int _selectType;

	void Start ()
	{
		UIManager.SetButtonEventHandler( selectBtn.gameObject,EnumButtonEvent.OnClick,OnSelectBtn,0,0);
		UIManager.SetButtonEventHandler( selectSkillBtn.gameObject,EnumButtonEvent.OnClick,OnSelectSkillBtn,0,0);
		UIManager.SetButtonEventHandler( equipType.gameObject,EnumButtonEvent.OnClick,OnClosEquipType,0,0);
		UIManager.SetButtonEventHandler( skillTypes.gameObject,EnumButtonEvent.OnClick,OnClosSkillType,0,0);

		for(int i=0;i<weaponTypeBtns.Count;i++)
		{
			UIManager.SetButtonEventHandler( weaponTypeBtns[i].gameObject,EnumButtonEvent.OnClick,OnWeaponTypeBtn,i+1,0);
		}

		for(int i=0;i<equipTypeBtns.Count;i++)
		{
			UIManager.SetButtonEventHandler( equipTypeBtns[i].gameObject,EnumButtonEvent.OnClick,OnEquipTypeBtn,i+1,0);
		}

		for(int i=0;i<skillTypeBtns.Count;i++)
		{
			UIManager.SetButtonEventHandler( skillTypeBtns[i].gameObject,EnumButtonEvent.OnClick,OnSkillTypeBtn,i+1,0);
		}
		//SelectType = 1;
		itemCell.SetActive (false); 
	}

	public int SelectType
	{
		set
		{
			if(_selectType != value)
			{
				_selectType = value;

				if(_selectType == 1)
				{
					equipInfoUI.gameObject.SetActive(true);
					skillInfoUI.gameObject.SetActive(false);
					SetEquipType (EquipmentSlot.ES_SingleHand,WeaponType.WT_Axe);
					equipInfoUI.Item = cellList[0].GetComponent<HelpEquipCellUI>().Item;
					selectObj = cellList[0].GetComponent<HelpEquipCellUI>();
					selectObj.back.gameObject.SetActive (true);
				}
				else if(_selectType == 2)
				{
					equipInfoUI.gameObject.SetActive(false);
					skillInfoUI.gameObject.SetActive(true);
					SetSkillType (1);
					skillInfoUI.Skill = cellList[0].GetComponent<HelpEquipCellUI>().Skill;
					selectObj = cellList[0].GetComponent<HelpEquipCellUI>();
					selectObj.back.gameObject.SetActive (true);
				} 
				/*else if(_selectType == 2)
				{
					equipInfoUI.gameObject.SetActive(false);
					skillInfoUI.gameObject.SetActive(true);
					SetSkillType (1);
					skillInfoUI.Skill = cellList[0].GetComponent<HelpEquipCellUI>().Skill;
					selectObj = cellList[0].GetComponent<HelpEquipCellUI>();
				}
				*/
			}
		}
		get
		{
			return _selectType;
		}
	}



	public void SetEquipType( EquipmentSlot type,WeaponType weapon = WeaponType.WT_None)
	{
		UpdateCellList ();
		foreach(ItemData x in ItemData.equipData)
		{
			if(x.slot_ == type && x.isShow_ == 1)
			{
				if(type == EquipmentSlot.ES_SingleHand || type == EquipmentSlot.ES_DoubleHand)
				{
					if(x.weaponType_ != weapon)
						continue;
				}

				GameObject objCell = null;
				if(cellPool.Count>0)
				{
					objCell = cellPool[0];
					cellPool.Remove(objCell);  
					UIManager.RemoveButtonAllEventHandler(objCell);
				}
				else  
				{
					 objCell = Object.Instantiate(itemCell) as GameObject;
				}


				HelpEquipCellUI cell = objCell.GetComponent<HelpEquipCellUI>();
				cell.Item  = x;
				UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickEquip, 0, 0);
				objCell.transform.parent = grid.transform;
				objCell.gameObject.SetActive(true);	
				objCell.transform.localScale = Vector3.one;
				cellList.Add(objCell);

			}
		}
		grid.Reposition ();
		skillInfoUI.gameObject.SetActive (false);
		equipInfoUI.Item = cellList[0].GetComponent<HelpEquipCellUI> ().Item;
	}



	//盾牌单独处理.
	public void SetShield()
	{
		UpdateCellList ();
		foreach(ItemData x in ItemData.equipData)
		{
			if(x.subType_ ==  ItemSubType.IST_Shield && x.isShow_ == 1)
			{	
				GameObject objCell = null;
				if(cellPool.Count>0)
				{
					objCell = cellPool[0];
					cellPool.Remove(objCell);  
					UIManager.RemoveButtonAllEventHandler(objCell);
				}
				else  
				{
					objCell = Object.Instantiate(itemCell) as GameObject;
				}
				
				HelpEquipCellUI cell = objCell.GetComponent<HelpEquipCellUI>();
				cell.Item  = x;
				UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickEquip, 0, 0);
				objCell.transform.parent = grid.transform;
				objCell.gameObject.SetActive(true);	
				objCell.transform.localScale = Vector3.one;
				cellList.Add(objCell);
				
			}
		}
		grid.Reposition ();
		skillInfoUI.gameObject.SetActive (false);
		equipInfoUI.Item = cellList[0].GetComponent<HelpEquipCellUI> ().Item;
	}




	public void SetHandType(WeaponType weapon )
	{
		UpdateCellList ();
		foreach(ItemData x in ItemData.equipData)
		{
			if(x.weaponType_ ==  weapon && x.isShow_ == 1)
			{	
				GameObject objCell = null;
				if(cellPool.Count>0)
				{
					objCell = cellPool[0];
					cellPool.Remove(objCell);  
					UIManager.RemoveButtonAllEventHandler(objCell);
				}
				else  
				{
					objCell = Object.Instantiate(itemCell) as GameObject;
				}
				
				HelpEquipCellUI cell = objCell.GetComponent<HelpEquipCellUI>();
				cell.Item  = x;
				UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickEquip, 0, 0);
				objCell.transform.parent = grid.transform;
				objCell.gameObject.SetActive(true);	
				objCell.transform.localScale = Vector3.one;
				cellList.Add(objCell);
				
			}
		}
		grid.Reposition ();
		skillInfoUI.gameObject.SetActive (false);
		equipInfoUI.Item = cellList[0].GetComponent<HelpEquipCellUI> ().Item;
	}



	public void SetSkillType(int type)
	{
		UpdateCellList ();
        foreach(SkillData[] x in SkillData.metaData.Values)
        {
			if(x == null || x[1] == null)
				continue;
            if(x[1]._LearnGroup == type)
            {
                GameObject objCell = null;
                if(cellPool.Count>0)
                {
                    objCell = cellPool[0];
                    cellPool.Remove(objCell);  
                    UIManager.RemoveButtonAllEventHandler(objCell);
                }
                else  
                {
                    objCell = Object.Instantiate(itemCell) as GameObject;
                }
				
                
                HelpEquipCellUI cell = objCell.GetComponent<HelpEquipCellUI>();
                cell.Skill  = x[1];
                UIManager.SetButtonEventHandler (objCell, EnumButtonEvent.OnClick, OnClickSkill, 0, 0);
                objCell.transform.parent = grid.transform;
				objCell.gameObject.SetActive(true);
				objCell.transform.localScale = Vector3.one;
                cellList.Add(objCell);
            }
        }
        grid.Reposition ();
		equipInfoUI.gameObject.SetActive (false);
		skillInfoUI.Skill = cellList[0].GetComponent<HelpEquipCellUI> ().Skill;
	}

	public void UpdateCellList()
	{
		foreach( GameObject o in cellList)
		{                        
			grid.RemoveChild(o.transform);
			o.transform.parent = null;
			o.gameObject.SetActive(false);
			cellPool.Add(o);
		}
		cellList.Clear ();


	}

	private void OnClickEquip(ButtonScript obj, object args, int param1, int param2)
	{
		if (selectObj != null)
			selectObj.back.gameObject.SetActive (false);
		skillInfoUI.gameObject.SetActive (false);
		equipInfoUI.Item = obj.GetComponent<HelpEquipCellUI> ().Item;
		selectObj = obj.GetComponent<HelpEquipCellUI>();
		selectObj.back.gameObject.SetActive (true);
	}

	private void OnClickSkill(ButtonScript obj, object args, int param1, int param2)
	{
		if (selectObj != null)
			selectObj.back.gameObject.SetActive (false);
		equipInfoUI.gameObject.SetActive (false);
		skillInfoUI.Skill = obj.GetComponent<HelpEquipCellUI> ().Skill;
		selectObj = obj.GetComponent<HelpEquipCellUI>();
		selectObj.back.gameObject.SetActive (true);
	}

	private void OnSelectBtn(ButtonScript obj, object args, int param1, int param2)	
	{
		equipType.gameObject.SetActive (true);
	}

	private void OnSelectSkillBtn(ButtonScript obj, object args, int param1, int param2)
	{
		skillTypes.gameObject.SetActive (true);
	}

	private void OnClosEquipType(ButtonScript obj, object args, int param1, int param2)
	{
		weaponTypes.gameObject.SetActive (false);
		equipType.gameObject.SetActive (false);
	}

	private void OnClosSkillType(ButtonScript obj, object args, int param1, int param2)
	{
		skillTypes.gameObject.SetActive (false);
	}

	private void OnEquipTypeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		equipTypeLab.text = obj.transform.FindChild ("Label").GetComponent<UILabel> ().text;
		switch(param1)
		{
		case  1:
		{
			weaponTypes.gameObject.SetActive(true);
		}
			break;
		case  2:
		{
			weaponTypes.gameObject.SetActive(false);
			equipType.gameObject.SetActive (false);
			SetEquipType (EquipmentSlot.ES_Body);
		}
			break;
		case  3:
		{
			weaponTypes.gameObject.SetActive(false);
			equipType.gameObject.SetActive (false);
			SetEquipType (EquipmentSlot.ES_Head);
		}
			break;
		case  4:
		{
			weaponTypes.gameObject.SetActive(false);
			equipType.gameObject.SetActive (false);
			SetShield();
		}
			break;
		case  5:
		{
			weaponTypes.gameObject.SetActive(false);
			equipType.gameObject.SetActive (false);
			SetEquipType (EquipmentSlot.ES_Boot);
		}
			break;
		case  6:
		{
			weaponTypes.gameObject.SetActive(false);
			equipType.gameObject.SetActive (false);
			SetEquipType (EquipmentSlot.ES_Ornament_0);
		}
			break;
		}


	}

	private void OnWeaponTypeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		weaponTypes.gameObject.SetActive(false);
		equipType.gameObject.SetActive (false);
		switch(param1)
		{
		case  1:
		{
			SetHandType (WeaponType.WT_Axe);
		}
			break;
		case  2:
		{

			SetHandType (WeaponType.WT_Sword);
		}
			break;
		case  3:
		{
			SetHandType (WeaponType.WT_Spear);
		}
			break;
		case  4:
		{
			SetHandType (WeaponType.WT_Bow);
		}
			break;
		case  5:
		{
			SetHandType (WeaponType.WT_Knife);
		}
			break;
		case  6:
		{
			SetHandType (WeaponType.WT_Stick);
		}
			break;
		case  7:
		{
			//SetHandType (WeaponType.WT_V);
		}
			break;
		}
	}

	private void OnSkillTypeBtn(ButtonScript obj, object args, int param1, int param2)
	{
		skillTypeLab.text = obj.transform.FindChild ("Label").GetComponent<UILabel> ().text;
		skillTypes.gameObject.SetActive(false);
		if(param1 <= 0)
			return;
		SetSkillType (param1);
	}
	
	void OnDestroy()
	{

		for(int i = 0 ;i< cellPool.Count;i++)
		{
			Destroy(cellPool[i]);
			cellPool[i] = null;
		}
		cellPool.Clear ();

	}

}

