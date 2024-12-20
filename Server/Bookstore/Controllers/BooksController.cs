using BusinessLayer.Interfaces;
using Common.DTO.Books;
using Common.DTO;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBooksBL _booksBL;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBooksBL booksBL, ILogger<BooksController> logger)
        {
            _booksBL = booksBL;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] AddBookDTO bookDTO)
        {
            //int adminId = int.Parse(User.FindFirst("UserId")?.Value);
            try
            {
                var result = await _booksBL.AddBookAsync(bookDTO);
                var response = new ResponseModel<BookDTO>
                {
                    Success = true,
                    Message = $"Book added successfully with name {bookDTO.Title}",
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book");
                return BadRequest(new ResponseModel<BookDTO> { Success = false, Message = "An error occurred while adding the book." });
            }
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            try
            {
                var result = await _booksBL.DeleteBookAsync(bookId);
                if (result)
                {
                    return Ok(new ResponseModel<bool> { Success = true,Message=$"Book deleted successfully with this BookId {bookId}" });
                }
                return NotFound(new ResponseModel<bool> { Success = false, Message = "Book not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting book");
                return BadRequest(new ResponseModel<bool> { Success = false, Message = "An error occurred while deleting the book." });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await _booksBL.GetAllBooksAsync();
                var response = new ResponseModel<List<BookDTO>>
                {
                    Success = true,
                    Message = "Books fetched successfully",
                    Data = books
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving books");
                return BadRequest(new ResponseModel<List<BookDTO>> { Success = false, Message = "An error occurred while retrieving books." });
            }
        }

        [AllowAnonymous]
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            try
            {
                var book = await _booksBL.GetBookByIdAsync(bookId);
                if (book != null)
                {
                    var response = new ResponseModel<BookDTO>
                    {
                        Success = true,
                        Message = "Book fetched successfully",
                        Data = book
                    };
                    return Ok(response);
                }
                return NotFound(new ResponseModel<BookDTO> { Success = false, Message = "Book not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving book");
                return BadRequest(new ResponseModel<BookDTO> { Success = false, Message = "An error occurred while retrieving the book." });
            }
        }

        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook(int bookId, [FromBody] AddBookDTO bookDTO)
        {
            //int adminId = int.Parse(User.FindFirst("UserId")?.Value);
            try
            {
                var result = await _booksBL.UpdateBookAsync(bookId, bookDTO);
                if (result != null)
                {
                    var response = new ResponseModel<BookDTO>
                    {
                        Success = true,
                        Message = "Book updated successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                return NotFound(new ResponseModel<BookDTO> { Success = false, Message = "Book not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating book");
                return BadRequest(new ResponseModel<BookDTO> { Success = false, Message = "An error occurred while updating the book." });
            }
        }
    }
}
