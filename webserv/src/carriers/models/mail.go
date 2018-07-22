package models

import (
	"time"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/toolbox"
)

type MailContent struct {
	ServId   int
	ServName string
	Area     string
}

func sendToMail(content []MailContent) {
	//recvs := map[string][]string{}
	//recvs["To"] = strings.Split(beego.AppConfig.String("mailrecv"), ",")
	//
	//if len(recvs["To"]) == 0 {
	//	beego.Error("Mail recver is nil")
	//	return
	//}
	//
	//////m := gomail.NewMessage()
	//m.SetHeader("From", "1042088863@qq.com")
	//m.SetHeaders(recvs)
	////m.SetHeader("To", "heycatprc@qq.com", "phil.yang@tanyu.mobi", "parsons.lin@tanyu.mobi")
	//m.SetHeader("Subject", "服务器故障")
	//b, _ := json.Marshal(content)
	//m.SetBody("text/html", string(b))
	//
	//d := gomail.NewDialer("smtp.qq.com", 465, "1042088863@qq.com", "qzpmqzdnytqgbfbh")
	//// Send the email to Bob, Cora and Dan.
	//if err := d.DialAndSend(m); err != nil {
	//	beego.Debug(err.Error())
	//}
}

var SendMailStepList = [5]int64{300, 1800, 3600, 7200, 18000}

func checkServers() error {

	now := time.Now()

	content := []MailContent{}
	for _, sv := range sc.Servs {
		offLineTime := now.Unix() - sv.LastUpdateTime

		sv.IsOnline = offLineTime < 300

		//beego.Info(sv.Id, sv.LastUpdateTime, now.Unix(), sv.IsOnline)
		if sv.Id < 2000 {
			continue
		}
		if sv.NoCheck == 1 {
			continue
		}

		if sv.IsOnline == false {

			if sv.SendMailStep < len(SendMailStepList) {

				if offLineTime > SendMailStepList[sv.SendMailStep] {
					content = append(content, MailContent{sv.Id, sv.Name, sv.ServAreaName})

					sv.SendMailStep++
				}
			}
		} else {
			sv.SendMailStep = 0

		}
	}

	if len(content) != 0 {
		sendToMail(content)
	}
	return nil
}

func InitCheckServerTask() {
	task := toolbox.NewTask("MailTask", beego.AppConfig.String("mailtime"), checkServers)
	toolbox.AddTask("MailTask", task)
}
