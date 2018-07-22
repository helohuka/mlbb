using UnityEngine;
using System.Collections;

public class DestroyByTimeEnd : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		GameObject.Destroy ( this.gameObject , 5f );
	}
}
