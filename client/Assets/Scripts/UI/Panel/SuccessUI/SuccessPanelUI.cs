using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SuccessPanelUI : UIBase
{
    public UIButton CloseBtn;
    public GameObject objInfo;
    public GameObject item;
    public UIGrid grid;
    public UILabel _SuccessTitleLable;
    public UILabel _SuccessJDLable;
    public UILabel _SuccessLQLable;
    public UIButton allBtn;
    public UIButton growingUpbtn;
    public UIButton fightBtn;
    public UIButton babyBtn;
    public UIButton frBtn;
    public UIButton tujianBtn;
    public int index;
    public List<UIButton> btns = new List<UIButton>();

    List<GameObject> objPool = new List<GameObject>();
    public UIPanel contentPanel;

    void Start()
    {
        btns.Add(allBtn);
        btns.Add(growingUpbtn);
        btns.Add(fightBtn);
        btns.Add(babyBtn);
        btns.Add(frBtn);
        btns.Add(tujianBtn);

        item.SetActive(false);
        _SuccessTitleLable.text = LanguageManager.instance.GetValue("Success_Title");
        _SuccessJDLable.text = LanguageManager.instance.GetValue("Success_SuccessJD");
        _SuccessLQLable.text = LanguageManager.instance.GetValue("Success_SuccessLQ");
        UIManager.SetButtonEventHandler(CloseBtn.gameObject, EnumButtonEvent.OnClick, OnClickClose, 0, 0);

        for (int i = 0; i < btns.Count; i++)
        {
            UIManager.SetButtonEventHandler(btns[i].gameObject, EnumButtonEvent.OnClick, OnClickbtn, i, 0);
        }
        GamePlayer.Instance.OnAchievementUpdate += Receive;
        OpenPanelAnimator.PlayOpenAnimation(this.panel, () =>
        {
            ButtonSelect(0);
            UpdateUI();
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_AchievementUIOpen);
        });
    }
    void OnClickbtn(ButtonScript obj, object args, int param1, int param2)
    {
        ButtonSelect(param1);
        index = param1;
        UpdateUI();
    }

    void ButtonSelect(int index)
    {
        for (int i = 0; i < btns.Count; i++)
        {
            if (index == i)
            {
                btns[i].isEnabled = false;
            }
            else
            {
                btns[i].isEnabled = true;
            }
        }
    }
    void Receive()
    {
        curCell.StateSp.gameObject.SetActive(true);
    }
    SuccessCell curCell;
    void OnClickReceive(ButtonScript obj, object args, int param1, int param2)
    {
        GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_AchievementReceived);
        if (SuccessSystem.isReceived(param1))
        {
            PopText.Instance.Show(LanguageManager.instance.GetValue("chengjiuWeiwangcheng"), PopText.WarningType.WT_Warning);
        }
        else
        {
            if (!BagSystem.instance.BagIsFull())
            {
                SuccessCell aCell = obj.GetComponentInParent<SuccessCell>();
                NetConnection.Instance.requestAchaward(param1);
                curCell = aCell;
            }
            else
            {
                PopText.Instance.Show(LanguageManager.instance.GetValue("EN_BagFull"));
            }
        }
    }

    void UpdateUI()
    {
        CategoryType[] cTypes = new CategoryType[(int)CategoryType.ACH_Max];
        if (index == (int)CategoryType.ACH_All)
        {
            for (int i = 0; i < (int)CategoryType.ACH_Max; ++i)
            {
                cTypes[i] = (CategoryType)i;
            }
        }
        else
        {
            cTypes[index] = (CategoryType)index;
        }

        int counter = 0;
        GameObject go = null;
        Dictionary<AchievementType, List<AchievementContent>> data = null;
        List<AchievementContent> finalList = new List<AchievementContent>();
        for (int i = 0; i < cTypes.Length; ++i)
        {
            if (!SuccessSystem.achievementByTab.ContainsKey(cTypes[i]))
                continue;

            data = SuccessSystem.achievementByTab[cTypes[i]];
            foreach (List<AchievementContent> seris in data.Values)
            {
                AchievementContent ac = null;
                for (int j = 0; j < seris.Count; ++j)
                {
                    if (!seris[j].isAward_)
                    {
                        ac = seris[j];
                        break;
                    }
                }

                if(ac == null)
                    ac = seris[seris.Count - 1];

                if (AchieveData.GetData(ac.data_._Id)._AchieveType == AchievementType.AT_Reward50)
                    continue;

                finalList.Add(ac);
            }
        }

        finalList.Sort(Compare);
        for (int i = 0; i < finalList.Count; ++i)
        {
            if (counter >= objPool.Count)
            {
                go = GameObject.Instantiate(item) as GameObject;
                objPool.Add(go);
            }
            else
            {
                go = objPool[counter];
            }
            go.SetActive(true);
            SuccessCell aCell = go.GetComponent<SuccessCell>();
            aCell.Info = finalList[i];

            UIManager.SetButtonEventHandler(aCell.receiveBtn.gameObject, EnumButtonEvent.OnClick, OnClickReceive, finalList[i].data_._Id, 0);
            go.transform.parent = grid.transform;
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;

            if(i == 0)
                GuideManager.Instance.RegistGuideAim(aCell.receiveBtn.gameObject, GuideAimType.GAT_FirstAchievement);

            counter++;
        }

        for (int i = counter; i < objPool.Count; ++i)
        {
            objPool[i].SetActive(false);
        }

        SpringPanel sp = contentPanel.GetComponent<SpringPanel>();
        if (sp != null)
            sp.enabled = false;
        contentPanel.clipOffset = Vector2.zero;
        contentPanel.transform.localPosition = Vector3.zero;
        contentPanel.GetComponent<UIScrollView>().ResetPosition();
        grid.Reposition();

        objInfo.GetComponent<RewardInfo>().Percentage(SuccessSystem.FinishCount, SuccessSystem.Reward50(), AchieveData.metaData.Count - 1); // remove reward50

        SuccessSystem.isDirty = false;
    }

    void Update()
    {
        if (SuccessSystem.isDirty)
        {
            UpdateUI();
            //SuccessSystem.isDirty = false;
        }
    }

    int Compare(AchievementContent ac1, AchievementContent ac2)
    {
        int a1score = 0;
        int a2score = 0;
        if (ac1.isAch_ && !ac1.isAward_)
            a1score = 1000;
        else if (!ac1.isAch_)
            a1score = 500;

        if (ac2.isAch_ && !ac2.isAward_)
            a2score = 1000;
        else if (!ac2.isAch_)
            a2score = 500;

        if (a1score > a2score) return -1;
        if (a1score < a2score) return 1;
        return 0;
    }

    void OnClickClose(ButtonScript obj, object args, int param1, int param2)
    {

        OpenPanelAnimator.CloseOpenAnimation(this.panel, () =>
        {
            Hide();
        });
    }

    public static void ShowMe()
    {
        UIBase.AsyncLoad(UIASSETS_ID.UIASSETS_SuccessPanelUI);
    }
    public static void SwithShowMe()
    {
        UIBase.SwitchShowPanelByName(UIASSETS_ID.UIASSETS_SuccessPanelUI);
    }
    public override void Destroyobj()
    {
    }
}
