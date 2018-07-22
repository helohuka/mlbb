package controllers

import (
	_ "encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	_ "strconv"
	_ "strings"
)

type DoBackupsSever struct {
	beego.Controller
}

func (c *DoBackupsSever) Post() {
	c.Ctx.Request.ParseForm()

	ID := c.Input().Get("ServID")
	beego.Debug(ID)
	//Area, Aerr := strconv.Atoi(c.Input().Get("Area"))
	//if Aerr != nil {
	//c.Ctx.WriteString("2")
	//return
	//}

	//Name := c.Input().Get("Name")

	//AreaName := c.Input().Get("AreaName")

	//Host := c.Input().Get("Host")

	//Port, Perr := strconv.Atoi(c.Input().Get("Port"))
	//if Perr != nil {
	//c.Ctx.WriteString("4")
	//return
	//}

	//LogDBHost := c.Input().Get("LogDBHost")

	//LogDBPort, Lerr := strconv.Atoi(c.Input().Get("LogDBPort"))
	//if Lerr != nil {
	//c.Ctx.WriteString("1")
	//return
	//}

	//LogDBUsr := c.Input().Get("LogDBUsr")

	//LogDBPwd := c.Input().Get("LogDBPwd")

	//LogDBName := c.Input().Get("LogDBName")

	//Path := c.Input().Get("Path")
	//DBGameName := c.Input().Get("DBgameName")

	//Source := c.Input().Get("Source")
	//SSHUsername := c.Input().Get("SSHUsername")
	//SSHPassword := c.Input().Get("SSHPassword")

	req := httplib.Post(beego.AppConfig.String("carriershost") + "servs/backup")

	//req.Param("password", SSHPassword)
	req.Param("servid", ID)
	//req.Param("package", Source)
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}
	c.Redirect("/alert?param="+string(str), 302)

}
