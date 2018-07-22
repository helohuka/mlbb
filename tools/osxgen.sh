#!/bin/bash 
./rpc -i ../Schema/env.arpc -o ../Server/common/gen_rpc/ -g cpp
./rpc -i ../Schema/global.arpc -o ../Server/common/gen_rpc/ -g cpp
./rpc -i ../Schema/com.arpc -o ../Server/common/gen_rpc/ -g cpp
./rpc -i ../Schema/proto.arpc -o ../Server/common/gen_rpc/ -g cpp
./rpc -i ../Schema/struct.arpc -o ../Server/common/gen_rpc/ -g cpp

./rpc -i ../Schema/com.arpc -o ../Client/Work/Assets/Scripts/Net/BinGen/ -g cs
./rpc -i ../Schema/struct.arpc -o ../Client/Work/Assets/Scripts/Net/BinGen/ -g cs
./rpc -i ../Schema/proto.arpc -o ../Client/Work/Assets/Scripts/Net/BinGen/ -g cs
./rpc -i ../Schema/global.arpc -o ../Client/Work/Assets/Scripts/Net/BinGen/ -g cs

