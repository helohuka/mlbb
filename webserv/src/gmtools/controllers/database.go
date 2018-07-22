package controllers

import (
	"database/sql"
	_ "encoding/json"
	"github.com/astaxie/beego"
	_ "github.com/go-sql-driver/mysql"
	_ "strconv"
	_ "strings"
	_ "time"
	_ "unsafe"
)

func Database() (*sql.DB, error) {
	dsn := beego.AppConfig.String("dbuser") + ":" + beego.AppConfig.String("dbpass") + "@tcp(" + beego.AppConfig.String("dbhost") + ":" + beego.AppConfig.String("dbport") + ")/" + beego.AppConfig.String("dbname")
	return sql.Open("mysql", dsn)
}

///公告相关记录

//系统公告添加
func SystemNoticeInsert(name string, title string, content string, color string, time string) {
	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()
	_, err = db.Exec("INSERT INTO `notice`(types,title,content,color,time) VALUES(?,?,?,?,?)", name, title, content, color, time)

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
}

//滚动公告
func RollNoticeInsert(sendType string, content string, timestr string, time string) {
	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()
	_, err = db.Exec("INSERT INTO `roll`(types,sendtype,content,timestr,time) VALUES(?,?,?,?,?)", "3", sendType, content, timestr, time)

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
}

//历史邮件
func InsertMailInsert(Title string, Sender string, Content string, recvers string, stritemids string, stritemsks string, LowLevel int, HighLevel int, time0 string, time1 string, SendType string, time string) {
	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()
	_, err = db.Exec("INSERT INTO `mail`(title,sender,content,recvers,stritemids,stritemsks,lowlevel,highlevel,time0,time1,sendtype,time) VALUES(?,?,?,?,?,?,?,?,?,?,?,?)", Title, Sender, Content, recvers, stritemids, stritemsks, LowLevel, HighLevel, time0, time1, SendType, time)

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
}

//活动
func LoginActivityInsert(Name string, OpenTime string, CloseTime string, time string) {
	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()
	_, err = db.Exec("INSERT INTO `login`(name,opentime,closetime,time) VALUES(?,?,?,?)", Name, OpenTime, CloseTime, time)

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
}

//模拟充值
func RecordInsert(playerid int, ShopId int, Payment int, orderid string, time string) {
	db, err := Database()

	if err != nil {
		beego.Error("DB fail", err)
		return
	}
	defer db.Close()
	_, err = db.Exec("INSERT INTO `record`(name,shopid,orderid,payment,roleid,time) VALUES(?,?,?,?,?,?)", "模拟充值", ShopId, orderid, Payment, playerid, time)
	if err != nil {
		beego.Error("DB fail", err)
		return
	}
}
