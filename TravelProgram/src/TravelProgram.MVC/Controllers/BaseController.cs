
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TravelProgram.MVC.Controllers
{
    public class BaseController : Controller
    {
        protected void SetFullName()
        {
            string token = HttpContext.Request.Cookies["token"];
            if (!string.IsNullOrEmpty(token))
            {
                var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secretKey);

                try
                {
                    var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    }, out SecurityToken validatedToken);

                    string fullname = claimsPrincipal?.Identity?.Name;

                    if (!string.IsNullOrEmpty(fullname))
                    {
                        ViewBag.FullName = fullname;
                    }
                }
                catch (SecurityTokenException)
                {
                    
                }
            }
        }
    }
}
