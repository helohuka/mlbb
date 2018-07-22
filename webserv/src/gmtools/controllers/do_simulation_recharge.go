package controllers

import (
	_ "encoding/json"
	"github.com/astaxie/beego"
	_ "strconv"
	_ "strings"
	_ "time"
)

type SimulationRechargeCmd struct {
	Type string
}

type DoSimulationRecharge struct {
	beego.Controller
}

func (c *DoSimulationRecharge) Post() {
	c.Ctx.Request.ParseForm()
	beego.Debug(c.GetStrings("ckb"))
	servs := MakeServIds(c.Input(), "simulation_recharge_serv_")

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}

	//jparam, _ := json.Marshal(cmd)
	//beego.Debug(string(jparam))
	//results := PostGMTServs(servs, jparam)
	//jresults, _ := json.Marshal(results)
	//c.Redirect("/alert?param="+string(jresults), 302)

}
