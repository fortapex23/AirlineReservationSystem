using System.ComponentModel.DataAnnotations;
using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.Areas.Admin.ViewModels.AirlineVMs
{
	public class AirlineCreateVM
	{
		public string Name { get; set; }

		public Countries Country { get; set; }
	}
}
