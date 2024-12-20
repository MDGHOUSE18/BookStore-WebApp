using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IWishListBL
    {
        Task<bool> AddWishListAsync(int userId, int bookId);
        Task<bool> DeleteWishListItemAsync(int wishListId);
        Task<List<WishListItemDTO>> GetWishListAsync(int userId);
        Task<bool> RemoveAllWishListItemsAsync(int userId);
    }
}
