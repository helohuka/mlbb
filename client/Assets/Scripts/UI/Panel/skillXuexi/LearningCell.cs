using UnityEngine;
using System.Collections;

public class LearningCell : MonoBehaviour {

	public UITexture icon;
	public UILabel nameLabel;
	public UISprite raSp;
	public UISprite xideSp;
	public UISprite deyiSp;
	private Profession prof;
	private SkillData skData_;
	public SkillData SkpData
	{
		set
		{
			if(value != null)
			{
				skData_ = value;
				raSp.gameObject.SetActive(false);
				prof = Profession.get ((JobType)GamePlayer.Instance.GetIprop(PropertyType.PT_Profession), GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
				bool isdeyi =	prof.isProudSkill(GamePlayer.Instance.GetIprop(PropertyType.PT_Profession),skData_._Id,GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel));
				nameLabel.text = skData_._Name;
				//icon.spriteName = skData_.resIconName;
				HeadIconLoader.Instance.LoadIcon (skData_._ResIconName, icon);
				if(isdeyi)
				{
					deyiSp.gameObject.SetActive(true);
				}else
				{
					deyiSp.gameObject.SetActive(false);
				}
				if(LearningUI.Instance.isLearn(skData_._Id))
				{
					xideSp.gameObject.SetActive(true);
				}else
				{
					xideSp.gameObject.SetActive(false);
				}
			}
		}
		get
		{
			return skData_;
		}
	}
	void Start () {
	
	}
	

}
