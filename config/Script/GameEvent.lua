
function GameEvent_Debug_Log(msg)
    if Debug then
        Sys.log("GameEvent.lua "..msg);
    end
end

function newserver()
	Sys.open_hotShop(3);
	Sys.open_discountStore(7);
	Sys.open_rechargeSingle(7);
	Sys.open_rechargeTotal(7);
	Sys.open_card(7);
	Sys.open_festival(7);
	Sys.open_employeeActivityTotal(7);
	Sys.open_mingiftbag(7);
	Sys.open_zhuanpan(7);
	Sys.open_integralshop(7);
end

function addWorldLevelBuff(RECEIVER,ADDED,DELE0,DELE1)
	if Entity.check_state(RECEIVER,DELE0) then
		Entity.remove_state(RECEIVER,DELE0);
	end 
	if Entity.check_state(RECEIVER,DELE1) then
		Entity.remove_state(RECEIVER,DELE1);
	end 
	
	if Entity.check_state(RECEIVER,ADDED) then
		return 
	end 
	Entity.insert_state(RECEIVER,ADDED,0,0);
end 

function checkWorldLevelBuff(RECEIVER,LEVEL,WORLD_LEVEL)
	if LEVEL < 10 then
		return  --//10级以下没有世界经验
	end
	local diffV = WORLD_LEVEL - LEVEL
	
	if diffV <= 5 then 
		--//没有差异 强制删除
		Entity.remove_state(RECEIVER,1002);
		Entity.remove_state(RECEIVER,1003);
		Entity.remove_state(RECEIVER,1004);
		return 
	end
	
	local diff = math.floor(diffV / 10)
	
	if diff == 0 then 
		addWorldLevelBuff(RECEIVER,1002,1003,1004)
	elseif diff == 1 then 
		addWorldLevelBuff(RECEIVER,1003,1002,1004)
	elseif diff >= 2 then 
		addWorldLevelBuff(RECEIVER,1004,1002,1003)
	end	
end

function PlayerOnline(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	
	local name = Player.getplayerName(RECEIVER);
	if ARG0 >= 7 then
		Player.sevenclose(RECEIVER);
	end
	

	local lv = Player.get_property(RECEIVER,PT_Level)
	local world_lv = Sys.get_world_level()
	checkWorldLevelBuff(RECEIVER,lv,world_lv)
end


function PlayerOffline(RECEIVER, ARG0, ARG1, ARG2, ARG3)

end

function CreatePlayerOK(RECEIVER,ARG0, ARG1, ARG2, ARG3)

end

-- 0 battle id;
-- 1 monster id , emplyee id, babay id, player id;
-- 2 target level
-- 3 EntityType
-- 4 same force is 1 ;

function playerLearnSkill(RECEIVER,skilltime)
	--if skilltime == 1 then
		--Player.set_opensubsystem(RECEIVER,OSSF_Achieve);
	--end
end


function PlayerKill(RECEIVER, ARG0, ARG1, ARG2, ARG3, ARG4)

	
	if ARG3 == ET_Player and ARG3 == ET_Baby then
		if ARG4 ~= 0 then
			Player.add_Reputation(RECEIVER,-40);
		end
	end
	
	Player.add_Reputation(RECEIVER,20);
end

function PlayerDie(RECEIVER, ARG0)

	Player.add_Reputation(RECEIVER,-40);
end

function PlayerFlee(RECEIVER, ARG0)
	Player.add_Reputation(RECEIVER,-20);
end

function PlayerUseSkill(RECEIVER,ARG0,ARG1)
	if ARG0 ~= 0 then
		Player.add_Reputation(RECEIVER,20);
	end
end

--打竞技场
function PlayerEnterJJC(RECEIVER,ARG0,ARG1)
	Player.reduce_questcounter(RECEIVER,90006);
end

--打造装备
function PlayerMakeEquip(RECEIVER,ARG0,ARG1)
	--Player.reduce_questcounter(RECEIVER,7);
	if ARG1 == 1 then
		Player.add_questcounter(RECEIVER,90011);
	end
end

--招募伙伴
function PlayerRecruitEmp(RECEIVER,ARG0,ARG1)
	--Player.reduce_questcounter(RECEIVER,20002);
end
--获得宠物完成任务
function PlayerAddBaby(RECEIVER,ARG0,ARG1,ARG2,ARG3)
	if ARG3 == false then
		return
	end
	if ARG0 == 9 then
		Player.add_questcounter(RECEIVER,6);
	end
	if ARG0 == 30 then
		Player.add_questcounter(RECEIVER,20002);
	end
	if ARG0 == 54 then
		Player.add_questcounter(RECEIVER,20013);
	end
	if ARG0 == 10030 then
		Player.add_questcounter(RECEIVER,20029);
	end
	if ARG0 == 10015 then
		Player.add_questcounter(RECEIVER,20033);
	end
	if ARG0 == 10157 then
		Player.add_questcounter(RECEIVER,20046);
	end
	if ARG0 == 54 then
		Player.add_questcounter(RECEIVER,10005);
	end
	if ARG0 == 33 then
		Player.add_questcounter(RECEIVER,10009);
	end
	if ARG0 == 51 then
		Player.add_questcounter(RECEIVER,60013);
	end
	if ARG0 == 10030 then
		Player.add_questcounter(RECEIVER,60014);
	end
	if ARG0 == 10091 then
		Player.add_questcounter(RECEIVER,60015);
	end
	if ARG0 == 10068 then
		Player.add_questcounter(RECEIVER,60016);
	end
	if ARG0 == 54 then
		Player.add_questcounter(RECEIVER,50013);
	end
	if ARG0 == 28 then
		Player.add_questcounter(RECEIVER,90101);
	end
	if ARG0 == 17 then
		Player.add_questcounter(RECEIVER,90102);
	end
	if ARG0 == 1000 then
		Player.add_questcounter(RECEIVER,90103);
	end
	if ARG0 == 61 then
		Player.add_questcounter(RECEIVER,90104);
	end
	if ARG0 == 10073 then
		Player.add_questcounter(RECEIVER,90105);
	end
	if ARG0 == 10011 then
		Player.add_questcounter(RECEIVER,90106);
	end
	if ARG0 == 10113 then
		Player.add_questcounter(RECEIVER,90107);
	end
	if ARG0 == 10014 then
		Player.add_questcounter(RECEIVER,90108);
	end
	if ARG0 == 10072 then
		Player.add_questcounter(RECEIVER,90109);
	end
	if ARG0 == 10123 then
		Player.add_questcounter(RECEIVER,90110);
	end
end
--删除宠物
function PlayerDelBaby(RECEIVER,ARG0,ARG1)
	if ARG0 == 9 then
		Player.reduce_questcounter(RECEIVER,6);
	end
	if ARG0 == 30 then
		Player.reduce_questcounter(RECEIVER,20002);
	end
	if ARG0 == 54 then
		Player.reduce_questcounter(RECEIVER,20013);
	end
	if ARG0 == 10030 then
		Player.reduce_questcounter(RECEIVER,20029);
	end
	if ARG0 == 10015 then
		Player.reduce_questcounter(RECEIVER,20033);
	end
	if ARG0 == 10157 then
		Player.reduce_questcounter(RECEIVER,20046);
	end
	if ARG0 == 54 then
		Player.reduce_questcounter(RECEIVER,10005);
	end
	if ARG0 == 33 then
		Player.reduce_questcounter(RECEIVER,10009);
	end
	if ARG0 == 51 then
		Player.reduce_questcounter(RECEIVER,60013);
	end
	if ARG0 == 10030  then
		Player.reduce_questcounter(RECEIVER,60014);
	end
	if ARG0 == 10091 then
		Player.reduce_questcounter(RECEIVER,60015);
	end
	if ARG0 == 10068 then
		Player.reduce_questcounter(RECEIVER,60016);
	end
	if ARG0 == 54 then
		Player.reduce_questcounter(RECEIVER,50013);
	end
	
end
--宠物存仓库
function PlayerDepositBaby(RECEIVER,ARG0,ARG1)
	if ARG0 == 9 then
		Player.reduce_questcounter(RECEIVER,6);
	end
	if ARG0 == 30 then
		Player.reduce_questcounter(RECEIVER,20002);
	end
	if ARG0 == 54 then
		Player.reduce_questcounter(RECEIVER,20013);
	end
	if ARG0 == 10030 then
		Player.reduce_questcounter(RECEIVER,20029);
	end
	if ARG0 == 10015 then
		Player.reduce_questcounter(RECEIVER,20033);
	end
	if ARG0 == 10157 then
		Player.reduce_questcounter(RECEIVER,20046);
	end
	if ARG0 == 54 then
		Player.reduce_questcounter(RECEIVER,10005);
	end
	if ARG0 == 33 then
		Player.reduce_questcounter(RECEIVER,10009);
	end
	if ARG0 == 51 then
		Player.reduce_questcounter(RECEIVER,60013);
	end
	if ARG0 == 10030 then
		Player.reduce_questcounter(RECEIVER,60014);
	end
	if ARG0 == 10091 then
		Player.reduce_questcounter(RECEIVER,60015);
	end
	if ARG0 == 10068 then
		Player.reduce_questcounter(RECEIVER,60016);
	end
	if ARG0 == 28 then
		Player.reduce_questcounter(RECEIVER,90101);
	end
	if ARG0 == 17 then
		Player.reduce_questcounter(RECEIVER,90102);
	end
	if ARG0 == 1000 then
		Player.reduce_questcounter(RECEIVER,90103);
	end
	if ARG0 == 61 then
		Player.reduce_questcounter(RECEIVER,90104);
	end
	if ARG0 == 10073 then
		Player.reduce_questcounter(RECEIVER,90105);
	end
	if ARG0 == 10011 then
		Player.reduce_questcounter(RECEIVER,90106);
	end
	if ARG0 == 10113 then
		Player.reduce_questcounter(RECEIVER,90107);
	end
	if ARG0 == 10014 then
		Player.reduce_questcounter(RECEIVER,90108);
	end
	if ARG0 == 10072 then
		Player.reduce_questcounter(RECEIVER,90109);
	end
	if ARG0 == 10123 then
		Player.reduce_questcounter(RECEIVER,90110);
	end
end

function PlayerLevelUp(RECEIVER,ARG0)
	local lv = Player.get_property(RECEIVER,PT_Level)
	if lv <= 20 then
		Player.autoprop(RECEIVER)
	end
	--世界等级
	local lv = Player.get_property(RECEIVER,PT_Level)
	local world_lv = Sys.get_world_level()
	checkWorldLevelBuff(RECEIVER,lv,world_lv)
	Player.addcoursegift(RECEIVER)
	--开启功能
	--[[
		10级开启商城、打造
		15级开启好友
		18级开启神器
		20级开启排行、离线竞技场、日常任务、英雄任务
		21级开启双倍经验
		20-23级开启组队
		23级开启通缉任务
		25级开启采蘑菇
		25级开启家族
		27级开启宠物神殿
		30级开启百人
		35级开启野外战场
		35级在线竞技场
	]]--
	if ARG0 == 5 then
		Player.set_opensubsystem(RECEIVER,OSSF_Bar);
		Player.set_opensubsystem(RECEIVER,OSSF_Skill);
		Player.startonlinetime(RECEIVER);
		Player.sevenopen(RECEIVER);
	elseif ARG0 == 2 then
		Player.set_opensubsystem(RECEIVER,OSSF_Achieve); --成就
	elseif ARG0 == 6 then
		Player.set_opensubsystem(RECEIVER,OSSF_Shop);	--6级开启商城
	elseif ARG0 == 9 then
		--Player.set_opensubsystem(RECEIVER,OSSF_EmployeePos10);	--10级开启伙伴位置
		Player.add_Employee(RECEIVER, 2021)
		Player.setemployeebattlegroup(RECEIVER, 2021)
	elseif ARG0 == 10 then
		Player.accept_quest(RECEIVER,90011);
	elseif ARG0 == 12 then
		Player.set_opensubsystem(RECEIVER,OSSF_EmployeePos15);	--15级开启伙伴位置
		Player.set_opensubsystem(RECEIVER,OSSF_EmployeeGet);
		Player.set_opensubsystem(RECEIVER,OSSF_EmployeeList);
		Player.set_opensubsystem(RECEIVER,OSSF_EmployeePosition);
		Player.set_opensubsystem(RECEIVER,OSSF_EmployeeEquip);
	elseif ARG0 == 13 then
		Player.set_opensubsystem(RECEIVER,OSSF_Friend);	--15开启好友
	elseif ARG0 == 21 then
		Player.set_opensubsystem(RECEIVER,OSSF_Rank);		--21级开启排行榜
		Player.set_opensubsystem(RECEIVER,OSSF_JJC);
	elseif ARG0 == 22 then
		Player.set_opensubsystem(RECEIVER,OSSF_DoubleExp);	--22级开启双倍时间功能
	elseif ARG0 == 14 then
		Player.set_opensubsystem(RECEIVER,OSSF_Guid);	--14级攻略
	elseif ARG0 == 15 then
		Player.set_opensubsystem(RECEIVER,OSSF_EmployeePos20);	--20级开启伙伴位置
	elseif ARG0 == 20 then
		Player.openscene(RECEIVER,1100)
		Player.accept_quest(RECEIVER,90002);
		Player.set_opensubsystem(RECEIVER,OSSF_Family);	--25级开启家族
		Player.set_opensubsystem(RECEIVER,OSSF_EveryTask);	--20级日常任务
		Player.set_opensubsystem(RECEIVER,OSSF_Activity);	--20级开启活动
	elseif ARG0 == 23 then
		Player.set_opensubsystem(RECEIVER,OSSF_AuctionHouse);
		Player.accept_quest(RECEIVER,90007);
	elseif ARG0 == 24 then	
		Player.accept_quest(RECEIVER,90004);
	elseif ARG0 == 25 then
		Player.accept_quest(RECEIVER,90003);
	elseif ARG0 == 30 then
		Player.accept_quest(RECEIVER,90009);
		Player.add_item(RECEIVER,10051,3)
	elseif ARG0 == 32 then
		Player.accept_quest(RECEIVER,90005);
	elseif ARG0 == 35 then
		Player.set_opensubsystem(RECEIVER,OSSF_PVPJJC); --在线竞技场
		Player.set_opensubsystem(RECEIVER,OSSF_Hundred); --百人
	elseif ARG0 == 45 then
		Player.set_opensubsystem(RECEIVER,OSSF_PetEquip);
	elseif ARG0 == 60 then	
		Player.openscene(RECEIVER,402);
	elseif ARG0 == 61 then
		Player.set_opensubsystem(RECEIVER,OSSF_Cystal);
		Player.openCystal(RECEIVER)
	end
end

function PlayerBattleOver(RECEIVER,ARG0,ARG1,ARG2)
    GameEvent_Debug_Log("PlayerBattleOver "..RECEIVER.." "..ARG0);
	if ARG0 == 1 then
		Player.set_guide(RECEIVER,1)
	end
	zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
end

function TalkNpc(RECEIVER,ARG0)
	if Player.getnpctype(ARG0) == NT_SinglePK then
		baoxiang_talk(RECEIVER,ARG0)
	end
	caiji_talk(RECEIVER,ARG0)
	tongji_talk(RECEIVER,ARG0)
	fuben_talk(RECEIVER,ARG0)
	join_family_talk(RECEIVER,ARG0)
	talk_guild_progenitus(RECEIVER,ARG0)
	talk_npc_quest(RECEIVER,ARG0)
	if Player.getnpctype(ARG0) == NT_TeamPK then
		guaiwugongcheng_talk(RECEIVER,ARG0)
	end
	if Player.getnpctype(ARG0) == NT_Xiji then
		if Player.get_activation_counter(RECEIVER,ACT_Family_2) < 20 then
			bangpaihuodong_talk(RECEIVER,ARG0)
		else
			Player.send_errorno(RECEIVER,EN_TimeXiji);
		end
		
	end
	if Player.getnpctype(ARG0) == NT_Mogu then
		if Player.get_activation_counter(RECEIVER,ACT_Mogu) < 20 then
			mushroom_talk(RECEIVER,ARG0)
		else
			Player.send_errorno(RECEIVER,EN_TimeMushroom);
		end
	end
end

function fuben_talk(RECEIVER,ARG0)
	if ARG0 == 9570 then
		Activity.update(RECEIVER,ACT_Copy,1);
		local isenter = Player.checkteamLevel(RECEIVER,30)
		if isenter then
			Player.copyGo(RECEIVER,1001,1001)
		end
	end
	if ARG0 == 9571 then
		Activity.update(RECEIVER,ACT_Copy,1);
		local isenter = Player.checkteamLevel(RECEIVER,40)
		if isenter then
			Player.copyGo(RECEIVER,1003,1003)
		end
	end
	if ARG0 == 9580 then
		Activity.update(RECEIVER,ACT_Copy,1);
		local isenter = Player.checkteamLevel(RECEIVER,50)
		if isenter then
			Player.copyGo(RECEIVER,1005,1005)
		end
	end
	if ARG0 == 9581 then
		Activity.update(RECEIVER,ACT_Copy,1);
		local isenter = Player.checkteamLevel(RECEIVER,60)
		if isenter then
			Player.copyGo(RECEIVER,1006,1006)
		end
	end
	if ARG0 == 9582 then
		Activity.update(RECEIVER,ACT_Copy,1);
		local isenter = Player.checkteamLevel(RECEIVER,60)
		if isenter then
			Player.copyGo(RECEIVER,1007,1007)
		end
	end
	if ARG0 == 9583 then
		Activity.update(RECEIVER,ACT_Copy,1);
		local isenter = Player.checkteamLevel(RECEIVER,70)
		if isenter then
			Player.copyGo(RECEIVER,1008,1008)
		end
	end
end

--天降宝箱
function baoxiang_talk(RECEIVER,ARG0)
	if ARG0 >=  57001 and ARG0 <=  57045 then
		local index = math.ceil(math.random(1,100));
		if index < 20 then
			Player.add_item(RECEIVER,5039,1)
		elseif index < 25 and index >= 20 then
			Player.add_item(RECEIVER,5098,1)
		elseif index < 30 and index >= 25 then
			Player.add_item(RECEIVER,5099,1)
		elseif index < 50 and index >= 30 then
			Player.add_item(RECEIVER,5086,1)
		elseif index < 60 and index >= 50 then
			Player.add_item(RECEIVER,3805,1)
		elseif index < 80 and index >= 60 then
			Player.add_item(RECEIVER,102063,1)
		else
			Player.add_item(RECEIVER,102061,1)
		end
		Activity.update(RECEIVER,ACT_AlonePK,1);
		Player.delnpc(RECEIVER,ARG0)
	end
end

function caiji_talk(RECEIVER,ARG0)
	if ARG0 >=  1500 and ARG0 <=  1502 then
		Player.add_item(RECEIVER,7030,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1503 and ARG0 <= 1505 then
		Player.add_item(RECEIVER,7031,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1506 and ARG0 <= 1508 then
		Player.add_item(RECEIVER,7032,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1509 and ARG0 <= 1511 then
		Player.add_item(RECEIVER,7033,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1512 and ARG0 <= 1514 then
		Player.add_item(RECEIVER,7034,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1515 and ARG0 <= 1517 then
		Player.add_item(RECEIVER,7035,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1518 and ARG0 <= 1520 then
		Player.add_item(RECEIVER,7036,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1521 and ARG0 <= 1523 then
		Player.add_item(RECEIVER,7037,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1524 and ARG0 <= 1526 then
		Player.add_item(RECEIVER,7038,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 >= 1527 and ARG0 <= 1529 then
		Player.add_item(RECEIVER,7039,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	
	if ARG0 == 56063 or ARG0 == 56066 or ARG0 == 56069 then
		Player.add_item(RECEIVER,21363,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 == 56064 or ARG0 == 56067 or ARG0 == 56070 then
		Player.add_item(RECEIVER,21363,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
	if ARG0 == 56065 or ARG0 == 56068 or ARG0 == 56071 then
		Player.add_item(RECEIVER,21363,1)
		Player.delprivnpc(RECEIVER,ARG0)
	end
end

function join_family_talk(RECEIVER,ARG0)
	if ARG0 == 20001 then
		--Sys.join_guild_battle_scene(RECEIVER)
	end
end

function talk_guild_progenitus(RECEIVER,ARG0)
	if ARG0 >= 60000 and ARG0 <= 60063 then
		Sys.talked_guild_progenitus(RECEIVER,ARG0)
	end
end

--蘑菇活动对话脚本
function mushroom_talk(RECEIVER,ARG0)
	Activity.update(RECEIVER,ACT_Mogu,1);
	local teamid = Player.isteamleader(RECEIVER)
	local lv = Player.get_property(RECEIVER,PT_Level)
	if teamid ~= 0 then
		lv = Team.get_level(teamid)
	end
	if ARG0 >= 56031 and ARG0 <= 56045 then
		local index = math.ceil(math.random(1,100))
		if index <= 20 then
			local teamid1 = Player.isteamleader(RECEIVER)
			if teamid1 ~= 0 then
				local n0,n1,n2,n3,n4 = Team.get_teammembers(teamid1);
				Player.add_money(n0,1000)
				Player.add_money(n1,1000)
				Player.add_money(n2,1000)
				Player.add_money(n3,1000)
				Player.add_money(n4,1000)
			else
				Player.add_money(RECEIVER,1000)
			end
			--Player.send_errorno(RECEIVER,EN_ItemMushroom);
		elseif index > 20 and index <= 80 then
				if lv < 20 then
					return
				elseif lv >= 20 and lv < 25 then
					Player.joinbattle(RECEIVER,3000);
				elseif lv >= 25 and lv < 30 then
					Player.joinbattle(RECEIVER,3001);
				elseif lv >= 30 and lv < 35 then
					Player.joinbattle(RECEIVER,3002);
				elseif lv >= 35 and lv < 40 then
					Player.joinbattle(RECEIVER,3003);
				elseif lv >= 40 and lv < 45 then
					Player.joinbattle(RECEIVER,3004);
				elseif lv >= 45 and lv < 50 then
					Player.joinbattle(RECEIVER,3005);
				elseif lv >= 50 and lv < 55 then
					Player.joinbattle(RECEIVER,3006);
				elseif lv >= 55 and lv <= 60 then
					Player.joinbattle(RECEIVER,3007);
				else
					Player.joinbattle(RECEIVER,3000);
				end
		elseif index > 80 and index <= 100 then
			local teamid = Player.isteamleader(RECEIVER)
			if teamid ~= 0 then
				local m0,m1,m2,m3,m4 = Team.get_teammembers(teamid);
				
				local hp = Player.get_property(m0,PT_HpCurr)
				local mp = Player.get_property(m0,PT_MpCurr)
				Player.change_property(m0,0,PT_HpCurr,-hp/2);
				Player.change_property(m0,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m1,PT_HpCurr)
				local mp = Player.get_property(m1,PT_MpCurr)
				Player.change_property(m1,0,PT_HpCurr,-hp/2);
				Player.change_property(m1,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m2,PT_HpCurr)
				local mp = Player.get_property(m2,PT_MpCurr)
				Player.change_property(m2,0,PT_HpCurr,-hp/2);
				Player.change_property(m2,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m3,PT_HpCurr)
				local mp = Player.get_property(m3,PT_MpCurr)
				Player.change_property(m3,0,PT_HpCurr,-hp/2);
				Player.change_property(m3,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m4,PT_HpCurr)
				local mp = Player.get_property(m4,PT_MpCurr)
				Player.change_property(m4,0,PT_HpCurr,-hp/2);
				Player.change_property(m4,0,PT_MpCurr,-mp/2);
			else
				local hp = Player.get_property(RECEIVER,PT_HpCurr)
				local mp = Player.get_property(RECEIVER,PT_MpCurr)
				Player.change_property(RECEIVER,0,PT_HpCurr,-hp/2);
				Player.change_property(RECEIVER,0,PT_MpCurr,-mp/2);
			end
		Player.send_errorno(RECEIVER,EN_BadMushroom);
		end
		Player.delnpc(RECEIVER,ARG0)
		--Player.add_activation_counter(RECEIVER,ACT_Mogu)
		
	elseif ARG0 >= 56131 and ARG0 <= 56145 then
		local index = math.ceil(math.random(1,100))
		if index <= 20 then
			local teamid1 = Player.isteamleader(RECEIVER)
			if teamid1 ~= 0 then
				local n0,n1,n2,n3,n4 = Team.get_teammembers(teamid1);
				Player.add_money(n0,1000)
				Player.add_money(n1,1000)
				Player.add_money(n2,1000)
				Player.add_money(n3,1000)
				Player.add_money(n4,1000)
			else
				Player.add_money(RECEIVER,1000)
			end
			--Player.send_errorno(RECEIVER,EN_ItemMushroom);
		elseif index > 20 and index <= 80 then
				if lv < 20 then
					return
				elseif lv >= 20 and lv < 25 then
					Player.joinbattle(RECEIVER,3000);
				elseif lv >= 25 and lv < 30 then
					Player.joinbattle(RECEIVER,3001);
				elseif lv >= 30 and lv < 35 then
					Player.joinbattle(RECEIVER,3002);
				elseif lv >= 35 and lv < 40 then
					Player.joinbattle(RECEIVER,3003);
				elseif lv >= 40 and lv < 45 then
					Player.joinbattle(RECEIVER,3004);
				elseif lv >= 45 and lv < 50 then
					Player.joinbattle(RECEIVER,3005);
				elseif lv >= 50 and lv < 55 then
					Player.joinbattle(RECEIVER,3006);
				elseif lv >= 55 and lv <= 60 then
					Player.joinbattle(RECEIVER,3007);
				else
					Player.joinbattle(RECEIVER,3000);
				end
		elseif index > 80 and index <= 100 then
			local teamid = Player.isteamleader(RECEIVER)
			if teamid ~= 0 then
				local m0,m1,m2,m3,m4 = Team.get_teammembers(teamid);
				
				local hp = Player.get_property(m0,PT_HpCurr)
				local mp = Player.get_property(m0,PT_MpCurr)
				Player.change_property(m0,0,PT_HpCurr,-hp/2);
				Player.change_property(m0,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m1,PT_HpCurr)
				local mp = Player.get_property(m1,PT_MpCurr)
				Player.change_property(m1,0,PT_HpCurr,-hp/2);
				Player.change_property(m1,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m2,PT_HpCurr)
				local mp = Player.get_property(m2,PT_MpCurr)
				Player.change_property(m2,0,PT_HpCurr,-hp/2);
				Player.change_property(m2,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m3,PT_HpCurr)
				local mp = Player.get_property(m3,PT_MpCurr)
				Player.change_property(m3,0,PT_HpCurr,-hp/2);
				Player.change_property(m3,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m4,PT_HpCurr)
				local mp = Player.get_property(m4,PT_MpCurr)
				Player.change_property(m4,0,PT_HpCurr,-hp/2);
				Player.change_property(m4,0,PT_MpCurr,-mp/2);
			else
				local hp = Player.get_property(RECEIVER,PT_HpCurr)
				local mp = Player.get_property(RECEIVER,PT_MpCurr)
				Player.change_property(RECEIVER,0,PT_HpCurr,-hp/2);
				Player.change_property(RECEIVER,0,PT_MpCurr,-mp/2);
			end
		Player.send_errorno(RECEIVER,EN_BadMushroom);
		end
		Player.delnpc(RECEIVER,ARG0)
		--Player.add_activation_counter(RECEIVER,ACT_Mogu)
		
	elseif ARG0 >= 56231 and ARG0 <= 56345 then
		local index = math.ceil(math.random(1,100))
		if index <= 20 then
			local teamid1 = Player.isteamleader(RECEIVER)
			if teamid1 ~= 0 then
				local n0,n1,n2,n3,n4 = Team.get_teammembers(teamid1);
				Player.add_money(n0,1000)
				Player.add_money(n1,1000)
				Player.add_money(n2,1000)
				Player.add_money(n3,1000)
				Player.add_money(n4,1000)
			else
				Player.add_money(RECEIVER,1000)
			end
			--Player.send_errorno(RECEIVER,EN_ItemMushroom);
		elseif index > 20 and index <= 80 then
				if lv < 20 then
					return
				elseif lv >= 20 and lv < 25 then
					Player.joinbattle(RECEIVER,3000);
				elseif lv >= 25 and lv < 30 then
					Player.joinbattle(RECEIVER,3001);
				elseif lv >= 30 and lv < 35 then
					Player.joinbattle(RECEIVER,3002);
				elseif lv >= 35 and lv < 40 then
					Player.joinbattle(RECEIVER,3003);
				elseif lv >= 40 and lv < 45 then
					Player.joinbattle(RECEIVER,3004);
				elseif lv >= 45 and lv < 50 then
					Player.joinbattle(RECEIVER,3005);
				elseif lv >= 50 and lv < 55 then
					Player.joinbattle(RECEIVER,3006);
				elseif lv >= 55 and lv <= 60 then
					Player.joinbattle(RECEIVER,3007);
				else
					Player.joinbattle(RECEIVER,3000);
				end
		elseif index > 80 and index <= 100 then
			local teamid = Player.isteamleader(RECEIVER)
			if teamid ~= 0 then
				local m0,m1,m2,m3,m4 = Team.get_teammembers(teamid);
				
				local hp = Player.get_property(m0,PT_HpCurr)
				local mp = Player.get_property(m0,PT_MpCurr)
				Player.change_property(m0,0,PT_HpCurr,-hp/2);
				Player.change_property(m0,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m1,PT_HpCurr)
				local mp = Player.get_property(m1,PT_MpCurr)
				Player.change_property(m1,0,PT_HpCurr,-hp/2);
				Player.change_property(m1,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m2,PT_HpCurr)
				local mp = Player.get_property(m2,PT_MpCurr)
				Player.change_property(m2,0,PT_HpCurr,-hp/2);
				Player.change_property(m2,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m3,PT_HpCurr)
				local mp = Player.get_property(m3,PT_MpCurr)
				Player.change_property(m3,0,PT_HpCurr,-hp/2);
				Player.change_property(m3,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m4,PT_HpCurr)
				local mp = Player.get_property(m4,PT_MpCurr)
				Player.change_property(m4,0,PT_HpCurr,-hp/2);
				Player.change_property(m4,0,PT_MpCurr,-mp/2);
			else
				local hp = Player.get_property(RECEIVER,PT_HpCurr)
				local mp = Player.get_property(RECEIVER,PT_MpCurr)
				Player.change_property(RECEIVER,0,PT_HpCurr,-hp/2);
				Player.change_property(RECEIVER,0,PT_MpCurr,-mp/2);
			end
		Player.send_errorno(RECEIVER,EN_BadMushroom);
		end
		Player.delnpc(RECEIVER,ARG0)
		--Player.add_activation_counter(RECEIVER,ACT_Mogu)

	elseif ARG0 >= 56331 and ARG0 <= 56345 then
		local index = math.ceil(math.random(1,100))
		if index <= 20 then
			local teamid1 = Player.isteamleader(RECEIVER)
			if teamid1 ~= 0 then
				local n0,n1,n2,n3,n4 = Team.get_teammembers(teamid1);
				Player.add_money(n0,1000)
				Player.add_money(n1,1000)
				Player.add_money(n2,1000)
				Player.add_money(n3,1000)
				Player.add_money(n4,1000)
			else
				Player.add_money(RECEIVER,1000)
			end
		elseif index > 20 and index <= 80 then
				if lv < 20 then
					return
				elseif lv >= 20 and lv < 25 then
					Player.joinbattle(RECEIVER,3008);
				elseif lv >= 25 and lv < 30 then
					Player.joinbattle(RECEIVER,3009);
				elseif lv >= 30 and lv < 35 then
					Player.joinbattle(RECEIVER,3010);
				elseif lv >= 35 and lv < 40 then
					Player.joinbattle(RECEIVER,3011);
				elseif lv >= 40 and lv < 45 then
					Player.joinbattle(RECEIVER,3012);
				elseif lv >= 45 and lv < 50 then
					Player.joinbattle(RECEIVER,3013);
				elseif lv >= 50 and lv < 55 then
					Player.joinbattle(RECEIVER,3014);
				elseif lv >= 55 and lv <= 60 then
					Player.joinbattle(RECEIVER,3015);
				else
					Player.joinbattle(RECEIVER,3000);
				end
		elseif index > 80 and index <= 100 then
			local teamid = Player.isteamleader(RECEIVER)
			if teamid ~= 0 then
				local m0,m1,m2,m3,m4 = Team.get_teammembers(teamid);
				
				local hp = Player.get_property(m0,PT_HpCurr)
				local mp = Player.get_property(m0,PT_MpCurr)
				Player.change_property(m0,0,PT_HpCurr,-hp/2);
				Player.change_property(m0,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m1,PT_HpCurr)
				local mp = Player.get_property(m1,PT_MpCurr)
				Player.change_property(m1,0,PT_HpCurr,-hp/2);
				Player.change_property(m1,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m2,PT_HpCurr)
				local mp = Player.get_property(m2,PT_MpCurr)
				Player.change_property(m2,0,PT_HpCurr,-hp/2);
				Player.change_property(m2,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m3,PT_HpCurr)
				local mp = Player.get_property(m3,PT_MpCurr)
				Player.change_property(m3,0,PT_HpCurr,-hp/2);
				Player.change_property(m3,0,PT_MpCurr,-mp/2);
				
				local hp = Player.get_property(m4,PT_HpCurr)
				local mp = Player.get_property(m4,PT_MpCurr)
				Player.change_property(m4,0,PT_HpCurr,-hp/2);
				Player.change_property(m4,0,PT_MpCurr,-mp/2);
			else
				local hp = Player.get_property(RECEIVER,PT_HpCurr)
				local mp = Player.get_property(RECEIVER,PT_MpCurr)
				Player.change_property(RECEIVER,0,PT_HpCurr,-hp/2);
				Player.change_property(RECEIVER,0,PT_MpCurr,-mp/2);
			end
		end

		Player.delnpc(RECEIVER,ARG0)
		--Player.add_activation_counter(RECEIVER,ACT_Mogu)
	end
end


--通缉任务对话脚本
function tongji_talk(RECEIVER,ARG0)
	--npc是6001的时候
	if ARG0 == 6001 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2001);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2016);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2031);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2046);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2061);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2076);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2091);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2106);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2121);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20946);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21051);
		else
			Player.joinbattle(RECEIVER,2001);
		end
	end
	
	if ARG0 == 6002 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2002);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2017);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2032);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2047);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2062);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2077);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2092);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2107);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2122);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20947);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21052);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end
	
	if ARG0 == 6003 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2003);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2018);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2033);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2048);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2063);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2078);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2093);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2108);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2123);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20948);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21053);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
    if ARG0 == 6004 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2004);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2019);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2034);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2049);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2064);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2079);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2094);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2109);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2124);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20949);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21054);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6005 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2005);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2020);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2035);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2050);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2065);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2080);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2095);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2110);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2125);
	    elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20950);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21055);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6006 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2006);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2021);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2036);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2051);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2066);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2081);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2096);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2111);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2126);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20951);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21056);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6007 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2007);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2022);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2037);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2052);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2067);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2082);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2097);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2112);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2127);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20952);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21057);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6008 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2008);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2023);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2038);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2053);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2068);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2083);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2098);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2113);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2128);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20953);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21058);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6009 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2009);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2024);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2039);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2054);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2069);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2084);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2099);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2114);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2129);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20954);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21059);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6010 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2010);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2025);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2040);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2055);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2070);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2085);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2100);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2115);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2130);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20955);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21060);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6011 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2011);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2026);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2041);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2056);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2071);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2086);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2101);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2116);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2131);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20956);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21061);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6012 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2012);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2027);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2042);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2057);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2072);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2087);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2102);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2117);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2132);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20957);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21062);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6013 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2013);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2028);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2043);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2058);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2073);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2088);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2103);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2118);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2133);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20958);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21063);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6014 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2014);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2029);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2044);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2059);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2074);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2089);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2104);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2119);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2134);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20959);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21064);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6015 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2015);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2030);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2045);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2060);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2075);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2090);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2105);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2120);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2135);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20960);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21065);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6016 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2136);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2151);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2166);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2181);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2196);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2211);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2226);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2241);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2256);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20961);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21066);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6017 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2137);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2152);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2167);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2182);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2197);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2212);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2227);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2242);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2257);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20962);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21067);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6018 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2138);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2153);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2168);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2183);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2198);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2213);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2228);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2243);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2258);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20963);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21068);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6019 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2139);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2154);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2169);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2184);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2199);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2214);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2229);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2244);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2259);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20964);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21069);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6020 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2140);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2155);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2170);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2185);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2200);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2215);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2230);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2245);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2260);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20965);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21070);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6021 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2141);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2156);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2171);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2186);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2201);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2216);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2231);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2246);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2261);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20966);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21071);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6022 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2142);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2157);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2172);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2187);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2202);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2217);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2232);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2247);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2262);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20967);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21072);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6023 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2143);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2158);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2173);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2188);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2203);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2218);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2233);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2248);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2263);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20968);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21073);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6024 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2144);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2159);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2174);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2189);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2204);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2219);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2234);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2249);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2264);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20969);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21074);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6025 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2145);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2160);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2175);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2190);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2205);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2220);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2235);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2250);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2265);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20970);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21075);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6026 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2146);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2161);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2176);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2191);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2206);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2221);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2236);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2251);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2266);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20971);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21076);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6027 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2147);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2162);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2177);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2192);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2207);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2222);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2237);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2252);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2267);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20972);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21077);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6028 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2148);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2163);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2178);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2193);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2208);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2223);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2238);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2253);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2268);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20973);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21078);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6029 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2149);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2164);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2179);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2194);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2209);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2224);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2239);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2254);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2269);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20974);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21079);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
	
	if ARG0 == 6030 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 15 then
			return
		elseif lv >= 15 and lv < 20 then
			Player.joinbattle(RECEIVER,2150);
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,2165);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,2180);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,2195);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,2210);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,2225);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,2240);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,2255);
		elseif lv >= 55 and lv < 60 then
			Player.joinbattle(RECEIVER,2270);
		elseif lv >= 60 and lv < 65 then
			Player.joinbattle(RECEIVER,20975);
		elseif lv >= 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,21080);
		else
			Player.joinbattle(RECEIVER,2001);
		end
		
	end	
end

--怪物攻城脚本
function guaiwugongcheng_talk(RECEIVER,ARG0)
	Activity.update(RECEIVER,ACT_TeamPK,1);
	if ARG0 == 56048 or ARG0 == 56148 or ARG0 == 56248 or ARG0 == 56348 or ARG0 == 56448 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3016);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3031);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3046);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3061);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3076);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3091);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3106);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3121);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3256);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3271);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end
	
	if ARG0 == 56049 or ARG0 == 56149 or ARG0 == 56249 or ARG0 == 56349 or ARG0 == 56449 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3017);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3032);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3047);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3062);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3077);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3092);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3107);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3122);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3257);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3272);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56050 or ARG0 == 56150 or ARG0 == 56250 or ARG0 == 56350 or ARG0 == 56450 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3018);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3033);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3048);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3063);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3078);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3093);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3108);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3123);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3258);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3273);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56051 or ARG0 == 56151 or ARG0 == 56251 or ARG0 == 56351 or ARG0 == 56451 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3019);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3034);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3049);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3064);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3079);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3094);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3109);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3124);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3259);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3274);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56052 or ARG0 == 56152 or ARG0 == 56252 or ARG0 == 56352 or ARG0 == 56452 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3020);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3035);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3050);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3065);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3080);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3095);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3110);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3125);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3260);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3275);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56053 or ARG0 == 56153 or ARG0 == 56253 or ARG0 == 56353 or ARG0 == 56453 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3021);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3036);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3051);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3066);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3081);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3096);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3111);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3126);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3261);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3276);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56054 or ARG0 == 56154 or ARG0 == 56254 or ARG0 == 56354 or ARG0 == 56454 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3022);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3037);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3052);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3067);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3082);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3097);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3112);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3127);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3262);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3277);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56055 or ARG0 == 56155 or ARG0 == 56255 or ARG0 == 56355 or ARG0 == 56455 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3023);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3038);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3053);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3068);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3083);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3098);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3113);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3128);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3263);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3278);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56056 or ARG0 == 56156 or ARG0 == 56256 or ARG0 == 56356 or ARG0 == 56456 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3024);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3039);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3054);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3069);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3084);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3099);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3114);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3129);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3264);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3279);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56057 or ARG0 == 56157 or ARG0 == 56257 or ARG0 == 56357 or ARG0 == 56457 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3025);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3040);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3055);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3070);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3085);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3100);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3115);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3130);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3265);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3280);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56058 or ARG0 == 56158 or ARG0 == 56258 or ARG0 == 56358 or ARG0 == 56458 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3026);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3041);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3056);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3071);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3086);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3101);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3116);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3131);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3266);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3281);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56059 or ARG0 == 56159 or ARG0 == 56259 or ARG0 == 56359 or ARG0 == 56459 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3027);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3042);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3057);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3072);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3087);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3102);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3117);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3132);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3267);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3282);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56060 or ARG0 == 56160 or ARG0 == 56260 or ARG0 == 56360 or ARG0 == 56460 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3028);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3043);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3058);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3073);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3088);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3103);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3118);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3133);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3268);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3283);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56061 or ARG0 == 56161 or ARG0 == 56261 or ARG0 == 56361 or ARG0 == 56461 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3029);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3044);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3059);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3074);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3089);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3104);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3119);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3134);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3269);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3284);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
	
	if ARG0 == 56062 or ARG0 == 56162 or ARG0 == 56262 or ARG0 == 56362 or ARG0 == 56462 then
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3030);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3045);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3060);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3075);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3090);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3105);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3120);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3135);
		elseif lv > 60 and lv <= 65 then
			Player.joinbattle(RECEIVER,3270);
		elseif lv > 65 and lv <= 70 then
			Player.joinbattle(RECEIVER,3285);
		else
			Player.joinbattle(RECEIVER,3016);
		end
		Player.delnpc(RECEIVER,ARG0)
		
	end	
end

function zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	--主角自动回血回魔
	local playerinstid = Player.getPlayerInstId(RECEIVER)
	local hpstore,hpstoremax = Entity.get_state_value(RECEIVER,1000);
	if hpstore ~= 0 and playerinstid ~= 0 then
		local hpmax = Player.get_property(RECEIVER,PT_HpMax)
		local hpcurr = Player.get_property(RECEIVER,PT_HpCurr)
		if hpmax - hpcurr <= hpstore then --//如果缺失的血量 比 存储的小
			Player.change_property(RECEIVER,playerinstid,PT_HpCurr,hpmax - hpcurr);
			--成就
			Player.addAchuevementValue(RECEIVER,AT_Blood,hpmax - hpcurr);
			Entity.set_state_value(RECEIVER,1000,hpcurr - hpmax + hpstore,hpstoremax);
			local hpstore1,hpstoremax1 = Entity.get_state_value(RECEIVER,1000);
			if hpstore1 <= 0 then
				Entity.remove_state(RECEIVER,1000)
			end
		else
			Player.change_property(RECEIVER,playerinstid,PT_HpCurr,hpstore);
			--成就
			Player.addAchuevementValue(RECEIVER,AT_Blood,hpstore);
			Entity.set_state_value(RECEIVER,1000,0,hpstoremax);
			local hpstore1,hpstoremax1 = Entity.get_state_value(RECEIVER,1000);
			if hpstore1 <= 0 then
				Entity.remove_state(RECEIVER,1000)
			end
		end
	end
	--宠物
	hpstore,hpstoremax = Entity.get_state_value(RECEIVER,1000);
	if ARG2 == nil or hpstore <= 0 then
		return
	end
	for i=1,table.getn(ARG2),1
		do
			local babyhpmax = Player.getBabyProp(RECEIVER,ARG2[i],PT_HpMax)
			local babyhpcurr = Player.getBabyProp(RECEIVER,ARG2[i],PT_HpCurr)
			if babyhpmax - babyhpcurr <= hpstore then
				Player.change_property(RECEIVER,ARG2[i],PT_HpCurr,babyhpmax - babyhpcurr);
				Player.addAchuevementValue(RECEIVER,AT_Blood,babyhpmax - babyhpcurr);
				Entity.set_state_value(RECEIVER,1000,babyhpcurr - babyhpmax + hpstore,hpstoremax);
				local hpstore1,hpstoremax1 = Entity.get_state_value(RECEIVER,1000);
				if hpstore1 <= 0 then
					Entity.remove_state(RECEIVER,1000)
				end
			else
				Player.change_property(RECEIVER,ARG2[i],PT_HpCurr,hpstore);
				--成就
				Player.addAchuevementValue(RECEIVER,AT_Blood,hpstore);
				Entity.set_state_value(RECEIVER,1000,0,hpstoremax);
				local hpstore1,hpstoremax1 = Entity.get_state_value(RECEIVER,1000);
				if hpstore1 <= 0 then
					Entity.remove_state(RECEIVER,1000)
				end
			end
		end
end

function zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	--主角自动回血回魔
	local playerinstid = Player.getPlayerInstId(RECEIVER)
	local mpstore,mpstoremax = Entity.get_state_value(RECEIVER,1001);
	if mpstore ~= 0 and playerinstid ~= 0 then
		local mpmax = Player.get_property(RECEIVER,PT_MpMax)
		local mpcurr = Player.get_property(RECEIVER,PT_MpCurr)
		if mpmax - mpcurr <= mpstore then
			Player.change_property(RECEIVER,playerinstid,PT_MpCurr,mpmax - mpcurr);
			--成就
			Player.addAchuevementValue(RECEIVER,AT_Magic,mpmax - mpcurr);
			Entity.set_state_value(RECEIVER,1001,mpcurr - mpmax + mpstore,mpstoremax);
			local mpstore1,mpstoremax1 = Entity.get_state_value(RECEIVER,1001);
			if mpstore1 <= 0 then
				Entity.remove_state(RECEIVER,1001)
			end
		else
			Player.change_property(RECEIVER,playerinstid,PT_MpCurr,mpstore);
			--成就
			Player.addAchuevementValue(RECEIVER,AT_Magic,mpstore);
			Entity.set_state_value(RECEIVER,1001,0,mpstoremax);
			local mpstore1,mpstoremax1 = Entity.get_state_value(RECEIVER,1001);
			if mpstore1 <= 0 then
				Entity.remove_state(RECEIVER,1001)
			end
		end
	end
	--宠物
	mpstore,mpstoremax = Entity.get_state_value(RECEIVER,1001);
	if ARG2 == nil or mpstore <= 0 then
		return
	end
	for i=1,table.getn(ARG2),1
		do
			local babympmax = Player.getBabyProp(RECEIVER,ARG2[i],PT_MpMax)
			local babympcurr = Player.getBabyProp(RECEIVER,ARG2[i],PT_MpCurr)
			if babympmax - babympcurr <= mpstore then
				Player.change_property(RECEIVER,ARG2[i],PT_MpCurr,babympmax - babympcurr);
				Player.addAchuevementValue(RECEIVER,AT_Magic,babympmax - babympcurr);
				Entity.set_state_value(RECEIVER,1001,babympcurr - babympmax + mpstore,mpstoremax);
				local mpstore1,mpstoremax1 = Entity.get_state_value(RECEIVER,1001);
				if mpstore1 <= 0 then
					Entity.remove_state(RECEIVER,1001)
				end
			else
				Player.change_property(RECEIVER,ARG2[i],PT_MpCurr,mpstore);
				--成就
				Player.addAchuevementValue(RECEIVER,AT_Magic,mpstore);
				Entity.set_state_value(RECEIVER,1001,0,mpstoremax);
				local mpstore1,mpstoremax1 = Entity.get_state_value(RECEIVER,1001);
				if mpstore1 <= 0 then
					Entity.remove_state(RECEIVER,1001)
				end
			end
		end
end

--逃跑成功事件
function PlayerFlee(RECEIVER,ARG0)
	zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
end

function wishingGO(RECEIVER,ARG0)
	Sys.log("wishingGO==========="..ARG0);
	if ARG0 == WIT_Exp then
		local index = math.ceil(math.random(10000,20000));
		Player.add_Exp(RECEIVER,index)
		Player.del_item(RECEIVER,5508,1)
		Player.send_errorno(RECEIVER,EN_AddMoney5W);
	elseif ARG0 == WIT_Money then
		local index = math.ceil(math.random(5000,10000));
		Player.add_money(RECEIVER,index)
		Player.del_item(RECEIVER,5508,1)
		Player.send_errorno(RECEIVER,EN_AddDionmand500);
	elseif ARG0 == WIT_Baby then
		if Player.get_bag_free_slot(RECEIVER) <= 1 then
			return EN_OpenBaoXiangBagFull;
		end
	    local baby_table = {4509,4510,1759,1760,1761,1762,1763}
	    local index = math.ceil(math.random(1,7));
	    Player.add_item(RECEIVER,baby_table[index],1)
		Player.del_item(RECEIVER,5508,1)
	elseif ARG0 == WIT_Employee then
		if Player.get_bag_free_slot(RECEIVER) <= 1 then
			return EN_OpenBaoXiangBagFull;
		end
	    local Employee = {102001,102002,102003,102004,102005,102006,102007,102008,102009,102010,102011,102012,102013,102014,102015,102016,102017,102018,102019,102020,102021,102022,102023,102024,102025,102026,102027,102028,102029,102030,102031,102032,102033,102034,102035,102036,102037,102038,102039,102040,102041,102042,102043,102044,102045,102046,102047,102048,102049,102050,102051,102052,102053,102054,102055,102056,102057,102058,102059,102060}
	    local index = math.ceil(math.random(1,60));
		local index1 = math.ceil(math.random(1,10));
		Player.add_item(RECEIVER,Employee[index],index1)
		Player.del_item(RECEIVER,5508,1)
	end
end

function guild_battle_open(RECEIVER,ARG0)
end

function guild_battle_close(RECEIVER,ARG0,ARG1)
	if 1 == ARG1 then
		Sys.send_guild_mail(ARG0,"系统","家族战胜利","恭喜你们在家族战中战胜敌人获得辉煌的胜利，现发送国王的奖励",100,100000,"5044,3");
	else 
		Sys.send_guild_mail(ARG0,"系统","家族战失败","你们在家族战中暂时失利，现发送国王的鼓励",20,50000,"5044,1");
	end
end



--帮派活动
function bangpaihuodong_talk(RECEIVER,ARG0)
	--Activity.update(RECEIVER,ACT_Family_2,1);
	if ARG0 == 70001 or ARG0 == 70017 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3516);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3531);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3546);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3561);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3576);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3591);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3606);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3621);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end
	
	if ARG0 == 70002 or ARG0 == 70018 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3517);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3532);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3547);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3562);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3577);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3592);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3607);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3622);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70003 or ARG0 == 70019 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3518);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3533);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3548);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3563);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3578);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3593);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3608);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3623);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70004 or ARG0 == 70020 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3519);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3534);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3549);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3564);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3579);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3594);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3609);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3624);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70005 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3520);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3535);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3550);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3565);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3580);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3595);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3610);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3625);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70006 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3521);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3536);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3551);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3566);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3581);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3596);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3611);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3626);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70007 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3522);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3537);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3552);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3567);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3582);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3597);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3612);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3627);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70008 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3523);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3538);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3553);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3568);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3583);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3598);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3613);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3628);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70009 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3524);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3539);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3554);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3569);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3584);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3599);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3614);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3629);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70010 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3525);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3540);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3555);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3570);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3585);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3600);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3615);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3630);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70011 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3526);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3541);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3556);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3571);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3586);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3601);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3616);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3631);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70012 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3527);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3542);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3557);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3572);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3587);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3602);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3617);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3632);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70013 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3528);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3543);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3558);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3573);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3588);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3603);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3618);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3633);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70014 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3529);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3544);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3559);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3574);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3589);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3604);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3619);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3634);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	
	
	if ARG0 == 70015 then
		Activity.update(RECEIVER,ACT_Family_2,1);
		local teamid = Player.isteamleader(RECEIVER)
		local lv = Player.get_property(RECEIVER,PT_Level)
		if teamid ~= 0 then
			lv = Team.get_level(teamid)
		end
		if lv < 20 then
			return
		elseif lv >= 20 and lv < 25 then
			Player.joinbattle(RECEIVER,3530);
		elseif lv >= 25 and lv < 30 then
			Player.joinbattle(RECEIVER,3545);
		elseif lv >= 30 and lv < 35 then
			Player.joinbattle(RECEIVER,3560);
		elseif lv >= 35 and lv < 40 then
			Player.joinbattle(RECEIVER,3575);
		elseif lv >= 40 and lv < 45 then
			Player.joinbattle(RECEIVER,3590);
		elseif lv >= 45 and lv < 50 then
			Player.joinbattle(RECEIVER,3605);
		elseif lv >= 50 and lv < 55 then
			Player.joinbattle(RECEIVER,3620);
		elseif lv >= 55 and lv <= 60 then
			Player.joinbattle(RECEIVER,3635);
		else
			Player.joinbattle(RECEIVER,3516);
		end
	end	

end

function update_activity_pet(RECEIVER)
	--Player.add_activation_counter(RECEIVER,ACT_Pet)
end

function Recharge(RECEIVER,ARG0,ARG1)
	local playername = Player.getplayerName(RECEIVER)
	if ARG0 == 5 and ARG1 == 1 then
		sys_send_mail("系统",playername,"VIP充值奖励","恭喜您获得会员充值奖励300钻石",0,0,"5047,1")
		--Sys.vipitem("系统","VIP每日礼包","恭喜您获得今日VIP礼包！");
	elseif ARG0 == 5 and ARG1 == 2 then
		sys_send_mail("系统",playername,"VIP充值奖励","恭喜您获得高级会员充值奖励600钻石",0,0,"5050,1")
	end
end

function rejust_phonenumber(RECEIVER)
	local playername = Player.getplayerName(RECEIVER)
	Sys.log("绑定手机号"..playername)
	sys_send_mail("系统",playername,"恭喜手机号绑定成功","恭喜手机号绑定成功",0,0,"5045,1")
end


--就职
function jiuzhi(RECEIVER,ARG0,ARG1,ARG2,ARG3)
	if ARG0 == JT_Newbie then
		--Player.add_item(RECEIVER,5033,1)
	end
end


--对话任务
function talk_npc_quest(RECEIVER,ARG0)
	if ARG0 == 1 then
		Player.add_questcounter(RECEIVER,51001);
	elseif ARG0 == 2 then
		Player.add_questcounter(RECEIVER,51002);
	elseif ARG0 == 3 then
		Player.add_questcounter(RECEIVER,51003);
	elseif ARG0 == 4 then
		Player.add_questcounter(RECEIVER,51004);
	elseif ARG0 == 5 then
		Player.add_questcounter(RECEIVER,51005);
	elseif ARG0 == 6 then
		Player.add_questcounter(RECEIVER,51006);
	elseif ARG0 == 7 then
		Player.add_questcounter(RECEIVER,51007);
	elseif ARG0 == 8 then
		Player.add_questcounter(RECEIVER,51008);
	elseif ARG0 == 9 then
		Player.add_questcounter(RECEIVER,51009);
	elseif ARG0 == 10 then
		Player.add_questcounter(RECEIVER,51010);
	elseif ARG0 == 207 then
		Player.add_questcounter(RECEIVER,51011);
	elseif ARG0 == 200 then
		Player.add_questcounter(RECEIVER,51012);
	elseif ARG0 == 9562 then
		Player.add_questcounter(RECEIVER,51013);
	elseif ARG0 == 9563 then
		Player.add_questcounter(RECEIVER,51014);
	elseif ARG0 == 9564 then
		Player.add_questcounter(RECEIVER,51015);
	elseif ARG0 == 9565 then
		Player.add_questcounter(RECEIVER,51016);
	elseif ARG0 == 9566 then
		Player.add_questcounter(RECEIVER,51017);
	elseif ARG0 == 9567 then
		Player.add_questcounter(RECEIVER,51018);	
	elseif ARG0 == 9568 then
		Player.add_questcounter(RECEIVER,51019);
	elseif ARG0 == 9572 then
		Player.add_questcounter(RECEIVER,51020);		
	end
end

function BabyLevelUp_Addpoint(RECEIVER,ARG0,ARG1,ARG2)
	if ARG2 == 1 and ARG1 <= 20 then
		Player.addBabyProp(RECEIVER,ARG0,PT_Strength,1)
	end
end