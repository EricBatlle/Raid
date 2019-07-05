CREATE TABLE IF NOT EXISTS `Day`
(
 `dayID`     int NOT NULL AUTO_INCREMENT ,
 `numDay`    int NOT NULL ,
 `numMonth`  int NOT NULL ,
 `numYear`   int NOT NULL ,
 `dayOfWeek` int NOT NULL ,

PRIMARY KEY (`DayID`)
);

CREATE TABLE IF NOT EXISTS `Raider`
(
 `raiderID`    int NOT NULL AUTO_INCREMENT ,
 `username`    varchar(45) NOT NULL ,
 `password`    varchar(45) NOT NULL ,
 `name`        varchar(45) NOT NULL ,
 `mainClass`   int NOT NULL ,
 `mainSpec`    int NOT NULL ,
 `offSpec`     int NOT NULL ,
 `daysRotated` int NOT NULL ,

PRIMARY KEY (`RaiderID`)
);

CREATE TABLE IF NOT EXISTS `Raider_day`
(
 `dayID`              int NOT NULL ,
 `raiderID`           int NOT NULL ,
 `isRaideable`        boolean NOT NULL ,
 `isRotative`         boolean NOT NULL ,
 `assistanceSelected` int NOT NULL ,
 `lateTime`           int NOT NULL ,

PRIMARY KEY (`dayID`, `raiderID`),
KEY `fkIdx_40` (`dayID`),
CONSTRAINT `FK_40` FOREIGN KEY `fkIdx_40` (`dayID`) REFERENCES `day` (`dayID`),
KEY `fkIdx_43` (`raiderID`),
CONSTRAINT `FK_43` FOREIGN KEY `fkIdx_43` (`raiderID`) REFERENCES `raider` (`raiderID`)
);

/*********************/

CREATE TABLE IF NOT EXISTS `Day`
(
 `dayID`     int NOT NULL AUTO_INCREMENT ,
 `numDay`    int NOT NULL ,
 `numMonth`  int NOT NULL ,
 `numYear`   int NOT NULL ,
 `dayOfWeek` int NOT NULL ,

PRIMARY KEY (`DayID`)
);
INSERT INTO Day (numDay, numMonth, numYear, dayOfWeek) VALUES (1,1,1,0);
INSERT INTO Day (numDay, numMonth, numYear, dayOfWeek) VALUES (2,1,1,1);

CREATE TABLE IF NOT EXISTS `Raider`
(
 `raiderID`    int NOT NULL AUTO_INCREMENT ,
 `username`    varchar(45) NOT NULL ,
 `password`    varchar(45) NOT NULL ,
 `name`        varchar(45) NOT NULL ,
 `mainClass`   int NOT NULL ,
 `mainSpec`    int NOT NULL ,
 `offSpec`     int NOT NULL ,
 `daysRotated` int NOT NULL ,

PRIMARY KEY (`RaiderID`)
);
INSERT INTO Raider (username, password, name, mainClass, mainSpec, offSpec, daysRotated) VALUES ("eric", "defaultPass", "Bronto", 1,2,0,20);
INSERT INTO Raider (username, password, name, mainClass, mainSpec, offSpec, daysRotated) VALUES ("joan", "defaultPass", "Burbuja", 0,0,0,2);

CREATE TABLE IF NOT EXISTS `Raider_day`
(
 `dayID`              int NOT NULL ,
 `raiderID`           int NOT NULL ,
 `isRaideable`        boolean NOT NULL ,
 `isRotative`         boolean NOT NULL ,
 `assistanceSelected` int NOT NULL ,
 `lateTime`           int NOT NULL ,

PRIMARY KEY (`dayID`, `raiderID`),
KEY `fkIdx_40` (`dayID`),
CONSTRAINT `FK_40` FOREIGN KEY `fkIdx_40` (`dayID`) REFERENCES `day` (`dayID`),
KEY `fkIdx_43` (`raiderID`),
CONSTRAINT `FK_43` FOREIGN KEY `fkIdx_43` (`raiderID`) REFERENCES `raider` (`raiderID`)
);

INSERT INTO Raider_day (dayID, raiderID)
SELECT dayID,raiderID FROM Day, Raider;

/*
CREATE DATABASE RaidDB;

CREATE TABLE IF NOT EXISTS `Day`
(
 `DayID`     int NOT NULL AUTO_INCREMENT ,
 `NumDay`    int NOT NULL ,
 `NumMonth`  int NOT NULL ,
 `NumYear`   int NOT NULL ,
 `DayOfWeek` int NOT NULL ,

PRIMARY KEY (`DayID`)
);


CREATE TABLE IF NOT EXISTS `accepted_raiders`
(
 `raiderID` int NOT NULL ,
 `dayID`    int NOT NULL ,

PRIMARY KEY (`raiderID`, `dayID`),
KEY `fkIdx_90` (`raiderID`),
CONSTRAINT `FK_90` FOREIGN KEY `fkIdx_90` (`raiderID`) REFERENCES `raider` (`raiderID`),
KEY `fkIdx_93` (`dayID`),
CONSTRAINT `FK_93` FOREIGN KEY `fkIdx_93` (`dayID`) REFERENCES `day` (`dayID`)
);


CREATE TABLE IF NOT EXISTS `declined_raiders`
(
 `dayID`    int NOT NULL ,
 `raiderID` int NOT NULL ,

PRIMARY KEY (`dayID`, `raiderID`),
KEY `fkIdx_102` (`dayID`),
CONSTRAINT `FK_102` FOREIGN KEY `fkIdx_102` (`dayID`) REFERENCES `day` (`dayID`),
KEY `fkIdx_105` (`raiderID`),
CONSTRAINT `FK_105` FOREIGN KEY `fkIdx_105` (`raiderID`) REFERENCES `raider` (`raiderID`)
);


CREATE TABLE IF NOT EXISTS `day_roster`
(
 `dayID`    int NOT NULL ,
 `raiderID` int NOT NULL ,

PRIMARY KEY (`DayID`, `RaiderID`),
KEY `fkIdx_108` (`DayID`),
CONSTRAINT `FK_108` FOREIGN KEY `fkIdx_108` (`DayID`) REFERENCES `Day` (`DayID`),
KEY `fkIdx_111` (`RaiderID`),
CONSTRAINT `FK_111` FOREIGN KEY `fkIdx_111` (`RaiderID`) REFERENCES `Raider` (`RaiderID`)
);


CREATE TABLE IF NOT EXISTS `Raider`
(
 `RaiderID`    int NOT NULL AUTO_INCREMENT ,
 `Username`    varchar(45) NOT NULL ,
 `Password`    varchar(45) NOT NULL ,
 `Name`        varchar(45) NOT NULL ,
 `MainClass`   int NOT NULL ,
 `MainSpec`    int NOT NULL ,
 `OffSpec`     int NOT NULL ,
 `DaysRotated` int NOT NULL ,

PRIMARY KEY (`RaiderID`)
);


CREATE TABLE IF NOT EXISTS `late_raiders`
(
 `dayID`    int NOT NULL ,
 `raiderID` int NOT NULL ,

PRIMARY KEY (`dayID`, `raiderID`),
KEY `fkIdx_96` (`dayID`),
CONSTRAINT `FK_96` FOREIGN KEY `fkIdx_96` (`dayID`) REFERENCES `day` (`dayID`),
KEY `fkIdx_99` (`RaiderID`),
CONSTRAINT `FK_99` FOREIGN KEY `fkIdx_99` (`raiderID`) REFERENCES `raider` (`raiderID`)
);


CREATE TABLE IF NOT EXISTS `Raider_day`
(
 `DayID`              int NOT NULL ,
 `RaiderID`           int NOT NULL ,
 `IsRaideable`        boolean NOT NULL ,
 `IsRotative`         boolean NOT NULL ,
 `AssistanceSelected` int NOT NULL ,
 `LateTime`           int NOT NULL ,

PRIMARY KEY (`DayID`, `RaiderID`),
KEY `fkIdx_40` (`DayID`),
CONSTRAINT `FK_40` FOREIGN KEY `fkIdx_40` (`DayID`) REFERENCES `Day` (`DayID`),
KEY `fkIdx_43` (`RaiderID`),
CONSTRAINT `FK_43` FOREIGN KEY `fkIdx_43` (`RaiderID`) REFERENCES `Raider` (`RaiderID`)
);
*/

/*********************************************************
CREATE TABLE IF NOT EXISTS `Day`
(
 `dayID`     int NOT NULL AUTO_INCREMENT ,
 `numDay`    int NOT NULL ,
 `numMonth`  int NOT NULL ,
 `numYear`   int NOT NULL ,
 `dayOfWeek` int NOT NULL ,

PRIMARY KEY (`DayID`)
);
INSERT INTO Day (NumDay, NumMonth, NumYear, DayOfWeek) VALUES (1,1,1,0);
INSERT INTO Day (NumDay, NumMonth, NumYear, DayOfWeek) VALUES (2,1,1,1);


CREATE TABLE IF NOT EXISTS `Raider`
(
 `daiderID`    int NOT NULL AUTO_INCREMENT ,
 `username`    varchar(45) NOT NULL ,
 `password`    varchar(45) NOT NULL ,
 `name`        varchar(45) NOT NULL ,
 `mainClass`   int NOT NULL ,
 `mainSpec`    int NOT NULL ,
 `offSpec`     int NOT NULL ,
 `daysRotated` int NOT NULL ,

PRIMARY KEY (`RaiderID`)
);
INSERT INTO Raider (Username, Password, Name, MainClass, MainSpec, OffSpec, DaysRotated) VALUES ("eric", "defaultPass", "Bronto", 1,2,0,20);
INSERT INTO Raider (Username, Password, Name, MainClass, MainSpec, OffSpec, DaysRotated) VALUES ("joan", "defaultPass", "Burbuja", 0,0,0,2);

CREATE TABLE IF NOT EXISTS `Raider_day`
(
 `dayID`              int NOT NULL ,
 `raiderID`           int NOT NULL ,
 `isRaideable`        boolean NOT NULL ,
 `isRotative`         boolean NOT NULL ,
 `assistanceSelected` int NOT NULL ,
 `lateTime`           int NOT NULL ,

PRIMARY KEY (`DayID`, `RaiderID`),
KEY `fkIdx_40` (`DayID`),
CONSTRAINT `FK_40` FOREIGN KEY `fkIdx_40` (`DayID`) REFERENCES `Day` (`DayID`),
KEY `fkIdx_43` (`RaiderID`),
CONSTRAINT `FK_43` FOREIGN KEY `fkIdx_43` (`RaiderID`) REFERENCES `Raider` (`RaiderID`)
);
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (1, 1, true, false, 0, 0 );
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (1, 2, true, false, 0, 0 );
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (2, 1, true, false, 0, 0 );
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (2, 2, true, false, 0, 0 );

*/

/*
CREATE TABLE IF NOT EXISTS `Day`
(
 `dayID`     int NOT NULL AUTO_INCREMENT ,
 `numDay`    int NOT NULL ,
 `numMonth`  int NOT NULL ,
 `numYear`   int NOT NULL ,
 `dayOfWeek` int NOT NULL ,

PRIMARY KEY (`DayID`)
);
INSERT INTO Day (numDay, numMonth, numYear, dayOfWeek) VALUES (1,1,1,0);
INSERT INTO Day (numDay, numMonth, numYear, dayOfWeek) VALUES (2,1,1,1);


CREATE TABLE IF NOT EXISTS `Raider`
(
 `raiderID`    int NOT NULL AUTO_INCREMENT ,
 `username`    varchar(45) NOT NULL ,
 `password`    varchar(45) NOT NULL ,
 `name`        varchar(45) NOT NULL ,
 `mainClass`   int NOT NULL ,
 `mainSpec`    int NOT NULL ,
 `offSpec`     int NOT NULL ,
 `daysRotated` int NOT NULL ,

PRIMARY KEY (`RaiderID`)
);
INSERT INTO Raider (username, password, name, mainClass, mainSpec, offSpec, daysRotated) VALUES ("eric", "defaultPass", "Bronto", 1,2,0,20);
INSERT INTO Raider (username, password, name, mainClass, mainSpec, offSpec, daysRotated) VALUES ("joan", "defaultPass", "Burbuja", 0,0,0,2);

CREATE TABLE IF NOT EXISTS `Raider_day`
(
 `dayID`              int NOT NULL ,
 `raiderID`           int NOT NULL ,
 `isRaideable`        boolean NOT NULL ,
 `isRotative`         boolean NOT NULL ,
 `assistanceSelected` int NOT NULL ,
 `lateTime`           int NOT NULL ,

PRIMARY KEY (`DayID`, `RaiderID`),
KEY `fkIdx_40` (`DayID`),
CONSTRAINT `FK_40` FOREIGN KEY `fkIdx_40` (`DayID`) REFERENCES `Day` (`DayID`),
KEY `fkIdx_43` (`RaiderID`),
CONSTRAINT `FK_43` FOREIGN KEY `fkIdx_43` (`RaiderID`) REFERENCES `Raider` (`RaiderID`)
);
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (1, 1, true, false, 0, 0 );
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (1, 2, true, false, 0, 0 );
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (2, 1, true, false, 0, 0 );
INSERT INTO Raider_day (DayID, RaiderID, IsRaideable, IsRotative, AssistanceSelected, LateTime)
    VALUES (2, 2, true, false, 0, 0 );
	*/