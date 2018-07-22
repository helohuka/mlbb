using UnityEngine;
using System.Collections;

public class FastUsePanel : MonoBehaviour {

    public UILabel title_;
    public UIButton closeBtn_;
    public UIButton useBtn_;
    public UILabel useBtnLabel_;
    public UISprite icon_;
    public UISprite btnSp_;

    COM_Item item_;
	// Use this for initialization
	void Start () {
        UIManager.SetButtonEventHandler(closeBtn_.gameObject, EnumButtonEvent.OnClick, (ButtonScript obj, object args, int param1, int param2) =>
        {
			GamePlayer.Instance.isUseCloseBtn = true;
            gameObject.SetActive(false);
        }, 0, 0);
	}

	public void OnUseBtnClick()
	{
		ItemData data = ItemData.GetData((int)item_.itemId_);
		if (data.mainType_ == ItemMainType.IMT_Equip)
		{
			if (data.slot_ == EquipmentSlot.ES_SingleHand)
			{
				if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
				{
					return;
				}
			}
			else if (data.slot_ == EquipmentSlot.ES_DoubleHand)
			{
				if (GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
				{
					return;
				}
			}
			
			title_.text = data.name_;
			useBtnLabel_.text = "装备";
			useBtn_.normalSprite = "lvanniu";
			btnSp_.spriteName = "lvanniu";
			UIManager.RemoveButtonEventHandler(useBtn_.gameObject, EnumButtonEvent.OnClick);
			UIManager.SetButtonEventHandler(useBtn_.gameObject, EnumButtonEvent.OnClick, (ButtonScript obj, object args, int param1, int param2) =>
			                                {
				
				ItemData _itemData = ItemData.GetData((int)item_.itemId_);
				
				if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) /10 +1 < _itemData.level_)
				{
					//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("equipLevel"));
					PopText.Instance.Show(LanguageManager.instance.GetValue("equipLevel"));
					return;
				}
				
				JobType jt = (JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
				int level = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);
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
				
				if(_itemData.slot_ == EquipmentSlot.ES_SingleHand)
				{
					if(_itemData.subType_ == ItemSubType.IST_Shield)
					{
						if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
					else
					{
						if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand] != null && ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].itemId_).subType_ != ItemSubType.IST_Shield)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_DoubleHand].instId_);
						}
					}
				}
				else if(_itemData.slot_ ==EquipmentSlot.ES_DoubleHand)
				{
					if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand] != null)
					{
						NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_SingleHand].instId_);
					}
				}
				else if(_itemData.slot_ == EquipmentSlot.ES_Ornament_0 ||_itemData.slot_ == EquipmentSlot.ES_Ornament_1 )
				{
					
					if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null && GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
					}
					else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
						else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null)
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
							}
						}
					}
					else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1] != null )
					{
						if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_)
						{
							NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
						else if(GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0] != null)
						{
							if(ItemData.GetData((int)GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_)
							{
								NetConnection.Instance.delEquipment((uint)GamePlayer.Instance.InstId ,GamePlayer.Instance.Equips[(int)EquipmentSlot.ES_Ornament_0].instId_);
							}
						}
					}
				}
				
				NetConnection.Instance.wearEquipment((uint)GamePlayer.Instance.InstId, item_.instId_);
				gameObject.SetActive(false);
			}, 0, 0);
		}
		else if (data.subType_ == ItemSubType.IST_Buff)
		{
			title_.text = data.name_;
			useBtnLabel_.text = "使用";
			useBtn_.normalSprite = "huanganniu";
			btnSp_.spriteName = "huanganniu";
			UIManager.RemoveButtonEventHandler(useBtn_.gameObject, EnumButtonEvent.OnClick);
			UIManager.SetButtonEventHandler(useBtn_.gameObject, EnumButtonEvent.OnClick, (ButtonScript obj, object args, int param1, int param2) =>
			                                {
				NetConnection.Instance.useItem((uint)item_.slot_, (uint)GamePlayer.Instance.InstId, (uint)1);
				gameObject.SetActive(false);
			}, 0, 0);
		}
		
		UIManager.Instance.AddItemCellUI(icon_, item_.itemId_);
		gameObject.SetActive(true);
		
	}
	public void OnUseClick()
	{
		ItemData data = ItemData.GetData((int)item_.itemId_);
		if (data.mainType_ == ItemMainType.IMT_Equip) {
			if (data.slot_ == EquipmentSlot.ES_SingleHand) {
				if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_DoubleHand] != null) {
					return;
				}
			} else if (data.slot_ == EquipmentSlot.ES_DoubleHand) {
				if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_SingleHand] != null) {
					return;
				}
			}
			
			ItemData _itemData = ItemData.GetData ((int)item_.itemId_);
			
			if (GamePlayer.Instance.GetIprop (PropertyType.PT_Level) / 10 + 1 < _itemData.level_) {
				//ErrorTipsUI.ShowMe(LanguageManager.instance.GetValue("equipLevel"));
				PopText.Instance.Show (LanguageManager.instance.GetValue ("equipLevel"));
				return;
			}
			
			JobType jt = (JobType)GamePlayer.Instance.GetIprop (PropertyType.PT_Profession);
			int level = GamePlayer.Instance.GetIprop (PropertyType.PT_ProfessionLevel);
			Profession profession = Profession.get (jt, level);
			if (null == profession)
				return;
			
			if (_itemData.slot_ == EquipmentSlot.ES_Ornament_0 || _itemData.slot_ == EquipmentSlot.ES_Ornament_1) {
				
			} else {
				if (!profession.canuseItem (_itemData.subType_, _itemData.level_)) {
					PopText.Instance.Show (LanguageManager.instance.GetValue ("equipProfession"));
					return;
				}
			}
			
			if (_itemData.slot_ == EquipmentSlot.ES_SingleHand) {
				if (_itemData.subType_ == ItemSubType.IST_Shield) {
					if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_DoubleHand] != null) {
						NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_DoubleHand].instId_);
					}
				} else {
					if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_DoubleHand] != null && ItemData.GetData ((int)GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_DoubleHand].itemId_).subType_ != ItemSubType.IST_Shield) {
						NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_DoubleHand].instId_);
					}
				}
			} else if (_itemData.slot_ == EquipmentSlot.ES_DoubleHand) {
				if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_SingleHand] != null) {
					NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_SingleHand].instId_);
				}
			} else if (_itemData.slot_ == EquipmentSlot.ES_Ornament_0 || _itemData.slot_ == EquipmentSlot.ES_Ornament_1) {
				
				if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0] != null && GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1] != null) {
					if (ItemData.GetData ((int)GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_) {
						NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0].instId_);
					} else if (ItemData.GetData ((int)GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_) {
						NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1].instId_);
					} else {
						NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0].instId_);
					}
				} else if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0] != null) {
					if (ItemData.GetData ((int)GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_) {
						NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0].instId_);
					} else if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1] != null) {
						if (ItemData.GetData ((int)GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_) {
							NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1].instId_);
						}
					}
				} else if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1] != null) {
					if (ItemData.GetData ((int)GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1].itemId_).subType_ == _itemData.subType_) {
						NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_1].instId_);
					} else if (GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0] != null) {
						if (ItemData.GetData ((int)GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0].itemId_).subType_ == _itemData.subType_) {
							NetConnection.Instance.delEquipment ((uint)GamePlayer.Instance.InstId, GamePlayer.Instance.Equips [(int)EquipmentSlot.ES_Ornament_0].instId_);
						}
					}
				}
			}
			
			NetConnection.Instance.wearEquipment ((uint)GamePlayer.Instance.InstId, item_.instId_);
			//gameObject.SetActive (false);
		}
		else if (data.subType_ == ItemSubType.IST_Buff)
		{
			NetConnection.Instance.useItem((uint)item_.slot_, (uint)GamePlayer.Instance.InstId, (uint)1);
			//gameObject.SetActive(false);
		}
		
		UIManager.Instance.AddItemCellUI(icon_, item_.itemId_);
		//gameObject.SetActive(true);
	}
	public void SetData(COM_Item item)
	{
		item_ = item;
		OnUseBtnClick ();
	}
}
