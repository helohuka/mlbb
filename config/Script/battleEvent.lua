--ARG0  Õ½¶·id

local test = 0;

function PlayerBattleStart(RECEIVER,ARG0, ARG1,ARG2,ARG3)
	AI_born_event(RECEIVER, ARG0, ARG1, ARG2, ARG3);
end

function PlayerBattleUpdata(RECEIVER,ARG0, ARG1, ARG2, ARG3)
	luanshe_num = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
	test = test + 1;
end

function PlayerBattleEnd(RECEIVER,ARG0, ARG1,ARG2)

end