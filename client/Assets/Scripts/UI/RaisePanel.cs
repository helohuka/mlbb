using UnityEngine;
using System.Collections;

public class RaisePanel : MonoBehaviour {

    public Transform outsideRoot_;

    public UIGrid gridRoot_;

    public UIButton[] mainBtn_;
    public UIButton[] closeBtn_;

    bool isActive_;

    public enum RaiseType
    {
        RT_RaiseBaby,
        RT_RaisePlayer,
        RT_RaiseSkill,
        RT_RaiseEquip,
        RT_PartnerCollect,
        RT_PartnerBattle,
        RT_PartnerRaise,
        RT_PartnerEquip,
        RT_Max,
    }

	// Use this for initialization
	void Start () {
        for (int i = 0; i < mainBtn_.Length; ++i)
        {
            if (mainBtn_[i].transform.parent.Equals(outsideRoot_))
                mainBtn_[i].gameObject.SetActive(false);
            else
                mainBtn_[i].gameObject.SetActive(true);
        }
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc == null)
            bc = gameObject.AddComponent<BoxCollider>();
        bc.size = new Vector3(ApplicationEntry.Instance.UIWidth, ApplicationEntry.Instance.UIHeight, 0f);
	}

	public void Show()
	{
		this.gameObject.SetActive(true);
		UpdateData ();
	}
    public void UpdateData()
    {
        for (int i = 0; i < RaiseUpSystem.warningDic.Length; ++i)
        {
            if (RaiseUpSystem.warningDic[i] == true)
            {
                UIManager.SetButtonEventHandler(mainBtn_[(int)i].gameObject, EnumButtonEvent.OnClick, OnMainClick, (int)i, 0);
                UIManager.SetButtonEventHandler(closeBtn_[(int)i].gameObject, EnumButtonEvent.OnClick, OnCloseClick, (int)i, 0);
                gridRoot_.AddChild(mainBtn_[(int)i].transform);
				mainBtn_[(int)i].gameObject.SetActive(true);
            }
            else
            {
                mainBtn_[i].gameObject.SetActive(false);
                UIManager.RemoveButtonEventHandler(mainBtn_[(int)i].gameObject, EnumButtonEvent.OnClick);
                UIManager.RemoveButtonEventHandler(closeBtn_[(int)i].gameObject, EnumButtonEvent.OnClick);
                mainBtn_[i].transform.parent = outsideRoot_;
            }
        }
		gridRoot_.Reposition ();
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(RaiseUpSystem.Instance.HasItem);
            GlobalInstanceFunction.Instance.Invoke(() => {
                if(isActive_)
                    gridRoot_.Reposition(); 
            }, 2);
        }
    }

    void OnEnable()
    {
        isActive_ = true;
        gridRoot_.Reposition();
    }

    void OnMainClick(ButtonScript obj, object args, int param1, int param2)
    {
		this.gameObject.SetActive(false);
        switch((RaiseType)param1)
        {
            case RaiseType.RT_RaiseBaby:
                MainbabyUI.SwithShowMe();
                break;
            case RaiseType.RT_RaisePlayer:
                PlayerPropertyUI.SwithShowMe();
                break;
            case RaiseType.RT_RaiseSkill:
			    MainbabySkillNpc.SwithShowMe();             
                break;
            case RaiseType.RT_RaiseEquip:
                CompoundUI.SwithShowMe(true);
                break;
            case RaiseType.RT_PartnerCollect:
                EmployessControlUI.SwithShowMe();
                break;
            case RaiseType.RT_PartnerBattle:
                EmployessControlUI.SwithShowMe(2);
                break;
            case RaiseType.RT_PartnerRaise:
                EmployessControlUI.SwithShowMe(3);
                break;
            case RaiseType.RT_PartnerEquip:
                EmployessControlUI.SwithShowMe(3);
                break;
            default:
                break;
        }
    }

    void OnCloseClick(ButtonScript obj, object args, int param1, int param2)
    {
        UIManager.RemoveButtonEventHandler(mainBtn_[(int)param1].gameObject, EnumButtonEvent.OnClick);
        UIManager.RemoveButtonEventHandler(closeBtn_[(int)param1].gameObject, EnumButtonEvent.OnClick);
        mainBtn_[param1].transform.parent = outsideRoot_;
        mainBtn_[param1].gameObject.SetActive(false);
        gridRoot_.Reposition();
        RaiseUpSystem.ResetRaise((RaiseType)param1);
    }
	


    void OnDisable()
    {
        isActive_ = false;
    }

    void OnClick()
    {
        gameObject.SetActive(false);
    }
}
