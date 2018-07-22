using UnityEngine;
using System.Collections;

public class SystemChatCell : MonoBehaviour {

	public UILabel ContentsLabel;
	private COM_ChatInfo ChatInfo_;	
	public COM_ChatInfo ChatInfo
	{
		set
		{
			if(value != null)
			{
				ChatInfo_ = value;
				ContentsLabel.text = ChatInfo_.content_;
			}
		}
		get
		{
			return ChatInfo_;
		}
	}




	void Start () {
	
	}
	

}
