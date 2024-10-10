using System.ComponentModel.DataAnnotations;
using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.Areas.Admin.ViewModels.AirportVMs
{
	public class AirportCreateVM
	{
		[Required]
		public string Name { get; set; }

		[Required]
        public AiportCities City { get; set; }

	}
}
