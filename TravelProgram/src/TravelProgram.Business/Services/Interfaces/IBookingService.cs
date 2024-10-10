using System.Linq.Expressions;
using TravelProgram.Business.DTOs.BookingDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface IBookingService
	{
		Task<BookingGetDto> CreateAsync(BookingCreateDto dto);
		Task UpdateAsync(int? id, BookingUpdateDto dto);
		Task DeleteAsync(int id);
		Task<BookingGetDto> GetById(int id);
		Task<ICollection<BookingGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Booking, bool>>? expression = null, params string[] includes);
		Task<BookingGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Booking, bool>>? expression = null, params string[] includes);
	}
}
