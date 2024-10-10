using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Business.DTOs.AirlineDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
	public class AirlineService : IAirlineService
	{
		private readonly IAirlineRepository _airlineRepository;
		private readonly IMapper _mapper;

		public AirlineService(IAirlineRepository AirlineRepository, IMapper mapper)
		{
			_airlineRepository = AirlineRepository;
			_mapper = mapper;
		}
		public async Task<AirlineGetDto> CreateAsync(AirlineCreateDto dto)
		{
			var existingAirline = await _airlineRepository
			.GetByExpression(false, t => t.Name == dto.Name)
			.FirstOrDefaultAsync();

			if (existingAirline != null)
				throw new Exception("A Airline with the same name already exists.");

			var Airline = _mapper.Map<Airline>(dto);
			Airline.CreatedTime = DateTime.Now;
			Airline.UpdatedTime = DateTime.Now;

			await _airlineRepository.CreateAsync(Airline);
			await _airlineRepository.CommitAsync();

			return _mapper.Map<AirlineGetDto>(Airline);
		}

		public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var Airline = await _airlineRepository.GetByIdAsync(id);
			if (Airline == null) throw new Exception("Airline not found.");

			_airlineRepository.Delete(Airline);
			await _airlineRepository.CommitAsync();
		}

		public async Task<ICollection<AirlineGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Airline, bool>>? expression = null, params string[] includes)
		{
			var Airlines = await _airlineRepository.GetByExpression(asnotracking, expression, includes).ToListAsync();

			return _mapper.Map<ICollection<AirlineGetDto>>(Airlines);
		}

		public async Task<AirlineGetDto> GetById(int id)
		{
			if (id < 1) throw new Exception();

			var Airline = await _airlineRepository.GetByIdAsync(id);
			if (Airline == null) throw new Exception("Airline not found");

			return _mapper.Map<AirlineGetDto>(Airline);
		}

		public async Task<AirlineGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Airline, bool>>? expression = null, params string[] includes)
		{
			var Airline = await _airlineRepository.GetByExpression(asnotracking, expression, includes).FirstOrDefaultAsync();
			if (Airline == null) throw new Exception("Airline not found");

			return _mapper.Map<AirlineGetDto>(Airline);
		}

		public async Task SoftDeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var airline = await _airlineRepository.GetByIdAsync(id);
			if (airline == null) throw new Exception("Airline not found.");

			airline.IsDeleted = true;

			await _airlineRepository.CommitAsync();
		}

		public async Task UpdateAsync(int? id, AirlineUpdateDto dto)
		{
			if (id < 1 || id is null) throw new NullReferenceException("id is invalid");

			var Airline = await _airlineRepository.GetByIdAsync((int)id);
			if (Airline == null) throw new Exception("Airline not found");

			var existingAirline = await _airlineRepository
			.GetByExpression(true, t => t.Name == dto.Name && t.Id != id)
			.FirstOrDefaultAsync();

			if (existingAirline != null)
				throw new Exception("a Airline with the same name already exists");

			_mapper.Map(dto, Airline);

			Airline.UpdatedTime = DateTime.Now;

			await _airlineRepository.CommitAsync();
		}
	}
}
