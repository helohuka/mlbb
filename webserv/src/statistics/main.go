package main

import (
	"github.com/astaxie/beego"
	"os"
	"os/exec"
	"path/filepath"
	"runtime"
	_ "statistics/routers"
	"strconv"
	"time"
)

func main() {

	beego.SetLogger("file", `{"filename":"log/`+strconv.FormatInt(time.Now().Unix(), 10)+`.log"}`)

	if (runtime.GOOS != "darwin") && (len(os.Args) > 1) && (os.Args[1] == "-d") {

		if os.Getppid() != 1 { //判断当其是否是子进程，当父进程return之后，子进程会被 系统1 号进程接管

			filePath, _ := filepath.Abs(os.Args[0])       //将命令行参数中执行文件路径转换成可用路径
			cmd := exec.Command(filePath, os.Args[2:]...) //将其他命令传入生成出的进程
			cmd.Stdin = os.Stdin                          //给新进程设置文件描述符，可以重定向到文件中
			cmd.Stdout = os.Stdout
			cmd.Stderr = os.Stderr
			cmd.Start() //开始执行新进程，不等待新进程退出
			return
		}
	}

	beego.Run()
}
