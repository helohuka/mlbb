using UnityEngine;  

[ExecuteInEditMode]    
[RequireComponent(typeof(UIRoot))]  
public class SZUIRootScale : MonoBehaviour  
{  
	public int aspectRatioHeight;  
	public int aspectRatioWidth;  
	public bool runOnlyOnce = false;  
	private UIRoot mRoot;  
	private bool mStarted = false;  

	void Awake()  
	{  
#if !UNITY_EDITOR
		runOnlyOnce = true;
#endif
		UICamera.onScreenResize += ScreenSizeChanged;  
	}  
	
	void OnDestroy()  
	{  
		UICamera.onScreenResize -= ScreenSizeChanged;  
	}  
	
	void Start()  
	{  
		mRoot = NGUITools.FindInParents<UIRoot>(this.gameObject);  
		
		mRoot.scalingStyle = UIRoot.Scaling.FixedSize;  
		
		this.Update();  
		mStarted = true;  
	}  
	
	void ScreenSizeChanged()  
	{   
		if (mStarted && runOnlyOnce) {  
			this.Update();  
		}   
	}  
	
	void Update()  
	{  
		float defaultAspectRatio = aspectRatioWidth * 1f / aspectRatioHeight;  
		float currentAspectRatio = Screen.width * 1f / Screen.height;  
		
		if (defaultAspectRatio > currentAspectRatio) {  
			int horizontalManualHeight = Mathf.FloorToInt(aspectRatioWidth / currentAspectRatio);  
			mRoot.manualHeight = horizontalManualHeight;  
		} else {  
			mRoot.manualHeight = aspectRatioHeight;  
		} 
		if (runOnlyOnce && Application.isPlaying) {  
			this.enabled = false;  
		}  
	}  
} 