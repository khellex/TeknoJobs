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
            CreateMap<Departments, DepartmentsResponseDto>();
            CreateMap<DepartmentsRequestDto, Departments>();
            // Add more mappings here
        }
    }
}
