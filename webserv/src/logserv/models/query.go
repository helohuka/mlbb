package models

import (
	"logserv/tools"

	"github.com/astaxie/beego"
)

type QueryRecord struct {
	UserName, PFID, Version, ResVersion, Time, IP string
}

func InsertQueryRecord(record *QueryRecord) {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError.Error())
		return
	}

	defer dbConn.Close()

	_, dbError = dbConn.Exec("INSERT INTO `QueryLog`(`AccountName`,`ChannelId`,`Version`,`Reversion`, `Time`,`IP`)VALUES(?,?,?,?,?,?)", record.UserName, record.PFID, record.Version, record.ResVersion, record.Time, record.IP)

	if dbError != nil {
		beego.Error(dbError)
	}
}
