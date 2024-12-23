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
        Task<OrderDTO> UpdateOrderAsync(int orderId, int statusId);
        Task<OrderDTO> GetOrderAsync(int orderId);
        Task<List<OrderDTO>> GetOrdersAsync(int userId);
    }
}
