--RECEIVER
-- ARG_TARGETID,		//0

Sys.log("load item.lua");
--script Sys.load_script "item.lua"
function item_bumo100(RECEIVER, ARG0)
	local damage = math.ceil(math.random(90,110));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo150(RECEIVER, ARG0)
	local damage = math.ceil(math.random(130,170));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo200(RECEIVER, ARG0)
	local damage = math.ceil(math.random(180,220));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo250(RECEIVER, ARG0)
	local damage = math.ceil(math.random(230,270));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo300(RECEIVER, ARG0)
	local damage = math.ceil(math.random(270,330));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo350(RECEIVER, ARG0)
	local damage = math.ceil(math.random(320,380));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo400(RECEIVER, ARG0)
	local damage = math.ceil(math.random(370,420));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo450(RECEIVER, ARG0)
	local damage = math.ceil(math.random(400,500));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo500(RECEIVER, ARG0)
	local damage = math.ceil(math.random(450,550));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo550(RECEIVER, ARG0)
	local damage = math.ceil(math.random(500,600));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo600(RECEIVER, ARG0)
	local damage = math.ceil(math.random(550,650));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo650(RECEIVER, ARG0)
	local damage = math.ceil(math.random(600,700));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo700(RECEIVER, ARG0)
	local damage = math.ceil(math.random(650,750));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo750(RECEIVER, ARG0)
	local damage = math.ceil(math.random(700,800));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo800(RECEIVER, ARG0)
	local damage = math.ceil(math.random(750,850));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo850(RECEIVER, ARG0)
	local damage = math.ceil(math.random(800,900));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo900(RECEIVER, ARG0)
	local damage = math.ceil(math.random(850,950));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo1000(RECEIVER, ARG0)
	local damage = math.ceil(math.random(950,1050));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end
function item_bumo1200(RECEIVER, ARG0)
	local damage = math.ceil(math.random(1150,1250));
	Player.change_property(RECEIVER,ARG0,PT_MpCurr,damage);
	return EN_None;
end




--血瓶道具
function item_buxue100(RECEIVER, ARG0)
	local damage = math.ceil(math.random(90,110));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue150(RECEIVER, ARG0)
	local damage = math.ceil(math.random(140,160));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue200(RECEIVER, ARG0)
	local damage = math.ceil(math.random(180,220));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue250(RECEIVER, ARG0)
	local damage = math.ceil(math.random(230,270));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue300(RECEIVER, ARG0)
	local damage = math.ceil(math.random(280,320));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue400(RECEIVER, ARG0)
	local damage = math.ceil(math.random(370,430));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue500(RECEIVER, ARG0)
	local damage = math.ceil(math.random(470,530));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue600(RECEIVER, ARG0)
	local damage = math.ceil(math.random(550,650));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue800(RECEIVER, ARG0)
	local damage = math.ceil(math.random(750,850));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

function item_buxue1000(RECEIVER, ARG0)
	local damage = math.ceil(math.random(950,1050));
	Player.change_property(RECEIVER,ARG0,PT_HpCurr,damage);
	return EN_None;
end

--加金币的道具
function add_jinbi1W(RECEIVER)
	Player.add_money(RECEIVER,1000)
	return EN_None;
end
function add_jinbi2W(RECEIVER)
	Player.add_money(RECEIVER,2000)
	return EN_None;
end
function add_jinbi3W(RECEIVER)
	Player.add_money(RECEIVER,3000)
	return EN_None;
end
function add_jinbi4W(RECEIVER)
	Player.add_money(RECEIVER,4000)
	return EN_None;
end
function add_jinbi5W(RECEIVER)
	Player.add_money(RECEIVER,5000)
	return EN_None;
end
function add_jinbi6W(RECEIVER)
	Player.add_money(RECEIVER,6000)
	return EN_None;
end

--加钻石道具
function add_zuanshi10(RECEIVER)
	Player.add_diamond(RECEIVER,10)
	return EN_None;
end

function add_zuanshi100(RECEIVER)
	Player.add_diamond(RECEIVER,100)
	return EN_None;
end
function add_zuanshi200(RECEIVER)
	Player.add_diamond(RECEIVER,200)
	return EN_None;
end
function add_zuanshi660(RECEIVER)
	Player.add_diamond(RECEIVER,660)
	return EN_None;
end
function add_zuanshi800(RECEIVER)
	Player.add_diamond(RECEIVER,800)
	return EN_None;
end
function add_zuanshi300(RECEIVER)
	Player.add_diamond(RECEIVER,300)
	return EN_None;
end
function add_zuanshi400(RECEIVER)
	Player.add_diamond(RECEIVER,400)
	return EN_None;
end
function add_zuanshi500(RECEIVER)
	Player.add_diamond(RECEIVER,500)
	return EN_None;
end
function add_zuanshi600(RECEIVER)
	Player.add_diamond(RECEIVER,600)
	return EN_None;
end
function add_zuanshi1000(RECEIVER)
	Player.add_diamond(RECEIVER,1000)
	return EN_None;
end
--单笔充值
function add_zuanshi60(RECEIVER)
	Player.add_diamond(RECEIVER,60)
	return EN_None;
end
function add_zuanshi980(RECEIVER)
	Player.add_diamond(RECEIVER,980)
	return EN_None;
end
function add_zuanshi1980(RECEIVER)
	Player.add_diamond(RECEIVER,1980)
	return EN_None;
end
function add_zuanshi3280(RECEIVER)
	Player.add_diamond(RECEIVER,3280)
	return EN_None;
end
function add_zuanshi6480(RECEIVER)
	Player.add_diamond(RECEIVER,6480)
	return EN_None;
end

--登陆
function add_zuanshi150(RECEIVER)
	Player.add_diamond(RECEIVER,150)
	return EN_None;
end
function add_zuanshi250(RECEIVER)
	Player.add_diamond(RECEIVER,250)
	return EN_None;
end
function add_zuanshi350(RECEIVER)
	Player.add_diamond(RECEIVER,350)
	return EN_None;
end

--紫色伙伴
function emp_purple(RECEIVER, ARG0)
	local emp = {2001,2007,2013,2019,2025,2031,2037,2043,2049,2055}
	local index = math.ceil(math.random(1,10));
	Player.add_Employee(RECEIVER,emp[index])
	return EN_None;
end

--橙色伙伴
function emp_golden(RECEIVER, ARG0)
	local emp = {2061,2062,2063,2064,2065,2066,2067,2068,2069,2070}
	local index = math.ceil(math.random(1,10));
	Player.add_Employee(RECEIVER,emp[index])
	return EN_None;
end

--僵尸改造
function jiangshi_gaizao(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 3 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,5081,1)
	Player.add_item(RECEIVER,5082,1)
	Player.add_item(RECEIVER,5083,1)
	return EN_None;
end


function item_guaguaka(RECEIVER, ARG0,ARG1)
	if Player.get_bag_free_slot(RECEIVER) <= 0 then
		return EN_OpenBaoXiangBagFull;
	end
	--local index = math.ceil(math.random(1,4));
	--if index <= 10 then
		--Player.add_item(RECEIVER,1001,1)
	--end
	Player.Lottery_Item(RECEIVER,ARG1)
	return EN_None;
end

--职业装备
function item_pro_equip_10(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 4 then
		return EN_OpenBaoXiangBagFull;
	end
	local pro = Player.get_property(RECEIVER,PT_Profession)
	if pro == JT_Axe then
		Player.add_item(RECEIVER,1025,1)
		Player.add_item(RECEIVER,1163,1)
		Player.add_item(RECEIVER,1210,1)
		Player.add_item(RECEIVER,1280,1);
	elseif pro == JT_Archer then
		Player.add_item(RECEIVER,1071,1)
		Player.add_item(RECEIVER,1186,1)
		Player.add_item(RECEIVER,1233,1)
		Player.add_item(RECEIVER,1303,1);
	elseif pro == JT_Mage then
		Player.add_item(RECEIVER,1094,1)
		Player.add_item(RECEIVER,1186,1)
		Player.add_item(RECEIVER,1256,1)
		Player.add_item(RECEIVER,1303,1);
	elseif pro == JT_Sage then
		Player.add_item(RECEIVER,1094,1)
		Player.add_item(RECEIVER,1186,1)
		Player.add_item(RECEIVER,1256,1)
		Player.add_item(RECEIVER,1303,1);
	else	
		Player.add_item(RECEIVER,1025,1)
		Player.add_item(RECEIVER,1163,1)
		Player.add_item(RECEIVER,1210,1)
		Player.add_item(RECEIVER,1280,1);
	end 
end

function item_pro_equip_20(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 4 then
		return EN_OpenBaoXiangBagFull;
	end
	local pro = Player.get_property(RECEIVER,PT_Profession)
	if pro == JT_Axe then
		Player.add_item(RECEIVER,1027,1)
		Player.add_item(RECEIVER,1165,1)
		Player.add_item(RECEIVER,1212,1)
		Player.add_item(RECEIVER,1282,1);
	elseif pro == JT_Archer then
		Player.add_item(RECEIVER,1073,1)
		Player.add_item(RECEIVER,1188,1)
		Player.add_item(RECEIVER,1235,1)
		Player.add_item(RECEIVER,1305,1);
	elseif pro == JT_Mage then
		Player.add_item(RECEIVER,1096,1)
		Player.add_item(RECEIVER,1188,1)
		Player.add_item(RECEIVER,1258,1)
		Player.add_item(RECEIVER,1305,1);
	elseif pro == JT_Sage then
		Player.add_item(RECEIVER,1096,1)
		Player.add_item(RECEIVER,1188,1)
		Player.add_item(RECEIVER,1258,1)
		Player.add_item(RECEIVER,1305,1);
	else	
		Player.add_item(RECEIVER,1027,1)
		Player.add_item(RECEIVER,1165,1)
		Player.add_item(RECEIVER,1212,1)
		Player.add_item(RECEIVER,1282,1);
	end 
end


function item_pro_equip_30(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 5 then
		return EN_OpenBaoXiangBagFull;
	end
	local pro = Player.get_property(RECEIVER,PT_Profession)
	if pro == JT_Axe then
		Player.add_item(RECEIVER,1029,1)
		Player.add_item(RECEIVER,1167,1)
		Player.add_item(RECEIVER,1214,1)
		Player.add_item(RECEIVER,1284,1);
	elseif pro == JT_Archer then
		Player.add_item(RECEIVER,1075,1)
		Player.add_item(RECEIVER,1190,1)
		Player.add_item(RECEIVER,1237,1)
		Player.add_item(RECEIVER,1307,1);
	elseif pro == JT_Mage then
		Player.add_item(RECEIVER,1098,1)
		Player.add_item(RECEIVER,1190,1)
		Player.add_item(RECEIVER,1260,1)
		Player.add_item(RECEIVER,1307,1);
	elseif pro == JT_Sage then
		Player.add_item(RECEIVER,1098,1)
		Player.add_item(RECEIVER,1190,1)
		Player.add_item(RECEIVER,1260,1)
		Player.add_item(RECEIVER,1307,1);
	elseif pro == JT_Sword then
		Player.add_item(RECEIVER,1006,1)
		Player.add_item(RECEIVER,1167,1)
		Player.add_item(RECEIVER,1214,1)
		Player.add_item(RECEIVER,1284,1)
		Player.add_item(RECEIVER,1330,1);
	elseif pro == JT_Knight then
		Player.add_item(RECEIVER,1052,1)
		Player.add_item(RECEIVER,1167,1)
		Player.add_item(RECEIVER,1214,1)
		Player.add_item(RECEIVER,1284,1);
	elseif pro == JT_Fighter then
		Player.add_item(RECEIVER,1190,1)
		Player.add_item(RECEIVER,1237,1)
		Player.add_item(RECEIVER,1307,1);
	elseif pro == JT_Ninja then
		Player.add_item(RECEIVER,1006,1)
	elseif pro == JT_Wizard then
		Player.add_item(RECEIVER,1098,1)
		Player.add_item(RECEIVER,1190,1)
		Player.add_item(RECEIVER,1260,1)
		Player.add_item(RECEIVER,1307,1);
	elseif pro == JT_Word then
		Player.add_item(RECEIVER,1098,1)
		Player.add_item(RECEIVER,1190,1)
		Player.add_item(RECEIVER,1260,1)
		Player.add_item(RECEIVER,1307,1);
	else	
		Player.add_item(RECEIVER,1029,1)
		Player.add_item(RECEIVER,1167,1)
		Player.add_item(RECEIVER,1214,1)
		Player.add_item(RECEIVER,1284,1);
	end 
end





--新手包
function item_xinshoubao(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 6 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_money(RECEIVER,10000)
	local pro = Player.get_property(RECEIVER,PT_Profession)
	local shuijing = {1351,1352,1353,1354}
	local index = math.ceil(math.random(1,4));
	Player.add_item(RECEIVER,shuijing[index],1)
	Player.add_item(RECEIVER,5100,1)
	Player.add_item(RECEIVER,5101,1)
	--Player.add_item(RECEIVER,5075,1)
	return EN_None;
end


function item_xinshoubao10(RECEIVER, ARG0)
	if Player.get_property(RECEIVER,PT_Level) < 10 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_bag_free_slot(RECEIVER) < 7 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(5055,1)
	Player.add_item(RECEIVER,5100,2)
	Player.add_item(RECEIVER,5101,2)
	Player.add_item(RECEIVER,5076,1)
	Player.add_item(RECEIVER,1696,1)
	return EN_None;
end

function item_xinshoubao20(RECEIVER, ARG0)
	if Player.get_property(RECEIVER,PT_Level) < 20 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_bag_free_slot(RECEIVER) < 7 then
		return EN_OpenBaoXiangBagFull;
	end
	local shuijing = {1348,1349,1350,1347}
	local index = math.ceil(math.random(1,4));
	Player.add_item(RECEIVER,shuijing[index],1)
	Player.add_item(RECEIVER,5100,2)
	Player.add_item(RECEIVER,5101,2)
	Player.add_item(RECEIVER,5077,1)
	Player.add_item(RECEIVER,1697,1)
	return EN_None;
end

function item_xinshoubao30(RECEIVER, ARG0)
	if Player.get_property(RECEIVER,PT_Level) < 30 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_bag_free_slot(RECEIVER) < 7 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,5100,2)
	Player.add_item(RECEIVER,5101,2)
	Player.add_item(RECEIVER,5078,1)
	Player.add_item(RECEIVER,1718,1)
	Player.add_item(RECEIVER,1698,1)
	return EN_None;
end

function item_xinshoubao40(RECEIVER, ARG0)
	if Player.get_property(RECEIVER,PT_Level) < 40 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_bag_free_slot(RECEIVER) < 7 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,1719,1)
	Player.add_item(RECEIVER,5102,2)
	Player.add_item(RECEIVER,5103,2)
	Player.add_item(RECEIVER,5079,1)
	Player.add_item(RECEIVER,1699,1)
	return EN_None;
end

function item_xinshoubao50(RECEIVER, ARG0)
	if Player.get_property(RECEIVER,PT_Level) < 50 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_bag_free_slot(RECEIVER) < 7 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,5103,2)
	Player.add_item(RECEIVER,5102,2)
	Player.add_item(RECEIVER,5080,1)
	Player.add_item(RECEIVER,5074,1)
	Player.add_item(RECEIVER,1723,1)
	return EN_None;
end

function item_xinshoubao60(RECEIVER, ARG0)
	if Player.get_property(RECEIVER,PT_Level) < 60 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_bag_free_slot(RECEIVER) < 7 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,5103,2)
	Player.add_item(RECEIVER,5102,2)
	Player.add_item(RECEIVER,1460,1)
	Player.add_item(RECEIVER,3781,1)
	Player.add_item(RECEIVER,3791,1)
	return EN_None;
end


--首冲礼包
function item_shouchong(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_questcounter(RECEIVER,90011);
	local pro = Player.get_property(RECEIVER,PT_Profession)
	if pro == JT_Axe then
		Player.add_item(RECEIVER,21025,1)
	elseif pro == JT_Archer then
		Player.add_item(RECEIVER,21071,1)
	elseif pro == JT_Mage then
		Player.add_item(RECEIVER,21094,1)
	elseif pro == JT_Sage then
		Player.add_item(RECEIVER,21094,1)
	elseif pro == JT_Wizard then
		Player.add_item(RECEIVER,21094,1)
	elseif pro == JT_Sword then
		Player.add_item(RECEIVER,21002,1)
	elseif pro == JT_Knight then
		Player.add_item(RECEIVER,21048,1)
	elseif pro == JT_Animal then
		Player.add_item(RECEIVER,21117,1)
	elseif pro == JT_Word then
		Player.add_item(RECEIVER,21094,1)
	elseif pro == JT_Ninja then
		Player.add_item(RECEIVER,21002,1)
	elseif pro == JT_Fighter then
		Player.add_item(RECEIVER,21234,1)
	else
		Player.add_item(RECEIVER,21002,1)
	end 
	return EN_None;
end

--60充值返利3级破标武器
function item_3jipbwuqi(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_questcounter(RECEIVER,90011);
	local pro = Player.get_property(RECEIVER,PT_Profession)
	if pro == JT_Axe then
		Player.add_item(RECEIVER,21027,1)
	elseif pro == JT_Archer then
		Player.add_item(RECEIVER,21073,1)
	elseif pro == JT_Mage then
		Player.add_item(RECEIVER,21096,1)
	elseif pro == JT_Sage then
		Player.add_item(RECEIVER,21096,1)
	elseif pro == JT_Wizard then
		Player.add_item(RECEIVER,21096,1)
	elseif pro == JT_Sword then
		Player.add_item(RECEIVER,21004,1)
	elseif pro == JT_Knight then
		Player.add_item(RECEIVER,21050,1)
	elseif pro == JT_Animal then
		Player.add_item(RECEIVER,21119,1)
	elseif pro == JT_Word then
		Player.add_item(RECEIVER,21096,1)
	elseif pro == JT_Ninja then
		Player.add_item(RECEIVER,21004,1)
	elseif pro == JT_Fighter then
		Player.add_item(RECEIVER,21236,1)
	else
		Player.add_item(RECEIVER,21004,1)
	end 
	return EN_None;
end

 -- 充值符文礼包
function item_3jifuwen(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {10001,10002,10003,10011,10012,10013,10021,10022,10023,10031,10032,10033,10041,10042,10043,10051,10052,10053,10061,10062,10063,10071,10072,10073}
	local index = math.ceil(math.random(1,24));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

 --6级符文礼包
function item_6jifuwen(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {10006,10016,10026,10036,10046,10056,10066,10076}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

 --7级符文礼包
function item_7jifuwen(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {10007,10017,10027,10037,10047,10057,10067,10077}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

 --8级符文礼包
function item_8jifuwen(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {10008,10018,10028,10038,10048,10058,10068,10078}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

--9级符文礼包
function item_9jifuwen(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {10009,10019,10029,10039,10049,10059,10069,10079}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

--10级符文礼包
function item_10jifuwen(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {10010,10020,10030,10040,10050,10060,10070,10080}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

--低级宠物装备礼包
function item_1jichongwuzhuangbei(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {70001,70002,70003,70004,70005,70006,70007,70008}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

--中级宠物装备礼包
function item_2jichongwuzhuangbei(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {70009,70010,70011,70012,70005,70006,70007,70008}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end

--高级宠物装备礼包
function item_3jichongwuzhuangbei(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_fuwen = {70009,70010,70011,70012,70013,70014,70015,700016}
	local index = math.ceil(math.random(1,8));
	Player.add_item(RECEIVER,item_fuwen[index],1)
    return EN_None;
end


 -- 充值王宠礼包
function baby_wangchong(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local item_wangchong = {30002,30003}
	local index = math.ceil(math.random(1,2));
	Player.add_baby(RECEIVER,item_wangchong[index],1);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
    return EN_None;
end


 -- 影子礼包
function baby_yingzi(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local item_wangchong = {10109,10110,10111,10112}
	local index = math.ceil(math.random(1,4));
	Player.add_baby(RECEIVER,item_wangchong[index],1);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
    return EN_None;
end

 -- 野外战场王宠宝箱
function baby_wangchong_PVP(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local item_wangchong = {30000,30004,30006,30008,30010}
	local index = math.ceil(math.random(1,5));
	Player.add_baby(RECEIVER,item_wangchong[index],1);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
    return EN_None;
end

 -- 龙系王宠宝箱
function baby_wangchong_long(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local item_wangchong = {30001,30007,30009,30011}
	local index = math.ceil(math.random(1,4));
	Player.add_baby(RECEIVER,item_wangchong[index],1);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
    return EN_None;
end

 -- 随机宠物宝箱
function baby_chongwu(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
    index_x = math.ceil(math.random(1,100));
	if index_x == 1 then
		local item_chongwu = {10019,10125,10126,30000,30004,30006,30008,30010}
		local index = math.ceil(math.random(1,8));
		Player.add_baby(RECEIVER,item_chongwu[index],1);
		Player.send_errorno(RECEIVER,EN_AddMoney2W);
	else
		local item_chongwu = {1,3,5,9,17,26,27,28,30,33,34,45,51,54,59,61,62,65,67,77,1000,10001,10002,10003,10004,10005,10006,10007,10008,10009,10011,10013,10014,10015,10016,10017,10018,10020,10021,10023,10024,10025,10026,10028,10030,10032,10033,10038,10039,10040,10041,10043,10048,10049,10051,10052,10053,10054,10055,10057,10058,10059,10061,10063,10064,10066,10067,10068,10069,10070,10071,10072,10073,10074,10076,10078,10080,10081,10082,10083,10084,10087,10088,10090,10091,10092,10094,10096,10098,10099,10100,10101,10102,10103,10104,10105,10106,10107,10108,10113,10114,10115,10116,10118,10121,10122,10123,10124,10127,10128,10130,10131,10132,10133,10135,10136,10137,10138,10140,10141,10142,10143,10149,10150,10151,10152,10153,10154,10156,10157,10159,10160}
		local index = math.ceil(math.random(1,132));
		Player.add_baby(RECEIVER,item_chongwu[index],1);
		Player.send_errorno(RECEIVER,EN_AddMoney2W);
	end
    return EN_None;
end

--家族战礼包
function item_jiazuzhan(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 4 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,5098,2)
	Player.add_item(RECEIVER,5099,2)
	Player.add_item(RECEIVER,5045,1)
	Player.add_item(RECEIVER,5044,1)
	return EN_None;
end

function item_xuecunchu_10w(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	if true == Entity.check_state(RECEIVER,1000) then
		local cv, mv = Entity.get_state_value(RECEIVER,1000);
		cv = cv + 100000;
		mv = mv + 100000;
		Entity.set_state_value(RECEIVER,1000,cv,mv);
		zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1000,100000,100000);
	zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	return EN_None;
end

function item_xuecunchu_5Q(RECEIVER,ARG0, ARG1, ARG2)
	if true == Entity.check_state(RECEIVER,1000) then
		local cv, mv = Entity.get_state_value(RECEIVER,1000);
		cv = cv + 5000;
		mv = mv + 5000;
		Entity.set_state_value(RECEIVER,1000,cv,mv);
		zidongbuxue(RECEIVER,ARG0, ARG1, ARG2)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1000,5000,5000);
	zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, 0)
	return EN_None;
end

function item_xuecunchu_1w(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	if true == Entity.check_state(RECEIVER,1000) then
		local cv, mv = Entity.get_state_value(RECEIVER,1000);
		cv = cv + 10000;
		mv = mv + 10000;
		Entity.set_state_value(RECEIVER,1000,cv,mv);
		zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1000,10000,10000);
	zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	return EN_None;
end

function item_xuecunchu_5w(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	if true == Entity.check_state(RECEIVER,1000) then
		local cv, mv = Entity.get_state_value(RECEIVER,1000);
		cv = cv + 50000;
		mv = mv + 50000;
		Entity.set_state_value(RECEIVER,1000,cv,mv);
		zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1000,50000,50000);
	zidongbuxue(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	return EN_None;
end

function item_mocunchu_10w(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	if true == Entity.check_state(RECEIVER,1001) then
		local cv, mv = Entity.get_state_value(RECEIVER,1001);
		cv = cv + 100000;
		mv = mv + 100000;
		Entity.set_state_value(RECEIVER,1001,cv,mv);
		zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1001,100000,100000);
	zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	return EN_None;
end

function item_mocunchu_5w(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	if true == Entity.check_state(RECEIVER,1001) then
		local cv, mv = Entity.get_state_value(RECEIVER,1001);
		cv = cv + 50000;
		mv = mv + 50000;
		Entity.set_state_value(RECEIVER,1001,cv,mv);
		zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1001,50000,50000);
	zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	return EN_None;
end
function item_mocunchu_5Q(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	if true == Entity.check_state(RECEIVER,1001) then
		local cv, mv = Entity.get_state_value(RECEIVER,1001);
		cv = cv + 5000;
		mv = mv + 5000;
		Entity.set_state_value(RECEIVER,1001,cv,mv);
		zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1001,5000,5000);
	zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	return EN_None;
end
function item_mocunchu_1w(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	if true == Entity.check_state(RECEIVER,1001) then
		local cv, mv = Entity.get_state_value(RECEIVER,1001);
		cv = cv + 10000;
		mv = mv + 10000;
		Entity.set_state_value(RECEIVER,1001,cv,mv);
		zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
		return EN_None;
	end 
	Entity.insert_state(RECEIVER,1001,10000,10000);
	zidongbumo(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	return EN_None;
end

function item_zaisheng(RECEIVER)
	if Player.get_property(RECEIVER,PT_Level) < 20 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_property(RECEIVER,PT_Stama) < 1 and Player.get_property(RECEIVER,PT_Strength) < 1 and Player.get_property(RECEIVER,PT_Power) < 1 and Player.get_property(RECEIVER,PT_Speed) < 1 and Player.get_property(RECEIVER,PT_Magic) < 1 then
		return EN_PropisNull;
	end
	Player.reset_property(RECEIVER)
	return EN_None;
end

function item_zhongzi_liliang(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_Strength) < 1 then
		return EN_NoThisPoint
	end
	Player.sub_property(RECEIVER,PT_Strength)
	return EN_None;
end
function item_zhongzi_sudu(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_Speed) < 1 then
		return EN_NoThisPoint
	end
	Player.sub_property(RECEIVER,PT_Speed)
	return EN_None;
end
function item_zhongzi_tili(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_Stama) < 1 then
		return EN_NoThisPoint
	end
	Player.sub_property(RECEIVER,PT_Stama)
	return EN_None;
end
function item_zhongzi_qiangdu(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_Power) < 1 then
		return EN_NoThisPoint
	end
	Player.sub_property(RECEIVER,PT_Power)
	return EN_None;
end
function item_zhongzi_moli(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_Magic) < 1 then
		return EN_NoThisPoint
	end
	Player.sub_property(RECEIVER,PT_Magic)
	return EN_None;
end

--时间水晶
function shishui_LV1(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_DoubleExp) >= 21600 then
		return EN_DoubleExpTimeFull
	end
	Player.change_property(RECEIVER,ARG0,PT_DoubleExp,3600);
	return EN_None;
end
function shishui_LV2(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_DoubleExp) >= 21600 then
		return EN_DoubleExpTimeFull
	end
	Player.change_property(RECEIVER,ARG0,PT_DoubleExp,7200);
	return EN_None;
end
function shishui_LV3(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_DoubleExp) >= 21600 then
		return EN_DoubleExpTimeFull
	end
	Player.change_property(RECEIVER,ARG0,PT_DoubleExp,10800);
	return EN_None;
end
function shishui_LV6(RECEIVER,ARG0)
	if Player.get_property(RECEIVER,PT_DoubleExp) >= 21600 then
		return EN_DoubleExpTimeFull
	end
	Player.change_property(RECEIVER,ARG0,PT_DoubleExp,21600);
	return EN_None;
end

function jinengcao_200(RECEIVER,ARG0)
	if Player.canuseaddskillexpitem(RECEIVER) then
		Player.add_SkillExp(RECEIVER,1756)
		return EN_None;
	end
	return EN_SkillExperr;
end

function jinengcao_500(RECEIVER,ARG0)
	if Player.canuseaddskillexpitem(RECEIVER) then
		Player.add_SkillExp(RECEIVER,1757)
		return EN_None;
	end
	return EN_SkillExperr;
end

function jinengcao_5000(RECEIVER,ARG0)
	if Player.canuseaddskillexpitem(RECEIVER) then
		Player.add_SkillExp(RECEIVER,1767)
		return EN_None;
	end
	return EN_SkillExperr;
end

function jinengcao_10000(RECEIVER,ARG0)
	if Player.canuseaddskillexpitem(RECEIVER) then
		Player.add_SkillExp(RECEIVER,1768)
		return EN_None;
	end
	return EN_SkillExperr;
end

function jinengcao_10W(RECEIVER,ARG0)
	if Player.canuseaddskillexpitem(RECEIVER) then
		Player.add_SkillExp(RECEIVER,1758)
		return EN_None;
	end
	return EN_SkillExperr;
end


function baby_exp_5000(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,5000)
	return EN_None;
end

function baby_exp_10000(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,10000)
	return EN_None;
end

function baby_exp_20000(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,20000)
	return EN_None;
end

function baby_exp_10W(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,100000)
	return EN_None;
end

function baby_exp_20W(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,200000)
	return EN_None;
end

function baby_exp_50W(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,500000)
	return EN_None;
end

function baby_exp_100W(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,1000000)
	return EN_None;
end
function baby_exp_200W(RECEIVER)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	if Player.get_BattleBabyProp(RECEIVER,PT_Level) >= Player.get_property(RECEIVER,PT_Level) + 5 then
		return EN_BabyLevelHigh;
	end
	Player.add_ExpByBattleBaby(RECEIVER,2000000)
	return EN_None;
end

function open_store_item(RECEIVER)
	local num = Player.getstoragesize(RECEIVER,ST_Item)
	if num >= 100 then
		return EN_Storefull;
	end
	Player.openstoragegrid(RECEIVER,ST_Item)
	return EN_None;
end

function open_store_baby(RECEIVER)
	local num = Player.getstoragesize(RECEIVER,ST_Baby)
	if num >= 30 then
		return EN_Storefull;
	end
	Player.openstoragegrid(RECEIVER,ST_Baby)
	return EN_None;
end

function baby_jiangshi(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,10024);
    return EN_None;
end
function baby_haidigui(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,17);
    return EN_None;
end
function baby_daofei(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,20);
    return EN_None;
end
function baby_kulouzhanshi(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,26);
    return EN_None;
end
function baby_tiejianpangxie(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,27);
    return EN_None;
end
function baby_duxie(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,28);
    return EN_None;
end
function baby_shirenmo(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,30);
    return EN_None;
end
function baby_heixiong(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,33);
    return EN_None;
end
function baby_tanshishou(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,49);
    return EN_None;
end
function baby_paomoruanniguai(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,51);
    return EN_None;
end
function baby_guaiguaixiang(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,61);
    return EN_None;
end
function baby_shuijingpo(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,62);
    return EN_None;
end
function baby_chouchouhua(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,65);
    return EN_None;
end
function baby_xixuehaibianfu(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,34);
    return EN_None;
end
function baby_shayingcike(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,76);
    return EN_None;
end
function baby_shalang(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,77);
    return EN_None;
end
function baby_shirenhua(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,30003);
    return EN_None;
end
function baby_chihou(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,30007);
    return EN_None;
end
function baby_yijiaozhanglao(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,83);
    return EN_None;
end
function baby_yijiaozhihuo(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,85);
    return EN_None;
end
function baby_ciqiu(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,92);
    return EN_None;
end
function baby_yijiaotu(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,82);
    return EN_None;
end
function baby_shuigui(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,67);
    return EN_None;
end
function baby_shuiguicao(RECEIVER,ARG0)
    if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,45);
    return EN_None;
end

--神器
function suipian_shenqi1(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local suipian = {3601,3602,3603,3604}
	local index = math.ceil(math.random(1,4));
	Player.add_item(RECEIVER,suipian[index],1)
	return EN_None;
end

function suipian_shenqi2(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local suipian = {3508,3506,3507,3508}
	local index = math.ceil(math.random(1,4));
	Player.add_item(RECEIVER,suipian[index],1)
	return EN_None;
end

function suipian_shenqi3(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local suipian = {3515,3605,3703}
	local index = math.ceil(math.random(1,3));
	Player.add_item(RECEIVER,suipian[index],1)
	return EN_None;
end




--宠物碎片
function baby_7040(RECEIVER, ARG0)
	if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local baby_table = {30001,30011}
	local index = math.ceil(math.random(1,2));
	Player.add_baby(RECEIVER,baby_table[index]);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
	return EN_None;
end

function baby_7041(RECEIVER, ARG0)
	if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local baby_table = {65,10007,10028,10030,10033,10067,10069,10070,10091,10113,10126,10128,10130,10157,10160,30002,30003,30004,30008,30010,10032,10078,10105,10122,10125,10150,10151}
	local index = math.ceil(math.random(1,27));
	Player.add_baby(RECEIVER,baby_table[index]);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
	return EN_None;
end

function baby_7042(RECEIVER, ARG0)
	if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local baby_table = {10011,10043,10127,10135,10136,10137,10138,10140,26,34,10039,10053,10072,10114,10115,10133,10149,10052,1,27,62,77,10001,10004,10008,10013,10021,10064,10080,10081,10092,10099,10100,10101,10102,10103,10116,10141,10142,10159,30006}
	local index = math.ceil(math.random(1,42));
	Player.add_baby(RECEIVER,baby_table[index]);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
	return EN_None;
end

function baby_7043(RECEIVER, ARG0)
	if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local baby_table = {30,33,58,59,10005,10009,10018,10019,10071,10090,10123,10124,10055,10111,10143,10152,30000,9,18,28,61,10002,10014,10016,10017,10036,10054,10059,10066,10082,10104,10106,10121,10131}
	local index = math.ceil(math.random(1,33));
	Player.add_baby(RECEIVER,baby_table[index]);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
	return EN_None;
end

function baby_7044(RECEIVER, ARG0)
	if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	local baby_table = {10041,10132,3,5,17,45,51,67,10003,10006,10025,10026,10038,10040,10048,10049,10051,10057,10058,10061,10063,10068,10073,10074,10076,10083,10084,10087,10088,10094,10096,10098,10107,10108,10118,10153,10154,10156}
	local index = math.ceil(math.random(1,38));
	Player.add_baby(RECEIVER,baby_table[index]);
	Player.send_errorno(RECEIVER,EN_AddMoney2W);
	return EN_None;
end

function suipian_zhanshen(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,102026,80)
	return EN_None;
end

function suipian_liefengyilong(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,110138,50)
	return EN_None;
end

function suipian_yatelasi(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,110151,50)
	return EN_None;
end

function baoshi(RECEIVER, ARG0,level)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local baoshi = {3724+level-1,3734+level-1,3744+level-1,3754+level-1,3764+level-1,3774+level-1,3784+level-1}
	local index = math.ceil(math.random(1,7));
	Player.add_item(RECEIVER,baoshi[index],1)
	return EN_None;
end



function baoxiang_gaizaotu(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local index = math.ceil(math.random(5120,5134));
	Player.add_item(RECEIVER,index,1)
	return EN_None;
end

function baoxiang_gaizaotu1(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local index = math.ceil(math.random(5135,5152));
	Player.add_item(RECEIVER,index,1)
	return EN_None;
end
 -- 装备锻造随机礼包5
function item_formula_5(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_formula_5 = {5175,5176,5177,5198,5199,5200,5221,5222,5223,5244,5245,5246,5267,5268,5269,5290,5291,5292,5336,5337,5338,5359,5360,5361,5383,5384,5385,5406,5407,5408,5430,5431,5432,5453,5454,5455,5476,5477,5478,5499,5500,5501}
	local index = math.ceil(math.random(1,42));
	Player.add_item(RECEIVER,item_formula_5[index],1)
    return EN_None;
end

 -- 装备锻造随机礼包6
function item_formula_6(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_formula_6 = {5178,8179,5201,5202,5224,5225,5247,5248,5270,5271,5293,5294,5339,5340,5362,5363,5386,5387,5409,5410,5433,5434,5456,5457,5479,5480,5502,5503}
	local index = math.ceil(math.random(1,28));
	Player.add_item(RECEIVER,item_formula_6[index],1)
    return EN_None;
end
 -- 装备锻造随机礼包7
function item_formula_7(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_formula_7 = {5180,5181,5182,5203,5204,5205,5226,5227,5228,5249,5250,5251,5272,5273,5274,5295,5296,5297,5341,5342,5343,5364,5365,5366,5388,5389,5390,5411,5412,5413,5435,5436,5437,5458,5459,5460,5481,5482,5483,5504,5505,5506}
	local index = math.ceil(math.random(1,42));
	Player.add_item(RECEIVER,item_formula_7[index],1)
    return EN_None;
end

 -- 装备锻造随机礼包8
function item_formula_8(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_formula_8 = {5183,5184,5185,5206,5207,5208,5229,5230,5231,5252,5253,5254,5275,5276,5277,5298,5299,5300,5344,5345,5346,5367,5368,5369,5370,5391,5392,5393,5414,5415,5416,5438,5439,5440,5461,5462,5463,5484,5485,5486,5807,5808,5809}
	local index = math.ceil(math.random(1,43));
	Player.add_item(RECEIVER,item_formula_8[index],1)
    return EN_None;
end

--材料包
function item_cailiaobao_1(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3004,30)
	Player.add_item(RECEIVER,3104,30)
	Player.add_item(RECEIVER,3404,30)
    return EN_None;
end

function item_cailiaobao_2(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3005,30)
	Player.add_item(RECEIVER,3105,30)
	Player.add_item(RECEIVER,3405,30)
    return EN_None;
end

function item_cailiaobao_3(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3006,30)
	Player.add_item(RECEIVER,3106,30)
	Player.add_item(RECEIVER,3406,30)
    return EN_None;
end

--家族材料包
function item_jiazucailiaobao_1(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3001,20)
	Player.add_item(RECEIVER,3101,20)
	Player.add_item(RECEIVER,3401,20)
    return EN_None;
end

function item_jiazucailiaobao_2(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3002,20)
	Player.add_item(RECEIVER,3102,20)
	Player.add_item(RECEIVER,3402,20)
    return EN_None;
end

function item_jiazucailiaobao_3(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3003,20)
	Player.add_item(RECEIVER,3103,20)
	Player.add_item(RECEIVER,3403,20)
    return EN_None;
end

function item_jiazucailiaobao_4(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3004,20)
	Player.add_item(RECEIVER,3104,20)
	Player.add_item(RECEIVER,3404,20)
    return EN_None;
end

function item_jiazucailiaobao_5(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3005,20)
	Player.add_item(RECEIVER,3105,20)
	Player.add_item(RECEIVER,3405,20)
    return EN_None;
end

function item_jiazucailiaobao_6(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3006,20)
	Player.add_item(RECEIVER,3106,20)
	Player.add_item(RECEIVER,3406,20)
    return EN_None;
end

function item_jiazucailiaobao_7(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3007,20)
	Player.add_item(RECEIVER,3107,20)
	Player.add_item(RECEIVER,3407,20)
    return EN_None;
end

function item_jiazucailiaobao_8(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3008,20)
	Player.add_item(RECEIVER,3108,20)
	Player.add_item(RECEIVER,3408,20)
    return EN_None;
end

function item_jiazucailiaobao_9(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3009,20)
	Player.add_item(RECEIVER,3109,20)
	Player.add_item(RECEIVER,3409,20)
    return EN_None;
end

function item_jiazucailiaobao_10(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) <= 2 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_item(RECEIVER,3010,20)
	Player.add_item(RECEIVER,3110,20)
	Player.add_item(RECEIVER,3410,20)
    return EN_None;
end

--渠道礼包
function item_libao1(RECEIVER, ARG0)
	Player.add_diamond(RECEIVER,300)
	return EN_None;
end

function item_libao2(RECEIVER, ARG0)
	if Player.get_property(RECEIVER,PT_Level) < 5 then
		return EN_OpenBaoXiangLevel;
	end
	if Player.get_baby_size(RECEIVER) < 0 or  Player.get_baby_size(RECEIVER) >= 3 then
        return EN_BabyFull;
    end
	Player.add_baby(RECEIVER,10156);
	return EN_None;
end

--进群礼包300钻、3W金、宠物强化之魂*5
function item_libao3(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	Player.add_diamond(RECEIVER,200)
	Player.add_money(RECEIVER,6000)
	Player.add_item(RECEIVER,5086,3)
	return EN_None;
end

--BUG提交礼包600钻、3W金、神秘火种*5
function item_libao4(RECEIVER, ARG0)
	Player.add_diamond(RECEIVER,100)
	Player.add_money(RECEIVER,6000)
	return EN_None;
end

function baoshi_1(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,1)
end
function baoshi_2(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,2)
end
function baoshi_3(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,3)
end
function baoshi_4(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,4)
end
function baoshi_5(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,5)
end
function baoshi_6(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,6)
end
function baoshi_7(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,7)
end
function baoshi_8(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,8)
end
function baoshi_9(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,9)
end
function baoshi_10(RECEIVER, ARG0)
	return baoshi(RECEIVER, ARG0,10)
end


--采集资格证道具
function caiji_zigezheng_1(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 4,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 4,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_1(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 4,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 4,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_2(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 5,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 5,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_2(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 5,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 5,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_3(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 6,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 6,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_3(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,6,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 6,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_4(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 10,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 10,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_4(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,10,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 10,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_5(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 11,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 11,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_5(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,11,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 11,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_6(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 12,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 12,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_6(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,12,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 12,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_7(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 16,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 16,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_7(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,16,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 16,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_8(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 17,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 17,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_8(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,17,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 17,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_9(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 18,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 18,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_9(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,18,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 18,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_10(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 19,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 19,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_10(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,19,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 19,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_11(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 20,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 20,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_11(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,20,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 20,GST_Advanced);
	return EN_None;
end

function caiji_zigezheng_12(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER, 21,GST_Vulgar)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 21,GST_Vulgar);
	return EN_None;
end

function caiji_GJzigezheng_12(RECEIVER, ARG0)
	local canuse = Player.checkOpenGather(RECEIVER,21,GST_Advanced)
	if canuse ~= EN_None then
		return canuse
	end
	Player.opengather(RECEIVER, 21,GST_Advanced);
	return EN_None;
end

function duanzao_1(RECEIVER, ARG0,ARG1)
	local isuse = Player.checkcompound(RECEIVER,ARG1-4167)
	if isuse == true then
		Player.opencompound(RECEIVER, ARG1-4167)
		return EN_None;
	else
		return EN_UseMakeRepeat
	end
end

 -- 随机4级装备
function item_equip_30(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_30 = {1006,1007,1029,1030,1052,1053,1075,1076,1098,1099,1121,1122,1167,1168,1190,1191,1214,1215,1237,1238,1260,1261,1262,1284,1285,1307,1308,1330,1331}
	local item_pb_30 = {21006,21007,21029,21030,21052,21053,21075,21076,21098,21099,21121,21122,21167,21168,21190,21191,21214,21215,21237,21238,21260,21261,21262,21284,21285,21307,21308,21330,21331}
	local index_x = math.ceil(math.random(1,100));
	local index = math.ceil(math.random(1,29));
	if index_x < 4 then
		Player.add_item(RECEIVER,item_pb_30[index],1)
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."在副本装备宝箱中获得了破标装备"
		Sys.notice(str)
	else
		Player.add_item(RECEIVER,item_30[index],1)
	end
    return EN_None;
end
 -- 随机5级装备
function item_equip_40(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_40 = {1008,1009,1010,1031,1032,1033,1054,1055,1056,1077,1078,1079,1100,1101,1102,1123,1124,1125,1169,1170,1171,1192,1193,1194,1216,1217,1218,1239,1240,1241,1263,1264,1265,1286,1287,1288,1309,1310,1311,1332,1333,1334}
	local item_pb_40 = {21008,21009,21010,21031,21032,21033,21054,21055,21056,21077,21078,21079,21100,21101,21102,21123,21124,21125,21169,21170,21171,21192,21193,21194,21216,21217,21218,21239,21240,21241,21263,21264,21265,21286,21287,21288,21309,21310,21311,21332,21333,21334}
	local index_x = math.ceil(math.random(1,100));
	local index = math.ceil(math.random(1,42));
	if index_x < 4 then
		Player.add_item(RECEIVER,item_pb_40[index],1)
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."在副本装备宝箱中获得了破标装备"
		Sys.notice(str)
	else
		Player.add_item(RECEIVER,item_40[index],1)
	end
    return EN_None;
end
 -- 随机6级装备
function item_equip_50(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_50 = {1011,1012,1034,1035,1057,1058,1080,1081,1103,1104,1126,1127,1172,1173,1195,1196,1219,1220,1242,1243,1266,1267,1289,1290,1312,1313,1335,1336}
	local item_pb_50 = {21011,21012,21034,21035,21057,21058,21080,21081,21103,21104,21126,21127,21172,21173,21195,21196,21219,21220,21242,21243,21266,21267,21289,21290,21312,21313,21335,21336}
	local index_x = math.ceil(math.random(1,100));
	local index = math.ceil(math.random(1,28));
	if index_x < 4 then
		Player.add_item(RECEIVER,item_pb_50[index],1)
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."在副本装备宝箱中获得了破标装备"
		Sys.notice(str)
	else
		Player.add_item(RECEIVER,item_50[index],1)
	end
    return EN_None;
end
 -- 随机7级装备
function item_equip_60(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_60 = {1013,1014,1015,1036,1037,1038,1059,1060,1061,1082,1083,1084,1105,1106,1107,1128,1129,1130,1174,1175,1176,1197,1198,1199,1221,1222,1223,1244,1245,1246,1268,1269,1270,1291,1292,1293,1314,1315,1316,1337,1338,1339}
	local item_pb_60 = {21013,21014,21015,21036,21037,21038,21059,21060,21061,21082,21083,21084,21105,21106,21107,21128,21129,21130,21174,21175,21176,21197,21198,21199,21221,21222,21223,21244,21245,21246,21268,21269,21270,21291,21292,21293,21314,21315,21316,21337,21338,21339}
	local index_x = math.ceil(math.random(1,100));
	local index = math.ceil(math.random(1,42));
	if index_x < 4 then
		Player.add_item(RECEIVER,item_pb_60[index],1)
		local name = Player.getplayerName(RECEIVER)
		local str = "恭喜".."[FF0000]"..name.."[-]".."在副本装备宝箱中获得了破标装备"
		Sys.notice(str)
	else
		Player.add_item(RECEIVER,item_60[index],1)
	end
    return EN_None;
end

 -- 100次任务奖励
function item_renwulian_100(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_100 = {21350,3806,3807,5045,10003,10013,10023,10033,10043,10053,10063,10073,5509}
	local item_shuliang = {5,1,1,1,1,1,1,1,1,1,1,1,1}
	local index = math.ceil(math.random(1,13));
	Player.add_item(RECEIVER,item_100[index],item_shuliang[index])
    return EN_None;
end

 -- 200次任务奖励
function item_renwulian_200(RECEIVER, ARG0)
	if Player.get_bag_free_slot(RECEIVER) < 1 then
		return EN_OpenBaoXiangBagFull;
	end
	local item_200 = {21350,3807,3808,5046,10004,10014,10024,10034,10044,10054,10064,10074,5510}
	local item_shuliang = {10,1,1,1,1,1,1,1,1,1,1,1,1}
	local index = math.ceil(math.random(1,13));
	Player.add_item(RECEIVER,item_200[index],item_shuliang[index])
    return EN_None;
end

--//神奇的包裹
function item_baoguo100(RECEIVER, ARG0,ARG1)
	if Player.getCurBagGirdNum(RECEIVER) >= 100 then
		return EN_BagSizeMax;
	end
	Player.openbaggrid(RECEIVER,ARG1);
	return EN_None;
end

function item_qianghua10(RECEIVER, ARG0)
	local babyinstid = Player.get_BattleBaby(RECEIVER)
	if babyinstid == 0 then
		return EN_NoBattleBaby;
	end
	
	if Player.getbabyintensifylevel(RECEIVER,babyinstid) >= 10 then
		return EN_NoBaby
	end
	Player.intensifybaby2target(RECEIVER,babyinstid,10)
end

function item_daliwan(RECEIVER, ARG0)
	Player.reset_property_1(RECEIVER)
	return EN_None;
end
