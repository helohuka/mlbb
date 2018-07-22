package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	_ "strconv"
	_ "strings"
	_ "time"
)

type UserRetentionCmd struct {
	Type string
}

type DoUserRetention struct {
	beego.Controller
}

func (c *DoUserRetention) Post() {
	c.Ctx.Request.ParseForm()

	servs := MakeServIds(c.Input(), "user_retention_serv_")

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}
	cmd := UserRetentionCmd{}
	cmd.Type = "GMT_UserRetention"

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)
	c.Redirect("/alert?param="+string(jresults), 302)

}
