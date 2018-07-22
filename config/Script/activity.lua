
function activity_player_sign(RECEIVER)

end

function activity_half_hour_ago(RECEIVER)

end

function activity_challenge(RECEIVER)
	Activity.update(RECEIVER,ACT_Challenge,1);
end

function activity_kill_monster(RECEIVER)

end

function activity_jjc(RECEIVER)
	Activity.update(RECEIVER,ACT_JJC,1);
end

function activity_make_equipment(RECEIVER)

end


function activity_gain_skill_exp(RECEIVER,ARG0)

end

function activity_gain_baby(RECEIVER)

end

function activity_gain_employ(RECEIVER)

end

function activity_richang(RECEIVER)
	Activity.update(RECEIVER,ACT_Richang,1);
end

function activity_xuyuan(RECEIVER)
	Activity.update(RECEIVER,ACT_Xuyuan,1);
end

function activity_pet(RECEIVER)
	Activity.update(RECEIVER,ACT_Pet,1);
end

function activity_tongji(RECEIVER)
	Activity.update(RECEIVER,ACT_Tongji,1);
end

function activity_exam(RECEIVER)
	Activity.update(RECEIVER,ACT_Exam,1);
end