using UnityEngine;
using System.Collections;

public class BabySkillEnter : MonoBehaviour {

	public UIButton closeBtn;
	public UIButton enterBtn;
	public UILabel crunamel;
	public UILabel curdec;
	public UITexture curicon;
	public UITexture newicon;
	public UILabel newnamel;
	public UILabel newdec;
	public int oldskillId;
	public int newSkillId;
	public int newSkillLevel;
	public SkillData curData
	{
		set
		{
			crunamel.text = value._Name;
			curdec.text = value._Desc;
			HeadIconLoader.Instance.LoadIcon (value._ResIconName, curicon);
		}
	}
	public SkillData newData
	{
		set
		{
			newnamel.text = value._Name;
			newdec.text = value._Desc;
			HeadIconLoader.Instance.LoadIcon (value._ResIconName, newicon);
		}
	}
	void Start () {
		UIManager.SetButtonEventHandler (closeBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose,0, 0);
		UIManager.SetButtonEventHandler (enterBtn.gameObject, EnumButtonEvent.OnClick, OnClickenterBtn,0, 0);
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		gameObject.SetActive (false);
	}
	void OnClickenterBtn(ButtonScript obj, object args, int param1, int param2)
	{
		if(GamePlayer.Instance.Properties[(int)PropertyType.PT_Money]<BabySkillLearning.Price) 
		{
			//ErrorTipsUI.ShowMe("金币不足");
			PopText.Instance.Show(LanguageManager.instance.GetValue("nomoney"));
		}else if(!IsArrivalsLevel())
		{
			//ErrorTipsUI.ShowMe("等级不足");
			PopText.Instance.Show(LanguageManager.instance.GetValue("EN_OpenBaoXiangLevel"));
		}else
		{
			NetConnection.Instance.babyLearnSkill((uint)BabyList.babyId,(uint)oldskillId,(uint)newSkillId,(uint)newSkillLevel);	
		}
		gameObject.SetActive (false);
	}
	bool IsArrivalsLevel()
	{
		SkillData sdata = SkillData.GetMinxiLevelData (BabySkillLearning.SkillId);
		if(sdata._IsPhysic == true)
		{
			int ilevel = GamePlayer.Instance.GetBabyInst(BabyList.babyId).GetIprop(PropertyType.PT_Level);
			int slevel = ilevel / 20 + 1;
			return (slevel >= sdata._Level);
		}
		else
		{
			int ilevel = GamePlayer.Instance.GetBabyInst(BabyList.babyId).GetIprop(PropertyType.PT_Level);
			int slevel = ilevel / 10 + 1;
			return (slevel >= sdata._Level);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
