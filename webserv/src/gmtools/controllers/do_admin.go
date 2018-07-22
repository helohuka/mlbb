package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
)

type DoAdminResult struct {
	Id       int
	Name     string
	Password string
	Level    int
	Admin    string
	Tel      string
	Time     string
}

type DoAdmin struct {
	beego.Controller
}

func (c *DoAdmin) Post() {
	c.Ctx.Request.ParseForm()

	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM gmtoos")

	if err != nil {
		beego.Error("DB fail", err)
		return
	}

	results := []DoAdminResult{}

	for records.Next() {
		tmp := DoAdminResult{}
		beego.Debug(tmp)
		err := records.Scan(&tmp.Id, &tmp.Name, &tmp.Password, &tmp.Level, &tmp.Admin, &tmp.Tel, &tmp.Time)
		if err != nil {
			beego.Error("DB fail", err)
			return
		}

		results = append(results, tmp)

	}

	jparam, _ := json.Marshal(results)

	c.Ctx.WriteString(string(jparam))

}
