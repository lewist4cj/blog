-- Active: 1766890084394@@127.0.0.1@3306@blog4cj
-- MySQL dump 10.13  Distrib 8.4.7, for Linux (aarch64)
--
-- Host: 127.0.0.1    Database: blog4g
-- ------------------------------------------------------
-- Server version	8.4.7-0ubuntu0.25.10.3

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */
;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */
;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */
;
/*!50503 SET NAMES utf8mb4 */
;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */
;
/*!40103 SET TIME_ZONE='+00:00' */
;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */
;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */
;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */
;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */
;

--
-- Table structure for table `article_digg_models`
--

DROP TABLE IF EXISTS `article_digg_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `article_digg_models` (
                                       `user_id` bigint unsigned DEFAULT NULL,
                                       `article_id` bigint unsigned DEFAULT NULL,
                                       `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                       UNIQUE KEY `idx_name` (`user_id`, `article_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `article_digg_models`
--

/*!40000 ALTER TABLE `article_digg_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `article_digg_models` ENABLE KEYS */
;

--
-- Table structure for table `article_models`
--

DROP TABLE IF EXISTS `article_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `article_models` (
                                  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                                  `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                  `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                                  `title` longtext,
                                  `desc` longtext,
                                  `content` longtext,
                                  `content_id` bigint unsigned DEFAULT NULL,
                                  `tag_list` longtext,
                                  `cover` longtext,
                                  `user_id` bigint unsigned DEFAULT NULL,
                                  `look_count` bigint DEFAULT NULL,
                                  `like_count` bigint DEFAULT NULL,
                                  `comment_count` bigint DEFAULT NULL,
                                  `collect_count` bigint DEFAULT NULL,
                                  `enable_comment` tinyint(1) DEFAULT NULL,
                                  `status` bigint DEFAULT NULL,
                                  PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `article_models`
--

/*!40000 ALTER TABLE `article_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `article_models` ENABLE KEYS */
;

--
-- Table structure for table `banner_models`
--

DROP TABLE IF EXISTS `banner_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `banner_models` (
                                 `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                                 `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                 `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                                 `cover` longtext,
                                 `href` longtext,
                                 PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `banner_models`
--

/*!40000 ALTER TABLE `banner_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `banner_models` ENABLE KEYS */
;

--
-- Table structure for table `category_models`
--

DROP TABLE IF EXISTS `category_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `category_models` (
                                   `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                                   `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                   `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                                   `title` varchar(32) DEFAULT NULL,
                                   `user_id` bigint unsigned DEFAULT NULL,
                                   PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `category_models`
--

/*!40000 ALTER TABLE `category_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `category_models` ENABLE KEYS */
;

--
-- Table structure for table `collect_models`
--

DROP TABLE IF EXISTS `collect_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `collect_models` (
                                  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                                  `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                  `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                                  `title` varchar(32) DEFAULT NULL,
                                  `abstract` varchar(256) DEFAULT NULL,
                                  `cover` varchar(256) DEFAULT NULL,
                                  `article_count` bigint DEFAULT NULL,
                                  `user_id` bigint unsigned DEFAULT NULL,
                                  PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `collect_models`
--

/*!40000 ALTER TABLE `collect_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `collect_models` ENABLE KEYS */
;

--
-- Table structure for table `comment_models`
--

DROP TABLE IF EXISTS `comment_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `comment_models` (
                                  `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                                  `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                  `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                                  `content` varchar(256) DEFAULT NULL,
                                  `user_id` bigint unsigned DEFAULT NULL,
                                  `article_id` bigint unsigned DEFAULT NULL,
                                  `parent_id` bigint unsigned DEFAULT NULL,
                                  `root_parent_id` bigint unsigned DEFAULT NULL,
                                  `digg_count` bigint DEFAULT NULL,
                                  PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `comment_models`
--

/*!40000 ALTER TABLE `comment_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `comment_models` ENABLE KEYS */
;

--
-- Table structure for table `global_notications`
--

DROP TABLE IF EXISTS `global_notications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `global_notications` (
                                      `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                                      `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                      `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                                      `title` varchar(32) DEFAULT NULL,
                                      `icon` varchar(256) DEFAULT NULL,
                                      `content` varchar(64) DEFAULT NULL,
                                      `href` varchar(256) DEFAULT NULL,
                                      PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `global_notications`
--

/*!40000 ALTER TABLE `global_notications` DISABLE KEYS */
;
/*!40000 ALTER TABLE `global_notications` ENABLE KEYS */
;

--
-- Table structure for table `log_models`
--

DROP TABLE IF EXISTS `log_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `log_models` (
                              `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                              `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                              `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                              `log_type` tinyint DEFAULT NULL,
                              `title` varchar(32) DEFAULT NULL,
                              `content` longtext,
                              `level` tinyint DEFAULT NULL,
                              `user_id` bigint unsigned DEFAULT NULL,
                              `ip` varchar(32) DEFAULT NULL,
                              `addr` varchar(64) DEFAULT NULL,
                              `is_read` tinyint(1) DEFAULT NULL,
                              `login_status` tinyint(1) DEFAULT NULL,
                              `user_name` varchar(32) DEFAULT NULL,
                              `pwd` varchar(32) DEFAULT NULL,
                              `login_type` tinyint DEFAULT NULL,
                              `service_name` varchar(32) DEFAULT NULL,
                              PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `log_models`
--

/*!40000 ALTER TABLE `log_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `log_models` ENABLE KEYS */
;

--
-- Table structure for table `user_article_collect_models`
--

DROP TABLE IF EXISTS `user_article_collect_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `user_article_collect_models` (
                                               `user_id` bigint unsigned DEFAULT NULL,
                                               `article_id` bigint unsigned DEFAULT NULL,
                                               `collect_id` bigint unsigned DEFAULT NULL,
                                               `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                               UNIQUE KEY `idx_name` (
                                                   `user_id`,
                                                   `article_id`,
                                                   `collect_id`
                                                   )
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `user_article_collect_models`
--

/*!40000 ALTER TABLE `user_article_collect_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `user_article_collect_models` ENABLE KEYS */
;

--
-- Table structure for table `user_article_look_history_models`
--

DROP TABLE IF EXISTS `user_article_look_history_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `user_article_look_history_models` (
                                                    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                                                    `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                                                    `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                                                    `user_id` bigint unsigned DEFAULT NULL,
                                                    `article_id` bigint unsigned DEFAULT NULL,
                                                    PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `user_article_look_history_models`
--

/*!40000 ALTER TABLE `user_article_look_history_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `user_article_look_history_models` ENABLE KEYS */
;

--
-- Table structure for table `user_conf_models`
--

DROP TABLE IF EXISTS `user_conf_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `user_conf_models` (
                                    `user_id` bigint unsigned DEFAULT NULL,
                                    `like_tags` longtext,
                                    `update_username_date` datetime(3) DEFAULT NULL,
                                    `publish_collections` tinyint(1) DEFAULT NULL,
                                    `publish_followings` tinyint(1) DEFAULT NULL,
                                    `publish_fans` tinyint(1) DEFAULT NULL,
                                    `theme_style_id` bigint unsigned DEFAULT NULL,
                                    UNIQUE KEY `uni_user_conf_models_user_id` (`user_id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `user_conf_models`
--

/*!40000 ALTER TABLE `user_conf_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `user_conf_models` ENABLE KEYS */
;

--
-- Table structure for table `user_models`
--

DROP TABLE IF EXISTS `user_models`;
/*!40101 SET @saved_cs_client     = @@character_set_client */
;
/*!50503 SET character_set_client = utf8mb4 */
;
CREATE TABLE `user_models` (
                               `id` bigint unsigned NOT NULL AUTO_INCREMENT,
                               `created_at` timestamp default CURRENT_TIMESTAMP NOT NULL,
                               `updated_at` timestamp default CURRENT_TIMESTAMP not null on update CURRENT_TIMESTAMP NOT NULL,
                               `username` varchar(32) DEFAULT NULL,
                               `nickname` varchar(32) DEFAULT NULL,
                               `password` varchar(64) DEFAULT NULL,
                               `avatar` varchar(256) DEFAULT NULL,
                               `abstract` varchar(256) DEFAULT NULL,
                               `register_src` tinyint DEFAULT NULL,
                               `code_age` bigint DEFAULT NULL,
                               `email` varchar(256) DEFAULT NULL,
                               `open_id` varchar(126) DEFAULT NULL,
                               `role` tinyint DEFAULT NULL,
                               PRIMARY KEY (`id`)
) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */
;

--
-- Dumping data for table `user_models`
--

/*!40000 ALTER TABLE `user_models` DISABLE KEYS */
;
/*!40000 ALTER TABLE `user_models` ENABLE KEYS */
;

--
-- Dumping routines for database 'blog4g'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */
;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */
;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */
;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */
;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */
;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */
;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */
;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */
;

-- Dump completed on 2026-01-09 12:54:41