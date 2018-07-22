using UnityEngine;
using System.Collections;

public class MainGood : MonoBehaviour {

    public UILabel name_;

    public UISprite triangle_;

	// Use this for initialization
	void Start () {
	
	}

    public void SetData(string name)
    {
        name_.text = name;
    }

    public void SetTriangle(bool open)
    {
        if (open)
            triangle_.spriteName = "sanjiao2";
        else
            triangle_.spriteName = "sanjiao";
    }
}
