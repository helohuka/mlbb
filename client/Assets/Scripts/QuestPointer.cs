using UnityEngine;
using System.Collections;

public class QuestPointer : MonoBehaviour {

    GameObject aimObj_;

    GameObject arrowPf_;

    GameObject arrow_;

    GameObject pointerPf_;

    GameObject pointer_;

    QuestSystem.PointInfo info_;

    Vector3 destLocation_;

    const float MinDistance_ = 2f;

    float rotateSpeed = 10f;

    bool openPointer_;

	// Use this for initialization
	void Start () {
        //arrowPf_ = (GameObject)Resources.Load<GameObject>("questArrow");
        //pointerPf_ = (GameObject)Resources.Load<GameObject>("scene_zhiyin");
        //StageMgr.OnSceneLoaded += SceneLoaded;
        //QuestSystem.OnQuestUpdate += RefreshInfo;
        //Prebattle.Instance.OnAllReady += RefreshInfo;
        //Prebattle.Instance.OnNpcLoaded += NpcLoaded;
    }

    void SceneLoaded(string sceneName)
    {
        RefreshInfo();
    }

    void NpcLoaded(int id)
    {
        if (QuestSystem.IsPointNpc(id))
            RefreshInfo();
    }

    void RefreshInfo()
    {
        aimObj_ = GameObject.FindGameObjectWithTag("Master");
        info_ = QuestSystem.GetPointInfo();
        openPointer_ = (aimObj_ != null && info_ != null && info_.sceneId_ == GameManager.SceneID);

        if (openPointer_)
        {
            destLocation_ = new Vector3(info_.location_.x, aimObj_.transform.position.y, info_.location_.y);
            if (arrow_ != null)
            {
                Destroy(arrow_);
            }

            if (pointer_ != null)
            {
                Destroy(pointer_);
            }

            arrow_ = (GameObject)GameObject.Instantiate(arrowPf_) as GameObject;
            pointer_ = (GameObject)GameObject.Instantiate(pointerPf_) as GameObject;

            pointer_.transform.localPosition = destLocation_;
            
            arrow_.transform.localRotation = Quaternion.identity;
            arrow_.transform.localScale = Vector3.one;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (openPointer_)
        {
            try
            {
                if (arrow_ == null)
                {
                    openPointer_ = false;
                    return;
                }
                if (Vector3.Distance(aimObj_.transform.position, destLocation_) > MinDistance_)
                {
                    if (arrow_.activeSelf == false)
                    {
                        arrow_.SetActive(true);
                        if (pointer_ != null)
                        {
                            pointer_.SetActive(true);
                        }
                    }
                    arrow_.transform.localPosition = aimObj_.transform.position;
                }
                else
                {
                    if (arrow_.activeSelf == true)
                    {
                        arrow_.SetActive(false);
                        if (pointer_ != null)
                            pointer_.SetActive(false);
                    }
                }
                arrow_.transform.LookAt(destLocation_);
            }
            catch (MissingReferenceException mre)
            {
                openPointer_ = false;
            }
        }
	}

    void OnDestroy()
    {
//        StageMgr.OnSceneLoaded -= SceneLoaded;
        QuestSystem.OnQuestUpdate -= RefreshInfo;
        Prebattle.Instance.OnAllReady -= RefreshInfo;
        Prebattle.Instance.OnNpcLoaded -= NpcLoaded;
    }

    void OnDrawGizmos()
    {
        if (openPointer_)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(aimObj_.transform.position, destLocation_);
        }
    }
}
