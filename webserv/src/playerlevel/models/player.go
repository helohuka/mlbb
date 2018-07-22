package models

import (
	"strconv"
	"database/sql"
	"github.com/astaxie/beego"
	_"github.com/astaxie/beego/toolbox"
	_"encoding/json"
	"encoding/json"
	"github.com/astaxie/beego/httplib"
	"github.com/astaxie/beego/toolbox"
	"strings"
	"fmt"
)

const (
	kSender = "运营组"
	kTitle = "应用宝预约礼包"
	kContent = "亲爱的玩家：\n    感谢您参与应用宝预约活动并下载了我们的游戏，我们将向您发放应用宝预约礼包。\n    礼包内容包括：\n        钻石*150 \n        生命储存*20000\n        魔力储存*20000 \n        金币*15000"
	kLowLevel = 1
	kHighLevel = 99
)

//邮件标题：应用宝预约礼包
//邮件内容：
//钻石*150
//生命储存*20000
//魔力储存*20000
//金币*15000


var mailItems []Item = []Item{Item{5526, 1},Item{5043, 3},Item{5100,2},Item{5101, 2}}

type (
	Player struct {
		UserName  string
		PlayerName string
		ServerId    int
		PlayerId    int
		PlayerLevel int
	}

	Server struct{
		ServerId 	int
		ServerName 	string
		LogicHost 	string
		LogicPort  	int //gmt 端口
		DBHost 		string
		DBPort		int
		DBUser		string
		DBPass		string
		DBName		string
	}

	Item struct {
		ItemId , ItemStack int
	}

	Mail struct {
		Type           string
		InsertMailType string
		Sender         string
		Title          string
		Content        string
		Recvers        []int
		Items          []Item
	}
)

var servers = map[int]*Server{}

func (this *Server) GameDB()*sql.DB{
	dsn := this.DBUser + ":" + this.DBPass + "@tcp(" + this.DBHost + ":" + strconv.FormatInt(int64(this.DBPort), 10) + ")/" + this.DBName
	db, err := sql.Open("mysql", dsn)
	if err != nil {
		beego.Error(err)
		return  nil
	}
	return  db
}

func (this* Server) GMTHost() string{

	return ""
}


func FindAllPlayerFromMysql(db *sql.DB) map[string]*Player {

	player := map[string]*Player{}
	dbRecords, err := db.Query("SELECT `ServId`,`ServName`,`ServHost`,`GMTPort`,`DBHost`,`DBPort`,`DBUsr`,`DBPwd`,`DBGameName` FROM Servs ")
	if err != nil{
		beego.Error(err)
		return player
	}


	for dbRecords.Next() {
		ser := Server{}
		err := dbRecords.Scan(&ser.ServerId, &ser.ServerName, &ser.LogicHost, &ser.LogicPort,&ser.DBHost,&ser.DBPort,&ser.DBUser,&ser.DBPass,&ser.DBName)
		if err != nil {
			beego.Error(err)
			continue
		}
		servers[ser.ServerId] = &ser
		sdb := ser.GameDB()
		defer sdb.Close()
		dbcs, err := sdb.Query("SELECT `PlayerGuid`, `UserName`, `PlayerName`,`PlayerLevel` FROM `Player`")
		if err != nil {
			beego.Error("mysql query err, serverid is ", ser.ServerId, err)
			continue
		}

		for dbcs.Next() {
			pla := Player{ServerId:ser.ServerId}

			err = dbcs.Scan(&pla.PlayerId, &pla.UserName, &pla.PlayerName, &pla.PlayerLevel)
			if oldplayer, ok := player[pla.UserName] ; ok{
				if pla.PlayerLevel > oldplayer.PlayerLevel {		//如果有相同Username的,需要保留等级高的
					player[pla.UserName] = &pla
				}
			} else {
				player[pla.UserName] = &pla
			}
		}

		beego.Debug("FindAllPlayerFromMysql: ", ser.ServerId)

	}
	return player
}

func Doing(){
	db := DataBase()
	defer  db.Close()
	allPlayers := FindAllPlayerFromMysql(db);

	for k, v := range allPlayers{
		kStrings :=strings.Split(k,"=")
		accName := kStrings[0]
		if len(kStrings) >= 2{
			accName = strings.Join(kStrings[1:],"")
		}

		TrySendMail(accName,*v, db)
	}

}

func TrySendMail(accName string, player Player,db*sql.DB){
	dbRecords, err := db.Query("SELECT `SortId` FROM `OpenId` WHERE `Account` = ?",accName)
	if err != nil{
		beego.Error(err)
		return
	}
	if dbRecords.Next() {
		PostMail(accName, &player)
		_, err = db.Exec("DELETE FROM `OpenId` WHERE `Account` = ? ", accName)

		if err != nil {
			beego.Error("DB fail", err)
		}
	}
}


func UpdateServer()  {
	Doing()
	toolbox.AddTask("UpdateServer", toolbox.NewTask("UpdateServer", "0 0/30 * * * *", func()error{
		Doing()
		return  nil
	}))
}


func PostMail(username string, playerinfo *Player) {

	mail := Mail{Type:"GMT_InsertMail",InsertMailType:"IGMT_PlayerId",Sender:kSender, Title:kTitle,Content:kContent,Recvers:[]int{playerinfo.PlayerId},Items:mailItems}

	jmail ,err := json.Marshal(mail)

	if err != nil{
		beego.Error(err)
		return
	}
	ser := servers[playerinfo.ServerId]
	if nil == ser {
		return
	}

	req := httplib.Post(fmt.Sprintf("http://%s:%d",ser.LogicHost,ser.LogicPort))

	req.Body(jmail)

	str, err := req.String()
	if err != nil{
		beego.Error(err)
		return
	}

	beego.Debug("results: ", str)

}

