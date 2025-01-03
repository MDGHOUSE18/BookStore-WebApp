using BusinessLayer.Interfaces;
using Common.DTO;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<OrderDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<Exception>))]
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
                return StatusCode(500, new ResponseModel<Exception>
                {
                    Success = false,
                    Message = "An error occurred while adding the order.",
                    Data = ex
                });
            }
        }

        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<OrderDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<OrderDTO>))]
        public async Task<IActionResult> CancelOrder([FromQuery] int statusId, int orderId)
        {
            try
            {
                _logger.LogInformation("Attempting to cancel order {OrderId}", orderId);
                var result = await _ordersBL.UpdateOrderStatusAsync(orderId, statusId);
                return Ok(new ResponseModel<OrderDTO> { Success = true, Message = "Order cancelled successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cancelling order {OrderId}", orderId);
                return StatusCode(500, new ResponseModel<OrderDTO>
                {
                    Success = false,
                    Message = "An error occurred while cancelling the order."
                });
            }
        }

        //[HttpGet("{orderId}")]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<OrderDTO>))]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<OrderDTO>))]
        //public async Task<IActionResult> GetOrder(int orderId)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Fetching details for order {OrderId}", orderId);
        //        var result = await _ordersBL.GetOrderAsync(orderId);
        //        return Ok(new ResponseModel<OrderDTO> { Success = true, Message = "Order fetched successfully", Data = result });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while fetching details for order {OrderId}", orderId);
        //        return StatusCode(500, new ResponseModel<OrderDTO>
        //        {
        //            Success = false,
        //            Message = "An error occurred while fetching the order details."
        //        });
        //    }
        //}

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<List<OrderDTO>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<List<OrderDTO>>))]
        public async Task<IActionResult> GetOrders()
        {
            int userId = int.Parse(User.FindFirst("UserId")?.Value);
            try
            {
                _logger.LogInformation("Fetching all orders for user {UserId}", userId);
                var result = await _ordersBL.GetOrdersAsync(userId);
                return Ok(new ResponseModel<List<OrderDTO>>
                {
                    Success = true,
                    Message = "Orders fetched successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching orders for user {UserId}", userId);
                return StatusCode(500, new ResponseModel<List<OrderDTO>>
                {
                    Success = false,
                    Message = "An error occurred while fetching the orders."
                });
            }
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<List<OrderDetailDTO>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<List<OrderDetailDTO>>))]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            try
            {
                _logger.LogInformation("Fetching details for order {OrderId}", orderId);
                var result = await _ordersBL.GetOrderDetailsAsync(orderId);
                return Ok(new ResponseModel<OrderDetailDTO> { Success = true, Message = "Order details fetched successfully", Data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching order details for order {OrderId}", orderId);
                return StatusCode(500, new ResponseModel<List<OrderDetailDTO>>
                {
                    Success = false,
                    Message = "An error occurred while fetching the order details."
                });
            }
        }
    }
}
