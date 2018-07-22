rpc.exe -i ../Schema/com.arpc -o ../Client/Assets/Scripts/Net/BinGen/ -g cs
rpc.exe -i ../Schema/struct.arpc -o ../Client/Assets/Scripts/Net/BinGen/ -g cs
rpc.exe -i ../Schema/proto.arpc -o ../Client/Assets/Scripts/Net/BinGen/ -g cs
rpc.exe -i ../Schema/global.arpc -o ../Client/Assets/Scripts/Net/BinGen/ -g cs

rpc.exe -i ../Schema/env.arpc -o ../Server/common/gen_rpc/ -g cpp
rpc.exe -i ../Schema/global.arpc -o ../Server/common/gen_rpc/ -g cpp
rpc.exe -i ../Schema/com.arpc -o ../Server/common/gen_rpc/ -g cpp
rpc.exe -i ../Schema/proto.arpc -o ../Server/common/gen_rpc/ -g cpp
rpc.exe -i ../Schema/structold.arpc -o ../Server/common/gen_rpc/ -g cpp
rpc.exe -i ../Schema/struct.arpc -o ../Server/common/gen_rpc/ -g cpp
