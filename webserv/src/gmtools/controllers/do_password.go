package controllers

import (
	"github.com/astaxie/beego"
)

type DoPassword struct {
	beego.Controller
}

func (c *DoPassword) Post() {
	c.Ctx.Request.ParseForm()
	username := c.GetString("name")
	old_password := c.GetString("old_password")
	new_password := c.GetString("new_password")

	db, err := Database()
	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()

	_, err1 := db.Exec("UPDATE `gmtoos` SET `password` = ? WHERE `name` = ?", new_password, username)
	beego.Debug(err1)
	c.Ctx.WriteString(string(old_password))
}
