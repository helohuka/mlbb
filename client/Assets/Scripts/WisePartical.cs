using UnityEngine;
using System.Collections;

public class WisePartical : MonoBehaviour {

    BecomevisableTest bvt;

    // Use this for initialization
    void Start()
    {
        GameObject shadow = GameObject.CreatePrimitive(PrimitiveType.Cube);
        shadow.transform.position = gameObject.transform.position;
        shadow.transform.localScale = Vector3.one * 0.0001f;
        shadow.transform.rotation = gameObject.transform.rotation;

        bvt = shadow.AddComponent<BecomevisableTest>();
        bvt.OnVisable += OnVisable;
    }

    void OnVisable(bool visable)
    {
        if(gameObject != null)
            gameObject.SetActive(visable);
    }

    void OnDestroy()
    {
        bvt.OnVisable -= OnVisable;
    }
}
