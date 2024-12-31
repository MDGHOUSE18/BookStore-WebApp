using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class UpdateCartItemDTO
    {
        //public int UserId { get; set; }
        public int BookId { get; set; }
        public int NewQuantity { get; set; }
    }

}
