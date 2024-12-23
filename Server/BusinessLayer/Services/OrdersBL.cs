using BusinessLayer.Interfaces;
using Common.DTO;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class OrdersBL : IOrdersBL
    {
        private readonly IOrdersRepo _ordersRepo;

        public OrdersBL(IOrdersRepo ordersRepo)
        {
             this._ordersRepo = ordersRepo;
        }
        public Task<OrderDTO> AddOrderAsync(int userId, int addressId)
        {
            return _ordersRepo.AddOrderAsync(userId, addressId);
        }

        public Task<OrderDTO> UpdateOrderAsync(int orderId, int statusId)
        {
            return _ordersRepo.UpdateOrderStatusAsync(orderId,statusId);   
        }

        public Task<OrderDTO> GetOrderAsync(int orderId)
        {
            return _ordersRepo.GetOrderAsync(orderId);
        }

        public Task<List<OrderDTO>> GetOrdersAsync(int userId)
        {
            return _ordersRepo.GetOrdersAsync(userId);
        }
    }
}
