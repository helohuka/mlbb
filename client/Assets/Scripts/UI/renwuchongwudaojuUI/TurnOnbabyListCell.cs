using UnityEngine;
using System.Collections;

public class TurnOnbabyListCell : MonoBehaviour {

	public UISprite iconkuang;
	public UISprite raceSp;
	public UISprite pinjiSp;
	public UISprite backsp;
	public UITexture icon;
	public UILabel nameLable;
	public UILabel levelLabel;
	public GameObject suo;
	private Baby _baby;
	public Baby babyData
	{
		set
		{
			if(value != null)
			{
				_baby = value;
				nameLable.text = _baby.InstName;
				levelLabel.text = _baby.GetIprop(PropertyType.PT_Level).ToString();
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(BabyData.GetData(_baby.GetIprop(PropertyType.PT_TableId))._AssetsID).assetsIocn_, icon);
				BabyData bdata = BabyData.GetData(_baby.GetIprop(PropertyType.PT_TableId));
				iconkuang.spriteName = BabyData.GetPetQuality(bdata._PetQuality);

				int Magic =   bdata._BIG_Magic - _baby.gear_[(int)BabyInitGear.BIG_Magic];
				int Stama =   bdata._BIG_Stama - _baby.gear_[(int)BabyInitGear.BIG_Stama];
				int Speed =   bdata._BIG_Speed - _baby.gear_[(int)BabyInitGear.BIG_Speed];
				int Power =   bdata._BIG_Power - _baby.gear_[(int)BabyInitGear.BIG_Power];
				int Strength =   bdata._BIG_Strength - _baby.gear_[(int)BabyInitGear.BIG_Strength];
				int num = Magic+Stama+Speed+Power+Strength;
				pinjiSp.spriteName = BabyData.GetBabyLeveSp(num);
				suo.SetActive(_baby.GetInst().isLock_);
			}
		}
		get
		{
			return _baby;
		}
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
