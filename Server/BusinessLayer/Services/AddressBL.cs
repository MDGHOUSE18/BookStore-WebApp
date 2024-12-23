using BusinessLayer.Interfaces;
using Common.DTO;
using Common.Modals;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AddressBL : IAddressBL
    {
        private readonly IAddressRepo _addressRepo;

        public AddressBL(IAddressRepo addressRepo)
        {
            this._addressRepo = addressRepo;
        }

        public Task<AddressDTO> AddAddressAsync(AddAddressDTO address,int userId)
        {
            return _addressRepo.AddAddressAsync(address, userId);
        }

        public Task<bool> DeleteAddressAsync(int addressId)
        {
            return _addressRepo.DeleteAddressAsync(addressId);
        }

        public Task<AddressDTO> GetAddressByIdAsync(int addressId)
        {
            return _addressRepo.GetAddressByIdAsync(addressId);
        }

        public Task<List<AddressDTO>> GetAddressesByUserIdAsync(int userId)
        {
            return _addressRepo.GetAddressesByUserIdAsync(userId);
        }

        public Task<AddressDTO> UpdateAddressAsync(UpdateAddressDTO address,int userId)
        {
            return _addressRepo.UpdateAddressAsync(address,userId);
        }
    }
}
