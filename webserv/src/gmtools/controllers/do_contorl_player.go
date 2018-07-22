package controllers

import (
	"encoding/json"
	"strconv"

	"github.com/astaxie/beego"
)

type ControPlayerCmd struct {
	Type        string `json:"Type"`
	CmdType     string `json:"CmdType"`
	PlayerId    int    `json:"PlayerId"`
	Param       int    `json:"Param"`
	ScriptParam string
}

type DoContorlPlayer struct {
	beego.Controller
}

func (c *DoContorlPlayer) Post() {
	c.Ctx.Request.ParseForm()

	serv := ToServIds(c.GetStrings("servs"))[0]

	var cmd ControPlayerCmd
	cmd.Type = "GMT_GMCommand"
	cmd.CmdType = c.GetString("player_type")
	cmd.PlayerId, _ = strconv.Atoi(c.GetString("playerid"))
	if cmd.CmdType == "GMCT_DoScript" {
		cmd.ScriptParam = c.GetString("param")
	} else {
		cmd.Param, _ = strconv.Atoi(c.GetString("param"))
	}

	jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	results := PostGMTServ(serv, jparam)
	jresults, _ := json.Marshal(results)

	c.Ctx.WriteString(string(jresults))
}
