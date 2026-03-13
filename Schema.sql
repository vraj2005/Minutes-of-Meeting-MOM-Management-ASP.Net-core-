USE master;
ALTER DATABASE MOM_DB
SET SINGLE_USER
WITH ROLLBACK IMMEDIATE;

DROP DATABASE MOM_DB;



CREATE DATABASE MOM_DB;


--MOM_MeetingType
CREATE TABLE MOM_MeetingType (
    MeetingTypeID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingTypeName NVARCHAR(200) NOT NULL,
    Remarks NVARCHAR(500),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE()
);
SELECT * FROM MOM_MeetingType;


--MOM_Department
CREATE TABLE MOM_Department (
    DepartmentID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(200) NOT NULL,
    Remarks NVARCHAR(500),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE()
);
SELECT * FROM MOM_Department;


--MOM_MeetingVenue
CREATE TABLE MOM_MeetingVenue (
    MeetingVenueID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingVenueName NVARCHAR(200) NOT NULL,
    Remarks NVARCHAR(500),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE()
);
SELECT * FROM MOM_MeetingVenue;


--MOM_Staff
CREATE TABLE MOM_Staff (
    StaffID INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentID INT NOT NULL,
    StaffName NVARCHAR(200) NOT NULL,
    Mobile NVARCHAR(20),
    Email NVARCHAR(200),
    Remarks NVARCHAR(500),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (DepartmentID) REFERENCES MOM_Department(DepartmentID)
);
SELECT * FROM MOM_Staff;


--MOM_Meetings
CREATE TABLE MOM_Meetings (
    MeetingID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingDate DATETIME NOT NULL,
    MeetingTypeID INT NOT NULL,
    DepartmentID INT NOT NULL,
    MeetingVenueID INT NOT NULL,
    MeetingDescription NVARCHAR(MAX),
    DocumentPath NVARCHAR(500),
    IsCancelled BIT NOT NULL DEFAULT 0,
    CancellationDateTime DATETIME,
    CancellationReason NVARCHAR(500),
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (MeetingTypeID) REFERENCES MOM_MeetingType(MeetingTypeID),
    FOREIGN KEY (DepartmentID) REFERENCES MOM_Department(DepartmentID),
    FOREIGN KEY (MeetingVenueID) REFERENCES MOM_MeetingVenue(MeetingVenueID)
);
SELECT * FROM MOM_Meetings;


--MOM_MeetingMember
CREATE TABLE MOM_MeetingMember (
    MeetingMemberID INT IDENTITY(1,1) PRIMARY KEY,
    MeetingID INT NOT NULL,
    StaffID INT NOT NULL,
    IsPresent BIT NOT NULL DEFAULT 0,
    Remarks NVARCHAR(500),

    FOREIGN KEY (MeetingID) REFERENCES MOM_Meetings(MeetingID),
    FOREIGN KEY (StaffID) REFERENCES MOM_Staff(StaffID)
);
SELECT * FROM MOM_MeetingMember;

--User 
CREATE TABLE [dbo].[User]
(
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(50) NOT NULL,
    MobileNo NVARCHAR(15) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(50) NOT NULL,
    Address NVARCHAR(200) NULL,
    Created DATETIME DEFAULT GETDATE(),
    Modified DATETIME DEFAULT GETDATE()
);

SELECT * FROM [dbo].[User];
--SP:
--User Login
CREATE PROCEDURE [dbo].[PR_User_Login]
@UserName NVARCHAR(50),
@Password NVARCHAR(50)
AS
BEGIN
SELECT
[dbo].[User].[UserID],
[dbo].[User].[UserName],
[dbo].[User].[MobileNo],
[dbo].[User].[Email],
[dbo].[User].[Password],
[dbo].[User].[Address]
FROM
[dbo].[User]
WHERE
[dbo].[User].[UserName] = @UserName
AND [dbo].[User].[Password] = @Password;
END

EXEC PR_User_Login
    @UserName = 'vraj',
    @Password = '123456';

--User Registration
CREATE PROCEDURE [dbo].[PR_User_Register]
    @UserName NVARCHAR(50),
    @MobileNo NVARCHAR(15),
    @Email NVARCHAR(100),
    @Password NVARCHAR(50),
    @Address NVARCHAR(200)
AS
BEGIN
IF EXISTS (SELECT 1 FROM [dbo].[User] WHERE UserName = @UserName)
BEGIN
    SELECT 'Username already exists' AS Message;
    RETURN;
END
INSERT INTO [dbo].[User]
(
    UserName,
    MobileNo,
    Email,
    Password,
    Address
)
VALUES
(
    @UserName,
    @MobileNo,
    @Email,
    @Password,
    @Address
);
SELECT 'User Registered Successfully' AS Message;
END

EXEC PR_User_Register
    @UserName = 'vraj',
    @MobileNo = '9876543210',
    @Email = 'vraj@gmail.com',
    @Password = '123456',
    @Address = 'Morbi';

--(1) MOM_MeetingType

--SelectAll
CREATE PROCEDURE PR_MOM_MeetingType_SelectAll
AS
SELECT
    MeetingTypeID,
    MeetingTypeName,
    Remarks,
    Created,
    Modified
FROM MOM_MeetingType
ORDER BY MeetingTypeName;

--SelectByPK
CREATE PROCEDURE PR_MOM_MeetingType_SelectByPK
@MeetingTypeID INT
AS
SELECT
    MeetingTypeID,
    MeetingTypeName,
    Remarks,
    Created,
    Modified
FROM MOM_MeetingType
WHERE MeetingTypeID = @MeetingTypeID;

--Insert
CREATE PROCEDURE PR_MOM_MeetingType_Insert
@MeetingTypeName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_MeetingType (MeetingTypeName, Remarks)
VALUES (@MeetingTypeName, @Remarks);


ALTER PROCEDURE PR_MOM_MeetingType_Insert
@MeetingTypeName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_MeetingType
(
    MeetingTypeName,
    Remarks,
    Created
)
VALUES
(
    @MeetingTypeName,
    @Remarks,
    GETDATE()
);


--UpdateByPK
CREATE PROCEDURE PR_MOM_MeetingType_UpdateByPK
@MeetingTypeID INT,
@MeetingTypeName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
UPDATE MOM_MeetingType
SET
    MeetingTypeName = @MeetingTypeName,
    Remarks = @Remarks,
    Modified = GETDATE()
WHERE MeetingTypeID = @MeetingTypeID;

--DelectByPK
CREATE PROCEDURE PR_MOM_MeetingType_DeleteByPK
@MeetingTypeID INT
AS
DELETE FROM MOM_MeetingType
WHERE MeetingTypeID = @MeetingTypeID;


--(2) MOM_Department

--SelectAll
CREATE PROCEDURE PR_MOM_Department_SelectAll
AS
SELECT
    DepartmentID,
    DepartmentName,
    Remarks,
    Created,
    Modified
FROM MOM_Department
ORDER BY DepartmentName;

--SelectByPK
CREATE PROCEDURE PR_MOM_Department_SelectByPK
@DepartmentID INT
AS
SELECT
    DepartmentID,
    DepartmentName,
    Remarks,
    Created,
    Modified
FROM MOM_Department
WHERE DepartmentID = @DepartmentID;

--Insert
CREATE PROCEDURE PR_MOM_Department_Insert
@DepartmentName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_Department (DepartmentName, Remarks)
VALUES (@DepartmentName, @Remarks);


ALTER PROCEDURE PR_MOM_Department_Insert
@DepartmentName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_Department
(
    DepartmentName,
    Remarks,
    Created
)
VALUES
(
    @DepartmentName,
    @Remarks,
    GETDATE()
);


--UpdateByPK
CREATE PROCEDURE PR_MOM_Department_UpdateByPK
@DepartmentID INT,
@DepartmentName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
UPDATE MOM_Department
SET
    DepartmentName = @DepartmentName,
    Remarks = @Remarks,
    Modified = GETDATE()
WHERE DepartmentID = @DepartmentID;


ALTER PROCEDURE PR_MOM_Department_UpdateByPK
@DepartmentID INT,
@DepartmentName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
UPDATE MOM_Department
SET
    DepartmentName = @DepartmentName,
    Remarks = @Remarks,
    Modified = GETDATE()
WHERE DepartmentID = @DepartmentID;


--DeleteByPK
CREATE PROCEDURE PR_MOM_Department_DeleteByPK
@DepartmentID INT
AS
DELETE FROM MOM_Department
WHERE DepartmentID = @DepartmentID;


--(3) MOM_MeetingVenue

--SelectAll
CREATE PROCEDURE PR_MOM_MeetingVenue_SelectAll
AS
SELECT
    MeetingVenueID,
    MeetingVenueName,
    Remarks,
    Created,
    Modified
FROM MOM_MeetingVenue
ORDER BY MeetingVenueName;

--SelectByPK
CREATE PROCEDURE PR_MOM_MeetingVenue_SelectByPK
@MeetingVenueID INT
AS
SELECT
    MeetingVenueID,
    MeetingVenueName,
    Remarks,
    Created,
    Modified
FROM MOM_MeetingVenue
WHERE MeetingVenueID = @MeetingVenueID;

--Insert
CREATE PROCEDURE PR_MOM_MeetingVenue_Insert
@MeetingVenueName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_MeetingVenue (MeetingVenueName, Remarks)
VALUES (@MeetingVenueName, @Remarks);


ALTER PROCEDURE PR_MOM_MeetingVenue_Insert
@MeetingVenueName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_MeetingVenue
(
    MeetingVenueName,
    Remarks,
    Created
)
VALUES
(
    @MeetingVenueName,
    @Remarks,
    GETDATE()
);


--UpdateByPK
CREATE PROCEDURE PR_MOM_MeetingVenue_UpdateByPK
@MeetingVenueID INT,
@MeetingVenueName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
UPDATE MOM_MeetingVenue
SET
    MeetingVenueName = @MeetingVenueName,
    Remarks = @Remarks,
    Modified = GETDATE()
WHERE MeetingVenueID = @MeetingVenueID;


ALTER PROCEDURE PR_MOM_MeetingVenue_UpdateByPK
@MeetingVenueID INT,
@MeetingVenueName NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
UPDATE MOM_MeetingVenue
SET
    MeetingVenueName = @MeetingVenueName,
    Remarks = @Remarks,
    Modified = GETDATE()
WHERE MeetingVenueID = @MeetingVenueID;

--DeleteByPK
CREATE PROCEDURE PR_MOM_MeetingVenue_DeleteByPK
@MeetingVenueID INT
AS
DELETE FROM MOM_MeetingVenue
WHERE MeetingVenueID = @MeetingVenueID;


--(4) MOM_Staff

--SelectAll
CREATE PROCEDURE PR_MOM_Staff_SelectAll
AS
SELECT
    S.StaffID,
    S.StaffName,
    S.Mobile,
    S.Email,
    D.DepartmentName,
    S.Remarks,
    S.Created
FROM MOM_Staff S
INNER JOIN MOM_Department D
    ON D.DepartmentID = S.DepartmentID
ORDER BY D.DepartmentName, S.StaffName;

--SelectByPK
CREATE PROCEDURE PR_MOM_Staff_SelectByPK
@StaffID INT
AS
SELECT
    StaffID,
    DepartmentID,
    StaffName,
    Mobile,
    Email,
    Remarks,
    Created,
    Modified
FROM MOM_Staff
WHERE StaffID = @StaffID;

--Insert
CREATE PROCEDURE PR_MOM_Staff_Insert
@DepartmentID INT,
@StaffName NVARCHAR(200),
@Mobile NVARCHAR(20),
@Email NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_Staff
(DepartmentID, StaffName, Mobile, Email, Remarks)
VALUES
(@DepartmentID, @StaffName, @Mobile, @Email, @Remarks);


ALTER PROCEDURE PR_MOM_Staff_Insert
@DepartmentID INT,
@StaffName NVARCHAR(200),
@Mobile NVARCHAR(20),
@Email NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_Staff
(
    DepartmentID,
    StaffName,
    Mobile,
    Email,
    Remarks,
    Created
)
VALUES
(
    @DepartmentID,
    @StaffName,
    @Mobile,
    @Email,
    @Remarks,
    GETDATE()
);


--UpdateByPK
CREATE PROCEDURE PR_MOM_Staff_UpdateByPK
@StaffID INT,
@DepartmentID INT,
@StaffName NVARCHAR(200),
@Mobile NVARCHAR(20),
@Email NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
UPDATE MOM_Staff
SET
    DepartmentID = @DepartmentID,
    StaffName = @StaffName,
    Mobile = @Mobile,
    Email = @Email,
    Remarks = @Remarks,
    Modified = GETDATE()
WHERE StaffID = @StaffID;


ALTER PROCEDURE PR_MOM_Staff_UpdateByPK
@StaffID INT,
@DepartmentID INT,
@StaffName NVARCHAR(200),
@Mobile NVARCHAR(20),
@Email NVARCHAR(200),
@Remarks NVARCHAR(500)
AS
UPDATE MOM_Staff
SET
    DepartmentID = @DepartmentID,
    StaffName = @StaffName,
    Mobile = @Mobile,
    Email = @Email,
    Remarks = @Remarks,
    Modified = GETDATE()
WHERE StaffID = @StaffID;


--DeleteByPK
CREATE PROCEDURE PR_MOM_Staff_DeleteByPK
@StaffID INT
AS
DELETE FROM MOM_Staff
WHERE StaffID = @StaffID;


--(5) MOM_Meetings

--SelectAll
CREATE PROCEDURE PR_MOM_Meetings_SelectAll
AS
SELECT
    M.MeetingID,
    M.MeetingDate,
    MT.MeetingTypeName,
    D.DepartmentName,
    MV.MeetingVenueName,
    M.IsCancelled,
    M.Created
FROM MOM_Meetings M
INNER JOIN MOM_MeetingType MT ON MT.MeetingTypeID = M.MeetingTypeID
INNER JOIN MOM_Department D ON D.DepartmentID = M.DepartmentID
INNER JOIN MOM_MeetingVenue MV ON MV.MeetingVenueID = M.MeetingVenueID
ORDER BY M.MeetingDate DESC;

--SelectByPK
CREATE PROCEDURE PR_MOM_Meetings_SelectByPK
@MeetingID INT
AS
SELECT *
FROM MOM_Meetings
WHERE MeetingID = @MeetingID;

--Insert
CREATE PROCEDURE PR_MOM_Meetings_Insert
@MeetingDate DATETIME,
@MeetingTypeID INT,
@DepartmentID INT,
@MeetingVenueID INT,
@MeetingDescription NVARCHAR(MAX),
@DocumentPath NVARCHAR(500)
AS
INSERT INTO MOM_Meetings
(MeetingDate, MeetingTypeID, DepartmentID, MeetingVenueID, MeetingDescription, DocumentPath)
VALUES
(@MeetingDate, @MeetingTypeID, @DepartmentID, @MeetingVenueID, @MeetingDescription, @DocumentPath);


ALTER PROCEDURE PR_MOM_Meetings_Insert
@MeetingDate DATETIME,
@MeetingTypeID INT,
@DepartmentID INT,
@MeetingVenueID INT,
@MeetingDescription NVARCHAR(MAX),
@DocumentPath NVARCHAR(500)
AS
INSERT INTO MOM_Meetings
(
    MeetingDate,
    MeetingTypeID,
    DepartmentID,
    MeetingVenueID,
    MeetingDescription,
    DocumentPath,
    Created
)
VALUES
(
    @MeetingDate,
    @MeetingTypeID,
    @DepartmentID,
    @MeetingVenueID,
    @MeetingDescription,
    @DocumentPath,
    GETDATE()
);


--UpdateByPK
CREATE PROCEDURE PR_MOM_Meetings_UpdateByPK
@MeetingID INT,
@MeetingDate DATETIME,
@MeetingTypeID INT,
@DepartmentID INT,
@MeetingVenueID INT,
@MeetingDescription NVARCHAR(MAX),
@DocumentPath NVARCHAR(500)
AS
UPDATE MOM_Meetings
SET
    MeetingDate = @MeetingDate,
    MeetingTypeID = @MeetingTypeID,
    DepartmentID = @DepartmentID,
    MeetingVenueID = @MeetingVenueID,
    MeetingDescription = @MeetingDescription,
    DocumentPath = @DocumentPath,
    Modified = GETDATE()
WHERE MeetingID = @MeetingID;

-- Created by GitHub Copilot in SSMS - review carefully before executing
ALTER PROCEDURE dbo.PR_MOM_Meetings_UpdateByPK
    @MeetingID INT,
    @MeetingDate DATETIME,
    @MeetingTypeID INT,
    @DepartmentID INT,
    @MeetingVenueID INT,
    @MeetingDescription NVARCHAR(MAX),
    @DocumentPath NVARCHAR(500),
    @IsCancelled BIT,
    @CancellationDateTime DATETIME,
    @CancellationReason NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.MOM_Meetings
    SET
        MeetingDate = @MeetingDate,
        MeetingTypeID = @MeetingTypeID,
        DepartmentID = @DepartmentID,
        MeetingVenueID = @MeetingVenueID,
        MeetingDescription = @MeetingDescription,
        DocumentPath = @DocumentPath,
        IsCancelled = @IsCancelled,
        CancellationDateTime = @CancellationDateTime,
        CancellationReason = @CancellationReason,
        Modified = GETDATE()
    WHERE MeetingID = @MeetingID;
END;


ALTER PROCEDURE PR_MOM_Meetings_UpdateByPK
@MeetingID INT,
@MeetingDate DATETIME,
@MeetingTypeID INT,
@DepartmentID INT,
@MeetingVenueID INT,
@MeetingDescription NVARCHAR(MAX),
@DocumentPath NVARCHAR(500)
AS
UPDATE MOM_Meetings
SET
    MeetingDate = @MeetingDate,
    MeetingTypeID = @MeetingTypeID,
    DepartmentID = @DepartmentID,
    MeetingVenueID = @MeetingVenueID,
    MeetingDescription = @MeetingDescription,
    DocumentPath = @DocumentPath,
    Modified = GETDATE()
WHERE MeetingID = @MeetingID;


--DeleteByPK
CREATE PROCEDURE PR_MOM_Meetings_DeleteByPK
@MeetingID INT
AS
DELETE FROM MOM_Meetings
WHERE MeetingID = @MeetingID;


--(6) MOM_MeetingMember

--SelectAll
CREATE PROCEDURE PR_MOM_MeetingMember_SelectAll
AS
SELECT
    MM.MeetingMemberID,
    S.StaffName,
    M.MeetingDate,
    MM.IsPresent,
    MM.Remarks
FROM MOM_MeetingMember MM
INNER JOIN MOM_Staff S ON S.StaffID = MM.StaffID
INNER JOIN MOM_Meetings M ON M.MeetingID = MM.MeetingID
ORDER BY M.MeetingDate DESC, S.StaffName;

--SelectByPK
CREATE PROCEDURE PR_MOM_MeetingMember_SelectByPK
@MeetingMemberID INT
AS
SELECT *
FROM MOM_MeetingMember
WHERE MeetingMemberID = @MeetingMemberID;

--Insert
CREATE PROCEDURE PR_MOM_MeetingMember_Insert
@MeetingID INT,
@StaffID INT,
@IsPresent BIT,
@Remarks NVARCHAR(500)
AS
INSERT INTO MOM_MeetingMember
(MeetingID, StaffID, IsPresent, Remarks)
VALUES
(@MeetingID, @StaffID, @IsPresent, @Remarks);

--UpdateByPK
CREATE PROCEDURE PR_MOM_MeetingMember_UpdateByPK
@MeetingMemberID INT,
@MeetingID INT,
@StaffID INT,
@IsPresent BIT,
@Remarks NVARCHAR(500)
AS
UPDATE MOM_MeetingMember
SET
    MeetingID = @MeetingID,
    StaffID = @StaffID,
    IsPresent = @IsPresent,
    Remarks = @Remarks
WHERE MeetingMemberID = @MeetingMemberID;

--DeleteByPK
CREATE PROCEDURE PR_MOM_MeetingMember_DeleteByPK
@MeetingMemberID INT
AS
DELETE FROM MOM_MeetingMember
WHERE MeetingMemberID = @MeetingMemberID;