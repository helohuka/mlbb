//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// A transition that simply fades out the old level and fades in the new one.
/// </summary>
[AddComponentMenu("Scripts/SceneManager/Fade Transition")]
public class SMFadeTransition : SMTransition {
	
	/// <summary>
	///  The fade time.
	/// </summary>
	public float duration = 1f;
	
	/// <summary>
	/// The overlay texture.
	/// </summary>
	public Texture overlayTexture;
	
	private float progress;
	
	void Awake() {
		if (overlayTexture == null) {
			Debug.LogError("Overlay texture is missing");
		}
	}
	
	protected override bool Process(float elapsedTime) {
		float effectTime = elapsedTime;
		// invert direction if necessary
		if (state == SMTransitionState.In) {
			effectTime = duration - effectTime;
		}
		
		progress = SMTransitionUtils.SmoothProgress(0, duration, effectTime);
		
		return elapsedTime < duration;
	}
	
	public void OnGUI() {
		GUI.depth = 0;
		Color c = GUI.color;
		GUI.color = new Color(1, 1, 1, progress);
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), overlayTexture);
		GUI.color = c;
	}	
}
