using UnityEngine;
using System.Collections;

public class NavTest : MonoBehaviour {

	public Transform target;
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {

		if (!target)
			return;

		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
		
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

        //currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        currentRotationAngle = wantedRotationAngle;

        //currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        currentHeight = wantedHeight;

		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		transform.position = target.position;
		transform.position -=  Vector3.forward * distance;

		transform.position = new Vector3(transform.position.x,currentHeight,transform.position.z) ;

		transform.LookAt (target);
	
	}
}
