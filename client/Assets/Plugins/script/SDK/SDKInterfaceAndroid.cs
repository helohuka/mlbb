#if UNITY_ANDROID

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Android平台接口的调用
/// </summary>
public class SDKInterfaceAndroid : SDKInterface
{ 

    private AndroidJavaObject jo;

    public SDKInterfaceAndroid()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
    }

    private T SDKCall<T>(string method, params object[] param)
    {
        try
        {
            return jo.Call<T>(method, param);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        return default(T);
    }

    private void SDKCall(string method, params object[] param)
    {
        try
        {
            jo.Call(method, param);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public override void Login()
    {
        SDKCall("login");
    }

    public override bool Logout()
    {
        SDKCall("logout");
        return true;
    }

    public override void SubmitGameData(ExtraGameData data)
    {
        Dictionary<string, object> map = new Dictionary<string, object>();
        map.Add("dataType", data.dataType);
        map.Add("roleID", data.roleID);
        map.Add("roleName", data.roleName);
        map.Add("roleLevel", data.roleLevel);
        map.Add("serverID", data.serverID);
        map.Add("serverName", data.serverName);
        map.Add("moneyNum", data.moneyNum);
        map.Add("roleCreateTime", data.roleCreateTime);
        map.Add("roleLevelUpTime", data.roleLevelUpTime);
		map.Add("vip", data.vip);
        string json = MiniJSON.Json.Serialize(map);

        SDKCall("submitExtraData", json);
    }

    public override bool SDKExit()
    {
        SDKCall("exit");
        return true;
    }

    public override void Pay(PayParams data)
    {
        Dictionary<string, object> map = new Dictionary<string, object>();

        map.Add("buyNum", data.buyNum);
        map.Add("coinNum", data.coinNum);
        map.Add("price", data.price);
        map.Add("productId", data.productId);
        map.Add("productName", data.productName);
        map.Add("productDesc", data.productDesc);
        map.Add("roleId", data.roleId);
        map.Add("roleName", data.roleName);
        map.Add("roleLevel", data.roleLevel);
        map.Add("serverId", data.serverId);
        map.Add("serverName", data.serverName);
        map.Add("vip", data.vip);
        map.Add("extension", data.extension);
        string json = MiniJSON.Json.Serialize(map);

        SDKCall("pay", json);
    }
}

#endif
