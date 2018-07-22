Sys.log("load MonsterEvent.lua");

--RECEIVER
--ARG_BATTLEID,		//0
--ARG_CASTERPOS,		//1
--ARG_TARGETPOS,		//2
--ARG_POSTABLE,		//3
--ARG_SKILLTABLE		//4


function AI_born_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

	--偷袭不发战报
	if Battle.getSneakAttack(ARG0) == SAT_SneakAttack then
		return
	end
	
	local num = table.maxn(ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if num == 0 then

		return;
	end
	local index = math.ceil(math.random(1,num));
	if ARG4[index] == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,random_pos,1);
		end
		return
	end
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	end

	if ARG4[index] == 2511 then
		local Etype = Entity.getType(RECEIVER)
		if Etype == ET_Emplyee then
			Battle.ai_pushOrder(RECEIVER,random_pos,8);
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,1);		
		return
	end
	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

function AI_update_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	print("MonsterEvent SKILL TABLE----"..table.concat(ARG4, ", "))
	local num = table.maxn(ARG4);
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if num == 0 then
		return;
	end
	local index = math.ceil(math.random(1,num));
	
	if ARG4[index] == 2 then
		local index1 = math.ceil(math.random(1,100));
		if index1 <= 10 then
			Battle.ai_pushOrder(RECEIVER,ARG1,2);
		else
			Battle.ai_pushOrder(RECEIVER,random_pos,1);
		end
		return
	end
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
	end

	if ARG4[index] == 2511 then
		local Etype = Entity.getType(RECEIVER)
		if Etype == ET_Emplyee then
			Battle.ai_pushOrder(RECEIVER,random_pos,8);
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,1);
		return
	end
	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

function test_deadth_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)

end


--伙伴初始AI
function AI_huoban_event_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--偷袭不发战报
	if Battle.getSneakAttack(ARG0) == SAT_SurpriseAttack then
		return
	end
	local num = table.maxn(ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if num == 0 then

		return;
	end
	local index = math.ceil(math.random(1,num));
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		random_pos = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG1,false)
	end

	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

--伙伴AI
function AI_huoban_event_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local num = table.maxn(ARG4);
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	if num == 0 then
		return;
	end
	local index = math.ceil(math.random(1,num));
	if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
		--选择自己阵营的一个随机目标
		random_pos = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG1,false)
	end

	Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
end

--新手战斗AI
function AI_guid_gongjian_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9001);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9012);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,9018);
	end
end

function AI_guid_fashi_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9016);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9002);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,9022);
	end
end

function AI_guid_chuanjiao_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9005);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9010);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,9020);
	end
end

function AI_guid_zhanfu_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9003);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,12);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,9009);
	end
end
function AI_guid_renzhe_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9007);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9004);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,9011);
	end
end

function AI_guid_zhoushu_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,BP_Down4,1);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,12);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,12);
	end
end

function AI_guid_wushi_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9019);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,12);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,12);
	end
end

function AI_guid_qishi_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG1,1021);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,BP_Down0,9011);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,1);
	end
end
function AI_guid_gedou_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) == 0 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9007);
	elseif Battle.getRound(ARG0) == 1 then
		Battle.ai_pushOrder(RECEIVER,ARG2,9012);
	else
		Battle.ai_pushOrder(RECEIVER,ARG2,1);
	end
end



function robot_born_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	Battle.ai_pushOrder(RECEIVER,random_pos,1);
end

function robot_update_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	Battle.ai_pushOrder(RECEIVER,random_pos,1);
end


--伙伴2动AI
--传教
function chuanjiao_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao_Updata(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function chuanjiao_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao_Updata(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao_Updata(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end

function chuanjiao1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao1_Updata(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function chuanjiao1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao1_Updata(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ChuanJiao1_Updata(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--法师
function fashi_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function fashi_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end

function fashi1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function fashi1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	FaShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--格斗
function gedou_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function gedou_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end

function gedou1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function gedou1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GeDou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--弓手
function gongshou_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function gongshou_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function gongshou1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function gongshou1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	GongShou1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--剑士
function jianshi_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function jianshi_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function jianshi1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function jianshi1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	JianShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--骑士
function qishi_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function qishi_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function qishi1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function qishi_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	QiShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--忍者
function renzhe_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	RenZhe_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	RenZhe_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function renzhe_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	RenZhe_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	RenZhe_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--巫师
function wushi_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function wushi_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function wushi1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function wushi1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	WuShi1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--战斧
function zhanfu_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function zhanfu_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function zhanfu1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function zhanfu1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhanFu1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
--咒术
function zhoushu_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function zhoushu_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function zhoushu1_2_born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu1_Born(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function zhoushu1_2_update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	ZhouShu1_Update(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end

--通用小怪
function AI_born_event_2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	AI_born_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	AI_update_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end
function AI_update_event_2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	AI_update_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	AI_update_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
end


--副本AI
function AI_fuben_50(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getRound(ARG0) >= 25 then
		Battle.ai_pushOrder(RECEIVER,ARG2,5501);
		AI_update_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	else
		AI_born_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		AI_update_event(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	end
end



function zhaohuanziji_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,table.maxn(ARG4)));
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	
	if table.maxn(my_table) < 2
	then
		Battle.ai_pushOrder(RECEIVER,ARG1,5514);
	else
		while ARG4[index] == 5514
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		--随机使用除了召唤以外的所有技能
		if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
			--选择自己阵营的一个随机目标
			random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
	monsterpushOrder(RECEIVER,TargetPos);
end

function zhaohuanjingying_UpdatePushOrder(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,table.maxn(ARG4)));
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	
	if table.maxn(my_table) < 2
	then
		Battle.ai_pushOrder(RECEIVER,ARG1,5516);
	else
		while ARG4[index] == 5516
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		--随机使用除了召唤以外的所有技能
		if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
			--选择自己阵营的一个随机目标
			random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
	monsterpushOrder(RECEIVER,TargetPos);
end


function xiaoguaiduo(RECEIVER,ARG0,ARG1,ARG2,ARG3,ARG4)
	local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
	local index = math.ceil(math.random(1,table.maxn(ARG4)));
	local my_table = get_opposite_pos(ARG0,ARG3,ARG1);
	
	if table.maxn(my_table) >= 6
	then
		Battle.ai_pushOrder(RECEIVER,ARG1,5501);
	else
		while ARG4[index] == 5501
		do
			index = math.ceil(math.random(1,table.maxn(ARG4)));
		end
		--随机使用除了召唤以外的所有技能
		if Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_TeamNoSelf or Battle.getSkill_TargetType(ARG0,ARG4[index]) == STT_Team then
			--选择自己阵营的一个随机目标
			random_pos = random_TargetPos(ARG0, ARG2, ARG1, ARG3, false)
		end
		Battle.ai_pushOrder(RECEIVER,random_pos,ARG4[index]);
	end
	monsterpushOrder(RECEIVER,TargetPos);
end