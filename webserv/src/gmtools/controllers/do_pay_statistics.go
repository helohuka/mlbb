package controllers

import (
	"database/sql"
	"encoding/json"
	"github.com/astaxie/beego"
	"strconv"
	_ "strings"
	_ "time"
)

type PayStatisticsResult struct {
	Game      string
	Roleid    int
	Orderid   string
	Accountid string
	Payment   float32
	Paytime   string
}
type PayStatisticsCmd struct {
	Type string
}

type DoPayStatistics struct {
	beego.Controller
}

func (c *DoPayStatistics) Post() {
	c.Ctx.Request.ParseForm()
	sever, _ := strconv.Atoi(c.GetString("id"))

	fb, err := Database1(sever)

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer fb.Close()
	var records *sql.Rows

	records, err = fb.Query("SELECT `paytime`,`orderid`,`accountid`,`roleid`,`payment` FROM `order`")

	if err != nil {
		beego.Error("DB fail", err)
		return
	}

	results := []PayStatisticsResult{}

	for records.Next() {

		tmp := PayStatisticsResult{}

		err := records.Scan(&tmp.Paytime, &tmp.Orderid, &tmp.Accountid, &tmp.Roleid, &tmp.Payment)
		if err != nil {
			beego.Error("DB fail", err)
			return
		}

		results = append(results, tmp)

	}

	jparam, _ := json.Marshal(results)

	c.Ctx.WriteString(string(jparam))

}
