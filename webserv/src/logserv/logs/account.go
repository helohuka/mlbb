package logs

import (
	"logserv/tools"
	"fmt"
	"strings"

	"github.com/astaxie/beego"
)

//game`,`pfid`, `pfname`, `accountid`, `createtime`, `mac`, `idfa`, `ip`, `devicetype
type (
	Account struct {
		PFID, PFName, AccountName, CreateTime, MAC, IDFA, IP, DeviceType string
	}
)

var accountNames map[string]int = map[string]int{}

func InitAccountLog() {
	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer conn.Close()
	records, err := conn.Query("SELECT AccountName FROM AccountLog")

	if err != nil {
		beego.Error(err.Error())
		return
	}
	name := ""
	for records.Next() {
		err := records.Scan(&name)
		if err != nil {
			beego.Error(err)
			continue
		}
		accountNames[name] = 1
	}

	beego.Info("Init account log ok")
}

func InsertAccountRecord(info Account) {

	if accountNames[info.AccountName] == 1 {
		return
	}

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer conn.Close()
	_, err = conn.Exec("INSERT INTO `AccountLog`(`PFID`,`PFName`,`AccountName`,`CreateTime`,`MAC`,`IDFA`,`IP`,`DeviceType`)VALUES(?,?,?,?,?,?,?,?)", info.PFID, info.PFName, info.AccountName, info.CreateTime, info.MAC, info.IDFA, info.IP, info.DeviceType)

	if err != nil {
		beego.Error(err.Error())
		return
	}

	accountNames[info.AccountName] = 1
}

func DumpAccountLog(begin, end string) string {

	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return "//Dump account log error " + err.Error()
	}
	defer conn.Close()
	records, err := conn.Query("SELECT `PFID`, `PFName`, `AccountName`, `CreateTime`, `MAC`, `IDFA`, `IP`, `DeviceType` FROM `AccountLog` WHERE `CreateTime` >= '" + begin + "' AND `CreateTime` < '" + end + "';")

	if err != nil {
		beego.Error(err.Error())
		return "//Dump account log error " + err.Error()
	}

	sqlCode := "/*" + begin + "~" + end + "*/\n"
	info := Account{}
	for records.Next() {
		err := records.Scan(&info.PFID, &info.PFName, &info.AccountName, &info.CreateTime, &info.MAC, &info.IDFA, &info.IP, &info.DeviceType)
		if err != nil {
			beego.Error(err.Error())
			continue
		}
		info.PFID, info.PFName = tools.CareSDKInfo(info.PFID, info.PFName)

		sqlCode += strings.Replace("INSERT INTO `account`(`game`,`pfid`, `pfname`, `accountid`, `createtime`, `mac`, `idfa`, `ip`, `devicetype`) VALUES"+fmt.Sprintf("('%s','%s','%s','%s','%s','%s','%s','%s','%s');", beego.AppConfig.String("gamename"), info.PFID, info.PFName, info.AccountName, info.CreateTime, info.MAC, info.IDFA, info.IP, info.DeviceType), "\n", "", 0) + "\n"
	}
	return sqlCode
}
