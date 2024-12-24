using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class FeedbackDTO
    {
        public int FeedbackId { get; set; }
        //public int UserId { get; set; }
        public int BookId { get; set; }
        public Decimal Rating { get; set; }
        public string Comments { get; set; }
    }
}
