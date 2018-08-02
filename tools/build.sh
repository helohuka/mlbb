#!/bin/sh
rm -rf ./server
mkdir ../server/build
cd ../server/build 
rm -rf ./* 
cmake ..
make install -j8 

cd ../../tools

mkdir -p server/bin
mkdir -p server/sql
mkdir -p server/config/Tables/LuaRoot/lua
mkdir -p server/config/Tables/Scene
mkdir -p server/config/Script/AI
mkdir -p server/config/Script/EmployeeAI

cp ../server/bin/world 			./server/bin/
cp ../server/bin/db 				./server/bin/
cp ../server/bin/gateway 			./server/bin/
cp ../server/bin/login 			./server/bin/
cp ../server/bin/mall 				./server/bin/
cp ../server/bin/scene 			./server/bin/
cp ../server/bin/gmtool 			./server/bin/
cp ../server/bin/server_stop.sh 	./server/bin/
cp ../server/bin/server_start.sh 	./server/bin/
cp ../server/bin/server_restart.sh ./server/bin/
#cp ../bin/env.lua 			./server/bin/

cp -PfR ../config/Tables/*.csv  ./server/config/Tables/
cp -PfR ../config/Tables/*.xml  ./server/config/Tables/
cp -PfR ../config/Tables/Scene/*.xml  ./server/config/Tables/Scene/
cp -PfR ../config/Tables/Scene/*.obj  ./server/config/Tables/Scene/
cp -PfR ../config/Tables/LuaRoot/lua/*.lua  ./server/config/Tables/LuaRoot/lua

cp -PfR ../config/Script/*.lua  ./server/config/Script/
cp -PfR ../config/Script/AI/*.lua  ./server/config/Script/AI/
cp -PfR ../config/Script/EmployeeAI/*.lua  ./server/config/Script/EmployeeAI/

cp -PfR ../sql/*.sql ./server/sql/

md5sum ./server/bin/world 	 		>>	./server/TAG.TXT
md5sum ./server/bin/db 			 	>>	./server/TAG.TXT
md5sum ../bin/gateway 		 		>>	./server/TAG.TXT
md5sum ./server/bin/login			>>	./server/TAG.TXT
md5sum ./server/bin/mall 			>>	./server/TAG.TXT
md5sum ./server/bin/scene 	 		>>	./server/TAG.TXT
md5sum ./server/bin/logser 	 		>>	./server/TAG.TXT
md5sum ./server/bin/gmtool 	 		>>	./server/TAG.TXT

tar vcjpf server.tar.gz ./server

mv server.tar.gz server.tar.gz.`date +%s`

ps 

