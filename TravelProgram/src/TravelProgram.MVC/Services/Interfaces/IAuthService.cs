using TravelProgram.MVC.ViewModels.AuthVMs;

namespace TravelProgram.MVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseVM> Login(UserLoginVM vm);
        Task<LoginResponseVM> AdminLogin(UserLoginVM vm);
        void Logout();
        Task<bool> Register(UserRegisterVM vm);
    }
}
