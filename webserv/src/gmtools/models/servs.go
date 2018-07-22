package models

import (
	"database/sql"
	"encoding/json"
	"errors"

	"strconv"

	"github.com/astaxie/beego"
)

type ServInner struct {
	///ID
	Id int `json:"id"`
	///当前容量
	Capa int `json:"capa"`
	///大区
	Area int `json:"area"`
	///名
	Name string `json:"name"`
	///大区名
	IsOnline   bool   `json:"isonline"`
	AreaName   string `json:"areaname"`
	Host       string `json:"host"`
	Port       int    `json:"port"`
	OauthPort  int    `json:"oauthport"`
	PayPort    int    `json:"payport"`
	GMTPort    int    `json:"gmtport"`
	LogDBHost  string `json:"logdbhost"`
	LogDBPort  int    `json:"logdbport"`
	LogDBUsr   string `json:"logdbusr"`
	LogDBPwd   string `json:"logdbpwd"`
	LogDBName  string `json:"logdbname"`
	ResVersion string `json:"CDN"`
	Versions   []string
	Channels   []string
	Sandbox    int
	Path       string
	DBGameName string
	NoCheck    int
}

type ServManager struct {
	AreaMap map[string][]ServInner
	IdMap   map[int]*ServInner
	Servs   []*ServInner
}

var sm ServManager

func UpdateServs(m map[string][]ServInner) {
	sm.AreaMap = m
	sm.Servs = []*ServInner{}
	sm.IdMap = map[int]*ServInner{}

	for _, ss := range m {
		for i := 0; i < len(ss); i++ {
			sm.Servs = append(sm.Servs, &ss[i])

			if sm.IdMap[ss[i].Id] != nil {
				beego.Debug("Id same error!!!", ss[i].Id)
			}

			sm.IdMap[ss[i].Id] = &ss[i]
		}
	}
}

func UpdateServsByJson(jsonstr string) {

	m := map[string][]ServInner{}
	json.Unmarshal([]byte(jsonstr), &m)
	UpdateServs(m)
}

func GetServById(id int) *ServInner {
	return sm.IdMap[id]
}

func GetGMTServHost(id int) string {
	sc := GetServById(id)
	if sc != nil {
		host := "http://" + sc.Host + ":" + strconv.Itoa(sc.GMTPort)
		return host
	}
	return ""
}
func GetGMTServName(id int) string {
	sc := GetServById(id)
	if sc != nil {
		name := sc.Name
		return name
	}
	return ""
}
func GetServLogMysql(id int) (*sql.DB, error) {

	sc := GetServById(id)
	if sc != nil {
		dsn := sc.LogDBUsr + ":" + sc.LogDBPwd + "@tcp(" + sc.LogDBHost + ":" + strconv.Itoa(sc.LogDBPort) + ")/" + sc.LogDBName
		beego.Debug(dsn)
		return sql.Open("mysql", dsn)
	}

	return nil, errors.New("Can not find serv")
}

func GetServerMap() *map[string][]ServInner {
	return &sm.AreaMap
}
