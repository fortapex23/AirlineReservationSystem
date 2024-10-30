using System.Linq.Expressions;
using TravelProgram.Business.DTOs.PlaneDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface IPlaneService
	{
        Task<bool> IsExist(Expression<Func<Plane, bool>> expression);
        Task<PlaneGetDto> CreateAsync(PlaneCreateDto dto);
		Task UpdateAsync(int? id, PlaneUpdateDto dto);
		Task DeleteAsync(int id);
		Task<PlaneGetDto> GetById(int id);
		Task<ICollection<PlaneGetDto>> GetByExpression(bool asnotracking = false, Expression<Func<Plane, bool>>? expression = null, params string[] includes);
		Task<PlaneGetDto> GetSingleByExpression(bool asnotracking = false, Expression<Func<Plane, bool>>? expression = null, params string[] includes);
	}
}
