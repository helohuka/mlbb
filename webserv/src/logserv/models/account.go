package models

import (
	"logserv/tools"
	"time"

	"github.com/astaxie/beego"

	_ "github.com/go-sql-driver/mysql"
)

type (
	//一条记录
	AccountRecord struct {
		//渠道ID,渠道名,账户名,创建时间,物理地址,物理地址,IP,设备类型
		PFID, PFName, AccountName, CreateTime, MAC, IDFA, IP, DeviceType string
	}

	//新增注册
	AccountRegist struct {
		DateTime string //日期
		Num      int    //新增数量
	}

	//渠道统计
	AccountChannelRegist struct {
		DateTime string //日期
		Id       string
		Name     string
		Num      int
	}

	//设备统计
	AccountDevice struct {
		Type string
		Num  int
	}

	//IDFA统计
	AccountIDFA struct {
		Key string
		Num int
	}

	AccountMac struct {
		Key string
		Num int
	}

	AccountLog struct {
		accounts map[string]bool
	}
)

var accountSet map[string]bool = map[string]bool{}

//插入数据库
func InsertAccountRecord(record *AccountRecord) {

	if accountSet[record.AccountName] {
		return
	}

	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError)
		return
	}
	defer dbConn.Close()

	_, dbError = dbConn.Exec("INSERT INTO `AccountLog`(`PFID`,`PFName`,`AccountName`,`CreateTime`,`MAC`,`IDFA`,`IP`,`DeviceType`)VALUES(?,?,?,?,?,?,?,?)", record.PFID, record.PFName, record.AccountName, record.CreateTime, record.MAC, record.IDFA, record.IP, record.DeviceType)

	if dbError != nil {
		beego.Error(dbError)
	}

	accountSet[record.AccountName] = true
}

//初始化账户日志
func InitAccountSet() {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError)
		return
	}
	defer dbConn.Close()
	records, dbError := dbConn.Query("SELECT `AccountName` FROM `AccountLog`")

	if dbError != nil {
		beego.Error(dbError.Error())
		return
	}
	name := ""
	for records.Next() {
		dbError := records.Scan(&name)
		if dbError != nil {
			beego.Error(dbError)
			continue
		}
		accountSet[name] = true
	}
}

//新增注册 开始时间 结束时间 日期为单位
func CalcAccountRegist(begin, end time.Time) []AccountRegist {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError)
		return nil
	}
	defer dbConn.Close()
	beego.Debug(begin, end)
	result := []AccountRegist{}
	delta := 24 * time.Hour
	for begin.Unix() < end.Unix() {
		res, dbError := dbConn.Query("SELECT COUNT(`AccountName`) FROM `AccountLog` WHERE `CreateTime` >= ? AND `CreateTime` < ?", begin.Format("2006-01-02 15:04:05"), begin.Add(delta).Format("2006-01-02 15:04:05"))
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}

		val := AccountRegist{}
		val.DateTime = begin.Format("2006-01-02 15:04:05")
		if res.Next() {
			dbError = res.Scan(&val.Num)
			if dbError != nil {
				beego.Error(dbError)
				return nil
			}
		}
		result = append(result, val)
		begin = begin.Add(delta)
	}

	return result
}

//渠道注册 开始时间 结束时间 日期为单位
func CalcCDNU(begin, end time.Time) []AccountChannelRegist {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError)
		return nil
	}
	defer dbConn.Close()

	result := []AccountChannelRegist{}
	delta := 24 * time.Hour
	for begin.Unix() < end.Unix() {
		res, dbError := dbConn.Query("SELECT COUNT(`AccountName`), `PFID`, `PFName` FROM `AccountLog` WHERE `CreateTime` >= ? AND `CreateTime` < ? GROUP BY `PFID`", begin.Format("2006-01-02 15:04:05"), begin.Add(delta).Format("2006-01-02 15:04:05"))
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}

		for res.Next() {
			val := AccountChannelRegist{}
			val.DateTime = begin.Format("2006-01-02 15:04:05")
			dbError = res.Scan(&val.Num, &val.Id, &val.Name)
			if dbError != nil {
				beego.Error(dbError)
				return nil
			}
			result = append(result, val)
		}

		begin = begin.Add(delta)
	}

	return result
}

//账户设备统计
func CalcDevice() []AccountDevice {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError)
		return nil
	}
	defer dbConn.Close()
	result := []AccountDevice{}
	res, dbError := dbConn.Query("SELECT COUNT(`AccountName`), `DeviceType` FROM `AccountLog` GROUP BY `DeviceType` ")

	for res.Next() {
		val := AccountDevice{}
		dbError = res.Scan(&val.Num, &val.Type)
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}
		result = append(result, val)
	}
	return result
}

//账户设备统计
func CalcMac() []AccountMac {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError)
		return nil
	}
	defer dbConn.Close()
	result := []AccountMac{}
	res, dbError := dbConn.Query("SELECT COUNT(`AccountName`), `MAC` FROM `AccountLog` GROUP BY `MAC` ")

	for res.Next() {
		val := AccountMac{}
		dbError = res.Scan(&val.Num, &val.Key)
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}
		result = append(result, val)
	}
	return result
}

//账户设备统计
func CalcIDFA() []AccountIDFA {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError)
		return nil
	}
	defer dbConn.Close()
	result := []AccountIDFA{}
	res, dbError := dbConn.Query("SELECT COUNT(`AccountName`), `IDFA` FROM `AccountLog` GROUP BY `IDFA` ")

	for res.Next() {
		val := AccountIDFA{}
		dbError = res.Scan(&val.Num, &val.Key)
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}
		result = append(result, val)
	}
	return result
}
