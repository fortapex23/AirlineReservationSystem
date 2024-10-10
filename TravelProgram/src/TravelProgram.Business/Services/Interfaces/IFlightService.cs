using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface IFlightService
	{
		Task<FlightGetDto> CreateAsync(FlightCreateDto dto);
		Task UpdateAsync(int? id, FlightUpdateDto dto);
		Task DeleteAsync(int id);
		Task<FlightGetDto> GetById(int id);
		Task<ICollection<FlightGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Flight, bool>>? expression = null, params string[] includes);
		Task<FlightGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Flight, bool>>? expression = null, params string[] includes);
	}
}
