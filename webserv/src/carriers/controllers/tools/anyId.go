package controllers

import (
	_ "carriers/models"
	_ "encoding/json"

	"github.com/astaxie/beego"
)

type UpdatePfid struct {
	beego.Controller
}

func (c *UpdatePfid) Post() {
	chId := c.Input().Get("channelId")
	pfId := c.Input().Get("pfId")

	if chId == "" || pfId == "" {
		c.Ctx.WriteString("服务器不在线")
		return
	}

	c.Ctx.WriteString("OK")
}
