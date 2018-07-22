using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BabyCell : MonoBehaviour {

	public UISprite zhongzuSp;
	public UITexture iconSp;
	public UILabel LevelLabel;
	public UILabel nameLabel;
	public UILabel HPLabel;
	public UILabel MPLabel;
	public UISprite iconBack;
	public UISprite numsp;
	private Baby _babyMainData;
	private List<string> _icons = new List<string>();
	public Baby BabyMainData
	{
		set
		{
			if(value != null)
			{
				_babyMainData = value;
				zhongzuSp.spriteName = BabyData.GetData( _babyMainData.GetIprop(PropertyType.PT_TableId))._RaceIcon;
				//iconSp.spriteName = EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId)).assetsID_).assetsIocn_;
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_, iconSp);
				if(!_icons.Contains(EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_);
				}

				nameLabel.text = _babyMainData.InstName;
				LevelLabel.text = _babyMainData.GetIprop(PropertyType.PT_Level).ToString();
				HPLabel.text = _babyMainData.GetIprop(PropertyType.PT_HpCurr).ToString()+"/"+ _babyMainData.GetIprop(PropertyType.PT_HpMax);
				MPLabel.text = _babyMainData.GetIprop(PropertyType.PT_MpCurr).ToString()+"/"+ _babyMainData.GetIprop(PropertyType.PT_MpMax);

				BabyData bdata = BabyData.GetData(_babyMainData.GetIprop(PropertyType.PT_TableId));
				iconBack.spriteName = BabyData.GetPetQuality(bdata._PetQuality);
				
				int Magic =   bdata._BIG_Magic - _babyMainData.gear_[(int)BabyInitGear.BIG_Magic];
				int Stama =   bdata._BIG_Stama - _babyMainData.gear_[(int)BabyInitGear.BIG_Stama];
				int Speed =   bdata._BIG_Speed - _babyMainData.gear_[(int)BabyInitGear.BIG_Speed];
				int Power =   bdata._BIG_Power - _babyMainData.gear_[(int)BabyInitGear.BIG_Power];
				int Strength =   bdata._BIG_Strength - _babyMainData.gear_[(int)BabyInitGear.BIG_Strength];
				int num = Magic+Stama+Speed+Power+Strength;
				numsp.spriteName = BabyData.GetBabyLeveSp(num);
			}
		}
		get
		{
			return _babyMainData;
		}
	}
	void Start () {
	
	}


	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}
}
