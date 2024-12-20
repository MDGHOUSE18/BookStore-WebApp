using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class AddCartDTO
    {
        //public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
