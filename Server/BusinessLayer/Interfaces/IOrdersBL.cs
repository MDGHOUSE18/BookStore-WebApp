using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IOrdersBL
    {
        Task<OrderDTO> AddOrderAsync(int userId, int addressId);
        Task<OrderDTO> GetOrderAsync(int orderId);
        Task<OrderDetailDTO> GetOrderDetailsAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersAsync(int userId);
        Task<OrderDTO> UpdateOrderStatusAsync(int orderId, int statusId);
    }
}
