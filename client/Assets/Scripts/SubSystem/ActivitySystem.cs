using System.Collections.Generic;

public class ActivitySystem {

	public delegate void Updateactiviti();
	public static event Updateactiviti UpdateactivitiOk;

	public delegate void ReceiveActivity (uint itemid);
	public static event ReceiveActivity ReceiveActivityOK;

	public static List<uint> flags = new List<uint> ();
    static ActivitySystem inst_ = new ActivitySystem();
    static public ActivitySystem Instance
    {
        get { return inst_; }
    }

    public class ActivityInfo
    {
        public enum ActivityState
        {
            AS_None,
            AS_Close,
            AS_Open,
        }
        public int id_;
        public ActivityState crtState_;
    }

    List<ActivityInfo> activitiesList_;

    ushort[] counter_;

	public int petTempTimes_ = 0;

    public delegate void ActivityUpdateHandler(int aid);
    public event ActivityUpdateHandler OnActivityUpdate;

    public delegate void ActivityOpenHandler(int aid);
    public event ActivityOpenHandler OnActivityOpen;

    public void Init()
    {
        Dictionary<int, DaliyActivityData> metaData = DaliyActivityData.MetaData();
        activitiesList_ = new List<ActivityInfo>();
        foreach(DaliyActivityData data in metaData.Values)
        {
            //if(data.activityKind_== ActivityType.ACT_Family_0 ||
            //    data.activityKind_ == ActivityType.ACT_Family_1 ||
            //    data.activityKind_ == ActivityType.ACT_Family_2 ||
            //    data.activityKind_ == ActivityType.ACT_Family_3 ||
            //    data.activityKind_ == ActivityType.ACT_Family_4)
            //    continue;
            ActivityInfo ai = new ActivityInfo();
            ai.id_ = data.id_;
            ai.crtState_ = string.IsNullOrEmpty(data.startTime_)? ActivityInfo.ActivityState.AS_Open: ActivityInfo.ActivityState.AS_Close;
            activitiesList_.Add(ai);
        }
    }

	public void UpdateactivitieOK()
	{


		if(UpdateactivitiOk != null)
		{
			UpdateactivitiOk();
		}
	}
    public List<ActivityInfo> GetAll()
    {
        return activitiesList_;
    }
	public void ReceiveOk(uint index)
	{
		flags.Add (index);
		if(ReceiveActivityOK != null)
		{
			ReceiveActivityOK(index);
		}
	}
    public ActivityInfo.ActivityState GetInfoState(int id)
    {
        for (int i = 0; i < activitiesList_.Count; ++i)
        {
            if(activitiesList_[i].id_ == id)
                return activitiesList_[i].crtState_;
        }
        return ActivityInfo.ActivityState.AS_None;
    }

    public void Update(ActivityType type, bool open)
    {
        DaliyActivityData data;
        for (int i = 0; i < activitiesList_.Count; ++i)
        {
            data = DaliyActivityData.GetData(activitiesList_[i].id_);
            if (data.activityKind_ == type)
            {
                activitiesList_[i].crtState_ = open ? ActivityInfo.ActivityState.AS_Open : ActivityInfo.ActivityState.AS_Close;
                if (open && OnActivityOpen != null)
                    OnActivityOpen(activitiesList_[i].id_);
                if (OnActivityUpdate != null)
					OnActivityUpdate( activitiesList_[i].id_);
                break;
            }
        }
    }

    public void SyncCounter(ActivityType type, int counter)
    {
        if (GamePlayer.Instance.ActivityTable != null)
        {
            for (int i = 0; i < GamePlayer.Instance.ActivityTable.activities_.Length; ++i)
                if (GamePlayer.Instance.ActivityTable.activities_[i].actId_ == (int)type)
                    GamePlayer.Instance.ActivityTable.activities_[i].counter_ = counter;
        }
        //counter_ = counter;
        if (OnActivityUpdate != null)
            OnActivityUpdate(0);
    }

    public void SyncReward(int reward)
    {
        if (GamePlayer.Instance.ActivityTable!= null)
            GamePlayer.Instance.ActivityTable.reward_ = reward;
    }

    public int GetCount(ActivityType type)
    {
        if (GamePlayer.Instance.ActivityTable != null)
        {
            for (int i = 0; i < GamePlayer.Instance.ActivityTable.activities_.Length; ++i)
                if (GamePlayer.Instance.ActivityTable.activities_[i].actId_ == (int)type)
                    return GamePlayer.Instance.ActivityTable.activities_[i].counter_;
        }
        return 0;
    }
}
