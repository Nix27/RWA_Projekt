using AutoMapper;
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
        }
    }
}
