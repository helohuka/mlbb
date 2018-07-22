using UnityEngine;
using System.Collections;
[RequireComponent (typeof(AudioSource))]
public class ShowLineCell : MonoBehaviour {

	public UISprite DiKuangSp;
	public UILabel ContentsLabel;
	public UIButton vecBtn;
	public bool isVec = false;
	private AudioClip audioC;
	private COM_ChatInfo ChatInfo_;
	AudioSource audioSource;
	public COM_ChatInfo ChatInfo
	{
		set
		{
			if(value != null)
			{
				ChatInfo_ = value;

				if(isVec)
				{
					vecBtn.gameObject.SetActive(true);
					ContentsLabel.gameObject.SetActive(false);
					//audioC = (AudioClip)ChatSystem.instance.CreateClip(ChatInfo_.audio_);
				}else
				{
					ContentsLabel.gameObject.SetActive(true);
					vecBtn.gameObject.SetActive(false);
						
				}				

				if(ChatInfo_.ck_ == ChatKind.CK_System)
				{
					ContentsLabel.text = ChatInfo_.content_;	
				}else
				{
					ContentsLabel.text ="["+ChatInfo_.playerName_+"]"+ ChatInfo_.content_;
				}






				DiKuangSp.width = ContentsLabel.width+20;
				DiKuangSp.height = ContentsLabel.height;
			}
		}
		get
		{
			return ChatInfo_;
		}
	}


	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		if(audioSource == null)return;
		audioSource.enabled = true;
		UIManager.SetButtonEventHandler (vecBtn.gameObject, EnumButtonEvent.OnClick, OnClickPlayVec, 0, 0);
	}
	private void OnClickPlayVec(ButtonScript obj, object args, int param1, int param2)
	{
		//ChatSystem.instance.PlayRecord(ChatInfo.audio_,audioSource);
	}

}
