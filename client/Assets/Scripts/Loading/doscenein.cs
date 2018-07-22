using UnityEngine;
using System.Collections;
using System.IO;
public enum EffectType
{
	BYCOn,
	BYCOff,
	Normal
}
public class doscenein : MonoBehaviour {



	public static doscenein doscene;   

	public  EffectType type;
  	private float in_time=1.0f;
	private float in_timer=0.0f;
	private string btnName;
	private Rect rect;
	private int w = 0;
	private int h = 0;
	private int GridNoY01 = 30;
	private	Texture2D snap_shot;

	public Texture2D SnapShot
	{
		set
		{
			snap_shot = value;
			w = snap_shot.width;
			h = snap_shot.height/GridNoY01;
		}
		get { return snap_shot; }
	}
	void Start () {
      in_timer=0.0f;
	  h = Screen.height / GridNoY01;
	  type = EffectType.Normal;
	}
	void Update () {
		in_timer+=Time.deltaTime;

	}	
	void DoBYCOff(float rate)
	{
		if (snap_shot == null)
			return;
		float hi = rate * h;
		for(int i = 0; i<GridNoY01;i++)
		{
			Color []c = snap_shot.GetPixels(0,i*h,w,h,0);
			Texture2D t = new Texture2D(w,h);
			t.SetPixels(0,0,w,h,c);
			t.Apply();	
			rect.width = snap_shot.width;
			rect.height = hi;
			rect.x = 0;
			rect.y = (((GridNoY01 - i)*h)+h)-(h+hi)*0.5f;
			GUI.DrawTexture (rect,t);
		}
	}
	void DoBYCOn(float rate)
	{
		if (snap_shot == null)
			return;
		float hi = h - rate * h;
		for(int i = 0; i<GridNoY01;i++)
		{
			Color []c = snap_shot.GetPixels(0,i*h,w,h,0);
			Texture2D t = new Texture2D(w,h);
			t.SetPixels(0,0,w,h,c);
			t.Apply();	
			rect.width = snap_shot.width;
			rect.height = hi;
			rect.x = 0;
			rect.y = (((GridNoY01 - i)*h)+h)-(h+hi)*0.5f;
			GUI.DrawTexture (rect,t);

		}

	}
	void OnGUI() {


		if (in_timer < in_time) {
			DoBYCOn (1.0f - in_timer / in_time);
		}
	}


//		if (type == EffectType.BYCOn) {
//				
//
//		} else if (type == EffectType.BYCOff) {
//				if (in_timer < in_time) {
//						DoBYCOff (1.0f - in_timer / in_time);
//				}
//
//		} 
//
//	}
	void Awake()
	{
		doscene = this;
	}
	public static doscenein Instance
	{
		get
		{
			return doscene;	
		}
	}
	public void DoTransitionEffect(Texture2D tex,EffectType stype)
	{
		SnapShot = tex;
		type = stype;
		in_timer=0.0f;
	}

}
