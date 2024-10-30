using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelProgram.Business.DTOs.AirportDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
	public class AirportService : IAirportService
	{
		private readonly IAirportRepository _AirportRepository;
		private readonly IMapper _mapper;

		public AirportService(IAirportRepository AirportRepository, IMapper mapper)
		{
			_AirportRepository = AirportRepository;
			_mapper = mapper;
		}

        public Task<bool> IsExist(Expression<Func<Airport, bool>> expression)
        {
            return _AirportRepository.Table.AnyAsync(expression);
        }

        public async Task<AirportGetDto> CreateAsync(AirportCreateDto dto)
		{
			var existingAirport = await _AirportRepository
			.GetByExpression(false, t => t.Name == dto.Name)
			.FirstOrDefaultAsync();

			if (existingAirport != null)
				throw new Exception("Airport with the same name already exists.");

			var Airport = _mapper.Map<Airport>(dto);
			Airport.CreatedTime = DateTime.Now;
			Airport.UpdatedTime = DateTime.Now;

			await _AirportRepository.CreateAsync(Airport);
			await _AirportRepository.CommitAsync();

			return _mapper.Map<AirportGetDto>(Airport);
		}

		public async Task DeleteAsync(int id)
		{
			if (id < 1) throw new Exception();

			var airport = await _AirportRepository.GetByIdAsync(id);
			if (airport == null) throw new Exception("Airport not found.");

			_AirportRepository.Delete(airport);
			await _AirportRepository.CommitAsync();
		}

		public async Task<ICollection<AirportGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Airport, bool>>? expression = null, params string[] includes)
		{
			var Airports = await _AirportRepository.GetByExpression(asnotracking, expression, includes).ToListAsync();

			return _mapper.Map<ICollection<AirportGetDto>>(Airports);
		}

		public async Task<AirportGetDto> GetById(int id)
		{
			if (id < 1) throw new Exception();

			var Airport = await _AirportRepository.GetByIdAsync(id);
			if (Airport == null) throw new Exception("Airport not found");

			return _mapper.Map<AirportGetDto>(Airport);
		}

		public async Task<AirportGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Airport, bool>>? expression = null, params string[] includes)
		{
			var Airport = await _AirportRepository.GetByExpression(asnotracking, expression, includes).FirstOrDefaultAsync();
			if (Airport == null) throw new Exception("Airport not found");

			return _mapper.Map<AirportGetDto>(Airport);
		}

		public async Task UpdateAsync(int? id, AirportUpdateDto dto)
		{
			if (id < 1 || id is null) throw new NullReferenceException("id is invalid");

			var airport = await _AirportRepository.GetByIdAsync((int)id);
			if (airport == null) throw new Exception("Airport not found");

			var existingAirport = await _AirportRepository
			.GetByExpression(true, t => t.Name == dto.Name && t.Id != id)
			.FirstOrDefaultAsync();

			if (existingAirport != null)
				throw new Exception("Airport with the same name already exists");

			_mapper.Map(dto, airport);

			airport.UpdatedTime = DateTime.Now;

			await _AirportRepository.CommitAsync();
		}
	}
}
