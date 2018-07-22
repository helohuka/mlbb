using UnityEngine;
using System.Collections;

public class NpcCell : MonoBehaviour {

	public UITexture icon;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UIButton goBtn;
	private NpcData _npcdata;
	public UILabel _SkillQianwang;
	public NpcData Npcdata
	{
		set
		{
			if(value != null)
			{
				_npcdata = value;
				nameLabel.text = _npcdata.Name;
				levelLabel.text = LanguageManager.instance.GetValue("babySkillChuansong").Replace("{n}",_npcdata.OpenLv.ToString());
				HeadIconLoader.Instance.LoadIcon (EntityAssetsData.GetData(_npcdata.AssetsID).assetsIocn_, icon);
			}

		}
		get
		{
			return _npcdata;
		}
	}


	void Start () {
		_SkillQianwang.text = LanguageManager.instance.GetValue("kill_Qianwang");
		UIManager.SetButtonEventHandler (goBtn.gameObject, EnumButtonEvent.OnClick, OnClickGo, (int)PropertyType.PT_Speed, 0);
	}
	void OnClickGo(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.GetIprop(PropertyType.PT_Level)<Npcdata.OpenLv)
		{

			PopText.Instance.Show(LanguageManager.instance.GetValue("pdengjibuzu").Replace("{n}",Npcdata.OpenLv.ToString()));

		}else
		{
			if (GameManager.Instance.ParseNavMeshInfo(Npcdata.Transfer))
			{
				Prebattle.Instance.ChangeWalkEff(Prebattle.WalkState.WS_AFP);
			}
		}

       
	}

}
