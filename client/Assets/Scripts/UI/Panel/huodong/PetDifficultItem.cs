using UnityEngine;
using System.Collections;

public class PetDifficultItem : MonoBehaviour {

    public delegate void SelectDifficultHandler(int diff);
    public event SelectDifficultHandler OnSelectDifficult;

    public GameObject mask_;

    public UILabel levelLmt_;

    public UISprite bg_;

    int battleID_;

    int diff_;

	// Use this for initialization
	void Start () {
	
	}

    public void SetData(int diff, int battleId, int level)
    {
        diff_ = diff;
        battleID_ = battleId;
        bg_.spriteName = DiffConvert(diff);
        levelLmt_.text = string.Format("[b]{0}级开启[-]", level);
        mask_.SetActive(GamePlayer.Instance.GetIprop(PropertyType.PT_Level) < level);
    }

    string DiffConvert(int diff)
    {
        switch (diff)
        {
            case 0:
                return "yiji";
            case 1:
                return "erji";
            case 2:
                return "sanji";
            case 3:
                return "siji";
            default:
                return "";
        }
    }
	


    void OnClick()
    {
        if (TeamSystem.RealTeamCount() < 3)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("arenapvpnum"), PopText.WarningType.WT_Warning);
            return;
        }

        bool isOpen = ActivitySystem.Instance.GetInfoState(4) == ActivitySystem.ActivityInfo.ActivityState.AS_Open;
        if(!isOpen)
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("EN_ActivityNoTime"), PopText.WarningType.WT_Warning);
            return;
        }

        NetConnection.Instance.enterBattle(battleID_);
        if (OnSelectDifficult != null)
            OnSelectDifficult(diff_);
    }
}
