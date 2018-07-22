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

type Oauth struct {
	beego.Controller
}

func (c *Oauth) Get() {
	//beego.Debug("Client=>", string(c.Ctx.Input.RequestBody))
	c.Post()
}

func (c *Oauth) Post() {
	beego.Debug("Client=>", string(c.Ctx.Input.RequestBody))

	req := httplib.Post(beego.AppConfig.String("oauthurl"))
	req.Header("content-type", "application/x-www-form-urlencoded")
	req.Body(c.Ctx.Input.RequestBody).Debug(true)

	str, err := req.String()

	if err != nil {
		// error
		c.Ctx.WriteString("{\"status\":\"fail\"}")
		return
	}
	b, _ := beego.AppConfig.Bool("Debug")
	beego.Debug("Server=>", str)
	var oauthObj models.OauthObject
	err = json.Unmarshal([]byte(str), &oauthObj)
	if err != nil {
		beego.Debug(err.Error())
	}
	beego.Debug("Login object :", oauthObj)

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

	//beego.Debug("LoginOauth Req oauth server result : ", str)
	c.Ctx.WriteString(str)
}
