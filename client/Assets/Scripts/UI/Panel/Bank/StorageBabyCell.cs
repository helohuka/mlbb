using UnityEngine;
using System.Collections;

public class StorageBabyCell : MonoBehaviour {

	public UISprite chuzhanSp;
	public UISlider HPSlider;
	public UISlider MPSlider;
	public UISprite backSp;
	public UISprite shenmingSp;
	public UISprite moliSp;

	public GameObject tipsobj;
	public GameObject levelObj;
	public GameObject suoObj;
	public UISprite disp;

	public UILabel _HpLable;
	public UILabel _MpLable;
	public UILabel _LevelLable;
	public UILabel _PlayLable;
	public UILabel _StandbyLable;

	public UISprite typeSp;
	public UITexture icon;
	public UITexture raceIcon;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel shengmingLabel;
	public UILabel moliLabel;
	public UIButton chuzhanButton;
	public UIButton daimingButton;
	public UISprite back;
	private Baby _babyMainData;
	public UISprite ban;
	public UISprite iconBack;
	public UISprite numsp;
	private bool isLock;
	private COM_BabyInst _BabyInst;
	public COM_BabyInst BabyInst
	{
		set
		{

			if(value == null)
			{
				//UIManager.RemoveButtonAllEventHandler (pane.gameObject);
				icon.gameObject.SetActive(false);
				raceIcon.gameObject.SetActive(false);
				numsp.spriteName = "";
				levelLabel.text = "";
				typeSp.gameObject.SetActive(false);
				nameLabel.text ="";
//				iconBack.spriteName = "cw_chongwutouxiang1";
//				iconBack.GetComponent<UIButton>().normalSprite = "cw_chongwutouxiang1";
				_BabyInst = value;
				return;
			}
			_BabyInst = value;
			typeSp.gameObject.SetActive(true);
			icon.gameObject.SetActive(true);
			raceIcon.gameObject.SetActive(true);
			if(nameLabel != null)
			nameLabel.text = _BabyInst.instName_;
			if(levelLabel != null)
			levelLabel.text = _BabyInst.properties_[(int)PropertyType.PT_Level].ToString(); 
			if(shengmingLabel != null)
			shengmingLabel.text = _BabyInst.properties_[(int)PropertyType.PT_HpCurr].ToString()+"/"+_BabyInst.properties_[(int)PropertyType.PT_HpMax];
			if(moliLabel != null)
			moliLabel.text = _BabyInst.properties_[(int)PropertyType.PT_MpCurr].ToString()+"/"+_BabyInst.properties_[(int)PropertyType.PT_MpMax];
			if(EntityAssetsData.GetData((int)_BabyInst.properties_[(int)PropertyType.PT_AssetId]) == null)
			{
				ClientLog.Instance.LogError(_BabyInst.properties_[(int)PropertyType.PT_AssetId]);
			}
			HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData((int)_BabyInst.properties_[(int)PropertyType.PT_AssetId]).assetsIocn_, icon);
			//HeadIconLoader.Instance.LoadIcon (BabyData.GetData((int)_BabyInst.properties_[(int)PropertyType.PT_TableId])._RaceIcon, raceIcon);
			if(HPSlider !=null)
			HPSlider.value = (_BabyInst.properties_[(int)PropertyType.PT_HpCurr]*1f)/(_BabyInst.properties_[(int)PropertyType.PT_HpMax]*1f);
			if(MPSlider !=null)
			MPSlider.value = (_BabyInst.properties_[(int)PropertyType.PT_MpCurr]*1f)/(_BabyInst.properties_[(int)PropertyType.PT_MpMax]*1f);
		
			BabyData bdata = BabyData.GetData((int)_BabyInst.properties_[(int)PropertyType.PT_TableId]);
			typeSp.spriteName  = bdata._Tpye.ToString();
//			iconBack.spriteName = BabyData.GetPetQuality(bdata._PetQuality);
//			iconBack.GetComponent<UIButton>().normalSprite =BabyData.GetPetQuality(bdata._PetQuality);
//
			int Magic =   bdata._BIG_Magic - _BabyInst.gear_[(int)BabyInitGear.BIG_Magic];
			int Stama =   bdata._BIG_Stama - _BabyInst.gear_[(int)BabyInitGear.BIG_Stama];
			int Speed =   bdata._BIG_Speed - _BabyInst.gear_[(int)BabyInitGear.BIG_Speed];
			int Power =   bdata._BIG_Power - _BabyInst.gear_[(int)BabyInitGear.BIG_Power];
			int Strength =   bdata._BIG_Strength - _BabyInst.gear_[(int)BabyInitGear.BIG_Strength];
			int num = Magic+Stama+Speed+Power+Strength;
			numsp.spriteName = BabyData.GetBabyLeveSp(num);



		}
		get
		{
			return _BabyInst;
		}
	}
	public bool Lock
	{
		set
		{
			isLock = value;
			//typeSp.gameObject.SetActive(isLock);
			numsp.gameObject.SetActive(isLock);
			levelObj.SetActive(isLock);
			disp.gameObject.SetActive(isLock);
			tipsobj.SetActive(!isLock);
			suoObj.SetActive(!isLock);
		}

	}
	public Baby BabyMainData
	{
		set
		{
			if(value != null)
			{
				_babyMainData = value;
				nameLabel.text = _babyMainData.InstName;
				levelLabel.text = _babyMainData.GetIprop(PropertyType.PT_Level).ToString();
				shengmingLabel.text = _babyMainData.GetIprop(PropertyType.PT_HpCurr).ToString()+"/"+_babyMainData.GetIprop(PropertyType.PT_HpMax);
				moliLabel.text = _babyMainData.GetIprop(PropertyType.PT_MpCurr).ToString()+"/"+_babyMainData.GetIprop(PropertyType.PT_MpMax);
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_, icon);
				HeadIconLoader.Instance.LoadIcon ( BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._RaceIcon, raceIcon);

				if(_babyMainData.isForBattle_)
				{
					chuzhanSp.gameObject.SetActive(true);
				}else
				{
					chuzhanSp.gameObject.SetActive(false);
				}
				BabyData bdata = BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId));
				iconBack.spriteName = BabyData.GetPetQuality(bdata._PetQuality);
				
				int Magic =   bdata._BIG_Magic - _babyMainData.gear_[(int)BabyInitGear.BIG_Magic];
				int Stama =   bdata._BIG_Stama - _babyMainData.gear_[(int)BabyInitGear.BIG_Stama];
				int Speed =   bdata._BIG_Speed - _babyMainData.gear_[(int)BabyInitGear.BIG_Speed];
				int Power =   bdata._BIG_Power - _babyMainData.gear_[(int)BabyInitGear.BIG_Power];
				int Strength =   bdata._BIG_Strength - _babyMainData.gear_[(int)BabyInitGear.BIG_Strength];
				int num = Magic+Stama+Speed+Power+Strength;
				numsp.spriteName = BabyData.GetBabyLeveSp(num);

				HPSlider.value = (_babyMainData.GetIprop(PropertyType.PT_HpCurr)*1f)/(_babyMainData.GetIprop(PropertyType.PT_HpMax)*1f);
				MPSlider.value = (_babyMainData.GetIprop(PropertyType.PT_MpCurr)*1f)/(_babyMainData.GetIprop(PropertyType.PT_MpMax)*1f);
				if(bdata._PetQuality == PetQuality.PE_Blue)
				{
					backSp.spriteName =  "zd_chongwukuang_lan";
					shenmingSp.spriteName = "wenzidi_3";
					moliSp.spriteName = "wenzidi_3";
				}else
					if(bdata._PetQuality == PetQuality.PE_Golden)
				{
					backSp.spriteName = "zd_chongwukuang_huang";
					shenmingSp.spriteName = "wenzidi_5";
					moliSp.spriteName = "wenzidi_5";
				}else
					if(bdata._PetQuality == PetQuality.PE_Green)
				{
					backSp.spriteName =  "zd_chongwukuang_lv";
					shenmingSp.spriteName = "wenzidi_2";
					moliSp.spriteName = "wenzidi_2";
				}
				else
					if(bdata._PetQuality == PetQuality.PE_Orange)
				{
					backSp.spriteName =  "zd_chongwukuang_cheng";
					shenmingSp.spriteName = "wenzidi_6";
					moliSp.spriteName = "wenzidi_6";
				}
				else
					if(bdata._PetQuality == PetQuality.PE_Pink)
				{
					backSp.spriteName =  "zd_chongwukuang_fen";
					shenmingSp.spriteName = "wenzidi_7";
					moliSp.spriteName = "wenzidi_7";
				}
				else
					if(bdata._PetQuality == PetQuality.PE_Purple)
				{
					backSp.spriteName =  "zd_chongwukuang_zi";
					shenmingSp.spriteName = "wenzidi_4";
					moliSp.spriteName = "wenzidi_4";
				}
				else
					if(bdata._PetQuality == PetQuality.PE_White)
				{
					backSp.spriteName =  "zd_chongwukuang";
					shenmingSp.spriteName = "wenzidi_1";
					moliSp.spriteName = "wenzidi_1";
				}



			}
		}
		get
		{
			return _babyMainData;
		}
	}
	
	void Start () {
		if(_HpLable !=null && _MpLable != null && _LevelLable != null &&_PlayLable != null && _StandbyLable != null)
		{
			_HpLable.text = LanguageManager.instance.GetValue("bank_Hp");
			_MpLable.text = LanguageManager.instance.GetValue("bank_Mp");
			_LevelLable.text = LanguageManager.instance.GetValue("bank_Level");
			_PlayLable.text = LanguageManager.instance.GetValue("bank_Paly");
			_StandbyLable.text = LanguageManager.instance.GetValue("bank_Stan");
		}

		if(daimingButton != null)
		UIManager.SetButtonEventHandler (daimingButton.gameObject, EnumButtonEvent.OnClick, OnClickDM,0,0);
		if(chuzhanButton != null)
		UIManager.SetButtonEventHandler (chuzhanButton.gameObject, EnumButtonEvent.OnClick, OnClickCZ,0,0);
	}

	private void OnClickCZ(ButtonScript obj, object args, int param1, int param2)
	{
		// 出战按钮是待命
		GamePlayer.Instance.BabyState ((int)BabyInst.instId_, false);
		NetConnection.Instance.setBattlebaby((uint)BabyInst.instId_, false);
		//		if(MainbabyListUI.BabyFightingStandby != null)
		//		{
		//			MainbabyListUI.BabyFightingStandby(BabyMainData.InstId,true);
		//		}
		chuzhanButton.gameObject.SetActive(false);
		daimingButton.gameObject.SetActive(true);
	}
	private void OnClickDM(ButtonScript obj, object args, int param1, int param2)
	{
		// 待命按钮是出战
		if((BabyInst.properties_[(int)PropertyType.PT_Level] - GamePlayer.Instance.GetIprop(PropertyType.PT_Level))>5)
		{
            PopText.Instance.Show(LanguageManager.instance.GetValue("babyLvGreaterThanMy"));
			
		}else
		{
			GamePlayer.Instance.BabyState ((int)BabyInst.instId_, true);
			NetConnection.Instance.setBattlebaby((uint)BabyInst.instId_, true);
//			if(MainbabyListUI.BabyFightingStandby != null)
//			{
//				MainbabyListUI.BabyFightingStandby((int)BabyInst.instId_,false);
//			}
			chuzhanButton.gameObject.SetActive(true);
			daimingButton.gameObject.SetActive(false);
		}
		
	}
	

}
