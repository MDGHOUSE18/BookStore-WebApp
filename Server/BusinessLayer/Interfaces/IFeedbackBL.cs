using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IFeedbackBL
    {
        Task<FeedbackDTO> AddFeedbackAsync(AddFeedbackDTO feedbackDTO, int userId);
        Task<List<FeedbackDTO>> GetFeedbacksAsync(int bookId);
    }
}
