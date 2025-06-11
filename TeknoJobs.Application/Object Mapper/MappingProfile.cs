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
    /// <summary>
    /// Provides mapping configurations for transforming domain entities to data transfer objects (DTOs) and vice versa.
    /// </summary>
    /// <remarks>This profile defines mappings between various domain models and their corresponding DTOs,
    /// enabling seamless  conversion for use in application layers such as APIs or services. It leverages AutoMapper to
    /// configure  mappings, including custom rules for specific properties where necessary.  Example mappings include:
    /// - <see cref="Departments"/> to <see cref="DepartmentsResponseDto"/> and <see cref="DepartmentsRequestDto"/> to
    /// <see cref="Departments"/>. - <see cref="Locations"/> to <see cref="LocationsResponseDto"/> and <see
    /// cref="LocationsRequestDto"/> to <see cref="Locations"/>. - <see cref="Jobs"/> to <see
    /// cref="JobDetailsResponseDto"/> and <see cref="JobsRequestDto"/> to <see cref="Jobs"/>.  For mappings involving
    /// <see cref="JobsRequestDto"/>, certain properties such as <c>Locations</c>, <c>Department</c>, 
    /// <c>PostedDate</c>, and <c>Code</c> are explicitly ignored during the mapping process.</remarks>
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


            //mapping for Jobs --> JobDetailsResponseDto
            CreateMap<Jobs, JobDetailsResponseDto>();
            CreateMap<Locations, LocationDto>();
            CreateMap<Departments, DepartmentDto>();

            //mapping for JobsRequestDto --> Jobs
            CreateMap<JobsRequestDto, Jobs>()
                .ForMember(dest => dest.Locations, opt => opt.Ignore())
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.PostedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Code, opt => opt.Ignore());
        }
    }
}
