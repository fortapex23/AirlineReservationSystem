using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.SeatDTOs;
using TravelProgram.Business.Services.Implementations;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SeatsController : ControllerBase
	{
		private readonly ISeatService _seatService;

		public SeatsController(ISeatService seatService)
		{
			_seatService = seatService;
		}

        [HttpGet("isexist/{id}")]
        public async Task<IActionResult> IsExist(int id)
        {
            bool exists = false;
            try
            {
                exists = await _seatService.IsExist(f => f.Id == id);
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

            return Ok(new ApiResponse<bool>
            {
                Data = exists,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(new ApiResponse<ICollection<SeatGetDto>>
			{
				Data = await _seatService.GetByExpression(true, null),
				ErrorMessage = null,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPost]
		public async Task<IActionResult> Create(SeatCreateDto dto)
		{
			SeatGetDto Seat = null;
			try
			{
				Seat = await _seatService.CreateAsync(dto);
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
			SeatGetDto dto = null;
			try
			{
				dto = await _seatService.GetById(id);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(new ApiResponse<SeatGetDto>
			{
				Data = dto,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, SeatUpdateDto dto)
		{

			try
			{
				await _seatService.UpdateAsync(id, dto);
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<SeatUpdateDto>
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
				await _seatService.DeleteAsync(id);
			}
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Data = null
                });
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
