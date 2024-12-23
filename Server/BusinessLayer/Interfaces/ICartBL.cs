using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICartBL
    {
        Task<CartItemDTO> AddCartItemAsync(AddCartDTO cart,int userId);
        Task<bool> DeleteCartAsync(int cartId);
        Task<List<CartItemDTO>> GetCartItemsAsync(int userId);
        Task<bool> RemoveAllCartItemsAsync(int userId);
    }
}
