using AutoMapper;
using cinema.api.Models;
using cinema.api.Models.Public;
using cinema.api.Public;
using cinema.context.Entities;

namespace cinema.api.Helpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

        CreateMap<OrderDto, Order>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<OrderStatus>(src.Status)));

        CreateMap<Category, CategoryDto>();

        CreateMap<Movie, MovieDto>();

        CreateMap<Screening, ScreeningDto>();

        CreateMap<User, UserDto>();

        CreateMap<Screening, UpcomingScreeningDto>();

        CreateMap<Movie, PublicMovieDto>()
            .ForMember(dest => dest.UpcomingScreenings, opt => opt.MapFrom(src => src.Screenings
                .Where(s => s.StartDateTime > DateTimeOffset.UtcNow)
                .OrderBy(s => s.StartDateTime)
                .Take(5)
                .ToList()));

        CreateMap<Seat, SeatDto>();
    }
}
