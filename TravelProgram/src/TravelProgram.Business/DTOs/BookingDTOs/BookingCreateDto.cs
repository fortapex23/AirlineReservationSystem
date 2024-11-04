using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.BookingDTOs
{
	public record BookingCreateDto(int FlightId, string AppUserId, string BookingNumber, int SeatId, 
                            DateTime CreatedTime, DateTime UpdatedTime);

	public class BookingCreateDtoValidator : AbstractValidator<BookingCreateDto>
	{
        public BookingCreateDtoValidator()
        {
            RuleFor(x=>x.FlightId).NotNull().NotEmpty();

            RuleFor(x=>x.AppUserId).NotNull().NotEmpty();

            RuleFor(x=>x.BookingNumber).NotNull().NotEmpty();

            RuleFor(x=>x.SeatId).NotNull().NotEmpty();

            //RuleFor(x=>x.Status).NotNull();
		}
	}
}
