using UnityEngine;
using System.Collections;

public class Mainbabystrengthen : MonoBehaviour {

	public UILabel _LevelLable;
	public UILabel _GrowingUpLable;
	public UILabel _DesOneLable;
	public UILabel _DesTwoLable;
	public UILabel _DesLable;
	public UILabel _StrengthenLable;



	public delegate void RefreshstrengthenH(int bid);
	public static RefreshstrengthenH RefreshstrengthenOk;
	
	public delegate void Refreshstrengthen (int bid);
	public static Refreshstrengthen RefreshGstrengthenOk;

	public delegate void RefreshstrengthenLevel (int bid,int level);
	public static RefreshstrengthenLevel RefreshGstrengthenlevelOk;

	public UILabel chneglv;

	public UILabel oldLevel;
	public UILabel oldChengzhang;
	public UILabel newLevel;
	public UILabel newChengzhang;
	public UILabel babyNameLabel;
	public UILabel numLabel;
	public UIButton enterBtn;
	public UITexture icon;
	public UITexture babyIcon;
	public UITexture raceIcon;
	public UISprite raSp;
	public UISprite raaSp;
	private Baby Inst;

	void Start () {
		InitUIText ();
//		RefreshstrengthenOk = Refresh;
//		RefreshGstrengthenOk = Refresh;
//		RefreshGstrengthenlevelOk = RefreshGstrengthenlevel;
		
		if(GamePlayer.Instance.babies_list_.Count!=0)
		{
			Refresh(MainbabyProperty.idss[0]);
			HeadIconLoader.Instance.LoadIcon (ItemData.GetData(itemID()).icon_, icon);
			numLabel.text = BagSystem.instance.GetItemMaxNum((uint)itemID())+"/"+itemCount ().ToString ();
		}else
		{
			ClearText();
		}

		BagSystem.instance.DelItemInstEvent += updateUI;
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick,OnClickbtn, 0, 0);
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			enterBtn.gameObject.SetActive(false);
		}
	}

	void InitUIText()
	{
		_LevelLable.text = LanguageManager.instance.GetValue("Baby_StrengthenLevel");
		_GrowingUpLable.text = LanguageManager.instance.GetValue("Baby_GrowingUp");
		_DesOneLable.text = LanguageManager.instance.GetValue("Baby_DesOne");
		_DesTwoLable.text = LanguageManager.instance.GetValue("Baby_DesTwo");
		_DesLable.text = LanguageManager.instance.GetValue("Baby_Des");
		_StrengthenLable.text = LanguageManager.instance.GetValue("Baby_Strengthen");
	}

	void updateUI(COM_Item item)
	{

		numLabel.text = BagSystem.instance.GetItemMaxNum((uint)itemID())+"/"+itemCount();
		
	}

	void OnEnable()
	{
		RefreshstrengthenOk = Refresh;
		RefreshGstrengthenOk = Refresh;
		RefreshGstrengthenlevelOk = RefreshGstrengthenlevel;
		//Refresh (BabyData.babyReId);

		if(GamePlayer.Instance.babies_list_.Count!=0)
		{
			Refresh(MainbabyProperty.idss[0]);
			if(BabyData.babyReId != 0&& BabyData.intensifyLevel !=0)
			{
				RefreshGstrengthenlevels (BabyData.babyReId, BabyData.intensifyLevel);
			}

			HeadIconLoader.Instance.LoadIcon (ItemData.GetData(itemID()).icon_, icon);
			numLabel.text = BagSystem.instance.GetItemMaxNum((uint)itemID())+"/"+itemCount ().ToString ();
		}else
		{
			ClearText();
		}
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			enterBtn.gameObject.SetActive(false);
		}
	}
	void OnDisable()
	{
		RefreshstrengthenOk = null;
		RefreshGstrengthenOk = null;
		RefreshGstrengthenlevelOk = null;
	}
	
	void OnDestroy()
	{
//		RefreshstrengthenOk = null;
//		RefreshGstrengthenOk = null;
		BagSystem.instance.DelItemInstEvent -= updateUI;
	}
	void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
	{

//		if (ShopData.GetShopId (itemID()) == 0)
//		{
//			//PopText.Instance.Show(LanguageManager.instance.GetValue("商店没有此物品"));
//			return;
//		}
//		int shopid = ShopData.GetShopId (itemID());
//		if(BagSystem.instance.GetItemMaxNum((uint)itemID())<=0&& BagSystem.instance.GetItemMaxNum((uint)itemID())< itemCount())
//		{
//			QuickBuyUI.ShowMe(shopid);
//		}else
//		{

//			COM_Item item =	BagSystem.instance.GetItemByItemId((uint)itemID());
//			ItemData idata = ItemData.GetData(itemID());
			//MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("xiaohaoitemwupin").Replace("{n1}",idata.name_).Replace("{n}",idata.desc_),()=>{
				
//				NetConnection.Instance.useItem((uint)item.slot_,(uint)Inst.InstId,(uint)1);
			//});
//		}

		NetConnection.Instance.intensifyBaby((uint)Inst.InstId);
	}
	void RefreshGstrengthenlevel(int babyid ,int level)
	{
		if(Inst==null)return;
		Refresh (babyid);
		if(Inst.intensifyLevel_ == level)
		{
			EffectAPI.PlayUIEffect( EFFECT_ID.EFFECT_qianghuashibai,gameObject.transform,null,null );
		}else
		{
			EffectAPI.PlayUIEffect( EFFECT_ID.EFFECT_qianghuachenggong ,gameObject.transform , null , null );
		}
		Inst.intensifyLevel_ = (uint)level;
		if(BagSystem.instance.GetItemMaxNum((uint)itemID())>=itemCount ())
		{
			enterBtn.isEnabled = true;
		}else
		{
			enterBtn.isEnabled = false;
		}
	}
	void RefreshGstrengthenlevels(int babyid ,int level)
	{
		if(Inst==null)return;
		Refresh (babyid);
//		if(Inst.intensifyLevel_ == level)
//		{
//			EffectAPI.PlayUIEffect( EFFECT_ID.EFFECT_qianghuashibai,gameObject.transform,null,null );
//		}else
//		{
//			EffectAPI.PlayUIEffect( EFFECT_ID.EFFECT_qianghuachenggong ,gameObject.transform , null , null );
//		}
		Inst.intensifyLevel_ = (uint)level;
		if(BagSystem.instance.GetItemMaxNum((uint)itemID())>=itemCount ())
		{
			enterBtn.isEnabled = true;
		}else
		{
			enterBtn.isEnabled = false;
		}
	}
	void Refresh(int uid)
	{

		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText();
		}else
		{
			float []addPoints = new float[5]; 
			Inst = GamePlayer.Instance.GetBabyInst (uid);
			if(Inst == null)return;
			BabyData bdata = BabyData.GetData (Inst.GetIprop (PropertyType.PT_TableId));
			HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(bdata._AssetsID).assetsIocn_, babyIcon);
			HeadIconLoader.Instance.LoadIcon (bdata._RaceIcon, raceIcon);
			
			babyNameLabel.text = Inst.InstName;
			
			if(Inst.intensifyLevel_ == 10)
			{
				enterBtn.isEnabled = false;
				newLevel.gameObject.SetActive(false);
				raSp.gameObject.SetActive(false);
				raaSp.gameObject.SetActive(false);
                oldLevel.text = LanguageManager.instance.GetValue("babyAlreadyOnTop");
				PetIntensiveData petData = PetIntensiveData.GetData ((int)Inst.intensifyLevel_);
				oldChengzhang.text = petData.grow_.ToString ();
				chneglv.text = petData.probability_+"%";
				newChengzhang.text ="";
				
			}else
			{
				
				newLevel.gameObject.SetActive(true);
				raSp.gameObject.SetActive(true);
				enterBtn.isEnabled = true;
				raaSp.gameObject.SetActive(true);
				oldLevel.text = Inst.intensifyLevel_.ToString();
				PetIntensiveData petData = PetIntensiveData.GetData ((int)Inst.intensifyLevel_);
				oldChengzhang.text = petData.grow_.ToString ();
				PetIntensiveData petNData = PetIntensiveData.GetData ((int)Inst.intensifyLevel_+1);
				newChengzhang.text = petNData.grow_.ToString ();
				newLevel.text = (Inst.intensifyLevel_ + 1).ToString ();
				chneglv.text = petNData.probability_+"%";
				if(BagSystem.instance.GetItemMaxNum((uint)itemID())>=itemCount ())
				{
					enterBtn.isEnabled = true;
				}else
				{
					enterBtn.isEnabled = false;
				}
				
			}
			
			updateUI (null);
		}




	}

	void ClearText()
	{
		oldLevel.text = "";
		oldChengzhang.text = "";
		newLevel.text = "";
		newChengzhang.text = "";
		babyNameLabel.text = "";
		numLabel.text = "";
		icon.mainTexture = null;
		enterBtn.isEnabled = false;
	}

	int itemCount()
	{

		PetIntensiveData petData = PetIntensiveData.GetData ((int)Inst.intensifyLevel_);
		return petData.itemnum_;
	}
	int itemID()
	{
		PetIntensiveData petData = PetIntensiveData.GetData ((int)Inst.intensifyLevel_);
		return petData.item_;
	}
	int SimAddPoint(PropertyType pointType, PropertyType effectType, float prop)
	{
		switch (pointType)
		{
		case PropertyType.PT_Stama:
			switch(effectType)
			{
			case PropertyType.PT_HpMax:
				return (int)(prop * 8f);
			case PropertyType.PT_MpMax:
				return (int)(prop * 1f);
			case PropertyType.PT_Attack:
				return (int)(prop * 0.1f);
			case PropertyType.PT_Defense:
				return (int)(prop * 0.1f);
			case PropertyType.PT_Agile:
				return (int)(prop * 0.1f);
			case PropertyType.PT_Reply:
				return (int)(prop * 0.8f);
			case PropertyType.PT_Spirit:
				return (int)(prop * -0.3f);
			}
			return 0;
		case PropertyType.PT_Strength:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return (int)(prop * 2f);
			case PropertyType.PT_MpMax:
				return (int)(prop * 2f);
			case PropertyType.PT_Attack:
				return (int)(prop * 2f);
			case PropertyType.PT_Defense:
				return (int)(prop * 0.2f);
			case PropertyType.PT_Agile:
				return (int)(prop * 0.2f);
			case PropertyType.PT_Reply:
				return (int)(prop * -0.1f);
			case PropertyType.PT_Spirit:
				return (int)(prop * -0.1f);
			}
			return 0;
		case PropertyType.PT_Power:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return (int)(prop * 3f);
			case PropertyType.PT_MpMax:
				return (int)(prop * 2f);
			case PropertyType.PT_Attack:
				return (int)(prop * 0.2f);
			case PropertyType.PT_Defense:
				return (int)(prop * 2f);
			case PropertyType.PT_Agile:
				return (int)(prop * 0.2f);
			case PropertyType.PT_Reply:
				return (int)(prop * -0.1f);
			case PropertyType.PT_Spirit:
				return (int)(prop * 0.2f);
			}
			return 0;
		case PropertyType.PT_Speed:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return (int)(prop * 3f);
			case PropertyType.PT_MpMax:
				return (int)(prop * 2f);
			case PropertyType.PT_Attack:
				return (int)(prop * 0.2f);
			case PropertyType.PT_Defense:
				return (int)(prop * 0.2f);
			case PropertyType.PT_Agile:
				return (int)(prop * 2f);
			case PropertyType.PT_Reply:
				return (int)(prop * 0.2f);
			case PropertyType.PT_Spirit:
				return (int)(prop * -0.1f);
			}
			return 0;
		case PropertyType.PT_Magic:
			switch (effectType)
			{
			case PropertyType.PT_HpMax:
				return (int)(prop * 1f);
			case PropertyType.PT_MpMax:
				return (int)(prop * 10f);
			case PropertyType.PT_Attack:
				return (int)(prop * 0.1f);
			case PropertyType.PT_Defense:
				return (int)(prop * 0.1f);
			case PropertyType.PT_Agile:
				return (int)(prop * 0.1f);
			case PropertyType.PT_Reply:
				return (int)(prop * -0.3f);
			case PropertyType.PT_Spirit:
				return (int)(prop * 0.8f);
			}
			return 0;
		}
		return 0;
	}

}
