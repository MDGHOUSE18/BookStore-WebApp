using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class AddFeedbackDTO
    {
        public int BookId { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public decimal Rating { get; set; }
        [MaxLength(1000, ErrorMessage = "Comments cannot exceed 1000 characters.")]
        public string Comments { get; set; }

        public bool ValidateComments()
        {
            return Comments.Split(' ').Length <= 100;  
        }
    }
}
