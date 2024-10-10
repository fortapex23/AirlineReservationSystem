using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Enum;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
	public class FlightService : IFlightService
	{
		private readonly IFlightRepository _flightRepository;
		private readonly IMapper _mapper;
		private readonly ISeatRepository _seatRepository;

        public FlightService(IFlightRepository FlightRepository, IMapper mapper, ISeatRepository seatRepository)
		{
			_flightRepository = FlightRepository;
			_mapper = mapper;
			_seatRepository = seatRepository;
        }
		public async Task<FlightGetDto> CreateAsync(FlightCreateDto dto)
		{
			var existingFlight = await _flightRepository
			.GetByExpression(false, x => x.FlightNumber == dto.FlightNumber)
			.FirstOrDefaultAsync();

			if (existingFlight != null)
				throw new Exception("A Flight with the same number already exists.");

			var flight = _mapper.Map<Flight>(dto);
			flight.CreatedTime = DateTime.Now;
			flight.UpdatedTime = DateTime.Now;

			await _flightRepository.CreateAsync(flight);

			await _flightRepository.CommitAsync();

			var planeSeats = await _seatRepository
				.GetByExpression(false, s => s.PlaneId == flight.PlaneId && s.FlightId == null)
				.ToListAsync();

			foreach (var seat in planeSeats)
			{
				seat.FlightId = flight.Id;

				if (seat.ClassType == SeatClassType.Economy)
				{
					seat.Price = dto.SeatPrice;
				}
				else if (seat.ClassType == SeatClassType.Business)
				{
					seat.Price = dto.SeatPrice * 2;
				}
			}

			await _flightRepository.CommitAsync();

			return _mapper.Map<FlightGetDto>(flight);
		}

		public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var flight = await _flightRepository.GetByIdAsync(id);
			if (flight == null) throw new Exception("Flight not found.");

			_flightRepository.Delete(flight);
			await _flightRepository.CommitAsync();
		}

		public async Task<ICollection<FlightGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Flight, bool>>? expression = null, params string[] includes)
		{
			var flights = await _flightRepository.GetByExpression(asnotracking, expression, includes).ToListAsync();

			return _mapper.Map<ICollection<FlightGetDto>>(flights);
		}

		public async Task<FlightGetDto> GetById(int id)
		{
			if (id < 1) throw new Exception();

			var flight = await _flightRepository.GetByIdAsync(id);
			if (flight == null) throw new Exception("Flight not found");

			return _mapper.Map<FlightGetDto>(flight);
		}

		public async Task<FlightGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Flight, bool>>? expression = null, params string[] includes)
		{
			var flight = await _flightRepository.GetByExpression(asnotracking, expression, includes).FirstOrDefaultAsync();
			if (flight == null) throw new Exception("Flight not found");

			return _mapper.Map<FlightGetDto>(flight);
		}

		public async Task UpdateAsync(int? id, FlightUpdateDto dto)
		{
			if (id < 1 || id is null) throw new NullReferenceException("id is invalid");

			var flight = await _flightRepository.GetByIdAsync((int)id);
			if (flight == null) throw new Exception("Flight not found");

			var existingFlight = await _flightRepository
			.GetByExpression(true, t => t.FlightNumber == dto.FlightNumber && t.Id != id)
			.FirstOrDefaultAsync();

			if (existingFlight != null)
				throw new Exception("a Flight with the same number already exists");

			_mapper.Map(dto, flight);

			flight.UpdatedTime = DateTime.Now;

			await _flightRepository.CommitAsync();
		}
	}
}
