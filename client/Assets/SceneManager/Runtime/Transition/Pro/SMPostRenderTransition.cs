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
/// base class for transitions running in the post render phase
/// </summary>
public abstract class SMPostRenderTransition : SMTransition {
	
	/// <summary>
	/// material used between the fade out and fade in effect
	/// </summary>
	public Material holdMaterial;
		
	private Camera tempCamera;
	private bool reentrantLock = false;
		
	protected virtual void Awake() {
		if (holdMaterial == null) {
			Debug.LogError("'Hold' material is missing");
		}
		
		tempCamera = gameObject.AddComponent<Camera>();
		tempCamera.cullingMask = 0;
		tempCamera.renderingPath = RenderingPath.Forward;
		tempCamera.depth = Mathf.Floor(float.MaxValue);
		tempCamera.clearFlags = CameraClearFlags.Depth;
	}
	
	void OnPostRender() {
		// just to be sure the coroutine is started only once each frame
		if (reentrantLock) {
			return;
		}
		
		reentrantLock = true;
		StartCoroutine(ProcessFrame());
	}
	
	IEnumerator ProcessFrame() {
		yield return new WaitForEndOfFrame();
#if !UNITY_3_5
		if (state == SMTransitionState.Prefetch) {
			reentrantLock = false; // release locks in this case as well
			yield break; // no effects in prefetch state
		}
#endif
		if (state == SMTransitionState.Hold) {
			OnRenderHold();		
		} else {
			OnRender();		
		}
		reentrantLock = false;
	}
	
	protected virtual void OnRenderHold() {
		GL.PushMatrix();
		GL.LoadOrtho();
		GL.LoadIdentity();
		for (var i = 0; i < holdMaterial.passCount; ++i) {
			holdMaterial.SetPass(i);
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
	
	/// <summary>
	/// invoked at the end of each frame
	/// </summary>
	protected abstract void OnRender();
	
}

