package sdk

import (
	"carriers/models"
	_ "encoding/json"

	"github.com/astaxie/beego"
)

type (
	UpdateAnyId struct {
		beego.Controller
	}
	GetSDKInfo struct {
		beego.Controller
	}
)

func (c *UpdateAnyId) Post() {
	anyId := c.Input().Get("anyId")
	pfId := c.Input().Get("pfId")
	pfName := c.Input().Get("pfName")
	desc := c.Input().Get("desc")

	if anyId == "" || pfId == "" {
		c.Ctx.WriteString("服务器不在线")
		return
	}

	models.AddAnyId(anyId, pfId, pfName, desc)

	c.Ctx.WriteString("OK")
}
