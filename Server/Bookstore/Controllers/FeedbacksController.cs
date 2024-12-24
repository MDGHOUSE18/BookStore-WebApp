using BusinessLayer.Interfaces;
using Common.DTO;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackBL _feedbackBL;
        private readonly ILogger<FeedbacksController> _logger;

        public FeedbacksController(IFeedbackBL feedbackBL, ILogger<FeedbacksController> logger)
        {
            _feedbackBL = feedbackBL;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(ResponseModel<FeedbackDTO>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 400)]
        public async Task<IActionResult> AddFeedback([FromBody] AddFeedbackDTO feedbackDTO)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            try
            {
                _logger.LogInformation("Received feedback submission request from user {UserId} for book {BookId}.", userId, feedbackDTO.BookId);

                var result = await _feedbackBL.AddFeedbackAsync(feedbackDTO, userId);

                if (result == null)
                {
                    _logger.LogWarning("Feedback already exists for user {UserId} and book {BookId}.", userId, feedbackDTO.BookId);
                    return BadRequest(new ResponseModel<string> { Success = false, Message = "Feedback already exists for this book." });
                }

                _logger.LogInformation("Feedback added successfully for user {UserId} and book {BookId}.", userId, feedbackDTO.BookId);

                return Ok(new ResponseModel<FeedbackDTO> { Success = true, Message = "Feedback added successfully!", Data = result });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding feedback for user {UserId} and book {BookId}.", userId, feedbackDTO.BookId);
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "An unexpected error occurred." });
            }
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(ResponseModel<List<FeedbackDTO>>), 200)]
        [ProducesResponseType(typeof(ResponseModel<string>), 404)]
        public async Task<IActionResult> GetFeedbacksForBook(int bookId)
        {
            try
            {
                _logger.LogInformation("Fetching feedbacks for book {BookId}.", bookId);

                var feedbacks = await _feedbackBL.GetFeedbacksAsync(bookId);

                if (feedbacks == null || feedbacks.Count == 0)
                {
                    _logger.LogWarning("No feedbacks found for book {BookId}.", bookId);
                    return NotFound(new ResponseModel<string> { Success = false, Message = "No feedbacks found for this book." });
                }

                _logger.LogInformation("Successfully retrieved feedbacks for book {BookId}.", bookId);

                return Ok(new ResponseModel<List<FeedbackDTO>> { Success = true, Message = "Feedbacks fetched successfully.", Data = feedbacks });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching feedbacks for book {BookId}.", bookId);
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "An unexpected error occurred." });
            }
        }
    }
}
