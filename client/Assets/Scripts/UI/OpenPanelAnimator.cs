using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public delegate void OpenPanelAnimatorFinished();

public class OpenPanelAnimator {
	private static OpenPanelAnimator _instance;
	public static OpenPanelAnimator instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = new OpenPanelAnimator();
			}
			return _instance;
		}
	}
	
	public static void PlayOpenAnimation(UIPanel control, OpenPanelAnimatorFinished callback=null)
	{
        if (callback != null)
            callback();
        //if(UIFactory.Instance.openPanelAnimCurve == null )
        //{
        //    return;
        //}
        //OpenPanelAnimator animator = OpenPanelAnimator.instance;
        //control.StartCoroutine(animator.CoroutineUpdate(control, true, callback));
	}
	
	public static void CloseOpenAnimation(UIPanel control, OpenPanelAnimatorFinished callback=null)
	{
        if (callback != null)
            callback();
        //Transform txObj = control.gameObject.transform.FindChild ("New Game Object");
        //if(txObj != null)
        //{
        //    txObj.gameObject.SetActive(false);
        //    //Destroy(txObj.gameObject);
        //}	
        //if(UIFactory.Instance.openPanelAnimCurve == null )
        //{
        //    return;
        //}
        //OpenPanelAnimator animator = OpenPanelAnimator.instance;
        //control.StartCoroutine(animator.CoroutineUpdate(control, false, callback));
	}

	private OpenPanelAnimator()
	{
	}
	
	private IEnumerator CoroutineUpdate(UIPanel control, bool isOpening, OpenPanelAnimatorFinished callback)
	{
		AnimationCurve animCurve = null;
		UIPanel target = control;

		if (target == null)
			yield return false;

		if(isOpening)
		{
			animCurve = UIFactory.Instance.openPanelAnimCurve;
		}
		else
		{
			animCurve = UIFactory.Instance.closePanelAnimCurve;
		}
		
		float _OpenAnimStartTime = Time.time;
		// update scale.
		float elapsedTime = Time.time - _OpenAnimStartTime;
		Keyframe maxTime = animCurve[animCurve.length-1];
		while(elapsedTime < maxTime.time)
		{
			if (target == null)
				yield return false;
            if (target.gameObject.transform.parent != null)
            {
                float curScale = animCurve.Evaluate(elapsedTime);
                target.transform.localScale = new Vector3(curScale, curScale, 1.0f);

                yield return null;
                elapsedTime = Time.time - _OpenAnimStartTime;
            }
            else
            {
                break;
            }
		}
		
		// assure set last frame..
		if(isOpening && target.gameObject.transform.parent != null)
		{
			target.gameObject.transform.localScale = Vector3.one;
		}
		
		if(callback != null)
		{
			callback();
		}
	}
}






