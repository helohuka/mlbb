using UnityEngine;
using System.Collections;

public class MainbabyReductionUI : MonoBehaviour {

	public UILabel _DesLable;
	public UILabel _GrowingUpLable;
	public UILabel _PhysicalLable;
	public UILabel _PowerLable;
	public UILabel _StrengthLable;
	public UILabel _SpeedLable;
	public UILabel _MagicLable;
	public UILabel _ReductionLable;

	public GameObject tishiObj;
	public UILabel dangshuLable;
	public delegate void RefreshGrowingUpH(int bid);
	public static RefreshGrowingUpH RefreshGrowingUpOk;

	public delegate void RefreshGrowingUpHan(int bid);
	public static RefreshGrowingUpHan RefreshGrowingUpHanOk;
	public UILabel tiliLabel;
	public UILabel liLiangLabel;
	public UILabel qiangduLabel;
	public UILabel suduLabel;
	public UILabel mofaLabel;
	public UILabel miaoshuLabel;
	public UITexture itemIcon;
	public UIButton enterBtn;
	public UILabel numlabel;
	public UISprite jiaotouSp;


	public UIProgressBar tiliSlider;
	public UIProgressBar liliangSlider;
	public UIProgressBar qiangduSlider;
	public UIProgressBar suduSlider;
	public UIProgressBar mofaSlider;

	public UISprite onesp;
	public UISprite twoSp;

	private Baby Inst;
	private int Uid;
	private int itemid;
	private int shopId;

    bool hasDestroyed;
		 
	void Start () {
		GlobalValue.Get(Constant.C_ResetBabyPay, out itemid);
		InitUIText ();
		RefreshGrowingUpOk = RefreshGrowingUp;
		RefreshGrowingUpHanOk = Refresh;
		HeadIconLoader.Instance.LoadIcon (ItemData.GetData(itemid).icon_, itemIcon);
		miaoshuLabel.text = ItemData.GetData(itemid).name_ /*LanguageManager.instance.GetValue ("babyxiaohao").Replace ("{n}",ItemData.GetData(itemid).name_)*/;
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickhuanyuanBtn,0, 0);
		numlabel.text = BagSystem.instance.GetItemMaxNum((uint)itemid)+"/1";
		BagSystem.instance.ItemChanged += updateUI;
		BagSystem.instance.UpdateItemEvent += updateUI;
		BagSystem.instance.DelItemEvent += updateUI;
        hasDestroyed = false;
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			enterBtn.gameObject.SetActive(false);
		}
	}
	void InitUIText()
	{
		_DesLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_Des");
		_GrowingUpLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_GrowingUp");
		_PhysicalLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_Physical");
		_PowerLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_Power");
		_StrengthLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_Strength");
		_SpeedLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_Speed");
		_MagicLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_Magic");
		_ReductionLable.text = LanguageManager.instance.GetValue("BabyHuanyuan_Reduction");
	}
	void updateUI(COM_Item item)
	{

		numlabel.text = BagSystem.instance.GetItemMaxNum((uint)itemid)+"/1";
	
	}
	void updateUI(uint instId)
	{
		numlabel.text = BagSystem.instance.GetItemMaxNum((uint)itemid)+"/1";
	}

	void OnClickhuanyuanBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if (ShopData.GetShopId (itemid) == 0)
						return;
		int shopid = ShopData.GetShopId (itemid);
		if(BagSystem.instance.GetItemMaxNum((uint)itemid)<=0)
		{
			QuickBuyUI.ShowMe(shopid);
		}else
		{
			if(IsbabyS())
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("huiyantishi"),()=>{
					if(type == 3)
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("gzcwubnhuanyuan"));
						return;
					}
					NetConnection.Instance.resetBaby (Uid);
					PopText.Instance.Show (LanguageManager.instance.GetValue("huanyuanchenggong"));
				});
			}else
			{
				MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("huanyuan"),()=>{
					if(type == 3)
					{
						PopText.Instance.Show(LanguageManager.instance.GetValue("gzcwubnhuanyuan"));
						return;
					}
					NetConnection.Instance.resetBaby (Uid);
					PopText.Instance.Show (LanguageManager.instance.GetValue("huanyuanchenggong"));
				});
			}

			MainbabyUI.Instance.isState = false;
		}


//		if(BagSystem.instance.GetItemByItemId ((uint)itemid)!= null)
//		{
//			NetConnection.Instance.resetBaby (Uid);
//			MainbabyUI.Instance.isState = false;
//		}else
//		{
//			PopText.Instance.Show(LanguageManager.instance.GetValue("meiyoudaoju"));
//		}
		
		
		
	}
	bool IsbabyS()
	{
		if(Inst == null)return false;
		BabyData bdata = BabyData.GetData(Inst.GetIprop(PropertyType.PT_TableId));
		int Magic =   bdata._BIG_Magic - Inst.gear_[(int)BabyInitGear.BIG_Magic];
		int Stama =   bdata._BIG_Stama - Inst.gear_[(int)BabyInitGear.BIG_Stama];
		int Speed =   bdata._BIG_Speed - Inst.gear_[(int)BabyInitGear.BIG_Speed];
		int Power =   bdata._BIG_Power - Inst.gear_[(int)BabyInitGear.BIG_Power];
		int Strength =   bdata._BIG_Strength - Inst.gear_[(int)BabyInitGear.BIG_Strength];
		int num = Magic+Stama+Speed+Power+Strength;
		string l = BabyData.GetBabyLeveSp(num);
		if(l.Equals("S"))
		{
			return true;
		}
		return false;
	}
	int type = 0;
	void RefreshGrowingUp(int uid)
	{
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText();
		}else
		{
			enterBtn.isEnabled = true;
			Uid = uid;
			Inst = GamePlayer.Instance.GetBabyInst (uid);
			BabyData bdata = BabyData.GetData (Inst.GetIprop(PropertyType.PT_TableId));
			mofaLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Magic].ToString()+"/"+bdata._BIG_Magic;
			tiliLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Stama].ToString()+"/"+bdata._BIG_Stama;
			suduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Speed].ToString()+"/"+bdata._BIG_Speed;
			qiangduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Power].ToString()+"/"+bdata._BIG_Power;
			liLiangLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Strength].ToString()+"/"+bdata._BIG_Strength;
			type = bdata._Tpye;
			tiliSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Stama]*1f)/(bdata._BIG_Stama*1f);
			liliangSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Strength]*1f)/(bdata._BIG_Strength*1f);
			qiangduSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Power]*1f)/(bdata._BIG_Power*1f);
			suduSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Speed]*1f)/(bdata._BIG_Speed*1f);
			mofaSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Magic]*1f)/(bdata._BIG_Magic*1f);

			int dd = Inst.gear_[(int)BabyInitGear.BIG_Magic]+Inst.gear_[(int)BabyInitGear.BIG_Stama]+ Inst.gear_[(int)BabyInitGear.BIG_Speed]+Inst.gear_[(int)BabyInitGear.BIG_Power]+Inst.gear_[(int)BabyInitGear.BIG_Strength];
			dangshuLable.text = dd.ToString();

		}
	

	}

	BabyData oldBdata;
	void Refresh(int uid)
	{
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText();
		}else
		{

			if(uid != Uid)return;
			enterBtn.isEnabled = true;
			Uid = MainbabyProperty.idss[0];
			Inst = GamePlayer.Instance.GetBabyInst (Uid);
			BabyData bdata = BabyData.GetData (Inst.GetIprop(PropertyType.PT_TableId));
			type = bdata._Tpye;
			int Magic =   bdata._BIG_Magic - Inst.gear_[(int)BabyInitGear.BIG_Magic];
			int Stama =   bdata._BIG_Stama - Inst.gear_[(int)BabyInitGear.BIG_Stama];
			int Speed =   bdata._BIG_Speed - Inst.gear_[(int)BabyInitGear.BIG_Speed];
			int Power =   bdata._BIG_Power - Inst.gear_[(int)BabyInitGear.BIG_Power];
			int Strength =   bdata._BIG_Strength - Inst.gear_[(int)BabyInitGear.BIG_Strength];
			int num = Magic+Stama+Speed+Power+Strength;
			twoSp.spriteName = BabyData.GetBabyLeveSp(num)+"_big";

			mofaLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Magic].ToString()+"/"+bdata._BIG_Magic;
			tiliLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Stama].ToString()+"/"+bdata._BIG_Stama;
			suduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Speed].ToString()+"/"+bdata._BIG_Speed;
			qiangduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Power].ToString()+"/"+bdata._BIG_Power;
			liLiangLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Strength].ToString()+"/"+bdata._BIG_Strength;
			
			tiliSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Stama]*1f)/(bdata._BIG_Stama*1f);
			liliangSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Strength]*1f)/(bdata._BIG_Strength*1f);
			qiangduSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Power]*1f)/(bdata._BIG_Power*1f);
			suduSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Speed]*1f)/(bdata._BIG_Speed*1f);
			mofaSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Magic]*1f)/(bdata._BIG_Magic*1f);

			int dd = Inst.gear_[(int)BabyInitGear.BIG_Magic]+Inst.gear_[(int)BabyInitGear.BIG_Stama]+ Inst.gear_[(int)BabyInitGear.BIG_Speed]+Inst.gear_[(int)BabyInitGear.BIG_Power]+Inst.gear_[(int)BabyInitGear.BIG_Strength];
			dangshuLable.text = dd.ToString();

			tishiObj.SetActive(true);
			EffectAPI.PlayUIEffect((EFFECT_ID)GlobalValue.EFFECT_UI_pinzhijinjie, jiaotouSp.transform,null,(GameObject obj)=>{
				obj.transform.localPosition = Vector3.zero;
			});
			GlobalInstanceFunction.Instance.Invoke(()=>{
                if (hasDestroyed)
                    return;

				tishiObj.SetActive(false);
				int Magic1 =   oldBdata._BIG_Magic - Inst.gear_[(int)BabyInitGear.BIG_Magic];
				int Stama1 =   oldBdata._BIG_Stama - Inst.gear_[(int)BabyInitGear.BIG_Stama];
				int Speed1 =   oldBdata._BIG_Speed - Inst.gear_[(int)BabyInitGear.BIG_Speed];
				int Power1 =   oldBdata._BIG_Power - Inst.gear_[(int)BabyInitGear.BIG_Power];
				int Strength1 =   oldBdata._BIG_Strength - Inst.gear_[(int)BabyInitGear.BIG_Strength];
				int num1 = Magic1+Stama1+Speed1+Power1+Strength1;
				onesp.spriteName = BabyData.GetBabyLeveSp(num1)+"_big";
				oldBdata = bdata;
			},1f);

		}


	}

	void OnEnable()
	{
		if(GamePlayer.Instance.babies_list_.Count==0)
		{
			ClearText();
		}else
		{
			Uid = MainbabyProperty.idss[0];
			Inst = GamePlayer.Instance.GetBabyInst (Uid);
			BabyData bdata = BabyData.GetData (Inst.GetIprop(PropertyType.PT_TableId));
			type = bdata._Tpye;



			int Magic =   bdata._BIG_Magic - Inst.gear_[(int)BabyInitGear.BIG_Magic];
			int Stama =   bdata._BIG_Stama - Inst.gear_[(int)BabyInitGear.BIG_Stama];
			int Speed =   bdata._BIG_Speed - Inst.gear_[(int)BabyInitGear.BIG_Speed];
			int Power =   bdata._BIG_Power - Inst.gear_[(int)BabyInitGear.BIG_Power];
			int Strength =   bdata._BIG_Strength - Inst.gear_[(int)BabyInitGear.BIG_Strength];
			int num = Magic+Stama+Speed+Power+Strength;
			onesp.spriteName = BabyData.GetBabyLeveSp(num)+"_big";
			oldBdata = bdata;

			mofaLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Magic].ToString()+"/"+bdata._BIG_Magic;
			tiliLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Stama].ToString()+"/"+bdata._BIG_Stama;
			suduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Speed].ToString()+"/"+bdata._BIG_Speed;
			qiangduLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Power].ToString()+"/"+bdata._BIG_Power;
			liLiangLabel.text ="[-][666666]"+ Inst.gear_[(int)BabyInitGear.BIG_Strength].ToString()+"/"+bdata._BIG_Strength;
			
			tiliSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Stama]*1f)/(bdata._BIG_Stama*1f);
			liliangSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Strength]*1f)/(bdata._BIG_Strength*1f);
			qiangduSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Power]*1f)/(bdata._BIG_Power*1f);
			suduSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Speed]*1f)/(bdata._BIG_Speed*1f);
			mofaSlider.value = (Inst.gear_[(int)BabyInitGear.BIG_Magic]*1f)/(bdata._BIG_Magic*1f);
			int dd = Inst.gear_[(int)BabyInitGear.BIG_Magic]+Inst.gear_[(int)BabyInitGear.BIG_Stama]+ Inst.gear_[(int)BabyInitGear.BIG_Speed]+Inst.gear_[(int)BabyInitGear.BIG_Power]+Inst.gear_[(int)BabyInitGear.BIG_Strength];
			dangshuLable.text = dd.ToString();
		}
		if (GlobalValue.isBattleScene(StageMgr.Scene_name))
		{
			enterBtn.gameObject.SetActive(false);
		}


	}
	void ClearText()
	{
		mofaLabel.text ="";
		tiliLabel.text ="";
		suduLabel.text ="";
		qiangduLabel.text ="";
		liLiangLabel.text ="";
		tiliSlider.value = 0;
		liliangSlider.value =0;
		qiangduSlider.value = 0;
		suduSlider.value = 0;
		mofaSlider.value = 0;
		enterBtn.isEnabled = false;
	}
	void OnDestroy()
	{
		RefreshGrowingUpOk = null;
        RefreshGrowingUpHanOk = null;
		BagSystem.instance.UpdateItemEvent -= updateUI;
		BagSystem.instance.DelItemEvent -= updateUI;
		BagSystem.instance.DelItemInstEvent -= updateUI;
        BagSystem.instance.ItemChanged += updateUI;
        hasDestroyed = true;
	}

}
