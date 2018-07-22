/*
SQLyog Ultimate v11.24 (64 bit)
MySQL - 5.6.12-log : Database - history
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`history` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `history`;

/*Table structure for table `activity` */

DROP TABLE IF EXISTS `activity`;

CREATE TABLE `activity` (
  `aid` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `name` varchar(50) NOT NULL,
  `OpenTime` datetime NOT NULL,
  `CloseTime` datetime NOT NULL,
  PRIMARY KEY (`aid`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8;


/*Table structure for table `email` */

DROP TABLE IF EXISTS `email`;

CREATE TABLE `email` (
  `emailid` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(10) NOT NULL,
  `sender` varchar(10) NOT NULL,
  `content` varchar(4049) NOT NULL,
  `recvers` varchar(10) NOT NULL,
  `item` varchar(30) NOT NULL,
  `stack` int(11) NOT NULL,
  `low_level` int(11) NOT NULL,
  `high_level` int(11) NOT NULL,
  `regist_low_time` datetime NOT NULL,
  `regist_high_time` datetime NOT NULL,
  `types` int(11) NOT NULL,
  `timmer` datetime NOT NULL,
  PRIMARY KEY (`emailid`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8;

/*Data for the table `email` */


/*Table structure for table `notice` */

DROP TABLE IF EXISTS `notice`;

CREATE TABLE `notice` (
  `guid` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `types` smallint(11) NOT NULL DEFAULT '0',
  `sendType` varchar(64) NOT NULL,
  `title` varchar(1024) NOT NULL,
  `content` varchar(4096) NOT NULL,
  `contentColor` varchar(64) NOT NULL,
  `time` datetime NOT NULL,
  PRIMARY KEY (`guid`)
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8;

/*Data for the table `notice` */


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
