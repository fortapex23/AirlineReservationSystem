using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.AirportDTOs;
using TravelProgram.Business.DTOs.BasketItemDTOs;
using TravelProgram.Business.Services.Implementations;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemController : ControllerBase
    {
        private readonly IBasketItemService _basketItemService;

        public BasketItemController(IBasketItemService basketItemService)
        {
            _basketItemService = basketItemService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(string appUserId, int seatId)
        {
            if (string.IsNullOrEmpty(appUserId))
            {
                return BadRequest("AppUserId is required.");
            }

            try
            {
                await _basketItemService.AddToBasketAsync(appUserId, seatId);
                return Ok("successfully added to basket");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("")]
        public IActionResult GetBasketItems(string appUserId)
        {
            if (string.IsNullOrEmpty(appUserId))
            {
                return BadRequest("AppUserId is required.");
            }

            return Ok(new ApiResponse<IQueryable<BasketItemDTO>>
            {
                Data = _basketItemService.GetBasketItems(appUserId),
                ErrorMessage = null,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromBasket(string appUserId, int seatId)
        {
            try
            {
                await _basketItemService.RemoveFromBasketAsync(appUserId, seatId);
                return Ok("Seat removed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed-{ex.Message}");
            }
        }
    }
}
