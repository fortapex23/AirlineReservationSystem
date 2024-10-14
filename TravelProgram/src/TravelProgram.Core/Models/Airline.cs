using TravelProgram.Core.Enum;

namespace TravelProgram.Core.Models
{
	public class Airline : BaseEntity
	{
		public string Name { get; set; }
        public Countries Country { get; set; }

        public ICollection<Plane> Planes { get; set; }
	}
}
