using AutoMapper;
using BL.DTO;
using BL.Models;
using MVC.Models;

namespace MVC.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<LoginRequestVM, LoginRequest>();
            CreateMap<ChangePasswordVM, ChangePasswordRequest>();
            CreateMap<CountryDto, CountryVM>();
            CreateMap<CountryVM, CountryDto>();
            CreateMap<GenreDto, GenreVM>();
            CreateMap<GenreVM, GenreDto>();
        }
    }
}
