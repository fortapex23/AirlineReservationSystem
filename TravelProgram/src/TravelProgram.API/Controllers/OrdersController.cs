﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.AirportDTOs;
using TravelProgram.Business.DTOs.OrderDTOs;
using TravelProgram.Business.Services.Implementations;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("isexist/{id}")]
        public async Task<IActionResult> IsExist(int id)
        {
            bool exists = false;
            try
            {
                exists = await _orderService.IsExist(f => f.Id == id);
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
            var orders = await _orderService.GetByExpression(true, null, "OrderItems");
            return Ok(new ApiResponse<ICollection<OrderGetDto>>
            {
                Data = orders,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateDto dto)
        {
            try
            {
                var order = await _orderService.CreateAsync(dto);
                //return CreatedAtAction(nameof(GetById), new { id = order.Id }, new ApiResponse<OrderGetDto>
                //{
                //    Data = order,
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
            OrderGetDto order = null;

            try
            {
                order = await _orderService.GetSingleByExpression(false, x => x.Id == id, "OrderItems");
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    ErrorMessage = ex.Message
                });
            }

			return Ok(new ApiResponse<OrderGetDto>
			{
				Data = order,
				StatusCode = StatusCodes.Status200OK
			});
		}

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderUpdateDto dto)
        {
            try
            {
                await _orderService.UpdateAsync(id, dto);
                return NoContent();
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _orderService.DeleteAsync(id);
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
