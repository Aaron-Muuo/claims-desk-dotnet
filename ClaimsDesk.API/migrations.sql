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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Customers` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FullName` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `PhoneNumber` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
        `NationalId` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
        `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Customers` PRIMARY KEY (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Organizations` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `LicenseNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Slug` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `CurrencyCode` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `Country` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Tin` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Address` longtext CHARACTER SET utf8mb4 NOT NULL,
        `AccountType` longtext CHARACTER SET utf8mb4 NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Organizations` PRIMARY KEY (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Permissions` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Permissions` PRIMARY KEY (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Roles` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Roles` PRIMARY KEY (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Branches` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Name` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
        `Code` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `IsHeadOffice` tinyint(1) NOT NULL,
        `Address` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Status` longtext CHARACTER SET utf8mb4 NOT NULL,
        `OrganizationId` int NOT NULL,
        CONSTRAINT `PK_Branches` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Branches_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Members` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `OrganizationId` int NOT NULL,
        `CustomerId` int NOT NULL,
        `MemberNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `Status` varchar(30) CHARACTER SET utf8mb4 NOT NULL,
        `JoinedDate` datetime(6) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Members` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Members_Customers_CustomerId` FOREIGN KEY (`CustomerId`) REFERENCES `Customers` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Members_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`Id`) ON DELETE RESTRICT
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Subscriptions` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `PlanRef` longtext CHARACTER SET utf8mb4 NULL,
        `PlanName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Status` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `OrganizationId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_Subscriptions` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Subscriptions_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `RolePermissions` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `RoleId` int NOT NULL,
        `PermissionId` int NOT NULL,
        `OrganizationId` int NOT NULL,
        CONSTRAINT `PK_RolePermissions` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_RolePermissions_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_RolePermissions_Permissions_PermissionId` FOREIGN KEY (`PermissionId`) REFERENCES `Permissions` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_RolePermissions_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `Users` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `FullName` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Username` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Email` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
        `PhoneNumber` longtext CHARACTER SET utf8mb4 NOT NULL,
        `PasswordHash` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Gender` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ReceiveNotifications` tinyint(1) NOT NULL,
        `ApprovalLimit` decimal(18,2) NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `RoleId` int NOT NULL,
        `BranchId` int NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Users_Branches_BranchId` FOREIGN KEY (`BranchId`) REFERENCES `Branches` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_Users_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `Roles` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `LicensePools` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `LicenseType` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `LicenseKey` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Seats` int NOT NULL,
        `Expires` tinyint(1) NOT NULL,
        `ExpiresAt` datetime(6) NULL,
        `SubscriptionId` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_LicensePools` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_LicensePools_Subscriptions_SubscriptionId` FOREIGN KEY (`SubscriptionId`) REFERENCES `Subscriptions` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `AuditTrails` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `Code` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `SourceIp` varchar(100) CHARACTER SET utf8mb4 NULL,
        `UserAgent` longtext CHARACTER SET utf8mb4 NULL,
        `RawDescription` longtext CHARACTER SET utf8mb4 NULL,
        `RequestRoute` longtext CHARACTER SET utf8mb4 NULL,
        `SourceController` varchar(150) CHARACTER SET utf8mb4 NULL,
        `SourceAction` varchar(150) CHARACTER SET utf8mb4 NULL,
        `Metadata` longtext CHARACTER SET utf8mb4 NULL,
        `BranchId` int NOT NULL,
        `UserId` int NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_AuditTrails` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AuditTrails_Branches_BranchId` FOREIGN KEY (`BranchId`) REFERENCES `Branches` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_AuditTrails_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE SET NULL
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE TABLE `UserLicenses` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `UserId` int NOT NULL,
        `LicensePoolId` int NOT NULL,
        `AssignedAt` datetime(6) NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_UserLicenses` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_UserLicenses_LicensePools_LicensePoolId` FOREIGN KEY (`LicensePoolId`) REFERENCES `LicensePools` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_UserLicenses_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_AuditTrails_BranchId` ON `AuditTrails` (`BranchId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_AuditTrails_UserId` ON `AuditTrails` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_Branches_OrganizationId` ON `Branches` (`OrganizationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Customers_Email` ON `Customers` (`Email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Customers_NationalId` ON `Customers` (`NationalId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_LicensePools_SubscriptionId` ON `LicensePools` (`SubscriptionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_Members_CustomerId` ON `Members` (`CustomerId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Members_OrganizationId_CustomerId` ON `Members` (`OrganizationId`, `CustomerId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Members_OrganizationId_MemberNumber` ON `Members` (`OrganizationId`, `MemberNumber`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Organizations_LicenseNumber` ON `Organizations` (`LicenseNumber`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Organizations_Slug` ON `Organizations` (`Slug`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_RolePermissions_OrganizationId` ON `RolePermissions` (`OrganizationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_RolePermissions_PermissionId` ON `RolePermissions` (`PermissionId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_RolePermissions_RoleId_PermissionId_OrganizationId` ON `RolePermissions` (`RoleId`, `PermissionId`, `OrganizationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_Subscriptions_OrganizationId` ON `Subscriptions` (`OrganizationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_UserLicenses_LicensePoolId` ON `UserLicenses` (`LicensePoolId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_UserLicenses_UserId` ON `UserLicenses` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE UNIQUE INDEX `IX_Users_BranchId_Email` ON `Users` (`BranchId`, `Email`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    CREATE INDEX `IX_Users_RoleId` ON `Users` (`RoleId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006180815_InitialCreate') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261006180815_InitialCreate', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE TABLE `AiPromptTemplates` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `OrganizationId` int NULL,
        `StageKey` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `ModelName` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `SystemPrompt` longtext CHARACTER SET utf8mb4 NOT NULL,
        `UserPromptTemplate` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Temperature` decimal(3,2) NOT NULL,
        `IsActive` tinyint(1) NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_AiPromptTemplates` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_AiPromptTemplates_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE TABLE `Claims` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `OrganizationId` int NOT NULL,
        `ClaimNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `MemberId` int NOT NULL,
        `PolicyNumber` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `IncidentDate` datetime(6) NOT NULL,
        `ReportedDate` datetime(6) NOT NULL,
        `IncidentType` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ClaimedAmount` decimal(18,2) NOT NULL,
        `ApprovedAmount` decimal(18,2) NULL,
        `CurrencyCode` varchar(10) CHARACTER SET utf8mb4 NOT NULL,
        `Status` int NOT NULL,
        `Channel` int NOT NULL,
        `CreatedByUserId` int NULL,
        `AssignedToUserId` int NULL,
        `ApprovedByUserId` int NULL,
        `RejectionReason` longtext CHARACTER SET utf8mb4 NULL,
        `CreatedAt` datetime(6) NOT NULL,
        `UpdatedAt` datetime(6) NULL,
        CONSTRAINT `PK_Claims` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Claims_Members_MemberId` FOREIGN KEY (`MemberId`) REFERENCES `Members` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Claims_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Claims_Users_ApprovedByUserId` FOREIGN KEY (`ApprovedByUserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Claims_Users_AssignedToUserId` FOREIGN KEY (`AssignedToUserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT,
        CONSTRAINT `FK_Claims_Users_CreatedByUserId` FOREIGN KEY (`CreatedByUserId`) REFERENCES `Users` (`Id`) ON DELETE RESTRICT
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE TABLE `ClaimAiAnalyses` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `ClaimId` int NOT NULL,
        `PromptTemplateId` int NULL,
        `AnalysisType` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `RiskScore` int NOT NULL,
        `RecommendedAction` varchar(50) CHARACTER SET utf8mb4 NOT NULL,
        `RecommendedReserveKSh` decimal(18,2) NULL,
        `Summary` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ExtractedDataJson` longtext CHARACTER SET utf8mb4 NOT NULL,
        `FlaggedAnomaliesJson` longtext CHARACTER SET utf8mb4 NOT NULL,
        `PromptTokens` int NOT NULL,
        `CompletionTokens` int NOT NULL,
        `ExecutionDurationMs` int NOT NULL,
        `CreatedAt` datetime(6) NOT NULL,
        CONSTRAINT `PK_ClaimAiAnalyses` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_ClaimAiAnalyses_AiPromptTemplates_PromptTemplateId` FOREIGN KEY (`PromptTemplateId`) REFERENCES `AiPromptTemplates` (`Id`) ON DELETE SET NULL,
        CONSTRAINT `FK_ClaimAiAnalyses_Claims_ClaimId` FOREIGN KEY (`ClaimId`) REFERENCES `Claims` (`Id`) ON DELETE CASCADE
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE TABLE `ClaimWorkflowLogs` (
        `Id` int NOT NULL AUTO_INCREMENT,
        `ClaimId` int NOT NULL,
        `PreviousStatus` int NOT NULL,
        `NewStatus` int NOT NULL,
        `PerformedByUserId` int NULL,
        `Notes` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
        `Timestamp` datetime(6) NOT NULL,
        CONSTRAINT `PK_ClaimWorkflowLogs` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_ClaimWorkflowLogs_Claims_ClaimId` FOREIGN KEY (`ClaimId`) REFERENCES `Claims` (`Id`) ON DELETE CASCADE,
        CONSTRAINT `FK_ClaimWorkflowLogs_Users_PerformedByUserId` FOREIGN KEY (`PerformedByUserId`) REFERENCES `Users` (`Id`)
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
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_AiPromptTemplates_OrganizationId_StageKey_IsActive` ON `AiPromptTemplates` (`OrganizationId`, `StageKey`, `IsActive`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_ClaimAiAnalyses_ClaimId` ON `ClaimAiAnalyses` (`ClaimId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_ClaimAiAnalyses_PromptTemplateId` ON `ClaimAiAnalyses` (`PromptTemplateId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_Claims_ApprovedByUserId` ON `Claims` (`ApprovedByUserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_Claims_AssignedToUserId` ON `Claims` (`AssignedToUserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_Claims_CreatedByUserId` ON `Claims` (`CreatedByUserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_Claims_MemberId` ON `Claims` (`MemberId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE UNIQUE INDEX `IX_Claims_OrganizationId_ClaimNumber` ON `Claims` (`OrganizationId`, `ClaimNumber`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_ClaimWorkflowLogs_ClaimId` ON `ClaimWorkflowLogs` (`ClaimId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    CREATE INDEX `IX_ClaimWorkflowLogs_PerformedByUserId` ON `ClaimWorkflowLogs` (`PerformedByUserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006214935_CreateClaimsAIModels') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261006214935_CreateClaimsAIModels', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006222149_AddAiCreditsTracking') THEN

    ALTER TABLE `Subscriptions` ADD `TopUpAiCreditsBalance` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006222149_AddAiCreditsTracking') THEN

    ALTER TABLE `LicensePools` ADD `FreeAiCreditsBalance` int NOT NULL DEFAULT 0;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20261006222149_AddAiCreditsTracking') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20261006222149_AddAiCreditsTracking', '9.0.0');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

