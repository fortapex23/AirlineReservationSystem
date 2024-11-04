using TravelProgram.MVC.ApiResponseMessages;

namespace TravelProgram.MVC.Services.Interfaces
{
    public interface ICrudService
    {
        Task<T> GetByIdAsync<T>(string endpoint, int? id);
        Task<T> GetByStringIdAsync<T>(string endpoint, string? id);
        Task<T> GetAllAsync<T>(string endpoint);
        Task Delete<T>(string endpoint, int id);
        Task Create<T>(string endpoint, T entity) where T : class;
        Task Update<T>(string endpoint, T entity) where T : class;
        Task<bool> IsExist(string endpoint, int? id);
    }
}
