#################################################################################
#platform linux	SHELL								#
#brief 	  服务器启动脚本							#		
#date  	  2013-2-27								#
#version 1.0.0									#
#										#
#开启服务器待完善								#
#1)worldserver 完全启动前不能启动其他服务器但是怎么知道worldserver完全启动	#
#  目前做法是强制sleep 20 秒							#
#2)启动相关的log只是流程输出并未检测是否已经启动				#
#################################################################################

#!/bin/sh
#开启服务器

start()
{
	`pwd`/world -d
	echo "Wait for world server......"
	sleep 10
	echo "World server start"
	`pwd`/scene -d
	echo "Scene server start"
	`pwd`/login -d
	echo "Login server start"
	`pwd`/db -d
	echo "DB server start"
	`pwd`/gateway -d
	echo "Gateway server start"
	`pwd`/mall -d
	echo "Mall server start"
	`pwd`/logser -d
	echo "logser server start"
	`pwd`/gmtool -d
	echo "gmtool server start"
	#sleep 30
	#`pwd`/test -d
	#echo "test server start"
}

if [ "$SHELL" != "/bin/bash" ] ; then 
	echo $SHELL
	echo "error must use /bin/bash"
	exit 0
fi

start

