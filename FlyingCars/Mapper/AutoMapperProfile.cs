using AutoMapper;
using FlyingCars.DTO;
using FlyingCars.Models;

namespace FlyingCars.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Department, DepartmentDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<History, HistoryDto>();
            CreateMap<Position, PositionDto>();
            CreateMap<EmployeeWithNavigationProperties, EmployeeWithNavigationPropertiesDto>();
        }
    }
}
