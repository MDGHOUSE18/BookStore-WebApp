use Bookstore;

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


CREATE OR ALTER PROCEDURE usp_UpdateBook
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
    SELECT * 
    FROM Books
    ORDER BY BookId;
END;


CREATE OR ALTER PROCEDURE usp_GetBookById
    @BookID INT
AS
BEGIN
    SELECT * FROM Books
    WHERE BookID = @BookID;
END;

INSERT INTO Books (Title, Author, Description, Price, DiscountedPrice, StockQuantity)
VALUES
    ('The Great Gatsby', 'F. Scott Fitzgerald', 'A novel set in the Roaring Twenties, exploring themes of wealth and social change.', 10.99, 8.99, 100),
    ('To Kill a Mockingbird', 'Harper Lee', 'A story of racial injustice in the Deep South, seen through the eyes of a young girl.', 12.99, 10.99, 150),
    ('1984', 'George Orwell', 'A dystopian novel depicting a totalitarian regime that uses surveillance and propaganda to control its citizens.', 14.99, 12.99, 200),
    ('Pride and Prejudice', 'Jane Austen', 'A classic romance novel that critiques the societal norms of early 19th-century England.', 9.99, 7.99, 120),
    ('The Catcher in the Rye', 'J.D. Salinger', 'A narrative about a disenchanted teenager navigating life in New York City.', 11.99, 9.99, 130),
    ('Moby-Dick', 'Herman Melville', 'An epic tale of obsession and revenge centered around the hunt for a legendary whale.', 15.99, 13.99, 80),
    ('War and Peace', 'Leo Tolstoy', 'A historical novel that intertwines the lives of several aristocratic families during the Napoleonic Wars.', 19.99, 17.99, 60),
    ('The Odyssey', 'Homer', 'An ancient Greek epic poem detailing the adventures of Odysseus as he returns home from the Trojan War.', 13.99, 11.99, 90),
    ('Crime and Punishment', 'Fyodor Dostoevsky', 'A psychological drama exploring the moral dilemmas of a young law student who commits a crime.', 14.99, 12.99, 110),
    ('The Hobbit', 'J.R.R. Tolkien', 'A fantasy novel about a hobbit’s unexpected journey to reclaim a stolen treasure.', 12.99, 10.99, 140),
    ('Brave New World', 'Aldous Huxley', 'A dystopian novel depicting a technologically advanced society that sacrifices individuality for stability.', 13.99, 11.99, 160),
    ('The Catcher in the Rye', 'J.D. Salinger', 'A narrative about a disenchanted teenager navigating life in New York City.', 11.99, 9.99, 130),
    ('The Lord of the Rings', 'J.R.R. Tolkien', 'An epic fantasy trilogy chronicling the quest to destroy a powerful ring that could bring about the end of the world.', 29.99, 25.99, 50),
    ('The Chronicles of Narnia', 'C.S. Lewis', 'A series of seven fantasy novels set in the magical land of Narnia.', 24.99, 20.99, 70),
    ('The Alchemist', 'Paulo Coelho', 'A philosophical novel about a shepherd’s journey to find his personal legend.', 16.99, 14.99, 110),
    ('The Da Vinci Code', 'Dan Brown', 'A mystery thriller that follows a symbologist uncovering secrets hidden in famous artworks.', 18.99, 16.99, 130),
    ('The Hunger Games', 'Suzanne Collins', 'A dystopian novel set in a future where children are forced to participate in a deadly televised competition.', 14.99, 12.99, 150),
    ('The Fault in Our Stars', 'John Green', 'A touching story of two teenagers who fall in love after meeting at a cancer support group.', 12.99, 10.99, 180),
    ('The Girl on the Train', 'Paula Hawkins', 'A psychological thriller about a woman who becomes entangled in a missing person case.', 15.99, 13.99, 140),
    ('Gone with the Wind', 'Margaret Mitchell', 'A historical novel set during the American Civil War, focusing on the life of Scarlett O’Hara.', 19.99, 17.99, 60),
    ('The Shining', 'Stephen King', 'A horror novel about a family staying in an isolated hotel during the winter, where the father succumbs to supernatural forces.', 14.99, 12.99, 100),
    ('The Road', 'Cormac McCarthy', 'A post-apocalyptic novel following a father and son’s journey through a desolate America.', 16.99, 14.99, 80),
    ('The Book Thief', 'Markus Zusak', 'A story narrated by Death, about a young girl living in Nazi Germany who steals books to share with others.', 13.99, 11.99, 120),
    ('The Kite Runner', 'Khaled Hosseini', 'A tale of friendship and redemption set against the backdrop of Afghanistan’s turbulent history.', 14.99, 12.99, 150),
    ('The Help', 'Kathryn Stockett', 'A novel about African American maids working in white households in 1960s Mississippi.', 16.99, 14.99, 130),
    ('The Secret Garden', 'Frances Hodgson Burnett', 'A children’s novel about a young girl who discovers a hidden, neglected garden and brings it back to life.', 9.99, 7.99, 200),
    ('Little Women', 'Louisa May Alcott', 'A classic novel following the lives of four sisters growing up during the American Civil War.', 12.99, 10.99, 180),
    ('The Outsiders', 'S.E. Hinton', 'A story about the struggles between two rival groups of teenagers in 1960s America.', 10.99, 8.99, 220),
    ('The Giver', 'Lois Lowry', 'A dystopian novel about a society that has eliminated all pain and strife by converting to "Sameness".', 11.99, 9.99, 150),
    ('The Maze Runner', 'James Dashner', 'A science fiction novel about a group of teenagers trapped in a maze with no memory of the outside world.', 13.99, 11.99, 130),
    ('Divergent', 'Veronica Roth', 'A dystopian novel set in a society divided into five factions, each representing a different value.', 14.99, 12.99, 120),
    ('The Perks of Being a Wallflower', 'Stephen Chbosky', 'A coming-of-age novel about a shy high school freshman navigating life and love.', 12.99, 10.99, 140),
    ('Looking for Alaska', 'John Green', 'A novel about a teenager’s search for meaning and understanding after a life-changing event.', 13.99, 11.99, 110) 

