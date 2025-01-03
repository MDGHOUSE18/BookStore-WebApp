using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IOrdersRepo
    {
        Task<OrderDTO> AddOrderAsync(int userId, int addressId);
        Task<OrderDTO> GetOrderAsync(int orderId);
        Task<OrderDetailDTO> GetOrderDetailsAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersAsync(int userId);
        Task<OrderDTO> UpdateOrderStatusAsync(int orderId, int statusId);
    }
}