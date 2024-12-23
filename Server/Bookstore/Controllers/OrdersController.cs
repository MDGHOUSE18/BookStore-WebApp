using BusinessLayer.Interfaces;
using Common.DTO;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersBL _ordersBL;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrdersBL ordersBL, ILogger<OrdersController> logger)
        {
            _ordersBL = ordersBL;
            _logger = logger;
        }

        [HttpPost("{addressId}")]
        public async Task<IActionResult> AddOrder(int addressId)
        {

            int userId = int.Parse(User.FindFirst("UserId")?.Value);
            try
            {
                _logger.LogInformation("Attempting to add order for user {UserId}", userId);
                var result = await _ordersBL.AddOrderAsync(userId, addressId);
                return Ok(new ResponseModel<OrderDTO> { Success = true, Message = "Order added successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding order for user {UserId}", userId);
                return StatusCode(500, new ResponseModel<System.Exception> { Success = false, Message = "An error occurred while adding the order." ,Data=ex});
            }
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> CancelOrder([FromQuery] int statusId,int orderId)
        {
            try
            {
                _logger.LogInformation("Attempting to cancel order {OrderId}", orderId);
                var result = await _ordersBL.UpdateOrderAsync(orderId, statusId);
                return Ok(new ResponseModel<OrderDTO> { Success = true, Message = "Order cancelled successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cancelling order {OrderId}", orderId);
                return StatusCode(500, new ResponseModel<OrderDTO> { Success = false, Message = "An error occurred while cancelling the order." });
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                _logger.LogInformation("Fetching details for order {OrderId}", orderId);
                var result = await _ordersBL.GetOrderAsync(orderId);
                return Ok(new ResponseModel<OrderDTO> { Success = true, Message = "Order fetched successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching details for order {OrderId}", orderId);
                return StatusCode(500, new ResponseModel<OrderDTO> { Success = false, Message = "An error occurred while fetching the order details." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {

            int userId = int.Parse(User.FindFirst("UserId")?.Value);
            try
            {
                _logger.LogInformation("Fetching all orders for user {UserId}", userId);
                var result = await _ordersBL.GetOrdersAsync(userId);
                return Ok(new ResponseModel<List<OrderDTO>> { Success = true, Message = "Orders fetched successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching orders for user {UserId}", userId);
                return StatusCode(500, new ResponseModel<List<OrderDTO>> { Success = false, Message = "An error occurred while fetching the orders." });
            }
        }

    }
}
