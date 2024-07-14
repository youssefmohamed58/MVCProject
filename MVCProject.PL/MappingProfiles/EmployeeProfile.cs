using AutoMapper;
using MVCProject.PL.ViewModels;
using Project.DAL.Models;

namespace MVCProject.PL.MappingProfiles
{
	public class EmployeeProfile : Profile
	{
        public EmployeeProfile()
        {
            CreateMap<Employee,EmployeeViewModel>().ReverseMap();
        }
    }
}
