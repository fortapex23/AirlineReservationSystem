using RestSharp;
using TravelProgram.MVC.ApiResponseMessages;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AuthVMs;

namespace TravelProgram.MVC.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly RestClient _restClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _restClient = new RestClient(_configuration.GetSection("Api:URL").Value);
        }

        public async Task<LoginResponseVM> AdminLogin(UserLoginVM vm)
        {
            var request = new RestRequest("/Auth/AdminLogin", Method.Post);
            request.AddJsonBody(vm);

            var response = await _restClient.ExecuteAsync<LoginResponseVM>(request);

            if (!response.IsSuccessful)
            {
                throw new Exception("couldnt login admin");
            }

            return response.Data;
        }

        public async Task<LoginResponseVM> Login(UserLoginVM vm)
        {
            var request = new RestRequest("/Auth/Login", Method.Post);
            request.AddJsonBody(vm);

            var response = await _restClient.ExecuteAsync<LoginResponseVM>(request);

            if (!response.IsSuccessful)
            {
                var errorMessage = response.Data?.ErrorMessage ?? "Couldnt login";

                if (errorMessage.Contains("Invalid email"))
                {
                    throw new UnauthorizedAccessException("Invalid email or password");
                }

                throw new Exception(errorMessage);
            }

            return response.Data;
        }

		public async Task<string> ForgotPassword(ForgotPasswordVM vm)
		{
			var request = new RestRequest("/Auth/ForgotPassword", Method.Post);
			request.AddJsonBody(vm);

			var response = await _restClient.ExecuteAsync<ApiResponseMessage<string>>(request);

			if (!response.IsSuccessful || response.Data?.Data == null)
			{
				throw new Exception(response.Data?.ErrorMessage ?? "Failed to reset password");
			}

			return response.Data.Data;
		}

		public void Logout()
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("token");
        }

        public async Task<bool> Register(UserRegisterVM vm)
        {
            var request = new RestRequest("/Auth/Register", Method.Post);
            request.AddJsonBody(vm);

            var response = await _restClient.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Login failed: {response.Content}");
            }

            return response.IsSuccessful;
        }
    }
}
