using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;

namespace TravelProgram.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FlightsController : ControllerBase
	{
		private readonly IFlightService _flightService;

        public FlightsController(IFlightService FlightService)
		{
			_flightService = FlightService;
        }

		[HttpGet("")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(new ApiResponse<ICollection<FlightGetDto>>
			{
				Data = await _flightService.GetByExpression(true, null, "Bookings", "Seats"),
				ErrorMessage = null,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] FlightCreateDto dto) //, IFormFile? imageFile
        {
			FlightGetDto flight = null;
			try
			{
                //string rootPath = _env.WebRootPath;

                flight = await _flightService.CreateAsync(dto);
            }
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<object>
				{
					StatusCode = StatusCodes.Status400BadRequest,
					ErrorMessage = ex.Message,
					Data = null
				});
			}

			return Created();
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			FlightGetDto dto = null;
			try
			{
				dto = await _flightService.GetSingleByExpression(true, x=>x.Id == id, "Bookings", "Seats");
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(new ApiResponse<FlightGetDto>
			{
				Data = dto,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, FlightUpdateDto dto)
		{

			try
			{
				await _flightService.UpdateAsync(id, dto);
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<FlightUpdateDto>
				{
					StatusCode = StatusCodes.Status400BadRequest,
					ErrorMessage = ex.Message,
					Data = null
				});
			}
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await _flightService.DeleteAsync(id);
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<object>
				{
					StatusCode = StatusCodes.Status400BadRequest,
					ErrorMessage = ex.Message,
					Data = null
				});
			}
			return Ok();
		}
	}
}
