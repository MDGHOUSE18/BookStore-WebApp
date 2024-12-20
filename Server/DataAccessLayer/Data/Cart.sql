use Bookstore;

CREATE TABLE Cart (
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_Cart_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Cart_Book FOREIGN KEY (BookId) REFERENCES Books(BookId)
);

CREATE OR ALTER PROCEDURE usp_AddCartItem
    @UserId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Cart WHERE UserId = @UserId AND BookId = @BookId)
    BEGIN
        -- If the item already exists in the cart, update the quantity
        UPDATE Cart
        SET Quantity = Quantity + @Quantity
        WHERE UserId = @UserId AND BookId = @BookId;
    END
    ELSE
    BEGIN
        -- If the item does not exist, insert a new row
        INSERT INTO Cart (UserId, BookId, Quantity)
        VALUES (@UserId, @BookId, @Quantity);
    END
END;

CREATE OR ALTER PROCEDURE usp_GetCartItems
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.BookId,
        b.Title,
        b.Author,
        b.Price,
        CASE 
            WHEN b.DiscountedPrice > 0 THEN b.Price - (b.Price * b.DiscountedPrice / 100) 
            ELSE b.Price 
        END AS DiscountedPrice,
        b.ImageUrl,
        b.StockQuantity,
        c.Quantity AS CartQuantity,
        c.Quantity * CASE 
            WHEN b.DiscountedPrice > 0 THEN b.Price - (b.Price * b.DiscountedPrice / 100) 
            ELSE b.Price 
        END AS TotalPrice
    FROM 
        Cart c
    INNER JOIN 
        Books b ON c.BookId = b.BookId
    WHERE 
        c.UserId = @UserId;
END;

EXEC usp_GetCartItems @UserId = 1;
Select * from Cart
CREATE OR ALTER PROCEDURE usp_DeleteCart
    @CartID INT
AS
BEGIN
    DELETE FROM Cart
    WHERE CartId = @CartID;

    --IF @@ROWCOUNT > 0
    --    RETURN 1;
    --ELSE
    --    RETURN 0;
END;


CREATE OR ALTER PROCEDURE usp_RemoveAllFromCart
    @UserID INT
AS
BEGIN
    DELETE FROM Cart
    WHERE UserID = @UserID;
END;