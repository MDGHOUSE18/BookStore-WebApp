using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class AddOrderDTO
    {
        public int CartId { get; set; }
        public int AddressId { get; set; }
    }
}
