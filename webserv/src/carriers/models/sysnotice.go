package models

import (
	"strings"

	"github.com/astaxie/beego"
)

var noticis map[string]string = map[string]string{}

func InitSystemNotice() {
	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()
	records, err := db.Query("SELECT * FROM `SystemNotice`")

	if err != nil {
		beego.Error(err.Error())
		return
	}

	channel, content := "", ""
	for records.Next() {
		err := records.Scan(&channel, &content)
		if err != nil {
			beego.Error(err.Error())
			continue
		}
		noticis[channel] = content
	}

	beego.Debug("Init system notice ok")
}

func UpdateSystemNotice(channel string, content string) {

	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	channels := strings.Split(channel, ",")

	for _, ch := range channels {
		noticis[ch] = content

		_, err = db.Exec("REPLACE INTO `SystemNotice`(`Channel`,`Notice`)VALUES(?,?)", ch, content)

		if err != nil {
			beego.Error(err.Error())
		}
	}

}

func GetSystemNotice(channel string) string {
	if val, ok := noticis[channel]; ok {
		return val
	}
	return ""
}
