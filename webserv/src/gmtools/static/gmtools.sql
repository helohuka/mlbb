-- phpMyAdmin SQL Dump
-- version 4.5.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: 2017-03-23 10:10:54
-- 服务器版本： 10.1.13-MariaDB
-- PHP Version: 5.6.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `gmtools`
--

-- --------------------------------------------------------

--
-- 表的结构 `accumulate`
--

CREATE TABLE `accumulate` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `discount`
--

CREATE TABLE `discount` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `extract`
--

CREATE TABLE `extract` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `gmtoos`
--

CREATE TABLE `gmtoos` (
  `id` int(11) NOT NULL,
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `password` varchar(100) COLLATE utf8_bin NOT NULL,
  `level` int(11) NOT NULL,
  `admin` varchar(100) COLLATE utf8_bin NOT NULL,
  `tel` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `hotspot`
--

CREATE TABLE `hotspot` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `integral`
--

CREATE TABLE `integral` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `login`
--

CREATE TABLE `login` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `mail`
--

CREATE TABLE `mail` (
  `title` varchar(100) COLLATE utf8_bin NOT NULL,
  `sender` varchar(100) COLLATE utf8_bin NOT NULL,
  `content` varchar(100) COLLATE utf8_bin NOT NULL,
  `recvers` varchar(100) COLLATE utf8_bin NOT NULL,
  `stritemids` varchar(100) COLLATE utf8_bin NOT NULL,
  `stritemsks` varchar(100) COLLATE utf8_bin NOT NULL,
  `lowlevel` int(11) NOT NULL,
  `highlevel` int(11) NOT NULL,
  `time0` varchar(100) COLLATE utf8_bin NOT NULL,
  `time1` varchar(100) COLLATE utf8_bin NOT NULL,
  `sendtype` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `money`
--

CREATE TABLE `money` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `notice`
--

CREATE TABLE `notice` (
  `types` varchar(100) COLLATE utf8_bin NOT NULL,
  `title` varchar(100) COLLATE utf8_bin NOT NULL,
  `content` varchar(300) COLLATE utf8_bin NOT NULL,
  `color` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `record`
--

CREATE TABLE `record` (
  `name` varchar(1000) COLLATE utf8_bin NOT NULL,
  `shopid` int(100) NOT NULL,
  `orderid` varchar(1000) COLLATE utf8_bin NOT NULL,
  `payment` int(100) NOT NULL,
  `roleid` int(100) NOT NULL,
  `time` varchar(1000) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `roll`
--

CREATE TABLE `roll` (
  `types` varchar(100) COLLATE utf8_bin NOT NULL,
  `sendtype` varchar(100) COLLATE utf8_bin NOT NULL,
  `content` varchar(10) COLLATE utf8_bin NOT NULL,
  `timestr` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `service`
--

CREATE TABLE `service` (
  `types` varchar(100) COLLATE utf8_bin NOT NULL,
  `title` varchar(100) COLLATE utf8_bin NOT NULL,
  `content` varchar(100) COLLATE utf8_bin NOT NULL,
  `color` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `single`
--

CREATE TABLE `single` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- --------------------------------------------------------

--
-- 表的结构 `turntable`
--

CREATE TABLE `turntable` (
  `name` varchar(100) COLLATE utf8_bin NOT NULL,
  `opentime` varchar(100) COLLATE utf8_bin NOT NULL,
  `closetime` varchar(100) COLLATE utf8_bin NOT NULL,
  `time` varchar(100) COLLATE utf8_bin NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `gmtoos`
--
ALTER TABLE `gmtoos`
  ADD PRIMARY KEY (`id`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `gmtoos`
--
ALTER TABLE `gmtoos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
