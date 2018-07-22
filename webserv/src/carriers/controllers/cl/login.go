package cl

import (
	"carriers/models"
	"fmt"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
)

type Login struct {
	beego.Controller
}

func (c *Login) GetHostString(token string) string {
	return fmt.Sprintf("http://api.le890.com/index.php?m=api&a=%s", token)
}

func (c *Login) Post() {
	uid := c.GetString("uid")
	tok := c.GetString("tok")

	req := httplib.Post(c.GetString(tok))
	req.Header("content-type", "application/x-www-form-urlencoded")
	req.Param("appid", appid)
	req.Param("uid", uid)
	req.Param("t", tok)

	str, err := req.String()

	if err != nil {
		beego.Error(err.Error())
		c.Ctx.WriteString("FAILED")
		return
	}

	if str != "success" {
		beego.Error(str)
		c.Ctx.WriteString("FAILED")
		return
	}

	//servs := models.GetServsByChannel(channel)
	servs := models.FindServerByChannel(fmt.Sprintf("%d", channel))
	if len(servs) == 0 {
		beego.Info("Channel =======> ", channel, " can not find servs")
		c.Ctx.WriteString("FAILED")
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
	c.Ctx.WriteString("SUCCESS")
}
