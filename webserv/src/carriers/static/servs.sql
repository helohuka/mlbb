
SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for Order
-- ----------------------------
DROP TABLE IF EXISTS `Order`;
CREATE TABLE `Order` (
  `Id` int(32) NOT NULL AUTO_INCREMENT,
  `JsonValue` varchar(4096) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1784 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for Servs
-- ----------------------------
DROP TABLE IF EXISTS `Servs`;
CREATE TABLE `Servs` (
  `ServId` int(32) NOT NULL,
  `ServName` varchar(60) COLLATE utf8_bin NOT NULL,
  `ServAreaName` varchar(60) COLLATE utf8_bin NOT NULL,
  `ServArea` int(32) NOT NULL,
  `ServHost` varchar(60) COLLATE utf8_bin NOT NULL,
  `ServPort` int(32) NOT NULL,
  `OauthPort` int(32) NOT NULL,
  `PayPort` int(32) NOT NULL,
  `GMTPort` int(32) NOT NULL,
  `DBHost` varchar(60) COLLATE utf8_bin NOT NULL,
  `DBPort` int(32) NOT NULL,
  `DBUsr` varchar(60) COLLATE utf8_bin NOT NULL,
  `DBPwd` varchar(60) COLLATE utf8_bin NOT NULL,
  `DBGameName` varchar(60) COLLATE utf8_bin NOT NULL,
  `DBLogName` varchar(60) COLLATE utf8_bin NOT NULL,
  `Channels` varchar(128) COLLATE utf8_bin NOT NULL,
  `IsNewServ` int(11) NOT NULL,
  `Notice` varchar(2048) COLLATE utf8_bin NOT NULL,
  `CDN` varchar(128) COLLATE utf8_bin NOT NULL,
  `Version` varchar(128) COLLATE utf8_bin NOT NULL,
  `Sandbox` tinyint(4) NOT NULL,
  `Path` varchar(256) NOT NULL,
  `NoCheck` int(32) NOT NULL,
  PRIMARY KEY (`ServId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


CREATE TABLE `Channel` (
  `Channle` varchar(128) COLLATE utf8_bin NOT NULL,
  `Serverid` varchar(4096) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`Channle`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
