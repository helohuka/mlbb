package controllers

import (
	"github.com/astaxie/beego"
)

type Regist struct {
	beego.Controller
}

func (c *Regist) checkUser(username string, password string) bool {

	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return false
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM `gmtoos` WHERE name=?", username)

	if err != nil {
		beego.Error("DB fail", err)
		return false
	}

	db_username := ""
	db_password := ""
	var db_level int
	db_Admin := ""
	db_tel := ""
	db_time := ""
	var db_id int

	for records.Next() {
		err := records.Scan(&db_id, &db_username, &db_password, &db_level, &db_Admin, &db_tel, &db_time)

		if err != nil {
			beego.Error("DB fail", err)
			return false
		}
	}

	if db_level == 0 && username == db_username && password == db_password {
		return true
	}

	return false

}
func (c *Regist) Post() {
	c.Ctx.Request.ParseForm()
	username := c.GetString("username")
	password := c.GetString("password")

	if c.checkUser(username, password) {

		c.Ctx.WriteString(string("ok"))
	}

}
