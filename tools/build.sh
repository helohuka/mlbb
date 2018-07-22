#!/bin/sh
rm -rf ./server

cd ../ && svn revert -R ./  && svn up ./ && cd ./Server/build && rm -rf ./* && cmake .. && make install -j8 && cd ../../bin && ./server_restart.sh

cd ../Tools

mkdir -p server/bin
mkdir -p server/sql
mkdir -p server/Config/Tables/LuaRoot/lua
mkdir -p server/Config/Tables/Scene
mkdir -p server/Config/Script/AI
mkdir -p server/Config/Script/EmployeeAI

cp ../bin/world 			./server/bin/
cp ../bin/db 				./server/bin/
cp ../bin/gateway 			./server/bin/
cp ../bin/login 			./server/bin/
cp ../bin/mall 				./server/bin/
cp ../bin/scene 			./server/bin/
cp ../bin/logser 			./server/bin/
cp ../bin/gmtool 			./server/bin/
cp ../bin/refreshsql 			./server/bin/
cp ../bin/refreshsql.sh 		./server/bin/
cp ../bin/server_stop.sh 	./server/bin/
cp ../bin/server_start.sh 	./server/bin/
cp ../bin/server_restart.sh ./server/bin/
#cp ../bin/env.lua 			./server/bin/

cp -PfR ../Config/Tables/*.csv  ./server/Config/Tables/
cp -PfR ../Config/Tables/*.xml  ./server/Config/Tables/
cp -PfR ../Config/Tables/Scene/*.xml  ./server/Config/Tables/Scene/
cp -PfR ../Config/Tables/Scene/*.obj  ./server/Config/Tables/Scene/
cp -PfR ../Config/Tables/LuaRoot/lua/*.lua  ./server/Config/Tables/LuaRoot/lua

cp -PfR ../Config/Script/*.lua  ./server/Config/Script/
cp -PfR ../Config/Script/AI/*.lua  ./server/Config/Script/AI/
cp -PfR ../Config/Script/EmployeeAI/*.lua  ./server/Config/Script/EmployeeAI/

cp -PfR ../Sql/*.sql ./server/sql/

echo `svn info ../` 				>> ./server/TAG.TXT
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

