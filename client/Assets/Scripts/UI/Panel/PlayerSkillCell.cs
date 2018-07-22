using UnityEngine;
using System.Collections;

public class PlayerSkillCell : MonoBehaviour {

	public UILabel titLable;
	public UISprite backSp;
	public UITexture skillIcon;
	public UILabel skillNameLabel;
	public UILabel skillLevelLabel;
	public UISprite raSp;
	public UISprite huiSp;
	public UILabel xiaohaoLabel;
	public UISprite deyiSp;
	private COM_Skill _Skill;

    public COM_Skill SkData
	{
		set
		{
			if(value != null)
			{
                
                _Skill = value;
                SkillData skdata = SkillData.GetData((int)_Skill.skillID_,(int)_Skill.skillLevel_);
				raSp.gameObject.SetActive(false);
				titLable.gameObject.SetActive(true);
				//skillIcon.spriteName = _skData.resIconName;
                HeadIconLoader.Instance.LoadIcon(skdata._ResIconName, skillIcon);
                skillNameLabel.text = skdata._Name;
               // skillLevelLabel.text = _Skill.skillLevel_.ToString();

                int jobid = GamePlayer.Instance.GetIprop(PropertyType.PT_Profession);
                int joblv = GamePlayer.Instance.GetIprop(PropertyType.PT_ProfessionLevel);

                Profession pf = Profession.get((JobType)jobid, joblv);
                if (null != pf)
                {
                    if (pf.isProudSkill(jobid, (int)_Skill.skillID_, 0))
                    {
                        //得意技
						deyiSp.gameObject.SetActive(true);
                    }
                    else
                    {
                        //不是得意技
						deyiSp.gameObject.SetActive(false);
                    }
                
                }

				xiaohaoLabel.text = skdata._Cost_mana.ToString();
			}
		}
		get
		{
            return _Skill;
		}
	}
	public void clearObj()
	{
		skillIcon.mainTexture = null;
		skillNameLabel.text = "";
		titLable.gameObject.SetActive(false);
		deyiSp.gameObject.SetActive (false);
	}
	void Start () {
	
	}
	

}
