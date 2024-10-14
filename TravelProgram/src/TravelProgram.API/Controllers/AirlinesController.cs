using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.AirlineDTOs;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AirlinesController : ControllerBase
	{
		private readonly IAirlineService _AirlineService;

		public AirlinesController(IAirlineService AirlineService)
		{
			_AirlineService = AirlineService;
		}

		[HttpGet("")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(new ApiResponse<ICollection<AirlineGetDto>>
			{
				Data = await _AirlineService.GetByExpression(true, null, "Planes"),
				ErrorMessage = null,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPost]
		public async Task<IActionResult> Create(AirlineCreateDto dto)
		{
			AirlineGetDto Airline = null;
			try
			{
				Airline = await _AirlineService.CreateAsync(dto);
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
			AirlineGetDto dto = null;
			try
			{
				dto = await _AirlineService.GetSingleByExpression(true, x=>x.Id == id, "Planes");
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(new ApiResponse<AirlineGetDto>
			{
				Data = dto,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, AirlineUpdateDto dto)
		{

			try
			{
				await _AirlineService.UpdateAsync(id, dto);
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<AirlineUpdateDto>
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
				await _AirlineService.DeleteAsync(id);
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
