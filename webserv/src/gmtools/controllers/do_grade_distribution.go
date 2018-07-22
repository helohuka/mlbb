package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	_ "strconv"
	_ "strings"
	_ "time"
)

type GradeDistributionCmd struct {
	Type string
}

type DoGradeDistribution struct {
	beego.Controller
}

func (c *DoGradeDistribution) Post() {
	c.Ctx.Request.ParseForm()

	servs := MakeServIds(c.Input(), "grade_distribution_serv_")

	if len(servs) == 0 {
		c.Redirect("/error?param=没有选择服务器", 302)
		return
	}
	cmd := GradeDistributionCmd{}
	cmd.Type = "GMT_GradeDistribution"

	jparam, _ := json.Marshal(cmd)

	results := PostGMTServs(servs, jparam)
	jresults, _ := json.Marshal(results)
	c.Redirect("/alert?param="+string(jresults), 302)

}
