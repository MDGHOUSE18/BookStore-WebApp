using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IAddressRepo
    {
        Task<AddressDTO> AddAddressAsync(AddAddressDTO address, int userId);
        Task<bool> DeleteAddressAsync(int addressId);
        Task<AddressDTO> GetAddressByIdAsync(int addressId);
        Task<List<AddressDTO>> GetAddressesByUserIdAsync(int userId);
        Task<AddressDTO> UpdateAddressAsync(UpdateAddressDTO address,int userId);
    }
}