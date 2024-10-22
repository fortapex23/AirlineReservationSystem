using TravelProgram.Business.DTOs.AirlineDTOs;
using TravelProgram.Core.Models;
using AutoMapper;
using TravelProgram.Business.DTOs.AirportDTOs;
using TravelProgram.Business.DTOs.PlaneDTOs;
using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Business.DTOs.SeatDTOs;
using TravelProgram.Business.DTOs.BookingDTOs;
using TravelProgram.Business.DTOs.BasketItemDTOs;
using TravelProgram.Business.DTOs.UserDTOs;

namespace TravelProgram.Business.MappingProfiles
{
	public class MapProfile : Profile
	{
        public MapProfile()
        {
			CreateMap<AirlineCreateDto, Airline>().ReverseMap();
			CreateMap<AirlineUpdateDto, Airline>().ReverseMap();
			CreateMap<AirlineGetDto, Airline>().ReverseMap();

			CreateMap<AirportCreateDto, Airport>().ReverseMap();
			CreateMap<AirportUpdateDto, Airport>().ReverseMap();
			CreateMap<AirportGetDto, Airport>().ReverseMap();

			CreateMap<PlaneCreateDto, Plane>().ReverseMap();
			CreateMap<PlaneUpdateDto, Plane>().ReverseMap();
			CreateMap<PlaneGetDto, Plane>().ReverseMap();

			CreateMap<FlightCreateDto, Flight>().ReverseMap();
			CreateMap<FlightUpdateDto, Flight>().ReverseMap();
			CreateMap<FlightGetDto, Flight>().ReverseMap();

			CreateMap<SeatCreateDto, Seat>().ReverseMap();
			CreateMap<SeatUpdateDto, Seat>().ReverseMap();
			CreateMap<SeatGetDto, Seat>().ReverseMap();

			CreateMap<BookingCreateDto, Booking>().ReverseMap();
			CreateMap<BookingUpdateDto, Booking>().ReverseMap();
			CreateMap<BookingGetDto, Booking>().ReverseMap();

			CreateMap<BasketItemDTO, BasketItem>().ReverseMap();

            CreateMap<UserGetDto, AppUser>().ReverseMap();

        }
    }
}
