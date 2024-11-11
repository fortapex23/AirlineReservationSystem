using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Enum;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
	public class FlightService : IFlightService
	{
		private readonly IFlightRepository _flightRepository;
		private readonly IMapper _mapper;
		private readonly ISeatRepository _seatRepository;
        private readonly IPlaneRepository _planeRepository;
        private readonly IAirportRepository _airportRepository;

        public FlightService(IFlightRepository FlightRepository, IMapper mapper, 
                        ISeatRepository seatRepository, IPlaneRepository planeRepository, IAirportRepository airportRepository)
		{
			_flightRepository = FlightRepository;
			_mapper = mapper;
			_seatRepository = seatRepository;
            _planeRepository = planeRepository;
            _airportRepository = airportRepository;
        }

        public async Task<ICollection<FlightGetDto>> SearchFlightsAsync(string departureCity, string destinationCity, DateTime? departureTime)
        {
            var query = _flightRepository.GetByExpression(true, null, "Bookings", "Seats");

            if (!string.IsNullOrEmpty(departureCity))
            {
                if (Enum.TryParse(typeof(AiportCities), departureCity, true, out var departureCityEnum))
                {
                    query = query.Where(f => f.DepartureAirport.City == (AiportCities)departureCityEnum);
                }
                else
                {
                    throw new ArgumentException($"Invalid departure city: {departureCity}");
                }
            }

            if (!string.IsNullOrEmpty(destinationCity))
            {
                if (Enum.TryParse(typeof(AiportCities), destinationCity, true, out var destinationCityEnum))
                {
                    query = query.Where(f => f.ArrivalAirport.City == (AiportCities)destinationCityEnum);
                }
                else
                {
                    throw new ArgumentException($"Invalid destination city: {destinationCity}");
                }
            }

            if (departureTime.HasValue)
            {
                query = query.Where(f => f.DepartureTime.Date == departureTime.Value.Date);
            }

            var flights = await query.ToListAsync();
            return _mapper.Map<ICollection<FlightGetDto>>(flights);
        }

        public async Task<FlightGetDto> CreateAsync(FlightCreateDto dto)
        {
            if (dto.BusinessSeatPrice < 0 || dto.EconomySeatPrice < 0)
                throw new Exception("Invalid seat prices");

            var alreadyFlights = await _flightRepository
                .GetByExpression(false, f => f.PlaneId == dto.PlaneId &&
                                             (f.DepartureTime.AddHours(-5) <= dto.DepartureTime && f.ArrivalTime.AddHours(5) >= dto.DepartureTime))
                .ToListAsync();

            if (alreadyFlights.Any())
                throw new Exception("This plane is already assigned to another flight in that time range");

            var existingFlight = await _flightRepository
                .GetByExpression(false, x => x.FlightNumber == dto.FlightNumber)
                .FirstOrDefaultAsync();

            if (existingFlight != null)
                throw new Exception("A Flight with the same number already exists.");

            var plane = await _planeRepository.GetByIdAsync(dto.PlaneId);

            if (plane is null)
                throw new Exception("No plane with this id");

            var depAir = await _airportRepository.GetByIdAsync(dto.DepartureAirportId);

            if (depAir is null)
                throw new Exception("No departure airport with this id");

            var arrAir = await _airportRepository.GetByIdAsync(dto.ArrivalAirportId);

            if (arrAir is null)
                throw new Exception("No arrival airport with this id");

            var flight = _mapper.Map<Flight>(dto);
            flight.CreatedTime = DateTime.Now;
            flight.UpdatedTime = DateTime.Now;

            await _flightRepository.CreateAsync(flight);
            await _flightRepository.CommitAsync();

            //var plane = await _planeRepository.GetByIdAsync(dto.PlaneId);
            //if (plane == null)
            //{
            //    throw new Exception("plane not found");
            //}

            var seats = new List<Seat>();
            for (int i = 1; i <= plane.EconomySeats; i++)
            {
                seats.Add(new Seat
                {
                    FlightId = flight.Id,
                    SeatNumber = i,
                    ClassType = SeatClassType.Economy,
                    Price = dto.EconomySeatPrice,
                    IsAvailable = true,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                });
            }

            for (int i = 1; i <= plane.BusinessSeats; i++)
            {
                seats.Add(new Seat
                {
                    FlightId = flight.Id,
                    SeatNumber = plane.EconomySeats + i,
                    ClassType = SeatClassType.Business,
                    Price = dto.BusinessSeatPrice,
                    IsAvailable = true,
                    CreatedTime = DateTime.Now,
                    UpdatedTime = DateTime.Now
                });
            }

            await _seatRepository.Table.AddRangeAsync(seats);
            await _seatRepository.CommitAsync();

            return _mapper.Map<FlightGetDto>(flight);
        }


        public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var flight = await _flightRepository.GetByIdAsync(id);
			if (flight == null) throw new Exception("Flight not found.");

            var books = await _flightRepository.Table.AnyAsync(x => x.Id == id && x.Bookings.Any());

            if (books)
                throw new InvalidOperationException("Cant delete flight because seats have already been booked");

            //if (flight.Seats != null && flight.Seats.Any())
            //{
            //    foreach (var se in flight.Seats.ToList())
            //    {
            //        _seatRepository.Delete(se);
            //    }
            //    await _seatRepository.CommitAsync();
            //}

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

            var existingPlane = await _planeRepository.GetByIdAsync(dto.PlaneId);
            if (existingPlane == null) throw new Exception("No plane with this id");

            var depAir = await _airportRepository.GetByIdAsync(dto.DepartureAirportId);

            if (depAir is null)
                throw new Exception("No departure airport with this id");

            var arrAir = await _airportRepository.GetByIdAsync(dto.ArrivalAirportId);

            if (arrAir is null)
                throw new Exception("No arrival airport with this id");

            var alreadyFlights = await _flightRepository
                .GetByExpression(false, f => f.PlaneId == dto.PlaneId &&
                                             (f.DepartureTime.AddHours(-5) <= dto.DepartureTime && f.ArrivalTime.AddHours(5) >= dto.DepartureTime))
                .ToListAsync();

            if (alreadyFlights.Any())
                throw new Exception("This plane is already assigned to another flight in that time range");

            bool planeChanged = flight.PlaneId != dto.PlaneId;

            bool seatConfigurationChanged = flight.PlaneId == dto.PlaneId &&
                                            (existingPlane.EconomySeats != flight.Seats.Count(s => s.ClassType == SeatClassType.Economy) ||
                                             existingPlane.BusinessSeats != flight.Seats.Count(s => s.ClassType == SeatClassType.Business));

            if (planeChanged || seatConfigurationChanged)
            {
                var books = await _flightRepository.Table.AnyAsync(x => x.Id == id && x.Bookings.Any());

                if (books)
                    throw new InvalidOperationException("Cant update flight plane because seats have already been booked");

                var seats = _seatRepository.GetByExpression(false, x => x.FlightId == (int)id);
                _seatRepository.Table.RemoveRange(seats);
                await _seatRepository.CommitAsync();

                var newSeats = new List<Seat>();
                for (int i = 1; i <= existingPlane.EconomySeats; i++)
                {
                    newSeats.Add(new Seat
                    {
                        FlightId = flight.Id,
                        SeatNumber = i,
                        ClassType = SeatClassType.Economy,
                        Price = dto.EconomySeatPrice,
                        IsAvailable = true,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now
                    });
                }

                for (int i = 1; i <= existingPlane.BusinessSeats; i++)
                {
                    newSeats.Add(new Seat
                    {
                        FlightId = flight.Id,
                        SeatNumber = existingPlane.EconomySeats + i,
                        ClassType = SeatClassType.Business,
                        Price = dto.BusinessSeatPrice,
                        IsAvailable = true,
                        CreatedTime = DateTime.Now,
                        UpdatedTime = DateTime.Now
                    });
                }

                await _seatRepository.Table.AddRangeAsync(newSeats);
                await _seatRepository.CommitAsync();
            }

            _mapper.Map(dto, flight);
            flight.UpdatedTime = DateTime.Now;

            await _flightRepository.CommitAsync();
        }

        public Task<bool> IsExist(Expression<Func<Flight, bool>> expression)
        {
            return _flightRepository.Table.AnyAsync(expression);
        }

    }
}
