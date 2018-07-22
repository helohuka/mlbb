using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UICenterOnIndex : MonoBehaviour {
	

	void Start () {



	}
	public void UpdateItemPosition(int index)
	{
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; ++i)
        {
            children[i] = transform.GetChild(i);
        }
        if (children.Length == 0)
            return;

		UICenterOnChild center = NGUITools.FindInParents<UICenterOnChild>(gameObject);
		UIPanel panel = NGUITools.FindInParents<UIPanel>(gameObject);
        if (panel == null)
            panel = gameObject.GetComponent<UIPanel>();
		
		if (center != null)
		{
			if (center.enabled)
                center.CenterOn(children[index]);
		}
		else if (panel != null && panel.clipping != UIDrawCall.Clipping.None)
		{
			UIScrollView sv = panel.GetComponent<UIScrollView>();
            Vector3 offset = -panel.cachedTransform.InverseTransformPoint(children[index].position);
			if (!sv.canMoveHorizontally) offset.x = panel.cachedTransform.localPosition.x;
			if (!sv.canMoveVertically) offset.y = panel.cachedTransform.localPosition.y;
			SpringPanel.Begin(panel.cachedGameObject, offset, 6f);
		}
	}

}
