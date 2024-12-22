using BusinessLayer.Interfaces;
using Common.DTO;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class WishListBL : IWishListBL
    {
        private readonly IWishListRepo _wishListRepo;

        public WishListBL(IWishListRepo wishListRepo)
        {
            this._wishListRepo = wishListRepo;
        }
        public Task<bool> AddWishListAsync(int userId, int bookId)
        {
            return _wishListRepo.AddWishListAsync(userId,bookId);
        }

        public Task<bool> DeleteWishListItemAsync(int wishListId)
        {
            return _wishListRepo.DeleteWishListItemAsync(wishListId);
        }

        public Task<List<WishListItemDTO>> GetWishListAsync(int userId)
        {
            return _wishListRepo.GetWishListAsync(userId);
        }

        public Task<bool> RemoveAllWishListItemsAsync(int userId)
        {
            return _wishListRepo.RemoveAllWishListItemsAsync(userId);
        }
    }
}
