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
/// The base class for all level transitions.
/// </summary>
public abstract class SMTransition : MonoBehaviour {
	
	protected SMTransitionState state = SMTransitionState.Out;
	
	/// <summary>
	/// Gets the current state of the transition. This is read-only, as the state is controlled by the transition 
	/// framework.
	/// </summary>
	/// <remarks>@since version 1.4.5</remarks>
	public SMTransitionState CurrentTransitionState {
		get {
			return state;
		}
	}

	public bool timeScaleIndependent = true;
	public bool loadAsync = false;
#if !UNITY_3_5   
	// Prefetching needs Unity 4 therefore we hide it in Unity 3.5
	public bool prefetchLevel = false;
#endif
	/// <summary>
	/// The id of the screen that is being loaded.
	/// </summary>
	[HideInInspector]
	public string screenId;
	
	void Start() {
		if (Time.timeScale <= 0f && !timeScaleIndependent) { 
			Debug.LogWarning("Time.timeScale is set to 0 and you have not enabled 'Time Scale Independent' at the transition prefab. " +
			                 "Therefore the transition animation would never start to play. Please either check the 'Time Scale Independent' checkbox" +
			                 "at the transition prefab or set Time.timeScale to a positive value before changing the level.", this);
			return; // do not do anything in this case.
		}

#if !UNITY_3_5   
		if (prefetchLevel && !loadAsync) {
			Debug.LogWarning("You can only prefetch the level when using asynchronous loading. " +
				"Please either uncheck the 'Prefetch Level' checkbox on your level transition prefab or check the " +
				"'Load Async' checkbox. Note, that asynchronous loading (and therefore level prefetching) requires a Unity Pro license.", this);
			return; // don't do anything in this case.
		}
#endif
		StartCoroutine(DoTransition());
	}

	/// <summary>
	/// This method actually does the transition. It is run in a coroutine and therefore needs to do
	/// yield returns to play an animation or do another progress over time. When this method returns
	/// the transition is expected to be finished.
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/> for showing the transition status. Use yield return statements to keep
	/// the transition running, otherwise simply end the method to stop the transition.
	/// </returns>
	protected virtual IEnumerator DoTransition() {
		// make sure the transition doesn't get lost when switching the level.
		DontDestroyOnLoad(gameObject);
#if !UNITY_3_5
        AsyncOperation asyncOperation = null;
		if (prefetchLevel) {
			state = SMTransitionState.Prefetch;
			SendMessage("SMBeforeTransitionPrefetch", this, SendMessageOptions.DontRequireReceiver);
			yield return 0; // wait one frame
			SendMessage("SMOnTransitionPrefetch", this, SendMessageOptions.DontRequireReceiver);
			
			var loadLevel = DoLoadLevel();
			while (loadLevel.MoveNext()) {
				var current = loadLevel.Current;
				if (asyncOperation == null) { // try to find the actual level loading operation
					asyncOperation = current as AsyncOperation;
					if (asyncOperation != null) {
						// hold on level switch until the transition is in hold state.
						asyncOperation.allowSceneActivation = false;
					}
				}
				yield return 0;
			}
		}
#endif
		state = SMTransitionState.Out;
		SendMessage("SMBeforeTransitionOut", this, SendMessageOptions.DontRequireReceiver);
		Prepare();
		SendMessage("SMOnTransitionOut", this, SendMessageOptions.DontRequireReceiver);
		float time = 0;
		
		while(Process(time)) {
			time += DeltaTime * 2f;
			// wait for the next frame
			yield return 0;
		}
		
		SendMessage("SMAfterTransitionOut", this, SendMessageOptions.DontRequireReceiver);
		// wait another frame...
		yield return 0;

        StageMgr.ExcuteBeginLoadEvent();
        Resources.UnloadUnusedAssets();

		state = SMTransitionState.Hold;
		SendMessage("SMBeforeTransitionHold", this, SendMessageOptions.DontRequireReceiver);
		SendMessage("SMOnTransitionHold", this, SendMessageOptions.DontRequireReceiver);

		// wait another frame...
		yield return 0;

#if !UNITY_3_5
		if (!prefetchLevel) {
#endif
		// level is not prefetched, load it right now.
		var loadLevel = DoLoadLevel();
		while (loadLevel.MoveNext()) {
			yield return loadLevel.Current;
		}
#if !UNITY_3_5
		}
		else {
			if (asyncOperation != null) { // this should always be true, but you never know...
				// level was prefetched, so activate level switch now.
				asyncOperation.allowSceneActivation = true;
                ScenePreloader.Instance.AllowEnter();
			}
		}
#endif
        while(true)
        {
            if (StageMgr.Scene_name.Equals(Application.loadedLevelName))
                break;
            yield return 0;
        }

        System.GC.Collect();

        StageMgr.ExcuteLoadedEvent();
		SendMessage("SMAfterTransitionHold", this, SendMessageOptions.DontRequireReceiver);
		// wait another frame...
		yield return 0;

		//LoadingScene.Instance.EnterScene(asyncOperation);
		state = SMTransitionState.In;
		SendMessage("SMBeforeTransitionIn", this, SendMessageOptions.DontRequireReceiver);
		Prepare();
		SendMessage("SMOnTransitionIn", this, SendMessageOptions.DontRequireReceiver);
		time = 0;
        
        while(Process(time)) {
			time += DeltaTime;
			// wait for the next frame
			yield return 0;
		}

		SendMessage("SMAfterTransitionIn", this, SendMessageOptions.DontRequireReceiver);
		// wait another frame...
		yield return 0;

		Destroy(gameObject);

        StageMgr.ExcuteAllLoadedEvent();
	}
	
	/// <summary>
	/// invoked during the <see cref="SMTransitionState.Hold"/> state to load the next scene. 
	/// Override this method to interrupt the transition before or after loading the scene. 
	/// Make sure to call <code>yield return base.LoadLevel()</code> in your implementation.
	/// </summary>
	/// <returns>
	/// A <see cref="IEnumerator"/> 
	/// </returns>
	protected virtual IEnumerator DoLoadLevel() {
		yield return LoadLevel();
	}
	
	/// <summary>
	/// Starts the actual load operation
	/// </summary>
	/// <returns>
	/// The load operation or <code>null</code>
	/// </returns>
	protected virtual YieldInstruction LoadLevel() {
		if (loadAsync) {
            if (ScenePreloader.Instance.isPreLoading)
                return ScenePreloader.Instance.async_;
			return Application.LoadLevelAsync(screenId);
		} else {
			Application.LoadLevel(screenId);
			return null;
		}
	}
	
	/// <summary>
	/// invoked at the start of the <see cref="SMTransitionState.In"/> and <see cref="SMTransitionState.Out"/> state to 
	/// initialize the transition
	/// </summary>
	protected virtual void Prepare() {
	}
	
	/// <summary>
	/// Invoked once per frame while the transition is in state <see cref="SMTransitionState.In"/> or <see cref="SMTransitionState.Out"/> 
	/// to calculate the progress
	/// </summary>
	/// <param name='elapsedTime'>
	/// the time that has elapsed since the start of current transition state in seconds. 
	/// </param>
	/// <returns>
	/// false if no further calls are necessary for the current state, true otherwise
	/// </returns>
	protected abstract bool Process(float elapsedTime);

	/// <summary>
	/// Gets the delta time according to the settings. If realTimeScaling is enabled, the time scale will not affect
	/// the speed at which the transition is playing. Otherwise a change to the time scale will affect the speed
	/// at which transitions are played.
	/// </summary>
	/// <value>The delta time.</value>
	protected virtual float DeltaTime {
		get {
			if (timeScaleIndependent) {
				return SMRealTimeHelper.deltaTime;
			}
			else {
				return Time.deltaTime;
			}
		}
	}
}
