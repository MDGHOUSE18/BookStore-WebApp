using BusinessLayer.Interfaces;
using Common.DTO;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class FeedbackBL : IFeedbackBL
    {
        private readonly IFeedbacksRepo _feedbacksRepo;
        public FeedbackBL(IFeedbacksRepo feedbacksRepo)
        {
            this._feedbacksRepo = feedbacksRepo;
        }
        public Task<FeedbackDTO> AddFeedbackAsync(AddFeedbackDTO feedbackDTO, int userId)
        {
            return _feedbacksRepo.AddFeedbackAsync(feedbackDTO, userId);
        }

        public Task<List<FeedbackDTO>> GetFeedbacksAsync(int bookId)
        {
            return _feedbacksRepo.GetFeedbacksAsync(bookId);
        }
    }
}
