package controllers

import (
	"encoding/json"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/httplib"
	"strconv"
	_ "strings"
)

type DoAddSeverCmd struct {
	Id        int
	Area      int
	Name      string
	AreaName  string
	Host      string
	Port      int
	LogDBHost string
	//LogDBPort   int
	LogDBUsr string
	LogDBPwd string
	///LogDBName   string
	//Path string
	//DBGameName  string
	Source string
	//SSHUsername string
	SSHPassword string
}

type DoAddSever struct {
	beego.Controller
}

func (c *DoAddSever) Post() {
	c.Ctx.Request.ParseForm()

	ID, err := strconv.Atoi(c.Input().Get("ServID"))

	if err != nil {
		c.Ctx.WriteString(err.Error())
		return
	}
	Area, Aerr := strconv.Atoi(c.Input().Get("Area"))
	if Aerr != nil {
		c.Ctx.WriteString("2")
		return
	}

	Name := c.Input().Get("Name")

	AreaName := c.Input().Get("AreaName")

	Host := c.Input().Get("Host")

	Port, Perr := strconv.Atoi(c.Input().Get("Port"))
	if Perr != nil {
		c.Ctx.WriteString("4")
		return
	}

	LogDBHost := c.Input().Get("LogDBHost")

	//LogDBPort, Lerr := strconv.Atoi(c.Input().Get("LogDBPort"))
	//if Lerr != nil {
	//c.Ctx.WriteString("1")
	//return
	//}

	LogDBUsr := c.Input().Get("LogDBUsr")

	LogDBPwd := c.Input().Get("LogDBPwd")

	//LogDBName := c.Input().Get("LogDBName")

	//Path := c.Input().Get("Path")
	//DBGameName := c.Input().Get("DBgameName")

	Source := c.Input().Get("Source")
	//SSHUsername := c.Input().Get("SSHUsername")
	SSHPassword := c.Input().Get("SSHPassword")
	cmd := DoAddSeverCmd{}
	cmd.Id = ID
	cmd.Area = Area
	cmd.Name = Name
	cmd.AreaName = AreaName
	cmd.Host = Host
	cmd.Port = Port

	cmd.LogDBHost = LogDBHost
	//cmd.LogDBPort = LogDBPort
	cmd.LogDBUsr = LogDBUsr
	cmd.LogDBPwd = LogDBPwd
	//cmd.LogDBName = LogDBName

	//cmd.Path = Path
	//cmd.DBGameName = DBGameName

	cmd.Source = Source
	//cmd.SSHUsername = SSHUsername
	cmd.SSHPassword = SSHPassword
	jparam, _ := json.Marshal(cmd)

	req := httplib.Post(beego.AppConfig.String("carriershost") + "servs/create")
	req.Param("servj", string(jparam))
	str, err := req.String()
	if err != nil {
		beego.Debug(err)
	}
	c.Redirect("/alert?param="+string(str), 302)

}
