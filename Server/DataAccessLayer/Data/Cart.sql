use Bookstore;

CREATE TABLE Cart (
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_Cart_User FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Cart_Book FOREIGN KEY (BookId) REFERENCES Books(BookId)
);

--1. Add Item to Cart
--This procedure adds an item to the cart. If the item already exists, it increments the quantity; otherwise, it inserts a new row.
CREATE OR ALTER PROCEDURE sp_AddItemToCart
    @UserId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Cart WHERE UserId = @UserId AND BookId = @BookId)
    BEGIN
        UPDATE Cart
        SET Quantity = Quantity + @Quantity
        WHERE UserId = @UserId AND BookId = @BookId;
    END
    ELSE
    BEGIN
        INSERT INTO Cart (UserId, BookId, Quantity)
        VALUES (@UserId, @BookId, @Quantity);
    END
END;

--2. Update Item Quantity in Cart
--This procedure updates the quantity of a specific item in the cart. If the new quantity is zero or less, it removes the item from the cart.
CREATE OR ALTER PROCEDURE sp_UpdateCartItemQuantity
    @UserId INT,
    @BookId INT,
    @Quantity INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Quantity <= 0
    BEGIN
        DELETE FROM Cart
        WHERE UserId = @UserId AND BookId = @BookId;
    END
    ELSE
    BEGIN
        UPDATE Cart
        SET Quantity = @Quantity
        WHERE UserId = @UserId AND BookId = @BookId;
    END
END;

--3. Remove Item from Cart
--This procedure removes a specific item from the cart.
CREATE OR ALTER PROCEDURE sp_RemoveItemFromCart
    @UserId INT,
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Cart
    WHERE UserId = @UserId AND BookId = @BookId;
END;

--4. Retrieve Cart Contents
--This procedure retrieves all items in the cart for a specific user, including details from the Books table.

CREATE OR ALTER PROCEDURE sp_GetCartContents
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.BookId,
        b.Title,
        b.Author,
        b.Price,
        c.Quantity,
        (b.Price * c.Quantity) AS TotalPrice,
		(ISNULL(b.DiscountedPrice, b.Price) * c.Quantity) AS TotalPriceWithDiscount
    FROM
        Cart c
    INNER JOIN
        Books b ON c.BookId = b.BookId
    WHERE
        c.UserId = @UserId;
END;


--5. Clear Cart
--This procedure removes all items from a user's cart.
CREATE OR ALTER PROCEDURE ClearCart
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Cart
    WHERE UserId = @UserId;
END;
