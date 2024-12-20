using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IWishListRepo
    {
        Task<bool> AddWishListAsync(int userId, int bookId);
        Task<bool> DeleteWishListItemAsync(int wishListId);
        Task<List<WishListItemDTO>> GetWishListAsync(int userId);
        Task<bool> RemoveAllWishListItemsAsync(int userId);
    }
}