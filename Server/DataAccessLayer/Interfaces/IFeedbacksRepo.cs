using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IFeedbacksRepo
    {
        Task<FeedbackDTO> AddFeedbackAsync(AddFeedbackDTO feedbackDTO, int userId);
        Task<List<FeedbackDTO>> GetFeedbacksAsync(int bookId);
    }
}