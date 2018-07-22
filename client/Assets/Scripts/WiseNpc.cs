using UnityEngine;
using System.Collections;

public class WiseNpc : MonoBehaviour {

    Renderer render = null;

    Animator animtor;

    Animation animation;

    UvAnimator uvAnimator;

	// Use this for initialization
	void Start () {
        animtor = GetComponent<Animator>();
        animation = GetComponent<Animation>();
        uvAnimator = GetComponent<UvAnimator>();
        if (render == null)
        {
            render = FindBodyRenderer(transform);
            
            if (render != null)
            {
                BecomevisableTest bvt = render.gameObject.AddComponent<BecomevisableTest>();
                bvt.OnVisable += OnVisable;
            }
        }
	}

    Renderer FindBodyRenderer(Transform root)
    { 
        if(root.CompareTag("Body"))
            return root.GetComponent<Renderer>();
        for (int i = 0; i < root.childCount; ++i)
        {
            Renderer render = FindBodyRenderer(root.GetChild(i));
            if (render != null)
                return render;
        }
        return null;
    }
	
    void OnVisable(bool visable)
    {
        if (animtor != null)
            animtor.enabled = visable;
        if (animation != null)
            animation.enabled = visable;
        if (uvAnimator != null)
            uvAnimator.enabled = visable;
    }
}
