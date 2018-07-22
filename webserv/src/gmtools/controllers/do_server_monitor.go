package controllers

import (
	"encoding/json"

	"github.com/astaxie/beego"
)

type ServerMonitorCmd struct {
	Type string
}

type DoServerMonitor struct {
	beego.Controller
}

func (c *DoServerMonitor) Post() {
	c.Ctx.Request.ParseForm()

	servs := ToServIds(c.GetStrings("servs"))
	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}
	cmd := ServerMonitorCmd{}
	cmd.Type = "GMT_ServerMonitor"

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)
	c.Redirect("/alert?param="+string(jresults), 302)

}
