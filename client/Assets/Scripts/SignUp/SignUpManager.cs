using UnityEngine;
using System.Collections;

public class SignUpManager {

    static SignUpManager inst_;
    public static SignUpManager Instance
    {
        get 
        {
            if (inst_ == null)
                inst_ = new SignUpManager();
            return inst_;
        }
    }

    public delegate void UpdateHandler(int idx = 0, bool isMend = false);
    public event UpdateHandler OnUpdateUI;

    int process_;

    bool sign7_, sign14_, sign28_;

    bool isMend_ = false;

    int[] infomation_;

    int signIndex_;

    int today_;
    public int Today { get { return today_; } }

    public int MaxCount { get { return infomation_.Length; } }

    public bool isEmpty()
    {
        return infomation_ == null || infomation_.Length == 0;
    }

    public void Init(int[] info, int process, bool sign7, bool sign14, bool sign28)
    {
        infomation_ = info;
        process_ = process;
        sign7_ = sign7;
        sign14_ = sign14;
        sign28_ = sign28;
        today_ = (int)System.DateTime.Today.Day;
    }

    public void SignUp(int index, bool isMend = false)
    {
        isMend_ = isMend;
        signIndex_ = index;
        // send message
        NetConnection.Instance.sign(signIndex_);
    }

    public void MendSign()
    {
        SignUp(FirstUnSignIndex);
    }

    public void SignUpOk()
    {
        process_ |=  0x1 << signIndex_ ;
//		if(isMend_)
//		{
//			int mendCost_ = 0;
//			GlobalValue.Get(Constant.C_SignPay, out mendCost_);
//			CommonEvent.ExcutePurchase(infomation_[signIndex_], 1, mendCost_);
//		}
//        else
//        {
//            GamePlayer.Instance.todaySigned_ = true;
//        }

        //Update UI
        if (OnUpdateUI != null)
            OnUpdateUI(signIndex_, isMend_);

        //PopText.Instance.Show(LanguageManager.instance.GetValue(isMend_ ? "mendOk" : "signOk"), PopText.WarningType.WT_Tip);
    }

    public void GetReward(int day)
    {
        if (day == 7)
            sign7_ = true;
        else if (day == 14)
            sign14_ = true;
        else
            sign28_ = true;

        PopText.Instance.Show(LanguageManager.instance.GetValue("comboSignReward"), PopText.WarningType.WT_Tip);

        if (OnUpdateUI != null)
            OnUpdateUI();
    }

    public bool IsSignUped(int index)
    {
        return (process_ & (0x1 << index)) != 0;
    }
    
    public int GetRewardIDByIndex(int index)
    {
        return infomation_[index];
    }

    public bool ReceivedGift7 { get { return sign7_; } }

    public bool ReceivedGift14 { get { return sign14_; } }

    public bool ReceivedGift28 { get { return sign28_; } }

    public int ComboSignDay
    {
        get
        {
            int count = 0;
            int currentDay = (int)System.DateTime.Today.Day;
            if (IsSignUped(currentDay) == false)
                currentDay -= 1;

            for (int i = 1; i <= currentDay; ++i)
            {
                if (IsSignUped(i))
                    count++;
                else
                    break;
            }
            return count;
        }
    }

    public bool CanMendSign
    {
        get { return FirstUnSignIndex != 0; }
    }

    public int FirstUnSignIndex
    {
        get 
        {
            for (int i = 1; i < (int)System.DateTime.Today.Day + 1; ++i)
            {
                if (IsSignUped(i) == false)
                    return i;
            }
            return 0;
        }
    }
}
