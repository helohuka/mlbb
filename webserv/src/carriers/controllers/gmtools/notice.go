package gmtools

import (
	"carriers/models"
	"encoding/json"

	"github.com/astaxie/beego"
)

type GMTReslut struct {
	Error int    `json:"error"`
	Desc  string `json:"desc"`
}

type SystemNotice struct {
	beego.Controller
}

func (c *SystemNotice) Post() {

	beego.Debug(c.GetString("channel"), c.GetString("content"))
	models.UpdateSystemNotice(c.GetString("channel"), c.GetString("content"))
	result := [1]GMTReslut{}
	result[0].Error = 0
	result[0].Desc = "Change system notice success"
	strresult, _ := json.Marshal(result)
	c.Ctx.WriteString((string)(strresult))
}

type ServerNotice struct {
	beego.Controller
}

func (c *ServerNotice) Post() {
	strservs := c.Input().Get("servs")
	beego.Debug(strservs)
	servids := []int{}
	json.Unmarshal(([]byte)(strservs), &servids)
	content := c.Input().Get("content")
	results := []GMTReslut{}

	for i := 0; i < len(servids); i++ {
		result := GMTReslut{}
		serv := models.GetServById(servids[i])
		if serv != nil {
			serv.UpdateNotice(content)
			result.Error = 0
			result.Desc = "Server " + serv.Name + " success"
		} else {
			result.Error = -1
			result.Desc = "Server " + serv.Name + " failed"
		}
		results = append(results, result)
	}
	strresults, _ := json.Marshal(results)
	c.Ctx.WriteString((string)(strresults))
}
