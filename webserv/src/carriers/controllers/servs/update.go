package servs

import (
	"carriers/models"
	_ "encoding/json"
	"strconv"

	"github.com/astaxie/beego"
)

type Update struct {
	beego.Controller
}

func (c *Update) Post() {

	servid, _ := strconv.Atoi(c.Input().Get("servid"))
	channels := c.Input().Get("channels")
	version := c.Input().Get("version")
	resVersion := c.Input().Get("resVersion")
	sandbox := 0
	if c.Input().Get("sandbox") == "1" {
		sandbox = 1
	}

	models.UpdateServerInfo(servid, channels, version, resVersion, sandbox)

	c.Ctx.WriteString("Success")
}
