using UnityEngine;
using System.Collections;

public class BabyLearningCell : MonoBehaviour {

	public UILabel nameLabel;
	public UITexture icon;
	public UISprite rasp;
	private SkillData skData_;
	public SkillData SkpData
	{
		set
		{
			if(value != null)
			{
				skData_ = value;
				nameLabel.text = skData_._Name;
				//icon.spriteName = skData_.resIconName;
				HeadIconLoader.Instance.LoadIcon (skData_._ResIconName, icon);
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
