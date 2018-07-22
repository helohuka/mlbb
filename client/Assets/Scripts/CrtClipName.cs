using UnityEngine;
using System.Collections;

public class CrtClipName : MonoBehaviour {

    string clipName;
    Animation anim;

	// Use this for initialization
	void Start () {
        anim = GetComponentInChildren<Animation>();
        if (anim != null)
        {
            foreach (AnimationState state in anim)
            {
                clipName = state.name;
                break;
            }
        }
	}

    float timer = 0f;
    float maxtime = 3f;
	// Update is called once per frame
	void Update () {
        if (string.IsNullOrEmpty(clipName))
            return;

        timer += Time.deltaTime;
        if (timer > maxtime)
        {
            if (!anim.IsPlaying(clipName))
                anim.Play(clipName);
        }
	}
}
