using UnityEngine;
using System.Collections;

public class UvAnimator : MonoBehaviour
{
	public float tileX = 24;
	public float tileY = 1;
	public float fps = 10.0f;
	
	// Update is called once per frame
	void Update ()
    {
		int index = (int)(Time.time * fps);
		index = index % (int)(tileX * tileY);
		Vector2 size = new Vector2(1.0f / tileX, 1.0f / tileY);
		
		int u = (int)(index % tileX);
		int v = (int)(index / tileX);
		
		Vector2 offset = new Vector2(u * size.x, v * size.y);
		
		renderer.material.SetTextureOffset("_MainTex", offset);
		renderer.material.SetTextureScale("_MainTex", size);
	}
}
