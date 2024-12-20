
using BusinessLayer.Interfaces;
using Common;
using Common.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Bookstore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishListsController : ControllerBase
    {
        private readonly IWishListBL _wishListBL;
        private readonly ILogger<WishListsController> _logger;

        public WishListsController(IWishListBL wishListBL, ILogger<WishListsController> logger)
        {
            _wishListBL = wishListBL;
            _logger = logger;
        }

        [HttpPost("{bookId}")]
        public async Task<IActionResult> AddWishListItem(int bookId)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation("Adding a new item to the wishlist for UserId: {UserId}, BookId: {BookId}", userId, bookId);

            try
            {
                var result = await _wishListBL.AddWishListAsync(userId, bookId);
                return Ok(new ResponseModel<bool>
                {
                    Success = result,
                    Message = result ? "Item added to wishlist successfully." : "Failed to add item to wishlist.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding an item to the wishlist.");
                return StatusCode(500, new ResponseModel<bool>
                {
                    Success = false,
                    Message = "An error occurred while adding the item to the wishlist.",
                    Data = false
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation("Fetching wishlist for UserId: {UserId}", userId);

            try
            {
                var wishList = await _wishListBL.GetWishListAsync(userId);
                return Ok(new ResponseModel<List<WishListItemDTO>>
                {
                    Success = true,
                    Message = "Wishlist retrieved successfully.",
                    Data = wishList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching the wishlist.");
                return StatusCode(500, new ResponseModel<List<WishListItemDTO>>
                {
                    Success = false,
                    Message = "An error occurred while fetching the wishlist.",
                    Data = null
                });
            }
        }

        [HttpDelete("{wishListId}")]
        public async Task<IActionResult> DeleteWishListItem(int wishListId)
        {
            _logger.LogInformation("Deleting wishlist item with WishListId: {WishListId}", wishListId);

            try
            {
                var result = await _wishListBL.DeleteWishListItemAsync(wishListId);
                return Ok(new ResponseModel<bool>
                {
                    Success = result,
                    Message = result ? "Item deleted from wishlist successfully." : "Failed to delete item from wishlist.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the wishlist item.");
                return StatusCode(500, new ResponseModel<bool>
                {
                    Success = false,
                    Message = "An error occurred while deleting the item from the wishlist.",
                    Data = false
                });
            }
        }

        [HttpDelete("RemoveAll")]
        public async Task<IActionResult> RemoveAllWishListItems()
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation("Removing all wishlist items for UserId: {UserId}", userId);

            try
            {
                var result = await _wishListBL.RemoveAllWishListItemsAsync(userId);
                return Ok(new ResponseModel<bool>
                {
                    Success = result,
                    Message = result ? "All items removed from wishlist successfully." : "Failed to remove items from wishlist.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing all wishlist items.");
                return StatusCode(500, new ResponseModel<bool>
                {
                    Success = false,
                    Message = "An error occurred while removing all items from the wishlist.",
                    Data = false
                });
            }
        }
    }
}
