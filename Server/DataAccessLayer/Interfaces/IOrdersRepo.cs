using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IOrdersRepo
    {
        Task<OrderDTO> AddOrderAsync(int userId, int addressId);
        Task<OrderDTO> UpdateOrderStatusAsync(int orderId, int statusId);
        Task<OrderDTO> GetOrderAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersAsync(int userId);
    }
}