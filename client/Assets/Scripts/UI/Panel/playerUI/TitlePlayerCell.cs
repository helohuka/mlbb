using UnityEngine;
using System.Collections;

public class TitlePlayerCell : MonoBehaviour {

	public UILabel nameLable;
	public UIButton CloseBtn;
	public UIButton genghuaBtn;

	public TitleData tData_;
	public TitleData titData
	{
		set
		{
			if(value != null)
			{
				tData_ = value;
				nameLable.text = tData_.desc_;
				UIManager.SetButtonEventHandler (CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);
				UIManager.SetButtonEventHandler (genghuaBtn.gameObject, EnumButtonEvent.OnClick, OnClickGenghua, 0, 0);
			}
		}get
		{
			return tData_;
		}
	}
	void OnClickClose(ButtonScript obj, object args, int param1, int param2)
	{
		Destroy (gameObject);
	}
	void OnClickGenghua(ButtonScript obj, object args, int param1, int param2)
	{
		NetConnection.Instance.setCurrentTitle (titData.id_);
		Destroy (gameObject);
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
