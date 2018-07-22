package models

import (
	"database/sql"
	"github.com/astaxie/beego"
)

func CarriersDB() (*sql.DB) {
	dsn :=  "xysk:123456@tcp(" + beego.AppConfig.String("center") + ":3306)/carriers";
	println(dsn)
	db ,err := sql.Open("mysql", dsn)
	if err != nil{
		beego.Error(err)
		return nil
	}
	println("dbis: ", db)
	return  db
}
