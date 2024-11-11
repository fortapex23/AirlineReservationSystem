using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelProgram.Business.DTOs.SeatDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
	public class SeatService : ISeatService
	{
		private readonly ISeatRepository _seatRepository;
		private readonly IMapper _mapper;
        private readonly IBookingRepository _bookingRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public SeatService(ISeatRepository SeatRepository, IMapper mapper, IBookingRepository bookingRepository,
								IOrderItemRepository orderItemRepository)
		{
			_seatRepository = SeatRepository;
			_mapper = mapper;
            _bookingRepository = bookingRepository;
            _orderItemRepository = orderItemRepository;
        }

        public Task<bool> IsExist(Expression<Func<Seat, bool>> expression)
        {
            return _seatRepository.Table.AnyAsync(expression);
        }

        public async Task<SeatGetDto> CreateAsync(SeatCreateDto dto)
		{
			var existingSeat = await _seatRepository
			.GetByExpression(false, t => t.SeatNumber == dto.SeatNumber)
			.FirstOrDefaultAsync();

			if (existingSeat != null)
				throw new Exception("A Seat with the same number already exists.");

			var Seat = _mapper.Map<Seat>(dto);
			Seat.CreatedTime = DateTime.Now;
			Seat.UpdatedTime = DateTime.Now;

			await _seatRepository.CreateAsync(Seat);
			await _seatRepository.CommitAsync();

			return _mapper.Map<SeatGetDto>(Seat);
		}

		public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var seat = await _seatRepository.GetByIdAsync(id);
			if (seat == null) throw new Exception("Seat not found.");

			var booking = _bookingRepository.GetByExpression(false, x => x.SeatId == id);

			if (booking is not null)
				throw new InvalidOperationException("this seat has been booked and cant be deleted");

            if (seat.OrderItems != null && seat.OrderItems.Any())
            {
                foreach (var orderItem in seat.OrderItems.ToList())
                {
                    _orderItemRepository.Delete(orderItem);
                }
                await _orderItemRepository.CommitAsync();
            }

            _seatRepository.Delete(seat);
			await _seatRepository.CommitAsync();
		}

		public async Task<ICollection<SeatGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Seat, bool>>? expression = null, params string[] includes)
		{
			var Seats = await _seatRepository.GetByExpression(asnotracking, expression, includes).ToListAsync();

			return _mapper.Map<ICollection<SeatGetDto>>(Seats);
		}

		public async Task<SeatGetDto> GetById(int id)
		{
			if (id < 1) throw new Exception();

			var Seat = await _seatRepository.GetByIdAsync(id);
			if (Seat == null) throw new Exception("Seat not found");

			return _mapper.Map<SeatGetDto>(Seat);
		}

		public async Task<SeatGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Seat, bool>>? expression = null, params string[] includes)
		{
			var Seat = await _seatRepository.GetByExpression(asnotracking, expression, includes).FirstOrDefaultAsync();
			if (Seat == null) throw new Exception("Seat not found");

			return _mapper.Map<SeatGetDto>(Seat);
		}

		public async Task UpdateAsync(int? id, SeatUpdateDto dto)
		{
			if (id < 1 || id is null) throw new NullReferenceException("id is invalid");

			var seat = await _seatRepository.GetByIdAsync((int)id);
			if (seat == null) throw new Exception("Seat not found");

			var existingSeat = await _seatRepository
			.GetByExpression(true, t => t.SeatNumber == dto.SeatNumber && t.Id != id)
			.FirstOrDefaultAsync();

			if (existingSeat != null)
				throw new Exception("a Seat with the same number already exists");

			_mapper.Map(dto, seat);

            seat.UpdatedTime = DateTime.Now;

			await _seatRepository.CommitAsync();
		}
	}
}
