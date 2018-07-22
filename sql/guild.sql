ALTER TABLE `Guild`
ADD COLUMN `PresentNum`  int NOT NULL AFTER `ProgenitusPos`;
ALTER TABLE `Player`
ADD COLUMN `VersionNumber`  int NOT NULL DEFAULT 0 AFTER `InDoorId`;

DROP TABLE IF EXISTS `EmployeeQuestTable`;
CREATE TABLE `EmployeeQuestTable` (
  `PlayerId` int(32) NOT NULL,
  `BinData` blob NOT NULL,
  PRIMARY KEY (`PlayerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;