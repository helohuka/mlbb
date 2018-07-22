package notice

import (
	"carriers/models"

	"github.com/astaxie/beego"
)

type System struct {
	beego.Controller
}

func (c *System) Post() {
	channelid := c.GetString("channelid")
	if channelid == "" {
		beego.Error("System notice channel id is 0")
		c.Ctx.WriteString("")
		return
	}
	notice := models.GetSystemNotice(channelid)
	//beego.Debug(channelid, "System notice:", notice)
	c.Ctx.WriteString(notice)
}
