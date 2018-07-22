using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelpEquipInfoUI : MonoBehaviour
{
	public UILabel nameLab;
	public UITexture icon;
	public UILabel levelLab;
	public UILabel typeLab;
	public UILabel descLab;
	public UILabel getwayLab;
	public UIButton makeBtn;
	public UISprite makeImg;
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();
	private List<string> _icons = new List<string>();
	public UIGrid grid;
	public GameObject propCell;
	public UISprite propImg;
	private ItemData _itemData;

	void Start ()
	{
		makeBtn.transform.localPosition = new Vector3 (161f, makeBtn.transform.localPosition.y, makeBtn.transform.localPosition.z);
		UIManager.SetButtonEventHandler (makeBtn.gameObject, EnumButtonEvent.OnClick, OnClickMake, 0, 0);
	}


	private void OnClickMake(ButtonScript obj, object args, int param1, int param2)
	{
		if(!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Make))
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("gongnengweikaiqi"));
			return;
		}
		CompoundUI.SwithShowMe ();
	}
	public ItemData Item
	{
		get
		{
			return _itemData;
		}
		set
		{
			if(value != null)
			{
				_itemData = value;
				nameLab.text = _itemData.name_;
				HeadIconLoader.Instance.LoadIcon(_itemData.icon_,icon);
				
				if(!_icons.Contains(_itemData.icon_))
				{
					_icons.Add(_itemData.icon_);
				}

				levelLab.text =_itemData.level_.ToString();
				descLab.text = _itemData.desc_;
				getwayLab.text = _itemData.acquiringWay_;
				if(MakeData.GetData(_itemData.id_) != null)
				{
					makeBtn.gameObject.SetActive(true);
					makeImg.gameObject.SetActive(true);
					getwayLab.gameObject.SetActive(false);
				}
				else
				{
					getwayLab.gameObject.SetActive(true);
					makeBtn.gameObject.SetActive(false);
					makeImg.gameObject.SetActive(false);
				}

				typeLab.text = LanguageManager.instance.GetValue(_itemData.mainType_.ToString());


				if(_itemData.propArr.Count > 0)
				{
					foreach( GameObject o in cellList)
					{
						grid.RemoveChild(o.transform);
						o.transform.parent = null;
						o.gameObject.SetActive(false);
						cellPool.Add(o);
					}
					cellList.Clear ();
					
					propImg.gameObject.SetActive(true);
					for(int i=0;i<_itemData.propArr.Count;i++)
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
							objCell = Object.Instantiate(propCell) as GameObject;
						}
						string sNum ="";
						string sNum1 ="";
						if(int.Parse(_itemData.propArr[i].Value[0]) > 0)
						{
							sNum ="+"+ int.Parse(_itemData.propArr[i].Value[0]);
						}
						else
						{
							sNum = ""+ int.Parse(_itemData.propArr[i].Value[0]);
						}
						if(int.Parse(_itemData.propArr[i].Value[1]) > 0)
						{
							sNum1 = "+"+int.Parse(_itemData.propArr[i].Value[1]);
						}
						else
						{
							sNum1 = ""+ int.Parse(_itemData.propArr[i].Value[1]);
						}


						if(int.Parse(_itemData.propArr[i].Value[1]) < 0)
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.grey;
						}
						else if(int.Parse(_itemData.propArr[i].Value[0]) >= 15 && int.Parse(_itemData.propArr[i].Value[1]) <= 17)
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.black;
						}
						else if(int.Parse(_itemData.propArr[i].Value[0]) >= 18 && int.Parse(_itemData.propArr[i].Value[1]) <= 19)
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.green;
						}
						else if(int.Parse(_itemData.propArr[i].Value[0]) >= 20 && int.Parse(_itemData.propArr[i].Value[1]) <= 21)
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.blue;
						}
						else if(int.Parse(_itemData.propArr[i].Value[0]) >= 22 &&int.Parse( _itemData.propArr[i].Value[1]) <= 23)
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.magenta;
						}
						else 
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().color = Color.grey;
						}

						if(sNum != sNum1)
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().text  =
								LanguageManager.instance.GetValue(_itemData.propArr[i].Key.ToString())+ sNum + "-"+sNum1;
						}
						else
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().text  =
								LanguageManager.instance.GetValue(_itemData.propArr[i].Key.ToString())+ sNum;
						}
						objCell.transform.parent = grid.transform;
						objCell.gameObject.SetActive(true);	
						objCell.transform.localScale = Vector3.one;
						cellList.Add(objCell);
					}
					grid.Reposition();
				}
				else
				{
					propImg.gameObject.SetActive(false);
				}

			}
		}
	}

	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}

}


