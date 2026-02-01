CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `BaseModel` (
    `Id` bigint NOT NULL AUTO_INCREMENT,
    `CreatedAt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    `UpdatedAt` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(),
    CONSTRAINT `PK_BaseModel` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

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

CREATE INDEX `IX_collect_models_UserModelId` ON `collect_models` (`UserModelId`);

CREATE INDEX `IX_log_models_UserModelId` ON `log_models` (`UserModelId`);

CREATE INDEX `IX_user_conf_models_UserModelId` ON `user_conf_models` (`UserModelId`);

CREATE UNIQUE INDEX `uni_user_conf_models_user_id` ON `user_conf_models` (`user_id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260201050635_InitialCreate', '8.0.23');

COMMIT;

START TRANSACTION;

ALTER TABLE `collect_models` DROP FOREIGN KEY `FK_collect_models_user_models_UserModelId`;

ALTER TABLE `log_models` DROP FOREIGN KEY `FK_log_models_user_models_UserModelId`;

ALTER TABLE `user_conf_models` DROP FOREIGN KEY `FK_user_conf_models_user_models_UserModelId`;

ALTER TABLE `user_conf_models` DROP INDEX `IX_user_conf_models_UserModelId`;

ALTER TABLE `log_models` DROP INDEX `IX_log_models_UserModelId`;

ALTER TABLE `collect_models` DROP INDEX `IX_collect_models_UserModelId`;

ALTER TABLE `user_conf_models` DROP COLUMN `UserModelId`;

ALTER TABLE `log_models` DROP COLUMN `UserModelId`;

ALTER TABLE `collect_models` DROP COLUMN `UserModelId`;

CREATE TABLE `article_digg_models` (
    `Id` bigint NOT NULL,
    `user_id` bigint NOT NULL,
    `article_id` bigint NOT NULL,
    CONSTRAINT `PK_article_digg_models` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_article_digg_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

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

CREATE TABLE `user_article_collect_models` (
    `Id` bigint NOT NULL,
    `user_id` bigint NOT NULL,
    `article_id` bigint NOT NULL,
    `collect_id` bigint NOT NULL,
    CONSTRAINT `PK_user_article_collect_models` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_user_article_collect_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `user_article_look_history_models` (
    `Id` bigint NOT NULL,
    `user_id` bigint NULL,
    `article_id` bigint NULL,
    CONSTRAINT `PK_user_article_look_history_models` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_user_article_look_history_models_BaseModel_Id` FOREIGN KEY (`Id`) REFERENCES `BaseModel` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260201051050_RemoveForeignKeyConstraints', '8.0.23');

COMMIT;

