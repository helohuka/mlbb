package models

import (
	"database/sql"
	"github.com/astaxie/beego"
	_"github.com/go-sql-driver/mysql"
)

func DataBase() (*sql.DB) {
	dsn :=  "xysk:123456@tcp(" + beego.AppConfig.String("center") + ":3306)/carriers";
	db ,err := sql.Open("mysql", dsn)
	if err != nil{
		beego.Error(err)
		return nil
	}
	return  db
}
