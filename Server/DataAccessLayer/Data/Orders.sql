use Bookstore;

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
    CartId INT,
    AddressID INT NOT NULL,
    Quantity INT NOT NULL, 
    TotalPrice DECIMAL(10, 2),
    OrdateDate DATE,
    Status INT,  

    CONSTRAINT FK_Orders_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT FK_Orders_CartID FOREIGN KEY (CartId) REFERENCES Cart(CartId),
    CONSTRAINT FK_Orders_AddressID FOREIGN KEY (AddressID) REFERENCES AddressTable(AddressID),
    CONSTRAINT FK_Orders_Status FOREIGN KEY (Status) REFERENCES OrderStatus(OrderStatusId) 
);


CREATE OR ALTER PROCEDURE usp_AddOrder
    @UserId INT,
    @CartId INT = NULL,
    @AddressID INT,
    @Quantity INT,
    @TotalPrice DECIMAL(10, 2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Check if Cart Item exists if CartId is provided
    IF @CartId IS NOT NULL
    BEGIN
        DECLARE @CartExists INT;
        SELECT @CartExists = COUNT(*) FROM Cart WHERE CartId = @CartId;
        
        IF @CartExists = 0
        BEGIN
            RAISERROR('Cart item does not exist', 16, 1);
            RETURN;
        END
    END

    -- 2. Check if the quantity is greater than or equal to the stock quantity
    DECLARE @StockQuantity INT;
    SELECT @StockQuantity = StockQuantity FROM Books WHERE BookId = (SELECT BookId FROM Cart WHERE CartId = @CartId);

    IF @Quantity > @StockQuantity
    BEGIN
        RAISERROR('Insufficient stock for the requested quantity', 16, 1);
        RETURN;
    END

    -- 3. Calculate total price based on quantity
    DECLARE @PricePerItem DECIMAL(10, 2);
    SELECT @PricePerItem = Price FROM Books WHERE BookId = (SELECT BookId FROM Cart WHERE CartId = @CartId);
    
    SET @TotalPrice = @Quantity * @PricePerItem;

    -- 4. Insert the order into the Orders table
    INSERT INTO Orders (UserId, CartId, AddressID, Quantity, TotalPrice, OrdateDate, Status)
    VALUES (@UserId, @CartId, @AddressID, @Quantity, @TotalPrice, GETDATE(), @Status);

    -- 5. Update book quantity in Books table
    UPDATE Books
    SET StockQuantity = StockQuantity - @Quantity
    WHERE BookId = (SELECT BookId FROM Cart WHERE CartId = @CartId);

    -- 6. Remove the item from the Cart (if CartId is provided)
    IF @CartId IS NOT NULL
    BEGIN
        DELETE FROM Cart WHERE CartId = @CartId;
    END

    -- 7. Return order details
    DECLARE @OrderId INT;
    SELECT @OrderId = SCOPE_IDENTITY();
    SELECT OrderId = @OrderId, UserId = @UserId, Quantity = @Quantity, TotalPrice = @TotalPrice, OrderDate = GETDATE();
END;


CREATE OR ALTER PROCEDURE usp_GetOrders
	@UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        o.OrderId,
        o.UserId,
        o.CartId,
        o.AddressID,
        o.Quantity,
        o.TotalPrice,
        o.OrdateDate,
        os.OrderStatus AS Status
    FROM Orders o
    JOIN OrderStatus os ON o.Status = os.OrderStatusId
	Where o.UserId=@UserId
    ORDER BY o.OrdateDate DESC;
END;

CREATE OR ALTER PROCEDURE usp_GetOrder
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        o.OrderId,
        o.UserId,
        o.CartId,
        o.AddressID,
        o.Quantity,
        o.TotalPrice,
        o.OrdateDate,
        os.OrderStatus AS Status
    FROM Orders o
    JOIN OrderStatus os ON o.Status = os.OrderStatusId
    WHERE o.OrderId = @OrderId;
END;


CREATE OR ALTER PROCEDURE usp_CancelOrder
    @OrderId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- 1. Check if the order exists
    DECLARE @OrderExists INT;
    SELECT @OrderExists = COUNT(*) FROM Orders WHERE OrderId = @OrderId;

    IF @OrderExists = 0
    BEGIN
        RAISERROR('Order does not exist', 16, 1);
        RETURN;
    END

    -- 2. Get current status of the order
    DECLARE @CurrentStatus INT;
    DECLARE @Quantity INT;
    DECLARE @BookId INT;

    SELECT 
        @CurrentStatus = Status,
        @Quantity = Quantity,
        @BookId = (SELECT BookId FROM Cart WHERE CartId = (SELECT CartId FROM Orders WHERE OrderId = @OrderId)) -- Get BookId from Cart (if CartId is available)
    FROM Orders WHERE OrderId = @OrderId;

    -- 3. Check if the order is already canceled
    IF @CurrentStatus = (SELECT OrderStatusId FROM OrderStatus WHERE OrderStatus = 'Canceled')
    BEGIN
        RAISERROR('Order is already canceled', 16, 1);
        RETURN;
    END

    -- 4. Update order status to 'Canceled'
    DECLARE @CanceledStatusId INT;
    SELECT @CanceledStatusId = OrderStatusId FROM OrderStatus WHERE OrderStatus = 'Canceled';

    UPDATE Orders
    SET Status = @CanceledStatusId
    WHERE OrderId = @OrderId;

    -- 5. Revert the stock quantity in Books table (if BookId is available)
    IF @BookId IS NOT NULL
    BEGIN
        UPDATE Books
        SET StockQuantity = StockQuantity + @Quantity
        WHERE BookId = @BookId;
    END

    -- 6. Return the updated order details after cancellation
    SELECT 
        o.OrderId,
        o.UserId,
        o.CartId,
        o.AddressID,
        o.Quantity,
        o.TotalPrice,
        o.OrdateDate,
        os.OrderStatus AS Status
    FROM Orders o
    JOIN OrderStatus os ON o.Status = os.OrderStatusId
    WHERE o.OrderId = @OrderId;
END;
