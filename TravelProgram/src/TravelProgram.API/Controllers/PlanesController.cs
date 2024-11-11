using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.AirlineDTOs;
using TravelProgram.Business.DTOs.PlaneDTOs;
using TravelProgram.Business.Services.Implementations;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlanesController : ControllerBase
	{
		private readonly IPlaneService _planeService;

		public PlanesController(IPlaneService PlaneService)
		{
			_planeService = PlaneService;
		}

        [HttpGet("isexist/{id}")]
        public async Task<IActionResult> IsExist(int id)
        {
            bool exists = false;
            try
            {
                exists = await _planeService.IsExist(f => f.Id == id);
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
			return Ok(new ApiResponse<ICollection<PlaneGetDto>>
			{
				Data = await _planeService.GetByExpression(true, null, "Flights"),
				ErrorMessage = null,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPost]
		public async Task<IActionResult> Create(PlaneCreateDto dto)
		{
			PlaneGetDto plane = null;
			try
			{
				plane = await _planeService.CreateAsync(dto);
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
			PlaneGetDto dto = null;
			try
			{
				dto = await _planeService.GetSingleByExpression(true, x=>x.Id == id, "Flights");
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}

			return Ok(new ApiResponse<PlaneGetDto>
			{
				Data = dto,
				StatusCode = StatusCodes.Status200OK
			});
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, PlaneUpdateDto dto)
		{

			try
			{
				await _planeService.UpdateAsync(id, dto);
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<PlaneUpdateDto>
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
				await _planeService.DeleteAsync(id);
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
