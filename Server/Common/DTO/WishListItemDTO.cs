using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class WishListItemDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountedPrice { get; set; }
        public string ImageUrl { get; set; }
        public int StockQuantity { get; set; }

    }
}
