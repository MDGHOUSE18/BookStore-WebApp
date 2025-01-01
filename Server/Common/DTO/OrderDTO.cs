using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? UpdatedAt { get; set; } 
        public string BookTitle { get; set; }
        public string Author { get; set; }
        public string Image { get; set; }
    }
}
