using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Desaturate")]
public class DesaturateEffect : ImageEffectBase {
	public float desaturateAmount;
	public Texture textureRamp;
	public float rampOffsetR;
	public float rampOffsetG;
	public float rampOffsetB;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetTexture("_RampTex", textureRamp);
		material.SetFloat("_Desat", desaturateAmount);
		material.SetVector("_RampOffset", new Vector4 (rampOffsetR, rampOffsetG, rampOffsetB, 0));
		ImageEffects.BlitWithMaterial(material, source, destination);
	}
}