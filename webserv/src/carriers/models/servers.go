package models

import (
	"encoding/json"
	"fmt"
	"strconv"
	"strings"
	"time"
	"unsafe"

	"github.com/astaxie/beego"
	_ "github.com/go-sql-driver/mysql"
)

type ServData struct {
	Id      int                 `json:"id"`
	Name    string              `json:"name"`
	Version int                 `json:"version"`
	Capa    int                 `json:"capa"`
	Added   map[string][]string `json:"added"`
	Deled   map[string][]string `json:"deled"`
}

type ServCore struct {
	Id           int    `json:"id"`
	Capa         int    `json:"capa"`
	Area         int    `json:"area"`
	Name         string `json:"name"`
	ServAreaName string `json:"areaname"`
	Host         string `json:"host"`
	Port         int    `json:"port"`

	IsNew int `json:"isnew"`
}

type ServQuery struct {
	ServCore
	IsOnline bool     `json:"isonline"`
	Players  []string `json:"players"`
}

type ServInner struct {
	ServQuery
	OauthPort   int    `json:"oauthport"`
	PayPort     int    `json:"payport"`
	GMTPort     int    `json:"gmtport"`
	DBHost      string `json:"logdbhost"`
	DBPort      int    `json:"logdbport"`
	DBUsr       string `json:"logdbusr"`
	DBPwd       string `json:"logdbpwd"`
	DBGameName  string
	DBLogName   string `json:"logdbname"`
	CDN         string `json:"CDN"`
	Versions    []string
	Channels    []string
	Sandbox     int
	SSHUsername string
	SSHPassword string
	Path        string
	Source      string
}

type Serv struct {
	ServInner
	NeedRestart     bool
	NoCheck         int
	LastUpdateTime  int64
	SendMailTimeout int64
	SendMailStep    int
	Notice          string
	Accounts        map[string][]string
}

func (sv *Serv) CheckVersions(version string) bool {
	if 0 == len(sv.Versions) {
		return true
	}
	for i := 0; i < len(sv.Versions); i++ {
		if sv.Versions[i] == version {
			return true
		}
	}
	return false
}

func (sv *Serv) CheckChannels(channel string) bool {

	for i := 0; i < len(sv.Channels); i++ {
		if sv.Channels[i] == channel {
			return true
		}
	}
	return false
}

func (sv *Serv) InitAccounts() {
	db, err := GameDB(sv)

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	records, err := db.Query("SELECT `UserName`,`PlayerName`,`InDoorId` FROM `Player`")
	if err != nil {
		records, err = db.Query("SELECT `UserName`,`PlayerName` FROM `Player`")
		if err != nil {
			beego.Error(err)
			return
		}
	}
	username, playername := "", ""
	servId := 0
	for records.Next() {
		err := records.Scan(&username, &playername, &servId)
		if err != nil {
			//兼容以前版本服务器
			err = records.Scan(&username, &playername)
			if err != nil {
				beego.Error(err)
				continue
			}
			servId = sv.Id
		}

		if username == "" {
			continue
		}

		if servId == sv.Id {
			// beego.Debug(username)
			sv.AddPlayer(username, playername)
		}
	}

	beego.Info("Server ", sv.Id, " init account infomation ok")
}

func (sv *Serv) DelPlayer(acc string, pla string) {
	oldplayers := sv.Accounts[acc]
	if oldplayers == nil {
		return
	}
	newplayers := []string{}
	for i := 0; i < len(oldplayers); i++ {
		if oldplayers[i] != pla {
			newplayers = append(newplayers, oldplayers[i])
			break
		}
	}

	sv.Accounts[acc] = newplayers
}

func (sv *Serv) AddPlayer(acc string, pla string) {
	oldplayers := sv.Accounts[acc]

	if oldplayers == nil {
		sv.Accounts[acc] = []string{pla}
		return
	}

	for i := 0; i < len(oldplayers); i++ {
		if oldplayers[i] == pla {
			return
		}
	}
	sv.Accounts[acc] = append(oldplayers, pla)

}

func (sv *Serv) GetPlayers(acc string) []string {
	return sv.Accounts[acc]
}

func (sv *Serv) BindUser(acc string) {
	sv.Players = sv.Accounts[acc]
}

func (sv *Serv) UpdateNotice(notice string) {
	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	_, err = db.Exec("UPDATE Servs SET Notice = ? WHERE ServId = ?", notice, sv.Id) ///("SELECT * FROM Servs")

	if err != nil {
		beego.Error(err.Error())
		return
	}

	sv.Notice = notice
}

func (sv *Serv) GetGMTHost() string {
	return fmt.Sprintf("http://%s:%d", sv.Host, sv.GMTPort)
}

type ServsCache struct {
	Servs     []*Serv
	IdServs   map[int]*Serv
	AreaServs map[string][]*ServInner
}

func (sc *ServsCache) Init() {

	sc.Servs = []*Serv{}
	sc.IdServs = map[int]*Serv{}
	sc.AreaServs = map[string][]*ServInner{}

	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	records, err := db.Query("SELECT * FROM Servs")

	if err != nil {
		beego.Error(err.Error())
		return
	}

	for records.Next() {
		ser := Serv{}
		ser.LastUpdateTime = time.Now().Unix()
		ser.Accounts = map[string][]string{}
		var channels, versions string
		err := records.Scan(&ser.Id, &ser.Name, &ser.ServAreaName, &ser.Area, &ser.Host, &ser.Port, &ser.OauthPort, &ser.PayPort, &ser.GMTPort, &ser.DBHost, &ser.DBPort, &ser.DBUsr, &ser.DBPwd, &ser.DBGameName, &ser.DBLogName, &channels, &ser.IsNew, &ser.Notice, &ser.CDN, &versions, &ser.Sandbox, &ser.Path, &ser.NoCheck)
		if err != nil {
			beego.Error(err)
			return
		}
		ser.Channels = strings.Split(channels, ",")
		ser.Versions = strings.Split(versions, ",")
		sc.Servs = append(sc.Servs, &ser)
		sc.IdServs[ser.Id] = &ser
		if sc.AreaServs[ser.ServAreaName] == nil {
			sc.AreaServs[ser.ServAreaName] = []*ServInner{}
		}
		sc.AreaServs[ser.ServAreaName] = append(sc.AreaServs[ser.ServAreaName], (*ServInner)(unsafe.Pointer(&ser)))

		go ser.InitAccounts()
	}

}

func (sc *ServsCache) FindServs(username string) []*ServQuery {
	servs := sc.Servs
	servj := make([]*ServQuery, len(servs))
	if servs == nil {
		return nil
	}
	nowtime := time.Now().Unix()
	for i := 0; i < len(servs); i++ {
		servs[i].IsOnline = (nowtime - servs[i].LastUpdateTime) < 10
		servs[i].BindUser(username)
		servj[i] = (*ServQuery)(unsafe.Pointer(servs[i]))
	}
	return servj
}

func (sc *ServsCache) FindServs2_(version string, channel string) []*Serv {
	servs := sc.Servs
	servj := []*Serv{}
	if servs == nil {
		return nil
	}
	nowtime := time.Now().Unix()
	for i := 0; i < len(servs); i++ {
		//beego.Debug("FIND SERV ", servs[i].Versions, servs[i].Channels)
		if servs[i].CheckVersions(version) && servs[i].CheckChannels(channel) {
			servs[i].IsOnline = (nowtime - servs[i].LastUpdateTime) < 10
			servj = append(servj, servs[i])
		}
	}
	return servj
}

func (sc *ServsCache) FindServs2(username string, version string, channel string) []*ServQuery {
	servs := sc.Servs
	servj := []*ServQuery{}
	if servs == nil {
		return nil
	}
	nowtime := time.Now().Unix()
	for i := 0; i < len(servs); i++ {
		if servs[i].CheckVersions(version) && servs[i].CheckChannels(channel) {
			servs[i].IsOnline = (nowtime - servs[i].LastUpdateTime) < 30
			servs[i].BindUser(username)
			servj = append(servj, (*ServQuery)(unsafe.Pointer(servs[i])))
		}
	}
	return servj
}

func (sc *ServsCache) FindServs3(channel string) []*ServQuery {
	servs := sc.Servs
	servj := []*ServQuery{}
	if servs == nil {
		return nil
	}
	nowtime := time.Now().Unix()
	for i := 0; i < len(servs); i++ {
		if servs[i].CheckChannels(channel) {
			servs[i].IsOnline = (nowtime - servs[i].LastUpdateTime) < 30
			servj = append(servj, (*ServQuery)(unsafe.Pointer(servs[i])))
		}
	}
	return servj
}

func (sc *ServsCache) GetGroupServsJson(username string) string {
	liservs := sc.FindServs(username)
	b, _ := json.Marshal(liservs)
	return string(b)
}

var sc ServsCache

func GetServsByChannel(ch string) []*ServQuery {
	return sc.FindServs3(ch)
}

func GetServsByChannel_(version string, channel string) []*Serv {
	return sc.FindServs2_(version, channel)
}

func GetServsJson2(username string, version string, channel string) string {
	liservs := sc.FindServs2(username, version, channel)
	b, _ := json.Marshal(liservs)
	return string(b)
}

func GetServById(id int) *Serv {
	return sc.IdServs[id]
}

func UpdateServ(data *ServData) {
	for _, sv := range sc.Servs {
		if sv.Id == data.Id {

			sv.Capa = data.Capa
			sv.LastUpdateTime = time.Now().Unix()
			beego.Info(data.Id, sv.LastUpdateTime)
			for k, v := range data.Added {
				for i := 0; i < len(v); i++ {
					sv.AddPlayer(k, v[i])
				}
			}

			for k, v := range data.Deled {
				for i := 0; i < len(v); i++ {
					sv.DelPlayer(k, v[i])
				}
			}
		}
	}

}

func GetAreaServsJson() string {
	b, _ := json.Marshal(sc.AreaServs)
	return string(b)
}

func GetServOauthHostById(id int) string {
	serv := GetServById(id)
	if nil == serv {
		beego.Debug("Can not find serv", id)
		return ""
	} else {
		return "http://" + serv.Host + ":" + strconv.Itoa(serv.OauthPort)
	}
}

func GetServPayHostById(id int) string {
	serv := GetServById(id)
	if nil == serv {
		beego.Debug("Can not find serv", id)
		return ""
	} else {
		return "http://" + serv.Host + ":" + strconv.Itoa(serv.PayPort)
	}
}

func InitServs() {
	sc.Init()
}

func UpdateServerInfo(servId int, channels string, versions string, resVersion string, sandbox int) {
	sv := sc.IdServs[servId]
	if nil == sv {
		return
	}
	if channels != "" {
		sv.Channels = strings.Split(channels, ",")
	} else {
		channels = strings.Join(sv.Channels, ",")
	}
	if versions != "" {
		sv.Versions = strings.Split(versions, ",")
	} else {
		versions = strings.Join(sv.Versions, ",")
	}

	if resVersion != "" {
		sv.CDN = resVersion
	} else {
		resVersion = sv.CDN
	}

	sv.Sandbox = sandbox

	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}

	defer db.Close()

	_, err = db.Exec("UPDATE Servs SET `Channels` = ?,`Version` = ?,`CDN` = ?, `Sandbox` = ?  WHERE `ServId` = ?", channels, versions, sv.CDN, sv.Sandbox, sv.Id)
	if err != nil {
		beego.Error(err.Error())
		return
	}

	beego.Debug(fmt.Sprintf("Update server infomation %s %s %s %d %d", channels, versions, resVersion, sv.Sandbox, sv.Id))
}

func ChangeServerNew(servId, isNew int) {
	sv := sc.IdServs[servId]
	if nil == sv {
		return
	}

	if sv.IsNew == isNew {
		return
	}

	sv.IsNew = isNew

	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}

	defer db.Close()

	_, err = db.Exec("UPDATE Servs SET `IsNewServ` = ? WHERE `ServId` = ?", isNew, servId)
	if err != nil {
		beego.Error(err.Error())
		return
	}

	beego.Debug(fmt.Sprintf("Change new server %d , %d", servId, isNew))
}

func CreateServer(sv *Serv) {
	if nil != sc.IdServs[sv.Id] {
		beego.Debug(" Server is already defined !!!")
		return
	}

	sc.Servs = append(sc.Servs, sv)
	sc.IdServs[sv.Id] = sv
	if nil == sc.AreaServs[sv.ServAreaName] {
		sc.AreaServs[sv.ServAreaName] = []*ServInner{(*ServInner)(unsafe.Pointer(sv))}
	} else {
		sc.AreaServs[sv.ServAreaName] = append(sc.AreaServs[sv.ServAreaName], (*ServInner)(unsafe.Pointer(sv)))
	}

	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}

	defer db.Close()

	_, err = db.Exec("INSERT INTO `Servs`(`ServId`,`ServName`,`ServAreaName`,`ServArea`,`ServHost`,`ServPort`,`OauthPort`,`PayPort`,`GMTPort`,`DBHost`,`DBPort`,`DBUsr`,`DBPwd`,`DBGameName`,`DBLogName`,`Channels`,`IsNewServ`,`Notice`,`CDN`,`Version`, `Sandbox`, `Path`,`NoCheck`)VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)", sv.Id, sv.Name, sv.ServAreaName, sv.Area, sv.Host, sv.Port, sv.OauthPort, sv.PayPort, sv.GMTPort, sv.DBHost, sv.DBPort, sv.DBUsr, sv.DBPwd, sv.DBGameName, sv.DBLogName, strings.Join(sv.Channels, ","), sv.IsNew, sv.Notice, sv.CDN, strings.Join(sv.Versions, ","), sv.Sandbox, sv.Path, sv.NoCheck)

	if err != nil {
		beego.Error(err.Error())
		return
	}

}

func InsertOrderLog(value string) {

	db, err := CarriersDB()

	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer db.Close()

	_, err = db.Exec("INSERT INTO `Order`(JsonValue)VALUES(?)", value)

	if err != nil {
		beego.Error(err.Error())
	}
}

func CheckSandbox(svId int) bool {
	sv := sc.IdServs[svId]
	if sv != nil {
		return sv.Sandbox != 0
	} else {
		return false
	}
}

func GetServers() []*Serv {
	return sc.Servs
}
