function QuestAccept(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.add_item(RECEIVER,3506,2);

end

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
	--宠物任务
function QuestSubmit_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.set_opensubsystem(RECEIVER,OSSF_Baby);
end

function QuestSubmit_5555(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,9)
	Player.del_baby(RECEIVER,instID);
	Player.errorhint(RECEIVER,EN_DelBaby1000)
end

function QuestSubmit_20002(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,30)
	Player.del_baby(RECEIVER,instID);
	Player.errorhint(RECEIVER,EN_DelBaby30)
end

function QuestSubmit_20013(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,54)
	Player.del_baby(RECEIVER,instID);
	Player.errorhint(RECEIVER,EN_DelBaby54)
end

function QuestSubmit_20029(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,10030)
	Player.del_baby(RECEIVER,instID);
end

function QuestSubmit_20033(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,10015)
	Player.del_baby(RECEIVER,instID);
end	

function QuestSubmit_20046(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,10157)
	Player.del_baby(RECEIVER,instID);
end	

function QuestSubmit_10005(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,54)
	Player.del_baby(RECEIVER,instID);
end

function QuestSubmit_10009(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,33)
	Player.del_baby(RECEIVER,instID);
end

function QuestSubmit_60013(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,51)
	Player.del_baby(RECEIVER,instID);
end

function QuestSubmit_60014(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,10030)
	Player.del_baby(RECEIVER,instID);
end

function QuestSubmit_60015(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,10091)
	Player.del_baby(RECEIVER,instID);
end

function QuestSubmit_60016(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,10068)
	Player.del_baby(RECEIVER,instID);
end

function QuestSubmit_50013(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local instID = Player.get_minlevel_baby(RECEIVER,54)
	Player.del_baby(RECEIVER,instID);
	Player.addGuildContribution(RECEIVER,10)
	Player.addGuildMoney(RECEIVER,100)
end
--
function QuestAccept_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.add_item(RECEIVER,5002,5);
end

--开启功能
function QuestSubmit_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	--Player.set_opensubsystem(RECEIVER,OSSF_Bar);
	--Player.set_opensubsystem(RECEIVER,OSSF_Skill);
end

function QuestAccept_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.openscene(RECEIVER,2)
	Player.openscene(RECEIVER,20)
end

function QuestSubmit_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	--Player.set_opensubsystem(RECEIVER,OSSF_Skill);
end

function QuestAccept_28(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.openMagic(RECEIVER);
	Player.set_opensubsystem(RECEIVER,OSSF_MagicItem);
end

function QuestAccept_jiuzhi(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	
end

function QuestAccept_18(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.set_opensubsystem(RECEIVER,OSSF_Make);
end

function QuestSubmit_18(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.openscene(RECEIVER,3)
	Player.openscene(RECEIVER,30)
end

function QuestSubmit_68(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.openscene(RECEIVER,4)
	Player.openscene(RECEIVER,40)
end

function QuestSubmit_140(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.openscene(RECEIVER,5)
end

function QuestSubmit_182(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.openscene(RECEIVER,402)
end

function QuestAccept_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.set_opensubsystem(RECEIVER,OSSF_BabyLeranSkill);
end

function QuestAccept_20(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	--Player.accept_quest(RECEIVER,90006);
end

function QuestSubmit_jiuzhi(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	--Player.set_opensubsystem(RECEIVER,OSSF_Baby);
	if Player.get_property(RECEIVER,PT_Level) < 10 then
		Player.forget_skill(RECEIVER,9008)
		Player.errorhint(RECEIVER,EN_DelDefaultSkill)
	end
	local pro = Player.get_property(RECEIVER,PT_Profession)
	Player.add_money(RECEIVER,2000)
	if pro == JT_Axe then
		Player.learn_skill(RECEIVER,1001)
		Player.learn_skill(RECEIVER,1061)
	elseif pro == JT_Archer then
		Player.learn_skill(RECEIVER,1041)
		Player.learn_skill(RECEIVER,1061)
	elseif pro == JT_Mage then
		Player.learn_skill(RECEIVER,2001)
		Player.learn_skill(RECEIVER,2031)
	elseif pro == JT_Sage then
		Player.learn_skill(RECEIVER,2361)
		Player.learn_skill(RECEIVER,2391)
	end

end

function QuestAccept_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	--Player.set_opensubsystem(RECEIVER,OSSF_EmployeeGet);
	--Player.set_opensubsystem(RECEIVER,OSSF_EmployeeList);
	--Player.set_opensubsystem(RECEIVER,OSSF_EmployeePosition);
	--Player.set_opensubsystem(RECEIVER,OSSF_EmployeeEquip);
	Player.add_Employee(RECEIVER, 2033)
	Player.setemployeebattlegroup(RECEIVER, 2033)
end

function QuestAccept_19(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.set_opensubsystem(RECEIVER,OSSF_Team);
end

function QuestAccept_23(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.change_property(RECEIVER,0,PT_EmployeeCurrency,50);
end


function QuestAccept_caiji(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1500)
	Player.addprivnpc(RECEIVER,1501)
	Player.addprivnpc(RECEIVER,1502)
end

--采集NPC
function QuestSubmit_caiji(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1500)
	Player.delprivnpc(RECEIVER,1501)
	Player.delprivnpc(RECEIVER,1502)
end

function QuestAccept_caiji1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1503)
	Player.addprivnpc(RECEIVER,1504)
	Player.addprivnpc(RECEIVER,1505)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1503)
	Player.delprivnpc(RECEIVER,1504)
	Player.delprivnpc(RECEIVER,1505)
end

function QuestAccept_caiji2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1506)
	Player.addprivnpc(RECEIVER,1507)
	Player.addprivnpc(RECEIVER,1508)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1506)
	Player.delprivnpc(RECEIVER,1507)
	Player.delprivnpc(RECEIVER,1508)
end

function QuestAccept_caiji3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1509)
	Player.addprivnpc(RECEIVER,1510)
	Player.addprivnpc(RECEIVER,1511)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1509)
	Player.delprivnpc(RECEIVER,1510)
	Player.delprivnpc(RECEIVER,1511)
end

function QuestAccept_caiji4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1512)
	Player.addprivnpc(RECEIVER,1513)
	Player.addprivnpc(RECEIVER,1514)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1512)
	Player.delprivnpc(RECEIVER,1513)
	Player.delprivnpc(RECEIVER,1514)
end

function QuestAccept_caiji5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1515)
	Player.addprivnpc(RECEIVER,1516)
	Player.addprivnpc(RECEIVER,1517)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1515)
	Player.delprivnpc(RECEIVER,1516)
	Player.delprivnpc(RECEIVER,1517)
end

function QuestAccept_caiji6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1518)
	Player.addprivnpc(RECEIVER,1519)
	Player.addprivnpc(RECEIVER,1520)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1518)
	Player.delprivnpc(RECEIVER,1519)
	Player.delprivnpc(RECEIVER,1520)
end

function QuestAccept_caiji7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1521)
	Player.addprivnpc(RECEIVER,1522)
	Player.addprivnpc(RECEIVER,1523)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1521)
	Player.delprivnpc(RECEIVER,1522)
	Player.delprivnpc(RECEIVER,1523)
end

function QuestAccept_caiji8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1524)
	Player.addprivnpc(RECEIVER,1525)
	Player.addprivnpc(RECEIVER,1526)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1524)
	Player.delprivnpc(RECEIVER,1525)
	Player.delprivnpc(RECEIVER,1526)
end

function QuestAccept_caiji9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,1527)
	Player.addprivnpc(RECEIVER,1528)
	Player.addprivnpc(RECEIVER,1529)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,1527)
	Player.delprivnpc(RECEIVER,1528)
	Player.delprivnpc(RECEIVER,1529)
end

function QuestSubmit_caiji10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,56063)
	Player.delprivnpc(RECEIVER,56066)
	Player.delprivnpc(RECEIVER,56069)
	Activity.update(RECEIVER,ACT_Family_4,1);
end

function QuestAccept_caiji10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,56063)
	Player.addprivnpc(RECEIVER,56066)
	Player.addprivnpc(RECEIVER,56069)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji11(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,56064)
	Player.delprivnpc(RECEIVER,56067)
	Player.delprivnpc(RECEIVER,56070)
	Activity.update(RECEIVER,ACT_Family_4,1);
end

function QuestAccept_caiji11(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,56064)
	Player.addprivnpc(RECEIVER,56067)
	Player.addprivnpc(RECEIVER,56070)
	Player.checkquestitem(RECEIVER)
end

function QuestSubmit_caiji12(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.delprivnpc(RECEIVER,56065)
	Player.delprivnpc(RECEIVER,56068)
	Player.delprivnpc(RECEIVER,56071)
	Activity.update(RECEIVER,ACT_Family_4,1);
end

function QuestAccept_caiji12(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addprivnpc(RECEIVER,56065)
	Player.addprivnpc(RECEIVER,56068)
	Player.addprivnpc(RECEIVER,56071)
	Player.checkquestitem(RECEIVER)
end

--副本
function QuestAccept_fuben_30(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.copyGo(RECEIVER,1001,1002)
end

function QuestAccept_fuben_40(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.copyGo(RECEIVER,1003,1004)
end

function QuestSubmit_fuben_30(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addAchuevementValue(RECEIVER,AT_Copy30,1);
end


function QuestSubmit_fuben_40(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addAchuevementValue(RECEIVER,AT_Copy40,1);
end

function QuestSubmit_fuben_50(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addAchuevementValue(RECEIVER,AT_Copy50,1);
end

function QuestSubmit_fuben_601(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addAchuevementValue(RECEIVER,AT_Copy60,1);
end

function QuestSubmit_fuben_602(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addAchuevementValue(RECEIVER,AT_Copy60,1);
end

function QuestSubmit_fuben_70(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addAchuevementValue(RECEIVER,AT_Copy70,1);
end


function QuestAccept_xinshou(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	--Player.accept_quest(RECEIVER,90007)
end

--指引任务
function QuestAccept_xinshou1(RECEIVER, ARG0, ARG1, ARG2,ARG3)	
	local pro = Player.get_property(RECEIVER,PT_Profession)
	if pro == JT_Axe then
		Player.add_item(RECEIVER,1023,1)
		--Player.add_item(RECEIVER,1208,1)
		--Player.add_item(RECEIVER,1278,1);
	elseif pro == JT_Archer then
		Player.add_item(RECEIVER,1069,1)
		--Player.add_item(RECEIVER,1231,1)
		--Player.add_item(RECEIVER,1301,1);
	elseif pro == JT_Mage then
		Player.add_item(RECEIVER,1092,1)
		--Player.add_item(RECEIVER,1254,1)
		--Player.add_item(RECEIVER,1301,1);
	elseif pro == JT_Sage then
		Player.add_item(RECEIVER,1092,1)
		--Player.add_item(RECEIVER,1254,1)
		--Player.add_item(RECEIVER,1301,1);
	else	
		Player.add_item(RECEIVER,1023,1)
		--Player.add_item(RECEIVER,1208,1)
		--Player.add_item(RECEIVER,1278,1);
	end 
	Player.add_item(RECEIVER,5100,1)
	Player.add_item(RECEIVER,5101,1)
	local shuijing = {1351,1352,1353,1354}
	local index = math.ceil(math.random(1,4));
	Player.add_item(RECEIVER,shuijing[index],1)
end

function QuestAccept_xinshou2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	--Player.accept_quest(RECEIVER,2)
end

--许愿任务
function QuestSubmit_xuyuan(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.add_item(RECEIVER,5508,1);
end

--帮派任务奖励
function QuestSubmit_bangpai(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.addGuildContribution(RECEIVER,10)
	Player.addGuildMoney(RECEIVER,100)
	Activity.update(RECEIVER,ACT_Family_1,1);
end

function item_test(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Player.checkquestitem(RECEIVER)
end

--环任务
function quest_rand(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Activity.update(RECEIVER,ACT_Rand,1);
end
