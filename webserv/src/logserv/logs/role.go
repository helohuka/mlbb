package logs

import (
	"fmt"
	"strings"

	"github.com/astaxie/beego"
	"logserv/tools"
)

type Role struct {
	PFID, PFName, RoleId, AccountName, CacheDate, ServerName, RoleFirstDate, RoleLastDate string
	FirstServerId, ServerId, RoleLevel, Gold, Diamond, Vip, CE                            int
}

func InsertRoleRecord(info Role) {

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}

	defer conn.Close()

	_, err = conn.Exec("INSERT INTO RoleLog(`PFID`,`PFName`,`RoleId`,`AccountName` ,`CacheDate`,`ServerId`,`ServerName`,`FirstServerId`,`RoleFirstDate`,`RoleLastDate`,`RoleLevel`,`Gold`,`Diamond`,`Vip`,`CE`) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", info.PFID, info.PFName, info.RoleId, info.AccountName, info.CacheDate, info.ServerId, info.ServerName, info.FirstServerId, info.RoleFirstDate, info.RoleLastDate, info.RoleLevel, info.Gold, info.Diamond, info.Vip, info.CE)

	if err != nil {
		beego.Error(err.Error())
		return
	}
}

func DumpRoleLog(date string) string {

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return "//Dump role log error " + err.Error()
	}
	defer conn.Close()
	records, err := conn.Query("SELECT `PFID`,`PFName`,`RoleId`,`AccountName`,`CacheDate`,`ServerId`,`ServerName`,`FirstServerId`,`RoleFirstDate`,`RoleLastDate`,`RoleLevel`,`Gold`,`Diamond`,`Vip`,`CE` FROM `RoleLog` WHERE `CacheDate` = ?", date)

	if err != nil {
		beego.Error(err.Error())
		return "//Dump role log error " + err.Error()
	}

	sqlCode := "/*" + date + "*/\n"
	info := Role{}
	for records.Next() {
		err := records.Scan(&info.PFID, &info.PFName, &info.RoleId, &info.AccountName, &info.CacheDate, &info.ServerId, &info.ServerName, &info.FirstServerId, &info.RoleFirstDate, &info.RoleLastDate, &info.RoleLevel, &info.Gold, &info.Diamond, &info.Vip, &info.CE)
		if err != nil {
			beego.Error(err.Error())
			continue
		}
		info.PFID, info.PFName = tools.CareSDKInfo(info.PFID, info.PFName)

		sqlCode += strings.Replace("INSERT INTO `role`(`game`, `pfid`,`pfname` ,`roleid` ,`cachedate`,`accountid`,`serverid` ,`servername`,`firstserid`,`rolefirstdate`,`rolelastdate` ,`rolelv` ,`gold` ,`diamond`,`vip` ,`ce`) VALUES"+fmt.Sprintf("('%s','%s','%s','%s','%s','%s',%d,'%s',%d,'%s','%s',%d,%d,%d,%d,%d);", beego.AppConfig.String("gamename"), info.PFID, info.PFName, info.RoleId, info.CacheDate, info.AccountName, info.ServerId, info.ServerName, info.FirstServerId, info.RoleFirstDate, info.RoleLastDate, info.RoleLevel, info.Gold, info.Diamond, info.Vip, info.CE), "\n", "", 0) + "\n"
	}
	return sqlCode
}
