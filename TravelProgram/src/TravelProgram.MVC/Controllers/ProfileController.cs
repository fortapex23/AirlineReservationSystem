using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Services.Interfaces;

namespace TravelProgram.MVC.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly ICrudService _crudService;

        public ProfileController(ICrudService crudService)
        {
            _crudService = crudService;
        }
        public IActionResult Index()
        {
            SetFullName();

            if (ViewBag.FullName is null)
            {
                return View("Login", "Auth");
            }



            return View();
        }
    }
}
