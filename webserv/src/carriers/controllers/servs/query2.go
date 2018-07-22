package servs

import (
	"carriers/models"
	_ "encoding/json"
	"github.com/astaxie/beego"
)

type QueryRecord struct {
	UserName, PFID, Version, Servs, Time, IP string
}

func InsertQueryRecord(record *QueryRecord) {
	dbConn, dbError := models.CarriersDB()
	if dbError != nil {
		beego.Error(dbError.Error())
		return
	}

	defer dbConn.Close()

	_, dbError = dbConn.Exec("INSERT INTO `QueryLog`(`UserName`,`PFID`,`Version`,`Servs`, `Time`,`IP`)VALUES(?,?,?,?,?,?)", record.UserName, record.PFID, record.Version, record.Servs, record.Time, record.IP)

	if dbError != nil {
		beego.Error(dbError)
	}
}

type Query2 struct {
	beego.Controller
}

func (c *Query2) Post() {
	c.Ctx.Request.ParseForm()
	username := c.Input().Get("username")
	version := c.Input().Get("version")
	channel := c.Input().Get("channel")

	beego.Info("Client query serverlist ", username, "|", version, "|", channel)

	s := models.GetServsJson2(username, version, channel)

	c.Ctx.WriteString(s)

	//record := QueryRecord{username, channel, version, s, time.Now().Format("2006-01-02 15:04:05"), c.Ctx.Request.RemoteAddr}

	//go InsertQueryRecord(&record)
}
