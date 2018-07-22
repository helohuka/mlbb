package servs

import (
	"carriers/models"
	_ "encoding/json"

	"github.com/astaxie/beego"
)

type CDN struct {
	beego.Controller
}

func (c *CDN) Post() {

	version := c.Input().Get("version")
	channel := c.Input().Get("channel")
	//beego.Debug("CDN::", version, channel)
	s := models.GetServsByChannel_(version, channel)
	if len(s) == 0 {
		beego.Debug("CDN Find none server", version, channel)
		c.Ctx.WriteString("0_0_0")
		return
	}
	c.Ctx.WriteString(s[0].CDN)
}
