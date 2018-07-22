package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
)

type DoScriptCmd struct {
	Type   string
	Script string
}

type DoExecute struct {
	beego.Controller
}

func (c *DoExecute) Post() {
	c.Ctx.Request.ParseForm()

	serv := ToServIds(c.GetStrings("servs"))[0]

	cmd := DoScriptCmd{"GMT_DoScript", c.GetString("content")}
	if len(cmd.Script) == 0 {
		c.Ctx.WriteString("Script parameter is nil!!!")
		return
	}
	jparam, _ := json.Marshal(cmd)

	results := PostGMTServ(serv, jparam)
	jresults, _ := json.Marshal(results)

	c.Ctx.WriteString(string(jresults))
}
