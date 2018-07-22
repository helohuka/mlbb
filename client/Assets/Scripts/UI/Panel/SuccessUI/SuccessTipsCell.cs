using UnityEngine;
using System.Collections;

public class SuccessTipsCell : MonoBehaviour {

	public UIButton CloseBtn;
	public UILabel nameLabel;
    private AchievementContent content_;
    public AchievementContent content
	{
		set
		{
			if(value !=null)
			{
                content_ = value;
                nameLabel.text = content_.data_._AtName;
			}
		}
		get
		{
            return content_;
		}
	}
	void Start () {
		UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickDesObj, 0, 0);

	}
	void OnClickDesObj(ButtonScript obj, object args, int param1, int param2)
	{
		Destroy (gameObject);
	}

}
