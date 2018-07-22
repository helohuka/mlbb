//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/SceneManager/Cinema Transition")]
public class SMCinemaTransition : SMPostRenderTransition {

	/// <summary>
	/// The size of the border in pixels (if geater than 1) or relative to the screen (otherwise).
	/// </summary>
	public float borderSize = .15f;
	
	/// <summary>
	/// start of the border animation
	/// </summary>
	public float borderStartOffset = 0;
	
	/// <summary>
	/// duration of the border animation
	/// </summary>
	public float borderSlideDuration = 1f;
	
	/// <summary>
	/// start of the tint effect
	/// </summary>
	public float tintStartOffset = 0.5f;
	
	/// <summary>
	/// duration of the tint effect
	/// </summary>
	public float tintDuration = 1f;
	
	/// <summary>
	/// start of the fade out effect
	/// </summary>
	public float fadeOutStartOffset = 2f;
	
	/// <summary>
	/// duration of the fade out effect
	/// </summary>
	public float fadeOutDuration = 1f;
	
	private Material tintMaterial;
	
	private float actualBorderSize;
	private float duration;
	private float borderProgress;
	private float tintProgress;
	private float fadeProgress;

	protected override void Prepare() {
		if (tintMaterial == null) {
			tintMaterial = new Material(Shader.Find("Scene Manager/Cinema Effect"));
		}
		
		duration = fadeOutStartOffset + fadeOutDuration;	
		float borderSizeInPixel = SMTransitionUtils.ToAbsoluteSize(borderSize, Screen.height);
		actualBorderSize = borderSizeInPixel / Screen.height;
	}
		
	protected override bool Process(float elapsedTime) {
		float effectTime = elapsedTime;
		// invert direction 
		if (state == SMTransitionState.In) {
			effectTime = duration - effectTime;
		}
		
		borderProgress = SMTransitionUtils.SmoothProgress(borderStartOffset, borderSlideDuration, effectTime);
		tintProgress = SMTransitionUtils.SmoothProgress(tintStartOffset, tintDuration, effectTime);
		fadeProgress = SMTransitionUtils.SmoothProgress(fadeOutStartOffset, fadeOutDuration, effectTime);
		
		return elapsedTime < duration;		
	}
	
	protected override void OnRender() {
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.LoadIdentity();

		DrawImage();
		DrawBorder();
		
		GL.PopMatrix(); 
	}
	
	private void DrawImage() {	
		tintMaterial.SetFloat("_TintOffset", tintProgress);
		tintMaterial.SetFloat("_FadeOffset", fadeProgress);
		for (var i = 0; i < tintMaterial.passCount; ++i) {
			tintMaterial.SetPass(i);
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
	
	private void DrawBorder() {
		float height = actualBorderSize * borderProgress;
		if (height > 0) {
			for (var i = 0; i < holdMaterial.passCount; ++i) {
				holdMaterial.SetPass(i);
				GL.Begin(GL.QUADS);
				GL.TexCoord3(0, 0, 0);
				GL.Vertex3(0, 1 - height, 0);
				GL.TexCoord3(0, 1, 0);
				GL.Vertex3(0, 1, 0);
				GL.TexCoord3(1, 1, 0);
				GL.Vertex3(1, 1, 0);
				GL.TexCoord3(1, 0, 0);
				GL.Vertex3(1, 1 - height, 0);
				
				GL.TexCoord3(0, 0, 0);
				GL.Vertex3(0, 0, 0);
				GL.TexCoord3(0, 1, 0);
				GL.Vertex3(0, height, 0);
				GL.TexCoord3(1, 1, 0);
				GL.Vertex3(1, height, 0);
				GL.TexCoord3(1, 0, 0);
				GL.Vertex3(1, 0, 0);				
				GL.End();
			}				
		}
	}
	
}

