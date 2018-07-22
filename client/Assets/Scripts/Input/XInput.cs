using UnityEngine;
using System.Collections.Generic;

public class XInput
{

    static XInput inst_ = null;
    static public XInput Instance
    {
        get
        {
            if (inst_ == null)
                inst_ = new XInput();
            return inst_;
        }
    }

    public enum ActorType
    {
        AT_None,
        AT_Npc,
        AT_OtherPlayer,
        AT_Baby,
        AT_Ground,
    }

    public delegate void MouseDown(Vector2 origin);
    public delegate void MouseMove(float x, float y);
    public delegate void MouseUp();
    public delegate void TouchGround(Vector3 pos);
    public delegate void TouchActor(ActorType type, int instId, Vector3 actorPos);
    public delegate void TouchDown();
    public delegate void TouchMutiActor(Vector2 uipos, ClickObj[] actors, bool show);

    public event MouseDown OnMoveBegin;
    public event MouseMove OnMove;
    public event MouseUp OnMoveEnd;
    public event TouchGround OnTouchGround;
    public event TouchActor OnTouchActor;
    public event TouchDown OnTouchDown;
    public event TouchMutiActor OnTouchMutiActor;


    float axisx, axisy;
    Vector2 originMousePos_;
    Vector2 currentMousePos_;
    bool pushDown_;

    public bool dealInput = true;

    public void Update()
    {
        if (!dealInput)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if ((GuideManager.Instance.IsGuiding_ && GuideManager.Instance.crtGuideType_ == GuideManager.GuideType.GT_UI) ||
                !GuideManager.Instance.IsGuiding_ && UICamera.hoveredObject != null && !UICamera.hoveredObject.CompareTag("MutiActor"))
            {
                if (OnTouchMutiActor != null)
                    OnTouchMutiActor(Vector2.zero, clickList_.ToArray(), false);
                return;
            }

            if (UICamera.hoveredObject != null && UICamera.hoveredObject.CompareTag("MutiActor"))
            {
                return;
            }

            if (OnTouchMutiActor != null)
                OnTouchMutiActor(Vector2.zero, clickList_.ToArray(), false);

            if (OnTouchDown != null)
                OnTouchDown();

            GameObject camObj = (GameObject)GameObject.FindGameObjectWithTag("GuideCam");
            if (camObj == null)
                camObj = Camera.main.gameObject;
            Camera cam = camObj.GetComponent<Camera>();
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitInfo;
            int layerMask_ = GuideManager.Instance.IsGuiding_ ? 1 << LayerMask.NameToLayer("Guide") : 1 << LayerMask.NameToLayer("NPC") | 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Ground");
            hitInfo = Physics.RaycastAll(ray, 10000f, layerMask_);
            if(hitInfo == null || hitInfo.Length == 0)
                return;

            FlushClickList(hitInfo);
        }
    }

    public class ClickObj
    {
        public ActorType aType_;
        public int id_;
        public Vector3 position_;

        public ClickObj(ActorType aType, int id, Vector3 position)
        {
            aType_ = aType;
            id_ = id;
            position_ = position;
        }
    }

    List<ClickObj> clickList_ = new List<ClickObj>();

    void FlushClickList(RaycastHit[] list)
    {
        clickList_.Clear();
        if (list == null)
            return;

        for (int i = 0; i < list.Length; ++i)
        {
            if (list[i].collider.CompareTag("Player"))
            {
                clickList_.Add(new ClickObj(ActorType.AT_OtherPlayer, int.Parse(list[i].transform.gameObject.name), list[i].transform.position));
            }

            if (list[i].collider.CompareTag("NPC"))
            {
                clickList_.Add(new ClickObj(ActorType.AT_Npc, int.Parse(list[i].transform.gameObject.name), list[i].transform.position));
            }
        }

        if (clickList_.Count == 0)
        {
            float minDis = float.MaxValue;
            Vector3 point = Vector3.zero;
            for (int i = 0; i < list.Length; ++i)
            {
                if (list[i].collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if (list[i].distance < minDis)
                    {
                        minDis = list[i].distance;
                        point = list[i].point;
                    }
                }
            }
            if (OnTouchGround != null)
                OnTouchGround(point);
            return;
        }

        if (clickList_.Count == 1)
        {
            if (OnTouchActor != null)
                OnTouchActor(clickList_[0].aType_, clickList_[0].id_, clickList_[0].position_);
            return;
        }

        if (OnTouchMutiActor != null)
            OnTouchMutiActor(Vector2.zero, clickList_.ToArray(), true);
    }

    public void ExcuteActor(int idx)
    {
        if (idx < 0 || idx > clickList_.Count - 1)
            return;

        if (OnTouchActor != null)
            OnTouchActor(clickList_[idx].aType_, clickList_[idx].id_, clickList_[idx].position_);
    }

    public void ForceDisplayStick(Vector2 pos)
    {
        if (OnMoveBegin != null)
            OnMoveBegin(pos);

        currentMousePos_ = pos;
        GlobalInstanceFunction.Instance.Invoke(() =>
        {
            GuideManager.Instance.ProcEvent(ScriptGameEvent.SGE_StickDisplay);
        }, 2);
    }

    public void StopMove()
    {
        if (OnMoveEnd != null)
            OnMoveEnd();
        axisx = 0f;
        axisy = 0f;
        pushDown_ = false;
    }
}
