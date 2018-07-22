//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;

/// <summary>
/// This enum contains the available transition states 
/// </summary>
public enum SMTransitionState {
	/// <summary>
	/// fade into the scene
	/// </summary>
	In,
	/// <summary>
	/// fade out of the scene
	/// </summary>
	Out,
	/// <summary>
	/// between out and in.
	/// </summary>
	Hold,
#if !UNITY_3_5
	/// <summary>
	/// before doing the transition while prefetching a level. This feature is only available in Unity 4.
	/// </summary>
	Prefetch
#endif
}

