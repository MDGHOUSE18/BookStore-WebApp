use BookstoresApp;

Truncate Table Orders

CREATE TABLE OrderStatus (
    OrderStatusId INT IDENTITY(1,1) PRIMARY KEY,
    OrderStatus VARCHAR(50) NOT NULL
);
INSERT INTO OrderStatus (OrderStatus)
VALUES
    ('Shipped'),
    ('Delivered'),
    ('Canceled');

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    AddressID INT NOT NULL,
    TotalPrice DECIMAL(10, 2) NOT NULL,
    TotalDiscountedPrice DECIMAL(10, 2) NOT NULL,
    OrderDate DATE NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE(),
    Status INT NOT NULL,
    
    CONSTRAINT FK_Orders_UserID FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Orders_AddressID FOREIGN KEY (AddressID) REFERENCES AddressTable(AddressID),
    CONSTRAINT FK_Orders_Status FOREIGN KEY (Status) REFERENCES OrderStatus(OrderStatusId)
);

SELECT * from OrderDetails
CREATE TABLE OrderDetails (
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    DiscountedPrice DECIMAL(10, 2) NOT NULL,
    
    CONSTRAINT FK_OrderDetails_OrderId FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderDetails_BookId FOREIGN KEY (BookId) REFERENCES Books(BookId)
);

SELECT * from Orders


CREATE OR ALTER PROCEDURE usp_CreateOrder
    @UserId INT,
    @AddressId INT
AS
BEGIN
    DECLARE @TotalPrice DECIMAL(10, 2) = 0;
    DECLARE @TotalDiscountedPrice DECIMAL(10, 2) = 0;
    DECLARE @OrderId INT;

    BEGIN TRANSACTION;

    BEGIN TRY
        SELECT 
            @TotalPrice = SUM(COALESCE(b.Price, 0) * COALESCE(c.Quantity, 0)),
            @TotalDiscountedPrice = SUM(COALESCE(b.DiscountedPrice, b.Price, 0) * COALESCE(c.Quantity, 0))
        FROM 
            Cart c
        INNER JOIN 
            Books b ON c.BookId = b.BookId
        WHERE 
            c.UserId = @UserId;

        IF @TotalPrice <= 0 OR @TotalDiscountedPrice <= 0
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Invalid price or discounted price', 16, 1);
            RETURN;
        END;

        IF EXISTS (
            SELECT 1 
            FROM Cart c
            INNER JOIN Books b ON c.BookId = b.BookId
            WHERE c.UserId = @UserId AND c.Quantity > b.StockQuantity
        )
        BEGIN
            ROLLBACK TRANSACTION;
            RAISERROR ('Insufficient stock', 16, 1);
            RETURN;
        END;

        INSERT INTO Orders (UserId, AddressID, TotalPrice, TotalDiscountedPrice, OrderDate, Status)
        VALUES (@UserId, @AddressId, @TotalPrice, @TotalDiscountedPrice, GETDATE(), 2);
        
        SET @OrderId = SCOPE_IDENTITY();

        INSERT INTO OrderDetails (OrderId, BookId, Quantity, Price, DiscountedPrice)
        SELECT 
            @OrderId, 
            c.BookId, 
            COALESCE(c.Quantity, 0), 
            COALESCE(b.Price, 0), 
            COALESCE(b.DiscountedPrice, b.Price, 0)
        FROM 
            Cart c
        INNER JOIN 
            Books b ON c.BookId = b.BookId
        WHERE 
            c.UserId = @UserId;

        UPDATE b
        SET b.StockQuantity = b.StockQuantity - c.Quantity
        FROM Cart c
        INNER JOIN Books b ON c.BookId = b.BookId
        WHERE c.UserId = @UserId;

        DELETE FROM Cart WHERE UserId = @UserId;

		SELECT 
		o.OrderId, 
		o.TotalPrice, 
		o.TotalDiscountedPrice, 
		o.OrderDate, 
		o.Status
		FROM Orders o
		WHERE o.UserId = @UserId
		AND o.OrderId = (
			SELECT TOP 1 OrderId
			FROM Orders
			WHERE UserId = @UserId
			ORDER BY OrderDate DESC, OrderId DESC
		);
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;

CREATE OR ALTER PROCEDURE usp_UpdateOrderStatus
    @OrderId INT,
    @StatusId INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM OrderStatus WHERE OrderStatusId = @StatusId)
    BEGIN
        PRINT 'Invalid StatusId';
        RETURN;
    END

    UPDATE Orders
    SET Status = @StatusId, UpdatedAt = GETDATE()
    WHERE OrderId = @OrderId;

    IF @StatusId = 4 
    BEGIN
        UPDATE b
        SET b.StockQuantity = b.StockQuantity + od.Quantity
        FROM OrderDetails od
        INNER JOIN Books b ON od.BookId = b.BookId
        WHERE od.OrderId = @OrderId;
    END

    PRINT 'Order status updated successfully';
END;

--CREATE OR ALTER PROCEDURE usp_GetOrders
--    @UserId INT
--AS
--BEGIN
--    SELECT 
--        o.OrderId, 
--        o.TotalPrice, 
--        o.TotalDiscountedPrice, 
--        o.OrderDate, 
--        o.Status
--    FROM Orders o
--    WHERE o.UserId = @UserId;
--END;
CREATE OR ALTER PROCEDURE usp_GetOrders
    @UserId INT
AS
BEGIN
    SELECT 
        o.OrderId, 
        o.TotalPrice, 
        o.TotalDiscountedPrice, 
        o.OrderDate, 
        o.Status,
        od.Quantity,
        b.Title AS BookTitle, 
        b.Author, 
        b.ImageData
    FROM Orders o
    INNER JOIN OrderDetails od ON o.OrderId = od.OrderId
    INNER JOIN Books b ON od.BookId = b.BookId
    WHERE o.UserId = @UserId
    ORDER BY o.OrderDate DESC, o.OrderId DESC; -- Orders can be sorted for better readability
END;


CREATE OR ALTER PROCEDURE usp_GetOrder
    @OrderId INT
AS
BEGIN
    SELECT 
        o.OrderId, 
        b.Title AS BookTitle, 
        b.Author, 
		b.ImageData,
        od.Quantity, 
        (b.Price * od.Quantity) AS TotalPrice, 
        (COALESCE(b.DiscountedPrice, b.Price) * od.Quantity) AS DiscountedPrice, 
        o.OrderDate, 
        o.Status
    FROM Orders o
    INNER JOIN OrderDetails od ON o.OrderId = od.OrderId
    INNER JOIN Books b ON od.BookId = b.BookId
    WHERE o.OrderId = @OrderId;
END;

CREATE OR ALTER PROCEDURE usp_GetOrders
    @UserId INT
AS
BEGIN
    SELECT 
        o.OrderId, 
        b.Title AS BookTitle, 
        b.Author, 
        b.ImageData, 
        b.Description, -- Add other book details as needed
        od.Quantity, 
        (b.Price * od.Quantity) AS TotalPrice, 
        (COALESCE(b.DiscountedPrice, b.Price) * od.Quantity) AS DiscountedPrice, 
        o.OrderDate, 
        o.Status
    FROM Orders o
    INNER JOIN OrderDetails od ON o.OrderId = od.OrderId
    INNER JOIN Books b ON od.BookId = b.BookId
    WHERE o.UserId = @UserId;
END;
