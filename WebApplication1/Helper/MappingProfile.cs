using AutoMapper;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<PokemonDto, Pokemon>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<OwnerDto, Owner>();
            CreateMap<Country, CountryDto>();
            CreateMap<CountryDto, Country>();
            CreateMap<Review, ReviewDto>();
            CreateMap<ReviewDto, Review>();
            CreateMap<Reviewer, ReviewerDto>();
            CreateMap<ReviewerDto, Reviewer>(); 

        }
    }
}
