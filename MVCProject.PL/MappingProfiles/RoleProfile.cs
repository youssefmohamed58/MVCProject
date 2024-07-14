using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MVCProject.PL.ViewModels;

namespace MVCProject.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
        }
    }
}
