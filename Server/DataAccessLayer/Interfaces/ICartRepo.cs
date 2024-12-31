using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface ICartRepo
    {
        Task<CartItemDTO> AddCartItemAsync(AddCartDTO cart,int userId);
        Task<bool> DeleteCartAsync(int cartId);
        Task<List<CartItemDTO>> GetCartItemsAsync(int userId);
        Task<bool> RemoveAllCartItemsAsync(int userId);
        Task<bool> UpdateCartItemQuantityAsync(UpdateCartItemDTO dto,int userId);
    }
}