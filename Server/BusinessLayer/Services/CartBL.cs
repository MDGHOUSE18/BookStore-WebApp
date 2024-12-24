using BusinessLayer.Interfaces;
using Common.DTO;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CartBL : ICartBL
    {
        private readonly ICartRepo _cardRepo;

        public CartBL(ICartRepo cartRepo)
        {
            this._cardRepo = cartRepo;
        }
        public async Task<CartItemDTO> AddCartItemAsync(AddCartDTO cart,int userId)
        {
            return await _cardRepo.AddCartItemAsync(cart, userId);
        }

        public async Task<bool> DeleteCartAsync(int cartId)
        {
            return await _cardRepo.DeleteCartAsync(cartId);
        }

        public async Task<List<CartItemDTO>> GetCartItemsAsync(int userId)
        {
            return await _cardRepo.GetCartItemsAsync(userId);
        }

        public async Task<bool> RemoveAllCartItemsAsync(int userId)
        {
            return await _cardRepo.RemoveAllCartItemsAsync(userId);
        }
    }
}
