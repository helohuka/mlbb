//
// Copyright (c) 2013 Ancient Light Studios
// All Rights Reserved
// 
// http://www.ancientlightstudios.com
//

using System;
using UnityEngine;

public class SMTransitionUtils {
	
	/// <summary>
	/// Clamps the time between start and duration and smooths the result
	/// </summary>
	/// <param name="startOffset">
	/// start of the effect
	/// </param>
	/// <param name="duration">
	/// duration of the effect
	/// </param>
	/// <param name="time">
	/// elapsed time of the effect
	/// </param>
	/// <returns>
	/// a value between 0 and 1
	/// </returns>
	public static float SmoothProgress(float startOffset, float duration, float time) {
		return Mathf.SmoothStep(0, 1, Progress(startOffset, duration, time));
	}
	
	/// <summary>
	/// Clamps the time between start and duration  
	/// </summary>
	/// <param name="startOffset">
	/// start of the effect
	/// </param>
	/// <param name="duration">
	/// duration of the effect
	/// </param>
	/// <param name="time">
	/// elapsed time of the effect
	/// </param>
	/// <returns>
	/// a value between 0 and 1 
	/// </returns>
	public static float Progress(float startOffset, float duration, float time) {
		return Mathf.Clamp(time - startOffset, 0, duration) / duration;
	}
	
	/// <summary>
	/// converts the given value into a pixel value
	/// </summary>
	/// <returns>
	/// The absolute size.
	/// </returns>
	/// <param name='value'>
	/// relative size if lesser than or equal 1. an absolute value otherwise
	/// </param>
	/// <param name='maxSize'>
	/// upper limit
	/// </param>
	public static int ToAbsoluteSize(float value, int maxSize) {
		if (value <= 1) {
			value = maxSize * value;
		}
		
		return (int) Mathf.Clamp(Mathf.Floor(value), 0, maxSize);
	}
}

