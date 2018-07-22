package models

import (
	"logserv/tools"
	"time"

	"github.com/astaxie/beego"
)

type (
	LoginRecord struct {
		PFID, PFName, AccountName, RoleId, LoginTime, LogoutTime, FirstTime, RoleFirstTime, MAC, IDFA, IP, DeviceType string
		FirstServerId, ServerId                                                                                       int
	}

	DAUInfo struct {
		Date   string
		ServId int
		Num    int
	}

	DNUInfo struct {
		Date   string
		ServId int
		Num    int
	}
)

func InsertLoginRecord(record *LoginRecord) {
	conn, err := tools.OpenDB()
	if err != nil {
		beego.Error(err.Error())
		return
	}

	defer conn.Close()

	if record.RoleFirstTime == "" {
		record.RoleFirstTime = "1970-01-01 00:00:00"
	}
	if record.FirstTime == "" {
		record.FirstTime = "1970-01-01 00:00:00"
	}

	_, err = conn.Exec("INSERT INTO `LoginLog`(`PFID`,`PFName`,`AccountName`,`RoleId`,`LoginTime`,`LogoutTime`,`FirstTime`,`RoleFirstTime`,`FirstServerId`,`ServerId`,`MAC`,`IDFA`, `IP`,`DeviceType`)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)", record.PFID, record.PFName, record.AccountName, record.RoleId, record.LoginTime, record.LogoutTime, record.FirstTime, record.RoleFirstTime, record.FirstServerId, record.ServerId, record.MAC, record.IDFA, record.IP, record.DeviceType)

	if err != nil {
		beego.Error(err.Error())
		return
	}
}

//统计每日活跃角色
func CalcDAU(begin, end time.Time) []DAUInfo {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError.Error())
		return nil
	}

	defer dbConn.Close()
	delta := 24 * time.Hour
	result := []DAUInfo{}
	for begin.Unix() < end.Unix() {
		res, dbError := dbConn.Query("SELECT DISTINCT `RoleId`, `ServerId` FROM `LoginLog` WHERE `LoginTime` >= ? AND `LoginTime` < ?", begin.Format("2006-01-02 15:04:05"), begin.Add(delta).Format("2006-01-02 15:04:05"))
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}

		tmpCache := map[int]int{}
		for res.Next() {
			servId := 0
			roleId := ""
			dbError = res.Scan(&roleId, &servId)
			if dbError != nil {
				beego.Error(dbError)
				return nil
			}

			tmpCache[servId] += 1
		}
		for k, v := range tmpCache {
			result = append(result, DAUInfo{begin.Format("2006-01-02"), k, v})
		}
		begin = begin.Add(delta)
	}

	return result
}

//统计每日活跃账户
func CalcDAU2(begin, end time.Time) []DAUInfo {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError.Error())
		return nil
	}

	defer dbConn.Close()
	delta := 24 * time.Hour
	result := []DAUInfo{}
	for begin.Unix() < end.Unix() {
		res, dbError := dbConn.Query("SELECT DISTINCT `AccountName`, `ServerId` FROM `LoginLog` WHERE `LoginTime` >= ? AND `LoginTime` < ?", begin.Format("2006-01-02 15:04:05"), begin.Add(delta).Format("2006-01-02 15:04:05"))
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}

		tmpCache := map[int]int{}
		for res.Next() {
			servId := 0
			userId := ""
			dbError = res.Scan(&userId, &servId)
			if dbError != nil {
				beego.Error(dbError)
				return nil
			}

			tmpCache[servId] += 1
		}
		for k, v := range tmpCache {
			result = append(result, DAUInfo{begin.Format("2006-01-02"), k, v})
		}
		begin = begin.Add(delta)
	}

	return result
}

//统计每日新增用户
func CalcDNU(begin, end time.Time) []DNUInfo {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError.Error())
		return nil
	}

	defer dbConn.Close()
	delta := 24 * time.Hour
	result := []DNUInfo{}
	for begin.Unix() < end.Unix() {
		res, dbError := dbConn.Query("SELECT DISTINCT `RoleId`, `ServerId` FROM `LoginLog` WHERE `FirstTime` >= ? AND `FirstTime` < ?", begin.Format("2006-01-02 15:04:05"), begin.Add(delta).Format("2006-01-02 15:04:05"))
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}

		tmpCache := map[int]int{}
		for res.Next() {
			servId := 0
			roleId := ""
			dbError = res.Scan(&roleId, &servId)
			if dbError != nil {
				beego.Error(dbError)
				return nil
			}

			tmpCache[servId] += 1
		}
		for k, v := range tmpCache {
			result = append(result, DNUInfo{begin.Format("2006-01-02"), k, v})
		}
		begin = begin.Add(delta)
	}

	return result
}

//统计每日新增账户
func CalcDNU2(begin, end time.Time) []DNUInfo {
	dbConn, dbError := tools.OpenDB()
	if dbError != nil {
		beego.Error(dbError.Error())
		return nil
	}

	defer dbConn.Close()
	delta := 24 * time.Hour
	result := []DNUInfo{}
	for begin.Unix() < end.Unix() {
		res, dbError := dbConn.Query("SELECT DISTINCT `AccountName`, `ServerId` FROM `LoginLog` WHERE `FirstTime` >= ? AND `FirstTime` < ?", begin.Format("2006-01-02 15:04:05"), begin.Add(delta).Format("2006-01-02 15:04:05"))
		if dbError != nil {
			beego.Error(dbError)
			return nil
		}

		tmpCache := map[int]int{}
		for res.Next() {
			servId := 0
			userId := ""
			dbError = res.Scan(&userId, &servId)
			if dbError != nil {
				beego.Error(dbError)
				return nil
			}

			tmpCache[servId] += 1
		}
		for k, v := range tmpCache {
			result = append(result, DNUInfo{begin.Format("2006-01-02"), k, v})
		}
		begin = begin.Add(delta)
	}

	return result
}
