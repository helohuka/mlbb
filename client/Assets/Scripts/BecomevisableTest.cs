using UnityEngine;
using System.Collections;

public class BecomevisableTest : MonoBehaviour {

    public delegate void VisableHandler(bool visable);
    public event VisableHandler OnVisable;

    void OnBecameVisible()
    {
        if (OnVisable != null)
            OnVisable(true);
    }

	void OnBecameInvisible()
    {
        if (OnVisable != null)
            OnVisable(false);
    }
}
