using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Tips : MonoBehaviour {
	
	public UILabel _LevelLable;
	public UILabel _SpeciesLable;
	public UILabel _TujingLable;

	public UISlider naijiuSlider;
	public UILabel naijiuLable;
	public UISprite xiuSp;
	public GameObject naijiuObj;
	public UISprite backSp;
	public GameObject decObj;
	public GameObject tujingdibanObj;
	public GameObject tujingdesObj;
	public GameObject tujingposObj;
	public GameObject tujingposEObj;

	public CloseCallBack closeCallback;
	public GameObject tipPene;
	public UILabel nameLab;
	public UILabel descLab;
	public UILabel propLab;
	public UILabel getWayLab;
	public UITexture icon;
	public UISprite tipsBg;

	public UILabel levelLab;
	public UILabel durabilityLab;
	public UILabel typeLab;
	public UISprite propImg;


	public BagSplitUI splitUI;
	public UISprite tipsImg;
	public UIButton sellBtn;
	
	
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();
	
	public UIGrid grid;
	public GameObject propCell;
	
	private COM_Item _itemInst;
	private ItemData _itemData;
	
	public BagCellUI bagCell;
	private bool _isSell;
	public bool isleft;
	private uint _playerInstId;
	
	void Start ()
	{
	    
		_LevelLable.text = LanguageManager.instance.GetValue("bank_Level");
		_SpeciesLable.text = LanguageManager.instance.GetValue("bank_Species");
		_TujingLable.text = LanguageManager.instance.GetValue("bank_Tujing");
		UIManager.SetButtonEventHandler (tipsBg.gameObject, EnumButtonEvent.OnClick, OnClose,0, 0);
		UIManager.SetButtonEventHandler (sellBtn.gameObject, EnumButtonEvent.OnClick, onSellBtn, 0, 0);
		
	}
	
	void OnEnable()
	{	
		if(isleft)
		{
			sellBtn.GetComponentInChildren<UILabel>().text = LanguageManager.instance.GetValue("bank_TakeOut");
		}else
		{
			sellBtn.GetComponentInChildren<UILabel>().text = LanguageManager.instance.GetValue("bank_TakeIn");
		}
		//GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagTipOpen);
	}
	

	
	public COM_Item Item
	{
		set
		{
			if(value != null)
			{
				_itemInst = value;
				ItemData data = ItemData.GetData((int)_itemInst.itemId_);
				if(data == null)
					return;
				ItemTabData = data;
				nameLab.text = data.name_; 
				descLab.text = data.desc_;
				getWayLab.text = data.acquiringWay_;
				HeadIconLoader.Instance.LoadIcon( data.icon_,icon);
				levelLab.text = data.level_.ToString();
				typeLab.text = LanguageManager.instance.GetValue(data.mainType_.ToString());
				if(data.mainType_ == ItemMainType.IMT_Equip)
				{
					decObj.SetActive(true);
					naijiuObj.SetActive(true);
					naijiuLable.gameObject.SetActive(true);
					naijiuSlider.gameObject.SetActive(true);
					tujingdibanObj.SetActive(false);
					naijiuLable.text = _itemInst.durability_+"/"+_itemInst.durabilityMax_;
					naijiuSlider.value =  (_itemInst.durability_*1f)/(_itemInst.durabilityMax_*1f);
					if(_itemInst.durability_ <= _itemInst.durabilityMax_*0.8)
					{
						xiuSp.gameObject.SetActive(true);
						if(_itemInst.durability_ <= _itemInst.durabilityMax_*0.5)
							xiuSp.spriteName = "huai";
						else
							xiuSp.spriteName = "xiu";
					}else
					{
						xiuSp.gameObject.SetActive(false);
					}
					backSp.height = 640;
					tujingdesObj.transform.parent = tujingposEObj.transform;
					tujingdesObj.transform.localPosition = Vector3.zero;
				
				}else
				{
					tujingdibanObj.SetActive(true);
					naijiuLable.gameObject.SetActive(false);
					naijiuSlider.gameObject.SetActive(false);
					xiuSp.gameObject.SetActive(false);
					decObj.SetActive(false);
					naijiuObj.SetActive(false);
					backSp.height = 520;
					tujingdesObj.transform.parent = tujingposObj.transform;
					tujingdesObj.transform.localPosition = Vector3.zero;
				}

				if(_itemInst.propArr.Length > 0)
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
					for(int i=0;i<_itemInst.propArr.Length;i++)
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
						
						
						for(int a=0;a<data.propArr.Count;a++)
						{
							if(_itemData.propArr[a].Key == _itemInst.propArr[i].type_)
							{
								string sNum = "";
								if(_itemInst.propArr[i].value_ > 0)
								{
									sNum = " +" + ((int)_itemInst.propArr[i].value_);
								}
								else
								{
									sNum =  " "+((int)_itemInst.propArr[i].value_).ToString();
								}
								if(_itemInst.propArr[i].value_  == float.Parse(_itemData.propArr[a].Value[1]))
								{
									objCell.transform.FindChild("name").GetComponent<UILabel>().text  =
										"[E845EB]"+
											LanguageManager.instance.GetValue(_itemInst.propArr[i].type_.ToString())+ sNum+"[-]";
								}
								else
								{
									objCell.transform.FindChild("name").GetComponent<UILabel>().text  = 
										LanguageManager.instance.GetValue(_itemInst.propArr[i].type_.ToString())+sNum;
								}
								break;
							}
							
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
		get
		{
			return _itemInst;
		}
	}
	
	public uint PlayerInstId
	{
		set
		{
			_playerInstId = value;
		}
		get
		{
			return _playerInstId;
		}
	}
	
	
	public ItemData ItemTabData
	{
		get
		{
			return _itemData;
		}
		set
		{
			_itemData = value;
		}
	}

	private void OnClose(ButtonScript obj, object args, int param1, int param2)
	{
		tipPene.SetActive (false);
		if(splitUI != null)
			splitUI.gameObject.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback();
		}
		
	}
	private void onSellBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(isleft)
		{

			NetConnection.Instance.storageItemToBag(Item.instId_);
		}else
		{
			ItemData idata = ItemData.GetData ((int)Item.itemId_);
			if(idata.mainType_ == ItemMainType.IMT_Quest)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("renwudaoju"));
				return;
			}

			NetConnection.Instance.depositItemToStorage (Item.instId_);
		}

		bagCell.itemIcon.gameObject.SetActive (false);
		bagCell.countLab.gameObject.SetActive (false);
		bagCell.pane.spriteName = "bb_daojukuang1";
		BankUI.sellItemList.Add (bagCell.Item);
		UIManager.RemoveButtonAllEventHandler (bagCell.pane.gameObject);
		gameObject.SetActive (false);
	}
	

	private void onShowItem(ButtonScript obj, object args, int param1, int param2)
	{
		//NetConnection.Instance.showItem (Item.instId_);
	}
}
