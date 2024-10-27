CREATE DATABASE UserManagementDB;
USE UserManagementDB;


--SHOW TABLES CREATED
SELECT name 
FROM sys.tables;


------------------USERS-------------------------

--TABLE USERS
CREATE TABLE Users(
UserID VARCHAR(50) NOT NULL,
NameUser Varchar(MAX) NOT NULL,
Email VARCHAR(100) NOT NULL, 
PasswordHash VARCHAR(200) NOT NULL, 
IsActive bit,
Created DATETIME2(7),
Modified DATETIME2(7),
Last_login DATETIME2(7),
PRIMARY KEY (UserID),
UNIQUE (Email)
);



--SHOW TABLE Users
SELECT * FROM Users

-- TO SEE INFORMATION ABOUT THE TABLE User
EXEC sp_help Users;

--------------------PHONES------------------------

--TABLE PHONES
CREATE TABLE Phones(
PhoneID INT IDENTITY(1,1) PRIMARY KEY,
UserID Varchar(50) NOT NULL,
Number VARCHAR(20) NOT NULL,
City_Code VARCHAR(MAX),
Country_Code VARCHAR(MAX),
);

--SHOW TABLE Phones
SELECT * FROM Phones

-- TO SEE INFORMATION ABOUT THE TABLE Phones
EXEC sp_help Phones;




----------FOREIGN KEY------------
ALTER TABLE Phones 
ADD CONSTRAINT FK_Phones_UsuariosID
FOREIGN KEY (UserID) REFERENCES Users(UserID);



-- EN CASO DE SER NECESARIO BORRAR LA FOREIGN KEY
ALTER TABLE Phones 
DROP CONSTRAINT FK_Phones_UsuariosID;