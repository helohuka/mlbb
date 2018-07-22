using UnityEngine;
using System.Collections;

public class CinemaMaskHandler : MonoBehaviour {

    bool skipShown_ = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        if (!skipShown_)
            CinemaUI.ShowMe();
    }
}
