package controllers

import (
	"encoding/json"
	"strconv"

	"github.com/astaxie/beego"
)

type QueryPlayerCmd struct {
	Type       string `json:"Type"`
	PlayerId   int    `json:"PlayerId"`
	PlayerName string `json:"PlayerName"`
}

type DoQueryPlayer struct {
	beego.Controller
}

func (c *DoQueryPlayer) Post() {
	c.Ctx.Request.ParseForm()

	serv := ToServIds(c.GetStrings("servs"))[0]

	radio := c.GetStrings("radio")[0]

	cmd := QueryPlayerCmd{}

	cmd.Type = "GMT_QueryPlayer"

	if radio == "true" {
		cmd.PlayerId, _ = strconv.Atoi(c.Input().Get("query_player_playerid"))

	} else {
		cmd.PlayerName = c.Input().Get("query_player_playername")

	}

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServ(serv, jparam)

	if results.ErrorNo == 0 {
		c.TplName = "query_player_result.html"
		info := PlayerInfo{}
		json.Unmarshal(([]byte)(results.ErrorDesc), &info)
		c.Data["Param"] = info

	} else {
		jresults, _ := json.Marshal(results)
		c.Redirect("/alert?param="+string(jresults), 302)
	}
}
