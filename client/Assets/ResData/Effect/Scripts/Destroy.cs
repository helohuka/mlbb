using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {

    public float lifetime = 2.0f;

    public delegate void FinishCallBack();
    public event FinishCallBack OnPlayFinish;

    void Awake()
    {
        Invoke("DestorySelf", lifetime);
    }

	public void SetLifeTime(float time)
	{
		CancelInvoke ("DestorySelf");
		Invoke("DestorySelf", time);
	}

    void DestorySelf()
    {
        if (OnPlayFinish != null)
            OnPlayFinish();
        Destroy(gameObject);
    }
}
