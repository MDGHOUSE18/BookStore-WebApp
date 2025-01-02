use BookstoresApp;

SELECT * FROM Users

--DROP TABLE Users;
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1), -- Auto-incrementing primary key
    FullName NVARCHAR(255) NOT NULL, -- Full name
    Email NVARCHAR(255) NOT NULL UNIQUE, -- Email with unique constraint
    PasswordHash NVARCHAR(255) NOT NULL, -- Password hash
    Role NVARCHAR(50) NOT NULL, -- Role of the user
    PhoneNumber NVARCHAR(20), -- Optional phone number
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(), -- Default to current date and time
    UpdatedAt DATETIME NULL, -- Nullable updated timestamp
    LastLogin DATETIME NULL, -- Nullable last login timestamp
);

Select * from Users;
CREATE OR ALTER PROCEDURE usp_CreateUser
    @FullName NVARCHAR(255),
    @Email NVARCHAR(255),
    @PasswordHash NVARCHAR(255),
    @Role NVARCHAR(50),
    @PhoneNumber NVARCHAR(20) = NULL
AS
BEGIN
    INSERT INTO Users (FullName, Email, PasswordHash, Role, PhoneNumber, CreatedAt)
    VALUES (@FullName, @Email, @PasswordHash, @Role, @PhoneNumber, GETDATE());
END

CREATE OR ALTER PROCEDURE usp_Login
    @Email NVARCHAR(100),
    @Password NVARCHAR(255)
AS
BEGIN
    -- Declare variable for UserId
    DECLARE @UserId INT;

    -- Check if the user exists and get the UserId
    SELECT @UserId = UserId
    FROM Users
    WHERE Email = @Email AND PasswordHash = @Password;

    -- If the user exists, update LastLogin and IsActive, then return user details
    IF @UserId IS NOT NULL
    BEGIN
        -- Update LastLogin and IsActive fields
        UPDATE Users
        SET LastLogin = GETDATE()
        WHERE UserID = @UserId;
		 -- Return user details (excluding sensitive fields)
        SELECT 
			UserId,
            FullName, 
            Email, 
            Role,  
            PhoneNumber, 
            CreatedAt
        FROM Users
        WHERE UserId = @UserId;
    END
END;

CREATE OR ALTER PROCEDURE usp_GetUserById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT UserId, FullName, Email, Role, PhoneNumber, CreatedAt, UpdatedAt, LastLogin
    FROM Users
    WHERE UserId = @Id;
END;



CREATE OR ALTER PROCEDURE usp_CheckUserByEmail
    @Email NVARCHAR(255) -- Input parameter for the email
AS
BEGIN
    -- Check if a user with the provided email exists in the Users table
    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
    BEGIN
        -- Return 1 if the user exists
        SELECT 1 AS UserExists;
    END
    ELSE
    BEGIN
        -- Return 0 if the user does not exist
        SELECT 0 AS UserExists;
    END
END

EXEC CheckUserByEmail @Email = 'user@example.com';



CREATE OR ALTER PROCEDURE usp_ResetPassword
    @Email NVARCHAR(255),
    @Password NVARCHAR(255)
AS
BEGIN
    DECLARE @UserExists INT;

    -- Check if user exists
    SELECT @UserExists = COUNT(*) FROM Users WHERE Email = @Email;

    IF @UserExists > 0
    BEGIN
        -- Reset the password logic here
        UPDATE Users SET PasswordHash = @Password WHERE Email = @Email;
        RETURN 1;  -- Indicate success
    END
    ELSE
    BEGIN
        RETURN 0;  -- Indicate failure (user not found)
    END
END


EXEC ResetPassword @Email="ghouseakr@gmail.com",@Password="TWRnQDEyMzQ=";


Select * from Users