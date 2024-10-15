using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> AddToBasket(string appUserId, int flightId)
        {
            if (string.IsNullOrEmpty(appUserId))
            {
                return BadRequest("AppUserId is required.");
            }

            try
            {
                await _basketItemService.AddToBasketAsync(appUserId, flightId);
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

            var basketItems = _basketItemService.GetBasketItems(appUserId);

            return Ok(basketItems);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveFromBasket(string appUserId, int flightId)
        {
            try
            {
                await _basketItemService.RemoveFromBasketAsync(appUserId, flightId);
                return Ok("Flight removed from basket successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to remove flight from the basket: {ex.Message}");
            }
        }
    }
}
