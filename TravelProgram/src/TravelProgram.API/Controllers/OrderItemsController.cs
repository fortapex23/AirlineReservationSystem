using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.OrderDTOs;
using TravelProgram.Business.DTOs.OrderItemDTOs;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var orderItems = await _orderItemService.GetByExpression(true, null);
            return Ok(new ApiResponse<ICollection<OrderItemGetDto>>
            {
                Data = orderItems,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByOrderId(int orderid)
        {
            var orderItem = await _orderItemService.GetByExpression(true, x => x.OrderId == orderid);
            return Ok(new ApiResponse<ICollection<OrderItemGetDto>>
            {
                Data = orderItem,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderItemCreateDto dto)
        {
            try
            {
                var orderItem = await _orderItemService.CreateAsync(dto);
                //return CreatedAtAction(nameof(GetById), new { id = orderItem.Id }, new ApiResponse<OrderItemGetDto>
                //{
                //    Data = orderItem,
                //    StatusCode = StatusCodes.Status201Created
                //});
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                });
            }
			return Created();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var orderItem = await _orderItemService.GetById(id);
                return Ok(new ApiResponse<OrderItemGetDto>
                {
                    Data = orderItem,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderItemService.DeleteAsync(id);
                return Ok(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message
                });
            }
        }
    }
}
