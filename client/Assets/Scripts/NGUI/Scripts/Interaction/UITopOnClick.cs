using UnityEngine;
using System.Collections;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Click")]
public class UITopOnClick : MonoBehaviour
{
	void OnClick ()
	{
        UIScrollView scrollView = NGUITools.FindInParents<UIScrollView>(gameObject);

        if (scrollView != null && scrollView.panel != null)
		{
            Vector3[] corners = scrollView.panel.worldCorners;
            Vector3 panelCenter = (corners[1] + corners[2]) * 0.5f;

            Transform panelTrans = scrollView.panel.cachedTransform;

            // Figure out the difference between the chosen child and the panel's center in local coordinates
            Vector3 cp = panelTrans.InverseTransformPoint(transform.position);
            Vector3 cc = panelTrans.InverseTransformPoint(panelCenter);
            Vector3 localOffset = cp - cc;

            // Offset shouldn't occur if blocked
            if (!scrollView.canMoveHorizontally) localOffset.x = 0f;
            if (!scrollView.canMoveVertically) localOffset.y = 0f;
            localOffset.z = 0f;

            // Spring the panel to this calculated position
            SpringPanel.Begin(scrollView.panel.cachedGameObject,
                panelTrans.localPosition - localOffset, 8f).onFinished = null;
		}
	}
}


