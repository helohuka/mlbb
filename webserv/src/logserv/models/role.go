package models

import (
	"logserv/tools"

	"github.com/astaxie/beego"
)

type RoleRecord struct {
	PFID, PFName, RoleId, AccountName, CacheDate, ServerName, RoleFirstDate, RoleLastDate string
	FirstServerId, ServerId, RoleLevel, Gold, Diamond, Vip, CE                            int
}

func InsertRoleRecords(records *[]RoleRecord) {

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}

	defer conn.Close()
	for _, record := range *records {
		_, err = conn.Exec("INSERT INTO RoleLog(`PFID`,`PFName`,`RoleId`,`AccountName` ,`CacheDate`,`ServerId`,`ServerName`,`FirstServerId`,`RoleFirstDate`,`RoleLastDate`,`RoleLevel`,`Gold`,`Diamond`,`Vip`,`CE`) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", record.PFID, record.PFName, record.RoleId, record.AccountName, record.CacheDate, record.ServerId, record.ServerName, record.FirstServerId, record.RoleFirstDate, record.RoleLastDate, record.RoleLevel, record.Gold, record.Diamond, record.Vip, record.CE)
	}
	if err != nil {
		beego.Error(err.Error())
		return
	}
}
