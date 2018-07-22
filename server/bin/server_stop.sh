#################################################################################
#platform linux	SHELL								#
#brief 	  shut down for server group						#		
#date  	  2013-2-27								#
#version 1.0.0									#
#################################################################################

#!/bin/sh

pwd=`pwd`
gateway=`ps aux | grep "$pwd/gateway -d" | grep -v "grep" | awk '{print $2}'`
echo $gateway
if [ -n "$gateway"  ] ; then
	kill -9 $gateway
	echo "kill gateway server process-${gateway}" 
	echo "wait for save role ......"
	sleep 20	
else
	echo "gateway-server is not running!"
fi

login=`ps aux | grep  "$pwd/login -d" | grep -v "grep" | awk '{print $2}'`
if [ -n "$login" ] ; then
	kill -9 $login
	echo "kill login server process-${login}" 
else
	echo "login-server is not running!"
fi

scene=`ps aux | grep "$pwd/scene -d" |  grep -v "grep" | awk '{print $2}'`
if [ -n "$scene" ] ; then
	kill -9 $scene
	echo "kill scene server process-${scene}" 
else
	echo "scene-server is not running!"
fi

mall=`ps aux | grep "$pwd/mall -d" |  grep -v "grep" | awk '{print $2}'`
if [ -n "$mall" ] ; then
	kill -9 $mall
	echo "kill mall server process-${mall}" 
else
	echo "mall-server is not running!"
fi

log=`ps aux | grep "$pwd/logser -d" |  grep -v "grep" | awk '{print $2}'`
if [ -n "$log" ] ; then
	kill -9 $log
	echo "kill log server process-${log}" 
else
	echo "log-server is not running!"
fi

gmtool=`ps aux | grep "$pwd/gmtool -d" | grep -v "grep" | awk '{print $2}'`
if [ -n "$gmtool" ] ; then
	kill -9 $gmtool
	echo "kill gmool server process-${gmtool}" 
else
	echo "gmtool-server is not running!"
fi 

world=`ps aux | grep "$pwd/world -d" | grep -v "grep" | awk '{print $2}'`
if [ -n "$world" ] ; then
	kill -9 $world
	echo "kill world server process-${world}" 
else
	echo "world-server is not running!"
fi 

db=`ps aux | grep "$pwd/db -d" | grep -v "grep" | awk '{print $2}'`
if [ -n "$db" ] ; then
        kill -9 $db
        echo "kill db server process-${db}" 
else
        echo "database-server is not running!"
fi

test=`ps aux | grep "$pwd/test -d" | grep -v "grep" | awk '{print $2}'`
if [ -n "$test" ] ; then
        kill -9 $test
        echo "kill test server process-${test}" 
else
        echo "test-server is not running!"
fi
