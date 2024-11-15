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
        private readonly IFlightRepository _flightRepository;

        public SeatService(ISeatRepository SeatRepository, IMapper mapper, IBookingRepository bookingRepository,
								IOrderItemRepository orderItemRepository, IFlightRepository flightRepository)
		{
			_seatRepository = SeatRepository;
			_mapper = mapper;
            _bookingRepository = bookingRepository;
            _orderItemRepository = orderItemRepository;
            _flightRepository = flightRepository;
        }

        public Task<bool> IsExist(Expression<Func<Seat, bool>> expression)
        {
            return _seatRepository.Table.AnyAsync(expression);
        }

        public async Task<SeatGetDto> CreateAsync(SeatCreateDto dto)
		{
			if (dto.SeatNumber <= 0)
				throw new Exception("Seat number cant be <= 0");

            if (dto.Price <= 0)
                throw new Exception("Price cant be <= 0");

            var existingSeat = await _seatRepository
			.GetByExpression(false, t => t.SeatNumber == dto.SeatNumber && t.FlightId == dto.FlightId)
			.FirstOrDefaultAsync();

			if (existingSeat != null)
				throw new Exception("A Seat with the same number already exists.");

			var flight = _flightRepository.GetByExpression(false, x => x.Id == dto.FlightId);
			if (flight is null)
				throw new Exception("No flight with this id");

			var seat = _mapper.Map<Seat>(dto);
			seat.CreatedTime = DateTime.Now;
			seat.UpdatedTime = DateTime.Now;

			await _seatRepository.CreateAsync(seat);
			await _seatRepository.CommitAsync();

			return _mapper.Map<SeatGetDto>(seat);
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

            if (dto.SeatNumber <= 0)
                throw new Exception("Seat number cant be <= 0");

            if (dto.Price <= 0)
                throw new Exception("Price cant be <= 0");

            var seat = await _seatRepository.GetByIdAsync((int)id);
			if (seat == null) throw new Exception("Seat not found");

			if (dto.IsAvailable == false)
				throw new Exception("Seat is not available for update");

            var existingSeat = await _seatRepository
			.GetByExpression(true, t => t.SeatNumber == dto.SeatNumber && t.Id != id && t.FlightId == dto.FlightId)
			.FirstOrDefaultAsync();

			if (existingSeat != null)
				throw new Exception("a Seat with the same number already exists");

            var flight = _flightRepository.GetByExpression(false, x => x.Id == dto.FlightId);
            if (flight is null)
                throw new Exception("No flight with this id");

            _mapper.Map(dto, seat);

            seat.UpdatedTime = DateTime.Now;

			await _seatRepository.CommitAsync();
		}
	}
}
