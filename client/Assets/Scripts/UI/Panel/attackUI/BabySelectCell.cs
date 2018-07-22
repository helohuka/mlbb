using UnityEngine;
using System.Collections;

public class BabySelectCell : MonoBehaviour {

	public UITexture raceIcon;
	public UISprite raceSp;
	public UITexture iconSp;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel hpLabel;
	public UILabel mpLabel;
	private Baby _mbaby;
	public UISprite iconBack;
	public UISprite numsp;
	public Baby Mbaby
	{
		set
		{
			if(value !=null)
			{
				_mbaby = value;
				//raceSp.spriteName = BabyData.GetData(_mbaby.GetIprop(PropertyType.PT_TableId)).RaceIcon_;
				//iconSp.spriteName = BabyData.GetData(_mbaby.GetIprop(PropertyType.PT_TableId)).resIcon_;
                nameLabel.text = _mbaby.InstName;
				levelLabel.text = _mbaby.GetIprop(PropertyType.PT_Level).ToString();
				hpLabel.text = _mbaby.GetIprop(PropertyType.PT_HpCurr)+"/"+_mbaby.GetIprop(PropertyType.PT_HpMax);
				mpLabel.text = _mbaby.GetIprop(PropertyType.PT_MpCurr)+"/"+_mbaby.GetIprop(PropertyType.PT_MpMax);


				BabyData bdata = BabyData.GetData(_mbaby.GetIprop(PropertyType.PT_TableId));
				iconBack.spriteName = BabyData.GetPetQuality(bdata._PetQuality);
				
				int Magic =   bdata._BIG_Magic - _mbaby.gear_[(int)BabyInitGear.BIG_Magic];
				int Stama =   bdata._BIG_Stama - _mbaby.gear_[(int)BabyInitGear.BIG_Stama];
				int Speed =   bdata._BIG_Speed - _mbaby.gear_[(int)BabyInitGear.BIG_Speed];
				int Power =   bdata._BIG_Power- _mbaby.gear_[(int)BabyInitGear.BIG_Power];
				int Strength =   bdata._BIG_Strength - _mbaby.gear_[(int)BabyInitGear.BIG_Strength];
				int num = Magic+Stama+Speed+Power+Strength;
				numsp.spriteName = BabyData.GetBabyLeveSp(num);


				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(_mbaby.GetIprop(PropertyType.PT_AssetId)).assetsIocn_, iconSp);
				HeadIconLoader.Instance.LoadIcon (BabyData.GetData(_mbaby.GetIprop(PropertyType.PT_TableId))._RaceIcon, raceIcon);
			}
		}

		get
		{
			return _mbaby;
		}
	}
	void Start () {
	
	}
	

}
