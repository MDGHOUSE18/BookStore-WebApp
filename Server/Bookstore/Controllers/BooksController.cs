using BusinessLayer.Interfaces;
using Common.DTO;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<BookDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<BookDTO>))]
        public async Task<IActionResult> AddBook([FromForm] AddBookDTO bookDTO)
        {
            _logger.LogInformation("Starting AddBook operation for book: {Title}", bookDTO.Title);

            try
            {
                var result = await _booksBL.AddBookAsync(bookDTO);
                var response = new ResponseModel<BookDTO>
                {
                    Success = true,
                    Message = $"Book added successfully with name {bookDTO.Title}",
                    Data = result
                };
                _logger.LogInformation("Book added successfully: {Title}", bookDTO.Title);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding book: {Title}", bookDTO.Title);
                return BadRequest(new ResponseModel<BookDTO>
                {
                    Success = false,
                    Message = "An error occurred while adding the book."
                });
            }
        }

        [HttpDelete("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<bool>))]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            _logger.LogInformation("Attempting to delete book with ID: {BookId}", bookId);

            try
            {
                var result = await _booksBL.DeleteBookAsync(bookId);
                if (result)
                {
                    _logger.LogInformation("Book deleted successfully: {BookId}", bookId);
                    return Ok(new ResponseModel<bool>
                    {
                        Success = true,
                        Message = $"Book deleted successfully with ID {bookId}",
                        Data = true
                    });
                }
                _logger.LogWarning("Book not found with ID: {BookId}", bookId);
                return NotFound(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "Book not found"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting book: {BookId}", bookId);
                return BadRequest(new ResponseModel<bool>
                {
                    Success = false,
                    Message = "An error occurred while deleting the book."
                });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<List<BookDTO>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<List<BookDTO>>))]
        public async Task<IActionResult> GetAllBooks()
        {
            _logger.LogInformation("Fetching all books.");

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
                return BadRequest(new ResponseModel<List<BookDTO>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving books."
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<BookDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<BookDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<BookDTO>))]
        public async Task<IActionResult> GetBookById(int bookId)
        {
            _logger.LogInformation("Fetching book with ID: {BookId}", bookId);

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
                _logger.LogWarning("Book not found with ID: {BookId}", bookId);
                return NotFound(new ResponseModel<BookDTO>
                {
                    Success = false,
                    Message = "Book not found"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving book: {BookId}", bookId);
                return BadRequest(new ResponseModel<BookDTO>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the book."
                });
            }
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<BookDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<BookDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<BookDTO>))]        
        public async Task<IActionResult> UpdateBook(int bookId, [FromForm] AddBookDTO bookDTO)
        {
            _logger.LogInformation("Attempting to update book with ID: {BookId}", bookId);

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
                _logger.LogWarning("Book not found with ID: {BookId}", bookId);
                return NotFound(new ResponseModel<BookDTO>
                {
                    Success = false,
                    Message = "Book not found"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating book: {BookId}", bookId);
                return BadRequest(new ResponseModel<BookDTO>
                {
                    Success = false,
                    Message = "An error occurred while updating the book."
                });
            }
        }

        [HttpPut("/image")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> UpdateBookImage([FromForm]UpdateBookImageDto dto)
        {
            _logger.LogInformation("Attempting to update image for book with ID: {BookId}", dto.BookId);

            if (dto.Image == null || dto.Image.Length == 0)
            {
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "No file uploaded."
                });
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await dto.Image.CopyToAsync(memoryStream);
                    byte[] imageData = memoryStream.ToArray();

                    bool isUpdated = await _booksBL.UpdateBookImageAsync(dto.BookId, imageData);

                    if (isUpdated)
                    {
                        return Ok(new ResponseModel<string>
                        {
                            Success = true,
                            Message = "Image updated successfully."
                        });
                    }

                    _logger.LogWarning("Book not found with ID: {BookId}", dto.BookId);
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Book not found."
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating image for book: {BookId}", dto.BookId);
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while updating the image."
                });
            }
        }

    }

}
