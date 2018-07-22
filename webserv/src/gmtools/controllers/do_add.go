package controllers

import (
	"github.com/astaxie/beego"
	"strconv"
	"time"
)

type DoAdd struct {
	beego.Controller
}

func (c *DoAdd) checkName(name string) bool {

	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return false
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM `gmtoos` WHERE name=?", name)

	if err != nil {
		beego.Error("DB fail", err)
		return false
	}
	var db_id int
	db_username := ""
	db_password := ""
	var db_level int
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

	if name == db_username {

		return false

	}

	return true

}
func (c *DoAdd) Post() {
	c.Ctx.Request.ParseForm()
	name := c.GetString("w_name")
	password := c.GetString("w_password")

	admin := c.GetString("w_admin")
	tel := c.GetString("w_tel")
	level, lerr := strconv.Atoi(c.GetString("w_level"))
	if lerr != nil {
		c.Ctx.WriteString(lerr.Error())
		return
	}
	time := time.Unix(time.Now().Unix(), 0).Format("2006-01-02 15:04:05")

	if c.checkName(name) {

		db, err := Database()

		if err != nil {
			beego.Error("DB fail", err)
			return
		}
		defer db.Close()
		_, err = db.Exec("INSERT INTO `gmtoos`(name,password,admin,tel,level,time) VALUES(?,?,?,?,?,?)", name, password, admin, tel, level, time)

		if err != nil {
			beego.Error("DB fail", err)
			return
		}
		c.Ctx.WriteString(string(name))
	}

}
