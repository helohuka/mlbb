package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	_ "strconv"
	_ "strings"
	_ "time"
)

type PlayerInformationCmd struct {
	Type string
}

type DoPlayerInformation struct {
	beego.Controller
}

func (c *DoPlayerInformation) Post() {
	c.Ctx.Request.ParseForm()

	servs := MakeServIds(c.Input(), "player_information_serv_")

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}
	cmd := PlayerInformationCmd{}
	cmd.Type = "GMT_PlayerInformation"

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)
	c.Redirect("/alert?param="+string(jresults), 302)

}
