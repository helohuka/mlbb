/*
Navicat MySQL Data Transfer

Source Server         : 10.10.10.254_3306
Source Server Version : 50631
Source Host           : 10.10.10.254:3306
Source Database       : game-log

Target Server Type    : MYSQL
Target Server Version : 50631
File Encoding         : 65001

Date: 2016-10-17 16:26:09
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for account
-- ----------------------------
DROP TABLE IF EXISTS `account`;
CREATE TABLE `account` (
  `sortid` int(11) NOT NULL AUTO_INCREMENT,
  `game` varchar(50) COLLATE utf8_bin NOT NULL,
  `pfid` varchar(50) COLLATE utf8_bin NOT NULL,
  `pfname` varchar(50) COLLATE utf8_bin NOT NULL,
  `accountid` varchar(50) COLLATE utf8_bin NOT NULL,
  `createtime` datetime NOT NULL,
  `mac` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `idfa` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `ip` varchar(50) COLLATE utf8_bin NOT NULL,
  `devicetype` varchar(50) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`sortid`)
) ENGINE=InnoDB AUTO_INCREMENT=48 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for login
-- ----------------------------
DROP TABLE IF EXISTS `login`;
CREATE TABLE `login` (
  `sortid` int(11) NOT NULL AUTO_INCREMENT,
  `game` varchar(50) COLLATE utf8_bin NOT NULL,
  `pfid` varchar(50) COLLATE utf8_bin NOT NULL,
  `pfname` varchar(50) COLLATE utf8_bin NOT NULL,
  `accountid` varchar(50) COLLATE utf8_bin NOT NULL,
  `roleid` int(32) NOT NULL,
  `firstserid` int(32) NOT NULL,
  `serverid` int(32) NOT NULL,
  `logintime` datetime NOT NULL,
  `logouttime` datetime NOT NULL,
  `firsttime` datetime NOT NULL,
  `rolefirsttime` datetime NOT NULL,
  `mac` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `idfa` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `ip` varchar(50) COLLATE utf8_bin NOT NULL,
  `devicetype` varchar(50) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`sortid`)
) ENGINE=InnoDB AUTO_INCREMENT=220 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for order
-- ----------------------------
DROP TABLE IF EXISTS `order`;
CREATE TABLE `order` (
  `sortid` int(11) NOT NULL AUTO_INCREMENT,
  `game` varchar(50) COLLATE utf8_bin NOT NULL,
  `pfid` varchar(50) COLLATE utf8_bin NOT NULL,
  `pfname` varchar(50) COLLATE utf8_bin NOT NULL,
  `orderid` varchar(50) COLLATE utf8_bin NOT NULL,
  `accountid` varchar(50) COLLATE utf8_bin NOT NULL,
  `roleid` int(32) NOT NULL,
  `rolelv` int(32) NOT NULL,
  `payment` float NOT NULL,
  `paytime` varchar(50) COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`sortid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for PlayerLoginout
-- ----------------------------
DROP TABLE IF EXISTS `PlayerLoginout`;
CREATE TABLE `PlayerLoginout` (
  `sortid` int(11) NOT NULL AUTO_INCREMENT,
  `playerGuid` int(32) NOT NULL,
  `PlayerName` varchar(50) COLLATE utf8_bin NOT NULL,
  `UIBehaviorId` int(32) NOT NULL,
  `dTime` datetime NOT NULL,
  `Version` int(32) NOT NULL,
  PRIMARY KEY (`sortid`)
) ENGINE=InnoDB AUTO_INCREMENT=1446 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for PlayerSay
-- ----------------------------
DROP TABLE IF EXISTS `PlayerSay`;
CREATE TABLE `PlayerSay` (
  `SortId` int(11) NOT NULL AUTO_INCREMENT,
  `PlayerGuid` int(32) NOT NULL,
  `PlayerName` varchar(50) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `ChannelId` int(11) NOT NULL,
  `Content` varchar(128) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `Tiemstamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`SortId`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

-- ----------------------------
-- Table structure for PlayerTrack
-- ----------------------------
DROP TABLE IF EXISTS `PlayerTrack`;
CREATE TABLE `PlayerTrack` (
  `SortId` int(11) NOT NULL AUTO_INCREMENT,
  `Tiemstamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `PlayerName` varchar(50) COLLATE utf8_bin NOT NULL,
  `PlayerGuid` int(32) NOT NULL,
  `ItemId` int(32) NOT NULL,
  `ItemInstId` int(32) NOT NULL,
  `ItemStack` int(32) NOT NULL,
  `MLB` int(32) NOT NULL,
  `Diamond` int(32) NOT NULL,
  `Money` int(32) NOT NULL,
  `ExpP` int(32) NOT NULL,
  `FromP` int(32) NOT NULL,
  PRIMARY KEY (`SortId`)
) ENGINE=InnoDB AUTO_INCREMENT=15463 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role` (
  `game` varchar(50) COLLATE utf8_bin NOT NULL,
  `pfid` varchar(50) NOT NULL,
  `pfname` varchar(50) NOT NULL,
  `roleid` int(32) NOT NULL AUTO_INCREMENT,
  `cachedate` date NOT NULL,
  `accountid` varchar(50) NOT NULL,
  `serverid` int(32) NOT NULL,
  `servername` varchar(50) NOT NULL,
  `firstserid` int(32) NOT NULL,
  `rolefirstdate` date NOT NULL,
  `rolelastdate` date NOT NULL,
  `rolelv` smallint(6) NOT NULL,
  `gold` bigint(20) NOT NULL,
  `diamond` bigint(20) NOT NULL,
  `vip` smallint(6) NOT NULL,
  `ce` bigint(20) NOT NULL,
  PRIMARY KEY (`roleid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;
