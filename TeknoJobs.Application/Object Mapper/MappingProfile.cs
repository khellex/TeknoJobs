using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Application.DTOs;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Application.Object_Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //mapping for Department --> DepartmentsResponseDto
            CreateMap<Departments, DepartmentsResponseDto>();

            //mapping for DepartmentsRequestDto --> Departments
            CreateMap<DepartmentsRequestDto, Departments>();


            //mapping for Locations --> LocationsResponseDto
            CreateMap<Locations, LocationsResponseDto>();

            //mapping for LocationsRequestDto --> Locations
            CreateMap<LocationsRequestDto, Locations>();
            
        }
    }
}
