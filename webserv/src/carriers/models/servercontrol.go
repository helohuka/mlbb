package models

import (
	"fmt"
	"net"

	"github.com/astaxie/beego"
	"github.com/astaxie/beego/toolbox"
	"golang.org/x/crypto/ssh"
	"time"
)

func MakeNewServer(sshHost, sshPassword, mysqlHost, mysqlUsername, mysqlPassword, gameDB, logDB, from string, mysqlPort, serverId, startPort int) {
	conf := ssh.ClientConfig{
		User: beego.AppConfig.String("logicserversshuser"),
		Auth: []ssh.AuthMethod{ssh.Password(sshPassword)},
		HostKeyCallback: func(hostname string, remote net.Addr, key ssh.PublicKey) error {
			return nil
		}}

	//解压服务器包
	cmd := fmt.Sprintf("pwd \n")
	cmd += fmt.Sprintf("wget http://%s/server/server.tar.gz.%s \n", beego.AppConfig.String("myip"), from)
	cmd += fmt.Sprintf("tar vxjpf server.tar.gz.%s\n", from)
	cmd += fmt.Sprintf("mv server %d\n", serverId)
	//创建数据库表
	cmd += fmt.Sprintf("cd %d \n", serverId)
	cmd += fmt.Sprintf("echo 'CREATE DATABASE IF NOT EXISTS `%d_game` DEFAULT CHARSET utf8 COLLATE utf8_bin;' >> ./sql/CreateDB.sql \n", serverId)
	cmd += fmt.Sprintf("echo 'CREATE DATABASE IF NOT EXISTS `%d_log` DEFAULT CHARSET utf8mb4 COLLATE utf8mb4_bin;' >> ./sql/CreateDB.sql \n", serverId)
	cmd += fmt.Sprintf("mysql -h%s -P%d -u%s -p'%s' < ./sql/CreateDB.sql \n", mysqlHost, mysqlPort, mysqlUsername, mysqlPassword)
	cmd += fmt.Sprintf("mysql -h%s -P%d -u%s -p'%s' %s < ./sql/DDL.sql \n", mysqlHost, mysqlPort, mysqlUsername, mysqlPassword, gameDB)
	cmd += fmt.Sprintf("mysql -h%s -P%d -u%s -p'%s' %s < ./sql/Log.sql \n", mysqlHost, mysqlPort, mysqlUsername, mysqlPassword, logDB)

	//写入配置文件
	cmd += fmt.Sprintf("cd bin \n")
	cmd += fmt.Sprintf("rm -rf env.lua \n")

	cmd += fmt.Sprintf("echo  'Env.setString(V_GatewayHost,\"127.0.0.1:%d\");'>> env.lua\n", startPort)
	cmd += fmt.Sprintf("echo  'Env.setString(V_GatewayListenClientMultiIndoor,\"%d,%d\");'>> env.lua\n", serverId, startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setInt(V_PayListenPort,%d);'>> env.lua\n", startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setString(V_AnySDKPayNotifyListen,\"0.0.0.0:%d\");'>> env.lua\n", startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setString(V_GMTListenWebServer,\"0.0.0.0:%d\");'>> env.lua\n", startPort)
	startPort += 1

	cmd += fmt.Sprintf("echo  'Env.setString(V_WorldListenDB,\"127.0.0.1:%d\");'>> env.lua\n", startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setString(V_WorldListenLogin,\"127.0.0.1:%d\");'>> env.lua\n", startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setString(V_WorldListenMall,\"127.0.0.1:%d\");'>> env.lua\n", startPort)
	startPort += 1

	cmd += fmt.Sprintf("echo  'Env.setString(V_WorldListenScene,\"127.0.0.1:%d\");'>> env.lua\n", startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setString(V_WorldListenLogser,\"127.0.0.1:%d\");'>> env.lua\n", startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setString(V_WorldListenGMT,\"127.0.0.1:%d\");'>> env.lua\n", startPort)
	startPort += 1
	cmd += fmt.Sprintf("echo  'Env.setString(V_WorldListenGateway,\"127.0.0.1:%d\");'>> env.lua\n", startPort)

	cmd += fmt.Sprintf("echo  'Env.setString(V_MysqlHost,\"%s:%d\");'>> env.lua\n", mysqlHost, mysqlPort)
	cmd += fmt.Sprintf("echo  'Env.setString(V_MysqlUser,\"%s\");'>> env.lua\n", mysqlUsername)
	cmd += fmt.Sprintf("echo  'Env.setString(V_MysqlPassword,\"%s\");'>> env.lua\n", mysqlPassword)
	cmd += fmt.Sprintf("echo  'Env.setString(V_DatabaseName,\"%s\");'>> env.lua\n", gameDB)
	cmd += fmt.Sprintf("echo  'Env.setString(V_logersName,\"%s\");'>> env.lua\n", logDB)

	cmd += fmt.Sprintf("echo  'Env.setInt(V_UsedAnySDK,0);'>> env.lua\n")

	cmd += fmt.Sprintf("echo  'Env.setInt(V_DebugLog,1);'>> env.lua\n")

	cmd += fmt.Sprintf("echo  'Env.setString(V_TableFolder,\"../config/Tables/\");'>> env.lua\n")
	cmd += fmt.Sprintf("echo  'Env.setString(V_ScriptFolder,\"../config/Script/\");'>> env.lua\n")

	cmd += fmt.Sprintf("echo  'Env.setString(V_CenterServerHost,\"http://%s\");'>> env.lua\n", beego.AppConfig.String("myip"))
	cmd += fmt.Sprintf("echo  'Env.setString(V_LogServerHost,\"http://%s:10998\");'>> env.lua\n", beego.AppConfig.String("myip"))

	cmd += fmt.Sprintf("echo  'Env.setString(V_SMSUsername,\"8a216da8586c7379015876b648360730\");'>> env.lua\n")
	cmd += fmt.Sprintf("echo  'Env.setString(V_SMSAuthToken,\"68b88e1a8af645fc9126862ae086aaee\");'>> env.lua\n")
	cmd += fmt.Sprintf("echo  'Env.setString(V_SMSAppId,\"8aaf070858862df3015889be921200d4\");'>> env.lua\n")
	cmd += fmt.Sprintf("echo  'Env.setString(V_SMSTmplateId,\"183858\");'>> env.lua\n")
	cmd += fmt.Sprintf("echo  'Env.setString(V_SMSTimeout,\"5\");'>> env.lua\n")

	cmd += fmt.Sprintf("./server_restart.sh \n")

	beego.Info(cmd)

	if client, err := ssh.Dial("tcp", sshHost, &conf); err == nil {
		defer client.Close()

		if sess, err := client.NewSession(); err == nil {
			defer sess.Close()
			sess.Stdout = beego.BeeLogger

			sess.Run(cmd)
		}

	}
}

func UpdateOnlineServer(sshPassword, packageName string, servId int) {

	if sv := sc.IdServs[servId]; sv != nil {
		conf := ssh.ClientConfig{
			User: beego.AppConfig.String("logicserversshuser"),
			Auth: []ssh.AuthMethod{ssh.Password(sshPassword)},
			HostKeyCallback: func(hostname string, remote net.Addr, key ssh.PublicKey) error {
				return nil
			}}
		unix := time.Now().Unix()
		cmd := fmt.Sprintf("wget http://%s/server/patch.tar.gz.%s \n", beego.AppConfig.String("myip"), packageName)
		cmd += fmt.Sprintf("tar vxjpf patch.tar.gz.%s\n", packageName)
		cmd += fmt.Sprintf("mv %d %d.backup.%d\n", servId, servId, unix)
		cmd += fmt.Sprintf("mv server %d\n", servId)
		cmd += fmt.Sprintf("cp %d.backup.%d/bin/env.lua %d/bin \n", sv.Id, unix, sv.Id)
		cmd += fmt.Sprintf("cd %d/bin\n", servId)
		cmd += fmt.Sprintf("./server_restart.sh\n")
		beego.Info(cmd)

		if client, err := ssh.Dial("tcp", sv.Host+":22", &conf); err == nil {
			defer client.Close()

			if sess, err := client.NewSession(); err == nil {
				defer sess.Close()
				sess.Stdout = beego.BeeLogger
				sess.Run(cmd)
			}

		}
	}
}

func Restart(sshPassword string, servId int) {
	if sv := sc.IdServs[servId]; sv != nil {
		conf := ssh.ClientConfig{
			User: beego.AppConfig.String("logicserversshuser"),
			Auth: []ssh.AuthMethod{ssh.Password(sshPassword)},
			HostKeyCallback: func(hostname string, remote net.Addr, key ssh.PublicKey) error {
				return nil
			}}

		cmd := fmt.Sprintf("cd %s/%d/bin \n", sv.Path, sv.Id)
		cmd += fmt.Sprintf("./server_restart.sh")

		beego.Info(cmd)

		if client, err := ssh.Dial("tcp", sv.Host+":22", &conf); err == nil {
			defer client.Close()

			if sess, err := client.NewSession(); err == nil {
				defer sess.Close()
				sess.Stdout = beego.BeeLogger
				sess.Run(cmd)
			}

		}
	}

}

func BackupDBGame(servId int) {
	if sv := sc.IdServs[servId]; sv != nil {
		conf := ssh.ClientConfig{
			User: beego.AppConfig.String("logicserversshuser"),
			Auth: []ssh.AuthMethod{ssh.Password(beego.AppConfig.String("logicserversshpass"))},
			HostKeyCallback: func(hostname string, remote net.Addr, key ssh.PublicKey) error {
				return nil
			}}

		cmd := fmt.Sprintf("mysqldump -uroot %d_game | gzip > %d_game_%d.sql;  \n", sv.Id, sv.Id, time.Now().Unix())
		beego.Info(cmd)

		if client, err := ssh.Dial("tcp", sv.DBHost+":22", &conf); err == nil {
			defer client.Close()

			if sess, err := client.NewSession(); err == nil {
				defer sess.Close()
				sess.Stdout = beego.BeeLogger
				sess.Run(cmd)
			}

		}
	}
}

func BackupAllDBGame() error {
	for _, sv := range sc.Servs {
		go BackupDBGame(sv.Id)
	}
	return nil
}

func InitBackupDB() {
	task := toolbox.NewTask("BackupDBGameTask", beego.AppConfig.String("backuptime"), BackupAllDBGame)
	toolbox.AddTask("BackupDBGameTask", task)
}
