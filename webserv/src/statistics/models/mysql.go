package models

import "database/sql"
import "github.com/astaxie/beego"

func DB() (*sql.DB, error) {
	dsn := beego.AppConfig.String("dbuser") + ":" + beego.AppConfig.String("dbpass") + "@tcp(" + beego.AppConfig.String("dbhost") + ":" + beego.AppConfig.String("dbport") + ")/" + beego.AppConfig.String("dbname")
	return sql.Open("mysql", dsn)
}
