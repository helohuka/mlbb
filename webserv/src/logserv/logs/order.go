package logs

import (
	"logserv/tools"
	"fmt"
	"strings"

	"github.com/astaxie/beego"
)

type Order struct {
	PFID, PFName, OrderId, RoleId, AccountName, PayTime string
	ServerId, RoleLevel, Payment                        int
}

func InsertOrderRecord(info Order) {

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer conn.Close()
	_, err = conn.Exec("INSERT INTO `OrderLog`(`PFID`,`PFName`,`OrderId`,`ServerId`,`RoleId`,`RoleLevel`,`AccountName`,`Payment`,`PayTime`)VALUES(?,?,?,?,?,?,?,?,?)", info.PFID, info.PFName, info.OrderId, info.ServerId, info.RoleId, info.RoleLevel, info.AccountName, info.Payment, info.PayTime)

	if err != nil {
		beego.Error(err.Error())
		return
	}
}

func DumpOrderLog(begin, end string) string {

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return "//Dump order log error " + err.Error()
	}
	defer conn.Close()
	records, err := conn.Query("SELECT `PFID`,`PFName`,`OrderId`,`ServerId`,`RoleId`,`RoleLevel`,`AccountName`,`Payment`,`PayTime` FROM `OrderLog` WHERE `PayTime` >= '" + begin + "' AND `PayTime` < '" + end + "';")

	if err != nil {
		beego.Error(err.Error())
		return "//Dump order log error " + err.Error()
	}

	sqlCode := "/*" + begin + "~" + end + "*/\n"
	info := Order{}
	for records.Next() {
		err := records.Scan(&info.PFID, &info.PFName, &info.OrderId, &info.ServerId, &info.RoleId, &info.RoleLevel, &info.AccountName, &info.Payment, &info.PayTime)
		if err != nil {
			beego.Error(err.Error())
			continue
		}
		info.PFID, info.PFName = tools.CareSDKInfo(info.PFID, info.PFName)

		if info.OrderId[:3] == "170" {
			info.PFID = "10445"
			info.PFName = "WEB"
		}else if info.OrderId[:1] == "M"{
			continue
		}
		sqlCode += strings.Replace("INSERT INTO `order`(`game`,`pfid`, `pfname`, `orderid`, `roleid`, `serverid`, `rolelv`,`accountid`, `payment`, `paytime`) VALUES"+fmt.Sprintf("('%s','%s','%s','%s', '%s', %d, %d, '%s', %d,'%s');", beego.AppConfig.String("gamename"), info.PFID, info.PFName, info.OrderId, info.RoleId, info.ServerId, info.RoleLevel, info.AccountName, info.Payment, info.PayTime), "\n", "", 0) + "\n"
	}
	return sqlCode
}
