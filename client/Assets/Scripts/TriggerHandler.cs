using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class TriggerHandler : MonoBehaviour {

	public delegate void EnterZone(uint id);
	public delegate void ExitZone(uint id);

	public EnterZone enterZone_;
	public ExitZone exitZone_;

	void Start()
	{
		BoxCollider bc = GetComponent<BoxCollider> ();
		if(bc == null)
			bc = gameObject.AddComponent<BoxCollider>();
		bc.isTrigger = true;
	}

	void OnTriggerEnter(Collider who)
	{
        if (!who.CompareTag("Master"))
            return;

		if(enterZone_ != null)
			enterZone_(uint.Parse(gameObject.name));
	}

	void OnTriggerExit(Collider who)
	{
        if (!who.CompareTag("Master"))
            return;

		if(exitZone_ != null)
			exitZone_(uint.Parse(gameObject.name));
	}
}
