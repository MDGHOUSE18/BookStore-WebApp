using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class CartItemDTO
    {
        public int CartId { get; set; }
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public string ImageUrl { get; set; }
        public int CartQuantity { get; set; }
    }
}
