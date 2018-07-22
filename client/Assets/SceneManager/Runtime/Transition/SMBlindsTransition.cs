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
/// Transition implementation that shows horizontal blinds.
/// </summary>
[AddComponentMenu("Scripts/SceneManager/Blinds Transition")]
public class SMBlindsTransition : SMTransition {
	
	/// <summary>
	/// The duration of the effect.
	/// </summary>
	public float duration = 0.2f;
	
	/// <summary>
	/// The time to flip a single piece
	/// </summary>
	public float blindsFlipTime = 0.05f;
	
	/// <summary>
	/// The height of the blinds in pixels (if geater than 1) or relative to the screen (otherwise).
	/// </summary>
	public float preferredBlindsHeight = .2f;
	
	/// <summary>
	/// The blinds texture.
	/// </summary>
	public Texture blindsTexture;
	
	private float effectTime;
	private float blindsStartOffset;
	private int numberOfBlinds;
	private float actualBlindsHeight;			
	
	void Awake() {
		if (blindsTexture == null) {
			Debug.LogError("Blinds texture is missing");
		}
	}
	
	protected override void Prepare () {
        //int preferredHeightInPixel = SMTransitionUtils.ToAbsoluteSize(preferredBlindsHeight, Screen.height);
        numberOfBlinds = 3;//Mathf.FloorToInt(Screen.height / preferredHeightInPixel);
		actualBlindsHeight = (float)Screen.height / (float)numberOfBlinds;
		blindsStartOffset = (duration - blindsFlipTime) / (float)numberOfBlinds;	
	}
	
	protected override bool Process(float elapsedTime) {
		effectTime = elapsedTime;
		// invert direction if necessary
		if (state == SMTransitionState.In) {
			effectTime = duration - effectTime;
		}
				
		return elapsedTime < duration;
	}
	
	public void OnGUI() {
		GUI.depth = 0;
		for(int i = 0; i < numberOfBlinds; i++) {		
			float progress = SMTransitionUtils.SmoothProgress(i * blindsStartOffset, blindsFlipTime, effectTime);
			float visibleHeight = actualBlindsHeight * progress;
			GUI.DrawTexture(new Rect(0, i * actualBlindsHeight + (actualBlindsHeight - visibleHeight) / 2f, Screen.width, visibleHeight), blindsTexture);
		}
	}
	
}


