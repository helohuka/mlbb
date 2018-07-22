using UnityEngine;
using System.Collections;

public class TitleCell : MonoBehaviour {

	public UILabel titleLabel;
    private	PlayerTitleData _titleData;
	public PlayerTitleData TitleData
	{
		set{
			if(value != null)
			{
				_titleData = value;
				titleLabel.text = _titleData.desc_;
			}
		}
		get{
			return _titleData;
		}
	}
	void Start () {
	
	}
	

}
