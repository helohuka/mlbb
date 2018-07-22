package controllers

import (
	"github.com/astaxie/beego"
)

type Login struct {
}
type DoLogin struct {
	beego.Controller
}

func (c *DoLogin) checkUser(username string, password string) bool {

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
	var db_id int
	db_username := ""
	db_password := ""
	db_level := 0
	db_Admin := ""
	db_tel := ""
	db_time := ""

	for records.Next() {
		err := records.Scan(&db_id, &db_username, &db_password, &db_level, &db_Admin, &db_tel, &db_time)

		if err != nil {
			beego.Error("DB fail", err)
			return false
		}
	}

	if username == db_username && password == db_password {

		return true

	}

	return false

}
func (c *DoLogin) Post() {
	c.Ctx.Request.ParseForm()
	username := c.GetString("username")
	password := c.GetString("password")

	if c.checkUser(username, password) {

		c.Ctx.WriteString(string(username))
	}

}
