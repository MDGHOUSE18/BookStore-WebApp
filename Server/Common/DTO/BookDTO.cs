using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DTO
{
    public class BookDTO
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string Image { get; set; }
        public int StockQuantity { get; set; }
        public int AdminUserID { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
