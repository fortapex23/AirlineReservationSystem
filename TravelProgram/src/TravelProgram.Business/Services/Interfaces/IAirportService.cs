using System.Linq.Expressions;
using TravelProgram.Business.DTOs.AirportDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface IAirportService
	{
        Task<bool> IsExist(Expression<Func<Airport, bool>> expression);
        Task<AirportGetDto> CreateAsync(AirportCreateDto dto);
		Task UpdateAsync(int? id, AirportUpdateDto dto);
		Task DeleteAsync(int id);
		Task<AirportGetDto> GetById(int id);
		Task<ICollection<AirportGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Airport, bool>>? expression = null, params string[] includes);
		Task<AirportGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Airport, bool>>? expression = null, params string[] includes);
	}
}
