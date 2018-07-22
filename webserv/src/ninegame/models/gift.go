package models

import (
	"github.com/astaxie/beego"
	"encoding/json"
	"fmt"
	"github.com/astaxie/beego/toolbox"
)

const(
	kEveryDayGiftId = 1
	kEveryWeekGiftId = 2
	kLevelGiftId = 3
	kRMBGiftId = 4
)

type (
	Item struct {
		ItemId , ItemStack int
	}

	UCGift struct{
		KaId  		string
		ReqType 	int
		ReqValue	int
		MailSender 	string
		MailTitle 	string
		MailContent 	string
		Items 		[]Item
		Recvers		[]string
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

var Gift = map[string]*UCGift{}

func InitGift(){

	if center := CarriersDB(); center != nil {
		defer  center.Close()
		giftRecords, err := center.Query("SELECT `KaId`,`MailSender`,`MailTitle`,`MailContent`,`ReqType`,`ReqValue`,`Items`,`Recvers` FROM UCGift")

		if err != nil {
			beego.Error(err.Error())
			return
		}

		for giftRecords.Next() {
			recvStr, itemStr := "",""
			gift := UCGift{}
			err := giftRecords.Scan(&gift.KaId,&gift.MailSender,&gift.MailTitle,&gift.MailContent,&gift.ReqType,&gift.ReqValue,&itemStr,&recvStr)
			if err != nil {
				beego.Error(err.Error())
				continue
			}

			err = json.Unmarshal([]byte(recvStr),&gift.Recvers)
			if err != nil {
				beego.Error(err.Error())
				continue
			}

			err = json.Unmarshal([]byte(itemStr),&gift.Items)
			if err != nil {
				beego.Error(err.Error())
				continue
			}

			Gift[gift.KaId] = &gift
		}
	}

}

func InsertGift(gift UCGift){
	Gift[gift.KaId] = &gift
	if center := CarriersDB(); center != nil {
		defer  center.Close()
		itemStr, _ := json.Marshal(gift.Items)
		recvStr, _ := json.Marshal(gift.Recvers)
		_, err := center.Exec("INSERT INTO `UCGift`(`KaId`,`MailSender`,`MailTitle`,`MailContent`,`ReqType`,`ReqValue`,`Items`,`Recvers`)VALUES(?,?,?,?,?,?,?,?)",gift.KaId,gift.MailSender,gift.MailTitle,gift.MailContent,gift.ReqType,gift.ReqValue,string(itemStr),string(recvStr))

		if err != nil {
			beego.Error(err.Error())
			return
		}
	}
}

func SaveGift(){
	if center := CarriersDB(); center != nil {
		defer  center.Close()
		for _, gift := range Gift{
			recvStr, _ := json.Marshal(gift.Recvers)
			_, err := center.Exec("UPDATE `UCGift` SET `Recvers`=? WHERE `KaId`=?",string(recvStr),gift.KaId)

			if err != nil {
				beego.Error(err.Error())
				return
			}
		}
	}
}

func GetGift(kaId string)*UCGift{
	return  Gift[kaId]
}

func IsGotGift(kaId, accountName string, serverId, playerId int) bool{
	gift := Gift[kaId]
	if gift == nil {
		return false
	}

	mkid := fmt.Sprintf("%s%d%d",accountName,serverId,playerId)
	if gift.ReqType == kRMBGiftId{
		mkid = accountName
	}

	for _, v :=range gift.Recvers{
		if v == mkid{
			return true
		}
	}
	return  false
}

func InitGiftTask(){
	toolbox.AddTask("EveryDayTask", toolbox.NewTask("EveryDayTask", "0 0 0 * * *", func()error{

		for _, gift :=range Gift{
			if gift.ReqType == kEveryDayGiftId{
				gift.Recvers = []string{}
			}
		}

		return  nil
	}))


	toolbox.AddTask("EveryWeekTask", toolbox.NewTask("EveryWeekTask", "0 0 0 * * 1", func()error{

		for _, gift :=range Gift{
			if gift.ReqType == kEveryWeekGiftId{
				gift.Recvers = []string{}
			}
		}

		return  nil
	}))

	toolbox.AddTask("SaveGiftTask", toolbox.NewTask("SaveGiftTask", "0 */5 * * * *", func()error{

		SaveGift()

		return  nil
	}))
}