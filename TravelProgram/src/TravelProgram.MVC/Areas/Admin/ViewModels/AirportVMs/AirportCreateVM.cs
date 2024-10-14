using System.ComponentModel.DataAnnotations;
using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.Areas.Admin.ViewModels.AirportVMs
{
	public class AirportCreateVM
	{
		public string Name { get; set; }

        public AiportCities City { get; set; }

	}
}
