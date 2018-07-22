using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void CloseCallBack();
public delegate void openFuwen(int id);
public class BagTipsUI : MonoBehaviour
{
	public CloseCallBack closeCallback;
	public openFuwen openHCFuwen;
	public GameObject tipPene;
	public UIButton UseBtn;
	public UILabel nameLab;
	public UILabel descLab;
	public UILabel propLab;
	public UILabel getWayLab;
	public UITexture icon;
	public UIButton DemountBtn;
	public UIButton EquipBtn;
	public UISprite tipsBg;
	public UIButton showBtn;
	public UILabel levelLab;
	public UILabel durabilityLab;
	public UILabel typeLab;
	public UISprite propImg;
	public UIButton useAllBtn;
	public UIButton excreteBtn;
	public BagSplitUI splitUI;
	public UISprite tipsImg;
	public UIButton sellBtn;
	public UIButton fixBtn;
	public UILabel naijiuLab;
	public UILabel naijiuWaring;
	public GameObject naijiuObj;
	public UISprite iconBack;
	public UIGrid grid;
	public GameObject propCell;
	public UISprite xiuImg;
	public UILabel naijiuDesc;

	public UILabel bagTipsSellBtnLab;
	public UILabel bagTipsUseBtnLab;
	public UILabel bagTipsCaiBtnLab;
	public UILabel bagTipsDemountBtnLab;
	public UILabel bagTipsFixBtnLab;
	public UILabel bagTipsLevelLab;
	public UILabel bagTipsTypeLab;
	public UILabel bagTipsNaijuLab;
	public UILabel bagTipsGetWayLab;
	public UILabel bagTipsGetWayTextLab;
	public UISprite suoImg;
	public UIButton lockBtn;
	public UIButton cancelLockBtn;
	public UILabel lastTime;
	public UILabel bindLab;
	public UIButton fuwenBtn;
	public UIButton fuwenHCBtn;


	int _ItemId;
	private COM_Item _itemInst;
	private ItemData _itemData;
	public BagCellUI bagCell;
	private bool _isSell;
	private uint _playerInstId;
	private List<GameObject> cellList = new List<GameObject>();
	private List<GameObject> cellPool = new List<GameObject>();


	void Start ()
	{
		UIManager.SetButtonEventHandler (tipsBg.gameObject, EnumButtonEvent.OnClick, OnClose,0, 0);
		UIManager.SetButtonEventHandler (UseBtn.gameObject, EnumButtonEvent.OnClick, OnUseBtn,0, 0);
		if(lockBtn != null)
			UIManager.SetButtonEventHandler (lockBtn.gameObject, EnumButtonEvent.OnClick, OnLockBtn,0, 0);
		if(cancelLockBtn != null)
			UIManager.SetButtonEventHandler (cancelLockBtn.gameObject, EnumButtonEvent.OnClick, OnCancelLockBtn,0, 0);
		if (DemountBtn != null)
		    UIManager.SetButtonEventHandler (DemountBtn.gameObject, EnumButtonEvent.OnClick, OnClickdemount,0, 0);
		UIManager.SetButtonEventHandler (EquipBtn.gameObject, EnumButtonEvent.OnClick, OnClickEquipBtn,0, 0);
		UIManager.SetButtonEventHandler (useAllBtn.gameObject, EnumButtonEvent.OnClick, onUseAll, 0, 0);
		UIManager.SetButtonEventHandler (excreteBtn.gameObject, EnumButtonEvent.OnClick, onSplit, 0, 0);
		//UIManager.SetButtonEventHandler (showBtn.gameObject, EnumButtonEvent.OnClick, onShowItem, 0, 0);
		UIManager.SetButtonEventHandler (sellBtn.gameObject, EnumButtonEvent.OnClick, onSellBtn, 0, 0);
		UIManager.SetButtonEventHandler (fixBtn.gameObject, EnumButtonEvent.OnClick, onFixBtn, 0, 0);
		if(fuwenBtn != null)
			UIManager.SetButtonEventHandler (fuwenBtn.gameObject, EnumButtonEvent.OnClick, onFuWenBtn, 0, 0);

		if(fuwenHCBtn != null)
			UIManager.SetButtonEventHandler (fuwenHCBtn.gameObject, EnumButtonEvent.OnClick, onFuwenHCBtnBtn, 0, 0);

		bagTipsSellBtnLab.text = LanguageManager.instance.GetValue ("bagTipsSellBtnLab");
		bagTipsUseBtnLab.text = LanguageManager.instance.GetValue("bagTipsUseBtnLab");
		bagTipsCaiBtnLab.text = LanguageManager.instance.GetValue("bagTipsCaiBtnLab");
		bagTipsDemountBtnLab.text = LanguageManager.instance.GetValue("bagTipsDemountBtnLab");
		bagTipsFixBtnLab.text = LanguageManager.instance.GetValue("bagTipsFixBtnLab");
		bagTipsLevelLab.text = LanguageManager.instance.GetValue("bagTipsLevelLab");
		bagTipsTypeLab.text = LanguageManager.instance.GetValue("bagTipsTypeLab");
		bagTipsNaijuLab.text = LanguageManager.instance.GetValue("bagTipsNaijuLab");
		bagTipsGetWayLab.text = LanguageManager.instance.GetValue("bagTipsGetWayLab");
		bagTipsGetWayTextLab.text = LanguageManager.instance.GetValue("bagTipsGetWayTextLab");

        CheckTips();
	}

    void CheckTips()
    {
        if (_itemInst != null)
        {
            ItemData data = ItemData.GetData((int)_itemInst.itemId_);
            if (data == null)
                return;

            if (data.mainType_ == ItemMainType.IMT_Equip)
            {
				if (GamePlayer.Instance.Equips[(int)data.slot_] != null)
                {
					if (GamePlayer.Instance.Equips[(int)data.slot_].instId_ == _itemInst.instId_)
                    {
                        return;
                    }
                }
            }
            GuideManager.Instance.RegistGuideAim(UseBtn.gameObject, GuideAimType.GAT_MainBagTipUseItem);
            GuideManager.Instance.RegistGuideAim(EquipBtn.gameObject, GuideAimType.GAT_MainbagTipEquip);

            GuideManager.Instance.RegistGuideAim(fuwenHCBtn.gameObject, GuideAimType.GAT_MainBagFuwenTipsCombieBtn);
            GuideManager.Instance.RegistGuideAim(fuwenBtn.gameObject, GuideAimType.GAT_MainBagFuwenTipsInsertBtn);

            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagTipOpen);
        }
    }

	void OnEnable()
	{
        CheckTips();
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
				if(_itemInst.isBind_)
				{
					if(bindLab != null)
						bindLab.gameObject.SetActive(true);
				}
				else
				{
					if(bindLab != null)
						bindLab.gameObject.SetActive(false);
				}
				if(iconBack != null)
					iconBack.spriteName = BagSystem.instance.GetQualityBack((int)data.quality_);

				if ((int)data.quality_ >= (int)QualityColor.QC_Orange2)
				{
					nameLab.color = Color.white;
					nameLab.text = "[fa6400]" + nameLab.text;
				} 
				if((int)data.quality_ <= (int)QualityColor.QC_White)
				{
					nameLab.color = Color.gray;
				}
				else if ((int)data.quality_ <= (int)QualityColor.QC_Green)
				{
					nameLab.color = Color.white;
					nameLab.text = "[39b31d]" + nameLab.text;
				}
				else if((int)data.quality_ <= (int)QualityColor.QC_Blue1)
				{
					nameLab.color = Color.white;
					nameLab.text = "[346da0]" + nameLab.text;
				}
				else if ((int)data.quality_ <= (int)QualityColor.QC_Purple2)
				{
					nameLab.color = Color.white;
					nameLab.text = "[bd58f4]" + nameLab.text;
				}
				else if ((int)data.quality_ <= (int)QualityColor.QC_Golden2)
				{
					nameLab.color = Color.white;
					nameLab.text = "[ba9207]" + nameLab.text;
				}
				else if ((int)data.quality_ <= (int)QualityColor.QC_Orange2)
				{
					nameLab.color = Color.white;
					nameLab.text = "[fa6400]" + nameLab.text;
				}
				else if ((int)data.quality_ <= (int)QualityColor.QC_Pink)
				{
					nameLab.color = Color.white;
					nameLab.text = "[e65652]" + nameLab.text;
				}

				if(!string.IsNullOrEmpty(data.desc_))
				{
					descLab.text = LanguageManager.instance.GetValue("tipsmisoshu") +data.desc_;
				}
				else
				{
					descLab.text = "";
				}

				getWayLab.text = data.acquiringWay_;
				HeadIconLoader.Instance.LoadIcon( data.icon_,icon);
				levelLab.text = data.level_.ToString();
				typeLab.text = LanguageManager.instance.GetValue(data.subType_.ToString());
				naijiuLab.text = _itemInst.durability_+"/"+_itemInst.durabilityMax_;
				naijiuDesc.gameObject.SetActive(false);
				naijiuObj.gameObject.SetActive(false);
				if(data.mainType_ == ItemMainType.IMT_Equip)
				{
					naijiuDesc.gameObject.SetActive(true);
					naijiuObj.gameObject.SetActive(true);
				}
				if(_itemInst.propArr.Length > 0)
				{
					for( int o = 0;o<cellList.Count;o++)
					{
						grid.RemoveChild(cellList[o].transform);
						cellList[o].transform.parent = null;
						cellList[o].gameObject.SetActive(false);
						cellPool.Add(cellList[o]);
					}
					cellList.Clear ();
				
					propImg.gameObject.SetActive(true);
					for(int i=0;i<_itemInst.propArr.Length;i++)
					{
						if(_itemInst.propArr[i].type_ == PropertyType.PT_Durability)
						{
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
							objCell = Object.Instantiate(propCell) as GameObject;
						}


						//for(int a=0;a<data.propArr.Count;a++)
						//{
							//if(_itemData.propArr[a].Key == _itemInst.propArr[i].type_)
							//{
						string sNum = "";
						if(_itemInst.propArr[i].value_ > 0)
						{
							sNum = " +" + ((int)_itemInst.propArr[i].value_);
						}
						else
						{
							sNum =  " "+((int)_itemInst.propArr[i].value_).ToString();
						}
						/*if(_itemInst.propArr[i].value_  == float.Parse(_itemData.propArr[a].Value[1]))
						{
							objCell.transform.FindChild("name").GetComponent<UILabel>().text  =
								"[E845EB]"+
									LanguageManager.instance.GetValue(_itemInst.propArr[i].type_.ToString())+ sNum+"[-]";
						} 
						else
						{
						*/
							UILabel lable =	objCell.transform.FindChild("name").GetComponent<UILabel>();
							lable.text  =  LanguageManager.instance.GetValue(_itemInst.propArr[i].type_.ToString())+sNum;
							if(_itemInst.durability_ < _itemInst.durabilityMax_/2)
							{
								lable.color = Color.red;
							}
							else if(ItemData.GetData((int)_itemInst.itemId_).mainType_ == ItemMainType.IMT_FuWen)
							{
								lable.color = Color.grey;
							}
							else
							{
								float perNum = EquipColorData.GetEquipPerNum(_itemData.level_, _itemInst.propArr[i].type_,_itemInst.propArr[i].value_);
								
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
								//}
								//break;
							//}

						//}

					//	objCell.transform.parent = grid.transform; 
						grid.AddChild(objCell.transform);
						objCell.transform.localScale = Vector3.one;
						objCell.gameObject.SetActive(true);
						cellList.Add(objCell);

					}
					grid.Reposition();
				}
				else
				{
					propImg.gameObject.SetActive(false);
				}

				if(data.canUse_)
				{
					if(data.usedFlag_ == ItemUseFlag.IUF_Battle)
					{
						if(!GamePlayer.Instance.isInBattle)
						{
							UseBtn.gameObject.SetActive(false);
							EquipBtn.gameObject.SetActive(false);
							useAllBtn.gameObject.SetActive(false);
							DemountBtn.gameObject.SetActive(false);
							fixBtn.gameObject.SetActive (false);
							xiuImg.gameObject.SetActive(false);
							if(fuwenBtn != null)
								fuwenBtn.gameObject.SetActive(false);
							if(fuwenHCBtn != null)
								fuwenHCBtn.gameObject.SetActive(false);
							if(data.maxCount_ > 1 && _itemInst.stack_ > 1)
							{
								excreteBtn.gameObject.SetActive(true);

							}
							else
							{
								excreteBtn.gameObject.SetActive(false);
							}
							return;
						}
					}
                    else if (data.usedFlag_ == ItemUseFlag.IUF_Scene)
					{
						if(GamePlayer.Instance.isInBattle)
						{
							UseBtn.gameObject.SetActive(false);
							EquipBtn.gameObject.SetActive(false);
							useAllBtn.gameObject.SetActive(false);
							excreteBtn.gameObject.SetActive(false);
							DemountBtn.gameObject.SetActive(false);
							fixBtn.gameObject.SetActive (false);
							xiuImg.gameObject.SetActive(false);
							if(fuwenBtn != null)
								fuwenBtn.gameObject.SetActive(false);
							if(fuwenHCBtn != null)
								fuwenHCBtn.gameObject.SetActive(false);
							return;
						}
					}


					if(data.mainType_ == ItemMainType.IMT_Equip)
					{
						UseBtn.gameObject.SetActive(false);
						EquipBtn.gameObject.SetActive(true);
                        if(DemountBtn != null)
						    DemountBtn.gameObject.SetActive(false);
						fixBtn.gameObject.SetActive (false);
						xiuImg.gameObject.SetActive(false);
						useAllBtn.gameObject.SetActive(false);
						excreteBtn.gameObject.SetActive(false);
						if(fuwenBtn != null)
						fuwenBtn.gameObject.SetActive(false);
						if(fuwenHCBtn != null)
							fuwenHCBtn.gameObject.SetActive(false);
						
					}
					else if(data.mainType_ == ItemMainType.IMT_FuWen)
					{
						if(fuwenHCBtn != null)
							fuwenHCBtn.gameObject.SetActive(true);
						if(fuwenBtn != null)
						fuwenBtn.gameObject.SetActive(true);
						UseBtn.gameObject.SetActive(false);
						EquipBtn.gameObject.SetActive(false);
						if(DemountBtn != null)
							DemountBtn.gameObject.SetActive(false);
						fixBtn.gameObject.SetActive (false);
						xiuImg.gameObject.SetActive(false);
						useAllBtn.gameObject.SetActive(false);
						//excreteBtn.gameObject.SetActive(false);
					}
					else
					{
						UseBtn.gameObject.SetActive(true);
						EquipBtn.gameObject.SetActive(false);
						useAllBtn.gameObject.SetActive(true);
						excreteBtn.gameObject.SetActive(false);
						DemountBtn.gameObject.SetActive(false);
						fixBtn.gameObject.SetActive (false);
						xiuImg.gameObject.SetActive(false);
						if(fuwenBtn != null)
							fuwenBtn.gameObject.SetActive(false);
						if(fuwenHCBtn != null)
							fuwenHCBtn.gameObject.SetActive(false);

					}	
					if(data.subType_ == ItemSubType.IST_Lottery || data.maxCount_ == 1)
					{
						useAllBtn.gameObject.SetActive(false);
					}
				}
				else
				{
					UseBtn.gameObject.SetActive(false);
					EquipBtn.gameObject.SetActive(false);
					useAllBtn.gameObject.SetActive(false);
					excreteBtn.gameObject.SetActive(false);
					DemountBtn.gameObject.SetActive(false);
					fixBtn.gameObject.SetActive (false);
					xiuImg.gameObject.SetActive(false);
					if(fuwenBtn != null)
						fuwenBtn.gameObject.SetActive(false);
					if(fuwenHCBtn != null)
						fuwenHCBtn.gameObject.SetActive(false);
				}

				if(data.maxCount_ > 1 && _itemInst.stack_ > 1)
				{
					if(data.mainType_ != ItemMainType.IMT_FuWen)
						excreteBtn.gameObject.SetActive(true);
				}
				else
				{
					excreteBtn.gameObject.SetActive(false);
				}

				if(GamePlayer.Instance.isInBattle)
				{
					useAllBtn.gameObject.SetActive(false);
				}


				if (GamePlayer.Instance.isInBattle && data.mainType_ == ItemMainType.IMT_Equip)
				{
					if(data.slot_ != EquipmentSlot.ES_SingleHand && data.slot_ != EquipmentSlot.ES_DoubleHand)
					{
						EquipBtn.gameObject.SetActive(false);
					}
				}
				//for(int i= 0;i<Item.PropArr.Length;i++)
				//{
//					propLab.text = Item.propArr[0].value_.ToString();
				//}
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


	public void ShowDemountBtn()
	{
		UseBtn.gameObject.SetActive(false);
		DemountBtn.gameObject.SetActive(true);
		if(fuwenHCBtn != null)
			fuwenHCBtn.gameObject.SetActive(false);
		if(fuwenBtn != null)
		fuwenBtn.gameObject.SetActive(false); 
		if(ItemData.GetData((int)Item.itemId_).slot_ == EquipmentSlot.ES_Ornament_0 ||ItemData.GetData((int)Item.itemId_).slot_ == EquipmentSlot.ES_Ornament_1 || ItemData.GetData((int)Item.itemId_).mainType_ == ItemMainType.IMT_FuWen)
		{
			fixBtn.gameObject.SetActive(false);
		}
		else
		{
			if(Item.durability_ <= Item.durabilityMax_*0.8)
			{
				fixBtn.gameObject.SetActive (true);
				xiuImg.gameObject.SetActive(true);
				if(Item.durability_ <= Item.durabilityMax_*0.5)
					xiuImg.spriteName = "huai";
				else
					xiuImg.spriteName = "xiu";
			}
		}
		EquipBtn.gameObject.SetActive (false);
		sellBtn.gameObject.SetActive (false);
		//showBtn.gameObject.SetActive (false);
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

	private void OnLockBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if ( ItemData.GetData ((int)Item.itemId_).mainType_ == ItemMainType.IMT_Equip || ItemData.GetData ((int)Item.itemId_).mainType_ == ItemMainType.IMT_FuWen 
			    || ItemData.GetData ((int)Item.itemId_).mainType_ == ItemMainType.IMT_EmployeeEquip || ItemData.GetData ((int)Item.itemId_).mainType_ == ItemMainType.IMT_BabyEquip)
		{
			if( !Item.isLock_)
			{
				NetConnection.Instance.lockItem((int)Item.instId_,true);
			}
		}
	}

	private void OnCancelLockBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if( Item.isLock_)
		{
			NetConnection.Instance.lockItem((int)Item.instId_,false);
		}
	}

	private void OnUseBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (Item == null) 
		{
			return;
		}
		ItemData itemTabel = ItemData.GetData ((int)Item.itemId_);
		if (itemTabel == null)
		{
			return;
		}

		GlobalValue.Get(Constant.C_WishItem, out _ItemId);
		if(itemTabel.id_ == _ItemId)
		{
			if(!Prebattle.Instance.IsWishingAvailable())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("xuyuanre"));
				return;
			}
			WishingTreeUI.SwithShowMe();
			return;
		}

		if((!GamePlayer.Instance.isInBattle) &&  ItemData.GetData( (int)Item.itemId_).subType_  == ItemSubType.IST_Blood)
		{
			BagUI.Instance.bagUsePanel.Show ();
			BagUI.Instance.bagUsePanel._item = Item;
			BagUI.Instance.bagUsePanel.stack  =1;
			tipPene.SetActive (false);
			if(closeCallback != null)
			{
				closeCallback();
			}

			return;
		}


        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_UseItem, (int)Item.itemId_);
		if(GamePlayer.Instance.isInBattle)
		{

			Battle.Instance.UseItem((int)Item.instId_);
			BagSystem.instance.battleOpenBag = false;
            BagUI.HideMe();
		}
		else
		{
			if(itemTabel.mainType_ == ItemMainType.IMT_Consumables)
			{
				/*if(itemTabel.subType_ == ItemSubType.IST_BabyExp)
				{
					if(GamePlayer.Instance.MaxUseAllNum(Item,1) > 0)
					{
						NetConnection.Instance.useItem (Item.slot_,(uint)GamePlayer.Instance.InstId, 1);
					}else
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("expItem"));  
					}
				}
				else 
			*/
				if(itemTabel.subType_ == ItemSubType.IST_SkillExp)
				{
					if(!GamePlayer.Instance.IsCanUseSkillExpItem())
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("EN_NoUpSkill"));
					}
					else
					{
						NetConnection.Instance.useItem ((uint)Item.slot_,(uint)GamePlayer.Instance.InstId, 1);
					}
				}
				else
				{

					NetConnection.Instance.useItem ((uint)Item.slot_,(uint)GamePlayer.Instance.InstId, 1);
				}


			}
		}
		tipPene.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback(); 
		}

//		COM_ChatInfo info = new COM_ChatInfo ();
//		info.ck_ = ChatKind.CK_System;
//		info.content_ = LanguageManager.instance.GetValue("delitem").Replace("{n}",itemTabel.name_);
        ChatSystem.PushSystemMessage(LanguageManager.instance.GetValue("delitem").Replace("{n}", itemTabel.name_));
	}

	private void OnClickdemount(ButtonScript obj, object args, int param1, int param2)
	{
		if (Item == null) 
		{
			return;
		}

		//if(BagSystem.instance.GetBagSize() >= GamePlayer.Instance.GetIprop(PropertyType.PT_BagNum))
		//{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("bagfull"));
			//PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			///return;
		//}
		if(ItemData.GetData((int)Item.itemId_).mainType_ == ItemMainType.IMT_FuWen)
		{
			NetConnection.Instance.takeoffFuwen(Item.slot_);
			tipPene.SetActive (false);
			if(closeCallback != null)
			{    
				closeCallback();
			}
			return;
		}    

        if (GamePlayer.Instance.isInBattle)
        {
            Battle.Instance.UseItem((int)Item.instId_);
			BagSystem.instance.battleOpenBag = false;
            BagUI.HideMe();
        }
        else
			NetConnection.Instance.delEquipment (PlayerInstId , Item.instId_);

		tipPene.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback();
		}

	} 


	private void OnClickEquipBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(Item == null)
		{
			return;
		} 
		if(_itemData == null || _itemData.mainType_ != ItemMainType.IMT_Equip)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("zhaungbeileixingcuowu"));
			return;
		}

		Entity player = null;
		if(PlayerInstId == GamePlayer.Instance.InstId)
		{
			player = GamePlayer.Instance;
		}
		else
		{
			player = GamePlayer.Instance.GetEmployeeById((int)PlayerInstId);
		}


		if(player.GetIprop(PropertyType.PT_Level) /10 +1 < _itemData.level_)
		{
			//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("equipLevel"));
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
			return;
		}

		JobType jt = (JobType)player.GetIprop(PropertyType.PT_Profession);
		int level = player.GetIprop(PropertyType.PT_ProfessionLevel);
        Profession profession = Profession.get(jt, level);
        if (null == profession)
            return;



		if(_itemData.slot_ == EquipmentSlot.ES_Ornament_0 || _itemData.slot_ == EquipmentSlot.ES_Ornament_1)
		{

		}
		else
		{
			if (!profession.canuseItem(_itemData.subType_, _itemData.level_))
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("equipProfession"));
				return;
			}
		}

		if(!Item.isBind_ && _itemData.bindType_ == BindType.BIT_Use)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("shifoubangding"),()=>{

				if (GamePlayer.Instance.isInBattle)
				{
					Battle.Instance.UseItem((int)Item.instId_);
					BagSystem.instance.battleOpenBag = false;
					BagUI.HideMe();
				}
				//else
				//NetConnection.Instance.wearEquipment(PlayerInstId, Item.instId_);
				
				if(_itemData.slot_ == EquipmentSlot.ES_SingleHand)
				{
					if(_itemData.subType_ == ItemSubType.IST_Shield)
					{
						if(player.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
					else
					{
						if(player.Equips[(int)EquipmentSlot.ES_DoubleHand] != null && ItemData.GetData((int)player.Equips[(int)EquipmentSlot.ES_DoubleHand].itemId_).subType_ != ItemSubType.IST_Shield)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
				}
				else if(_itemData.slot_ ==EquipmentSlot.ES_DoubleHand)
				{
					if(player.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_SingleHand].instId_);
					}
				}
				else if(_itemData.slot_ == EquipmentSlot.ES_Ornament_0 ||_itemData.slot_ == EquipmentSlot.ES_Ornament_1 )
				{
					
					if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null && GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
					}
					else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(uint)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if(GamePlayer.Instance.Equips[(uint)EquipmentSlot.ES_Ornament_1] != null)
						{
							if(ItemData.GetData((int)(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_)).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
							}
						}
					}
					else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null )
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
							}
						}
					}
				}
				
				if(!GamePlayer.Instance.isInBattle)
					NetConnection.Instance.wearEquipment(PlayerInstId, Item.instId_);
				tipPene.SetActive (false);
				if(closeCallback != null)
				{
					closeCallback();
				}
				
				GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EquipItem);


			},false,()=>{
				tipPene.SetActive (false);
				if(closeCallback != null)
				{
					closeCallback();
				}
			});
		}
		else
		{

			if (GamePlayer.Instance.isInBattle)
			{
				Battle.Instance.UseItem((int)Item.instId_);
				BagSystem.instance.battleOpenBag = false;
				BagUI.HideMe();
			}
			//else
			//NetConnection.Instance.wearEquipment(PlayerInstId, Item.instId_);
			
			if(_itemData.slot_ == EquipmentSlot.ES_SingleHand)
			{
				if(_itemData.subType_ == ItemSubType.IST_Shield)
				{
					if(player.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
					}
				}
				else
				{
					if(player.Equips[(int)EquipmentSlot.ES_DoubleHand] != null && ItemData.GetData((int)player.Equips[(int)EquipmentSlot.ES_DoubleHand].itemId_).subType_ != ItemSubType.IST_Shield)
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
					}
				}
			}
			else if(_itemData.slot_ ==EquipmentSlot.ES_DoubleHand)
			{
				if(player.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
				{
					NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_SingleHand].instId_);
				}
			}
			else if(_itemData.slot_ == EquipmentSlot.ES_Ornament_0 ||_itemData.slot_ == EquipmentSlot.ES_Ornament_1 )
			{
				
				if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null && GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
				{
					if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
					}
					else if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
					}
					else
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
					}
				}
				else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
				{
					if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(uint)EquipmentSlot.ES_Ornament_0].instId_);
					}
					else if(GamePlayer.Instance.Equips[(uint)EquipmentSlot.ES_Ornament_1] != null)
					{
						if(ItemData.GetData((int)(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_)).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
					}
				}
				else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null )
				{
					if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
					{
						NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
					}
					else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment(PlayerInstId ,player.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
					}
				}
			}
			
			if(!GamePlayer.Instance.isInBattle)
				NetConnection.Instance.wearEquipment(PlayerInstId, Item.instId_);
			
			tipPene.SetActive (false);
			if(closeCallback != null)
			{
				closeCallback();
			}
			
			GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EquipItem);

		}

	}

	public void EquipEQuip()
	{

	}

	public bool shoWSell
	{
		set
		{
			_isSell = value;
			UseBtn.gameObject.SetActive(_isSell);
			EquipBtn.gameObject.SetActive(_isSell);
			useAllBtn.gameObject.SetActive(_isSell);
			excreteBtn.gameObject.SetActive(_isSell);
			DemountBtn.gameObject.SetActive(_isSell);
			//showBtn.gameObject.SetActive(_isSell);
			if(_isSell || ItemTabData.mainType_ == ItemMainType.IMT_Quest)
				sellBtn.gameObject.SetActive(false);
			else
				sellBtn.gameObject.SetActive(true);
		}
		get
		{
			return _isSell;
		}
	}

	private void onFixBtn(ButtonScript obj, object args, int param1, int param2)
	{
		FixUI.ShowMe ((int)_itemInst.instId_);
		tipPene.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback(); 
		}
	}

	public void onFuWenBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(ItemData.GetData((int)Item.itemId_).mainType_ != ItemMainType.IMT_FuWen)
		{
			return;
		}
		NetConnection.Instance.wearFuwen ((int)Item.instId_);
		tipPene.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback();
		}

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BagFuwenClickTipsInsertBtn);
	}

	public void onFuwenHCBtnBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (openHCFuwen != null)
			openHCFuwen ((int)Item.instId_);

		tipPene.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback();
		}
	}

	private void onSellBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (ItemData.GetData ((int)bagCell.Item.itemId_).price_ <= 0 || ItemData.GetData((int)bagCell.Item.itemId_).mainType_ == ItemMainType.IMT_Quest || Item.isLock_ )
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("cantsell"));
			tipPene.SetActive (false);
			if(closeCallback != null)
			{
				closeCallback(); 
			}
			return;
		}


		if(BagUI.Instance.sellItemList.Count>= 16)
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("sellMax"));
			tipPene.SetActive (false);
			if(closeCallback != null)
			{
				closeCallback(); 
			}
			return;
		}


		GameObject sellObj = null;
		bagCell.itemIcon.gameObject.SetActive(false);
		bagCell.countLab.gameObject.SetActive(false);
		bagCell.debirsImg.gameObject.SetActive(false);
		bagCell.suoImg.gameObject.SetActive(false);
		bagCell.pane.spriteName = "bb_daojukuang1";
		bagCell.countLab.gameObject.SetActive(false);
		UIManager.RemoveButtonAllEventHandler ( bagCell.pane.gameObject);
		
		sellObj = Object.Instantiate(bagCell.gameObject) as GameObject;
		sellObj.GetComponent<BagCellUI>().Item =  bagCell.Item;
		sellObj.transform.parent = BagUI.Instance.sellGrid.transform;
		sellObj.SetActive(true);
		BagUI.Instance.sellItemList.Add( bagCell.Item);
		BagUI.Instance.AddsellCell(sellObj);
		sellObj.transform.localScale = Vector3.one;
		sellObj.transform.localPosition = Vector3.one;
		BagUI.Instance.sellGrid.Reposition();
		
		int money = 0;
		List<COM_Item> sellList = BagUI.Instance.sellItemList;
		for(int x = 0;x<sellList.Count;x++)
		{
			money += ItemData.GetData((int)sellList[x].itemId_).price_ * sellList[x].stack_;
		}
		BagUI.Instance.sellMoneyLab.text = money.ToString();
		tipPene.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback(); 
		}
	}

	private void onUseAll(ButtonScript obj, object args, int param1, int param2)
	{
		ItemData idata = ItemData.GetData ((int)Item.itemId_);

		if(!GamePlayer.Instance.isInBattle && Item.itemId_ == 5023 || Item.itemId_ == 5004)
		{
			BagUI.Instance.bagUsePanel.Show ();
			BagUI.Instance.bagUsePanel._item = Item;
			BagUI.Instance.bagUsePanel.stack = (uint)Item.stack_;
			tipPene.SetActive (false);
			if(closeCallback != null)
			{
				closeCallback();
			}
			return;
		}

		/*if(idata.subType_ == ItemSubType.IST_BabyExp)
		{
			if(GamePlayer.Instance. MaxUseAllNum(Item,Item.stack_) ==0)
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("expItem"));
				return;
			}
			if( Item.stack_ <= GamePlayer.Instance. MaxUseAllNum(Item,Item.stack_) )
			{
				NetConnection.Instance.useItem (Item.slot_,(uint)GamePlayer.Instance.InstId, (uint)Item.stack_);
			}
			if( Item.stack_ >= GamePlayer.Instance. MaxUseAllNum(Item,Item.stack_) )
			{
				NetConnection.Instance.useItem (Item.slot_,(uint)GamePlayer.Instance.InstId, (uint)GamePlayer.Instance.MaxUseAllNum(Item,Item.stack_));
			}
		}
		else */
		if(idata.subType_ == ItemSubType.IST_SkillExp)
		{
			if(!GamePlayer.Instance.IsCanUseSkillExpItem())
			{
				PopText.Instance.Show(LanguageManager.instance.GetValue("EN_NoUpSkill"));
				return;
			}
			else
			{
				NetConnection.Instance.useItem ((uint)Item.slot_,(uint)GamePlayer.Instance.InstId, (uint)Item.stack_);
			}
		}
		else
		{
			NetConnection.Instance.useItem((uint)Item.slot_,(uint)GamePlayer.Instance.InstId, (uint)Item.stack_);
		}

      //  NetConnection.Instance.useItem(Item.slot_,(uint)GamePlayer.Instance.InstId, (uint)Item.stack_);
		tipPene.gameObject.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback();
		}
	}

	private void onSplit(ButtonScript obj, object args, int param1, int param2)
	{
		if(BagSystem.instance.BagIsFull())
		{
			PopText.Instance.Show(LanguageManager.instance.GetValue("bagfull"));
			return;
		}

		tipsImg.gameObject.SetActive (false);
		if(closeCallback != null)
		{
			closeCallback();
		}
		//tipPene.SetActive (false);
		splitUI.ItemInst = _itemInst;
		splitUI.gameObject.SetActive (true);
	}

	private void onShowItem(ButtonScript obj, object args, int param1, int param2)
	{
		//NetConnection.Instance.showItem (Item.instId_);
	}
}

