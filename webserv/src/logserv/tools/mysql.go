package tools

import (
	"database/sql"

	"github.com/astaxie/beego"
	"github.com/jlaffaye/ftp"
)

func OpenDB() (*sql.DB, error) {
	ret, err := sql.Open("mysql", beego.AppConfig.String("dsn"))
	if err != nil {
		return nil, err
	}
	return ret, nil
}

func LogFTP() *ftp.ServerConn {
	serv, err := ftp.Connect(beego.AppConfig.String("ftphost"))
	if err != nil {
		beego.Error(err.Error())
		return nil
	}
	err = serv.Login(beego.AppConfig.String("ftpuser"), beego.AppConfig.String("ftppwd"))
	if err != nil {
		beego.Error(err.Error())
		return nil
	}
	return serv
}