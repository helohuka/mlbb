using UnityEngine;
using System.Collections;

public class babySkillCell : MonoBehaviour {


	public UILabel mofaObj;

	public UISprite backSp;
	public UITexture IocnSp;
	public UILabel nameLabel;
	public UILabel levelLabel;
	public UILabel xiaohaoLabel;
	public UISprite huiSp;

	private SkillData _skData;
	
	public SkillData SkData
	{
		set
		{
			if(value != null)
			{
				_skData = value;
				//IocnSp.spriteName = _skData.resIconName;
				mofaObj.gameObject.SetActive(true);
				nameLabel.text =_skData._Name;
				levelLabel.text = _skData._Level.ToString();
				xiaohaoLabel.text = _skData._Cost_mana.ToString();
				HeadIconLoader.Instance.LoadIcon (_skData._ResIconName, IocnSp);
				if(_skData._SkillType == SkillType.SKT_CannotUse)
				{
					huiSp.gameObject.SetActive(true);
					gameObject.GetComponent<UIButton>().isEnabled = false;
				}else
				{
					huiSp.gameObject.SetActive(false);
					gameObject.GetComponent<UIButton>().isEnabled = true;
				}
				
			}
		}
		get
		{
			return _skData;
		}
	}
	public void clearObj()
	{
		IocnSp.mainTexture = null;
		nameLabel.text = "";
		mofaObj.gameObject.SetActive(false);

	}

	void Start () {
	
	}
	

}
