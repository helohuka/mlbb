using UnityEngine;
using System.Collections;

public class GetInfoOnClick : MonoBehaviour {

    public delegate void ClickInformationHandler(string info, int param = 0);
    public static event ClickInformationHandler OnClickInformation;

    public int param_;

    void OnClick()
    {
		Transform lbl = transform.FindChild ("Task2Label");
		UILabel lal = null;
		if(lbl != null)
		{
			lal = lbl.GetComponent<UILabel>();
		}
        else
        {
            lal = gameObject.GetComponent<UILabel>();
        }

		if (lal != null)
        {
			string info = lal.GetInfoAtPosition(UICamera.lastHit.point);
            if (!string.IsNullOrEmpty(info))
            {
				int sceneid = 0;
				GlobalValue.Get(Constant.C_FamilyBattleScene, out sceneid);
				if(sceneid == GameManager.SceneID)
				{
					MessageBoxUI.ShowMe(LanguageManager.instance.GetValue("likaijiazuzhan"),()=>{
						if (OnClickInformation != null)
							OnClickInformation(info, param_);
					});
				}
				else
				{
					if (OnClickInformation != null)
						OnClickInformation(info, param_);
				}
					

            }
        }
    }
}
