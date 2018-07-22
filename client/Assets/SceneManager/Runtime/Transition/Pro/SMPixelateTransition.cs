//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/SceneManager/Pixelate Transition")]
public class SMPixelateTransition : SMPostRenderTransitionWithMobileShader {
	
	/// <summary>
	/// The maximum size of a pixel.
	/// </summary>
	public float maxBlockSize = 50;
	
	/// <summary>
	/// start of the pixelate effect
	/// </summary>
	public float pixelateStartOffset = 0;
	
	/// <summary>
	/// duration of the pixelate effect
	/// </summary>
	public float pixelateDuration = 2;
	
	/// <summary>
	/// start of the fade effect
	/// </summary>
	public float fadeStartOffset = 1.5f;
	
	/// <summary>
	/// duration of the fade effect
	/// </summary>
	public float fadeDuration = .5f;

	private float duration;
	private float pixelateProgress;
	private float fadeProgress;

	protected override string FullShaderName {
		get {
			return "Scene Manager/Pixelate Effect";
		}
	}

	protected override void DoPrepare() {
		duration = Mathf.Max(pixelateStartOffset + pixelateDuration, fadeStartOffset + fadeDuration);
	}

	protected override bool Process(float elapsedTime) {
		float effectTime = elapsedTime - simplifiedShaderLagCompensation;
		float calcBase = effectTime;
		// invert direction 
		if (state == SMTransitionState.In) {
			calcBase = duration - effectTime;
		}
		
		pixelateProgress = SMTransitionUtils.SmoothProgress(pixelateStartOffset, pixelateDuration, calcBase);
		fadeProgress = SMTransitionUtils.SmoothProgress(fadeStartOffset, fadeDuration, calcBase);
		
		return effectTime < duration;		
	}
	
	protected override void DoRender() {
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.LoadIdentity();

		DrawImage();
		
		GL.PopMatrix(); 
	}
	
	private void DrawImage() {	
		shaderMaterial.SetFloat("_BlockSize", pixelateProgress * maxBlockSize + 1);
		shaderMaterial.SetFloat("_FadeOffset", fadeProgress);
		for (var i = 0; i < shaderMaterial.passCount; ++i) {
			shaderMaterial.SetPass(i);
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
	}	
	
}

