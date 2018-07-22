using UnityEngine;
using System.Collections;

public class AttachCamera : MonoBehaviour
{
	Transform myTransform;
	public Transform target;
	public Vector3 offset = new Vector3(0, 5, -5);
	
	void Start()
	{
		myTransform = this.transform;
	}
	
	void FixedUpdate()
	{
		if (target != null)
		{
			myTransform.position = target.position + offset;
			myTransform.LookAt(target.position, Vector3.up);
		}
	}
}
