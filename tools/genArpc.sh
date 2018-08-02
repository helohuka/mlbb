#!/bin/bash 
./rpc -i ../schema/env.arpc -o ../server/common/gen_rpc/ -g cpp
./rpc -i ../schema/global.arpc -o ../server/common/gen_rpc/ -g cpp
./rpc -i ../schema/com.arpc -o ../server/common/gen_rpc/ -g cpp
./rpc -i ../schema/proto.arpc -o ../server/common/gen_rpc/ -g cpp
./rpc -i ../schema/struct.arpc -o ../server/common/gen_rpc/ -g cpp
./rpc -i ../schema/structold.arpc -o ../server/common/gen_rpc/ -g cpp
