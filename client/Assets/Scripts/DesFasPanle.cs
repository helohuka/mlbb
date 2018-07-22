using UnityEngine;
using System.Collections;

public class DesFasPanle : MonoBehaviour {

	private float lifetime = 5.0f;		
	void OnEnable()
	{
		Invoke("DestorySelf", lifetime);
	}
	void OnDisable()
	{

		CancelInvoke ("DestorySelf");
		lifetime = 5.0f;
		if(!GamePlayer.Instance.isUseCloseBtn)
		{
			FastUpload.Instance.UseAll ();
			FastUsePanel fa = gameObject.GetComponent<FastUsePanel>();
			fa.OnUseClick();
			DestorySelf();

		}
		GamePlayer.Instance.isUseCloseBtn = false;
	}
	void DestorySelf()
	{
		gameObject.SetActive (false);
	}
}
