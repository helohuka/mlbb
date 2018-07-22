package login

import (
	"carriers/models"
	"encoding/json"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

/*
 * 统一登录验证地址
 * */

type AnyOauth struct {
	beego.Controller
}

func (c *AnyOauth) Get() {
	beego.Error("LoginOauth use get method!!! Host:", c.Ctx.Request.URL.Host)
	c.Ctx.WriteString("{}")
}

func (c *AnyOauth) Post() {
	//beego.Info("AnyLogin app host :", c.Ctx.Request.URL.Host)
	//beego.Debug("AnyLoginOauth Post body length :", len(c.Ctx.Input.RequestBody))
	beego.Debug("AnyLoginOauth Post body content :", string(c.Ctx.Input.RequestBody))

	req := httplib.Post(beego.AppConfig.String("anyoauthurl"))
	req.Header("content-type", "application/x-www-form-urlencoded")
	req.Body(c.Ctx.Input.RequestBody)

	str, err := req.String()

	if err != nil {
		// error
		c.Ctx.WriteString("{\"status\":\"fail\"}")
		return
	}
	beego.Debug("LoginOauth Req oauth server result : ", str)

	var oauthObj models.OauthObject
	json.Unmarshal([]byte(str), &oauthObj)

	b, _ := beego.AppConfig.Bool("Debug")
	if b {
		c.Ctx.WriteString(str)
		return
	}

	beego.Debug("Login Status:", oauthObj.Status)
	if oauthObj.Status == "ok" {
		channel := oauthObj.Common.Channel
		//servs := models.GetServsByChannel(channel)
		servs := models.FindServerByChannel(channel)
		if len(servs) == 0 {
			beego.Debug("Channel =======> ", channel, " can not find servs")
			if b {
				c.Ctx.WriteString(str)
				return
			}
			c.Ctx.WriteString("{\"status\":\"fail\"}")
			return
		}
		for _, id := range servs {
			host := models.GetServOauthHostById(id)
			//beego.Debug("Connect game server " + host)
			gamereq := httplib.Post(host)
			gamereq.Body([]byte(str))
			_, gameerr := gamereq.String()
			if gameerr != nil {
				beego.Debug(gameerr.Error(), channel, "<>", id)
			}
		}
		c.Ctx.WriteString(str)
		return

	} else if oauthObj.Status == "fail" {
		c.Ctx.WriteString(str)
		return
	} else {
		c.Ctx.WriteString("{\"status\":\"fail\"}")
		return
	}

	c.Ctx.WriteString(str)

}
