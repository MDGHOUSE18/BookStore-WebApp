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
        [HttpPost("{bookId}")]
        public async Task<IActionResult> AddCartItem([FromBody] int quantity, int bookId)
        {
            AddCartDTO cart = new AddCartDTO
            {
                Quantity = quantity,
                BookId = bookId
            };
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation($"Attempting to add an item to the cart for UserId {userId}");

            var result = await _cartBL.AddCartItemAsync(cart,userId);
            if (result)
            {
                return Ok(new ResponseModel<bool>
                {
                    Success = true,
                    Message = "Item added to cart successfully.",
                    Data = result
                });
            }
            return BadRequest(new ResponseModel<bool>
            {
                Success = false,
                Message = "Failed to add item to the cart.",
                Data = result
            });
        }

        // Get cart items
        [HttpGet]
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
    }
}
