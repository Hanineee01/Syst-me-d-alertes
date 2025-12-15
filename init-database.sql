-- ============================================
-- Script de création de la base de données
-- T-Tron Alert System
-- Base de données: t_tron_alert
-- SGBD: MySQL / MariaDB
-- ============================================

-- Créer la base de données si elle n'existe pas
CREATE DATABASE IF NOT EXISTS `t_tron_alert`
DEFAULT CHARACTER SET utf8mb4
DEFAULT COLLATE utf8mb4_general_ci;

-- Utiliser la base de données
USE `t_tron_alert`;

-- ============================================
-- Table: Alerts
-- Description: Stocke les alertes du système
-- ============================================

CREATE TABLE IF NOT EXISTS `Alerts` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `Title` VARCHAR(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `Message` VARCHAR(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
    `Level` INT NOT NULL COMMENT '0=Info, 1=Warning, 2=Critical',
    `CreatedAt` DATETIME(6) NOT NULL,
    `IsRead` TINYINT(1) NOT NULL DEFAULT 0,
    `IsArchived` TINYINT(1) NOT NULL DEFAULT 0,
    `TargetWorkstationId` LONGTEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,

    PRIMARY KEY (`Id`),
    INDEX `IX_Alerts_CreatedAt` (`CreatedAt`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ============================================
-- Données de test (optionnel)
-- ============================================

-- Décommentez les lignes suivantes pour ajouter des données de test:

/*
INSERT INTO `Alerts` (`Title`, `Message`, `Level`, `CreatedAt`, `IsRead`, `IsArchived`, `TargetWorkstationId`)
VALUES
    ('Alerte de test Info', 'Ceci est une alerte de niveau Info', 0, NOW(6), 0, 0, 'poste-1'),
    ('Alerte de test Warning', 'Ceci est une alerte de niveau Warning', 1, NOW(6), 0, 0, 'poste-1'),
    ('Alerte de test Critical', 'Ceci est une alerte de niveau Critical', 2, NOW(6), 0, 0, 'poste-2');
*/

-- ============================================
-- Fin du script
-- ============================================
