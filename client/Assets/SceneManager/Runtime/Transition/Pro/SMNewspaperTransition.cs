//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/SceneManager/Newspaper Transition")]
public class SMNewspaperTransition : SMPostRenderTransitionWithMobileShader {
	
	/// <summary>
	/// rotation angle
	/// </summary>
	public float angle = 360 * 3;
	
	/// <summary>
	/// duration of the effect
	/// </summary>
	public float duration = 2;
	private float progress;

		
	protected override bool Process(float elapsedTime) {
		float effectTime = elapsedTime - simplifiedShaderLagCompensation;
		float calcBase = effectTime;
		// invert direction 
		if (state == SMTransitionState.In) {
			calcBase = duration - effectTime;
		}
		
		progress = SMTransitionUtils.SmoothProgress(0, duration, calcBase);
		
		return effectTime < duration;		
	}
	
	protected override void DoRender() {
		GL.PushMatrix();
		// pixel matrix instead of orthogonal to maintain aspect ratio during rotation
		GL.LoadPixelMatrix();
		GL.LoadIdentity();
		DrawImage();

		// background is z-tested and drawn behind the image. Doing it this way, allows us to skip
		// one grabpass when drawing the background therefore saving at least one drawcall
		DrawBackground();
		
		GL.PopMatrix(); 
	}

	private void DrawImage() {
		float dx = Screen.width / 2f;
		float dy = Screen.height / 2f;
		GL.PushMatrix();
		Quaternion rotation = Quaternion.AngleAxis(progress * angle, Vector3.forward);
		GL.MultMatrix(Matrix4x4.TRS(new Vector3(dx, dy, 0), rotation, Vector3.one * (1 - progress)));
		for (var i = 0; i < shaderMaterial.passCount; ++i) {
			shaderMaterial.SetPass(i);
			GL.Begin(GL.QUADS);
			GL.TexCoord3(0, 0, 0);
			GL.Vertex3(-dx, -dy, 0);
			GL.TexCoord3(0, 1, 0);
			GL.Vertex3(-dx, dy, 0);
			GL.TexCoord3(1, 1, 0);
			GL.Vertex3(dx, dy, 0);
			GL.TexCoord3(1, 0, 0);
			GL.Vertex3(dx, -dy, 0);
			GL.End();
		}	
		GL.PopMatrix();
	}
}

