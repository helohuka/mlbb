#################################################################################
#platform linux	SHELL								#
#brief 	  shut down for server group						#		
#date  	  2013-2-27								#
#version 1.0.0									#
#################################################################################

#!/bin/sh

gateway=`ps -e | grep "gateway" | awk '{print $1}'`
worldserver=`ps -e | grep "world" | awk '{print $1}'`  
sceneserver=`ps -e | grep "scene" | awk '{print $1}'`  
loginserver=`ps -e | grep "login" | awk '{print $1}'`  
dbserver=`ps -e | grep "db" | awk '{print $1}'`  

needrestart=''
if [ -n "$gateway"  ] ; then
	echo '--'
else 
	needrestart='--'
fi


if [ -n "$worldserver" ] ; then
	echo '--'
else
	needrestart='--'
fi


if [ -n "$sceneserver" ] ; then
	echo '--'
else
	needrestart='--'
fi



if [ -n "$loginserver" ] ; then
	echo '--'
else
	needrestart='--'
fi


if [ -n "$dbserver" ] ; then
	echo '--'
else
	needrestart='--'
fi

if [ $needrestart ] ; then
	. ./server_restart.sh
fi

