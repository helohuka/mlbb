package models

import (
	"database/sql"
	"strconv"
	"github.com/astaxie/beego"
	_ "github.com/go-sql-driver/mysql"
	"encoding/json"
	"github.com/astaxie/beego/httplib"
	"fmt"
	"github.com/astaxie/beego/toolbox"
)

type (
	Player struct {
		ServerName  string
		PlayerName  string
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
		Accounts 	map[string]map[int]*Player
	}
)

func (this *Server) DB()*sql.DB{
	dsn := this.DBUser + ":" + this.DBPass + "@tcp(" + this.DBHost + ":" + strconv.FormatInt(int64(this.DBPort), 10) + ")/" + this.DBName
	db, err := sql.Open("mysql", dsn)
	if err != nil {
		beego.Error(err)
		return  nil
	}
	return  db
}

func (this *Server) UpdateAccounts(){
	if db := this.DB(); db != nil{
		defer  db.Close()
		dbRecords, err := db.Query("SELECT `UserName`,`PlayerGuid`,`PlayerName`,`PlayerLevel` FROM `Player`")
		if err != nil {
			beego.Error(err)
			return
		}
		for dbRecords.Next() {
			acc:=""
			pla := Player{ServerName:this.ServerName,ServerId:this.ServerId}
			err := dbRecords.Scan(&acc,&pla.PlayerId,&pla.PlayerName,&pla.PlayerLevel)
			if err != nil{
				beego.Error(err)
				continue
			}
			if _, ok := this.Accounts[acc]; !ok{
				this.Accounts[acc] = map[int]*Player{}
			}
			this.Accounts[acc][pla.PlayerId] = &pla
		}
	}
	beego.Debug("Update Account ", this.ServerName)
}

func (this *Server) GetPlayer(accountName string, playerId int)*Player{
	if v,ok := this.Accounts[accountName]; ok{
		if p,ok2 := v[playerId]; ok2{
			return  p
		}
	}
	return  nil
}

func (this *Server)SendGift(accountName, kaid string , playerId int) bool {

	gift := GetGift(kaid)
	if gift == nil {
		return false
	}

	player := this.GetPlayer(accountName,playerId)

	if gift.ReqType == kLevelGiftId{
		if player.PlayerLevel < gift.ReqValue{
			beego.Info("%s %d %d Level Gift level less ",accountName,this.ServerId,playerId)
			return false
		}
	}

	mail := Mail{Type:"GMT_InsertMail",InsertMailType:"IGMT_PlayerId",Sender:gift.MailSender, Title:gift.MailTitle,Content:gift.MailContent,Recvers:[]int{playerId},Items:gift.Items}

	jmail ,err := json.Marshal(mail)

	if err != nil{
		beego.Error(err)
		return false
	}

	req := httplib.Post(fmt.Sprintf("http://%s:%d",this.LogicHost,this.LogicPort))

	req.Body(jmail)

	str, err := req.String()
	if err != nil{
		beego.Error(err)
		return  false
	}

	beego.Debug(str)

	if gift.ReqType == kRMBGiftId{
		gift.Recvers = append(gift.Recvers,accountName)
	}else {
		gift.Recvers = append(gift.Recvers,fmt.Sprintf("%s%d%d",accountName,this.ServerId,playerId))
	}

	return  true
}

var Servers = []*Server{}

func UpdateServers(){
	beego.Debug("First")
	if center := CarriersDB(); center != nil {
		println("1: ", center)
		defer  center.Close()
		Servers = []*Server{}
		println("2: ", Servers)
		centerRecords, err := center.Query("SELECT `ServId`,`ServName`,`ServHost`,`GMTPort`,`DBHost`,`DBPort`,`DBUsr`,`DBPwd`,`DBGameName` FROM Servs WHERE `Channels` LIKE '%000255%'")
		println("3: ", centerRecords)
		if err != nil {
			beego.Error(err.Error())
			return
		}

		for centerRecords.Next() {
			ser := Server{Accounts:map[string]map[int]*Player{}}
			err := centerRecords.Scan(&ser.ServerId,&ser.ServerName,&ser.LogicHost,&ser.LogicPort,&ser.DBHost,&ser.DBPort,&ser.DBUser,&ser.DBPass,&ser.DBName)
			if err != nil {
				beego.Error(err)
				continue
			}
			println("4: ", &ser)
			Servers = append(Servers,&ser)
			go ser.UpdateAccounts()

			beego.Debug("Update Server")
		}
	}
}



func GetServerById(serverId int)*Server{
	for _,v :=range Servers{
		if v.ServerId == serverId{
			return  v
		}
	}
	return  nil
}


func InitUpdateServersTask(){
	toolbox.AddTask("UpdateServersTask", toolbox.NewTask("UpdateServersTask", "0 */30 * * * *", func()error{
		UpdateServers()
		return  nil
	}))
}
