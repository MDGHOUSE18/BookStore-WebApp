use Bookstore;


CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(10, 2) NOT NULL,
    DiscountedPrice DECIMAL(10, 2),
    ImageUrl NVARCHAR(500),
    StockQuantity INT NOT NULL,
    DateAdded DATETIME DEFAULT GETDATE(),
    LastUpdated DATETIME DEFAULT GETDATE()
);

CREATE OR ALTER PROCEDURE sp_AddBook
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @Price DECIMAL(10, 2),
    @DiscountedPrice DECIMAL(10, 2),
    @ImageUrl NVARCHAR(500),
    @StockQuantity INT,
    @BookID INT OUTPUT
AS
BEGIN
    INSERT INTO Books (Title, Author, Description, Price, DiscountedPrice, ImageUrl, StockQuantity, DateAdded, LastUpdated)
    VALUES (@Title, @Author, @Description, @Price, @DiscountedPrice, @ImageUrl, @StockQuantity, GETDATE(), GETDATE());

    SET @BookID = SCOPE_IDENTITY();

    IF @@ROWCOUNT > 0
        RETURN 1;
    ELSE
        RETURN 0;
END;


CREATE OR ALTER PROCEDURE sp_UpdateBook
    @BookID INT,
    @Title NVARCHAR(255),
    @Author NVARCHAR(255),
    @Description NVARCHAR(MAX),
    @Price DECIMAL(10, 2),
    @DiscountedPrice DECIMAL(10, 2),
    @ImageUrl NVARCHAR(500),
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
        ImageUrl = @ImageUrl,
        StockQuantity = @StockQuantity,
        LastUpdated = GETDATE()
    WHERE BookID = @BookID;

    IF @@ROWCOUNT > 0
        RETURN 1;
    ELSE
        RETURN 0;
END;

CREATE OR ALTER PROCEDURE sp_DeleteBook
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

CREATE OR ALTER PROCEDURE sp_GetAllBooks
AS
BEGIN
    SELECT * 
    FROM Books
    ORDER BY DateAdded DESC;
END;


CREATE OR ALTER PROCEDURE sp_GetBookById
    @BookID INT
AS
BEGIN
    SELECT * 
    FROM Books
    WHERE BookID = @BookID;
END;
