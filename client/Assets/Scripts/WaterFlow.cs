using UnityEngine;
using System.Collections;

public class WaterFlow : MonoBehaviour {

    public float m_SpeedU = 0.1f;
    public float m_SpeedV = -0.1f;

	// Update is called once per frame
	void Update () {
        float newOffsetU = Time.time * m_SpeedU;
        float newOffsetV = Time.time * m_SpeedV;

        if (this.renderer)
        {
            renderer.material.mainTextureOffset = new Vector2(newOffsetU, newOffsetV);
        }
	}
}