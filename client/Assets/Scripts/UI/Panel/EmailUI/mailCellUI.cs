using UnityEngine;
using System.Collections;

public class mailCellUI : MonoBehaviour
{

	public UILabel nameLab;
	public UISprite icon;
	private COM_Mail _mail;
	public UISprite back;


	void Start ()
	{

	}


	public COM_Mail Mail
	{
		set
		{
			if(value != null)
			{
				_mail = value;
				nameLab.text= value.title_;

				if(!_mail.isRead_)
				{
					icon.spriteName = "guanbixin";
				}
				else
				{
					icon.spriteName = "dakaixin";
				}
			}
		}
		get
		{
			return _mail;
		}
	}

}

