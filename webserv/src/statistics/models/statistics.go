package models

import (
	_ "github.com/astaxie/beego"
	"github.com/astaxie/beego"
)

func Statistics(js string) error {
	db, err := DB()
	defer db.Close()

	if err != nil {
		beego.Error(err)
		return err
	}

	_, dbError := db.Exec("INSERT INTO `Statistics` (`info`)VALUES(?)", js)

	if dbError != nil {
		beego.Error(dbError)
		return dbError
	}

	return nil
}
