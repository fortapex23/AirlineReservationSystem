using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelProgram.Business.DTOs.BookingDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
	public class BookingService : IBookingService
	{
		private readonly IBookingRepository _bookingRepository;
		private readonly IMapper _mapper;

		public BookingService(IBookingRepository BookingRepository, IMapper mapper)
		{
			_bookingRepository = BookingRepository;
			_mapper = mapper;
		}
		public async Task<BookingGetDto> CreateAsync(BookingCreateDto dto)
		{
			var existingBooking = await _bookingRepository
			.GetByExpression(false, x => x.BookingNumber == dto.BookingNumber)
			.FirstOrDefaultAsync();

			if (existingBooking != null)
				throw new Exception("A Booking with the same number already exists.");

			var booking = _mapper.Map<Booking>(dto);
			booking.CreatedTime = DateTime.Now;
			booking.UpdatedTime = DateTime.Now;

			await _bookingRepository.CreateAsync(booking);
			await _bookingRepository.CommitAsync();

			return _mapper.Map<BookingGetDto>(booking);
		}

		public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var booking = await _bookingRepository.GetByIdAsync(id);
			if (booking == null) throw new Exception("Booking not found.");

			_bookingRepository.Delete(booking);
			await _bookingRepository.CommitAsync();
		}

		public async Task<ICollection<BookingGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Booking, bool>>? expression = null, params string[] includes)
		{
			var bookings = await _bookingRepository.GetByExpression(asnotracking, expression, includes).ToListAsync();

			return _mapper.Map<ICollection<BookingGetDto>>(bookings);
		}

		public async Task<BookingGetDto> GetById(int id)
		{
			if (id < 1) throw new Exception();

			var booking = await _bookingRepository.GetByIdAsync(id);
			if (booking == null) throw new Exception("Booking not found");

			return _mapper.Map<BookingGetDto>(booking);
		}

		public async Task<BookingGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Booking, bool>>? expression = null, params string[] includes)
		{
			var booking = await _bookingRepository.GetByExpression(asnotracking, expression, includes).FirstOrDefaultAsync();
			if (booking == null) throw new Exception("Booking not found");

			return _mapper.Map<BookingGetDto>(booking);
		}

		public async Task UpdateAsync(int? id, BookingUpdateDto dto)
		{
			if (id < 1 || id is null) throw new NullReferenceException("id is invalid");

			var booking = await _bookingRepository.GetByIdAsync((int)id);
			if (booking == null) throw new Exception("Booking not found");

			var existingBooking = await _bookingRepository
			.GetByExpression(true, t => t.BookingNumber == dto.BookingNumber && t.Id != id)
			.FirstOrDefaultAsync();

			if (existingBooking != null)
				throw new Exception("a Booking with the same number already exists");

			_mapper.Map(dto, booking);

			booking.UpdatedTime = DateTime.Now;

			await _bookingRepository.CommitAsync();
		}
	}
}
