# Bookstore Application

The **Bookstore Application** is a web API developed using **ASP.NET Core 6** with **ADO.NET** for database operations. It provides functionalities to manage books, orders, feedback, carts, wish lists, users, and addresses.

---

## Server Details

### Technologies Used

- **ASP.NET Core 6**: Framework for building the Web API.
- **ADO.NET**: Used for database operations.
- **SQL Server**: Database for storing application data.
- **Swagger UI**: Integrated for API documentation and testing.
- **JWT Authentication**: Secure mechanism for user authentication.

---

## API Endpoints

### Books
- POST /api/Books: Add a new book.
- GET /api/Books: Retrieve all books.
- GET /api/Books/{bookId}: Get details of a specific book.
- PUT /api/Books/{bookId}: Update a book's details.
- DELETE /api/Books/{bookId}: Remove a book.

### Orders
- POST /api/Orders/{addressId}: Place a new order using an address.
- GET /api/Orders: Retrieve all orders for the authenticated user.
- GET /api/Orders/{orderId}: Get details of a specific order.
- PUT /api/Orders/{orderId}: Update the status of an order.

### Feedbacks
- POST /api/Feedbacks: Submit feedback for a book.
- GET /api/Feedbacks/{bookId}: Retrieve feedback for a specific book.

### Carts
- POST /api/Carts: Add an item to the cart.
- GET /api/Carts: Retrieve all items in the user's cart.
- DELETE /api/Carts/{cartId}: Remove an item from the cart.
- DELETE /api/Carts/ClearCart: Clear all items from the cart.

### Wish Lists
- POST /api/WishLists/{bookId}: Add a book to the wish list.
- GET /api/WishLists: Retrieve all items in the wish list.
- DELETE /api/WishLists/{wishListId}: Remove an item from the wish list.
- DELETE /api/WishLists/RemoveAll: Clear the wish list.

### Users
- POST /api/Users/register: Register a new user.
- POST /api/Users/login: Authenticate and log in a user.
- POST /api/Users/forgotPassword: Request a password reset.
- POST /api/Users/resetPassword: Reset the password.
- GET /api/Users: Retrieve user details.

### Addresses
- POST /api/Address: Add a new address.
- GET /api/Address: Retrieve all addresses for the user.
- GET /api/Address/{addressId}: Retrieve a specific address.
- PUT /api/Address: Update an address.
- DELETE /api/Address/{addressId}: Remove an address.

---

## Running the Server

1. Clone the repository: git clone [https://github.com/yourusername/bookstore-api.git](https://github.com/MDGHOUSE18/BookStore-WebApp.git)
2. Navigate to the project directory: cd BookStore-WebApp
3. Restore dependencies: dotnet restore
4. Update the database connection string in `appsettings.json`: { "ConnectionStrings": { "DefaultConnection": "Your-SQL-Server-Connection-String" } }
5. Run the application: dotnet run
6. Access Swagger UI for API testing: [https://localhost:7223/swagger/index.html](https://localhost:7223/swagger/index.html)


---

## Notes

- Ensure SQL Server is configured and accessible with the provided connection string.
- JWT authentication is required for accessing protected endpoints.

