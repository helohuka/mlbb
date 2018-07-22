using UnityEngine;
using System.Collections;

public class CaptureScreenShot : MonoBehaviour {

	[HideInInspector]
	public bool CaptureScreen = false;
	
	void OnPostRender()
	{
		if (CaptureScreen)
		{
			//Vector3 Temp = gameObject.transform.position;
			int iScreenW = Screen.width;
			int iScreenH = Screen.height;
			float scale = (float)iScreenH/iScreenW;
			float fDefScale = 1.5f; //(float)960/640;

			float [] array = new float[5];

			if(scale >= fDefScale)
			{

				//rGameRect.width
				array[1] = iScreenW;
				//rGameRect.height
				array[2]= fDefScale * iScreenW;

				//rGameRect.left
				array[3]= 0f;
				//rGameRect.top
				array[4]= (iScreenH - array[2])/2; //rGameRect.height)/2;
			}
			else{
				//rGameRect.height
				array[2]= iScreenH;
				//rGameRect.width
				array[1]= iScreenH / fDefScale;

				//rGameRect.top
				array[4]= 0f;
				//rGameRect.left
				array[3]= (iScreenW - array[1])/2; //rGameRect.width)/2;
			}
			
//			Rect rGameRect = new Rect (array[3], array[4], array[1], array[2]);

			Rect rect = new Rect (0f, 0f, Screen.width, Screen.height);
			Texture2D screenShot = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
			screenShot.ReadPixels (rect, 0, 0);
//			Texture2D screenShot = new Texture2D ((int)rGameRect.width, (int)rGameRect.height, TextureFormat.RGB24, false);
//			screenShot.ReadPixels (rGameRect, 0, 0);
			screenShot.Apply ();
			CaptureScreen = false;
			StageMgr.tex = screenShot;
			if(PrebattleEvent.getInstance.DoCaptureScreen != null)
			{
				PrebattleEvent.getInstance.DoCaptureScreen(screenShot);
			}
			//doscenein.Instance.snap_shot = screenShot;
			//GlobleScriptMgr.GetInstance().BackGround = screenShot;

		}
	}
}
