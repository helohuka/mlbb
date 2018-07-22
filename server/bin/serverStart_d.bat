
#taskkill/im world.exe /f 
taskkill/im login.exe /f 
taskkill/im db.exe /f 
taskkill/im gateway.exe /f 
#taskkill/im mall.exe /f
taskkill/im scene.exe /f
start world.exe
@ping -n 10 127.1>nul
start login.exe
start db.exe
start gateway.exe
#start mall.exe
start scene.exe
