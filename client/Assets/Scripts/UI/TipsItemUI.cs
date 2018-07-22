using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsItemUI : MonoBehaviour
{
	public UIPanel pane;
	public UITexture itemIcon;
	public UILabel nameLab;
	public UILabel descLab;
	public UILabel levelLab;
	public UILabel typeLab;
	public UILabel propLab;
	public UISprite propImg;
	public UISprite clseback;
	public UIGrid grid;
	public GameObject propCell;
	public UISprite iconBack;
	public UISprite backImg;
	public UILabel topGetWayLab;
	public UILabel topGetWayInfoLab;
	public UILabel getWayLab;
	public UILabel getWayInfoLab;
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();
	private UIPanel _tooltip;

	public static bool Loading = false;
	public static TipsItemUI _instance;
	public static TipsItemUI instance
	{
		get
		{
			if (_instance == null && Loading == false)
			{
				Loading = true;
				
				string uiResPath = GlobalInstanceFunction.Instance.GetAssetsName((int)UIASSETS_ID.UIASSETS_TipsITemUI, AssetLoader.EAssetType.ASSET_UI);
				
				AssetLoader.LoadAssetBundle(uiResPath, AssetLoader.EAssetType.ASSET_UI, (Assets, paramData) =>
				                            {
					if (null == Assets || null == Assets.mainAsset)
					{
						Loading = false;
						return;
					}
					
					GameObject go = (GameObject)GameObject.Instantiate(Assets.mainAsset) as GameObject;
					TipsItemUI t = (TipsItemUI)go.GetComponent<TipsItemUI>();
					t.AttachToGameObject(go);
					_instance = t;
					Loading = false;
				}
				, null);
				
			}//
			return _instance;

		}
		set
		{
			_instance = value;
		}
	}
	
	void Start ()
	{
		UIManager.SetButtonEventHandler (clseback.gameObject, EnumButtonEvent.OnClick, OnClose, 0, 0);

	}

	int defaultDepth;
	public void setData(object value, int startDefault = 1)
	{
		defaultDepth = startDefault;
		ShowTips();
		if(value is ItemData)
		{
			data = (ItemData)value;
		}
		else if(value is COM_Item)
		{
			ItemInst = (COM_Item)value;
		}

	}

	private ItemData data
	{
		set
		{
			HeadIconLoader.Instance.LoadIcon(value.icon_, itemIcon);
			nameLab.text = value.name_;
			descLab.text = value.desc_;
			levelLab.text = value.level_.ToString();
			typeLab.text = LanguageManager.instance.GetValue(value.mainType_.ToString());
			iconBack.spriteName = BagSystem.instance.GetQualityBack((int)value.quality_);
			topGetWayInfoLab.text  = value.acquiringWay_;
			getWayInfoLab.text  = value.acquiringWay_;
			if(value.propArr.Count > 0)
			{
				if(value.subType_ == ItemSubType.IST_Fashion)
				{
					propImg.gameObject.SetActive(false);
					backImg.height = 382;
					topGetWayLab.gameObject.SetActive(true);
					getWayLab.gameObject.SetActive(false);
					return;
				}
				backImg.height = 635;
				topGetWayLab.gameObject.SetActive(false);
				getWayLab.gameObject.SetActive(true);
				for( int o = 0;o<cellList.Count;o++)
				{
					grid.RemoveChild(cellList[o].transform);
					cellList[o].transform.parent = null;
					cellList[o].gameObject.SetActive(false);
					cellPool.Add(cellList[o]);
				}
				cellList.Clear ();
				
				propImg.gameObject.SetActive(true);
				for(int i=0;i<value.propArr.Count;i++)
				{
					//if(value.propArr[i] == null)
					//	continue;
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
					string sNum = "";
					if(int.Parse(value.propArr[i].Value[0]) == int.Parse(value.propArr[i].Value[1]))
					{
						if(int.Parse(value.propArr[i].Value[0]) >0 )
							sNum = " +" + value.propArr[i].Value[0];
						else
							sNum = " " + value.propArr[i].Value[0];
					}
					else if(int.Parse(value.propArr[i].Value[0]) > 0)
					{
						sNum = " +" + (value.propArr[i].Value[0] +" - " +value.propArr[i].Value[1]);
					}
					else
					{
						sNum =  " "+(value.propArr[i].Value[0] + "- "+value.propArr[i].Value[1]);
					}
				

					UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>();
					lable.text  =  LanguageManager.instance.GetValue(value.propArr[i].Key.ToString())+sNum;
				
					if(ItemData.GetData((int)value.id_).mainType_ == ItemMainType.IMT_FuWen)
					{
						lable.color = Color.grey;
					}
					else
					{
						float perNum = EquipColorData.GetEquipPerNum(value.level_, value.propArr[i].Key,int.Parse(value.propArr[i].Value[1]));
						if(perNum < 0)
						{
							lable.color = Color.grey;
						}
						else if(perNum < 24)
						{
							lable.color = Color.grey;
						}
						else if(perNum >= 25 && perNum <= 49)
						{
							lable.color = Color.green;
						}
						else if(perNum >=50 && perNum <= 74)
						{
							lable.color = Color.blue;
						}
						else if(perNum >= 75 && perNum <= 84)
						{
							lable.color = Color.magenta;
						}
						else if(perNum >= 85 && perNum <= 94)
						{
							lable.color = Color.yellow;
						}
						else 
						{
							lable.color = Color.white;
							lable.text = "[FECE29]" + lable.text;
						}
					}
			
					grid.AddChild(objCell.transform);
					objCell.transform.localScale = Vector3.one;
					objCell.gameObject.SetActive(true);
					cellList.Add(objCell);
				}
				grid.Reposition();
			}
			else
			{
				backImg.height = 382;
				topGetWayLab.gameObject.SetActive(true);
				getWayLab.gameObject.SetActive(false);
				propImg.gameObject.SetActive(false);
			}
		}
	}


	private COM_Item ItemInst
	{
		set
		{
			ItemData itemD = ItemData.GetData((int)value.itemId_);
			if(itemD == null)
				return;
			HeadIconLoader.Instance.LoadIcon(itemD.icon_, itemIcon);
			nameLab.text = itemD.name_;
			descLab.text = itemD.desc_;
			levelLab.text = itemD.level_.ToString();
			typeLab.text = LanguageManager.instance.GetValue(itemD.mainType_.ToString());
			iconBack.spriteName = BagSystem.instance.GetQualityBack((int)itemD.quality_);
			topGetWayInfoLab.text  = itemD.acquiringWay_;
			getWayInfoLab.text  = itemD.acquiringWay_;
			if(value.propArr.Length > 0)
			{
				if(itemD.subType_ == ItemSubType.IST_Fashion)
				{
					propImg.gameObject.SetActive(false);
					topGetWayLab.gameObject.SetActive(true);
					getWayLab.gameObject.SetActive(false);
					backImg.height = 382;
					return;
				}
				topGetWayLab.gameObject.SetActive(false);
				getWayLab.gameObject.SetActive(true);
				backImg.height = 635;
				for( int o = 0;o<cellList.Count;o++)
				{
					grid.RemoveChild(cellList[o].transform);
					cellList[o].transform.parent = null;
					cellList[o].gameObject.SetActive(false);
					cellPool.Add(cellList[o]);
				}
				cellList.Clear ();
				
				propImg.gameObject.SetActive(true);
				for(int i=0;i<value.propArr.Length;i++)
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
					string sNum = "";
					if((int)value.propArr[i].value_ > 0)
					{
						sNum = " +" + (int)value.propArr[i].value_;
					}
					else
					{
						sNum =  " "+(int)value.propArr[i].value_ ;
					}
					
					UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>();
					lable.text  =  LanguageManager.instance.GetValue(value.propArr[i].type_.ToString())+sNum;
					
					float perNum = EquipColorData.GetEquipPerNum(itemD.level_, value.propArr[i].type_,(int)value.propArr[i].value_);
					if(perNum < 0)
					{
						lable.color = Color.grey;
					}
					else if(perNum < 24)
					{
						lable.color = Color.grey;
					}
					else if(perNum >= 25 && perNum <= 49)
					{
						lable.color = Color.green;
					}
					else if(perNum >=50 && perNum <= 74)
					{
						lable.color = Color.blue;
					}
					else if(perNum >= 75 && perNum <= 84)
					{
						lable.color = Color.magenta;
					}
					else if(perNum >= 85 && perNum <= 94)
					{
						lable.color = Color.yellow;
					}
					else 
					{
						lable.color = Color.white;
						lable.text = "[FECE29]" + lable.text;
					}
					
					grid.AddChild(objCell.transform);
					objCell.transform.localScale = Vector3.one;
					objCell.gameObject.SetActive(true);
					cellList.Add(objCell);
				}
				grid.Reposition();
			}
			else
			{
				topGetWayLab.gameObject.SetActive(true);
				getWayLab.gameObject.SetActive(false);
				backImg.height = 382;
				propImg.gameObject.SetActive(false);
			}
		}
	}


	public void ShowTips()
	{
		if(_tooltip != null)
		{
			_tooltip.gameObject.SetActive(true);
			UIManager.Instance.AdjustUIDepth(_tooltip.transform, true, -500f, defaultDepth);
		}
	}


	public void HideTips()
	{
		if(_tooltip != null)
		{
			_tooltip.gameObject.SetActive(false);
		}
	}


	private void setPosition( Vector2 position )
	{
		// The tooltip should appear above the mouse
		var cursorOffset = new Vector2( 0, _tooltip.GetComponent<UISprite>().height + 50 );
		
		// Convert position from "screen coordinates" to "gui coordinates"

		
		_tooltip.transform.position = position;
		
	}

	public void AttachToGameObject(GameObject goSelf)
	{
		_tooltip = (UIPanel)goSelf.GetComponent<UIPanel>();
		_tooltip.gameObject.transform.parent = ApplicationEntry.Instance.uiRoot.GetComponent<UIPanel>().transform;
		_tooltip.gameObject.transform.localScale = Vector3.one;
		
		// 开始要关闭。.
		HideTips();
	}


	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		pane.gameObject.SetActive (false);
	}
}

