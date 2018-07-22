package controllers

import (
	"database/sql"
	_ "encoding/json"
	"errors"
	_ "github.com/astaxie/beego"
	_ "github.com/go-sql-driver/mysql"
	"gmtools/models"
	"strconv"
	_ "strings"
	_ "time"
	_ "unsafe"
)

func Database1(severid int) (*sql.DB, error) {
	sc := models.GetServById(severid)
	if sc != nil {
		dsn := sc.LogDBUsr + ":" + sc.LogDBPwd + "@tcp(" + sc.LogDBHost + ":" + strconv.Itoa(sc.LogDBPort) + ")/" + sc.LogDBName

		return sql.Open("mysql", dsn)
	}
	return nil, errors.New("Can not find serv")
}
