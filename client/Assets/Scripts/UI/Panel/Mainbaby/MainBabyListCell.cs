using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainBabyListCell : MonoBehaviour {


	public UISprite gensuiSp;
	public UISprite suodingSp;
	public UISprite backSp;
	public UISprite shenmingSp;
	public UISprite moliSp;
	public UISprite leixingSp;

	public UISlider HPSlider;
	public UISlider MPSlider;
	public UISprite chuzhanSp;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel shengmingLabel;
	public UILabel moliLabel;
//	public UIButton chuzhanButton;
//	public UIButton daimingButton;
	public UITexture zhongzuicON;
	public UITexture icontou;
	public UISprite back;
	private Baby _babyMainData;
	private BabyData babydata;
	public UISprite ban;
	public UISprite iconBack;
	public UISprite numsp;
	private List<string> _icons = new List<string>();
	Baby Inst;
	void Start () {

//		UIManager.SetButtonEventHandler (daimingButton.gameObject, EnumButtonEvent.OnClick, OnClickDM,0,0);
//		UIManager.SetButtonEventHandler (chuzhanButton.gameObject, EnumButtonEvent.OnClick, OnClickCZ,0,0);
	}

	void Update () {
	

	  _babyMainData = GamePlayer.Instance.GetBabyInst (_babyMainData.InstId);
		if(_babyMainData != null )
		{
			if( _babyMainData.GetIprop(PropertyType.PT_Free)> 0)
			{
				this.gameObject.GetComponent<UISprite>().MarkOn (UISprite.MarkAnthor.MA_RightTop,-10f,-10f);
			}
			else
			{
				this.gameObject.GetComponent<UISprite>().MarkOff ();
			}
		}
	}
//	private void OnClickCZ(ButtonScript obj, object args, int param1, int param2)
//	{
//        // 出战按钮是待命
//		GamePlayer.Instance.BabyState (BabyMainData.InstId, false);
//        NetConnection.Instance.setBattlebaby((uint)BabyMainData.InstId, false);
//		if(MainbabyState.SetKJBtnstateEventOk != null)
//		{
//			MainbabyState.SetKJBtnstateEventOk(false);
//		}
//		if(MainbabyListUI.BabyFightingStandby != null)
//		{
//			MainbabyListUI.BabyFightingStandby(BabyMainData.InstId,true);
//		}
//		daimingButton.gameObject.SetActive(true);
//		chuzhanButton.gameObject.SetActive(false);
//
//	}
//	private void OnClickDM(ButtonScript obj, object args, int param1, int param2)
//	{
//        // 待命按钮是出战
//		if((BabyMainData.GetIprop(PropertyType.PT_Level) - GamePlayer.Instance.GetIprop(PropertyType.PT_Level))>5)
//		{
//            PopText.Instance.Show(LanguageManager.instance.GetValue("babyLvGreaterThanMy"));
//
//		}else
//		{
//			GamePlayer.Instance.BabyState (BabyMainData.InstId, true);
//			NetConnection.Instance.setBattlebaby((uint)BabyMainData.InstId, true);
//			if(MainbabyListUI.BabyFightingStandby != null)
//			{
//				MainbabyListUI.BabyFightingStandby(BabyMainData.InstId,false);
//			}
//			if(MainbabyState.SetKJBtnstateEventOk != null)
//			{
//				MainbabyState.SetKJBtnstateEventOk(true);
//			}
//			chuzhanButton.gameObject.SetActive(true);
//			daimingButton.gameObject.SetActive(false);
//		}
//
//	}






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

				HPSlider.value = (_babyMainData.GetIprop(PropertyType.PT_HpCurr)*1f)/(_babyMainData.GetIprop(PropertyType.PT_HpMax)*1f);
				MPSlider.value = (_babyMainData.GetIprop(PropertyType.PT_MpCurr)*1f)/(_babyMainData.GetIprop(PropertyType.PT_MpMax)*1f);
				leixingSp.spriteName = BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._Tpye.ToString();
                string s = BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._RaceIcon;
				string a = EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_;
				BabyData bdata = BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId));
				iconBack.spriteName = BabyData.GetPetQuality(bdata._PetQuality);

				int Magic =   bdata._BIG_Magic - _babyMainData.gear_[(int)BabyInitGear.BIG_Magic];
				int Stama =   bdata._BIG_Stama - _babyMainData.gear_[(int)BabyInitGear.BIG_Stama];
				int Speed =   bdata._BIG_Speed - _babyMainData.gear_[(int)BabyInitGear.BIG_Speed];
				int Power =   bdata._BIG_Power - _babyMainData.gear_[(int)BabyInitGear.BIG_Power];
				int Strength =   bdata._BIG_Strength - _babyMainData.gear_[(int)BabyInitGear.BIG_Strength];
				int num = Magic+Stama+Speed+Power+Strength;
				numsp.spriteName = BabyData.GetBabyLeveSp(num);
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_, icontou);
				//HeadIconLoader.Instance.LoadIcon ( BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._RaceIcon, zhongzuicON);
				if(_babyMainData.isForBattle_)
				{
					chuzhanSp.gameObject.SetActive(true);
				}else
				{
					chuzhanSp.gameObject.SetActive(false);
				}

				if( _babyMainData.GetInst().isLock_)
				{
					suodingSp.gameObject.SetActive(true);
				}else
				{
					suodingSp.gameObject.SetActive(false);
				}
				if( _babyMainData.GetInst().isShow_)
				{
					gensuiSp.gameObject.SetActive(true);
				}else
				{
					gensuiSp.gameObject.SetActive(false);
				}
//				if(bdata._PetQuality == PetQuality.PE_Blue)
//				{
//					backSp.spriteName =  "zd_chongwukuang_lan";
//					shenmingSp.spriteName = "wenzidi_3";
//					moliSp.spriteName = "wenzidi_3";
//				}else
//					if(bdata._PetQuality == PetQuality.PE_Golden)
//				{
//					backSp.spriteName = "zd_chongwukuang_huang";
//					shenmingSp.spriteName = "wenzidi_5";
//					moliSp.spriteName = "wenzidi_5";
//				}else
//					if(bdata._PetQuality == PetQuality.PE_Green)
//				{
//					backSp.spriteName =  "zd_chongwukuang_lv";
//					shenmingSp.spriteName = "wenzidi_2";
//					moliSp.spriteName = "wenzidi_2";
//				}
//				else
//					if(bdata._PetQuality == PetQuality.PE_Orange)
//				{
//					backSp.spriteName =  "zd_chongwukuang_cheng";
//					shenmingSp.spriteName = "wenzidi_6";
//					moliSp.spriteName = "wenzidi_6";
//				}
//				else
//					if(bdata._PetQuality == PetQuality.PE_Pink)
//				{
//					backSp.spriteName =  "zd_chongwukuang_fen";
//					shenmingSp.spriteName = "wenzidi_7";
//					moliSp.spriteName = "wenzidi_7";
//				}
//				else
//					if(bdata._PetQuality == PetQuality.PE_Purple)
//				{
//					backSp.spriteName =  "zd_chongwukuang_zi";
//					shenmingSp.spriteName = "wenzidi_4";
//					moliSp.spriteName = "wenzidi_4";
//				}
//				else
//					if(bdata._PetQuality == PetQuality.PE_White)
//				{
//					backSp.spriteName =  "zd_chongwukuang";
//					shenmingSp.spriteName = "wenzidi_1";
//					moliSp.spriteName = "wenzidi_1";
//				}





				if(!_icons.Contains(EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_);
				}
				if(!_icons.Contains(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._RaceIcon))
				{
					_icons.Add(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._RaceIcon);
				}
			}
		}
		get
		{
			return _babyMainData;
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
