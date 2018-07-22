package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	_ "github.com/astaxie/beego/httplib"
	_ "gmtools/models"
	_ "strconv"
)

type QueryMoneyCmd struct {
	Type string `json:"Type"`
}
type QueryDiaCmd struct {
	Type string `json:"Type"`
}
type QueryRMBCmd struct {
	Type string `json:"Type"`
}
type QueryMoney struct {
	beego.Controller
}
type QueryDia struct {
	beego.Controller
}
type QueryRMB struct {
	beego.Controller
}

func (c *QueryRMB) Post() {
	serv := ToServIds(c.GetStrings("servs"))[0]
	cmd := QueryRMBCmd{}
	cmd.Type = "GMT_QueryRMB"
	jparam, _ := json.Marshal(cmd)
	results := PostGMTServ(serv, jparam)
	jresults, _ := json.Marshal(results)
	c.Ctx.WriteString(string(jresults))
}
func (c *QueryDia) Post() {
	serv := ToServIds(c.GetStrings("servs"))[0]
	cmd := QueryDiaCmd{}
	cmd.Type = "GMT_QueryDia"
	jparam, _ := json.Marshal(cmd)
	results := PostGMTServ(serv, jparam)
	jresults, _ := json.Marshal(results)
	c.Ctx.WriteString(string(jresults))
}
func (c *QueryMoney) Post() {
	serv := ToServIds(c.GetStrings("servs"))[0]

	cmd := QueryMoneyCmd{}
	cmd.Type = "GMT_QueryMoney"
	jparam, _ := json.Marshal(cmd)

	results := PostGMTServ(serv, jparam)
	jresults, _ := json.Marshal(results)

	c.Ctx.WriteString(string(jresults))
}
