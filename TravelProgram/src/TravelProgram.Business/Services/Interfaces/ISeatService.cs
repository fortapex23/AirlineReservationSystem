using System.Linq.Expressions;
using TravelProgram.Business.DTOs.SeatDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface ISeatService
	{
        Task<bool> IsExist(Expression<Func<Seat, bool>> expression);
        Task<SeatGetDto> CreateAsync(SeatCreateDto dto);
		Task UpdateAsync(int? id, SeatUpdateDto dto);
		Task DeleteAsync(int id);
		Task<SeatGetDto> GetById(int id);
		Task<ICollection<SeatGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Seat, bool>>? expression = null, params string[] includes);
		Task<SeatGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Seat, bool>>? expression = null, params string[] includes);
	}
}
