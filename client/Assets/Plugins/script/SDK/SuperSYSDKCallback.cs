using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// java 回调 unity 接口
/// </summary>
public class SuperSYSDKCallback : MonoBehaviour
{

    private static SuperSYSDKCallback _instance;

    private static object _lock = new object();

    //初始化回调对象
    public static SuperSYSDKCallback InitCallback()
    {
        UnityEngine.Debug.LogError("Callback->InitCallback");

        lock (_lock)
        {
            if (_instance == null)
            {
                GameObject callback = GameObject.Find("(SuperSYSDK_callback)");
                if (callback == null)
                {
                    callback = new GameObject("(SuperSYSDK_callback)");
                    _instance = callback.AddComponent<SuperSYSDKCallback>();
                    UnityEngine.Object.DontDestroyOnLoad(callback);

                }
                else
                {
                    _instance = callback.GetComponent<SuperSYSDKCallback>();
                }
            }

            return _instance;
        }
    }


    //初始化成功回调
    public void OnInitSuc()
    {
        //初始化成功后才可以调用登录
        UnityEngine.Debug.LogError("Callback->OnInitSuc");
    }

    //登录成功回调
    public void OnLoginSuc(string jsonData)
    {
        UnityEngine.Debug.LogError("Callback->OnLoginSuc");

        LoginResult data = parseLoginResult(jsonData);
        if (data == null)
        {
            UnityEngine.Debug.LogError("The data parse error." + jsonData);
            return;
        }

        if (SDKInterface.Instance.OnLoginSuc != null)
        {
            SDKInterface.Instance.OnLoginSuc.Invoke(data);
        }
    }

    //登出回调
    public void OnLogout()
    {
        UnityEngine.Debug.LogError("Callback->OnLogout");

        if (SDKInterface.Instance.OnLogout != null)
        {
            SDKInterface.Instance.OnLogout.Invoke();
        }
    }

    //支付回调
    public void OnPaySuc(string jsonData)
    {
        UnityEngine.Debug.LogError("Callback->OnPaySuc");

        int payCode = parsePayResult(jsonData);
        if (SDKInterface.Instance.OnPaySuc != null)
        {
            SDKInterface.Instance.OnPaySuc.Invoke(payCode);
        }
    }

    private LoginResult parseLoginResult(string str)
    {
        object jsonParsed = MiniJSON.Json.Deserialize(str);
        if (jsonParsed != null)
        {
            Dictionary<string, object> jsonMap = jsonParsed as Dictionary<string, object>;
            LoginResult data = new LoginResult();
            if (jsonMap.ContainsKey("isSuc"))
            {
                data.isSuc = bool.Parse(jsonMap["isSuc"].ToString());
            }
            if (jsonMap.ContainsKey("isSwitchAccount"))
            {
                data.isSwitchAccount = bool.Parse(jsonMap["isSwitchAccount"].ToString());
            }
            if (jsonMap.ContainsKey("userID"))
            {
                data.userID = jsonMap["userID"].ToString();
            }
            if (jsonMap.ContainsKey("token"))
            {
                data.token = jsonMap["token"].ToString();
            }

            return data;
        }

        return null;
    }

    private int parsePayResult(string str)
    {
        int payCode = 0;
        object jsonParsed = MiniJSON.Json.Deserialize(str);
        if (jsonParsed != null)
        {
            Dictionary<string, object> jsonMap = jsonParsed as Dictionary<string, object>;   
            
            if (jsonMap.ContainsKey("payCode"))
            {
                payCode = int.Parse(jsonMap["payCode"].ToString());
            }
        }

        return payCode;
    }
}
