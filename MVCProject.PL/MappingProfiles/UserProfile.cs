using AutoMapper;
using MVCProject.PL.ViewModels;
using Project.DAL.Models;

namespace MVCProject.PL.MappingProfiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<ApplicationUser, UserViewModel>();
		}
	}
}
