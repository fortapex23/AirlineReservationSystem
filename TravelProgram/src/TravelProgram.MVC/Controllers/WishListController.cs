using Microsoft.AspNetCore.Mvc;

namespace TravelProgram.MVC.Controllers
{
    public class WishListController : BaseController
    {
        public IActionResult Index()
        {
            SetFullName();

            return View();
        }
    }
}
