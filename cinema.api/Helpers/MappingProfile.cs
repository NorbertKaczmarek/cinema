using AutoMapper;
using cinema.api.Models;
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
    }
}
