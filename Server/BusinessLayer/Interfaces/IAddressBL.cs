using Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAddressBL
    {
        Task<AddressDTO> AddAddressAsync(AddAddressDTO address, int userId);
        Task<bool> DeleteAddressAsync(int addressId);
        Task<AddressDTO> GetAddressByIdAsync(int addressId);
        Task<List<AddressDTO>> GetAddressesByUserIdAsync(int userId);
        Task<AddressDTO> UpdateAddressAsync(UpdateAddressDTO address,int userId);
    }
}
