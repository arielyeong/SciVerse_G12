update tblRegisteredUsers Set Password='wapp'

select * from tblRegisteredUsers

DELETE FROM tblRegisteredUsers;

DBCC CHECKIDENT ('tblRegisteredUsers', RESEED, 0);
TRUNCATE TABLE tblRegisteredUsers;

DELETE FROM tblRegisteredUsers
WHERE RID IN (13);

ALTER TABLE tblRegisteredUsers
ADD Role NVARCHAR(20) NOT NULL DEFAULT 'User';

SELECT * FROM [tblContactUs]

DROP TABLE tblContactUs;

DROP TABLE tblRegisteredUsers;

CREATE TABLE [dbo].[tblContactUs] (
    [CID]            INT            IDENTITY (1, 1) NOT NULL,
    [contactName]    NVARCHAR (50)  NULL,
    [contactEmail]   NVARCHAR (50)  NULL,
    [contactMessage] NVARCHAR (MAX) NULL
);


CREATE TABLE [dbo].[tblRegisteredUsers] (
    [RID]          INT            IDENTITY (1, 1) NOT NULL,
    [fullName]     NVARCHAR (100) NULL,
    [emailAddress] NVARCHAR (50)  NULL,
    [username]     NVARCHAR (50)  NULL,
    [password]     NVARCHAR (50)  NULL,
    [age]          INT            NULL,
    [gender]       NCHAR (10)     NULL,
    [country]      NVARCHAR (MAX) NULL,
    [picture]      NVARCHAR (MAX) NULL,
    [dateRegister] DATETIME       NULL,
    [role]         NVARCHAR (20)  DEFAULT ('User') NOT NULL,
    PRIMARY KEY CLUSTERED ([RID] ASC)
);

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Kaja Rooney', 'Rooney', 'krooney0', 'wapp', 15, 'Female', 'Colombia', 'https://robohash.org/suntquiet.png?size=50x50&set=set1', '2025-02-19 11:42:17', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Ettore MacAnulty', 'MacAnulty', 'emacanulty1', 'wapp', 8, 'Male', 'New Zealand', 'https://robohash.org/inventoreliberosit.png?size=50x50&set=set1', '2024-11-05 06:14:41', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Jorge Gerlts', 'Gerlts', 'jgerlts2', 'wapp', 17, 'Male', 'Thailand', 'https://robohash.org/sapientenihilfugit.png?size=50x50&set=set1', '2025-06-12 07:32:32', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Nancee Pegler', 'Pegler', 'npegler3', 'wapp', 15, 'Female', 'Iceland', 'https://robohash.org/earumabvoluptatem.png?size=50x50&set=set1', '2025-09-15 07:16:42', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Aida Santarelli', 'Santarelli', 'aida', 'wapp"', 16, 'Female', 'Indonesia', 'https://robohash.org/facerequivelit.png?size=50x50&set=set1', '2025-02-20 01:28:28', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Mirilla Tefft', 'Tefft', 'mtefft5', 'wapp', 8, 'Female', 'China', 'https://robohash.org/magnamquiaeaque.png?size=50x50&set=set1', '2025-01-12 23:54:23', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Catlee Cobbled', 'Cobbled', 'ccobbled6', 'wapp', 11, 'Female', 'Czech Republic', 'https://robohash.org/voluptatemexet.png?size=50x50&set=set1', '2025-02-04 23:05:16', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Brinna Cleaver', 'Cleaver', 'bcleaver7', 'wapp', 8, 'Female', 'Canada', 'https://robohash.org/laborumquiain.png?size=50x50&set=set1', '2025-01-23 15:38:34', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('Yeong Huey Yee', 'hueyyee@mail.com', 'hueyyee', 'wapp', 18, 'Female', 'China', 'https://robohash.org/aspernaturvelrepudiandae.png?size=50x50&set=set1', '2025-07-26 20:58:16', 'User');

INSERT INTO tblRegisteredUsers (fullName, emailAddress, username, password, age, gender, country, picture, dateRegister, role) 
VALUES ('System Admin', 'admin@example.com', 'admin', 'admin123', 30, 'Male', 'Malaysia', 'https://robohash.org/admin.png?size=50x50&set=set1', GETDATE(), 'Admin');

