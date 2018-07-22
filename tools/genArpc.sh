#!/bin/bash 
/usr/local/bin/rpc -i ../Schema/env.arpc -o ../Server/common/gen_rpc/ -g cpp
/usr/local/bin/rpc -i ../Schema/global.arpc -o ../Server/common/gen_rpc/ -g cpp
/usr/local/bin/rpc -i ../Schema/com.arpc -o ../Server/common/gen_rpc/ -g cpp
/usr/local/bin/rpc -i ../Schema/proto.arpc -o ../Server/common/gen_rpc/ -g cpp
/usr/local/bin/rpc -i ../Schema/struct.arpc -o ../Server/common/gen_rpc/ -g cpp
/usr/local/bin/rpc -i ../Schema/structold.arpc -o ../Server/common/gen_rpc/ -g cpp
