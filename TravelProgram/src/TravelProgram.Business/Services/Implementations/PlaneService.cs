using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Numerics;
using TravelProgram.Business.DTOs.PlaneDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Enum;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using Plane = TravelProgram.Core.Models.Plane;

namespace TravelProgram.Business.Services.Implementations
{
	public class PlaneService : IPlaneService
	{

		private readonly IPlaneRepository _planeRepository;
		private readonly IMapper _mapper;
		private readonly ISeatRepository _seatRepository;

		public PlaneService(IPlaneRepository PlaneRepository, IMapper mapper, ISeatRepository seatRepository)
		{
			_planeRepository = PlaneRepository;
			_mapper = mapper;
			_seatRepository = seatRepository;
		}
		public async Task<PlaneGetDto> CreateAsync(PlaneCreateDto dto)
		{
			var existingPlane = await _planeRepository
			.GetByExpression(false, t => t.Name == dto.Name)
			.FirstOrDefaultAsync();

			if (existingPlane != null)
				throw new Exception("A Plane with the same name already exists.");

			var plane = _mapper.Map<Plane>(dto);
			plane.CreatedTime = DateTime.Now;
			plane.UpdatedTime = DateTime.Now;

			await _planeRepository.CreateAsync(plane);
			await _planeRepository.CommitAsync();

			var seats = new List<Seat>();
			for (int i = 1; i <= plane.TotalSeats; i++)
			{
				var seat = new Seat
				{
					PlaneId = plane.Id,
					SeatNumber = i,
					IsAvailable = true,
					ClassType = i <= (plane.TotalSeats * 4) / 5 ? SeatClassType.Economy : SeatClassType.Business,
					CreatedTime = DateTime.Now,
					UpdatedTime = DateTime.Now,
					IsDeleted = false
				};
				seats.Add(seat);
			}

			_seatRepository.Table.AddRange(seats);
			await _seatRepository.CommitAsync();

			return _mapper.Map<PlaneGetDto>(plane);
		}

		public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var Plane = await _planeRepository.GetByIdAsync(id);
			if (Plane == null) throw new Exception("Plane not found.");

			_planeRepository.Delete(Plane);
			await _planeRepository.CommitAsync();
		}

		public async Task<ICollection<PlaneGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Plane, bool>>? expression = null, params string[] includes)
		{
			var Planes = await _planeRepository.GetByExpression(asnotracking, expression, includes).ToListAsync();

			return _mapper.Map<ICollection<PlaneGetDto>>(Planes);
		}

		public async Task<PlaneGetDto> GetById(int id)
		{
			if (id < 1) throw new Exception();

			var Plane = await _planeRepository.GetByIdAsync(id);
			if (Plane == null) throw new Exception("Plane not found");

			return _mapper.Map<PlaneGetDto>(Plane);
		}

		public async Task<PlaneGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Plane, bool>>? expression = null, params string[] includes)
		{
			var Plane = await _planeRepository.GetByExpression(asnotracking, expression, includes).FirstOrDefaultAsync();
			if (Plane == null) throw new Exception("Plane not found");

			return _mapper.Map<PlaneGetDto>(Plane);
		}

		public async Task UpdateAsync(int? id, PlaneUpdateDto dto)
		{
			if (id < 1 || id is null) throw new NullReferenceException("id is invalid");

			var airport = await _planeRepository.GetByIdAsync((int)id);
			if (airport == null) throw new Exception("Plane not found");

			var existingPlane = await _planeRepository
			.GetByExpression(true, t => t.Name == dto.Name && t.Id != id)
			.FirstOrDefaultAsync();

			if (existingPlane != null)
				throw new Exception("a Plane with the same name already exists");

			_mapper.Map(dto, airport);

			airport.UpdatedTime = DateTime.Now;

			await _planeRepository.CommitAsync();
		}
	}
}
