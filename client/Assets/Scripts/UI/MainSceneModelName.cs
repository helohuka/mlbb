using UnityEngine;
using System.Collections;

public class MainSceneModelName : MonoBehaviour
{
		public SpriteRenderer name;
		void Start ()
		{

			//GamePlayer.Instance.OpenSubSystemEnvet += new RequestEventHandler<int> (OpenSystem);
			//GamePlayer.Instance.PlayerLevelUpEvent += new RequestEventHandler<int> (OpenSystem);
			//OpenSystem (0);
		}


	public void  OpenSystem(int flag)
	{
		/*if (transform.CompareTag("arena"))
		{
			if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_JJC)) 
			{
				name.gameObject.SetActive (false);
			} 
			else 
			{
				name.gameObject.SetActive (true);
			}
		}
		
		if (transform.CompareTag("jiuba"))
		{
			if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Bar)) 
			{
				name.gameObject.SetActive (false);
			} 
			else 
			{
				name.gameObject.SetActive (true);
			}
		}
		
		if (transform.tag == "Team")
		{
			if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Castle)) 
			{
				name.gameObject.SetActive (false);
			} 
			else 
			{
				name.gameObject.SetActive (true);
			}
		}
		if (transform.tag == "PVP")
		{
			if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Hundred)) 
			{
				name.gameObject.SetActive (false);
			} 
			else 
			{
				name.gameObject.SetActive (true);
			}
		}
		if (transform.tag == "shop")
		{
			if (!GamePlayer.Instance.GetOpenSubSystemFlag (OpenSubSystemFlag.OSSF_Shop)) 
			{
				name.gameObject.SetActive (false);
			} 
			else 
			{
				name.gameObject.SetActive (true);
			}
		}
		*/

	}

	void OnDestroy()
	{
		//GamePlayer.Instance.OpenSubSystemEnvet -= OpenSystem;
		//GamePlayer.Instance.PlayerLevelUpEvent -= OpenSystem;
	}


}

