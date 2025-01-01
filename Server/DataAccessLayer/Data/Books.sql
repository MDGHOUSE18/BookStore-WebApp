use BookstoresApp;



Select * from books
ALTER TABLE Books DROP COLUMN ImageUrl;
ALTER TABLE Books ADD ImageData VARBINARY(MAX);

CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2) NOT NULL,
    DiscountedPrice DECIMAL(10, 2),
    ImageData VARBINARY(MAX),
    StockQuantity INT NOT NULL,
    DateAdded DATETIME DEFAULT GETDATE(),
    LastUpdated DATETIME DEFAULT GETDATE()
);

CREATE OR ALTER PROCEDURE usp_AddBook
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @Price DECIMAL(10, 2),
    @DiscountedPrice DECIMAL(10, 2),
    @ImageData VARBINARY(MAX),
    @StockQuantity INT,
    @BookID INT OUTPUT
AS
BEGIN
    INSERT INTO Books (Title, Author, Description, Price, DiscountedPrice, ImageData, StockQuantity, DateAdded, LastUpdated)
    VALUES (@Title, @Author, @Description, @Price, @DiscountedPrice, @ImageData, @StockQuantity, GETDATE(), GETDATE());

    SET @BookID = SCOPE_IDENTITY();

    IF @@ROWCOUNT > 0
        RETURN 1;
    ELSE
        RETURN 0;
END;



CREATE OR ALTER PROCEDURE usp_UpdateBook
    @BookID INT,
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @Price DECIMAL(10, 2),
    @DiscountedPrice DECIMAL(10, 2),
    @ImageData VARBINARY(MAX),
    @StockQuantity INT
AS
BEGIN
    UPDATE Books
    SET 
        Title = @Title,
        Author = @Author,
        Description = @Description,
        Price = @Price,
        DiscountedPrice = @DiscountedPrice,
        ImageData = @ImageData,
        StockQuantity = @StockQuantity,
        LastUpdated = GETDATE()
    WHERE BookID = @BookID;

    IF @@ROWCOUNT > 0
        RETURN 1;
    ELSE
        RETURN 0;
END;


CREATE OR ALTER PROCEDURE usp_DeleteBook
    @BookID INT
AS
BEGIN
    DELETE FROM Books
    WHERE BookID = @BookID;

    IF @@ROWCOUNT > 0
        RETURN 1;
    ELSE
        RETURN 0;
END;


CREATE OR ALTER PROCEDURE usp_GetAllBooks
AS
BEGIN
    SELECT 
        BookID,
        Title,
        Author,
        Description,
        Price,
        DiscountedPrice,
        ImageData,
        StockQuantity
    FROM Books
    ORDER BY BookID;
END;



CREATE OR ALTER PROCEDURE usp_GetBookById
    @BookID INT
AS
BEGIN
    SELECT 
        BookID,
        Title,
        Author,
        Description,
        Price,
        DiscountedPrice,
        ImageData,
        StockQuantity
    FROM Books
    WHERE BookID = @BookID;
END;



