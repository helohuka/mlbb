--RECEIVER
-- ARG_BATTLEID,		//0
-- ARG_CASTERPOS,		//1
-- ARG_TARGETPOS,		//2
-- ARG_POSTABLE,		//3
-- ARG_ORDERPARAM,		//4
-- ARG_MAX = 5
Sys.log("load guidskill.lua");
--script Sys.load_script "guidskill.lua"


--全体恢复
function guid_huifu_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3)	
	Battle.change_prop(ARG0,BP_Down2,PT_HpCurr,0);
	Battle.insert_state(ARG0,BP_Down2,202,0);
	Battle.change_prop(ARG0,BP_Down1,PT_HpCurr,0);
	Battle.insert_state(ARG0,BP_Down1,202,0);
	Battle.change_prop(ARG0,BP_Down0,PT_HpCurr,0);
	Battle.insert_state(ARG0,BP_Down0,202,0);
	Battle.change_prop(ARG0,BP_Down3,PT_HpCurr,0);
	Battle.insert_state(ARG0,BP_Down3,202,0);
	Battle.change_prop(ARG0,BP_Down4,PT_HpCurr,0);
	Battle.insert_state(ARG0,BP_Down4,202,0);
	Battle.change_prop(ARG0,BP_Down7,PT_HpCurr,0);
	Battle.insert_state(ARG0,BP_Down7,202,0);
end

--乱射4
function guid_luanshe_atk(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-400,false);
end

function guid_luanshe_skill_1(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Battle.change_prop(ARG0,BP_Up0,PT_HpCurr,-430,false);
end
function guid_luanshe_skill_2(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Battle.change_prop(ARG0,BP_Up3,PT_HpCurr,-400,false);
end
function guid_luanshe_skill_3(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-200,false);
end
function guid_luanshe_skill_4(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Battle.change_prop(ARG0,BP_Up1,PT_HpCurr,-400,false);
end
function guid_luanshe_skill_5(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	Battle.change_prop(ARG0,BP_Up4,PT_HpCurr,-430,false);
end

--强风
function guid_qiangli_fengren(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-502,false);
	Battle.change_prop(ARG0,BP_Up1,PT_HpCurr,-498,false);
	Battle.change_prop(ARG0,BP_Up3,PT_HpCurr,-483,false);
end

--战斧乾坤10
function guid_qiankun10(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-906,false);
end

--远程乾坤10
function guid_qiankun9(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-608,false);
end


--忍者攻击
function guid_renzhe_atk(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.insert_state(ARG0,BP_Down2,2,state_type);
	Battle.change_prop(ARG0,BP_Down2,PT_HpCurr,0,false);
	Battle.cutTime_state(ARG0,BP_Down2,ST_Dodge,1)
end

--全体补血
function guid_buxue_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Down0,PT_HpCurr,500);
	Battle.change_prop(ARG0,BP_Down1,PT_HpCurr,500);	
	Battle.change_prop(ARG0,BP_Down2,PT_HpCurr,90);	
	Battle.change_prop(ARG0,BP_Down3,PT_HpCurr,500);	
	--Battle.change_prop(ARG0,BP_Down4,PT_HpCurr,500);
	Battle.change_prop(ARG0,BP_Down7,PT_HpCurr,50);
end


--全体补血2
function guid_buxue_quanti2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Up0,PT_HpCurr,100);
	Battle.change_prop(ARG0,BP_Up1,PT_HpCurr,100);	
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,100);	
	Battle.change_prop(ARG0,BP_Up3,PT_HpCurr,100);	
	Battle.change_prop(ARG0,BP_Up4,PT_HpCurr,100);	
end

--单体补血
function guid_buxue_danti(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,1500);
end

--单体补血2
function guid_buxue_danti2(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Down1,PT_HpCurr,1000);
end

--[[//全体昏睡
function guid_hunshui_quanti(RECEIVER, ARG0, ARG1, ARG2,ARG3)
	local pos_table = get_opposite_pos(ARG0,ARG3,ARG2);
	for i=1,table.getn(pos_table),1
		do
			Battle.change_prop(ARG0,pos_table[i],PT_HpCurr,0);
	end
	Battle.insert_state(ARG0,BP_Down3,104,0);
	Battle.insert_state(ARG0,BP_Down1,104,0);
end]]--

--气功弹
function guid_qigongdan_6(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	Battle.change_prop(ARG0,BP_Down0,PT_HpCurr,-850,false);
	Battle.change_prop(ARG0,BP_Down1,PT_HpCurr,-850,true);
	Battle.change_prop(ARG0,BP_Down3,PT_HpCurr,-850,false);
	Battle.change_prop(ARG0,BP_Down4,PT_HpCurr,-850,false);
	Battle.change_prop(ARG0,BP_Down2,PT_HpCurr,-100,false);
	Battle.change_prop(ARG0,BP_Down7,PT_HpCurr,-55,false);
end

--[[--强力洁净
function guid_jiejing(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	Battle.change_prop(ARG0,BP_Down2,PT_HpCurr,0,false);
	Battle.change_prop(ARG0,BP_Down1,PT_HpCurr,0,false);
	Battle.change_prop(ARG0,BP_Down3,PT_HpCurr,0,false);
	Battle.remove_state(ARG0,BP_Down3,ST_Basilisk)
	Battle.remove_state(ARG0,BP_Down1,ST_Basilisk)
end]]--

--战斧连击
function guid_lianji_skill_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	local hpcurr = Battle.get_prop(ARG0,BP_Up2,PT_HpCurr);
	if hpcurr < 1 then
		return
	end
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-265,false);
end

--气绝回复
function guid_qijue(RECEIVER, ARG0, ARG1, ARG2)
	Battle.change_prop(ARG0,BP_Down4,PT_HpCurr,500);
end

--骑士诸人
function guid_zhuren_skill_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Down1,PT_HpCurr,-500,false);
end

--明镜
function guid_mingjing_skill_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,ARG1,PT_HpCurr,190,false);
end


--[[---攻吸
function guid_gongxi_skill_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Down2,PT_HpCurr,0,false);
	Battle.insert_state(ARG0,BP_Down2,5,0);
end]]--


--[[--中毒
function guid_zhongdu_skill_1(RECEIVER, ARG0, ARG1, ARG2, ARG3,ARG4)
	Battle.change_prop(ARG0,BP_Down1,PT_HpCurr,0,false);
	Battle.insert_state(ARG0,BP_Down1,94,0);
end]]--

--[[--崩击技能
function guid_bengji(RECEIVER, ARG0, ARG1, ARG2,ARG3,ARG4,level)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-1500,true);
end]]--

--全体魔法
function guid_mofa_quanti(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	Battle.change_prop(ARG0,BP_Up0,PT_HpCurr,-420);
	Battle.change_prop(ARG0,BP_Up1,PT_HpCurr,-400);
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-400);
	Battle.change_prop(ARG0,BP_Up3,PT_HpCurr,-400);
	Battle.change_prop(ARG0,BP_Up4,PT_HpCurr,-420);
	Battle.change_prop(ARG0,BP_Up7,PT_HpCurr,-450);
end

--单体魔法
function guid_mofa_danti(RECEIVER, ARG0, ARG1, ARG2, ARG3)
	Battle.change_prop(ARG0,BP_Up2,PT_HpCurr,-700);
end

