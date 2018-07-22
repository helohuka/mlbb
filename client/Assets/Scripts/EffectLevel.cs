using UnityEngine;
using System.Collections;

public class EffectLevel : MonoBehaviour {

	//目标控件 
	public UIWidget target; 
	//记录上帧的RenderQueue 
	private int oldRQ; 
	void Update() 
	{ 
		AdjustRQ ();//这个必须放在Update/LateUpdate/FinxUpdate里。因为NGUI的drawl更新是在lateUpdate()里的。 
	} 
	public void AdjustRQ() 
	{ 
		if (target == null || target.drawCall == null) 
			return; 
		if (target.drawCall.renderQueue == oldRQ) 
			return; 
		Renderer[] rends = transform.GetComponentsInChildren<Renderer> (true); 
		if (rends == null)return; 
		for (int i=0; i<rends.Length; ++i) { 
			if(rends[i].material!=null) 
				rends[i].material.renderQueue=target.drawCall.renderQueue+1;//将renderqueue调整到目标控件之上 
		} 
		oldRQ = target.drawCall.renderQueue; 
	} 
}
