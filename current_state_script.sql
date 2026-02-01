CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE TABLE `BaseModel` (
        `Id` bigint NOT NULL AUTO_INCREMENT,
        `CreatedAt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
        `UpdatedAt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(),
        CONSTRAINT `PK_BaseModel` PRIMARY KEY (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE TABLE `user_models` (
        `Id` bigint NOT NULL,
        `Username` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
        `Nickname` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
        `Password` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
        `Avatar` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
        `Abstract` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
        `RegisterSrc` tinyint NOT NULL,
        `CodeAge` int NOT NULL,
        `Email` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
        `OpenId` varchar(126) CHARACTER SET utf8mb4 NOT NULL,
        `Role` tinyint NOT NULL,
        CONSTRAINT `PK_user_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_user_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE TABLE `collect_models` (
        `Id` bigint NOT NULL,
        `Title` varchar(32) CHARACTER SET utf8mb4 NULL,
        `Abstract` varchar(256) CHARACTER SET utf8mb4 NULL,
        `Cover` varchar(256) CHARACTER SET utf8mb4 NULL,
        `article_count` bigint NULL,
        `user_id` bigint NULL,
        `UserModelId` bigint NULL,
        CONSTRAINT `PK_collect_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_collect_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_collect_models_user_models_UserModelId` FOREIGN KEY (`UserModelId`) REFERENCES `user_models` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE TABLE `log_models` (
        `Id` bigint NOT NULL,
        `log_type` int NOT NULL,
        `Title` varchar(32) CHARACTER SET utf8mb4 NULL,
        `Content` longtext CHARACTER SET utf8mb4 NULL,
        `Level` int NULL,
        `user_id` bigint NULL,
        `Ip` varchar(32) CHARACTER SET utf8mb4 NULL,
        `Addr` varchar(64) CHARACTER SET utf8mb4 NULL,
        `is_read` tinyint(1) NULL,
        `login_status` tinyint(1) NULL,
        `user_name` varchar(32) CHARACTER SET utf8mb4 NULL,
        `Pwd` varchar(32) CHARACTER SET utf8mb4 NULL,
        `login_type` int NULL,
        `service_name` varchar(32) CHARACTER SET utf8mb4 NULL,
        `UserModelId` bigint NULL,
        CONSTRAINT `PK_log_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_log_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_log_models_user_models_UserModelId` FOREIGN KEY (`UserModelId`) REFERENCES `user_models` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE TABLE `user_conf_models` (
        `Id` bigint NOT NULL,
        `user_id` bigint NOT NULL,
        `UserModelId` bigint NOT NULL,
        `LikeTags` longtext CHARACTER SET utf8mb4 NOT NULL,
        `update_username_date` datetime(6) NULL,
        `publish_collections` tinyint unsigned NOT NULL,
        `publish_followings` tinyint unsigned NOT NULL,
        `publish_fans` tinyint unsigned NOT NULL,
        `theme_style_id` bigint NULL,
        CONSTRAINT `PK_user_conf_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_user_conf_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_user_conf_models_user_models_UserModelId` FOREIGN KEY (`UserModelId`) REFERENCES `user_models` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE INDEX `IX_collect_models_UserModelId` ON `collect_models` (`UserModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE INDEX `IX_log_models_UserModelId` ON `log_models` (`UserModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE INDEX `IX_user_conf_models_UserModelId` ON `user_conf_models` (`UserModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    CREATE UNIQUE INDEX `uni_user_conf_models_user_id` ON `user_conf_models` (`user_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201050635_InitialCreate') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260201050635_InitialCreate', '8.0.23');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `collect_models` DROP FOREIGN KEY `FK_collect_models_user_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `log_models` DROP FOREIGN KEY `FK_log_models_user_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `user_conf_models` DROP FOREIGN KEY `FK_user_conf_models_user_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `user_conf_models` DROP INDEX `IX_user_conf_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `log_models` DROP INDEX `IX_log_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `collect_models` DROP INDEX `IX_collect_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `user_conf_models` DROP COLUMN `UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `log_models` DROP COLUMN `UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    ALTER TABLE `collect_models` DROP COLUMN `UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    CREATE TABLE `article_digg_models` (
        `Id` bigint NOT NULL,
        `user_id` bigint NOT NULL,
        `article_id` bigint NOT NULL,
        CONSTRAINT `PK_article_digg_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_article_digg_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    CREATE TABLE `comment_models` (
        `Id` bigint NOT NULL,
        `Content` varchar(256) CHARACTER SET utf8mb4 NULL,
        `user_id` bigint NULL,
        `article_id` bigint NULL,
        `parent_id` bigint NULL,
        `root_parent_id` bigint NULL,
        CONSTRAINT `PK_comment_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_comment_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    CREATE TABLE `user_article_collect_models` (
        `Id` bigint NOT NULL,
        `user_id` bigint NOT NULL,
        `article_id` bigint NOT NULL,
        `collect_id` bigint NOT NULL,
        CONSTRAINT `PK_user_article_collect_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_user_article_collect_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    CREATE TABLE `user_article_look_history_models` (
        `Id` bigint NOT NULL,
        `user_id` bigint NULL,
        `article_id` bigint NULL,
        CONSTRAINT `PK_user_article_look_history_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_user_article_look_history_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051050_RemoveForeignKeyConstraints') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260201051050_RemoveForeignKeyConstraints', '8.0.23');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `user_article_look_history_models` ADD `ArticleModelId` bigint NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `user_article_collect_models` ADD `ArticleModelId` bigint NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `comment_models` ADD `ArticleModelId` bigint NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `article_digg_models` ADD `ArticleModelId` bigint NULL;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE TABLE `article_models` (
        `Id` bigint NOT NULL,
        `Title` longtext CHARACTER SET utf8mb4 NULL,
        `desc` longtext CHARACTER SET utf8mb4 NULL,
        `Content` longtext CHARACTER SET utf8mb4 NULL,
        `content_id` bigint unsigned NULL,
        `tag_list` longtext CHARACTER SET utf8mb4 NULL,
        `Cover` longtext CHARACTER SET utf8mb4 NULL,
        `user_id` bigint NULL,
        `look_count` bigint NULL,
        `like_count` bigint NULL,
        `comment_count` bigint NULL,
        `collect_count` bigint NULL,
        `enable_comment` tinyint(1) NULL,
        `Status` bigint NULL,
        `UserModelId` bigint NULL,
        CONSTRAINT `PK_article_models` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_article_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_article_models_user_models_UserModelId` FOREIGN KEY (`UserModelId`) REFERENCES `user_models` (`Id`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE INDEX `idx_users_username` ON `user_models` (`Username`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE INDEX `IX_user_article_look_history_models_ArticleModelId` ON `user_article_look_history_models` (`ArticleModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE INDEX `IX_user_article_collect_models_ArticleModelId` ON `user_article_collect_models` (`ArticleModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE INDEX `IX_comment_models_ArticleModelId` ON `comment_models` (`ArticleModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE INDEX `IX_article_digg_models_ArticleModelId` ON `article_digg_models` (`ArticleModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE INDEX `idx_articles_user_id` ON `article_models` (`user_id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    CREATE INDEX `IX_article_models_UserModelId` ON `article_models` (`UserModelId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `article_digg_models` ADD CONSTRAINT `FK_article_digg_models_article_models_ArticleModelId` FOREIGN KEY (`ArticleModelId`) REFERENCES `article_models` (`Id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `comment_models` ADD CONSTRAINT `FK_comment_models_article_models_ArticleModelId` FOREIGN KEY (`ArticleModelId`) REFERENCES `article_models` (`Id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `user_article_collect_models` ADD CONSTRAINT `FK_user_article_collect_models_article_models_ArticleModelId` FOREIGN KEY (`ArticleModelId`) REFERENCES `article_models` (`Id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    ALTER TABLE `user_article_look_history_models` ADD CONSTRAINT `FK_user_article_look_history_models_article_models_ArticleModel~` FOREIGN KEY (`ArticleModelId`) REFERENCES `article_models` (`Id`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051556_PerformanceOptimization') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260201051556_PerformanceOptimization', '8.0.23');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `article_digg_models` DROP FOREIGN KEY `FK_article_digg_models_article_models_ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `article_models` DROP FOREIGN KEY `FK_article_models_user_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `comment_models` DROP FOREIGN KEY `FK_comment_models_article_models_ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `user_article_collect_models` DROP FOREIGN KEY `FK_user_article_collect_models_article_models_ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `user_article_look_history_models` DROP FOREIGN KEY `FK_user_article_look_history_models_article_models_ArticleModel~`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `user_article_look_history_models` DROP INDEX `IX_user_article_look_history_models_ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `user_article_collect_models` DROP INDEX `IX_user_article_collect_models_ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `comment_models` DROP INDEX `IX_comment_models_ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `article_models` DROP INDEX `IX_article_models_UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `article_digg_models` DROP INDEX `IX_article_digg_models_ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `user_article_look_history_models` DROP COLUMN `ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `user_article_collect_models` DROP COLUMN `ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `comment_models` DROP COLUMN `ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `article_models` DROP COLUMN `UserModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    ALTER TABLE `article_digg_models` DROP COLUMN `ArticleModelId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20260201051729_FinalPerformanceOptimization') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20260201051729_FinalPerformanceOptimization', '8.0.23');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

