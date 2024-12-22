use Bookstore;

CREATE TABLE WishList (
    WishListId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    CONSTRAINT FK_WishList_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_WishList_Book FOREIGN KEY (BookId) REFERENCES Books(BookId)
);


CREATE OR ALTER PROCEDURE usp_AddWishList
    @UserId INT,
    @BookId INT
AS
BEGIN
	INSERT INTO WishList (UserId, BookId)
    VALUES (@UserId, @BookId);
END;

CREATE OR ALTER PROCEDURE usp_GetWishList
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        w.BookId,
        b.Title,
        b.Author,
        b.Price,
        CASE 
            WHEN b.DiscountedPrice > 0 THEN b.Price - (b.Price * b.DiscountedPrice / 100) 
            ELSE b.Price 
        END AS DiscountedPrice,
        b.ImageUrl,
        b.StockQuantity
    FROM 
        WishList w
    INNER JOIN 
        Books b ON w.BookId = b.BookId
    WHERE 
        w.UserId = @UserId;
END;


CREATE OR ALTER PROCEDURE usp_DeleteItem
    @WishListId INT
AS
BEGIN
    DELETE FROM WishList
    WHERE WishListId = @WishListId;
END;


CREATE OR ALTER PROCEDURE usp_RemoveAll
    @UserID INT
AS
BEGIN
    DELETE FROM WishList
    WHERE UserID = @UserID;
END;