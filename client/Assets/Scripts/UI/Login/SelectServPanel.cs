using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;

public class SelectServPanel : MonoBehaviour {

    public GameObject selectServItem_;
    public UIGrid grid_;
    public UIButton closeBtn_;

    static List<Serv> servData_;
    Dictionary<string, GameObject> servObject_;
    Dictionary<string, string[]> servRoleName_;

    public UIButton[] tabs_;
    public UILabel[] labs_;

    int crtTab_;

    void Awake()
    {
        servRoleName_ = new Dictionary<string, string[]>();
    }
	// Use this for initialization
	void Start () {

        UIEventListener.Get(closeBtn_.gameObject).onClick += OnClose;

        //StartCoroutine(pullServRole());
        UpdateUI();
        //StartCoroutine(pullServPing());
	}

    class Serv
    {
        public bool isonline;
        public int id;
        public int capa;
        public int area;
        public int port;
        public int isnew;
        public string group;
        public string name;
        public string areaname;
        public string host;
        public string[] players;
    }

    public void SetData(string jsonStr)
    {
        servObject_ = new Dictionary<string, GameObject>();
        servData_ = LitJson.JsonMapper.ToObject<List<Serv>>(jsonStr);

        int maxIdx = 0;
        List<string> tabstr = new List<string>();
        for (int i = 0; i < servData_.Count; ++i)
        {
            if (!tabstr.Contains(servData_[i].areaname))
            {
                labs_[tabstr.Count].text = servData_[i].areaname;
                tabstr.Add(servData_[i].areaname);
            }

            if (maxIdx <= tabstr.Count)
                maxIdx = tabstr.Count;
        }
        tabstr.Clear();

        for (int i = maxIdx; i < tabs_.Length; ++i)
        {
            tabs_[i].gameObject.SetActive(false);
        }
        crtTab_ = 0;

        if (string.IsNullOrEmpty(GameManager.ServName_) && servData_.Count > 0)
        {
            string servName = servData_[0].name;
            for (int i = 0; i < servData_.Count; ++i)
            {
                if (servData_[i].isnew == 1)
                {
                    servName = servData_[i].name;
                    break;
                }
            }
            GameManager.ServName_ = servName;
            GameManager.ServId_ = ServId(GameManager.ServName_);
            GameManager.serChanged_ = true;
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < tabs_.Length; ++i)
        {
            if (crtTab_ == i)
                tabs_[i].normalSprite = "qieyeliang";
            else
                tabs_[i].normalSprite = "qieyean";
        }

        SelectServItem ssi = null;
        GameObject ssigo = null;
        int objIdx = 0;

        servObject_.Clear();
        for (int i = 0; i < grid_.transform.childCount; ++i)
        {
            ssigo = grid_.transform.GetChild(i).gameObject;
            UIEventListener.Get(ssigo).onClick -= OnClickServItem;
			UIEventListener.Get(ssigo).onClick -= OnClickOfflineServItem;
            grid_.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        for (int i = 0; i < servData_.Count; ++i)
        {
            if (!labs_[crtTab_].text.Equals(servData_[i].areaname))
                continue;

            if (objIdx >= grid_.transform.childCount)
            {
                ssigo = (GameObject)GameObject.Instantiate(selectServItem_) as GameObject;
                ssigo.transform.parent = grid_.transform;
                ssigo.transform.localScale = Vector3.one;
                ssi = ssigo.GetComponent<SelectServItem>();
                ssi.SetData(servData_[i].name, servData_[i].capa, servData_[i].isnew == 1, servData_[i].players, servData_[i].isonline);
                UIEventListener listener = UIEventListener.Get(ssigo);
                listener.parameter = servData_[i].name;
                if(servData_[i].isonline || servData_[i].areaname.Equals("内测大区"))
                    listener.onClick += OnClickServItem;
                else
                    listener.onClick += OnClickOfflineServItem;
                ssigo.SetActive(true);
                servObject_.Add(servData_[i].name, ssigo);
            }
            else
            {
                ssigo = grid_.transform.GetChild(objIdx).gameObject;
                ssi = ssigo.GetComponent<SelectServItem>();
                ssi.SetData(servData_[i].name, servData_[i].capa, servData_[i].isnew == 1, servData_[i].players, servData_[i].isonline);
                UIEventListener listener = UIEventListener.Get(ssigo);
                listener.parameter = servData_[i].name;
				if (servData_[i].isonline || servData_[i].areaname.Equals("内测大区"))
                    listener.onClick += OnClickServItem;
                else
                    listener.onClick += OnClickOfflineServItem;
                ssigo.SetActive(true);
                servObject_.Add(servData_[i].name, ssigo);
            }
            objIdx++;
        }
        grid_.Reposition();
    }

    public void OnTabSelect(GameObject go)
    {
        switch (go.name)
        {
            case "Tab1":
                if (crtTab_ == 0)
                    return;
                crtTab_ = 0;
                break;
            case "Tab2":
                if (crtTab_ == 1)
                    return;
                crtTab_ = 1;
                break;
            case "Tab3":
                if (crtTab_ == 2)
                    return;
                crtTab_ = 2;
                break;
            case "Tab4":
                if (crtTab_ == 3)
                    return;
                crtTab_ = 3;
                break;
            case "Tab5":
                if (crtTab_ == 4)
                    return;
                crtTab_ = 4;
                break;
            case "Tab6":
                if (crtTab_ == 5)
                    return;
                crtTab_ = 5;
                break;
            case "Tab7":
                if (crtTab_ == 6)
                    return;
                crtTab_ = 6;
                break;
            case "Tab8":
                if (crtTab_ == 7)
                    return;
                crtTab_ = 7;
                break;
        }
        
        UpdateUI();
        //StartCoroutine(pullServPing());
    }

    public string Host(string servName)
    {
        for (int i = 0; i < servData_.Count; ++i)
        {
            if (servData_[i].name.Equals(servName))
                return servData_[i].host;
        }
        return "";
    }

    public int Port(string servName)
    {
        for (int i = 0; i < servData_.Count; ++i)
        {
            if (servData_[i].name.Equals(servName))
                return servData_[i].port;
        }
        return -1;
    }

    public int ServId(string servName)
    {
        for (int i = 0; i < servData_.Count; ++i)
        {
            if (servData_[i].name.Equals(servName))
                return servData_[i].id;
        }
        return -1;
    }

    void OnClickServItem(GameObject go)
    {
        GameManager.ServName_ = UIEventListener.Get(go).parameter.ToString();
        GameManager.ServId_ = ServId(GameManager.ServName_);
        GameManager.serChanged_ = true;
        gameObject.SetActive(false);
    }

    void OnClickOfflineServItem(GameObject go)
    {
        ApplicationEntry.Instance.PostSocketErr(10061);
    }

    void OnClose(GameObject go)
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        //StartCoroutine(pullServPing());
    }

    IEnumerator pullServPing()
    {
        string host = "";
        int port = 0;
        for (int i = 0; i < servData_.Count; ++i)
        {
            if (!servObject_.ContainsKey(servData_[i].name))
                continue;

            host = servData_[i].host;
            port = servData_[i].port;

            IPAddress[] ips = Dns.GetHostAddresses(host);
            Ping reply = new Ping(ips[0].ToString());
            yield return new WaitForSeconds(0.35f);

            if (!servObject_.ContainsKey(servData_[i].name))
                continue;

            if (reply.isDone)
            {
                servObject_[servData_[i].name].GetComponent<SelectServItem>().FlushPing(reply.time);
            }
            else
            {
                servObject_[servData_[i].name].GetComponent<SelectServItem>().FlushPing(350L);
            }
        }
        yield return null;
    }
}
