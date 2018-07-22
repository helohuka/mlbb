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
/// A transition that just loads the new level without any visual effect.
/// </summary>
[AddComponentMenu("Scripts/SceneManager/Plain Transition")]
public class SMPlainTransition : SMTransition {
		
	protected override IEnumerator DoTransition() {
		yield return 0; // to make the compiler happy
		Application.LoadLevel(screenId);
	}
	
	protected override bool Process(float elapsedTime) {
		return false;
	}
}
