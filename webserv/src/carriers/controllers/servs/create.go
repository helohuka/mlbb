package servs

import (
	"carriers/models"
	"encoding/json"

	"fmt"
	"github.com/astaxie/beego"
	"strconv"
	"strings"
)

type Create struct {
	beego.Controller
}

func (c *Create) Post() {
	servj := c.Input().Get("servj")
	serv := models.Serv{}

	err := json.Unmarshal([]byte(servj), &serv)

	if err != nil {
		beego.Debug(err)
		c.Ctx.WriteString("Fail")
		return
	}

	if models.GetServById(serv.Id) != nil {
		beego.Debug("servid triwc")
		c.Ctx.WriteString("Fail")
		return
	}

	mysqluser := beego.AppConfig.String("logicservermysqluser")
	mysqlpass := beego.AppConfig.String("logicservermysqlpass")
	serv.DBUsr = mysqluser
	serv.DBPwd = mysqlpass
	serv.DBPort = 3306
	serv.DBGameName = fmt.Sprintf("%d_game", serv.Id)
	serv.DBLogName = fmt.Sprintf("%d_log", serv.Id)

	models.MakeNewServer(serv.Host+":22", serv.SSHPassword, serv.DBHost, mysqluser, mysqlpass, serv.DBGameName, serv.DBLogName, serv.Source, serv.DBPort, serv.Id, serv.Port)
	serv.IsNew = 1
	serv.OauthPort = serv.Port + 1
	serv.PayPort = serv.OauthPort + 1
	serv.GMTPort = serv.PayPort + 1

	models.CreateServer(&serv)

	c.Ctx.WriteString("OK")
}

type ChangeNewServer struct {
	beego.Controller
}

func (c *ChangeNewServer) Post() {
	servid, _ := strconv.Atoi(c.Input().Get("servid"))
	isnew, _ := strconv.Atoi(c.Input().Get("isnew"))
	models.ChangeServerNew(servid, isnew)
	c.Ctx.WriteString("OK")
}

type RestartServer struct {
	beego.Controller
}

func (c *RestartServer) Post() {
	password := c.GetString("password")
	servid, _ := strconv.Atoi(c.GetString("servid"))
	beego.Debug(beego.AppConfig.String("logicserversshuser"), password, servid)
	models.Restart(password, servid)

	c.Ctx.WriteString("OK")
}

type UpdateServer struct {
	beego.Controller
}

func (c *UpdateServer) Post() {
	password := c.GetString("password")
	packageName := c.GetString("package")

	servers := strings.Split(c.GetString("servid"), ",")

	for _, v := range servers {
		servid, _ := strconv.Atoi(v)

		models.BackupDBGame(servid)

		models.UpdateOnlineServer(password, packageName, servid)
	}
	c.Ctx.WriteString("OK")
}

type BackupServer struct {
	beego.Controller
}

func (c *BackupServer) Post() {
	servid, _ := strconv.Atoi(c.GetString("servid"))
	beego.Debug(servid)
	models.BackupDBGame(servid)

	c.Ctx.WriteString("OK")
}
