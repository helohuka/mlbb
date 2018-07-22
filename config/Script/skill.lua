--RECEIVER
-- ARG_BATTLEID,		//0
-- ARG_CASTERPOS,		//1
-- ARG_TARGETPOS,		//2
-- ARG_POSTABLE,		//3
-- ARG_ORDERPARAM,		//4
-- ARG_MAX = 5
Sys.log("load skill.lua");
--script Sys.load_script "skill.lua"

local luanshe_num = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}

function Sk_1_GloAction(RECEIVER, ARG0, ARG1, ARG2, ARG3, ARG4, ARG5, ARG6, ARG7, ARG8, ARG9)
	nrm_raw(RECEIVER, ARG0, ARG1, ARG2);
end

function nrm_filter(RECEIVER, ARG0, ARG1, ARG2)
	
end

-- script Sys.load_script(skill.lua)  
--//普通攻击技能调用
--//RECEIVER CALLER 
--//ARG0	 BATTLE ID
--//ARG1	 TARGET POSTION
function nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	if Battle.getBattleDataID(ARG0) == 1 then
		if ARG1 == BP_Up3 then
			Battle.change_report_skill(ARG0, ARG1, 2511)
			skill_ansha(RECEIVER, ARG0, ARG1, ARG2,ARG3)
			return;
		end
		Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-1);
		return;
	end
	--判断是否合击
	if ARG4[OPT_Unite] >= 2 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		return;
	end
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	--判断目标是否可攻击
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if ARG4[OPT_IsNo] == 1 then
		random_pos = random_TargetPos(ARG0, ARG1, ARG2, ARG3, false)
	end
	if random_pos == BP_None then
		return;
	end
	--判断是否暗杀
	local check_ansha = check_ansha(RECEIVER, ARG0, ARG1, random_pos)
	if check_ansha then
		Battle.change_report_skill(ARG0, ARG1, 2511)
		skill_ansha(RECEIVER, ARG0, ARG1, random_pos,ARG3)
		return;
	end
	
	--判断目标是否被护卫
	if ARG4[OPT_Huwei] ~= 0 and Battle.get_prop(ARG0,ARG4[OPT_Huwei],PT_HpCurr) >= 1 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG4[OPT_Huwei], ARG3,ARG4)
		return
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		local weapon = get_hand_weapon(ARG0, ARG1);
		if weapon == WT_Bow or weapon == WT_Knife then
			Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
			Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1);
			return;
		end
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

function xiaodao_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断目标是否可攻击
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);

		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	
	local random_pos2 = random_pos +5
	local a = 0;
	local b = 0;
	if random_pos <= 10 and random_pos >= 1 then
		a = 10;
		b = 1;
	elseif random_pos <= 20 and random_pos >= 11 then
		a = 20;
		b = 11;
	else
		return false;
	end
	if random_pos2 >= b and random_pos2 <= a and ARG3[random_pos2] ~=0 and Battle.get_prop(ARG0,random_pos2,PT_HpCurr) > 0 then
		local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos2,0);
		--判断是否必杀
		local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos2)
		if damage == 0 then
			isbisha = false
		end
		if isbisha then
			local target_defense= Battle.get_prop(ARG0,random_pos2,PT_Defense);
			local target_Lv= Battle.get_prop(ARG0,random_pos2,PT_Level);
			local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
			damage = damage -target_defense * caster_Lv / (target_Lv * 2);
		end
		--是否攻击反弹
		local check_gongfan = Battle.check_state(ARG0,random_pos2,ST_ActionBounce);
		if check_gongfan then
			Battle.change_prop(ARG0,random_pos2,PT_HpCurr,0);

			Battle.cutTime_state(ARG0,random_pos2,ST_ActionBounce,1)
			return;
		end
		Battle.change_prop(ARG0,random_pos2,PT_HpCurr,damage,isbisha);
		remove_hunshui(RECEIVER, ARG0,random_pos2,damage)
	end
	local random_pos1 = random_pos -5
	if random_pos1 >= b and random_pos1 <= a and ARG3[random_pos1] ~=0 and Battle.get_prop(ARG0,random_pos1,PT_HpCurr) > 0 then
		local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos1,0);
		--判断是否必杀
		local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos1)
		if damage == 0 then
			isbisha = false
		end
		if isbisha then
			local target_defense= Battle.get_prop(ARG0,random_pos1,PT_Defense);
			local target_Lv= Battle.get_prop(ARG0,random_pos1,PT_Level);
			local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
			damage = damage -target_defense * caster_Lv / (target_Lv * 2);
		end
		--是否攻击反弹
		local check_gongfan = Battle.check_state(ARG0,random_pos1,ST_ActionBounce);
		if check_gongfan then
			Battle.change_prop(ARG0,random_pos1,PT_HpCurr,0);

			Battle.cutTime_state(ARG0,random_pos1,ST_ActionBounce,1)
			return;
		end
		Battle.change_prop(ARG0,random_pos1,PT_HpCurr,damage,isbisha);
		remove_hunshui(RECEIVER, ARG0,random_pos1,damage)
	end
end


--混乱调用的普通攻击
function hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local pos_table = get_all_pos(ARG0,ARG3,ARG1);
	local random_index = math.ceil(math.random(1,table.getn(pos_table)));
	local random_pos = pos_table[random_index];
	if random_pos == BP_None then
		return;
	end	
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--判断目标是否被护卫
	if ARG4[OPT_Huwei] ~= 0 and Battle.get_prop(ARG0,ARG4[OPT_Huwei],PT_HpCurr) >= 1 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG4[OPT_Huwei], ARG3,ARG4)
		return
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		local weapon = get_hand_weapon(ARG0, ARG1);
		if weapon == WT_Bow or weapon == WT_Knife then
			Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
			Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1);
			return;
		end
		weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

--法师混乱用技能
function hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local pos_table = get_all_pos(ARG0,ARG3,ARG1);
	local random_index = math.ceil(math.random(1,table.getn(pos_table)));
	local random_pos = pos_table[random_index];
	if random_pos == BP_None then
		return;
	end	
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		local weapon = get_hand_weapon(ARG0, ARG1);
		if weapon == WT_Bow or weapon == WT_Knife then
			Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
			Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1);
			return;
		end
		weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

--攻击技能武器判断
function weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	--如果是伙伴则根据职业push普通攻击
	local Etype = Entity.getType(RECEIVER)
	if Etype == ET_Emplyee then
		local profesion = Battle.get_prop(ARG0,ARG1,PT_Profession)
		if profesion == JT_Axe or profesion == JT_Sword or profesion == JT_Ninja or profesion == JT_Mage or profesion == JT_Sage or profesion == JT_Wizard or profesion == JT_Word then
			Battle.change_order_skill(ARG0, ARG1, 8)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return
		elseif profesion == JT_Knight then
			Battle.change_order_skill(ARG0, ARG1, 9)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return
		elseif profesion == JT_Knight then
			Battle.change_order_skill(ARG0, ARG1, 10)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return
		elseif profesion == JT_Fighter then
			Battle.change_order_skill(ARG0, ARG1, 1)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return
		else
			return
		end
	end
	local selfweapon = get_hand_weapon(ARG0, ARG1);
	if selfweapon == WT_Bow then
		Battle.change_report_skill(ARG0, ARG1, 10)
	elseif selfweapon == WT_Knife then
		Battle.change_report_skill(ARG0, ARG1, 11)
	elseif selfweapon == WT_Axe or selfweapon == WT_Sword or selfweapon == WT_Stick then
		Battle.change_report_skill(ARG0, ARG1, 8)
	elseif selfweapon == WT_Spear then
		Battle.change_report_skill(ARG0, ARG1, 9)
	else
		Battle.change_report_skill(ARG0, ARG1, 1)
	end
end

--合击调用普通攻击
function heji_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local damage = huwei_damage(RECEIVER, ARG0, ARG1, ARG2,ARG4[OPT_Unite])
	damage = damage * ((ARG4[OPT_Unite]-1)/10 + 1);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, ARG2)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,ARG2,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,ARG2,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,ARG2,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,ARG2,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,ARG2,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,ARG2,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,ARG2,damage)
end


--暗杀
function skill_ansha(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	if Battle.getBattleDataID(ARG0) == 1 then
		local hp_max = Battle.get_prop(ARG0,ARG2,PT_HpMax);
		Battle.change_prop(ARG0,ARG2,PT_HpCurr,-hp_max);
		return;
	end
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local hp_max = Battle.get_prop(ARG0,random_pos,PT_HpMax);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,-hp_max);
end


--反击调取的普通攻击
function skill_fanji_atk(RECEIVER, ARG0, caster_pos, target_pos)
	local hp_curr = Battle.get_prop(ARG0,target_pos,PT_HpCurr);
	local hp_curr1 = Battle.get_prop(ARG0,caster_pos,PT_HpCurr);
	if hp_curr < 1 then
		return ;
	end
	if hp_curr1 < 1 then
		return ;
	end
	local damage = nrm_damage(RECEIVER, ARG0, caster_pos, target_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, caster_pos, target_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,target_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,target_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,caster_pos,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--判断攻反
	local check_gongfan = Battle.check_state(ARG0,target_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.add_Counter(caster_pos,ARG0,target_pos,PT_HpCurr,0,false);
		Battle.changeProp_fanji(ARG0,caster_pos,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,target_pos,ST_ActionBounce,1)
		remove_hunshui(RECEIVER, ARG0,target_pos,damage)
		fanji_damage(RECEIVER, ARG0, caster_pos, target_pos,0);
		return;
	end
	Battle.add_Counter(caster_pos,ARG0,target_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,target_pos,damage)
	fanji_damage(RECEIVER, ARG0, caster_pos, target_pos,0);
end


--//基础防御技能调用
--//RECEIVER CALLER
--//ARG0	 BATTLE ID
--//ARG1	 TARGET POSTION
function nrm_def(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,1,0);
end

--//攻击魔法防御
function nrm_magic_def(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,173,0);
end

--//圣盾
function skill_shengdun_1(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,23,0);
end
function skill_shengdun_2(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,24,0);
end
function skill_shengdun_3(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,25,0);
end
function skill_shengdun_4(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,26,0);
end
function skill_shengdun_5(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,27,0);
end
function skill_shengdun_6(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,28,0);
end
function skill_shengdun_7(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,29,0);
end
function skill_shengdun_8(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,30,0);
end
function skill_shengdun_9(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,31,0);
end
function skill_shengdun_10(RECEIVER, ARG0, ARG1, ARG2)
	Battle.insert_state(ARG0,ARG1,32,0);
end

--//攻击吸收
function skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end	
	--不能同时存在两种巫术
	local check_wu = check_wushu(RECEIVER, ARG0, ARG1, random_pos)
	if check_wu then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	Battle.insert_state(ARG0,random_pos,2+level,0);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

function skill_gongjixishou_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_gongjixishou_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_gongjixishou_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_gongjixishou_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_gongjixishou_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_gongjixishou_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_gongjixishou_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_gongjixishou_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_gongjixishou_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_gongjixishou_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjixishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--//魔法吸收
function skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end	
	--不能同时存在两种巫术
	local check_wu = check_wushu(RECEIVER, ARG0, ARG1, random_pos)
	if check_wu then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	Battle.insert_state(ARG0,random_pos,12+level,0);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

function skill_mofaxishou_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_mofaxishou_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_mofaxishou_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_mofaxishou_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_mofaxishou_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_mofaxishou_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_mofaxishou_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_mofaxishou_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_mofaxishou_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_mofaxishou_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofaxishou(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--//攻击无效
function skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end	
	--不能同时存在两种巫术
	local check_wu = check_wushu(RECEIVER, ARG0, ARG1, random_pos)
	if check_wu then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	Battle.insert_state(ARG0,random_pos,52+level,0);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

function skill_gongjiwuxiao_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_gongjiwuxiao_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_gongjiwuxiao_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_gongjiwuxiao_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_gongjiwuxiao_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_gongjiwuxiao_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_gongjiwuxiao_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_gongjiwuxiao_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_gongjiwuxiao_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_gongjiwuxiao_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjiwuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--//魔法无效
function skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end	
	--不能同时存在两种巫术
	local check_wu = check_wushu(RECEIVER, ARG0, ARG1, random_pos)
	if check_wu then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	Battle.insert_state(ARG0,random_pos,62+level,0);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

function skill_mofawuxiao_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_mofawuxiao_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_mofawuxiao_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_mofawuxiao_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_mofawuxiao_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_mofawuxiao_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_mofawuxiao_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_mofawuxiao_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_mofawuxiao_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_mofawuxiao_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofawuxiao(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--//攻击反弹
function skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end	
	--不能同时存在两种巫术
	local check_wu = check_wushu(RECEIVER, ARG0, ARG1, random_pos)
	if check_wu then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	Battle.insert_state(ARG0,random_pos,32+level,0);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

function skill_gongjifantan_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_gongjifantan_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_gongjifantan_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_gongjifantan_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_gongjifantan_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_gongjifantan_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_gongjifantan_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_gongjifantan_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_gongjifantan_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_gongjifantan_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_gongjifantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--//魔法反弹
function skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end	
	--不能同时存在两种巫术
	local check_wu = check_wushu(RECEIVER, ARG0, ARG1, random_pos)
	if check_wu then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	Battle.insert_state(ARG0,random_pos,42+level,0);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

function skill_mofafantan_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_mofafantan_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_mofafantan_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_mofafantan_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_mofafantan_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_mofafantan_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_mofafantan_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_mofafantan_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_mofafantan_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_mofafantan_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofafantan(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--咒术攻击
function skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level,chushiID)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断目标是否可攻击
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	--判断目标是否被护卫
	if ARG4[OPT_Huwei] ~= 0 and Battle.get_prop(ARG0,ARG4[OPT_Huwei],PT_HpCurr) >= 1 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG4[OPT_Huwei], ARG3,ARG4)
		return
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	if damage ~= 0 then
		--不能同时存在两种咒术
		local check_zhou = check_zhoushu(RECEIVER, ARG0, ARG1, random_pos)
		if check_zhou then
			Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
			fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
			return;
		else
			--计算抗性
			if chushiID == 92 then
				local du = Battle.get_prop(ARG0,random_pos,PT_NoPoison);
				level = level - math.ceil(du/10)
			elseif chushiID == 102 then
				local shihua = Battle.get_prop(ARG0,random_pos,PT_NoPetrifaction);
				level = level - math.ceil(shihua/10)
			elseif chushiID == 112 then
				local shui = Battle.get_prop(ARG0,random_pos,PT_NoSleep);
				level = level - math.ceil(shui/10)
			elseif chushiID == 122 then
				local hunluan = Battle.get_prop(ARG0,random_pos,PT_NoChaos);
				level = level - math.ceil(hunluan/10)
			elseif chushiID == 132 then
				local jiuzui = Battle.get_prop(ARG0,random_pos,PT_NoDrunk);
				level = level - math.ceil(jiuzui/10)
			end
			if level <= 0 then
				Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
				fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
				return;
			end
			local table_index = {35,35,40,40,45,45,50,50,55,55}
			local poison_index = math.ceil(math.random(1,100));
			if poison_index < table_index[level] then
				Battle.insert_state(ARG0,random_pos,chushiID+level,0);
			end
			Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
			fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
			return
		end
	else
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	end
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

--中毒攻击
function skill_zhongdu_atk_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1,92)
end
function skill_zhongdu_atk_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2,92)
end
function skill_zhongdu_atk_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3,92)
end
function skill_zhongdu_atk_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4,92)
end

--酒醉攻击
function skill_jiuzui_atk_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1,132)
end
function skill_jiuzui_atk_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2,132)
end
function skill_jiuzui_atk_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3,132)
end
function skill_jiuzui_atk_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4,132)
end

--昏睡攻击
function skill_hunshui_atk_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1,112)
end
function skill_hunshui_atk_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2,112)
end
function skill_hunshui_atk_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3,112)
end
function skill_hunshui_atk_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4,112)
end

--石化攻击
function skill_shihua_atk_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1,102)
end
function skill_shihua_atk_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2,102)
end
function skill_shihua_atk_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3,102)
end
function skill_shihua_atk_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4,102)
end

--混乱攻击
function skill_hunluan_atk_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1,122)
end
function skill_hunluan_atk_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2,122)
end
function skill_hunluan_atk_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3,122)
end
function skill_hunluan_atk_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4,122)
end


--遗忘攻击
function skill_yiwang_atk_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1,142)
end
function skill_yiwang_atk_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2,142)
end
function skill_yiwang_atk_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3,142)
end
function skill_yiwang_atk_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhoushu_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4,142)
end

--//单体咒术
function skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,level,chushiID)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end
	local table_index = {35,35,40,40,45,45,50,50,55,55}
	if chushiID == 102 or chushiID == 112 then
		table_index = {30,30,35,35,40,40,45,45,50,50}
	end
	--不能同时存在两种咒术
	local check_zhou = check_zhoushu(RECEIVER, ARG0, ARG1, random_pos)
	if check_zhou then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	--计算抗性
	if chushiID == 92 then
		local du = Battle.get_prop(ARG0,random_pos,PT_NoPoison);
		level = level - math.ceil(du/10)
	elseif chushiID == 102 then
		local shihua = Battle.get_prop(ARG0,random_pos,PT_NoPetrifaction);
		level = level - math.ceil(shihua/10)
	elseif chushiID == 112 then
		local shui = Battle.get_prop(ARG0,random_pos,PT_NoSleep);
		level = level - math.ceil(shui/10)
	elseif chushiID == 122 then
		local hunluan = Battle.get_prop(ARG0,random_pos,PT_NoChaos);
		level = level - math.ceil(hunluan/10)
	elseif chushiID == 132 then
		local jiuzui = Battle.get_prop(ARG0,random_pos,PT_NoDrunk);
		level = level - math.ceil(jiuzui/10)
	end
	if level <= 0 then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		return;
	end
	local poison_index = math.ceil(math.random(1,100));
	if poison_index < table_index[level] then
		Battle.insert_state(ARG0,random_pos,chushiID+level,0);
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

--中毒魔法
function skill_zhongdu_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,92)
end
function skill_zhongdu_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,92)
end
function skill_zhongdu_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,92)
end
function skill_zhongdu_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,92)
end
function skill_zhongdu_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,92)
end
function skill_zhongdu_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,92)
end
function skill_zhongdu_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,92)
end
function skill_zhongdu_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,92)
end
function skill_zhongdu_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,92)
end
function skill_zhongdu_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,92)
end

--//中毒掉血
function zhongdu_state(RECEIVER, ARG0, ARG1)
	local lv = Battle.get_prop(ARG0,ARG1,PT_Level);
	local damage = -((lv-1) * 4 + 30)*0.35
	local hp_curr = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	local hp_max = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	if hp_curr - ((lv-1) * 4 + 30)*0.35 < 1 then
		damage = 1 - hp_curr
	end
	Battle.changeProp_state(ARG0,ARG1,PT_HpCurr,damage);
end

--//酒醉魔法
function skill_jiuzui_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,132)
end
function skill_jiuzui_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,132)
end
function skill_jiuzui_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,132)
end
function skill_jiuzui_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,132)
end
function skill_jiuzui_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,132)
end
function skill_jiuzui_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,132)
end
function skill_jiuzui_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,132)
end
function skill_jiuzui_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,132)
end
function skill_jiuzui_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,132)
end
function skill_jiuzui_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,132)
end

--//酒醉掉魔
function jiuzui_state(RECEIVER, ARG0, ARG1)
	local mp_curr = Battle.get_prop(ARG0,ARG1,PT_MpCurr);
	local mp_max = Battle.get_prop(ARG0,ARG1,PT_MpMax);
	local damage = -mp_max/21
	if mp_curr - mp_max/21 < 1 then
		damage = mp_curr - 1
	end
	Battle.changeProp_state(ARG0,ARG1,PT_MpCurr,damage);
end


--//石化魔法
function skill_shihua_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,102)
end
function skill_shihua_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,102)
end
function skill_shihua_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,102)
end
function skill_shihua_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,102)
end
function skill_shihua_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,102)
end
function skill_shihua_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,102)
end
function skill_shihua_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,102)
end
function skill_shihua_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,102)
end
function skill_shihua_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,102)
end
function skill_shihua_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,102)
end


--//昏睡魔法
function skill_hunshui_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,112)
end
function skill_hunshui_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,112)
end
function skill_hunshui_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,112)
end
function skill_hunshui_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,112)
end
function skill_hunshui_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,112)
end
function skill_hunshui_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,112)
end
function skill_hunshui_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,112)
end
function skill_hunshui_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,112)
end
function skill_hunshui_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,112)
end
function skill_hunshui_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,112)
end

--混乱魔法
function skill_hunluan_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,122)
end
function skill_hunluan_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,122)
end
function skill_hunluan_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,122)
end
function skill_hunluan_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,122)
end
function skill_hunluan_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,122)
end
function skill_hunluan_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,122)
end
function skill_hunluan_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,122)
end
function skill_hunluan_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,122)
end
function skill_hunluan_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,122)
end
function skill_hunluan_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,122)
end

--遗忘魔法
function skill_yiwang_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,142)
end
function skill_yiwang_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,142)
end
function skill_yiwang_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,142)
end
function skill_yiwang_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,142)
end
function skill_yiwang_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,142)
end
function skill_yiwang_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,142)
end
function skill_yiwang_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,142)
end
function skill_yiwang_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,142)
end
function skill_yiwang_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,142)
end
function skill_yiwang_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,142)
end

--//全体咒术
function skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,level,chushiID)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local table_index = {20,20,23,23,26,26,29,29,32,32}
	if chushiID == 102 or chushiID == 112 then
		table_index = {15,15,18,18,21,21,24,24,27,27}
	end
	for i=1,table.getn(pos_table),1
		do
			--不能同时存在两种咒术
			local check_zhou = check_zhoushu(RECEIVER, ARG0, ARG1, pos_table[i])
			if check_zhou then
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
			else
				--计算抗性
				if chushiID == 92 then
					local du = Battle.get_prop(ARG0,pos_table[i],PT_NoPoison);
					level = level - math.ceil(du/10)
				elseif chushiID == 102 then
					local shihua = Battle.get_prop(ARG0,pos_table[i],PT_NoPetrifaction);
					level = level - math.ceil(shihua/10)
				elseif chushiID == 112 then
					local shui = Battle.get_prop(ARG0,pos_table[i],PT_NoSleep);
					level = level - math.ceil(shui/10)
				elseif chushiID == 122 then
					local hunluan = Battle.get_prop(ARG0,pos_table[i],PT_NoChaos);
					level = level - math.ceil(hunluan/10)
				elseif chushiID == 132 then
					local jiuzui = Battle.get_prop(ARG0,pos_table[i],PT_NoDrunk);
					level = level - math.ceil(jiuzui/10)
				end
				if level <= 0 then
					Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
				else
					local poison_index = math.ceil(math.random(1,100));
					if poison_index < table_index[level] then
						Battle.insert_state(ARG0,pos_table[i],chushiID+level,0);
					end
					Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
				end
			end
	end
end

--全体中毒
function skill_zhongdu_quanti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,92)
end
function skill_zhongdu_quanti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,92)
end
function skill_zhongdu_quanti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,92)
end
function skill_zhongdu_quanti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,92)
end
function skill_zhongdu_quanti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,92)
end
function skill_zhongdu_quanti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,92)
end
function skill_zhongdu_quanti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,92)
end
function skill_zhongdu_quanti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,92)
end
function skill_zhongdu_quanti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,92)
end
function skill_zhongdu_quanti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,92)
end

--//全体酒醉魔法
function skill_jiuzui_quanti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,132)
end
function skill_jiuzui_quanti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,132)
end
function skill_jiuzui_quanti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,132)
end
function skill_jiuzui_quanti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,132)
end
function skill_jiuzui_quanti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,132)
end
function skill_jiuzui_quanti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,132)
end
function skill_jiuzui_quanti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,132)
end
function skill_jiuzui_quanti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,132)
end
function skill_jiuzui_quanti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,132)
end
function skill_jiuzui_quanti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,132)
end

--//全体石化魔法
function skill_shihua_quanti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,102)
end
function skill_shihua_quanti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,102)
end
function skill_shihua_quanti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,102)
end
function skill_shihua_quanti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,102)
end
function skill_shihua_quanti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,102)
end
function skill_shihua_quanti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,102)
end
function skill_shihua_quanti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,102)
end
function skill_shihua_quanti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,102)
end
function skill_shihua_quanti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,102)
end
function skill_shihua_quanti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,102)
end


--//全体昏睡魔法
function skill_hunshui_quanti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,112)
end
function skill_hunshui_quanti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,112)
end
function skill_hunshui_quanti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,112)
end
function skill_hunshui_quanti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,112)
end
function skill_hunshui_quanti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,112)
end
function skill_hunshui_quanti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,112)
end
function skill_hunshui_quanti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,112)
end
function skill_hunshui_quanti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,112)
end
function skill_hunshui_quanti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,112)
end
function skill_hunshui_quanti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,112)
end

--全体混乱魔法
function skill_hunluan_quanti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,122)
end
function skill_hunluan_quanti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,122)
end
function skill_hunluan_quanti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,122)
end
function skill_hunluan_quanti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,122)
end
function skill_hunluan_quanti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,122)
end
function skill_hunluan_quanti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,122)
end
function skill_hunluan_quanti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,122)
end
function skill_hunluan_quanti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,122)
end
function skill_hunluan_quanti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,122)
end
function skill_hunluan_quanti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,122)
end

--全体遗忘魔法
function skill_yiwang_quanti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,142)
end
function skill_yiwang_quanti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,142)
end
function skill_yiwang_quanti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,142)
end
function skill_yiwang_quanti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,142)
end
function skill_yiwang_quanti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,142)
end
function skill_yiwang_quanti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,142)
end
function skill_yiwang_quanti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,142)
end
function skill_yiwang_quanti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,142)
end
function skill_yiwang_quanti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,142)
end
function skill_yiwang_quanti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,142)
end


--//强力咒术
function skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,level,chushiID)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local table_index = {20,20,25,25,30,30,35,35,40,40}
	if chushiID == 102 or chushiID == 112 then
		table_index = {15,15,20,20,25,25,30,30,35,35}
	end
	local random_pos = ARG2;	
	local pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	if table.getn(pos_table) == 0 then
		local random_pos = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG2,true);
		if random_pos == BP_None then
			return;
		end
		pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	end
	--传给服务器
	for i=1,table.getn(pos_table),1
		do
			--不能同时存在两种咒术
			local check_zhou = check_zhoushu(RECEIVER, ARG0, ARG1, pos_table[i])
			if check_zhou then
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
			else
				--计算抗性
				if chushiID == 92 then
					local du = Battle.get_prop(ARG0,pos_table[i],PT_NoPoison);
					level = level - math.ceil(du/10)
				elseif chushiID == 102 then
					local shihua = Battle.get_prop(ARG0,pos_table[i],PT_NoPetrifaction);
					level = level - math.ceil(shihua/10)
				elseif chushiID == 112 then
					local shui = Battle.get_prop(ARG0,pos_table[i],PT_NoSleep);
					level = level - math.ceil(shui/10)
				elseif chushiID == 122 then
					local hunluan = Battle.get_prop(ARG0,pos_table[i],PT_NoChaos);
					level = level - math.ceil(hunluan/10)
				elseif chushiID == 132 then
					local jiuzui = Battle.get_prop(ARG0,pos_table[i],PT_NoDrunk);
					level = level - math.ceil(jiuzui/10)
				end
				if level <= 0 then
					Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
				else
					local poison_index = math.ceil(math.random(1,100));
					if poison_index < table_index[level] then
						Battle.insert_state(ARG0,pos_table[i],chushiID+level,0);
					end
					Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
				end
			end
	end
end

--强力中毒
function skill_zhongdu_qiangli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,92)
end
function skill_zhongdu_qiangli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,92)
end
function skill_zhongdu_qiangli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,92)
end
function skill_zhongdu_qiangli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,92)
end
function skill_zhongdu_qiangli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,92)
end
function skill_zhongdu_qiangli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,92)
end
function skill_zhongdu_qiangli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,92)
end
function skill_zhongdu_qiangli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,92)
end
function skill_zhongdu_qiangli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,92)
end
function skill_zhongdu_qiangli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,92)
end

--//强力酒醉魔法
function skill_jiuzui_qiangli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,132)
end
function skill_jiuzui_qiangli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,132)
end
function skill_jiuzui_qiangli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,132)
end
function skill_jiuzui_qiangli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,132)
end
function skill_jiuzui_qiangli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,132)
end
function skill_jiuzui_qiangli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,132)
end
function skill_jiuzui_qiangli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,132)
end
function skill_jiuzui_qiangli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,132)
end
function skill_jiuzui_qiangli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,132)
end
function skill_jiuzui_qiangli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,132)
end

--//强力石化魔法
function skill_shihua_qiangli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,102)
end
function skill_shihua_qiangli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,102)
end
function skill_shihua_qiangli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,102)
end
function skill_shihua_qiangli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,102)
end
function skill_shihua_qiangli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,102)
end
function skill_shihua_qiangli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,102)
end
function skill_shihua_qiangli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,102)
end
function skill_shihua_qiangli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,102)
end
function skill_shihua_qiangli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,102)
end
function skill_shihua_qiangli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,102)
end


--//强力昏睡魔法
function skill_hunshui_qiangli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,112)
end
function skill_hunshui_qiangli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,112)
end
function skill_hunshui_qiangli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,112)
end
function skill_hunshui_qiangli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,112)
end
function skill_hunshui_qiangli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,112)
end
function skill_hunshui_qiangli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,112)
end
function skill_hunshui_qiangli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,112)
end
function skill_hunshui_qiangli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,112)
end
function skill_hunshui_qiangli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,112)
end
function skill_hunshui_qiangli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,112)
end

--强力混乱魔法
function skill_hunluan_qiangli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,122)
end
function skill_hunluan_qiangli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,122)
end
function skill_hunluan_qiangli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,122)
end
function skill_hunluan_qiangli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,122)
end
function skill_hunluan_qiangli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,122)
end
function skill_hunluan_qiangli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,122)
end
function skill_hunluan_qiangli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,122)
end
function skill_hunluan_qiangli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,122)
end
function skill_hunluan_qiangli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,122)
end
function skill_hunluan_qiangli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,122)
end

--强力遗忘魔法
function skill_yiwang_qiangli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,1,142)
end
function skill_yiwang_qiangli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,2,142)
end
function skill_yiwang_qiangli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,3,142)
end
function skill_yiwang_qiangli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,4,142)
end
function skill_yiwang_qiangli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,5,142)
end
function skill_yiwang_qiangli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,6,142)
end
function skill_yiwang_qiangli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,7,142)
end
function skill_yiwang_qiangli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,8,142)
end
function skill_yiwang_qiangli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,9,142)
end
function skill_yiwang_qiangli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_zhoushu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,10,142)
end

--反击技能
function skill_fanji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,level)
	--判断武器
	local weaponId, weaponType = Entity.getWeapon(RECEIVER);
	if weaponType == WT_Bow or weaponType == WT_Knife then
		weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
		nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		return
	end
	Battle.insert_state(ARG0,ARG1,152+level,0);
end

function skill_fanji_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_fanji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,1)
end


--//恢复魔法
function skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local hp_curr = Battle.get_prop(ARG0,ARG2,PT_HpCurr);
	if hp_curr < 1 then
		return;
	end
	local check_huifu = Battle.check_state(ARG0,ARG2,ST_Recover);
	local check_huifu1 = Battle.check_state(ARG0,ARG2,ST_StrongRecover);
	local check_huifu2 = Battle.check_state(ARG0,ARG2,ST_GroupRecover);
	if check_huifu or check_huifu1 or check_huifu2 then
		Battle.change_prop(ARG0,ARG2,PT_HpCurr,0);
		return;
	end
	Battle.insert_state(ARG0,ARG2,162+level,0);
	Battle.change_prop(ARG0,ARG2,PT_HpCurr,0);
end

function skill_huifu_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_huifu_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_huifu_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_huifu_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_huifu_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_huifu_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_huifu_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_huifu_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_huifu_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_huifu_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--//恢复补血
function huifu_state(RECEIVER, ARG0, ARG1,level)
	local reply = Battle.get_prop(ARG0,ARG1,PT_Reply);
	if reply <= 0 then
		reply = 1
	end
	local check_huifu = Battle.check_state(ARG0,ARG1,ST_Recover);
	local check_huifu1 = Battle.check_state(ARG0,ARG1,ST_StrongRecover);
	local check_huifu2 = Battle.check_state(ARG0,ARG1,ST_GroupRecover);
	local hpmax = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	local hpcurr = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	if PT_HpCurr >= PT_HpMax then
		return
	end
	if check_huifu then
		Battle.changeProp_state(ARG0,ARG1,PT_HpCurr,reply*level/5);
	elseif check_huifu1 then
		Battle.changeProp_state(ARG0,ARG1,PT_HpCurr,reply*level*12/100);
	elseif check_huifu2 then
		Battle.changeProp_state(ARG0,ARG1,PT_HpCurr,reply*level*8/100);
	end
end

function huifu_state_1(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,1)
end
function huifu_state_2(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,2)
end
function huifu_state_3(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,3)
end
function huifu_state_4(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,4)
end
function huifu_state_5(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,5)
end
function huifu_state_6(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,6)
end
function huifu_state_7(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,7)
end
function huifu_state_8(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,8)
end
function huifu_state_9(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,9)
end
function huifu_state_10(RECEIVER, ARG0, ARG1)
	huifu_state(RECEIVER, ARG0, ARG1,10)
end

--强力恢复
function skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local random_pos = ARG2;	
	local pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	if table.getn(pos_table) == 0 then
		local random_pos = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG2,true);
		if random_pos == BP_None then
			return;
		end
		pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	end
	--传给服务器
	for i=1,table.getn(pos_table),1
		do
			--不能同时存在两种恢复
			local check_huifu = Battle.check_state(ARG0,pos_table[i],ST_Recover);
			local check_huifu1 = Battle.check_state(ARG0,pos_table[i],ST_StrongRecover);
			local check_huifu2 = Battle.check_state(ARG0,pos_table[i],ST_GroupRecover);
			if check_huifu or check_huifu1 or check_huifu2 then
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
			else
				Battle.insert_state(ARG0,pos_table[i],182+level,0);		
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
			end
	end
end

function skill_huifu_qiangli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_huifu_qiangli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_huifu_qiangli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_huifu_qiangli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_huifu_qiangli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_huifu_qiangli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_huifu_qiangli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_huifu_qiangli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_huifu_qiangli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_huifu_qiangli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--全体恢复
function skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	for i=1,table.getn(pos_table),1
		do
			--不能同时存在两种咒术
			local check_huifu = Battle.check_state(ARG0,pos_table[i],ST_Recover);
			local check_huifu1 = Battle.check_state(ARG0,pos_table[i],ST_StrongRecover);
			local check_huifu2 = Battle.check_state(ARG0,pos_table[i],ST_GroupRecover);
			if check_huifu or check_huifu1 or check_huifu2 then
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
			else
				Battle.insert_state(ARG0,pos_table[i],192+level,0);
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
			end
	end
end

function skill_huifu_quanti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,1)
end
function skill_huifu_quanti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,2)
end
function skill_huifu_quanti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,3)
end
function skill_huifu_quanti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,4)
end
function skill_huifu_quanti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,5)
end
function skill_huifu_quanti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,6)
end
function skill_huifu_quanti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,7)
end
function skill_huifu_quanti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,8)
end
function skill_huifu_quanti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,9)
end
function skill_huifu_quanti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,10)
end

--//明镜
function skill_mingjing(RECEIVER, ARG0, ARG1,level)
	local hp_max = Battle.get_prop(ARG0,ARG1,PT_HpMax);
	local hp_index = math.ceil(math.random(5,(level+1)*5));
	
	Battle.change_prop(ARG0,ARG1,PT_HpCurr,hp_max*hp_index/100);
	local caster_dodge = Battle.get_prop(ARG0,ARG1,PT_Dodge);
	Battle.change_prop(ARG0,ARG1,PT_Dodge,-caster_dodge);
end

function skill_mingjing_1(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,1)
end
function skill_mingjing_2(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,2)
end
function skill_mingjing_3(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,3)
end
function skill_mingjing_4(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,4)
end
function skill_mingjing_5(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,5)
end
function skill_mingjing_6(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,6)
end
function skill_mingjing_7(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,7)
end
function skill_mingjing_8(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,8)
end
function skill_mingjing_9(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,9)
end
function skill_mingjing_10(RECEIVER, ARG0, ARG1)
	skill_mingjing(RECEIVER, ARG0, ARG1,11)
end

--//基础换位技能调用
--//RECEIVER CALLER
--//ARG0	 BATTLE ID
--//ARG1	 TARGET POSTION
function nrm_cge(RECEIVER, ARG0, ARG1, ARG2)

	Battle.change_position(ARG0,ARG1,ARG2);
end

--//基础逃跑技能调用
--//RECEIVER CALLER
--//ARG0	 BATTLE ID
--//ARG1	 TARGET POSTION
function nrm_raw(RECEIVER, ARG0, ARG1,ARG2, ARG3, ARG4)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	if Battle.get_BattleType(ARG0) == BT_PVP then
		Battle.run_away(ARG0,RECEIVER);
		return
	end
	if Battle.getSneakAttack(ARG0) == SAT_SneakAttack then
		Battle.run_away(ARG0,RECEIVER);
		return
	end
	if Battle.get_runawayNum(ARG0,RECEIVER) >= 3 then
		Battle.run_away(ARG0,RECEIVER);
		return
	end
	
	local caster_lv = Battle.get_prop(ARG0,ARG1,PT_Level);
	local random_pos = check_hp_curr(ARG0,ARG1,13,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local target_lv = Battle.get_prop(ARG0,random_pos,PT_Level);
	local num = (caster_lv - target_lv) * 2 
	if num < 0 then
		num = 0
	end
	local raw_index = math.random(1,100);
	if raw_index <= 30 + num then
		Battle.run_away(ARG0,RECEIVER);
	else
		Battle.set_runawayNum(ARG0,RECEIVER);
		return false;
	end
end

--//收放
--// ARG3 是宠物INST ID
function use_baby(RECEIVER, ARG0, ARG1, ARG2, ARG3, ARG4)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local baby_id = ARG4[OPT_BabyId];
	local now_is_battle = Battle.is_battle_baby(ARG0,baby_id);
	if now_is_battle then
		Battle.select_baby(ARG0,RECEIVER,baby_id,false);
	else
		Battle.select_baby(ARG0,RECEIVER,baby_id,true);
	end
end


--抓宠
function get_baby(RECEIVER, ARG0, ARG1, ARG2, ARG3, ARG4)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	Battle.zhuachong(ARG0,random_pos,RECEIVER);
end

--//乾坤技能调用
function skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断武器
	local Etype = Entity.getType(RECEIVER)
	if Etype == ET_Player then
		local weaponId, weaponType = Entity.getWeapon(RECEIVER);
		if weaponType == WT_Knife then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return
		end
	end
	if Battle.getBattleDataID(ARG0) == 1 then
		Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-1);
		return;
	end
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断目标是否被护卫
	if ARG4[OPT_Huwei] ~= 0 and Battle.get_prop(ARG0,ARG4[OPT_Huwei],PT_HpCurr) >= 1 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG4[OPT_Huwei], ARG3,ARG4)
		return
	end
	--判断目标是否可攻击
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	damage = (1 + level / 10) * damage
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

function skill_qiankun_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 1);
end
function skill_qiankun_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 2);
end
function skill_qiankun_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 3);
end
function skill_qiankun_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 4);
end
function skill_qiankun_5(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 5);
end
function skill_qiankun_6(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 6);
end
function skill_qiankun_7(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 7);
end
function skill_qiankun_8(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 8);
end
function skill_qiankun_9(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 9);
end
function skill_qiankun_10(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_qiankun(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4, 10);
end

--//诸刃技能调用
function skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断武器
	local Etype = Entity.getType(RECEIVER)
	if Etype == ET_Player then
		local weaponId, weaponType = Entity.getWeapon(RECEIVER);
		if weaponType == WT_Knife then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return
		end
	end
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断目标是否被护卫
	if ARG4[OPT_Huwei] ~= 0 and Battle.get_prop(ARG0,ARG4[OPT_Huwei],PT_HpCurr) >= 1 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG4[OPT_Huwei], ARG3,ARG4)
		return
	end
	--判断目标是否可攻击
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local caster_attack = Battle.get_prop(ARG0,ARG1,PT_Attack);
	local caster_defense= Battle.get_prop(ARG0,ARG1,PT_Defense);
	local damage = zhuren_damage(RECEIVER, ARG0, ARG1, random_pos,0,level);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

function skill_zhuren_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1)
end
function skill_zhuren_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2)
end
function skill_zhuren_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3)
end
function skill_zhuren_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4)
end
function skill_zhuren_5(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,5)
end
function skill_zhuren_6(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,6)
end
function skill_zhuren_7(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,7)
end
function skill_zhuren_8(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,8)
end
function skill_zhuren_9(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,9)
end
function skill_zhuren_10(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhuren(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,10)
end
 

--崩击技能
function skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断武器
	local weaponId, weaponType = Entity.getWeapon(RECEIVER);
	if weaponType == WT_Bow or weaponType == WT_Knife  then
		weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
		nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		return
	end
	local random_pos = ARG2;
	random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local damage = bengji_damage(RECEIVER, ARG0, ARG1, random_pos,level);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
end


function skill_bengji_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1)
end
function skill_bengji_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2)
end
function skill_bengji_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3)
end
function skill_bengji_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4)
end
function skill_bengji_5(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,5)
end
function skill_bengji_6(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,6)
end
function skill_bengji_7(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,7)
end
function skill_bengji_8(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,8)
end
function skill_bengji_9(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,9)
end
function skill_bengji_10(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,10)
end

--战栗袭心
function skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断武器
	local weaponId, weaponType = Entity.getWeapon(RECEIVER);
	if weaponType == WT_Bow or weaponType == WT_Knife  then
		weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
		nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		return
	end
	local random_pos = ARG2;
	random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	--是否攻击无效
	local check_gongwu = Battle.check_state(ARG0,random_pos,ST_ActionInvalid);
	if check_gongwu then
		Battle.cutTime_state(ARG0,random_pos,ST_ActionInvalid,1)
		Battle.change_prop(ARG0,random_pos,PT_MpCurr,0);
		return;
	end
	local caster_MpMax = Battle.get_prop(ARG0,random_pos,PT_MpCurr);
	local damage = level * 5 * caster_MpMax / 100 
	Battle.change_prop(ARG0,random_pos,PT_MpCurr,-damage);
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

function skill_zhanli_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1);
end
function skill_zhanli_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2);
end
function skill_zhanli_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3);
end
function skill_zhanli_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4);
end
function skill_zhanli_5(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,5);
end
function skill_zhanli_6(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,6);
end
function skill_zhanli_7(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,7);
end
function skill_zhanli_8(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,8);
end
function skill_zhanli_9(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,9);
end
function skill_zhanli_10(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_zhanli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,10);
end

--单体补血
function skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	if ARG3[ARG2] == 0 then
		return;
	end
	local hp_curr = Battle.get_prop(ARG0,ARG2,PT_HpCurr);
	if hp_curr < 1 then
		return;
	end
	local caster_camp = Battle.get_Force(ARG0,ARG1);
	local target_camp = Battle.get_Force(ARG0,ARG2);
	if caster_camp == target_camp then
		--// 获得目标回复
	local target_reply = Battle.get_prop(ARG0,ARG2,PT_Reply);
	if target_reply <= 0 then
		target_reply = 1
	end
	local damage = (level*60*target_reply/100);
	Battle.change_prop(ARG0,ARG2,PT_HpCurr,damage);
		
	else 

		return false;
	end
end

function skill_buxue_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 1);
end
function skill_buxue_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 2);
end
function skill_buxue_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 3);
end
function skill_buxue_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 4);
end
function skill_buxue_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 5);
end
function skill_buxue_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 6);
end
function skill_buxue_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 7);
end
function skill_buxue_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 8);
end
function skill_buxue_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 9);
end
function skill_buxue_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_buxue_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 10);
end


--强力补血
function skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	local caster_camp = Battle.get_Force(ARG0,ARG1);
	local target_camp = Battle.get_Force(ARG0,ARG2);
	if caster_camp ~= target_camp then
		return;
	end
	
	local random_pos = ARG2;	
	local pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	if table.getn(pos_table) == 0 then
		return;
	end
	--传给服务器
	for i=1,table.getn(pos_table),1
		do
			local hp_curr = Battle.get_prop(ARG0,pos_table[i],PT_HpCurr);
			if hp_curr < 1 then
				return;
			end
			local target_reply = Battle.get_prop(ARG0,pos_table[i],PT_Reply);
				if target_reply <= 0 then
					target_reply = 1
				end
			local damage = (level*35*target_reply/100);
			Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,damage);
	end
end

function skill_buxue_qiangli_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,1)
end
function skill_buxue_qiangli_2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,2)
end
function skill_buxue_qiangli_3(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,3)
end
function skill_buxue_qiangli_4(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,4)
end
function skill_buxue_qiangli_5(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,5)
end
function skill_buxue_qiangli_6(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,6)
end
function skill_buxue_qiangli_7(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,7)
end
function skill_buxue_qiangli_8(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,8)
end
function skill_buxue_qiangli_9(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,9)
end
function skill_buxue_qiangli_10(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,10)
end


--全体补血
function skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	local caster_camp = Battle.get_Force(ARG0,ARG1);
	local target_camp = Battle.get_Force(ARG0,ARG2);
	if caster_camp ~= target_camp then
		return;
	end
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	if table.getn(pos_table) == 0 then
		return
	end
	for i=1,table.getn(pos_table),1
		do
			local hp_curr = Battle.get_prop(ARG0,pos_table[i],PT_HpCurr);
			if hp_curr < 1 then
				return;
			end
			local target_reply = Battle.get_prop(ARG0,pos_table[i],PT_Reply);
				if target_reply <= 0 then
					target_reply = 1
				end
			local damage = (level*23*target_reply/100);
			Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,damage);			
	end
end

function skill_buxue_quanti_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,1)
end
function skill_buxue_quanti_2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,2)
end
function skill_buxue_quanti_3(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,3)
end
function skill_buxue_quanti_4(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,4)
end
function skill_buxue_quanti_5(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,5)
end
function skill_buxue_quanti_6(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,6)
end
function skill_buxue_quanti_7(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,7)
end
function skill_buxue_quanti_8(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,8)
end
function skill_buxue_quanti_9(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,9)
end
function skill_buxue_quanti_10(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,10)
end


--气绝回复
function skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	if ARG3[ARG2] == 0 then
		return
	end
	local hp_curr = Battle.get_prop(ARG0,ARG2,PT_HpCurr);
	if hp_curr < 1 then
		local caster_camp = Battle.get_Force(ARG0,ARG1);
		local target_camp = Battle.get_Force(ARG0,ARG2);
		if caster_camp == target_camp then
			local damage = 60 * level;
			local hp_max = Battle.get_prop(ARG0,ARG2,PT_HpMax);
			if hp_max < damage then
				damage = hp_max
			end
			Battle.change_prop(ARG0,ARG2,PT_HpCurr,damage);
		else 

			return false;
		end
	else
		Battle.change_prop(ARG0,ARG2,PT_HpCurr,0);
	end
end

function skill_qijue_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 1)
end
function skill_qijue_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 2)
end
function skill_qijue_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 3)
end
function skill_qijue_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 4)
end
function skill_qijue_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 5)
end
function skill_qijue_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 6)
end
function skill_qijue_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 7)
end
function skill_qijue_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 8)
end
function skill_qijue_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 9)
end
function skill_qijue_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_qijue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 10)
end

--连击调取的普通攻击
function skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断武器
	local weaponId, weaponType = Entity.getWeapon(RECEIVER);
	if weaponType == WT_Bow or weaponType == WT_Knife  then
		weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
		nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		return
	end
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断目标是否被护卫
	if ARG4[OPT_Huwei] ~= 0 and Battle.get_prop(ARG0,ARG4[OPT_Huwei],PT_HpCurr) >= 1 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG4[OPT_Huwei], ARG3,ARG4)
		return
	end
	--判断目标是否可攻击
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None or random_pos == ARG1 then
		return;
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage - target_defense * caster_Lv / (target_Lv * 2);
	end
	local damagetable = {2,1.7,3,2.7,4,3.7,5,4.5,6,5}
	damage = damage * (1 + level / 10 ) / damagetable[level]
	if damage < 0 and damage > -1 then
		damage = - 1
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos,0);
end

--连击技能
function skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		return
	end
	--判断武器
	local weaponId, weaponType = Entity.getWeapon(RECEIVER);
	if weaponType == WT_Bow or weaponType == WT_Knife  then
		return
	end
	local hp_curr = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	if hp_curr < 1 then
		return;
	end
	--判断目标是否被护卫
	if ARG4[OPT_Huwei] ~= 0 and Battle.get_prop(ARG0,ARG4[OPT_Huwei],PT_HpCurr) >= 1 then
		heji_atk(RECEIVER, ARG0, ARG1, ARG4[OPT_Huwei], ARG3,ARG4)
		return
	end
	local random_pos1 = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG2,false);
	if random_pos1 == BP_None then
		return;
	end
	--// 获得基础攻击力
	local random_pos = check_hp_curr(ARG0,ARG1, random_pos1,ARG3,false);
	if random_pos == BP_None or random_pos == ARG1 then
		return;
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage - target_defense * caster_Lv / (target_Lv * 2);
	end
	local damagetable = {2,1.7,3,2.7,4,3.7,5,4.5,6,5}
	damage = damage * (1 + level / 10 ) / damagetable[level]
	if damage < 0 and damage > -1 then
		damage = - 1
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage,isbisha);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);	
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	--是否反击
	fanji_damage(RECEIVER, ARG0, ARG1, random_pos)
end

function skill_lianji_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1)
end
function skill_lianji_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,1)
end
function skill_lianji_atk2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2)
end
function skill_lianji_2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,2)
end
function skill_lianji_atk3(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3)
end
function skill_lianji_3(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,3)
end
function skill_lianji_atk4(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4)
end
function skill_lianji_4(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,4)
end
function skill_lianji_atk5(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,5)
end
function skill_lianji_5(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,5)
end
function skill_lianji_atk6(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,6)
end
function skill_lianji_6(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,6)
end
function skill_lianji_atk7(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,7)
end
function skill_lianji_7(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,7)
end
function skill_lianji_atk8(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,8)
end
function skill_lianji_8(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,8)
end
function skill_lianji_atk9(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,9)
end
function skill_lianji_9(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,9)
end
function skill_lianji_atk10(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,10)
end
function skill_lianji_10(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	skill_lianji(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4,10)
end

--//吸血魔法
function skill_xixue(RECEIVER, ARG0, ARG1, ARG2, ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1, ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end
	local damage = magic_damage(RECEIVER, ARG0, ARG1, random_pos,level) * 2 / 3;
	if damage > 0 and damage < 1 then
		damage = 1;
	end
	local check_mofan = Battle.check_state(ARG0,random_pos,ST_MagicBounce);
	if check_mofan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage);
		Battle.cutTime_state(ARG0,random_pos,ST_MagicBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage);
	Battle.change_prop(ARG0,ARG1,PT_HpCurr,-damage/2);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
end

function skill_xixue_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 1);
end
function skill_xixue_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 2);
end
function skill_xixue_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 3);
end
function skill_xixue_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 4);
end
function skill_xixue_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 5);
end
function skill_xixue_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 6);
end
function skill_xixue_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 7);
end
function skill_xixue_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 8);
end
function skill_xixue_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 9);
end
function skill_xixue_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_xixue(RECEIVER, ARG0, ARG1, ARG2,ARG3, 10);
end

--//单体魔法
function skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2, ARG3,level,shuijing_magic)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local random_pos = check_hp_curr(ARG0,ARG1, ARG2,ARG3,true);
	if random_pos == BP_None then
		return;
	end
	local damage = magic_damage(RECEIVER, ARG0, ARG1, random_pos,level,shuijing_magic);
	local check_mofan = Battle.check_state(ARG0,random_pos,ST_MagicBounce);
	if check_mofan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage);
		Battle.cutTime_state(ARG0,random_pos,ST_MagicBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
		
end

--火魔法
function skill_huoyan_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 1,PT_Fire);
end
function skill_huoyan_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 2,PT_Fire);
end
function skill_huoyan_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 3,PT_Fire);
end
function skill_huoyan_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 4,PT_Fire);
end
function skill_huoyan_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 5,PT_Fire);
end
function skill_huoyan_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 6,PT_Fire);
end
function skill_huoyan_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 7,PT_Fire);
end
function skill_huoyan_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 8,PT_Fire);
end
function skill_huoyan_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 9,PT_Fire);
end
function skill_huoyan_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 10,PT_Fire);
end

--风魔法
function skill_fengren_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 1,PT_Wind);
end
function skill_fengren_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 2,PT_Wind);
end
function skill_fengren_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 3,PT_Wind);
end
function skill_fengren_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 4,PT_Wind);
end
function skill_fengren_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 5,PT_Wind);
end
function skill_fengren_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 6,PT_Wind);
end
function skill_fengren_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 7,PT_Wind);
end
function skill_fengren_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 8,PT_Wind);
end
function skill_fengren_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 9,PT_Wind);
end
function skill_fengren_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 10,PT_Wind);
end

--地魔法
function skill_yunshi_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 1,PT_Land);
end
function skill_yunshi_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 2,PT_Land);
end
function skill_yunshi_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 3,PT_Land);
end
function skill_yunshi_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 4,PT_Land);
end
function skill_yunshi_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 5,PT_Land);
end
function skill_yunshi_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 6,PT_Land);
end
function skill_yunshi_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 7,PT_Land);
end
function skill_yunshi_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 8,PT_Land);
end
function skill_yunshi_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 9,PT_Land);
end
function skill_yunshi_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 10,PT_Land);
end

--冰魔法
function skill_bingdong_danti_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 1,PT_Water);
end
function skill_bingdong_danti_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 2,PT_Water);
end
function skill_bingdong_danti_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 3,PT_Water);
end
function skill_bingdong_danti_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 4,PT_Water);
end
function skill_bingdong_danti_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 5,PT_Water);
end
function skill_bingdong_danti_6(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 6,PT_Water);
end
function skill_bingdong_danti_7(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 7,PT_Water);
end
function skill_bingdong_danti_8(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 8,PT_Water);
end
function skill_bingdong_danti_9(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 9,PT_Water);
end
function skill_bingdong_danti_10(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	skill_mofa_danti(RECEIVER, ARG0, ARG1, ARG2,ARG3, 10,PT_Water);
end

--强力魔法
function skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,level,shuijing_magic)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	--// 获得目标精神
	local random_pos = ARG2;	
	local pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	if table.getn(pos_table) == 0 then
		local random_pos = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG2,true);
		if random_pos == BP_None then
			return;
		end
		pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	end
	local damage = 1;
	--传给服务器
	for i=1,table.getn(pos_table),1
		do
			damage = 0.6 * magic_damage(RECEIVER, ARG0, ARG1, pos_table[i],level,shuijing_magic);
			if damage > 0 and damage < 1 then
				damage = 1;
			end
			local check_mofan = Battle.check_state(ARG0,pos_table[i],ST_MagicBounce);
			if check_mofan then
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
				Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage);
				Battle.cutTime_state(ARG0,pos_table[i],ST_MagicBounce,1)
			else
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,damage);
				remove_hunshui(RECEIVER, ARG0,pos_table[i],damage)
			end
	end
end
--强力风
function skill_fengren_qiangli_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Wind)
end
function skill_fengren_qiangli_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Wind)
end
function skill_fengren_qiangli_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Wind)
end
function skill_fengren_qiangli_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Wind)
end
function skill_fengren_qiangli_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Wind)
end
function skill_fengren_qiangli_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Wind)
end
function skill_fengren_qiangli_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Wind)
end
function skill_fengren_qiangli_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Wind)
end
function skill_fengren_qiangli_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Wind)
end
function skill_fengren_qiangli_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Wind)
end
--强力火焰
function skill_huoyan_qiangli_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Fire)
end
function skill_huoyan_qiangli_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Fire)
end
function skill_huoyan_qiangli_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Fire)
end
function skill_huoyan_qiangli_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Fire)
end
function skill_huoyan_qiangli_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Fire)
end
function skill_huoyan_qiangli_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Fire)
end
function skill_huoyan_qiangli_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Fire)
end
function skill_huoyan_qiangli_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Fire)
end
function skill_huoyan_qiangli_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Fire)
end
function skill_huoyan_qiangli_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Fire)
end
--强力陨石
function skill_yunshi_qiangli_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Land)
end
function skill_yunshi_qiangli_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Land)
end
function skill_yunshi_qiangli_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Land)
end
function skill_yunshi_qiangli_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Land)
end
function skill_yunshi_qiangli_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Land)
end
function skill_yunshi_qiangli_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Land)
end
function skill_yunshi_qiangli_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Land)
end
function skill_yunshi_qiangli_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Land)
end
function skill_yunshi_qiangli_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Land)
end
function skill_yunshi_qiangli_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Land)
end
--强力冰冻
function skill_bingdong_qiangli_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Water)
end
function skill_bingdong_qiangli_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Water)
end
function skill_bingdong_qiangli_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Water)
end
function skill_bingdong_qiangli_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Water)
end
function skill_bingdong_qiangli_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Water)
end
function skill_bingdong_qiangli_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Water)
end
function skill_bingdong_qiangli_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Water)
end
function skill_bingdong_qiangli_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Water)
end
function skill_bingdong_qiangli_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Water)
end
function skill_bingdong_qiangli_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Water)
end



--全体魔法
function skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,level,shuijing_magic)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local damage = 1;
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	if table.getn(pos_table) == 0 then
		return
	end
	for i=1,table.getn(pos_table),1
		do
			damage = 0.4 * magic_damage(RECEIVER, ARG0, ARG1, pos_table[i],level,shuijing_magic);
			if damage > 0 and damage < 1 then
				damage = 1;
			end
			local check_mofan = Battle.check_state(ARG0,pos_table[i],ST_MagicBounce);
			if check_mofan then
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
				Battle.changeProp_fanji(ARG0,ARG1,PT_HpCurr,damage);
				Battle.cutTime_state(ARG0,pos_table[i],ST_MagicBounce,1)
			else
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,damage);
				remove_hunshui(RECEIVER, ARG0,pos_table[i],damage)
			end
	end
end
--全体陨石
function skill_yunshi_quanti_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Land)
end
function skill_yunshi_quanti_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Land)
end
function skill_yunshi_quanti_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Land)
end
function skill_yunshi_quanti_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Land)
end
function skill_yunshi_quanti_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Land)
end
function skill_yunshi_quanti_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Land)
end
function skill_yunshi_quanti_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Land)
end
function skill_yunshi_quanti_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Land)
end
function skill_yunshi_quanti_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Land)
end
function skill_yunshi_quanti_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Land)
end
--全体冰冻
function skill_bingdong_quanti_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Water)
end
function skill_bingdong_quanti_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Water)
end
function skill_bingdong_quanti_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Water)
end
function skill_bingdong_quanti_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Water)
end
function skill_bingdong_quanti_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Water)
end
function skill_bingdong_quanti_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Water)
end
function skill_bingdong_quanti_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Water)
end
function skill_bingdong_quanti_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Water)
end
function skill_bingdong_quanti_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Water)
end
function skill_bingdong_quanti_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Water)
end
--全体火焰
function skill_huoyan_quanti_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Fire)
end
function skill_huoyan_quanti_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Fire)
end
function skill_huoyan_quanti_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Fire)
end
function skill_huoyan_quanti_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Fire)
end
function skill_huoyan_quanti_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Fire)
end
function skill_huoyan_quanti_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Fire)
end
function skill_huoyan_quanti_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Fire)
end
function skill_huoyan_quanti_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Fire)
end
function skill_huoyan_quanti_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Fire)
end
function skill_huoyan_quanti_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Fire)
end
--全体风刃
function skill_fengren_quanti_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,1,PT_Wind)
end
function skill_fengren_quanti_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,2,PT_Wind)
end
function skill_fengren_quanti_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,3,PT_Wind)
end
function skill_fengren_quanti_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,4,PT_Wind)
end
function skill_fengren_quanti_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,5,PT_Wind)
end
function skill_fengren_quanti_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,6,PT_Wind)
end
function skill_fengren_quanti_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,7,PT_Wind)
end
function skill_fengren_quanti_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,8,PT_Wind)
end
function skill_fengren_quanti_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,9,PT_Wind)
end
function skill_fengren_quanti_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,10,PT_Wind)
end


--气功弹
function skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	--判断武器
	local weaponId, weaponType = Entity.getWeapon(RECEIVER);
	if weaponType ~= WT_None then
		weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
		nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
		return
	end
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0)
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	--如果是伙伴，伤害减免20%
	local Etype = Entity.getType(RECEIVER)
	if Etype == ET_Emplyee then
		damage = damage * 8 / 10
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	for i=1,table.getn(pos_table),1
		do
			if pos_table[i] == random_pos or pos_table[i] == ARG1 then
				table.remove(pos_table,i);
			end
		end
	local table_num1 = {1,1,2,2,3,3,3,4,4,5}
	local table_num2 = {1,2,2,3,3,4,5,5,6,7}
	local num = math.ceil(math.random(table_num1[level],table_num2[level]));
	while table.getn(pos_table) ~= 0 and num - 1 > 0 do
		local r_index = math.ceil(math.random(1,table.getn(pos_table)));
		damage = nrm_damage(RECEIVER, ARG0, ARG1, pos_table[r_index],0);
		--判断是否必杀
		local isbisha = check_bisha(RECEIVER, ARG0, ARG1, pos_table[r_index])
		if damage == 0 then
		isbisha = false
	end
		if isbisha then
			local target_defense= Battle.get_prop(ARG0,pos_table[r_index],PT_Defense);
			local target_Lv= Battle.get_prop(ARG0,pos_table[r_index],PT_Level);
			local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
			damage = damage -target_defense * caster_Lv / (target_Lv * 2);
		end
		--是否攻击反弹
		local check_gongfan = Battle.check_state(ARG0,pos_table[r_index],ST_ActionBounce);
		if check_gongfan then
			Battle.change_prop(ARG0,pos_table[r_index],PT_HpCurr,0);
			Battle.cutTime_state(ARG0,pos_table[r_index],ST_ActionBounce,1)
			return;
		end
		if Etype == ET_Emplyee then
			damage = damage * 8 / 10
		end
		Battle.change_prop(ARG0,pos_table[r_index],PT_HpCurr,damage,isbisha);
		remove_hunshui(RECEIVER, ARG0,pos_table[r_index],damage)
		table.remove(pos_table,r_index);
		num = num - 1;
	end
end

function skill_qigongdan_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,1)
end
function skill_qigongdan_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,2)
end
function skill_qigongdan_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,3)
end
function skill_qigongdan_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,4)
end
function skill_qigongdan_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,5)
end
function skill_qigongdan_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,6)
end
function skill_qigongdan_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,7)
end
function skill_qigongdan_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,8)
end
function skill_qigongdan_9(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,9)
end
function skill_qigongdan_10(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_qigongdan6(RECEIVER, ARG0, ARG1, ARG2, ARG3,10)
end

--攻击判断
function nrm_atk_condition(RECEIVER, ARG0, ARG1, ARG2)
	if ARG1 == ARG2 then
		return false;
	else
		return true;
	end
end



--耗魔判断
function Mp_condition(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,ARG5)
	local mp_curr = Battle.get_prop(ARG0,ARG1,PT_MpCurr);
	
	if ARG5 >  mp_curr then
	
	--如果是伙伴则根据职业push普通攻击
		local Etype = Entity.getType(RECEIVER)
		if Etype == ET_Emplyee then
			local profesion = Battle.get_prop(ARG0,ARG1,PT_Profession)
			if profesion == JT_Axe or profesion == JT_Sword or profesion == JT_Ninja or profesion == JT_Mage or profesion == JT_Sage or profesion == JT_Wizard or profesion == JT_Word then
				Battle.change_order_skill(ARG0, ARG1, 8)
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.change_order_target(ARG0,ARG1,random_pos);
				return false
			elseif profesion == JT_Knight then
				Battle.change_order_skill(ARG0, ARG1, 9)
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.change_order_target(ARG0,ARG1,random_pos);
				return false
			elseif profesion == JT_Knight then
				Battle.change_order_skill(ARG0, ARG1, 10)
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.change_order_target(ARG0,ARG1,random_pos);
				return false
			elseif profesion == JT_Fighter then
				Battle.change_order_skill(ARG0, ARG1, 1)
				local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
				Battle.change_order_target(ARG0,ARG1,random_pos);
				return false
			else
				return false
			end
		end
		 --如果不是伙伴判武器
        local weaponId, weaponType = Entity.getWeapon(RECEIVER);
		if weaponType == WT_Bow then
			Battle.change_order_skill(ARG0, ARG1, 10)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return false
		end
		if weaponType == WT_Axe or weaponType == WT_Sword or weaponType == WT_Stick then
			Battle.change_order_skill(ARG0, ARG1, 8)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return false
		end
		if weaponType == WT_Spear then
			Battle.change_order_skill(ARG0, ARG1, 9)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return false
		end
		if weaponType == WT_Knife then
			Battle.change_order_skill(ARG0, ARG1, 11)
			local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
			Battle.change_order_target(ARG0,ARG1,random_pos);
			return false
		end
		Battle.change_order_skill(ARG0, ARG1, 1)
		local random_pos = random_TargetPos(ARG0,ARG1,ARG2,ARG3,false);
		Battle.change_order_target(ARG0,ARG1,random_pos);
		return false;
	else
		return true;
	end
end

--什么都不做判断
function budon_condition(RECEIVER, ARG0, ARG1, ARG2)
	return false;
end


--护卫技能
function skill_huwei(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local hp_curr = Battle.get_prop(ARG0,ARG2,PT_HpCurr);
	local hp_curr1 = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	if hp_curr < 1 or hp_curr1 < 1 then
		return false;
	end
	Battle.set_huwei(ARG0,ARG2,ARG1);
	return false;
end

--单体洁净技能
function skill_jiejing(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local hp_curr = Battle.get_prop(ARG0,ARG2,PT_HpCurr);
	if hp_curr < 1 then
		return false;
	end
	Battle.change_prop(ARG0,ARG2,PT_HpCurr,0,false);
	local zhoushu = {ST_Poison,ST_Basilisk,ST_Sleep,ST_Chaos,ST_Drunk,ST_Forget}
	local r_index = math.ceil(math.random(1,100))
	if r_index >= level * 30 then
		return
	end
	for i=1,table.getn(zhoushu),1 
		do
			if Battle.check_state(ARG0,ARG2,zhoushu[i]) then
				Battle.remove_state(ARG0,ARG2,zhoushu[i])
			end
		end
end

function skill_jiejing_1(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1)
end
function skill_jiejing_2(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2)
end
function skill_jiejing_3(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3)
end


--强力洁净技能
function skill_jiejing_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local zhoushu = {ST_Poison,ST_Basilisk,ST_Sleep,ST_Chaos,ST_Drunk,ST_Forget}
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local random_pos = ARG2;	
	local pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	if table.getn(pos_table) == 0 then
		random_pos = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG2,true);
		if random_pos == BP_None then
			return;
		end
		pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
		for i=1,table.getn(pos_table),1
			do
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0,false);
		end
	end
	--传给服务器
	for i=1,table.getn(pos_table),1
		do
			Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0,false);
			local r_index = math.ceil(math.random(1,100))
			if r_index < level * 30 then
				
				for j=1,table.getn(zhoushu),1 
					do
						if Battle.check_state(ARG0,pos_table[i],zhoushu[j]) then
							Battle.remove_state(ARG0,pos_table[i],zhoushu[j])
						end
					end
			end
	end
end

function skill_jiejing_4(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1)
end
function skill_jiejing_5(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2)
end
function skill_jiejing_6(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing_qiangli(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3)
end



--//全体洁净
function skill_jiejing_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			return;
		end
	end 
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local zhoushu = {ST_Poison,ST_Basilisk,ST_Sleep,ST_Chaos,ST_Drunk,ST_Forget}
	for i=1,table.getn(pos_table),1
		do
			Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0,false);
			local r_index = math.ceil(math.random(1,100))
			if r_index < level * 30 then
				for j=1,table.getn(zhoushu),1 
					do
						if Battle.check_state(ARG0,pos_table[i],zhoushu[j]) then
							Battle.remove_state(ARG0,pos_table[i],zhoushu[j])
						end
					end
			end
	end
end

function skill_jiejing_7(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,1)
end
function skill_jiejing_8(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,2)
end
function skill_jiejing_9(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,3)
end
function skill_jiejing_10(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	skill_jiejing_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,4)
end



--乱射调取的普通攻击
function skill_luanshe_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4)
	--判断武器
	local Etype = Entity.getType(RECEIVER)
	if Etype == ET_Player then
		local weaponId, weaponType = Entity.getWeapon(RECEIVER);
		if weaponType ~= WT_Bow then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			nrm_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return
		end
	end
	luanshe_num = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
	local index = {45,30,15,5,5,5,5,5,5,5,5,5}
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		local hun_index = math.ceil(math.random(1,100));
		if hun_index <= 100 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		elseif hun_index < 40 and hun_index > 20 then
			weapon_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3)
			hunluan_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
			return;
		end
	end 
	--判断目标是否可攻击
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None or random_pos == ARG1 then
		return;
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	damage = index[luanshe_num[random_pos]] / 100 * damage
	if damage < 0 and damage > -1 then
		damage = -1
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	if damage < 0 then
		luanshe_num[random_pos] = luanshe_num[random_pos] + 1
	end
end

--乱射技能
function skill_luanshe(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	--判断是否混乱
	local check_hun = Battle.check_state(ARG0,ARG1,ST_Chaos);
	if check_hun then
		return
	end
	--判断武器
	local Etype = Entity.getType(RECEIVER)
	if Etype == ET_Player then
		local weaponId, weaponType = Entity.getWeapon(RECEIVER);
		if weaponType ~= WT_Bow then
			return
		end
	end
	local index = {45,30,15,5,5,5,5,5,5,5,5,5}
	--// 获得基础攻击力
	local random_pos = get_luanshe_pos(ARG0,ARG1,ARG2,ARG3);
	if random_pos == BP_None or random_pos == ARG1 then
		return;
	end
	local damage = nrm_damage(RECEIVER, ARG0, ARG1, random_pos,0);
	--判断是否必杀
	local isbisha = check_bisha(RECEIVER, ARG0, ARG1, random_pos)
	if damage == 0 then
		isbisha = false
	end
	if isbisha then
		local target_defense= Battle.get_prop(ARG0,random_pos,PT_Defense);
		local target_Lv= Battle.get_prop(ARG0,random_pos,PT_Level);
		local caster_Lv= Battle.get_prop(ARG0,ARG1,PT_Level);
		damage = damage -target_defense * caster_Lv / (target_Lv * 2);
	end
	damage = index[luanshe_num[random_pos]] / 100 * damage
	if damage < 0 and damage > -1 then
		damage = -1
	end
	--是否攻击反弹
	local check_gongfan = Battle.check_state(ARG0,random_pos,ST_ActionBounce);
	if check_gongfan then
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
		Battle.cutTime_state(ARG0,random_pos,ST_ActionBounce,1)
		return;
	end
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,damage,isbisha);
	remove_hunshui(RECEIVER, ARG0,random_pos,damage)
	if damage < 0 then
		luanshe_num[random_pos] = luanshe_num[random_pos] + 1
	end
end


--获取乱射位置
function get_luanshe_pos(ARG0,ARG1,ARG2,ARG3)
	--获取位置
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local temp = pos_table[1]
	if table.getn(pos_table) == 0 then
		return BP_None
	end
	for i=1,table.getn(pos_table),1
		do
			if luanshe_num[temp] > luanshe_num[pos_table[i]] then
				temp = pos_table[i]
			end
		end
	local l_table = {}
	for i=1,table.getn(pos_table),1
		do
			if luanshe_num[temp] == luanshe_num[pos_table[i]] then
				table.insert(l_table,pos_table[i]);
			end
		end
	if table.getn(l_table) ~= 0 then
		local r_index = math.ceil(math.random(1,table.getn(l_table)));
		temp = l_table[r_index]
	end
	return temp
end

--血瓶
function skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,damage)
	local hp_curr = Battle.get_prop(ARG0,ARG2,PT_HpCurr);
	if hp_curr < 1 then
		return;
	end
	local caster_camp = Battle.get_Force(ARG0,ARG1);
	local target_camp = Battle.get_Force(ARG0,ARG2);
	if caster_camp == target_camp then
		--// 获得目标回复
	local target_reply = Battle.get_prop(ARG0,ARG2,PT_Reply);

	local damage = (damage*target_reply/100);
	local hp_max = Battle.get_prop(ARG0,ARG2,PT_HpMax);
	if hp_max - hp_curr < damage then
		damage = hp_max - hp_curr
	end
	Battle.change_prop(ARG0,ARG2,PT_HpCurr,damage);
		
	else 

		return false;
	end
end

function skill_buxue_xueping100(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,100)
end
function skill_buxue_xueping150(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,150)
end
function skill_buxue_xueping200(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,200)
end
function skill_buxue_xueping250(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,250)
end
function skill_buxue_xueping300(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,300)
end
function skill_buxue_xueping400(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,400)
end
function skill_buxue_xueping500(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,500)
end
function skill_buxue_xueping600(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,600)
end
function skill_buxue_xueping800(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,800)
end
function skill_buxue_xueping1000(RECEIVER, ARG0, ARG1, ARG2)
	skill_buxue_xueping(RECEIVER, ARG0, ARG1, ARG2,1000)
end



--大地之怒
function skill_dadi(RECEIVER, ARG0, ARG1, ARG2, ARG3,damage)
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	if table.getn(pos_table) == 0 then
		return
	end
	for i=1,table.getn(pos_table),1
		do
			if damage > 0 and damage < 1 then
				damage = 1;
			end
			Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,-damage);
			remove_hunshui(RECEIVER, ARG0,pos_table[i],damage)
	end
end

function skill_dadi_200(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_dadi(RECEIVER, ARG0, ARG1, ARG2, ARG3,200)
end

function skill_dadi_400(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	skill_dadi(RECEIVER, ARG0, ARG1, ARG2, ARG3,400)
end

function skill_jisi_danti(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local hun_index = math.ceil(math.random(1,100));
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if hun_index <= 20 then
		if random_pos == BP_None then
			return;
		end
		local hp_max= Battle.get_prop(ARG0,random_pos,PT_HpCurr);
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,-hp_max);
	else
		Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
	end
end

function skill_jisi_qiangli(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	local random_pos = ARG2;	
	local pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	if table.getn(pos_table) == 0 then
		random_pos = random_enemy_pos_ex(ARG0,ARG3,ARG1,ARG2,true);
		if random_pos == BP_None then
			return;
		end
		pos_table = qiangli_pos_table(RECEIVER, ARG0, ARG1, random_pos, ARG3);
	end
	--传给服务器
	for i=1,table.getn(pos_table),1
		do
			local hun_index = math.ceil(math.random(1,100));
			if hun_index <= 20 then
				local hp_max= Battle.get_prop(ARG0,pos_table[i],PT_HpCurr);
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,-hp_max);
			else
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);	
			end
	end
end


function skill_jisi_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	if table.getn(pos_table) == 0 then
		return
	end
	for i=1,table.getn(pos_table),1
		do
			local hun_index = math.ceil(math.random(1,100));
			if hun_index <= 20 then
				local hp_max= Battle.get_prop(ARG0,pos_table[i],PT_HpCurr);
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,-hp_max);
			else
				Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);	
			end
	end
end

function zhaohuan_1(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {59,61}
	Battle.add_Monster(ARG0,monsterclass)
end

function zhaohuan_2(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {65,65}
	Battle.add_Monster(ARG0,monsterclass)
end

function zhaohuan_3(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {113}
	Battle.add_Monster(ARG0,monsterclass)
end

function zhaohuan_4(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {110,110,110,130,130,130}
	Battle.add_Monster(ARG0,monsterclass)
end

function zhaohuan_5(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {147,147,147,147}
	Battle.add_Monster(ARG0,monsterclass)
end

function zhaohuan_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {133,133,133,133,133}
	Battle.add_Monster(ARG0,monsterclass)
end

function zhaohuan_7(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {51039,51039}
	Battle.add_Monster(ARG0,monsterclass)
end

--自爆
function zibao(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	local hp= Battle.get_prop(ARG0,random_pos,PT_HpCurr);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,-hp/5);
end

--召唤自己
function zhaohuan_ziji(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {70058}
	Battle.add_Monster(ARG0,monsterclass)
end

function zhaohuan_jingying(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {70075,70075,70075,70075}
	Battle.add_Monster(ARG0,monsterclass)
end

--点燃
function dianran(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local random_pos = check_hp_curr(ARG0,ARG1,ARG2,ARG3,false);
	if random_pos == BP_None then
		return;
	end
	Battle.insert_state(ARG0,random_pos,300,0);
	Battle.change_prop(ARG0,random_pos,PT_HpCurr,0);
end

--//点燃掉血
function dianran_state(RECEIVER, ARG0, ARG1)
	local hp_curr = Battle.get_prop(ARG0,ARG1,PT_HpCurr);
	Battle.changeProp_state(ARG0,ARG1,PT_HpCurr,-hp_curr/2);
end

function zhaohuan_8(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	local monsterclass = {70072,70072}
	Battle.add_Monster(ARG0,monsterclass)
end
