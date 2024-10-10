using System.Linq.Expressions;
using TravelProgram.Business.DTOs.AirlineDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface IAirlineService
	{
		Task<AirlineGetDto> CreateAsync(AirlineCreateDto dto);
		Task UpdateAsync(int? id, AirlineUpdateDto dto);
		Task DeleteAsync(int id);
		Task SoftDeleteAsync(int id);
		Task<AirlineGetDto> GetById(int id);
		Task<ICollection<AirlineGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Airline, bool>>? expression = null, params string[] includes);
		Task<AirlineGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Airline, bool>>? expression = null, params string[] includes);
	}
}
