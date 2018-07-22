using UnityEngine;
using System.Collections;

public class BabySkillInfo : MonoBehaviour {

	public GameObject babyList;
	public UITexture skillIcon;
	public UISprite CurrencyIcon;
	public UILabel nameLabel;
	public UILabel desLabel;
	public UILabel CurrencyLabel;
	public UIButton BeginBtn;
	private SkillData skData_;
	public SkillData SkpData
	{
		set
		{
			if(value != null)
			{
				skData_ = value;
				nameLabel.text = skData_._Name;
				desLabel.text = skData_._Desc;
				CurrencyLabel.text = skData_._LearnCoin.ToString();
				//skillIcon.spriteName = skData_.resIconName;
				HeadIconLoader.Instance.LoadIcon (skData_._ResIconName, skillIcon);
				BabySkillLearning.Price = skData_._LearnCoin;
			}
		}
		get
		{
			return skData_;
		}
	}

	void Start () {
		UIManager.SetButtonEventHandler (BeginBtn.gameObject, EnumButtonEvent.OnClick, OnClickBegin, 0, 0);

        GuideManager.Instance.RegistGuideAim(BeginBtn.gameObject, GuideAimType.GAT_BabySkillLearningBtn);

        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_BabyLearningSkillUI);
    }
	void OnClickBegin(ButtonScript obj, object args, int param1, int param2)
	{
		babyList.SetActive (true);
		//NetConnection.Instance.learnSkill ((uint)SkpData.id_);
	}

}
