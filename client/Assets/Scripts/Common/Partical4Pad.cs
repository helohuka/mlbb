using UnityEngine;
using System.Collections;

public class Partical4Pad : MonoBehaviour {

    //scale partical on uiroot.
    public void SetScale()
    {
        if (GameManager.Instance.IsPad)
        {
            ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.transform.localScale *= 0.75f;
                ps.startSize *= 0.75f;
                ps.startSpeed *= 0.75f;
            }
            ParticleSystem[] pars = gameObject.GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < pars.Length; ++i)
            {
                pars[i].transform.localScale *= 0.75f;
                pars[i].startSize *= 0.75f;
                pars[i].startSpeed *= 0.75f;
            }
        }
    }
}
