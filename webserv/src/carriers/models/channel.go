package models

import (
	"database/sql"
	"encoding/json"
	"errors"
	"github.com/astaxie/beego"
	"github.com/astaxie/beego/toolbox"
	_ "github.com/go-sql-driver/mysql"
	"strconv"
	"strings"
	"unsafe"
	"time"
)

var Serverids = map[string][]int{}

func FindChannel() string {

	db, _ := CarriersDB()
	defer db.Close()
	dbRecords, err := db.Query("SELECT * FROM `Channel` ")
	if err != nil {
		beego.Error(err)
	}

	var cc = map[string][][]string{}
	for dbRecords.Next() {

		if err != nil {
			beego.Error("DB fail", err)
		}

		var cha string
		var cla string

		err := dbRecords.Scan(&cha, &cla)
		if err != nil {
			beego.Error(err)
		}

		channle := strings.Split(cha, "=")

		c_name := channle[0]
		c_version := channle[1]

		if c_name == "" && c_version == "" {
			deleteChannel(cha, db)
			continue
		}

		var vv = []string{c_name, c_version, cla}

		cc[c_name] = append(cc[c_name], vv)

	}
	b, _ := json.Marshal(cc)

	return string(b)
}

func DeleteChannel(channel string, version string) error {
	sqlkey := channel + "=" + version
	db, _ := CarriersDB()
	defer db.Close()
	return deleteChannel(sqlkey, db)
}

func deleteChannel(sqlkey string, db *sql.DB) error {
	_, err := db.Exec("DELETE FROM `Channel` WHERE `Channle`= ? ", sqlkey)

	if err != nil {
		beego.Error(err.Error())
		return err
	}

	delete(Serverids, sqlkey)

	return nil
}

func updateChannel(sqlkey string, sids string) {
	sers := strings.Split(sids, ",")
	var s = []int{}
	for i := 0; i < len(sers); i++ {
		i_s, _ := strconv.Atoi(sers[i])
		s = append(s, i_s)
	}
	Serverids[sqlkey] = s

}

func InitServers() error {
	db, _ := CarriersDB()
	defer db.Close()

	Sids := map[string][]int{} // 中间变量 不要在防止执行函数的时候有玩家访问
	dbRecords, err := db.Query("SELECT * FROM `Channel` ")
	if err != nil {
		beego.Error(err)
		return err
	}

	for dbRecords.Next() {
		var cha string
		var cla string

		err := dbRecords.Scan(&cha, &cla)
		if err != nil {
			beego.Error(err)
		}

		clas := strings.Split(cla, ",")
		var sids = []int{}
		for i := 0; i < len(clas); i++ {
			i_c, _ := strconv.Atoi(clas[i])
			sids = append(sids, i_c)
		}
		Sids[cha] = sids
	}

	Serverids = Sids

	return nil
}

func InsertChannel(channelname string, version string, serverids string) error {
	db, _ := CarriersDB()
	defer db.Close()

	if channelname == "" || version == "" {
		beego.Error("need value")
		return errors.New("need value")
	}

	channel := channelname + "=" + version

	query, err := db.Query("SELECT * FROM `Channel` WHERE `Channle` = ?", channel)
	if err != nil {
		beego.Error(err)
		return err
	}

	if query.Next() { // 如果有内容 就更新数据
		_, dbError := db.Exec("UPDATE Channel SET Serverid = ? WHERE Channle = ?", serverids, channel)
		if dbError != nil {
			beego.Error(dbError)
		}
	} else { // 没有这个key 就插入
		_, dbError := db.Exec("INSERT INTO `Channel`(`Channle`,`Serverid`)VALUES(?,?)", channel, serverids)

		if dbError != nil {
			beego.Error(dbError)
			return dbError
		}
	}
	updateChannel(channel, serverids)
	return nil
}

func FindChannelByProperty(SqlKey string) map[int]int {
	var serverids = map[int]int{}

	servers, ok := Serverids[SqlKey]
	if !ok {
		beego.Error("cant find", SqlKey)
	}

	for i := 0; i < len(servers); i++ {
		serverids[servers[i]] = 1
	}

	return serverids
}

func FindServerByChannel(c string) []int {
	m := map[int]bool{}

	for k, v := range Serverids {
		if strings.Index(k, c) != -1 {
			for _, i := range v {
				m[i] = true
			}
		}
	}

	r := []int{}
	for i, _ := range m {
		r = append(r, i)
	}
	return r
}

func FindServers4(username string, channel string, version string) []*ServQuery {
	servs := sc.Servs
	servj := []*ServQuery{}
	if servs == nil {
		return nil
	}
	key := channel + "=" + version
	nowtime := time.Now().Unix()
	CanLinkServers := FindChannelByProperty(key)
	for i := 0; i < len(servs); i++ {
		_, ok := CanLinkServers[servs[i].Id]
		if !ok {
			continue
		}
		servs[i].IsOnline = (nowtime - servs[i].LastUpdateTime) < 10
		servs[i].BindUser(username)
		servj = append(servj, (*ServQuery)(unsafe.Pointer(servs[i])))
	}

	return servj
}

func GetServsJson4(username string, version string, channel string) string {
	liservs := FindServers4(username, channel, version)
	b, _ := json.Marshal(liservs)

	return string(b)
}

func InitChannleByVersion() {

	InitServers()
	task := toolbox.NewTask("InitChannleByVersion", beego.AppConfig.String("updatechannel"), InitServers)
	toolbox.AddTask("InitChannleByVersion", task)
}
