package logs

import (
	"logserv/tools"
	"fmt"
	"strings"

	"github.com/astaxie/beego"
)

type Login struct {
	PFID, PFName, AccountName, RoleId, LoginTime, LogoutTime, FirstTime, RoleFirstTime, MAC, IDFA, IP, DeviceType string
	FirstServerId, ServerId                                                                                       int
}

func InsertLoginRecord(info Login) {
	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer conn.Close()
	_, err = conn.Exec("INSERT INTO `LoginLog`(`PFID`,`PFName`,`AccountName`,`RoleId`,`LoginTime`,`LogoutTime`,`FirstTime`,`RoleFirstTime`,`FirstServerId`,`ServerId`,`MAC`,`IDFA`, `IP`,`DeviceType`)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)", info.PFID, info.PFName, info.AccountName, info.RoleId, info.LoginTime, info.LogoutTime, info.FirstTime, info.RoleFirstTime, info.FirstServerId, info.ServerId, info.MAC, info.IDFA, info.IP, info.DeviceType)

	if err != nil {
		beego.Error(err.Error())
		return
	}
}

func DumpLoginLog(begin, end string) string {

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return "//Dump login log error " + err.Error()
	}
	defer conn.Close()
	records, err := conn.Query("SELECT `PFID`,`PFName`,`AccountName`,`RoleId`,`LoginTime`,`LogoutTime`,`FirstTime`,`RoleFirstTime`,`FirstServerId`,`ServerId`,`MAC`,`IDFA`,`IP`,`DeviceType` FROM `LoginLog` WHERE `LoginTime` >= '" + begin + "' AND `LoginTime` < '" + end + "';")

	if err != nil {
		beego.Error(err.Error())
		return "//Dump login log error " + err.Error()
	}

	sqlCode := "/*" + begin + "~" + end + "*/\n"
	info := Login{}
	for records.Next() {
		err := records.Scan(&info.PFID, &info.PFName, &info.AccountName, &info.RoleId, &info.LoginTime, &info.LogoutTime, &info.FirstTime, &info.RoleFirstTime, &info.FirstServerId, &info.ServerId, &info.MAC, &info.IDFA, &info.IP, &info.DeviceType)
		if err != nil {
			beego.Error(err.Error())
			continue
		}
		info.PFID, info.PFName = tools.CareSDKInfo(info.PFID, info.PFName)

		sqlCode += strings.Replace("INSERT INTO `login`(`game`,`pfid`,`roleid`,`pfname`,`accountid`,`logintime`,`logouttime`,`firsttime`,`rolefirsttime`,`firstserid`,`serverid`,`mac`,`idfa`,`ip`,`devicetype`) VALUES"+fmt.Sprintf("('%s','%s','%s','%s','%s','%s','%s','%s','%s',%d,%d,'%s','%s','%s','%s');", beego.AppConfig.String("gamename"), info.PFID, info.RoleId, info.PFName, info.AccountName, info.LoginTime, info.LogoutTime, info.FirstTime, info.RoleFirstTime, info.ServerId, info.ServerId, info.MAC, info.IDFA, info.IP, info.DeviceType), "\n", "", 0) + "\n"
	}
	return sqlCode
}
