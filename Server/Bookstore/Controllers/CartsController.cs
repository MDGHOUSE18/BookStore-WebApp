using BusinessLayer.Interfaces;
using Common.DTO;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartsController : ControllerBase
    {
        private readonly ICartBL _cartBL;
        private readonly ILogger<CartsController> _logger;

        public CartsController(ICartBL cartBL, ILogger<CartsController> logger)
        {
            _cartBL = cartBL;
            _logger = logger;
        }

        // Add an item to the cart
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<CartItemDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<CartItemDTO>))]
        public async Task<IActionResult> AddCartItem([FromBody] AddCartDTO cart)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation($"Attempting to add an item to the cart for UserId {userId}");

            var result = await _cartBL.AddCartItemAsync(cart, userId);
            if (result != null)
            {
                return Ok(new ResponseModel<CartItemDTO>
                {
                    Success = true,
                    Message = "Item added to cart successfully.",
                    Data = result
                });
            }
            return BadRequest(new ResponseModel<CartItemDTO>
            {
                Success = false,
                Message = "Failed to add item to the cart.",
                Data = result
            });
        }

        // Get cart items
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<List<CartItemDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<CartItemDTO>))]
        public async Task<IActionResult> GetCartItems()
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation("Fetching cart items for UserId {UserId}", userId);

            var cartItems = await _cartBL.GetCartItemsAsync(userId);
            if (cartItems != null)
            {
                return Ok(new ResponseModel<List<CartItemDTO>>
                {
                    Success = true,
                    Message = "Cart items fetched successfully.",
                    Data = cartItems
                });
            }
            return NotFound(new ResponseModel<CartItemDTO>
            {
                Success = false,
                Message = "No items found in the cart.",
                Data = null
            });
        }

        // Delete a specific cart item
        [HttpDelete("{cartId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<bool>))]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            _logger.LogInformation("Attempting to delete cart item with CartId {CartId}", cartId);

            var result = await _cartBL.DeleteCartAsync(cartId);
            if (result)
            {
                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Message = "Cart item deleted successfully.",
                    Data = result
                });
            }
            return BadRequest(new ResponseModel<bool>
            {
                Success = false,
                Message = "Failed to delete cart item.",
                Data = result
            });
        }

        // Remove all items from the cart
        [HttpDelete("ClearCart")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<bool>))]
        public async Task<IActionResult> RemoveAllCartItems()
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation("Attempting to clear the cart for UserId {UserId}", userId);

            var result = await _cartBL.RemoveAllCartItemsAsync(userId);
            if (result)
            {
                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Message = "Cart cleared successfully.",
                    Data = result
                });
            }
            return BadRequest(new ResponseModel<bool>
            {
                Success = false,
                Message = "Failed to clear the cart.",
                Data = result
            });
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<bool>))]
        public async Task<IActionResult> UpdateCartItemQuantity([FromBody] UpdateCartItemDTO dto)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation("Attempting to update cart item for UserId {UserId}, BookId {BookId} with new quantity {NewQuantity}", userId, dto.BookId, dto.NewQuantity);
            
            var result = await _cartBL.UpdateCartItemQuantityAsync(dto,userId);
            if (result)
            {
                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Message = "Cart item updated successfully.",
                    Data = result
                });
            }
            return BadRequest(new ResponseModel<bool>
            {
                Success = false,
                Message = "Failed to update the cart item.",
                Data = result
            });
        }

    }

}
