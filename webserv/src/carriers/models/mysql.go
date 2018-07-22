package models

import (
	"database/sql"
	"strconv"

	"github.com/astaxie/beego"
)

func CarriersDB() (*sql.DB, error) {
	dsn := beego.AppConfig.String("dbuser") + ":" + beego.AppConfig.String("dbpass") + "@tcp(" + beego.AppConfig.String("dbhost") + ":" + beego.AppConfig.String("dbport") + ")/" + beego.AppConfig.String("dbname")
	return sql.Open("mysql", dsn)
}

func LogDB(sv *Serv) (*sql.DB, error) {
	//dsn := mysql.Config{}
	//dsn.User = sv.LogDBUsr
	//dsn.Passwd = sv.LogDBPwd
	//dsn.Net = "tcp"
	//dsn.Addr = sv.LogDBHost + ":" + strconv.FormatInt(int64(sv.LogDBPort), 10)
	//dsn.DBName = sv.LogDBName
	//dsn.Timeout = time.Millisecond * 500
	dsn := sv.DBUsr + ":" + sv.DBPwd + "@tcp(" + sv.DBHost + ":" + strconv.FormatInt(int64(sv.DBPort), 10) + ")/" + sv.DBLogName
	//beego.Debug(dsn.FormatDSN())

	return sql.Open("mysql", dsn)
}

func GameDB(sv *Serv) (*sql.DB, error) {
	//dsn := mysql.Config{}
	//dsn.User = sv.LogDBUsr
	//dsn.Passwd = sv.LogDBPwd
	//dsn.Net = "tcp"
	//dsn.Addr = sv.LogDBHost + ":" + strconv.FormatInt(int64(sv.LogDBPort), 10)
	//dsn.DBName = sv.LogDBName
	//dsn.Timeout = time.Millisecond * 500
	dsn := sv.DBUsr + ":" + sv.DBPwd + "@tcp(" + sv.DBHost + ":" + strconv.FormatInt(int64(sv.DBPort), 10) + ")/" + sv.DBGameName
	//beego.Debug(dsn.FormatDSN())

	return sql.Open("mysql", dsn)
}
