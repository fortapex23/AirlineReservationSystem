using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.BookingDTOs
{
	public record BookingUpdateDto(int FlightId, string AppUserId, string BookingNumber, int SeatId, 
									DateTime UpdatedTime);

	public class BookingUpdateDtoValidator : AbstractValidator<BookingUpdateDto>
	{
		public BookingUpdateDtoValidator()
		{
			RuleFor(x => x.FlightId).NotNull().NotEmpty();

			RuleFor(x => x.AppUserId).NotNull().NotEmpty();

			RuleFor(x => x.BookingNumber).NotNull().NotEmpty();

			RuleFor(x => x.SeatId).NotNull().NotEmpty();

			//RuleFor(x => x.Status).NotNull();
		}
	}
}
