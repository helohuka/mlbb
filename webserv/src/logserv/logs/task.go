package logs

import (
	"archive/zip"
	"os"
	"time"

	"github.com/astaxie/beego"
	_ "github.com/astaxie/beego/toolbox"
	"github.com/astaxie/beego/toolbox"
	"logserv/tools"
)

//上传FTP
func upload(remotpath string, localpath string, file string) {
	serv := tools.LogFTP()
	if nil == serv {
		return
	}
	defer serv.Logout()
	defer serv.Quit()
	err := serv.ChangeDir(remotpath)
	if err != nil {
		err = serv.MakeDir(remotpath)
		if err != nil {
			beego.Error(err.Error())
		}
		err = serv.ChangeDir(remotpath)
		if err != nil {
			beego.Error(err.Error())
			return
		}
	}

	fd, err := os.Open(file)
	if err != nil {
		beego.Error(err.Error())
		return
	}
	defer fd.Close()
	err = serv.Stor(file, fd)
	if err != nil {
		beego.Error(err.Error())
	}
	beego.Info("Send Log Success The File Name Is ", file)
}

func dumpTask() error {
	tools.InitSDKInfo()

	filename := beego.AppConfig.String("gamename") + "_" + time.Now().Add(-24*time.Hour).Format("20060102")

	ziparchive, _ := os.Create(filename + ".zip")

	zipbuffer := zip.NewWriter(ziparchive)

	zipwriter, _ := zipbuffer.Create(filename + ".sql")

	begin := time.Now().Add(-24 * time.Hour).Format("2006-01-02")
	end := time.Now().Format("2006-01-02")

	code := ("/* " + time.Now().Format(time.RFC1123) + "* */\n")
	code += DumpAccountLog(begin+" 00:00:00", end+" 00:00:00")
	zipwriter.Write([]byte(code))
	code = ("/* " + time.Now().Format(time.RFC1123) + "* */\n")
	code += DumpLoginLog(begin+" 00:00:00", end+" 00:00:00")
	zipwriter.Write([]byte(code))
	code = ("/* " + time.Now().Format(time.RFC1123) + "* */\n")
	code += DumpOrderLog(begin+" 00:00:00", end+" 00:00:00")
	zipwriter.Write([]byte(code))
	code = ("/* " + time.Now().Format(time.RFC1123) + "* */\n")
	code += DumpRoleLog(end)
	zipwriter.Write([]byte(code))

	zipbuffer.Close()

	ziparchive.Close()

	pathname := time.Now().Format("200601")

	beego.Info("Dump Log Success, The File Is ", filename)

	upload(pathname, "./", filename+".zip")

	return nil
}

func InitLogging() {
	go InitAccountLog()
	go dumpTask()
	task := toolbox.NewTask("LogTask", beego.AppConfig.String("logtime"), dumpTask)
	toolbox.AddTask("LogTask", task)
}
