use BookstoresApp;



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
        UPDATE Cart
        SET Quantity = Quantity + @Quantity
        WHERE UserId = @UserId AND BookId = @BookId;
    END
    ELSE
    BEGIN
        -- Insert a new item into the cart
        INSERT INTO Cart (UserId, BookId, Quantity)
        VALUES (@UserId, @BookId, @Quantity);
    END

    SELECT 
        c.CartId,
        c.BookId,
        b.Title,
        b.Author,
        b.Price, 
        b.DiscountedPrice,  
        b.ImageData,
        c.Quantity AS CartQuantity
    FROM 
        Cart c
    INNER JOIN 
        Books b ON c.BookId = b.BookId
    WHERE 
        c.UserId = @UserId AND c.BookId = @BookId;
END;



CREATE OR ALTER PROCEDURE usp_GetCartItems
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        c.CartId,                            
        c.BookId,
        b.Title,
        b.Author,
        b.Price,  
        b.DiscountedPrice,
        b.ImageData,
		b.StockQuantity,
        c.Quantity AS CartQuantity           
    FROM 
        Cart c
    INNER JOIN 
        Books b ON c.BookId = b.BookId
    WHERE 
        c.UserId = @UserId;
END;


CREATE OR ALTER PROCEDURE usp_DeleteCart
    @CartID INT
AS
BEGIN
    DELETE FROM Cart
    WHERE BookId = @CartID;
END;



CREATE OR ALTER PROCEDURE usp_RemoveAllFromCart
    @UserID INT
AS
BEGIN
    DELETE FROM Cart
    WHERE UserID = @UserID;
END;

CREATE OR ALTER PROCEDURE usp_UpdateCartItemQuantity
    @UserId INT,
    @BookId INT,
    @NewQuantity INT
AS
BEGIN
    -- Update the cart item quantity
    UPDATE Cart
    SET Quantity = Quantity+@NewQuantity
    WHERE UserId = @UserId AND BookId = @BookId;
END;
