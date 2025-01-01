use BookstoresApp;


SELECT * FROM AddressTypeTable
CREATE TABLE AddressTypeTable (
    ID INT PRIMARY KEY IDENTITY(1,1),
    TypeOfAddress NVARCHAR(50) NOT NULL
);

CREATE TABLE AddressTable (
    AddressID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    TypeID INT NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    CONSTRAINT FK_AddressTable_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_AddressTable_TypeID FOREIGN KEY (TypeID) REFERENCES AddressTypeTable(ID)
);

INSERT INTO AddressTypeTable (TypeOfAddress)
VALUES ('Home'), ('Work'), ('Other');


CREATE OR ALTER PROCEDURE usp_AddAddress
    @UserID INT,
    @TypeID INT,
    @Address NVARCHAR(255),
    @City NVARCHAR(100),
    @State NVARCHAR(100)
AS
BEGIN
    -- Insert the new address into the AddressTable
    INSERT INTO AddressTable (UserID, TypeID, Address, City, State)
    VALUES (@UserID, @TypeID, @Address, @City, @State);

    -- Return the inserted row
    SELECT TOP 1 u.FullName,u.PhoneNumber, TypeOfAddress, Address, City, State
    FROM AddressTable a
	Inner Join AddressTypeTable at ON a.TypeID=at.ID
	Inner Join Users u ON u.UserId=a.UserID
	WHERE a.UserID = @UserID AND a.TypeID = @TypeID AND a.Address = @Address AND a.City = @City AND a.State = @State
    ORDER BY a.AddressID DESC;
END;


CREATE OR ALTER PROCEDURE usp_GetAddressById
    @AddressID INT
AS
BEGIN
    SELECT u.FullName,u.PhoneNumber, TypeOfAddress, Address, City, State
    FROM AddressTable a
	Inner Join AddressTypeTable at ON a.TypeID=at.ID
	Inner Join Users u ON u.UserId=a.UserID
    WHERE a.AddressID = @AddressID;
END;

CREATE OR ALTER PROCEDURE usp_UpdateAddress
    @AddressID INT,
    @UserID INT,
    @TypeID INT,
    @Address NVARCHAR(255),
    @City NVARCHAR(100),
    @State NVARCHAR(100)
AS
BEGIN
    UPDATE AddressTable
    SET UserID = @UserID,
        TypeID = @TypeID,
        Address = @Address,
        City = @City,
        State = @State
    WHERE AddressID = @AddressID;

	SELECT u.FullName,u.PhoneNumber, TypeOfAddress, Address, City, State
    FROM AddressTable a
	Inner Join AddressTypeTable at ON a.TypeID=at.ID
	Inner Join Users u ON u.UserId=a.UserID
	Where a.AddressID = @AddressID
END;

CREATE OR ALTER PROCEDURE usp_DeleteAddress
    @AddressID INT
AS
BEGIN
    DELETE FROM AddressTable
    WHERE AddressID = @AddressID;
END;

CREATE OR ALTER PROCEDURE usp_GetAddressesByUserId
    @UserID INT
AS
BEGIN
    SELECT u.FullName,u.PhoneNumber, TypeOfAddress, Address, City, State
    FROM AddressTable a
	Inner Join AddressTypeTable at ON a.TypeID=at.ID
	Inner Join Users u ON u.UserId=a.UserID
    WHERE a.UserID = @UserID;
END;
