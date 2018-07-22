ALTER TABLE `Player`
ADD COLUMN `Money`  int(32) NOT NULL AFTER `PlayerGrade`,
ADD COLUMN `Diamond`  int(32) NOT NULL AFTER `Money`,
ADD COLUMN `Magic`  int(32) NOT NULL AFTER `Diamond`,
ADD COLUMN `LogoutTime`  bigint(20) NOT NULL AFTER `Magic`;
ALTER TABLE `Baby`
ADD COLUMN `BabyLevel`  int(32) NOT NULL AFTER `BabyName`,
ADD COLUMN `TableID`  int(32) NOT NULL AFTER `BabyLevel`,
ADD COLUMN `AddProp`  int(32) NOT NULL AFTER `TableID`;