using UnityEngine;
using System.Collections;

public class SkipButton : MonoBehaviour {

    bool onceFlag_ = false;
	// Use this for initialization
	void Start () {
	
	}
	

    void OnClick()
    {
        if (onceFlag_ == false)
            SwitchScene();
        onceFlag_ = true;
    }

    public void SwitchScene()
    {
        StageMgr.OnSceneLoaded += LoadedCallBack;
		StageMgr.LoadingAsyncScene(GlobalValue.StageName_MainScene, SwitchScenEffect.SMFadeTransition);
    }

    // 只有从开头动画进入主城 才有可能是第一次进入游戏
    void LoadedCallBack(string sceneName)
    {
        StageMgr.OnSceneLoaded -= LoadedCallBack;
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_EnterMainScene);
        if (GamePlayer.Instance.IsFirstLogin)
        {
            NpcRenwuUI.talkFinishCallBack_ = () =>
            {
                if (GamePlayer.Instance.BattleBaby != null)
                    GetBabyPanelUI.ShowMe(GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_TableId));
                else
                    GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_FirstEnterMainScene);
            };
            bool ret = GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_Talk_FirstEnterMainScene);
            if (!ret)
            {
                GetBabyPanelUI.ShowMe(GamePlayer.Instance.BattleBaby.GetIprop(PropertyType.PT_TableId));
                NpcRenwuUI.talkFinishCallBack_ = null;
            }
        }
    }
}
