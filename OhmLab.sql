
-- =======================================
-- SCRIPT TẠO CƠ SỞ DỮ LIỆU PHÒNG LAB
-- =======================================
USE [master]
GO

IF DB_ID('OhmLab_DB') IS NOT NULL
BEGIN
    --DROP DATABASE [OhmLab_DB]
	ALTER DATABASE [OhmLab_DB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE [OhmLab_DB];
END
GO
CREATE DATABASE [OhmLab_DB]
GO

USE [OhmLab_DB]
GO
-- ========================
-- 1. Bảng KitTemplate
-- ========================
CREATE TABLE KitTemplate (
    KitTemplate_id NVARCHAR(50) NOT NULL,
    KitTemplate_Name NVARCHAR(50) NOT NULL,
    KitTemplate_Quantity INT NOT NULL,
    KitTemplate_Description NVARCHAR(MAX) NULL,
	KitTemplate_Url_Img NVARCHAR(MAX) NULL,
    KitTemplate_Status NVARCHAR(50) NOT NULL
PRIMARY KEY CLUSTERED ([KitTemplate_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- ========================
-- 2. Bảng Accessory
-- ========================
CREATE TABLE Accessory (
    Accessory_id INT NOT NULL IDENTITY (1,1),
    Accessory_Name NVARCHAR(50) NOT NULL,
    Accessory_Description NVARCHAR(MAX) NULL,
	Accessory_Url_Img NVARCHAR(MAX) NULL,
    Accessory_CreateDate DATE NOT NULL,
    Accessory_ValueCode NVARCHAR(50) NOT NULL,	
    Accessory_Case NVARCHAR(50) NOT NULL,
    Accessory_Status NVARCHAR(50) NOT NULL,
PRIMARY KEY CLUSTERED ([Accessory_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- ========================
-- 3. Bảng Quan hệ giữa Accessory và KitTemplate
-- ========================
CREATE TABLE Accessory_KitTemplate (
    Accessory_KitTemplate_id INT NOT NULL IDENTITY (1,1),
    KitTemplate_id NVARCHAR(50) NOT NULL,
    Accessory_id INT NOT NULL,
	Accessory_Quantity INT NOT NULL,
    Accessory_KitTemplate_Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (KitTemplate_id) REFERENCES KitTemplate(KitTemplate_id),
	FOREIGN KEY (Accessory_id) REFERENCES Accessory(Accessory_id),
PRIMARY KEY CLUSTERED ([Accessory_KitTemplate_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 4. Bảng Kit
-- ========================
CREATE TABLE Kit (
    Kit_id NVARCHAR(50) NOT NULL,
	KitTemplate_id NVARCHAR(50) NOT NULL,
    Kit_Name NVARCHAR(50) NOT NULL,
    Kit_Description NVARCHAR(MAX) NULL,
	Kit_Url_Img NVARCHAR(MAX) NULL,
	Kit_Url_QR NVARCHAR(MAX) NOT NULL,
    Kit_CreateDate DATE NOT NULL,
    Kit_Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (KitTemplate_id) REFERENCES KitTemplate(KitTemplate_id),
PRIMARY KEY CLUSTERED ([Kit_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 5. Bảng Quan hệ giữa Kit và Accessory
-- ========================
CREATE TABLE Kit_Accessory (
	Kit_Accessory_id INT NOT NULL IDENTITY (1,1),
    Kit_id NVARCHAR(50) NOT NULL,
	Accessory_id INT NOT NULL,
    Accessory_Quantity INT NOT NULL,
    Kit_Accessory_Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (Kit_id) REFERENCES Kit(Kit_id),
	FOREIGN KEY (Accessory_id) REFERENCES Accessory(Accessory_id),
PRIMARY KEY CLUSTERED ([Kit_Accessory_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 6. Bảng User
-- ========================
CREATE TABLE [User] (
    [User_id] UNIQUEIDENTIFIER NOT NULL,
    User_FullName NVARCHAR(50) NOT NULL,
    User_RollNumber NVARCHAR(50) NOT NULL,
    User_Email NVARCHAR(100) NOT NULL,
	User_RoleName NVARCHAR(50) NOT NULL,
    User_NumberCode NVARCHAR(50) NOT NULL,
    [Status] NVARCHAR(50) NOT NULL
PRIMARY KEY CLUSTERED ([User_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO




-- ========================
-- 7. Bảng Semester
-- ========================
CREATE TABLE Semester (
    Semester_id INT NOT NULL IDENTITY (1,1),
    Semester_Name NVARCHAR(50) NOT NULL,
    Semester_StartDate DATE NOT NULL,
    Semester_EndDate DATE NOT NULL,
    Semester_Description NVARCHAR(MAX) NULL,
    Semester_Status NVARCHAR(50) NOT NULL,
PRIMARY KEY CLUSTERED ([Semester_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- ========================
-- 8. Bảng Subject
-- ========================
CREATE TABLE [Subject] (
    Subject_id INT NOT NULL IDENTITY (1,1),
    Subject_Name NVARCHAR(100) NOT NULL,
	Subject_Code NVARCHAR(50) NOT NULL,
    Subject_Description NVARCHAR(MAX) NULL,
    Subject_Status NVARCHAR(50) NOT NULL,
PRIMARY KEY CLUSTERED ([Subject_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO
	

-- ========================
-- 9. Bảng Quan hệ giữa Semester và Subject
-- ========================
CREATE TABLE [Semester_Subject] (
    Semester_Subject_id INT NOT NULL IDENTITY (1,1),
    Subject_id INT NOT NULL,
	Semester_id INT NOT NULL,
    Semester_Subject NVARCHAR(50) NOT NULL,
	FOREIGN KEY (Subject_id) REFERENCES [Subject](Subject_id),
	FOREIGN KEY (Semester_id) REFERENCES Semester(Semester_id),
PRIMARY KEY CLUSTERED ([Semester_Subject_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO




-- ========================
-- 10. Bảng EquipmentType
-- ========================
CREATE TABLE EquipmentType (
    EquipmentType_id NVARCHAR(50) NOT NULL,
    EquipmentType_Name NVARCHAR(100) NOT NULL,
	EquipmentType_Code NVARCHAR(50) NOT NULL,
    EquipmentType_Description NVARCHAR(MAX) NULL,
    EquipmentType_Quantity INT NOT NULL,
    EquipmentType_Url_Img NVARCHAR(MAX) NULL,
    EquipmentType_CreateDate DATETIME NOT NULL,
    EquipmentType_Status NVARCHAR(50) NOT NULL
PRIMARY KEY CLUSTERED ([EquipmentType_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 11. Bảng Equipment
-- ========================
CREATE TABLE Equipment (
    Equipment_id NVARCHAR(50) NOT NULL,
	EquipmentType_id NVARCHAR(50) NOT NULL,
    Equipment_Name NVARCHAR(50) NOT NULL,
	Equipment_Code NVARCHAR(50) NOT NULL,
    Equipment_NumberSerial NVARCHAR(50) NOT NULL,
    Equipment_Description NVARCHAR(MAX) NULL,
	EquipmentType_Url_Img NVARCHAR(MAX) NULL,
    Equipment_QR NVARCHAR(MAX) NOT NULL,
    Equipment_Status NVARCHAR(50) NOT NULL
	FOREIGN KEY (EquipmentType_id) REFERENCES [EquipmentType](EquipmentType_id),
PRIMARY KEY CLUSTERED ([Equipment_id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- ========================
-- 12. Bảng Lab
-- ========================
CREATE TABLE Lab (
    Lab_id INT NOT NULL IDENTITY (1,1),
	Subject_id INT NOT NULL,
    Lab_Name NVARCHAR(50) NOT NULL,
    Lab_Request NVARCHAR(MAX) NOT NULL,
    Lab_Target NVARCHAR(MAX) NOT NULL,
    Lab_Status NVARCHAR(50) NOT NULL
	FOREIGN KEY (Subject_id) REFERENCES [Subject](Subject_id),
PRIMARY KEY CLUSTERED ([Lab_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 13. Bảng Quan hệ giữa Lab và KitTemplate
-- ========================
CREATE TABLE Lab_KitTemplate (
    Lab_KitTemplate_id INT NOT NULL IDENTITY (1,1),
	KitTemplate_id NVARCHAR(50) NOT NULL,
    Lab_KitTemplate_Status NVARCHAR(50) NOT NULL
	FOREIGN KEY (KitTemplate_id) REFERENCES [KitTemplate](KitTemplate_id),
PRIMARY KEY CLUSTERED ([Lab_KitTemplate_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 13. Bảng Quan hệ giữa Lab và EquipmentType
-- ========================
CREATE TABLE Lab_EquipmentType (
    Lab_EquipmentType_id INT NOT NULL IDENTITY (1,1),
	EquipmentType_id NVARCHAR(50) NOT NULL,
    Lab_EquipmentType_Status NVARCHAR(50) NOT NULL
	FOREIGN KEY (EquipmentType_id) REFERENCES [EquipmentType](EquipmentType_id),
PRIMARY KEY CLUSTERED ([Lab_EquipmentType_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO




-- ========================
-- 14. Bảng Slot
-- ========================
CREATE TABLE Slot (
    Slot_id INT NOT NULL IDENTITY (1,1),
    Slot_Name NVARCHAR(50) NOT NULL,
    Slot_StartTime NVARCHAR(50) NOT NULL,
    Slot_EndTime NVARCHAR(50) NOT NULL,
    Slot_Description NVARCHAR(MAX) NULL,
    Slot_Status NVARCHAR(50) NOT NULL
PRIMARY KEY CLUSTERED ([Slot_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 15. Bảng ScheduleType
-- ========================
CREATE TABLE ScheduleType (
    ScheduleType_id INT NOT NULL IDENTITY (1,1),
	Slot_id INT NOT NULL,
    ScheduleType_Name NVARCHAR(50) NOT NULL,
    ScheduleType_Description NVARCHAR(MAX) NULL,
    ScheduleType_DOW NVARCHAR(50) NOT NULL,
    ScheduleType_Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (Slot_id) REFERENCES [Slot](Slot_id),
PRIMARY KEY CLUSTERED ([ScheduleType_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 17. Bảng Class
-- ========================
CREATE TABLE Class (
    Class_id INT NOT NULL IDENTITY (1,1),
	Subject_id INT NOT NULL,
	Lecturer_id UNIQUEIDENTIFIER NULL,
	ScheduleType_id INT NULL,
    Class_Name NVARCHAR(50) NOT NULL,
	Class_Description NVARCHAR(MAX) NULL,
	Class_Status NVARCHAR(50) NOT NULL,
    FOREIGN KEY (Subject_id) REFERENCES [Subject](Subject_id),
	FOREIGN KEY (Lecturer_id) REFERENCES [User]([User_id]),
	FOREIGN KEY (ScheduleType_id) REFERENCES [ScheduleType](ScheduleType_id),
PRIMARY KEY CLUSTERED ([Class_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 18. Bảng Quan hệ giữa Class và User
-- ========================
CREATE TABLE Class_User (
    Class_User_id INT NOT NULL IDENTITY (1,1),
	Class_id INT NOT NULL,
	[User_id] UNIQUEIDENTIFIER NOT NULL,
	Class_User_Description NVARCHAR(MAX) NULL,
	Class_User_Status NVARCHAR(50) NOT NULL,
    FOREIGN KEY (Class_id) REFERENCES [Class](Class_id),
	FOREIGN KEY ([User_id]) REFERENCES [User]([User_id]),
PRIMARY KEY CLUSTERED ([Class_User_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 19. Bảng Schedule
-- ========================
CREATE TABLE Schedule (
    Schedule_id INT NOT NULL IDENTITY (1,1),
	Class_id INT NOT NULL,
	Schedule_Name NVARCHAR(50) NOT NULL,
	Schedule_Date DATE NOT NULL,
	Schedule_Description NVARCHAR(MAX) NULL,
    FOREIGN KEY (Class_id) REFERENCES Class(Class_id),
PRIMARY KEY CLUSTERED ([Schedule_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO



-- ========================
-- 20. Bảng Team
-- ========================
CREATE TABLE Team (
    Team_id INT NOT NULL IDENTITY (1,1),
	Class_id INT NOT NULL,
    Team_Name NVARCHAR(50) NOT NULL,
    Team_Description NVARCHAR(MAX) NULL,
	FOREIGN KEY (Class_id) REFERENCES Class(Class_id),
PRIMARY KEY CLUSTERED ([Team_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

-- ========================
-- 21. Bảng Quan hệ giữa Team và User
-- ========================
CREATE TABLE Team_User (
    Team_User_id INT NOT NULL IDENTITY (1,1), 
	Team_id INT NOT NULL,
	[User_id] UNIQUEIDENTIFIER NOT NULL,
	Team_User_Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (Team_id) REFERENCES Team(Team_id),
	FOREIGN KEY ([User_id]) REFERENCES [User]([User_id]),
PRIMARY KEY CLUSTERED (Team_User_id ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO



-- ========================
-- 21. Bảng Quan hệ giữa Team và Equipment
-- ========================
CREATE TABLE Team_EquipmentType (
    Team_EquipmentType_id INT NOT NULL IDENTITY (1,1), 
	Team_id INT NOT NULL,
	EquipmentType_id NVARCHAR(50) NOT NULL,
    Team_EquipmentType_Name NVARCHAR(50) NOT NULL,
    Team_EquipmentType_Description NVARCHAR(MAX) NULL,
	Team_EquipmentType_DateBorrow DATE NOT NULL,
	Team_EquipmentType_DateGiveBack DATE NULL,
	Team_EquipmentType_Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (Team_id) REFERENCES Team(Team_id),
	FOREIGN KEY (EquipmentType_id) REFERENCES EquipmentType(EquipmentType_id),
PRIMARY KEY CLUSTERED (Team_EquipmentType_id ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 22. Bảng Quan hệ giữa Team và Kit
-- ========================
CREATE TABLE Team_KitTemplate (
    Team_KitTemplate_id INT NOT NULL IDENTITY (1,1),
	Team_id INT NOT NULL,
	KitTemplate_id NVARCHAR(50) NOT NULL,
    Team_KitTemplate_Name NVARCHAR(50) NOT NULL,
    Team_KitTemplate_Description NVARCHAR(MAX) NULL,
	Team_KitTemplate_DateBorrow DATE NOT NULL,
	Team_KitTemplate_DateGiveBack DATE NULL,
	Team_KitTemplate_Status NVARCHAR(50) NOT NULL,
	FOREIGN KEY (Team_id) REFERENCES Team(Team_id),
	FOREIGN KEY (KitTemplate_id) REFERENCES KitTemplate(KitTemplate_id),
PRIMARY KEY CLUSTERED (Team_KitTemplate_id ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO


-- ========================
-- 23. Bảng Report
-- ========================
CREATE TABLE Report (
    Report_id INT NOT NULL IDENTITY (1,1),
	[User_id] UNIQUEIDENTIFIER NOT NULL,
	Schedule_id INT NOT NULL,
    Report_Title NVARCHAR(50) NOT NULL,
    Report_Description NVARCHAR(MAX) NULL,
    Report_CreateDate DATETIME NOT NULL,
    Report_Status NVARCHAR(50) NOT NULL
	FOREIGN KEY ([User_id]) REFERENCES [User]([User_id]),
	FOREIGN KEY (Schedule_id) REFERENCES Schedule(Schedule_id),
PRIMARY KEY CLUSTERED ([Report_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO




-- ========================
-- 25. Bảng Grade
-- ========================
CREATE TABLE Grade (
    Grade_id INT NOT NULL IDENTITY (1,1),
	[User_id] UNIQUEIDENTIFIER NOT NULL,
	Team_id INT NOT NULL,
    Lab_id INT NOT NULL,
    Grade_Description NVARCHAR(MAX) NULL,
    Grade_Status NVARCHAR(50) NOT NULL
	FOREIGN KEY ([User_id]) REFERENCES [User]([User_id]),
	FOREIGN KEY (Team_id) REFERENCES Team(Team_id),
	FOREIGN KEY (Lab_id) REFERENCES Lab(Lab_id),
PRIMARY KEY CLUSTERED ([Grade_id] ASC)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO