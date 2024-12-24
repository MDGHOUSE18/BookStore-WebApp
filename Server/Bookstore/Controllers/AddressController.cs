using BusinessLayer.Interfaces;
using Common;
using Common.DTO;
using Common.Modals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressBL _addressBL;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressBL addressBL, ILogger<AddressController> logger)
        {
            _addressBL = addressBL;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<AddressDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> AddAddress(AddAddressDTO address)
        {
            _logger.LogInformation("Starting AddAddress operation.");
            var userId = int.Parse(User.FindFirst("UserId").Value);
            try
            {
                var addedAddress = await _addressBL.AddAddressAsync(address, userId);
                if (addedAddress != null)
                {
                    _logger.LogInformation("Address added successfully.");
                    return Ok(new ResponseModel<AddressDTO> { Success = true, Message = "Address added successfully.", Data = addedAddress });
                }

                _logger.LogWarning("Failed to add the address.");
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to add the address." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding address.");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal server error." });
            }
        }

        [HttpGet("{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<AddressDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> GetAddressById(int addressId)
        {
            _logger.LogInformation($"Fetching address with ID: {addressId}");
            try
            {
                var address = await _addressBL.GetAddressByIdAsync(addressId);
                if (address != null)
                {
                    _logger.LogInformation("Address fetched successfully.");
                    return Ok(new ResponseModel<AddressDTO> { Success = true, Message = "Address fetched successfully.", Data = address });
                }

                _logger.LogWarning($"Address with ID: {addressId} not found.");
                return NotFound(new ResponseModel<string> { Success = false, Message = "Address not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching address with ID: {addressId}");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal server error." });
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<AddressDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> UpdateAddress(UpdateAddressDTO address)
        {
            _logger.LogInformation("Starting UpdateAddress operation.");
            var userId = int.Parse(User.FindFirst("UserId").Value);
            try
            {
                var updatedAddress = await _addressBL.UpdateAddressAsync(address, userId);
                if (updatedAddress != null)
                {
                    _logger.LogInformation("Address updated successfully.");
                    return Ok(new ResponseModel<AddressDTO> { Success = true, Message = "Address updated successfully.", Data = updatedAddress });
                }

                _logger.LogWarning("Failed to update the address.");
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to update the address." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating address.");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal server error." });
            }
        }

        [HttpDelete("{addressId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<AddressDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            _logger.LogInformation($"Starting DeleteAddress operation for ID: {addressId}");
            var userId = int.Parse(User.FindFirst("UserId").Value);
            try
            {
                var deletedAddress = await _addressBL.DeleteAddressAsync(addressId);
                if (deletedAddress)
                {
                    _logger.LogInformation("Address deleted successfully.");
                    return Ok(new ResponseModel<AddressDTO> { Success = true, Message = "Address deleted successfully." });
                }

                _logger.LogWarning($"Failed to delete address with ID: {addressId}");
                return BadRequest(new ResponseModel<string> { Success = false, Message = "Failed to delete address." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting address with ID: {addressId}");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal server error." });
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseModel<List<AddressDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseModel<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseModel<string>))]
        public async Task<IActionResult> GetAddressesByUserId()
        {
            var userId = int.Parse(User.FindFirst("UserId").Value);
            _logger.LogInformation($"Fetching addresses for user ID: {userId}");
            try
            {
                var addresses = await _addressBL.GetAddressesByUserIdAsync(userId);
                if (addresses != null && addresses.Count > 0)
                {
                    _logger.LogInformation("Addresses fetched successfully.");
                    return Ok(new ResponseModel<List<AddressDTO>> { Success = true, Message = "Addresses fetched successfully.", Data = addresses });
                }

                _logger.LogWarning($"No addresses found for user ID: {userId}");
                return NotFound(new ResponseModel<string> { Success = false, Message = "No addresses found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching addresses for user ID: {userId}");
                return StatusCode(500, new ResponseModel<string> { Success = false, Message = "Internal server error." });
            }
        }
    }

}
