using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BabyListCell : MonoBehaviour {

	//public UITexture RaceSp;
	public UITexture IconSp;
	public UILabel namel;
	public UISprite iconBack;
	public UISprite stypesp;
	//public UISprite numsp;
	public UISprite stateSp;
	public UISprite backImg;
	private BabyData _bdata;
	private List<string> _icons = new List<string>();
	public BabyData Bdata
	{
		set
		{
			if(value != null)
			{
				_bdata = value;
				//RaceSp.spriteName = _bdata.RaceIcon_;
				//IconSp.spriteName = _bdata.resIcon_;

				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(_bdata._AssetsID).assetsIocn_, IconSp);

				if(!_icons.Contains(EntityAssetsData.GetData(_bdata._AssetsID).assetsIocn_))
				{
					_icons.Add(EntityAssetsData.GetData(_bdata._AssetsID).assetsIocn_);
				}
				stypesp.spriteName = _bdata._Tpye.ToString();
				//HeadIconLoader.Instance.LoadIcon (_bdata._RaceIcon, RaceSp);
                if (!_icons.Contains(_bdata._RaceIcon))
                {
                    _icons.Add(_bdata._RaceIcon);
                }
				namel.text = _bdata._Name;

				//UIManager.SetButtonEventHandler (examineBtn.gameObject, EnumButtonEvent.OnClick, OnClickExamine, _bdata.id_, 0);
				if(TuJianUI.IsCaptureBaby(_bdata._Id))
				{
					stateSp.gameObject.SetActive(true);
				}else
				{
					stateSp.gameObject.SetActive(false);
				}



				
//				int Magic =   bdata.BIG_Magic_ - _babyMainData.gear_[(int)BabyInitGear.BIG_Magic];
//				int Stama =   bdata.BIG_Stama_ - _babyMainData.gear_[(int)BabyInitGear.BIG_Stama];
//				int Speed =   bdata.BIG_Speed_ - _babyMainData.gear_[(int)BabyInitGear.BIG_Speed];
//				int Power =   bdata.BIG_Power_ - _babyMainData.gear_[(int)BabyInitGear.BIG_Power];
//				int Strength =   bdata.BIG_Strength_ - _babyMainData.gear_[(int)BabyInitGear.BIG_Strength];
//				int num = Magic+Stama+Speed+Power+Strength;
				//numsp.spriteName = BabyData.GetBabyLeveSp(num);


			}
		}
		get
		{
			return _bdata;
		}
	}
//	void OnClickExamine(ButtonScript obj, object args, int param1, int param2)
//	{
//		TuJianUI.babyId = param1;
//		TuJianUI.Instance.OpenBabyInfoObj ();
////		BabyInfo binfo = TuJianUI.Instance.babyInfoObj.GetComponent<BabyInfo> ();
////		binfo.Bdata = BabyData.GetData (param1);
////		TuJianUI.Instance.babyListObj.SetActive (false);
//	}

	void Start () {
		iconBack.spriteName = BabyData.GetPetQuality(Bdata._PetQuality);
	}



	void OnDestroy()
	{
		for(int n = 0;n<_icons.Count;n++)
		{
			HeadIconLoader.Instance.Delete(_icons[n]);
		}
	}



}
