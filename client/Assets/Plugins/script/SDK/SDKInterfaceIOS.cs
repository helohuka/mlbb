#if UNITY_IOS
using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

public class SDKInterfaceIOS : SDKInterface
{
  
    [DllImport("__Internal")]
    private static extern void login();

    [DllImport("__Internal")]
    private static extern void logout();

    [DllImport("__Internal")]
    private static extern void pay(string json);

    [DllImport("__Internal")]
    private static extern void __exit();

    [DllImport("__Internal")]
    private static extern void submitGameData(string json);

    public override void Login()
    {
        login();
    }

    public override bool Logout()
    {
        logout();
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

        pay(json);
    }

    public override bool SDKExit()
    {
        __exit();
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
        submitGameData(json);
    }
}

#endif