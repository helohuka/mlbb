package cl

import (

	"carriers/models"
	"crypto/md5"
	"encoding/hex"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"

	"encoding/json"
	"fmt"
)

type (
	Pay struct {
		beego.Controller
	}

	Pay2 struct {
		beego.Controller
	}

	params struct {
		IsPay	 bool `json:"isPay"`
		OrderId   string `json:"orderID"`
		Status    int `json:"status"`
		Amount    float32 `json:"amount"`
		ChannelId int `json:"channelID"`
		ProductId string `json:"productID"`
		UserId    string `json:"userID"`
		RoleId    string `json:"roleID"`
		PayTime   int `json:"time"`
		ServerID int `json:"serverId"`
	}

	params2 struct {
		IsPay	 bool `json:"isPay"`
		OrderId   string `json:"orderID"`
		AppId 	  int `json:"appid"`
		ServerID  int `json:"serverID"`
		Channel int `json:"channel"`
		Vip    int `json:"vip"`
		Gift    int `json:"gift"`
		UserName    string `json:"username"`
		RoleID string `json:"roleID"`
		RoleName    string `json:"roleName"`
		Amount    float32 `json:"amount"`
		PayTime   int `json:"time"`
	}
)

func (c *Pay) FetchParams() *params {

	//orderID(String) : SDKServer 生成的订单号，游戏需根据订单号去重
	//status(int) : 状态，1：成功; 其他失败
	//deviceType(int) :1 - 安卓 2 - ios
	//channelID(int) : SDK 子渠道号，不同渠道分成不一样，记录对账使用
	//userID(String) : 用户唯一标识
	//serverID(int) : 游戏服务器 ID
	//roleID(String) : 游戏服角色 ID
	//productID(String) : 游戏服商品 ID
	//currency(String) : 货币类型，默认人民币 - RMB 平台币 - BTC(不计入流水)
	//amount(int) : 订单金额，单位分
	//time(int) : 时间戳，单位秒*/

	orderID := c.GetString("orderID", "");
	status, _ := c.GetInt("status", -1);
	deviceType, _ := c.GetInt("deviceType", -1);
	channelID, _ := c.GetInt("channelID", -1);
	userID := c.GetString("userID", "");
	serverID, _ := c.GetInt("serverID", -1);
	roleID := c.GetString("roleID", "");
	productID := c.GetString("productID", "");
	currency := c.GetString("currency", "");
	amount, _ := c.GetInt("amount", -1);
	time, _ := c.GetInt("time", -1);
	extension := c.GetString("extension", "");

	sign0 := c.GetString("sign", "");

	str := "orderID=" + orderID + "&";
	str += fmt.Sprintf("status=%d&", status);
	str += fmt.Sprintf("deviceType=%d&", deviceType);
	str += fmt.Sprintf("channelID=%d&", channelID);
	str += "userID=" + userID + "&";
	str += fmt.Sprintf("serverID=%d&", serverID);
	str += "roleID=" + roleID + "&";
	str += "productID=" + productID + "&";
	str += "currency=" + currency + "&";
	str += fmt.Sprintf("amount=%d&", amount);
	str += fmt.Sprintf("time=%d&", time);
	str += "extension=" + extension + "&";
	str += "key=6bc2519c792f1c217d632a6f4a4b6c0e" ;

	md5val := md5.Sum([]byte(str))
	sign1 := hex.EncodeToString(md5val[:])

	beego.Debug("sign0 = " + sign0 + " sign1" + sign1)

	if status != 1 {
		beego.Error("pay status != 1")
		return nil
	}

	if sign0 != sign1{
		beego.Error(" sign0 != sign1")
	}

	rparams := params{
		true,
		orderID,
		status,
		float32(amount) / 100,
		channelID,
		productID,
		userID,
		roleID,
		time,
		serverID,
	}
	return &rparams;

}

func (c *Pay) Post() {
	beego.Debug(c.Ctx.Request.URL.RawQuery)

	oparams := c.FetchParams()

	if oparams == nil{
		c.Ctx.WriteString("FAIL")
		return
	}

	beego.Debug(oparams)

	host := models.GetServPayHostById(oparams.ServerID)
	gamereq := httplib.Post(host)
	gamereq.JSONBody(oparams)

	_, gameerr := gamereq.String()
	if gameerr != nil {
		beego.Debug("Connect game server fail")
		c.Ctx.WriteString("FAIL")
		return
	}

	b, _ := json.Marshal(oparams)

	models.InsertOrderLog(string(b))

	c.Ctx.WriteString("SUCCESS")
}


////////////////////////////////////////////////////////////////

func (c *Pay2) FetchParams() *params2 {

	orderID := c.GetString("orderID", "");
	appID, _ := c.GetInt("appid", -1);
	serverID, _ := c.GetInt("serverID", -1);
	channel, _ := c.GetInt("channel", -1);
	vip,_:= c.GetInt("vip", -1);
	gift, _ := c.GetInt("gift", -1);
	username := c.GetString("username", "");
	roleID := c.GetString("roleID", "");
	roleName := c.GetString("roleName", "");
	amount, _ := c.GetInt("amount", -1);
	time, _ := c.GetInt("time", -1);

	sign0 := c.GetString("sign", "");

	str := "orderID=" + orderID + "&";
	str += fmt.Sprintf("appid=%d&", appID);
	str += fmt.Sprintf("serverID=%d&", serverID);
	str += fmt.Sprintf("channel=%d&", channel);
	str += fmt.Sprintf("vip=%d&", vip);
	str += fmt.Sprintf("gift=%d&", gift);
	str += "username=" + username + "&";
	str += "roleID=" + roleID+ "&";
	str += "roleName=" + roleName+ "&";
	str += fmt.Sprintf("amount=%d&", amount);
	str += fmt.Sprintf("time=%d&", time);
	str += "key=6bc2519c792f1c217d632a6f4a4b6c0e" ;

	md5val := md5.Sum([]byte(str))
	sign1 := hex.EncodeToString(md5val[:])

	beego.Debug("sign0 = " + sign0 + " sign1" + sign1)

	if sign0 != sign1{
		beego.Error(" sign0 != sign1")
	}

	rparams := params2{
		false,
		orderID ,
		appID,
		serverID,
		channel,
		vip,
		gift,
		username,
		roleID ,
		roleName,
		float32(amount) * 3,
		time,
	}
	return &rparams;

}


func (c *Pay2) Post() {
	beego.Debug(c.Ctx.Request.URL.RawQuery)

	oparams := c.FetchParams()

	if oparams == nil{
		c.Ctx.WriteString("FAIL")
		return
	}

	beego.Debug(oparams)

	host := models.GetServPayHostById(oparams.ServerID)
	gamereq := httplib.Post(host)
	gamereq.JSONBody(oparams)

	_, gameerr := gamereq.String()
	if gameerr != nil {
		beego.Debug("Connect game server fail")
		c.Ctx.WriteString("FAIL")
		return
	}

	b, _ := json.Marshal(oparams)

	models.InsertOrderLog(string(b))

	c.Ctx.WriteString("SUCCESS")
}

