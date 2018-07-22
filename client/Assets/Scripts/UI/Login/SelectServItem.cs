using UnityEngine;
using System.Collections;

public class SelectServItem : MonoBehaviour {

    public UISprite status_;
    public UILabel servName_;
    public UILabel[] roles_;
    public UILabel ping_;
    public GameObject isNew_;

    int green = 500;
    int yellow = 1000;

    public void SetData(string servName, int capa, bool isNew, string[] players, bool isOnline)
    {
        string status = "";
        if (!isOnline)
        {
            status = "4";
        }
        else
        {
            if (capa < green)
                status = "3";
            else if (capa < yellow)
                status = "2";
        }

        status_.spriteName = "tishi" + status;
        servName_.text = servName;
        isNew_.SetActive(isNew);

        for (int i = 0; i < roles_.Length; ++i)
        {
            roles_[i].text = LanguageManager.instance.GetValue("NoPlayer");
        }

        if (players != null)
        {
            for (int i = 0; i < players.Length; ++i)
            {
                roles_[i].text = players[i];
            }
        }
    }

    public void FlushPing(long delay)
    {
        string foreStr = "<1ms";
        Color color = GlobalValue.GREEN;
        if (delay > 349)
        {
            foreStr = ">350ms";
            color = GlobalValue.RED;
        }
        else if (delay > 150)
        {
            foreStr = delay.ToString() + "ms";
            color = GlobalValue.YELLOW;
        }
        else if (delay > 1)
        {
            foreStr = delay.ToString() + "ms";
            color = GlobalValue.GREEN;
        }

        ping_.text = foreStr;
        ping_.color = color;
    }
}
