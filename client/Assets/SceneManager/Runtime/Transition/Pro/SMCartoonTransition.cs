//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/SceneManager/Cartoon Transition")]
public class SMCartoonTransition : SMPostRenderTransition {
				
	public Color borderColor = new Color(.5f, 0, 0, 1);
	public Vector2 center = new Vector2(.5f, .5f);	
	public float duration = 2;
	
	private Material material;
	private float time;
	private float length;
	private float progress;
	
	protected override void Prepare() {
		if (material == null) {
			material = new Material(Shader.Find("Scene Manager/Cartoon Effect"));
			material.SetTexture("_Background", holdMaterial.mainTexture);
		}
		
		Vector2 pixelCenter = new Vector2(SMTransitionUtils.ToAbsoluteSize(center.x, Screen.width), 
			SMTransitionUtils.ToAbsoluteSize(center.y, Screen.height));
		
		Vector2 bottomLeftPath = pixelCenter - new Vector2(0, 0);
		Vector2 topLeftPath = pixelCenter - new Vector2(0, Screen.height);
		Vector2 topRightPath = pixelCenter - new Vector2(Screen.width, Screen.height);
		Vector2 bottomRightPath = pixelCenter - new Vector2(Screen.width, 0);
		
		length = Mathf.Max(bottomLeftPath.magnitude, topLeftPath.magnitude, topRightPath.magnitude, bottomRightPath.magnitude);	
		material.SetFloat("_CenterX", pixelCenter.x);
		material.SetFloat("_CenterY", pixelCenter.y);
		
		material.SetColor("_BorderColor", borderColor);
	}

	protected override bool Process(float elapsedTime) {
		float effectTime = elapsedTime;
		// invert direction 
		if (state == SMTransitionState.In) {
			effectTime = duration - effectTime;
		}
		
		progress = SMTransitionUtils.SmoothProgress(0, duration, effectTime);
		
		return elapsedTime < duration;
	}

	protected override void OnRender() {
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.LoadIdentity();

		material.SetFloat("_Distance", length * (1 - progress));
		for (var i = 0; i < material.passCount; ++i) {
			material.SetPass(i);
			GL.Begin(GL.QUADS);
			GL.TexCoord3(0, 0, 0);
			GL.Vertex3(0, 0, 0);
			GL.TexCoord3(0, 1, 0);
			GL.Vertex3(0, 1, 0);
			GL.TexCoord3(1, 1, 0);
			GL.Vertex3(1, 1, 0);
			GL.TexCoord3(1, 0, 0);
			GL.Vertex3(1, 0, 0);
			GL.End();
		}

		GL.PopMatrix(); 
	}
}

