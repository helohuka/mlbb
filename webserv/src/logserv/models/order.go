package models

import (
	"logserv/tools"

	"github.com/astaxie/beego"
	"time"
)

type (
	OrderRecord struct {
		PFID, PFName, OrderId, RoleId, AccountName, PayTime string
		ServerId, RoleLevel, Payment                        int
	}

	RRSInfo struct {
		PFID, PFName, AccountName, RoleId string
		ServerId, Payment, RoleLevel      int
	}
)

func InsertOrderRecord(record *OrderRecord) {
	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}

	defer conn.Close()
	record.PayTime = time.Now().Format("2006-01-02 15:04:05")
	_, err = conn.Exec("INSERT INTO `OrderLog`(`PFID`,`PFName`,`OrderId`,`ServerId`,`RoleId`,`RoleLevel`,`AccountName`,`Payment`,`PayTime`)VALUES(?,?,?,?,?,?,?,?,?)", record.PFID, record.PFName, record.OrderId, record.ServerId, record.RoleId, record.RoleLevel, record.AccountName, record.Payment, record.PayTime)

	if err != nil {
		beego.Error(err.Error())
		return
	}
}

//角色充值统计
func CalcRRS() []RRSInfo {
	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return nil
	}
	defer conn.Close()

	records, err := conn.Query("SELECT `PFID`,`PFName`,`AccountName`,RoleId,`ServerId`,Sum(`Payment`),Max(`RoleLevel`) FROM `OrderLog` GROUP BY `RoleId` ORDER BY SUM(`Payment`) DESC")

	if err != nil {
		beego.Error(err.Error())
		return nil
	}

	result := []RRSInfo{}
	value := RRSInfo{}
	for records.Next() {
		err := records.Scan(&value.PFID, &value.PFName, &value.AccountName, &value.RoleId, &value.ServerId, &value.Payment, &value.RoleLevel)
		if err != nil {
			beego.Error(err.Error())
			continue
		}
		result = append(result, value)
	}

	return result
}
