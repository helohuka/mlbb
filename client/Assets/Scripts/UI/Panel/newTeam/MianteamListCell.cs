using UnityEngine;
using System.Collections;

public class MianteamListCell : MonoBehaviour {

	public UITexture icon;
	public UISprite zhiyeIcon;
	public UILabel levelLabel;
	public UILabel nameLabel;
	public UISlider hpSlider;
	public UISlider mhSlider;
	public UIButton iconBtn;
	public UISprite LeadersP;
	public UISprite heiSp;
	public bool isLeader_;
	private COM_SimplePlayerInst  _SimpleInformation;
	public COM_SimplePlayerInst SimpleInformation
	{
		set
		{
			if(value != null)
			{
				_SimpleInformation = value;
				//icon.spriteName = EntityAssetsData.GetData(_SimpleInformation.asset_id_).assetsIocn_;
				string n = "xiao_"+EntityAssetsData.GetData((int)_SimpleInformation.properties_[(int)PropertyType.PT_AssetId]).assetsIocn_;
				HeadIconLoader.Instance.LoadIcon (n, icon);
				levelLabel.text = _SimpleInformation.properties_[(int)PropertyType.PT_Level].ToString();
				nameLabel.text = _SimpleInformation.instName_;
				//HeadIconLoader.Instance.LoadIcon (Profession.get ((JobType)_SimpleInformation.jt_,_SimpleInformation.jl_).jobtype_.ToString(), zhiyeIcon);
				//hpSlider.value = _SimpleInformation.
				zhiyeIcon.spriteName = Profession.get ((JobType)_SimpleInformation.properties_[(int)PropertyType.PT_Profession],((int)_SimpleInformation.properties_[(int)PropertyType.PT_ProfessionLevel])).jobtype_.ToString();
//				if(TeamSystem.IsTeamLeader())
//				{
//					LeadersP.spriteName = "duizhang(1)";
//				}else
//				{
//					LeadersP.spriteName = "duiyuan(1)";
//				}
//				BloodChanged(_SimpleInformation.properties_[(int)PropertyType.PT_HpCurr],_SimpleInformation.properties_[(int)PropertyType.PT_HpMax]);
//				MagicChanged(_SimpleInformation.properties_[(int)PropertyType.PT_MpCurr],_SimpleInformation.properties_[(int)PropertyType.PT_MpMax]);
			}
		}
		get
		{
			return _SimpleInformation;
		}
	}
	private	void  BloodChanged(float bloodValue, float maxvalue)
	{
		if (hpSlider == null)return;
		hpSlider.value += bloodValue * 1f / maxvalue * 1f;
		
	}
	private void MagicChanged(float MagicValue,  float maxvalue)
	{
		if (mhSlider == null)return;
		mhSlider.value += MagicValue * 1f / maxvalue * 1f;
	}
	void Start () {
		//UIManager.SetButtonEventHandler(iconBtn.gameObject, EnumButtonEvent.OnClick, OnRighticon, 0, 0);
		isLeader_ = TeamSystem.IsTeamLeader ();

		//LeaderSprite ();	
		//TeamSystem.OnTeamDirtyProps += updatePros;
		updatePros ();
	}

	void updatePros()
	{
		BloodChanged(_SimpleInformation.properties_[(int)PropertyType.PT_HpCurr],_SimpleInformation.properties_[(int)PropertyType.PT_HpMax]);
		MagicChanged(_SimpleInformation.properties_[(int)PropertyType.PT_MpCurr],_SimpleInformation.properties_[(int)PropertyType.PT_MpMax]);
	}
	void OnRighticon(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.InstId == SimpleInformation.instId_)
		{
			MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("tuiduiMessage"),()=>{
				NetConnection.Instance.exitTeam();
				NetConnection.Instance.exitLobby();
			});
		}
	}

}
